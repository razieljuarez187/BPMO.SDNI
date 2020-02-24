namespace BPMO.SDNI.Flota.RPT
{
    partial class RentaDiariaGeneralMesSucursalRPT
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
            this.sbrptElementoDetail = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.sbrptElementoHder = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtMesAnio = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtMesNombre = new DevExpress.XtraReports.UI.XRTableCell();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
            this.Detail.HeightF = 23.95833F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sbrptElementoDetail
            // 
            this.sbrptElementoDetail.LocationFloat = new DevExpress.Utils.PointFloat(0F, 60.00001F);
            this.sbrptElementoDetail.Name = "sbrptElementoDetail";
            this.sbrptElementoDetail.ReportSource = new BPMO.SDNI.Flota.RPT.RentaDiariaGeneralModeloSucursalElementoRPT();
            this.sbrptElementoDetail.SizeF = new System.Drawing.SizeF(1074F, 23F);
            this.sbrptElementoDetail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbrptElementoDetail_BeforePrint);
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
            this.BottomMargin.HeightF = 58F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sbrptElementoHder,
            this.xrTable1,
            this.sbrptElementoDetail});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Mes", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WholePage;
            this.GroupHeader1.HeightF = 83.00001F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
            // 
            // sbrptElementoHder
            // 
            this.sbrptElementoHder.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 25F);
            this.sbrptElementoHder.Name = "sbrptElementoHder";
            this.sbrptElementoHder.ReportSource = new BPMO.SDNI.Flota.RPT.RentaDiariaGeneralMesSucursalElementoRPT();
            this.sbrptElementoHder.SizeF = new System.Drawing.SizeF(1074F, 35F);
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.Transparent;
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1074F, 25F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrtMesAnio,
            this.xrtMesNombre});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Total Flota";
            this.xrTableCell1.Weight = 0.27932960893854752D;
            // 
            // xrtMesAnio
            // 
            this.xrtMesAnio.Name = "xrtMesAnio";
            this.xrtMesAnio.Weight = 2.1229050279329611D;
            this.xrtMesAnio.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrtMesAnio_BeforePrint);
            // 
            // xrtMesNombre
            // 
            this.xrtMesNombre.Name = "xrtMesNombre";
            this.xrtMesNombre.Text = "xrtMesNombre";
            this.xrtMesNombre.Weight = 0.5977653631284916D;
            this.xrtMesNombre.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrtMesNombre_BeforePrint);
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // RentaDiariaGeneralMesSucursalRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1});
            this.DataMember = "SubTotalXModelo";
            this.DataSource = this.reporteRDSucursalDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(14, 12, 0, 58);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "12.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrtMesAnio;
        private DevExpress.XtraReports.UI.XRTableCell xrtMesNombre;
        private Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.XRSubreport sbrptElementoHeader;
        private DevExpress.XtraReports.UI.XRSubreport sbrptElementoHder;
        private DevExpress.XtraReports.UI.XRSubreport sbrptElementoDetail;
    }
}
