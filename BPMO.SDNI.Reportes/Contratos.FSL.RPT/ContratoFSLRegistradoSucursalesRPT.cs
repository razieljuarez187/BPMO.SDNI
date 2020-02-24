//Satisface el CU020 – Reporte FSL Registrados

using System;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
    /// <summary>
    /// Plantilla de la sección de agrupamiento por sucursales del reporte de Flota Activa de FSL Registrados
    /// </summary>
    public partial class ContratoFSLRegistradoSucursalesRPT : XtraReport
    {
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        public ContratoFSLRegistradoSucursalesRPT()
        {
            InitializeComponent();
        }       

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte de un elemento de agrupamiento de equipos por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void contratoFSLRegistradoSucursalElementoRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var subReport = ((XRSubreport)sender);

            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = DataSource;
            var modeloID = GetCurrentColumnValue<int>("ModeloID");
            var sucursalID = GetCurrentColumnValue<int>("SucursalID");
            subReport.ReportSource.FilterString = String.Format("[ModeloID] = {0} And [SucursalID] = {1}", modeloID, sucursalID);
        }
    }
}
