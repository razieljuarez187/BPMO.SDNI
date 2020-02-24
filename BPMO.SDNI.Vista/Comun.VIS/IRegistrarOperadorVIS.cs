// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS {
    public interface IRegistrarOperadorVIS {

        #region Propiedades
        int? UsuarioID { get; }
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

        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }

        List<OperadorBO> Operadores { get; set; }
        #endregion Propiedades

        #region Metodos
        void PrepararNuevo();

        void EstablecerPaqueteNavegacion(string nombre, object valor);

        void RedirigirAConsulta();
        void RedirigirADetalle();
        void RedirigirSinPermisoAcceso();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion Metodos
    }
}