namespace BPMO.SDNI.Flota.RPT
{
    partial class DiasRentaTipoUnidadRPT
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ModeloNombre = new DevExpress.XtraReports.UI.CalculatedField();
            this.SucursalNombre = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xsbrptChartGlobal = new DevExpress.XtraReports.UI.XRSubreport();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.TituloGrafica = new DevExpress.XtraReports.Parameters.Parameter();
            this.TituloGraficaGlobal = new DevExpress.XtraReports.Parameters.Parameter();
            this.GrupoVacio = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xsbrptChartDetails});
            this.Detail.HeightF = 34.375F;
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
            this.xsbrptChartDetails.ReportSource = new BPMO.SDNI.Flota.RPT.DiasRentaElementoGraficaModeloRPT();
            this.xsbrptChartDetails.SizeF = new System.Drawing.SizeF(521F, 23F);
            this.xsbrptChartDetails.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xsbrptChartDetails_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 1F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 22F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 18.75003F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // tblTituloEtiquetas
            // 
            this.tblTituloEtiquetas.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(170.6666F, 43.75F);
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
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(906.7084F, 48.87502F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // pbLogo
            // 
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(906.7084F, 71.875F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(142.2917F, 23F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // ModeloNombre
            // 
            this.ModeloNombre.DataMember = "Modelo_Sucursal";
            this.ModeloNombre.FieldType = DevExpress.XtraReports.UI.FieldType.String;
            this.ModeloNombre.Name = "ModeloNombre";
            this.ModeloNombre.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.ModeloNombre_GetValue);
            // 
            // SucursalNombre
            // 
            this.SucursalNombre.DataMember = "Modelo_Sucursal";
            this.SucursalNombre.FieldType = DevExpress.XtraReports.UI.FieldType.String;
            this.SucursalNombre.Name = "SucursalNombre";
            this.SucursalNombre.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.SucursalNombre_GetValue);
            // 
            // ReportFooter
            // 
            this.ReportFooter.HeightF = 15.625F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xsbrptChartGlobal
            // 
            this.xsbrptChartGlobal.Id = 0;
            this.xsbrptChartGlobal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6F);
            this.xsbrptChartGlobal.Name = "xsbrptChartGlobal";
            this.xsbrptChartGlobal.ReportSource = new BPMO.SDNI.Flota.RPT.DiasRentaGlobalGraficaRPT();
            this.xsbrptChartGlobal.SizeF = new System.Drawing.SizeF(1055F, 23F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pbLogo,
            this.tblTituloEtiquetas,
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.PageHeader.HeightF = 106.2499F;
            this.PageHeader.Name = "PageHeader";
            // 
            // TituloGrafica
            // 
            this.TituloGrafica.Description = "Título Reporte";
            this.TituloGrafica.Name = "TituloGrafica";
            this.TituloGrafica.ValueInfo = "Días de Renta";
            this.TituloGrafica.Visible = false;
            // 
            // TituloGraficaGlobal
            // 
            this.TituloGraficaGlobal.Description = "Título Reporte Global";
            this.TituloGraficaGlobal.Name = "TituloGraficaGlobal";
            this.TituloGraficaGlobal.ValueInfo = "Días de Renta Global Idealease";
            this.TituloGraficaGlobal.Visible = false;
            // 
            // GrupoVacio
            // 
            this.GrupoVacio.DataMember = "Modelos";
            this.GrupoVacio.Expression = "1";
            this.GrupoVacio.FieldType = DevExpress.XtraReports.UI.FieldType.Int32;
            this.GrupoVacio.Name = "GrupoVacio";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageBreak1,
            this.xsbrptChartGlobal});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GrupoVacio", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 41.66667F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 33.83334F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // DiasRentaTipoUnidadRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter,
            this.PageHeader,
            this.GroupHeader2});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.ModeloNombre,
            this.SucursalNombre,
            this.GrupoVacio});
            this.DataMember = "Modelos";
            this.DataSource = this.reporteRDSucursalDS;
            this.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(20, 22, 1, 22);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.TituloGrafica,
            this.TituloGraficaGlobal});
            this.Version = "13.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.PorcentajeUtilizacionRDTipoUnidadRPT_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Flota.Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.CalculatedField ModeloNombre;
        private DevExpress.XtraReports.UI.CalculatedField SucursalNombre;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRSubreport xsbrptChartGlobal;
        private DevExpress.XtraReports.UI.XRSubreport xsbrptChartDetails;
        private DevExpress.XtraReports.Parameters.Parameter TituloGrafica;
        private DevExpress.XtraReports.Parameters.Parameter TituloGraficaGlobal;
        private DevExpress.XtraReports.UI.CalculatedField GrupoVacio;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
    }
}
