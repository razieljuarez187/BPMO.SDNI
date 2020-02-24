namespace BPMO.SDNI.Flota.RPT
{
    partial class FlotaActivaSucursalTotalesRPT
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
            this.flotaActivaSucursalTotalesElementoRPT = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.flotaActivaRDDS = new BPMO.SDNI.Flota.Reportes.DA.FlotaActivaRDDS();
            ((System.ComponentModel.ISupportInitialize)(this.flotaActivaRDDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.flotaActivaSucursalTotalesElementoRPT});
            this.Detail.HeightF = 26.04167F;
            this.Detail.MultiColumn.ColumnCount = 5;
            this.Detail.MultiColumn.ColumnSpacing = 5F;
            this.Detail.MultiColumn.ColumnWidth = 220F;
            this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // flotaActivaSucursalTotalesElementoRPT
            // 
            this.flotaActivaSucursalTotalesElementoRPT.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.flotaActivaSucursalTotalesElementoRPT.Name = "flotaActivaSucursalTotalesElementoRPT";
            this.flotaActivaSucursalTotalesElementoRPT.ReportSource = new BPMO.SDNI.Flota.RPT.FlotaActivaSucursalTotalesElementoRPT();
            this.flotaActivaSucursalTotalesElementoRPT.SizeF = new System.Drawing.SizeF(220F, 23F);
            this.flotaActivaSucursalTotalesElementoRPT.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.flotaActivaSucursalTotalesElementoRPT_BeforePrint);
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
            // flotaActivaRDDS
            // 
            this.flotaActivaRDDS.DataSetName = "FlotaActivaRDDS";
            this.flotaActivaRDDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FlotaActivaSucursalTotalesRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataMember = "Sucursal";
            this.DataSource = this.flotaActivaRDDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "12.1";
            ((System.ComponentModel.ISupportInitialize)(this.flotaActivaRDDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private Reportes.DA.FlotaActivaRDDS flotaActivaRDDS;
        private DevExpress.XtraReports.UI.XRSubreport flotaActivaSucursalTotalesElementoRPT;
    }
}
