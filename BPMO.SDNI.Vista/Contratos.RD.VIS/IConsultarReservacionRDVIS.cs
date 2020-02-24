// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Contratos.RD.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IConsultarReservacionRDVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }

        string Numero { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteID { get; set; }
        string ModeloNombre { get; set; }
        int? ModeloID { get; set; }
        string NumeroEconomico { get; set; }

        DateTime? FechaReservacionInicial { get; set; }
        TimeSpan? HoraReservacionInicial { get; set; }
        DateTime? FechaReservacionFinal { get; set; }
        TimeSpan? HoraReservacionFinal { get; set; }

        int? UsuarioReservoID { get; set; }
        string UsuarioReservoNombre { get; set; }

        bool? Activo { get; set; }

        List<ReservacionRDBO> Resultado { get; }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal
        /// </summary>
        int? SucursalID { get; set; }//SC051
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        string SucursalNombre { get; set; }//SC051
        List<object> SucursalesAutorizadas { get; set; }//SC051
        #endregion

        #region Métodos
        void EstablecerResultado(List<ReservacionRDBO> resultado);

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);
        
        void PermitirRegistrar(bool permitir);

        void RedirigirSinPermisoAcceso();
        void RedirigirADetalle();

        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}
