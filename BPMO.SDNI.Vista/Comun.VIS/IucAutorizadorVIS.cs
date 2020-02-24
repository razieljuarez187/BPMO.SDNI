//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IucAutorizadorVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; set; }

        int? AutorizadorID { get; set; }
        string SucursalNombre { get; set; }
        int? SucursalID { get; set; }
        Enum TipoAutorizacion { get; }
        string EmpleadoNombre { get; set; }
        int? EmpleadoID { get; set; }
        string Email { get; set; }
        string Telefono { get; set; }
        bool? SoloNotificacion { get; set; }
        bool? Estatus { get; set; }

        int? UC { get; set; }
        int? UUA { get; set; }
        string UsuarioCreacion { set; }
        string UsuarioEdicion { set; }
        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }
        #endregion

        #region Metodos
        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararVisualizacion();

        void EstablecerOpcionesTiposAutorizacion(Dictionary<int, string> tipos);

        void HabilitarSoloNotificacion(bool habilitar);
        void HabilitarEstatus(bool habilitar);

        void MostrarEstatus(bool mostrar);
        void MostrarDatosRegistro(bool mostrar);
        void MostrarDatosActualizacion(bool mostrar);
        string ModoRegistro { get; set; }
        void PermitirSeleccionarSucursal(bool permitir);
        void PermitirSeleccionarEmpleado(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
