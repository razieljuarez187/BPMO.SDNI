using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IConsultarTarifaPSLVIS {
        #region Propiedades

        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? SucursalID { get; set; }
        string NombreSucursal { get; set; }
        int? ModeloID { get; set; }
        string NombreModelo { get; set; }
        string CodigoMoneda { get; set; }
        int? TipoTarifa { get; set; }
        EPeriodosTarifa PeriodoTarifa { get; set; }
        Enum TarifaTurno { get; set; }
        List<TarifaPSLBO> ListaTarifas { get; set; }

        bool? Estatus { get; set; }

        #region SC0024
        int IndicePaginaResultado { get; set; }
        #endregion

        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);
        void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipo);
        void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turno);
        void EstablecerOpcionesPeriodoTarifa(Dictionary<string, string> periodo);
        void EstablecerDatosNavegacion(object tarifa, Dictionary<string, object> elementosFiltro);//SC0024
        void RedirigirADetalle();
        void ActualizarListaTarifas();
        void PermitirRegistrar(bool activo);
        void RedirigirSinPermisoAcceso();
        void LimpiarSesion();
        #region SC0024
        object ObtenerPaqueteNavegacion();//SC0024
        void LimpiarPaqueteNavegacion();//SC0024
        #endregion
        #endregion
    }
}