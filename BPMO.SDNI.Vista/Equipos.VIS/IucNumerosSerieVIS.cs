//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucNumerosSerieVIS
    {
        string Radiador { get; set; }
        string PostEnfriador { get; set; }
        
        string Nombre { get; set; }
        string Serie { get; set; }

        List<NumeroSerieBO> NumerosSerie { get; set; }
        List<NumeroSerieBO> UltimoNumerosSerie { get; set; }
        
        #region SC0030
        string SerieMotor { get; set; }
        #endregion
        string SerieTurboCargador { get; set; }
        string SerieCompresorAire { get; set; }
        string SerieECM { get; set; }
        string SerieAlternador { get; set; }
        string SerieMarcha { get; set; }
        string SerieBaterias { get; set; }
        string TransmisionSerie { get; set; }
        string TransmisionModelo { get; set; }
        string EjeDireccionSerie { get; set; }
        string EjeDireccionModelo { get; set; }
        string EjeTraseroDelanteroSerie { get; set; }
        string EjeTraseroDelanteroModelo { get; set; }
        string EjeTraseroTraseroSerie { get; set; }
        string EjeTraseroTraseroModelo { get; set; }

        void PrepararNuevo();
        
        void PrepararNuevoNumeroSerie();
        
        void HabilitarModoEdicion(bool habilitar);
        
        void PermitirAgregarNumeroSerie(bool permitir);
        void ActualizarNumeroSerie();
        
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
    }
}
