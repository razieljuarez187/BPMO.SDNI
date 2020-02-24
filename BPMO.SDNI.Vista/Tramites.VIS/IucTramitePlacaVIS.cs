//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.VIS
{
    public interface IucTramitePlacaVIS
    {
        #region Propiedades
        ETipoTramite? tipo { get; set; }
        int? UC { get; }
        int? UUA { get; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        
        object UltimoObjeto { get; set; }
        string Numero { get; set; }
        string NumeroGuia { get; set; }
        DateTime? FechaEnvio { get; set; }
        DateTime? FechaRecepcion { get; set; }		
        bool? Activo { get; set; }		
        #endregion

        #region Métodos
        void ModoEdicion(bool habilitar);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        void PlacaEstatal();
        void PlacaFederal();
        #endregion
    }
}
