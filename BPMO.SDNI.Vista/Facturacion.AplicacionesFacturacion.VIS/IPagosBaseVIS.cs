// Satisface al caso de uso CU004 - Consulta de Pagos a Facturar

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    public interface IPagosBaseVIS
    {
        #region Propiedades

        EDepartamento? DepartamentoSeleccionado { get; }
        DateTime? FechaVencimientoInicio { get; set; }
        DateTime? FechaVencimientoFin { get; set; }
        List<PagoUnidadContratoBO> PagosConsultados { get; set; }
        List<PagoContratoPSLBO> PagosConsultadosPSL { get; set; }
        string SucursalNombre { get; }
        #region SC0035
        string NumeroContrato { get; set; }
        int? CuentaClienteID { get; set; }
        string NombreCuentaCliente { get; set; }
        string VinNumeroEconomico { get; set; }
        #endregion
        int? SucursalSeleccionadaID { get; }
        List<SucursalBO> SucursalesUsuario { get; set; }
        int? UnidadOperativaID { get; }
        int? UsuarioID { get; }
        AdscripcionBO Adscripcion { get; }

        #endregion 

        #region Metodos

        void MarcarPagosFacturar();
        void MarcarPagoNoFacturados();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSession();

        #endregion
    }
}
