using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT{
    /// <summary>
    /// Plantilla para generación de reporte de Flota Activa
    /// </summary>
    public partial class FlotaActivaRPT : DevExpress.XtraReports.UI.XtraReport { 
        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        /// <param name="datos">Datos para llenado de reporte</param>
        public FlotaActivaRPT(Dictionary<string, object> datos) : this(){            
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];            
        }

        /// <summary>
        /// Contructor de la plantilla
        /// </summary>
        /// <param name="datos">Datos para llenado de reporte</param>
        public FlotaActivaRPT(){
            this.InitializeComponent();
        }    

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigerados_SummaryGetResult(object sender, SummaryGetResultEventArgs e){
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");          
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.ModeloID == modeloID &&
                                        x.EsRefrigerante)
                            .Count();

            e.Handled = true;
        }      

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecos_SummaryGetResult(object sender, SummaryGetResultEventArgs e){
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.ModeloID == modeloID &&
                                        !x.EsRefrigerante)
                            .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalModelo_SummaryGetResult(object sender, SummaryGetResultEventArgs e){
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.ModeloID == modeloID)
                            .Count();

            e.Handled = true;
        }
        
        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra el agrupado por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void flotaActivaSucursalesRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e){
            if (this.flotaActivaSucursalesRPT.ReportSource == null)
                return;

            this.flotaActivaSucursalesRPT.ReportSource.DataSource = this.DataSource;
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            this.flotaActivaSucursalesRPT.ReportSource.FilterString = String.Format("[ModeloID] = {0}", modeloID);            
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra los totales por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void flotaActivaSucursalTotalesRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e){
            if (this.flotaActivaSucursalTotalesRPT.ReportSource != null)
                this.flotaActivaSucursalTotalesRPT.ReportSource.DataSource = this.DataSource;            
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado total general
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalGeneral_SummaryGetResult(object sender, SummaryGetResultEventArgs e){
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD.Count();
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de refrigerados general
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalRefrigeradosGeneral_SummaryGetResult(object sender, SummaryGetResultEventArgs e){
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => x.EsRefrigerante)
                            .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el calculo del sumado de secos general
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void tcTotalSecosGeneral_SummaryGetResult(object sender, SummaryGetResultEventArgs e){
            e.Result = (this.DataSource as FlotaActivaRDDS).FlotaActivaRD
                            .Where(x => !x.EsRefrigerante)
                            .Count();

            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar la imagen de logo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e){
            String url = (this.DataSource as FlotaActivaRDDS).ConfiguracionModulo
                                                    .Select(x => x.URLLogoEmpresa)
                                                    .FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }               
    }
}
