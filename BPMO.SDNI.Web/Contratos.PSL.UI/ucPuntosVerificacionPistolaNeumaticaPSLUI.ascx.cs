using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionPistolaNeumaticaPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionPistolaNeumaticaPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionPistolaNeumaticaPSLUI";
        private ucPuntosVerificacionPistolaNeumaticaPSLPRE presentador;
        #endregion
        #region Propiedades
        /// <summary>
        /// Asigna u obtiene el valor del checkbox chbxAjustePresionCompresor
        /// </summary>
        public bool? TieneAjustePresionCompresor {
            get {
                return this.chbxAjustePresionCompresor.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxAjustePresionCompresor.Checked = true;
                    else
                        chbxAjustePresionCompresor.Checked = false;

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
        /// Asigna u obtiene el valor del checkbox chbxCondicionLubricador
        /// </summary>
        public bool? TieneCondicionLubricador {
            get {
                return this.chbxCondicionLubricador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxCondicionLubricador.Checked = true;
                    else
                        chbxCondicionLubricador.Checked = false;

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
        ///  Asigna u obtiene el valor del checkbox chbxNicelAceiteLubricador
        /// </summary>
        public bool? TieneNivelAceiteLubricador {
            get {
                return this.chbxNivelAceiteLubricador.Checked ? true : false;
            }
            set {
                if (value.HasValue) {
                    if ((bool)value.Value)
                        chbxNivelAceiteLubricador.Checked = true;
                    else
                        chbxNivelAceiteLubricador.Checked = false;

                }
            }
        }

        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {

            try {
                presentador = new ucPuntosVerificacionPistolaNeumaticaPSLPRE(this);
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


            this.chbxAjustePresionCompresor.Enabled = activo;
            this.chbxCondicionBujes.Enabled = activo;
            this.chbxCondicionLubricador.Enabled = activo;
            this.chbxCondicionMangueras.Enabled = activo;
            this.chbxCondicionPica.Enabled = activo;
            this.chbxCondicionPintura.Enabled = activo;
            this.chbxEstructura.Enabled = activo;
            this.chbxNivelAceiteLubricador.Enabled = activo;

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