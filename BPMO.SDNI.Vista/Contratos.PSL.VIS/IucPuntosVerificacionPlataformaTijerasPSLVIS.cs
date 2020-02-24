using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionPlataformaTijerasPSLVIS {
        /// <summary>
        /// Verifica si tiene aceite hidráulico
        /// </summary>
        bool? tieneAceiteHidraulico { get; set; }
        /// <summary>
        /// Verifica si tiene aceite el  motor
        /// </summary>
        bool? tieneAceiteMotor { get; set; }
        /// <summary>
        /// Verifica si tiene almohadillas
        /// </summary>
        bool? tieneAlmohadillas { get; set; }
        /// <summary>
        /// Verifica si tiene barras de dirección
        /// </summary>
        bool? tieneBarrasDireccion { get; set; }
        /// <summary>
        /// Verifica si tiene batería
        /// </summary>
        bool? tieneBateria { get; set; }
        /// <summary>
        /// Verifica si tiene brazos de tijera
        /// </summary>
        bool? tieneBrazosTijera { get; set; }
        /// <summary>
        /// Verifica si tiene chasis
        /// </summary>
        bool? tieneChasis { get; set; }
        /// <summary>
        /// Verifica si tiene cilindro de dirección
        /// </summary>
        bool? tieneCilindroDireccion { get; set; }
        /// <summary>
        /// Verifica si tiene cilindro elevador
        /// </summary>
        bool? tieneCilindroElevador { get; set; }
        /// <summary>
        /// Verifica si tiene combustible
        /// </summary>
        bool? tieneCombustible { get; set; }
        /// <summary>
        /// Verifica si tiene conjunto barandillas
        /// </summary>
        bool? tieneConjuntoBarandillas { get; set; }
        /// <summary>
        /// Verifica si tiene conjunto de neumáticos
        /// </summary>
        bool? tieneConjuntoNeumaticosRuedas { get; set; }
        /// <summary>
        /// Verifica si tiene controles en la plataforma
        /// </summary>
        bool? tieneControlesPlataforma { get; set; }
        /// <summary>
        /// Verifica si tiene controles de tierra
        /// </summary>
        bool? tieneControlesTierra { get; set; }
        /// <summary>
        /// Verifica si tiene faros de emergencia
        /// </summary>
        bool? tieneFarosEmergencia { get; set; }
        /// <summary>
        /// Verifica si tiene pasadores de pivote
        /// </summary>
        bool? tienePasadoresPivote { get; set; }
        /// <summary>
        /// Verifica si tiene pivotes de dirección
        /// </summary>
        bool? tienePivotesDireccion { get; set; }
        /// <summary>
        /// Verifica si tiene plataforma
        /// </summary>
        bool? tienePlataforma { get; set; }
        /// <summary>
        /// Verifica si tiene prueba de switch pothole
        /// </summary>
        bool? tienePruebaSwitchPothole { get; set; }
        /// <summary>
        /// Verifica si tiene reductores de transito
        /// </summary>
        bool? tieneReductoresTransito { get; set; }
        /// <summary>
        /// Verifica si tiene refrigerante
        /// </summary>
        bool? tieneRefrigerante { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad de transito extendida
        /// </summary>
        bool? tieneVelocidadTransitoExtendida { get; set; }
        /// <summary>
        /// Verifica si tiene velocidad de transito retraída
        /// </summary>
        bool? tieneVelocidadTransitoRetraida { get; set; }

        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}