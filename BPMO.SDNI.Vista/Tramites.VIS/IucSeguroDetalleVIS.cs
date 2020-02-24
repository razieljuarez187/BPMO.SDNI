//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucSeguroDetalleVIS
    {
        #region Propiedades
        string VIN { get; set; }
        string Modelo { get; set; }
        string NumeroPoliza { get; set; }
        string Aseguradora { get; set; }
        string Contacto { get; set; }
        decimal? PrimaAnual { get; set; }
        decimal? PrimaSemestral { get; set; }
        DateTime? VigenciaInicial { get; set; }
        DateTime? VigenciaFinal { get; set; }
        string Observaciones { get; set; }
        bool? Activo { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        int? TramitableID { get; set; }
        ETipoTramitable? TipoTramitable { get; set; }
        int? TramiteID { get; set; }
        ETipoTramite? TipoTramite { get; }
        List<DeducibleBO> Deducibles { get; set; }
        List<EndosoBO> Endosos { get; set; }
        List<SiniestroBO> Siniestros { get; set; }
        decimal? PrimaAnualTotal { get; set; }
        decimal? TotalEndosos { get; set; }
        #endregion

        #region Métodos
        void PrepararVista();

        void DatoAInterfazUsuario(object obj);
        object InterfazUsuarioADato();

        void LimpiarSesion();
        void ActualizarLista();
        #endregion
    }
}