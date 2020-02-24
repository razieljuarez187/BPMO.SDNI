using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionRetroExcavadoraPSLVIS {
        #region propiedades

        /// <summary>
        /// Verifica si tiene aceite de transmisión
        /// </summary>
        bool tieneAceiteTransmision { get; set; }
        /// <summary>
        /// Verifica si tiene aceite diferencial adelante
        /// </summary>
        bool tieneAceiteDiferencialDel { get; set; }
        /// <summary>
        /// Verifica si tiene aceite diferencial trasero
        /// </summary>
        bool tieneAceiteDiferencialTrasero { get; set; }
        /// <summary>
        /// verifica si tiene aceite hidráulico
        /// </summary>
        bool tieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene aceite en el motor
        /// </summary>
        bool tieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene aceite de planetarios en la parte delantera
        /// </summary>
        bool tieneAceitePlanetariosDel { get; set; }
        /// <summary>
        /// Verifica si tiene alarma de reversa
        /// </summary>
        bool tieneAlarmaReversa { get; set; }
        /// <summary>
        /// Verifica si tiene antena de monitoreo
        /// </summary>
        bool tieneAntenaMonitoreo { get; set; }
        /// <summary>
        /// Verifica si tiene asiento el operador
        /// </summary>
        bool tieneAsientoOperador { get; set; }
        /// <summary>
        /// Verifica si tiene banda el ventilador
        /// </summary>
        bool tieneBandaVentilador { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene bloqueo diferencial
        /// </summary>
        bool tieneBloqueoDiferencial { get; set; }
        /// <summary>
        /// Verifica si tiene bote delantero
        /// </summary>
        bool tieneBoteDelantero { get; set; }
        /// <summary>
        /// Verifica si tiene bote trasero
        /// </summary>
        bool tieneBoteTrasero { get; set; }
        /// <summary>
        /// Verifica si tiene bujes pasadores en el cargador
        /// </summary>
        bool tieneBujesPasadoresCargador { get; set; }
        /// <summary>
        /// Verifica si tiene bujes pasadores retro
        /// </summary>
        bool tieneBujesPasadoresRetro { get; set; }
        /// <summary>
        /// Verifica si tiene cartucho de filtro de aire
        /// </summary>
        bool tieneCartuchoFiltroAire { get; set; }
        /// <summary>
        /// Verifica si tiene cinturón de seguridad
        /// </summary>
        bool tieneCinturonSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene claxon
        /// </summary>
        bool tieneClaxon { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool tieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene condición el asiento
        /// </summary>
        bool tieneCondicionAsiento { get; set; }
        /// <summary>
        /// Verifica si tiene condición las calcas
        /// </summary>
        bool tieneCondicionCalcas { get; set; }
        /// <summary>
        /// Verifica si tienen condición las llantas
        /// </summary>
        bool tieneCondicionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool tieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene desacople
        /// </summary>
        bool tieneDesacople { get; set; }
        /// <summary>
        /// Verifica si tiene espejo retrovisor
        /// </summary>
        bool tieneEspejoRetrovisor { get; set; }
        /// <summary>
        /// Verifica si tiene estructura el chasis
        /// </summary>
        bool tieneEstructuraChasis { get; set; }
        /// <summary>
        /// Verifica si tiene freno de estacionamiento
        /// </summary>
        bool tieneFrenoEstacionamiento { get; set; }
        /// <summary>
        /// Verifica si tiene indicadores
        /// </summary>
        bool tieneIndicadores { get; set; }
        /// <summary>
        /// Verifica si tiene joystick
        /// </summary>
        bool tieneJoystick { get; set; }
        /// <summary>
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool tieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de advertencia
        /// </summary>
        bool tieneLucesAdvertencia { get; set; }
        /// <summary>
        /// Verifica si tiene luces direccionales
        /// </summary>
        bool tieneLucesDireccionales { get; set; }
        /// <summary>
        /// Verifica si tiene luces intermitentes
        /// </summary>
        bool tieneLucesIntermitentes { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo delanteras
        /// </summary>
        bool tieneLucesTrabajoDelantera { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo traseras
        /// </summary>
        bool tieneLucesTrabajoTraseras { get; set; }
        /// <summary>
        /// Verifica si tiene mangueras abrazaderas
        /// </summary>
        bool tieneManguerasAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si tiene neutralizador de transmisión
        /// </summary>
        bool tieneNeutralizadorTransmision { get; set; }
        /// <summary>
        /// Verifica si tiene palanca de transito
        /// </summary>
        bool tienePalancaTransito { get; set; }
        /// <summary>
        /// Verifica si tienen presión las llantas
        /// </summary>
        bool tienePresionLLantas { get; set; }
        /// <summary>
        /// Verifica si tiene símbolos de seguridad
        /// </summary>
        bool tieneSimbolosSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene tacómetro
        /// </summary>
        bool tieneTacometro { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el combustible
        /// </summary>
        bool tieneTapaCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene tapa el hidráulico
        /// 
        /// </summary>
        bool tieneTapaHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad máxima
        /// </summary>
        bool tieneVelocidadMaxima { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima
        /// </summary>
        bool tieneVelocidadMinima { get; set; }
        /// <summary>
        /// Verifica si tiene Zapatas estabilizadoras
        /// </summary>
        bool tieneZapatasEstabilizadores { get; set; }

        #endregion

        #region Métodos
        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}