using System;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionEntregaRecepcionPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionEntregaRecepcionPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionEntregaRecepcionPSLUI";
        private ucPuntosVerificacionEntregaRecepcionPSLPRE presentador;
        #endregion

        #region Propiedades
        #region Existencia (Campos booleanos)

        /// <summary>
        /// Verifica si tiene bandas
        /// </summary>
        public bool? TieneBandas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneBandas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneBandas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene filtro de aceite
        /// </summary>
        public bool? TieneFiltroAceite {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneFiltroAceite.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneFiltroAceite.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene filtro de agua
        /// </summary>
        public bool? TieneFiltroAgua {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneFiltroAgua.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneFiltroAgua.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene filtro de combustible
        /// </summary>
        public bool? TieneFiltroCombustible {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneFiltroCombustible.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneFiltroCombustible.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene filtro de aire
        /// </summary>
        public bool? TieneFiltroAire {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneFiltroAire.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneFiltroAire.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        private bool? tieneMangueras;
        /// <summary>
        /// Verifica si tiene mangueras
        /// </summary>
        public bool? TieneMangueras {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneMangueras.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneMangueras.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        #endregion

        #region Medidores (Campos booleanos)

        /// <summary>
        /// Verifica si tiene amperímetro
        /// </summary>
        public bool? TieneAmperimetro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneAmperimetro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneAmperimetro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene voltímetro
        /// </summary>
        public bool? TieneVoltimetro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneVoltimetro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneVoltimetro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene horómetro
        /// </summary>
        public bool? TieneHorometro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneHorometro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneHorometro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene manómetro
        /// </summary>
        public bool? TieneManometro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneManometro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneManometro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene interruptor
        /// </summary>
        public bool? TieneInterruptor {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneInterruptor.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneInterruptor.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        #endregion

        #region Motor (Campos booleanos)

        /// <summary>
        /// Verifica si tiene el nivel de aceite
        /// </summary>
        public bool? TieneNivelAceite {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneNivelAceite.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneNivelAceite.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene el nivel de anticongelante
        /// </summary>
        public bool? TieneNivelAnticongelante {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneNivelAnticongelante.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneNivelAnticongelante.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        #endregion

        #region Voltaje (Campos numericos)

        /// <summary>
        /// Indica el voltaje L1N
        /// </summary>
        public decimal? VoltajeL1N {
            get {
                if (!string.IsNullOrEmpty(this.txtVoltajeL1.Text) && !string.IsNullOrWhiteSpace(this.txtVoltajeL1.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtVoltajeL1.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtVoltajeL1.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Indica el voltaje L2N
        /// </summary>
        public decimal? VoltajeL2N {
            get {
                if (!string.IsNullOrEmpty(this.txtVoltajeL2.Text) && !string.IsNullOrWhiteSpace(this.txtVoltajeL2.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtVoltajeL2.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtVoltajeL2.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Indica el voltaje L3N
        /// </summary>
        public decimal? VoltajeL3N {
            get {
                if (!string.IsNullOrEmpty(this.txtVoltajeL3.Text) && !string.IsNullOrWhiteSpace(this.txtVoltajeL3.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtVoltajeL3.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtVoltajeL3.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Indica el voltaje L1L2
        /// </summary>
        public decimal? VoltajeL1L2 {
            get {
                if (!string.IsNullOrEmpty(this.txtVoltajeL1L2.Text) && !string.IsNullOrWhiteSpace(this.txtVoltajeL1L2.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtVoltajeL1L2.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtVoltajeL1L2.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Indica el voltaje L2L3
        /// </summary>
        public decimal? VoltajeL2L3 {
            get {
                if (!string.IsNullOrEmpty(this.txtVoltajeL2L3.Text) && !string.IsNullOrWhiteSpace(this.txtVoltajeL2L3.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtVoltajeL2L3.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtVoltajeL2L3.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Indica el voltaje L3L1
        /// </summary>
        public decimal? VoltajeL3L1 {
            get {
                if (!string.IsNullOrEmpty(this.txtVoltajeL3L1.Text) && !string.IsNullOrWhiteSpace(this.txtVoltajeL3L1.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtVoltajeL3L1.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtVoltajeL3L1.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        #endregion

        #region Accesorios (Campos booleanos)

        /// <summary>
        /// Verifica si tiene Cables
        /// </summary>
        public bool? TieneCables {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneCables.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneCables.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene tramos
        /// </summary>
        public bool? TieneTramos {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneTramos.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneTramos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene líneas
        /// </summary>
        public bool? TieneLineas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneLineas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneLineas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene calibres
        /// </summary>
        public bool? TieneCalibres {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneCalibres.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneCalibres.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene zapatas
        /// </summary>
        public bool? TieneZapatas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneZapatas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneZapatas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        #endregion

        #region Bateria (Campos numericos)

        /// <summary>
        /// Obtiene o agrega batería cantidad
        /// </summary>
        public decimal? BateriaCantidad {
            get {
                if (!string.IsNullOrEmpty(this.txtBateriaCantidad.Text) && !string.IsNullOrWhiteSpace(this.txtBateriaCantidad.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtBateriaCantidad.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtBateriaCantidad.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o agrega batería marca
        /// </summary>
        public string BateriaMarca {
            get {
                if (!string.IsNullOrEmpty(this.txtBateriaMarca.Text) && !string.IsNullOrWhiteSpace(this.txtBateriaMarca.Text)) {
                    return this.txtBateriaMarca.Text.ToUpper();
                }
                return null;
            }
            set { this.txtBateriaMarca.Text = !string.IsNullOrEmpty( value) ? value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o agrega batería placas
        /// </summary>
        public decimal? BateriaPlacas {
            get {
                if (!string.IsNullOrEmpty(this.txtBateriaPlacas.Text) && !string.IsNullOrWhiteSpace(this.txtBateriaPlacas.Text)) {
                    decimal val;
                    return Decimal.TryParse(this.txtBateriaPlacas.Text, out val) ? (decimal?)val : null;
                }
                return null;
            }
            set { this.txtBateriaPlacas.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        #endregion

        #region Datos Remolque (Campos alfanumericos)

        /// <summary>
        /// Datos suspensión
        /// </summary>
        public string Suspension {
            get { return this.txtSuspension.Text.Trim().ToUpper(); }
            set {
                this.txtSuspension.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        /// <summary>
        /// Datos gancho
        /// </summary>
        public string Gancho {
            get { return this.txtGancho.Text.Trim().ToUpper(); }
            set {
                this.txtGancho.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        /// <summary>
        ///Datos gato nivelación
        /// </summary>
        public string GatoNivelacion {
            get { return this.txtGatoNivelacion.Text.Trim().ToUpper(); }
            set {
                this.txtGatoNivelacion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        /// <summary>
        /// Verifica si tiene arnés de conexión
        /// </summary>
        public string ArnesConexion {
            get { return this.txtArnesConexion.Text.Trim().ToUpper(); }
            set {
                this.txtArnesConexion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        #endregion

        #region Llantas (Campos booleanos)

        /// <summary>
        /// Verifica si tiene el eje 1 llanta d
        /// </summary>
        public bool? TieneEje1LlantaD {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneEje1LlantaD.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneEje1LlantaD.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene eje 2 llanta d
        /// </summary>
        public bool? TieneEje2LlantaD {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneEje2LlantaD.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneEje2LlantaD.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene eje 2 llanta d
        /// </summary>
        public bool? TieneEje3LlantaD {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneEje3LlantaD.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneEje3LlantaD.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene eje 1 llanta I
        /// </summary>
        public bool? TieneEje1LlantaI {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneEje1LlantaI.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneEje1LlantaI.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene eje 2 llanta I
        /// </summary>
        public bool? TieneEje2LlantaI {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneEje2LlantaI.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneEje2LlantaI.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene eje 3 llanta I
        /// </summary>
        public bool? TieneEje3LlantaI {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneEje3LlantaI.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneEje3LlantaI.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene tapa lluvia llanta D
        /// </summary>
        public bool? TieneTapaLluviaLlantaD {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneTapaLluviaLlantaD.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneTapaLluviaLlantaD.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene tapa de lluvia Llanta I
        /// </summary>
        public bool? TieneTapaLluviaLlantaI {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneTapaLluviaLlantaI.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneTapaLluviaLlantaI.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        #endregion

        #region Lamparas (Campos booleanos)

        /// <summary>
        /// Verifica si tiene lampara derecha
        /// </summary>
        public bool? TieneLamparaDerecha {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneLamparaDerecha.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneLamparaDerecha.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene lampara izquierda
        /// </summary>
        public bool? TieneLamparaIzquierda {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneLamparaIzquierda.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneLamparaIzquierda.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene señal satelital
        /// </summary>
        public bool? TieneSenalSatelital {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneSenalSatelital.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneSenalSatelital.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        /// <summary>
        /// Verifica si tiene diodos
        /// </summary>
        public bool? TieneDiodos {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTieneDiodos.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTieneDiodos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucPuntosVerificacionEntregaRecepcionPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Coloca los controles en Modo de Edición
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edición</param>
        public void ModoEdicion(bool activo) {
            #region Existencia (Campos booleanos)

            this.rbtTieneBandas.Enabled = activo;
            this.rbtTieneFiltroAceite.Enabled = activo;
            this.rbtTieneFiltroAgua.Enabled = activo;
            this.rbtTieneFiltroCombustible.Enabled = activo;
            this.rbtTieneFiltroAire.Enabled = activo;
            this.rbtTieneMangueras.Enabled = activo;

            #endregion

            #region Medidores (Campos booleanos)

            this.rbtTieneAmperimetro.Enabled = activo;
            this.rbtTieneVoltimetro.Enabled = activo;
            this.rbtTieneHorometro.Enabled = activo;
            this.rbtTieneManometro.Enabled = activo;
            this.rbtTieneInterruptor.Enabled = activo;

            #endregion

            #region Motor (Campos booleanos)

            this.rbtTieneNivelAceite.Enabled = activo;
            this.rbtTieneNivelAnticongelante.Enabled = activo;

            #endregion

            #region Voltaje (Campos numericos)

            this.txtVoltajeL1.Enabled = activo;
            this.txtVoltajeL2.Enabled = activo;
            this.txtVoltajeL3.Enabled = activo;
            this.txtVoltajeL1L2.Enabled = activo;
            this.txtVoltajeL2L3.Enabled = activo;
            this.txtVoltajeL3L1.Enabled = activo;

            #endregion

            #region Accesorios (Campos booleanos)

            this.rbtTieneCables.Enabled = activo;
            this.rbtTieneTramos.Enabled = activo;
            this.rbtTieneLineas.Enabled = activo;
            this.rbtTieneCalibres.Enabled = activo;
            this.rbtTieneZapatas.Enabled = activo;
            #endregion

            #region Bateria (Campos numericos)

            this.txtBateriaCantidad.Enabled = activo;
            this.txtBateriaMarca.Enabled = activo;
            this.txtBateriaPlacas.Enabled = activo;

            #endregion

            #region Datos Remolque (Campos alfanumericos)

            this.txtSuspension.Enabled = activo;
            this.txtGancho.Enabled = activo;
            this.txtGatoNivelacion.Enabled = activo;
            this.txtArnesConexion.Enabled = activo;


            #endregion

            #region Llantas (Campos booleanos)

            this.rbtTieneEje1LlantaD.Enabled = activo;
            this.rbtTieneEje2LlantaD.Enabled = activo;
            this.rbtTieneEje3LlantaD.Enabled = activo;
            this.rbtTieneEje1LlantaI.Enabled = activo;
            this.rbtTieneEje2LlantaI.Enabled = activo;
            this.rbtTieneEje3LlantaI.Enabled = activo;
            this.rbtTieneTapaLluviaLlantaD.Enabled = activo;
            this.rbtTieneTapaLluviaLlantaI.Enabled = activo;

            #endregion

            #region Lamparas (Campos booleanos)

            this.rbtTieneLamparaDerecha.Enabled = activo;
            this.rbtTieneLamparaIzquierda.Enabled = activo;
            this.rbtTieneSenalSatelital.Enabled = activo;
            this.rbtTieneDiodos.Enabled = activo;

            #endregion
        }
        #endregion
    }
}