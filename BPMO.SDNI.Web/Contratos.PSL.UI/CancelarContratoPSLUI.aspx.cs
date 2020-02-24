//Cerrar Contrato PSL
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class CancelarContratoPSLUI : System.Web.UI.Page, ICancelarContratoPSLVIS {
        #region Atributos
        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "CancelarContratoPSLUI";

        /// <summary>
        /// Presentador para Cancelar Contrato RD
        /// </summary>
        private CancelarContratoPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        public object UltimoObjeto {
            get {
                if (Session["UltimoObjetoContratoRD"] != null)
                    return Session["UltimoObjetoContratoRD"];

                return null;
            }
            set {
                Session["UltimoObjetoContratoRD"] = value;
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value))
                    return int.Parse(this.hdnContratoID.Value);
                return null;
            }
            set { this.hdnContratoID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public EEstatusContrato? EstatusID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value))
                    return (EEstatusContrato)Enum.Parse(typeof(EEstatusContrato), this.hdnEstatusID.Value);
                return null;
            }
            set { this.hdnEstatusID.Value = value != null ? value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece l tipo de contrato que se va a registrar
        /// </summary>
        public int? TipoContrato {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnTipoContrato.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnTipoContrato.Value)
                           ? (Int32.TryParse(this.hdnTipoContrato.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnTipoContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA {
            get {
                if (!string.IsNullOrEmpty(this.hdnUUA.Value) && !string.IsNullOrWhiteSpace(this.hdnUUA.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUUA.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUUA.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA {
            get {
                if (!string.IsNullOrEmpty(this.hdnFUA.Value) && !string.IsNullOrWhiteSpace(this.hdnFUA.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFUA.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFUA.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public DateTime? FechaContrato {
            get {
                if (!string.IsNullOrEmpty(this.hdnFechaContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaContrato.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaContrato.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public DateTime? FechaPromesaDevolucion {
            get {
                if (!string.IsNullOrEmpty(this.hdnFechaPromesaDevolucion.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaPromesaDevolucion.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaPromesaDevolucion.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaPromesaDevolucion.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public DateTime? FechaDevolucion {
            get {
                if (!string.IsNullOrEmpty(this.hdnFechaDevolucion.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaDevolucion.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaDevolucion.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaDevolucion.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public int? UnidadID {
            get {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value))
                    return int.Parse(this.hdnUnidadID.Value);
                return null;
            }
            set { this.hdnUnidadID.Value = value != null ? value.ToString() : string.Empty; }
        }
        public int? EquipoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value))
                    return int.Parse(this.hdnEquipoID.Value);
                return null;
            }
            set { this.hdnEquipoID.Value = value != null ? value.ToString() : string.Empty; }
        }

        public string ObservacionesCancelacion {
            get {
                return (String.IsNullOrEmpty(this.txtObservacionesCierre.Text)) ? null : this.txtObservacionesCierre.Text.Trim().ToUpper();
            }
            set {
                this.txtObservacionesCierre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        public DateTime? FechaCancelacion {
            get {
                DateTime? fecha = null;
                TimeSpan? hora = null;

                if (!string.IsNullOrEmpty(this.txtFechaCierre.Text) && !string.IsNullOrWhiteSpace(this.txtFechaCierre.Text)) {
                    DateTime parse = new DateTime();
                    if (DateTime.TryParse(this.txtFechaCierre.Text, out parse))
                        fecha = parse;

                    if (!string.IsNullOrEmpty(this.txtHoraCierre.Text) && !string.IsNullOrWhiteSpace(this.txtHoraCierre.Text)) {
                        var time = DateTime.ParseExact(this.txtHoraCierre.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                        hora = time.TimeOfDay;
                    }
                }

                if (fecha.HasValue && hora.HasValue)
                    fecha = fecha.Value.Add(hora.Value);

                return fecha.HasValue ? fecha : null;
            }
            set {
                this.txtFechaCierre.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
                if (value != null && value.Value.TimeOfDay != null) {
                    DateTime d = new DateTime().Add(value.Value.TimeOfDay);
                    this.txtHoraCierre.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                } else
                    this.txtHoraCierre.Text = string.Empty;
            }
        }
        public string MotivoCancelacion {
            get {
                return (String.IsNullOrEmpty(this.txtMotivo.Text)) ? null : this.txtMotivo.Text.Trim().ToUpper();
            }
            set {
                this.txtMotivo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        /// <summary>
        /// Variable de session con datos del checklist de recepción
        /// </summary>
        public Dictionary<string, object> DatosReporte {
            get {
                if (Session["DatosReporte"] != null)
                    return Session["DatosReporte"] as Dictionary<string, object>;

                return null;
            }
        }

        /// <summary>
        /// Variable de session con el nombre del reporte
        /// </summary>
        public string NombreReporte {
            get {
                if (Session["NombreReporte"] != null)
                    return Session["NombreReporte"] as string;
                return null;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new CancelarContratoPSLPRE(this, this.ucHerramientas, this.ucResumenContratoPSL, this.ucDatosGeneralesElementoUI, this.ucEquiposAliadosUnidadUI);
                if (!Page.IsPostBack) {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }

                this.txtObservacionesCierre.Attributes.Add("onkeyup", "checkText(this,500);");
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Metodos
        public void PrepararEdicion() {
            this.ucDatosGeneralesElementoUI.EstablecerModoLectura();
        }

        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        public void RedirigirACerrar() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/CerrarContratoPSLUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarContratoPSLUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Redirige a los usuarios a la página de imprimir el check List
        /// </summary>
        public void RedirigirAImprimir() {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", false);
        }

        public void PermitirCancelar(bool permitir) {
            this.txtFechaCierre.Enabled = permitir;
            this.txtMotivo.Enabled = permitir;
            this.txtObservacionesCierre.Enabled = permitir;
            this.btnGuardar.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir) {
            this.hlkRegistro.Enabled = permitir;
        }

        /// <summary>
        /// Limpia las variables usadas para la edición de la session
        /// </summary>
        public void LimpiarSesion() {
            if (Session["UltimoObjetoContratoRD"] != null)
                Session.Remove("UltimoObjetoContratoRD");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null) {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa Generación.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa) {
            //Obteniendo el nombre de las etiquetas del archivo resource correspondiente.
            string FT = ObtenerEtiquetadelResource("EQ01", tipoEmpresa);

            //Se válida si la variable "FT" NO está vacía
            if (!string.IsNullOrEmpty(FT)) {
                this.ucEquiposAliadosUnidadUI.CambiarEtiquetas(FT);
            }
        }

        /// <summary>
        /// Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerEtiquetadelResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa) {
            string Etiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string EtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            EtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(EtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje)) {
                EtiquetaObtenida = request.cEtiqueta;
                if (Etiqueta != "NO APLICA") {
                    Etiqueta = EtiquetaObtenida;
                }
            }
            return Etiqueta;
        }
        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                this.presentador.CancelarEdicion();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar la cancelación del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                this.presentador.CancelarContrato();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click: " + ex.Message);
            }

        }
        #endregion

    }
}