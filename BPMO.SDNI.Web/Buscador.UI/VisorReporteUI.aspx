<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
	AutoEventWireup="true" CodeBehind="VisorReporteUI.aspx.cs" Inherits="BPMO.SDNI.Buscador.UI.VisorReporteUI" %>

<%@ Register Assembly="DevExpress.XtraReports.v13.2.Web, Version=13.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="PaginaContenido">
		<!-- Barra de localización -->
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">Visor de Reportes</asp:Label>
		</div>
		<!--Navegación secundaria-->
		<div>
			<!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="width: 100%;">
				&nbsp;
			</div>
		</div>
		<div class="GroupBody" style="margin: 23px auto 10px; width: 940px;">
			<div id="EncabezadoDatosDelCliente" class="GroupHeader" style="width: 940px; margin: 0;">
				<asp:Label ID="lblEncabezadoReporte" runat="server" Text="Visor de Reportes"></asp:Label>
			</div>
			<div id="dvReport" style="min-height: 340px; margin-top: 10px">
                <center>
                    <div style="width: 60%; margin: auto;">
					<dx:ReportToolbar ID="rptToolBar" runat='server' ShowDefaultButtons='False' ReportViewerID="rptViewer">
						<Items>
							<dx:ReportToolbarButton ItemKind='Search' ToolTip="Mostrar la pantalla de busqueda" />
							<dx:ReportToolbarSeparator />
							<dx:ReportToolbarButton ItemKind='PrintReport' ToolTip="Imprimir el reporte" />
							<dx:ReportToolbarSeparator />
							<dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' ToolTip="Primera página" />
							<dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' ToolTip="Página anterior" />
							<dx:ReportToolbarLabel ItemKind='PageLabel' Text="Página" />
							<dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
							</dx:ReportToolbarComboBox>
							<dx:ReportToolbarLabel ItemKind='OfLabel' Text="de" />
							<dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
							<dx:ReportToolbarButton ItemKind='NextPage' ToolTip="Página siguiente" />
							<dx:ReportToolbarButton ItemKind='LastPage' ToolTip="Última página" />
							<dx:ReportToolbarSeparator />
							<dx:ReportToolbarButton ItemKind='SaveToDisk' ToolTip="Exportar el reporte y guardarlo en disco" />
							<dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
								<Elements>
									<dx:ListElement Value='pdf' />
								</Elements>
							</dx:ReportToolbarComboBox>
						</Items>
						<Styles>
							<LabelStyle>
								<Margins MarginLeft='10px' MarginRight='10px' />
							</LabelStyle>
						</Styles>
					</dx:ReportToolbar>
                    </div>
					<div style="width: 95%; overflow: scroll;">
						<dx:ReportViewer ID="rptViewer" runat="server" LoadingPanelText="Cargando...">
							<Border BorderWidth="10px" BorderColor="#CCCCCC"></Border>
						</dx:ReportViewer>
					</div>
                </center>
			</div>
		</div>
		<br />
	</div>
</asp:Content>
