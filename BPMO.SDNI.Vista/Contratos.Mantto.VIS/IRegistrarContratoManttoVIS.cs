//Satisface al CU027 - Registrar Contrato de Mantenimiento
//Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IRegistrarContratoManttoVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        string UnidadOperativaNombre { get; }
        int? ModuloID { get; }

        int? ContratoID { get; set; }
        string NumeroContrato { get; set; }
        int? TipoContratoID { get; }
        int? EstatusID { get; set; }
        DateTime? FC { get; }
        int? UC { get; }
        DateTime? FUA { get; }
        int? UUA { get; }

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

        List<RepresentanteLegalBO> RepresentantesLegales { get; set; }
        bool? SoloRepresentantes { get; set; }
        List<ObligadoSolidarioBO> ObligadosSolidarios { get; set; }
        bool? ObligadosComoAvales { get; set; }
        List<AvalBO> Avales { get; set; }

        List<LineaContratoManttoBO> LineasContrato { get; set; }

        string UbicacionTaller { get; set; }
        decimal? DepositoGarantia { get; set; }
        decimal? ComisionApertura { get; set; }
        int? IncluyeSeguroID { get; set; }
        int? IncluyeLavadoID { get; set; }
        int? IncluyePinturaRotulacionID { get; set; }
        int? IncluyeLlantasID { get; set; }
        string DireccionAlmacenaje { get; set; }

        List<DatoAdicionalAnexoBO> DatosAdicionales { get; set; }

        string Observaciones { get; set; }
        int? DireccionClienteID { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();

        void PermitirGuardarBorrador(bool permitir);
        void PermitirGuardarTerminado(bool permitir);

        void RedirigirSinPermisoAcceso();
        void RedirigirAConsulta();
        void RedirigirADetalles();

        void LimpiarSesion();

        void EstablecerPaqueteNavegacion(string key, object value);

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
