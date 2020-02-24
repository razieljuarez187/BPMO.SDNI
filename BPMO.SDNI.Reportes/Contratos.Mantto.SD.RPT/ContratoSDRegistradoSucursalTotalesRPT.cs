//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados

using System;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.SD.RPT
{
    /// <summary>
    /// Subreporte que visualiza los subtotales por sucursales
    /// </summary>
    public partial class ContratoSDRegistradoSucursalTotalesRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ContratoSDRegistradoSucursalTotalesRPT()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte qde un elemento de agrupamiento por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void flotaActivaRDSucursalTotalesElementoRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport subReport = ((XRSubreport)sender);

            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = this.DataSource;            
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            subReport.ReportSource.FilterString = String.Format("[SucursalID] = {0}", sucursalID);
        }
    }
}
