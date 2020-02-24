//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucDatosTecnicosVIS
    {
        int? ValorInicialHorometro { get; set; }
        int? ValorFinalHorometro { get; set; }
        bool? EsHorometroActivo { get; set; }
        int? ValorInicialOdometro { get; set; }
        int? ValorFinalOdometro { get; set; }
        bool? EsOdometroActivo { get; set; }

        List<HorometroBO> Horometros { get; set; }
        List<HorometroBO> UltimoHorometros { get; set; }
        List<OdometroBO> Odometros { get; set; }
        List<OdometroBO> UltimoOdometros { get; set; }

        decimal? PBVMaximoRecomendado { get; set; }
        decimal? PBCMaximoRecomendado { get; set; }
        decimal? CapacidadTanque { get; set; }
        decimal? RendimientoTanque { get; set; }

        void PrepararNuevo();
        void PrepararNuevoOdometro();
        void PrepararNuevoHorometro();

        void HabilitarModoEdicion(bool habilitar);

        void PermitirAgregarHorometro(bool permitir);
        void PermitirAgregarOdometro(bool permitir);

        void ActualizarOdometros();
        void ActualizarHorometros();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
    }
}
