//Satisface el CU066 - Reporte Sistemas Revisados
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using System.Data;
using DevExpress.XtraCharts;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Clase de Subreporte para la Gráfica de Sistemas Revisados
    /// </summary>
    public partial class SistemasRevisadosGraficaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public SistemasRevisadosGraficaRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza la grafica de sistemas revisados
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void xrChart1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DataSet datasource = GetCurrentRow() as DataSet;
            xrChart1.DataSource = datasource;
            Series series = this.xrChart1.Series[0];
            ((PieSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
            series.Label.ResolveOverlappingMode = ResolveOverlappingMode.Default;
        }


        /// <summary>
        /// Realiza la asignación del origen de datos con el reporte y la gráfica
        /// </summary>
        public void BindToDataSource()
        {
            if (this.DataSource != null)
            {
                this.xrChart1.DataSource = this.DataSource;
            }
        }
    }
}
