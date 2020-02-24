using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface ICancelarContratoPSLVIS {
        #region Propiedades

        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        object UltimoObjeto { get; set; }
        int? ContratoID { get; set; }
        EEstatusContrato? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }
        DateTime? FechaContrato { get; set; }
        DateTime? FechaPromesaDevolucion { get; set; }
        DateTime? FechaDevolucion { get; set; }
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string ObservacionesCancelacion { get; set; }
        DateTime? FechaCancelacion { get; set; }
        string MotivoCancelacion { get; set; }
        int? TipoContrato { get; set; }

        Dictionary<string, object> DatosReporte { get; }
        string NombreReporte { get; }
        #endregion

        #region Métodos

        void PrepararEdicion();
        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);
        void RedirigirACerrar();
        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();
        void PermitirCancelar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void RedirigirAImprimir();

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);
        #endregion
    }
}