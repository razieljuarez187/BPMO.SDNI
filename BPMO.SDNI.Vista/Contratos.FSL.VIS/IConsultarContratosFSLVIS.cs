// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.FSL.BOF;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IConsultarContratosFSLVIS
    {
        #region Propiedades
        UnidadOperativaBO UnidadOperativa { get; }
        string NumeroContrato { get; set; }
        int? CuentaClienteID { get; set; }
        int? ClienteID { get; set; }
        string NombreCuentaCliente { get; set; }
        int? PlazoMeses { get; set; }
        DateTime? FechaInicioContrato{ get; set; }
        DateTime? FechaTerminoContrato { get; set; }
        EEstatusContrato? Estatus { get; }
        List<ContratoFSLBOF> ContratosEncontrados { get; }
        UsuarioBO Usuario { get; }
        String SucursalNombre { get; set; }
        int? SucursalID { get; set; }
        //SC_0008
        int? UsuarioID { get; }
        int? UnidadAdscripcionID { get; }
        //
        List<object> SucursalesAutorizadas { get; set; }//SC_0051
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarContratosEncontrados();
        void CargarEstatus(List<EEstatusContrato> listado);
        void CargarContratosEncontrados(List<ContratoFSLBOF> contratos);
        void EstablecerPaqueteNavegacion(string Clave, int? ContratoID);
        void EstablecerPaqueteNavegacionImprimir(string Clave, Dictionary<string, object> DatosReporte);
        void IrADetalle();
        void IrAImprimir();
        void BloquearConsulta();

        void PermitirRegistrar(bool status);//SC_0008
        void RedirigirSinPermisoAcceso();//SC_0008
        #region SC0038
        void CargarArchivos(List<object> resultado);
        List<object> ObtenerPlantillas(string key);
        #endregion
        void LimpiarSession();//SC_0051
        #endregion
    }
}