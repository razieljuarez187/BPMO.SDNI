using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IRegistrarContratoROCVIS {
        #region Propiedades
        int? ContratoID { get; set; }
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
        SucursalBO SucursalSeleccionada { get; }

        DateTime? FechaContrato { get; set; }
        string NumeroContrato { get; set; }
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
        DateTime? FechaInicioArrendamiento { get; set; }
        DateTime? FechaPromesaDevolucion { get; set; }
        DateTime? FechaInicioActual { get; set; }
        DateTime? FechaPromesaActual { get; set; }
        string Observaciones { get; set; }
        bool? IncluyeSD { get; set; }

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
        string ClaveProductoSATPredeterminado { get; }

        #region Campos ROC
        decimal? MontoTotalArrendamiento { get; set; }
        DateTime? FechaPagoRenta { get; set; }
        int? Plazo { get; set; }
        decimal? InversionInicial { get; set; }
        #endregion

        List<LineaContratoPSLBO> LineasContrato { get; set; }
        int? ModuloID { get; }
        #endregion

        #region Métodos
        void PrepararNuevo();

        void PermitirGuardarBorrador(bool permitir);
        void PermitirGuardarTerminado(bool permitir);

        void RedirigirSinPermisoAcceso();
        void RedirigirAConsulta();
        void RedirigirADetalles();
        void LimpiarSesion();
        void EstablecerPaqueteNavegacion(string key, object value);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        /// <summary>
        /// Despliega la Línea del Contrato
        /// </summary>
        void CambiarALinea();
        /// <summary>
        /// Despliega los detalles del contrato
        /// </summary>
        void CambiaAContrato();
        #endregion
    }
}
