
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionMiniCargadorPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMiniCargadorPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionMiniCargadorPSLVIS vista;
        internal IucPuntosVerificacionMiniCargadorPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionMiniCargadorPSLPRE(IucPuntosVerificacionMiniCargadorPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionMiniCargadorPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionMiniCargadorBO bo = (ListadoVerificacionMiniCargadorBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.TieneAceiteCompartimientoCadena = null;
            this.vista.TieneAceiteHidraulico = null;
            this.vista.TieneAceiteMotor = null;
            this.vista.TieneAntenaMonitoreo = null;
            this.vista.TieneBandaVentilador = null;
            this.vista.TieneBarraSeguridad = null;
            this.vista.TieneBateria = null;
            this.vista.TieneCartuchoFiltro = null;
            this.vista.TieneCombustible = null;
            this.vista.TieneCondicionAsiento = null;
            this.vista.TieneCondicionCalcas = null;
            this.vista.TieneCondicionLlantas = null;
            this.vista.TieneCondicionPintura = null;
            this.vista.TieneCopleMecanico = null;
            this.vista.TieneEstructuraChasis = null;
            this.vista.TieneFrenoEstacionamiento = null;
            this.vista.TieneIndicadores = null;
            this.vista.TieneInterruptorDesconexion = null;
            this.vista.TieneLamparasTablero = null;
            this.vista.TieneLiquidoRefrigerante = null;
            this.vista.TieneLucesAdvertencia = null;
            this.vista.TieneLucesTrabajoDelantera = null;
            this.vista.TieneLucesTrabajoTrasera = null;
            this.vista.TieneMaguerasYAbrazaderas = null;
            this.vista.TienePalancas = null;
            this.vista.TienePasadoresCargador = null;
            this.vista.TienePresionLlantas = null;
            this.vista.TieneSimbolosSeguridad = null;
            this.vista.TieneTacometro = null;
            this.vista.TieneTapaCombustible = null;
            this.vista.TieneTapaHidraulico = null;
            this.vista.TieneVelocidadMaxMotor = null;
            this.vista.TieneVelocidadMinMotor = null;

        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region En General

            if (!vista.TienePresionLlantas.Value)
                s.Append("Presión de Llantas, ");


            if (!vista.TieneBandaVentilador.Value)
                s.Append("Banda del Ventilador, ");


            if (!vista.TieneMaguerasYAbrazaderas.Value)
                s.Append("Mangueras y abrazaderas de Admisión de Aire, ");


            if (!vista.TieneCartuchoFiltro.Value)
                s.Append("Cartucho de Filtro de Aire, ");

            if (!vista.TieneCopleMecanico.Value)
                s.Append("Cople Mecánico del Bote, ");

            #endregion

            #region Niveles de Flujos

            if (!vista.TieneCombustible.Value)
                s.Append("Combustible, ");

            if (!vista.TieneAceiteMotor.Value)
                s.Append("Aceite del Motor, ");

            if (!vista.TieneAceiteHidraulico.Value)
                s.Append("Aceite Hidráulico, ");

            if (!vista.TieneLiquidoRefrigerante.Value)
                s.Append("Líquido Refrigerante, ");

            if (!vista.TieneAceiteCompartimientoCadena.Value)
                s.Append("Aceite del Compartimiento de Cadenas, ");

            if (!vista.TieneBateria.Value)
                s.Append("Batería, ");

            #endregion

            #region Lubricacion

            if (!vista.TienePasadoresCargador.Value)
                s.Append("Pasadores del Cargador, ");

            #endregion

            #region Funciones electricas

            if (!vista.TieneLucesTrabajoDelantera.Value)
                s.Append("Luces de Trabajo Delantera, ");

            if (!vista.TieneLucesTrabajoTrasera.Value)
                s.Append("Luces de Trabajo Traseras, ");

            if (!vista.TieneLamparasTablero.Value)
                s.Append("Lámparas del Tablero, ");

            if (!vista.TieneInterruptorDesconexion.Value)
                s.Append("Interruptor de Desconexión, ");


            #endregion

            #region Controles

            if (!vista.TienePalancas.Value)
                s.Append("Palancas, ");

            if (!vista.TieneLucesAdvertencia.Value)
                s.Append("Luces de Advertencia, ");

            if (!vista.TieneIndicadores.Value)
                s.Append("Indicadores, ");

            if (!vista.TieneTacometro.Value)
                s.Append("Tacómetro, ");

            if (!vista.TieneFrenoEstacionamiento.Value)
                s.Append("Freno de Estacionamiento, ");

            if (!vista.TieneBarraSeguridad.Value)
                s.Append("Barra de Seguridad, ");

            if (!vista.TieneVelocidadMinMotor.Value)
                s.Append("Velocidad Mínima del Motor, ");

            if (!vista.TieneVelocidadMaxMotor.Value)
                s.Append("Velocidad Máxima del Motor, ");
            #endregion

            #region Miscelanios

            if (!vista.TieneTapaCombustible.Value)
                s.Append("Tapa de Combustible , ");

            if (!vista.TieneTapaHidraulico.Value)
                s.Append("Tapa del Hidráulico, ");

            if (!vista.TieneCondicionAsiento.Value)
                s.Append("Condición del Asiento, ");

            if (!vista.TieneCondicionLlantas.Value)
                s.Append("Condición de Llantas, ");

            if (!vista.TieneCondicionPintura.Value)
                s.Append("Condición de Pintura, ");

            if (!vista.TieneCondicionCalcas.Value)
                s.Append("Condición de Calcas, ");

            if (!vista.TieneSimbolosSeguridad.Value)
                s.Append("Símbolos de Seguridad de la Máquina, ");

            if (!vista.TieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis, ");

            if (!vista.TieneAntenaMonitoreo.Value)
                s.Append("Antenas Monitoreo Satelital, ");

            #endregion

            if (s.Length > 0)
                return "Es necesario agregar observación para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }
        #endregion

        public object InterfazUsuarioADato() {
            ListadoVerificacionMiniCargadorBO bo = new ListadoVerificacionMiniCargadorBO();

            if (this.vista.TieneAceiteCompartimientoCadena.HasValue)
                bo.TieneAceiteCompartimientoCadena = this.vista.TieneAceiteCompartimientoCadena.Value;


            if (this.vista.TieneAceiteHidraulico.HasValue)
                bo.TieneAceiteHidraulico = this.vista.TieneAceiteHidraulico.Value;

            if (this.vista.TieneAceiteMotor.HasValue)
                bo.TieneAceiteMotor = this.vista.TieneAceiteMotor.Value;

            if (this.vista.TieneAntenaMonitoreo.HasValue)
                bo.TieneAntenaMonitoreo = this.vista.TieneAntenaMonitoreo.Value;

            if (this.vista.TieneBandaVentilador.HasValue)
                bo.TieneBandaVentilador = this.vista.TieneBandaVentilador.Value;

            if (this.vista.TieneBarraSeguridad.HasValue)
                bo.TieneBarraSeguridad = this.vista.TieneBarraSeguridad.Value;

            if (this.vista.TieneBateria.HasValue)
                bo.TieneBateria = this.vista.TieneBateria.Value;

            if (this.vista.TieneCartuchoFiltro.HasValue)
                bo.TieneCartuchoFiltro = this.vista.TieneCartuchoFiltro.Value;

            if (this.vista.TieneCombustible.HasValue)
                bo.TieneCombustible = this.vista.TieneCombustible.Value;

            if (this.vista.TieneCondicionAsiento.HasValue)
                bo.TieneCondicionAsiento = this.vista.TieneCondicionAsiento.Value;

            if (this.vista.TieneCondicionCalcas.HasValue)
                bo.TieneCondicionCalcas = this.vista.TieneCondicionCalcas.Value;

            if (this.vista.TieneCondicionLlantas.HasValue)
                bo.TieneCondicionLlantas = this.vista.TieneCondicionLlantas.Value;

            if (this.vista.TieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.TieneCondicionPintura.Value;

            if (this.vista.TieneCopleMecanico.HasValue)
                bo.TieneCopleMecanico = this.vista.TieneCopleMecanico.Value;

            if (this.vista.TieneEstructuraChasis.HasValue)
                bo.TieneEstructuraChasis = this.vista.TieneEstructuraChasis.Value;

            if (this.vista.TieneFrenoEstacionamiento.HasValue)
                bo.TieneFrenoEstacionamiento = this.vista.TieneFrenoEstacionamiento.Value;

            if (this.vista.TieneIndicadores.HasValue)
                bo.TieneIndicadores = this.vista.TieneIndicadores.Value;

            if (this.vista.TieneInterruptorDesconexion.HasValue)
                bo.TieneInterruptorDesconexion = this.vista.TieneInterruptorDesconexion.Value;

            if (this.vista.TieneLamparasTablero.HasValue)
                bo.TieneLamparasTablero = this.vista.TieneLamparasTablero.Value;

            if (this.vista.TieneLiquidoRefrigerante.HasValue)
                bo.TieneLiquidoRefrigerante = this.vista.TieneLiquidoRefrigerante.Value;

            if (this.vista.TieneLucesAdvertencia.HasValue)
                bo.TieneLucesAdvertencia = this.vista.TieneLucesAdvertencia.Value;

            if (this.vista.TieneLucesTrabajoDelantera.HasValue)
                bo.TieneLucesTrabajoDelantera = this.vista.TieneLucesTrabajoDelantera.Value;

            if (this.vista.TieneLucesTrabajoTrasera.HasValue)
                bo.TieneLucesTrabajoTrasera = this.vista.TieneLucesTrabajoTrasera.Value;

            if (this.vista.TieneMaguerasYAbrazaderas.HasValue)
                bo.TieneMaguerasYAbrazaderas = this.vista.TieneMaguerasYAbrazaderas.Value;

            if (this.vista.TienePalancas.HasValue)
                bo.TienePalancas = this.vista.TienePalancas.Value;

            if (this.vista.TienePresionLlantas.HasValue)
                bo.TienePresionLlantas = this.vista.TienePresionLlantas.Value;

            if (this.vista.TieneSimbolosSeguridad.HasValue)
                bo.TieneSimbolosSeguridad = this.vista.TieneSimbolosSeguridad.Value;

            if (this.vista.TieneTacometro.HasValue)
                bo.TieneTacometro = this.vista.TieneTacometro.Value;

            if (this.vista.TieneTapaCombustible.HasValue)
                bo.TieneTapaCombustible = this.vista.TieneTapaCombustible.Value;


            if (this.vista.TieneTapaHidraulico.HasValue)
                bo.TieneTapaHidraulico = this.vista.TieneTapaHidraulico.Value;


            if (this.vista.TieneVelocidadMaxMotor.HasValue)
                bo.TieneVelocidadMaxMotor = this.vista.TieneVelocidadMaxMotor.Value;


            if (this.vista.TieneVelocidadMinMotor.HasValue)
                bo.TieneVelocidadMinMotor = this.vista.TieneVelocidadMinMotor.Value;

            if (this.vista.TienePasadoresCargador.HasValue)
                bo.TienePasadoresCargador = this.vista.TienePasadoresCargador.Value;

            return bo;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionMiniCargadorBO bo) {

            #region GENERAL (Campos booleanos)
            this.vista.TienePresionLlantas = bo.TienePresionLlantas;
            this.vista.TieneBandaVentilador = bo.TieneBandaVentilador;
            this.vista.TieneMaguerasYAbrazaderas = bo.TieneMaguerasYAbrazaderas;
            this.vista.TieneCartuchoFiltro = bo.TieneCartuchoFiltro;
            this.vista.TieneCopleMecanico = bo.TieneCopleMecanico;


            #endregion

            #region Niveles de Fluido (Campos booleanos)
            this.vista.TieneCombustible = bo.TieneCombustible;
            this.vista.TieneAceiteMotor = bo.TieneAceiteMotor;
            this.vista.TieneAceiteHidraulico = bo.TieneAceiteHidraulico;
            this.vista.TieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante;
            this.vista.TieneAceiteCompartimientoCadena = bo.TieneAceiteCompartimientoCadena;
            this.vista.TieneBateria = bo.TieneBateria;


            #endregion

            #region Lubricacion (Campos booleanos)

            this.vista.TienePasadoresCargador = bo.TienePasadoresCargador;

            #endregion

            #region Funciones Electricas


            this.vista.TieneLucesTrabajoDelantera = bo.TieneLucesTrabajoDelantera;

            this.vista.TieneLucesTrabajoTrasera = bo.TieneLucesTrabajoTrasera;

            this.vista.TieneLamparasTablero = bo.TieneLamparasTablero;

            this.vista.TieneInterruptorDesconexion = bo.TieneInterruptorDesconexion;

            #endregion

            #region Controles

            this.vista.TienePalancas = bo.TienePalancas;
            this.vista.TieneLucesAdvertencia = bo.TieneLucesAdvertencia;
            this.vista.TieneIndicadores = bo.TieneIndicadores;
            this.vista.TieneTacometro = bo.TieneTacometro;
            this.vista.TieneFrenoEstacionamiento = bo.TieneFrenoEstacionamiento;
            this.vista.TieneBarraSeguridad = bo.TieneBarraSeguridad;
            this.vista.TieneVelocidadMinMotor = bo.TieneVelocidadMinMotor;
            this.vista.TieneVelocidadMaxMotor = bo.TieneVelocidadMaxMotor;

            #endregion

            #region Miscelanios

            this.vista.TieneTapaCombustible = bo.TieneTapaCombustible;

            this.vista.TieneTapaHidraulico = bo.TieneTapaHidraulico;

            this.vista.TieneCondicionAsiento = bo.TieneCondicionAsiento;

            this.vista.TieneCondicionLlantas = bo.TieneCondicionLlantas;

            this.vista.TieneCondicionPintura = bo.TieneCondicionPintura;

            this.vista.TieneCondicionCalcas = bo.TieneCondicionCalcas;

            this.vista.TieneSimbolosSeguridad = bo.TieneSimbolosSeguridad;

            this.vista.TieneEstructuraChasis = bo.TieneEstructuraChasis;

            this.vista.TieneAntenaMonitoreo = bo.TieneAntenaMonitoreo;

            #endregion

        }
    }
}