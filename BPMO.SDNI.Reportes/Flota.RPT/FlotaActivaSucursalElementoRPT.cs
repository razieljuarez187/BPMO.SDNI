//Satisface el CU019 - Reporte de Flota Activa de RD Registrados

using System;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Plantilla para generación de la sección de unidades por sucursal del reporte de Flota Activa de RD Registrados
    /// </summary>
    public partial class FlotaActivaSucursalElementoRPT : DevExpress.XtraReports.UI.XtraReport
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
        public FlotaActivaSucursalElementoRPT()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se reinicia el sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryReset(object sender, EventArgs e)
        {
            this.totalRefrigerados = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia de registro para el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryRowChanged(object sender, EventArgs e)
        {
            bool refrigerado = this.GetCurrentColumnValue<bool>("EsRefrigerante");
            this.totalRefrigerados += (refrigerado ? 1 : 0);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //e.Result = this.totalRefrigerados;
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            int modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["FlotaActivaRD"].AsEnumerable().Count(x => Convert.ToInt32(x["SucursalID"]) == sucursalID && Convert.ToBoolean(x["EsRefrigerante"]) && Convert.ToInt32(x["ModeloID"]) == modeloId);
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se reinicia el sumado de secos por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryReset(object sender, EventArgs e)
        {
            this.totalSecos = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia de registro para el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryRowChanged(object sender, EventArgs e)
        {
            bool seco = !this.GetCurrentColumnValue<bool>("EsRefrigerante");
            this.totalSecos += (seco ? 1 : 0);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //e.Result = this.totalRefrigerados;
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            int modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["FlotaActivaRD"].AsEnumerable().Count(x => Convert.ToInt32(x["SucursalID"]) == sucursalID && !Convert.ToBoolean(x["EsRefrigerante"]) && Convert.ToInt32(x["ModeloID"]) == modeloId);
            e.Handled = true;
        }
    }
}
