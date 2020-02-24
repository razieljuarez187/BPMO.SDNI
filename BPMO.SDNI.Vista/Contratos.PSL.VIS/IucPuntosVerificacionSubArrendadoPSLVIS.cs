using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionSubArrendadoPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene aire acondicionado
        /// </summary>
        bool? tieneAireAcondicionado { get; set; }
        /// <summary>
        /// Verifica si tiene alarma de movimiento
        /// </summary>
        bool? tieneAlarmaMovimiento { get; set; }
        /// <summary>
        /// Verifica si tiene amperímetro
        /// </summary>
        bool? tieneAmperimetro { get; set; }
        /// <summary>
        /// Verifica si tiene asiento el operador
        /// </summary>
        bool? tieneAsientoOperador { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene bote delantero
        /// </summary>
        bool? tieneBoteDelantero { get; set; }
        /// <summary>
        /// Verifica si tiene bote trasero
        /// </summary>
        bool? tieneBoteTrasero { get; set; }
        /// <summary>
        /// Verifica si tiene brazo pluma
        /// </summary>
        bool? tieneBrazoPluma { get; set; }
        /// <summary>
        /// Verifica si tiene central de engrane
        /// </summary>
        bool? tieneCentralEngrane { get; set; }
        /// <summary>
        /// Verifica si tiene chasis
        /// </summary>
        bool? tieneChasis { get; set; }
        /// <summary>
        /// Verifica si tiene cofre el motor
        /// </summary>
        bool? tieneCofreMotor { get; set; }
        /// <summary>
        /// Verifica si tiene contrapeso
        /// </summary>
        bool? tieneContrapeso { get; set; }
        /// <summary>
        /// Verifica si tiene cristales laterales
        /// </summary>
        bool? tieneCristalesLaterales { get; set; }
        /// <summary>
        /// Verifica si tiene cuartos direccionales
        /// </summary>
        bool? tieneCuartosDireccionales { get; set; }
        /// <summary>
        /// Verifica si tiene ensamblaje la rueda
        /// </summary>
        bool? tieneEnsamblajeRueda { get; set; }
        /// <summary>
        /// Verifica si tiene espejo retrovisor
        /// </summary>
        bool? tieneEspejoRetrovisor { get; set; }
        /// <summary>
        /// Verifica si tiene estabilizadores
        /// </summary>
        bool? tieneEstabilizadores { get; set; }
        /// <summary>
        /// Verifica si tiene estéreo
        /// </summary>
        bool? tieneEstereo { get; set; }
        /// <summary>
        /// Verifica si tiene estructura el chasis
        /// </summary>
        bool? tieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene faros delanteros
        /// </summary>
        bool? tieneFarosDelanteros { get; set; }
        /// <summary>
        /// Verifica si tiene faros traseros
        /// </summary>
        bool? tieneFarosTraseros { get; set; }
        /// <summary>
        /// Verifica si tiene frecuento metro
        /// </summary>
        bool? tieneFrecuentometro { get; set; }
        /// <summary>
        /// Verifica si tiene funcionamiento
        /// </summary>
        bool? tieneFuncionamiento { get; set; }
        /// <summary>
        /// Verifica si tiene horómetro
        /// </summary>
        bool? tieneHorometro { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores los interruptores
        /// </summary>
        bool? tieneIndicadoresInterruptores { get; set; }
        /// <summary>
        /// Verifica si tiene interruptor termomagnético
        /// </summary>
        bool? tieneInterruptorTermomagnetico { get; set; }
        /// <summary>
        /// Verifica si tiene kit de martillo
        /// </summary>
        bool? tieneKitMartillo { get; set; }
        /// <summary>
        /// Verifica si tiene lamparas
        /// </summary>
        bool? tieneLamparas { get; set; }
        /// <summary>
        /// Verifica si tiene limpiaparabrisas
        /// </summary>
        bool? tieneLimpiaparabrisas { get; set; }
        /// <summary>
        /// Verifica si tiene llantas
        /// </summary>
        bool? tieneLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene manómetro de presión
        /// </summary>
        bool? tieneManometroPresion { get; set; }
        /// <summary>
        /// Verifica si tiene molduras tolvas
        /// </summary>
        bool? tieneMoldurasTolvas { get; set; }
        /// <summary>
        /// Verifica si tiene niveles los fluidos
        /// </summary>
        bool? tieneNivelesFluido { get; set; }
        /// <summary>
        /// Verifica si tiene nivel los fluidos
        /// </summary>
        bool? tieneNivelesFluidos { get; set; }
        /// <summary>
        /// Verifica si tiene palanca de control
        /// </summary>
        bool? tienePalancaControl { get; set; }
        /// <summary>
        /// Verifica si tiene panorámico
        /// </summary>
        bool? tienePanoramico { get; set; }
        /// <summary>
        /// Verifica si tiene parrilla radiador
        /// </summary>
        bool? tieneParrillaRadiador { get; set; }
        /// <summary>
        /// Verifica si tiene pintura
        /// </summary>
        bool? tienePintura { get; set; }
        /// <summary>
        /// Verifica si tiene pintura EP
        /// </summary>
        bool? tienePinturaEP { get; set; }
        /// <summary>
        /// Verifica si las puertas tienen cerraduras
        /// </summary>
        bool? tienePuertasCerraduras { get; set; }
        /// <summary>
        /// Verifica si tiene sistema eléctrico
        /// </summary>
        bool? tieneSistemaElectrico { get; set; }
        /// <summary>
        /// Verifica si tiene sistema remolque
        /// </summary>
        bool? tieneSistemaRemolque { get; set; }
        /// <summary>
        /// Verifica si tiene sistema vibratorio
        /// </summary>
        bool? tieneSistemaVibratorio { get; set; }
        /// <summary>
        /// Verifica si tiene el tablero instrumentos
        /// </summary>
        bool? tieneTableroInstrumentos { get; set; }
        /// <summary>
        /// Verifica si tiene tapa los fluidos
        /// </summary>
        bool? tieneTapaFluidos { get; set; }
        /// <summary>
        /// Verifica si tiene tensión la cadena
        /// </summary>
        bool? tieneTensionCadena { get; set; }
        /// <summary>
        /// Verifica si tiene voltaje
        /// </summary>
        bool? tieneTipoVoltaje { get; set; }
        /// <summary>
        /// Verifica si tiene vástagos
        /// </summary>
        bool? tieneVastagos { get; set; }
        /// <summary>
        /// Verifica si tiene ventilador eléctrico
        /// </summary>
        bool? tieneVentiladorElectrico { get; set; }
        /// <summary>
        /// Verifica si tiene voltímetro
        /// </summary>
        bool? tieneVoltimetro { get; set; }
        /// <summary>
        /// Verifica si tiene zapata
        /// </summary>
        bool? tieneZapata { get; set; }
        /// <summary>
        /// Verifica  si tiene zapata de rodillo
        /// </summary>
        bool? tieneZapataRodillo { get; set; }

        string comentariosGenerales { get; set; }

        #endregion

        #region Métodos
        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion

    }
}