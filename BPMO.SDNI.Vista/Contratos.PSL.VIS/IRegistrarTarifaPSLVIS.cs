// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IRegistrarTarifaPSLVIS {
        #region Propiedades

        int? UC { get; }
        DateTime FC { get; }
        int? UnidadOperativaID { get; }
        decimal? PrecioCombustible { set; }
        int? ModuloID { get; }
        int? ModeloID { get; set; }
        string NombreModelo { get; set; }
        int? TipoTarifaSeleccionada { get; }
        string MonedaSeleccionada { get; }
        int? SucursalID { get; set; }
        string NombreSucursal { get; set; }
        int? SucursalNoAplicaID { get; set; }
        string NombreSucursalNoAplica { get; set; }
        List<SucursalBO> ListaSucursalSeleccionada { get; set; }
        bool? AplicarOtrasSucursales { get; set; }
        decimal? Tarifa { get; }
        decimal? TarifaHrAdicional { get; }
        int? TarifaTurnoSeleccionada { get; }
        int? PeriodoTarifaSeleccionada { get; }

        #endregion

        #region Métodos

        void LimpiarSesion();
        void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipos);
        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);
        void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turno);
        void EstablecerOpcionesPeriodoTarifa(Dictionary<string, string> periodo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void ModoEdicion(bool activo);
        void ModoRegistrarTarifa(bool activo);
        void MostrarAplicarSucursal(bool activo);
        void MostrarCapturaTarifas(bool activo);
        void ModoConsulta(bool activo);
        void RedirigirAConsulta();
        void RedirigirADetalle();
        void RedirigirSinPermisoAcceso();
        void PermitirAgregarSucursales(bool activo);
        void EstablecerPaqueteNavegacion(object tarifa);
        //void MostrarVigencia(bool activo);
        void MostrarLeyendaSucursales(bool mostrar, string leyenda);

        #endregion
    }
}