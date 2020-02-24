//Satisface el CU028 – Reporte Días de Renta

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Plantilla de Reporte Porcentaje Utilización de Renta Diaria
    /// </summary>
    public partial class DiasRentaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Contructor por default
        /// </summary>
        public DiasRentaRPT()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Constructor por default que recibe el diccionario con los datos para mostrar el reporte
        /// </summary>
        /// <param name="datos">Diccionario de Datos</param>
        public DiasRentaRPT(Dictionary<string, object> datos)
            : this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];

            if (((BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS)(datos["DataSource"])).Sucursales.Count == 1)
            {
                this.xrTableCell5.Text = "REPORTE DE DÍAS DE RENTA: " + ((BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS)(datos["DataSource"])).Sucursales[0].Nombre;
            }
            else { this.xrTableCell5.Text = "REPORTE DE DÍAS DE RENTA"; }
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza la imagen que representa el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as ReporteRDSucursalDS).ConfiguracionesSistema.AsEnumerable()
                                                    .Select(x => x.UrlLogoEmpresa)
                                                    .FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualize el reporte
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void PorcentajeUtilizacionRDRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DiasRentaElementoGraficaRPT subReport = this.xsbrptChartDetails.ReportSource as DiasRentaElementoGraficaRPT;
            subReport.BindToDataSource(this);
            subReport.CreateAxisX();

            DiasRentaGlobalGraficaRPT subGlobalReport = this.xsbrptChartGlobal.ReportSource as DiasRentaGlobalGraficaRPT;
            subGlobalReport.BindToDataSource(this);
            subGlobalReport.CreateAxisX();
        }               

        /// <summary>
        /// Evento que se ejecutará antes de visualizar la gráfica por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void xsbrptChartDetails_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport report = (sender as XRSubreport);
            DiasRentaElementoGraficaRPT subReport = report.ReportSource as DiasRentaElementoGraficaRPT;
           
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            subReport.SetFilter(sucursalID);           
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza la gráfica global
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void xsbrptChartGlobal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.xsbrptChartGlobal.Visible = this.PrintingSystem.Document.PageCount == 0;
            this.pageBreakGlobal.Visible = this.xsbrptChartGlobal.Visible;
        }
    }
}
