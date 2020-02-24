//Satisface al caso de uso CU029 - Consultar Contratos de Mantenimiento
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.Mantto.BOF;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IConsultarContratoManttoVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }

        string NumeroContrato { get; set; }
        DateTime? FechaContratoInicial { get; set; }
        DateTime? FechaContratoFinal { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteID { get; set; }
        string SucursalNombre { get; set; }
        int? SucursalID { get; set; }
        int? EstatusID { get; set; }
        int? TipoContratoID { get; set; }
        string NumeroSerie { get; set; }
        string NumeroEconomico { get; set; }

        List<ContratoManttoBOF> Resultado { get; }

        List<object> SucursalesAutorizadas { get; set; }//SC_0051
        #endregion

        #region Métodos
        void EstablecerResultado(List<ContratoManttoBOF> resultado);
        void EstablecerOpcionesEstatus(Dictionary<int, string> estatus);

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void CargarArchivos(List<object> resultado);
        List<object> ObtenerPlantillas(string key);

        void PermitirRegistrar(bool permitir);

        void RedirigirSinPermisoAcceso();
        void RedirigirADetalle();

        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}