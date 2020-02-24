//Satisface el CU029 – Reporte Días de Renta por Tipo de Unidad

using System;
using System.Data;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using System.Text;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Plantilla de la gráfica del Reporte Porcentaje Utilización de una sucursal y un modelo
    /// </summary>
    public partial class DiasRentaElementoGraficaModeloRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Contructor por default
        /// </summary>
        public DiasRentaElementoGraficaModeloRPT()
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
        /// <param name="modeloID">Id del modelo a mostrar</param>
        public void SetFilter(int? modeloID)
        {
            StringBuilder filter = new StringBuilder();
           
            if (modeloID.HasValue)
                filter.AppendFormat("[ModeloID] = {0}", modeloID);

            this.FilterString = filter.ToString();

            foreach (Series serie in this.chartTimeUtilization.Series)
            {
                serie.DataFilters.Clear();
                serie.DataFilters.ConjunctionMode = ConjunctionTypes.And;                

                if (modeloID.HasValue)
                {
                    DataFilter filterModelo = new DataFilter("ModeloID", typeof(Int32).FullName, DataFilterCondition.Equal, modeloID);
                    serie.DataFilters.Add(filterModelo);
                }
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

            ReporteRDSucursalDS.SubTotalXTipoUnidadRow row = ((this.GetCurrentRow() as DataRowView).Row as ReporteRDSucursalDS.SubTotalXTipoUnidadRow);
            if (row != null && row.ModelosRow != null && !String.IsNullOrEmpty(row.ModelosRow.Nombre))
                chart.Titles[0].Text = String.Format("{0} {1}", this.TituloGrafica.Value.ToString().ToUpper(), row.ModelosRow.Nombre.ToUpper());
        }
    }
}
