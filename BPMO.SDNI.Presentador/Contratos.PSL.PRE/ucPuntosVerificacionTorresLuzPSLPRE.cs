

using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;


namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionTorresLuzPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionTorresLuzPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionTorresLuzPSLVIS vista;
        internal IucPuntosVerificacionTorresLuzPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionTorresLuzPSLPRE(IucPuntosVerificacionTorresLuzPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionTorresLuzPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionTorresLuzBO bo = (ListadoVerificacionTorresLuzBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {

            #region General
            this.vista.tienePresionLlantas = null;
            this.vista.tieneBandaVentilador = null;
            this.vista.tieneManguerasAbrazaderas = null;
            this.vista.tieneCartuchoFiltroAire = null;
            this.vista.tieneCableLevanteTorre = null;
            #endregion
            #region Niveles Fluidos
            this.vista.tieneCombustible = null;
            this.vista.tieneAceiteMotor = null;
            this.vista.tieneLiquidoRefrigerante = null;
            this.vista.tieneBateria = null;
            #endregion
            #region Funciones Electricas
            this.vista.tieneLucesTorre = null;
            this.vista.tieneLamparasTablero = null;
            #endregion
            #region Controles
            this.vista.tieneLucesAdvertencia = null;
            this.vista.tieneSwitchEncendido = null;
            this.vista.tieneVelocidadMinima = null;
            this.vista.tieneVelocidadMaxima = null;
            #endregion
            #region Miscelaneos
            this.vista.tieneTapaCombustible = null;
            this.vista.tieneCondicionLLantas = null;
            this.vista.tieneCondicionPintura = null;
            this.vista.tieneCondicionCalcas = null;
            this.vista.tieneSimbolosSeguridad = null;
            this.vista.tieneEstructuraChasis = null;
            this.vista.tieneAntenasMonitoreo = null;

            #endregion

        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();
            #region General
            if (!vista.tienePresionLlantas.Value)
                s.Append("Presión de llantas, ");
            if (!this.vista.tieneBandaVentilador.Value)
                s.Append("Banda del Ventilador, ");
            if (!this.vista.tieneManguerasAbrazaderas.Value)
                s.Append("Mangueras y Abrazaderas de Admisión de Aire, ");
            if (!this.vista.tieneCartuchoFiltroAire.Value)
                s.Append("Cartucho del Filtro de Aire, ");
            if (!this.vista.tieneCableLevanteTorre.Value)
                s.Append("Cable de Levante de la Torre, ");
            #endregion
            #region Niveles de fluidos
            if (!vista.tieneCombustible.Value)
                s.Append("Combustible, ");
            if (!this.vista.tieneAceiteMotor.Value)
                s.Append("Aceite del Motor, ");
            if (!this.vista.tieneLiquidoRefrigerante.Value)
                s.Append("Líquido Refrigerante, ");
            if (!this.vista.tieneBateria.Value)
                s.Append("Batería, ");
            #endregion
            #region Funciones electricas
            if (!this.vista.tieneLucesTorre.Value)
                s.Append("Luces de la Torre, ");
            if (!this.vista.tieneLamparasTablero.Value)
                s.Append("Lámparas del Tablero, ");
            #endregion
            #region Controles
            if (!vista.tieneLucesAdvertencia.Value)
                s.Append("Luces de Advertencia, ");
            if (!this.vista.tieneSwitchEncendido.Value)
                s.Append("Switch de Encendido, ");
            if (!this.vista.tieneVelocidadMinima.Value)
                s.Append("Velocidad Mínima del Motor, ");
            if (!this.vista.tieneVelocidadMaxima.Value)
                s.Append("Velocidad Máxima del Motor, ");
            #endregion
            #region Miscelaneos
            if (!vista.tieneTapaCombustible.Value)
                s.Append("Tapa de Combustible, ");
            if (!this.vista.tieneCondicionLLantas.Value)
                s.Append("Condición de Llantas, ");
            if (!this.vista.tieneCondicionPintura.Value)
                s.Append("Condición de Pintura,");
            if (!this.vista.tieneCondicionCalcas.Value)
                s.Append("Condición de Calcas, ");
            if (!vista.tieneSimbolosSeguridad.Value)
                s.Append("Símbolos de Seguridad de la Máquina, ");
            if (!this.vista.tieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis , ");
            if (!this.vista.tieneAntenasMonitoreo.Value)
                s.Append("Antenas Monitoreo Satelital, ");

            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }
        #endregion

        public void DatoToInterfazUsuario(ListadoVerificacionTorresLuzBO bo) {

            #region General
            this.vista.tienePresionLlantas = bo.TienePresionLlantas.HasValue ? bo.TienePresionLlantas : null;
            this.vista.tieneBandaVentilador = bo.TieneBandaVentilador.HasValue ? bo.TieneBandaVentilador : null;
            this.vista.tieneManguerasAbrazaderas = bo.TieneManguerasAbrazaderas.HasValue ? bo.TieneManguerasAbrazaderas : null;
            this.vista.tieneCartuchoFiltroAire = bo.TieneCartuchoFiltroAire.HasValue ? bo.TieneCartuchoFiltroAire : null;
            this.vista.tieneCableLevanteTorre = bo.TieneCableLevanteTorre.HasValue ? bo.TieneCableLevanteTorre : null;
            #endregion
            #region Niveles de fluidos
            this.vista.tieneCombustible = bo.TieneCombustible.HasValue ? bo.TieneCombustible : null;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor.HasValue ? bo.TieneAceiteMotor : null;
            this.vista.tieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante.HasValue ? bo.TieneLiquidoRefrigerante : null;
            this.vista.tieneBateria = bo.TieneBateria.HasValue ? bo.TieneBateria : null;

            #endregion
            #region Funciones Electricas
            this.vista.tieneLucesTorre = bo.TieneLucesTorre.HasValue ? bo.TieneLucesTorre : null;
            this.vista.tieneLamparasTablero = bo.TieneLamparasTablero.HasValue ? bo.TieneLamparasTablero : null;
            #endregion
            #region Controles
            this.vista.tieneLucesAdvertencia = bo.TieneLucesAdvertencia.HasValue ? bo.TieneLucesAdvertencia : null;
            this.vista.tieneSwitchEncendido = bo.TieneSwitchEncendido.HasValue ? bo.TieneSwitchEncendido : null;
            this.vista.tieneVelocidadMinima = bo.TieneVelocidadMinima.HasValue ? bo.TieneVelocidadMinima : null;
            this.vista.tieneVelocidadMaxima = bo.TieneVelocidadMaxima.HasValue ? bo.TieneVelocidadMaxima : null;
            #endregion

            #region Miscelaneos

            this.vista.tieneTapaCombustible = bo.TieneTapaCombustible.HasValue ? bo.TieneTapaCombustible : null;
            this.vista.tieneCondicionLLantas = bo.TieneCondicionLlantas.HasValue ? bo.TieneCondicionLlantas : null;
            this.vista.tieneCondicionPintura = bo.TieneCondicionPintura.HasValue ? bo.TieneCondicionPintura : null;
            this.vista.tieneCondicionCalcas = bo.TieneCondicionCalcas.HasValue ? bo.TieneCondicionCalcas : null;
            this.vista.tieneSimbolosSeguridad = bo.TieneSimbolosSeguridad.HasValue ? bo.TieneSimbolosSeguridad : null;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis.HasValue ? bo.TieneEstructuraChasis : null;
            this.vista.tieneAntenasMonitoreo = bo.TieneAntenasMonitoreo.HasValue ? bo.TieneAntenasMonitoreo : null;

            #endregion

        }
        public object InterfazUsuarioADato() {
            ListadoVerificacionTorresLuzBO bo = new ListadoVerificacionTorresLuzBO();

            #region General

            if (this.vista.tienePresionLlantas.HasValue)
                bo.TienePresionLlantas = this.vista.tienePresionLlantas.Value;

            if (this.vista.tieneBandaVentilador.HasValue)
                bo.TieneBandaVentilador = this.vista.tieneBandaVentilador.Value;

            if (this.vista.tieneManguerasAbrazaderas.HasValue)
                bo.TieneManguerasAbrazaderas = this.vista.tieneManguerasAbrazaderas.Value;

            if (this.vista.tieneCartuchoFiltroAire.HasValue)
                bo.TieneCartuchoFiltroAire = this.vista.tieneCartuchoFiltroAire.Value;

            if (this.vista.tieneCableLevanteTorre.HasValue)
                bo.TieneCableLevanteTorre = this.vista.tieneCableLevanteTorre.Value;

            #endregion
            #region Niveles de fluidos

            if (this.vista.tieneCombustible.HasValue)
                bo.TieneCombustible = this.vista.tieneCombustible.Value;

            if (this.vista.tieneAceiteMotor.HasValue)
                bo.TieneAceiteMotor = this.vista.tieneAceiteMotor.Value;

            if (this.vista.tieneLiquidoRefrigerante.HasValue)
                bo.TieneLiquidoRefrigerante = this.vista.tieneLiquidoRefrigerante.Value;

            if (this.vista.tieneBateria.HasValue)
                bo.TieneBateria = this.vista.tieneBateria.Value;

            #endregion
            #region Funciones Electricas

            if (this.vista.tieneLucesTorre.HasValue)
                bo.TieneLucesTorre = this.vista.tieneLucesTorre.Value;

            if (this.vista.tieneLamparasTablero.HasValue)
                bo.TieneLamparasTablero = this.vista.tieneLamparasTablero.Value;


            #endregion
            #region Controles

            if (this.vista.tieneLucesAdvertencia.HasValue)
                bo.TieneLucesAdvertencia = this.vista.tieneLucesAdvertencia.Value;

            if (this.vista.tieneSwitchEncendido.HasValue)
                bo.TieneSwitchEncendido = this.vista.tieneSwitchEncendido.Value;

            if (this.vista.tieneVelocidadMinima.HasValue)
                bo.TieneVelocidadMinima = this.vista.tieneVelocidadMinima.Value;

            if (this.vista.tieneVelocidadMaxima.HasValue)
                bo.TieneVelocidadMaxima = this.vista.tieneVelocidadMaxima.Value;

            #endregion

            #region Miscelaneos

            if (this.vista.tieneTapaCombustible.HasValue)
                bo.TieneTapaCombustible = this.vista.tieneTapaCombustible.Value;

            if (this.vista.tieneCondicionLLantas.HasValue)
                bo.TieneCondicionLlantas = this.vista.tieneCondicionLLantas.Value;

            if (this.vista.tieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.tieneCondicionPintura.Value;

            if (this.vista.tieneCondicionCalcas.HasValue)
                bo.TieneCondicionCalcas = this.vista.tieneCondicionCalcas.Value;

            if (this.vista.tieneSimbolosSeguridad.HasValue)
                bo.TieneSimbolosSeguridad = this.vista.tieneSimbolosSeguridad.Value;

            if (this.vista.tieneEstructuraChasis.HasValue)
                bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis.Value;

            if (this.vista.tieneAntenasMonitoreo.HasValue)
                bo.TieneAntenasMonitoreo = this.vista.tieneAntenasMonitoreo.Value;

            #endregion

            return bo;
        }
    }
}