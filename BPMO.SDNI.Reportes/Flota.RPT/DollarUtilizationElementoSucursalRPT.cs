using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using BPMO.SDNI.Flota.Reportes.DA;

namespace BPMO.SDNI.Flota.RPT
{
    public partial class DollarUtilizationElementoSucursalRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor por default
        /// </summary>
        public DollarUtilizationElementoSucursalRPT()
        {
            InitializeComponent();
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
                this.xrChartElementoSucursal.DataSource = this.DataSource;
            }
        }

        /// <summary>
        /// Establece el filtro a aplicar a la gráfica
        /// </summary>
        /// <param name="sucursalID">Id de la sucursal a mostrar</param>
        public void SetFilter(int sucursalID)
        {
            this.FilterString = String.Format("[SucursalID] = {0}", sucursalID);
            foreach (Series serie in this.xrChartElementoSucursal.Series)
            {
                serie.DataFilters.Clear();
                DataFilter filter = new DataFilter("SucursalID", typeof(Int32).FullName, DataFilterCondition.Equal, sucursalID);
                serie.DataFilters.Add(filter);
            }
        }

        /// <summary>
        /// Crea las coordenadas del eje X, usando las primeras letras del mes
        /// </summary>
        public void CreateAxisX()
        {
            XYDiagram diagrama = (this.xrChartElementoSucursal.Diagram as XYDiagram);
            diagrama.AxisX.CustomLabels.Clear();

            DollarUtilizationDS rs = this.DataSource as DollarUtilizationDS;

            foreach (DollarUtilizationDS.MesesRow row in rs.Meses.Rows)
            {
                CustomAxisLabel customAxisLabel = new CustomAxisLabel();
                customAxisLabel.Name = row.Nombre.Substring(0,1).ToUpper();
                customAxisLabel.AxisValue = row.Mes;
                diagrama.AxisX.CustomLabels.Add(customAxisLabel);
            }
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualizar la gráfica.
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void xrChartElementoSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRChart xrChart = sender as XRChart;

            DollarUtilizationDS.ResultadoDollarUtilizationXSucursalRow row = ((this.GetCurrentRow() as DataRowView).Row as DollarUtilizationDS.ResultadoDollarUtilizationXSucursalRow);
            if (row != null && row.SucursalesRow != null && !string.IsNullOrEmpty(row.SucursalesRow.Nombre))
                xrChart.Titles[0].Text = String.Format(" {0} {1}", this.TituloGrafica.Value.ToString().ToUpper(), row.SucursalesRow.Nombre.ToUpper());
        }
    }
}
