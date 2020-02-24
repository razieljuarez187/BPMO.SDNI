//Satisface el CU019 - Reporte de Flota Activa de RD Registrados

using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;
using System.Linq;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Subreporte que visualiza los subtotales de una sucursal especifica
    /// </summary>
    public partial class FlotaActivaSucursalTotalesElementoRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FlotaActivaSucursalTotalesElementoRPT()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalUnidadesSucursal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.SucursalID == sucursalID)
                            .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigeradosSucursal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.SucursalID == sucursalID && x.EsRefrigerante)
                            .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecosSucursal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.SucursalID == sucursalID && !x.EsRefrigerante)
                            .Count();

            e.Handled = true;
        }

    }
}
