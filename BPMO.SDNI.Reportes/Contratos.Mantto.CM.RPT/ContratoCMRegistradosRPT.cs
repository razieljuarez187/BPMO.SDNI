//Satisface al caso de uso CU021 - Reporte de Contratos de Mantenimiento Registrados

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.RPT
{
    /// <summary>
    /// Plantilla para generación de reporte de Contratos de Mantenimiento Registrados
    /// </summary>
    public partial class ContratoCMRegistradosRPT : XtraReport
    { 
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        /// <param name="datos">Datos para llenado de reporte</param>
        public ContratoCMRegistradosRPT(Dictionary<string, object> datos)
            : this()
        {            
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];            
        }

        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        /// <param name="datos">Datos para llenado de reporte</param>
        public ContratoCMRegistradosRPT()
        {
            this.InitializeComponent();
        }

        #region Conteo Totales
        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrRefrigerados_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["ModeloID"]) == modeloID && Convert.ToBoolean(x["EsRefrigerante"]) == true);
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["ModeloID"]) == modeloID && Convert.ToBoolean(x["EsRefrigerante"])  == false);

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToInt32(x["ModeloID"]) == modeloID);

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotalRefrigerados_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => Convert.ToBoolean(x["EsRefrigerante"]));
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotalSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable().Count(x => !Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;}

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrTotalFlota_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = (this.DataSource as DataSet).Tables["ConsultarContratosRegistrados"].AsEnumerable()
                            .Count();

            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra el agrupado por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void sbrptDetailsSucursales_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.sbrptDetailsSucursales.ReportSource == null)
                return;

            this.sbrptDetailsSucursales.ReportSource.DataSource = this.DataSource;
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            this.sbrptDetailsSucursales.ReportSource.FilterString = String.Format("[ModeloID] = {0}", modeloID);            
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra el total de flota por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void sbrptFlotaSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if(this.sbrptFlotaSucursal.ReportSource != null) 
                sbrptFlotaSucursal.ReportSource.DataSource = this.DataSource as DataSet;
        }

        /// <summary>
        /// Evento que se ejecuta para colocar la url de la imagen del logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRPictureBox) sender).ImageUrl = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].Rows[0]["UrlLogoEmpresa"].ToString();}        
    }
}
