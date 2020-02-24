//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IRegistrarActaNacimientoVIS
    {
        #region Propiedades
        //Página 0
        int? EquipoId { get; set; }
        int? UnidadId { get; set; }
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
        string MarcaNombre { get; set; }
        int? MarcaId { get; set; }
        string ModeloNombre { get; set; }
        int? ModeloId { get; set; }
        int? Anio { get; set; }
        DateTime? FechaCompra { get; set; }
        decimal? MontoFactura { get; set; }
        string FabricanteNombre { get; set; }
        int? FabricanteId { get; set; }
        string MotorizacionNombre { get; set; }
        int? MotorizacionId { get; set; }
        string AplicacionNombre { get; set; }
        int? AplicacionId { get; set; }
        DateTime? FechaProximoServicio { get; set; }
        int? KilometrajeProximoServicio { get; set; }
        int? KilometrajeInicial { get; set; }
        #region SC0001
        int? HorasIniciales { get; set; }
        int? CombustibleTotal { get; set; }
        #endregion
        #region SC0002
        bool? EntraMantenimiento { get; set; }
        GridView GridAliados { get; set; }
        string Mensaje { get; set; }
        #endregion
        //Página 1
        string Propietario { get; set; }
        string Cliente { get; set; }
        int? ClienteId { get; set; }
        int? UnidadOperativaId { get; }
        string SucursalNombre { get; set; }
        int? SucursalId { get; set; }
        Enum Area { get; set; }

       


        #region [REQ:13285 Integración de los negocios de generación y construcción]
        
        string OrdenCompraProveedor { get; set; } 
        decimal? MontoArrendamiento { get; set; }
        string CodigoMoneda { get; set; }
        DateTime? FechaInicioArrendamiento { get; set; }
        DateTime? FechaFinArrendamiento { get; set; }
        int? ProveedorID { get; set; }
        int verTabDT { get; set; }
        int verTabNS { get; set; }
        int verTabLL { get; set; }
        int verTabEA { get; set; }        
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
        List<LlantaBO> Llantas { get; set; }
        int? RefaccionID { get; set; }
        string RefaccionCodigo { get; set; }
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
        string LibroActivos { get; set; }
        string NombreClienteUnidadOperativa { get; set; }
        #endregion

        #region[REQ: 13285: Integración de Generación y Construcción]
        List<CatalogoBaseBO> ListaAcciones { get; set; }
        bool? Accesorio { get; set; }
        DateTime? FechaInicioDepreciacion { get; set; }
        DateTime? FechaIdealDesflote { get; set; }
        decimal? TasaDepreciacion { get; set; }
        int? VidaUtil { get; set; }
        decimal? PorcentajeValorResidual { get; set; }
        decimal? ValorResidual { get; set; }
        decimal? SaldoPorDepreciar { get; set; }
        List<ArchivoBO> ArchivosOC { get; set; }
        string ValoresTabs { get; set; }
        #endregion


        #region Métodos
        void PrepararNuevo();

        void HabilitarActivoFijo(bool habilitar);
        void HabilitarUnidad(bool habilitar);
        void HabilitarModelo(bool habilitar);

        #region SC0001
        void HabilitarKMInicial(bool habilitar);
        void HabilitarHRSInicial(bool habilitar);
        #endregion

        void PermitirRegresar(bool habilitar);
        void PermitirContinuar(bool habilitar);
        void PermitirCancelar(bool habilitar);
        void PermitirGuardarBorrador(bool habilitar);
        void PermitirGuardarTerminada(bool habilitar);

        void OcultarContinuar(bool ocultar);
        void OcultarTerminar(bool ocultar);

        void EstablecerPagina(int numeroPagona);

        void RedirigirAConsulta();
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #region SC0008

        void RedirigirSinPermisoAcceso();
        void RegistrarScriptMensaje(string key, string script);

        #endregion

        #region[REQ:13285 Integración de los negocios de generación y construcción]
        void EstablecerAcciones();
        int ConfigurarTab(string cEtiquetaBuscar, int tabVisible);
        string ObtenerEtiquetadelResource(string cEtiquetaBuscar, ETipoEmpresa tipoEmpresa);
        int ValidarTab(string cEtiquetaBuscar);
        #endregion

        #endregion

    }
}
