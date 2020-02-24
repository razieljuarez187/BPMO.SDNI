//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IDetalleActaNacimientoVIS
    {
        #region Propiedades
        int? EquipoID { get; set; }
        int? UnidadID { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        EEstatusUnidad? EstatusUnidad { get; set; }

        string NumeroSerie { get; set; }
        string ClaveActivoOracle { get; set; }
        int? LiderID { get; set; }
        string NumeroEconomico { get; set; }
        string TipoUnidadNombre { get; set; }
        int? TipoUnidadId { get; set; }
        int? MarcaId { get; set; }
        string ModeloNombre { get; set; }
        int? ModeloId { get; set; }
        int? Anio { get; set; }
        DateTime? FechaCompra { get; set; }
        decimal? MontoFactura { get; set; }
        //Página 1
        string Propietario { get; set; }
        string Cliente { get; set; }
        int? ClienteId { get; set; }
        int? UnidadOperativaId { get; }
        string SucursalNombre { get; set; }
        int? SucursalId { get; set; }
        Enum Area { get; set; }
        string FabricanteNombre { get; set; }
        #region SC0002
        bool? EntraMantenimiento { get; set; }
        #endregion
        //Página 2
        List<HorometroBO> Horometros { get; set; }
        List<OdometroBO> Odometros { get; set; }
        decimal? PBVMaximoRecomendado { get; set; }
        decimal? PBCMaximoRecomendado { get; set; }
        decimal? CapacidadTanque { get; set; }
        decimal? RendimientoTanque { get; set; }
        //Página 3
        string Radiador { get; set; }
        string PostEnfriador { get; set; }
        
        //mis propiedades
        List<NumeroSerieBO> NumerosSerie { get; set; }
        //***********************************************
               
        #region SC0030
        string SerieMotor { get; set; }
        #endregion
        string SerieTurboCargador { get; set; }
        string SerieCompresorAire { get; set; }
        string SerieECM { get; set; }
        string SerieAlternador { get; set; }
        string SerieMarcha { get; set; }
        string SerieBaterias { get; set; }
        string TransmisionSerie { get; set; }
        string TransmisionModelo { get; set; }
        string EjeDireccionSerie { get; set; }
        string EjeDireccionModelo { get; set; }
        string EjeTraseroDelanteroSerie { get; set; }
        string EjeTraseroDelanteroModelo { get; set; }
        string EjeTraseroTraseroSerie { get; set; }
        string EjeTraseroTraseroModelo { get; set; }
        //Página 4
        int? EnllantableID { get; set; }
        int? TipoEnllantable { get; set; }
        string DescripcionEnllantable { get; set; }
        List<LlantaBO> Llantas { get; set; }
        int? RefaccionID { get; set; }
        string RefaccionCodigo { get; set; }
        int? RefaccionSucursalID { get; set; }
        string RefaccionSucursalNombre { get; set; }
        string RefaccionMarca { get; set; }
        string RefaccionModelo { get; set; }
        string RefaccionMedida { get; set; }
        decimal? RefaccionProfundidad { get; set; }
        bool? RefaccionRevitalizada { get; set; }
        bool? RefaccionStock { get; set; }
        bool? RefaccionActivo { get; set; }
        DateTime? RefaccionFC { get; set; }
        DateTime? RefaccionFUA { get; set; }
        int? RefaccionUC { get; set; }
        int? RefaccionUUA { get; set; }
        //Página 5
        List<EquipoAliadoBO> EquiposAliados { get; set; }

        int? PaginaActual { get; }

        //RQM 14150
        List<CatalogoBaseBO> ListaAcciones { get; set; }
        string ValoresTabs { get; set; }
        List<UnidadBO> lstUnidades { get; set; }

        //RQM 14150, Pagina 1, datos generales, generación y construcción
        string OrdenCompraProveedor { get; set; }
        decimal? MontoArrendamiento { get; set; }
        string CodigoMoneda { get; set; }
        DateTime? FechaInicioArrendamiento { get; set; }
        DateTime? FechaFinArrendamiento { get; set; }
        int? ProveedorID { get; set; }

        //RQM 13285
        List<ArchivoBO> Archivos { get; set; }
        List<ArchivoBO> ArchivosOC { get; set; }

        ETipoEmpresa EmpresaConPermiso { set; get; }
        #endregion

        #region Métodos

        #region RQM 14150
        void EstablecerAcciones();
        void HabilitaBoton(bool habilitado, string boton);
        void PermitirActualizar(bool actualizar);
        #endregion

        void PrepararVisualizacion();

        void EstablecerDatosNavegacion(string nombre, object valor);
        object ObtenerDatosNavegacion();

        void PermitirRegresar(bool habilitar);
        void PermitirContinuar(bool habilitar);
        void PermitirRedirigirAEdicion(bool habilitar);
        void PermitirConsultarHistorial(bool habilitar);

        void MostrarActaNacimientoOriginal(bool mostrar);

        void EstablecerPagina(int numeroPagona);

        void RedirigirAConsulta();
        void RedirigirAEdicion();
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #region SC0008

        void PermitirRegistrar(bool habilitar);
        void RedirigirSinPermisoAcceso();

        #endregion

        #endregion
    }
}
