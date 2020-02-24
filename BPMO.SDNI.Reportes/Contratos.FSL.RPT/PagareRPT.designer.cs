namespace BPMO.SDNI.Contratos.FSL.RPT
{
	partial class PagareRPT
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagareRPT));
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
			this.DetalleFirmas = new DevExpress.XtraReports.UI.DetailBand();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.lblTitulo = new DevExpress.XtraReports.UI.XRRichText();
			this.lblPagare = new DevExpress.XtraReports.UI.XRRichText();
			this.lblBuenoPor = new DevExpress.XtraReports.UI.XRRichText();
			((System.ComponentModel.ISupportInitialize)(this.lblTitulo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblPagare)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblBuenoPor)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPagare});
			this.Detail.HeightF = 23F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
			this.BottomMargin.HeightF = 55F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// DetailReport
			// 
			this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetalleFirmas});
			this.DetailReport.Level = 0;
			this.DetailReport.Name = "DetailReport";
			// 
			// DetalleFirmas
			// 
			this.DetalleFirmas.HeightF = 0F;
			this.DetalleFirmas.Name = "DetalleFirmas";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblBuenoPor,
            this.lblTitulo});
			this.ReportHeader.HeightF = 56.41666F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// lblTitulo
			// 
			this.lblTitulo.Font = new System.Drawing.Font("Times New Roman", 9.75F);
			this.lblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.lblTitulo.Name = "lblTitulo";
			this.lblTitulo.SerializableRtfString = resources.GetString("lblTitulo.SerializableRtfString");
			this.lblTitulo.SizeF = new System.Drawing.SizeF(698F, 29.25F);
			this.lblTitulo.StylePriority.UseFont = false;
			// 
			// lblPagare
			// 
			this.lblPagare.Font = new System.Drawing.Font("Times New Roman", 9.75F);
			this.lblPagare.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.lblPagare.Name = "lblPagare";
			this.lblPagare.SerializableRtfString = resources.GetString("lblPagare.SerializableRtfString");
			this.lblPagare.SizeF = new System.Drawing.SizeF(697.9999F, 23F);
			// 
			// lblBuenoPor
			// 
			this.lblBuenoPor.Font = new System.Drawing.Font("Times New Roman", 9.75F);
			this.lblBuenoPor.LocationFloat = new DevExpress.Utils.PointFloat(0F, 29.25F);
			this.lblBuenoPor.Name = "lblBuenoPor";
			this.lblBuenoPor.SerializableRtfString = resources.GetString("lblBuenoPor.SerializableRtfString");
			this.lblBuenoPor.SizeF = new System.Drawing.SizeF(698F, 27.16667F);
			this.lblBuenoPor.StylePriority.UseFont = false;
			// 
			// PagareRPT
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport,
            this.ReportHeader});
			this.Margins = new System.Drawing.Printing.Margins(72, 80, 50, 55);
			this.Version = "12.1";
			((System.ComponentModel.ISupportInitialize)(this.lblTitulo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblPagare)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblBuenoPor)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

		#endregion

		private DevExpress.XtraReports.UI.DetailBand Detail;
		private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
		private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
		private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
		private DevExpress.XtraReports.UI.DetailBand DetalleFirmas;
		private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
		private DevExpress.XtraReports.UI.XRRichText lblTitulo;
		private DevExpress.XtraReports.UI.XRRichText lblPagare;
		private DevExpress.XtraReports.UI.XRRichText lblBuenoPor;
	}
}
