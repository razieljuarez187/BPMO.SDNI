using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.RPT {
    public partial class SubResumenIngresoRentaRPT : DevExpress.XtraReports.UI.XtraReport {
        public SubResumenIngresoRentaRPT() {
            InitializeComponent();
        }

        public SubResumenIngresoRentaRPT(DataTable table) {
            InitializeComponent();
            this.DataSource = table;
            ImprimirReporte();
        }

        public void ImprimirReporte() {
            try {
                this.DataMember = "ResumenIngreso";
                this.xrSucursal.DataBindings.Add("Text", this.DataSource, "SUCURSAL");
                this.xrContado.DataBindings.Add("Text", this.DataSource, "Contado");
                this.xrCredito.DataBindings.Add("Text", this.DataSource, "Credito");
                this.xrSubTotal.DataBindings.Add("Text", this.DataSource, "TRENTA", "{0: $#,0.00}");
                this.xrIva.DataBindings.Add("Text", this.DataSource, "TIVARENTA", "{0: $#,0.00}");
                this.xrTotal.DataBindings.Add("Text", this.DataSource, "TTOTAL", "{0: $#,0.00}");
            }
            catch (Exception e) {
                throw new Exception("SubResumenIngresoRentaRPT.ImprimirReporte:Error al intentar generar el reporte." + e.Message);
            }
        }

    }
}
