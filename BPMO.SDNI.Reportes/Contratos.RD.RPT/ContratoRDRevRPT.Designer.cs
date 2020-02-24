namespace BPMO.SDNI.Contratos.RD.RPT
{
    partial class ContratoRDRevRPT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContratoRDRevRPT));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrlblClausulas = new DevExpress.XtraReports.UI.XRRichText();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrlblTitulo = new DevExpress.XtraReports.UI.XRRichText();
            this.xrlblDatosEncabezado = new DevExpress.XtraReports.UI.XRRichText();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrlblClausulas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrlblTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrlblDatosEncabezado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlblClausulas});
            this.Detail.HeightF = 39.58333F;
            this.Detail.MultiColumn.ColumnCount = 2;
            this.Detail.MultiColumn.ColumnSpacing = 40F;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlblClausulas
            // 
            this.xrlblClausulas.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrlblClausulas.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrlblClausulas.Name = "xrlblClausulas";
            this.xrlblClausulas.SerializableRtfString = resources.GetString("xrlblClausulas.SerializableRtfString");
            this.xrlblClausulas.SizeF = new System.Drawing.SizeF(375.4167F, 23F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 20F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 33.00001F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlblTitulo
            // 
            this.xrlblTitulo.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrlblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(345.8333F, 0F);
            this.xrlblTitulo.Name = "xrlblTitulo";
            this.xrlblTitulo.SerializableRtfString = resources.GetString("xrlblTitulo.SerializableRtfString");
            this.xrlblTitulo.SizeF = new System.Drawing.SizeF(100F, 12.58333F);
            // 
            // xrlblDatosEncabezado
            // 
            this.xrlblDatosEncabezado.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrlblDatosEncabezado.LocationFloat = new DevExpress.Utils.PointFloat(108.3334F, 12.58333F);
            this.xrlblDatosEncabezado.Name = "xrlblDatosEncabezado";
            this.xrlblDatosEncabezado.SerializableRtfString = resources.GetString("xrlblDatosEncabezado.SerializableRtfString");
            this.xrlblDatosEncabezado.SizeF = new System.Drawing.SizeF(597.9166F, 23F);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlblTitulo,
            this.xrlblDatosEncabezado});
            this.ReportHeader.HeightF = 37.5F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // ContratoRDRevRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 33);
            this.Version = "12.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrlblClausulas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrlblTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrlblDatosEncabezado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRRichText xrlblTitulo;
        private DevExpress.XtraReports.UI.XRRichText xrlblDatosEncabezado;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRRichText xrlblClausulas;
    }
}
