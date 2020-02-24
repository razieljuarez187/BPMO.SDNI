namespace BPMO.SDNI
{
    partial class ReporteOrdenSalidaRPT
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
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.TextResultado = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.TextNumeroEconomico = new DevExpress.XtraReports.UI.XRLabel();
            this.TextSerie = new DevExpress.XtraReports.UI.XRLabel();
            this.TextNombreSucursal = new DevExpress.XtraReports.UI.XRLabel();
            this.TextNombreCliente = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTipoPlaca = new DevExpress.XtraReports.UI.XRLabel();
            this.lblEconomico = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSerie = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSucursal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCliente = new DevExpress.XtraReports.UI.XRLabel();
            this.lblFechaSalida = new DevExpress.XtraReports.UI.XRLabel();
            this.LblFolio = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GeneralStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.ImageLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblTitulo = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.TextFirmaJefe = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.lblFirmaCliente = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblFirmaVigilante = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.obtenerOrdenSalidaDS1 = new BPMO.SDNI.Mantenimiento.Reportes.DA.ObtenerOrdenSalidaDS();
            this.lblPlaca = new DevExpress.XtraReports.UI.CalculatedField();
            this.LabelStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TextStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.lblResultado = new DevExpress.XtraReports.UI.CalculatedField();
            this.lbFechaSalida = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.obtenerOrdenSalidaDS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel1,
            this.TextResultado,
            this.xrPageBreak1,
            this.TextNumeroEconomico,
            this.TextSerie,
            this.TextNombreSucursal,
            this.TextNombreCliente,
            this.lblTipoPlaca,
            this.lblEconomico,
            this.lblSerie,
            this.lblSucursal,
            this.lblCliente,
            this.lblFechaSalida,
            this.LblFolio});
            this.Detail.HeightF = 423.9584F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnidadMantenimiento.UnidadMantenimientoID")});
            this.xrLabel2.ForeColor = System.Drawing.Color.Red;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(645.9167F, 21.45834F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(117.0833F, 23F);
            this.xrLabel2.StyleName = "GeneralStyle";
            this.xrLabel2.StylePriority.UseForeColor = false;
            this.xrLabel2.Text = "xrLabel2";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "lbFechaSalida")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(574.0416F, 59.33332F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(188.9581F, 23F);
            this.xrLabel1.StyleName = "GeneralStyle";
            this.xrLabel1.Text = "TextFechaSalida";
            // 
            // TextResultado
            // 
            this.TextResultado.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "lblResultado")});
            this.TextResultado.LocationFloat = new DevExpress.Utils.PointFloat(236.8543F, 387.4583F);
            this.TextResultado.Name = "TextResultado";
            this.TextResultado.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextResultado.SizeF = new System.Drawing.SizeF(441.7707F, 23F);
            this.TextResultado.StyleName = "TextStyle";
            this.TextResultado.Text = "TextResultado";
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 410.4583F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // TextNumeroEconomico
            // 
            this.TextNumeroEconomico.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnidadMantenimiento.NumeroEconomico")});
            this.TextNumeroEconomico.LocationFloat = new DevExpress.Utils.PointFloat(236.8542F, 324.9583F);
            this.TextNumeroEconomico.Name = "TextNumeroEconomico";
            this.TextNumeroEconomico.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextNumeroEconomico.SizeF = new System.Drawing.SizeF(441.7705F, 22.99997F);
            this.TextNumeroEconomico.StyleName = "TextStyle";
            this.TextNumeroEconomico.Text = "TextNumeroEconomico";
            // 
            // TextSerie
            // 
            this.TextSerie.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnidadMantenimiento.Serie")});
            this.TextSerie.LocationFloat = new DevExpress.Utils.PointFloat(236.8542F, 261.4167F);
            this.TextSerie.Name = "TextSerie";
            this.TextSerie.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextSerie.SizeF = new System.Drawing.SizeF(441.7707F, 23.00002F);
            this.TextSerie.StyleName = "TextStyle";
            this.TextSerie.Text = "TextSerie";
            // 
            // TextNombreSucursal
            // 
            this.TextNombreSucursal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnidadMantenimiento.NombreSucursal")});
            this.TextNombreSucursal.LocationFloat = new DevExpress.Utils.PointFloat(236.8542F, 197.875F);
            this.TextNombreSucursal.Name = "TextNombreSucursal";
            this.TextNombreSucursal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextNombreSucursal.SizeF = new System.Drawing.SizeF(441.7707F, 23F);
            this.TextNombreSucursal.StyleName = "TextStyle";
            this.TextNombreSucursal.Text = "TextNombreSucursal";
            // 
            // TextNombreCliente
            // 
            this.TextNombreCliente.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnidadMantenimiento.NombreCliente")});
            this.TextNombreCliente.LocationFloat = new DevExpress.Utils.PointFloat(236.8542F, 136.4167F);
            this.TextNombreCliente.Name = "TextNombreCliente";
            this.TextNombreCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextNombreCliente.SizeF = new System.Drawing.SizeF(441.7707F, 23F);
            this.TextNombreCliente.StyleName = "TextStyle";
            this.TextNombreCliente.Text = "TextNombreCliente";
            // 
            // lblTipoPlaca
            // 
            this.lblTipoPlaca.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "lblPlaca")});
            this.lblTipoPlaca.LocationFloat = new DevExpress.Utils.PointFloat(48.62477F, 387.4583F);
            this.lblTipoPlaca.Name = "lblTipoPlaca";
            this.lblTipoPlaca.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTipoPlaca.SizeF = new System.Drawing.SizeF(188.2295F, 23F);
            this.lblTipoPlaca.StyleName = "LabelStyle";
            // 
            // lblEconomico
            // 
            this.lblEconomico.LocationFloat = new DevExpress.Utils.PointFloat(48.6249F, 324.9583F);
            this.lblEconomico.Name = "lblEconomico";
            this.lblEconomico.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblEconomico.SizeF = new System.Drawing.SizeF(188.2294F, 23F);
            this.lblEconomico.StyleName = "LabelStyle";
            this.lblEconomico.Text = "Número Económico:";
            // 
            // lblSerie
            // 
            this.lblSerie.LocationFloat = new DevExpress.Utils.PointFloat(48.62502F, 261.4167F);
            this.lblSerie.Name = "lblSerie";
            this.lblSerie.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSerie.SizeF = new System.Drawing.SizeF(188.2292F, 23.00002F);
            this.lblSerie.StyleName = "LabelStyle";
            this.lblSerie.Text = "Número de Serie:";
            // 
            // lblSucursal
            // 
            this.lblSucursal.LocationFloat = new DevExpress.Utils.PointFloat(48.62496F, 197.875F);
            this.lblSucursal.Name = "lblSucursal";
            this.lblSucursal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSucursal.SizeF = new System.Drawing.SizeF(188.2293F, 23F);
            this.lblSucursal.StyleName = "LabelStyle";
            this.lblSucursal.Text = "Sucursal:";
            // 
            // lblCliente
            // 
            this.lblCliente.LocationFloat = new DevExpress.Utils.PointFloat(48.62502F, 136.4167F);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCliente.SizeF = new System.Drawing.SizeF(188.2292F, 23F);
            this.lblCliente.StyleName = "LabelStyle";
            this.lblCliente.Text = "Cliente:";
            // 
            // lblFechaSalida
            // 
            this.lblFechaSalida.LocationFloat = new DevExpress.Utils.PointFloat(441.4375F, 59.33332F);
            this.lblFechaSalida.Name = "lblFechaSalida";
            this.lblFechaSalida.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFechaSalida.SizeF = new System.Drawing.SizeF(132.6042F, 23F);
            this.lblFechaSalida.StyleName = "GeneralStyle";
            this.lblFechaSalida.Text = "Fecha de Salida:";
            // 
            // LblFolio
            // 
            this.LblFolio.LocationFloat = new DevExpress.Utils.PointFloat(591.7499F, 21.45834F);
            this.LblFolio.Multiline = true;
            this.LblFolio.Name = "LblFolio";
            this.LblFolio.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LblFolio.SizeF = new System.Drawing.SizeF(54.16672F, 23F);
            this.LblFolio.StyleName = "GeneralStyle";
            this.LblFolio.Text = "Folio:\r\n\r\n";
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
            // GeneralStyle
            // 
            this.GeneralStyle.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GeneralStyle.Name = "GeneralStyle";
            this.GeneralStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.ImageLogo,
            this.lblTitulo});
            this.PageHeader.HeightF = 102.0833F;
            this.PageHeader.Name = "PageHeader";
            // 
            // ImageLogo
            // 
            this.ImageLogo.LocationFloat = new DevExpress.Utils.PointFloat(48.62502F, 0F);
            this.ImageLogo.Name = "ImageLogo";
            this.ImageLogo.SizeF = new System.Drawing.SizeF(227.0833F, 100F);
            this.ImageLogo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ImageLogo_BeforePrint);
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.LocationFloat = new DevExpress.Utils.PointFloat(399.9999F, 24.95832F);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTitulo.SizeF = new System.Drawing.SizeF(245.9167F, 43.83333F);
            this.lblTitulo.StylePriority.UseFont = false;
            this.lblTitulo.StylePriority.UseTextAlignment = false;
            this.lblTitulo.Text = "PASE DE SALIDA";
            this.lblTitulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.TextFirmaJefe,
            this.xrLine3,
            this.lblFirmaCliente,
            this.xrLine2,
            this.lblFirmaVigilante,
            this.xrLine1});
            this.PageFooter.HeightF = 229.1667F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnidadMantenimiento.JefeOperaciones")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(37.5F, 34.04166F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            this.xrLabel3.StyleName = "GeneralStyle";
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "TextNombreJefe";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TextFirmaJefe
            // 
            this.TextFirmaJefe.LocationFloat = new DevExpress.Utils.PointFloat(37.5F, 62.29172F);
            this.TextFirmaJefe.Multiline = true;
            this.TextFirmaJefe.Name = "TextFirmaJefe";
            this.TextFirmaJefe.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TextFirmaJefe.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            this.TextFirmaJefe.StyleName = "GeneralStyle";
            this.TextFirmaJefe.StylePriority.UseTextAlignment = false;
            this.TextFirmaJefe.Text = "Jefe de Operaciones\r\n";
            this.TextFirmaJefe.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(37.5F, 11.04171F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            // 
            // lblFirmaCliente
            // 
            this.lblFirmaCliente.LocationFloat = new DevExpress.Utils.PointFloat(287.8126F, 129.5001F);
            this.lblFirmaCliente.Multiline = true;
            this.lblFirmaCliente.Name = "lblFirmaCliente";
            this.lblFirmaCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFirmaCliente.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            this.lblFirmaCliente.StyleName = "GeneralStyle";
            this.lblFirmaCliente.StylePriority.UseTextAlignment = false;
            this.lblFirmaCliente.Text = "Nombre y Firma del Cliente\r\n";
            this.lblFirmaCliente.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(287.8126F, 106.5001F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            // 
            // lblFirmaVigilante
            // 
            this.lblFirmaVigilante.LocationFloat = new DevExpress.Utils.PointFloat(519.7918F, 34.04166F);
            this.lblFirmaVigilante.Name = "lblFirmaVigilante";
            this.lblFirmaVigilante.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFirmaVigilante.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            this.lblFirmaVigilante.StyleName = "GeneralStyle";
            this.lblFirmaVigilante.StylePriority.UseTextAlignment = false;
            this.lblFirmaVigilante.Text = "Nombre y Firma del Vigilante";
            this.lblFirmaVigilante.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(519.7918F, 11.04171F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(216.0416F, 23F);
            // 
            // obtenerOrdenSalidaDS1
            // 
            this.obtenerOrdenSalidaDS1.DataSetName = "ObtenerOrdenSalidaDS";
            this.obtenerOrdenSalidaDS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblPlaca
            // 
            this.lblPlaca.Expression = "Iif([TipoTramite]==2,\'Placa Federal:\',Iif([TipoTramite]==1,\'Placa Estatal:\',\'\'))";
            this.lblPlaca.Name = "lblPlaca";
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
            // lblResultado
            // 
            this.lblResultado.Expression = "Iif([TipoTramite]==2 Or [TipoTramite]==1,[Resultado],\'\')";
            this.lblResultado.Name = "lblResultado";
            // 
            // lbFechaSalida
            // 
            this.lbFechaSalida.Expression = "Iif([FechaSalida]==null, LocalDateTimeNow(),[FechaSalida])";
            this.lbFechaSalida.Name = "lbFechaSalida";
            // 
            // ReporteOrdenSalidaRPT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.lblPlaca,
            this.lblResultado,
            this.lbFechaSalida});
            this.DataMember = "UnidadMantenimiento";
            this.DataSource = this.obtenerOrdenSalidaDS1;
            this.Margins = new System.Drawing.Printing.Margins(38, 39, 0, 0);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.GeneralStyle,
            this.LabelStyle,
            this.TextStyle});
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.obtenerOrdenSalidaDS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel LblFolio;
        private DevExpress.XtraReports.UI.XRControlStyle GeneralStyle;
        private DevExpress.XtraReports.UI.XRLabel lblFechaSalida;
        private DevExpress.XtraReports.UI.XRLabel lblEconomico;
        private DevExpress.XtraReports.UI.XRLabel lblSerie;
        private DevExpress.XtraReports.UI.XRLabel lblSucursal;
        private DevExpress.XtraReports.UI.XRLabel lblCliente;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel lblTitulo;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLabel lblFirmaCliente;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel lblFirmaVigilante;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private Mantenimiento.Reportes.DA.ObtenerOrdenSalidaDS obtenerOrdenSalidaDS1;
        private DevExpress.XtraReports.UI.CalculatedField lblPlaca;
        private DevExpress.XtraReports.UI.XRLabel TextNumeroEconomico;
        private DevExpress.XtraReports.UI.XRLabel TextSerie;
        private DevExpress.XtraReports.UI.XRLabel TextNombreSucursal;
        private DevExpress.XtraReports.UI.XRLabel TextNombreCliente;
        private DevExpress.XtraReports.UI.XRLabel lblTipoPlaca;
        private DevExpress.XtraReports.UI.XRLabel TextFirmaJefe;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRControlStyle LabelStyle;
        private DevExpress.XtraReports.UI.XRControlStyle TextStyle;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRLabel TextResultado;
        private DevExpress.XtraReports.UI.CalculatedField lblResultado;
        private DevExpress.XtraReports.UI.XRPictureBox ImageLogo;
        private DevExpress.XtraReports.UI.CalculatedField lbFechaSalida;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
    }
}
