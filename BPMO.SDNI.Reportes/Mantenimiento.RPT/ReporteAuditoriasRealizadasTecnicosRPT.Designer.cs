namespace BPMO.SDNI.Mantenimiento.RPT
{
    partial class ReporteAuditoriasRealizadasTecnicosRPT
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
            this.dtlTecnicos = new DevExpress.XtraReports.UI.DetailBand();
            this.xrtDetail = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtcOrdenServicio = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtcFechaAuditoria = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtCalificacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtcObservaciones = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteAuditoriasRealizadasDS = new BPMO.SDNI.Mantenimiento.Reportes.DA.ReporteAuditoriasRealizadasDS();
            this.ghTecnicos = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrtHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrlTecnico = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlOrdenServicio = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlFechaAuditoria = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlCalificacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlObservaciones = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtcNombreTecnico = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.gfTecnicos = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrlPromedioAuditorias = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlLabelPromedioAuditorias = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlAuditoriasRealizadas = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlLabelAuditoriasRealizadas = new DevExpress.XtraReports.UI.XRLabel();
            this.PorcentajeCalificacion = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrtDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteAuditoriasRealizadasDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // dtlTecnicos
            // 
            this.dtlTecnicos.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtDetail});
            this.dtlTecnicos.HeightF = 25F;
            this.dtlTecnicos.Name = "dtlTecnicos";
            this.dtlTecnicos.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.dtlTecnicos.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TecnicoID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.dtlTecnicos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.dtlTecnicos.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.dtlTecnicos_BeforePrint);
            // 
            // xrtDetail
            // 
            this.xrtDetail.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrtDetail.Name = "xrtDetail";
            this.xrtDetail.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrtDetail.SizeF = new System.Drawing.SizeF(899.9999F, 25F);
            this.xrtDetail.StylePriority.UseTextAlignment = false;
            this.xrtDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrtcOrdenServicio,
            this.xrtcFechaAuditoria,
            this.xrtCalificacion,
            this.xrtcObservaciones});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell10.Weight = 0.66666674013492022D;
            // 
            // xrtcOrdenServicio
            // 
            this.xrtcOrdenServicio.Name = "xrtcOrdenServicio";
            this.xrtcOrdenServicio.Weight = 0.36666672487612911D;
            // 
            // xrtcFechaAuditoria
            // 
            this.xrtcFechaAuditoria.Name = "xrtcFechaAuditoria";
            this.xrtcFechaAuditoria.Weight = 0.50000007276183245D;
            // 
            // xrtCalificacion
            // 
            this.xrtCalificacion.Name = "xrtCalificacion";
            this.xrtCalificacion.StylePriority.UseTextAlignment = false;
            this.xrtCalificacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrtCalificacion.Weight = 0.26666669459402537D;
            // 
            // xrtcObservaciones
            // 
            this.xrtcObservaciones.Multiline = true;
            this.xrtcObservaciones.Name = "xrtcObservaciones";
            this.xrtcObservaciones.StylePriority.UseTextAlignment = false;
            this.xrtcObservaciones.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrtcObservaciones.Weight = 1.1999997676330927D;
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
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteAuditoriasRealizadasDS
            // 
            this.reporteAuditoriasRealizadasDS.DataSetName = "ReporteAuditoriasRealizadasDS";
            this.reporteAuditoriasRealizadasDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ghTecnicos
            // 
            this.ghTecnicos.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtHeader});
            this.ghTecnicos.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TecnicoID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.ghTecnicos.HeightF = 50F;
            this.ghTecnicos.Name = "ghTecnicos";
            // 
            // xrtHeader
            // 
            this.xrtHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrtHeader.Name = "xrtHeader";
            this.xrtHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2});
            this.xrtHeader.SizeF = new System.Drawing.SizeF(899.9999F, 50F);
            this.xrtHeader.StylePriority.UseTextAlignment = false;
            this.xrtHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrlTecnico,
            this.xrlOrdenServicio,
            this.xrlFechaAuditoria,
            this.xrlCalificacion,
            this.xrlObservaciones});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrlTecnico
            // 
            this.xrlTecnico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlTecnico.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlTecnico.ForeColor = System.Drawing.Color.White;
            this.xrlTecnico.Name = "xrlTecnico";
            this.xrlTecnico.StylePriority.UseBackColor = false;
            this.xrlTecnico.StylePriority.UseBorders = false;
            this.xrlTecnico.StylePriority.UseForeColor = false;
            this.xrlTecnico.StylePriority.UseTextAlignment = false;
            this.xrlTecnico.Text = "Técnico";
            this.xrlTecnico.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrlTecnico.Weight = 0.66666674013492022D;
            // 
            // xrlOrdenServicio
            // 
            this.xrlOrdenServicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlOrdenServicio.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlOrdenServicio.ForeColor = System.Drawing.Color.White;
            this.xrlOrdenServicio.Name = "xrlOrdenServicio";
            this.xrlOrdenServicio.StylePriority.UseBackColor = false;
            this.xrlOrdenServicio.StylePriority.UseBorders = false;
            this.xrlOrdenServicio.StylePriority.UseForeColor = false;
            this.xrlOrdenServicio.Text = "Orden de Servicio";
            this.xrlOrdenServicio.Weight = 0.36666671216046987D;
            // 
            // xrlFechaAuditoria
            // 
            this.xrlFechaAuditoria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlFechaAuditoria.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlFechaAuditoria.ForeColor = System.Drawing.Color.White;
            this.xrlFechaAuditoria.Multiline = true;
            this.xrlFechaAuditoria.Name = "xrlFechaAuditoria";
            this.xrlFechaAuditoria.StylePriority.UseBackColor = false;
            this.xrlFechaAuditoria.StylePriority.UseBorders = false;
            this.xrlFechaAuditoria.StylePriority.UseForeColor = false;
            this.xrlFechaAuditoria.Text = "Fecha Auditoría\r\n";
            this.xrlFechaAuditoria.Weight = 0.50000008547749175D;
            // 
            // xrlCalificacion
            // 
            this.xrlCalificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlCalificacion.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlCalificacion.ForeColor = System.Drawing.Color.White;
            this.xrlCalificacion.Name = "xrlCalificacion";
            this.xrlCalificacion.StylePriority.UseBackColor = false;
            this.xrlCalificacion.StylePriority.UseBorders = false;
            this.xrlCalificacion.StylePriority.UseForeColor = false;
            this.xrlCalificacion.Text = "Calificación";
            this.xrlCalificacion.Weight = 0.26666669459402537D;
            // 
            // xrlObservaciones
            // 
            this.xrlObservaciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlObservaciones.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlObservaciones.ForeColor = System.Drawing.Color.White;
            this.xrlObservaciones.Name = "xrlObservaciones";
            this.xrlObservaciones.StylePriority.UseBackColor = false;
            this.xrlObservaciones.StylePriority.UseBorders = false;
            this.xrlObservaciones.StylePriority.UseForeColor = false;
            this.xrlObservaciones.Text = "Observaciones";
            this.xrlObservaciones.Weight = 1.1999997676330927D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtcNombreTecnico,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrtcNombreTecnico
            // 
            this.xrtcNombreTecnico.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReporteAuditoriasRealizadasDA.Tecnico")});
            this.xrtcNombreTecnico.Name = "xrtcNombreTecnico";
            this.xrtcNombreTecnico.StylePriority.UseTextAlignment = false;
            this.xrtcNombreTecnico.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrtcNombreTecnico.Weight = 0.66666674013492022D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Weight = 0.36666672487612911D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Weight = 0.50000007276183245D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Weight = 0.26666669459402537D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Weight = 1.1999997676330927D;
            // 
            // gfTecnicos
            // 
            this.gfTecnicos.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlPromedioAuditorias,
            this.xrlLabelPromedioAuditorias,
            this.xrlAuditoriasRealizadas,
            this.xrlLabelAuditoriasRealizadas});
            this.gfTecnicos.HeightF = 50F;
            this.gfTecnicos.Name = "gfTecnicos";
            this.gfTecnicos.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.gfTecnicos_BeforePrint);
            // 
            // xrlPromedioAuditorias
            // 
            this.xrlPromedioAuditorias.LocationFloat = new DevExpress.Utils.PointFloat(800F, 25F);
            this.xrlPromedioAuditorias.Name = "xrlPromedioAuditorias";
            this.xrlPromedioAuditorias.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlPromedioAuditorias.SizeF = new System.Drawing.SizeF(100F, 25F);
            this.xrlPromedioAuditorias.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:0%}";
            this.xrlPromedioAuditorias.Summary = xrSummary1;
            this.xrlPromedioAuditorias.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlLabelPromedioAuditorias
            // 
            this.xrlLabelPromedioAuditorias.LocationFloat = new DevExpress.Utils.PointFloat(600F, 25F);
            this.xrlLabelPromedioAuditorias.Name = "xrlLabelPromedioAuditorias";
            this.xrlLabelPromedioAuditorias.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlLabelPromedioAuditorias.SizeF = new System.Drawing.SizeF(194.5831F, 25F);
            this.xrlLabelPromedioAuditorias.StylePriority.UseTextAlignment = false;
            this.xrlLabelPromedioAuditorias.Text = "Promedio Calificación Auditorías";
            this.xrlLabelPromedioAuditorias.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlAuditoriasRealizadas
            // 
            this.xrlAuditoriasRealizadas.LocationFloat = new DevExpress.Utils.PointFloat(800F, 0F);
            this.xrlAuditoriasRealizadas.Name = "xrlAuditoriasRealizadas";
            this.xrlAuditoriasRealizadas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlAuditoriasRealizadas.SizeF = new System.Drawing.SizeF(100F, 25F);
            this.xrlAuditoriasRealizadas.StylePriority.UseTextAlignment = false;
            this.xrlAuditoriasRealizadas.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrlLabelAuditoriasRealizadas
            // 
            this.xrlLabelAuditoriasRealizadas.LocationFloat = new DevExpress.Utils.PointFloat(646.6664F, 0F);
            this.xrlLabelAuditoriasRealizadas.Name = "xrlLabelAuditoriasRealizadas";
            this.xrlLabelAuditoriasRealizadas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlLabelAuditoriasRealizadas.SizeF = new System.Drawing.SizeF(147.9166F, 25F);
            this.xrlLabelAuditoriasRealizadas.StylePriority.UseTextAlignment = false;
            this.xrlLabelAuditoriasRealizadas.Text = "Auditorías Realizadas";
            this.xrlLabelAuditoriasRealizadas.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // PorcentajeCalificacion
            // 
            this.PorcentajeCalificacion.DataMember = "Tecnicos.Tecnicos_ReporteAuditoriasRealizadasDA";
            this.PorcentajeCalificacion.Expression = "([ResultadoAuditoria] * 100) / [CantidadAuditoria]";
            this.PorcentajeCalificacion.Name = "PorcentajeCalificacion";
            // 
            // ReporteAuditoriasRealizadasTecnicosRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.dtlTecnicos,
            this.TopMargin,
            this.BottomMargin,
            this.ghTecnicos,
            this.gfTecnicos});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.PorcentajeCalificacion});
            this.DataMember = "ReporteAuditoriasRealizadasDA";
            this.DataSource = this.reporteAuditoriasRealizadasDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 0, 0);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrtDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteAuditoriasRealizadasDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrtHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand dtlTecnicos;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.ReporteAuditoriasRealizadasDS reporteAuditoriasRealizadasDS;
        private DevExpress.XtraReports.UI.GroupHeaderBand ghTecnicos;
        private DevExpress.XtraReports.UI.GroupFooterBand gfTecnicos;
        private DevExpress.XtraReports.UI.XRTable xrtHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrlFechaAuditoria;
        private DevExpress.XtraReports.UI.XRTableCell xrlCalificacion;
        private DevExpress.XtraReports.UI.XRTableCell xrlObservaciones;
        private DevExpress.XtraReports.UI.XRTableCell xrlTecnico;
        private DevExpress.XtraReports.UI.XRTableCell xrlOrdenServicio;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrtcNombreTecnico;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTable xrtDetail;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrtcOrdenServicio;
        private DevExpress.XtraReports.UI.XRTableCell xrtcFechaAuditoria;
        private DevExpress.XtraReports.UI.XRTableCell xrtCalificacion;
        private DevExpress.XtraReports.UI.XRTableCell xrtcObservaciones;
        private DevExpress.XtraReports.UI.XRLabel xrlPromedioAuditorias;
        private DevExpress.XtraReports.UI.XRLabel xrlLabelPromedioAuditorias;
        private DevExpress.XtraReports.UI.XRLabel xrlAuditoriasRealizadas;
        private DevExpress.XtraReports.UI.XRLabel xrlLabelAuditoriasRealizadas;
        private DevExpress.XtraReports.UI.CalculatedField PorcentajeCalificacion;
    }
}
