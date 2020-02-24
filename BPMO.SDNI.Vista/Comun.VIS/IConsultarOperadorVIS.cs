// Satisface al CU092 - Catálogo de Operadores
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IConsultarOperadorVIS
    {
        #region Propiedades
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }

        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        string Nombre { get; set; }
        string LicenciaNumero { get; set; }
        bool? Estatus { get; set; }

        List<OperadorBO> Resultado { get; }
        #endregion

        #region Métodos
        void EstablecerResultado(List<OperadorBO> resultado);

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
