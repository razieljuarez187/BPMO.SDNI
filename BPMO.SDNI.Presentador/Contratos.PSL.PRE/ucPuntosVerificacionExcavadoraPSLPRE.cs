using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionExcavadoraPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionExcavadoraPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionExcavadoraPSLVIS vista;
        internal IucPuntosVerificacionExcavadoraPSLVIS Vista { get { return vista; } }

        /// <summary>
        /// El DataContext que provee acceso a la Base de Datos
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Controlador de Punto de verificación de excavadora
        /// </summary>
        //private TarifaPSLBR tarifaBr;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del Presentador
        /// </summary>
        /// <param name="vistaActual">Vista a la cual estará asociado el Presentador</param>
        public ucPuntosVerificacionExcavadoraPSLPRE(IucPuntosVerificacionExcavadoraPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionExcavadoraPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionExcavadoraBO bo = (ListadoVerificacionExcavadoraBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.tieneAceiteHidraulico = null;
            this.vista.tieneAceiteMotor = null;
            this.vista.tieneAireAcondicionado = null;
            this.vista.tieneAlarmaReversa = null;
            this.vista.tieneAutoaceleracion = null;
            this.vista.tieneBalancinBote = null;
            this.vista.tieneBateria = null;
            this.vista.tieneBisagrasCofreMotor = null;
            this.vista.tieneBrazoPluma = null;
            this.vista.tieneCombustible = null;

            this.vista.tieneCristalesCabina = null;
            this.vista.tieneEspejosRetrovisores = null;
            this.vista.tieneHorometro = null;
            this.vista.tieneIndicadores = null;
            this.vista.tieneInterruptorDesconexion = null;
            this.vista.tieneJoystick = null;
            this.vista.tieneLamparasTablero = null;
            this.vista.tieneLimpiaparabrisas = null;
            this.vista.tieneLiquidoRefrigerante = null;
            this.vista.tieneLucesAdvertencia = null;

            this.vista.tieneLucesTrabajo = null;
            this.vista.tienePalancaBloqueoPilotaje = null;
            this.vista.tienePuertasCerraduras = null;
            this.vista.tieneReductorEngranesTransito = null;
            this.vista.tieneReductorSwing = null;
            this.vista.tieneRodillosTransito = null;
            this.vista.tienesContrapeso = null;
            this.vista.tieneTensionCadena = null;
            this.vista.tieneVastagosGatos = null;
            this.vista.tieneVelocidadMinimaMotor = null;
            this.vista.tieneVelocidadMaximaMotor = null;
            this.vista.tieneZapatas = null;
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();
            #region 1 - General
            if (!vista.tieneZapatas.Value)
                s.Append("Zapatas, ");
            if (!this.vista.tieneBrazoPluma.Value)
                s.Append("Brazo y Pluma, ");
            if (!this.vista.tienesContrapeso.Value)
                s.Append("Contrapeso, ");
            if (!this.vista.tieneVastagosGatos.Value)
                s.Append("Vástagos de Gatos, ");
            if (!this.vista.tieneTensionCadena.Value)
                s.Append("Tensión de Cadena, ");
            if (!this.vista.tieneRodillosTransito.Value)
                s.Append("Rodillos de Tránsito, ");
            if (!this.vista.tieneEspejosRetrovisores.Value)
                s.Append("Espejos Retrovisores, ");
            if (!this.vista.tieneCristalesCabina.Value)
                s.Append("Cristales de Cabina, ");
            if (!this.vista.tienePuertasCerraduras.Value)
                s.Append("Puertas y Cerraduras, ");
            if (!this.vista.tieneBisagrasCofreMotor.Value)
                s.Append("Bisagras Cofre del Motor, ");
            if (!this.vista.tieneBalancinBote.Value)
                s.Append("Balancín del Bote (H), ");
            #endregion
            #region 2 - Niveles de Fluidos
            if (!this.vista.tieneCombustible.Value)
                s.Append("Combustible, ");
            if (!this.vista.tieneAceiteMotor.Value)
                s.Append("Aceite del Motor, ");
            if (!this.vista.tieneAceiteHidraulico.Value)
                s.Append("Aceite Hidráulico, ");
            if (!this.vista.tieneLiquidoRefrigerante.Value)
                s.Append("Líquido Refrigerante, ");
            if (!this.vista.tieneReductorEngranesTransito.Value)
                s.Append("Reductor de Engranes de Tránsito, ");
            if (!this.vista.tieneReductorSwing.Value)
                s.Append("Reductor del Swing, ");
            if (!this.vista.tieneBateria.Value)
                s.Append("Batería, ");
            #endregion
            #region 3 - Lubricación
            if (!this.vista.tienePasadoresBoom.Value)
                s.Append("Pasadores Boom, ");
            if (!this.vista.tienePasadoresBrazo.Value)
                s.Append("Pasadores Brazo, ");
            if (!this.vista.tienePasadoresBote.Value)
                s.Append("Pasadores Bote, ");
            if (!this.vista.tieneTornameza.Value)
                s.Append("Tornamesa, ");
            if (!this.vista.tieneCentralEngrase.Value)
                s.Append("Central de Engrase, ");
            #endregion
            #region 4 - Funciones Eléctricas
            if (!this.vista.tieneLucesTrabajo.Value)
                s.Append("Luces de Trabajo, ");
            if (!this.vista.tieneLamparasTablero.Value)
                s.Append("Lámparas del Tablero, ");
            if (!this.vista.tieneInterruptorDesconexion.Value)
                s.Append("Interrumptor de Desconexión, ");
            if (!this.vista.tieneAlarmaReversa.Value)
                s.Append("Alarma de Reversa, ");
            if (!this.vista.tieneHorometro.Value)
                s.Append("Horómetro, ");
            if (!this.vista.tieneLimpiaparabrisas.Value)
                s.Append("Limpiaparabrisas, ");
            #endregion
            #region 5 - Controles
            if (!this.vista.tieneJoystick.Value)
                s.Append("Joysticks, ");
            if (!this.vista.tieneLucesAdvertencia.Value)
                s.Append("Luces de Advertencia, ");
            if (!this.vista.tieneIndicadores.Value)
                s.Append("Indicadores, ");
            if (!this.vista.tienePalancaBloqueoPilotaje.Value)
                s.Append("Palanca Bloqueo Pilotaje, ");
            if (!this.vista.tieneAireAcondicionado.Value)
                s.Append("Aire Acondicionado, ");
            if (!this.vista.tieneAutoaceleracion.Value)
                s.Append("Autoaceleración, ");
            if (!this.vista.tieneVelocidadMinimaMotor.Value)
                s.Append("Velocidad Mínima del Motor, ");
            if (!this.vista.tieneVelocidadMaximaMotor.Value)
                s.Append("Velocidad Máxima del Motor, ");
            #endregion
            #region 6 - Misceláneos
            if (!this.vista.tieneTapaCombustible.Value)
                s.Append("Tapa de Combustible, ");
            if (!this.vista.tieneCondicionAsiento.Value)
                s.Append("Condición del Asiento, ");
            if (!this.vista.tieneCondicionPintura.Value)
                s.Append("Condición de Pintura, ");
            if (!this.vista.tieneCondicionCalcas.Value)
                s.Append("Condición de Calcas, ");
            if (!this.vista.tieneSimboloSeguridadMaquina.Value)
                s.Append("Símbolos de Seguridad de la Máquina, ");
            if (!this.vista.tieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis, ");
            if (!this.vista.tieneAntenasMonitoreoSatelital.Value)
                s.Append("Antenas Monitoreo Satelital, ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        #endregion

        public object InterfazUsuarioADato() {
            ListadoVerificacionExcavadoraBO bo = new ListadoVerificacionExcavadoraBO();

            if (this.vista.tieneZapatas.HasValue)
                bo.TieneZapatas = this.vista.tieneZapatas.Value;
            if (this.vista.tieneBrazoPluma.HasValue)
                bo.TieneBrazoPluma = this.vista.tieneBrazoPluma.Value;
            if (this.vista.tienesContrapeso.HasValue)
                bo.TieneContrapeso = this.vista.tienesContrapeso.Value;
            if (this.vista.tieneVastagosGatos.HasValue)
                bo.TieneVastagosGatos = this.vista.tieneVastagosGatos.Value;
            if (this.vista.tieneTensionCadena.HasValue)
                bo.TieneTensionCadena = this.vista.tieneTensionCadena.Value;
            if (this.vista.tieneRodillosTransito.HasValue)
                bo.TieneRodillosTransito = this.vista.tieneRodillosTransito.Value;
            if (this.vista.tieneEspejosRetrovisores.HasValue)
                bo.TieneEspejosRetrovisores = this.vista.tieneEspejosRetrovisores.Value;
            if (this.vista.tieneCristalesCabina.HasValue)
                bo.TieneCristalesCabina = this.vista.tieneCristalesCabina.Value;
            if (this.vista.tienePuertasCerraduras.HasValue)
                bo.TienePuertasCerraduras = this.vista.tienePuertasCerraduras.Value;
            if (this.vista.tieneBisagrasCofreMotor.HasValue)
                bo.TieneBisagrasCofreMotor = this.vista.tieneBisagrasCofreMotor.Value;
            if (this.vista.tieneBalancinBote.HasValue)
                bo.TieneBalancinBote = this.vista.tieneBalancinBote.Value;
            if (this.vista.tieneLucesTrabajo.HasValue)
                bo.TieneLucesTrabajo = this.vista.tieneLucesTrabajo.Value;
            if (this.vista.tieneLamparasTablero.HasValue)
                bo.TieneLamparasTablero = this.vista.tieneLamparasTablero.Value;
            if (this.vista.tieneInterruptorDesconexion.HasValue)
                bo.TieneInterruptorDesconexion = this.vista.tieneInterruptorDesconexion.Value;
            if (this.vista.tieneAlarmaReversa.HasValue)
                bo.TieneAlarmaReversa = this.vista.tieneAlarmaReversa.Value;
            if (this.vista.tieneHorometro.HasValue)
                bo.TieneHorometro = this.vista.tieneHorometro.Value;
            if (this.vista.tieneLimpiaparabrisas.HasValue)
                bo.TieneLimpiaparabrisas = this.vista.tieneLimpiaparabrisas.Value;
            if (this.vista.tieneCombustible.HasValue)
                bo.TieneCombustible = this.vista.tieneCombustible.Value;
            if (this.vista.tieneAceiteMotor.HasValue)
                bo.TieneAceiteMotor = this.vista.tieneAceiteMotor.Value;
            if (this.vista.tieneAceiteHidraulico.HasValue)
                bo.TieneAceiteHidraulico = this.vista.tieneAceiteHidraulico.Value;
            if (this.vista.tieneLiquidoRefrigerante.HasValue)
                bo.TieneLiquidoRefrigerante = this.vista.tieneLiquidoRefrigerante.Value;
            if (this.vista.tieneReductorEngranesTransito.HasValue)
                bo.TieneReductorEngranesTransito = this.vista.tieneReductorEngranesTransito.Value;
            if (this.vista.tieneReductorSwing.HasValue)
                bo.TieneReductorSwing = this.vista.tieneReductorSwing.Value;
            if (this.vista.tieneBateria.HasValue)
                bo.TieneBateria = this.vista.tieneBateria.Value;
            if (this.vista.tieneJoystick.HasValue)
                bo.TieneJoystick = this.vista.tieneJoystick.Value;
            if (this.vista.tieneLucesAdvertencia.HasValue)
                bo.TieneLucesAdvertencia = this.vista.tieneLucesAdvertencia.Value;
            if (this.vista.tieneIndicadores.HasValue)
                bo.TieneIndicadores = this.vista.tieneIndicadores.Value;
            if (this.vista.tienePalancaBloqueoPilotaje.HasValue)
                bo.TienePalancaBloqueoPilotaje = this.vista.tienePalancaBloqueoPilotaje.Value;
            if (this.vista.tieneAireAcondicionado.HasValue)
                bo.TieneAireAcondicionado = this.vista.tieneAireAcondicionado.Value;
            if (this.vista.tieneAutoaceleracion.HasValue)
                bo.TieneAutoaceleracion = this.vista.tieneAutoaceleracion.Value;
            if (this.vista.tieneVelocidadMinimaMotor.HasValue)
                bo.TieneVelocidadMinimaMotor = this.vista.tieneVelocidadMinimaMotor.Value;
            if (this.vista.tieneVelocidadMaximaMotor.HasValue)
                bo.TieneVelocidadMaximaMotor = this.vista.tieneVelocidadMaximaMotor.Value;
            if (this.vista.tienePasadoresBoom.HasValue)
                bo.TienePasadoresBoom = this.vista.tienePasadoresBoom.Value;
            if (this.vista.tienePasadoresBrazo.HasValue)
                bo.TienePasadoresBrazo = this.vista.tienePasadoresBrazo.Value;
            if (this.vista.tienePasadoresBote.HasValue)
                bo.TienePasadoresBote = this.vista.tienePasadoresBote.Value;
            if (this.vista.tieneTornameza.HasValue)
                bo.TieneTornameza = this.vista.tieneTornameza.Value;
            if (this.vista.tieneCentralEngrase.HasValue)
                bo.TieneCentralEngrase = this.vista.tieneCentralEngrase.Value;
            if (this.vista.tieneTapaCombustible.HasValue)
                bo.TieneTapaCombustible = this.vista.tieneTapaCombustible.Value;
            if (this.vista.tieneCondicionAsiento.HasValue)
                bo.TieneCondicionAsiento = this.vista.tieneCondicionAsiento.Value;
            if (this.vista.tieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.tieneCondicionPintura.Value;
            if (this.vista.tieneCondicionCalcas.HasValue)
                bo.TieneCondicionCalcas = this.vista.tieneCondicionCalcas.Value;
            if (this.vista.tieneSimboloSeguridadMaquina.HasValue)
                bo.TieneSimboloSeguridadMaquina = this.vista.tieneSimboloSeguridadMaquina.Value;
            if (this.vista.tieneEstructuraChasis.HasValue)
                bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis.Value;
            if (this.vista.tieneAntenasMonitoreoSatelital.HasValue)
                bo.TieneAntenasMonitoreoSatelital = this.vista.tieneAntenasMonitoreoSatelital.Value;

            return bo;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionExcavadoraBO bo) {
            this.vista.tieneZapatas = bo.TieneZapatas.HasValue ? bo.TieneZapatas : null;
            this.vista.tieneBrazoPluma = bo.TieneBrazoPluma.HasValue ? bo.TieneBrazoPluma : null;
            this.vista.tienesContrapeso = bo.TieneContrapeso.HasValue ? bo.TieneContrapeso : null;
            this.vista.tieneVastagosGatos = bo.TieneVastagosGatos.HasValue ? bo.TieneVastagosGatos : null;
            this.vista.tieneTensionCadena = bo.TieneTensionCadena.HasValue ? bo.TieneTensionCadena : null;
            this.vista.tieneRodillosTransito = bo.TieneRodillosTransito.HasValue ? bo.TieneRodillosTransito : null;
            this.vista.tieneEspejosRetrovisores = bo.TieneEspejosRetrovisores.HasValue ? bo.TieneEspejosRetrovisores : null;
            this.vista.tieneCristalesCabina = bo.TieneCristalesCabina.HasValue ? bo.TieneCristalesCabina : null;
            this.vista.tienePuertasCerraduras = bo.TienePuertasCerraduras.HasValue ? bo.TienePuertasCerraduras : null;
            this.vista.tieneBisagrasCofreMotor = bo.TieneBisagrasCofreMotor.HasValue ? bo.TieneBisagrasCofreMotor : null;
            this.vista.tieneBalancinBote = bo.TieneBalancinBote.HasValue ? bo.TieneBalancinBote : null;
            this.vista.tieneLucesTrabajo = bo.TieneLucesTrabajo.HasValue ? bo.TieneLucesTrabajo : null;
            this.vista.tieneLamparasTablero = bo.TieneLamparasTablero.HasValue ? bo.TieneLamparasTablero : null;
            this.vista.tieneInterruptorDesconexion = bo.TieneInterruptorDesconexion.HasValue ? bo.TieneInterruptorDesconexion : null;
            this.vista.tieneAlarmaReversa = bo.TieneAlarmaReversa.HasValue ? bo.TieneAlarmaReversa : null;
            this.vista.tieneHorometro = bo.TieneHorometro.HasValue ? bo.TieneHorometro : null;
            this.vista.tieneLimpiaparabrisas = bo.TieneLimpiaparabrisas.HasValue ? bo.TieneLimpiaparabrisas : null;
            this.vista.tieneCombustible = bo.TieneCombustible.HasValue ? bo.TieneCombustible : null;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor.HasValue ? bo.TieneAceiteMotor : null;
            this.vista.tieneAceiteHidraulico = bo.TieneAceiteHidraulico.HasValue ? bo.TieneAceiteHidraulico : null;
            this.vista.tieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante.HasValue ? bo.TieneLiquidoRefrigerante : null;
            this.vista.tieneReductorEngranesTransito = bo.TieneReductorEngranesTransito.HasValue ? bo.TieneReductorEngranesTransito : null;
            this.vista.tieneReductorSwing = bo.TieneReductorSwing.HasValue ? bo.TieneReductorSwing : null;
            this.vista.tieneBateria = bo.TieneBateria.HasValue ? bo.TieneBateria : null;
            this.vista.tieneJoystick = bo.TieneJoystick.HasValue ? bo.TieneJoystick : null;
            this.vista.tieneLucesAdvertencia = bo.TieneLucesAdvertencia.HasValue ? bo.TieneLucesAdvertencia : null;
            this.vista.tieneIndicadores = bo.TieneIndicadores.HasValue ? bo.TieneIndicadores : null;
            this.vista.tienePalancaBloqueoPilotaje = bo.TienePalancaBloqueoPilotaje.HasValue ? bo.TienePalancaBloqueoPilotaje : null;
            this.vista.tieneAireAcondicionado = bo.TieneAireAcondicionado.HasValue ? bo.TieneAireAcondicionado : null;
            this.vista.tieneAutoaceleracion = bo.TieneAutoaceleracion.HasValue ? bo.TieneAutoaceleracion : null;
            this.vista.tieneVelocidadMinimaMotor = bo.TieneVelocidadMinimaMotor.HasValue ? bo.TieneVelocidadMinimaMotor : null;
            this.vista.tieneVelocidadMaximaMotor = bo.TieneVelocidadMaximaMotor.HasValue ? bo.TieneVelocidadMaximaMotor : null;
            this.vista.tienePasadoresBoom = bo.TienePasadoresBoom.HasValue ? bo.TienePasadoresBoom : null;
            this.vista.tienePasadoresBrazo = bo.TienePasadoresBrazo.HasValue ? bo.TienePasadoresBrazo : null;
            this.vista.tienePasadoresBote = bo.TienePasadoresBote.HasValue ? bo.TienePasadoresBote : null;
            this.vista.tieneTornameza = bo.TieneTornameza.HasValue ? bo.TieneTornameza : null;
            this.vista.tieneCentralEngrase = bo.TieneCentralEngrase.HasValue ? bo.TieneCentralEngrase : null;
            this.vista.tieneTapaCombustible = bo.TieneTapaCombustible.HasValue ? bo.TieneTapaCombustible : null;
            this.vista.tieneCondicionAsiento = bo.TieneCondicionAsiento.HasValue ? bo.TieneCondicionAsiento : null;
            this.vista.tieneCondicionPintura = bo.TieneCondicionPintura.HasValue ? bo.TieneCondicionPintura : null;
            this.vista.tieneCondicionCalcas = bo.TieneCondicionCalcas.HasValue ? bo.TieneCondicionCalcas : null;
            this.vista.tieneSimboloSeguridadMaquina = bo.TieneSimboloSeguridadMaquina.HasValue ? bo.TieneSimboloSeguridadMaquina : null;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis.HasValue ? bo.TieneEstructuraChasis : null;
            this.vista.tieneAntenasMonitoreoSatelital = bo.TieneAntenasMonitoreoSatelital.HasValue ? bo.TieneAntenasMonitoreoSatelital : null;
        }
    }
}