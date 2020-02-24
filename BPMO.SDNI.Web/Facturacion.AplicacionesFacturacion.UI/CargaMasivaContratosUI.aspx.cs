// Satisface el caso de uso CU015 – Carga Masiva Facturación Contratos
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    /// <summary>
    /// Forma que realiza el proceso de carga masiva de contratos facturados 
    /// </summary>
    public partial class CargaMasivaContratosUI : System.Web.UI.Page, ICargaMasivaContratosVIS
    {
        #region Constants
        /// <summary>
        /// Clave del Guid asignado la instancia de la página
        /// </summary>
        private const String PAGEGUIDINDEX = "__PAGEGUID";
        #endregion

        #region Atributos
        /// <summary>
        /// Id único global de la instancia del control
        /// </summary>        
        private Guid _GUID;

        /// <summary>
        /// Presentador de para editar registro
        /// </summary>
        private CargaMasivaContratosPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(CargaMasivaContratosUI).Name;

        /// <summary>
        /// Lista de contratos que sirve de bitacora o cache de los contratos recuperados
        /// </summary>
        private List<ContratoBO> _contratos;

        /// <summary>
        ///  Datos extraidos del archivo de carga
        /// </summary>
        private DataSet _pagosCargados;

        /// <summary>
        /// Equipos ya procesados del archivo de carga
        /// </summary>
        private IList<EquipoBO> _equipos;

        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal = 0          
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)this.Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)this.Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value))
                    if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.Value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                    return this.txtSucursal.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el departamento o área seleccionada
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        public ETipoContrato? Departamento
        {
            get
            {
                if (this.ddlDepartamento.SelectedIndex == -1)
                    return null;

                return (ETipoContrato)Enum.ToObject(typeof(ETipoContrato), Convert.ToInt32(this.ddlDepartamento.SelectedValue));
            }
            set
            {
                this.ddlDepartamento.SelectedValue = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Obtiene el archivo cargado que contiene los pagos
        /// </summary>
        /// <value>Valor de tipo byte[]</value>
        public byte[] Archivo
        {
            get 
            {
                return this.fuArchivo.HasFile ? this.fuArchivo.FileBytes : null;
            }
        }

        /// <summary>
        /// Obtiene el nombre del archivo que fue cargado
        /// </summary>
        /// <value>Valor de tipo String</value>
        public string NombreArchivo
        {
            get 
            {
                return this.fuArchivo.FileName;
            }
        }        

        /// <summary>
        /// Lista de eventos ocurridos durante la carga del archivo de pagos
        /// </summary>
        /// <value>Valor de tipo Lista</value>
        public IList Eventos
        {
            get
            {
                IList objeto = null;
                string nombreSession = String.Format("EVENTOS_{0}", this.ViewState_Guid);
                if (this.Session[nombreSession] != null)
                    objeto = (IList)this.Session[nombreSession];

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("EVENTOS_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }       

        /// <summary>
        /// Obtiene los datos extraidos del archivo de carga
        /// </summary>
        /// <value>Objeto de tipo DataSet</value>
        public DataSet PagosCargados
        {
            get
            {
                return this._pagosCargados;
            }
            set
            {
                this._pagosCargados = value;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa los contratos que sirve de bitacora o cache de los contratos recuperados
        /// </summary>
        public IList<ContratoBO> Contratos
        {
            get
            {
                if (this._contratos == null)
                    this._contratos = new List<ContratoBO>();

                return this._contratos;
            }
            set
            {
                if (Object.Equals(this._contratos, value))                
                    this._contratos = value != null ? new List<ContratoBO>(value) : null;                
            }
        }        

        /// <summary>
        /// Obtiene los Equipos ya procesados del archivo de carga
        /// </summary>
        /// <value>Valor de tipo Lista</value>
        public IList<EquipoBO> Equipos
        {
            get
            {
                if (this._equipos == null)
                    this._equipos = new List<EquipoBO>();

                return this._equipos;
            }
            set
            {
                this._equipos = value;
            }
        }

        ///<summary>
        ///Obtiene un valor que representa un Id único global de la instancia del control
        ///</summary>
        ///<value>
        ///Objeto GUID con clave única de la instancia
        ///</value>
        internal Guid GUID
        {
            get
            {
                if (this._GUID == Guid.Empty)
                    this.RegisterGuid();

                return this._GUID;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ViewState_Guid
        {
            get
            {
                return this.GUID.ToString();
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el objeto que fue seleccionado del buscador
        /// </summary>
        /// <value>
        /// Objeto que fue seleccionado de tipo Object
        /// </value>
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", this.ViewState_Guid);
                if (this.Session[nombreSession] != null)
                    objeto = (this.Session[nombreSession]);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el objeto que tiene la información de filtrado del buscador
        /// </summary>
        /// <value>
        /// Objeto de filtrado de tipo Object
        /// </value>
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (this.Session[this.ViewState_Guid] != null)
                    objeto = (this.Session[this.ViewState_Guid]);

                return objeto;
            }
            set
            {
                if (value != null)
                    this.Session[this.ViewState_Guid] = value;
                else
                    this.Session.Remove(this.ViewState_Guid);
            }
        }

        /// <summary>
        /// Enumerador que contiene los buscadores existentes en la UI
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)this.ViewState["BUSQUEDA"];
            }
            set
            {
                this.ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa la carga inicial de la vista
        /// </summary>
        public void Inicializar()
        {
            this.Eventos = null;
            this.PagosCargados = null;
            this.Contratos = null;
            this.Equipos = null;
            this.LimpiarSesion();
         
            this.pnlFormulario.Visible = true;
            this.grdEventos.Visible = false;
            
            this.RegisterGuid();
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {
            
        }

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>       
        public bool ValidarCampos()
        {
            return this.IsValid;
        }

        /// <summary>
        /// Registra la clave única global del control en la página
        /// </summary>
        private void RegisterGuid()
        {
            string hiddenFieldValue = this.Request.Form[CargaMasivaContratosUI.PAGEGUIDINDEX];
            if (hiddenFieldValue == null)
            {
                this._GUID = Guid.NewGuid();
                hiddenFieldValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._GUID.ToString()));
            }
            else
            {
                string guidValue = Encoding.UTF8.GetString(Convert.FromBase64String(hiddenFieldValue));
                this._GUID = new Guid(guidValue);
            }

            ScriptManager.RegisterHiddenField(this, CargaMasivaContratosUI.PAGEGUIDINDEX, hiddenFieldValue);
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Despliega el Detalle del registro Seleccionado
        /// </summary>
        public void RedirigirAResultadoCarga()
        {
            this.pnlFormulario.Visible = false;
            this.grdEventos.Visible = true;
            this.grdEventos.DataSource = this.Eventos;
            this.grdEventos.DataBind();
        }

        /// <summary>
        /// Inicializa o carga la relación de Departamentos o áreas disponibles
        /// </summary>
        private void InicializarDepartamentos()
        {
            this.ddlDepartamento.ClearSelection();
            this.ddlDepartamento.Items.Clear();
            var items = Enum.GetValues(typeof(ETipoContrato))
                            .Cast<ETipoContrato>()
                            .Select(it => new
                            {
                                Value = (int)it,
                                Text = new Func<String>(() =>
                                        {
                                            String value = Enum.GetName(typeof(ETipoContrato), it);

                                            DescriptionAttribute descriptor = typeof(ETipoContrato)
                                                                                 .GetField(value)
                                                                                 .GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                                                 .Cast<DescriptionAttribute>()
                                                                                 .FirstOrDefault();

                                            if (descriptor != null)
                                                value = descriptor.Description;

                                            return value;
                                        }).Invoke()
                            })
                            .ToList();

            this.ddlDepartamento.DataSource = items;
            this.ddlDepartamento.DataBind();

            this.ddlDepartamento.SelectedValue = ((int)ETipoContrato.FSL).ToString();
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            this.ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = this.presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + this.ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;           
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new CargaMasivaContratosPRE(this);

                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.Inicializar();
                    this.presentador.RealizarPrimeraCarga();
                    this.InicializarDepartamentos();                    
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + "Page_Load: " + ex.GetBaseException().Message);
            }
        }        

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = this.SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                this.SucursalNombre = nombreSucursal;
                if (this.SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    this.SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicia la carga del archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {                
                this.presentador.CargarArchivo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Carga de archivos", ETipoMensajeIU.ERROR, nombreClase + ".btnAceptar_Click: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecua cuando se recibe el aviso de resultado del buscador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:                   
                        this.DesplegarBOSelecto(this.ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se valida el tipo de archivo cargado
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cmvfuArchivo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.NombreArchivo == null || (Path.GetExtension(args.Value) != ".xls" && Path.GetExtension(args.Value) != ".xlsx"))
                args.IsValid = false;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace un cambio de página en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdEventos.DataSource = this.Eventos;
                this.grdEventos.PageIndex = e.NewPageIndex;
                this.grdEventos.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdOperadores_PageIndexChanging: " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}