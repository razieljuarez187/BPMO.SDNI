using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionCompresoresPortatilesPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene aceite el compresor
        /// </summary>
        bool? tieneAceiteCompresor { get; set; }
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
        /// Verifica si tiene barra tiro
        /// </summary>
        bool? tieneBarraTiro { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene botón de servicio de aire
        /// </summary>
        bool? tieneBtnServicioAire { get; set; }
        /// <summary>
        /// Verifica si tiene filtro el cartucho
        /// </summary>
        bool? tieneCartuchoFiltro { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? tieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene calcas
        /// </summary>
        bool? tieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tienen condición las llantas
        /// </summary>
        bool? tieneCondicionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? tieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene estructura el chasis
        /// </summary>
        bool? tieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores
        /// </summary>
        bool? tieneIndicadores { get; set; }
        /// <summary>
        /// Verifica si tiene lamparas el tablero
        /// </summary>
        bool? tieneLamparasTablero { get; set; }
        /// <summary>
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool? tieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de transito
        /// </summary>
        bool? tieneLucesTransito { get; set; }
        /// <summary>
        /// Verifica si tiene mangueras y abrazaderas
        /// </summary>
        bool? tieneManguerasYAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si tiene manómetro presión
        /// </summary>
        bool? tieneManometroPresion { get; set; }
        /// <summary>
        /// Verifica si tiene presión en llantas
        /// </summary>
        bool? tienePresionEnLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool? tieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene switch de arranque
        /// </summary>
        bool? tieneSwitchArranque { get; set; }
        /// <summary>
        /// Verifica si tiene tacómetro
        /// </summary>
        bool? tieneTacometro { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el combustible
        /// </summary>
        bool? tieneTapaCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima el motor
        /// </summary>
        bool? tieneVelocidadMaxMotor { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima el motor
        /// </summary>
        bool? tieneVelocidadMinMotor { get; set; }

        string lubricacion { get; set; }
        #endregion
        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}