using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionMotoNiveladoraPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionMotoNiveladoraPSLVIS {

        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMotoNiveladoraPSLUI";
        private ucPuntosVerificacionMotoNiveladoraPSLPRE presentador;


        private IucPuntosVerificacionMotoNiveladoraPSLVIS vista;
        internal IucPuntosVerificacionMotoNiveladoraPSLVIS Vista { get { return vista; } }
        #endregion
        #region Propiedades
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteDiferencial
        /// </summary>
        public bool? tieneAceiteDiferencial {
            get {
                return this.chbxAceiteDiferencial.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteDiferencial.Checked = true;
                    else
                        chbxAceiteDiferencial.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteCajaEngranesGiracirculo
        /// </summary>
        public bool? tieneAceiteEngranesGiracirculo {
            get {
                return this.chbxAceiteCajaEngranesGiracirculo.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteCajaEngranesGiracirculo.Checked = true;
                    else
                        chbxAceiteCajaEngranesGiracirculo.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxAceiteMandosFinales
        /// </summary>
        public bool? tieneAceiteMandosFinales {
            get {
                return this.chbxAceiteMandosFinales.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteMandosFinales.Checked = true;
                    else
                        chbxAceiteMandosFinales.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxAceiteTandem
        /// </summary>
        public bool? tieneAceiteTandem {
            get {
                return this.chbxAceiteTandem.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteTandem.Checked = true;
                    else
                        chbxAceiteTandem.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAceiteTransmision
        /// </summary>
        public bool? tieneAceiteTransmision {
            get {
                return this.chbxAceiteTransmision.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteTransmision.Checked = true;
                    else
                        chbxAceiteTransmision.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAjusteGiracirculoCuchilla
        /// </summary>
        public bool? tieneAjusteGiracirculoCuchilla {
            get {
                return this.chbxAjusteGiracirculoCuchilla.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAjusteGiracirculoCuchilla.Checked = true;
                    else
                        chbxAjusteGiracirculoCuchilla.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxAntenasMonitoreoSatelital
        /// </summary>
        public bool? TieneAntenasMonitoreo {
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
        /// Asigna u obtiene el valor del checkbox chbxArticulacionesChasis
        /// </summary>
        public bool? tieneArticulacionesChasis {
            get {
                return this.chbxArticulacionesChasis.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxArticulacionesChasis.Checked = true;
                    else
                        chbxArticulacionesChasis.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxArticulacionesCuchilla
        /// </summary>
        public bool? tieneArticulacionesCuchilla {
            get {
                return this.chbxArticulacionesCuchilla.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxArticulacionesCuchilla.Checked = true;
                    else
                        chbxArticulacionesCuchilla.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxArticulacionesDireccion
        /// </summary>
        public bool? tieneArticulacionesDireccion {
            get {
                return this.chbxArticulacionesDireccion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxArticulacionesDireccion.Checked = true;
                    else
                        chbxArticulacionesDireccion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxArticulacionesEscarificador
        /// </summary>
        public bool? tieneArticulacionesEscarificador {
            get {
                return this.chbxArticulacionesEscarificador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxArticulacionesEscarificador.Checked = true;
                    else
                        chbxArticulacionesEscarificador.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxArticulacionesRipper
        /// </summary>
        public bool? tieneArticulacionesRipper {
            get {
                return this.chbxArticulacionesRipper.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxArticulacionesRipper.Checked = true;
                    else
                        chbxArticulacionesRipper.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBandaVentilador
        /// </summary>
        public bool? tieneBandaVentilador {
            get {
                return this.chbxBandaVentilador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBandaVentilador.Checked = true;
                    else
                        chbxBandaVentilador.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxCartuchoFiltroAire
        /// </summary>
        public bool? tieneCartuchoFiltro {
            get {
                return this.chbxCartuchoFiltroAire.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCartuchoFiltroAire.Checked = true;
                    else
                        chbxCartuchoFiltroAire.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCinturonSeguridad
        /// </summary>
        public bool? tieneCinturonSeguridad {
            get {
                return this.chbxCinturonSeguridad.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCinturonSeguridad.Checked = true;
                    else
                        chbxCinturonSeguridad.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxClaxon
        /// </summary>
        public bool? tieneClaxon {
            get {
                return this.chbxClaxon.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxClaxon.Checked = true;
                    else
                        chbxClaxon.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxCondicionLlantas
        /// </summary>
        public bool? tieneCondicionLlantas {
            get {
                return this.chbxCondicionLlantas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionLlantas.Checked = true;
                    else
                        chbxCondicionLlantas.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxCuchilla
        /// </summary>
        public bool? tieneCuchilla {
            get {
                return this.chbxCuchilla.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCuchilla.Checked = true;
                    else
                        chbxCuchilla.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxFrenoEstacionamiento
        /// </summary>
        public bool? tieneFrenoEstacionamiento {
            get {
                return this.chbxFrenoEstacionamiento.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxFrenoEstacionamiento.Checked = true;
                    else
                        chbxFrenoEstacionamiento.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxLucesDireccionales
        /// </summary>
        public bool? tieneLucesDireccionales {
            get {
                return this.chbxLucesDireccionales.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesDireccionales.Checked = true;
                    else
                        chbxLucesDireccionales.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesIntermitentes
        /// </summary>
        public bool? tieneLucesIntermitentes {
            get {
                return this.chbxLucesIntermitentes.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesIntermitentes.Checked = true;
                    else
                        chbxLucesIntermitentes.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajoDelanteras
        /// </summary>
        public bool? tieneLucesTrabajoDelantera {
            get {
                return this.chbxLucesTrabajoDelanteras.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesTrabajoDelanteras.Checked = true;
                    else
                        chbxLucesTrabajoDelanteras.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxLucesTrabajoTraseras
        /// </summary>
        public bool? tieneLucesTrabajoTrasera {
            get {
                return this.chbxLucesTrabajoTraseras.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxLucesTrabajoTraseras.Checked = true;
                    else
                        chbxLucesTrabajoTraseras.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxManguerasAbrazaderasAdmisionAire
        /// </summary>
        public bool? tieneManguerasAbrazaderas {
            get {
                return this.chbxManguerasAbrazaderasAdmisionAire.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxManguerasAbrazaderasAdmisionAire.Checked = true;
                    else
                        chbxManguerasAbrazaderasAdmisionAire.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPalancasFuncionesHidraulicos
        /// </summary>
        public bool? tienePalancaFuncionesHidraulicos {
            get {
                return this.chbxPalancasFuncionesHidraulicos.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPalancasFuncionesHidraulicos.Checked = true;
                    else
                        chbxPalancasFuncionesHidraulicos.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPalancaTransito
        /// </summary>
        public bool? tienePalancaTransito {
            get {
                return this.chbxPalancaTransito.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPalancaTransito.Checked = true;
                    else
                        chbxPalancaTransito.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPresionLlantas
        /// </summary>
        public bool? tienePresionEnLlantas {
            get {
                return this.chbxPresionLlantas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPresionLlantas.Checked = true;
                    else
                        chbxPresionLlantas.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxRipperEscarificador
        /// </summary>
        public bool? tieneRipperEscarificador {
            get {
                return this.chbxRipperEscarificador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxRipperEscarificador.Checked = true;
                    else
                        chbxRipperEscarificador.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxSimbolosSeguridadMaquina
        /// </summary>
        public bool? tieneSimbolosSeguridad {
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
        /// Asigna u obtiene el valor del checkbox chbxTacometro
        /// </summary>
        public bool? tieneTacometro {
            get {
                return this.chbxTacometro.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTacometro.Checked = true;
                    else
                        chbxTacometro.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxTapaHidraulico
        /// </summary>
        public bool? tieneTapaHidraulico {
            get {
                return this.chbxTapaHidraulico.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTapaHidraulico.Checked = true;
                    else
                        chbxTapaHidraulico.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMaximaMotor
        /// </summary>
        public bool? tieneVelocidadMaxMotor {
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
        /// Asigna u obtiene el valor del checkbox chbxVelocidadMinimaMotor
        /// </summary>
        public bool? tieneVelocidadMinMotor {
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
        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {

                presentador = new ucPuntosVerificacionMotoNiveladoraPSLPRE(this);
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
            this.chbxAceiteCajaEngranesGiracirculo.Enabled = activo;
            this.chbxAceiteDiferencial.Enabled = activo;
            this.chbxAceiteHidraulico.Enabled = activo;
            this.chbxAceiteMandosFinales.Enabled = activo;
            this.chbxAceiteMotor.Enabled = activo;
            this.chbxAceiteTandem.Enabled = activo;
            this.chbxAceiteTransmision.Enabled = activo;
            this.chbxAjusteGiracirculoCuchilla.Enabled = activo;
            this.chbxAlarmaReversa.Enabled = activo;
            this.chbxAntenasMonitoreoSatelital.Enabled = activo;
            this.chbxArticulacionesChasis.Enabled = activo;
            this.chbxArticulacionesCuchilla.Enabled = activo;
            this.chbxArticulacionesDireccion.Enabled = activo;
            this.chbxArticulacionesEscarificador.Enabled = activo;
            this.chbxArticulacionesRipper.Enabled = activo;
            this.chbxBandaVentilador.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxCartuchoFiltroAire.Enabled = activo;
            this.chbxCinturonSeguridad.Enabled = activo;
            this.chbxClaxon.Enabled = activo;
            this.chbxCombustible.Enabled = activo;
            this.chbxCondicionAsiento.Enabled = activo;
            this.chbxCondicionCalcas.Enabled = activo;
            this.chbxCondicionLlantas.Enabled = activo;

            this.chbxCondicionPintura.Enabled = activo;
            this.chbxCuchilla.Enabled = activo;
            this.chbxEspejosRetrovisores.Enabled = activo;
            this.chbxEstructuraChasis.Enabled = activo;
            this.chbxFrenoEstacionamiento.Enabled = activo;
            this.chbxIndicadores.Enabled = activo;
            this.chbxLiquidoRefrigerante.Enabled = activo;
            this.chbxLucesAdvertencia.Enabled = activo;

            this.chbxLucesDireccionales.Enabled = activo;
            this.chbxLucesIntermitentes.Enabled = activo;
            this.chbxLucesTrabajoDelanteras.Enabled = activo;
            this.chbxLucesTrabajoTraseras.Enabled = activo;
            this.chbxManguerasAbrazaderasAdmisionAire.Enabled = activo;
            this.chbxPalancasFuncionesHidraulicos.Enabled = activo;
            this.chbxPalancaTransito.Enabled = activo;
            this.chbxPresionLlantas.Enabled = activo;

            this.chbxRipperEscarificador.Enabled = activo;
            this.chbxSimbolosSeguridadMaquina.Enabled = activo;
            this.chbxTacometro.Enabled = activo;
            this.chbxTapaCombustible.Enabled = activo;
            this.chbxTapaHidraulico.Enabled = activo;
            this.chbxVelocidadMaximaMotor.Enabled = activo;
            this.chbxVelocidadMinimaMotor.Enabled = activo;

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