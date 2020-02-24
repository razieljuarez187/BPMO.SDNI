// Satisface al CU080 – Editar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IEditarActaNacimientoVIS
    {
        #region Propiedades
        UnidadBO UltimoObjeto { get; set; }

        //Página 0
        int? EquipoId { get; set; }
        int? UnidadId { get; set; }
        DateTime? FC { get; set; }
        DateTime? FUA { get; }
        int? UC { get; set; }
        int? UUA { get; }
        EEstatusUnidad? EstatusUnidad { get; set; }

        string NumeroSerie { get; set; }
        string ClaveActivoOracle { get; set; }
        int? LiderID { get; set; }
        string NumeroEconomico { get; set; }
        string TipoUnidadNombre { get; set; }
        int? MarcaId { get; set; }
        string ModeloNombre { get; set; }
        int? ModeloId { get; set; }
        string FabricanteNombre { get; set; }
        int? Anio { get; set; }
        DateTime? FechaCompra { get; set; }
        decimal? MontoFactura { get; set; }
        string LibroActivos { get; set; }
        string ClaveOracleUnidad { get; set; }
        string ClaveOracleUnidadLider { get; set; }
        //Página 1
        string Propietario { get; set; }
        string Cliente { get; set; }
        int? ClienteId { get; set; }
        int? UnidadOperativaId { get; }
        string SucursalNombre { get; set; }
        int? SucursalId { get; set; }
        Enum Area { get; set; }

        #region REQ. 13285 Acta de nacimiento.

        //Datos generales, generación y construcción
        int? ArrendamientoId { get; set; }
        string OrdenCompraProveedor { get; set; }
        decimal? MontoArrendamiento { get; set; }
        string CodigoMoneda { get; set; }
        DateTime? FechaInicioArrendamiento { get; set; }
        DateTime? FechaFinArrendamiento { get; set; }
        int? ProveedorID { get; set; }
        int? PedimentoId { get; set; }
        string NumeroPedimento { get; set; }
        DateTime? FechaInicioArrendamientoNuevo { get; set; }
        DateTime? FechaFinArrendamientoNuevo { get; set; }

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
        
        List<NumeroSerieBO> NumerosSerie { get; set; }

        #region SC0002
        bool? EntraMantenimiento { get; set; }
        GridView GridAliados { get; set; } 
        #endregion

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

        List<int?> SucursalesSeguridad { get; set; }

        int? ModuloID { get; }
        string NombreClienteUnidadOperativa { get; set; }

        #region REQ 13285 Acta de Nacimiento

        List<CatalogoBaseBO> ListaAcciones { get; set; }
        ETipoEmpresa EmpresaConPermiso { set; get; }
        string ValoresTabs { set; get; }
        List<ArchivoBO> Archivos { get; set; }
        List<ArchivoBO> ArchivosOC { get; set; }

        #endregion
        #endregion

        #region Métodos
        void PrepararEdicion();

        object ObtenerDatosNavegacion();

        void PermitirRegresar(bool habilitar);
        void PermitirContinuar(bool habilitar);
        void PermitirCancelar(bool habilitar);
        void PermitirGuardarBorrador(bool habilitar);
        void PermitirGuardarTerminada(bool habilitar);

        void OcultarContinuar(bool ocultar);
        void OcultarTerminar(bool ocultar);
        void OcultarBorrador(bool ocultar);

        void EstablecerPagina(int numeroPagona);

        void RedirigirAConsulta();
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        
        void RegistrarScriptMensaje(string p, string p_2);

        string Mensaje { get; set; }
        #region SC0008

        void RedirigirSinPermisoAcceso();
        void PermitirRegistrar(bool habilitar);

        #endregion

        #region REQ 13285 Acta de Nacimiento.

        void EstablecerAcciones();

        #endregion
        #endregion

        
    }
}
