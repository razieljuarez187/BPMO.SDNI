//Satisface al CU024 - Reporte de Dollar Utilization
using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Reporte de Dollar Utilization
    /// </summary>
    public partial class DollarUtilizationRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor que recibe el diccionario con los datos para mostrar el reporte
        /// </summary>
        /// <param name="datos">Diccionario de datos</param>
        public DollarUtilizationRPT(Dictionary<string, object> datos)
            : this()
        {
            this.DataSource = datos["DataSource"];
            this.chartDollarUtilization.DataSource = this.DataSource;
            this.CreateAxisX();
            string rangoFechas = "DE " + datos["MesAnioInicio"].ToString().ToUpper() + " A " + datos["MesAnioFin"].ToString().ToUpper();
            this.xrTCRangoFechas.Text = rangoFechas;
            if (((BPMO.SDNI.Flota.Reportes.DA.DollarUtilizationDS)(datos["DataSource"])).Sucursales.Count == 1)
            {
                this.xrTableCell17.Text = "REPORTE DE DOLLAR UTILIZATION: " + ((BPMO.SDNI.Flota.Reportes.DA.DollarUtilizationDS)(datos["DataSource"])).Sucursales[0].Nombre;
            }
            else { this.xrTableCell17.Text = "REPORTE DE DOLLAR UTILIZATION"; }
        }

        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public DollarUtilizationRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualizar la imagen que representa el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DollarUtilizationDS).ConfiguracionModulo[0].URLLogoEmpresa;
            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Crea las coordenadas de Eje X con la primera letra del Mes
        /// </summary>
        public void CreateAxisX()
        {
            XYDiagram diagrama = (this.chartDollarUtilization.Diagram as XYDiagram);
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
        /// Evento que se genera antes de visualizar la gráfica por sucursal.
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void xrSRChartDetails_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport subReport = (sender as XRSubreport);
            DollarUtilizationElementoSucursalRPT subReportDUSuc = subReport.ReportSource as DollarUtilizationElementoSucursalRPT;

            int sucursalID = this.GetCurrentColumnValue<Int32>("SucursalID");
            subReportDUSuc.SetFilter(sucursalID);
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualizar el reporte
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void DollarUtilizationRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DollarUtilizationElementoSucursalRPT subReportDUSuc = this.xrSRChartDetails.ReportSource as DollarUtilizationElementoSucursalRPT;
            subReportDUSuc.BindToDataSource(this);
            subReportDUSuc.CreateAxisX();
        }
    }
}
