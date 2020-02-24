//Satisface al caso de uso CU021 - Reporte de Contratos de Mantenimiento Registrados
using System;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.RPT
{
    public partial class ContratoCMRegistradosFlotaTotalRPT : XtraReport
    {
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        public ContratoCMRegistradosFlotaTotalRPT()
        {
            InitializeComponent();
        }
       
        /// <summary>
        /// Método ejecutado antes de imprimir cada sección del subreporte
        /// </summary>
        private void sbrptFlotaTotalElemento_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport subReport = ((XRSubreport)sender);
            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = this.DataSource;
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            subReport.ReportSource.FilterString = String.Format("[SucursalID] = {0}", sucursalID);
        }
    }
}
