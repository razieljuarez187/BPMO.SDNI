// Satisface al CU062 - Obtener Orden Servicio Idealease
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.BO;
using System.Data;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información del Detalle Mantenimiento Idealease, al usuario.
    /// </summary>
    public partial class DetalleMantenimientoUI : System.Web.UI.Page, IDetalleMantenimientoVIS {

        #region Propiedades

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private DetalleMantenimientoPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(DetalleMantenimientoUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos del Mantenimiento Idealease seleccionado.
        /// </summary>
        public Dictionary<string, string> MantenimientoToHash { 
            get { return Session["mantenimientoHash"] as Dictionary<string, string>; }
            set { Session["mantenimientoHash"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Folio de la Orden de Servicio por buscar.
        /// </summary>
        public int? FolioOrdenServicio { 
            get { return Int32.Parse(Session["FolioOrdenServicio"].ToString()); }
            set { Session["FolioOrdenServicio"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de Mantenimiento Idealease seleccionado.
        /// </summary>
        public bool EsUnidad {
            get { return (bool)Session["esUnidad"]; }
            set { Session["esUnidad"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el índice del Mantenimiento Idealease seleccionado.
        /// </summary>
        public int Index {
            get { return Int32.Parse(Session["indexMantenimientoSeleccionado"].ToString()); }
            set { Session["indexMantenimientoSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el listado de Mantenimientos Unidades Idealease encontradas.
        /// </summary>
        public List<MantenimientoUnidadBO> MantenimientosUnidad {
            get { return Session["listaEdicionOrdenesServicio"] as List<MantenimientoUnidadBO>; }
            set { Session["listaEdicionOrdenesServicio"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el listado de Mantenimientos Equipos Aliados Idealease encontradas.
        /// </summary>
        public List<MantenimientoEquipoAliadoBO> MantenimientosEquipoAliado {
            get { return Session["listaEdicionOrdenesServicio"] as List<MantenimientoEquipoAliadoBO>; }
            set { Session["listaEdicionOrdenesServicio"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de los Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease.
        /// </summary>
        public DataSet EquiposAliadosMantenimiento {
            get { return Session["equiposAliadosMantenimiento"] as DataSet; }
            set { Session["equiposAliadosMantenimiento"] = value; }
        }

        #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;
                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        public string LibroActivos {
            get {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor; 
            }
            set {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }
            #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new DetalleMantenimientoPRE(this);
                if(!IsPostBack) {
                    this.presentador.PrepararSeguridad();
                    if (Session["FolioOrdenServicio"] != null) {
                        EquiposAliadosMantenimiento = new DataSet();
                        presentador.MostrarDatosInterfaz();
                        Index = 0;
                        if (EsUnidad) {
                            presentador.ConsultarMantenimientoUnidadCompleto();
                        } else {
                            presentador.ConsultarMantenimientoEquipoCompleto();
                        }
                        CargarDatosMantenimiento();
                    } else {
                        if(MantenimientoToHash != null && MantenimientoToHash.Keys.Count > 0) {
                            Session["recargarOrdenesServicioIdealease"] = false;
                            this.CargarDatosMantenimiento();
                        }
                    }
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }        
        }
        
        #endregion

        #region Métodos
        
            #region Seguridad

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

            #endregion

        /// <summary>
        /// Establece una nueva instancia del diccionario de datos del Mantenimiento Idealease seleccionado y el índice del Mantenimiento Idealease seleccionado.
        /// </summary>
        private void LimpiarSesion() {
            this.Index = -1;
            this.MantenimientoToHash = new Dictionary<string, string>();
        }

        /// <summary>
        /// Realiza la redirección al visor de Registro de Mantenimiento Idealease.
        /// </summary>
        private void RedirigirAEntrada() {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/RegistrarUnidadUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta de Mantenimientos Idealease.
        /// </summary>
        private void RedirigirAMantenimiento() {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ConsultarMantenimientoUI.aspx?origin=cancel"));
        }

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
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

        /// <summary>
        /// Despliega el diccionario de datos del Mantenimiento Idealease seleccionado.
        /// </summary>
        public void CargarDatosMantenimiento(){
            this.txtFInventario.Text = MantenimientoToHash["inventario"];
            this.txtFDescripcionFalla.Text = MantenimientoToHash["falla"];
            this.txtFCodigosFalla.Text = MantenimientoToHash["codigosFalla"];
            this.txtFObservaciones.Text = MantenimientoToHash["observaciones"];
            this.txtFFechaA.Text = MantenimientoToHash["fechaApertura"];
            this.txtFFechaC.Text = MantenimientoToHash["fechaCierre"];
            if(EsUnidad){
                pnlEquiposAliados.Visible = true;
                CargarEquiposAliadosUnidad();
            } else {
                pnlEquiposAliados.Visible = false;
            }
        }

        /// <summary>
        /// Despliega la lista de los Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease seleccionado.
        /// </summary>
        public void CargarEquiposAliadosUnidad(){
            this.GridEquipoAliado.DataSource = EquiposAliadosMantenimiento.Tables[0];
            this.GridEquipoAliado.DataBind();
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Realiza la redirección al visor de Registro de Mantenimiento Idealease o Consulta de Mantenimientos Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            if (Session["FolioOrdenServicio"] == null){
                RedirigirAMantenimiento();
            }else{
                Session["FolioOrdenServicio"] = null;
                RedirigirAEntrada();
            }
        }

        /// <summary>
        /// Despliega los Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease de acuerdo a la Paginación especificada.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void GridEquipoAliado_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                GridEquipoAliado.PageIndex = e.NewPageIndex;
                if(EsUnidad){
                    CargarEquiposAliadosUnidad();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".GridEquipoAliado_PageIndexChanging:" + ex.Message);
            }
        }

        #endregion

    }
}