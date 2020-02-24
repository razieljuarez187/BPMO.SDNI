// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface ICancelarContratoRDVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }

        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }

        DateTime? FechaContrato { get; set; }
        DateTime? FechaPromesaDevolucion { get; set; }
        
        DateTime? FechaDevolucion { get; set; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        int? KmRecorrido { get; set; }
        
        string ObservacionesCancelacion { get; set; }
        DateTime? FechaCancelacion { get; set; }
        string MotivoCancelacion { get; set; }
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
        #endregion
    }
}
