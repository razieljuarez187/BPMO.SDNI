namespace BPMO.SDNI.Mantenimiento.RPT
{
    partial class ReporteAuditoriasRealizadasRPT
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrsTecnicos = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.pbLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.tblTituloEtiquetas = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.reporteAuditoriasRealizadasDS = new BPMO.SDNI.Mantenimiento.Reportes.DA.ReporteAuditoriasRealizadasDS();
            this.gfSucursal = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrtPorcentajesSucursal = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtLabelAuditoriasRealizadas = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalAuditoriasSucursal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtLabelNumeroTecnicos = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTecnicosAuditados = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtPromedioTecnico = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtPromedioTecnicos = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtLabelPromedioCalificacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtPromedioCalificacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.ghSucursal = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrlJefeOperaciones = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlTitleJefe = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlNombreSucursal = new DevExpress.XtraReports.UI.XRLabel();
            this.rfGeneral = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalGeneralAuditorias = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalGeneralTecnicos = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalGeneralPromedioAuditorias = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotalGeneralPromedioCalificaciones = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabelTituloTotal = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteAuditoriasRealizadasDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtPorcentajesSucursal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrsTecnicos});
            this.Detail.HeightF = 23.95833F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SucursalID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrsTecnicos
            // 
            this.xrsTecnicos.Id = 0;
            this.xrsTecnicos.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 0F);
            this.xrsTecnicos.Name = "xrsTecnicos";
            this.xrsTecnicos.ReportSource = new BPMO.SDNI.Mantenimiento.RPT.ReporteAuditoriasRealizadasTecnicosRPT();
            this.xrsTecnicos.SizeF = new System.Drawing.SizeF(900F, 23F);
            this.xrsTecnicos.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrsTecnicos_BeforePrint);
            // 
            // TopMargin
            // 
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
            this.pbLogo,
            this.xrPageInfo2,
            this.xrPageInfo1,
            this.tblTituloEtiquetas});
            this.PageHeader.Name = "PageHeader";
            // 
            // pbLogo
            // 
            this.pbLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "ConfiguracionesSistema.UrlLogoEmpresa")});
            this.pbLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.SizeF = new System.Drawing.SizeF(159F, 100F);
            this.pbLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.pbLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.pbLogo_BeforePrint);
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy hh:mm:ss tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(729.5835F, 58.33333F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(170.4166F, 20.83333F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(729.5835F, 79.16666F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(170.4167F, 20.83334F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // tblTituloEtiquetas
            // 
            this.tblTituloEtiquetas.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTituloEtiquetas.LocationFloat = new DevExpress.Utils.PointFloat(273.5833F, 34.79166F);
            this.tblTituloEtiquetas.Name = "tblTituloEtiquetas";
            this.tblTituloEtiquetas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.tblTituloEtiquetas.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow14});
            this.tblTituloEtiquetas.SizeF = new System.Drawing.SizeF(375.0831F, 62.5F);
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
            this.xrTableCell17.Text = "REPORTE DE AUDITORÍAS REALIZADAS";
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
            // reporteAuditoriasRealizadasDS
            // 
            this.reporteAuditoriasRealizadasDS.DataSetName = "ReporteAuditoriasRealizadasDS";
            this.reporteAuditoriasRealizadasDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gfSucursal
            // 
            this.gfSucursal.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtPorcentajesSucursal});
            this.gfSucursal.HeightF = 112.5F;
            this.gfSucursal.Name = "gfSucursal";
            this.gfSucursal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gfSucursal_BeforePrint);
            // 
            // xrtPorcentajesSucursal
            // 
            this.xrtPorcentajesSucursal.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 10.00001F);
            this.xrtPorcentajesSucursal.Name = "xrtPorcentajesSucursal";
            this.xrtPorcentajesSucursal.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow4});
            this.xrtPorcentajesSucursal.SizeF = new System.Drawing.SizeF(300F, 100F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtLabelAuditoriasRealizadas,
            this.xrtTotalAuditoriasSucursal});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrtLabelAuditoriasRealizadas
            // 
            this.xrtLabelAuditoriasRealizadas.Name = "xrtLabelAuditoriasRealizadas";
            this.xrtLabelAuditoriasRealizadas.StylePriority.UseTextAlignment = false;
            this.xrtLabelAuditoriasRealizadas.Text = "Auditorías Realizadas";
            this.xrtLabelAuditoriasRealizadas.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtLabelAuditoriasRealizadas.Weight = 1.3333333333333333D;
            // 
            // xrtTotalAuditoriasSucursal
            // 
            this.xrtTotalAuditoriasSucursal.Name = "xrtTotalAuditoriasSucursal";
            this.xrtTotalAuditoriasSucursal.StylePriority.UseTextAlignment = false;
            this.xrtTotalAuditoriasSucursal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtTotalAuditoriasSucursal.Weight = 0.66666666666666674D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtLabelNumeroTecnicos,
            this.xrtTecnicosAuditados});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrtLabelNumeroTecnicos
            // 
            this.xrtLabelNumeroTecnicos.Name = "xrtLabelNumeroTecnicos";
            this.xrtLabelNumeroTecnicos.StylePriority.UseTextAlignment = false;
            this.xrtLabelNumeroTecnicos.Text = "Numero Tecnicos Auditados";
            this.xrtLabelNumeroTecnicos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtLabelNumeroTecnicos.Weight = 1.3333333333333333D;
            // 
            // xrtTecnicosAuditados
            // 
            this.xrtTecnicosAuditados.Name = "xrtTecnicosAuditados";
            this.xrtTecnicosAuditados.StylePriority.UseTextAlignment = false;
            this.xrtTecnicosAuditados.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtTecnicosAuditados.Weight = 0.66666666666666674D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtPromedioTecnico,
            this.xrtPromedioTecnicos});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrtPromedioTecnico
            // 
            this.xrtPromedioTecnico.Name = "xrtPromedioTecnico";
            this.xrtPromedioTecnico.StylePriority.UseTextAlignment = false;
            this.xrtPromedioTecnico.Text = "Promedio Auditorías por Técnico";
            this.xrtPromedioTecnico.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtPromedioTecnico.Weight = 1.3333333333333333D;
            // 
            // xrtPromedioTecnicos
            // 
            this.xrtPromedioTecnicos.Name = "xrtPromedioTecnicos";
            this.xrtPromedioTecnicos.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:0%}";
            this.xrtPromedioTecnicos.Summary = xrSummary1;
            this.xrtPromedioTecnicos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtPromedioTecnicos.Weight = 0.66666666666666674D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtLabelPromedioCalificacion,
            this.xrtPromedioCalificacion});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrtLabelPromedioCalificacion
            // 
            this.xrtLabelPromedioCalificacion.Name = "xrtLabelPromedioCalificacion";
            this.xrtLabelPromedioCalificacion.StylePriority.UseTextAlignment = false;
            this.xrtLabelPromedioCalificacion.Text = "Promedio Calificación General";
            this.xrtLabelPromedioCalificacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtLabelPromedioCalificacion.Weight = 1.3333333333333333D;
            // 
            // xrtPromedioCalificacion
            // 
            this.xrtPromedioCalificacion.Name = "xrtPromedioCalificacion";
            this.xrtPromedioCalificacion.StylePriority.UseTextAlignment = false;
            this.xrtPromedioCalificacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtPromedioCalificacion.Weight = 0.66666666666666674D;
            // 
            // ghSucursal
            // 
            this.ghSucursal.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlJefeOperaciones,
            this.xrlTitleJefe,
            this.xrlNombreSucursal});
            this.ghSucursal.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SucursalID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.ghSucursal.HeightF = 75F;
            this.ghSucursal.Name = "ghSucursal";
            this.ghSucursal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ghSucursal_BeforePrint);
            // 
            // xrlJefeOperaciones
            // 
            this.xrlJefeOperaciones.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursales.Sucursales_JefeOperaciones.JefeTaller")});
            this.xrlJefeOperaciones.LocationFloat = new DevExpress.Utils.PointFloat(125F, 48.25001F);
            this.xrlJefeOperaciones.Name = "xrlJefeOperaciones";
            this.xrlJefeOperaciones.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlJefeOperaciones.SizeF = new System.Drawing.SizeF(423.9583F, 26.125F);
            this.xrlJefeOperaciones.StylePriority.UseTextAlignment = false;
            this.xrlJefeOperaciones.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlTitleJefe
            // 
            this.xrlTitleJefe.LocationFloat = new DevExpress.Utils.PointFloat(0F, 48.25001F);
            this.xrlTitleJefe.Name = "xrlTitleJefe";
            this.xrlTitleJefe.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlTitleJefe.SizeF = new System.Drawing.SizeF(125F, 26.125F);
            this.xrlTitleJefe.StylePriority.UseTextAlignment = false;
            this.xrlTitleJefe.Text = "Jefe Operaciones:";
            this.xrlTitleJefe.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlNombreSucursal
            // 
            this.xrlNombreSucursal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlNombreSucursal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursales.NombreSucursal")});
            this.xrlNombreSucursal.ForeColor = System.Drawing.Color.White;
            this.xrlNombreSucursal.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 10.00001F);
            this.xrlNombreSucursal.Name = "xrlNombreSucursal";
            this.xrlNombreSucursal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNombreSucursal.SizeF = new System.Drawing.SizeF(900.0001F, 23F);
            this.xrlNombreSucursal.StylePriority.UseBackColor = false;
            this.xrlNombreSucursal.StylePriority.UseForeColor = false;
            this.xrlNombreSucursal.StylePriority.UseTextAlignment = false;
            this.xrlNombreSucursal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // rfGeneral
            // 
            this.rfGeneral.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrLabelTituloTotal});
            this.rfGeneral.HeightF = 139.5833F;
            this.rfGeneral.Name = "rfGeneral";
            this.rfGeneral.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rfGeneral_BeforePrint);
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 37.5F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5,
            this.xrTableRow6,
            this.xrTableRow7,
            this.xrTableRow8});
            this.xrTable1.SizeF = new System.Drawing.SizeF(300F, 100F);
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrtTotalGeneralAuditorias});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Auditorías Realizadas";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell1.Weight = 1.3333333333333333D;
            // 
            // xrtTotalGeneralAuditorias
            // 
            this.xrtTotalGeneralAuditorias.Name = "xrtTotalGeneralAuditorias";
            this.xrtTotalGeneralAuditorias.StylePriority.UseTextAlignment = false;
            this.xrtTotalGeneralAuditorias.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtTotalGeneralAuditorias.Weight = 0.66666666666666674D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrtTotalGeneralTecnicos});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Numero Tecnicos Auditados";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.Weight = 1.3333333333333333D;
            // 
            // xrtTotalGeneralTecnicos
            // 
            this.xrtTotalGeneralTecnicos.Name = "xrtTotalGeneralTecnicos";
            this.xrtTotalGeneralTecnicos.StylePriority.UseTextAlignment = false;
            this.xrtTotalGeneralTecnicos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtTotalGeneralTecnicos.Weight = 0.66666666666666674D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrtTotalGeneralPromedioAuditorias});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Promedio Auditorías por Técnico";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell5.Weight = 1.3333333333333333D;
            // 
            // xrtTotalGeneralPromedioAuditorias
            // 
            this.xrtTotalGeneralPromedioAuditorias.Name = "xrtTotalGeneralPromedioAuditorias";
            this.xrtTotalGeneralPromedioAuditorias.StylePriority.UseTextAlignment = false;
            this.xrtTotalGeneralPromedioAuditorias.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtTotalGeneralPromedioAuditorias.Weight = 0.66666666666666674D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrtTotalGeneralPromedioCalificaciones});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Promedio Calificación General";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 1.3333333333333333D;
            // 
            // xrtTotalGeneralPromedioCalificaciones
            // 
            this.xrtTotalGeneralPromedioCalificaciones.Name = "xrtTotalGeneralPromedioCalificaciones";
            this.xrtTotalGeneralPromedioCalificaciones.StylePriority.UseTextAlignment = false;
            this.xrtTotalGeneralPromedioCalificaciones.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrtTotalGeneralPromedioCalificaciones.Weight = 0.66666666666666674D;
            // 
            // xrLabelTituloTotal
            // 
            this.xrLabelTituloTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrLabelTituloTotal.ForeColor = System.Drawing.Color.White;
            this.xrLabelTituloTotal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabelTituloTotal.Name = "xrLabelTituloTotal";
            this.xrLabelTituloTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabelTituloTotal.SizeF = new System.Drawing.SizeF(900.0001F, 23F);
            this.xrLabelTituloTotal.StylePriority.UseBackColor = false;
            this.xrLabelTituloTotal.StylePriority.UseForeColor = false;
            this.xrLabelTituloTotal.StylePriority.UseTextAlignment = false;
            this.xrLabelTituloTotal.Text = "TOTAL GENERAL";
            this.xrLabelTituloTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ReporteAuditoriasRealizadasRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.gfSucursal,
            this.ghSucursal,
            this.rfGeneral});
            this.DataMember = "Sucursales";
            this.DataSource = this.reporteAuditoriasRealizadasDS;
            this.Landscape = true;
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.tblTituloEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteAuditoriasRealizadasDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtPorcentajesSucursal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable tblTituloEtiquetas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRPictureBox pbLogo;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private Reportes.DA.ReporteAuditoriasRealizadasDS reporteAuditoriasRealizadasDS;
        private DevExpress.XtraReports.UI.GroupFooterBand gfSucursal;
        private DevExpress.XtraReports.UI.GroupHeaderBand ghSucursal;
        private DevExpress.XtraReports.UI.XRLabel xrlNombreSucursal;
        private DevExpress.XtraReports.UI.XRLabel xrlJefeOperaciones;
        private DevExpress.XtraReports.UI.XRLabel xrlTitleJefe;
        private DevExpress.XtraReports.UI.XRSubreport xrsTecnicos;
        private DevExpress.XtraReports.UI.XRTable xrtPorcentajesSucursal;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrtLabelAuditoriasRealizadas;
        private DevExpress.XtraReports.UI.XRTableCell xrtTotalAuditoriasSucursal;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrtLabelNumeroTecnicos;
        private DevExpress.XtraReports.UI.XRTableCell xrtTecnicosAuditados;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrtPromedioTecnico;
        private DevExpress.XtraReports.UI.XRTableCell xrtPromedioTecnicos;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrtLabelPromedioCalificacion;
        private DevExpress.XtraReports.UI.XRTableCell xrtPromedioCalificacion;
        private DevExpress.XtraReports.UI.ReportFooterBand rfGeneral;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrtTotalGeneralAuditorias;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrtTotalGeneralTecnicos;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrtTotalGeneralPromedioAuditorias;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrtTotalGeneralPromedioCalificaciones;
        private DevExpress.XtraReports.UI.XRLabel xrLabelTituloTotal;
    }
}
