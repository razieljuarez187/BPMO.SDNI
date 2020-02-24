//Satisface al CU075 - Catálogo de Equipo Aliado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
namespace BPMO.SDNI.Equipos.VIS
{
    public interface IRegistrarEquipoAliadoVIS
    {
        #region Propiedades
        #region REQ 13596

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion
        #endregion
        #region Métodos
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

		void RedirigirSinPermisoAcceso();
        #endregion
    }
}