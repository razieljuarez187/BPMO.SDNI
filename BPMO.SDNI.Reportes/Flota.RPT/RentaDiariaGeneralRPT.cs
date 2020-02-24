//Satisface el CU016 – Reporte de Renta Diaria General
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Reporte Principal de Renta Diaria General
    /// </summary>
    public partial class RentaDiariaGeneralRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor por default que recibe el diccionario con los datos para mostrar el reporte
        /// </summary>
        /// <param name="datos">Diccionario de Datos</param>
        public RentaDiariaGeneralRPT(Dictionary<string, object> datos): this()
        {
            this.DataSource = datos["DataSource"];
        }

        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public RentaDiariaGeneralRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta para llamar al Reporte por Sucursal
        /// </summary>
        private void ReporteSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteSucursal = sender as XRSubreport;
            if(reporteSucursal.ReportSource == null)
                return;
            reporteSucursal.ReportSource.DataSource = this.DataSource;

            var sucursalId = this.GetCurrentColumnValue<int>("SucursalID");
            reporteSucursal.ReportSource.FilterString = String.Format("[SucursalID] = {0}", sucursalId);
        }

        /// <summary>
        /// Evento ejecutado para imprimir el logo
        /// </summary>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as ReporteRDSucursalDS).ConfiguracionesSistema.First().UrlLogoEmpresa;

            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta al llamar al subreporte por Sucursal antes de imprimir la seccion de detalle
        /// </summary>
        private void dtlSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            RentaDiariaGeneralMesSucursalRPT dataSource = (this.ReporteSucursal.ReportSource = new RentaDiariaGeneralMesSucursalRPT())  as RentaDiariaGeneralMesSucursalRPT;
            dataSource.UsarSucursal = true;
        }

        /// <summary>
        /// Evento ejecutado al llamar al reporte sin Filtrar por Sucursal antes de imprimir la seccion del footer
        /// </summary>
        private void ReportFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            RentaDiariaGeneralMesSucursalRPT dataSource = (this.ReporteGeneral.ReportSource = new RentaDiariaGeneralMesSucursalRPT()) as RentaDiariaGeneralMesSucursalRPT;
            dataSource.UsarSucursal = false;
            dataSource.DataMember = "Meses";
        }

        /// <summary>
        /// Evento ejecutado antes de llamar al subreporte que no se agrupa por sucursal
        /// </summary>
        private void ReporteGeneral_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteSucursal = sender as XRSubreport;
            if(reporteSucursal.ReportSource == null)
                return;
            reporteSucursal.ReportSource.DataSource = this.DataSource;
        }
    }
}
