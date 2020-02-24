//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;
using System.Web.UI.WebControls;

using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucAsignacionEquiposAliadosVIS
    {
        string EquipoAliadoNumeroSerie { get; set; }
        int? EquipoAliadoId { get; set; }
        List<EquipoAliadoBO> EquiposAliados { get; set; }
        List<EquipoAliadoBO> UltimoEquiposAliados { get; set; }
        int? UnidadOperativaId { get; }
        int? UsuarioAutenticado { get; set; }
        List<int?> SucursalesSeguridad { get; set; }

        GridView GridAliados { get; set; }
        void PrepararNuevo();
        void PrepararNuevoEquipoAliado();

        void HabilitarModoEdicion(bool habilitar);

        void ActualizarEquiposAliados();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #region 13285 Acta de nacimiento

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);

        #endregion
    }
}
