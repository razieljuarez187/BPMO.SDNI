//Satisface al caso de uso CU021 - Reporte de Contratos de Mantenimiento Registrados
using System;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.RPT
{
    public partial class ContratoCMRegistradosFlotaTotalElementoRPT : XtraReport
    {
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        public ContratoCMRegistradosFlotaTotalElementoRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotalUnidadesRefrigeradas_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["BranchID"]) == sucursalID && Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;
        }
        
        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotalUnidadesSecas_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["BranchID"]) == sucursalID && !Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de todas las unidades
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotalUnidadesFlota_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["BranchID"]) == sucursalID);

            e.Handled = true;
        }

    }
}
