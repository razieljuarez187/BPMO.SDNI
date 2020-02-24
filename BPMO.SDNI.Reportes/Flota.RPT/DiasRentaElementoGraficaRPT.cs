//Satisface el CU028 – Reporte Días de Renta

using System;
using System.Data;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Plantilla de la gráfica del Reporte Porcentaje Utilización de una sucursal
    /// </summary>
    public partial class DiasRentaElementoGraficaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor por default
        /// </summary>
        public DiasRentaElementoGraficaRPT()
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
                this.TituloGrafica.Value = report.Parameters["TituloGrafica"].Value;
                this.DataSource = report.DataSource;                
                this.chartTimeUtilization.DataSource = this.DataSource;
            }
        }

        /// <summary>
        /// Establece el filtro a aplicar a la gráfcia
        /// </summary>
        /// <param name="sucursalID">Id de la sucursal a mostrar</param>
        public void SetFilter(int sucursalID)
        {
            this.FilterString = String.Format("[SucursalID] = {0}", sucursalID);
            foreach (Series serie in this.chartTimeUtilization.Series)
            {
                serie.DataFilters.Clear();
                DataFilter filter = new DataFilter("SucursalID", typeof(Int32).FullName, DataFilterCondition.Equal, sucursalID);
                serie.DataFilters.Add(filter);
            }
        }

        /// <summary>
        /// Crea las cordenadas del eje x, usando las primeras letras del mes
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

            ReporteRDSucursalDS.SubTotalXSucursalRow row = ((this.GetCurrentRow() as DataRowView).Row as ReporteRDSucursalDS.SubTotalXSucursalRow);
            if (row != null && row.SucursalesRow != null && !String.IsNullOrEmpty(row.SucursalesRow.Nombre))
                chart.Titles[0].Text = String.Format("{0} {1}", this.TituloGrafica.Value.ToString().ToUpper(), row.SucursalesRow.Nombre.ToUpper());
        }
    }
}
