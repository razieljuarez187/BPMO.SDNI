using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Xml;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ContratoPagareRPT : DevExpress.XtraReports.UI.XtraReport {
        
        public ContratoPagareRPT(Dictionary<string, object> origen) {
            InitializeComponent();
            EnlazarControles(origen);          
        }

        private void EnlazarControles(Dictionary<string, object> origen) {
            try {
                string nameRpt = string.Empty;
                DataSet dsDatos = origen.ContainsKey("datosReporte") ? (DataSet)origen["datosReporte"] : null;
                this.DataSource = dsDatos;

                this.xrMonto.DataBindings.Add("Html", this.DataSource, "SeccionMonto");
                this.xrSeccion_1.DataBindings.Add("Html", this.DataSource, "Seccion1");
                this.xrSeccion_2.DataBindings.Add("Html", this.DataSource, "Seccion2");
                this.xrSeccion_3.DataBindings.Add("Html", this.DataSource, "Seccion3");
                this.xrDomicilio.DataBindings.Add("Html", this.DataSource, "SeccionDomicilio");
                this.xrFecha.DataBindings.Add("Html", this.DataSource, "SeccionFecha");
                this.xrCliente.DataBindings.Add("Html", this.DataSource, "SeccionCliente");
                this.xrAval.DataBindings.Add("Html", this.DataSource, "SeccionAval");
            }
            catch (Exception ex) {
                throw new Exception(".ImpirmirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
    }
}
