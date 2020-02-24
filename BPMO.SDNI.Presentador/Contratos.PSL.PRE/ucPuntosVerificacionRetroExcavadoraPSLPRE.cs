
using System;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionRetroExcavadoraPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionRetroExcavadoraPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionRetroExcavadoraPSLVIS vista;
        internal IucPuntosVerificacionRetroExcavadoraPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionRetroExcavadoraPSLPRE(IucPuntosVerificacionRetroExcavadoraPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionRetroExcavadoraPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.tieneAceiteDiferencialDel = false;
            this.vista.tieneAceiteDiferencialTrasero = false;
            this.vista.tieneAceiteHidraulico = false;
            this.vista.tieneAceiteMotor = false;
            this.vista.tieneAceitePlanetariosDel = false;
            this.vista.tieneAlarmaReversa = false;
            this.vista.tieneAntenaMonitoreo = false;
            this.vista.tieneAntenaMonitoreo = false;
            this.vista.tieneAsientoOperador = false;
            this.vista.tieneBandaVentilador = false;
            this.vista.tieneBateria = false;
            this.vista.tieneBloqueoDiferencial = false;
            this.vista.tieneBoteDelantero = false;
            this.vista.tieneBoteTrasero = false;
            this.vista.tieneBujesPasadoresCargador = false;
            this.vista.tieneBujesPasadoresRetro = false;
            this.vista.tieneCartuchoFiltroAire = false;
            this.vista.tieneCinturonSeguridad = false;
            this.vista.tieneClaxon = false;
            this.vista.tieneCombustible = false;
            this.vista.tieneCondicionAsiento = false;
            this.vista.tieneCondicionCalcas = false;
            this.vista.tieneCondicionLlantas = false;
            this.vista.tieneCondicionPintura = false;
            this.vista.tieneDesacople = false;
            this.vista.tieneEspejoRetrovisor = false;
            this.vista.tieneEstructuraChasis = false;
            this.vista.tieneFrenoEstacionamiento = false;
            this.vista.tieneIndicadores = false;
            this.vista.tieneJoystick = false;
            this.vista.tieneLiquidoRefrigerante = false;
            this.vista.tieneLucesDireccionales = false;
            this.vista.tieneLucesIntermitentes = false;
            this.vista.tieneLucesTrabajoDelantera = false;
            this.vista.tieneLucesTrabajoTraseras = false;
            this.vista.tieneManguerasAbrazaderas = false;
            this.vista.tieneNeutralizadorTransmision = false;
            this.vista.tienePalancaTransito = false;
            this.vista.tienePresionLLantas = false;
            this.vista.tieneSimbolosSeguridad = false;
            this.vista.tieneTacometro = false;
            this.vista.tieneTapaCombustible = false;
            this.vista.tieneTapaHidraulico = false;
            this.vista.tieneVelocidadMaxima = false;
            this.vista.tieneVelocidadMinima = false;
            this.vista.tieneZapatasEstabilizadores = false;

        }

        public void DatoToInterfazUsuario(ListadoVerificacionRetroExcavadoraBO bo) {
            if (bo == null)
                return;

            #region General (Campos booleanos)
            this.vista.tienePresionLLantas = bo.TienePresionLLantas;
            this.vista.tieneBandaVentilador = bo.TieneBandaVentilador;
            this.vista.tieneManguerasAbrazaderas = bo.TieneManguerasAbrazaderas;
            this.vista.tieneCartuchoFiltroAire = bo.TieneCartuchoFiltroAire;
            this.vista.tieneBoteDelantero = bo.TieneBoteDelantero;
            this.vista.tieneBoteTrasero = bo.TieneBoteTrasero;
            this.vista.tieneZapatasEstabilizadores = bo.TieneZapatasEstabilizadores;
            this.vista.tieneCinturonSeguridad = bo.TieneCinturonSeguridad;
            this.vista.tieneEspejoRetrovisor = bo.TieneEspejoRetrovisor;
            #endregion

            #region Niveles Flujos (Campos booleanos)
            this.vista.tieneCombustible = bo.TieneCombustible;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor;
            this.vista.tieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante;
            this.vista.tieneAceiteHidraulico = bo.TieneAceiteHidraulico;
            this.vista.tieneAceiteTransmision = bo.TieneAceiteTransmision;
            this.vista.tieneAceiteDiferencialTrasero = bo.TieneAceiteDiferencialTrasero;
            this.vista.tieneAceiteDiferencialDel = bo.TieneAceiteDiferencialDelantero;
            this.vista.tieneAceitePlanetariosDel = bo.TieneAceitePlanetariosDelantero;
            this.vista.tieneBateria = bo.TieneBateria;
            #endregion

            #region Lubricación (Campos booleanos)
            this.vista.tieneBujesPasadoresCargador = bo.TieneBujesPasadoresCargador;
            this.vista.tieneBujesPasadoresRetro = bo.TieneBujesPasadoresRetro;
            this.vista.tieneAsientoOperador = bo.TieneAsientoOperador;
            #endregion

            #region Funciones Electricias (Campos numericos)
            this.vista.tieneLucesTrabajoDelantera = bo.TieneLucesTrabajoDelantera;
            this.vista.tieneLucesTrabajoTraseras = bo.TieneLucesTrabajoTraseras;
            this.vista.tieneClaxon = bo.TieneClaxon;
            this.vista.tieneAlarmaReversa = bo.TieneAlarmaReversa;
            this.vista.tieneLucesIntermitentes = bo.TieneLucesIntermitentes;
            this.vista.tieneLucesDireccionales = bo.TieneLucesDireccionales;
            this.vista.tieneNeutralizadorTransmision = bo.TieneNeutralizadorTransmision;
            this.vista.tieneDesacople = bo.TieneDesacople;
            this.vista.tieneBloqueoDiferencial = bo.TieneBloqueoDiferencial;
            #endregion

            #region Controles (Campos booleanos)
            this.vista.tienePalancaTransito = bo.TienePalancaTransito;
            this.vista.tieneJoystick = bo.TieneJoystick;
            this.vista.tieneLucesAdvertencia = bo.TieneLucesAdvertencia;
            this.vista.tieneIndicadores = bo.TieneIndicadores;
            this.vista.tieneTacometro = bo.TieneTacometro;
            this.vista.tieneFrenoEstacionamiento = bo.TieneFrenoEstacionamiento;
            this.vista.tieneVelocidadMinima = bo.TieneVelocidadMinima;
            this.vista.tieneVelocidadMaxima = bo.TieneVelocidadMaxima;
            #endregion

            #region Miscelaneo (Campos numericos)
            this.vista.tieneTapaCombustible = bo.TieneTapaCombustible;
            this.vista.tieneTapaHidraulico = bo.TieneTapaHidraulico;
            this.vista.tieneCondicionAsiento = bo.TieneCondicionAsiento;
            this.vista.tieneCondicionLlantas = bo.TieneCondicionLlantas;
            this.vista.tieneCondicionPintura = bo.TieneCondicionPintura;
            this.vista.tieneCondicionCalcas = bo.TieneCondicionCalcas;
            this.vista.tieneSimbolosSeguridad = bo.TieneSimbolosSeguridad;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis;
            this.vista.tieneAntenaMonitoreo = bo.TieneAntenaMonitoreo;
            #endregion
        }

        public ListadoVerificacionRetroExcavadoraBO InterfazUsuarioADato() {
            ListadoVerificacionRetroExcavadoraBO bo = new ListadoVerificacionRetroExcavadoraBO();

            #region General (Campos booleanos)
            bo.TienePresionLLantas = this.vista.tienePresionLLantas;
            bo.TieneBandaVentilador = this.vista.tieneBandaVentilador;
            bo.TieneManguerasAbrazaderas = this.vista.tieneManguerasAbrazaderas;
            bo.TieneCartuchoFiltroAire = this.vista.tieneCartuchoFiltroAire;
            bo.TieneBoteDelantero = this.vista.tieneBoteDelantero;
            bo.TieneBoteTrasero = this.vista.tieneBoteTrasero;
            bo.TieneZapatasEstabilizadores = this.vista.tieneZapatasEstabilizadores;
            bo.TieneCinturonSeguridad = this.vista.tieneCinturonSeguridad;
            bo.TieneEspejoRetrovisor = this.vista.tieneEspejoRetrovisor;
            #endregion

            #region Niveles Flujos (Campos booleanos)
            bo.TieneCombustible = this.vista.tieneCombustible;
            bo.TieneAceiteMotor = this.vista.tieneAceiteMotor;
            bo.TieneLiquidoRefrigerante = this.vista.tieneLiquidoRefrigerante;
            bo.TieneAceiteHidraulico = this.vista.tieneAceiteHidraulico;
            bo.TieneAceiteTransmision = this.vista.tieneAceiteTransmision;
            bo.TieneAceiteDiferencialTrasero = this.vista.tieneAceiteDiferencialTrasero;
            bo.TieneAceiteDiferencialDelantero = this.vista.tieneAceiteDiferencialDel;
            bo.TieneAceitePlanetariosDelantero = this.vista.tieneAceitePlanetariosDel;
            bo.TieneBateria = this.vista.tieneBateria;
            #endregion

            #region Lubricación (Campos booleanos)
            bo.TieneBujesPasadoresCargador = this.vista.tieneBujesPasadoresCargador;
            bo.TieneBujesPasadoresRetro = this.vista.tieneBujesPasadoresRetro;
            bo.TieneAsientoOperador = this.vista.tieneAsientoOperador;
            #endregion

            #region Funciones Electricias (Campos numericos)
            bo.TieneLucesTrabajoDelantera = this.vista.tieneLucesTrabajoDelantera;
            bo.TieneLucesTrabajoTraseras = this.vista.tieneLucesTrabajoTraseras;
            bo.TieneClaxon = this.vista.tieneClaxon;
            bo.TieneAlarmaReversa = this.vista.tieneAlarmaReversa;
            bo.TieneLucesIntermitentes = this.vista.tieneLucesIntermitentes;
            bo.TieneLucesDireccionales = this.vista.tieneLucesDireccionales;
            bo.TieneNeutralizadorTransmision = this.vista.tieneNeutralizadorTransmision;
            bo.TieneDesacople = this.vista.tieneDesacople;
            bo.TieneBloqueoDiferencial = this.vista.tieneBloqueoDiferencial;
            #endregion

            #region Controles (Campos booleanos)
            bo.TienePalancaTransito = this.vista.tienePalancaTransito;
            bo.TieneJoystick = this.vista.tieneJoystick;
            bo.TieneLucesAdvertencia = this.vista.tieneLucesAdvertencia;
            bo.TieneIndicadores = this.vista.tieneIndicadores;
            bo.TieneTacometro = this.vista.tieneTacometro;
            bo.TieneFrenoEstacionamiento = this.vista.tieneFrenoEstacionamiento;
            bo.TieneVelocidadMinima = this.vista.tieneVelocidadMinima;
            bo.TieneVelocidadMaxima = this.vista.tieneVelocidadMaxima;
            #endregion

            #region Miscelaneo (Campos numericos)
            bo.TieneTapaCombustible = this.vista.tieneTapaCombustible;
            bo.TieneTapaHidraulico = this.vista.tieneTapaHidraulico;
            bo.TieneCondicionAsiento = this.vista.tieneCondicionAsiento;
            bo.TieneCondicionLlantas = this.vista.tieneCondicionLlantas;
            bo.TieneCondicionPintura = this.vista.tieneCondicionPintura;
            bo.TieneCondicionCalcas = this.vista.tieneCondicionCalcas;
            bo.TieneSimbolosSeguridad = this.vista.tieneSimbolosSeguridad;
            bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis;
            bo.TieneAntenaMonitoreo = this.vista.tieneAntenaMonitoreo;
            #endregion

            return bo;
        }



        public bool FaltanDatosObligatorios(out string datosFaltantes) {
            datosFaltantes = "";
            if (!this.vista.tienePresionLLantas)
                datosFaltantes += "Presión de llantas, ";
            if (!this.vista.tieneBandaVentilador)
                datosFaltantes += "Banda del ventilador, ";
            if (!this.vista.tieneManguerasAbrazaderas)
                datosFaltantes += "Mangueras y abrazaderas de admisión de aire, ";
            if (!this.vista.tieneCartuchoFiltroAire)
                datosFaltantes += "Cartucho del filtro de aire, ";
            if (!this.vista.tieneBoteDelantero)
                datosFaltantes += "Bote delantero, ";
            if (!this.vista.tieneBoteTrasero)
                datosFaltantes += "Bote trasero, ";
            if (!this.vista.tieneZapatasEstabilizadores)
                datosFaltantes += "Zapatas de los estabilizadores, ";
            if (!this.vista.tieneCinturonSeguridad)
                datosFaltantes += "Cinturón de seguridad, ";
            if (!this.vista.tieneEspejoRetrovisor)
                datosFaltantes += "Espejo retrovisor, ";
            if (!this.vista.tieneCombustible)
                datosFaltantes += "Combustible, ";
            if (!this.vista.tieneAceiteMotor)
                datosFaltantes += "Aceite del motor, ";
            if (!this.vista.tieneLiquidoRefrigerante)
                datosFaltantes += "Líquido refrigerante, ";
            if (!this.vista.tieneAceiteHidraulico)
                datosFaltantes += "Aceite hidráulico, ";
            if (!this.vista.tieneAceiteTransmision)
                datosFaltantes += "Aceite de transmisión, ";
            if (!this.vista.tieneAceiteDiferencialTrasero)
                datosFaltantes += "Aceite diferencial trasero, ";
            if (!this.vista.tieneAceiteDiferencialDel)
                datosFaltantes += "Aceite diferencial del. (4X4), ";
            if (!this.vista.tieneAceitePlanetariosDel)
                datosFaltantes += "Aceite planetarios del. (4X4), ";
            if (!this.vista.tieneBateria)
                datosFaltantes += "Batería, ";
            if (!this.vista.tieneBujesPasadoresCargador)
                datosFaltantes += "Bujes y pasadores del cargador, ";
            if (!this.vista.tieneBujesPasadoresRetro)
                datosFaltantes += "Bujes y pasadores de la retro, ";
            if (!this.vista.tieneAsientoOperador)
                datosFaltantes += "Asiento del operador, ";
            if (!this.vista.tieneLucesTrabajoDelantera)
                datosFaltantes += "Luces de trabajo delantera, ";
            if (!this.vista.tieneLucesTrabajoTraseras)
                datosFaltantes += "Luces de trabajo traseras, ";
            if (!this.vista.tieneClaxon)
                datosFaltantes += "Claxon, ";
            if (!this.vista.tieneAlarmaReversa)
                datosFaltantes += "Alarma de reversa, ";
            if (!this.vista.tieneLucesIntermitentes)
                datosFaltantes += "Luces intermitentes, ";
            if (!this.vista.tieneLucesDireccionales)
                datosFaltantes += "Luces de direccionales, ";
            if (!this.vista.tieneNeutralizadorTransmision)
                datosFaltantes += "Neutralizador de transmisión, ";
            if (!this.vista.tieneDesacople)
                datosFaltantes += "Desacople 4X4, ";
            if (!this.vista.tieneBloqueoDiferencial)
                datosFaltantes += "Bloqueo del diferencial, ";
            if (!this.vista.tienePalancaTransito)
                datosFaltantes += "Palanca de tránsito, ";
            if (!this.vista.tieneJoystick)
                datosFaltantes += "Joystick, ";
            if (!this.vista.tieneLucesAdvertencia)
                datosFaltantes += "Luces de advertencia, ";
            if (!this.vista.tieneIndicadores)
                datosFaltantes += "Indicadores, ";
            if (!this.vista.tieneTacometro)
                datosFaltantes += "Tacómetro, ";
            if (!this.vista.tieneFrenoEstacionamiento)
                datosFaltantes += "Freno de estacionamiento, ";
            if (!this.vista.tieneVelocidadMinima)
                datosFaltantes += "Velocidad mínima del motor, ";
            if (!this.vista.tieneVelocidadMaxima)
                datosFaltantes += "Velocidad máxima del motor, ";
            if (!this.vista.tieneTapaCombustible)
                datosFaltantes += "Tapa de combustible, ";
            if (!this.vista.tieneTapaHidraulico)
                datosFaltantes += "Tapa del hidráulico, ";
            if (!this.vista.tieneCondicionAsiento)
                datosFaltantes += "Condición del asiento, ";
            if (!this.vista.tieneCondicionLlantas)
                datosFaltantes += "Condición de llantas, ";
            if (!this.vista.tieneCondicionPintura)
                datosFaltantes += "Condición de pintura, ";
            if (!this.vista.tieneCondicionCalcas)
                datosFaltantes += "Condición de calcas, ";
            if (!this.vista.tieneSimbolosSeguridad)
                datosFaltantes += "Símbolos de seguridad de la máquina, ";
            if (!this.vista.tieneEstructuraChasis)
                datosFaltantes += "Estructura/chasis, ";
            if (!this.vista.tieneAntenaMonitoreo)
                datosFaltantes += "Antenas monitoreo satelital, ";

            if (datosFaltantes.Length > 0) {
                datosFaltantes = datosFaltantes.Substring(0, datosFaltantes.Length - 2);
                return true;
            }
            return false;
        }

        #endregion
    }
}