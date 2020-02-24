// Satisface al CU059 - Reporte Mantenimiento Realizado Unidad

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Reporte de Mantenimiento Realizado Unidad
    /// </summary>
    public partial class ReporteMantenimientoRealizadoUnidadRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propiedades
        public string TareasPendientes { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ReporteMantenimientoRealizadoUnidadRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor sobrecargado con parametros
        /// </summary>
        /// <param name="parameters"></param>
        public ReporteMantenimientoRealizadoUnidadRPT(Dictionary<string, object> datos)
            : this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");
            if (!datos.ContainsKey("SistemasRevisadosDS"))
                throw new ArgumentNullException("datos.SistemasRevisadosDS");
            if (!datos.ContainsKey("RendimientoUnidadDS"))
                throw new ArgumentNullException("datos.RendimientoUnidadDS");

            this.DataSource = datos["DataSource"];
            this.srServiciosRealizadosGrafica.ReportSource.DataSource = datos["SistemasRevisadosDS"] as SistemasRevisadosDS;
            this.srReporteRendimientoCamion.ReportSource.DataSource = datos["RendimientoUnidadDS"] as ReporteRendimientoUnidadDS;
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar el logo de la empresa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                    .Select(x => (String)x["URLLogoEmpresa"])
                                                    .FirstOrDefault();
            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar la imagen de combustible de entrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbCombustibleEntrada_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["MantenimientoUnidad"].AsEnumerable()
                                                    .Select(x => (String)x["UrlCombustibleEntrada"])
                                                    .FirstOrDefault();
            this.pbCombustibleEntrada.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar la imagen de combustible de salida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbCombustibleSalida_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["MantenimientoUnidad"].AsEnumerable()
                                                    .Select(x => (String)x["UrlCombustibleSalida"])
                                                    .FirstOrDefault();
            this.pbCombustibleSalida.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar la grafica de servicios realizados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void srServiciosRealizadosGrafica_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SistemasRevisadosGraficaRPT subGlobalReport = this.srServiciosRealizadosGrafica.ReportSource as SistemasRevisadosGraficaRPT;
            subGlobalReport.BindToDataSource();
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar la grafica de rendimiento de unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void srReporteRendimientoCamion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var unidadId = this.GetCurrentColumnValue<int>("UnidadID");

            XRSubreport reporteMeses = sender as XRSubreport;
            ReporteRendimientoUnidadGraficaRPT subGlobalReport = reporteMeses.ReportSource as ReporteRendimientoUnidadGraficaRPT;
            subGlobalReport.SetFilter(unidadId);
            //ReporteRendimientoUnidadGraficaRPT subGlobalReport = this.srReporteRendimientoCamion.ReportSource as ReporteRendimientoUnidadGraficaRPT;
            //subGlobalReport.BindToDataSource();
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar el detalle de codigos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void srReporteMantenimientoRealizadoUnidadCodigos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.srReporteMantenimientoRealizadoUnidadCodigos.ReportSource.DataSource = this.DataSource;
        }

        /// <summary>
        /// Evento que se ejecuta antes de pintar los servicios realizados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void srReporteMantenimientoRealizadoUnidadServiciosRealizadosRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.srReporteMantenimientoRealizadoUnidadServiciosRealizadosRPT.ReportSource.DataSource = this.DataSource;
        }

        private void xrTableCell25_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void ReporteMantenimientoRealizadoUnidadRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ReporteRendimientoUnidadGraficaRPT subGlobalReport = this.srReporteRendimientoCamion.ReportSource as ReporteRendimientoUnidadGraficaRPT;
            ReporteRendimientoUnidadDS data = subGlobalReport.DataSource as ReporteRendimientoUnidadDS;
            subGlobalReport.BindToDatasource(data);
            subGlobalReport.CreateAxisX();
        }

        private void GrupoPieTareasPendientes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            if (!string.IsNullOrWhiteSpace(this.TareasPendientes)) {
                this.GrupoPieTareasPendientes.Visible = true;
                this.tdTareasPendientes.Text = this.TareasPendientes;
            }
        }

    }
}
