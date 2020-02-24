using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IConsultarConfiuracionDescuentoPSLVIS {
        #region Propiedades

        int? UsuarioAutenticado { get; }
        int? UnidadOperativaId {
            get;
        }
        int? ModuloID { get; }
        string LibroActivos { get; set; }

        String NombreCliente { get; set; }

        int? ClienteID { get; set; }

        String Sucursal { get; set; }

        int? SucursalID {
            get;
            set;
        }

        DateTime? FechaInicial { get; set; }

        DateTime? FechaFinal { get; set; }

        int? Estatus { get; set; }

        List<ConfiguracionDescuentoBO> Resultado { get; set; }
        int IndicePaginaResultado { get; set; }

        List<SucursalBO> SucursalesAutorizadas { get; set; }

        UsuarioBO Usuario { get; }

        UnidadOperativaBO UnidadOperativa { get; }

        #region REQ 13285 Lista de acciones permitirdas para el usuario.

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion

        #endregion


        #region Métodos
        void PrepararBusqueda();

        void ActualizarResultado();

        void EstablecerPaqueteNavegacion(string nombre, object valor);
        void RedirigirADetalles();
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #region SC0008
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion

    }
}