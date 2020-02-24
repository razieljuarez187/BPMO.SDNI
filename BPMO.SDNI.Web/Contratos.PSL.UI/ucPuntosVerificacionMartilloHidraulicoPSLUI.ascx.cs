using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionMartilloHidraulicoPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionMartilloHidraulicoPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionMartillosHidraulicosPSLUI";
        private ucPuntosVerificacionMartilloHidraulicoPSLPRE presentador;
        #endregion
        #region Propiedades
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCombustible
        /// </summary>
        public bool? TieneCombustible {
            get;
            set;
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionBujes
        /// </summary>
        public bool? TieneCondicionBujes {
            get {
                return this.chbxCondicionBujes.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionBujes.Checked = true;
                    else
                        chbxCondicionBujes.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionCalcas
        /// </summary>
        public bool? TieneCondicionCalcas {
            get {
                return this.chbxCalcas.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCalcas.Checked = true;
                    else
                        chbxCalcas.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionMangueras
        /// </summary>
        public bool? TieneCondicionMangueras {
            get {
                return this.chbxCondicionMangueras.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionMangueras.Checked = true;
                    else
                        chbxCondicionMangueras.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionPasadores
        /// </summary>
        public bool? TieneCondicionPasadores {
            get {
                return this.chbxCondicionPasadores.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionPasadores.Checked = true;
                    else
                        chbxCondicionPasadores.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionPica
        /// </summary>
        public bool? TieneCondicionPica {
            get {
                return this.chbxCondicionPica.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionPica.Checked = true;
                    else
                        chbxCondicionPica.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxCondicionPintura
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
        /// Asigna u obtiene el valor del checkbox chbxEstructura
        /// </summary>
        public bool? TieneEstructura {
            get {
                return this.chbxEstructura.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxEstructura.Checked = true;
                    else
                        chbxEstructura.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxGraseraManual
        /// </summary>
        public bool? TieneGraseraManual {
            get {
                return this.chbxGraseraManual.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxGraseraManual.Checked = true;
                    else
                        chbxGraseraManual.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxSimbolosSeguridadMaquina
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
        /// Asigna u obtiene el valor del checkbox chbxTaponesManguerasAlimentacion
        /// </summary>
        public bool? TieneTaponesMangueras {
            get {
                return this.chbxTaponesManguerasAlimentacion.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTaponesManguerasAlimentacion.Checked = true;
                    else
                        chbxTaponesManguerasAlimentacion.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxTorqueTornillosBaseMartillo
        /// </summary>
        public bool? TieneTorqueTornillos {
            get {
                return this.chbxTorqueTornillosBaseMartillo.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxTorqueTornillosBaseMartillo.Checked = true;
                    else
                        chbxTorqueTornillosBaseMartillo.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxBujes
        /// </summary>
        public bool? TieneBujes {
            get {
                return this.chbxBujes.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxBujes.Checked = true;
                    else
                        chbxBujes.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPasadores
        /// </summary>
        public bool? TienePasadores {
            get {
                return this.chbxPasadores.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPasadores.Checked = true;
                    else
                        chbxPasadores.Checked = false;

                }
            }
        }
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxPica
        /// </summary>
        public bool? TienePica {
            get {
                return this.chbxPica.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxPica.Checked = true;
                    else
                        chbxPica.Checked = false;

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
            this.chbxBujes.Enabled = activo;
            this.chbxCalcas.Enabled = activo;
            this.chbxCondicionBujes.Enabled = activo;
            this.chbxCondicionMangueras.Enabled = activo;
            this.chbxCondicionPasadores.Enabled = activo;
            this.chbxCondicionPica.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxEstructura.Enabled = activo;

            this.chbxGraseraManual.Enabled = activo;
            this.chbxPasadores.Enabled = activo;
            this.chbxPica.Enabled = activo;
            this.chbxSimbolosSeguridadMaquina.Enabled = activo;
            this.chbxTaponesManguerasAlimentacion.Enabled = activo;
            this.chbxTorqueTornillosBaseMartillo.Enabled = activo;
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
                presentador = new ucPuntosVerificacionMartilloHidraulicoPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion
    }
}