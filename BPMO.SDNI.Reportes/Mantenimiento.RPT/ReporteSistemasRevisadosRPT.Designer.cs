namespace BPMO.SDNI.Mantenimiento.RPT
{
    partial class ReporteSistemasRevisadosRPT
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.sistemasRevisadosDS1 = new BPMO.SDNI.Mantenimiento.Reportes.DA.SistemasRevisadosDS();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ImageLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.FechaActual = new DevExpress.XtraReports.UI.XRPageInfo();
            this.TextTituloReporte = new DevExpress.XtraReports.UI.XRLabel();
            this.TextNombreEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.LblNumeroSerie = new DevExpress.XtraReports.UI.XRLabel();
            this.TextNumeroSerie = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.TextTipoMantenimiento = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTipoMantenimiento = new DevExpress.XtraReports.UI.XRLabel();
            this.TextOrdenServicioID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblOrdenServicioID = new DevExpress.XtraReports.UI.XRLabel();
            this.LabelFechaServicio = new DevExpress.XtraReports.UI.XRLabel();
            this.TextFechaServicio = new DevExpress.XtraReports.UI.XRLabel();
            this.TextNombreCliente = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNombreCliente = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.SubreporteGrafica = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.LabelStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TextStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.sistemasRevisadosDS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sistemasRevisadosDS1
            // 
            this.sistemasRevisadosDS1.DataSetName = "SistemasRevisadosDS";
            this.sistemasRevisadosDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50F;
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
            // ImageLogo
            // 
            this.ImageLogo.LocationFloat = new DevExpress.Utils.PointFloat(21.45859F, 0F);
            this.ImageLogo.Name = "ImageLogo";
            this.ImageLogo.SizeF = new System.Drawing.SizeF(216.6667F, 115.625F);
            this.ImageLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ImageLogo_BeforePrint);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.FechaActual,
            this.TextTituloReporte,
            this.TextNombreEmpresa,
            this.ImageLogo});
            this.PageHeader.HeightF = 115.625F;
            this.PageHeader.Name = "PageHeader";
            // 
            // FechaActual
            // 
            this.FechaActual.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FechaActual.Format = "{0:dd/MM/yyyy hh:mm tt}";
            this.FechaActual.LocationFloat = new DevExpress.Utils.PointFloat(250F, 92.62502F);
            this.FechaActual.Name = "FechaActual";
            this.FechaActual.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.FechaActual.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.FechaActual.SizeF = new System.Drawing.SizeF(513F, 23F);
            this.FechaActual.StyleName = "TextStyle";
            this.FechaActual.StylePriority.UseFont = false;
            this.FechaActual.StylePriority.UseTextAlignment = false;
            this.FechaActual.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // TextTituloReporte
            // 
            this.TextTituloReporte.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextTituloReporte.LocationFloat = new DevExpress.Utils.PointFloat(250F, 56.49999F);
            this.TextTituloReporte.Name = "TextTituloReporte";
            this.TextTituloReporte.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextTituloReporte.SizeF = new System.Drawing.SizeF(513F, 26.12502F);
            this.TextTituloReporte.StylePriority.UseFont = false;
            this.TextTituloReporte.StylePriority.UseTextAlignment = false;
            this.TextTituloReporte.Text = "REPORTE DE SISTEMAS REVISADOS UNIDAD";
            this.TextTituloReporte.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TextNombreEmpresa
            // 
            this.TextNombreEmpresa.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ConfiguracionesSistema.NombreEmpresa")});
            this.TextNombreEmpresa.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextNombreEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(249.9999F, 10.00001F);
            this.TextNombreEmpresa.Name = "TextNombreEmpresa";
            this.TextNombreEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextNombreEmpresa.SizeF = new System.Drawing.SizeF(513.0001F, 34.74998F);
            this.TextNombreEmpresa.StylePriority.UseFont = false;
            this.TextNombreEmpresa.StylePriority.UseTextAlignment = false;
            this.TextNombreEmpresa.Text = "TextNombreEmpresa";
            this.TextNombreEmpresa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LblNumeroSerie,
            this.TextNumeroSerie});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("UnidadID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 64.58332F;
            this.GroupHeader1.Level = 1;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // LblNumeroSerie
            // 
            this.LblNumeroSerie.LocationFloat = new DevExpress.Utils.PointFloat(10.00013F, 41.58332F);
            this.LblNumeroSerie.Name = "LblNumeroSerie";
            this.LblNumeroSerie.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LblNumeroSerie.SizeF = new System.Drawing.SizeF(171.2499F, 23F);
            this.LblNumeroSerie.StyleName = "LabelStyle";
            this.LblNumeroSerie.Text = "Número de Serie:";
            // 
            // TextNumeroSerie
            // 
            this.TextNumeroSerie.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServicioMantenimiento.NumeroSerie")});
            this.TextNumeroSerie.LocationFloat = new DevExpress.Utils.PointFloat(200.0003F, 41.58332F);
            this.TextNumeroSerie.Name = "TextNumeroSerie";
            this.TextNumeroSerie.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextNumeroSerie.SizeF = new System.Drawing.SizeF(572.9998F, 23F);
            this.TextNumeroSerie.StyleName = "TextStyle";
            this.TextNumeroSerie.Text = "TextNumeroSerie";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.TextTipoMantenimiento,
            this.lblTipoMantenimiento,
            this.TextOrdenServicioID,
            this.lblOrdenServicioID,
            this.LabelFechaServicio,
            this.TextFechaServicio,
            this.TextNombreCliente,
            this.lblNombreCliente});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("OrdenServicioID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 187.8333F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // TextTipoMantenimiento
            // 
            this.TextTipoMantenimiento.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServicioMantenimiento.TipoServicio")});
            this.TextTipoMantenimiento.LocationFloat = new DevExpress.Utils.PointFloat(200.0003F, 48.95833F);
            this.TextTipoMantenimiento.Name = "TextTipoMantenimiento";
            this.TextTipoMantenimiento.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextTipoMantenimiento.SizeF = new System.Drawing.SizeF(215.625F, 22.99999F);
            this.TextTipoMantenimiento.StyleName = "TextStyle";
            // 
            // lblTipoMantenimiento
            // 
            this.lblTipoMantenimiento.LocationFloat = new DevExpress.Utils.PointFloat(9.999943F, 48.95833F);
            this.lblTipoMantenimiento.Multiline = true;
            this.lblTipoMantenimiento.Name = "lblTipoMantenimiento";
            this.lblTipoMantenimiento.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTipoMantenimiento.SizeF = new System.Drawing.SizeF(171.2501F, 23F);
            this.lblTipoMantenimiento.StyleName = "LabelStyle";
            this.lblTipoMantenimiento.Text = "Tipo de Servicio:";
            // 
            // TextOrdenServicioID
            // 
            this.TextOrdenServicioID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServicioMantenimiento.OrdenServicioID")});
            this.TextOrdenServicioID.LocationFloat = new DevExpress.Utils.PointFloat(200.0003F, 13.12501F);
            this.TextOrdenServicioID.Name = "TextOrdenServicioID";
            this.TextOrdenServicioID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextOrdenServicioID.SizeF = new System.Drawing.SizeF(215.625F, 23F);
            this.TextOrdenServicioID.StyleName = "TextStyle";
            this.TextOrdenServicioID.Text = "TextOrdenServicioID";
            // 
            // lblOrdenServicioID
            // 
            this.lblOrdenServicioID.LocationFloat = new DevExpress.Utils.PointFloat(10.00013F, 13.12501F);
            this.lblOrdenServicioID.Multiline = true;
            this.lblOrdenServicioID.Name = "lblOrdenServicioID";
            this.lblOrdenServicioID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblOrdenServicioID.SizeF = new System.Drawing.SizeF(171.2499F, 23F);
            this.lblOrdenServicioID.StyleName = "LabelStyle";
            this.lblOrdenServicioID.Text = "Orden de Servicio:";
            // 
            // LabelFechaServicio
            // 
            this.LabelFechaServicio.LocationFloat = new DevExpress.Utils.PointFloat(10.0002F, 124.625F);
            this.LabelFechaServicio.Name = "LabelFechaServicio";
            this.LabelFechaServicio.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LabelFechaServicio.SizeF = new System.Drawing.SizeF(171.2498F, 23F);
            this.LabelFechaServicio.StyleName = "LabelStyle";
            this.LabelFechaServicio.Text = "Fecha de Servicio:";
            // 
            // TextFechaServicio
            // 
            this.TextFechaServicio.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServicioMantenimiento.FechaServicio")});
            this.TextFechaServicio.LocationFloat = new DevExpress.Utils.PointFloat(200.0003F, 124.625F);
            this.TextFechaServicio.Name = "TextFechaServicio";
            this.TextFechaServicio.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextFechaServicio.SizeF = new System.Drawing.SizeF(249.3747F, 23F);
            this.TextFechaServicio.StyleName = "TextStyle";
            this.TextFechaServicio.Text = "TextN";
            // 
            // TextNombreCliente
            // 
            this.TextNombreCliente.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServicioMantenimiento.NombreCliente")});
            this.TextNombreCliente.LocationFloat = new DevExpress.Utils.PointFloat(200.0003F, 88.12498F);
            this.TextNombreCliente.Multiline = true;
            this.TextNombreCliente.Name = "TextNombreCliente";
            this.TextNombreCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextNombreCliente.SizeF = new System.Drawing.SizeF(572.9995F, 23F);
            this.TextNombreCliente.StyleName = "TextStyle";
            // 
            // lblNombreCliente
            // 
            this.lblNombreCliente.LocationFloat = new DevExpress.Utils.PointFloat(10.0002F, 88.12498F);
            this.lblNombreCliente.Multiline = true;
            this.lblNombreCliente.Name = "lblNombreCliente";
            this.lblNombreCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNombreCliente.SizeF = new System.Drawing.SizeF(171.2498F, 23F);
            this.lblNombreCliente.StyleName = "LabelStyle";
            this.lblNombreCliente.Text = "Cliente:";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.SubreporteGrafica,
            this.xrPageBreak1});
            this.GroupFooter1.HeightF = 582.9167F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // SubreporteGrafica
            // 
            this.SubreporteGrafica.CanShrink = true;
            this.SubreporteGrafica.Id = 0;
            this.SubreporteGrafica.LocationFloat = new DevExpress.Utils.PointFloat(21.45859F, 10.00001F);
            this.SubreporteGrafica.Name = "SubreporteGrafica";
            this.SubreporteGrafica.ReportSource = new BPMO.SDNI.Mantenimiento.RPT.SistemasRevisadosGraficaRPT();
            this.SubreporteGrafica.SizeF = new System.Drawing.SizeF(728.54F, 550F);
            this.SubreporteGrafica.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Subreporte_Grafica_BeforePrint);
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 574.6667F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // LabelStyle
            // 
            this.LabelStyle.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelStyle.Name = "LabelStyle";
            this.LabelStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // TextStyle
            // 
            this.TextStyle.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextStyle.Name = "TextStyle";
            this.TextStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // ReporteSistemasRevisadosRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupFooter1});
            this.DataMember = "ServicioMantenimiento";
            this.DataSource = this.sistemasRevisadosDS1;
            this.Margins = new System.Drawing.Printing.Margins(39, 38, 50, 0);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.LabelStyle,
            this.TextStyle});
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.sistemasRevisadosDS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.SistemasRevisadosDS sistemasRevisadosDS1;
        private DevExpress.XtraReports.UI.XRPictureBox ImageLogo;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel TextTituloReporte;
        private DevExpress.XtraReports.UI.XRLabel TextNombreEmpresa;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel TextNumeroSerie;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel TextFechaServicio;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel LblNumeroSerie;
        private DevExpress.XtraReports.UI.XRControlStyle LabelStyle;
        private DevExpress.XtraReports.UI.XRControlStyle TextStyle;
        private DevExpress.XtraReports.UI.XRLabel LabelFechaServicio;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRSubreport SubreporteGrafica;
        private DevExpress.XtraReports.UI.XRPageInfo FechaActual;
        private DevExpress.XtraReports.UI.XRLabel TextNombreCliente;
        private DevExpress.XtraReports.UI.XRLabel lblNombreCliente;
        private DevExpress.XtraReports.UI.XRLabel TextOrdenServicioID;
        private DevExpress.XtraReports.UI.XRLabel lblOrdenServicioID;
        private DevExpress.XtraReports.UI.XRLabel TextTipoMantenimiento;
        private DevExpress.XtraReports.UI.XRLabel lblTipoMantenimiento;
    }
}
