// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IEditarTarifaRDVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }
        decimal? PrecioCombustible { set; }
        DateTime? FUA { get; }
        int? TarifaID { get; set; }
        string NombreSucursal {set; }
        int? SucursalID { get; set; }
        string NombreModelo { set; }
        int? ModeloID { get; set; }
        string NombreMoneda { set; }
        string CodigoMoneda { get; set; }
        string NombreTipoTarifa { set; }
        int? TipoTarifa { get; set; }
        string Descripcion { get; set; }
        string NombreCliente { set; }
        int? CuentaClienteID { get; set; }
        DateTime? Vigencia { get; set; }
        bool? Estatus { get; set; }
        bool? AplicarOtrasSucursales { get; set; }
        int? SucursalNoAplicaID { get; set; }
        string NombreSucursalNoAplica { get; set; }
        List<TarifaRDBO> TarifasAnteriores { get; set; }
        List<SucursalBO> SessionListaSucursalSeleccionada { get; set; }

        string Observaciones { get; set; }
        #endregion

        #region Métodos

        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void MostrarCliente(bool activo);
        void MostrarAplicarSucursal(bool activo);
        void RedirigirADetalle();
        void RedirigirAConsulta();
        void PermitirRegistrar(bool activo);
        void RedirigirSinPermisoAcceso();
        void PermitirAgregarSucursales(bool activo);
        void EstablecerPaqueteNavegacion(object tarifa);
        object ObtenerDatosNavegacion();

        void MostrarLeyendaSucursales(bool mostrar, string leyenda);

        #endregion
    }
}
