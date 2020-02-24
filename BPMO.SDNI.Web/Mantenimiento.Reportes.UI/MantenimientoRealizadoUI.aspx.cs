using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using System.Data;
using BPMO.SDNI.Contratos.BO;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.Servicio.Catalogos.BO;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    public partial class MantenimientoRealizadoUI : ReportPage, IMantenimientoRealizadoVIS
    {
        #region Atributos
        private MantenimientoRealizadoPRE presentador = null;
        private string nombreClase;
        #endregion

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nombreClase = this.GetType().Name;
                presentador = new MantenimientoRealizadoPRE(this);


                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.Master.DepartamentoFiltroVisible = true; 
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);

            }
        }

        #endregion

        #region Propiedades

        #region propiedades idealise

        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                id = this.Master.UsuarioID;
                return id;
            }
        }

        public int? UnidadOperativaId
        {
            get
            {
                return this.Master.UnidadOperativaID;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        #endregion

        #region Propiedades Buscador

        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal
        }

        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

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
                return (ECatalogoBuscador)ViewState["BUSQUEDA2"];
            }
            set
            {
                ViewState["BUSQUEDA2"] = value;
            }
        }



        #endregion

        #region Propiedades Interfaz


        public int? SucursalID
        {
            get
            {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        public string SucursalNombre
        {
            get
            {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

        public int? TallerID
        {
            get
            {
                int? tallerID = null;
                if (!String.IsNullOrEmpty(this.ddTalleres.SelectedValue))
                    tallerID = int.Parse(this.ddTalleres.SelectedValue);
                return tallerID;
            }
        }

        public string NumeroVIN
        {
            get
            {
                string uiVIN = this.txtVin.Text;
                if (uiVIN.Trim().Length > 0)
                {
                    return uiVIN.ToUpper();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.txtVin.Text = value;
                }
                else
                {
                    this.txtVin.Text = String.Empty;
                }
            }
        }


        public ETipoContrato? Departamento
        {
            get 
            { 
                return this.Master.Departamento; 
            }
            set 
            {
                this.Master.Departamento = value; 
            }
        }


        public int? UnidadID
        {
            get
            {
                int? unidadId = null;
                if (hdnUnidadID.Value != string.Empty)
                    unidadId = int.Parse(hdnUnidadID.Value);
                return unidadId;
            }
            set
            {
                if (value != null)
                    hdnUnidadID.Value = value.ToString();
                else
                    hdnUnidadID.Value = string.Empty;
            }
        }

        public DateTime? FechaInicio
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicio.Text))
                {
                    var dateTemp = DateTime.Parse(this.txtFechaInicio.Text);
                    fecha = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, 0, 0, 0);
                }

                return fecha;
            }
        }

        public DateTime? FechaFin
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaFin.Text))
                {
                    var dateTemp = DateTime.Parse(this.txtFechaFin.Text);
                    fecha = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, 23, 59, 59);
                }
                return fecha;
            }
        }

        #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String IdentificadorReporte
        {
            get { return "PLEN.BEP.15.MODMTTO.CU047"; }
        } 

        #endregion

        #region Métodos

        #region Accesos y Seguridad

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        #region Metodos Buscador

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            try
            {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar2('" + ViewState_Guid + "','" + catalogo + "');");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar configuraciones", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
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

        private void DesplegarFormBusquedaUnidad()
        {
            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }


        private void DesplegarFormBusquedaSucursal()
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar la Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }


        #endregion

        public void enlazarControles(List<TallerBO> talleres)
        {
            //ListItem todos = new ListItem()
            //{
            //    Text = "TODOS",
            //    Value = "0"
            //};

            List<ListItem> itemsTalleres = new List<ListItem>();

            foreach (var item in talleres)
            {
                ListItem taller = new ListItem()
                {
                    Text = item.Nombre,
                    Value = item.Id.ToString()
                };

                itemsTalleres.Add(taller);
            }

            ddTalleres.Items.Clear();
            itemsTalleres.ForEach(x => ddTalleres.Items.Add(x));
            ddTalleres.Enabled = true;

            //ddTalleres.Items.Add(todos);
            //ddTalleres.SelectedValue = "0";        
        }


        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }


        /// <summary>
        /// Evento del boton para buscar unidades por vin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, EventArgs e)
        {
            this.DesplegarFormBusquedaUnidad();
        }

        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            this.DesplegarFormBusquedaSucursal();
        }

        /// <summary>
        /// Evento del buscador general
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult2_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult2_Click: " + ex.Message);
            }
        }

        #endregion
       
    }
}