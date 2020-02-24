//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Tramites.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucSiniestroSeguroVIS
    {
        #region Propiedades
        int? SiniestroID { get; set; }
        string Numero { get; set; }
        DateTime? Fecha { get; set; }
        string Descripcion { get; set; }
        string Estatus { get; set; }
        List<SiniestroBO> Siniestros { get; set; }
        List<SiniestroBO> SiniestrosBorrados { get; set; }
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
        void ReestablecerColumnas();
        #region CU0004
        void ColumnasModoRegistrar();
        void HabilitarControles();
        void DeshabilitarControles();
        #endregion

        #endregion
    }
}