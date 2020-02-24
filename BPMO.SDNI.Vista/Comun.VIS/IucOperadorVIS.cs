// Satisface al CU092 - Catálogo de Operadores
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IucOperadorVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; }

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
        #endregion

        #region Metodos
        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararVisualizacion();

        void PermitirSeleccionarCuentaCliente(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
