// Satisface al CU002 - Editar Contrato Renta Diaria
using System;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IAgregarDocumentosContratoVIS
    {
        #region Propiedades
        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }

        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        #endregion

        #region Metodos
        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirRegistrar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion        
    }
}