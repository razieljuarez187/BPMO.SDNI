//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Tramites.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucTramiteGPSVIS
    {
        #region Propiedades
        int? UC { get; }
        int? UUA { get; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        GPSBO UltimoObjetoGPS { get; set; }
        string Compania { get; set; }
        DateTime? FechaInstalacion { get; set; }
        string Numero { get; set; }
        #endregion

        #region Métodos
        void ModoEdicion(bool habilitar);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        #endregion
    }
}
