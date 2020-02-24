//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion
// Satisface a la solicitud de cambio SC0016

using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using System;
using System.Collections.Generic;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    /// <summary>
    /// Interface que implementa la sección del módulo de facturación que visualiza los costos de la cabezera
    /// </summary>
    public interface IucInformacionCabeceraVIS
    {
        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando la forma de pago ha cambiado
        /// </summary>
        event EventHandler FormaPagoChanged;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la sucursal
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de una sucursal
        /// </summary>
        /// <value>Objeto de tip String</value>
        string SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el sistema origen
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string SistemaOrigen { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de transaccion asociada 
        /// </summary>
        /// <value>Objeto de tipo ETipoContrato</value>
        ETipoContrato? TipoTransaccion { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el número de referencia asociada a una transacción
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string NumeroReferencia { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el código de la moneda destino 
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string CodigoMoneda { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la forma de pago
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string FormaPago { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de cambio para la factura
        /// </summary>
        /// <value>Objeto de tipo Decimal</value>
        decimal? TipoCambio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa los dias de factura 
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? DiasFactura { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de taza cambaria
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string TipoTasaCambiario { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el número de días de crédito
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? DiasCredito { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el límite de crédito
        /// </summary>
        /// <value>Objeto de tipo Decimal</value>
        decimal? LimiteCredito { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el crédito disponible
        /// </summary>
        /// <value>Objeto de tipo Decimal</value>
        decimal? CreditoDisponible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el departamento asociado 
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string Departamento { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa si se aplica el flag BanderaCores
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool BanderaCores { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Proceso que prepara la visualización de una nueva factura
        /// </summary>
        void PrepararNuevo();

        /// <summary>
        /// Visualiza las formas de pago para una factura
        /// </summary>
        /// <param name="formasPago">Lista de formas de pago</param>
        void MostrarFormasPago(IList<String> formasPago);

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        /// <summary>
        /// Obtiene la carpeta raiz donde se encuentra la aplicación
        /// </summary>
        /// <returns>Dirección donde se encuentra la aplicación</returns>
        string ObtenerCarpetaRaiz();
        #endregion
    }
}