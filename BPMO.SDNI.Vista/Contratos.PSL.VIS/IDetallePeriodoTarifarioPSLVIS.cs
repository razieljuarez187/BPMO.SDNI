// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IDetallePeriodoTarifarioPSLVIS {
        #region Propiedades
        int? ModuloID { get; }
        int? UnidadOperativaID { get; }
        int? UnidadOperativaSeleccionadaID { get; }
        int? UsuarioID { get; }
        UsuarioBO Usuario { get; }
        string Estatus { set; }
        DateTime? FechaRegistro { set; }
        DateTime? FechaModificacion { set; }
        string UsuarioRegistro { set; }
        string UsuarioModificacion { set; }
        #endregion

        #region Métodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void PermitirEditar(bool activo);
        void RedirigirSinPermisoAcceso();
        void ModoDetalle(bool activo);
        void LimpiarSesion();
        object ObtenerDatosNavegacion();
        void EstablecerDatosNavegacion(object tarifa);
        void EstablecerOpcionesUnidadesOperativas(Dictionary<int, string> unidadesOperativas);
        void RedirigirAEditar();

        #endregion
    }
}