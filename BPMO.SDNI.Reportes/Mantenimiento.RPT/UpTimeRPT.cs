//Satisface al CU069 - Reporte Up Time
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class UpTimeRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public UpTimeRPT()
        {
            InitializeComponent();
        }

        public UpTimeRPT(Dictionary<string, object> datos)
            : this()
        {
            if (!datos.ContainsKey("DataSource"))
            {
                throw new ArgumentException("datos.DataSource");
            }

            this.DataSource = datos["DataSource"];
        }

        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionesSistema"].AsEnumerable()
                                                    .Select(x => (String)x["URLLogoEmpresa"])
                                                    .FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void xrSubreport1_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteUnidades = sender as XRSubreport;
            if (reporteUnidades.ReportSource == null)
                return;
            reporteUnidades.ReportSource.DataSource = this.DataSource;
            var areaID = this.GetCurrentColumnValue<Int32>("AreaID");

            reporteUnidades.ReportSource.FilterString = String.Format("[AreaID] = {0}", areaID).ToString();
        }

        private void xrLabelRangoFechas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void xrLabelRangoFechas_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            string inicioPeriodo = (this.DataSource as DataSet).Tables["ConfiguracionesSistema"].AsEnumerable()
                                                   .Select(x => x["FechaInicio"]).FirstOrDefault().ToString();

            string finPeriodo = (this.DataSource as DataSet).Tables["ConfiguracionesSistema"].AsEnumerable()
                                                   .Select(x => x["FechaFin"]).FirstOrDefault().ToString();

            if (inicioPeriodo != string.Empty && finPeriodo != string.Empty)
                xrLabelRangoFechas.Text = "DEL " + inicioPeriodo + " AL " + finPeriodo;
            else
                xrLabelRangoFechas.Text = string.Empty;

        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteTotales = sender as XRSubreport;
            if (reporteTotales.ReportSource == null)
                return;
            reporteTotales.ReportSource.DataSource = this.DataSource;
        }

    }
}
