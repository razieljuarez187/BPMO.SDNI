using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionRetroExcavadoraPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionRetroExcavadoraPSLVIS {

        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionRetroExcavadoraPSLUI";
        private ucPuntosVerificacionRetroExcavadoraPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteTransmision
        /// </summary>
        public bool tieneAceiteTransmision {
            get {
                return this.chbxAceiteTransmision.Checked;
            }
            set {
                chbxAceiteTransmision.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteDiferencialDel
        /// </summary>
        public bool tieneAceiteDiferencialDel {
            get {
                return this.chbxAceiteDiferencialDel.Checked;
            }
            set {
                this.chbxAceiteDiferencialDel.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteDiferencialTrasero
        /// </summary>
        public bool tieneAceiteDiferencialTrasero {
            get {
                return this.chbxAceiteDiferencialTrasero.Checked;
            }
            set {
                this.chbxAceiteDiferencialTrasero.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceitePlanetariosDel4x4
        /// </summary>
        public bool tieneAceitePlanetariosDel {
            get {
                return this.chbxAceitePlanetariosDel4x4.Checked;
            }
            set {
                this.chbxAceitePlanetariosDel4x4.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAntenasMonitoreoSatelital
        /// </summary>
        public bool tieneAntenaMonitoreo {
            get {
                return this.chbxAntenasMonitoreoSatelital.Checked;
            }
            set {
                this.chbxAntenasMonitoreoSatelital.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAsientoOperador
        /// </summary>
        public bool tieneAsientoOperador {
            get {
                return this.chbxAsientoOperador.Checked;
            }
            set {
                this.chbxAsientoOperador.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBandaVentilador
        /// </summary>
        public bool tieneBandaVentilador {
            get {
                return this.chbxBandaVentilador.Checked;
            }
            set {
                this.chbxBandaVentilador.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBloqueoDiferencial
        /// </summary>
        public bool tieneBloqueoDiferencial {
            get {
                return this.chbxBloqueoDiferencial.Checked;
            }
            set {
                this.chbxBloqueoDiferencial.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBoteDelantero
        /// </summary>
        public bool tieneBoteDelantero {
            get {
                return this.chbxBoteDelantero.Checked;
            }
            set {
                this.chbxBoteDelantero.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBoteTrasero
        /// </summary>
        public bool tieneBoteTrasero {
            get {
                return this.chbxBoteTrasero.Checked;
            }
            set {
                this.chbxBoteTrasero.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBujesPasadoresCargador
        /// </summary>
        public bool tieneBujesPasadoresCargador {
            get {
                return this.chbxBujesPasadoresCargador.Checked;
            }
            set {
                this.chbxBujesPasadoresCargador.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBujesPasadoresRetro
        /// </summary>
        public bool tieneBujesPasadoresRetro {
            get {
                return this.chbxBujesPasadoresRetro.Checked;
            }
            set {
                chbxBujesPasadoresRetro.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCartuchoFiltroAire
        /// </summary>
        public bool tieneCartuchoFiltroAire {
            get {
                return this.chbxCartuchoFiltroAire.Checked;
            }
            set {
                this.chbxCartuchoFiltroAire.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCinturonSeguridad
        /// </summary>
        public bool tieneCinturonSeguridad {
            get {
                return this.chbxCinturonSeguridad.Checked;
            }
            set {
                this.chbxCinturonSeguridad.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxClaxon
        /// </summary>
        public bool tieneClaxon {
            get {
                return this.chbxClaxon.Checked;
            }
            set {
                this.chbxClaxon.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionLlantas
        /// </summary>
        public bool tieneCondicionLlantas {
            get {
                return this.chbxCondicionLlantas.Checked;
            }
            set {
                this.chbxCondicionLlantas.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxDesacople4x4
        /// </summary>
        public bool tieneDesacople {
            get {
                return this.chbxDesacople4x4.Checked;
            }
            set {
                this.chbxDesacople4x4.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxEspejoRetrovisor
        /// </summary>
        public bool tieneEspejoRetrovisor {
            get {
                return this.chbxEspejoRetrovisor.Checked;
            }
            set {
                this.chbxEspejoRetrovisor.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxFrenoEstacionamiento
        /// </summary>
        public bool tieneFrenoEstacionamiento {
            get {
                return this.chbxFrenoEstacionamiento.Checked;
            }
            set {
                chbxFrenoEstacionamiento.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesDireccionales
        /// </summary>
        public bool tieneLucesDireccionales {
            get {
                return this.chbxLucesDireccionales.Checked;
            }
            set {
                this.chbxLucesDireccionales.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesIntermitentes
        /// </summary>
        public bool tieneLucesIntermitentes {
            get {
                return this.chbxLucesIntermitentes.Checked;
            }
            set {
                this.chbxLucesIntermitentes.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajoDelanteras
        /// </summary>
        public bool tieneLucesTrabajoDelantera {
            get {
                return this.chbxLucesTrabajoDelanteras.Checked;
            }
            set {
                this.chbxLucesTrabajoDelanteras.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajoTraseras
        /// </summary>
        public bool tieneLucesTrabajoTraseras {
            get {
                return this.chbxLucesTrabajoTraseras.Checked;
            }
            set {
                chbxLucesTrabajoTraseras.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxManguerasAbrazaderasAdmisionAire
        /// </summary>
        public bool tieneManguerasAbrazaderas {
            get {
                return this.chbxManguerasAbrazaderasAdmisionAire.Checked;
            }
            set {
                this.chbxManguerasAbrazaderasAdmisionAire.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxNeutralizadorTransmision
        /// </summary>
        public bool tieneNeutralizadorTransmision {
            get {
                return this.chbxNeutralizadorTransmision.Checked;
            }
            set {
                this.chbxNeutralizadorTransmision.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPalancaTransito
        /// </summary>
        public bool tienePalancaTransito {
            get {
                return this.chbxPalancaTransito.Checked;
            }
            set {
                this.chbxPalancaTransito.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPresionLlantas
        /// </summary>
        public bool tienePresionLLantas {
            get {
                return this.chbxPresionLlantas.Checked;
            }
            set {
                this.chbxPresionLlantas.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxSimbolosSeguridadMaquina
        /// </summary>
        public bool tieneSimbolosSeguridad {
            get {
                return this.chbxSimbolosSeguridadMaquina.Checked;
            }
            set {
                chbxSimbolosSeguridadMaquina.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTacometro
        /// </summary>
        public bool tieneTacometro {
            get {
                return this.chbxTacometro.Checked;
            }
            set {

                chbxTacometro.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTapaHidraulico
        /// </summary>
        public bool tieneTapaHidraulico {
            get {
                return this.chbxTapaHidraulico.Checked;
            }
            set {

                chbxTapaHidraulico.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMaximaMotor
        /// </summary>
        public bool tieneVelocidadMaxima {
            get {
                return this.chbxVelocidadMaximaMotor.Checked;
            }
            set {

                chbxVelocidadMaximaMotor.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMinimaMotor
        /// </summary>
        public bool tieneVelocidadMinima {
            get {
                return this.chbxVelocidadMinimaMotor.Checked;
            }
            set {
                chbxVelocidadMinimaMotor.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxZapatasEstabilizadores
        /// </summary>
        public bool tieneZapatasEstabilizadores {
            get {
                return this.chbxZapatasEstabilizadores.Checked;
            }
            set {
                chbxZapatasEstabilizadores.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteMotor
        /// </summary>
        public bool tieneAceiteHidraulico {
            get {
                return this.chbxAceiteHidraulico.Checked;
            }
            set {
                chbxAceiteHidraulico.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteMotor
        /// </summary>
        public bool tieneAceiteMotor {
            get {
                return this.chbxAceiteMotor.Checked;
            }
            set {
                chbxAceiteMotor.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAlarmaReversa
        /// </summary>
        public bool tieneAlarmaReversa {
            get {
                return this.chbxAlarmaReversa.Checked;
            }
            set {
                chbxAlarmaReversa.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBateria
        /// </summary>
        public bool tieneBateria {
            get {
                return this.chbxBateria.Checked;
            }
            set {
                chbxBateria.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCombustible
        /// </summary>
        public bool tieneCombustible {
            get {
                return this.chbxCombustible.Checked;
            }
            set {
                chbxCombustible.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionAsiento
        /// </summary>
        public bool tieneCondicionAsiento {
            get {
                return this.chbxCondicionAsiento.Checked;
            }
            set {
                chbxCondicionAsiento.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionCalcas
        /// </summary>
        public bool tieneCondicionCalcas {
            get {
                return this.chbxCondicionCalcas.Checked;
            }
            set {
                chbxCondicionCalcas.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionPintura
        /// </summary>
        public bool tieneCondicionPintura {
            get {
                return this.chbxCondicionPintura.Checked;
            }
            set {
                chbxCondicionPintura.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxEstructuraChasis
        /// </summary>
        public bool tieneEstructuraChasis {
            get {
                return this.chbxEstructuraChasis.Checked;
            }
            set {
                chbxEstructuraChasis.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxIndicadores
        /// </summary>
        public bool tieneIndicadores {
            get {
                return this.chbxIndicadores.Checked;
            }
            set {
                chbxIndicadores.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxJoystick
        /// </summary>
        public bool tieneJoystick {
            get {
                return this.chbxJoystick.Checked;
            }
            set {
                chbxJoystick.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLiquidoRefrigerante
        /// </summary>
        public bool tieneLiquidoRefrigerante {
            get {
                return this.chbxLiquidoRefrigerante.Checked;
            }
            set {
                chbxLiquidoRefrigerante.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesAdvertencia
        /// </summary>
        public bool tieneLucesAdvertencia {
            get {
                return this.chbxLucesAdvertencia.Checked;
            }
            set {
                chbxLucesAdvertencia.Checked = value;
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTapaCombustible
        /// </summary>
        public bool tieneTapaCombustible {
            get {
                return this.chbxTapaCombustible.Checked;
            }
            set {
                chbxTapaCombustible.Checked = value;
            }
        }


        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionRetroExcavadoraPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Coloca los controles en Modo de Edicion
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edicion</param>
        public void ModoEdicion(bool activo) {
            this.chbxAceiteDiferencialDel.Enabled = activo;
            this.chbxAceiteDiferencialTrasero.Enabled = activo;
            this.chbxAceiteHidraulico.Enabled = activo;
            this.chbxAceiteTransmision.Enabled = activo;
            this.chbxAceiteMotor.Enabled = activo;
            this.chbxAceitePlanetariosDel4x4.Enabled = activo;
            this.chbxAlarmaReversa.Enabled = activo;
            this.chbxAntenasMonitoreoSatelital.Enabled = activo;
            this.chbxAsientoOperador.Enabled = activo;
            this.chbxBandaVentilador.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxBloqueoDiferencial.Enabled = activo;
            this.chbxBoteDelantero.Enabled = activo;
            this.chbxBoteTrasero.Enabled = activo;
            this.chbxBujesPasadoresCargador.Enabled = activo;
            this.chbxBujesPasadoresRetro.Enabled = activo;
            this.chbxCartuchoFiltroAire.Enabled = activo;
            this.chbxCinturonSeguridad.Enabled = activo;
            this.chbxClaxon.Enabled = activo;
            this.chbxCombustible.Enabled = activo;
            this.chbxCondicionAsiento.Enabled = activo;
            this.chbxCondicionCalcas.Enabled = activo;
            this.chbxCondicionLlantas.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxDesacople4x4.Enabled = activo;
            this.chbxEspejoRetrovisor.Enabled = activo;
            this.chbxEstructuraChasis.Enabled = activo;
            this.chbxFrenoEstacionamiento.Enabled = activo;
            this.chbxIndicadores.Enabled = activo;
            this.chbxJoystick.Enabled = activo;
            this.chbxLiquidoRefrigerante.Enabled = activo;
            this.chbxLucesDireccionales.Enabled = activo;
            this.chbxLucesIntermitentes.Enabled = activo;
            this.chbxLucesTrabajoDelanteras.Enabled = activo;
            this.chbxLucesTrabajoTraseras.Enabled = activo;
            this.chbxManguerasAbrazaderasAdmisionAire.Enabled = activo;
            this.chbxNeutralizadorTransmision.Enabled = activo;
            this.chbxPalancaTransito.Enabled = activo;
            this.chbxPresionLlantas.Enabled = activo;
            this.chbxSimbolosSeguridadMaquina.Enabled = activo;
            this.chbxTacometro.Enabled = activo;
            this.chbxTapaCombustible.Enabled = activo;
            this.chbxTapaHidraulico.Enabled = activo;
            this.chbxVelocidadMaximaMotor.Enabled = activo;
            this.chbxVelocidadMinimaMotor.Enabled = activo;
            this.chbxZapatasEstabilizadores.Enabled = activo;
            this.chbxLucesAdvertencia.Enabled = activo;
        }
        /// <summary>
        /// Muestra mensaje en la interfaz del usuario
        /// </summary>
        /// <param name="mensaje">Mensaje que será mostrado</param>
        /// <param name="tipo">El tipo de mensaje que sera mostrado</param>
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