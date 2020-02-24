namespace BPMO.SDNI.Flota.RPT
{
    partial class AniejamientoFlotaRPT
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
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrsrpUnidad = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtEtiquetaReporte = new DevExpress.XtraReports.UI.XRTableCell();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrtAnioHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtCellAnio = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtNombreModelo = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtcNombreModelo = new DevExpress.XtraReports.UI.XRTableCell();
            this.aniejamientoFlotaDS = new BPMO.SDNI.Flota.Reportes.DA.AniejamientoFlotaDS();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrtCountUnidades = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtCellCountUnidades = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrtTotalUnidades = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtLabelTotalUnidades = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtCellTotalUnidades = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtAnioHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtNombreModelo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aniejamientoFlotaDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtCountUnidades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtTotalUnidades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrsrpUnidad});
            this.Detail.HeightF = 15F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrsrpUnidad
            // 
            this.xrsrpUnidad.Id = 0;
            this.xrsrpUnidad.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrsrpUnidad.Name = "xrsrpUnidad";
            this.xrsrpUnidad.ReportSource = new BPMO.SDNI.Flota.RPT.AniejamientoFlotaUnidadRPT();
            this.xrsrpUnidad.SizeF = new System.Drawing.SizeF(1076F, 15F);
            this.xrsrpUnidad.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrsrpUnidad_BeforePrint);
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
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPageInfo2,
            this.tblTituloEtiquetas,
            this.pbLogo});
            this.PageHeader.HeightF = 105.2083F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(932.2082F, 79.16667F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(142.2917F, 20.83334F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(932.2084F, 58.33334F);
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
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(176.8339F, 37.50001F);
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
            this.xrTableCell17.Text = "REPORTE DE AÑEJAMIENTO DE FLOTA";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell17.Weight = 0.99131829478095046D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtEtiquetaReporte});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrtEtiquetaReporte
            // 
            this.xrtEtiquetaReporte.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtEtiquetaReporte.Name = "xrtEtiquetaReporte";
            this.xrtEtiquetaReporte.StylePriority.UseFont = false;
            this.xrtEtiquetaReporte.StylePriority.UseTextAlignment = false;
            this.xrtEtiquetaReporte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrtEtiquetaReporte.Weight = 0.99131829478095057D;
            // 
            // pbLogo
            // 
            this.pbLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "ConfiguracionesSistema.UrlLogoEmpresa")});
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(1.5F, 0F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtAnioHeader,
            this.xrtNombreModelo});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ModeloNombre", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ModeloID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 40.00002F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.RepeatEveryPage = true;
            this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
            // 
            // xrtAnioHeader
            // 
            this.xrtAnioHeader.BackColor = System.Drawing.Color.Transparent;
            this.xrtAnioHeader.BorderColor = System.Drawing.Color.Black;
            this.xrtAnioHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtAnioHeader.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtAnioHeader.ForeColor = System.Drawing.Color.Black;
            this.xrtAnioHeader.LocationFloat = new DevExpress.Utils.PointFloat(326.0001F, 20.00001F);
            this.xrtAnioHeader.Name = "xrtAnioHeader";
            this.xrtAnioHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrtAnioHeader.SizeF = new System.Drawing.SizeF(750F, 20F);
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
            this.xrtCellAnio.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtCellAnio.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtCellAnio.ForeColor = System.Drawing.Color.Black;
            this.xrtCellAnio.Name = "xrtCellAnio";
            this.xrtCellAnio.StylePriority.UseBackColor = false;
            this.xrtCellAnio.StylePriority.UseBorders = false;
            this.xrtCellAnio.StylePriority.UseFont = false;
            this.xrtCellAnio.StylePriority.UseForeColor = false;
            this.xrtCellAnio.StylePriority.UseTextAlignment = false;
            this.xrtCellAnio.Text = "xrtCellAnio";
            this.xrtCellAnio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrtCellAnio.Weight = 3D;
            // 
            // xrtNombreModelo
            // 
            this.xrtNombreModelo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrtNombreModelo.BorderColor = System.Drawing.Color.Black;
            this.xrtNombreModelo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtNombreModelo.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrtNombreModelo.ForeColor = System.Drawing.Color.White;
            this.xrtNombreModelo.LocationFloat = new DevExpress.Utils.PointFloat(326F, 0F);
            this.xrtNombreModelo.Name = "xrtNombreModelo";
            this.xrtNombreModelo.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrtNombreModelo.SizeF = new System.Drawing.SizeF(750F, 20F);
            this.xrtNombreModelo.StylePriority.UseBackColor = false;
            this.xrtNombreModelo.StylePriority.UseBorderColor = false;
            this.xrtNombreModelo.StylePriority.UseBorders = false;
            this.xrtNombreModelo.StylePriority.UseFont = false;
            this.xrtNombreModelo.StylePriority.UseForeColor = false;
            this.xrtNombreModelo.StylePriority.UseTextAlignment = false;
            this.xrtNombreModelo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtcNombreModelo});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrtcNombreModelo
            // 
            this.xrtcNombreModelo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelos.ModeloNombre")});
            this.xrtcNombreModelo.Name = "xrtcNombreModelo";
            this.xrtcNombreModelo.StylePriority.UseTextAlignment = false;
            this.xrtcNombreModelo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrtcNombreModelo.Weight = 2.8378378378378377D;
            // 
            // aniejamientoFlotaDS
            // 
            this.aniejamientoFlotaDS.DataSetName = "AniejamientoFlotaDS";
            this.aniejamientoFlotaDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.ExcludeSchema;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtCountUnidades});
            this.GroupFooter1.HeightF = 15F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrtCountUnidades
            // 
            this.xrtCountUnidades.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrtCountUnidades.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtCountUnidades.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold);
            this.xrtCountUnidades.ForeColor = System.Drawing.Color.White;
            this.xrtCountUnidades.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrtCountUnidades.Name = "xrtCountUnidades";
            this.xrtCountUnidades.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrtCountUnidades.SizeF = new System.Drawing.SizeF(326F, 15F);
            this.xrtCountUnidades.StylePriority.UseBackColor = false;
            this.xrtCountUnidades.StylePriority.UseBorders = false;
            this.xrtCountUnidades.StylePriority.UseFont = false;
            this.xrtCountUnidades.StylePriority.UseForeColor = false;
            this.xrtCountUnidades.StylePriority.UseTextAlignment = false;
            this.xrtCountUnidades.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtCellCountUnidades});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrtCellCountUnidades
            // 
            this.xrtCellCountUnidades.Name = "xrtCellCountUnidades";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            this.xrtCellCountUnidades.Summary = xrSummary1;
            this.xrtCellCountUnidades.Weight = 3D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtTotalUnidades});
            this.ReportFooter.HeightF = 15F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrtTotalUnidades
            // 
            this.xrtTotalUnidades.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrtTotalUnidades.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtTotalUnidades.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold);
            this.xrtTotalUnidades.ForeColor = System.Drawing.Color.White;
            this.xrtTotalUnidades.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrtTotalUnidades.Name = "xrtTotalUnidades";
            this.xrtTotalUnidades.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrtTotalUnidades.SizeF = new System.Drawing.SizeF(326F, 15F);
            this.xrtTotalUnidades.StylePriority.UseBackColor = false;
            this.xrtTotalUnidades.StylePriority.UseBorders = false;
            this.xrtTotalUnidades.StylePriority.UseFont = false;
            this.xrtTotalUnidades.StylePriority.UseForeColor = false;
            this.xrtTotalUnidades.StylePriority.UseTextAlignment = false;
            this.xrtTotalUnidades.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtLabelTotalUnidades,
            this.xrtCellTotalUnidades});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrtLabelTotalUnidades
            // 
            this.xrtLabelTotalUnidades.Name = "xrtLabelTotalUnidades";
            this.xrtLabelTotalUnidades.Text = "Total de Unidades: ";
            this.xrtLabelTotalUnidades.Weight = 1D;
            // 
            // xrtCellTotalUnidades
            // 
            this.xrtCellTotalUnidades.Name = "xrtCellTotalUnidades";
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            this.xrtCellTotalUnidades.Summary = xrSummary2;
            this.xrtCellTotalUnidades.Weight = 2D;
            this.xrtCellTotalUnidades.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrtCellTotalUnidades_BeforePrint);
            // 
            // AniejamientoFlotaRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupFooter1,
            this.ReportFooter});
            this.DataMember = "Modelos";
            this.DataSource = this.aniejamientoFlotaDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(12, 12, 12, 100);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.AniejamientoFlotaRPT_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtAnioHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtNombreModelo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aniejamientoFlotaDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtCountUnidades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtTotalUnidades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow14;
        private DevExpress.XtraReports.UI.XRTableCell xrtEtiquetaReporte;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private Reportes.DA.AniejamientoFlotaDS aniejamientoFlotaDS;
        private DevExpress.XtraReports.UI.XRTable xrtAnioHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellAnio;
        private DevExpress.XtraReports.UI.XRTable xrtNombreModelo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrtcNombreModelo;
        private DevExpress.XtraReports.UI.XRSubreport xrsrpUnidad;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable xrtCountUnidades;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellCountUnidades;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRTable xrtTotalUnidades;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrtLabelTotalUnidades;
        private DevExpress.XtraReports.UI.XRTableCell xrtCellTotalUnidades;
    }
}
