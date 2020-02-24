//Satisface CU009 – Consultar Tablero de Seguimiento Unidades Renta Diaria
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.BOF;
using System;

namespace BPMO.SDNI.Flota.VIS{

    public interface IConsultarTableroRDVIS{

        #region Propiedades
        int? UnidadOperativaID { get;}
        string NumeroEconomico { get; set; }
        int? MarcaID { get; set; }
        string MarcaNombre { get; set; }
        int? ModeloID { get; set; }
        string ModeloNombre { get; set; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? EstatusUnidad { get; set; }
        bool? EstaEnTaller { get; set; }
        bool? EstaReservada { get; set; }
        string NumeroContrato { get; set; }
        DateTime? FechaContratoInicial { get; set; }
        DateTime? FechaContratoFinal { get; set; }
        List<FlotaRDBOF> UnidadesRD { get; set; }
        int IndicePaginaResultado { get; set; }
        int? UsuarioAutenticado { get; }
        bool ActivarDetallesUnidad { get; set; }
        bool ActivarDetallesContrato { get; set; }
        #endregion

        #region Métodos
        void PrepararBusqueda();
        void ActualizarResultado();
        void EstablecerPaqueteNavegacion(string Nombre, int? unidadID);
        void RedirigirAReservaciones();
        void RedirigirADetallesUnidad();
        void RedirigirADetallesContrato();
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void HabilitarModelo(bool estado);
        void RedirigirSinPermisoAcceso();
        void DesactivarReservaciones();
        #endregion
    }
}