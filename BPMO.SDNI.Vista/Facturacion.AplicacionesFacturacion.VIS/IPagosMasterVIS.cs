// Satisface al caso de uso CU004 - Consulta de Pagos a Facturar
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    public delegate void DelegateAccionConsultar();
    public interface IPagosMasterVIS: IPagosBaseVIS
    {
        #region Propiedades
        DelegateAccionConsultar AccionConsultar { get; set; }
        int TotalFacturar { get; set; }
        int TotalNoFacturado { get; set; }
        #endregion      
  
        #region Metodos
        void CargarDepartamentos(List<EDepartamento> listado);
        void CargarSucursales(List<SucursalBO> lstSucursales);
        void EstablecerSucursalSeleccionada(int? Id);
        void IrConsultarPagosFacturar();
        void IrConsultarPagosNoFacturados();
        #endregion
    }
}
