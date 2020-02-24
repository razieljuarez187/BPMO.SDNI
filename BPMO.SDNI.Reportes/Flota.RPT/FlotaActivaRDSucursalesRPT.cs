//Satisface el CU019 - Reporte de Flota Activa de RD Registrados

using System;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Plantilla de la sección de agrupamiento por sucursales del reporte de Flota Activa de RD Registrados
    /// </summary>
    public partial class FlotaActivaRDSucursalesRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        public FlotaActivaRDSucursalesRPT()
        {
            this.InitializeComponent();
        }       

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte qde un elemento de agrupamiento de equipos por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void flotaActivaRDSucursalElementoRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport subReport = ((XRSubreport)sender);

            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = this.DataSource;
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            subReport.ReportSource.FilterString = String.Format("[ModeloID] = {0} And [SucursalID] = {1}", modeloID, sucursalID);
        }
    }
}
