//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IConsultarSeguroVIS
    {
        #region Propiedades 
        string VIN { get; set; }
        string NumeroPoliza { get; set; }
        string Aseguradora { get; set; }
        List<SeguroBO> Seguros { get; set; }
        int? TramitableID { get; set; }
        bool? Vencido { get; set; }

        #region SC0008
        int? UsuarioId { get; }
        int? UnidadOperativaId { get; }
        #endregion
        #endregion

        #region Métodos
        void PrepararBusqueda();
        void LimpiarSesion();
        void ActualizarLista();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        void EstablecerPaqueteNavegacion(string key, int? id);
        void IrADetalle();

        void RedirigirSinPermisoAcceso(); //SC0008
        #endregion
    }
}