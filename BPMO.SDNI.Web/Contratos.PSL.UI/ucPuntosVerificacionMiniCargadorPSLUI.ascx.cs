using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionMiniCargadorPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionMiniCargadorPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMiniCargadorPSLUI";
        private ucPuntosVerificacionMiniCargadorPSLPRE presentador;
        #endregion
        #region Propiedades
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxAceiteCompartimientoCadenas
        /// </summary>
        public bool? TieneAceiteCompartimientoCadena {
            get {
                return this.chbxAceiteCompartimientoCadenas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAceiteCompartimientoCadenas.Checked = true;
                    else
                        chbxAceiteCompartimientoCadenas.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxAceiteHidraulico
        /// </summary>
        public bool? TieneAceiteHidraulico {
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
        ///  Asigna u obtiene el valor del checkbox chbxAceiteMotor
        /// </summary>
        public bool? TieneAceiteMotor {
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
        ///  Asigna u obtiene el valor del checkbox chbxAntenasMonitoreoSatelital
        /// </summary>
        public bool? TieneAntenaMonitoreo {
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
        ///  Asigna u obtiene el valor del checkbox chbxBandaVentilador
        /// </summary>
        public bool? TieneBandaVentilador {
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
        ///  Asigna u obtiene el valor del checkbox chbxBarraSeguridad
        /// </summary>
        public bool? TieneBarraSeguridad {
            get {
                return this.chbxBarraSeguridad.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBarraSeguridad.Checked = true;
                    else
                        chbxBarraSeguridad.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxBateria
        /// </summary>
        public bool? TieneBateria {
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
        ///  Asigna u obtiene el valor del checkbox chbxCartuchoFiltroAire
        /// </summary>
        public bool? TieneCartuchoFiltro {
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
        ///  Asigna u obtiene el valor del checkbox chbxCombustible
        /// </summary>
        public bool? TieneCombustible {
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
        ///  Asigna u obtiene el valor del checkbox chbxCondicionAsiento
        /// </summary>
        public bool? TieneCondicionAsiento {
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
        ///  Asigna u obtiene el valor del checkbox chbxCondicionCalcas
        /// </summary>
        public bool? TieneCondicionCalcas {
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
        ///  Asigna u obtiene el valor del checkbox chbxCondicionLlantas
        /// </summary>
        public bool? TieneCondicionLlantas {
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
        ///  Asigna u obtiene el valor del checkbox chbxCondicionPintura
        /// </summary>
        public bool? TieneCondicionPintura {
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
        ///  Asigna u obtiene el valor del checkbox chbxCopleMecanicoBote
        /// </summary>
        public bool? TieneCopleMecanico {
            get {
                return this.chbxCopleMecanicoBote.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCopleMecanicoBote.Checked = true;
                    else
                        chbxCopleMecanicoBote.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxEstructuraChasis
        /// </summary>
        public bool? TieneEstructuraChasis {
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
        ///  Asigna u obtiene el valor del checkbox chbxFrenoEstacionamiento
        /// </summary>
        public bool? TieneFrenoEstacionamiento {
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
        ///  Asigna u obtiene el valor del checkbox chbxIndicadores
        /// </summary>
        public bool? TieneIndicadores {
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
        ///  Asigna u obtiene el valor del checkbox chbxInterruptorDesconexion
        /// </summary>
        public bool? TieneInterruptorDesconexion {
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
        ///  Asigna u obtiene el valor del checkbox chbxLamparasTablero
        /// </summary>
        public bool? TieneLamparasTablero {
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
        ///  Asigna u obtiene el valor del checkbox chbxLiquidoRefrigerante
        /// </summary>
        public bool? TieneLiquidoRefrigerante {
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
        ///  Asigna u obtiene el valor del checkbox chbxLucesAdvertencia
        /// </summary>
        public bool? TieneLucesAdvertencia {
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
        ///  Asigna u obtiene el valor del checkbox chbxLucesTrabajoDelanteras
        /// </summary>
        public bool? TieneLucesTrabajoDelantera {
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
        ///  Asigna u obtiene el valor del checkbox chbxLucesTrabajoTraseras
        /// </summary>
        public bool? TieneLucesTrabajoTrasera {
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
        ///  Asigna u obtiene el valor del checkbox chbxManguerasAbrazaderasAdmisionAire
        /// </summary>
        public bool? TieneMaguerasYAbrazaderas {
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
        ///  Asigna u obtiene el valor del checkbox chbxPalancas
        /// </summary>
        public bool? TienePalancas {
            get {
                return this.chbxPalancas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPalancas.Checked = true;
                    else
                        chbxPalancas.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxPasadoresCargador
        /// </summary>
        public bool? TienePasadoresCargador {
            get {
                return this.chbxPasadoresCargador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPasadoresCargador.Checked = true;
                    else
                        chbxPasadoresCargador.Checked = false;

                }
            }
        }
        /// <summary>
        ///  Asigna u obtiene el valor del checkbox chbxPresionLlantas
        /// </summary>
        public bool? TienePresionLlantas {
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
        ///  Asigna u obtiene el valor del checkbox chbxSimbolosSeguridadMaquina
        /// </summary>
        public bool? TieneSimbolosSeguridad {
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
        ///  Asigna u obtiene el valor del checkbox chbxTacometro
        /// </summary>
        public bool? TieneTacometro {
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
        ///  Asigna u obtiene el valor del checkbox chbxTapaCombustible
        /// </summary>
        public bool? TieneTapaCombustible {
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
        ///  Asigna u obtiene el valor del checkbox chbxTapaHidraulico
        /// </summary>
        public bool? TieneTapaHidraulico {
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
        ///  Asigna u obtiene el valor del checkbox chbxVelocidadMaximaMotor
        /// </summary>
        public bool? TieneVelocidadMaxMotor {
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
        ///  Asigna u obtiene el valor del checkbox chbxVelocidadMinimaMotor
        /// </summary>
        public bool? TieneVelocidadMinMotor {
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
        #region Métodos
        /// <summary>
        /// Coloca los controles en Modo de Edición
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edición</param>
        public void ModoEdicion(bool activo) {
            this.chbxAceiteCompartimientoCadenas.Enabled = activo;
            this.chbxAceiteHidraulico.Enabled = activo;
            this.chbxAceiteMotor.Enabled = activo;
            this.chbxAntenasMonitoreoSatelital.Enabled = activo;
            this.chbxBandaVentilador.Enabled = activo;
            this.chbxBarraSeguridad.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxCartuchoFiltroAire.Enabled = activo;

            this.chbxCombustible.Enabled = activo;
            this.chbxCondicionAsiento.Enabled = activo;
            this.chbxCondicionCalcas.Enabled = activo;
            this.chbxCondicionLlantas.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxCopleMecanicoBote.Enabled = activo;
            this.chbxEstructuraChasis.Enabled = activo;
            this.chbxFrenoEstacionamiento.Enabled = activo;

            this.chbxIndicadores.Enabled = activo;
            this.chbxInterruptorDesconexion.Enabled = activo;
            this.chbxLamparasTablero.Enabled = activo;
            this.chbxLiquidoRefrigerante.Enabled = activo;
            this.chbxLucesAdvertencia.Enabled = activo;
            this.chbxLucesTrabajoDelanteras.Enabled = activo;
            this.chbxLucesTrabajoTraseras.Enabled = activo;
            this.chbxManguerasAbrazaderasAdmisionAire.Enabled = activo;

            this.chbxPalancas.Enabled = activo;
            this.chbxPasadoresCargador.Enabled = activo;
            this.chbxPresionLlantas.Enabled = activo;
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
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionMiniCargadorPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion


    }
}