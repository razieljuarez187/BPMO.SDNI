using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionMotoNiveladoraPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMotoNiveladoraPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionMotoNiveladoraPSLVIS vista;
        internal IucPuntosVerificacionMotoNiveladoraPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionMotoNiveladoraPSLPRE(IucPuntosVerificacionMotoNiveladoraPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionMotoNiveladoraPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionMotoNiveladoraBO bo = (ListadoVerificacionMotoNiveladoraBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {

            #region Generales
            this.vista.tienePresionEnLlantas = null;
            this.vista.tieneBandaVentilador = null;
            this.vista.tieneManguerasAbrazaderas = null;
            this.vista.tieneCartuchoFiltro = null;
            this.vista.tieneCuchilla = null;
            this.vista.tieneAjusteGiracirculoCuchilla = null;
            this.vista.tieneRipperEscarificador = null;
            this.vista.tieneCinturonSeguridad = null;
            this.vista.tieneEspejosRetrovisores = null;
            #endregion

            #region Niveles de fluidos

            this.vista.tieneCombustible = null;
            this.vista.tieneAceiteMotor = null;
            this.vista.tieneLiquidoRefrigerante = null;
            this.vista.tieneAceiteHidraulico = null;
            this.vista.tieneAceiteTransmision = null;
            this.vista.tieneAceiteMandosFinales = null;
            this.vista.tieneAceiteTandem = null;
            this.vista.tieneAceiteEngranesGiracirculo = null;
            this.vista.tieneBateria = null;

            #endregion

            #region Lubricación
            this.vista.tieneArticulacionesCuchilla = null;
            this.vista.tieneArticulacionesRipper = null;
            this.vista.tieneArticulacionesEscarificador = null;
            this.vista.tieneArticulacionesDireccion = null;
            this.vista.tieneArticulacionesChasis = null;
            #endregion

            #region Funciones eléctricas
            this.vista.tieneLucesTrabajoDelantera = null;
            this.vista.tieneLucesTrabajoTrasera = null;
            this.vista.tieneClaxon = null;
            this.vista.tieneAlarmaReversa = null;
            this.vista.tieneLucesIntermitentes = null;
            this.vista.tieneLucesDireccionales = null;
            #endregion

            #region Controles
            this.vista.tienePalancaTransito = null;
            this.vista.tienePalancaFuncionesHidraulicos = null;
            this.vista.tieneLucesAdvertencia = null;
            this.vista.tieneIndicadores = null;
            this.vista.tieneTacometro = null;
            this.vista.tieneFrenoEstacionamiento = null;
            this.vista.tieneVelocidadMinMotor = null;
            this.vista.tieneVelocidadMaxMotor = null;
            #endregion

            #region Miscelaneos
            this.vista.tieneTapaCombustible = null;
            this.vista.tieneTapaHidraulico = null;
            this.vista.tieneCondicionAsiento = null;
            this.vista.tieneCondicionLlantas = null;
            this.vista.tieneCondicionPintura = null;
            this.vista.tieneCondicionCalcas = null;
            this.vista.tieneSimbolosSeguridad = null;
            this.vista.tieneEstructuraChasis = null;
            this.vista.TieneAntenasMonitoreo = null;
            #endregion

        }

        /// <summary>
        /// Asignación de valores al BO
        /// </summary>
        /// <returns>Retorna un objeto de ListadoVerificacionMotoNiveladoraBO</returns>
        public object InterfazUsuarioADato() {
            ListadoVerificacionMotoNiveladoraBO bo = new ListadoVerificacionMotoNiveladoraBO();

            #region Generales

            if (this.vista.tieneAceiteEngranesGiracirculo.HasValue)
                bo.TieneAceiteEngranesGiracirculo = this.vista.tieneAceiteEngranesGiracirculo.Value;
            if (this.vista.tienePresionEnLlantas.HasValue)
                bo.TienePresionLlantas = this.vista.tienePresionEnLlantas.Value;
            if (this.vista.tieneBandaVentilador.HasValue)
                bo.TieneBandaVentilador = this.vista.tieneBandaVentilador.Value;
            if (this.vista.tieneManguerasAbrazaderas.HasValue)
                bo.TieneMaguerasYAbrazaderas = this.vista.tieneManguerasAbrazaderas.Value;
            if (this.vista.tieneCartuchoFiltro.HasValue)
                bo.TieneCartuchoFiltro = this.vista.tieneCartuchoFiltro.Value;
            if (this.vista.tieneCuchilla.HasValue)
                bo.TieneCuchilla = this.vista.tieneCuchilla.Value;
            if (this.vista.tieneAjusteGiracirculoCuchilla.HasValue)
                bo.TieneAjusteGiracirculoCuchilla = this.vista.tieneAjusteGiracirculoCuchilla.Value;
            if (this.vista.tieneRipperEscarificador.HasValue)
                bo.TieneRipperEscarificador = this.vista.tieneRipperEscarificador.Value;
            if (this.vista.tieneCinturonSeguridad.HasValue)
                bo.TieneCinturonSeguridad = this.vista.tieneCinturonSeguridad.Value;
            if (this.vista.tieneEspejosRetrovisores.HasValue)
                bo.TieneEspejosRetrovisores = this.vista.tieneEspejosRetrovisores.Value;

            #endregion

            #region Niveles de fluidos

            if (this.vista.tieneCombustible.HasValue)
                bo.TieneCombustible = this.vista.tieneCombustible.Value;
            if (this.vista.tieneAceiteMotor.HasValue)
                bo.TieneAceiteMotor = this.vista.tieneAceiteMotor.Value;
            if (this.vista.tieneLiquidoRefrigerante.HasValue)
                bo.TieneLiquidoRefrigerante = this.vista.tieneLiquidoRefrigerante.Value;
            if (this.vista.tieneAceiteHidraulico.HasValue)
                bo.TieneAceiteHidraulico = this.vista.tieneAceiteHidraulico.Value;
            if (this.vista.tieneAceiteTransmision.HasValue)
                bo.TieneAceiteTransmision = this.vista.tieneAceiteTransmision.Value;
            if (this.vista.tieneAceiteDiferencial.HasValue)
                bo.TieneAceiteDiferencial = this.vista.tieneAceiteDiferencial.Value;
            if (this.vista.tieneAceiteMandosFinales.HasValue)
                bo.TieneAceiteMandosFinales = this.vista.tieneAceiteMandosFinales.Value;
            if (this.vista.tieneAceiteTandem.HasValue)
                bo.TieneAceiteTandem = this.vista.tieneAceiteTandem.Value;
            if (this.vista.tieneAceiteEngranesGiracirculo.HasValue)
                bo.TieneAceiteEngranesGiracirculo = this.vista.tieneAceiteEngranesGiracirculo.Value;
            if (this.vista.tieneBateria.HasValue)
                bo.TieneBateria = this.vista.tieneBateria.Value;

            #endregion

            #region Lubricación

            if (this.vista.tieneArticulacionesCuchilla.HasValue)
                bo.TieneArticulacionesCuchilla = this.vista.tieneArticulacionesCuchilla.Value;
            if (this.vista.tieneArticulacionesRipper.HasValue)
                bo.TieneArticulacionesRiper = this.vista.tieneArticulacionesRipper.Value;
            if (this.vista.tieneArticulacionesEscarificador.HasValue)
                bo.TieneArticulacionesEscarificador = this.vista.tieneArticulacionesEscarificador.Value;
            if (this.vista.tieneArticulacionesDireccion.HasValue)
                bo.TieneArticulacionesDireccion = this.vista.tieneArticulacionesDireccion.Value;
            if (this.vista.tieneArticulacionesChasis.HasValue)
                bo.TieneArticulacionesChasis = this.vista.tieneArticulacionesChasis.Value;

            #endregion

            #region Funciones eléctricas

            if (this.vista.tieneLucesTrabajoDelantera.HasValue)
                bo.TieneLucesTrabajoDelantera = this.vista.tieneLucesTrabajoDelantera.Value;
            if (this.vista.tieneLucesTrabajoTrasera.HasValue)
                bo.TieneLucesTrabajoTrasera = this.vista.tieneLucesTrabajoTrasera.Value;
            if (this.vista.tieneClaxon.HasValue)
                bo.TieneClaxon = this.vista.tieneClaxon.Value;
            if (this.vista.tieneAlarmaReversa.HasValue)
                bo.TieneAlarmaReversa = this.vista.tieneAlarmaReversa.Value;
            if (this.vista.tieneLucesIntermitentes.HasValue)
                bo.TieneLucesIntermitentes = this.vista.tieneLucesIntermitentes.Value;
            if (this.vista.tieneLucesDireccionales.HasValue)
                bo.TieneLucesDireccionales = this.vista.tieneLucesDireccionales.Value;

            #endregion

            #region Controles

            if (this.vista.tienePalancaTransito.HasValue)
                bo.TienePalancaTransito = this.vista.tienePalancaTransito.Value;
            if (this.vista.tienePalancaFuncionesHidraulicos.HasValue)
                bo.TienePalancaFuncionesHidraulicos = this.vista.tienePalancaFuncionesHidraulicos.Value;
            if (this.vista.tieneLucesAdvertencia.HasValue)
                bo.TieneLucesAdvertencia = this.vista.tieneLucesAdvertencia.Value;
            if (this.vista.tieneIndicadores.HasValue)
                bo.TieneIndicadores = this.vista.tieneIndicadores.Value;
            if (this.vista.tieneTacometro.HasValue)
                bo.TieneTacometro = this.vista.tieneTacometro.Value;
            if (this.vista.tieneFrenoEstacionamiento.HasValue)
                bo.TieneFrenoEstacionamiento = this.vista.tieneFrenoEstacionamiento.Value;
            if (this.vista.tieneVelocidadMinMotor.HasValue)
                bo.TieneVelocidadMinMotor = this.vista.tieneVelocidadMinMotor.Value;
            if (this.vista.tieneVelocidadMaxMotor.HasValue)
                bo.TieneVelocidadMaxMotor = this.vista.tieneVelocidadMaxMotor.Value;

            #endregion

            #region Miscelaneos

            if (this.vista.tieneTapaCombustible.HasValue)
                bo.TieneTapaCombustible = this.vista.tieneTapaCombustible.Value;
            if (this.vista.tieneTapaHidraulico.HasValue)
                bo.TieneTapaHidraulico = this.vista.tieneTapaHidraulico.Value;
            if (this.vista.tieneCondicionAsiento.HasValue)
                bo.TieneCondicionAsiento = this.vista.tieneCondicionAsiento.Value;
            if (this.vista.tieneCondicionLlantas.HasValue)
                bo.TieneCondicionLlantas = this.vista.tieneCondicionLlantas.Value;
            if (this.vista.tieneCondicionPintura.HasValue)
                bo.TieneCondicionPintura = this.vista.tieneCondicionPintura.Value;
            if (this.vista.tieneCondicionCalcas.HasValue)
                bo.TieneCondicionCalcas = this.vista.tieneCondicionCalcas.Value;
            if (this.vista.tieneSimbolosSeguridad.HasValue)
                bo.TieneSimbolosSeguridad = this.vista.tieneSimbolosSeguridad.Value;
            if (this.vista.tieneEstructuraChasis.HasValue)
                bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis.Value;
            if (this.vista.TieneAntenasMonitoreo.HasValue)
                bo.TieneAntenaMonitoreo = this.vista.TieneAntenasMonitoreo.Value;

            #endregion

            return bo;
        }

        /// <summary>
        /// Asigna los valores del BO a las vistas
        /// </summary>
        /// <param name="bo">Objeto ListadoVerificacionMotoNiveladoraBO</param>
        public void DatoToInterfazUsuario(ListadoVerificacionMotoNiveladoraBO bo) {
            #region Generales
            this.vista.tienePresionEnLlantas = bo.TienePresionLlantas;
            this.vista.tieneBandaVentilador = bo.TieneBandaVentilador;
            this.vista.tieneManguerasAbrazaderas = bo.TieneMaguerasYAbrazaderas;
            this.vista.tieneCartuchoFiltro = bo.TieneCartuchoFiltro;
            this.vista.tieneCuchilla = bo.TieneCuchilla;
            this.vista.tieneAjusteGiracirculoCuchilla = bo.TieneAjusteGiracirculoCuchilla;
            this.vista.tieneRipperEscarificador = bo.TieneRipperEscarificador;
            this.vista.tieneCinturonSeguridad = bo.TieneCinturonSeguridad;
            this.vista.tieneEspejosRetrovisores = bo.TieneEspejosRetrovisores;
            #endregion

            #region Niveles de fluidos

            this.vista.tieneCombustible = bo.TieneCombustible;
            this.vista.tieneAceiteMotor = bo.TieneAceiteMotor;
            this.vista.tieneLiquidoRefrigerante = bo.TieneLiquidoRefrigerante;
            this.vista.tieneAceiteHidraulico = bo.TieneAceiteHidraulico;
            this.vista.tieneAceiteTransmision = bo.TieneAceiteTransmision;
            this.vista.tieneAceiteMandosFinales = bo.TieneAceiteMandosFinales;
            this.vista.tieneAceiteTandem = bo.TieneAceiteTandem;
            this.vista.tieneAceiteEngranesGiracirculo = bo.TieneAceiteEngranesGiracirculo;
            this.vista.tieneBateria = bo.TieneBateria;
            this.vista.tieneAceiteDiferencial = bo.TieneAceiteDiferencial;

            #endregion

            #region Lubricación
            this.vista.tieneArticulacionesCuchilla = bo.TieneArticulacionesCuchilla;
            this.vista.tieneArticulacionesRipper = bo.TieneArticulacionesRiper;
            this.vista.tieneArticulacionesEscarificador = bo.TieneArticulacionesEscarificador;
            this.vista.tieneArticulacionesDireccion = bo.TieneArticulacionesDireccion;
            this.vista.tieneArticulacionesChasis = bo.TieneArticulacionesChasis;
            #endregion

            #region Funciones eléctricas
            this.vista.tieneLucesTrabajoDelantera = bo.TieneLucesTrabajoDelantera;
            this.vista.tieneLucesTrabajoTrasera = bo.TieneLucesTrabajoTrasera;
            this.vista.tieneClaxon = bo.TieneClaxon;
            this.vista.tieneAlarmaReversa = bo.TieneAlarmaReversa;
            this.vista.tieneLucesIntermitentes = bo.TieneLucesIntermitentes;
            this.vista.tieneLucesDireccionales = bo.TieneLucesDireccionales;
            #endregion

            #region Controles
            this.vista.tienePalancaTransito = bo.TienePalancaTransito;
            this.vista.tienePalancaFuncionesHidraulicos = bo.TienePalancaFuncionesHidraulicos;
            this.vista.tieneLucesAdvertencia = bo.TieneLucesAdvertencia;
            this.vista.tieneIndicadores = bo.TieneIndicadores;
            this.vista.tieneTacometro = bo.TieneTacometro;
            this.vista.tieneFrenoEstacionamiento = bo.TieneFrenoEstacionamiento;
            this.vista.tieneVelocidadMinMotor = bo.TieneVelocidadMinMotor;
            this.vista.tieneVelocidadMaxMotor = bo.TieneVelocidadMaxMotor;
            #endregion

            #region Miscelaneos
            this.vista.tieneTapaCombustible = bo.TieneTapaCombustible;
            this.vista.tieneTapaHidraulico = bo.TieneTapaHidraulico;
            this.vista.tieneCondicionAsiento = bo.TieneCondicionAsiento;
            this.vista.tieneCondicionLlantas = bo.TieneCondicionLlantas;
            this.vista.tieneCondicionPintura = bo.TieneCondicionPintura;
            this.vista.tieneCondicionCalcas = bo.TieneCondicionCalcas;
            this.vista.tieneSimbolosSeguridad = bo.TieneSimbolosSeguridad;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis;
            this.vista.TieneAntenasMonitoreo = bo.TieneAntenaMonitoreo;
            #endregion
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            #region Generales
            if (!this.vista.tienePresionEnLlantas.Value)
                s.Append("Presión de llantas, ");
            if (!this.vista.tieneBandaVentilador.Value)
                s.Append("Banda del Ventilador, ");
            if (!this.vista.tieneManguerasAbrazaderas.Value)
                s.Append("Mangueras Abrazaderas de Admisión de Aire, ");
            if (!this.vista.tieneCartuchoFiltro.Value)
                s.Append("Cartucho del Filtro de Aire, ");
            if (!this.vista.tieneCuchilla.Value)
                s.Append("Cuchilla, ");
            if (!this.vista.tieneAjusteGiracirculoCuchilla.Value)
                s.Append("Ajuste del Giracírculo de Cuchilla, ");
            if (!this.vista.tieneRipperEscarificador.Value)
                s.Append("Ripper/Escarificador, ");
            if (!this.vista.tieneCinturonSeguridad.Value)
                s.Append("Cinturón de Seguridad, ");
            if (!this.vista.tieneEspejosRetrovisores.Value)
                s.Append("Espejos Retrovisores, ");
            #endregion

            #region Niveles de fluidos
            if (!this.vista.tieneCombustible.Value)
                s.Append("Combustible, ");
            if (!this.vista.tieneAceiteMotor.Value)
                s.Append("Aceite del Motor, ");
            if (!this.vista.tieneLiquidoRefrigerante.Value)
                s.Append("Líquido Refrigerante, ");
            if (!this.vista.tieneAceiteHidraulico.Value)
                s.Append("Aceite Hidráulico, ");
            if (!this.vista.tieneAceiteTransmision.Value)
                s.Append("Aceite de Transmisión, ");
            if (!this.vista.tieneAceiteDiferencial.Value)
                s.Append("Aceite Diferencial, ");
            if (!this.vista.tieneAceiteMandosFinales.Value)
                s.Append("Aceite Mandos Finales, ");
            if (!this.vista.tieneAceiteTandem.Value)
                s.Append("Aceite Tandem, ");
            if (!this.vista.tieneAceiteEngranesGiracirculo.Value)
                s.Append("Aceite Caja Engranes Giracírculo, ");
            if (!this.vista.tieneBateria.Value)
                s.Append("Batería, ");
            #endregion

            #region Lubricación
            if (!this.vista.tieneArticulacionesCuchilla.Value)
                s.Append("Articulaciones de Cuchilla, ");
            if (!this.vista.tieneArticulacionesRipper.Value)
                s.Append("Articulaciones de Ripper, ");
            if (!this.vista.tieneArticulacionesEscarificador.Value)
                s.Append("Articulaciones de Escarificador, ");
            if (!this.vista.tieneArticulacionesDireccion.Value)
                s.Append("Articulaciones de Dirección, ");
            if (!this.vista.tieneArticulacionesChasis.Value)
                s.Append("Articulaciones de Chasis, ");
            #endregion

            #region Funciones eléctricas
            if (!this.vista.tieneLucesTrabajoDelantera.Value)
                s.Append("Luces de Trabajo Delantera, ");
            if (!this.vista.tieneLucesTrabajoTrasera.Value)
                s.Append("Luces de Trabajo Traseras, ");
            if (!this.vista.tieneClaxon.Value)
                s.Append("Claxon, ");
            if (!this.vista.tieneAlarmaReversa.Value)
                s.Append("Alarma Reversa, ");
            if (!this.vista.tieneLucesIntermitentes.Value)
                s.Append("Luces Intermitentes, ");
            if (!this.vista.tieneLucesDireccionales.Value)
                s.Append("Luces de Direccionales, ");
            #endregion

            #region Controles
            if (!this.vista.tienePalancaTransito.Value)
                s.Append("Palanca de Tránsito, ");
            if (!this.vista.tienePalancaFuncionesHidraulicos.Value)
                s.Append("Palancas de Funciones Hidráulicos, ");
            if (!this.vista.tieneLucesAdvertencia.Value)
                s.Append("Luces de Advertencia, ");
            if (!this.vista.tieneIndicadores.Value)
                s.Append("Indicadores, ");
            if (!this.vista.tieneTacometro.Value)
                s.Append("Tacómetro, ");
            if (!this.vista.tieneFrenoEstacionamiento.Value)
                s.Append("Freno de Estacionamiento, ");
            if (!this.vista.tieneVelocidadMinMotor.Value)
                s.Append("Velocidad Mínima del Motor, ");
            if (!this.vista.tieneVelocidadMaxMotor.Value)
                s.Append("Velocidad Máxima del Motor, ");
            #endregion

            #region Miscelaneos
            if (!this.vista.tieneTapaCombustible.Value)
                s.Append("Tapa de Combustible, ");
            if (!this.vista.tieneTapaHidraulico.Value)
                s.Append("Tapa del Hidráulico, ");
            if (!this.vista.tieneCondicionAsiento.Value)
                s.Append("Condición del Asiento, ");
            if (!this.vista.tieneCondicionLlantas.Value)
                s.Append("Condición de Llantas, ");
            if (!this.vista.tieneCondicionPintura.Value)
                s.Append("Condición de Pintura, ");
            if (!this.vista.tieneCondicionCalcas.Value)
                s.Append("Condición de Calcas, ");
            if (!this.vista.tieneSimbolosSeguridad.Value)
                s.Append("Símbolos de Seguridad de la máquina, ");
            if (!this.vista.tieneEstructuraChasis.Value)
                s.Append("Estructura/Chasis, ");
            if (!this.vista.TieneAntenasMonitoreo.Value)
                s.Append("Antenas de Monitoreo Satelital, ");
            #endregion

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);
            return null;
        }
        #endregion
    }
}