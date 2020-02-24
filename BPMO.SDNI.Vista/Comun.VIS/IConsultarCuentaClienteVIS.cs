//Satiface al caso de uso CU068 - Catáloglo de Clientes
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS {
    public interface IConsultarCuentaClienteVIS {
        #region Propiedades
        UnidadOperativaBO UnidadOperativa { get; }
        List<CuentaClienteIdealeaseBO> ListaClientes { get; set; }
        string NombreCuenta { get; set; }
        string Nombre { get; set; }
        bool? Fisica { get; set; }
        string RFC { get; set; }

        #region SC0008
        int? UsuarioId { get; }
        #endregion

        int? CuentaClienteID { get; set; }
        int? ClienteID { get; set; }

        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle);
        void MostrarDatos();

        void LimpiarSesion();

        void RedirigirADetalles();

        void EstablecerPaqueteNavegacion(string nombre, object valor);

        #region SC0008
        void PermitirRegistrar(bool permitir);
        void RedirigirSinPermisoAcceso();
        #endregion

        #endregion
    }
}