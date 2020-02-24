//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Tramites.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucDeducibleSeguroVIS
    {
        #region Propiedades
        List<DeducibleBO> DeduciblesBorrados { get; set; }
        List<DeducibleBO> Deducibles { get; set; }
        int? DeducibleID { get; set; }
        string Concepto { get; set; }
        decimal? Porcentaje { get; set; }
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