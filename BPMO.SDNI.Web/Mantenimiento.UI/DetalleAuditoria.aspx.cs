// Satisface al CU072 - Obtener Auditoría
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using System.IO;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información Detalle Auditoría, al usuario.
    /// </summary>
    public partial class DetalleAuditoria : System.Web.UI.Page, IDetalleAuditoriaVIS {

        #region Propiedades

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private DetalleAuditoriaPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(DetalleAuditoria).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa la Auditoría de Mantenimiento Idealease a buscar.
        /// </summary>
        /// <value>Objeto de tipo AuditoriaMantenimientoBO.</value>
        public AuditoriaMantenimientoBO Auditoria {
            get { return (AuditoriaMantenimientoBO)Session["auditoriaSeleccionada"]; }
            set { Session["auditoriaSeleccionada"] = value; }
        }

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
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
        public int? UnidadOperativaID {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
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
                this.presentador = new DetalleAuditoriaPRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + " .Page_Load: " + ex.Message);
            }
            if(!IsPostBack){
                presentador.PrepararSeguridad();
                CargarDatos();
            }
            this.gvActividadesAuditoria.DataSource = Auditoria.DetalleAuditoria;
            this.gvActividadesAuditoria.DataBind();
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
        /// Despliega la información de la Auditoría Mantenimiento Seleccionada.
        /// </summary>
        private void CargarDatos() {
            if(Auditoria != null){
                this.txtFecha.Text = Auditoria.FechaAuditoria.ToString();
                this.txtIdAuditoria.Text = Auditoria.AuditoriaMantenimientoID.ToString();
                this.txtIdOS.Text = Auditoria.OrdenServicio.Id.ToString();
                this.txtSucursal.Text = Auditoria.OrdenServicio.AdscripcionServicio.Sucursal.Nombre != null ? Auditoria.OrdenServicio.AdscripcionServicio.Sucursal.Nombre : "";
                this.txtTecnicos.Text = "";

                if(Auditoria.Tecnicos.Count > 0) {
                    foreach (TecnicoBO tecnico in Auditoria.Tecnicos) {
			            this.txtTecnicos.Text += tecnico.Empleado.NombreCompleto + Environment.NewLine;
			        }
                }
            
                this.txtTipoMantenimiento.Text = Auditoria.TipoMantenimiento.ToString();
                this.txtObservaciones.Text = Auditoria.Observaciones != null ? Auditoria.Observaciones : "";
                this.gvActividadesAuditoria.DataSource = Auditoria.DetalleAuditoria;
                this.gvActividadesAuditoria.DataBind();
                Session["evidencia"] = Auditoria.EvidenciaMantenimiento;
                ibtDescargar.OnClientClick = "javascript:window.open('../Mantenimiento.UI/hdlrDescargarEvidenciaAuditoria.ashx?archivoID=" + Auditoria.AuditoriaMantenimientoID + "'); return false;";
                }
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta de Auditoría Mantenimiento Idealease.
        /// </summary>
        private void RedirigirAConsultaAuditoria() {
            Session["recargarAuditorias"] = null;
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ConsultarAuditoriaUI.aspx"));
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Realiza la redirección al visor de Consulta de Auditoría Mantenimiento Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnFinalizar_Click(Object sender, EventArgs e) {
            Auditoria = new AuditoriaMantenimientoBO();
            RedirigirAConsultaAuditoria();
        }

        /// <summary>
        /// Evento que despliega la calificación seleccionada de cada actividad del PM asociado.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvActividadesAuditoria_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                 EResultadoAuditoria resultado = (EResultadoAuditoria)DataBinder.Eval(e.Row.DataItem, "ResultadoAuditoria");
                if(resultado != null) {
                    RadioButton rb = null;
                    switch (resultado) { 
                        case EResultadoAuditoria.Ajustado:
                            rb = (RadioButton)e.Row.FindControl("chbxAjustado");
                        break;
                        case EResultadoAuditoria.Reparar:
                            rb = (RadioButton)e.Row.FindControl("chbxReparar");
                        break;
                        case EResultadoAuditoria.Satisfactoria:
                            rb = (RadioButton)e.Row.FindControl("chbxSatisfactorio");
                        break;
                    }
                    rb.Checked = true;
                }
            }
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

        #endregion
        
    }
}