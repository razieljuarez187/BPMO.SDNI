namespace BPMO.SDNI.Flota.RPT
{
    partial class DollarUtilizationRPT
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
            DevExpress.XtraCharts.TextAnnotation textAnnotation1 = new DevExpress.XtraCharts.TextAnnotation();
            DevExpress.XtraCharts.ChartAnchorPoint chartAnchorPoint1 = new DevExpress.XtraCharts.ChartAnchorPoint();
            DevExpress.XtraCharts.FreePosition freePosition1 = new DevExpress.XtraCharts.FreePosition();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.reporteDollarUtilization = new BPMO.SDNI.Flota.Reportes.DA.DollarUtilizationDS();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTCRangoFechas = new DevExpress.XtraReports.UI.XRTableCell();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.chartDollarUtilization = new DevExpress.XtraReports.UI.XRChart();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.TituloGrafica = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrSRChartDetails = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this.reporteDollarUtilization)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDollarUtilization)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSRChartDetails});
            this.Detail.HeightF = 33.41662F;
            this.Detail.KeepTogether = true;
            this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SucursalID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("NombreSucursal", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteDollarUtilization
            // 
            this.reporteDollarUtilization.DataSetName = "ReporteDollarUtilization";
            this.reporteDollarUtilization.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 3.708331F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(841.5831F, 69.16669F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(142.2917F, 20.83334F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(841.5834F, 48.33334F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(142.2916F, 20.83333F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // tblTituloEtiquetas
            // 
            this.tblTituloEtiquetas.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(243.875F, 28.50001F);
            this.tblTituloEtiquetas.Name = "tblTituloEtiquetas";
            this.tblTituloEtiquetas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.tblTituloEtiquetas.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow14});
            this.tblTituloEtiquetas.SizeF = new System.Drawing.SizeF(587.5831F, 62.5F);
            this.tblTituloEtiquetas.StylePriority.UseFont = false;
            this.tblTituloEtiquetas.StylePriority.UsePadding = false;
            this.tblTituloEtiquetas.StylePriority.UseTextAlignment = false;
            this.tblTituloEtiquetas.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ConfiguracionModulo.NombreCliente")});
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Weight = 0.99131829478095046D;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "REPORTE DE DOLLAR UTILIZATION";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 0.99131829478095046D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTCRangoFechas});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTCRangoFechas
            // 
            this.xrTCRangoFechas.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTCRangoFechas.Name = "xrTCRangoFechas";
            this.xrTCRangoFechas.StylePriority.UseFont = false;
            this.xrTCRangoFechas.Weight = 0.99131829478095057D;
            // 
            // pbLogo
            // 
            this.pbLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "ConfiguracionesSistema.UrlLogoEmpresa")});
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(74.87501F, 10.00001F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 28.87478F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pbLogo,
            this.tblTituloEtiquetas,
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.PageHeader.HeightF = 126.0417F;
            this.PageHeader.Name = "PageHeader";
            // 
            // chartDollarUtilization
            // 
            chartAnchorPoint1.X = 0;
            chartAnchorPoint1.Y = 43;
            textAnnotation1.AnchorPoint = chartAnchorPoint1;
            textAnnotation1.Border.Visible = false;
            textAnnotation1.ConnectorStyle = DevExpress.XtraCharts.AnnotationConnectorStyle.None;
            textAnnotation1.Font = new System.Drawing.Font("Arial Narrow", 9.7F);
            textAnnotation1.Name = "Anotacion";
            textAnnotation1.ShapeKind = DevExpress.XtraCharts.ShapeKind.Rectangle;
            freePosition1.DockCorner = DevExpress.XtraCharts.DockCorner.LeftBottom;
            freePosition1.InnerIndents.Left = 0;
            freePosition1.InnerIndents.Top = 0;
            textAnnotation1.ShapePosition = freePosition1;
            textAnnotation1.Text = "* Expresado de Millones de pesos";
            this.chartDollarUtilization.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation1});
            this.chartDollarUtilization.BackColor = System.Drawing.Color.Transparent;
            this.chartDollarUtilization.BorderColor = System.Drawing.Color.Black;
            this.chartDollarUtilization.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.chartDollarUtilization.DataMember = "ResultadoDollarUtilization";
            this.chartDollarUtilization.DataSource = this.reporteDollarUtilization;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Title.Font = new System.Drawing.Font("Arial Narrow", 9.7F);
            xyDiagram1.AxisY.Title.Text = "* Miles de Pesos";
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            secondaryAxisY1.AxisID = 0;
            secondaryAxisY1.Name = "EjeY Secundaria 1";
            secondaryAxisY1.Title.Font = new System.Drawing.Font("Arial Narrow", 9.7F);
            secondaryAxisY1.Title.Text = "DU";
            secondaryAxisY1.Title.Visible = true;
            secondaryAxisY1.VisibleInPanesSerializable = "-1";
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
            this.chartDollarUtilization.Diagram = xyDiagram1;
            this.chartDollarUtilization.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            this.chartDollarUtilization.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.chartDollarUtilization.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.chartDollarUtilization.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartDollarUtilization.Legend.EquallySpacedItems = false;
            this.chartDollarUtilization.LocationFloat = new DevExpress.Utils.PointFloat(10.00004F, 10.00001F);
            this.chartDollarUtilization.Name = "chartDollarUtilization";
            series1.ArgumentDataMember = "Mes";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            pointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions1.ValueNumericOptions.Precision = 0;
            sideBySideBarSeriesLabel1.PointOptions = pointOptions1;
            sideBySideBarSeriesLabel1.Position = DevExpress.XtraCharts.BarSeriesLabelPosition.Center;
            series1.Label = sideBySideBarSeriesLabel1;
            series1.Name = "Valor Flota";
            series1.ValueDataMembersSerializable = "ResultadoDollarUtilization.ValorFlota";
            series2.ArgumentDataMember = "Mes";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            series2.Name = "Dollar Utilization";
            series2.ValueDataMembersSerializable = "ResultadoDollarUtilization.DollarUtilizationMensual";
            lineSeriesView1.AxisYName = "EjeY Secundaria 1";
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.View = lineSeriesView1;
            this.chartDollarUtilization.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartDollarUtilization.SizeF = new System.Drawing.SizeF(1052F, 577.9167F);
            chartTitle1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold);
            chartTitle1.Text = "DOLLAR UTILIZATION MENSUALIZADO";
            this.chartDollarUtilization.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 4.124991F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.chartDollarUtilization});
            this.GroupHeader1.HeightF = 602.9167F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // TituloGrafica
            // 
            this.TituloGrafica.Name = "TituloGrafica";
            this.TituloGrafica.ValueInfo = "Dollar Utilization Mensualizado";
            this.TituloGrafica.Visible = false;
            // 
            // xrSRChartDetails
            // 
            this.xrSRChartDetails.Id = 0;
            this.xrSRChartDetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrSRChartDetails.Name = "xrSRChartDetails";
            this.xrSRChartDetails.ReportSource = new BPMO.SDNI.Flota.RPT.DollarUtilizationElementoSucursalRPT();
            this.xrSRChartDetails.SizeF = new System.Drawing.SizeF(1072F, 32.79163F);
            this.xrSRChartDetails.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSRChartDetails_BeforePrint);
            // 
            // DollarUtilizationRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportHeader,
            this.GroupHeader1});
            this.DataMember = "Sucursales";
            this.DataSource = this.reporteDollarUtilization;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(12, 16, 4, 29);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.TituloGrafica});
            this.Version = "13.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DollarUtilizationRPT_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.reporteDollarUtilization)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDollarUtilization)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.DollarUtilizationDS reporteDollarUtilization;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow14;
        private DevExpress.XtraReports.UI.XRTableCell xrTCRangoFechas;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRChart chartDollarUtilization;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRSubreport xrSRChartDetails;
        private DevExpress.XtraReports.Parameters.Parameter TituloGrafica;
    }
}
