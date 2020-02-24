//Satisface al CU017 - Imprimir Calcomanía Mantenimiento
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class ReporteCalcomaniasRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteCalcomaniasRPT()
        {
            InitializeComponent();
        }

        public ReporteCalcomaniasRPT(Dictionary<string, object> datos)
            : this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentException("datos.DataSource");

            this.DataSource = datos["DataSource"];
        }

        private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionesSistema"].AsEnumerable()
                                                    .Select(x => (String)x["URLLogoEmpresa"])
                                                    .FirstOrDefault();

            this.pbLogo.ImageUrl = url;

        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //XRSubreport reporteUnidades = sender as XRSubreport;
            //if (reporteUnidades.ReportSource == null)
            //    return;
            //reporteUnidades.ReportSource.DataSource = this.DataSource;
        }

    }
}
