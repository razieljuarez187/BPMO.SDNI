// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
// Consutrccion de Mejoras - Cobro de Rangos de Km y Horas.

using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    /// <summary>
    /// Vista que implementa la Interfaz que controla las Tarifas y los Rangos de una Tarifa de RD
    /// </summary>
    public interface IucTarifaRDVIS
    {
        #region Propiedades
        /// <summary>
        /// Capacidad de Carga de la Unidad
        /// </summary>
        int? CapacidadCarga { get; set; }
        /// <summary>
        /// Tarifa Diaria de la Unidad
        /// </summary>
        decimal? TarifaDiaria { get; set; }
        /// <summary>
        /// Kilometros Libres de la Unidad
        /// </summary>
        int? KmLibres { get; set; }
        /// <summary>
        /// Horas Libres de la Unidad
        /// </summary>
        int? HorasLibres { get; set; }
        /// <summary>
        /// Determina si la Tarifa es para Cobrar KM u Horas
        /// </summary>
        bool? CobraKm { get; set; }
        /// <summary>
        /// Lista de Rangos que le pertenecen a la Tarifa
        /// </summary>
        List<RangoTarifaRDBO> RangosTarifa { get; set; }
        /// <summary>
        /// Rango Inicial
        /// </summary>
        int? RangoInicial { get; set; }
        /// <summary>
        /// Rango Final
        /// </summary>
        int? RangoFinal { get; set; }
        /// <summary>
        /// Costo Rango
        /// </summary>
        decimal? CostoRango { get; set; }
        /// <summary>
        /// Determina si sera el Rango Final de la Tarifa
        /// </summary>
        bool? EsRangoFinal { get; set; }
        /// <summary>
        /// Determina si se pueden Crear Rangos o Se crea un UnicoRango
        /// </summary>
        bool? CrearRangos { get; set; }
        /// <summary>
        /// Tarifa por Km Adicional / Modo Antiguo
        /// </summary>
        decimal? TarifaKmAdicional { get; set; }
        /// <summary>
        /// Tarifa por Hr Adicional / Modo Antiguo
        /// </summary>
        decimal? TarifaHrAdicional { get; set; }

        #endregion

        #region Métodos
        void LimpiarSesion();
        void ModoEdicion(bool activo);
        void PresentarRangos(List<RangoTarifaRDBO> rangoTarifa);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void PermitirTipoCargo(bool permitir);
        void PermitirKmLibres(bool permitir);
        void PermitirHrsLibres(bool permitir);
        void PermitirRangoInicial(bool permitir);
        void PermiritRangoCargo(bool permitir);
        void PermitirRangoFinal(bool permitir);
        void PermitirCargoAdicional(bool permitir);
        void PermitirAgregarRangos(bool permitir);
        void PermitirAdicionales(bool permitir);
        void ModoAntiguo(bool modoAntiguo);

        #endregion
    }
}
