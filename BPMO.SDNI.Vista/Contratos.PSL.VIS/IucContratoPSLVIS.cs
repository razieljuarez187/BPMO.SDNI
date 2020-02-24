using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucContratoPSLVIS {
        #region Propiedades
        int? ContratoID { get; set; }

        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        int? ModuloID { get; }
        decimal? TasaInteres { get; set; }

        int? EstatusID { get; set; }

        DateTime? FechaContrato { get; set; }
        string NumeroContrato { get; set; }

        int? UnidadOperativaID { get; }
        UnidadOperativaBO UnidadOperativa { get; }
        string NombreEmpresa { get; set; }
        string Session_DomicilioEmpresa { get; set; }
        string DomicilioEmpresa { get; set; }
        string RepresentanteEmpresa { get; set; }

        SucursalBO SucursalSeleccionada { get; }
        List<SucursalBO> SucursalesAutorizadas { get; set; }
        List<MonedaBO> ListaMonedas { get; set; }
        
        int? ClienteID { get; set; }
        bool? ClienteEsFisica { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteTipoID { get; set; }
        string CuentaClienteNumeroCuenta { get; set; } //Se agrega el Número de Cuenta de Oracle
        int? ClienteDireccionClienteID { get; set; } //Id de DireccionCliente
        string ClienteRFC { get; set; }
        string ClienteDireccionCompleta { get; set; }
        string ClienteDireccionCalle { get; set; }
        string ClienteDireccionCodigoPostal { get; set; }
        string ClienteDireccionCiudad { get; set; }
        string ClienteDireccionEstado { get; set; }
        string ClienteDireccionMunicipio { get; set; }
        string ClienteDireccionPais { get; set; }
        string ClienteDireccionColonia { get; set; }
        List<RepresentanteLegalBO> RepresentantesTotales { get; set; }
        List<RepresentanteLegalBO> RepresentantesSeleccionados { get; set; }
        int? RepresentanteLegalSeleccionadoID { get; }

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

        int? DiasRenta { get; set; }

        Int32? DiasFacturar { get; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string NumeroSerie { get; set; }

        int? ProductoServicioId { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }

        List<LineaContratoPSLBO> LineasContrato { get; set; }
        decimal? Maniobra { get; set; }
        ETipoContrato? TipoContrato { get; set; }
        string ModoRegistro { get; set; }
        DateTime? FechaInicioElegidoPrevio { get; set; }
        DateTime? FechaDevolucionElegidoPrevio { get; set; }
        bool? IncluyeSD { get; set; }
        string CodigoMonedaElegidoPrevio { get; set; }
        #region Campos ROC
        decimal? MontoTotalArrendamiento { get; set; }
        DateTime? FechaPagoRenta { get; set; }
        int? Plazo { get; set; }
        decimal? InversionInicial { get; set; }
        bool? EsROC { get; set; }
        #endregion
        #region Cálculo de Subtotales
        /// <summary>
        /// Obtiene o establece  el Monto del Contrato
        /// </summary>
        decimal? MontoTotal {
            get;
            set;
        }
        /// <summary>
        /// Obtiene o establece el porcentaje del seguro de acuerdo a la tarifa
        /// </summary>
        decimal? PorcentajeSeguro { get; set; }
        #endregion

        short DiasAnterioridadContrato { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararRenovacion();
        void CargarSucursales(List<SucursalBO> lstSucursales);
        void EstablecerSucursalSeleccionada(int? Id);
        void DesplegarDireccionCliente();

        void PermitirSeleccionarDireccionCliente(bool permitir);
        void PermitirSeleccionarUnidad(bool permitir);
        void PermitirSeleccionarRepresentantes(bool permitir);
        void PermitirAgregarRepresentantes(bool permitir);
        void PermitirSeleccionarTipoConfirmacion(bool permitir);
        void PermitirAsignarAutorizadorOrdenCompra(bool permitir);
        void PermitirAgregarProductoServicio(bool permitir);

        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);
        void ActualizarRepresentantesLegales();
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        bool? SoloRepresentantes { get; set; }
        List<AvalBO> AvalesTotales { get; set; }
        List<AvalBO> AvalesSeleccionados { get; set; }
        int? AvalSeleccionadoID { get; }
        List<RepresentanteLegalBO> RepresentantesAvalTotales { get; set; }
        List<RepresentanteLegalBO> RepresentantesAvalSeleccionados { get; set; }
        int? RepresentanteAvalSeleccionadoID { get; }

        void MostrarPersonasCliente(bool mostrar);
        void MostrarAvales(bool mostrar);
        void MostrarRepresentantesAval(bool mostrar);
        void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes);
        void ActualizarAvales();
        void PermitirSeleccionarMoneda(bool permitir);
        void PermitirSeleccionarAvales(bool permitir);
        void PermitirAgregarAvales(bool permitir);
        void MostrarRepresentantesLegales(bool mostrar);
        void HabilitarAgregarUnidad(bool habilitar);
        void RenderizarGridLineas();
        void PrepararVistaDetalle();
        #endregion
    }
}