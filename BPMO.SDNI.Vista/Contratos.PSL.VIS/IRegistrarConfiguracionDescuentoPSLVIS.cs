using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IRegistrarConfiguracionDescuentoPSLVIS {

        #region Propiedades
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }

        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Unidad Operativa sobre la que trabaja el usuario en el sistema
        /// </summary>
        int? UnidadOperativaID { get; }

        #endregion

        #region Métodos

        void PermitirGuardarTerminado(bool permitir);

        void RedirigirSinPermisoAcceso();

        void RedirigirAConsulta();

        void RedirigirADetalles();

        void PrepararNuevo();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void LimpiarSesion();

        void EstablecerPaqueteNavegacion(string key, object value);
        #endregion

    }
}