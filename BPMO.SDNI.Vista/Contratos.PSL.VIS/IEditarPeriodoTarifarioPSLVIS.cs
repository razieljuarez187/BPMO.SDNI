// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IEditarPeriodoTarifarioPSLVIS {
        #region Propiedades
        object UltimoObjeto { get; set; }
        int? UnidadOperativaID { get; }
        UsuarioBO Usuario { get; }
        int? UnidadOperativaSeleccionada { get; set; }
        int? UsuarioID { get; }
        int? ModuloID { get; }

        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }
        int? UC { get; set; }
        int? UUA { get; set; }

        #endregion

        #region Métodos

        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void RedirigirADetalle();
        void RedirigirSinPermisoAcceso();
        void EstablecerPaqueteNavegacion(object periodoTarifa);
        void EstablecerOpcionesUnidadesOperativas(Dictionary<int, string> unidadesOperativas);
        object ObtenerDatosNavegacion();
        #endregion
    }
}