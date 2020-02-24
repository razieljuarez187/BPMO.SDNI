using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionVibroCompactadorPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionVibroCompactadorPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionVibroCompactadorPSLUI";
        private ucPuntosVerificacionVibroCompactadorPSLPRE presentador;

        #endregion
        #region Propiedades
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteHidraulico
        /// </summary>
        public bool? tieneAceiteHidraulico {
            get {

                return this.chbxAceiteHidraulico.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteHidraulico.Checked = true;
                    else
                        chbxAceiteHidraulico.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteMotor
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
        /// Asigna u obtiene el valor del checkbox chbxAlarmaReversa
        /// </summary>
        public bool? tieneAlarmaReversa {
            get {

                return this.chbxAlarmaReversa.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAlarmaReversa.Checked = true;
                    else
                        chbxAlarmaReversa.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAntenasMonitoreoSatelital
        /// </summary>
        public bool? tieneAntenaMonitoreo {
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
        /// Asigna u obtiene el valor del checkbox chbxBandaVentilador
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
        /// Asigna u obtiene el valor del checkbox chbxBateria
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
        /// Asigna u obtiene el valor del checkbox chbxCabinaOperador
        /// </summary>
        public bool? tieneCabinaOperador {
            get {

                return this.chbxCabinaOperador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCabinaOperador.Checked = true;
                    else
                        chbxCabinaOperador.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCajaReduccionEngranes
        /// </summary>
        public bool? tieneCajaReduccionEngranes {
            get {

                return this.chbxCajaReduccionEngranes.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCajaReduccionEngranes.Checked = true;
                    else
                        chbxCajaReduccionEngranes.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCartuchoFiltroAire
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
        /// Asigna u obtiene el valor del checkbox chbxCinturonSeguridad
        /// </summary>
        public bool? tieneCinturonSeguridad {
            get {

                return this.chbxCinturonSeguridad.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCinturonSeguridad.Checked = true;
                    else
                        chbxCinturonSeguridad.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCofreMotor
        /// </summary>
        public bool? tieneCofreMotor {
            get {

                return this.chbxCofreMotor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCofreMotor.Checked = true;
                    else
                        chbxCofreMotor.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCombustible
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
        /// Asigna u obtiene el valor del checkbox chbxCondicionAsiento
        /// </summary>
        public bool? tieneCondicionAsiento {
            get {

                return this.chbxCondicionAsiento.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionAsiento.Checked = true;
                    else
                        chbxCondicionAsiento.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionCalcas
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
        /// Asigna u obtiene el valor del checkbox chbxCondicionLlantas
        /// </summary>
        public bool? tieneCondicionLlanta {
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
        /// Asigna u obtiene el valor del checkbox chbxCondicionPintura
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
        /// Asigna u obtiene el valor del checkbox chbxEstructuraChasis
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
        /// Asigna u obtiene el valor del checkbox chbxFrenoEstacionamiento
        /// </summary>
        public bool? tieneFrenoEstacionamiento {
            get {

                return this.chbxFrenoEstacionamiento.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxFrenoEstacionamiento.Checked = true;
                    else
                        chbxFrenoEstacionamiento.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxIndicadores
        /// </summary>
        public bool? tieneIndicadores {
            get {

                return this.chbxIndicadores.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxIndicadores.Checked = true;
                    else
                        chbxIndicadores.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxInterruptorDesconexion
        /// </summary>
        public bool? tieneInterruptorDesconexion {
            get {

                return this.chbxInterruptorDesconexion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxInterruptorDesconexion.Checked = true;
                    else
                        chbxInterruptorDesconexion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLamparasTablero
        /// </summary>
        public bool? tieneLamparaTablero {
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
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLiquidoRefrigerante
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
        /// Asigna u obtiene el valor del checkbox chbxLucesAdvertencia
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
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajoDelanteras
        /// </summary>
        public bool? tieneLucesTrabajoDelantera {
            get {

                return this.chbxLucesTrabajoDelanteras.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesTrabajoDelanteras.Checked = true;
                    else
                        chbxLucesTrabajoDelanteras.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajoTraseras
        /// </summary>
        public bool? tieneLucesTrabajoTraseras {
            get {

                return this.chbxLucesTrabajoTraseras.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesTrabajoTraseras.Checked = true;
                    else
                        chbxLucesTrabajoTraseras.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxManguerasAbrazaderasAdmisionAire
        /// </summary>
        public bool? tieneManguerasAbrazaderas {
            get {

                return this.chbxManguerasAbrazaderas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxManguerasAbrazaderas.Checked = true;
                    else
                        chbxManguerasAbrazaderas.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPalanca
        /// </summary>
        public bool? tienePalanca {
            get {

                return this.chbxPalanca.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPalanca.Checked = true;
                    else
                        chbxPalanca.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPivotesArticulacionDireccion
        /// </summary>
        public bool? tienePivoteArticulacionDireccion {
            get {

                return this.chbxPivotesArticulacionDireccion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPivotesArticulacionDireccion.Checked = true;
                    else
                        chbxPivotesArticulacionDireccion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPresionLlantas
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
        /// Asigna u obtiene el valor del checkbox chbxRascadoresTambor
        /// </summary>
        public bool? tieneRascadoresTambor {
            get {

                return this.chbxRascadoresTambor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxRascadoresTambor.Checked = true;
                    else
                        chbxRascadoresTambor.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxReductorEngranes
        /// </summary>
        public bool? tieneReductorEngranes {
            get {

                return this.chbxReductorEngranes.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxReductorEngranes.Checked = true;
                    else
                        chbxReductorEngranes.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxSimbolosSeguridadMaquina
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
        /// Asigna u obtiene el valor del checkbox chbxSistemaVibracion
        /// </summary>
        public bool? tieneSistemaVibracion {
            get {

                return this.chbxSistemaVibracion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxSistemaVibracion.Checked = true;
                    else
                        chbxSistemaVibracion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTacometro
        /// </summary>
        public bool? tieneTacometro {
            get {

                return this.chbxTacometro.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTacometro.Checked = true;
                    else
                        chbxTacometro.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTapaCombustible
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
        /// Asigna u obtiene el valor del checkbox chbxTapaHidraulico
        /// </summary>
        public bool? tieneTapaHidraulico {
            get {

                return this.chbxTapaHidraulico.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTapaHidraulico.Checked = true;
                    else
                        chbxTapaHidraulico.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMaximaMotor
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
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMinimaMotor
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
        /// Asigna u obtiene el valor del checkbox chbxVibrador
        /// </summary>
        public bool? tieneVibrador {
            get {

                return this.chbxVibrador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxVibrador.Checked = true;
                    else
                        chbxVibrador.Checked = false;

                }
            }
        }
        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionVibroCompactadorPSLPRE(this);
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
            this.chbxAceiteHidraulico.Enabled = activo;
            this.chbxAceiteMotor.Enabled = activo;
            this.chbxAlarmaReversa.Enabled = activo;
            this.chbxAntenasMonitoreoSatelital.Enabled = activo;
            this.chbxBandaVentilador.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxCabinaOperador.Enabled = activo;
            this.chbxCajaReduccionEngranes.Enabled = activo;

            this.chbxCartuchoFiltroAire.Enabled = activo;
            this.chbxCinturonSeguridad.Enabled = activo;
            this.chbxCofreMotor.Enabled = activo;
            this.chbxCombustible.Enabled = activo;
            this.chbxCondicionAsiento.Enabled = activo;
            this.chbxCondicionCalcas.Enabled = activo;
            this.chbxCondicionLlantas.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxEstructuraChasis.Enabled = activo;
            this.chbxFrenoEstacionamiento.Enabled = activo;
            this.chbxIndicadores.Enabled = activo;
            this.chbxInterruptorDesconexion.Enabled = activo;

            this.chbxLamparasTablero.Enabled = activo;
            this.chbxLiquidoRefrigerante.Enabled = activo;
            this.chbxLucesAdvertencia.Enabled = activo;
            this.chbxLucesTrabajoDelanteras.Enabled = activo;
            this.chbxLucesTrabajoTraseras.Enabled = activo;
            this.chbxManguerasAbrazaderas.Enabled = activo;

            this.chbxPalanca.Enabled = activo;
            this.chbxPivotesArticulacionDireccion.Enabled = activo;
            this.chbxPresionLlantas.Enabled = activo;
            this.chbxRascadoresTambor.Enabled = activo;
            this.chbxReductorEngranes.Enabled = activo;
            this.chbxSimbolosSeguridadMaquina.Enabled = activo;

            this.chbxSistemaVibracion.Enabled = activo;
            this.chbxTacometro.Enabled = activo;
            this.chbxTapaCombustible.Enabled = activo;
            this.chbxTapaHidraulico.Enabled = activo;
            this.chbxVelocidadMaximaMotor.Enabled = activo;
            this.chbxVelocidadMinimaMotor.Enabled = activo;
            this.chbxVibrador.Enabled = activo;
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