using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPuntosVerificacionSubArrendadoPSLUI : System.Web.UI.UserControl, IucPuntosVerificacionSubArrendadoPSLVIS {
        #region Atributos
        /// <summary>
        ///  Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionSubArrendadoPSLUI";

        /// <summary>
        /// Presentador de los equipos sub arreandados
        /// </summary>
        private ucPuntosVerificacionSubArrendadoPSLPRE presentador;
        #endregion

        #region Constructor
        /// <summary>
        /// Constrcutor de la clase ucPuntosVerificacionSubArrendadoPSLUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucPuntosVerificacionSubArrendadoPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion

        #region Propiedades
        public bool? tieneAireAcondicionado {
            get {
                bool val;
                return Boolean.TryParse(this.rbtAireAcondicionado.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtAireAcondicionado.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneAlarmaMovimiento {
            get {
                bool val;
                return Boolean.TryParse(this.rbtAlarmaMovimiento.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtAlarmaMovimiento.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneAmperimetro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtAmperimetro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtAmperimetro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneAsientoOperador {
            get {
                bool val;
                return Boolean.TryParse(this.rbtAsientoOperador.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtAsientoOperador.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneBateria {
            get {
                bool val;
                return Boolean.TryParse(this.rbtBateria.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtBateria.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneBoteDelantero {
            get {
                bool val;
                return Boolean.TryParse(this.rbtBoteDelantero.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtBoteDelantero.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneBoteTrasero {
            get {
                bool val;
                return Boolean.TryParse(this.rbtBoteTrasero.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtBoteTrasero.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneBrazoPluma {
            get {
                bool val;
                return Boolean.TryParse(this.rbtBrazoPluma.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtBrazoPluma.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneCentralEngrane {
            get {
                bool val;
                return Boolean.TryParse(this.rbtCentralEngrane.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtCentralEngrane.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneChasis {
            get {
                bool val;
                return Boolean.TryParse(this.rbtChasis.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtChasis.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneCofreMotor {
            get {
                bool val;
                return Boolean.TryParse(this.rbtCofreMotor.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtCofreMotor.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneContrapeso {
            get {
                bool val;
                return Boolean.TryParse(this.rbtContrapeso.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtContrapeso.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneCristalesLaterales {
            get {
                bool val;
                return Boolean.TryParse(this.rbtCristalesLaterales.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtCristalesLaterales.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneCuartosDireccionales {
            get {
                bool val;
                return Boolean.TryParse(this.rbtCuartosDireccionales.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtCuartosDireccionales.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneEnsamblajeRueda {
            get {
                bool val;
                return Boolean.TryParse(this.rbtEnsambleRueda.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtEnsambleRueda.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneEspejoRetrovisor {
            get {
                bool val;
                return Boolean.TryParse(this.rbtEspejoRetrovisor.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtEspejoRetrovisor.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneEstabilizadores {
            get {
                bool val;
                return Boolean.TryParse(this.rbtEstabilizadores.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtEstabilizadores.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneEstereo {
            get {
                bool val;
                return Boolean.TryParse(this.rbtEstereo.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtEstereo.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneEstructuraChasis {
            get {
                bool val;
                return Boolean.TryParse(this.rbtEstructuraChasis.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtEstructuraChasis.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneFarosDelanteros {
            get {
                bool val;
                return Boolean.TryParse(this.rbtFarosDelanteros.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtFarosDelanteros.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneFarosTraseros {
            get {
                bool val;
                return Boolean.TryParse(this.rbtFarosTraseros.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtFarosTraseros.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneFrecuentometro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtFrecuentometro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtFrecuentometro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneFuncionamiento {
            get {
                bool val;
                return Boolean.TryParse(this.rbtFuncionamiento.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtFuncionamiento.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneHorometro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtHorometro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtHorometro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneIndicadoresInterruptores {
            get {
                bool val;
                return Boolean.TryParse(this.rbtIndicadoresInterruptores.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtIndicadoresInterruptores.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneInterruptorTermomagnetico {
            get {
                bool val;
                return Boolean.TryParse(this.rbtInterruptorTermomagnetico.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtInterruptorTermomagnetico.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneKitMartillo {
            get {
                bool val;
                return Boolean.TryParse(this.rbtKitMartillo.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtKitMartillo.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneLamparas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtLamparas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtLamparas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneLimpiaparabrisas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtLimpiaparabrisas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtLimpiaparabrisas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneLlantas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtLlantas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtLlantas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneManometroPresion {
            get {
                bool val;
                return Boolean.TryParse(this.rbtManometroPresion.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtManometroPresion.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneMoldurasTolvas {
            get {
                bool val;
                return Boolean.TryParse(this.rbtMoldurasTolvas.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtMoldurasTolvas.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneNivelesFluido {
            get {
                bool val;
                return Boolean.TryParse(this.rbtNivelesFluido.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtNivelesFluido.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneNivelesFluidos {
            get {
                bool val;
                return Boolean.TryParse(this.rbtNivelesFluidos.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtNivelesFluidos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tienePalancaControl {
            get {
                bool val;
                return Boolean.TryParse(this.rbtPalancasControl.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtPalancasControl.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tienePanoramico {
            get {
                bool val;
                return Boolean.TryParse(this.rbtPanoramico.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtPanoramico.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneParrillaRadiador {
            get {
                bool val;
                return Boolean.TryParse(this.rbtParrillaRadiador.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtParrillaRadiador.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tienePintura {
            get {
                bool val;
                return Boolean.TryParse(this.rbtPintura.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtPintura.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tienePinturaEP {
            get {
                bool val;
                return Boolean.TryParse(this.rbtPinturaEP.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtPinturaEP.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tienePuertasCerraduras {
            get {
                bool val;
                return Boolean.TryParse(this.rbtPuertasCerraduras.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtPuertasCerraduras.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneSistemaElectrico {
            get {
                bool val;
                return Boolean.TryParse(this.rbtSistemaElectrico.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtSistemaElectrico.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneSistemaRemolque {
            get {
                bool val;
                return Boolean.TryParse(this.rbtSistemaRemolque.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtSistemaRemolque.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneSistemaVibratorio {
            get {
                bool val;
                return Boolean.TryParse(this.rbtSistemaVibratorio.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtSistemaVibratorio.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneTableroInstrumentos {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTableroInstrumentos.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTableroInstrumentos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneTapaFluidos {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTapaFluidos.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTapaFluidos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneTensionCadena {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTensionCadena.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTensionCadena.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneTipoVoltaje {
            get {
                bool val;
                return Boolean.TryParse(this.rbtTipoVoltaje.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtTipoVoltaje.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneVastagos {
            get {
                bool val;
                return Boolean.TryParse(this.rbtVastagos.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtVastagos.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneVentiladorElectrico {
            get {
                bool val;
                return Boolean.TryParse(this.rbtVentiladorElectrico.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtVentiladorElectrico.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneVoltimetro {
            get {
                bool val;
                return Boolean.TryParse(this.rbtVoltimetro.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtVoltimetro.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneZapata {
            get {
                bool val;
                return Boolean.TryParse(this.rbtZapata.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtZapata.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public bool? tieneZapataRodillo {
            get {
                bool val;
                return Boolean.TryParse(this.rbtZapataRodillo.SelectedValue, out val) ? (bool?)val : null;
            }
            set {
                if (value.HasValue) {
                    this.rbtZapataRodillo.SelectedValue = value.Value ? Boolean.TrueString : Boolean.FalseString;
                }
            }
        }

        public string comentariosGenerales {
            get { return this.txtComentariosGenerales.Text.Trim().ToUpper(); }
            set {
                this.txtComentariosGenerales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                ? value.Trim().ToUpper()
                                                : string.Empty;
            }
        }

        #endregion

        #region Métodos
        public void ModoEdicion(bool activo) {
            this.rbtNivelesFluido.Enabled = false;
            this.rbtTapaFluidos.Enabled = false;
            this.rbtSistemaElectrico.Enabled = false;
            this.rbtFarosTraseros.Enabled = false;
            this.rbtFarosDelanteros.Enabled = false;
            this.rbtCuartosDireccionales.Enabled = false;
            this.rbtLimpiaparabrisas.Enabled = false;
            this.rbtBateria.Enabled = false;
            this.rbtChasis.Enabled = false;
            this.rbtEstabilizadores.Enabled = false;
            this.rbtZapata.Enabled = false;
            this.rbtBoteTrasero.Enabled = false;
            this.rbtBoteDelantero.Enabled = false;
            this.rbtBrazoPluma.Enabled = false;
            this.rbtContrapeso.Enabled = false;
            this.rbtVastagos.Enabled = false;
            this.rbtTensionCadena.Enabled = false;
            this.rbtNivelesFluidos.Enabled = false;
            this.rbtSistemaRemolque.Enabled = false;
            this.rbtEnsambleRueda.Enabled = false;
            this.rbtEstructuraChasis.Enabled = false;
            this.rbtPintura.Enabled = false;
            this.rbtLlantas.Enabled = false;
            this.rbtSistemaVibratorio.Enabled = false;
            this.rbtZapataRodillo.Enabled = false;
            this.rbtAsientoOperador.Enabled = false;
            this.rbtEspejoRetrovisor.Enabled = false;
            this.rbtPalancasControl.Enabled = false;
            this.rbtTableroInstrumentos.Enabled = false;
            this.rbtMoldurasTolvas.Enabled = false;
            this.rbtAireAcondicionado.Enabled = false;
            this.rbtCristalesLaterales.Enabled = false;
            this.rbtPanoramico.Enabled = false;
            this.rbtPuertasCerraduras.Enabled = false;
            this.rbtCofreMotor.Enabled = false;
            this.rbtParrillaRadiador.Enabled = false;
            this.rbtAlarmaMovimiento.Enabled = false;
            this.rbtEstereo.Enabled = false;
            this.rbtVentiladorElectrico.Enabled = false;
            this.rbtIndicadoresInterruptores.Enabled = false;
            this.rbtPinturaEP.Enabled = false;
            this.rbtKitMartillo.Enabled = false;
            this.rbtCentralEngrane.Enabled = false;
            this.rbtAmperimetro.Enabled = false;
            this.rbtVoltimetro.Enabled = false;
            this.rbtHorometro.Enabled = false;
            this.rbtFrecuentometro.Enabled = false;
            this.rbtInterruptorTermomagnetico.Enabled = false;
            this.rbtManometroPresion.Enabled = false;
            this.rbtTipoVoltaje.Enabled = false;
            this.rbtLamparas.Enabled = false;
            this.rbtFuncionamiento.Enabled = false;
            this.txtComentariosGenerales.Enabled = false;
        }

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