//Satisface el Caso de uso CU003 - Consultar Contratos Renta Diaria
//Satisface a la solicitud de cambio SC0035
using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.BO;
using System.Collections.Generic;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    public interface IucResumenContratoRDVIS
    {
        #region Propiedades

        UnidadOperativaBO UnidadOperativa { get; }
        DateTime? FechaContrato { get; set; }
        int? EmpresaID { get; set; }
        string NombreEmpresa { get; set; }
        string NombreSucursal { get; set; }
        string NombreCuentaCliente { get; set; }
        string RFCCliente { get; set; }
        string DireccionCliente { get; set; }
        UsuarioBO Usuario { get; }
        int? ModuloID { get; }
        /// <summary>
        /// Numero de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        String NumeroCuentaCliente { get; set; }
        #region SC0035
        DateTime? FechaCierreContrato { get; set; }
        EFrecuencia? FrecuenciaFacturacion { get; set; }
        #endregion

        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion

    }
}
