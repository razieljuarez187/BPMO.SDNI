namespace BPMO.SDNI.Mantenimiento.RPT
{
    partial class ReporteRendimientoUnidadGraficaRPT
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel1 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel2 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel3 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel4 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel5 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.CustomAxisLabel customAxisLabel6 = new DevExpress.XtraCharts.CustomAxisLabel();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteRendimientoUnidadDS = new BPMO.SDNI.Mantenimiento.Reportes.DA.ReporteRendimientoUnidadDS();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrcGraficaRendimiento = new DevExpress.XtraReports.UI.XRChart();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRendimientoUnidadDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrcGraficaRendimiento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 9F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteRendimientoUnidadDS
            // 
            this.reporteRendimientoUnidadDS.DataSetName = "ReporteRendimientoUnidadDS";
            this.reporteRendimientoUnidadDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrcGraficaRendimiento});
            this.ReportHeader.HeightF = 262.5F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrcGraficaRendimiento
            // 
            this.xrcGraficaRendimiento.BackColor = System.Drawing.Color.Transparent;
            this.xrcGraficaRendimiento.BorderColor = System.Drawing.Color.Black;
            this.xrcGraficaRendimiento.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrcGraficaRendimiento.DataMember = "RendimientoUnidad";
            this.xrcGraficaRendimiento.DataSource = this.reporteRendimientoUnidadDS;
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
            xyDiagram1.AxisX.CustomLabels.AddRange(new DevExpress.XtraCharts.CustomAxisLabel[] {
            customAxisLabel1,
            customAxisLabel2,
            customAxisLabel3,
            customAxisLabel4,
            customAxisLabel5,
            customAxisLabel6});
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("Arial Narrow", 9.75F);
            xyDiagram1.AxisX.Title.Text = "MESES";
            xyDiagram1.AxisX.Title.Visible = true;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = true;
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Label.Font = new System.Drawing.Font("Arial Narrow", 9.75F);
            xyDiagram1.AxisY.Title.Text = "REND. ACUM.";
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BackColor = System.Drawing.Color.Transparent;
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            this.xrcGraficaRendimiento.Diagram = xyDiagram1;
            this.xrcGraficaRendimiento.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.xrcGraficaRendimiento.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.xrcGraficaRendimiento.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrcGraficaRendimiento.Name = "xrcGraficaRendimiento";
            series1.ArgumentDataMember = "AnioMes";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            pointSeriesLabel1.LineColor = System.Drawing.Color.Black;
            pointSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAroundPoint;
            series1.Label = pointSeriesLabel1;
            series1.Name = "Rendimiento Km";
            series1.ValueDataMembersSerializable = "ViajeRendimientoKms";
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.View = lineSeriesView1;
            series2.ArgumentDataMember = "AnioMes";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            pointSeriesLabel2.LineColor = System.Drawing.Color.Aqua;
            pointSeriesLabel2.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
            series2.Label = pointSeriesLabel2;
            series2.Name = "Rendimiento Hr";
            series2.ValueDataMembersSerializable = "ViajeRendimientoHrs";
            series2.View = lineSeriesView2;
            this.xrcGraficaRendimiento.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.xrcGraficaRendimiento.SeriesTemplate.View = lineSeriesView3;
            this.xrcGraficaRendimiento.SizeF = new System.Drawing.SizeF(629.9999F, 262.5F);
            this.xrcGraficaRendimiento.StylePriority.UseBackColor = false;
            chartTitle1.Font = new System.Drawing.Font("Arial Narrow", 9.75F);
            chartTitle1.Text = "RENDIMIENTO UNIDAD";
            this.xrcGraficaRendimiento.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // ReporteRendimientoUnidadGraficaRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.DataMember = "RendimientoUnidad";
            this.DataSource = this.reporteRendimientoUnidadDS;
            this.Margins = new System.Drawing.Printing.Margins(0, 217, 9, 0);
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.reporteRendimientoUnidadDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrcGraficaRendimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.ReporteRendimientoUnidadDS reporteRendimientoUnidadDS;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRChart xrcGraficaRendimiento;
    }
}
