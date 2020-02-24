//Satisface el CU020 – Reporte Contratos FSL Registrados

using System;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
    /// <summary>
    /// Plantilla para generación de la sección de unidades por sucursal del Reporte de Contratos de FSL Registrados
    /// </summary>
    public partial class ContratoFSLRegistradoSucursalElementoRPT : XtraReport
    {
        /// <summary>
        /// Contador de unidades que estan como refrigerados
        /// </summary>
        private int totalRefrigerados;

        /// <summary>
        /// Contador de unidades que estan marcados como secos
        /// </summary>
        private int totalSecos;

        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        public ContratoFSLRegistradoSucursalElementoRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se reinicia el sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryReset(object sender, EventArgs e)
        {
            totalRefrigerados = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia de registro para el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryRowChanged(object sender, EventArgs e)
        {
            var refrigerado = GetCurrentColumnValue<bool>("EsRefrigerante");
            totalRefrigerados += (refrigerado ? 1 : 0);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            int modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => Convert.ToInt32(x["SucursalID"]) == sucursalID && Convert.ToBoolean(x["EsRefrigerante"]) && Convert.ToInt32(x["ModeloID"]) == modeloId);
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se reinicia el sumado de secos por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryReset(object sender, EventArgs e)
        {
            totalSecos = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia de registro para el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryRowChanged(object sender, EventArgs e)
        {
            var refrigerado = GetCurrentColumnValue<bool>("EsRefrigerante");
            totalSecos += (refrigerado ? 0 : 1);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            int modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => Convert.ToInt32(x["SucursalID"]) == sucursalID && !Convert.ToBoolean(x["EsRefrigerante"]) && Convert.ToInt32(x["ModeloID"]) == modeloId);
            e.Handled = true;
        }
    }
}
