using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class RegistrarRecepcionPSLUI : System.Web.UI.Page, IRegistrarRecepcionPSLVIS {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private RegistrarRecepcionPSLPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "RegistrarRecepcionUnidadPSLUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioID {
            get {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
            }
        }

        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public int? UnidadOperativaID {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                           ? masterMsj.Adscripcion.UnidadOperativa.Id
                           : null;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }
        //Administración del Wizard
        /// <summary>
        /// Obtiene la página actual en la que se esta trabajando
        /// </summary>
        public int? PaginaActual {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnPaginaActual.Value) && !string.IsNullOrWhiteSpace(this.hdnPaginaActual.Value)
                           ? (Int32.TryParse(this.hdnPaginaActual.Value, out val) ? (int?)val : null)
                           : null;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre del usuario que recibe la unidad
        /// </summary>
        public string NombreUsuarioRecibe {
            get { return this.txtUsuarioRecibe.Text.Trim().ToUpper(); }
            set {
                this.txtUsuarioRecibe.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre del usuario que entregó la unidad
        /// </summary>
        public string NombreUsuarioEntrega {
            get { return this.txtUsuarioEntrego.Text.Trim().ToUpper(); }
            set {
                this.txtUsuarioEntrego.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnContratoID.Value)
                           ? (Int32.TryParse(this.hdnContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set {
                this.hdnContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato de renta diaría
        /// </summary>
        public string NumeroContrato {
            get { return this.txtNumeroContrato.Text.Trim().ToUpper(); }
            set {
                this.txtNumeroContrato.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                  ? value.Trim().ToUpper()
                                                  : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el estatus del contrato
        /// </summary>
        public int? EstatusContratoID {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnEstatusContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnEstatusContratoID.Value)
                           ? (Int32.TryParse(this.hdnEstatusContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set {
                this.hdnEstatusContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la línea del contrato
        /// </summary>
        public int? LineaContratoID {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnLineaContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnLineaContratoID.Value)
                           ? (Int32.TryParse(this.hdnLineaContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set {
                this.hdnLineaContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Fecha en la que se ejecuta el contrato
        /// </summary>
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
        /// <summary>
        /// Obtiene o establece la hora del contrato de renta diaria
        /// </summary>
        public TimeSpan? HoraContrato {
            get {
                if (!string.IsNullOrEmpty(this.hdnHoraContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnHoraContrato.Value)) {
                    TimeSpan time;
                    return TimeSpan.TryParse(this.hdnHoraContrato.Value, out time) ? (TimeSpan?)time : null;
                }
                return null;
            }
            set { this.hdnHoraContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string NombreCliente {
            get { return this.txtNombreCliente.Text.Trim().ToUpper(); }
            set {
                this.txtNombreCliente.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la licencía del operador
        /// </summary>
        public string NumeroLicencia {
            get { return this.txtNumeroLicencia.Text.Trim().ToUpper(); }
            set {
                this.txtNumeroLicencia.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                              ? value.Trim().ToUpper()
                                              : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas estatales de la unidad
        /// </summary>
        public string PlacasEstatales {
            get { return this.txtPlacasEstatales.Text.Trim().ToUpper(); }
            set {
                this.txtPlacasEstatales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID {
            get {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUnidadID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// obtiene o establece el identificador del equipo
        /// </summary>
        public int? EquipoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnEquipoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEquipoID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que será rentada
        /// </summary>
        public string NumeroSerie {
            get { return this.txtNumeroSerie.Text.Trim().ToUpper(); }
            set {
                this.txtNumeroSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string NumeroEconomico {
            get { return this.txtNumeroEconomico.Text.Trim().ToUpper(); }
            set {
                this.txtNumeroEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la capacidad del tanque
        /// </summary>
        public decimal? CapacidadTanque {
            get {
                if (!string.IsNullOrEmpty(this.hdnCapacidadTanque.Value) && !string.IsNullOrWhiteSpace(this.hdnCapacidadTanque.Value)) {
                    decimal val;
                    return Decimal.TryParse(this.hdnCapacidadTanque.Value, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.hdnCapacidadTanque.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de check list
        /// </summary>
        public int? TipoListado {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnTipoCheckList.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnTipoCheckList.Value)
                           ? (Int32.TryParse(this.hdnTipoCheckList.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnTipoCheckList.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de listado de verificación PSL
        /// </summary>
        public int? TipoListadoVerificacionPSL {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnTipoListadoVerificacionPSL.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnTipoListadoVerificacionPSL.Value)
                           ? (Int32.TryParse(this.hdnTipoListadoVerificacionPSL.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnTipoListadoVerificacionPSL.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
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
        /// Obtiene o establece la fecha en que se realiza el check list
        /// </summary>
        public DateTime? FechaListado {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaRecepcion.Text) && !string.IsNullOrWhiteSpace(this.txtFechaRecepcion.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaRecepcion.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set {
                this.txtFechaRecepcion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o estable la hora de salida de la unidad
        /// </summary>
        public TimeSpan? HoraListado {
            get {
                if (!string.IsNullOrEmpty(this.txtHoraRecepcion.Text) && !string.IsNullOrWhiteSpace(this.txtHoraRecepcion.Text)) {
                    var time = DateTime.ParseExact(this.txtHoraRecepcion.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set {
                if (value != null) {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraRecepcion.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                } else
                    this.txtHoraRecepcion.Text = string.Empty;
            }
        }
        public int? Horometro {
            get {
                if (!string.IsNullOrEmpty(this.txtHorometro.Text) && !string.IsNullOrWhiteSpace(this.txtHorometro.Text)) {
                    int val = 0;
                    return Int32.TryParse(this.txtHorometro.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtHorometro.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el combustible de salida
        /// </summary>
        public int? Combustible {
            get {
                if (!string.IsNullOrEmpty(this.txtCombustible.Text) && !string.IsNullOrWhiteSpace(this.txtCombustible.Text)) {
                    int val = 0;
                    return Int32.TryParse(this.txtCombustible.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.txtCombustible.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty;
            }
        }

        public EArea? Area {
            get {
                EArea? area = null;
                if (this.hdnArea != null)
                    area = (EArea)Enum.Parse(typeof(EArea), this.hdnArea.Value);
                return area;
            }
            set {

                if (value == null)
                    this.hdnArea.Value = string.Empty;
                else
                    this.hdnArea.Value = ((int)value).ToString();
            }
        }

        #region Información entrega
        /// <summary>
        /// Obtiene o establece el identificador del check list de entrega
        /// </summary>
        public int? CheckListEntregaID {
            get {
                if (!string.IsNullOrEmpty(this.hdnCheckListEntregaID.Value) && !string.IsNullOrWhiteSpace(this.hdnCheckListEntregaID.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnCheckListEntregaID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.hdnCheckListEntregaID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la fecha en que se entego la undiad al cliente
        /// </summary>
        public DateTime? FechaListadoEntrega {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtFechaEntrega.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaEntrega.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set {
                this.txtFechaEntrega.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene y establece la hora en la que se entrego la unidad al cliente
        /// </summary>
        public TimeSpan? HoraListadoEntrega {
            get {
                if (!string.IsNullOrEmpty(this.txtHoraEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtHoraEntrega.Text)) {
                    var time = DateTime.ParseExact(this.txtHoraEntrega.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set {
                if (value != null) {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraEntrega.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                } else
                    this.txtHoraEntrega.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el kilometraje con el que fue entregada la unidad
        /// </summary>
        public int? HorometroEntrega {
            get {
                if (!string.IsNullOrEmpty(this.txtHorometroEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtHorometroEntrega.Text)) {
                    int val = 0;
                    return Int32.TryParse(this.txtHorometroEntrega.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.txtHorometroEntrega.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el combustible de entrega de la unidad
        /// </summary>
        public int? CombustibleEntrega {
            get {
                if (!string.IsNullOrEmpty(this.txtCombustibleEntrega.Text) && !string.IsNullOrWhiteSpace(this.txtCombustibleEntrega.Text)) {
                    int val = 0;
                    return Int32.TryParse(this.txtCombustibleEntrega.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.txtCombustibleEntrega.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece las observaciones a la documetnación la momento de la entrega al cliente
        /// </summary>
        public string ObservacionesEntrega {
            get { return this.txtObservacionEntrega.Text.Trim().ToUpper(); }
            set {
                this.txtObservacionEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la observación de la sección seleccionada
        /// </summary>
        public string ObservacionesRecepcion {
            get { return this.txtObservacionRecepcion.Text.Trim().ToUpper(); }
            set {
                this.txtObservacionRecepcion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                        ? value.Trim().ToUpper()
                                                        : string.Empty;
            }
        }

        /// <summary>
        /// Indica la tolerancia en kilometros para la recepción
        /// </summary>
        public int? KilometrajeDiario {
            get {
                if (!String.IsNullOrEmpty(this.hdnKilometrajeDiario.Value))
                    return int.Parse(this.hdnKilometrajeDiario.Value);
                else
                    return null;
            }
            set {
                this.hdnKilometrajeDiario.Value = value.ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece los archivos que se desean asociar al check list
        /// </summary>
        public object NuevosArchivos {
            get { return Session["NewObject"] as List<ArchivoBO>; }
            set { Session["NewObject"] = value; }
        }

        /// <summary>
        /// Contiene la lista de acciones a las cuales tiene acceso el usuario.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion

        #endregion

        #region Métodos
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        public void LimpiarSesion() {
            Session.Remove("NewObject");
            Session.Remove("CheckListEntrega");
            Session.Remove("VerificacionesSeccionEntrega");
            Session.Remove("VerificacionesLlantaEntrega");
            Session.Remove("ImagenesSecciones");
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.UcCatalogoDocumentosUI1.LimpiarSesion();
        }

        /// <summary>
        /// Limpia el contrato al cual se le esta registrando el check list
        /// </summary>
        public void LimpiarPaqueteNavegacion() {
            Session.Remove("RegistrarRecepcionUI");
            Session.Remove("LineaContratoPSLIDR");
        }

        /// <summary>
        /// Establece la configuración inicial para el registro del check list
        /// </summary>
        public void PrepararNuevo() {
            #region Readonly
            this.txtUsuarioEntrego.ReadOnly = true;
            this.txtUsuarioRecibe.ReadOnly = true;
            this.txtNumeroContrato.ReadOnly = true;
            this.txtNombreCliente.ReadOnly = true;
            this.txtNumeroLicencia.ReadOnly = true;
            this.txtNumeroEconomico.ReadOnly = true;
            this.txtNumeroSerie.ReadOnly = true;
            this.txtPlacasEstatales.ReadOnly = true;
            #endregion

            #region Enabled
            this.txtUsuarioEntrego.Enabled = false;
            this.txtUsuarioRecibe.Enabled = false;
            this.txtNumeroContrato.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.txtNumeroLicencia.Enabled = false;
            this.txtNumeroEconomico.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.txtPlacasEstatales.Enabled = false;
            this.txtObservacionEntrega.Enabled = false;

            this.txtCombustibleEntrega.Enabled = false;
            this.txtFechaEntrega.Enabled = false;
            this.txtHoraEntrega.Enabled = false;
            this.txtHorometroEntrega.Enabled = false;
            
            this.ucucPuntosVerificacionExcavadoraEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionRetroExcavadoraEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionCompresoresPortatilesE.ModoEdicion(false);
            this.ucucPuntosVerificacionEntregaRecepcionEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionVibroCompactadorEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionPistolaNeumaticaEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionMartilloHidraulicoEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionTorresLuzEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionSubArrendadoEntrega.ModoEdicion(false);
            this.ucPuntosVerificacionPlataformaTijerasE.ModoEdicion(false);
            this.ucucPuntosVerificacionMotoNiveladoraEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionMontaCargaEntrega.ModoEdicion(false);
            this.ucucPuntosVerificacionMiniCargadorEntrega.ModoEdicion(false);
            #endregion
        }
        /// <summary>
        /// Redirige aa los usuarios que no tienen permisos de ejecutar la acción
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }

        /// <summary>
        /// Redirige a los usaurios a la página de consulta de Check List
        /// </summary>
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarListadoVerificacionPSLUI.aspx"), false);
        }

        /// <summary>
        /// Redirige a los usuarios a la página de imprimir el check List
        /// </summary>
        public void RedirigirAImprimir(string error) {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx?error=" + error, false);
        }

        /// <summary>
        /// Redirige a los usuarios a la pagina de cancelacipon de contratos
        /// </summary>
        public void RedirigirACancelarContrato() {
            Response.Redirect("../Contratos.PSL.UI/CancelarContratoPSLUI.aspx", false);
        }

        public void EstablecerPaqueteNavegacion(string key, object value) {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
        }

        /// <summary>
        /// Guarda un objeto en session para el cancelar contrato
        /// </summary>
        /// <param name="key">Clave del paquete</param>
        /// <param name="value">Objeto que se desea guardar</param>
        public void EstablecerPaqueteNavegacionCancelar(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacionCancelar: No se ha establecido la clave para el paquete de navegación del contrato a cancelar. ");
            Session[key] = value;
        }

        /// <summary>
        /// Obtiene de session el contrato al que se le desea registrar el check list
        /// </summary>
        /// <returns>Contrato al que se le registra el check list</returns>
        public object ObtenerPaqueteContrato() {
            return Session["RegistrarRecepcionPSLUI"] ?? null;
        }
        /// <summary>
        /// Obtiene un paquete guardado en la session
        /// </summary>
        /// <returns>Objeto guardado en la session</returns>
        public object ObtenerPaqueteLineaContrato() {
            return Session["LineaContratoPSLIDR"] ?? null;
        }
        /// <summary>
        /// Establece el número de página
        /// </summary>
        /// <param name="numeroPagina">Numero de página</param>
        public void EstablecerPagina(int numeroPagina) {
            this.mvCU077.SetActiveView((View)this.mvCU077.FindControl("vwPagina" + numeroPagina.ToString()));
            this.hdnPaginaActual.Value = numeroPagina.ToString();
        }

        /// <summary>
        /// Habilita o deshabilita el botón de regresar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirRegresar(bool habilitar) {
            this.btnAnterior.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita o deshabilita el botón de continuar
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirContinuar(bool habilitar) {
            this.btnContinuar.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita o deshabilita el botón de cancelar registro
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirCancelar(bool habilitar) {
            this.btnCancelar.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita o deshabilita el boton de guardar check list
        /// </summary>
        /// <param name="habilitar">Estado</param>
        public void PermitirGuardarTerminada(bool habilitar) {
            this.btnTerminar.Enabled = habilitar;
        }

        /// <summary>
        /// Hace visible o invisible el botón de continuar 
        /// </summary>
        /// <param name="ocultar">Estado</param>
        public void OcultarContinuar(bool ocultar) {
            this.btnContinuar.Visible = !ocultar;
        }

        /// <summary>
        /// Hace visible o invisible el botón de terminar
        /// </summary>
        /// <param name="ocultar">Estado</param>
        public void OcultarTerminar(bool ocultar) {
            this.btnTerminar.Visible = !ocultar;
        }

        /// <summary>
        /// Reestablece los botones para las acciones en caso de un fallo
        /// </summary>
        private void ReestablecerControles() {
            this.btnAnterior.Enabled = false;
            this.btnTerminar.Visible = false;
            this.btnTerminar.Enabled = false;
            this.btnContinuar.Enabled = false;
            this.btnCancelar.Enabled = true;
        }

        /// <summary>
        /// Obtiene o establece los tipos de archivo de imagen
        /// </summary>
        public object TiposArchivoImagen {
            get {
                return Session["TiposArchivosImagen"] as List<TipoArchivoBO>;
            }
            set {
                Session["TiposArchivosImagen"] = value;
            }
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        public void EstablecerPuntosVerificacionCheckList() {

            switch (UnidadOperativaID) {
                case (int)ETipoEmpresa.Construccion:
                    if (((EAreaConstruccion)this.Area) == EAreaConstruccion.RO || ((EAreaConstruccion)this.Area) == EAreaConstruccion.ROC) {
                        switch ((ETipoUnidad)Enum.Parse(typeof(ETipoUnidad), TipoListadoVerificacionPSL.Value.ToString())) {
                            case ETipoUnidad.LV_COMPRESOR:
                                this.mvPuntosVerificacion.ActiveViewIndex = 0;
                                break;
                            case ETipoUnidad.LV_MONTACARGA:
                                this.mvPuntosVerificacion.ActiveViewIndex = 1;
                                break;
                            case ETipoUnidad.LV_MOTONIVELADORA:
                                this.mvPuntosVerificacion.ActiveViewIndex = 2;
                                break;
                            case ETipoUnidad.LV_MINICARGADOR:
                                this.mvPuntosVerificacion.ActiveViewIndex = 3;
                                break;
                            case ETipoUnidad.LV_MARTILLO_HIDRAULICO:
                                this.mvPuntosVerificacion.ActiveViewIndex = 4;
                                break;
                            case ETipoUnidad.LV_EXCAVADORA:
                                this.mvPuntosVerificacion.ActiveViewIndex = 6;
                                break;
                            case ETipoUnidad.LV_SUBARRENDADO:
                                this.mvPuntosVerificacion.ActiveViewIndex = 7;
                                break;
                            case ETipoUnidad.LV_TORRES_LUZ:
                                this.mvPuntosVerificacion.ActiveViewIndex = 8;
                                break;
                            case ETipoUnidad.LV_PLATAFORMA_TIJERAS:
                                this.mvPuntosVerificacion.ActiveViewIndex = 9;
                                break;
                            case ETipoUnidad.LV_RETRO_EXCAVADORA:
                                this.mvPuntosVerificacion.ActiveViewIndex = 10;
                                break;
                            case ETipoUnidad.LV_VIBRO_COMPACTADOR:
                                this.mvPuntosVerificacion.ActiveViewIndex = 11;
                                break;
                            case ETipoUnidad.LV_PISTOLA_NEUMATICA:
                                this.mvPuntosVerificacion.ActiveViewIndex = 12;
                                break;

                        }

                    }
                    if (((EAreaConstruccion)this.Area) == EAreaConstruccion.RE) {
                        this.mvPuntosVerificacion.ActiveViewIndex = 7;
                    }
                    break;
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                    this.mvPuntosVerificacion.ActiveViewIndex = 5;
                    break;

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

        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa Generación.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa) {
            //Obteniendo el nombre de las etiquetas del archivo resource correspondiente.
            string FT = ObtenerEtiquetadelResource("EQ01", tipoEmpresa);

            //Se válida si la variable "FT" NO está vacía
            if (!string.IsNullOrEmpty(FT)) {
                ucucEquiposAliadosUnidadUI.CambiarEtiquetas(FT);
            }
        }

        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.presentador = new RegistrarRecepcionPSLPRE(this);
                this.presentador.PresentadorDocumentosEntrega = new ucCatalogoDocumentosPRE(ucCatalogoDocumentos);
                this.presentador.PresentadorDocumentosRecepcion = new ucCatalogoDocumentosPRE(UcCatalogoDocumentosUI1);
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                this.presentador.PresentadorEquiposAliados = new ucEquiposAliadosUnidadPRE(ucucEquiposAliadosUnidadUI);
                this.presentador.PresentadorExcavadoraEntrega = new ucPuntosVerificacionExcavadoraPSLPRE(ucucPuntosVerificacionExcavadoraEntrega);
                this.presentador.PresentadorExcavadoraRecepcion = new ucPuntosVerificacionExcavadoraPSLPRE(ucucPuntosVerificacionExcavadoraRecepcion);
                this.presentador.PresentadorRetroExcavadoraEntrega = new ucPuntosVerificacionRetroExcavadoraPSLPRE(ucucPuntosVerificacionRetroExcavadoraEntrega);
                this.presentador.PresentadorRetroExcavadoraRecepcion = new ucPuntosVerificacionRetroExcavadoraPSLPRE(ucucPuntosVerificacionRetroExcavadoraRecepcion);
                this.presentador.PresentadorEntregaRecepcionE = new ucPuntosVerificacionEntregaRecepcionPSLPRE(ucucPuntosVerificacionEntregaRecepcionEntrega);
                this.presentador.PresentadorEntregaRecepcionR = new ucPuntosVerificacionEntregaRecepcionPSLPRE(ucucPuntosVerificacionEntregaRecepcionRecepcion);
                this.presentador.PresentadorPistolaNeumaticaEntrega = new ucPuntosVerificacionPistolaNeumaticaPSLPRE(ucucPuntosVerificacionPistolaNeumaticaEntrega);
                this.presentador.PresentadorPistolaNeumaticaRecepcion = new ucPuntosVerificacionPistolaNeumaticaPSLPRE(ucucPuntosVerificacionPistolaNeumaticaRecepcion);
                this.presentador.PresentadorCompresoresPortatilesE = new ucPuntosVerificacionCompresoresPortatilesPSLPRE(ucucPuntosVerificacionCompresoresPortatilesE);
                this.presentador.PresentadorCompresoresPortatilesR = new ucPuntosVerificacionCompresoresPortatilesPSLPRE(ucucPuntosVerificacionCompresoresPortatilesR);
                this.presentador.PresentadorPlataformaTijerasE = new ucPuntosVerificacionPlataformaTijerasPSLPRE(ucPuntosVerificacionPlataformaTijerasE);
                this.presentador.PresentadorPlataformaTijerasR = new ucPuntosVerificacionPlataformaTijerasPSLPRE(ucPuntosVerificacionPlataformaTijerasR);
                this.presentador.PresentadorSubArrendadosE = new ucPuntosVerificacionSubArrendadoPSLPRE(ucucPuntosVerificacionSubArrendadoEntrega);
                this.presentador.PresentadorSubArrendadosR = new ucPuntosVerificacionSubArrendadoPSLPRE(ucucPuntosVerificacionSubArrendadoRecepcion);
                this.presentador.PresentadorVibroCompactadorEntrega = new ucPuntosVerificacionVibroCompactadorPSLPRE(ucucPuntosVerificacionVibroCompactadorEntrega);
                this.presentador.PresentadorVibroCompactadorRecepcion = new ucPuntosVerificacionVibroCompactadorPSLPRE(ucucPuntosVerificacionVibroCompactadorRecepcion);
                this.presentador.PresentadorTorresLuzEntrega = new ucPuntosVerificacionTorresLuzPSLPRE(ucucPuntosVerificacionTorresLuzEntrega);
                this.presentador.PresentadorTorresLuzRecepcion = new ucPuntosVerificacionTorresLuzPSLPRE(ucucPuntosVerificacionTorresLuzRecepcion);
                this.presentador.PresentadorMartilloHidraulicoEntrega = new ucPuntosVerificacionMartilloHidraulicoPSLPRE(ucucPuntosVerificacionMartilloHidraulicoEntrega);
                this.presentador.PresentadorMartilloHidraulicoRecepcion = new ucPuntosVerificacionMartilloHidraulicoPSLPRE(ucucPuntosVerificacionMartilloHidraulicoRecepcion);
                this.presentador.PresentadorMontaCargaEntrega = new ucPuntosVerificacionMontaCargaPSLPRE(ucucPuntosVerificacionMontaCargaEntrega);
                this.presentador.PresentadorMontacargaRecepcion = new ucPuntosVerificacionMontaCargaPSLPRE(ucucPuntosVerificacionMontaCargaRecepcion);
                this.presentador.PresentadorMiniCargadorEntrega = new ucPuntosVerificacionMiniCargadorPSLPRE(ucucPuntosVerificacionMiniCargadorEntrega);
                this.presentador.PresentadorMiniCargadorRecepcion = new ucPuntosVerificacionMiniCargadorPSLPRE(ucucPuntosVerificacionMiniCargadorRecepcion);
                this.presentador.PresentadorMotoNiveladoraE = new ucPuntosVerificacionMotoNiveladoraPSLPRE(ucucPuntosVerificacionMotoNiveladoraEntrega);
                this.presentador.PresentadorMotoNiveladoraR = new ucPuntosVerificacionMotoNiveladoraPSLPRE(ucucPuntosVerificacionMotoNiveladoraRecepcion);


                if (!Page.IsPostBack)
                    this.presentador.PrepararNuevo();
            } catch (Exception ex) {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Eventos
        protected void btnContinuar_Click(object sender, EventArgs e) {
            try {
                this.presentador.AvanzarPagina();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnContinuar_Click:" + Environment.NewLine + ex.Message);
            }
        }

        protected void btnAnterior_Click(object sender, EventArgs e) {
            try {
                this.presentador.RetrocederPagina();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnAnterior_Click:" + Environment.NewLine + ex.Message);
            }
        }

        protected void btnTerminar_Click(object sender, EventArgs e) {
            try {
                bool redirigir = this.presentador.RegistrarRecepcion();

            } catch (Exception ex) {
                if (ex.ToString().Contains("Ya se ha registrado un check list de recepción para la unidad. No se puede registrar de nuevo")) {
                    var s = "Ya se ha registrado un check list de recepción para la unidad. No se puede registrar de nuevo";
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                } else
                    this.MostrarMensaje("Inconsistencia al guardar el Check List.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + Environment.NewLine + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                this.presentador.CancelarRegistro();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Redirige a una página en especifico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBrincarPagina_Click(object sender, EventArgs e) {
            try {
                int numeroPagina = int.Parse(this.hdnPaginaBrinco.Value);
                this.presentador.IrAPagina(numeroPagina);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnBrincarPagina_Click:" + Environment.NewLine + ex.Message);
            }
        }

        protected void txtCombustible_TextChanged(object sender, EventArgs e) {
            try {
                string s = this.presentador.ValidarTanque();
                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s)) {
                    this.txtCombustible.Text = string.Empty;
                    this.txtCombustible.Focus();
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    this.txtCombustible.Focus();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al momento de evaluar la cantidad de combustible.", ETipoMensajeIU.ERROR, string.Format("{0}.txtCombustible_TextChanged: {1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion

        protected void txtHorometro_OnTextChanged(object sender, EventArgs e) {
            try {
                if (this.HorometroEntrega.HasValue && this.Horometro.HasValue) {
                    string s;

                    //Se valida que las horas esten correcta
                    if ((s = this.presentador.ValidarKilometraje()) != null) {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }
                }
                this.txtCombustible.Focus();
            } catch (Exception ex) {

            }
        }
    }
}