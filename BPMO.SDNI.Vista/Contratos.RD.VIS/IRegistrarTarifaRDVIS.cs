// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IRegistrarTarifaRDVIS
    {
        #region Propiedades

        int? UC { get;}
        DateTime FC { get;}
        int? UnidadOperativaID { get;}
        decimal? PrecioCombustible { set; }
        int? ModeloID { get; set; }
        string NombreModelo { get; set; }
        
        int? ClienteID { get; set; }
        string NombreCliente { get; set; }
        int? TipoTarifaSeleccionada { get;}
        string MonedaSeleccionada { get;}
        int? SucursalID { get; set; }
        string NombreSucursal { get; set; }
        int? SucursalNoAplicaID { get; set; }
        string NombreSucursalNoAplica { get; set; }
        List<SucursalBO> ListaSucursalSeleccionada { get; set; }
        string Descripcion { get; set; }
        DateTime? Vigencia { get; set; }
        bool? AplicarOtrasSucursales { get; set; }

        string Observaciones { get; set; }

        #endregion

        #region Métodos

        void LimpiarSesion();
        void EstablecerOpcionesTipoTarifa(Dictionary<int,string> tipos);
        void EstablecerOpcionesMoneda(Dictionary<string,string> monedas);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void MostrarCliente(bool activo);
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
        void MostrarVigencia(bool activo);
        void MostrarObservaciones(bool activo);

        void MostrarLeyendaSucursales(bool mostrar, string leyenda);

        #endregion
    }
}
