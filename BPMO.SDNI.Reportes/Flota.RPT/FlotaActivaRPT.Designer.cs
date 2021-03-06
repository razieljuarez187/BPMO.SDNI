﻿namespace BPMO.SDNI.Flota.RPT {
    partial class FlotaActivaRPT {
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.flotaActivaSucursalesRPT = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.flotaActivaRDDS = new BPMO.SDNI.Flota.Reportes.DA.FlotaActivaRDDS();
            this.grpHrdModelo = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.tblGrupoModeloTitulo = new DevExpress.XtraReports.UI.XRTable();
            this.trGrupoModeloTitulo = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcGrupoModeloTitulo = new DevExpress.XtraReports.UI.XRTableCell();
            this.grpftModelo = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.tblTotalesPorModelo = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tclblTotalModelo = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcTotalModelo = new DevExpress.XtraReports.UI.XRTableCell();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.flotaActivaSucursalTotalesRPT = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcTotalGeneral = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcTotalRefrigeradosGeneral = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tclblTotalSecosModelo = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flotaActivaRDDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblGrupoModeloTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTotalesPorModelo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.flotaActivaSucursalesRPT});
            this.Detail.HeightF = 37.41665F;
            this.Detail.MultiColumn.ColumnCount = 3;
            this.Detail.MultiColumn.ColumnSpacing = 2F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // flotaActivaSucursalesRPT
            // 
            this.flotaActivaSucursalesRPT.Id = 0;
            this.flotaActivaSucursalesRPT.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.00001F);
            this.flotaActivaSucursalesRPT.Name = "flotaActivaSucursalesRPT";
            this.flotaActivaSucursalesRPT.ReportSource = new BPMO.SDNI.Flota.RPT.FlotaActivaSucursalesRPT();
            this.flotaActivaSucursalesRPT.SizeF = new System.Drawing.SizeF(900F, 23F);
            this.flotaActivaSucursalesRPT.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.flotaActivaSucursalesRPT_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 48F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // pbLogo
            // 
            this.pbLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "ConfiguracionModulo.URLLogoEmpresa")});
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // tblTituloEtiquetas
            // 
            this.tblTituloEtiquetas.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(159F, 43.75F);
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ConfiguracionModulo.NombreCliente")});
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
            this.xrTableCell5.Text = "FLOTA ACTIVA";
            this.xrTableCell5.Weight = 1D;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 49F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // flotaActivaRDDS
            // 
            this.flotaActivaRDDS.DataSetName = "FlotaActivaRDDS";
            this.flotaActivaRDDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // grpHrdModelo
            // 
            this.grpHrdModelo.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblGrupoModeloTitulo});
            this.grpHrdModelo.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Nombre", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ModeloID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.grpHrdModelo.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WholePage;
            this.grpHrdModelo.HeightF = 33.33333F;
            this.grpHrdModelo.Name = "grpHrdModelo";
            // 
            // tblGrupoModeloTitulo
            // 
            this.tblGrupoModeloTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.tblGrupoModeloTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tblGrupoModeloTitulo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 8.333333F);
            this.tblGrupoModeloTitulo.Name = "tblGrupoModeloTitulo";
            this.tblGrupoModeloTitulo.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.trGrupoModeloTitulo});
            this.tblGrupoModeloTitulo.SizeF = new System.Drawing.SizeF(900F, 25F);
            this.tblGrupoModeloTitulo.StylePriority.UseBackColor = false;
            this.tblGrupoModeloTitulo.StylePriority.UseBorders = false;
            // 
            // trGrupoModeloTitulo
            // 
            this.trGrupoModeloTitulo.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcGrupoModeloTitulo});
            this.trGrupoModeloTitulo.Name = "trGrupoModeloTitulo";
            this.trGrupoModeloTitulo.Weight = 1D;
            // 
            // tcGrupoModeloTitulo
            // 
            this.tcGrupoModeloTitulo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelo.Nombre")});
            this.tcGrupoModeloTitulo.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcGrupoModeloTitulo.ForeColor = System.Drawing.Color.White;
            this.tcGrupoModeloTitulo.Name = "tcGrupoModeloTitulo";
            this.tcGrupoModeloTitulo.StylePriority.UseBackColor = false;
            this.tcGrupoModeloTitulo.StylePriority.UseBorders = false;
            this.tcGrupoModeloTitulo.StylePriority.UseFont = false;
            this.tcGrupoModeloTitulo.StylePriority.UseForeColor = false;
            this.tcGrupoModeloTitulo.StylePriority.UseTextAlignment = false;
            this.tcGrupoModeloTitulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tcGrupoModeloTitulo.Weight = 3D;
            // 
            // grpftModelo
            // 
            this.grpftModelo.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblTotalesPorModelo});
            this.grpftModelo.HeightF = 34.375F;
            this.grpftModelo.Name = "grpftModelo";
            // 
            // tblTotalesPorModelo
            // 
            this.tblTotalesPorModelo.BackColor = System.Drawing.Color.LightGray;
            this.tblTotalesPorModelo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tblTotalesPorModelo.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 0F);
            this.tblTotalesPorModelo.Name = "tblTotalesPorModelo";
            this.tblTotalesPorModelo.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9});
            this.tblTotalesPorModelo.SizeF = new System.Drawing.SizeF(899.9999F, 25F);
            this.tblTotalesPorModelo.StylePriority.UseBackColor = false;
            this.tblTotalesPorModelo.StylePriority.UseBorders = false;
            this.tblTotalesPorModelo.StylePriority.UseTextAlignment = false;
            this.tblTotalesPorModelo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tclblTotalModelo,
            this.tcTotalModelo,
            this.tclblTotalSecosModelo});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // tclblTotalModelo
            // 
            this.tclblTotalModelo.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tclblTotalModelo.Name = "tclblTotalModelo";
            this.tclblTotalModelo.StylePriority.UseFont = false;
            this.tclblTotalModelo.Text = "Total:";
            this.tclblTotalModelo.Weight = 0.5D;
            // 
            // tcTotalModelo
            // 
            this.tcTotalModelo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelo.ModeloID")});
            this.tcTotalModelo.Name = "tcTotalModelo";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.tcTotalModelo.Summary = xrSummary1;
            this.tcTotalModelo.Text = "tcTotalModelo";
            this.tcTotalModelo.Weight = 0.5D;
            this.tcTotalModelo.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.tcTotalModelo_SummaryGetResult);
            // 
            // formattingRule1
            // 
            this.formattingRule1.Name = "formattingRule1";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.pbLogo,
            this.tblTituloEtiquetas,
            this.xrPageInfo1});
            this.PageHeader.HeightF = 101.0417F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(757.7084F, 53.99998F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(757.7084F, 76.99998F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(142.2917F, 23F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            this.PageFooter.Name = "PageFooter";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.flotaActivaSucursalTotalesRPT,
            this.xrTable2,
            this.xrTable1});
            this.ReportFooter.HeightF = 133.3333F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // flotaActivaSucursalTotalesRPT
            // 
            this.flotaActivaSucursalTotalesRPT.Id = 0;
            this.flotaActivaSucursalTotalesRPT.LocationFloat = new DevExpress.Utils.PointFloat(0F, 47.87499F);
            this.flotaActivaSucursalTotalesRPT.Name = "flotaActivaSucursalTotalesRPT";
            this.flotaActivaSucursalTotalesRPT.ReportSource = new BPMO.SDNI.Flota.RPT.FlotaActivaSucursalTotalesRPT();
            this.flotaActivaSucursalTotalesRPT.SizeF = new System.Drawing.SizeF(900.0001F, 23F);
            this.flotaActivaSucursalTotalesRPT.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.flotaActivaSucursalTotalesRPT_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.BackColor = System.Drawing.Color.LightGray;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 83.87499F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable2.SizeF = new System.Drawing.SizeF(899.9999F, 25F);
            this.xrTable2.StylePriority.UseBackColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.tcTotalGeneral,
            this.tcTotalRefrigeradosGeneral});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Text = "Total:";
            this.xrTableCell2.Weight = 0.5D;
            // 
            // tcTotalGeneral
            // 
            this.tcTotalGeneral.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelo.ModeloID")});
            this.tcTotalGeneral.Name = "tcTotalGeneral";
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tcTotalGeneral.Summary = xrSummary2;
            this.tcTotalGeneral.Text = "tcTotalGeneral";
            this.tcTotalGeneral.Weight = 0.5D;
            this.tcTotalGeneral.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.tcTotalGeneral_SummaryGetResult);
            // 
            // tcTotalRefrigeradosGeneral
            // 
            this.tcTotalRefrigeradosGeneral.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelo.ModeloID")});
            this.tcTotalRefrigeradosGeneral.ForeColor = System.Drawing.Color.LightGray;
            this.tcTotalRefrigeradosGeneral.Name = "tcTotalRefrigeradosGeneral";
            this.tcTotalRefrigeradosGeneral.StylePriority.UseForeColor = false;
            xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tcTotalRefrigeradosGeneral.Summary = xrSummary3;
            this.tcTotalRefrigeradosGeneral.Text = "tcTotalRefrigeradosGeneral";
            this.tcTotalRefrigeradosGeneral.Weight = 2D;
            this.tcTotalRefrigeradosGeneral.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.tcTotalRefrigeradosGeneral_SummaryGetResult);
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(6.612144E-05F, 10F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(900F, 25F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.ForeColor = System.Drawing.Color.White;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Flota Total";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 3D;
            // 
            // tclblTotalSecosModelo
            // 
            this.tclblTotalSecosModelo.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tclblTotalSecosModelo.Name = "tclblTotalSecosModelo";
            this.tclblTotalSecosModelo.StylePriority.UseFont = false;
            this.tclblTotalSecosModelo.Weight = 2D;
            // 
            // FlotaActivaRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.grpHrdModelo,
            this.grpftModelo,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter});
            this.DataMember = "Modelo";
            this.DataSource = this.flotaActivaRDDS;
            this.DisplayName = "Flota Activa RD Registrados";
            this.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 48, 49);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flotaActivaRDDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblGrupoModeloTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTotalesPorModelo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.FlotaActivaRDDS flotaActivaRDDS;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.GroupHeaderBand grpHrdModelo;
        private DevExpress.XtraReports.UI.XRTable tblGrupoModeloTitulo;
        private DevExpress.XtraReports.UI.XRTableRow trGrupoModeloTitulo;
        private DevExpress.XtraReports.UI.XRTableCell tcGrupoModeloTitulo;
        private DevExpress.XtraReports.UI.GroupFooterBand grpftModelo;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable tblTotalesPorModelo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow9;
        private DevExpress.XtraReports.UI.XRTableCell tclblTotalModelo;
        private DevExpress.XtraReports.UI.XRTableCell tcTotalModelo;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.UI.XRSubreport flotaActivaSucursalesRPT;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tcTotalGeneral;
        private DevExpress.XtraReports.UI.XRTableCell tcTotalRefrigeradosGeneral;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRSubreport flotaActivaSucursalTotalesRPT;
        private DevExpress.XtraReports.UI.XRTableCell tclblTotalSecosModelo;
    }
}
