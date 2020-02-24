//Satisface al CU069 - Reporte Up Time
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class UpTimeSucursalRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public UpTimeSucursalRPT()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteUnidades = sender as XRSubreport;
            if (reporteUnidades.ReportSource == null)
                return;
            reporteUnidades.ReportSource.DataSource = this.DataSource;
            var sucursalID = this.GetCurrentColumnValue<Int32>("SucursalID");
            var areaID = this.GetCurrentColumnValue<Int32>("AreaID");
            reporteUnidades.ReportSource.FilterString = String.Format("[SucursalID] = {0} AND [AreaID] = {1}", sucursalID, areaID).ToString();
        }

    }
}
