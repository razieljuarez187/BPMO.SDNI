//Satisface el CU020 – Reporte Contratos FSL Registrados

using DevExpress.XtraReports.UI;
using System;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
    /// <summary>
    /// Subreporte que visualiza los subtotales por sucursales
    /// </summary>
    public partial class ContratoFSLRegistradoSucursalTotalesRPT : XtraReport
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ContratoFSLRegistradoSucursalTotalesRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte qde un elemento de agrupamiento por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void contratoFSLRegistradoSucursalTotalesElementoRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var subReport = ((XRSubreport)sender);

            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = DataSource;            
            var sucursalID = GetCurrentColumnValue<int>("SucursalID");
            subReport.ReportSource.FilterString = String.Format("[SucursalID] = {0}", sucursalID);
        }
    }
}
