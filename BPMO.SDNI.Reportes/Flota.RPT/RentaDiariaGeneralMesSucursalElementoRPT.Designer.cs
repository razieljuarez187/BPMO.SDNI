namespace BPMO.SDNI.Flota.RPT
{
    partial class RentaDiariaGeneralMesSucursalElementoRPT
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrtblHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrLTotalFlota = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLDiasMes = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlDiasXUnidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlDiasObjetivo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlDiasRenta = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlPorcentajeUtilizacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reporteRDSucursalDS = new BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS();
            ((System.ComponentModel.ISupportInitialize)(this.xrtblHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtblHeader});
            this.Detail.HeightF = 35.41667F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrtblHeader
            // 
            this.xrtblHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtblHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrtblHeader.Name = "xrtblHeader";
            this.xrtblHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrtblHeader.SizeF = new System.Drawing.SizeF(1074F, 35F);
            this.xrtblHeader.StylePriority.UseBorders = false;
            this.xrtblHeader.StylePriority.UseTextAlignment = false;
            this.xrtblHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrLTotalFlota,
            this.xrLDiasMes,
            this.xrlDiasXUnidad,
            this.xrlDiasObjetivo,
            this.xrlDiasRenta,
            this.xrlPorcentajeUtilizacion});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrLTotalFlota
            // 
            this.xrLTotalFlota.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrLTotalFlota.Name = "xrLTotalFlota";
            this.xrLTotalFlota.StylePriority.UseBackColor = false;
            this.xrLTotalFlota.Weight = 1D;
            // 
            // xrLDiasMes
            // 
            this.xrLDiasMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrLDiasMes.ForeColor = System.Drawing.Color.White;
            this.xrLDiasMes.Name = "xrLDiasMes";
            this.xrLDiasMes.StylePriority.UseBackColor = false;
            this.xrLDiasMes.StylePriority.UseForeColor = false;
            this.xrLDiasMes.Weight = 7.6000006802521787D;
            // 
            // xrlDiasXUnidad
            // 
            this.xrlDiasXUnidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlDiasXUnidad.ForeColor = System.Drawing.Color.White;
            this.xrlDiasXUnidad.Name = "xrlDiasXUnidad";
            this.xrlDiasXUnidad.StylePriority.UseBackColor = false;
            this.xrlDiasXUnidad.StylePriority.UseForeColor = false;
            this.xrlDiasXUnidad.Text = "Días X Unidad";
            this.xrlDiasXUnidad.Weight = 0.43761784140407051D;
            // 
            // xrlDiasObjetivo
            // 
            this.xrlDiasObjetivo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlDiasObjetivo.ForeColor = System.Drawing.Color.White;
            this.xrlDiasObjetivo.Name = "xrlDiasObjetivo";
            this.xrlDiasObjetivo.StylePriority.UseBackColor = false;
            this.xrlDiasObjetivo.StylePriority.UseForeColor = false;
            this.xrlDiasObjetivo.Text = "Días Objetivo";
            this.xrlDiasObjetivo.Weight = 0.55000002152427618D;
            // 
            // xrlDiasRenta
            // 
            this.xrlDiasRenta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlDiasRenta.ForeColor = System.Drawing.Color.White;
            this.xrlDiasRenta.Name = "xrlDiasRenta";
            this.xrlDiasRenta.StylePriority.UseBackColor = false;
            this.xrlDiasRenta.StylePriority.UseForeColor = false;
            this.xrlDiasRenta.Text = "Días Renta";
            this.xrlDiasRenta.Weight = 0.44400998107173256D;
            // 
            // xrlPorcentajeUtilizacion
            // 
            this.xrlPorcentajeUtilizacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(69)))), ((int)(((byte)(140)))));
            this.xrlPorcentajeUtilizacion.ForeColor = System.Drawing.Color.White;
            this.xrlPorcentajeUtilizacion.Name = "xrlPorcentajeUtilizacion";
            this.xrlPorcentajeUtilizacion.StylePriority.UseBackColor = false;
            this.xrlPorcentajeUtilizacion.StylePriority.UseForeColor = false;
            this.xrlPorcentajeUtilizacion.Text = "% Utilización";
            this.xrlPorcentajeUtilizacion.Weight = 0.708372086099305D;
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
            this.BottomMargin.HeightF = 13F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // reporteRDSucursalDS
            // 
            this.reporteRDSucursalDS.DataSetName = "ReporteRDSucursalDS";
            this.reporteRDSucursalDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // RentaDiariaGeneralMesSucursalElementoRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataSource = this.reporteRDSucursalDS;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(14, 12, 0, 13);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "12.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrtblHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reporteRDSucursalDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrtblHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrLTotalFlota;
        private DevExpress.XtraReports.UI.XRTableCell xrLDiasMes;
        private DevExpress.XtraReports.UI.XRTableCell xrlDiasXUnidad;
        private DevExpress.XtraReports.UI.XRTableCell xrlDiasObjetivo;
        private DevExpress.XtraReports.UI.XRTableCell xrlDiasRenta;
        private DevExpress.XtraReports.UI.XRTableCell xrlPorcentajeUtilizacion;
        private Reportes.DA.ReporteRDSucursalDS reporteRDSucursalDS;
    }
}
