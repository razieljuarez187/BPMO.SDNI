using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionTorresLuzPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene aceite el motor
        /// </summary>
        bool? tieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene antenas de monitoreo
        /// </summary>
        bool? tieneAntenasMonitoreo { get; set; }
        /// <summary>
        /// Verifica si tiene banda el ventilador
        /// </summary>
        bool? tieneBandaVentilador { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene cable para levantar la torre
        /// </summary>
        bool? tieneCableLevanteTorre { get; set; }
        /// <summary>
        /// Verifica si tiene cartuchos de filtro de aire
        /// </summary>
        bool? tieneCartuchoFiltroAire { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? tieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tienen condición las calcas
        /// </summary>
        bool? tieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tienen condición las llantas
        /// </summary>
        bool? tieneCondicionLLantas { get; set; }
        /// <summary>
        /// Verifica si tienen condición las llantas
        /// </summary>
        bool? tieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene estructura el chasis
        /// </summary>
        bool? tieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool? tieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de advertencia
        /// </summary>
        bool? tieneLucesAdvertencia { get; set; }
        /// <summary>
        /// Verifica si tiene luces la torre
        /// </summary>
        bool? tieneLucesTorre { get; set; }
        /// <summary>
        /// Verifica si tiene mangueras abrazaderas
        /// </summary>
        bool? tieneManguerasAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si tienen presión las llantas
        /// </summary>
        bool? tienePresionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool? tieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene switch de encendido
        /// </summary>
        bool? tieneSwitchEncendido { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el combustible
        /// </summary>
        bool? tieneTapaCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima
        /// </summary>
        bool? tieneVelocidadMaxima { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima
        /// </summary>
        bool? tieneVelocidadMinima { get; set; }
        /// <summary>
        /// Verifica si tiene lamparas el tablero
        /// </summary>
        bool? tieneLamparasTablero { get; set; }
        #endregion
        #region Métodos
        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}