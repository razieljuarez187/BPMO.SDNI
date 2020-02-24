//Satisface al CU001 - Registrar Contrato de Renta Diaria
//Satisface al CU002 - Editar Contrato Renta Diaria
// BEP1401 Satisface a la SC0026
using System;
using System.Collections.Generic;

using System.Data;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IucContratoRDVIS
    {
        #region Propiedades
        int? ContratoID { get; set; }

        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        int? ModuloID { get; }

        int? EstatusID { get; set; }

        DateTime? FechaContrato { get; set; }
        TimeSpan? HoraContrato { get; set; }
        string NumeroContrato { get; set; }

        int? UnidadOperativaID { get; }
        string NombreEmpresa { get; set; }
        string DomicilioEmpresa { get; set; }
        string RepresentanteEmpresa { get; set; }

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        string DireccionSucursal { get; set; }

        decimal? PorcentajePagoPostFactura { get; set; }
        int? DiasPagoPostFactura { get; set; }

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
        int? LectorKilometrajeID { get; set; }
        int? FormaPagoID { get; set; }
        int? MotivoRentaID { get; set; }
        int? FrecuenciaFacturacionID { get; set; }
        string AutorizadorTipoConfirmacion { get; set; }
        string AutorizadorOrdenCompra { get; set; }
        string MercanciaTransportar { get; set; }
        string DestinoAreaOperacion { get; set; }
        decimal? PorcentajeDeducible { get; set; }
        bool? BitacoraViajeConductor { get; set; }
        DateTime? FechaPromesaDevolucion { get; set; }
        TimeSpan? HoraPromesaDevolucion { get; set; }
        string Observaciones { get; set; }

        int? DiasRenta { get; set; }
        #region SC_0013
        int? OperadorID { get; set; }
        int? OperadorCuentaClienteID { get; set; }
        #endregion

        #region BEP1401.SC0026
        Int32? DiasFacturar { get; set; }
        #endregion
        string OperadorNombre { get; set; }
        DateTime? OperadorFechaNacimiento { get; set; }
        int? OperadorAniosExperiencia { get; set; }
        string OperadorDireccionCalle { get; set; }
        string OperadorDireccionCiudad { get; set; }
        string OperadorDireccionEstado { get; set; }
        string OperadorDireccionCP { get; set; }
        int? OperadorLicenciaTipoID { get; set; }
        string OperadorLicenciaNumero { get; set; }
        DateTime? OperadorLicenciaFechaExpiracion { get; set; }
        string OperadorLicenciaEstado { get; set; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string VIN { get; set; }
        string NumeroEconomico { get; set; }
        int? ModeloID { get; set; }
        string ModeloNombre { get; set; }
        string MarcaNombre { get; set; }
        int? UnidadAnio { get; set; }
        string UnidadSucursalNombre { get; set; }
        string UnidadPlacaEstatal { get; set; }
        string UnidadPlacaFederal { get; set; }
        decimal? UnidadPBC { get; set; }
        decimal? UnidadCapacidadTanque { get; set; }
        decimal? UnidadRendimientoTanque { get; set; }
        string UnidadNumeroPoliza { get; set; }
        string UnidadAseguradora { get; set; }
        decimal? UnidadMontoDeducible { get; set; }
        decimal? UnidadMontoDeposito { get; set; }
        int? ProductoServicioId { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }

        int? TarifaID { get; set; }
        string TarifaDescripcion { get; set; }
        int? TipoTarifaID { get; set; }
        int? TarifaSucursalID { get; set; }
        int? TarifaModeloID { get; set; }
        int? TarifaUnidadOperativaID { get; set; }
        int? TarifaCuentaClienteID { get; set; }
        string TarifaCodigoMoneda { get; set; }
        int? CapacidadCarga { get; set; }
        decimal? TarifaDiaria { get; set; }
        int? KmsLibres { get; set; }
        decimal? TarifaKmAdicional { get; set; }
        int? HrsLibres { get; set; }
        decimal? TarifaHrAdicional { get; set; }

        string TarifaPersonalizadaCodigoAutorizacion { get; set; }
        int? TarifaPersonalizadaTipoTarifaID { get; set; }
        int? TarifaPersonalizadaCapacidadCarga { get; set; }
        decimal? TarifaPersonalizadaTarifaDiaria { get; set; }
        int? TarifaPersonalizadaKmsLibres { get; set; }
        decimal? TarifaPersonalizadaTarifaKmAdicional { get; set; }
        int? TarifaPersonalizadaHrsLibres { get; set; }
        decimal? TarifaPersonalizadaTarifaHrAdicional { get; set; }

        decimal? CargoHoraPosecion { get; set; }
        decimal? CargoAlteracionMedidorKm { get; set; }
        decimal? CargoEntregaInpuntual { get; set; }
        /// <summary>
        /// Obtiene o establece las sucursales autorizadas del usuario logueado
        /// </summary>
        List<SucursalBO> SucursalesAutorizadas { get; set; }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario logueado
        /// </summary> 
        UnidadOperativaBO UnidadOperativa { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();
        void PrepararEdicion();

        void PermitirSeleccionarDireccionCliente(bool permitir);
        void PermitirSeleccionarTarifas(bool permitir);
        void PermitirSeleccionarUnidad(bool permitir);
        void PermitirSeleccionarRepresentantes(bool permitir);
        void PermitirAgregarRepresentantes(bool permitir);
        void PermitirSeleccionarTipoConfirmacion(bool permitir);
        void PermitirAsignarAutorizadorOrdenCompra(bool permitir);
        void PermitirSeleccionarOperador(bool permitir);
        void PemitirSeleccionarDiasFacturar(Boolean permitir);

        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);
        void EstablecerOpcionesFrecuenciaFacturacion(Dictionary<string, string> frecuenciaFacturacion);
        void EstablecerEquiposAliadoUnidad(DataTable dt);

        void ActualizarRepresentantesLegales();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        string ObtenerClaveDeducible();//SC0018

	    void PermitirValidarCodigoAutorizacion(bool permitir);
	    void PermitirSolicitarCodigoAutorizacion(bool permitir);

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
		void PermitirSeleccionarAvales(bool permitir);
		void PermitirAgregarAvales(bool permitir);
	    void MostrarRepresentantesLegales(bool mostrar);
        void PermitirAgregarProductoServicio(bool permitir);
        void DesplegarDireccionCliente();
	    #endregion
    }
}