
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;
namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionVibroCompactadorPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionVibroCompactadorPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionVibroCompactadorPSLVIS vista;
        internal IucPuntosVerificacionVibroCompactadorPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionVibroCompactadorPSLPRE(IucPuntosVerificacionVibroCompactadorPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionVibroCompactadorPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionVibroCompactadorBO bo = (ListadoVerificacionVibroCompactadorBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.tieneAceiteHidraulico = null;
            this.vista.tieneAceiteMotor = null;
            this.vista.tieneAlarmaReversa = null;
            this.vista.tieneAntenaMonitoreo = null;
            this.vista.tieneBandaVentilador = null;
            this.vista.tieneBateria = null;
            this.vista.tieneCabinaOperador = null;
            this.vista.tieneCajaReduccionEngranes = null;
            this.vista.tieneCartuchoFiltroAire = null;
            this.vista.tieneCinturonSeguridad = null;
            this.vista.tieneCofreMotor = null;
            this.vista.tieneCombustible = null;
            this.vista.tieneCondicionCalcas = null;
            this.vista.tieneCondicionAsiento = null;
            this.vista.tieneCondicionLlanta = null;
            this.vista.tieneCondicionPintura = null;
            this.vista.tieneEstructuraChasis = null;
            this.vista.tieneFrenoEstacionamiento = null;
            this.vista.tieneIndicadores = null;
            this.vista.tieneInterruptorDesconexion = null;
            this.vista.tieneLamparaTablero = null;
            this.vista.tieneLiquidoRefrigerante = null;
            this.vista.tieneLucesAdvertencia = null;
            this.vista.tieneLucesTrabajoDelantera = null;
            this.vista.tieneLucesTrabajoTraseras = null;
            this.vista.tieneManguerasAbrazaderas = null;
            this.vista.tienePalanca = null;
            this.vista.tienePresionLlantas = null;
            this.vista.tienePivoteArticulacionDireccion = null;
            this.vista.tieneRascadoresTambor = null;
            this.vista.tieneReductorEngranes = null;
            this.vista.tieneSimbolosSeguridad = null;
            this.vista.tieneSistemaVibracion = null;
            this.vista.tieneTacometro = null;
            this.vista.tieneTapaCombustible = null;
            this.vista.tieneTapaHidraulico = null;
            this.vista.tieneVelocidadMaxima = null;
            this.vista.tieneVelocidadMinima = null;

        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region General
            if (!vista.tienePresionLlantas.Value)
                s.Append("Presión de llantas, ");
            if (!this.vista.tieneBandaVentilador.Value)
                s.Append("Banda del ventilador, ");
            if (!this.vista.tieneManguerasAbrazaderas.Value)
                s.Append("Mangueras y abrazaderas de admisión de aire, ");
            if (!this.vista.tieneCartuchoFiltroAire.Value)
                s.Append("Cartucho del filtro de aire, ");
            if (!this.vista.tieneRascadoresTambor.Value)
                s.Append("Rascadores del tambor, ");
            if (!this.vista.tieneCinturonSeguridad.Value)
                s.Append("Cinturón de seguridad, ");

            #endregion

            #region Niveles de fluidos
            if (!vista.tieneCombustible.Value)
                s.Append("Combustible, ");
            if (!this.vista.tieneAceiteMotor.Value)
                s.Append("Aceite del motor, ");
            if (!this.vista.tieneAceiteHidraulico.Value)
                s.Append("Aceite hidráulico, ");
            if (!this.vista.tieneLiquidoRefrigerante.Value)
                s.Append("Líquido refrigerante, ");
            if (!this.vista.tieneReductorEngranes.Value)
                s.Append("Reductor de engranes, ");
            if (!this.vista.tieneVibrador.Value)
                s.Append("Vibrador, ");
            if (!this.vista.tieneCajaReduccionEngranes.Value)
                s.Append("Caja de reducción de engranes, ");
            if (!this.vista.tieneBateria.Value)
                s.Append("Batería , ");

            #endregion

            #region Lubricación
            if (!vista.tienePivoteArticulacionDireccion.Value)
                s.Append("Pivotes de la articulación y dirección, ");
            if (!this.vista.tieneCabinaOperador.Value)
                s.Append("Cabina del operador, ");
            if (!this.vista.tieneCofreMotor.Value)
                s.Append("Cofre del motor, ");


            #endregion

            #region Funciones eléctricas
            if (!vista.tieneLucesTrabajoDelantera.Value)
                s.Append("Luces de trabajo delantera, ");
            if (!this.vista.tieneLucesTrabajoTraseras.Value)
                s.Append("Luces de trabajo traseras, ");
            if (!this.vista.tieneLamparaTablero.Value)
                s.Append("Lámparas del tablero, ");
            if (!this.vista.tieneInterruptorDesconexion.Value)
                s.Append("Interruptor de desconexión, ");
            if (!this.vista.tieneAlarmaReversa.Value)
                s.Append("Alarma de reversa, ");

            #endregion

            #region Controles
            if (!vista.tienePalanca.Value)
                s.Append("Palanca, ");
            if (!this.vista.tieneLucesAdvertencia.Value)
                s.Append("Luces de advertencia, ");
            if (!this.vista.tieneIndicadores.Value)
                s.Append("Indicadores, ");
            if (!this.vista.tieneTacometro.Value)
                s.Append("Tacómetro, ");
            if (!this.vista.tieneFrenoEstacionamiento.Value)
                s.Append("Freno de estacionamiento, ");
            if (!this.vista.tieneSistemaVibracion.Value)
                s.Append("Sistema de vibración, ");
            if (!this.vista.tieneVelocidadMinima.Value)
                s.Append("Velocidad mínima del motor, ");
            if (!this.vista.tieneVelocidadMaxima.Value)
                s.Append("Velocidad máxima del motor, ");

            #endregion

            #region Misceláneos
            if (!vista.tieneTapaCombustible.Value)
                s.Append("Tapa de combustible, ");
            if (!this.vista.tieneTapaHidraulico.Value)
                s.Append("Tapa del hidráulico, ");
            if (!this.vista.tieneCondicionAsiento.Value)
                s.Append("Condición del asiento, ");
            if (!this.vista.tieneCondicionLlanta.Value)
                s.Append("Condición de llantas, ");
            if (!this.vista.tieneCondicionPintura.Value)
                s.Append("Condición de pintura, ");
            if (!this.vista.tieneCondicionCalcas.Value)
                s.Append("Condición de calcas, ");
            if (!this.vista.tieneSimbolosSeguridad.Value)
                s.Append("Símbolos de seguridad de la máquina, ");
            if (!this.vista.tieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis, ");
            if (!this.vista.tieneAntenaMonitoreo.Value)
                s.Append("Antenas monitoreo satelital, ");

            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }
        #endregion

        public object InterfazUsuarioADato() {
            ListadoVerificacionVibroCompactadorBO bo = new ListadoVerificacionVibroCompactadorBO();

            #region General (Campos booleanos)
            bo.TienePresionLLantas = this.vista.tienePresionLlantas;
            bo.TieneBandaVentilador = this.vista.tieneBandaVentilador;
            bo.TieneManguerasAbrazaderas = this.vista.tieneManguerasAbrazaderas;
            bo.TieneCartuchoFiltroAire = this.vista.tieneCartuchoFiltroAire;
            bo.TieneRascadoresTambor = this.vista.tieneRascadoresTambor;
            bo.TieneCinturonSeguridad = this.vista.tieneCinturonSeguridad;

            #endregion

            #region Niveles Flujos (Campos booleanos)
            bo.TieneCombustible = this.vista.tieneCombustible;
            bo.TieneAceiteMotor = this.vista.tieneAceiteMotor;
            bo.TieneLiquidoRefrigerante = this.vista.tieneLiquidoRefrigerante;
            bo.TieneAceiteHidraulico = this.vista.tieneAceiteHidraulico;
            bo.TieneReductorEngranes = this.vista.tieneReductorEngranes;
            bo.TieneVibrador = this.vista.tieneVibrador;
            bo.TieneCajaReduccionEngranes = this.vista.tieneCajaReduccionEngranes;
            bo.TieneBateria = this.vista.tieneBateria;
            #endregion

            #region Lubricación (Campos booleanos)
            bo.TienePivoteArticulacionDireccion = this.vista.tienePivoteArticulacionDireccion;
            bo.TieneCabinaOperador = this.vista.tieneCabinaOperador;
            bo.TieneCofreMotor = this.vista.tieneCofreMotor;
            #endregion

            #region Funciones Electricas (Campos numericos)
            bo.TieneLucesTrabajoDelantera = this.vista.tieneLucesTrabajoDelantera;
            bo.TieneLucesTrabajoTraseras = this.vista.tieneLucesTrabajoTraseras;
            bo.TieneLamparaTablero = this.vista.tieneLamparaTablero;
            bo.TieneInterruptorDesconexion = this.vista.tieneInterruptorDesconexion;
            bo.TieneAlarmaReversa = this.vista.tieneAlarmaReversa;

            #endregion

            #region Controles (Campos booleanos)
            bo.TienePalanca = this.vista.tienePalanca;
            bo.TieneLucesAdvertencia = this.vista.tieneLucesAdvertencia;
            bo.TieneIndicadores = this.vista.tieneIndicadores;
            bo.TieneTacometro = this.vista.tieneTacometro;
            bo.TieneFrenoEstacionamiento = this.vista.tieneFrenoEstacionamiento;
            bo.TieneSistemaVibracion = this.vista.tieneSistemaVibracion;
            bo.TieneVelocidadMinima = this.vista.tieneVelocidadMinima;
            bo.TieneVelocidadMaxima = this.vista.tieneVelocidadMaxima;
            #endregion

            #region Miscelaneo (Campos numericos)
            bo.TieneTapaCombustible = this.vista.tieneTapaCombustible;
            bo.TieneTapaHidraulico = this.vista.tieneTapaHidraulico;
            bo.TieneCondicionAsiento = this.vista.tieneCondicionAsiento;
            bo.TieneCondicionLlantas = this.vista.tieneCondicionLlanta;
            bo.TieneCondicionPintura = this.vista.tieneCondicionPintura;
            bo.TieneCondicionCalcas = this.vista.tieneCondicionCalcas;
            bo.TieneSimbolosSeguridad = this.vista.tieneSimbolosSeguridad;
            bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis;
            bo.TieneAntenaMonitoreo = this.vista.tieneAntenaMonitoreo;
            #endregion

            return bo;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionVibroCompactadorBO bo) {

            #region GENERAL (Campos booleanos)
            this.vista.tienePresionLlantas = bo.TienePresionLLantas.HasValue ? bo.TienePresionLLantas : null;
            this.vista.tieneBandaVentilador = bo.TieneBandaVentilador.HasValue ? bo.TieneBandaVentilador : null;
            this.vista.tieneManguerasAbrazaderas = bo.TieneManguerasAbrazaderas.HasValue ? bo.TieneManguerasAbrazaderas : null;
            this.vista.tieneCartuchoFiltroAire = bo.TieneCartuchoFiltroAire.HasValue ? bo.TieneCartuchoFiltroAire : null;
            this.vista.tieneRascadoresTambor = bo.TieneRascadoresTambor.HasValue ? bo.TieneRascadoresTambor : null;
            this.vista.tieneCinturonSeguridad = bo.TieneCinturonSeguridad.HasValue ? bo.TieneCinturonSeguridad : null;

            #endregion
            #region Niveles Flujos (Campos booleanos)
            this.vista.tieneCombustible = bo.TieneCombustible.HasValue ? bo.TieneCombustible : null;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor.HasValue ? bo.TieneAceiteMotor : null;
            this.vista.tieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante.HasValue ? bo.TieneLiquidoRefrigerante : null;
            this.vista.tieneAceiteHidraulico = bo.TieneAceiteHidraulico.HasValue ? bo.TieneAceiteHidraulico : null;
            this.vista.tieneReductorEngranes = bo.TieneReductorEngranes.HasValue ? bo.TieneReductorEngranes : null;
            this.vista.tieneVibrador = bo.TieneVibrador.HasValue ? bo.TieneVibrador : null;
            this.vista.tieneCajaReduccionEngranes = bo.TieneCajaReduccionEngranes.HasValue ? bo.TieneCajaReduccionEngranes : null;
            this.vista.tieneBateria = bo.TieneBateria.HasValue ? bo.TieneBateria : null;
            #endregion

            #region Lubricación (Campos booleanos)
            this.vista.tienePivoteArticulacionDireccion = bo.TienePivoteArticulacionDireccion.HasValue ? bo.TienePivoteArticulacionDireccion : null;
            this.vista.tieneCabinaOperador = bo.TieneCabinaOperador.HasValue ? bo.TieneCabinaOperador : null;
            this.vista.tieneCofreMotor = bo.TieneCofreMotor.HasValue ? bo.TieneCofreMotor : null;
            #endregion

            #region Funciones Electricias (Campos numericos)
            this.vista.tieneLucesTrabajoDelantera = bo.TieneLucesTrabajoDelantera.HasValue ? bo.TieneLucesTrabajoDelantera : null;
            this.vista.tieneLucesTrabajoTraseras = bo.TieneLucesTrabajoTraseras.HasValue ? bo.TieneLucesTrabajoTraseras : null;
            this.vista.tieneLamparaTablero = bo.TieneLamparaTablero.HasValue ? bo.TieneLamparaTablero : null;
            this.vista.tieneInterruptorDesconexion = bo.TieneInterruptorDesconexion.HasValue ? bo.TieneInterruptorDesconexion : null;
            this.vista.tieneAlarmaReversa = bo.TieneAlarmaReversa.HasValue ? bo.TieneAlarmaReversa : null;
            #endregion

            #region Controles (Campos booleanos)
            this.vista.tienePalanca = bo.TienePalanca.HasValue ? bo.TienePalanca : null;
            this.vista.tieneLucesAdvertencia = bo.TieneLucesAdvertencia.HasValue ? bo.TieneLucesAdvertencia : null;
            this.vista.tieneIndicadores = bo.TieneIndicadores.HasValue ? bo.TieneIndicadores : null;
            this.vista.tieneTacometro = bo.TieneTacometro.HasValue ? bo.TieneTacometro : null;
            this.vista.tieneFrenoEstacionamiento = bo.TieneFrenoEstacionamiento.HasValue ? bo.TieneFrenoEstacionamiento : null;
            this.vista.tieneSistemaVibracion = bo.TieneSistemaVibracion.HasValue ? bo.TieneSistemaVibracion : null;
            this.vista.tieneVelocidadMinima = bo.TieneVelocidadMinima.HasValue ? bo.TieneVelocidadMinima : null;
            this.vista.tieneVelocidadMaxima = bo.TieneVelocidadMaxima.HasValue ? bo.TieneVelocidadMaxima : null;

            #endregion

            #region Miscelaneo (Campos numericos)
            this.vista.tieneTapaCombustible = bo.TieneTapaCombustible.HasValue ? bo.TieneTapaCombustible : null;
            this.vista.tieneTapaHidraulico = bo.TieneTapaHidraulico.HasValue ? bo.TieneTapaHidraulico : null;
            this.vista.tieneCondicionAsiento = bo.TieneCondicionAsiento.HasValue ? bo.TieneCondicionAsiento : null;
            this.vista.tieneCondicionLlanta = bo.TieneCondicionLlantas.HasValue ? bo.TieneCondicionLlantas : null;
            this.vista.tieneCondicionPintura = bo.TieneCondicionPintura.HasValue ? bo.TieneCondicionPintura : null;
            this.vista.tieneCondicionCalcas = bo.TieneCondicionCalcas.HasValue ? bo.TieneCondicionCalcas : null;
            this.vista.tieneSimbolosSeguridad = bo.TieneSimbolosSeguridad.HasValue ? bo.TieneSimbolosSeguridad : null;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis.HasValue ? bo.TieneEstructuraChasis : null;
            this.vista.tieneAntenaMonitoreo = bo.TieneAntenaMonitoreo.HasValue ? bo.TieneAntenaMonitoreo : null;
            #endregion

        }
    }
}