//Satisface al CU081 - Consultar Seguimiento Flota
using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Flota.VIS
{
    public interface IConsultarSeguimientoFlotaVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }

        string VIN { get; set; }
        string NumeroEconomico { get; set; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? MarcaID { get; set; }
        string MarcaNombre { get; set; }
        int? ModeloID { get; set; }
        string ModeloNombre { get; set; }
        int? TipoUnidadID { get; set; }
        string TipoUnidadNombre { get; set; }
        int? AreaID { get; set; }
        string Propietario { get; set; }
        int? EstatusID { get; set; }
        DateTime? FechaAltaInicial { get; set; }
        DateTime? FechaAltaFinal { get; set; }
        DateTime? FechaBajaInicial { get; set; }
        DateTime? FechaBajaFinal { get; set; }

        #region [RQM 13285- Integración Construcción y Generacción]

        EAreaConstruccion? ETipoRentaConstruccion { get; set; }
        EAreaGeneracion? ETipoRentaGeneracion { get; set; }
        EAreaEquinova? ETipoRentaEquinova { get; set; }
        ETipoEmpresa EnumTipoEmpresa { set; get; }

        void CargarAreaIdealease(Dictionary<string, object> tipoAreaIdealease);
        void CargarAreaConstruccion(Dictionary<string, object> tipoAreaCostruccion);
        void CargarAreaGeneracion(Dictionary<string, object> tipoAreaGeneracion);
        void CargarAreaEquinova(Dictionary<string, object> tipoAreaEquinova);

        #endregion
        
        object Resultado { get; }
        #endregion

        #region Métodos
        void EstablecerResultado(object resultado);
        void EstablecerOpcionesEstatus(Dictionary<int, string> estatus);

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void PermitirRealizarCambioSucursalEquipoAliado(bool permitir);

        void RedirigirSinPermisoAcceso();
        void RedirigirAExpediente();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
