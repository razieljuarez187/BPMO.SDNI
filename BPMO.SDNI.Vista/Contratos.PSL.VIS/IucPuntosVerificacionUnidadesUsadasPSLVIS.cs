using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucPuntosVerificacionUnidadesUsadasPSLVIS {
        #region
        /// <summary>
        /// Verifica si tiene copia de bitácora de servicio
        /// </summary>
        bool? tieneCopiaBitacoraServicio { get; set; }
        /// <summary>
        /// Verifica si tiene copia de pedimento
        /// </summary>
        bool? tieneCopiaPedimento { get; set; }
        /// <summary>
        /// Verifica si tiene factura YXml fiscal
        /// </summary>
        bool? tieneFacturaYXmlFiscal { get; set; }
        /// <summary>
        /// Verifica si tiene fotografías internas o externas en la unidad
        /// </summary>
        bool? tieneFotografiasInternaExternaUnidad { get; set; }
        /// <summary>
        /// Verifica si tiene lavado exterior
        /// </summary>
        bool? tieneLavadoExterior { get; set; }
        /// <summary>
        /// Verifica si tiene lavado interior
        /// </summary>
        bool? tieneLavadoInterior { get; set; }
        #endregion
        #region Métodos
        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}