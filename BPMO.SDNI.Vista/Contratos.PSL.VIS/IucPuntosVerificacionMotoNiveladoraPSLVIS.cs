using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionMotoNiveladoraPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene aceite diferencial
        /// </summary>
        bool? tieneAceiteDiferencial { get; set; }
        /// <summary>
        /// Verifica si tiene aceite de engranes gira circulo
        /// </summary>
        bool? tieneAceiteEngranesGiracirculo { get; set; }
        /// <summary>
        /// Verifica si tiene aceite hidráulico
        /// </summary>
        bool? tieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene aceite de mandos finales
        /// </summary>
        bool? tieneAceiteMandosFinales { get; set; }
        /// <summary>
        /// Verifica si tiene aceite motor
        /// </summary>
        bool? tieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene aceite de tandem
        /// </summary>
        bool? tieneAceiteTandem { get; set; }
        /// <summary>
        /// Verifica si tiene aceite de transmisión
        /// </summary>
        bool? tieneAceiteTransmision { get; set; }
        /// <summary>
        /// Verifica si tiene ajuste gira circulo de cuchilla
        /// </summary>
        bool? tieneAjusteGiracirculoCuchilla { get; set; }
        /// <summary>
        /// Verifica si tiene alarma de reversa
        /// </summary>
        bool? tieneAlarmaReversa { get; set; }
        /// <summary>
        /// Verifica si tiene antenas motor
        /// </summary>
        bool? TieneAntenasMonitoreo { get; set; }
        /// <summary>
        /// Verifica si tiene articulaciones chasis
        /// </summary>
        bool? tieneArticulacionesChasis { get; set; }
        /// <summary>
        /// Verifica si tiene articulaciones de cuchilla
        /// </summary>
        bool? tieneArticulacionesCuchilla { get; set; }
        /// <summary>
        /// Verifica si tiene articulaciones de dirección
        /// </summary>
        bool? tieneArticulacionesDireccion { get; set; }
        /// <summary>
        /// Verifica si tiene articulaciones de escarificador
        /// </summary>
        bool? tieneArticulacionesEscarificador { get; set; }
        /// <summary>
        /// Verifica si tiene articulaciones de ripper
        /// </summary>
        bool? tieneArticulacionesRipper { get; set; }
        /// <summary>
        /// Verifica si tiene banda en el ventilador
        /// </summary>
        bool? tieneBandaVentilador { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene filtro el cartucho
        /// </summary>
        bool? tieneCartuchoFiltro { get; set; }
        /// <summary>
        /// Verifica si tiene cinturón de seguridad
        /// </summary>
        bool? tieneCinturonSeguridad { get; set; }
        /// <summary>
        /// Verifica si tiene claxon
        /// </summary>
        bool? tieneClaxon { get; set; }
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
        bool? tieneCondicionLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? tieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene cuchilla
        /// </summary>
        bool? tieneCuchilla { get; set; }
        /// <summary>
        /// Verifica si tiene espejos retrovisores
        /// </summary>
        bool? tieneEspejosRetrovisores { get; set; }
        /// <summary>
        /// Verifica si tiene estructura chasis
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
        /// Verifica si tiene liquido refrigerante
        /// </summary>
        bool? tieneLiquidoRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene luces de advertencia
        /// </summary>
        bool? tieneLucesAdvertencia { get; set; }
        /// <summary>
        /// Verifica si tiene luces direccionales
        /// </summary>
        bool? tieneLucesDireccionales { get; set; }
        /// <summary>
        /// Verifica si tiene luces intermitentes
        /// </summary>
        bool? tieneLucesIntermitentes { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo en la parte delantera
        /// </summary>
        bool? tieneLucesTrabajoDelantera { get; set; }
        /// <summary>
        /// Verifica si tiene luces de trabajo en la parte trasera
        /// </summary>
        bool? tieneLucesTrabajoTrasera { get; set; }
        /// <summary>
        /// Verifica si tiene mangueras y abrazaderas
        /// </summary>
        bool? tieneManguerasAbrazaderas { get; set; }
        /// <summary>
        /// Verifica si la palanca tiene funciones hidráulicas
        /// </summary>
        bool? tienePalancaFuncionesHidraulicos { get; set; }
        /// <summary>
        /// Verifica si tiene palanca de transito
        /// </summary>
        bool? tienePalancaTransito { get; set; }
        /// <summary>
        /// Verifica si tiene presión en llantas
        /// </summary>
        bool? tienePresionEnLlantas { get; set; }
        /// <summary>
        /// Verifica si tiene ripper escarificador
        /// </summary>
        bool? tieneRipperEscarificador { get; set; }
        /// <summary>
        /// Verifica si tiene símbolo de seguridad
        /// </summary>
        bool? tieneSimbolosSeguridad { get; set; }
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
        /// Verifica si tiene velocidad máxima el motor
        /// </summary>
        bool? tieneVelocidadMaxMotor { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad mínima el motor
        /// </summary>
        bool? tieneVelocidadMinMotor { get; set; }
        #endregion
        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}