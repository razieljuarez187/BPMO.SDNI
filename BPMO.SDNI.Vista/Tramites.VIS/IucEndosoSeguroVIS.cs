//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System.Collections.Generic;
using BPMO.SDNI.Tramites.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucEndosoSeguroVIS
    {
        #region Propiedades
        int? EndosoID { get; set; }
        string Motivo { get; set; }
        decimal? Importe { get; set; }
        List<EndosoBO> Endosos { get; set; }
        List<EndosoBO> EndososBorrados { get; set; }
        decimal? PrimaAnualTotal { get; set; }
        decimal? TotalEndosos { get; set; }
        decimal? PrimaAnual { get; set; }
        int IndicePaginaResultado { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararVista();
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        void ActualizarLista();
        void LimpiarCampos();
        #endregion
    }
}