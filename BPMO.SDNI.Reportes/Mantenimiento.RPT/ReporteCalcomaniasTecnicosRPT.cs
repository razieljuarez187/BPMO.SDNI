//Satisface al CU017 - Imprimir Calcomanía Mantenimientos
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class ReporteCalcomaniasTecnicosRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteCalcomaniasTecnicosRPT()
        {
            InitializeComponent();
            
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //var tecnico = this.GetCurrentColumnValue<String>("Tecnico");

            //if (!xrLabel1.Text.Contains(tecnico))
            //{
            //    xrLabel1.Text = tecnico;
            //}

            //List<String> tecnicos = new List<String>();
            //if (!tecnicos.Contains(tecnico))
            //{
            //    tecnicos.Add(tecnico);
            //}
            //foreach (var item in tecnicos)
            //{
            //    xrLabel1.Text.con = item;
            //}
        }

    }
}
