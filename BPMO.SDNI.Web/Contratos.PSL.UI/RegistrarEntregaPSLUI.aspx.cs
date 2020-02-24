using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
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
    public partial class RegistrarEntregaPSLUI : System.Web.UI.Page, IRegistrarEntregaPSLVIS {
        #region Atributos

        /// <summary>
        /// Presentador de la página
        /// </summary>
        private RegistrarEntregaPSLPRE presentador = null;

        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "RegistrarEntregaUnidadPSLUI";

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

        /// <summary>
        /// Obtiene o establece el nombre del usuario que entrega la unidad
        /// </summary>
        public string NombreUsuarioEntrega {
            get { return this.txtNombreUsuarioEntrega.Text.Trim().ToUpper(); }
            set {
                this.txtNombreUsuarioEntrega.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }

        public int? HorometroSalida {
            get {
                int val;
                return !string.IsNullOrEmpty(this.txtHorometro.Text) &&
                       !string.IsNullOrWhiteSpace(this.txtHorometro.Text)
                           ? (Int32.TryParse(this.txtHorometro.Text, out val) ? (int?)val : null)
                           : null;
            }

            set {
                this.txtHorometro.Text = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }

        /// <summary>
        ///  Obtiene o Asigna el ultimo valor del Horometro de Entrega de la Unidad
        /// </summary>
        public Int32? HorometroAnterior {
            get {
                if (!string.IsNullOrEmpty(this.hdnHorometroAnterior.Value) && !string.IsNullOrWhiteSpace(this.hdnHorometroAnterior.Value)) {
                    int val = 0;
                    return Int32.TryParse(this.hdnHorometroAnterior.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.hdnHorometroAnterior.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el combustible de salida
        /// </summary>
        public int? Combustible {
            get {
                if (!string.IsNullOrEmpty(this.txtCombustibleSalida.Text) && !string.IsNullOrWhiteSpace(this.txtCombustibleSalida.Text)) {
                    int val = 0;
                    return Int32.TryParse(this.txtCombustibleSalida.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set {
                this.txtCombustibleSalida.Text = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty;
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
        /// Obtiene o establece la fecha en que se realiza el check list
        /// </summary>
        public DateTime? FechaListado {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaSalida.Text) && !string.IsNullOrWhiteSpace(this.txtFechaSalida.Text)) {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaSalida.Text, out date)
                               ? (DateTime?)date
                               : null;
                }
                return null;
            }
            set {
                this.txtFechaSalida.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o estable la hora de salida de la unidad
        /// </summary>
        public TimeSpan? HoraListado {
            get {
                if (!string.IsNullOrEmpty(this.txtHoraSalida.Text) && !string.IsNullOrWhiteSpace(this.txtHoraSalida.Text)) {
                    var time = DateTime.ParseExact(this.txtHoraSalida.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set {
                if (value != null) {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraSalida.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                } else
                    this.txtHoraSalida.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece l tipo de check list que se va a registrar
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
        /// Capacidad del tanque de la unidad que se va a entregar
        /// </summary>
        public decimal? CapacidadTanque {
            get {
                if (!string.IsNullOrEmpty(this.txtTanqueCobustible.Text) && !string.IsNullOrWhiteSpace(this.txtTanqueCobustible.Text)) {
                    decimal val;
                    return decimal.TryParse(this.txtTanqueCobustible.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtTanqueCobustible.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public string Observaciones {
            get { return this.txtObservaciones.Text.Trim().ToUpper(); }

            set {
                this.txtObservaciones.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                             ? value.Trim().ToUpper()
                                             : string.Empty;
            }

        }

        public List<ArchivoBO> Adjuntos {
            get {
                if (ucCatalogoDocumentos.ObtenerArchivos() != null)
                    return ucCatalogoDocumentos.ObtenerArchivos();
                else
                    return new List<ArchivoBO>();
            }
            set { ucCatalogoDocumentos.EstablecerListasArchivos(value, null); }
        }

        public List<TipoArchivoBO> TiposArchivo {
            get { return ucCatalogoDocumentos.TiposArchivo; }
            set { ucCatalogoDocumentos.TiposArchivo = value; }
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

        /// <summary>
        /// Contiene la lista de acciones a las cuales tiene acceso el usuario.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }

        public bool? EsIntercambio {
            get {
                if (Session["EsIntercambio"] == null) return null;
                return (bool)Session["EsIntercambio"];
            }
            set {
                Session["EsIntercambio"] = value;
            }
        }
        /// <summary>
        /// Último contrato que se consultó
        /// </summary>
        public object UltimoObjeto {
            get {
                if (Session["UltimoObjetoContratoRO"] != null)
                    return Session["UltimoObjetoContratoRO"];

                return null;
            }
            set {
                if (value != null)
                    Session["UltimoObjetoContratoRO"] = value;
                else
                    Session.Remove("UltimoObjetoContratoRO");
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Reestablece los controles en caso de alguna inconsistencia
        /// </summary>
        private void ReestablecerControles() {
            this.btnCancelar.Enabled = true;
            this.btnGuardar.Enabled = false;
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
        /// Elimina de la Session las variables usadas en el caso de uso
        /// </summary>
        public void LimpiarSesion() {
            Session.Remove("NewObject");
            Session.Remove("CheckListEntrega");
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
            Session.Remove("UltimoObjetoContratoRO");
        }
        /// <summary>
        /// Limpia el contrato al cual se le esta registrando el check list
        /// </summary>
        public void LimpiarPaqueteNavegacion() {
            Session.Remove("RegistrarEntregaPSLUI");
            Session.Remove("LineaContratoPSLID");
        }

        public void LimpiarVariableIntercambio() {
            if (Session["NombreReporte"] != null)
                Session.Remove("EsIntercambio");
        }
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de consulta de check list
        /// </summary>
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarListadoVerificacionPSLUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de impresión del reporte
        /// </summary>
        /// <param name="error">Mensaje de error que se mostrará en la interfaz</param>
        public void RedirigirAImprimir(string error) {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx?error=" + error, false);
        }
        /// <summary>
        /// Guarda en la variable de Session un paquete para su posterior consulta
        /// </summary>
        /// <param name="key">Clave del paquete de navegación</param>
        /// <param name="value">Paquete que se desea guardar en la session</param>
        public void EstablecerPaqueteNavegacion(string key, object value) {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
        }
        /// <summary>
        /// Obtiene un paquete guardado en la session
        /// </summary>
        /// <returns>Objeto guardado en la session</returns>
        public object ObtenerPaqueteContrato() {
            return Session["RegistrarEntregaPSLUI"] ?? null;
        }

        /// <summary>
        /// Obtiene un paquete guardado en la session
        /// </summary>
        /// <returns>Objeto guardado en la session</returns>
        public object ObtenerPaqueteLineaContrato() {
            return Session["LineaContratoPSLID"] ?? null;
        }

        public void BloquearRegistro() {
            btnGuardar.Enabled = false;
            txtCombustibleSalida.Enabled = false;
            txtHoraSalida.Enabled = false;
            txtHorometro.Enabled = false;
            txtObservaciones.Enabled = false;
            txtTanqueCobustible.Enabled = false;

            ucCatalogoDocumentos.presentador.ModoEditable(false);
            ucPuntosVerificacionExcavadoraPSLUI.ModoEdicion(false);
            ucPuntosVerificacionEntregaRecepcionPSLUI.ModoEdicion(false);
            ucPuntosVerificacionRetroExcavadoraPSLUI.ModoEdicion(false);
            ucPuntosVerificacionMotoNiveladoraPSLUI.ModoEdicion(false);
            ucPuntosVerificacionPistolaNeumaticaPSLUI.ModoEdicion(false);
            ucPuntosVerificacionMartilloHidraulicoPSLUI.ModoEdicion(false);
            ucPuntosVerificacionTorresLuzPSLUI.ModoEdicion(false);
            ucPuntosVerificacionVibroCompactadorPSLUI.ModoEdicion(false);
            txtCombustibleSalida.ReadOnly = true;
            txtHoraSalida.ReadOnly = true;
            txtObservaciones.ReadOnly = true;
            txtHorometro.ReadOnly = true;
            txtTanqueCobustible.ReadOnly = true;
        }
        /// <summary>
        /// Prepara la interfaz para el registro de un CheckList
        /// </summary>
        public void PrepararNuevo() {
            this.txtNumeroContrato.ReadOnly = true;
            this.txtNombreCliente.ReadOnly = true;
            this.txtNumeroEconomico.ReadOnly = true;
            this.txtNumeroSerie.ReadOnly = true;
            this.txtPlacasEstatales.ReadOnly = true;
            this.txtNombreUsuarioEntrega.ReadOnly = true;
            this.txtNumeroLicencia.ReadOnly = true;
            this.txtFechaSalida.ReadOnly = true;
            txtTanqueCobustible.ReadOnly = true;


            this.txtNumeroContrato.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.txtNombreCliente.Enabled = false;
            this.txtNumeroEconomico.Enabled = false;
            this.txtPlacasEstatales.Enabled = false;
            this.txtNombreUsuarioEntrega.Enabled = false;
            this.txtNumeroLicencia.Enabled = false;
            this.txtFechaSalida.Enabled = false;
            txtTanqueCobustible.Enabled = false;


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
                            case ETipoUnidad.LV_UNIDADES_USADAS:
                                this.mvPuntosVerificacion.ActiveViewIndex = 13;
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

                this.presentador = new RegistrarEntregaPSLPRE(this);
                this.presentador.PresentadorDocumentos = new ucCatalogoDocumentosPRE(ucCatalogoDocumentos);
                this.presentador.PresentadorEquipoAliados = new ucEquiposAliadosUnidadPRE(ucucEquiposAliadosUnidadUI);
                this.presentador.PresentadorExcavadora = new ucPuntosVerificacionExcavadoraPSLPRE(ucPuntosVerificacionExcavadoraPSLUI);
                this.presentador.PresentadorEntregaRecepcion = new ucPuntosVerificacionEntregaRecepcionPSLPRE(ucPuntosVerificacionEntregaRecepcionPSLUI);
                this.presentador.PresentadorRetroExcavadora = new ucPuntosVerificacionRetroExcavadoraPSLPRE(ucPuntosVerificacionRetroExcavadoraPSLUI);
                this.presentador.PresentadorCompresorPortatil = new ucPuntosVerificacionCompresoresPortatilesPSLPRE(ucPuntosVerificacionCompresoresPortatilesPSLUI);
                this.presentador.PresentadorMontaCarga = new ucPuntosVerificacionMontaCargaPSLPRE(ucPuntosVerificacionMontaCargaPSLUI);
                this.presentador.PresentadorMiniCargador = new ucPuntosVerificacionMiniCargadorPSLPRE(ucPuntosVerificacionMiniCargadorPSLUI);
                this.presentador.PresentadorPlataformaTijeras = new ucPuntosVerificacionPlataformaTijerasPSLPRE(ucPuntosVerificacionPlataformaTijerasPSLUI);
                this.presentador.PresentadorPistolaNeumatica = new ucPuntosVerificacionPistolaNeumaticaPSLPRE(ucPuntosVerificacionPistolaNeumaticaPSLUI);
                this.presentador.PresentadorSubArrendados = new ucPuntosVerificacionSubArrendadoPSLPRE(ucPuntosVerificacionSubArrendadoPSLUI);
                this.presentador.PresentadorMotoNiveladora = new ucPuntosVerificacionMotoNiveladoraPSLPRE(ucPuntosVerificacionMotoNiveladoraPSLUI);
                this.presentador.PresentadorMartilloHidraulico = new ucPuntosVerificacionMartilloHidraulicoPSLPRE(ucPuntosVerificacionMartilloHidraulicoPSLUI);
                this.presentador.PresentadorTorresLuz = new ucPuntosVerificacionTorresLuzPSLPRE(ucPuntosVerificacionTorresLuzPSLUI);
                this.presentador.PresentadorVibroCompactador = new ucPuntosVerificacionVibroCompactadorPSLPRE(ucPuntosVerificacionVibroCompactadorPSLUI);

                if (!Page.IsPostBack)
                    this.presentador.PrepararNuevo();
            } catch (Exception ex) {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }

        }
        #endregion

        #region Eventos
        /// <summary>
        /// Cancela el registro del listado de verificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                presentador.CancelarRegistro();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al cancelar el registro del Check List.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Guarda los listados de verificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                presentador.RegistrarEntrega();
            } catch (Exception ex) {
                if (ex.ToString().Contains("Ya se ha registrado un check list de entrega para la unidad. No se puede registrar de nuevo")) {
                    var s = "Ya se ha registrado un check list de entrega para la unidad. No se puede registrar de nuevo";
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                } else
                    this.MostrarMensaje("Inconsistencia al guardar el Check List.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }


        protected void txtHorometro_OnTextChanged(object sender, EventArgs e) {
            try {
                string s = this.presentador.ValidarHorometro();
                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s)) {
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al calcular las horas.", ETipoMensajeIU.ERROR, nombreClase + ".txtHorometro_OnTextChanged: " + Environment.NewLine + ex.Message);
            }
        }

        protected void txtCombustibleSalida_TextChanged(object sender, EventArgs e) {
            try {
                string s = this.presentador.ValidarTanque();
                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s)) {
                    this.txtCombustibleSalida.Text = string.Empty;
                    this.txtCombustibleSalida.Focus();
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    this.txtCombustibleSalida.Focus();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al momento de evaluar la cantidad de combustible.", ETipoMensajeIU.ERROR, string.Format("{0}.txtCombustibleSalida_TextChanged: {1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion
    }
}