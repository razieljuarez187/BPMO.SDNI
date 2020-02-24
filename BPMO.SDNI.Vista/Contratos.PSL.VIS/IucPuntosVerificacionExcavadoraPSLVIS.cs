using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    /// <summary>
    /// Vista que implemente la Interfaz que controla los puntos de verificación de una excavadora
    /// </summary>
    public interface IucPuntosVerificacionExcavadoraPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica la cantidad de aceite hidráulico
        /// </summary>
        bool? tieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica aceite de motor
        /// </summary>
        bool? tieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene aire acondicionado
        /// </summary>
        bool? tieneAireAcondicionado { get; set; }
        /// <summary>
        /// Verifica si tiene alarma de reversa
        /// </summary>
        bool? tieneAlarmaReversa { get; set; }
        /// <summary>
        /// Verifica si tiene antenas de monitoreo satelital
        /// </summary>
        bool? tieneAntenasMonitoreoSatelital { get; set; }
        /// <summary>
        /// Verifica si tiene auto aceleración
        /// </summary>
        bool? tieneAutoaceleracion { get; set; }
        /// <summary>
        /// Verifica si tiene balancín bote
        /// </summary>
        bool? tieneBalancinBote { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene bisagra cofre motor
        /// </summary>
        bool? tieneBisagrasCofreMotor { get; set; }
        /// <summary>
        /// Verifica si tiene brazo pluma
        /// </summary>
        bool? tieneBrazoPluma { get; set; }
        /// <summary>
        /// Verifica si tiene central de engrase
        /// </summary>
        bool? tieneCentralEngrase { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? tieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene condición el asiento
        /// </summary>
        bool? tieneCondicionAsiento { get; set; }
        /// <summary>
        /// Verifica si tiene condición las calcas
        /// </summary>
        bool? tieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? tieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene cristales la cabina
        /// </summary>
        bool? tieneCristalesCabina { get; set; }
        /// <summary>
        /// Verifica si tiene espejos retrovisores
        /// </summary>
        bool? tieneEspejosRetrovisores { get; set; }
        /// <summary>
        /// Verifica si tiene estructura chasis
        /// </summary>
        bool? tieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene horómetro
        /// </summary>
        bool? tieneHorometro { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores
        /// </summary>
        bool? tieneIndicadores { get; set; }
        /// <summary>
        /// Verifica si tiene interruptor de desconexión
        /// </summary>
        bool? tieneInterruptorDesconexion { get; set; }
        /// <summary>
        /// Verifica si tiene joystick
        /// </summary>
        bool? tieneJoystick { get; set; }
        /// <summary>
        /// Verifica si tiene lamparas en el tablero
        /// </summary>
        bool? tieneLamparasTablero { get; set; }
        /// <summary>
        /// Verifica si tiene limpia parabrisas
        /// </summary>
        bool? tieneLimpiaparabrisas { get; set; }
        /// <summary>
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool? tieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de advertencia
        /// </summary>
        bool? tieneLucesAdvertencia { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo
        /// </summary>
        bool? tieneLucesTrabajo { get; set; }
        /// <summary>
        /// Verifica si tiene palanca de bloqueo de pilotaje
        /// </summary>
        bool? tienePalancaBloqueoPilotaje { get; set; }
        /// <summary>
        /// Verifica si tiene pasadores boom
        /// </summary>
        bool? tienePasadoresBoom { get; set; }
        /// <summary>
        /// Verifica si tiene pasadores bote
        /// </summary>
        bool? tienePasadoresBote { get; set; }
        /// <summary>
        /// Verifica si tiene pasadores brazo
        /// </summary>
        bool? tienePasadoresBrazo { get; set; }
        /// <summary>
        /// Verifica si tiene puertas cerraduras
        /// </summary>
        bool? tienePuertasCerraduras { get; set; }
        /// <summary>
        /// Verifica si tiene reductor de engranes de transito
        /// </summary>
        bool? tieneReductorEngranesTransito { get; set; }
        /// <summary>
        /// Verifica si tiene reductor swing
        /// </summary>
        bool? tieneReductorSwing { get; set; }
        /// <summary>
        /// Verifica si tiene rodillos de transito
        /// </summary>
        bool? tieneRodillosTransito { get; set; }
        /// <summary>
        /// Verifica si tiene contrapeso
        /// </summary>
        bool? tienesContrapeso { get; set; }
        /// <summary>
        /// Verifica si tiene símbolo de seguridad la maquina
        /// </summary>
        bool? tieneSimboloSeguridadMaquina { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el combustible
        /// </summary>
        bool? tieneTapaCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene tensión la cadena
        /// </summary>
        bool? tieneTensionCadena { get; set; }
        /// <summary>
        /// Verifica si tiene tornamesa
        /// </summary>
        bool? tieneTornameza { get; set; }
        /// <summary>
        /// Verifica si tiene vástagos gatos
        /// </summary>
        bool? tieneVastagosGatos { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima motor
        /// </summary>
        bool? tieneVelocidadMaximaMotor { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima motor
        /// </summary>
        bool? tieneVelocidadMinimaMotor { get; set; }
        /// <summary>
        /// Verifica si tiene zapatas
        /// </summary>
        bool? tieneZapatas { get; set; }


        #endregion

        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}