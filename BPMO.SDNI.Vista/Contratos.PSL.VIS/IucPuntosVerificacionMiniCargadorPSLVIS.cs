using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionMiniCargadorPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene aceite en el compartimiento de la cadena
        /// </summary>
        bool? TieneAceiteCompartimientoCadena { get; set; }
        /// <summary>
        /// Verifica si tiene aceite hidráulico
        /// </summary>
        bool? TieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene aceite el motor
        /// </summary>
        bool? TieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene antena de monitoreo
        /// </summary>
        bool? TieneAntenaMonitoreo { get; set; }
        /// <summary>
        /// Verifica si tiene banda el ventilador
        /// </summary>
        bool? TieneBandaVentilador { get; set; }
        /// <summary>
        /// Verifica si tiene barra de seguridad
        /// </summary>
        bool? TieneBarraSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? TieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene el cartucho filtro
        /// </summary>
        bool? TieneCartuchoFiltro { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? TieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene condición el asiento
        /// </summary>
        bool? TieneCondicionAsiento { get; set; }
        /// <summary>
        /// Verifica si tienen condición las calcas
        /// </summary>
        bool? TieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tienen condición las llantas
        /// </summary>
        bool? TieneCondicionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? TieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene cople mecánico
        /// </summary>
        bool? TieneCopleMecanico { get; set; }
        /// <summary>
        /// Verifica si tiene estructura el chasis
        /// </summary>
        bool? TieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene el freno de estacionamiento
        /// </summary>
        bool? TieneFrenoEstacionamiento { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores
        /// </summary>
        bool? TieneIndicadores { get; set; }
        /// <summary>
        /// Verifica si tiene interruptor de desconexión
        /// </summary>
        bool? TieneInterruptorDesconexion { get; set; }
        /// <summary>
        /// Verifica si tiene lamparas en el tablero
        /// </summary>
        bool? TieneLamparasTablero { get; set; }
        /// <summary>
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool? TieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de advertencia
        /// </summary>
        bool? TieneLucesAdvertencia { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo delanteras
        /// </summary>
        bool? TieneLucesTrabajoDelantera { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo traseras
        /// </summary>
        bool? TieneLucesTrabajoTrasera { get; set; }
        /// <summary>
        /// Verifica si tiene mangueras y abrazadores
        /// </summary>
        bool? TieneMaguerasYAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si tiene palancas
        /// </summary>
        bool? TienePalancas { get; set; }
        /// <summary>
        /// Verifica si tiene pasadores de cargador
        /// </summary>
        bool? TienePasadoresCargador { get; set; }
        /// <summary>
        /// Verifica si tiene presión en las llantas
        /// </summary>
        bool? TienePresionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool? TieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene tacómetro
        /// </summary>
        bool? TieneTacometro { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el combustible
        /// </summary>
        bool? TieneTapaCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el hidráulico
        /// </summary>
        bool? TieneTapaHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima el motor
        /// </summary>
        bool? TieneVelocidadMaxMotor { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima el motor
        /// </summary>
        bool? TieneVelocidadMinMotor { get; set; }
        #endregion
        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}