using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.RPT {
    public partial class SubEquiposNumerosSerieRPT : DevExpress.XtraReports.UI.XtraReport {
        public SubEquiposNumerosSerieRPT() {
            InitializeComponent();
        }

        public SubEquiposNumerosSerieRPT(DataTable table) {
            InitializeComponent();
            this.DataSource = table;
            ImprimirReporte();
        }

        public void ImprimirReporte() {
            try {
                this.DataMember = "ListaSerie";
                this.xrFactura.DataBindings.Add("Text", this.DataSource, "SerieFolio");
                this.xrSerie.DataBindings.Add("Text", this.DataSource, "Serie");
                this.xrNumeroEconomico.DataBindings.Add("Text", this.DataSource, "NumeroEconomico");
            }
            catch (Exception e) {
                throw new Exception("SubEquiposNumerosSerieRPT.ImprimirReporte:Error al intentar generar el reporte." + e.Message);
            }
        }

    }
}
