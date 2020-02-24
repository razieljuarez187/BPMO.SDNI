using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionExcavadoraPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionExcavadoraPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionExcavadoraPSLUI";
        private ucPuntosVerificacionExcavadoraPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxZapatas
        /// </summary>
        public bool? tieneZapatas {
            get {
                bool val;
                return Boolean.TryParse(this.chbxZapatas.Checked.ToString(), out val) ? (bool?)val : null;
                //return this.chbxZapatas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxZapatas.Checked = true;
                    else
                        chbxZapatas.Checked = false;

                }
            }

        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBrazoPluma
        /// </summary>
        public bool? tieneBrazoPluma {
            get {
                return this.chbxBrazoPluma.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBrazoPluma.Checked = true;
                    else
                        chbxBrazoPluma.Checked = false;

                }
            }

        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxContrapeso
        /// </summary>
        public bool? tienesContrapeso {
            get {

                return this.chbxContrapeso.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxContrapeso.Checked = true;
                    else
                        chbxContrapeso.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVastagosGatos
        /// </summary>
        public bool? tieneVastagosGatos {
            get {

                return this.chbxVastagosGatos.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxVastagosGatos.Checked = true;
                    else
                        chbxVastagosGatos.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTensionCadena
        /// </summary>
        public bool? tieneTensionCadena {
            get {

                return this.chbxTensionCadena.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTensionCadena.Checked = true;
                    else
                        chbxTensionCadena.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxRodillosTransito
        /// </summary>
        public bool? tieneRodillosTransito {
            get {

                return this.chbxRodillosTransito.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxRodillosTransito.Checked = true;
                    else
                        chbxRodillosTransito.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxEspejosRetrovisores
        /// </summary>
        public bool? tieneEspejosRetrovisores {
            get {

                return this.chbxEspejosRetrovisores.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxEspejosRetrovisores.Checked = true;
                    else
                        chbxEspejosRetrovisores.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCristalesCabina
        /// </summary>
        public bool? tieneCristalesCabina {
            get {

                return this.chbxCristalesCabina.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCristalesCabina.Checked = true;
                    else
                        chbxCristalesCabina.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPuertasCerraduras
        /// </summary>
        public bool? tienePuertasCerraduras {
            get {

                return this.chbxPuertasCerraduras.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPuertasCerraduras.Checked = true;
                    else
                        chbxPuertasCerraduras.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBisagrasCofreMotor
        /// </summary>
        public bool? tieneBisagrasCofreMotor {
            get {

                return this.chbxBisagrasCofreMotor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBisagrasCofreMotor.Checked = true;
                    else
                        chbxBisagrasCofreMotor.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBalancinBote
        /// </summary>
        public bool? tieneBalancinBote {
            get {

                return this.chbxBalancinBote.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBalancinBote.Checked = true;
                    else
                        chbxBalancinBote.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajo
        /// </summary>
        public bool? tieneLucesTrabajo {
            get {

                return this.chbxLucesTrabajo.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesTrabajo.Checked = true;
                    else
                        chbxLucesTrabajo.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxHorometro
        /// </summary>
        public bool? tieneHorometro {
            get {

                return this.chbxHorometro.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxHorometro.Checked = true;
                    else
                        chbxHorometro.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLimpiaparabrisas
        /// </summary>
        public bool? tieneLimpiaparabrisas {
            get {

                return this.chbxLimpiaparabrisas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLimpiaparabrisas.Checked = true;
                    else
                        chbxLimpiaparabrisas.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxReductorEngranesTransito
        /// </summary>
        public bool? tieneReductorEngranesTransito {
            get {

                return this.chbxReductorEngranesTransito.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxReductorEngranesTransito.Checked = true;
                    else
                        chbxReductorEngranesTransito.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxReductorSwing
        /// </summary>
        public bool? tieneReductorSwing {
            get {

                return this.chbxReductorSwing.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxReductorSwing.Checked = true;
                    else
                        chbxReductorSwing.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxJoystick
        /// </summary>
        public bool? tieneJoystick {
            get {

                return this.chbxJoystick.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxJoystick.Checked = true;
                    else
                        chbxJoystick.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxPalancaBloqueoPilotaje
        /// </summary>
        public bool? tienePalancaBloqueoPilotaje {
            get {

                return this.chbxPalancaBloqueoPilotaje.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPalancaBloqueoPilotaje.Checked = true;
                    else
                        chbxPalancaBloqueoPilotaje.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAireAcondicionado
        /// </summary>
        public bool? tieneAireAcondicionado {
            get {

                return this.chbxAireAcondicionado.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAireAcondicionado.Checked = true;
                    else
                        chbxAireAcondicionado.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAutoaceleracion
        /// </summary>
        public bool? tieneAutoaceleracion {
            get {

                return this.chbxAutoaceleracion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAutoaceleracion.Checked = true;
                    else
                        chbxAutoaceleracion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMinimaMotor
        /// </summary>
        public bool? tieneVelocidadMinimaMotor {
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
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMaximaMotor
        /// </summary>
        public bool? tieneVelocidadMaximaMotor {
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
        ///  Asigna u obtiene el valor del checkbox chbxAntenasMonitoreoSatelital
        /// </summary>
        public bool? tieneAntenasMonitoreoSatelital {
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
        /// Asigna u obtiene el valor del checkbox chbxCentralEngrase
        /// </summary>
        public bool? tieneCentralEngrase {
            get {

                return this.chbxCentralEngrase.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCentralEngrase.Checked = true;
                    else
                        chbxCentralEngrase.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxPasadoresBoom
        /// </summary>
        public bool? tienePasadoresBoom {
            get {

                return this.chbxPasadoresBoom.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPasadoresBoom.Checked = true;
                    else
                        chbxPasadoresBoom.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPasadoresBote
        /// </summary>
        public bool? tienePasadoresBote {
            get {

                return this.chbxPasadoresBote.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPasadoresBote.Checked = true;
                    else
                        chbxPasadoresBote.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPasadoresBrazo
        /// </summary>
        public bool? tienePasadoresBrazo {
            get {

                return this.chbxPasadoresBrazo.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPasadoresBrazo.Checked = true;
                    else
                        chbxPasadoresBrazo.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxSimboloSeguridadMaquina
        /// </summary>
        public bool? tieneSimboloSeguridadMaquina {
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
        /// Asigna u obtiene el valor del checkbox chbxTornameza
        /// </summary>
        public bool? tieneTornameza {
            get {

                return this.chbxTornamesa.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTornamesa.Checked = true;
                    else
                        chbxTornamesa.Checked = false;

                }
            }
        }


        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionExcavadoraPSLPRE(this);
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
            this.chbxAireAcondicionado.Enabled = activo;
            this.chbxAlarmaReversa.Enabled = activo;
            this.chbxAntenasMonitoreoSatelital.Enabled = activo;
            this.chbxAutoaceleracion.Enabled = activo;
            this.chbxBalancinBote.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxBisagrasCofreMotor.Enabled = activo;
            this.chbxBrazoPluma.Enabled = activo;
            this.chbxCentralEngrase.Enabled = activo;
            this.chbxCombustible.Enabled = activo;
            this.chbxCondicionAsiento.Enabled = activo;
            this.chbxCondicionCalcas.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxContrapeso.Enabled = activo;
            this.chbxCristalesCabina.Enabled = activo;
            this.chbxEspejosRetrovisores.Enabled = activo;
            this.chbxEstructuraChasis.Enabled = activo;
            this.chbxHorometro.Enabled = activo;
            this.chbxIndicadores.Enabled = activo;
            this.chbxInterruptorDesconexion.Enabled = activo;
            this.chbxJoystick.Enabled = activo;
            this.chbxLamparasTablero.Enabled = activo;
            this.chbxLimpiaparabrisas.Enabled = activo;
            this.chbxLiquidoRefrigerante.Enabled = activo;
            this.chbxLucesAdvertencia.Enabled = activo;
            this.chbxLucesTrabajo.Enabled = activo;
            this.chbxPalancaBloqueoPilotaje.Enabled = activo;
            this.chbxPasadoresBoom.Enabled = activo;
            this.chbxPasadoresBote.Enabled = activo;
            this.chbxPasadoresBrazo.Enabled = activo;
            this.chbxPuertasCerraduras.Enabled = activo;
            this.chbxReductorEngranesTransito.Enabled = activo;
            this.chbxReductorSwing.Enabled = activo;
            this.chbxRodillosTransito.Enabled = activo;
            this.chbxReductorSwing.Enabled = activo;
            this.chbxRodillosTransito.Enabled = activo;
            this.chbxSimbolosSeguridadMaquina.Enabled = activo;
            this.chbxTapaCombustible.Enabled = activo;
            this.chbxTensionCadena.Enabled = activo;
            this.chbxTornamesa.Enabled = activo;
            this.chbxVastagosGatos.Enabled = activo;
            this.chbxVelocidadMaximaMotor.Enabled = activo;
            this.chbxVelocidadMinimaMotor.Enabled = activo;
            this.chbxZapatas.Enabled = activo;
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