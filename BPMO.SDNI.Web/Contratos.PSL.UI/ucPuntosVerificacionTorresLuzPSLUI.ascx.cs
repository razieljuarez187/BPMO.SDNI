using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionTorresLuzPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionTorresLuzPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionTorresLuzPSLUI";
        private ucPuntosVerificacionTorresLuzPSLPRE presentador;

        #endregion
        #region Propiedades
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxAceiteMotor
        /// </summary>
        public bool? tieneAceiteMotor {
            get {

                return this.chbxAceiteMotor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteMotor.Checked = true;
                    else
                        chbxAceiteMotor.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxAntenasMonitoreoSatelital
        /// </summary>
        public bool? tieneAntenasMonitoreo {
            get {

                return this.chbxAntenasMonitoreoSatelital.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAntenasMonitoreoSatelital.Checked = true;
                    else
                        chbxAntenasMonitoreoSatelital.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxBandaVentilador
        /// </summary>
        public bool? tieneBandaVentilador {
            get {

                return this.chbxBandaVentilador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBandaVentilador.Checked = true;
                    else
                        chbxBandaVentilador.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxBateria
        /// </summary>
        public bool? tieneBateria {
            get {

                return this.chbxBateria.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBateria.Checked = true;
                    else
                        chbxBateria.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxCableLevanteTorre
        /// </summary>
        public bool? tieneCableLevanteTorre {
            get {

                return this.chbxCableLevanteTorre.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCableLevanteTorre.Checked = true;
                    else
                        chbxCableLevanteTorre.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxCartuchoFiltroAire
        /// </summary>
        public bool? tieneCartuchoFiltroAire {
            get {

                return this.chbxCartuchoFiltroAire.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCartuchoFiltroAire.Checked = true;
                    else
                        chbxCartuchoFiltroAire.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxCombustible
        /// </summary>
        public bool? tieneCombustible {
            get {

                return this.chbxCombustible.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCombustible.Checked = true;
                    else
                        chbxCombustible.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxCondicionCalcas
        /// </summary>
        public bool? tieneCondicionCalcas {
            get {

                return this.chbxCondicionCalcas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionCalcas.Checked = true;
                    else
                        chbxCondicionCalcas.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxCondicionLlantas
        /// </summary>
        public bool? tieneCondicionLLantas {
            get {

                return this.chbxCondicionLlantas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionLlantas.Checked = true;
                    else
                        chbxCondicionLlantas.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxCondicionPintura
        /// </summary>
        public bool? tieneCondicionPintura {
            get {

                return this.chbxCondicionPintura.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionPintura.Checked = true;
                    else
                        chbxCondicionPintura.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxEstructuraChasis
        /// </summary>
        public bool? tieneEstructuraChasis {
            get {

                return this.chbxEstructuraChasis.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxEstructuraChasis.Checked = true;
                    else
                        chbxEstructuraChasis.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxLiquidoRefrigerante
        /// </summary>
        public bool? tieneLiquidoRefrigerante {
            get {

                return this.chbxLiquidoRefrigerante.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLiquidoRefrigerante.Checked = true;
                    else
                        chbxLiquidoRefrigerante.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxLucesAdvertencia
        /// </summary>
        public bool? tieneLucesAdvertencia {
            get {

                return this.chbxLucesAdvertencia.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesAdvertencia.Checked = true;
                    else
                        chbxLucesAdvertencia.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxLucesTorre
        /// </summary>
        public bool? tieneLucesTorre {
            get {

                return this.chbxLucesTorre.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesTorre.Checked = true;
                    else
                        chbxLucesTorre.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxManguerasAbrazaderasAdmisionAire
        /// </summary>
        public bool? tieneManguerasAbrazaderas {
            get {

                return this.chbxManguerasAbrazaderasAdmisionAire.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxManguerasAbrazaderasAdmisionAire.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAire.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxPresionLlantas
        /// </summary>
        public bool? tienePresionLlantas {
            get {

                return this.chbxPresionLlantas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPresionLlantas.Checked = true;
                    else
                        chbxPresionLlantas.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxSimbolosSeguridadMaquina
        /// </summary>
        public bool? tieneSimbolosSeguridad {
            get {

                return this.chbxSimbolosSeguridadMaquina.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxSimbolosSeguridadMaquina.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquina.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxSwitchEncendido
        /// </summary>
        public bool? tieneSwitchEncendido {
            get {

                return this.chbxSwitchEncendido.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxSwitchEncendido.Checked = true;
                    else
                        chbxSwitchEncendido.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxTapaCombustible
        /// </summary>
        public bool? tieneTapaCombustible {
            get {

                return this.chbxTapaCombustible.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTapaCombustible.Checked = true;
                    else
                        chbxTapaCombustible.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxVelocidadMaximaMotor
        /// </summary>
        public bool? tieneVelocidadMaxima {
            get {

                return this.chbxVelocidadMaximaMotor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxVelocidadMaximaMotor.Checked = true;
                    else
                        chbxVelocidadMaximaMotor.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxVelocidadMinimaMotor
        /// </summary>
        public bool? tieneVelocidadMinima {
            get {

                return this.chbxVelocidadMinimaMotor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxVelocidadMinimaMotor.Checked = true;
                    else
                        chbxVelocidadMinimaMotor.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLamparasTablero
        /// </summary>
        public bool? tieneLamparasTablero {
            get {

                return this.chbxLamparasTablero.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLamparasTablero.Checked = true;
                    else
                        chbxLamparasTablero.Checked = false;

                }
            }
        }
        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionTorresLuzPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion
        #region Métodos
        /// <summary>
        /// Coloca los controles en Modo de Edición
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edición</param>
        public void ModoEdicion(bool activo) {

            this.chbxAceiteMotor.Enabled = activo;
            this.chbxAntenasMonitoreoSatelital.Enabled = activo;
            this.chbxBandaVentilador.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxCableLevanteTorre.Enabled = activo;
            this.chbxCartuchoFiltroAire.Enabled = activo;
            this.chbxCombustible.Enabled = activo;
            this.chbxCondicionCalcas.Enabled = activo;

            this.chbxCondicionLlantas.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxEstructuraChasis.Enabled = activo;
            this.chbxLiquidoRefrigerante.Enabled = activo;
            this.chbxLucesAdvertencia.Enabled = activo;
            this.chbxLucesTorre.Enabled = activo;
            this.chbxManguerasAbrazaderasAdmisionAire.Enabled = activo;
            this.chbxLamparasTablero.Enabled = activo;
            this.chbxPresionLlantas.Enabled = activo;
            this.chbxSimbolosSeguridadMaquina.Enabled = activo;
            this.chbxSwitchEncendido.Enabled = activo;
            this.chbxTapaCombustible.Enabled = activo;

            this.chbxVelocidadMaximaMotor.Enabled = activo;
            this.chbxVelocidadMinimaMotor.Enabled = activo;

        }
        /// <summary>
        /// Muestra mensaje en la interfaz del usuario
        /// </summary>
        /// <param name="mensaje">Mensaje que será mostrado</param>
        /// <param name="tipo">El tipo de mensaje que será mostrado</param>
        /// <param name="detalle">Detalle del mensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion
    }
}