//Satisface al caso de uso CU071 - reporte de rendimiento de unidad
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// reporte de Rendimiento por unidad
    /// </summary>
    public partial class ReporteRendimientoUnidadRPT : XtraReport
    {
        #region Constructor
        public ReporteRendimientoUnidadRPT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        /// <param name="datosReporte">Diccionario de datos con los datos para el reporte</param>
        //public ReporteRendimientoUnidadRPT(Dictionary<String, Object> datosReporte)
        //{
        //    InitializeComponent();
        //    this.DataSource = datosReporte["DataSource"];
        //}

        #endregion
        #region Eventos
        /// <summary>
        /// Envento ejecutado antes de la impresion de cada detail
        /// </summary>
        private void Detail_BeforePrint(object sender, PrintEventArgs e)
        {
        }
        /// <summary>
        /// Evento Ejecutado antes de la impresion del subreporte de datos de rendimiento
        /// </summary>
        private void xrSubreport1_BeforePrint(object sender, PrintEventArgs e)
        {
            var unidadId = this.GetCurrentColumnValue<int>("UnidadID");

            XRSubreport reporteMeses = sender as XRSubreport;
            if (reporteMeses.ReportSource == null)
                return;

            reporteMeses.ReportSource.DataSource = this.DataSource;
            reporteMeses.ReportSource.FilterString = String.Format("[UnidadID] = {0}", unidadId);
        }
        /// <summary>
        /// Envento ejecutado antes de la impresion del logo
        /// </summary>
        private void pbLogo_BeforePrint(object sender, PrintEventArgs e)
        {
            //String url = (this.DataSource as ReporteRendimientoUnidadDS).ConfiguracionesSistema.AsEnumerable().Select(x => x.UrlLogoEmpresa).FirstOrDefault();

            //this.pbLogo.ImageUrl = url;
        }
        /// <summary>
        /// Evento que se ejecuta antes de la impresion de la grafica de rendimiento
        /// </summary>
        private void xrSubreport2_BeforePrint(object sender, PrintEventArgs e)
        {
            var unidadId = this.GetCurrentColumnValue<int>("UnidadID");

            XRSubreport reporteMeses = sender as XRSubreport;
            ReporteRendimientoUnidadGraficaRPT subGlobalReport = reporteMeses.ReportSource as ReporteRendimientoUnidadGraficaRPT;
            subGlobalReport.SetFilter(unidadId);
        }
        /// <summary>
        /// Evento ejecutado antes de la impresion del subreporte 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReporteRendimientoUnidadRPT_BeforePrint(object sender, PrintEventArgs e)
        {
            ReporteRendimientoUnidadGraficaRPT subGlobalReport = this.xrSubreport2.ReportSource as ReporteRendimientoUnidadGraficaRPT;
            subGlobalReport.BindToDataSource(this);
            subGlobalReport.CreateAxisX();
        }
        #endregion
    }
}