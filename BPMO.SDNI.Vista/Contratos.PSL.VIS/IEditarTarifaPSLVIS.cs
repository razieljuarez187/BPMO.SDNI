// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IEditarTarifaPSLVIS {
        #region Propiedades
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }
        int? ModuloID { get; }
        decimal? PrecioCombustible { set; }
        DateTime? FUA { get; }
        int? TarifaPSLID { get; set; }
        string NombreSucursal { set; }
        int? SucursalID { get; set; }
        string NombreModelo { set; }
        int? ModeloID { get; set; }
        string NombreMoneda { set; }
        string CodigoMoneda { get; set; }
        string NombreTipoTarifa { set; }
        int? TipoTarifa { get; set; }
        bool? Estatus { get; set; }
        bool? AplicarOtrasSucursales { get; set; }
        int? SucursalNoAplicaID { get; set; }
        string NombreSucursalNoAplica { get; set; }
        string NombreTarifaTurno { get; set; }
        Enum TarifaTurno { get; set; }
        string NombrePeriodoTarifa { get; set; }
        EPeriodosTarifa PeriodoTarifa { get; set; }
        List<TarifaPSLBO> TarifasAnteriores { get; set; }
        List<SucursalBO> SessionListaSucursalSeleccionada { get; set; }

        #endregion

        #region Métodos

        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
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