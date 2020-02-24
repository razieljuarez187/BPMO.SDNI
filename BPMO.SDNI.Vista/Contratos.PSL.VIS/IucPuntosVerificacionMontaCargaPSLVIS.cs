using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionMontaCargaPSLVIS {
        /// <summary>
        /// Verifica si tiene aceite el eje delantero
        /// </summary>
        bool? TieneAceiteEjeDelantero { get; set; }
        /// <summary>
        /// Verifica si tienen aceite los frenos
        /// </summary>
        bool? TieneAceiteFrenos { get; set; }
        /// <summary>
        /// Verifica si tiene aceite hidráulico
        /// </summary>
        bool? TieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene aceite motor
        /// </summary>
        bool? TieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene aceite de transmisión
        /// </summary>
        bool? TieneAceiteTransmision { get; set; }
        /// <summary>
        /// Verifica si tiene alarma de reversa
        /// </summary>
        bool? TieneAlarmaReversa { get; set; }
        /// <summary>
        /// Verifica si tiene antenas de monitoreo
        /// </summary>
        bool? TieneAntenasMonitoreo { get; set; }
        /// <summary>
        /// Verifica si el ventilador tiene banda
        /// </summary>
        bool? TieneBandaVentilador { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? TieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene cartucho filtro
        /// </summary>
        bool? TieneCartuchoFiltro { get; set; }
        /// <summary>
        /// Verifica si tiene cinturón de seguridad
        /// </summary>
        bool? TieneCinturonSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene cofre el motor
        /// </summary>
        bool? TieneCofreMotor { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? TieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene condición las calas
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
        /// Verifica si tiene espejo retrovisor
        /// </summary>
        bool? TieneEspejoRetrovisor { get; set; }
        /// <summary>
        /// Verifica si tiene estructura chasis
        /// </summary>
        bool? TieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene freno de estacionamiento
        /// </summary>
        bool? TieneFrenoEstacionamiento { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores
        /// </summary>
        bool? TieneIndicadores { get; set; }
        /// <summary>
        /// Verifica si tiene lamparas de tablero
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
        /// Verifica si tiene mangueras y abrazaderas
        /// </summary>
        bool? TieneManguerasYAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si tiene operación mástil
        /// </summary>
        bool? TieneOperacionMastil { get; set; }
        /// <summary>
        /// Verifica si tiene palanca de avance de reversa
        /// </summary>
        bool? TienePalancaAvanceReversa { get; set; }
        /// <summary>
        /// Verifica si tiene pivote de articulación
        /// </summary>
        bool? TienePivoteArticulacion { get; set; }
        /// <summary>
        /// Verifica si tiene presión en las llantas
        /// </summary>
        bool? TienePresionEnLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool? TieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene tapa combustible
        /// </summary>
        bool? TieneTapaCombustible { get; set; }

        /// <summary>
        /// Verifica si tiene tapa del hidráulico
        /// </summary>
        bool? TieneTapaHidraulico { get; set; }

        /// <summary>
        /// Verifica si tiene condición el asiento
        /// </summary>
        bool? TieneCondicionAsiento { get; set; }


        /// <summary>
        /// Verifica la tensión cadena
        /// </summary>
        bool? TieneTensionCadena { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima el motor
        /// </summary>
        bool? TieneVelocidadMaxMotor { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima el motor
        /// </summary>
        bool? TieneVelocidadMinMotor { get; set; }

        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}