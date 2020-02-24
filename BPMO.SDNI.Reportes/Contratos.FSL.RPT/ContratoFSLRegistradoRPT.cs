//Satisface el CU020 – Reporte Contratos FSL Registrados

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
    /// <summary>
    /// Plantilla para generación de reporte de Flota Activa Contratos FSL Registrados
    /// </summary>
    public partial class ContratoFSLRegistradoRPT : XtraReport
    { 
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        /// <param name="datos">Datos para llenado de reporte</param>
        public ContratoFSLRegistradoRPT(Dictionary<string, object> datos)
            : this()
        {            
            if (!datos.ContainsKey("DataSource"))
                throw new NullReferenceException("No se proporcionó el conjunto de datos del reporte.");

            DataSource = datos["DataSource"];            
        }

        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        public ContratoFSLRegistradoRPT()
        {
            InitializeComponent();
        }    

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ContratoFSLRegistrado"].AsEnumerable()
                            .Where(x => Convert.ToInt32(x["ModeloID"]) == modeloID &&
                                        Convert.ToBoolean(x["EsRefrigerante"]))
                            .Count();

            e.Handled = true;
        }      

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as DataSet).Tables["ContratoFSLRegistrado"].AsEnumerable()
                            .Where(x => Convert.ToInt32(x["ModeloID"]) == modeloID &&
                                        !Convert.ToBoolean(x["EsRefrigerante"]))
                            .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalModelo_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            int modeloID = GetCurrentColumnValue<int>("ModeloID");
            DataSet dataSet = DataSource as DataSet;
            if (dataSet != null)
                e.Result = dataSet.Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => Convert.ToInt32(x["ModeloID"]) == modeloID);

            e.Handled = true;
        }
                
        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra el agrupado por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void contratoFSLRegistradoSucursalesRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.contratoFSLRegistradoSucursalesRPT.ReportSource == null)
                return;

            this.contratoFSLRegistradoSucursalesRPT.ReportSource.DataSource = this.DataSource;
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");            
            this.contratoFSLRegistradoSucursalesRPT.ReportSource.FilterString = String.Format("[ModeloID] = {0}", modeloID);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra los totales por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void contratoFSLRegistradoSucursalTotalesRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.contratoFSLRegistradoSucursalTotalesRPT.ReportSource != null)
                this.contratoFSLRegistradoSucursalTotalesRPT.ReportSource.DataSource = this.DataSource;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado total general
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalGeneral_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            DataSet dataSet = DataSource as DataSet;
            if (dataSet != null)
                e.Result = dataSet.Tables["ContratoFSLRegistrado"].AsEnumerable()                  
                    .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados general
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigeradosGeneral_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            DataSet dataSet = DataSource as DataSet;
            if (dataSet != null)
                e.Result = dataSet.Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos general
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecosGeneral_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            var dataSet = DataSource as DataSet;
            if (dataSet != null)
                e.Result = dataSet.Tables["ContratoFSLRegistrado"].AsEnumerable().Count(x => !Convert.ToBoolean(x["EsRefrigerante"]));

            e.Handled = true;
        }
        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar la cja de imagen que muestra el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void pbxLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var ds = DataSource as DataSet;
            if (ds != null)
                pbxLogo.ImageUrl = ds.Tables["ConfiguracionUnidadOperativa"].Rows[0]["URLLogoEmpresa"].ToString();
        }
    }
}
