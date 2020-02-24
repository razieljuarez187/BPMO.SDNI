//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    /// <summary>
    /// Interface que implementa la sección del módulo de facturación que visualiza los datos en el panel de herramientas
    /// </summary>
    public interface IucHerramientasPrefacturaVIS
    {
        /// <summary>
        /// Obtiene o establece un valor que representa el número de contrato asociada a la factura
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string NumeroContrato { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el número de pago asociada a la factura
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        short? NumeroPago { get; set; }       

        /// <summary>
        /// Proceso que prepara la visualización de una nueva factura
        /// </summary>
        void PrepararNuevo();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
    }
}