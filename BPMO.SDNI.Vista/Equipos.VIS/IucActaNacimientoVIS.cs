//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucActaNacimientoVIS
    {
        string NumeroSerie { get; set; }
        string ClaveActivoOracle { get; set; }
        int? LiderID { get; set; }
        string NumeroEconomico { get; set; }
        string TipoUnidad { get; set; }
        string Modelo { get; set; }
        int? Anio { get; set; }
        DateTime? FechaCompra { get; set; }
        decimal? MontoFactura { get; set; }

        string Propietario { get; set; }
        string Cliente { get; set; }
        string Sucursal { get; set; }
        string Area { get; set; }

        List<HorometroBO> Horometros { get; set; }
        List<OdometroBO> Odometros { get; set; }
        decimal? PBVMaximoRecomendado { get; set; }
        decimal? PBCMaximoRecomendado { get; set; }
        decimal? CapacidadTanque { get; set; }
        decimal? RendimientoTanque { get; set; }

        string Radiador { get; set; }
        string PostEnfriador { get; set; }
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

        List<LlantaBO> Llantas { get; set; }
        string RefaccionCodigo { get; set; }
        string RefaccionMarca { get; set; }
        string RefaccionModelo { get; set; }
        string RefaccionMedida { get; set; }
        decimal? RefaccionProfundidad { get; set; }
        Boolean? RefaccionRevitalizada { get; set; }

        List<EquipoAliadoBO> EquiposAliados { get; set; }
       
        List<NumeroSerieBO> NumerosSerie { get; set; }
        #region 13285 Acta de nacimiento

        int? UnidadOperativaID { get; set; }

        #endregion

        void ActualizarOdometros();
        void ActualizarHorometros();
        void ActualizarLlantas();
        void ActualizarEquiposAliados();
        
        void ActualizarNumerosSerie();
        
        void LimpiarSesion();

        #region 13285 Acta de nacimiento

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa, string valoresTabs);

        #endregion
    }
}
