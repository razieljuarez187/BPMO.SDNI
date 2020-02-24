using System;
using System.Collections.Generic;
using System.Data;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.RPT {
    /// <summary>
    /// Reporte de ingreso de rentas
    /// </summary>
    public partial class IngresoRentasRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        /// <param name="datos">Información del reporte</param>
        public IngresoRentasRPT(Dictionary<string, object> datos) {
            InitializeComponent();
            
            ImprimirReporte(datos);
        }

        /// <summary>
        /// Se realiza en enlazado de los datos en el reporte
        /// </summary>
        /// <param name="datos">Información del reporte</param>
        public void ImprimirReporte(Dictionary<string,object> datos) {
            try {
                this.xrLogo.ImageUrl = datos["Logo"].ToString();
                this.xrFecActual.Text = DateTime.Now.ToShortDateString();
                this.xrHoraActual.Text = DateTime.Now.ToShortTimeString();
                this.xrUsuario.Text = datos["Usuario"].ToString();
                this.xrUnidadOperativa.Text = datos["UO"].ToString();
                this.xrFechas.Text = datos["RangoFechas"].ToString();
                this.xrSucursal.Text = datos["Sucursal"].ToString();
                DataSet ds = (DataSet)datos["datasource"];
                this.xrSubSucursalIngreso.ReportSource = new SubIngresoSucursalRPT(ds.Tables[0]);
                this.xrSubResumen.ReportSource = new SubResumenIngresoRentaRPT(ds.Tables[1]);
                this.xrsrSeries.ReportSource = new SubEquiposNumerosSerieRPT(ds.Tables[2]);
                
                decimal total = 0;
                decimal subtotal = 0;
                decimal sumaiva = 0;
                foreach (DataRow row in ds.Tables[0].Rows) {
                    decimal totalRow = 0;
                    decimal subtotalRow = 0;
                    decimal ivaRow = 0;
                    total += (Decimal.TryParse(row["Total"].ToString(), out totalRow)) ? totalRow : 0;
                    subtotal += (Decimal.TryParse(row["RENTA"].ToString(), out subtotalRow)) ? subtotalRow : 0;
                    sumaiva += (Decimal.TryParse(row["IVARENTA"].ToString(), out ivaRow)) ? ivaRow : 0;
                }

                this.xrSubtotalResumen.Text = string.Format("{0: $#,0.00}",subtotal);
                this.xrIVAResumen.Text = string.Format("{0: $#,0.00}", sumaiva);
                this.xrTotalResumen.Text = string.Format("{0: $#,0.00}",total);

                

            }
            catch (Exception e) {
                throw new Exception("IngresoRentasRPT.ImprimirReporte:Error al intentar generar el reporte." + e.Message);
            }
        }
    }
}
