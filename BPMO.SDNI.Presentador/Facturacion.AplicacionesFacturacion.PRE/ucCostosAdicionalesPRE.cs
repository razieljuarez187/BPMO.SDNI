//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion
//Satisface al Reporte de Inconsistencia RI0024
// BEP1401 Satisface a la SC0030

using System;
using System.Linq;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.BR;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.GeneradorPaquetesFacturacion.BR;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BR;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE
{
    /// <summary>
    /// Presentador para la vista que visualiza los costos adicionales de una factura
    /// </summary>
    public class ucCostosAdicionalesPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx = null;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IucCostosAdicionalesVIS vista;

        /// <summary>
        /// Controlador para los pagos de unidad
        /// </summary>
        private PagoUnidadContratoBR pagoUnidadContratoBR;

        /// <summary>
        /// Controlador para los tipos de renglón de facturación
        /// </summary>
        private TipoRenglonFacturacionBR tipoRenglonFacturacionBR;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ucCostosAdicionalesPRE";
        #endregion

        #region Propiedades

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por default
        /// </summary>
        /// <param name="view">Vista a la que se asociada el presentador</param>
        public ucCostosAdicionalesPRE(IucCostosAdicionalesVIS view)
        {
            try
            {
                this.vista = view;
                this.dctx = FacadeBR.ObtenerConexion();
                this.pagoUnidadContratoBR = new PagoUnidadContratoBR();
                this.tipoRenglonFacturacionBR = new TipoRenglonFacturacionBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencia en los parámetros de configuración", ETipoMensajeIU.ERROR, this.nombreClase + ".ucContratoManttoPRE:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Agrega un costo adicional a la factura
        /// </summary>
        public void AgregarCostoAdicional()
        {
            //RI0024, Validación de precio mayor a cero
            if (this.vista.Precio <= 0)
                throw new Exception("El precio unitario del costo adicional debe de ser mayor a cero");
            if (string.IsNullOrEmpty(this.vista.DescripcionProductoServicio))
                throw new Exception("El producto o servicio es obligatorio.");

            IGeneradorPaquetesFacturacionBR generador = this.ObtenerGeneradorPaquetes(this.vista.TipoContrato.GetValueOrDefault());
            DetalleTransaccionBO detalle = new DetalleTransaccionBO() { ProductoServicio = new ProductoServicioBO() };     

            int? id = this.vista.CostosAdicionales
                        .Select(it => (int?)it.Id)
                        .Max();

            if (!id.HasValue)
                id = 0;
            id++;

            detalle.Id = id;
            detalle.Cantidad = 1;
            detalle.UnidadMedida = generador.UnidadMedida;
            detalle.CostoUnitario = generador.Costo;
            detalle.AplicaIVA = true;

            detalle.Articulo = new Basicos.BO.ArticuloBO();

            //SC0030
            detalle.Articulo.Nombre = String.Format("{0}: {1}", this.vista.Concepto, this.vista.Descripcion != null ? this.vista.Descripcion.ToUpper() : null);                    

            detalle.PrecioUnitario = this.vista.Precio;
            detalle.DescuentoUnitario = 0M;
            detalle.RetencionUnitaria = 0M;

            detalle.TipoRenglon = this.vista.Concepto;
            detalle.ProductoServicio.Id = this.vista.ProductoServicioId;
            detalle.ProductoServicio.NombreCorto = this.vista.ClaveProductoServicio;
            detalle.ProductoServicio.Nombre = this.vista.DescripcionProductoServicio;
            detalle.Activo = true;

            this.vista.CostosAdicionales.Add(detalle);
            this.vista.MostrarListaCostosAdicionales();
            this.Limpiar();           
        }
        
        /// <summary>
        /// Elimina un costo adicional a la factura
        /// </summary>
        /// <param name="id">Id del costo adicional a eliminar</param>
        public void EliminarCostoAdicional(int id)
        {
            DetalleTransaccionBO detalle = this.vista.CostosAdicionales
                                                .Where(it => it.Id == id)
                                                .FirstOrDefault();

            if (detalle != null)
                this.vista.CostosAdicionales.Remove(detalle);

            this.vista.MostrarListaCostosAdicionales();
        }

        /// <summary>
        /// Limpia los datos del costo adicional capturado
        /// </summary>
        private void Limpiar()
        {
            this.vista.Descripcion = null;
            this.vista.Precio = null;
            this.vista.Concepto = null;
            this.vista.ProductoServicioId = null;
            this.vista.ClaveProductoServicio = string.Empty;
            this.vista.DescripcionProductoServicio = string.Empty;
        }

        /// <summary>
        /// Proceso que se ejecuta cuando se crea una nueva factura
        /// </summary>
        public void PrepararNuevo()
        {
            this.vista.MostrarListaTiposRenglon(this.tipoRenglonFacturacionBR.Consultar(this.vista.ObtenerCarpetaRaiz()));
        }

        /// <summary>
        /// Obtiene el paquete de facturación según un tipo de comprobante
        /// </summary>
        /// <param name="tipoContrato">Tipo de comporbante</param>
        /// <returns>Generador de paquetes de facturación</returns>
        private IGeneradorPaquetesFacturacionBR ObtenerGeneradorPaquetes(ETipoContrato tipoContrato)
        {
            IGeneradorPaquetesFacturacionBR generador = null;
            switch (tipoContrato)
            {
                case ETipoContrato.FSL:
                    generador = new GeneradorPaqueteFacturacionFSLBR();
                    break;

                case ETipoContrato.RD:
                    generador = new GeneradorPaqueteFacturacionRDBR();
                    break;

                case ETipoContrato.CM:
                    generador = new GeneradorPaqueteFacturacionManttoBR();
                    break;

                case ETipoContrato.SD:
                    generador = new GeneradorPaqueteFacturacionManttoBR();
                    break;
            }

            return generador;
        }

        #region Métodos Buscador
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "ProductoServicio":
                    ProductoServicioBO producto = new ProductoServicioBO() { Activo = true };

                    if (!string.IsNullOrEmpty(vista.ClaveProductoServicio)) {
                        int auxNum = 0;
                        if (Int32.TryParse(vista.ClaveProductoServicio, out auxNum))
                            producto.NombreCorto = vista.ClaveProductoServicio;
                        else
                            producto.Nombre = vista.ClaveProductoServicio;
                    }

                    obj = producto;
                    break;
            }

            return obj;
        }
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "ProductoServicio":
                    ProductoServicioBO producto = (ProductoServicioBO)selecto ?? new ProductoServicioBO();
                    vista.ProductoServicioId = producto.Id;
                    vista.ClaveProductoServicio = producto.NombreCorto;
                    vista.DescripcionProductoServicio = producto.Nombre;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
