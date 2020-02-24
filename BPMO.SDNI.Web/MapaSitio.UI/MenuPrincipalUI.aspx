<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
	AutoEventWireup="true" CodeBehind="MenuPrincipalUI.aspx.cs" Inherits="BPMO.SDNI.MapaSitio.UI.MenuPrincipalUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="PaginaContenido" runat="server">
		<div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">MENU PRINCIPAL</asp:Label>
		</div>
		<center>
			<br />
			<br />
			<br />
			<br />
			<br />        
            <asp:Image id="imgPortadaEmpresa" runat="server" style="margin-top: 60px;
				margin-left: auto; margin-right: auto; margin-bottom: 30px;"  /> 
		</center>
	</div>
</asp:Content>



