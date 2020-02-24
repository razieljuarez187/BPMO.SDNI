using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Flota.Reportes.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.Reportes.UI {
    public partial class ConsultarControlRentasPSLUI : System.Web.UI.Page, IConsultarControlRentasPSLVIS {
        #region Propiedades

        private ConsultarControlRentasPSLPRE presentador = null;
        private string nombreClase = "ConsultarControlRentasPSLUI";

        /// <summary>
        /// Enum de buscador general
        /// </summary>
        public enum ECatalogoBuscador {
            Unidad,
            Sucursal,
            Cliente,
            Modelo
        }

        /// <summary>
        /// Enum para especificar el tipo de servicio
        /// </summary>
        public enum ETipoServicio {
            Unidad,
            EquipoAliado
        }

        #region Filtros Busqueda

        public int? SucursalID {
            get {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        public string SucursalNombre {
            get {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

        public int? Estatus {
            get {
                int? estatus = null;
                if (!String.IsNullOrEmpty(this.ddEstatus.SelectedValue))
                    estatus = int.Parse(this.ddEstatus.SelectedValue);
                return estatus;
            }
        }

        public DataTable UnidadRentas {
            get { return Session["RentasEncontrados"] as DataTable; }
            set { Session["RentasEncontrados"] = value; }
        }

        public DataSet ReporteRentas {
            get { return Session["ReporteRentas"] as DataSet; }
            set { Session["ReporteRentas"] = value; }
        }


        #endregion

        #region Buscador General

        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto {
            get {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }

        protected object Session_ObjetoBuscador {
            get {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }

        public ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }

        public ETipoServicio StateOrdenServicio {
            get {
                return (ETipoServicio)Session["TIPOORDENSERVICIO"];
            }
            set {
                Session["TIPOORDENSERVICIO"] = value;
            }
        }

        #endregion


        public List<CatalogoBaseBO> ListaAcciones { get; set; }

        public int? UsuarioID {
            get {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
            }
        }

        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        public int? UC {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }


        public GridView GvUnidadesRentas {
            get {
                return this.gvUnidadesRentas;
            }
        }

        private AdscripcionBO Adscripcion {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj.Adscripcion;
            }
        }

        public int IndicePaginaResultado {
            get { return this.gvUnidadesRentas.PageIndex; }
            set { this.gvUnidadesRentas.PageIndex = value; }
        }

        public string TipoPermiso {
            get {
                string valor = null;
                if (this.hdnTipoPermiso.Value.Trim().Length > 0)
                    valor = this.hdnTipoPermiso.Value.Trim().ToUpper();
                return valor;

            }
            set {
                if (value != null)
                    this.hdnTipoPermiso.Value = value.ToString();
                else
                    this.hdnTipoPermiso.Value = string.Empty;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Evento que se ejecuta al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConsultarControlRentasPSLPRE(this);
                if (!IsPostBack) {
                    this.presentador.PrepararBusqueda();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }

            if (Session["RentasEncontrados"] == null) {
                Session["RentasEncontrados"] = new DataTable();
            }

            this.presentador.CargarDatos();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Limpia los registros de ingreso de la sesion
        /// </summary>
        public void LimpiarSesion() {
            if (Session["RentasEncontrados"] != null)
                Session.Remove("RentasEncontrados");
            if (Session["ReporteRentas"] != null)
                Session.Remove("ReporteRentas");
        }


        /// <summary>
        /// Establece el Paquete de Navegación para imprimir los Formatos del Contrato
        /// </summary>
        /// <param name="Clave">Clave del Paquete de Navegación</param>
        /// <param name="DatosReporte">Datos del Reporte</param>
        public void EstablecerPaqueteNavegacion(string Clave, byte[] DatosReporte) {
            Session["NombreReporte"] = Clave;
            Session["DatosReporte"] = DatosReporte;
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        public void CargarDatos() {
            this.presentador.CargarDatos();
        }

        #region Privados

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        public void PonerEnSesion(string key, object value) {
            Session[key] = value;
        }

        private void RemoverEnSesion(string key) {
            if (Session[key] != null)
                Session.Remove(key);
        }

        private object RecuperarEnSesion(string key) {
            return Session[key];
        }

        #endregion

        /// <summary>
        /// Redirige a la pagina informativa de falta de permisos para acceder
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            try {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
            } catch (Exception ex) {

                this.MostrarMensaje("Error al consultar unidades", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catálogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            try {
                string nombreSucursal = this.SucursalNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                this.SucursalNombre = nombreSucursal;
                if (this.SucursalNombre != null) {
                    EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para buscar mantenimientos según filtros aplicados
        /// </summary>
        protected void OnclickConsultarReporte(Object sender, EventArgs e) {
            try {
                this.presentador.Consultar();
            } catch (Exception ex) {

                this.MostrarMensaje("Error al consultar la información de Rentas :", ETipoMensajeIU.ERROR, ex.Message);
            }
        }


        /// <summary>
        /// Evento para desplegar resultado de buscador general
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                this.DesplegarBOSelecto(ViewState_Catalogo);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para cambiar el paginado de resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidadesRentas_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.gvUnidadesRentas.DataSource = this.UnidadRentas;
                this.gvUnidadesRentas.PageIndex = e.NewPageIndex;
                this.gvUnidadesRentas.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".gvUnidadesRentas_PageIndexChanging: " + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Carga y despliega el listado de Estatus
        /// </summary>
        /// <param name="listado"></param>
        public void CargarEstatus(List<EEstatusUnidad> listado) {
            var Lista = new List<KeyValuePair<int, string>>();

            if (listado != null)
                Lista.AddRange(from estatus in listado let tipo = ((DescriptionAttribute)estatus.GetType().GetField(estatus.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description select new KeyValuePair<int, string>((int)estatus, tipo));

            // Agregar el Item de fachada
            Lista.Insert(0, new KeyValuePair<int, string>(-1, "TODOS"));
            //Limpiar el DropDownList Actual
            ddEstatus.Items.Clear();
            // Asignar Lista al DropDownList
            ddEstatus.DataTextField = "Value";
            ddEstatus.DataValueField = "Key";
            ddEstatus.DataSource = Lista;
            ddEstatus.DataBind();
        }

        public void ActualizarResultado(System.Data.DataTable dt) {
            if (Session["RentasEncontrados"] == null) {
                Session["RentasEncontrados"] = new DataTable();
            }

            this.gvUnidadesRentas.DataSource = this.UnidadRentas;
            this.gvUnidadesRentas.DataBind();
        }

        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa) {
            throw new NotImplementedException();
        }

        public void PrepararBusqueda() {
            this.txtSucursal.Text = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.ddEstatus.SelectedValue = "-1";
        }

        protected void OnclickExportarReporteRentas(object sender, EventArgs e) {
            try {
                this.presentador.ExportarReporteExcel();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al exportar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".OnclickExportarReporteRentas" + ex.Message);

            }
        }

        public void RegistraScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
    }
}