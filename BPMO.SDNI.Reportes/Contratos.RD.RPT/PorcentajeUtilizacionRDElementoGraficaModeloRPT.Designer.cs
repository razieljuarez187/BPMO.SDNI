namespace BPMO.SDNI.Contratos.RD.RPT
{
    partial class PorcentajeUtilizacionRDElementoGraficaModeloRPT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PorcentajeUtilizacionRDElementoGraficaModeloRPT));
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
            DevExpress.XtraCharts.SideBySideBarSeriesView sideBySideBarSeriesView1 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.chartTimeUtilization = new DevExpress.XtraReports.UI.XRChart();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.TituloGrafica = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.chartTimeUtilization)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // chartTimeUtilization
            // 
            resources.ApplyResources(this.chartTimeUtilization, "chartTimeUtilization");
            this.chartTimeUtilization.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.chartTimeUtilization.DataMember = "SubTotalXTipoUnidad";
            this.chartTimeUtilization.DataSource = this.reporteRDSucursalDS;
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
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = true;
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Label.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font")));
            xyDiagram1.AxisY.Title.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font1")));
            xyDiagram1.AxisY.Title.Text = resources.GetString("resource.Text");
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.AutoSideMargins = true;
            xyDiagram1.DefaultPane.BackColor = System.Drawing.Color.Transparent;
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            secondaryAxisY1.AxisID = 0;
            secondaryAxisY1.Label.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font2")));
            secondaryAxisY1.Name = "Secondary AxisY 1";
            secondaryAxisY1.Title.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font3")));
            secondaryAxisY1.Title.Text = resources.GetString("resource.Text1");
            secondaryAxisY1.Title.Visible = true;
            secondaryAxisY1.VisibleInPanesSerializable = "-1";
            secondaryAxisY1.WholeRange.AutoSideMargins = true;
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
            this.chartTimeUtilization.Diagram = xyDiagram1;
            this.chartTimeUtilization.Legend.AlignmentHorizontal = ((DevExpress.XtraCharts.LegendAlignmentHorizontal)(resources.GetObject("chartTimeUtilization.Legend.AlignmentHorizontal")));
            this.chartTimeUtilization.Legend.AlignmentVertical = ((DevExpress.XtraCharts.LegendAlignmentVertical)(resources.GetObject("chartTimeUtilization.Legend.AlignmentVertical")));
            this.chartTimeUtilization.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartTimeUtilization.Name = "chartTimeUtilization";
            series1.ArgumentDataMember = "AnioMes";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            sideBySideBarSeriesLabel1.Antialiasing = true;
            resources.ApplyResources(sideBySideBarSeriesLabel1, "sideBySideBarSeriesLabel1");
            sideBySideBarSeriesLabel1.Position = DevExpress.XtraCharts.BarSeriesLabelPosition.Center;
            sideBySideBarSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.Default;
            series1.Label = sideBySideBarSeriesLabel1;
            resources.ApplyResources(series1, "series1");
            series1.ValueDataMembersSerializable = "TotalFlotaXModelo";
            sideBySideBarSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            series1.View = sideBySideBarSeriesView1;
            series2.ArgumentDataMember = "AnioMes";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            pointSeriesLabel1.Antialiasing = true;
            resources.ApplyResources(pointSeriesLabel1, "pointSeriesLabel1");
            pointSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAroundPoint;
            series2.Label = pointSeriesLabel1;
            resources.ApplyResources(series2, "series2");
            series2.ValueDataMembersSerializable = "PorcentajeUtilizacion";
            lineSeriesView1.AxisYName = "Secondary AxisY 1";
            series2.View = lineSeriesView1;
            this.chartTimeUtilization.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartTimeUtilization.StylePriority.UseBackColor = false;
            this.chartTimeUtilization.StylePriority.UsePadding = false;
            resources.ApplyResources(chartTitle1, "chartTitle1");
            this.chartTimeUtilization.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            this.chartTimeUtilization.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.chartTimeUtilization_BeforePrint);
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TopMargin
            // 
            resources.ApplyResources(this.TopMargin, "TopMargin");
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // BottomMargin
            // 
            resources.ApplyResources(this.BottomMargin, "BottomMargin");
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.chartTimeUtilization});
            resources.ApplyResources(this.ReportHeader, "ReportHeader");
            this.ReportHeader.Name = "ReportHeader";
            // 
            // ReportFooter
            // 
            resources.ApplyResources(this.ReportFooter, "ReportFooter");
            this.ReportFooter.Name = "ReportFooter";
            // 
            // TituloGrafica
            // 
            resources.ApplyResources(this.TituloGrafica, "TituloGrafica");
            this.TituloGrafica.Name = "TituloGrafica";
            this.TituloGrafica.ValueInfo = "Time Utilization";
            this.TituloGrafica.Visible = false;
            // 
            // PorcentajeUtilizacionRDElementoGraficaModeloRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.ReportFooter,
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.DataMember = "SubTotalXTipoUnidad";
            this.DataSource = this.reporteRDSucursalDS;
            resources.ApplyResources(this, "$this");
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.TituloGrafica});
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTimeUtilization)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Flota.Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.XRChart chartTimeUtilization;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.Parameters.Parameter TituloGrafica;
    }
}
