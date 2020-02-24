//Satisface el CU028 – Reporte Días de Renta
//Satisface el CU029 – Reporte Días de Renta por Tipo de Unidad

using System;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Plantilla de Reporte Porcentaje Utilización Global
    /// </summary>
    public partial class DiasRentaGlobalGraficaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Contructor por default
        /// </summary>
        public DiasRentaGlobalGraficaRPT()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Realiza la asignación del origen de datos con el reporte y la gráfica
        /// </summary>
        /// <param name="report">Reporte padre que contiene el origen de datos</param>
        public void BindToDataSource(XtraReport report)
        {
            if (report == null)
                return;

            if (report.DataSource != this.DataSource)
            {
                this.TituloGraficaGlobal.Value = report.Parameters["TituloGraficaGlobal"].Value;
                this.DataSource = report.DataSource;
                this.chartTimeUtilization.DataSource = this.DataSource;
            }
        }

        /// <summary>
        /// Crea las cordenas de Eje X con la primera letra del Mes
        /// </summary>
        public void CreateAxisX()
        {
            XYDiagram diagram = (this.chartTimeUtilization.Diagram as XYDiagram);
            diagram.AxisX.CustomLabels.Clear();

            ReporteRDSucursalDS dataSet = this.DataSource as ReporteRDSucursalDS;

            foreach (ReporteRDSucursalDS.MesesRow row in dataSet.Meses.Rows)
            {
                CustomAxisLabel customAxisLabel = new CustomAxisLabel();
                customAxisLabel.Name = row.Nombre.Substring(0, 1).ToUpper();
                customAxisLabel.AxisValue = String.Format("{0}-{1}", row.Anio, row.Mes);
                diagram.AxisX.CustomLabels.Add(customAxisLabel);
            }
        }

        /// <summary>
        /// Evento que se ejecuta antes de realizar la visualización de la grafica de reporte
        /// </summary>
        /// <param name="sender">Objeto que disparo el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void chartTimeUtilization_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRChart chart = sender as XRChart;
            chart.Titles[0].Text = this.TituloGraficaGlobal.Value.ToString().ToUpper();
        }        
    }
}
