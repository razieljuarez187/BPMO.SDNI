//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucAsignacionLlantasVIS
    {
        #region Propiedades
        int? EnllantableID { get; set; }
        int? SucursalEnllantableID { get; set; }
        int? TipoEnllantable { get; set; }
        string DescripcionEnllantable { get; set; }

        int? LlantaID { get; set; }
        string Codigo { get; set; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
        string Medida { get; set; }
        decimal? Profundidad { get; set; }
        bool? Revitalizada { get; set; }
        bool? Stock { get; set; }
        bool? Activo { get; set; }
        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }
        int? UC { get; set; }
        int? UUA { get; set; }
        int? Posicion { get; set; }

        List<LlantaBO> Llantas { get; set; }
        List<LlantaBO> UltimoLlantas { get; set; }

        int? RefaccionID { get; set; }
        string RefaccionCodigo { get; set; }
        int? RefaccionSucursalID { get; set; }
        string RefaccionSucursalNombre { get; set; }
        string RefaccionMarca { get; set; }
        string RefaccionModelo { get; set; }
        string RefaccionMedida { get; set; }
        decimal? RefaccionProfundidad { get; set; }
        bool? RefaccionRevitalizada { get; set; }
        bool? RefaccionStock { get; set; }
        bool? RefaccionActivo { get; set; }
        DateTime? RefaccionFC { get; set; }
        DateTime? RefaccionFUA { get; set; }
        int? RefaccionUC { get; set; }
        int? RefaccionUUA { get; set; }

        IucLlantaVIS VistaLlanta { get; }
        IucLlantaVIS VistaRefaccion { get; }

        int? UsuarioAutenticado { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();

        void HabilitarModoEdicion(bool habilitar);

        void ActualizarLlantas();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
