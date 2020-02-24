//Satisface al CU082 - Registrar Movimiento de Flota
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista para la pagina de alta de unidad
    /// </summary>
    public interface IRegistrarAltaUnidadVIS
    {
        #region Propiedades

        int? ModuloID { get; }
        int? UnidadID { get; set; }
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? SucursalID { get; set; }
        int? EmpresaID { get; set; }
        string NombreEmpresa { get; set; }
        string SucursalNombre { get; set; }
        string DomicilioSucursal { get; set; }
        string VIN { get; set; }
        string ClaveActivoOracle { get; set; }
        int? LiderID { get; set; }
        string NumeroEconomico { get; set; }
        string TipoUnidadNombre { get; set; }
        int? TipoUnidadId { get; set; }
        string ModeloNombre { get; set; }
        int? ModeloId { get; set; }
        int? Anio { get; set; }
        DateTime? FechaCompra { get; set; }
        decimal? MontoFactura { get; set; }
        /// <summary>
        /// Obtiene o establece el folio de la factura de compra de la unidad
        /// </summary>
        string FolioFactura { get; set; }
        string Observaciones { get; set; }
        object UltimoObjeto { get; set; }
        bool? EstaDisponible { get; set; }
        bool? EstaEnContrato { get; set; }
        bool? TieneEquipoAliado { get; set; }
        string NumeroPlaca { get; set; }
        #endregion

        #region Métodos

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);
        void RedirigirSinPermisoAcceso();
        void RedirigirAExpediente();
        void RedirigirAConsultaDeUnidades();
        void RedirigirAConsultaDeEquipoAliado();
        void LimpiarSesion();
        void PermitirRegistrar(bool status);
        void PermitirConsultar(bool status);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #endregion
    }
}