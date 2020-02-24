//Satisface al CU030 - Registrar Terminación de Contrato de Mantenimiento
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.Mantto.BO;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface ICancelarContratoManttoVIS
    {
        #region Propiedades

        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        int? ModuloID { get; }

        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        string NumeroContrato { get; set; }
        int? TipoContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }

        string RepresentanteEmpresa { get; set; }

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }

        DateTime? FechaContrato { get; set; }
        DateTime? FechaInicioContrato { get; set; }
        DateTime? FechaTerminacionContrato { get; set; }
        int? Plazo { get; set; }

        string CodigoMoneda { get; set; }

        int? ClienteID { get; set; }
        bool? ClienteEsFisica { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteTipoID { get; set; }
        string ClienteDireccionCompleta { get; set; }
        string ClienteDireccionCalle { get; set; }
        string ClienteDireccionCodigoPostal { get; set; }
        string ClienteDireccionCiudad { get; set; }
        string ClienteDireccionEstado { get; set; }
        string ClienteDireccionMunicipio { get; set; }
        string ClienteDireccionPais { get; set; }
        string ClienteDireccionColonia { get; set; }

        List<LineaContratoManttoBO> LineasContrato { get; set; }

        string UbicacionTaller { get; set; }
        decimal? DepositoGarantia { get; set; }
        decimal? ComisionApertura { get; set; }
        int? IncluyeSeguroID { get; set; }
        int? IncluyeLavadoID { get; set; }
        int? IncluyePinturaRotulacionID { get; set; }
        int? IncluyeLlantasID { get; set; }
        string DireccionAlmacenaje { get; set; }
        string Observaciones { get; set; }

        string ObservacionesCancelacion { get; set; }
        DateTime? FechaCancelacion { get; set; }
        string MotivoCancelacion { get; set; }
        #endregion

        #region Métodos

        void DeshabilitarBotonCancelar();
        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirACerrar();
        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirCancelar(bool permitir);
        void PermitirRegistrar(bool permitir);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}