using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionPlataformaTijerasPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionPlataformaTijerasPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionPlataformaTijerasPSLUI";
        private ucPuntosVerificacionPlataformaTijerasPSLPRE presentador;
        #endregion
        #region Propiedades
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
        /// Asigna u obtiene el valor del checkbox chbxAlmohadillasDesgasteDeslizantes
        /// </summary>
        public bool? tieneAlmohadillas {
            get {
                return this.chbxAlmohadillasDesgasteDeslizantes.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAlmohadillasDesgasteDeslizantes.Checked = true;
                    else
                        chbxAlmohadillasDesgasteDeslizantes.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBarrasDireccion
        /// </summary>
        public bool? tieneBarrasDireccion {
            get {
                return this.chbxBarrasDireccion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBarrasDireccion.Checked = true;
                    else
                        chbxBarrasDireccion.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxBrazosTijera
        /// </summary>
        public bool? tieneBrazosTijera {
            get {
                return this.chbxBrazosTijera.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBrazosTijera.Checked = true;
                    else
                        chbxBrazosTijera.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxChasis
        /// </summary>
        public bool? tieneChasis {
            get {
                return this.chbxChasis.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxChasis.Checked = true;
                    else
                        chbxChasis.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCilindroDireccion
        /// </summary>
        public bool? tieneCilindroDireccion {
            get {
                return this.chbxCilindroDireccion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCilindroDireccion.Checked = true;
                    else
                        chbxCilindroDireccion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCilindroElevador
        /// </summary>
        public bool? tieneCilindroElevador {
            get {
                return this.chbxCilindroElevador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCilindroElevador.Checked = true;
                    else
                        chbxCilindroElevador.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCombustible
        /// </summary>
        public bool? tieneCombustible {
            get;
            set;
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxConjuntoBarandillas
        /// </summary>
        public bool? tieneConjuntoBarandillas {
            get {
                return this.chbxConjuntoBarandillas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxConjuntoBarandillas.Checked = true;
                    else
                        chbxConjuntoBarandillas.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxConjuntoNeumaticosRuedas
        /// </summary>
        public bool? tieneConjuntoNeumaticosRuedas {
            get {
                return this.chbxConjuntoNeumaticosRuedas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxConjuntoNeumaticosRuedas.Checked = true;
                    else
                        chbxConjuntoNeumaticosRuedas.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxFuncionamientoControlesPlataforma
        /// </summary>
        public bool? tieneControlesPlataforma {
            get {
                return this.chbxFuncionamientoControlesPlataforma.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxFuncionamientoControlesPlataforma.Checked = true;
                    else
                        chbxFuncionamientoControlesPlataforma.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxFuncionamientoControlesTierra
        /// </summary>
        public bool? tieneControlesTierra {
            get {
                return this.chbxFuncionamientoControlesTierra.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxFuncionamientoControlesTierra.Checked = true;
                    else
                        chbxFuncionamientoControlesTierra.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxParosEmergencia
        /// </summary>
        public bool? tieneFarosEmergencia {
            get {
                return this.chbxParosEmergencia.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxParosEmergencia.Checked = true;
                    else
                        chbxParosEmergencia.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPasadoresPivoteTijera
        /// </summary>
        public bool? tienePasadoresPivote {
            get {
                return this.chbxPasadoresPivoteTijera.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPasadoresPivoteTijera.Checked = true;
                    else
                        chbxPasadoresPivoteTijera.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPivotesDireccion
        /// </summary>
        public bool? tienePivotesDireccion {
            get {
                return this.chbxPivotesDireccion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPivotesDireccion.Checked = true;
                    else
                        chbxPivotesDireccion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPlataforma
        /// </summary>
        public bool? tienePlataforma {
            get {
                return this.chbxPlataforma.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPlataforma.Checked = true;
                    else
                        chbxPlataforma.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPruebaSwitchesLimitePothole
        /// </summary>
        public bool? tienePruebaSwitchPothole {
            get {
                return this.chbxPruebaSwitchesLimitePothole.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPruebaSwitchesLimitePothole.Checked = true;
                    else
                        chbxPruebaSwitchesLimitePothole.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxReductoresTransito
        /// </summary>
        public bool? tieneReductoresTransito {
            get {
                return this.chbxReductoresTransito.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxReductoresTransito.Checked = true;
                    else
                        chbxReductoresTransito.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxRefrigerante
        /// </summary>
        public bool? tieneRefrigerante {
            get {
                return this.chbxRefrigerante.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxRefrigerante.Checked = true;
                    else
                        chbxRefrigerante.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadTransitoPlataformaExtendida
        /// </summary>
        public bool? tieneVelocidadTransitoExtendida {
            get {
                return this.chbxVelocidadTransitoPlataformaExtendida.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxVelocidadTransitoPlataformaExtendida.Checked = true;
                    else
                        chbxVelocidadTransitoPlataformaExtendida.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxVelocidadTransitoPlataformaRetraida
        /// </summary>
        public bool? tieneVelocidadTransitoRetraida {
            get {
                return this.chbxVelocidadTransitoPlataformaRetraida.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxVelocidadTransitoPlataformaRetraida.Checked = true;
                    else
                        chbxVelocidadTransitoPlataformaRetraida.Checked = false;

                }
            }
        }
        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionPlataformaTijerasPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion
        #region Métodos
        /// <summary>
        /// Coloca los controles en Modo de Edicion
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edicion</param>
        public void ModoEdicion(bool activo) {
            this.chbxAceiteHidraulico.Enabled = activo;
            this.chbxAceiteMotor.Enabled = activo;
            this.chbxAlmohadillasDesgasteDeslizantes.Enabled = activo;
            this.chbxBarrasDireccion.Enabled = activo;
            this.chbxBateria.Enabled = activo;
            this.chbxBrazosTijera.Enabled = activo;
            this.chbxChasis.Enabled = activo;
            this.chbxCilindroDireccion.Enabled = activo;
            this.chbxCilindroElevador.Enabled = activo;
            this.chbxConjuntoBarandillas.Enabled = activo;
            this.chbxConjuntoNeumaticosRuedas.Enabled = activo;
            this.chbxFuncionamientoControlesPlataforma.Enabled = activo;
            this.chbxFuncionamientoControlesTierra.Enabled = activo;
            this.chbxParosEmergencia.Enabled = activo;
            this.chbxPasadoresPivoteTijera.Enabled = activo;
            this.chbxPivotesDireccion.Enabled = activo;
            this.chbxPlataforma.Enabled = activo;
            this.chbxPruebaSwitchesLimitePothole.Enabled = activo;
            this.chbxReductoresTransito.Enabled = activo;
            this.chbxRefrigerante.Enabled = activo;
            this.chbxVelocidadTransitoPlataformaExtendida.Enabled = activo;
            this.chbxVelocidadTransitoPlataformaRetraida.Enabled = activo;


        }
        /// <summary>
        /// Muestra mensaje en la interfaz del usuario
        /// </summary>
        /// <param name="mensaje">Mensaje que será mostrado</param>
        /// <param name="tipo">El tipo de mensaje que sera mostrado</param>
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