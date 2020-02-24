//Satisface al CU075 - Catálogo de Equipo Aliado
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
namespace BPMO.SDNI.Equipos.VIS
{
    public interface IDetalleEquipoAliadoVIS
    {
        #region Propiedades
        int? EquipoAliadoID { get; set; }

        #region REQ 13596

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion

        #endregion

        #region Métodos
        void PrepararVista();

        object ObtenerDatosNavegacion();

        #region SC_0008
        void PermitirEliminar(bool permitir);
        void PermitirEditar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void RedirigirSinPermisoAcceso();
        #endregion

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}