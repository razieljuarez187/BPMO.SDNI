// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IRegistrarReservacionRDVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }
        List<int?> SucursalesSeguridad { get; set; }

        int? ReservacionID { get; set; }
        DateTime? FC { get; set; }
        int? UC { get; set; }
        DateTime? FUA { get; set; }
        int? UUA { get; set; }
        bool? Activo { get; set; }
        int? TipoID { get; set; }

        string Numero { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? ModeloID { get; set; }
        string ModeloNombre { get; set; }

        DateTime? FechaReservacionInicial { get; set; }
        DateTime? FechaReservacionFinal { get; set; }

        int? UnidadID { get; set; }
        string NumeroEconomico { get; set; }

        int? UsuarioReservoID { get; set; }
        string UsuarioReservoNombre { get; set; }

        string Observaciones { get; set; }

        object ReservacionesRealizadas { get; set; }
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

        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
