namespace BPMO.SDNI.Flota.RPT
{
    partial class RentaDiariaGeneralSucursalModeloRPT
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesView sideBySideBarSeriesView1 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtNombreModelo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDiasMes = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtWhiteSpace = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            this.grpFTotales = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrGraficaUtilizacion = new DevExpress.XtraReports.UI.XRChart();
            this.xrtPorcentajeTotal = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTPorcentajeUtilizacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalesUtilizacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPromedioUtilizacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalDisponibles = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTotalDisponibles = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalesDisponibles = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPromedioDisponibles = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalRentados = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTTotalesRentados = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalesRentados = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPromedioTotalRentados = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTTotales = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTTotalFlota = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalesFlota = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTPromedio = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTPromediosLabels = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtCellPromedioLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.grpHTotales = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrGraficaUtilizacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtPorcentajeTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtTotalDisponibles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtTotalRentados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTTotales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTPromediosLabels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ModeloID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1074F, 25F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtNombreModelo,
            this.xrtDiasMes,
            this.xrtWhiteSpace});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrtNombreModelo
            // 
            this.xrtNombreModelo.Name = "xrtNombreModelo";
            this.xrtNombreModelo.Text = "xrtNombreModelo";
            this.xrtNombreModelo.Weight = 0.27932952369391584D;
            this.xrtNombreModelo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrtNombreModelo_BeforePrint);
            // 
            // xrtDiasMes
            // 
            this.xrtDiasMes.Name = "xrtDiasMes";
            this.xrtDiasMes.Weight = 2.1229051131775925D;
            // 
            // xrtWhiteSpace
            // 
            this.xrtWhiteSpace.Name = "xrtWhiteSpace";
            this.xrtWhiteSpace.Weight = 0.5977653631284916D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 11F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 11F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // grpFTotales
            // 
            this.grpFTotales.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrGraficaUtilizacion,
            this.xrtPorcentajeTotal,
            this.xrTable4,
            this.xrtTotalDisponibles,
            this.xrtTotalRentados,
            this.xrTTotales});
            this.grpFTotales.HeightF = 467.7083F;
            this.grpFTotales.Name = "grpFTotales";
            this.grpFTotales.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.grpFTotales_BeforePrint);
            // 
            // xrGraficaUtilizacion
            // 
            this.xrGraficaUtilizacion.BorderColor = System.Drawing.Color.Black;
            this.xrGraficaUtilizacion.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrGraficaUtilizacion.DataMember = "SubTotalXDia";
            this.xrGraficaUtilizacion.DataSource = this.reporteRDSucursalDS;
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("Tahoma", 6F);
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            xyDiagram1.AxisX.NumericScaleOptions.AggregateFunction = DevExpress.XtraCharts.AggregateFunction.None;
            xyDiagram1.AxisX.NumericScaleOptions.AutoGrid = false;
            xyDiagram1.AxisX.NumericScaleOptions.GridSpacing = 0.5D;
            xyDiagram1.AxisX.NumericScaleOptions.ScaleMode = DevExpress.XtraCharts.ScaleMode.Automatic;
            xyDiagram1.AxisX.Title.Text = "Días";
            xyDiagram1.AxisX.Title.Visible = true;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = true;
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Title.Text = "Utilización";
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.AutoSideMargins = true;
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            this.xrGraficaUtilizacion.Diagram = xyDiagram1;
            this.xrGraficaUtilizacion.Legend.Visible = false;
            this.xrGraficaUtilizacion.LocationFloat = new DevExpress.Utils.PointFloat(99.99975F, 151.0416F);
            this.xrGraficaUtilizacion.Name = "xrGraficaUtilizacion";
            series1.ArgumentDataMember = "Dia";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series1.Name = "Días";
            series1.ValueDataMembersSerializable = "PorcentajeUtilizacionFlota";
            sideBySideBarSeriesView1.BarWidth = 0.3D;
            series1.View = sideBySideBarSeriesView1;
            this.xrGraficaUtilizacion.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.xrGraficaUtilizacion.SizeF = new System.Drawing.SizeF(760.0002F, 297.9167F);
            chartTitle1.Text = "Sucursal - Mes";
            this.xrGraficaUtilizacion.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // xrtPorcentajeTotal
            // 
            this.xrtPorcentajeTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtPorcentajeTotal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 100F);
            this.xrtPorcentajeTotal.Name = "xrtPorcentajeTotal";
            this.xrtPorcentajeTotal.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrtPorcentajeTotal.SizeF = new System.Drawing.SizeF(1074F, 36.45833F);
            this.xrtPorcentajeTotal.StylePriority.UseBorders = false;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTPorcentajeUtilizacion,
            this.xrTotalesUtilizacion,
            this.xrPromedioUtilizacion});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.StylePriority.UseBorders = false;
            this.xrTableRow5.StylePriority.UseTextAlignment = false;
            this.xrTableRow5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTPorcentajeUtilizacion
            // 
            this.xrTPorcentajeUtilizacion.Name = "xrTPorcentajeUtilizacion";
            this.xrTPorcentajeUtilizacion.Text = "Porcentaje de Utilización Flota";
            this.xrTPorcentajeUtilizacion.Weight = 0.27932952702749547D;
            // 
            // xrTotalesUtilizacion
            // 
            this.xrTotalesUtilizacion.Name = "xrTotalesUtilizacion";
            this.xrTotalesUtilizacion.Weight = 2.122905155561809D;
            // 
            // xrPromedioUtilizacion
            // 
            this.xrPromedioUtilizacion.Name = "xrPromedioUtilizacion";
            this.xrPromedioUtilizacion.Text = "xrPromedioUtilizacion";
            this.xrPromedioUtilizacion.Weight = 0.59776531741069516D;
            // 
            // xrTable4
            // 
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1074F, 25F);
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.Weight = 3D;
            // 
            // xrtTotalDisponibles
            // 
            this.xrtTotalDisponibles.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtTotalDisponibles.LocationFloat = new DevExpress.Utils.PointFloat(0F, 50F);
            this.xrtTotalDisponibles.Name = "xrtTotalDisponibles";
            this.xrtTotalDisponibles.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrtTotalDisponibles.SizeF = new System.Drawing.SizeF(1074F, 25F);
            this.xrtTotalDisponibles.StylePriority.UseBorders = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTotalDisponibles,
            this.xrTotalesDisponibles,
            this.xrPromedioDisponibles});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.StylePriority.UseBorders = false;
            this.xrTableRow3.StylePriority.UseTextAlignment = false;
            this.xrTableRow3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTotalDisponibles
            // 
            this.xrTotalDisponibles.Name = "xrTotalDisponibles";
            this.xrTotalDisponibles.Text = "Total Disponibles";
            this.xrTotalDisponibles.Weight = 0.27932952702749547D;
            // 
            // xrTotalesDisponibles
            // 
            this.xrTotalesDisponibles.Name = "xrTotalesDisponibles";
            this.xrTotalesDisponibles.Weight = 2.1229054965403744D;
            // 
            // xrPromedioDisponibles
            // 
            this.xrPromedioDisponibles.Name = "xrPromedioDisponibles";
            this.xrPromedioDisponibles.Text = "xrPromedioDisponibles";
            this.xrPromedioDisponibles.Weight = 0.59776497643212978D;
            // 
            // xrtTotalRentados
            // 
            this.xrtTotalRentados.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtTotalRentados.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
            this.xrtTotalRentados.Name = "xrtTotalRentados";
            this.xrtTotalRentados.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrtTotalRentados.SizeF = new System.Drawing.SizeF(1074F, 25F);
            this.xrtTotalRentados.StylePriority.UseBorders = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTTotalesRentados,
            this.xrTotalesRentados,
            this.xrPromedioTotalRentados});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.StylePriority.UseBorders = false;
            this.xrTableRow7.StylePriority.UseTextAlignment = false;
            this.xrTableRow7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTTotalesRentados
            // 
            this.xrTTotalesRentados.Name = "xrTTotalesRentados";
            this.xrTTotalesRentados.Text = "Total Rentados";
            this.xrTTotalesRentados.Weight = 0.99999967839096238D;
            // 
            // xrTotalesRentados
            // 
            this.xrTotalesRentados.Name = "xrTotalesRentados";
            this.xrTotalesRentados.Weight = 7.5999982516326838D;
            // 
            // xrPromedioTotalRentados
            // 
            this.xrPromedioTotalRentados.Name = "xrPromedioTotalRentados";
            this.xrPromedioTotalRentados.Text = "xrPromedioTotalRentados";
            this.xrPromedioTotalRentados.Weight = 2.1400014596247914D;
            // 
            // xrTTotales
            // 
            this.xrTTotales.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTTotales.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTTotales.Name = "xrTTotales";
            this.xrTTotales.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTTotales.SizeF = new System.Drawing.SizeF(1074F, 25F);
            this.xrTTotales.StylePriority.UseBorders = false;
            this.xrTTotales.StylePriority.UseTextAlignment = false;
            this.xrTTotales.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTTotalFlota,
            this.xrTotalesFlota,
            this.xrTPromedio});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTTotalFlota
            // 
            this.xrTTotalFlota.Name = "xrTTotalFlota";
            this.xrTTotalFlota.Text = "Total Flota";
            this.xrTTotalFlota.Weight = 0.27932952369391584D;
            // 
            // xrTotalesFlota
            // 
            this.xrTotalesFlota.Name = "xrTotalesFlota";
            this.xrTotalesFlota.Weight = 2.1229051131775925D;
            // 
            // xrTPromedio
            // 
            this.xrTPromedio.Name = "xrTPromedio";
            this.xrTPromedio.Text = "xrTPromedio";
            this.xrTPromedio.Weight = 0.5977653631284916D;
            this.xrTPromedio.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTPromedio_BeforePrint);
            // 
            // xrTPromediosLabels
            // 
            this.xrTPromediosLabels.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTPromediosLabels.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTPromediosLabels.Name = "xrTPromediosLabels";
            this.xrTPromediosLabels.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTPromediosLabels.SizeF = new System.Drawing.SizeF(1074F, 25F);
            this.xrTPromediosLabels.StylePriority.UseBorders = false;
            this.xrTPromediosLabels.StylePriority.UseTextAlignment = false;
            this.xrTPromediosLabels.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrtCellPromedioLabel});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.Weight = 0.27932952369391584D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.Weight = 2.1229056246453823D;
            // 
            // xrtCellPromedioLabel
            // 
            this.xrtCellPromedioLabel.Name = "xrtCellPromedioLabel";
            this.xrtCellPromedioLabel.Text = "Promedio Mensual";
            this.xrtCellPromedioLabel.Weight = 0.59776485166070181D;
            // 
            // grpHTotales
            // 
            this.grpHTotales.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTPromediosLabels});
            this.grpHTotales.HeightF = 25F;
            this.grpHTotales.Name = "grpHTotales";
            // 
            // RentaDiariaGeneralSucursalModeloRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.grpFTotales,
            this.grpHTotales});
            this.DataMember = "SubTotalXModelo";
            this.DataSource = this.reporteRDSucursalDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(14, 12, 11, 11);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrGraficaUtilizacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtPorcentajeTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtTotalDisponibles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtTotalRentados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTTotales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTPromediosLabels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrtNombreModelo;
        private DevExpress.XtraReports.UI.XRTableCell xrtDiasMes;
        private DevExpress.XtraReports.UI.XRTableCell xrtWhiteSpace;
        private DevExpress.XtraReports.UI.GroupFooterBand grpFTotales;
        private DevExpress.XtraReports.UI.XRTable xrTTotales;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTTotalFlota;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalesFlota;
        private DevExpress.XtraReports.UI.XRTableCell xrTPromedio;
        private DevExpress.XtraReports.UI.XRTable xrtPorcentajeTotal;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTPorcentajeUtilizacion;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalesUtilizacion;
        private DevExpress.XtraReports.UI.XRTableCell xrPromedioUtilizacion;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTable xrtTotalDisponibles;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalDisponibles;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalesDisponibles;
        private DevExpress.XtraReports.UI.XRTableCell xrPromedioDisponibles;
        private DevExpress.XtraReports.UI.XRTable xrtTotalRentados;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTTotalesRentados;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalesRentados;
        private DevExpress.XtraReports.UI.XRTableCell xrPromedioTotalRentados;
        private DevExpress.XtraReports.UI.XRChart xrGraficaUtilizacion;
        private DevExpress.XtraReports.UI.XRTable xrTPromediosLabels;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellPromedioLabel;
        private DevExpress.XtraReports.UI.GroupHeaderBand grpHTotales;
    }
}
