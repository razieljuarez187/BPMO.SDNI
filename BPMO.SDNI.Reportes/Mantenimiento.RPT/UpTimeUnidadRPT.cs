//Satisface al CU069 - Reporte Up Time
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using BPMO.SDNI.Flota.Reportes.DA;
using System.Linq;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class UpTimeUnidadRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public UpTimeUnidadRPT()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

    }
}
