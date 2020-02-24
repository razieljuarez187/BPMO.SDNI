<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Lite.Master" AutoEventWireup="true" CodeBehind="PaginaSinAccesoUI.aspx.cs" Inherits="BPMO.SDNI.MapaSitio.UI.PaginaSinAccesoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #dvContentImage
        {
            width: 645px;
            height: 376px;
            margin: 0 auto 0 auto;
            text-align: center;
        }
        #dvContentImage img { margin: 0px; padding: 0px;
            width: 582px;
        }
        #dvContentImage span
        {
        	text-transform: uppercase;
            font-weight: bold;
            color: #5C5E5D;
            font-size: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dvContentImage">
        <img src="../Contenido/Imagenes/seguridad-candado.png" alt="Estamos Trabajando para Usted" />
        <span>Usted no tiene permiso para realizar esta acción</span>
        <asp:Button ID="btnRegresar" runat="server" CssClass="btnGenerico" 
            Text="Regresar" onclick="btnRegresar_Click" />
    </div>
</asp:Content>
