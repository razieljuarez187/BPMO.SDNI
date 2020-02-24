
using System;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucPuntosVerificacionSubArrendadoPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ucPuntosVerificacionSubArrendadoPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IucPuntosVerificacionSubArrendadoPSLVIS vista;
        internal IucPuntosVerificacionSubArrendadoPSLVIS Vista { get { return vista; } }

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
        public ucPuntosVerificacionSubArrendadoPSLPRE(IucPuntosVerificacionSubArrendadoPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ucPuntosVerificacionSubArrendadoPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        public void Registrar() {
            ListadoVerificacionSubArrendadoBO bo = (ListadoVerificacionSubArrendadoBO)this.InterfazUsuarioADato();

        }
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.tieneAireAcondicionado = null;
            this.vista.tieneAlarmaMovimiento = null;
            this.vista.tieneAmperimetro = null;
            this.vista.tieneAsientoOperador = null;
            this.vista.tieneBateria = null;
            this.vista.tieneBoteDelantero = null;
            this.vista.tieneBoteTrasero = null;
            this.vista.tieneBrazoPluma = null;
            this.vista.tieneCentralEngrane = null;
            this.vista.tieneChasis = null;
            this.vista.tieneCofreMotor = null;
            this.vista.tieneContrapeso = null;
            this.vista.tieneCristalesLaterales = null;
            this.vista.tieneCuartosDireccionales = null;
            this.vista.tieneEnsamblajeRueda = null;
            this.vista.tieneEspejoRetrovisor = null;
            this.vista.tieneEstabilizadores = null;
            this.vista.tieneEstereo = null;
            this.vista.tieneEstructuraChasis = null;

            this.vista.tieneFarosDelanteros = null;
            this.vista.tieneFarosTraseros = null;
            this.vista.tieneFrecuentometro = null;
            this.vista.tieneFuncionamiento = null;
            this.vista.tieneHorometro = null;
            this.vista.tieneIndicadoresInterruptores = null;
            this.vista.tieneInterruptorTermomagnetico = null;
            this.vista.tieneKitMartillo = null;
            this.vista.tieneLamparas = null;
            this.vista.tieneLimpiaparabrisas = null;
            this.vista.tieneLlantas = null;
            this.vista.tieneManometroPresion = null;
            this.vista.tieneMoldurasTolvas = null;
            this.vista.tieneNivelesFluidos = null;
            this.vista.tieneNivelesFluido = null;
            this.vista.tienePalancaControl = null;
            this.vista.tienePanoramico = null;
            this.vista.tieneParrillaRadiador = null;
            this.vista.tienePintura = null;
            this.vista.tienePinturaEP = null;
            this.vista.tienePuertasCerraduras = null;
            this.vista.tieneSistemaElectrico = null;
            this.vista.tieneSistemaRemolque = null;
            this.vista.tieneSistemaVibratorio = null;
            this.vista.tieneTableroInstrumentos = null;
            this.vista.tieneTapaFluidos = null;
            this.vista.tieneTensionCadena = null;
            this.vista.tieneTipoVoltaje = null;
            this.vista.tieneVastagos = null;
            this.vista.tieneVentiladorElectrico = null;
            this.vista.tieneVoltimetro = null;
            this.vista.tieneZapata = null;
            this.vista.tieneZapataRodillo = null;
        }

        public string ValidarCampos() {
            StringBuilder s = new StringBuilder();

            if (!vista.tieneNivelesFluido.Value)
                s.Append("Niveles de fluidos, ");
            if (!vista.tieneTapaFluidos.Value)
                s.Append("Tapa de fluidos, ");
            if (!vista.tieneSistemaElectrico.Value)
                s.Append("Sistema eléctrico, ");
            if (!vista.tieneFarosTraseros.Value)
                s.Append("Faros traseros, ");
            if (!vista.tieneFarosDelanteros.Value)
                s.Append("Faros delanteros, ");
            if (!vista.tieneCuartosDireccionales.Value)
                s.Append("Cuartos y direccionales, ");
            if (!vista.tieneLimpiaparabrisas.Value)
                s.Append("Limpiaparabrisas, ");
            if (!vista.tieneBateria.Value)
                s.Append("Batería, ");
            if (!vista.tieneChasis.Value)
                s.Append("Chasis, ");
            if (!vista.tieneEstabilizadores.Value)
                s.Append("Estabilizadores, ");
            if (!vista.tieneZapata.Value)
                s.Append("Zapata, ");
            if (!vista.tieneBoteTrasero.Value)
                s.Append("Bote trasero, ");
            if (!vista.tieneBoteDelantero.Value)
                s.Append("Bote delantero, ");
            if (!vista.tieneBrazoPluma.Value)
                s.Append("Brazo y pluma, ");
            if (!vista.tieneContrapeso.Value)
                s.Append("Contrapeso, ");
            if (!vista.tieneVastagos.Value)
                s.Append("Gatos HD (vástagos), ");
            if (!vista.tieneTensionCadena.Value)
                s.Append("Tensión de cadena, ");
            if (!vista.tieneNivelesFluidos.Value)
                s.Append("Nivel de fluidos, ");
            if (!vista.tieneSistemaRemolque.Value)
                s.Append("Sistema de remolque, ");
            if (!vista.tieneEnsamblajeRueda.Value)
                s.Append("Ensamble de rueda, ");
            if (!vista.tieneEstructuraChasis.Value)
                s.Append("Estructura o chasis, ");
            if (!vista.tienePintura.Value)
                s.Append("Pintura, ");
            if (!vista.tieneLlantas.Value)
                s.Append("Llantas, ");
            if (!vista.tieneSistemaVibratorio.Value)
                s.Append("Sistema vibratorio, ");
            if (!vista.tieneZapataRodillo.Value)
                s.Append("Zapata o rodillo, ");
            if (!vista.tieneAsientoOperador.Value)
                s.Append("Asiento del operador, ");
            if (!vista.tieneEspejoRetrovisor.Value)
                s.Append("Espejo retrovisor, ");
            if (!vista.tienePalancaControl.Value)
                s.Append("Palancas de control, ");
            if (!vista.tieneTableroInstrumentos.Value)
                s.Append("Tablero de instrumentos, ");
            if (!vista.tieneMoldurasTolvas.Value)
                s.Append("Molduras y tolvas, ");
            if (!vista.tieneAireAcondicionado.Value)
                s.Append("Aire acondicionado, ");
            if (!vista.tieneCristalesLaterales.Value)
                s.Append("Cristales laterales, ");
            if (!vista.tienePanoramico.Value)
                s.Append("Panorámico, ");
            if (!vista.tienePuertasCerraduras.Value)
                s.Append("Puertas y cerraduras, ");
            if (!vista.tieneCofreMotor.Value)
                s.Append("Cofre de motor, ");
            if (!vista.tieneParrillaRadiador.Value)
                s.Append("Parrilla de radiador, ");
            if (!vista.tieneAlarmaMovimiento.Value)
                s.Append("Alarma de movimiento, ");
            if (!vista.tieneEstereo.Value)
                s.Append("Estéreo, ");
            if (!vista.tieneVentiladorElectrico.Value)
                s.Append("Ventilador eléctrico, ");
            if (!vista.tieneIndicadoresInterruptores.Value)
                s.Append("Indicadores e interruptores, ");
            if (!vista.tienePinturaEP.Value)
                s.Append("Pintura equipos portátiles, ");
            if (!vista.tieneKitMartillo.Value)
                s.Append("Kit HD para martillo, ");
            if (!vista.tieneCentralEngrane.Value)
                s.Append("Central de engrane, ");
            if (!vista.tieneAmperimetro.Value)
                s.Append("Amperímetro, ");
            if (!vista.tieneVoltimetro.Value)
                s.Append("Voltímetro, ");
            if (!vista.tieneHorometro.Value)
                s.Append("Horómetro, ");
            if (!vista.tieneFrecuentometro.Value)
                s.Append("Frecuentómetro, ");
            if (!vista.tieneInterruptorTermomagnetico.Value)
                s.Append("Interruptor termomagnético, ");
            if (!vista.tieneManometroPresion.Value)
                s.Append("Manómetro de presión, ");
            if (!vista.tieneTipoVoltaje.Value)
                s.Append("Tipo de voltaje, ");
            if (!vista.tieneLamparas.Value)
                s.Append("Lámparas, ");
            if (!vista.tieneFuncionamiento.Value)
                s.Append("Funcionamiento, ");

            if (s.Length > 0)
                return "Es necesario capturar observaciones para los siguientes puntos de verificación: \n" + s.ToString().Substring(0, s.Length - 2);

            return null;
        }

        public object InterfazUsuarioADato() {
            ListadoVerificacionSubArrendadoBO bo = new ListadoVerificacionSubArrendadoBO();

            if (this.vista.tieneNivelesFluido.HasValue)
                bo.TieneNivelesFluido = this.vista.tieneNivelesFluido.Value;
            if (this.vista.tieneTapaFluidos.HasValue)
                bo.TieneTapaFluidos = this.vista.tieneTapaFluidos.Value;
            if (this.vista.tieneSistemaElectrico.HasValue)
                bo.TieneSistemaElectrico = this.vista.tieneSistemaElectrico.Value;
            if (this.vista.tieneFarosTraseros.HasValue)
                bo.TieneFarosTraseros = this.vista.tieneFarosTraseros.Value;
            if (this.vista.tieneFarosDelanteros.HasValue)
                bo.TieneFarosDelanteros = this.vista.tieneFarosDelanteros.Value;
            if (this.vista.tieneCuartosDireccionales.HasValue)
                bo.TieneCuartosDireccionales = this.vista.tieneCuartosDireccionales.Value;
            if (this.vista.tieneLimpiaparabrisas.HasValue)
                bo.TieneLimpiaparabrisas = this.vista.tieneLimpiaparabrisas.Value;
            if (this.vista.tieneBateria.HasValue)
                bo.TieneBateria = this.vista.tieneBateria.Value;
            if (this.vista.tieneChasis.HasValue)
                bo.TieneChasis = this.vista.tieneChasis.Value;
            if (this.vista.tieneEstabilizadores.HasValue)
                bo.TieneEstabilizadores = this.vista.tieneEstabilizadores.Value;
            if (this.vista.tieneZapata.HasValue)
                bo.TieneZapata = this.vista.tieneZapata.Value;
            if (this.vista.tieneBoteTrasero.HasValue)
                bo.TieneBoteTrasero = this.vista.tieneBoteTrasero.Value;
            if (this.vista.tieneBoteDelantero.HasValue)
                bo.TieneBoteDelantero = this.vista.tieneBoteDelantero.Value;
            if (this.vista.tieneBrazoPluma.HasValue)
                bo.TieneBrazoPluma = this.vista.tieneBrazoPluma.Value;
            if (this.vista.tieneContrapeso.HasValue)
                bo.TieneContrapeso = this.vista.tieneContrapeso.Value;
            if (this.vista.tieneVastagos.HasValue)
                bo.TieneVastagos = this.vista.tieneVastagos.Value;
            if (this.vista.tieneTensionCadena.HasValue)
                bo.TieneTensionCadena = this.vista.tieneTensionCadena.Value;
            if (this.vista.tieneNivelesFluidos.HasValue)
                bo.TieneNivelesFluidos = this.vista.tieneNivelesFluidos.Value;
            if (this.vista.tieneSistemaRemolque.HasValue)
                bo.TieneSistemaRemolque = this.vista.tieneSistemaRemolque.Value;
            if (this.vista.tieneEnsamblajeRueda.HasValue)
                bo.TieneEnsamblajeRueda = this.vista.tieneEnsamblajeRueda.Value;
            if (this.vista.tieneEstructuraChasis.HasValue)
                bo.TieneEstructuraChasis = this.vista.tieneEstructuraChasis.Value;
            if (this.vista.tienePintura.HasValue)
                bo.TienePintura = this.vista.tienePintura.Value;
            if (this.vista.tieneLlantas.HasValue)
                bo.TieneLlantas = this.vista.tieneLlantas.Value;
            if (this.vista.tieneSistemaVibratorio.HasValue)
                bo.TieneSistemaVibratorio = this.vista.tieneSistemaVibratorio.Value;
            if (this.vista.tieneZapataRodillo.HasValue)
                bo.TieneZapataRodillo = this.vista.tieneZapataRodillo.Value;
            if (this.vista.tieneAsientoOperador.HasValue)
                bo.TieneAsientoOperador = this.vista.tieneAsientoOperador.Value;
            if (this.vista.tieneEspejoRetrovisor.HasValue)
                bo.TieneEspejoRetrovisor = this.vista.tieneEspejoRetrovisor.Value;
            if (this.vista.tienePalancaControl.HasValue)
                bo.TienePalancaControl = this.vista.tienePalancaControl.Value;
            if (this.vista.tieneTableroInstrumentos.HasValue)
                bo.TieneTableroInstrumentos = this.vista.tieneTableroInstrumentos.Value;
            if (this.vista.tieneMoldurasTolvas.HasValue)
                bo.TieneMoldurasTolvas = this.vista.tieneMoldurasTolvas.Value;
            if (this.vista.tieneAireAcondicionado.HasValue)
                bo.TieneAireAcondicionado = this.vista.tieneAireAcondicionado.Value;
            if (this.vista.tieneCristalesLaterales.HasValue)
                bo.TieneCristalesLaterales = this.vista.tieneCristalesLaterales.Value;
            if (this.vista.tienePanoramico.HasValue)
                bo.TienePanoramico = this.vista.tienePanoramico.Value;
            if (this.vista.tienePuertasCerraduras.HasValue)
                bo.TienePuertasCerraduras = vista.tienePuertasCerraduras.Value;
            if (this.vista.tieneCofreMotor.HasValue)
                bo.TieneCofreMotor = vista.tieneCofreMotor.Value;
            if (this.vista.tieneParrillaRadiador.HasValue)
                bo.TieneParrillaRadiador = vista.tieneParrillaRadiador.Value;
            if (this.vista.tieneAlarmaMovimiento.HasValue)
                bo.TieneAlarmaMovimiento = vista.tieneAlarmaMovimiento.Value;
            if (this.vista.tieneEstereo.HasValue)
                bo.TieneEstereo = vista.tieneEstereo.Value;
            if (this.vista.tieneVentiladorElectrico.HasValue)
                bo.TieneVentiladorElectrico = vista.tieneVentiladorElectrico.Value;
            if (this.vista.tieneIndicadoresInterruptores.HasValue)
                bo.TieneIndicadoresInterruptores = vista.tieneIndicadoresInterruptores.Value;
            if (this.vista.tienePinturaEP.HasValue)
                bo.TienePinturaEP = vista.tienePinturaEP.Value;
            if (this.vista.tieneKitMartillo.HasValue)
                bo.TieneKitMartillo = vista.tieneKitMartillo.Value;
            if (this.vista.tieneCentralEngrane.HasValue)
                bo.TieneCentralEngrane = vista.tieneCentralEngrane.Value;
            if (this.vista.tieneAmperimetro.HasValue)
                bo.TieneAmperimetro = vista.tieneAmperimetro.Value;
            if (this.vista.tieneVoltimetro.HasValue)
                bo.TieneVoltimetro = vista.tieneVoltimetro.Value;
            if (this.vista.tieneHorometro.HasValue)
                bo.TieneHorometro = vista.tieneHorometro.Value;
            if (this.vista.tieneFrecuentometro.HasValue)
                bo.TieneFrecuentometro = vista.tieneFrecuentometro.Value;
            if (this.vista.tieneInterruptorTermomagnetico.HasValue)
                bo.TieneInterruptorTermomagnetico = vista.tieneInterruptorTermomagnetico.Value;
            if (this.vista.tieneManometroPresion.HasValue)
                bo.TieneManometroPresion = vista.tieneManometroPresion.Value;
            if (this.vista.tieneTipoVoltaje.HasValue)
                bo.TieneTipoVoltaje = vista.tieneTipoVoltaje.Value;
            if (this.vista.tieneLamparas.HasValue)
                bo.TieneLamparas = vista.tieneLamparas.Value;
            if (this.vista.tieneFuncionamiento.HasValue)
                bo.TieneFuncionamiento = vista.tieneFuncionamiento.Value;
            if (!string.IsNullOrEmpty(this.vista.comentariosGenerales))
                bo.ComentariosGenerales = this.vista.comentariosGenerales;

            return bo;
        }

        public void DatoToInterfazUsuario(ListadoVerificacionSubArrendadoBO bo) {
            this.vista.tieneNivelesFluido = bo.TieneNivelesFluido;
            this.vista.tieneTapaFluidos = bo.TieneTapaFluidos;
            this.vista.tieneSistemaElectrico = bo.TieneSistemaElectrico;
            this.vista.tieneFarosTraseros = bo.TieneFarosTraseros;
            this.vista.tieneFarosDelanteros = bo.TieneFarosDelanteros;
            this.vista.tieneCuartosDireccionales = bo.TieneCuartosDireccionales;
            this.vista.tieneLimpiaparabrisas = bo.TieneLimpiaparabrisas;
            this.vista.tieneBateria = bo.TieneBateria;
            this.vista.tieneChasis = bo.TieneChasis;
            this.vista.tieneEstabilizadores = bo.TieneEstabilizadores;
            this.vista.tieneZapata = bo.TieneZapata;
            this.vista.tieneBoteTrasero = bo.TieneBoteTrasero;
            this.vista.tieneBoteDelantero = bo.TieneBoteDelantero;
            this.vista.tieneBrazoPluma = bo.TieneBrazoPluma;
            this.vista.tieneContrapeso = bo.TieneContrapeso;
            this.vista.tieneVastagos = bo.TieneVastagos;
            this.vista.tieneTensionCadena = bo.TieneTensionCadena;

            this.vista.tieneNivelesFluidos = bo.TieneNivelesFluidos;
            this.vista.tieneSistemaRemolque = bo.TieneSistemaRemolque;
            this.vista.tieneEnsamblajeRueda = bo.TieneEnsamblajeRueda;
            this.vista.tieneEstructuraChasis = bo.TieneEstructuraChasis;
            this.vista.tienePintura = bo.TienePintura;
            this.vista.tieneLlantas = bo.TieneLlantas;
            this.vista.tieneSistemaVibratorio = bo.TieneSistemaVibratorio;
            this.vista.tieneZapataRodillo = bo.TieneZapataRodillo;
            this.vista.tieneAsientoOperador = bo.TieneAsientoOperador;
            this.vista.tieneEspejoRetrovisor = bo.TieneEspejoRetrovisor;
            this.vista.tienePalancaControl = bo.TienePalancaControl;
            this.vista.tieneTableroInstrumentos = bo.TieneTableroInstrumentos;
            this.vista.tieneMoldurasTolvas = bo.TieneMoldurasTolvas;
            this.vista.tieneAireAcondicionado = bo.TieneAireAcondicionado;
            this.vista.tieneCristalesLaterales = bo.TieneCristalesLaterales;
            this.vista.tienePanoramico = bo.TienePanoramico;
            this.vista.tienePuertasCerraduras = bo.TienePuertasCerraduras;
            this.vista.tieneCofreMotor = bo.TieneCofreMotor;
            this.vista.tieneParrillaRadiador = bo.TieneParrillaRadiador;
            this.vista.tieneAlarmaMovimiento = bo.TieneAlarmaMovimiento;
            this.vista.tieneEstereo = bo.TieneEstereo;
            this.vista.tieneVentiladorElectrico = bo.TieneVentiladorElectrico;
            this.vista.tieneIndicadoresInterruptores = bo.TieneIndicadoresInterruptores;
            this.vista.tienePinturaEP = bo.TienePinturaEP;
            this.vista.tieneKitMartillo = bo.TieneKitMartillo;
            this.vista.tieneCentralEngrane = bo.TieneCentralEngrane;
            this.vista.tieneAmperimetro = bo.TieneAmperimetro;
            this.vista.tieneVoltimetro = bo.TieneVoltimetro;
            this.vista.tieneHorometro = bo.TieneHorometro;
            this.vista.tieneFrecuentometro = bo.TieneFrecuentometro;
            this.vista.tieneInterruptorTermomagnetico = bo.TieneInterruptorTermomagnetico;
            this.vista.tieneManometroPresion = bo.TieneManometroPresion;
            this.vista.tieneTipoVoltaje = bo.TieneTipoVoltaje;
            this.vista.tieneLamparas = bo.TieneLamparas;
            this.vista.tieneFuncionamiento = bo.TieneFuncionamiento;
            this.vista.comentariosGenerales = bo.ComentariosGenerales;
        }

        #endregion
    }
}