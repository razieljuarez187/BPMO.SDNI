using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IDetalleConfiguracionDescuentoPSLVIS {
        #region Propiedades

        int? UnidadOperativaId { get; }
        int? UsuarioID { get; }
        int? ClienteID { get; set; }
        int? ModeloID { get; set; }
        int? ConfiguracionDescuentoID { get; set; }
        int? SucursalID { get; set; }
        string Cliente { get; set; }
        string ContactoComercial { get; set; }

        List<ConfiguracionDescuentoBO> Resultado { get; set; }
        int IndicePaginaResultado { get; set; }
        #endregion

        #region Métodos
        void ActualizarResultado();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void PermitirEditar(bool activo);
        //void PermitirRegistrar(bool activo);
        void RedirigirSinPermisoAcceso();
        void ModoDetalle(bool activo);
        void LimpiarSesion();
        object ObtenerDatosNavegacion();
        void EstablecerDatosNavegacion(object configuracionDescuento);
        void RedirigirAEditar();
        #region SC0024
        void RegresarAConsultar();//SC0024
        //void PermitirRegresar(bool permiso); //SC0024

        #endregion

        #endregion

    }
}