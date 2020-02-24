//Satisface al CU068 - Reporte de Mantenimiento Realizado Contra Programado

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Reporte Mantenimiento Realizado Contra Programado
    /// </summary>
    public partial class ReporteMantenimientoRealizadoContraProgramadoRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteMantenimientoRealizadoContraProgramadoRPT()
        {
            InitializeComponent();
        }

        public ReporteMantenimientoRealizadoContraProgramadoRPT(Dictionary<string, object> datos): this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");
            this.DataSource = datos["DataSource"];
        }


        private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                    .Select(x => (String)x["URLLogoEmpresa"])
                                                    .FirstOrDefault();
            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte que muestra el agrupado por sucursales
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void mantenimientoRealizadoContraProgramadoTotalesRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (this.mantenimientoRealizadoContraProgramadoTotalesRPT.ReportSource != null)
               this.mantenimientoRealizadoContraProgramadoTotalesRPT.ReportSource.DataSource = this.DataSource;            
        }

        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                   .Select(x => (String)x["URLLogoEmpresa"])
                                                   .FirstOrDefault();
            this.pbLogo.ImageUrl = url;
        }

        private void ReporteMantenimientoRealizadoContraProgramadoRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
         
        }
    }
}
