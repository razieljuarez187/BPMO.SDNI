
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionCompresoresPortatilesPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionCompresoresPortatilesPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionCompresoresPortatilesPSLVIS vista;
        internal IucPuntosVerificacionCompresoresPortatilesPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionCompresoresPortatilesPSLPRE(IucPuntosVerificacionCompresoresPortatilesPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionCompresoresPortatilesPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionCompresorPortatilBO bo = (ListadoVerificacionCompresorPortatilBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.tieneAceiteCompresor = null;
            this.vista.tieneAceiteMotor = null;
            this.vista.tieneAntenasMonitoreo = null;
            this.vista.tieneBandaVentilador = null;
            this.vista.tieneBarraTiro = null;
            this.vista.tieneBateria = null;
            this.vista.tieneBtnServicioAire = null;
            this.vista.tieneCartuchoFiltro = null;
            this.vista.tieneCombustible = null;
            this.vista.tieneCondicionCalcas = null;
            this.vista.tieneCondicionLlantas = null;
            this.vista.tieneCondicionPintura = null;
            this.vista.tieneEstructuraChasis = null;
            this.vista.tieneIndicadores = null;
            this.vista.tieneLamparasTablero = null;
            this.vista.tieneLiquidoRefrigerante = null;
            this.vista.tieneLucesTransito = null;
            this.vista.tieneManguerasYAbrazaderas = null;
            this.vista.tieneManometroPresion = null;
            this.vista.tienePresionEnLlantas = null;
            this.vista.tieneSimbolosSeguridad = null;
            this.vista.tieneSwitchArranque = null;
            this.vista.tieneTacometro = null;
            this.vista.tieneTapaCombustible = null;
            this.vista.tieneVelocidadMaxMotor = null;
            this.vista.tieneVelocidadMinMotor = null;
            this.vista.lubricacion = null;
        }
        #endregion

        public object InterfazUsuarioADato() {
            ListadoVerificacionCompresorPortatilBO bo = new ListadoVerificacionCompresorPortatilBO();

            #region General (Campos booleanos)
            if (this.vista.tienePresionEnLlantas != null)
                bo.TienePresionEnLlantas = this.vista.tienePresionEnLlantas.Value;
            if (this.vista.tieneBandaVentilador != null)
                bo.TieneBandaVentilador = this.vista.tieneBandaVentilador.Value;
            if (this.vista.tieneBarraTiro != null)
                bo.TieneBarraTiro = this.vista.tieneBarraTiro.Value;
            if (this.vista.tieneManguerasYAbrazaderas != null)
                bo.TieneManguerasYAbrazaderas = this.vista.tieneManguerasYAbrazaderas.Value;
            if (this.vista.tieneCartuchoFiltro != null)
                bo.TieneCartuchoFiltro = this.vista.tieneCartuchoFiltro.Value;
            if (this.vista.tieneBtnServicioAire != null)
                bo.TieneBtnServicioAire = this.vista.tieneBtnServicioAire.Value;
            #endregion

            #region Niveles Flujos (Campos booleanos)
            if (this.vista.tieneCombustible != null)
                bo.TieneCombustible = this.vista.tieneCombustible.Value;
            if (this.vista.tieneAceiteMotor != null)
                bo.TieneAceiteMotor = this.vista.tieneAceiteMotor.Value;
            if (this.vista.tieneAceiteCompresor != null)
                bo.TieneAceiteCompresor = this.vista.tieneAceiteCompresor.Value;
            if (this.vista.tieneLiquidoRefrigerante != null)
                bo.TieneLiquidoRefrigerante = this.vista.tieneLiquidoRefrigerante.Value;
            if (this.vista.tieneBateria != null)
                bo.TieneBateria = this.vista.tieneBateria.Value;
            #endregion

            #region Lubricación (Campos booleanos)

            #endregion

            #region Funciones Electricias (Campos numericos)
            if (this.vista.tieneLucesTransito != null)
                bo.TieneLucesTransito = this.vista.tieneLucesTransito.Value;
            if (this.vista.tieneSwitchArranque != null)
                bo.TieneSwitchArranque = this.vista.tieneSwitchArranque.Value;
            if (this.vista.tieneIndicadores != null)
                bo.TieneIndicadores = this.vista.tieneIndicadores.Value;
            if (this.vista.tieneLamparasTablero != null)
                bo.TieneLamparasTablero = this.vista.tieneLamparasTablero.Value;
            #endregion

            #region Controles (Campos booleanos)
            if (this.vista.tieneIndicadores != null)
                bo.TieneIndicadores = this.vista.tieneIndicadores.Value;
            if (this.vista.tieneTacometro != null)
                bo.TieneTacometro = this.vista.tieneTacometro.Value;
            if (this.vista.tieneManometroPresion != null)
                bo.TieneManometroPresion = this.vista.tieneManometroPresion.Value;
            if (this.vista.tieneVelocidadMinMotor != null)
                bo.TieneVelocidadMinMotor = this.vista.tieneVelocidadMinMotor.Value;
            if (this.vista.tieneVelocidadMaxMotor != null)
                bo.TieneVelocidadMaxMotor = this.vista.tieneVelocidadMaxMotor.Value;
            #endregion

            #region Miscelaneo (Campos numericos)
            if (this.vista.tieneTapaCombustible != null)
                bo.TieneTapaCombustible = this.vista.tieneTapaCombustible.Value;
            if (this.vista.tieneCondicionLlantas != null)
                bo.TieneCondicionLlantas = this.vista.tieneCondicionLlantas.Value;
            if (this.vista.tieneCondicionPintura != null)
                bo.TieneCondicionPintura = this.vista.tieneCondicionPintura.Value;
            if (this.vista.tieneCondicionCalcas != null)
                bo.TieneCondicionCalcas = this.vista.tieneCondicionCalcas.Value;
            if (this.vista.tieneSimbolosSeguridad != null)
                bo.TieneSimbolosSeguridad = this.vista.tieneSimbolosSeguridad.Value;
            if (this.vista.tieneEstructuraChasis != null)
                bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis.Value;
            if (this.vista.tieneAntenasMonitoreo != null)
                bo.TieneAntenasMonitoreo = this.vista.tieneAntenasMonitoreo.Value;
            if (this.vista.lubricacion != null)
                bo.Lubricacion = this.vista.lubricacion;
            #endregion


            return bo;
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region General
            if (!this.vista.tienePresionEnLlantas.Value)
                s.Append("Presión de llantas, ");
            if (!this.vista.tieneBandaVentilador.Value)
                s.Append("Banda del Ventilador, ");
            if (!this.vista.tieneManguerasYAbrazaderas.Value)
                s.Append("Mangueras y Abrazaderas de Admisión de Aire, ");
            if (!this.vista.tieneCartuchoFiltro.Value)
                s.Append("Cartucho del Filtro de Aire, ");
            if (!this.vista.tieneBarraTiro.Value)
                s.Append("Barra de Tiro, ");
            #endregion

            #region Niveles de fluidos
            if (!this.vista.tieneCombustible.Value)
                s.Append("Combustible, ");
            if (!this.vista.tieneAceiteMotor.Value)
                s.Append("Aceite del Motor, ");
            if (!this.vista.tieneAceiteCompresor.Value)
                s.Append("Aceite del Compresor, ");
            if (!this.vista.tieneLiquidoRefrigerante.Value)
                s.Append("Líquido Refrigerante, ");
            if (!this.vista.tieneBateria.Value)
                s.Append("Batería, ");
            #endregion

            #region Funciones eléctricas
            if (!this.vista.tieneLucesTransito.Value)
                s.Append("Luces de Tránsito, ");
            if (!this.vista.tieneLamparasTablero.Value)
                s.Append("Lámparas del Tablero, ");
            #endregion

            #region Controles
            if (!this.vista.tieneSwitchArranque.Value)
                s.Append("Switch de Arranque, ");
            if (!this.vista.tieneBtnServicioAire.Value)
                s.Append("Botón de Servicio de Aire, ");
            if (!this.vista.tieneIndicadores.Value)
                s.Append("Indicadores, ");
            if (!this.vista.tieneTacometro.Value)
                s.Append("Tacómetro, ");
            if (!this.vista.tieneManometroPresion.Value)
                s.Append("Manómetro de Presión, ");
            if (!this.vista.tieneVelocidadMinMotor.Value)
                s.Append("Velocidad Mínima del Motor, ");
            if (!this.vista.tieneVelocidadMaxMotor.Value)
                s.Append("Velocidad Máxima del Motor, ");
            #endregion

            #region Misceláneos
            if (!this.vista.tieneTapaCombustible.Value)
                s.Append("Tapa de Combustible, ");
            if (!this.vista.tieneCondicionLlantas.Value)
                s.Append("Condición de Llantas, ");
            if (!this.vista.tieneCondicionPintura.Value)
                s.Append("Condición de Pintura, ");
            if (!this.vista.tieneCondicionCalcas.Value)
                s.Append("Condición de Calcas, ");
            if (!this.vista.tieneSimbolosSeguridad.Value)
                s.Append("Símbolos de Seguridad de la Máquina, ");
            if (!this.vista.tieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis, ");
            if (!this.vista.tieneAntenasMonitoreo.Value)
                s.Append("Antenas Monitoreo Satelital, ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionCompresorPortatilBO bo) {
            #region General (Campos booleanos)
            this.vista.tienePresionEnLlantas = bo.TienePresionEnLlantas.HasValue ? bo.TienePresionEnLlantas : false;
            this.vista.tieneBandaVentilador = bo.TieneBandaVentilador.HasValue ? bo.TieneBandaVentilador : false;
            this.vista.tieneBarraTiro = bo.TieneBarraTiro.HasValue ? bo.TieneBarraTiro : false;
            this.vista.tieneManguerasYAbrazaderas = bo.TieneManguerasYAbrazaderas.HasValue ? bo.TieneManguerasYAbrazaderas : false;
            this.vista.tieneCartuchoFiltro = bo.TieneCartuchoFiltro.HasValue ? bo.TieneCartuchoFiltro : false;
            this.vista.tieneBtnServicioAire = bo.TieneBtnServicioAire.HasValue ? bo.TieneBtnServicioAire : false;
            #endregion

            #region Niveles Flujos (Campos booleanos)
            this.vista.tieneCombustible = bo.TieneCombustible.HasValue ? bo.TieneCombustible : false;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor.HasValue ? bo.TieneAceiteMotor : false;
            this.vista.tieneAceiteCompresor = bo.TieneAceiteCompresor.HasValue ? bo.TieneAceiteCompresor : false;
            this.vista.tieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante.HasValue ? bo.TieneLiquidoRefrigerante : false;
            this.vista.tieneBateria = bo.TieneBateria.HasValue ? bo.TieneBateria : false;
            #endregion

            #region Lubricación (Campos booleanos)

            #endregion

            #region Funciones Electricias (Campos numericos)
            this.vista.tieneLucesTransito = bo.TieneLucesTransito.HasValue ? bo.TieneLucesTransito : false;
            this.vista.tieneSwitchArranque = bo.TieneSwitchArranque.HasValue ? bo.TieneSwitchArranque : false;
            this.vista.tieneIndicadores = bo.TieneIndicadores.HasValue ? bo.TieneIndicadores : false;
            this.vista.tieneLamparasTablero = bo.TieneLamparasTablero.HasValue ? bo.TieneLamparasTablero : false;
            #endregion

            #region Controles (Campos booleanos)
            this.vista.tieneTacometro = bo.TieneTacometro.HasValue ? bo.TieneTacometro : false;
            this.vista.tieneManometroPresion = bo.TieneManometroPresion.HasValue ? bo.TieneManometroPresion : false;
            this.vista.tieneVelocidadMinMotor = bo.TieneVelocidadMinMotor.HasValue ? bo.TieneVelocidadMinMotor : false;
            this.vista.tieneVelocidadMaxMotor = bo.TieneVelocidadMaxMotor.HasValue ? bo.TieneVelocidadMaxMotor : false;
            #endregion

            #region Miscelaneo (Campos numericos)
            this.vista.tieneTapaCombustible = bo.TieneTapaCombustible.HasValue ? bo.TieneTapaCombustible : false;
            this.vista.tieneCondicionLlantas = bo.TieneCondicionLlantas.HasValue ? bo.TieneCondicionLlantas : false;
            this.vista.tieneCondicionPintura = bo.TieneCondicionPintura.HasValue ? bo.TieneCondicionPintura : false;
            this.vista.tieneCondicionCalcas = bo.TieneCondicionCalcas.HasValue ? bo.TieneCondicionCalcas : false;
            this.vista.tieneSimbolosSeguridad = bo.TieneSimbolosSeguridad.HasValue ? bo.TieneSimbolosSeguridad : false;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis.HasValue ? bo.TieneEstructuraChasis : false;
            this.vista.tieneAntenasMonitoreo = bo.TieneAntenasMonitoreo.HasValue ? bo.TieneAntenasMonitoreo : false;
            this.vista.lubricacion = !string.IsNullOrEmpty(bo.Lubricacion) ? bo.Lubricacion : string.Empty;
            #endregion
        }
    }
}