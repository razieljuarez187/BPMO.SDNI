//Satisface el Caso de uso CU003 - Consultar Contratos Renta Diaria
//Satisface el Caso de uso CU014 - Imprimir Contrato de Renta Diaria
using System;
using System.Collections.Generic;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.RD.BOF;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IConsultarContratoRDVIS
    {

        #region Propiedades

        UnidadOperativaBO UnidadOperativa { get; }
        string NumeroContrato { get; set; }
        int? CuentaClienteID { get; set; }
        int? ClienteID { get; set; }
        string NombreCuentaCliente { get; set; }
        DateTime? FechaInicioContrato { get; set; }
        DateTime? FechaFinContrato { get; set; }
        #region SC0020
        EEstatusContrato? Estatus { get; set; }
        #endregion SC0020
        string NumeroSerie { get; set; }
        string NumeroEcononomico { get; set; }
        int? UnidadID { get; set; }
        List<ContratoRDBOF> ContratosEncontrados { get; }
        UsuarioBO Usuario { get; }
        String SucursalNombre { get;  }
        int? SucursalID { get;  }
        int? UsuarioID { get; }
        int? UnidadAdscripcionID { get; }
        List<SucursalBO> SucursalesAutorizadas { get; set; }
        #endregion

        #region Métodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarContratosEncontrados();
        void CargarEstatus(List<EEstatusContrato> listado);
        void CargarContratosEncontrados(List<ContratoRDBOF> contratos);
        void IrADetalle();
        void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, Dictionary<string, object> DatosReporte);
        void IrAImprimir();
        void BloquearConsulta();
        void PermitirRegistrar(bool status);
        void RedirigirSinPermisoAcceso();
        #region SC0020
        object ObtenerPaqueteNavegacion();
        void EstablecerPaqueteNavegacion(string ClaveContrato, ContratoRDBOF contrato, Dictionary<string, object> elementosFiltro);
        void LimpiarPaqueteNavegacion();
        void EstablecerPagResultados(int indice);
        int ObtenerPagResultados();
        void CargarSucursales(List<SucursalBO> lstSucursales);
        #endregion SC0020
        #region SC0038
        void CargarArchivos(List<object> resultado);
        List<object> ObtenerPlantillas(string key);
        #endregion
        #endregion
    }
}
