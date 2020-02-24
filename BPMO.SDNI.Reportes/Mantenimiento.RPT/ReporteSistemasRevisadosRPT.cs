//Satisface el CU066 - Reporte Sistemas Revisados
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Clase de Reporte de Sistemas Revisados
    /// </summary>
    public partial class ReporteSistemasRevisadosRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteSistemasRevisadosRPT()
        {
            InitializeComponent();
        }

         public ReporteSistemasRevisadosRPT(Dictionary<string, object> datos)
        {
            InitializeComponent();
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];
        }

         /// <summary>
         /// Evento que se ejecuta antes de visualiza la imagen que representa el logo de la empresa
         /// </summary>
         /// <param name="sender">Objeto que genero el evento</param>
         /// <param name="e">Argumentos relacionados con el evento</param>
         private void ImageLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
         {
             String url = (this.DataSource as SistemasRevisadosDS).ConfiguracionesSistema.AsEnumerable().Select(x => x.URLLogoEmpresa).FirstOrDefault();
             this.ImageLogo.ImageUrl = url;
         }

         /// <summary>
         /// Evento que se ejecuta antes de visualizar el subreporte de la grafica de sistemas revisados
         /// </summary>
         /// <param name="sender">Objeto que genero el evento</param>
         /// <param name="e">Argumentos relacionados con el evento</param>   
         private void Subreporte_Grafica_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
         {
             XRSubreport subreporte = sender as XRSubreport;
             if (subreporte.ReportSource == null)
                 return;
             subreporte.ReportSource.DataSource = this.DataSource;
             var ordenServicioID = this.GetCurrentColumnValue<Int32>("OrdenServicioID");

             subreporte.ReportSource.FilterString = String.Format("[OrdenServicioID] = {0}", ordenServicioID).ToString();
         }
    }
}
