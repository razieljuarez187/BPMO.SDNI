//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IRegistrarSeguroVIS
    {
        #region Propiedades
        string VIN { get; set; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        int? UC { get; }
        int? UUA { get; }
        #region SC0008
        int? UnidadOperativaId { get; }
        #endregion
        #endregion

        #region Métodos
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #region SC0008
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion
    }
}