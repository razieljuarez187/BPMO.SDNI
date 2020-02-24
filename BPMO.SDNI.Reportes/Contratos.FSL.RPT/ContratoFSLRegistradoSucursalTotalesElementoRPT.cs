//Satisface el CU020 – Reporte Contratos FSL Registrados

using System;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
    /// <summary>
    /// Subreporte que visualiza los subtotales de una sucursal especifica
    /// </summary>
    public partial class ContratoFSLRegistradoSucursalTotalesElementoRPT : XtraReport
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ContratoFSLRegistradoSucursalTotalesElementoRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalUnidadesSucursal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as DataSet).Tables["ContratoFSLRegistrado"].AsEnumerable()
                            .Where(x => Convert.ToInt32(x["SucursalID"]) == sucursalID)
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
            var sucursalID = GetCurrentColumnValue<int>("SucursalID");
            var dataSet = DataSource as DataSet;
            if (dataSet != null)
                e.Result = dataSet.Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => Convert.ToInt32(x["SucursalID"]) == sucursalID && Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecosSucursal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            var sucursalID = GetCurrentColumnValue<int>("SucursalID");
            var dataSet = DataSource as DataSet;
            if (dataSet != null)
                e.Result = dataSet.Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => Convert.ToInt32(x["SucursalID"]) == sucursalID && !Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;
        }

    }
}
