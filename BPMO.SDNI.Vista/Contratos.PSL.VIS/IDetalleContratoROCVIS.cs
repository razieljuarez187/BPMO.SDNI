using System;
using System.Collections.Generic;
using System.Xml;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IDetalleContratoROCVIS {
        #region Propiedades
        int? ContratoID { get; set; }
        object UltimoObjeto { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        int? EstatusID { get; set; }

        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Unidad Operativa sobre la que trabaja el usuario en el sistema
        /// </summary>
        int? UnidadOperativaID { get; }

        string UnidadOperativaNombre { get; }

        DateTime? FechaContrato { get; set; }
        string NumeroContrato { get; set; }
        SucursalBO SucursalSeleccionada { get; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteTipoID { get; set; }
        int? ClienteDireccionId { get; set; } //Id de la Dirección del cliente
        string ClienteDireccionCalle { get; set; }
        string ClienteDireccionCodigoPostal { get; set; }
        string ClienteDireccionCiudad { get; set; }
        string ClienteDireccionEstado { get; set; }
        string ClienteDireccionMunicipio { get; set; }
        string ClienteDireccionPais { get; set; }
        string ClienteDireccionColonia { get; set; }

        bool? SoloRepresentantes { get; set; }
        List<RepresentanteLegalBO> RepresentantesLegales { get; set; }
        List<AvalBO> Avales { get; set; }
        string CodigoMoneda { get; set; }

        int? TipoConfirmacionID { get; set; }
        int? FormaPagoID { get; set; }
        int? FrecuenciaFacturacionID { get; }
        string AutorizadorTipoConfirmacion { get; set; }
        string AutorizadorOrdenCompra { get; set; }
        string MercanciaTransportar { get; set; }
        string DestinoAreaOperacion { get; set; }
        DateTime? FechaPromesaDevolucion { get; set; }
        string Observaciones { get; set; }

        /// <summary>
        /// Número de días que se cobrarán en la primera Factura
        /// </summary>
        Int32? DiasFacturar { get; }

        //Línea del Contrato
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        int? ProductoServicioId { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }

        List<LineaContratoPSLBO> LineasContrato { get; set; }
        ETipoContrato? TipoContrato { get; set; }
        #region CheckList
        int? ModuloID { get; }
        /// <summary>
        /// Obtiene o establece el identificador de la línea de contrato a la cual pertenece el check List
        /// </summary>
        int? LineaContratoID { get; set; }
        /// <summary>
        /// Obtiene o establece l tipo de check list que se va a registrar
        /// </summary>
        int? TipoListadoVerificacionPSL { get; set; }
        #endregion
        #endregion

        #region Métodos
        void PrepararNuevo();
        void PrepararVisualizacion();

        void RedirigirSinPermisoAcceso();
        void RedirigirAConsulta();
        void RedirigirADetalles();
        void LimpiarSesion();
        void EstablecerPaqueteNavegacion(string key, object value);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        void RedirigirAEdicion(bool lEnCurso);
        void RedirigirARenovar();
        void RedirigirAImprimirContrato();
        /// <summary>
        /// Despliega la Línea del Contrato
        /// </summary>
        void CambiarALinea();
        /// <summary>
        /// Despliega los detalles del contrato
        /// </summary>
        void CambiaAContrato();

        void RedirigirAIntercambioUnidad();

        void RedirigirACierre();
        void RedirigirAImprimir();
        void EstablecerPaqueteNavegacionReporte(string key, object value);

        /// <summary>
        /// Obtiene los datos de navegación para desplegar en la ventana
        /// </summary>
        /// <returns>Objeto de regreso con información</returns>
        object ObtenerDatosNavegacion();

        /// <summary>
        /// Permite habilitar o deshabilitar el botón de registrar
        /// </summary>
        /// <param name="permitir">Indica si se habilita o deshabilita el botón</param>
        void PermitirRegistrar(bool permitir);
        /// <summary>
        /// Permite habilitar o deshabilitar el botón de editar
        /// </summary>
        /// <param name="permitir">Indica si se habilita o deshabilita el botón</param>
        void PermitirEditar(bool permitir);
        /// <summary>
        /// Permite habilitar o deshabilitar el botón de editar
        /// </summary>
        /// <param name="permitir">Indica si se habilita o deshabilita el botón</param>
        void PermitirGenerarPago(bool permitir);
        /// <summary>
        /// Obtiene la plantilla xml del reporte
        /// </summary>
        /// <returns>XML de la plantilla</returns>
        XmlDocument ObtenerXmlReporte();

        #endregion
    }
}
