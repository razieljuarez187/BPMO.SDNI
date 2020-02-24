//Satisface el Caso de uso CU003 - Consultar Contratos Renta Diaria
// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IDetalleContratoRDVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? ModuloID { get; }

        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }

        string Observaciones { get; set; }

        bool? Cancelable { get; set; }
        bool? Cerrable { get; set; }
        #endregion

        #region Métodos
        void PrepararVisualizacion();

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirAEditar();
        void RedirigirAConsulta();
        void RedirigirAAgregarDocumentos();
        void RedirigirAImprimir();
        void RedirigirACancelacion();
        void RedirigirACierre();
        void RedirigirSinPermisoAcceso();

        void PermitirRegresar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void PermitirEditar(bool permitir);
        void PermitirCerrar(bool permitir);
        void PermitirEliminar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        
        #region SC0038
        List<object> ObtenerPlantillas(string key);
        #endregion
        #endregion
    }
}