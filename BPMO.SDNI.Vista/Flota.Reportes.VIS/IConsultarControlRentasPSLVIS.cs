using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Flota.VIS {
    /// <summary>
    /// Vista para la página de Centro de Control de Rentas
    /// </summary>
    public interface IConsultarControlRentasPSLVIS {
        #region Propiedades

        int? ModuloID { get; }
        int? UsuarioID { get; }
        int? UnidadOperativaId { get; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? Estatus { get; }
        GridView GvUnidadesRentas { get; }
        DataTable UnidadRentas { get; set; }
        DataSet ReporteRentas { get; set; }
        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion

        #region Métodos

        void PrepararBusqueda();
        void CargarEstatus(List<EEstatusUnidad> listado);
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);
        void RedirigirSinPermisoAcceso();
        void ActualizarResultado(DataTable unidad);
        void EstablecerPaqueteNavegacion(string nombreReporte, byte[] datosReporte);
        void RegistraScript(string key, string script);
        #endregion
    }
}