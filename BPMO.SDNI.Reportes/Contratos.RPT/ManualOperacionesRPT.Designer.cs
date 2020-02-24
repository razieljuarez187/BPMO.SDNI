namespace BPMO.SDNI.Contratos.RPT
{
	partial class ManualOperacionesRPT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualOperacionesRPT));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrRchDetalleManual = new DevExpress.XtraReports.UI.XRRichText();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPicLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPicture = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.xrRchCliente = new DevExpress.XtraReports.UI.XRRichText();
            this.xrRchSubtitulo = new DevExpress.XtraReports.UI.XRRichText();
            this.xrRchTitulo = new DevExpress.XtraReports.UI.XRRichText();
            this.xrRchIndice = new DevExpress.XtraReports.UI.XRRichText();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchDetalleManual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchSubtitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchIndice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRchDetalleManual});
            this.Detail.HeightF = 17.50002F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 3, 100F);
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrRchDetalleManual
            // 
            this.xrRchDetalleManual.CanShrink = true;
            this.xrRchDetalleManual.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRchDetalleManual.KeepTogether = true;
            this.xrRchDetalleManual.LocationFloat = new DevExpress.Utils.PointFloat(3F, 0F);
            this.xrRchDetalleManual.Name = "xrRchDetalleManual";
            this.xrRchDetalleManual.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 2, 2, 100F);
            this.xrRchDetalleManual.SerializableRtfString = resources.GetString("xrRchDetalleManual.SerializableRtfString");
            this.xrRchDetalleManual.SizeF = new System.Drawing.SizeF(745F, 17.50002F);
            this.xrRchDetalleManual.StylePriority.UsePadding = false;
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
            this.BottomMargin.HeightF = 50F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPicLogo});
            this.PageHeader.HeightF = 73.5417F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPicLogo
            // 
            this.xrPicLogo.Image = ((System.Drawing.Image)(resources.GetObject("xrPicLogo.Image")));
            this.xrPicLogo.LocationFloat = new DevExpress.Utils.PointFloat(609.7916F, 0F);
            this.xrPicLogo.Name = "xrPicLogo";
            this.xrPicLogo.SizeF = new System.Drawing.SizeF(122.92F, 72.74F);
            this.xrPicLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1});
            this.PageFooter.HeightF = 26.04167F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Format = "Página {0} de {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(277.0833F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPicture,
            this.xrPageBreak1,
            this.xrRchCliente,
            this.xrRchSubtitulo,
            this.xrRchTitulo,
            this.xrRchIndice});
            this.ReportHeader.HeightF = 173.9583F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrPicture
            // 
            this.xrPicture.Image = ((System.Drawing.Image)(resources.GetObject("xrPicture.Image")));
            this.xrPicture.LocationFloat = new DevExpress.Utils.PointFloat(617.0834F, 0F);
            this.xrPicture.Name = "xrPicture";
            this.xrPicture.SizeF = new System.Drawing.SizeF(122.9166F, 74.75002F);
            this.xrPicture.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 161.9583F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // xrRchCliente
            // 
            this.xrRchCliente.Font = new System.Drawing.Font("Times New Roman", 18F);
            this.xrRchCliente.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 74.75001F);
            this.xrRchCliente.Name = "xrRchCliente";
            this.xrRchCliente.SerializableRtfString = resources.GetString("xrRchCliente.SerializableRtfString");
            this.xrRchCliente.SizeF = new System.Drawing.SizeF(730F, 32.375F);
            this.xrRchCliente.StylePriority.UseFont = false;
            // 
            // xrRchSubtitulo
            // 
            this.xrRchSubtitulo.Font = new System.Drawing.Font("Times New Roman", 18F);
            this.xrRchSubtitulo.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 42.37501F);
            this.xrRchSubtitulo.Name = "xrRchSubtitulo";
            this.xrRchSubtitulo.SerializableRtfString = resources.GetString("xrRchSubtitulo.SerializableRtfString");
            this.xrRchSubtitulo.SizeF = new System.Drawing.SizeF(730F, 32.375F);
            this.xrRchSubtitulo.StylePriority.UseFont = false;
            // 
            // xrRchTitulo
            // 
            this.xrRchTitulo.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.xrRchTitulo.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrRchTitulo.Name = "xrRchTitulo";
            this.xrRchTitulo.SerializableRtfString = resources.GetString("xrRchTitulo.SerializableRtfString");
            this.xrRchTitulo.SizeF = new System.Drawing.SizeF(730F, 32.375F);
            this.xrRchTitulo.StylePriority.UseFont = false;
            // 
            // xrRchIndice
            // 
            this.xrRchIndice.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRchIndice.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 131.5833F);
            this.xrRchIndice.Name = "xrRchIndice";
            this.xrRchIndice.SerializableRtfString = resources.GetString("xrRchIndice.SerializableRtfString");
            this.xrRchIndice.SizeF = new System.Drawing.SizeF(730F, 23F);
            // 
            // ManualOperacionesRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrRchDetalleManual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchSubtitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRchIndice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		private DevExpress.XtraReports.UI.DetailBand Detail;
		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
		private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
		private DevExpress.XtraReports.UI.XRRichText xrRchDetalleManual;
		private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
		private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
		private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
		private DevExpress.XtraReports.UI.XRRichText xrRchIndice;
		private DevExpress.XtraReports.UI.XRRichText xrRchTitulo;
		private DevExpress.XtraReports.UI.XRRichText xrRchCliente;
		private DevExpress.XtraReports.UI.XRRichText xrRchSubtitulo;
		private DevExpress.XtraReports.UI.XRPictureBox xrPicLogo;
		private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRPictureBox xrPicture;
	}
}
