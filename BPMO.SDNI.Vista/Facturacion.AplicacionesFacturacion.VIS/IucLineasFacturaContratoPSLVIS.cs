using System.Collections.Generic;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS {
    /// <summary>
    /// Interface que implementa la sección del módulo de facturación que visualiza las líneas de contrato
    /// </summary>
    public interface IucLineasFacturaContratoPSLVIS {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el número de líneas a facturar
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? Lineas { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el subtotal
        /// </summary>
        /// <value>Objeto de tipo Decimal</value>
        decimal? SubTotal { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el impuesto de la factura
        /// </summary>
        /// <value>Objeto de tipo Decimal</value>
        decimal? Impuesto { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el total de la factura
        /// </summary>
        /// <value>Objeto de Decimal</value>
        decimal? TotalFactura { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del Uso CFDI (SAT)
        /// </summary>
        int? UsoCFDIId { get; set; }

        /// <summary>
        /// Obtiene o establece la clave del Uso CFDI (SAT)
        /// </summary>
        string ClaveUsoCFDI { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del Uso CFDI (SAT)
        /// </summary>
        string DescripcionUsoCFDI { get; set; }
        #endregion

        #region Métodos

        /// <summary>
        /// Visualiza la lista de líneas de la factura en curso
        /// </summary>
        /// <param name="list">Lista de objeto de detalle de línea de transacción</param>
        void MostrarLineasContrato(IList<DetalleTransaccionBO> list);

        /// <summary>
        /// Visualiza los total de la transacción en curso
        /// </summary>
        /// <param name="transaccion">Transacción en curso</param>
        void MostrarTotales(TransaccionBO transaccion);

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}