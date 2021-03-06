﻿//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados

using System;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.SD.RPT
{
    /// <summary>
    /// Plantilla para generación de la sección de unidades por sucursal del reporte de Flota Activa de RD Registrados
    /// </summary>
    public partial class ContratoSDRegistradoSucursalElementoRPT : DevExpress.XtraReports.UI.XtraReport
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
        public ContratoSDRegistradoSucursalElementoRPT()
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
            int sucursalID = this.GetCurrentColumnValue<int>("BranchID");
            int modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            //e.Result = this.totalRefrigerados;
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["BranchID"]) == sucursalID && Convert.ToBoolean(x["EsRefrigerante"]) && Convert.ToInt32(x["ModeloID"]) == modeloId);
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
            bool refrigerado = this.GetCurrentColumnValue<bool>("EsRefrigerante");
            this.totalSecos += (refrigerado ? 0 : 1);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("BranchID");
            int modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            //e.Result = this.totalSecos;
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["BranchID"]) == sucursalID && !Convert.ToBoolean(x["EsRefrigerante"]) && Convert.ToInt32(x["ModeloID"]) == modeloId);
            e.Handled = true;
        }
    }
}
