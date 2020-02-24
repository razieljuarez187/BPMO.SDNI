// Satisface al CU001 - Ingresar Unidad a Taller 

using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Mantenimiento.BO;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using System.Data;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IRegisitrarUnidadVIS
    {
        #region Propiedades
        
        int? UnidadOperativaId { get; }
        
        int? UsuarioAutenticado { get; }

        int? UC { get; }

        string LibroActivos { get; set; }

        List<UnidadBO> ResultadoBusquedaUnidades { get; set; }

        int IndicePaginaResultado { get; set; }

        List<MantenimientoBOF> Mantenimientos { get; set; }

        List<TareaPendienteBO> ListaTareasPendientes { get; set; }

        GridView GvMantenimientos { get; }

        GridView GvUnidadesSeleccion { get; }

        DropDownList DdTalleres { get; }

        DropDownList DdTipoOrdenServicio { get; }

        GridViewRow RowEnSesion { get; }

        String ClienteNombre { get; }

        String ModeloNombre { get; }

        void PonerEnSesion(string key, object value);

        System.Web.UI.WebControls.Image ImgCombustible { get; }

        string RootPath { get; }
        /// <summary>
        /// Determina si esta habilitado poder leer del archivo InLine
        /// </summary>
        bool? LeerInLine { get; set; }
        /// <summary>
        /// Configuraciones de mantenimiento por modelo consultadas
        /// </summary>
        List<ConfiguracionMantenimientoBO> ListaConfiguracionesMantenimiento { get; set; }

        #region Filtros Mantenimientos
        
        int? SucursalID { get; set; }

        String SucursalNombre { get; set; }

        int? TallerID { get;}

        int? Estatus { get;}

        int? OrdenServicioID { get; }

        string NumeroSerie { get; }

        DateTime? FechaInicio { get;}

        DateTime? FechaFin { get;}

        #endregion

        #region Registrar Orden Servicio

        string DescripcionFalla { get; }

        int CombustibleEntrada { get; }

        int CombustibleSalida { get; }

        int CombustibleTotal { get; }

        string Inventario { get; }

        string CodigosFalla { get; }

        string MotivoCancelacion { get; }

        int? ModuloID { get; }

        int? UnidadOperativaDestinoID { get; }

        int? SucursalDestinoID { get; }
        #endregion

        /// <summary>
        /// Reporte en formato Base64 para exportación
        /// </summary>
        byte[] Reporte64 { set; }
        #endregion

        #region Métodos

        /// <summary>
        /// Redirige a la pagina informativa de falta de permisos para acceder
        /// </summary>
        void RedirigirSinPermisoAcceso();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void PrepararBusqueda();

        void CargarTalleres();

        void CrearCabeceraComplemento();

        void MostrarResultadoUnidades();

        bool IsUnidad();

        void CargarDatos();

        void RedirigeExportarReporte();

        #region CU064
        void redirigeCU064(bool imprimirPendientes);
        #endregion

        #region CU051
        /// <summary>
        /// Despliega el visor de los formatos a imprimir
        /// </summary>
        void IrAImprimir();

        /// <summary>
        /// Establece el Paquete de Navegacion para imprimir el reporte
        /// </summary>
        /// <param name="clave">Clave del Paquete de Navegacion</param>
        /// <param name="datosReporte">Datos del Reporte</param>
        void EstablecerPaqueteNavegacionImprimir(string clave, Dictionary<string, object> datosReporte, bool ConfigurarNombreSesion = false);

        /// <summary>
        /// Identificador del reporte de salida - CU051
        /// </summary>
        string IdentificadorReporteCU051 { get; }

        void redirigePaseSalida();

        #endregion

        #region CU017
        void redirigeCU017();
        #endregion

        #region CU063
        void HabilitarTareasPendientes();
        void RedirigeCU063();
        #endregion

        #endregion

        DataSet MantenimientosProgramadosPendientes { get; set; }

        void DesplegarListaMantenimientosProgramadosPendientes();

        void CargarEstatus();
    }
}
