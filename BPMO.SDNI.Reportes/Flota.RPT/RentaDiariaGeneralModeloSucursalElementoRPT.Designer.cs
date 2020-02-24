namespace BPMO.SDNI.Flota.RPT
{
    partial class RentaDiariaGeneralModeloSucursalElementoRPT
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtNombreModelo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDiasMes = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDiasXUnidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDiasObjetivo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDiasRenta = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtPorcentajeUtilizacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.sbrptTotalesMes = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrlTotalFlotaRentados = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTDiasMesTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTTotalDiasXUnidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTTotalDiasObjetivo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTTotalDiasRenta = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTTotalPorcentajeUtilizacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.Detail.HeightF = 25.08335F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ModeloID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable3.SizeF = new System.Drawing.SizeF(1074F, 25.08335F);
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtNombreModelo,
            this.xrtDiasMes,
            this.xrtDiasXUnidad,
            this.xrtDiasObjetivo,
            this.xrtDiasRenta,
            this.xrtPorcentajeUtilizacion});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrtNombreModelo
            // 
            this.xrtNombreModelo.Name = "xrtNombreModelo";
            this.xrtNombreModelo.Text = "xrtNombreModelo";
            this.xrtNombreModelo.Weight = 0.27932952369391584D;
            // 
            // xrtDiasMes
            // 
            this.xrtDiasMes.Name = "xrtDiasMes";
            this.xrtDiasMes.Weight = 2.1229051131775925D;
            // 
            // xrtDiasXUnidad
            // 
            this.xrtDiasXUnidad.Name = "xrtDiasXUnidad";
            this.xrtDiasXUnidad.Text = "xrtDiasXUnidad";
            this.xrtDiasXUnidad.Weight = 0.12223960833842526D;
            // 
            // xrtDiasObjetivo
            // 
            this.xrtDiasObjetivo.Name = "xrtDiasObjetivo";
            this.xrtDiasObjetivo.Text = "xrtDiasObjetivo";
            this.xrtDiasObjetivo.Weight = 0.15363128491620109D;
            // 
            // xrtDiasRenta
            // 
            this.xrtDiasRenta.Name = "xrtDiasRenta";
            this.xrtDiasRenta.Text = "xrtDiasRenta";
            this.xrtDiasRenta.Weight = 0.12402531288189596D;
            // 
            // xrtPorcentajeUtilizacion
            // 
            this.xrtPorcentajeUtilizacion.Name = "xrtPorcentajeUtilizacion";
            this.xrtPorcentajeUtilizacion.Text = "xrtPorcentajeUtilizacion";
            this.xrtPorcentajeUtilizacion.Weight = 0.19786915699196928D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 13F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageBreak1,
            this.sbrptTotalesMes,
            this.xrTable4});
            this.GroupFooter1.HeightF = 106.8267F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter1_BeforePrint);
            // 
            // sbrptTotalesMes
            // 
            this.sbrptTotalesMes.LocationFloat = new DevExpress.Utils.PointFloat(0F, 72.36833F);
            this.sbrptTotalesMes.Name = "sbrptTotalesMes";
            this.sbrptTotalesMes.ReportSource = new BPMO.SDNI.Flota.RPT.RentaDiariaGeneralSucursalModeloRPT();
            this.sbrptTotalesMes.SizeF = new System.Drawing.SizeF(1074F, 23F);
            this.sbrptTotalesMes.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbrptTotalesMes_BeforePrint);
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5,
            this.xrTableRow4});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1074F, 50.16F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Weight = 2.9999999999999996D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrlTotalFlotaRentados,
            this.xrTDiasMesTotal,
            this.xrTTotalDiasXUnidad,
            this.xrTTotalDiasObjetivo,
            this.xrTTotalDiasRenta,
            this.xrTTotalPorcentajeUtilizacion});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrlTotalFlotaRentados
            // 
            this.xrlTotalFlotaRentados.Name = "xrlTotalFlotaRentados";
            this.xrlTotalFlotaRentados.Text = "Flota Rentados";
            this.xrlTotalFlotaRentados.Weight = 0.27932963358329715D;
            // 
            // xrTDiasMesTotal
            // 
            this.xrTDiasMesTotal.Name = "xrTDiasMesTotal";
            this.xrTDiasMesTotal.Weight = 2.122905173777474D;
            // 
            // xrTTotalDiasXUnidad
            // 
            this.xrTTotalDiasXUnidad.Name = "xrTTotalDiasXUnidad";
            this.xrTTotalDiasXUnidad.Text = "xrTTotalDiasXUnidad";
            this.xrTTotalDiasXUnidad.Weight = 0.12223926735989873D;
            // 
            // xrTTotalDiasObjetivo
            // 
            this.xrTTotalDiasObjetivo.Name = "xrTTotalDiasObjetivo";
            this.xrTTotalDiasObjetivo.Text = "xrTTotalDiasObjetivo";
            this.xrTTotalDiasObjetivo.Weight = 0.15363145540546438D;
            // 
            // xrTTotalDiasRenta
            // 
            this.xrTTotalDiasRenta.Name = "xrTTotalDiasRenta";
            this.xrTTotalDiasRenta.Text = "xrTTotalDiasRenta";
            this.xrTTotalDiasRenta.Weight = 0.12402531288189594D;
            // 
            // xrTTotalPorcentajeUtilizacion
            // 
            this.xrTTotalPorcentajeUtilizacion.Name = "xrTTotalPorcentajeUtilizacion";
            this.xrTTotalPorcentajeUtilizacion.Text = "xrTTotalPorcentajeUtilizacion";
            this.xrTTotalPorcentajeUtilizacion.Weight = 0.19786915699196928D;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 104.8266F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // RentaDiariaGeneralModeloSucursalElementoRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupFooter1});
            this.DataMember = "SubTotalXModelo";
            this.DataSource = this.reporteRDSucursalDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(14, 12, 0, 13);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "12.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrtNombreModelo;
        private DevExpress.XtraReports.UI.XRTableCell xrtDiasMes;
        private DevExpress.XtraReports.UI.XRTableCell xrtDiasXUnidad;
        private DevExpress.XtraReports.UI.XRTableCell xrtDiasObjetivo;
        private DevExpress.XtraReports.UI.XRTableCell xrtDiasRenta;
        private DevExpress.XtraReports.UI.XRTableCell xrtPorcentajeUtilizacion;
        private Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrlTotalFlotaRentados;
        private DevExpress.XtraReports.UI.XRTableCell xrTDiasMesTotal;
        private DevExpress.XtraReports.UI.XRTableCell xrTTotalDiasXUnidad;
        private DevExpress.XtraReports.UI.XRTableCell xrTTotalDiasObjetivo;
        private DevExpress.XtraReports.UI.XRTableCell xrTTotalDiasRenta;
        private DevExpress.XtraReports.UI.XRTableCell xrTTotalPorcentajeUtilizacion;
        private DevExpress.XtraReports.UI.XRSubreport sbrptTotalesMes;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
    }
}
