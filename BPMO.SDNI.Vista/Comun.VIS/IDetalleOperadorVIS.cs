// Satisface al CU092 - Catálogo de Operadores
using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IDetalleOperadorVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }

        object UltimoObjeto { get; set; }

        int? OperadorID { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        string Nombre { get; set; }
        int? AñosExperiencia { get; set; }
        DateTime? FechaNacimiento { get; set; }
        string DireccionCalle { get; set; }
        string DireccionCiudad { get; set; }
        string DireccionCP { get; set; }
        string DireccionEstado { get; set; }
        int? LicenciaTipoID { get; set; }
        string LicenciaNumero { get; set; }
        DateTime? LicenciaFechaExpiracion { get; set; }
        string LicenciaEstado { get; set; }
        bool? Estatus { get; set; }

        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }
        int? UC { get; set; }
        int? UUA { get; set; }

        bool? EstatusNuevo { get; }
        DateTime? FechaDesactivacion { get; set; }
        int? UsuarioDesactivacionID { get; set; }
        string MotivoDesactivacion { get; set; }
        #endregion

        #region Métodos
        void PrepararVisualizacion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void EstablecerActivarDesactivar(bool esActivar);

        void RedirigirAEditar();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirRegresar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void PermitirEditar(bool permitir);
        void PermitirDesactivar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
