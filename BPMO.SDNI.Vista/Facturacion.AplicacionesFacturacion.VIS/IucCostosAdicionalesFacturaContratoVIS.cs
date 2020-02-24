using System;
using System.Collections.Generic;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.eFacturacion.Procesos.Enumeradores;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS {
    /// <summary>
    /// Interface que implementa la sección del módulo de facturación que visualiza los costos adicionales
    /// </summary>
    public interface IucCostosAdicionalesFacturaContratoVIS {
        /// <summary>
        /// Obtiene o establece la moneda de destino asociada a la prefactua
        /// </summary>
        /// <value>
        /// Objeto de tipo String 
        /// </value>
        String CodigoMoneda { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de renglón de un concepto a agregar
        /// </summary>
        /// <value>Objeto de tipo ETipoRenglon</value>
        ETipoRenglon Concepto { get; set; }

        /// <summary>
        /// Obtiene o establece el precio de un concepto a agregar
        /// </summary>
        /// <value>Objeto de tipo decimal</value>
        Decimal? Precio { get; set; }

        /// <summary>
        /// Identificador de Producto-Servicio
        /// </summary>
        int? ProductoServicioId { get; set; }

        /// <summary>
        /// Clave del Producto o servicio (Catálogo SAT)
        /// </summary>
        string ClaveProductoServicio { get; set; }

        /// <summary>
        /// Descripción del Producto o Servicio (Catálogo SAT)
        /// </summary>
        string DescripcionProductoServicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la descripción de un concepto a agregar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de contrato de la factura en curso
        /// </summary>
        /// <value>Objeto de tipo ETipoContrato</value>
        ETipoContrato? TipoContrato { get; }

        /// <summary>
        /// Obtiene o establece la lista de costos adicionales que se han agregado
        /// </summary>
        /// <value>Objeto de tipo IList de DetalleTransaccionBO</value>
        IList<DetalleTransaccionBO> CostosAdicionales { get; set; }

        /// <summary>
        /// Visualiza los renglones permitidos para los costos adicionales
        /// </summary>
        /// <param name="tiposRenglon">Lista de tipo ETipoRenglon</param>
        void MostrarListaTiposRenglon(IList<ETipoRenglon> tiposRenglon);

        /// <summary>
        /// Visualiza los costos adicionales actualmente
        /// </summary>
        void MostrarListaCostosAdicionales();

        void MostrarCostoAdicionalFactura(int pagoUnidadPSLID);

        object LineaCostoFacturaModel { get; set; }

        int PagoContratoID { get; set; }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        /// <summary>
        /// Proceso que prepara la visualización de una nueva factura
        /// </summary>
        void PrepararNuevo();

        /// <summary>
        /// Limpia los datos de sesión
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Regresa el directorio raíz de la aplicación en curso
        /// </summary>
        /// <returns>Objeto de tipo String</returns>
        String ObtenerCarpetaRaiz();

        /// <summary>
        /// Establece las monedas que serán visibles para enviar a facturar
        /// </summary>
        /// <param name="monedas">Diccionario con las monedas a visualizar</param>
        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);

        /// <summary>
        /// Realiza un llamado al Método del Presentador para agregar un costo adicional
        /// </summary>
        void AgregarCostoAdicional();

        /// <summary>
        /// Permite habilitar / deshabilitar los controles del grid para agregar conceptos
        /// </summary>
        /// <param name="habilitar">Indica si se habilita o deshabilita el control</param>
        void permitirCaptura(bool habilitar);
    }
}