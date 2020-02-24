//Satisface al caso de uso CU026 - Calendarizar Mantenimiento
//Satisface al caso de uso CU030 - Recalendarizar Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Globalization;
using System.Drawing;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// UI de datos de mantenimiento
    /// </summary>
    public partial class ucDatosCitaMantenimientoUI : UserControl, IucDatosCitaMantenimientoVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ucDatosCitaMantenimientoUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal
        }
        /// <summary>
        /// Presentador del user control
        /// </summary>
        private ucDatosCitaMantenimientoPRE presentador; 
        #endregion
        #region Propiedades
        /// <summary>
        /// Identificador de la unidad operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnSucursalID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value)
                           ? (Int32.TryParse(this.hdnSucursalID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnSucursalID.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtNombreSucursalDetail.Text) && !string.IsNullOrWhiteSpace(this.txtNombreSucursalDetail.Text) ? this.txtNombreSucursalDetail.Text.Trim().ToUpper() : string.Empty;
            }
            set
            {
                this.txtNombreSucursalDetail.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Identificador del taller
        /// </summary>
        public int? TallerID 
        {
            get
            {
                if (this.ddlTaller.Items == null) return null;
                if (this.ddlTaller.Items.Count <= 0) return null;
                if (this.ddlTaller.SelectedIndex == 0 || this.ddlTaller.SelectedIndex == -1) return null;

                TallerBO confTaller = this.Talleres[this.ddlTaller.SelectedIndex - 1];
                return confTaller.Id;
            }
            set
            {
                if (value != null && value != 0)
                {
                    if (this.ddlTaller.Items != null && this.ddlTaller.Items.Count > 0 && this.Talleres != null && this.Talleres.Any())
                    {
                        var index = 0;
                        for (int i = 0; i < this.ddlTaller.Items.Count; i++)
                        {
                            if (this.ddlTaller.Items[i].Value == value.ToString())
                            {
                                index = i; break;
                            }
                        }
                        if(index != 0)
                            this.ddlTaller.SelectedIndex = index;
                    }
                }
            }
        }
        /// <summary>
        /// Nombre del taller
        /// </summary>
        public string NombreTaller { get; set; }
        /// <summary>
        /// Determina si el taller es externo
        /// </summary>
        public bool? esExterno 
        {
            get
            {
                bool value;
                if (bool.TryParse(this.hdnEsExterno.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnEsExterno.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Listado de talleres
        /// </summary>
        public List<TallerBO> Talleres
        {
            get
            {
                var listado = new List<TallerBO>();
                if (Session["ListadoTalleres"] != null)
                    listado = Session["ListadoTalleres"] as List<TallerBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoTalleres"] == null)
                    Session.Add("ListadoTalleres", value);
                else
                    Session["ListadoTalleres"] = value;
            }
        }
        /// <summary>
        /// Listado de Mantto de equipos aliados
        /// </summary>
        public List<MantenimientoProgramadoEquipoAliadoBO> ListadoManttoEquiposAliados
        {
            get
            {
                var listado = new List<MantenimientoProgramadoEquipoAliadoBO>();
                if (Session["ListadoEquiposAliados"] != null)
                    listado = Session["ListadoEquiposAliados"] as List<MantenimientoProgramadoEquipoAliadoBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoEquiposAliados"] == null)
                    Session.Add("ListadoEquiposAliados", value);
                else
                    Session["ListadoEquiposAliados"] = value;
            }
        }
        /// <summary>
        /// Listado de contactos del cliente
        /// </summary>
        public List<ContactoClienteBO> ListadoContactosCliente
        {
            get
            {
                var listado = new List<ContactoClienteBO>();
                if (Session["ListadoContactosCliente"] != null)
                    listado = Session["ListadoContactosCliente"] as List<ContactoClienteBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoContactosCliente"] == null)
                    Session.Add("ListadoContactosCliente", value);
                else
                    Session["ListadoContactosCliente"] = value;
            }
        }
        /// <summary>
        /// Nombre del cliente de la unidad
        /// </summary>
        public string ClienteNombre
        {
            get
            {
                return this.txtNombreCliente.Text;
            }
            set
            {
                this.txtNombreCliente.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Area/Departamento de la Unidad
        /// </summary>
        public string Area
        {
            get
            {
                return this.txtArea.Text;
            }
            set
            {
                this.txtArea.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Identificador de la Unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnUnidadID.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnUnidadID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Identificador del Equipo de la Unidad
        /// </summary>
        public int? EquipoID
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnUnidadID.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnEquipoID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Identificador del cliente de la unidad
        /// </summary>
        public int? ClienteID
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnClienteID.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnClienteID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// vin de la unidad
        /// </summary>
        public string VINUnidad
        {
            get
            {
                return this.txtVINUnidad.Text;
            }
            set
            {
                this.txtVINUnidad.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Numero economico de la unidad
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                return this.txtNumeroEconomico.Text;
            }
            set
            {
                this.txtNumeroEconomico.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Placa estatal de la unidad
        /// </summary>
        public string PlacaEstatal
        {
            get
            {
                return this.txtPlacaEstatal.Text;
            }
            set
            {
                this.txtPlacaEstatal.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Placa Federal de la unidad
        /// </summary>
        public string PlacaFederal
        {
            get
            {
                return this.txtPlacaFederal.Text;
            }
            set
            {
                this.txtPlacaFederal.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Tipo Mantenimiento
        /// </summary>
        public string TipoMantenimiento
        {
            get
            {
                return this.txtTipoMantto.Text;
            }
            set
            {
                this.txtTipoMantto.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Tiempo de la unidad en mantenimiento
        /// </summary>
        public decimal? TiempoMantenimiento
        {
            get
            {
                decimal value;
                if (decimal.TryParse(this.txtTiempoMantenimiento.Text, out value))
                    return value;
                return null;
            }
            set
            {
                this.txtTiempoMantenimiento.Text = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Fecha en la que se planea la cita
        /// </summary>
        public DateTime? FechaCita
        {
            get
            {
                DateTime value;
                if (DateTime.TryParse(this.txtFechaCita.Text, out value))
                    return value;
                return null;
            }
            set
            {
                this.txtFechaCita.Text = value != null ? value.Value.ToShortDateString() : String.Empty;
            }
        }
        /// <summary>
        /// Hora en la que se planea la cita
        /// </summary>
        public TimeSpan? HoraCita
        {
            get
            {
                if (this.FechaCita == null)
                    return null;
                TimeSpan value;
                if (TimeSpan.TryParse(new TimeSpan((Convert.ToDateTime(this.txtHoraCita.Text.Trim())).Hour, (Convert.ToDateTime(this.txtHoraCita.Text.Trim())).Minute , (Convert.ToDateTime(this.txtHoraCita.Text.Trim()).Second)).ToString(), out value))
                    return value;
                return null;
            }
            set
            {
                this.txtHoraCita.Text = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Identificador del Mantenimiento Programado
        /// </summary>
        public int? MantenimientoProgramadoID
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnMantenimientoProgramadoID.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnMantenimientoProgramadoID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Identificador de la cita de mantenimiento
        /// </summary>
        public int? CitaMantenimientoID
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnCitaMantenimientoID.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnCitaMantenimientoID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Estatus de la cita de mantenimiento
        /// </summary>
        public EEstatusCita? EstatusCita
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnEstatusCita.Value, out value))
                    return (EEstatusCita)value;
                return null;
            }
            set
            {
                this.hdnEstatusCita.Value = value != null ? value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Identificador del ContactoCliente
        /// </summary>
        public int? ContactoClienteID
        {
            get
            {
                int value;
                if (int.TryParse(this.hdnContactoClienteID.Value, out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnContactoClienteID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el campo Nombre de la sucursal es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool SucursalFiltroVisible
        {
            get
            {
                return this.txtNombreSucursalDetail.Enabled;
            }
            set
            {
                this.txtNombreSucursalDetail.Enabled = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el dropdownList Nombre del taller es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool TallerFiltroVisible
        {
            get
            {
                return this.ddlTaller.Enabled;
            }
            set
            {
                this.ddlTaller.Enabled = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el botón es habilitado o no
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool BtnBuscarSucursalVisible 
        {
            get
            {
                return this.ibtnBuscarSucursal.Enabled;
            }
            set
            {
                this.ibtnBuscarSucursal.Enabled = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el texto del título
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String EtiquetaTitulo
        {
            get
            {
                return this.lblTitulo.Text;
            }
            set
            {
                this.lblTitulo.Text = value;
            }
        }

        private string mensajeError;

        public string MensajeError
        {
            get
            {
                mensajeError = LabelError.Text;
                return mensajeError;
            }
            set
            {
                LabelError.Text = value;
            }
        }

        #region Propiedades para el Buscador
        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set
            {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucDatosCitaMantenimientoPRE(this);
                if (!this.IsPostBack)
                    this.presentador.IniciarVista();

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
        public void PrepararNuevo()
        {

        }
        /// <summary>
        /// Establece los talleres a los que tiene acceso de acuerdo a la sucursal
        /// </summary>
        /// <param name="talleres">Talleres Permitidos</param>
        public void EstablecerTalleres(List<TallerBO> talleres)
        {
            this.Talleres = talleres;
            List<ListItem> lista = Talleres.Select(x => new ListItem(x.Nombre, x.Id.ToString())).ToList();

            this.ddlTaller.Items.Clear();

            this.ddlTaller.DataSource = lista;
            this.ddlTaller.DataTextField = "Text";
            this.ddlTaller.DataValueField = "Value";
            this.ddlTaller.DataBind();

            this.ddlTaller.Items.Insert(0, new ListItem("SELECCIONE", "0"));
        }
        /// <summary>
        /// Envia a sesion un objeto para redirigir a otra interfaz
        /// </summary>
        /// <param name="key">Llave utilizada para enviar el objeto</param>
        /// <param name="objeto">Objeto que sera enviado a otra interfaz</param>
        public void EstablecerPaqueteNavegacion(string key, object objeto)
        {
            Session[key] = objeto;
        }
        /// <summary>
        /// Obtiene un objeto enviado desde otra interfaz
        /// </summary>
        /// <param name="key">Llave para obtener el objeto</param>
        /// <returns>Objeto que fue enviado por otra interfaz</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            return Session[key];
        }
        /// <summary>
        /// Coloca los equipos aliados en la interfaz
        /// </summary>
        public void EstablecerEquiposAliados()
        {
            this.grvEquiposAliados.DataSource = this.ListadoManttoEquiposAliados;
            this.grvEquiposAliados.DataBind();

        }
        /// <summary>
        /// Coloca los contactos de clientes en la interfaz
        /// </summary>
        public void EstablecerContactosCliente()
        {
            List<DetalleContactoClienteBO> detalle = new List<DetalleContactoClienteBO>();
            detalle.AddRange(this.ListadoContactosCliente.SelectMany(x => x.Detalles).ToList());
            this.grvContactoCliente.DataSource = detalle;
            this.grvContactoCliente.DataBind();

        }
        /// <summary>
        /// Redirige a la pagina informativa de falta de permisos para acceder
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
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
        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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
        #endregion
        #endregion
        #region Eventos
        /// <summary>
        /// Envento realizado al llenar la lista de contactos del cliente
        /// </summary>
        protected void grvContactoCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var contactoCliente = (DetalleContactoClienteBO)e.Row.DataItem;
                    if (this.ContactoClienteID != null)
                    {
                        if (this.ContactoClienteID == contactoCliente.ContactoCliente.ContactoClienteID)
                        {
                            e.Row.BackColor = Color.FromName("GREY");
                            e.Row.ForeColor = Color.FromName("WHITE");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdRepresentantesLegales_RowDataBound: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento ejecutado al realizar un check sobre los contactos del cliente
        /// </summary>
        protected void grvContactoCliente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                this.EstablecerContactosCliente();
                this.grvContactoCliente.Rows[index].BackColor = Color.FromName("GREY");
                this.grvContactoCliente.Rows[index].ForeColor = Color.FromName("WHITE");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grvContactoCliente_RowCommand: " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void btnReloadContactos_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ActualizarContactosCliente();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnReloadContactos_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento de seleccion de Taller
        /// </summary>
        protected void ddlTaller_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var index = this.ddlTaller.SelectedIndex;
                if (index == 0)
                {
                    this.esExterno = null;
                }
                else
                {
                    var indice = index - 1;
                    var config = this.Talleres[indice];
                    this.esExterno = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".ddlTaller_SelectedIndexChanged: " + ex.Message);
            }

        }
        #region Eventos para el buscador
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        protected void txtNombreSucursalDetail_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Session_BOSelecto = null;
                this.SucursalID = null;

                if (!string.IsNullOrEmpty(this.txtNombreSucursalDetail.Text) && !string.IsNullOrWhiteSpace(this.txtNombreSucursalDetail.Text))
                {
                    string sucursalNombre = this.SucursalNombre;

                    this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                    this.SucursalNombre = sucursalNombre;
                    if (this.UnidadOperativaID.Value != null && this.SucursalNombre != null)
                    {
                        this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                        this.SucursalNombre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreSucursalDetail_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.ToString() + ".ibtnBuscaSucursal_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Evento del boton del buscador de bepensa
        /// </summary>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion

        protected void txtFechaCita_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.SucursalID != null && this.TallerID != null)
                {
                    string mensaje = "";
                    bool antesManttoProgramado = this.presentador.ValidarDiaAnteriorManttoProgramado();
                    if (antesManttoProgramado)
                    {
                        mensaje += "LA FECHA SELECCIONADA ES MENORA A LA DEL MANTENIMIENTO PROGRAMADO. ";
                        this.MostrarMensaje(mensaje, ETipoMensajeIU.ADVERTENCIA);
                        this.FechaCita = null;
                        return;
                    }
                    bool habil = this.presentador.validarFechaHabil();
                    if (habil == false)
                        mensaje += "LA FECHA SELECCIONADA ES UN DÍA INHABIL. ";
                    if (!this.presentador.EsTallerExterno() && habil && !antesManttoProgramado)
                    {
                        if (this.presentador.LimiteTallerSuperado(this.FechaCita))
                            mensaje += "El límite de capacidad del taller en la fecha seleccionada será superado. ";
                    }

                    if (!String.IsNullOrEmpty(mensaje))
                    {
                        //this.MostrarMensaje(mensaje, ETipoMensajeIU.ADVERTENCIA);
                        MensajeError = mensaje;
                        string origen = ((TextBox)sender).ID;
                        this.RegistrarScript("Advertencia", "confirmarDiaHabil('" + origen + "');");
                    }                      
                    
                }
                else
                {
                    this.MostrarMensaje("Debe seleccionar una sucursal y un taller", ETipoMensajeIU.ADVERTENCIA);
                    this.FechaCita = null;
                }
            }
            catch (Exception ex)
            {

                this.MostrarMensaje("Error al validar dia habil", ETipoMensajeIU.ERROR, ex.Message);
            }
           
        }
        #endregion
    }
}