using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionPistolaNeumaticaPSLVIS {
        #region propiedades
        /// <summary>
        /// Verifica si tiene ajuste de presión del compresor
        /// </summary>
        bool? TieneAjustePresionCompresor { get; set; }
        /// <summary>
        /// Verifica si tiene condición los bujes
        /// </summary>
        bool? TieneCondicionBujes { get; set; }
        /// <summary>
        /// Verifica si tiene condición el lubricador
        /// </summary>
        bool? TieneCondicionLubricador { get; set; }
        /// <summary>
        /// Verifica si tiene condición las mangueras
        /// </summary>
        bool? TieneCondicionMangueras { get; set; }
        /// <summary>
        /// Verifica si tiene condición Pica
        /// </summary>
        bool? TieneCondicionPica { get; set; }
        /// <summary>
        /// Verifica si tiene condición la pintura
        /// </summary>
        bool? TieneCondicionPintura { get; set; }
        /// <summary>
        /// Verifica si tiene estructura
        /// </summary>
        bool? TieneEstructura { get; set; }
        /// <summary>
        /// Verifica si tiene nivel de aceite el lubricador
        /// </summary>
        bool? TieneNivelAceiteLubricador { get; set; }
        #endregion

        #region Métodos

        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion
    }
}