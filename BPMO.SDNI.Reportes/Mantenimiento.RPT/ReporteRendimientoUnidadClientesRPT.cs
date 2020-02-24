using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class ReporteRendimientoUnidadClientesRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteRendimientoUnidadClientesRPT()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteUnidades = sender as XRSubreport;
            if (reporteUnidades.ReportSource == null)
                return;

            reporteUnidades.ReportSource.DataSource = this.DataSource;

            var areaID = this.GetCurrentColumnValue<Int32>("AreaID");
            var clienteID = this.GetCurrentColumnValue<Int32>("ClienteID");

            reporteUnidades.ReportSource.FilterString = String.Format("[ClienteID] = {0} ", clienteID).ToString();
        }

    }
}
