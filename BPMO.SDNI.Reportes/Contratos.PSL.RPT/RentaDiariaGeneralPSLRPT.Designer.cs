namespace BPMO.SDNI.Contratos.PSL.RPT
{
    partial class RentaDiariaGeneralPSLRPT
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
            this.dtlSucursal = new DevExpress.XtraReports.UI.DetailBand();
            this.ReporteSucursal = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteRDSucursalDS = new BPMO.SDNI.Contratos.PSL.Reportes.DA.RentasSucursalDS();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.grpHSucursal = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrlNombreSucursal = new DevExpress.XtraReports.UI.XRLabel();
            this.grpFGeneral = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.ReporteGeneral = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // dtlSucursal
            // 
            this.dtlSucursal.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.ReporteSucursal});
            this.dtlSucursal.HeightF = 22F;
            this.dtlSucursal.Name = "dtlSucursal";
            this.dtlSucursal.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.dtlSucursal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.dtlSucursal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.dtlSucursal_BeforePrint);
            // 
            // ReporteSucursal
            // 
            this.ReporteSucursal.Id = 0;
            this.ReporteSucursal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.ReporteSucursal.Name = "ReporteSucursal";
            this.ReporteSucursal.ReportSource = new BPMO.SDNI.Flota.RPT.RentaDiariaGeneralMesSucursalRPT();
            this.ReporteSucursal.SizeF = new System.Drawing.SizeF(1076F, 22F);
            this.ReporteSucursal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ReporteSucursal_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 12.5F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pbLogo,
            this.tblTituloEtiquetas,
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.PageHeader.HeightF = 112.4999F;
            this.PageHeader.Name = "PageHeader";
            // 
            // pbLogo
            // 
            this.pbLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "ConfiguracionesSistema.UrlLogoEmpresa")});
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(2.999973F, 2.000007F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // tblTituloEtiquetas
            // 
            this.tblTituloEtiquetas.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(178.3339F, 39.50002F);
            this.tblTituloEtiquetas.Name = "tblTituloEtiquetas";
            this.tblTituloEtiquetas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.tblTituloEtiquetas.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow14});
            this.tblTituloEtiquetas.SizeF = new System.Drawing.SizeF(743.8332F, 62.5F);
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ConfiguracionesSistema.NombreCliente")});
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Text = "xrTableCell9";
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
            this.xrTableCell17.Text = "REPORTE DE RENTA DIARIA GENERAL";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell17.Weight = 0.99131829478095046D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.Weight = 0.99131829478095057D;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(933.7082F, 81.16668F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(142.2917F, 20.83334F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(933.7084F, 60.33335F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(142.2916F, 20.83333F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // grpHSucursal
            // 
            this.grpHSucursal.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlNombreSucursal});
            this.grpHSucursal.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SucursalID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.grpHSucursal.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.grpHSucursal.HeightF = 37.50002F;
            this.grpHSucursal.Name = "grpHSucursal";
            // 
            // xrlNombreSucursal
            // 
            this.xrlNombreSucursal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlNombreSucursal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursales.Nombre")});
            this.xrlNombreSucursal.ForeColor = System.Drawing.Color.Silver;
            this.xrlNombreSucursal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.500008F);
            this.xrlNombreSucursal.Name = "xrlNombreSucursal";
            this.xrlNombreSucursal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNombreSucursal.SizeF = new System.Drawing.SizeF(1076F, 25F);
            this.xrlNombreSucursal.StylePriority.UseBackColor = false;
            this.xrlNombreSucursal.StylePriority.UseForeColor = false;
            this.xrlNombreSucursal.StylePriority.UseTextAlignment = false;
            this.xrlNombreSucursal.Text = "xrlNombreSucursal";
            this.xrlNombreSucursal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // grpFGeneral
            // 
            this.grpFGeneral.Expanded = false;
            this.grpFGeneral.HeightF = 36.45833F;
            this.grpFGeneral.Name = "grpFGeneral";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.ReporteGeneral});
            this.ReportFooter.HeightF = 23.95833F;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ReportFooter_BeforePrint);
            // 
            // ReporteGeneral
            // 
            this.ReporteGeneral.Id = 0;
            this.ReporteGeneral.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.ReporteGeneral.Name = "ReporteGeneral";
            this.ReporteGeneral.ReportSource = new BPMO.SDNI.Flota.RPT.RentaDiariaGeneralMesSucursalRPT();
            this.ReporteGeneral.SizeF = new System.Drawing.SizeF(1076F, 23F);
            this.ReporteGeneral.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ReporteGeneral_BeforePrint);
            // 
            // RentaDiariaGeneralRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.dtlSucursal,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.grpHSucursal,
            this.grpFGeneral,
            this.ReportFooter});
            this.DataMember = "Sucursales";
            this.DataSource = this.reporteRDSucursalDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(12, 12, 12, 100);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand dtlSucursal;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.RentasSucursalDS reporteRDSucursalDS;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.GroupHeaderBand grpHSucursal;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRSubreport ReporteSucursal;
        private DevExpress.XtraReports.UI.XRLabel xrlNombreSucursal;
        private DevExpress.XtraReports.UI.GroupFooterBand grpFGeneral;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRSubreport ReporteGeneral;
    }
}
