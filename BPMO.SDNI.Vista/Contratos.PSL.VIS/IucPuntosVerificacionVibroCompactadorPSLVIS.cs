using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionVibroCompactadorPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene aceite hidráulico
        /// </summary>
        bool? tieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene aceite en el motor
        /// </summary>
        bool? tieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene alarma de reversa
        /// </summary>
        bool? tieneAlarmaReversa { get; set; }
        /// <summary>
        /// Verifica si tiene antena de monitoreo
        /// </summary>
        bool? tieneAntenaMonitoreo { get; set; }
        /// <summary>
        /// Verifica si tiene banda el ventilador
        /// </summary>
        bool? tieneBandaVentilador { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene cabina el operador
        /// </summary>
        bool? tieneCabinaOperador { get; set; }
        /// <summary>
        /// Verifica si tiene caja de reducción de engranes
        /// </summary>
        bool? tieneCajaReduccionEngranes { get; set; }
        /// <summary>
        /// Verifica si tiene cartucho de filtro de aire
        /// </summary>
        bool? tieneCartuchoFiltroAire { get; set; }
        /// <summary>
        /// Verifica si tiene cinturón de seguridad
        /// </summary>
        bool? tieneCinturonSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene cofre el motor
        /// </summary>
        bool? tieneCofreMotor { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? tieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene condición el asiento
        /// </summary>
        bool? tieneCondicionAsiento { get; set; }
        /// <summary>
        /// Verifica si tienen condición las calcas
        /// </summary>
        bool? tieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tienen condición las llantas
        /// </summary>
        bool? tieneCondicionLlanta { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? tieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene estructura el chasis
        /// </summary>
        bool? tieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene freno de estacionamiento
        /// </summary>
        bool? tieneFrenoEstacionamiento { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores
        /// </summary>
        bool? tieneIndicadores { get; set; }
        /// <summary>
        /// Verifica si tiene interruptor de desconexión
        /// </summary>
        bool? tieneInterruptorDesconexion { get; set; }
        /// <summary>
        /// Verifica si tiene lampara el tablero
        /// </summary>
        bool? tieneLamparaTablero { get; set; }
        /// <summary>
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool? tieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de advertencia
        /// </summary>
        bool? tieneLucesAdvertencia { get; set; }
        /// <summary>
        /// Verifica si tiene luces delanteras
        /// </summary>
        bool? tieneLucesTrabajoDelantera { get; set; }
        /// <summary>
        /// Verifica si tiene luces traseras
        /// </summary>
        bool? tieneLucesTrabajoTraseras { get; set; }
        /// <summary>
        /// Verifica si tiene mangueras abrazaderas de admisión de aire
        /// </summary>
        bool? tieneManguerasAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si tiene palanca
        /// </summary>
        bool? tienePalanca { get; set; }
        /// <summary>
        /// Verifica si tiene pivote de articulación de dirección
        /// </summary>
        bool? tienePivoteArticulacionDireccion { get; set; }
        /// <summary>
        /// Verifica si tiene presión las llantas
        /// </summary>
        bool? tienePresionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene rascadores el tambor
        /// </summary>
        bool? tieneRascadoresTambor { get; set; }
        /// <summary>
        /// Verifica si tiene reductor de engranes
        /// </summary>
        bool? tieneReductorEngranes { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool? tieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene sistema de vibración
        /// </summary>
        bool? tieneSistemaVibracion { get; set; }
        /// <summary>
        /// Verifica si tiene tacómetro
        /// </summary>
        bool? tieneTacometro { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el combustible
        /// </summary>
        bool? tieneTapaCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el hidráulico
        /// </summary>
        bool? tieneTapaHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima
        /// </summary>
        bool? tieneVelocidadMaxima { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima
        /// </summary>
        bool? tieneVelocidadMinima { get; set; }
        /// <summary>
        /// Verifica si tiene vibrador
        /// </summary>
        bool? tieneVibrador { get; set; }
        #endregion
        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}