namespace BPMO.SDNI.Mantenimiento.RPT
{
    partial class ReporteMantenimientoRealizadoContraProgramadoTotalesRPT
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
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteMantenimientoRealizadoContraProgramadoDS = new BPMO.SDNI.Mantenimiento.Reportes.DA.MantenimientoRealizadoContraProgramadoDS();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.reporteMantenimientoRealizadoContraProgramadoDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // mantenimientoRealizadoContraProgramadoTotalesElementoRPT
            // 
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT.Id = 0;
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0.9583314F);
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT.Name = "mantenimientoRealizadoContraProgramadoTotalesElementoRPT";
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT.ReportSource = new BPMO.SDNI.Mantenimiento.RPT.ReporteMantenimientoRealizadoContraProgramadoTotalesElementoRPT();
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT.SizeF = new System.Drawing.SizeF(1400F, 23F);
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT_BeforePrint);
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
            this.BottomMargin.HeightF = 1F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteMantenimientoRealizadoContraProgramadoDS
            // 
            this.reporteMantenimientoRealizadoContraProgramadoDS.DataSetName = "ReporteMantenimientoRealizadoContraProgramadoDS";
            this.reporteMantenimientoRealizadoContraProgramadoDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.mantenimientoRealizadoContraProgramadoTotalesElementoRPT});
            this.GroupHeader1.HeightF = 23.95833F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // ReporteMantenimientoRealizadoContraProgramadoTotalesRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1});
            this.DataMember = "Totales";
            this.DataSource = this.reporteMantenimientoRealizadoContraProgramadoDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 1);
            this.PageHeight = 850;
            this.PageWidth = 1400;
            this.PaperKind = System.Drawing.Printing.PaperKind.Legal;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.reporteMantenimientoRealizadoContraProgramadoDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRSubreport mantenimientoRealizadoContraProgramadoTotalesElementoRPT;
        private Reportes.DA.MantenimientoRealizadoContraProgramadoDS reporteMantenimientoRealizadoContraProgramadoDS;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
    }
}
