//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados

using System;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.SD.RPT
{
    /// <summary>
    /// Subreporte que visualiza los subtotales de una sucursal especifica
    /// </summary>
    public partial class ContratoSDRegistradoSucursalTotalesElementoRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ContratoSDRegistradoSucursalTotalesElementoRPT()
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
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable()
                            .Where(x => Convert.ToInt32(x["BranchID"]) == sucursalID)
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
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable()
                            .Where(x => Convert.ToInt32(x["BranchID"]) == sucursalID && Convert.ToBoolean(x["EsRefrigerante"]))
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
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable()
                            .Where(x => Convert.ToInt32(x["BranchID"]) == sucursalID && !Convert.ToBoolean(x["EsRefrigerante"]))
                            .Count();            

            e.Handled = true;
        }

    }
}
