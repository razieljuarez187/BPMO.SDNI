// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface ICerrarContratoPSLVIS {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? ModuloID { get; }

        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string ObservacionesCierre { get; set; }
        DateTime? FechaCierre { get; set; }
        DateTime? FechaContrato { get; set; }
        DateTime? FechaRecepcion { get; set; }
        List<LineaContratoPSLBO> LineasContrato { get; set; }

        decimal? ImporteDeposito { get; set; }
        decimal? ImporteReembolso { get; set; }
        string PersonaRecibeReembolso { get; set; }

        int? TipoContrato { get; set; }
        #endregion

        #region Métodos
        void PrepararEdicion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirACancelar();
        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirCerrar(bool permitir);
        void PermitirRegistrar(bool permitir);

        void LimpiarSesion();

        void CambiarACargoAdicional();
        void CambiarACierre();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EstablecerEtiquetas();
        #endregion
    }
}