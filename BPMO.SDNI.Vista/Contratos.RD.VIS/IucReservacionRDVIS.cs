// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IucReservacionRDVIS
    {
        #region Propiedades
        int? ReservacionID { get; set; }
        DateTime? FC { get; set; }
        int? UC { get; set; }
        DateTime? FUA { get; set; }
        int? UUA { get; set; }
        bool? Activo { get; set; }
        int? UnidadOperativaID { get; set; }
        List<int?> SucursalesSeguridad { get; set; }
        int? TipoID { get; set; }

        string Numero { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? ModeloID { get; set; }
        string ModeloNombre { get; set; }

        DateTime? FechaReservacionInicial { get; set; }
        TimeSpan? HoraReservacionInicial { get; set; }
        DateTime? FechaReservacionFinal { get; set; }
        TimeSpan? HoraReservacionFinal { get; set; }

        string NumeroEconomico { get; set; }
        int? UnidadID { get; set; }
        string UnidadSerie { get; set; }
        int? UnidadAnio { get; set; }
        string UnidadPlacaEstatal { get; set; }
        string UnidadPlacaFederal { get; set; }
        decimal? UnidadCapacidadCarga { get; set; }
        string UnidadMarcaNombre { get; set; }
        decimal? UnidadCapacidadTanque { get; set; }
        decimal? UnidadRendimientoTanque { get; set; }
        string UnidadEstatusOperacion { get; set; }
        string UnidadEstatusMantenimiento { get; set; }
        DateTime? UnidadFechaPlaneadaLiberacion { get; set; }
        Object UnidadEquiposAliados { get; set; }

        int? UsuarioReservoID { get; set; }
        string UsuarioReservoNombre { get; set; }

        string Observaciones { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la sucursal
        /// </summary>
        int? SucursalID { get; set; }//SC051
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        string SucursalNombre { get; set; }//SC051
        #endregion

        #region Métodos
        void PrepararNuevo();

        void HabilitarModoEdicion(bool habilitar);
        void HabilitarCuentaCliente(bool habilitar);
        void HabilitarModelo(bool habilitar);
        void HabilitarUnidad(bool habilitar);
        void HabilitarSucursal(bool habilitar);//SC051

        void MostrarDetalleUnidad(bool mostrar);
        void MostrarUsuarioReservo(bool mostrar);
        void MostrarNumeroReservacion(bool mostrar);
        void MostrarActivo(bool mostrar);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
