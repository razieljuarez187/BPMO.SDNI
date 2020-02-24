//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using System.Collections.Generic;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    /// <summary>
    /// Interface que implementa la sección del módulo de facturación que visualiza las líneas de contrato
    /// </summary>
    public interface IucLineasFacturaVIS
    {
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
        /// Proceso que prepara la visualización de una nueva factura
        /// </summary>
        void PrepararNuevo();       

        /// <summary>
        /// Vlsualiza la lista de lineas de la factura en cusro
        /// </summary>
        /// <param name="list">Lista de objeto de detalle de linea de transacción</param>
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
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}