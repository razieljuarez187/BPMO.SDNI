//Satiface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.Primitivos.Enumeradores;
using System;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IConsultarAutorizadorVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }

        string SucursalNombre { get; set; }
        int? SucursalID { get; set; }
        Enum TipoAutorizacion { get; set; }
        string EmpleadoNombre { get; set; }
        int? EmpleadoID { get; set; }
        bool? Estatus { get; set; }

        List<AutorizadorBO> Resultado { get; }
        #endregion

        #region Métodos
        void EstablecerResultado(List<AutorizadorBO> resultado);

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void PermitirRegistrar(bool permitir);

        void EstablecerOpcionesTiposAutorizacion(Dictionary<int, string> tipos);

        void RedirigirSinPermisoAcceso();
        void RedirigirADetalle();

        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}
