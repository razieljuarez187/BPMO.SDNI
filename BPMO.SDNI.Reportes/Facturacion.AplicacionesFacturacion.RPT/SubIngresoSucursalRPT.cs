using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.RPT {
    public partial class SubIngresoSucursalRPT : DevExpress.XtraReports.UI.XtraReport {
        public SubIngresoSucursalRPT() {
            InitializeComponent();
        }

        public SubIngresoSucursalRPT(DataTable table) {
            InitializeComponent();
            this.DataSource = table;
            ImprimirReporte(table);
        }

        public void ImprimirReporte(DataTable Table) {
            try {
                if (Table.Rows.Count > 0) {
                    this.GroupHeader1.GroupFields.Add(new GroupField("SUCURSAL"));
                    this.xrSucursal.DataBindings.Add("Text", this.DataSource, "SUCURSAL");

                    this.xrIDCliente.DataBindings.Add("Text", this.DataSource, "cuenta_cliente_id");
                    this.xrCliente.DataBindings.Add("Text", this.DataSource, "NombreCliente");
                    this.xrTipoPago.DataBindings.Add("Text", this.DataSource, "nomCredito");
                    this.xrFFactura.DataBindings.Add("Text", this.DataSource, "FechaDocumento");
                    this.xrNFactura.DataBindings.Add("Text", this.DataSource, "ElFolio");
                    this.xrImporteDLS.DataBindings.Add("Text", this.DataSource, "RENTADLS", "{0: $#,0.00}");
                    this.xrTipoCambio.DataBindings.Add("Text", this.DataSource, "TasaCambiaria", "{0: $#,0.00}");
                    this.xrImporteMXN.DataBindings.Add("Text", this.DataSource, "RENTA", "{0: $#,0.00}");
                    this.xrIva.DataBindings.Add("Text", this.DataSource, "IVARENTA", "{0: $#,0.00}");
                    this.xrTotal.DataBindings.Add("Text", this.DataSource, "Total", "{0: $#,0.00}");
                    this.xrPromotor.DataBindings.Add("Text", this.DataSource, "promotor");
                    this.xrContrato.DataBindings.Add("Text", this.DataSource, "NumeroContrato");


                    this.xrSubTotal.DataBindings.Add("Text", DataSource, "RENTA");
                    this.xrSubTotal.Summary.Func = SummaryFunc.Sum;
                    this.xrSubTotal.Summary.Running = SummaryRunning.Group;
                    this.xrSubTotal.Summary.FormatString = "{0: $#,0.00}";
                    this.xrTotalIVA.DataBindings.Add("Text", DataSource, "IVARENTA");
                    this.xrTotalIVA.Summary.Func = SummaryFunc.Sum;
                    this.xrTotalIVA.Summary.Running = SummaryRunning.Group;
                    this.xrTotalIVA.Summary.FormatString = "{0: $#,0.00}";
                    this.xrTotalGlobal.DataBindings.Add("Text", DataSource, "Total");
                    this.xrTotalGlobal.Summary.Func = SummaryFunc.Sum;
                    this.xrTotalGlobal.Summary.Running = SummaryRunning.Group;
                    this.xrTotalGlobal.Summary.FormatString = "{0: $#,0.00}";

                    this.xrMensaje.Visible = false;
                }
                else {
                    this.xrMensaje.Text = "No existe información para mostrar.";
                    this.xrLabel3.Visible = false;
                    this.xrLabel6.Visible = false;
                    this.xrTotalIVA.Visible = false;
                    this.xrTable1.Visible = false;
                }
            }
            catch (Exception e) {
                throw new Exception("SubResumenIngresoRentaRPT.ImprimirReporte:Error al intentar generar el reporte." + e.Message);
            }
        }
    }
}
