// Satisface al CU002 - Editar Contrato Renta Diaria
// BEP1401 Satisface a la SC0026
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IEditarContratoRDVIS
    {
        #region Propiedades
        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }
        int? UC { get; set; }
        int? UUA { get; set; }
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
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteTipoID { get; set; }
        int? ClienteDireccionId { get; set; } //Id de la Direccion del cliente
        string ClienteDireccionCompleta { set; }
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
        string Observaciones { get; set; }
        #region BEP1401.SC0026
        /// <summary>
        /// Número de días que se cobrarán en la primera Factura
        /// </summary>
        Int32? DiasFacturar { get; set; }
        #endregion
        //Operador
        #region SC_0013
        int? OperadorID { get; set; }
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

        //Línea del Contrato
        int? LineaContratoID { get; set; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string VIN { get; set; }
        string NumeroEconocimico { get; set; }
        int? ModeloID { get; set; }
        string ModeloNombre { get; set; }
        int? ProductoServicioId { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }

        int? TipoTarifaID { get; set; }
        int? CapacidadCarga { get; set; }
        decimal? TarifaDiaria { get; set; }
        int? KmsLibres { get; set; }
        decimal? TarifaKmAdicional { get; set; }
        int? HrsLibres { get; set; }
        decimal? TarifaHrAdicional { get; set; }
        string TarifaDescripcion { get; set; }
        #endregion

        #region Métodos
        void PrepararEdicion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void PermitirGuardarBorrador(bool permitir);
        void PermitirGuardarTerminado(bool permitir);

        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirRegistrar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #region SC0038
        List<object> ObtenerPlantillas(string key);
        #endregion
        #endregion
    }
}