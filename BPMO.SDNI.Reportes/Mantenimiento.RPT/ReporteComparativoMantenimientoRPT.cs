// Satisface al CU075 - Reporte Comparativo Mantenimiento
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using System.Globalization;

namespace BPMO.SDNI.Mantenimiento.RPT {

    public partial class ReporteComparativoMantenimientoRPT : DevExpress.XtraReports.UI.XtraReport {

        public ReporteComparativoMantenimientoRPT() {
            InitializeComponent();
        }

        public ReporteComparativoMantenimientoRPT(Dictionary<string, object> datos) {
            InitializeComponent();
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];
        }

        private void ImageLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            String url = (this.DataSource as ReporteComparativoMantenimientoDS).ConfiguracionSistema.AsEnumerable().Select(x => x.URLLogoEmpresa).FirstOrDefault();
            this.ImagenLogo.ImageUrl = url;
        }

        private void rowDetalle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            int total = GetCurrentColumnValue<int>("Total");
            int totalPreventivos = GetCurrentColumnValue<int>("TotalPreventivos");
            int totalCorrectivos = GetCurrentColumnValue<int>("TotalCorrectivos");
            CalcularPorcentaje(totalPreventivos, total, dTotalPreventivo);
            CalcularPorcentaje(totalCorrectivos, total, dTotalCorrectivo);
        }

        private void fArea_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            bool deArea = true;
            CalcularFooterPorcentajeTotal(deArea, fTotalAreaPreventivos, fTotalAreaCorrectivos);
        }

        private void fSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            bool deArea = false;
            CalcularFooterPorcentajeTotal(deArea, fTotalSucursalPreventivos, fTotalSucursalCorrectivos);
        }

        private void fTotalGeneral_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            int totalMantenimientos = 0;
            int totalMantenimientoPre = 0;
            int totalMantenimientoCo = 0;
            ReporteComparativoMantenimientoDS report = this.DataSource as ReporteComparativoMantenimientoDS;
            foreach (ReporteComparativoMantenimientoDS.MantenimientoRow mantenimiento in report.Mantenimiento) {
                totalMantenimientos += mantenimiento.Total;
                if (mantenimiento.Total > 0) {
                    totalMantenimientoPre += mantenimiento.TotalPreventivos;
                    totalMantenimientoCo += mantenimiento.TotalCorrectivos;
                }
            }
            CalcularPorcentaje(totalMantenimientoPre, totalMantenimientos, fTotalGeneralPre);
            CalcularPorcentaje(totalMantenimientoCo, totalMantenimientos, fTotalGeneralCo);
        }

        private void CalcularFooterPorcentajeTotal(bool deArea, XRTableCell celdaPreventivos, XRTableCell celdaCorrectivos) {
            int sucursalId = this.GetCurrentColumnValue<int>("SucusalId");
            int mesId = this.GetCurrentColumnValue<int>("MesId");
            int anio = this.GetCurrentColumnValue<int>("Anio");
            ReporteComparativoMantenimientoDS report = this.DataSource as ReporteComparativoMantenimientoDS;
            List<ReporteComparativoMantenimientoDS.MantenimientoRow> mantenimientos = report.Mantenimiento.Where(x => x.SucusalId == sucursalId && x.Anio == anio && x.MesId == mesId).ToList();
            if (deArea) {
                int areaId = this.GetCurrentColumnValue<int>("AreaId");
                mantenimientos = mantenimientos.Where(x => x.AreaId == areaId).ToList();
            }
            int totalMantenimientos = 0;
            int totalMantenimientoPre = 0;
            int totalMantenimientoCo = 0;
            foreach (ReporteComparativoMantenimientoDS.MantenimientoRow mantenimiento in mantenimientos) {
                totalMantenimientos += mantenimiento.Total;
                if (mantenimiento.Total > 0) {
                    totalMantenimientoPre += mantenimiento.TotalPreventivos;
                    totalMantenimientoCo += mantenimiento.TotalCorrectivos;
                }
            }
            CalcularPorcentaje(totalMantenimientoPre, totalMantenimientos, celdaPreventivos);
            CalcularPorcentaje(totalMantenimientoCo, totalMantenimientos, celdaCorrectivos);
        }

        private void CalcularPorcentaje(int parametro, int totalGeneral, XRTableCell celda) {
            Decimal total = Decimal.Parse(parametro.ToString());
            Decimal porcentaje = total != 0 && totalGeneral != 0 ? (total / totalGeneral) : 0M;
            porcentaje = Decimal.Round(porcentaje, 2);
            celda.Text = ConvertToPorcentaje(porcentaje);
        }

        private String ConvertToPorcentaje(Decimal porcentaje){
            NumberFormatInfo numberFormat = new CultureInfo("es-ES", false).NumberFormat;
            numberFormat.PercentDecimalDigits = 0;
            return porcentaje.ToString("P", numberFormat);
        }

    }
}
