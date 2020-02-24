using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using System.Linq;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class ReporteRendimientoUnidadAreasRPT : DevExpress.XtraReports.UI.XtraReport
    {

        public ReporteRendimientoUnidadAreasRPT(Dictionary<String, Object> datosReporte)
        {
            InitializeComponent();
            this.DataSource = datosReporte["DataSource"];
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteClientes = sender as XRSubreport;
            if (reporteClientes.ReportSource == null)
                return;

            reporteClientes.ReportSource.DataSource = this.DataSource;
            var areaID = this.GetCurrentColumnValue<Int32>("AreaID");

            reporteClientes.ReportSource.FilterString = String.Format("[AreaID] = {0}", areaID).ToString();
        }

        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void pbLogo_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as ReporteRendimientoUnidadDS).ConfiguracionesSistema.AsEnumerable().Select(x => x.UrlLogoEmpresa).FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }

    }
}
