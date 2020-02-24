namespace BPMO.SDNI.Flota.RPT
{
    partial class DollarUtilizationElementoSucursalRPT
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
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel1 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel2 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel3 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel4 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel5 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel6 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel7 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrChartElementoSucursal = new DevExpress.XtraReports.UI.XRChart();
            this.reporteDollarUtilization = new BPMO.SDNI.Flota.Reportes.DA.DollarUtilizationDS();
            this.TituloGrafica = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrChartElementoSucursal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteDollarUtilization)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 10.41667F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 5F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 14F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrChartElementoSucursal});
            this.ReportHeader.HeightF = 595.8333F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrChartElementoSucursal
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
            this.xrChartElementoSucursal.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation1});
            this.xrChartElementoSucursal.BorderColor = System.Drawing.Color.Black;
            this.xrChartElementoSucursal.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChartElementoSucursal.DataMember = "ResultadoDollarUtilizationXSucursal";
            this.xrChartElementoSucursal.DataSource = this.reporteDollarUtilization;
            customAxisLabel1.AxisValueSerializable = "0";
            customAxisLabel1.Name = "Label 0";
            customAxisLabel2.AxisValueSerializable = "1";
            customAxisLabel2.Name = "Label 1";
            customAxisLabel3.AxisValueSerializable = "2";
            customAxisLabel3.Name = "Label 2";
            customAxisLabel4.AxisValueSerializable = "3";
            customAxisLabel4.Name = "Label 3";
            customAxisLabel5.AxisValueSerializable = "4";
            customAxisLabel5.Name = "Label 4";
            customAxisLabel6.AxisValueSerializable = "5";
            customAxisLabel6.Name = "Label 5";
            customAxisLabel7.AxisValueSerializable = "6";
            customAxisLabel7.Name = "Label 6";
            xyDiagram1.AxisX.CustomLabels.AddRange(new DevExpress.XtraCharts.CustomAxisLabel[] {
            customAxisLabel1,
            customAxisLabel2,
            customAxisLabel3,
            customAxisLabel4,
            customAxisLabel5,
            customAxisLabel6,
            customAxisLabel7});
            xyDiagram1.AxisX.Label.Angle = -1;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = true;
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Title.Font = new System.Drawing.Font("Arial Narrow", 9.7F);
            xyDiagram1.AxisY.Title.Text = "* Miles de Pesos";
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BackColor = System.Drawing.Color.Transparent;
            xyDiagram1.DefaultPane.BorderColor = System.Drawing.Color.Transparent;
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
            this.xrChartElementoSucursal.Diagram = xyDiagram1;
            this.xrChartElementoSucursal.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.xrChartElementoSucursal.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.xrChartElementoSucursal.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.xrChartElementoSucursal.Legend.EquallySpacedItems = false;
            this.xrChartElementoSucursal.Legend.Font = new System.Drawing.Font("Arial Narrow", 9.7F);
            this.xrChartElementoSucursal.LocationFloat = new DevExpress.Utils.PointFloat(8F, 10.00001F);
            this.xrChartElementoSucursal.Name = "xrChartElementoSucursal";
            series1.ArgumentDataMember = "Mes";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            sideBySideBarSeriesLabel1.Antialiasing = true;
            pointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions1.ValueNumericOptions.Precision = 0;
            sideBySideBarSeriesLabel1.PointOptions = pointOptions1;
            sideBySideBarSeriesLabel1.Position = DevExpress.XtraCharts.BarSeriesLabelPosition.Center;
            sideBySideBarSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.Default;
            series1.Label = sideBySideBarSeriesLabel1;
            series1.LegendText = "Miles de Pesos";
            series1.Name = "Valor Flota";
            series1.ValueDataMembersSerializable = "ResultadoDollarUtilizationXSucursal.ValorFlota";
            series2.ArgumentDataMember = "Mes";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.LegendText = "Dollar Utilization";
            series2.Name = "Dollar Utilization";
            series2.ValueDataMembersSerializable = "ResultadoDollarUtilizationXSucursal.DollarUtilizationMensual";
            lineSeriesView1.AxisYName = "EjeY Secundaria 1";
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.View = lineSeriesView1;
            this.xrChartElementoSucursal.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.xrChartElementoSucursal.SideBySideBarDistanceFixed = 0;
            this.xrChartElementoSucursal.SizeF = new System.Drawing.SizeF(1052F, 577.92F);
            chartTitle1.Font = new System.Drawing.Font("Arial Narrow", 9.7F, System.Drawing.FontStyle.Bold);
            chartTitle1.Text = "DOLLAR UTILIZATION MENSUALIZADO";
            this.xrChartElementoSucursal.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            this.xrChartElementoSucursal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrChartElementoSucursal_BeforePrint);
            // 
            // reporteDollarUtilization
            // 
            this.reporteDollarUtilization.DataSetName = "ReporteDollarUtilizationXSucursal";
            this.reporteDollarUtilization.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TituloGrafica
            // 
            this.TituloGrafica.Name = "TituloGrafica";
            this.TituloGrafica.ValueInfo = "Dollar Utilization Mensualizado";
            // 
            // DollarUtilizationElementoSucursalRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.DataMember = "ResultadoDollarUtilizationXSucursal";
            this.DataSource = this.reporteDollarUtilization;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(9, 6, 5, 14);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.TituloGrafica});
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChartElementoSucursal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteDollarUtilization)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.DollarUtilizationDS reporteDollarUtilization;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRChart xrChartElementoSucursal;
        private DevExpress.XtraReports.Parameters.Parameter TituloGrafica;
    }
}
