﻿// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IConsultarTarifasRDVIS
    {
        #region Propiedades

        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? SucursalID { get; set; }
        string NombreSucursal { get; set; }
        int? ModeloID { get; set; }
        string NombreModelo { get; set; }
        string CodigoMoneda { get; set; }//SC0024
        int? TipoTarifa { get; set; }//SC0024
        string NombreCliente { get; set; }
        int? CuentaClienteID { get; set; }
        string Descripcion { get; set; }
        List<TarifaRDBO> ListaTarifas { get; set; }

        int? CapacidadCarga { get; set; }
        bool? Estatus { get; set; }

        #region SC0024
        int IndicePaginaResultado { get; set; }
        #endregion 

        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EstablecerOpcionesMoneda(Dictionary<string,string> monedas);
        void EstablecerOpcionesTipoTarifa(Dictionary<int,string> tipo);
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
