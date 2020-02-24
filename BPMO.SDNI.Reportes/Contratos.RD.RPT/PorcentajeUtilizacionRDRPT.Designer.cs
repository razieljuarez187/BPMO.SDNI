namespace BPMO.SDNI.Contratos.RD.RPT
{
    partial class PorcentajeUtilizacionRDRPT
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
            if (disposing && (components != null))
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
            this.xsbrptChartDetails = new DevExpress.XtraReports.UI.XRSubreport();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.TituloSucursal = new DevExpress.XtraReports.UI.CalculatedField();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.pageBreakGlobal = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xsbrptChartGlobal = new DevExpress.XtraReports.UI.XRSubreport();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.TituloGrafica = new DevExpress.XtraReports.Parameters.Parameter();
            this.TituloGraficaGlobal = new DevExpress.XtraReports.Parameters.Parameter();
            this.GrupoVacio = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xsbrptChartDetails});
            this.Detail.HeightF = 32.29167F;
            this.Detail.KeepTogether = true;
            this.Detail.MultiColumn.ColumnCount = 2;
            this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SucursalID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Nombre", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xsbrptChartDetails
            // 
            this.xsbrptChartDetails.Id = 0;
            this.xsbrptChartDetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5F);
            this.xsbrptChartDetails.Name = "xsbrptChartDetails";
            this.xsbrptChartDetails.ReportSource = new BPMO.SDNI.Contratos.RD.RPT.PorcentajeUtilizacionRDElementoGraficaRPT();
            this.xsbrptChartDetails.SizeF = new System.Drawing.SizeF(521F, 23F);
            this.xsbrptChartDetails.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xsbrptChartDetails_BeforePrint);
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.BottomMargin.HeightF = 28F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TituloSucursal
            // 
            this.TituloSucursal.DataMember = "ReporteRDSucursal";
            this.TituloSucursal.Expression = "\'Time Utilization \' + [SucursalNombre]";
            this.TituloSucursal.Name = "TituloSucursal";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1,
            this.tblTituloEtiquetas,
            this.pbLogo});
            this.PageHeader.HeightF = 109.3749F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(912.7083F, 43.75F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(912.7083F, 66.75F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(142.2917F, 23F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // tblTituloEtiquetas
            // 
            this.tblTituloEtiquetas.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(170.2083F, 43.75F);
            this.tblTituloEtiquetas.Name = "tblTituloEtiquetas";
            this.tblTituloEtiquetas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.tblTituloEtiquetas.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2,
            this.xrTableRow3});
            this.tblTituloEtiquetas.SizeF = new System.Drawing.SizeF(598.7084F, 56.25F);
            this.tblTituloEtiquetas.StylePriority.UseFont = false;
            this.tblTituloEtiquetas.StylePriority.UsePadding = false;
            this.tblTituloEtiquetas.StylePriority.UseTextAlignment = false;
            this.tblTituloEtiquetas.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ConfiguracionesSistema.NombreCliente")});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Weight = 1D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Weight = 1D;
            // 
            // pbLogo
            // 
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // pageBreakGlobal
            // 
            this.pageBreakGlobal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 39.04167F);
            this.pageBreakGlobal.Name = "pageBreakGlobal";
            // 
            // xsbrptChartGlobal
            // 
            this.xsbrptChartGlobal.Id = 0;
            this.xsbrptChartGlobal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
            this.xsbrptChartGlobal.Name = "xsbrptChartGlobal";
            this.xsbrptChartGlobal.ReportSource = new BPMO.SDNI.Contratos.RD.RPT.PorcentajeUtilizacionRDGlobalGraficaRPT();
            this.xsbrptChartGlobal.SizeF = new System.Drawing.SizeF(1055F, 23F);
            // 
            // ReportFooter
            // 
            this.ReportFooter.Expanded = false;
            this.ReportFooter.HeightF = 7.291667F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 23.95833F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // TituloGrafica
            // 
            this.TituloGrafica.Description = "Título Reporte";
            this.TituloGrafica.Name = "TituloGrafica";
            this.TituloGrafica.ValueInfo = "Time Utilization";
            this.TituloGrafica.Visible = false;
            // 
            // TituloGraficaGlobal
            // 
            this.TituloGraficaGlobal.Description = "Título Reporte Global";
            this.TituloGraficaGlobal.Name = "TituloGraficaGlobal";
            this.TituloGraficaGlobal.ValueInfo = "Time Utilization Idealease";
            this.TituloGraficaGlobal.Visible = false;
            // 
            // GrupoVacio
            // 
            this.GrupoVacio.DataMember = "Sucursales";
            this.GrupoVacio.Expression = "1";
            this.GrupoVacio.FieldType = DevExpress.XtraReports.UI.FieldType.Int32;
            this.GrupoVacio.Name = "GrupoVacio";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xsbrptChartGlobal,
            this.pageBreakGlobal});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GrupoVacio", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 45.83333F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // PorcentajeUtilizacionRDRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportFooter,
            this.ReportHeader,
            this.GroupHeader1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.TituloSucursal,
            this.GrupoVacio});
            this.DataMember = "Sucursales";
            this.DataSource = this.reporteRDSucursalDS;
            this.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(21, 24, 0, 28);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.TituloGrafica,
            this.TituloGraficaGlobal});
            this.Version = "13.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.PorcentajeUtilizacionRDRPT_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Flota.Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.CalculatedField TituloSucursal;
        private DevExpress.XtraReports.UI.XRSubreport xsbrptChartDetails;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRSubreport xsbrptChartGlobal;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.Parameters.Parameter TituloGrafica;
        private DevExpress.XtraReports.Parameters.Parameter TituloGraficaGlobal;
        private DevExpress.XtraReports.UI.XRPageBreak pageBreakGlobal;
        private DevExpress.XtraReports.UI.CalculatedField GrupoVacio;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
    }
}
