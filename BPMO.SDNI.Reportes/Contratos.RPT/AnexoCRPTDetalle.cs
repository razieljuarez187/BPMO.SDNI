using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.RPT {
    public partial class AnexoCRPTDetalle : DevExpress.XtraReports.UI.XtraReport {
        public AnexoCRPTDetalle() {
            try {
                InitializeComponent();
                EnlazarControles();
            }
            catch {
                throw;
            }
        }

        /// <summary>
        /// Enlaza los controles del reporte con la información contenida en la fuente de datos
        /// </summary>
        private void EnlazarControles() {
                        
            this.lblNombre.DataBindings.Add("Text", DataSource, "Nombre");
            this.lblNumeroSerie2.DataBindings.Add("Text", DataSource, "NumeroSerie");
            
        }

        
    }
}
