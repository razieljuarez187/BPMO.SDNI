//Satisface al caso de uso CU071 - reporte de rendimiento de unidad
using System;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Grafica de Rendimiento de Unidad
    /// </summary>
    public partial class ReporteRendimientoUnidadGraficaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Constructor
        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public ReporteRendimientoUnidadGraficaRPT()
        {
            InitializeComponent();
        } 
        #endregion

        #region Metodos
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
                this.DataSource = report.DataSource;
                this.xrcGraficaRendimiento.DataSource = this.DataSource;
            }
        }

        /// <summary>
        /// Realiza la asignación del origen de datos con el reporte y la gráfica
        /// </summary>
        public void BindToDataSource()
        {

            if (this.DataSource != null)
            {
                this.xrcGraficaRendimiento.DataSource = this.DataSource;
            }
        }

        public void BindToDatasource(object dataSource)
        {
            if (dataSource != null)
            {
                this.xrcGraficaRendimiento.DataSource = dataSource;
            }
        }

        /// <summary>
        /// Establece el filtro a aplicar a la gráfcia
        /// </summary>
        /// <param name="sucursalID">Id de la unidad a mostrar</param>
        public void SetFilter(int unidaId)
        {
            this.FilterString = String.Format("[UnidadID] = {0}", unidaId);
            foreach (Series serie in this.xrcGraficaRendimiento.Series)
            {
                serie.DataFilters.Clear();
                DataFilter filter = new DataFilter("UnidadID", typeof(Int32).FullName, DataFilterCondition.Equal, unidaId);
                serie.DataFilters.Add(filter);
            }
        }

        /// <summary>
        /// Crea las cordenadas del eje x, usando las primeras letras del mes
        /// </summary>
        public void CreateAxisX()
        {
            XYDiagram diagram = (this.xrcGraficaRendimiento.Diagram as XYDiagram);
            diagram.AxisX.CustomLabels.Clear();

            ReporteRendimientoUnidadDS dataSet = this.DataSource as ReporteRendimientoUnidadDS;

            foreach (ReporteRendimientoUnidadDS.MesesRow row in dataSet.Meses.Rows)
            {
                CustomAxisLabel customAxisLabel = new CustomAxisLabel();
                customAxisLabel.Name = row.NombreMes.Substring(0, 1).ToUpper();
                customAxisLabel.AxisValue = String.Format("{0}-{1}", row.Anio, row.Mes);
                customAxisLabel.Visible = true;
                diagram.AxisX.CustomLabels.Add(customAxisLabel);
            }
        } 
        #endregion
    }
}
