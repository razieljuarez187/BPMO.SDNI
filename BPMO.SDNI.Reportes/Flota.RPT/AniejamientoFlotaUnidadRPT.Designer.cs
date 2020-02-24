namespace BPMO.SDNI.Flota.RPT
{
    partial class AniejamientoFlotaUnidadRPT
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrtAnioHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtCellAnio = new DevExpress.XtraReports.UI.XRTableCell();
            this.aniejamientoFlotaDS1 = new BPMO.SDNI.Flota.Reportes.DA.AniejamientoFlotaDS();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.gHeaderAnio = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xtrDetail = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtCellSucursal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtCellReferencia = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtCellVin = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtCellAnioDetail = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrtAnioHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aniejamientoFlotaDS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtrDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xtrDetail});
            this.Detail.HeightF = 15F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("FechaRentaId", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 12F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 14.19472F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrtAnioHeader
            // 
            this.xrtAnioHeader.BackColor = System.Drawing.Color.Transparent;
            this.xrtAnioHeader.BorderColor = System.Drawing.Color.Black;
            this.xrtAnioHeader.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrtAnioHeader.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtAnioHeader.ForeColor = System.Drawing.Color.Black;
            this.xrtAnioHeader.LocationFloat = new DevExpress.Utils.PointFloat(326.0002F, 0F);
            this.xrtAnioHeader.Name = "xrtAnioHeader";
            this.xrtAnioHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrtAnioHeader.SizeF = new System.Drawing.SizeF(750F, 15F);
            this.xrtAnioHeader.StylePriority.UseBackColor = false;
            this.xrtAnioHeader.StylePriority.UseBorderColor = false;
            this.xrtAnioHeader.StylePriority.UseBorders = false;
            this.xrtAnioHeader.StylePriority.UseFont = false;
            this.xrtAnioHeader.StylePriority.UseForeColor = false;
            this.xrtAnioHeader.StylePriority.UseTextAlignment = false;
            this.xrtAnioHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtCellAnio});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrtCellAnio
            // 
            this.xrtCellAnio.BackColor = System.Drawing.Color.Transparent;
            this.xrtCellAnio.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrtCellAnio.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtCellAnio.ForeColor = System.Drawing.Color.Black;
            this.xrtCellAnio.Name = "xrtCellAnio";
            this.xrtCellAnio.StylePriority.UseBackColor = false;
            this.xrtCellAnio.StylePriority.UseBorders = false;
            this.xrtCellAnio.StylePriority.UseFont = false;
            this.xrtCellAnio.StylePriority.UseForeColor = false;
            this.xrtCellAnio.StylePriority.UseTextAlignment = false;
            this.xrtCellAnio.Text = "xrtCellAnio";
            this.xrtCellAnio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrtCellAnio.Weight = 2.5714285714285716D;
            // 
            // aniejamientoFlotaDS1
            // 
            this.aniejamientoFlotaDS1.DataSetName = "AniejamientoFlotaDS";
            this.aniejamientoFlotaDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.ExcludeSchema;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable1.SizeF = new System.Drawing.SizeF(326F, 15F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FlotaXFechaCompra.FechaRentaId")});
            this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 7F);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell1.Summary = xrSummary1;
            this.xrTableCell1.Weight = 3D;
            // 
            // gHeaderAnio
            // 
            this.gHeaderAnio.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrtAnioHeader});
            this.gHeaderAnio.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("FechaRentaId", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ModeloID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.gHeaderAnio.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.gHeaderAnio.HeightF = 15F;
            this.gHeaderAnio.Name = "gHeaderAnio";
            this.gHeaderAnio.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gHeaderAnio_BeforePrint);
            // 
            // xtrDetail
            // 
            this.xtrDetail.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.xtrDetail.LocationFloat = new DevExpress.Utils.PointFloat(0.000205829F, 0F);
            this.xtrDetail.Name = "xtrDetail";
            this.xtrDetail.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xtrDetail.SizeF = new System.Drawing.SizeF(1076F, 15F);
            this.xtrDetail.StylePriority.UseFont = false;
            this.xtrDetail.StylePriority.UseTextAlignment = false;
            this.xtrDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtCellSucursal,
            this.xrtCellReferencia,
            this.xrtCellVin,
            this.xrtCellAnioDetail});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrtCellSucursal
            // 
            this.xrtCellSucursal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtCellSucursal.Multiline = true;
            this.xrtCellSucursal.Name = "xrtCellSucursal";
            this.xrtCellSucursal.StylePriority.UseBorders = false;
            this.xrtCellSucursal.Text = "xrtCellSucursal";
            this.xrtCellSucursal.Weight = 0.22304833422806183D;
            // 
            // xrtCellReferencia
            // 
            this.xrtCellReferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtCellReferencia.Multiline = true;
            this.xrtCellReferencia.Name = "xrtCellReferencia";
            this.xrtCellReferencia.StylePriority.UseBorders = false;
            this.xrtCellReferencia.Text = "xrtCellReferencia";
            this.xrtCellReferencia.Weight = 0.351301122332151D;
            // 
            // xrtCellVin
            // 
            this.xrtCellVin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtCellVin.Multiline = true;
            this.xrtCellVin.Name = "xrtCellVin";
            this.xrtCellVin.StylePriority.UseBorders = false;
            this.xrtCellVin.Text = "xrtCellVin";
            this.xrtCellVin.Weight = 0.33457190928405978D;
            // 
            // xrtCellAnioDetail
            // 
            this.xrtCellAnioDetail.Name = "xrtCellAnioDetail";
            this.xrtCellAnioDetail.Text = "xrtCellAnioDetail";
            this.xrtCellAnioDetail.Weight = 2.0910786341557275D;
            // 
            // AniejamientoFlotaUnidadRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.gHeaderAnio});
            this.DataMember = "FlotaXFechaCompra";
            this.DataSource = this.aniejamientoFlotaDS1;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(12, 12, 12, 14);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.AniejamientoFlotaUnidadRPT_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrtAnioHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aniejamientoFlotaDS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtrDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrtAnioHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellAnio;
        private Reportes.DA.AniejamientoFlotaDS aniejamientoFlotaDS1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.GroupHeaderBand gHeaderAnio;
        private DevExpress.XtraReports.UI.XRTable xtrDetail;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellSucursal;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellReferencia;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellVin;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellAnioDetail;
    }
}
