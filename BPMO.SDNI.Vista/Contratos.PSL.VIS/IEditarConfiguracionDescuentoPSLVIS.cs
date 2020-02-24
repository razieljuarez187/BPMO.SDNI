using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IEditarConfiguracionDescuentoPSLVIS {
        #region Propiedades

        int? UnidadOperativaId { get; }
        int? UsuarioAutenticado { get; }
        int? ModeloID { get; set; }
        int? ConfiguracionDescuentoID { get; set; }
        ConfiguracionDescuentoBO UltimoObjeto { get; set; }
        void RedirigirSinPermisoAcceso();
        List<ConfiguracionDescuentoBO> ListaDescuentos {
            get;
            set;

        }
        List<ConfiguracionDescuentoBO> ListaDetalle {
            get;
            set;
        }

        #endregion

        #region Métodos
        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);
        void RedirigirADetalle();
        void RedirigirAConsulta();
        void LimpiarSesiones();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion

    }
}