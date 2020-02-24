//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion

using System.Collections.Generic;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    /// <summary>
    /// Interface que implementa la sección del módulo de facturación que visualiza la información adicional
    /// </summary>
    public interface IucInformacionAdicionalVIS
    {
        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta de un dato adicional a agregar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string Etiqueta { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el valor de un dato adicional a agregar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string Valor { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa los datos adicionales agregados actualmente.
        /// </summary>
        /// <value>Objeto de tipo IList de DatosAdicionalesFacturaBO</value>
        IList<DatosAdicionalesFacturaBO> DatosAdicionales { get; set; }

        /// <summary>
        /// Visualiza la lista de datos adicionales agregados actualmente
        /// </summary>
        void MostrarListaDatosAdicionales();

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
                
         /// <summary>
        /// Limpia los datos de sesión
        /// </summary>
        void LimpiarSesion();
    }
}