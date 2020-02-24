// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
// Satisface al Caso de Uso CU015 - Registrar Contrato Full Service Leasing
using System;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucInformacionPagoVIS
    {
        #region Propiedades

        DateTime? FechaInicioContrato { get; set; }
        DateTime? FechaTerminacionContrato { get; set; }
        #region Cuenta Bancaria
        //string Beneficiario { get; set; }
        //string Lugar { get; set; }
        //string Banco { get; set; }
        //Banco CuentaBancariaSeleccionada { get; }
        //List<Banco> ListadoCuentasBancarias { get; set; }
        #endregion
        decimal? Total { set; }
        decimal? Mensualidad { set; }
        int? DiasPago { set; get; } // SC0007
        #endregion

        #region Metodos

        void ConfigurarModoConsultar();
        void ConfigurarModoEditar();
        #region Cuenta Bancaria
        //void EstablecerCuentaBancariaSeleccionada(int? Id);
        #endregion
        #endregion
    }
}
