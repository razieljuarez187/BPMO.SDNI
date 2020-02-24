
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionMontaCargaPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMontaCargaPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionMontaCargaPSLVIS vista;
        internal IucPuntosVerificacionMontaCargaPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionMontaCargaPSLPRE(IucPuntosVerificacionMontaCargaPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionMontaCargaPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionMontaCargaBO bo = (ListadoVerificacionMontaCargaBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.TieneAceiteEjeDelantero = null;
            this.vista.TieneAceiteFrenos = null;
            this.vista.TieneAceiteHidraulico = null;
            this.vista.TieneAceiteMotor = null;
            this.vista.TieneAceiteTransmision = null;
            this.vista.TieneAlarmaReversa = null;
            this.vista.TieneAntenasMonitoreo = null;
            this.vista.TieneBandaVentilador = null;
            this.vista.TieneBateria = null;
            this.vista.TieneCartuchoFiltro = null;
            this.vista.TieneCinturonSeguridad = null;
            this.vista.TieneCofreMotor = null;
            this.vista.TieneCombustible = null;
            this.vista.TieneCondicionCalcas = null;
            this.vista.TieneCondicionLlantas = null;
            this.vista.TieneCondicionPintura = null;
            this.vista.TieneEspejoRetrovisor = null;
            this.vista.TieneEstructuraChasis = null;
            this.vista.TieneFrenoEstacionamiento = null;
            this.vista.TieneIndicadores = null;
            this.vista.TieneLamparasTablero = null;
            this.vista.TieneLiquidoRefrigerante = null;
            this.vista.TieneLucesAdvertencia = null;
            this.vista.TieneLucesTrabajoDelantera = null;
            this.vista.TieneLucesTrabajoTrasera = null;
            this.vista.TieneManguerasYAbrazaderas = null;
            this.vista.TieneOperacionMastil = null;
            this.vista.TienePalancaAvanceReversa = null;
            this.vista.TienePivoteArticulacion = null;
            this.vista.TienePresionEnLlantas = null;
            this.vista.TieneSimbolosSeguridad = null;
            this.vista.TieneTapaCombustible = null;
            this.vista.TieneTensionCadena = null;
            this.vista.TieneVelocidadMaxMotor = null;
            this.vista.TieneVelocidadMinMotor = null;
            this.vista.TieneTapaHidraulico = null;
            this.vista.TieneCondicionAsiento = null;

        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            if (!vista.TienePresionEnLlantas.Value)
                s.Append("Presión de llantas, ");

            if (!vista.TieneBandaVentilador.Value)
                s.Append("Banda del ventilador, ");

            if (!this.vista.TieneManguerasYAbrazaderas.Value)
                s.Append("Mangueras y abrazaderas de admisión de aire, ");

            if (!this.vista.TieneCartuchoFiltro.Value)
                s.Append("Cartucho del filtro de aire, ");

            if (!this.vista.TieneOperacionMastil.Value)
                s.Append("Operación de mástil, ");

            if (!this.vista.TieneCinturonSeguridad.Value)
                s.Append("Cinturón de seguridad, ");

            if (!this.vista.TieneTensionCadena.Value)
                s.Append("Tensión cadena, ");

            if (!this.vista.TieneEspejoRetrovisor.Value)
                s.Append("Espejo retrovisor, ");

            if (!this.vista.TieneCombustible.Value)
                s.Append("Combustible, ");

            if (!this.vista.TieneAceiteMotor.Value)
                s.Append("Aceite motor, ");

            if (!this.vista.TieneAceiteHidraulico.Value)
                s.Append("Aceite Hidráulico, ");

            if (!this.vista.TieneLiquidoRefrigerante.Value)
                s.Append("Líquido refrigerante, ");

            if (!this.vista.TieneAceiteTransmision.Value)
                s.Append("Aceite de transmisión, ");

            if (!this.vista.TieneAceiteEjeDelantero.Value)
                s.Append("Aceite eje delantero, ");

            if (!this.vista.TieneAceiteFrenos.Value)
                s.Append("Aceite de frenos, ");

            if (!this.vista.TieneBateria.Value)
                s.Append("Batería, ");

            if (!this.vista.TienePivoteArticulacion.Value)
                s.Append("Pivote de la articulación y dirección, ");

            if (!this.vista.TieneCofreMotor.Value)
                s.Append("Cofre del Motor, ");

            if (!this.vista.TieneLucesTrabajoDelantera.Value)
                s.Append("Luces de trabajo delantera, ");

            if (!this.vista.TieneLucesTrabajoTrasera.Value)
                s.Append("Luces de trabajo traseras, ");

            if (!this.vista.TieneLamparasTablero.Value)
                s.Append("Lámparas del tablero, ");

            if (!this.vista.TieneAlarmaReversa.Value)
                s.Append("Alarma de reversa, ");

            if (!this.vista.TienePalancaAvanceReversa.Value)
                s.Append("Palanca avance/reversa, ");

            if (!this.vista.TieneLucesAdvertencia.Value)
                s.Append("Luces de advertencia, ");

            if (!this.vista.TieneIndicadores.Value)
                s.Append("Indicadores, ");

            if (!this.vista.TieneFrenoEstacionamiento.Value)
                s.Append("Freno de estacionamiento, ");

            if (!this.vista.TieneVelocidadMinMotor.Value)
                s.Append("Velocidad mínima del motor, ");

            if (!this.vista.TieneVelocidadMaxMotor.Value)
                s.Append("Velocidad máxima del motor, ");

            if (!this.vista.TieneTapaCombustible.Value)
                s.Append("Tapa de combustible, ");

            if (!this.vista.TieneTapaHidraulico.Value)
                s.Append("Tapa del hidráulico, ");

            if (!this.vista.TieneCondicionAsiento.Value)
                s.Append("Condición del asiento, ");

            if (!this.vista.TieneCondicionLlantas.Value)
                s.Append("Condición de llantas, ");

            if (!this.vista.TieneCondicionPintura.Value)
                s.Append("Condición de pintura, ");

            if (!this.vista.TieneCondicionCalcas.Value)
                s.Append("Condición de calcas, ");

            if (!this.vista.TieneSimbolosSeguridad.Value)
                s.Append("Símbolos de seguridad de la máquina, ");

            if (!this.vista.TieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis, ");

            if (!this.vista.TieneAntenasMonitoreo.Value)
                s.Append("Antenas monitoreo satelital, ");



            if (s.Length > 0)
                return "Es necesario agregar observación para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionMontaCargaBO bo) {

            #region General

            this.vista.TienePresionEnLlantas = bo.TienePresionEnLlantas;
            this.vista.TieneBandaVentilador = bo.TieneBandaVentilador;
            this.vista.TieneManguerasYAbrazaderas = bo.TieneManguerasYAbrazaderas;
            this.vista.TieneCartuchoFiltro = bo.TieneCartuchoFiltro;
            this.vista.TieneOperacionMastil = bo.TieneOperacionMastil;
            this.vista.TieneCinturonSeguridad = bo.TieneCinturonSeguridad;
            this.vista.TieneTensionCadena = bo.TieneTensionCadena;
            this.vista.TieneEspejoRetrovisor = bo.TieneEspejoRetrovisor;

            #endregion

            #region Niveles de Flujo

            this.vista.TieneCombustible = bo.TieneCombustible;
            this.vista.TieneAceiteMotor = bo.TieneAceiteMotor;
            this.vista.TieneAceiteHidraulico = bo.TieneAceiteHidraulico;
            this.vista.TieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante;
            this.vista.TieneAceiteTransmision = bo.TieneAceiteTransmision;
            this.vista.TieneAceiteEjeDelantero = bo.TieneAceiteEjeDelantero;
            this.vista.TieneAceiteFrenos = bo.TieneAceiteFrenos;
            this.vista.TieneBateria = bo.TieneBateria;

            #endregion

            #region Lubricacion

            this.vista.TienePivoteArticulacion = bo.TienePivoteArticulacion;
            this.vista.TieneCofreMotor = bo.TieneCofreMotor;

            #endregion

            #region Funciones Electricas

            this.vista.TieneLucesTrabajoDelantera = bo.TieneLucesTrabajoDelantera;
            this.vista.TieneLucesTrabajoTrasera = bo.TieneLucesTrabajoTrasera;
            this.vista.TieneLamparasTablero = bo.TieneLamparasTablero;
            this.vista.TieneAlarmaReversa = bo.TieneAlarmaReversa;

            #endregion

            #region Controles

            this.vista.TienePalancaAvanceReversa = bo.TienePalancaAvanceReversa;
            this.vista.TieneLucesAdvertencia = bo.TieneLucesAdvertencia;
            this.vista.TieneIndicadores = bo.TieneIndicadores;
            this.vista.TieneFrenoEstacionamiento = bo.TieneFrenoEstacionamiento;
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
            this.Vista.TieneSimbolosSeguridad = bo.TieneSimbolosSeguridad;
            this.vista.TieneEstructuraChasis = bo.TieneEstructuraChasis;
            this.Vista.TieneAntenasMonitoreo = bo.TieneAntenasMonitoreo;


            #endregion


        }

        #endregion

        public object InterfazUsuarioADato() {
            ListadoVerificacionMontaCargaBO bo = new ListadoVerificacionMontaCargaBO();

            if (this.vista.TienePresionEnLlantas.HasValue)
                bo.TienePresionEnLlantas = this.vista.TienePresionEnLlantas.Value;

            if (this.vista.TieneBandaVentilador.HasValue)
                bo.TieneBandaVentilador = this.vista.TieneBandaVentilador.Value;

            if (this.vista.TieneManguerasYAbrazaderas.HasValue)
                bo.TieneManguerasYAbrazaderas = this.vista.TieneManguerasYAbrazaderas.Value;

            if (this.vista.TieneCartuchoFiltro.HasValue)
                bo.TieneCartuchoFiltro = this.vista.TieneCartuchoFiltro.Value;

            if (this.vista.TieneOperacionMastil.HasValue)
                bo.TieneOperacionMastil = this.vista.TieneOperacionMastil.Value;

            if (this.vista.TieneCinturonSeguridad.HasValue)
                bo.TieneCinturonSeguridad = this.vista.TieneCinturonSeguridad.Value;

            if (this.vista.TieneTensionCadena.HasValue)
                bo.TieneTensionCadena = this.vista.TieneTensionCadena.Value;

            if (this.vista.TieneEspejoRetrovisor.HasValue)
                bo.TieneEspejoRetrovisor = this.vista.TieneEspejoRetrovisor.Value;

            if (this.vista.TieneCombustible.HasValue)
                bo.TieneCombustible = this.vista.TieneCombustible.Value;

            if (this.vista.TieneAceiteMotor.HasValue)
                bo.TieneAceiteMotor = this.vista.TieneAceiteMotor.Value;

            if (this.vista.TieneAceiteHidraulico.HasValue)
                bo.TieneAceiteHidraulico = this.vista.TieneAceiteHidraulico.Value;

            if (this.vista.TieneLiquidoRefrigerante.HasValue)
                bo.TieneLiquidoRefrigerante = this.vista.TieneLiquidoRefrigerante.Value;

            if (this.vista.TieneAceiteTransmision.HasValue)
                bo.TieneAceiteTransmision = this.vista.TieneAceiteTransmision.Value;

            if (this.vista.TieneAceiteEjeDelantero.HasValue)
                bo.TieneAceiteEjeDelantero = this.vista.TieneAceiteEjeDelantero.Value;

            if (this.vista.TieneAceiteFrenos.HasValue)
                bo.TieneAceiteFrenos = this.vista.TieneAceiteFrenos.Value;

            if (this.vista.TieneBateria.HasValue)
                bo.TieneBateria = this.vista.TieneBateria.Value;

            if (this.vista.TienePivoteArticulacion.HasValue)
                bo.TienePivoteArticulacion = this.vista.TienePivoteArticulacion.Value;

            if (this.vista.TieneCofreMotor.HasValue)
                bo.TieneCofreMotor = this.vista.TieneCofreMotor.Value;

            if (this.vista.TieneLucesTrabajoDelantera.HasValue)
                bo.TieneLucesTrabajoDelantera = this.vista.TieneLucesTrabajoDelantera.Value;

            if (this.vista.TieneLucesTrabajoTrasera.HasValue)
                bo.TieneLucesTrabajoTrasera = this.vista.TieneLucesTrabajoTrasera.Value;

            if (this.vista.TieneLamparasTablero.HasValue)
                bo.TieneLamparasTablero = this.vista.TieneLamparasTablero.Value;

            if (this.vista.TieneAlarmaReversa.HasValue)
                bo.TieneAlarmaReversa = this.vista.TieneAlarmaReversa.Value;

            if (this.vista.TienePalancaAvanceReversa.HasValue)
                bo.TienePalancaAvanceReversa = this.vista.TienePalancaAvanceReversa.Value;

            if (this.vista.TieneLucesAdvertencia.HasValue)
                bo.TieneLucesAdvertencia = this.vista.TieneLucesAdvertencia.Value;

            if (this.vista.TieneIndicadores.HasValue)
                bo.TieneIndicadores = this.vista.TieneIndicadores.Value;

            if (this.vista.TieneFrenoEstacionamiento.HasValue)
                bo.TieneFrenoEstacionamiento = this.vista.TieneFrenoEstacionamiento.Value;

            if (this.vista.TieneVelocidadMinMotor.HasValue)
                bo.TieneVelocidadMinMotor = this.vista.TieneVelocidadMinMotor.Value;

            if (this.vista.TieneVelocidadMaxMotor.HasValue)
                bo.TieneVelocidadMaxMotor = this.vista.TieneVelocidadMaxMotor.Value;

            if (this.vista.TieneTapaCombustible.HasValue)
                bo.TieneTapaCombustible = this.vista.TieneTapaCombustible.Value;

            if (this.vista.TieneCondicionLlantas.HasValue)
                bo.TieneCondicionLlantas = this.vista.TieneCondicionLlantas.Value;

            if (this.vista.TieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.TieneCondicionPintura.Value;

            if (this.vista.TieneCondicionCalcas.HasValue)
                bo.TieneCondicionCalcas = this.vista.TieneCondicionCalcas.Value;

            if (this.vista.TieneSimbolosSeguridad.HasValue)
                bo.TieneSimbolosSeguridad = this.vista.TieneSimbolosSeguridad.Value;

            if (this.vista.TieneEstructuraChasis.HasValue)
                bo.TieneEstructuraChasis = this.vista.TieneEstructuraChasis.Value;

            if (this.vista.TieneAntenasMonitoreo.HasValue)
                bo.TieneAntenasMonitoreo = this.vista.TieneAntenasMonitoreo.Value;

            if (this.vista.TieneTapaHidraulico.HasValue)
                bo.TieneTapaHidraulico = this.vista.TieneTapaHidraulico.Value;

            if (this.vista.TieneCondicionAsiento.HasValue)
                bo.TieneCondicionAsiento = this.vista.TieneCondicionAsiento.Value;


            return bo;
        }
    }
}