using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class ReporteMantenimientoRealizadoContraProgramadoTotalesRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteMantenimientoRealizadoContraProgramadoTotalesRPT()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte de un elemento
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void mantenimientoRealizadoContraProgramadoTotalesElementoRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport subReport = ((XRSubreport)sender);

            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = this.DataSource;
        }
    }
}
