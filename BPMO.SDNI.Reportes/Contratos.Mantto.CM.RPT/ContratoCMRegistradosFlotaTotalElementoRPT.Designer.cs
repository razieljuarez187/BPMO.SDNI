namespace BPMO.SDNI.Contratos.Mantto.CM.RPT
{
    partial class ContratoCMRegistradosFlotaTotalElementoRPT
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTotalUnidades = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalUnidadesFlota = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTotalRefrigerados = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalUnidadesRefrigeradas = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTotalSecos = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalUnidadesSecas = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrlNombreSucursal = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.consultarContratosRegistradosManttoDS1 = new BPMO.SDNI.Contratos.Mantto.Reportes.DA.ConsultarContratosRegistradosManttoDS();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.consultarContratosRegistradosManttoDS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 56.68105F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.LightGray;
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow3});
            this.xrTable1.SizeF = new System.Drawing.SizeF(220F, 56.67F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTotalUnidades,
            this.xrTotalUnidadesFlota});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTotalUnidades
            // 
            this.xrTotalUnidades.CanGrow = false;
            this.xrTotalUnidades.Name = "xrTotalUnidades";
            this.xrTotalUnidades.Text = "Total";
            this.xrTotalUnidades.Weight = 1.2960000008322974D;
            // 
            // xrTotalUnidadesFlota
            // 
            this.xrTotalUnidadesFlota.CanGrow = false;
            this.xrTotalUnidadesFlota.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursal.SucursalID")});
            this.xrTotalUnidadesFlota.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.xrTotalUnidadesFlota.Name = "xrTotalUnidadesFlota";
            this.xrTotalUnidadesFlota.StylePriority.UseFont = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTotalUnidadesFlota.Summary = xrSummary1;
            this.xrTotalUnidadesFlota.Text = "xrTotalUnidadesFlota";
            this.xrTotalUnidadesFlota.Weight = 1.2959999991677025D;
            this.xrTotalUnidadesFlota.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrTotalUnidadesFlota_SummaryGetResult);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTotalRefrigerados,
            this.xrTotalUnidadesRefrigeradas});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTotalRefrigerados
            // 
            this.xrTotalRefrigerados.CanGrow = false;
            this.xrTotalRefrigerados.Name = "xrTotalRefrigerados";
            this.xrTotalRefrigerados.Text = "Refrigerados";
            this.xrTotalRefrigerados.Weight = 1.2959999872381036D;
            // 
            // xrTotalUnidadesRefrigeradas
            // 
            this.xrTotalUnidadesRefrigeradas.CanGrow = false;
            this.xrTotalUnidadesRefrigeradas.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursal.SucursalID")});
            this.xrTotalUnidadesRefrigeradas.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.xrTotalUnidadesRefrigeradas.Name = "xrTotalUnidadesRefrigeradas";
            this.xrTotalUnidadesRefrigeradas.Scripts.OnSummaryReset = "xrTotalUnidadesRefrigeradas_SummaryReset";
            this.xrTotalUnidadesRefrigeradas.StylePriority.UseFont = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTotalUnidadesRefrigeradas.Summary = xrSummary2;
            this.xrTotalUnidadesRefrigeradas.Text = "xrTotalUnidadesRefrigeradas";
            this.xrTotalUnidadesRefrigeradas.Weight = 1.296000012761896D;
            this.xrTotalUnidadesRefrigeradas.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrTotalUnidadesRefrigeradas_SummaryGetResult);
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTotalSecos,
            this.xrTotalUnidadesSecas});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTotalSecos
            // 
            this.xrTotalSecos.CanGrow = false;
            this.xrTotalSecos.Name = "xrTotalSecos";
            this.xrTotalSecos.Text = "Secos";
            this.xrTotalSecos.Weight = 1.2959999872381036D;
            // 
            // xrTotalUnidadesSecas
            // 
            this.xrTotalUnidadesSecas.CanGrow = false;
            this.xrTotalUnidadesSecas.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursal.SucursalID")});
            this.xrTotalUnidadesSecas.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.xrTotalUnidadesSecas.Name = "xrTotalUnidadesSecas";
            this.xrTotalUnidadesSecas.StylePriority.UseFont = false;
            xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTotalUnidadesSecas.Summary = xrSummary3;
            this.xrTotalUnidadesSecas.Text = "xrTotalUnidadesSecas";
            this.xrTotalUnidadesSecas.Weight = 1.296000012761896D;
            this.xrTotalUnidadesSecas.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrTotalUnidadesSecas_SummaryGetResult);
            // 
            // xrlNombreSucursal
            // 
            this.xrlNombreSucursal.BackColor = System.Drawing.Color.LightGray;
            this.xrlNombreSucursal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrlNombreSucursal.CanGrow = false;
            this.xrlNombreSucursal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sucursal.Nombre")});
            this.xrlNombreSucursal.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold);
            this.xrlNombreSucursal.LocationFloat = new DevExpress.Utils.PointFloat(0F, 8.249998F);
            this.xrlNombreSucursal.Name = "xrlNombreSucursal";
            this.xrlNombreSucursal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlNombreSucursal.SizeF = new System.Drawing.SizeF(220F, 25F);
            this.xrlNombreSucursal.StylePriority.UseBackColor = false;
            this.xrlNombreSucursal.StylePriority.UseBorders = false;
            this.xrlNombreSucursal.StylePriority.UseFont = false;
            this.xrlNombreSucursal.StylePriority.UseTextAlignment = false;
            this.xrlNombreSucursal.Text = "xrlNombreSucursal";
            this.xrlNombreSucursal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            // consultarContratosRegistradosManttoDS1
            // 
            this.consultarContratosRegistradosManttoDS1.DataSetName = "ConsultarContratosRegistradosManttoDS";
            this.consultarContratosRegistradosManttoDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlNombreSucursal});
            this.GroupHeader1.HeightF = 33.25F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 10.91102F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // ContratoCMRegistradosFlotaTotalElementoRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.GroupFooter1});
            this.DataMember = "Sucursal";
            this.DataSource = this.consultarContratosRegistradosManttoDS1;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 878, 0, 0);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "12.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.consultarContratosRegistradosManttoDS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalUnidades;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalUnidadesFlota;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalRefrigerados;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalUnidadesRefrigeradas;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalSecos;
        private DevExpress.XtraReports.UI.XRTableCell xrTotalUnidadesSecas;
        private DevExpress.XtraReports.UI.XRLabel xrlNombreSucursal;
        private Reportes.DA.ConsultarContratosRegistradosManttoDS consultarContratosRegistradosManttoDS1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
    }
}
