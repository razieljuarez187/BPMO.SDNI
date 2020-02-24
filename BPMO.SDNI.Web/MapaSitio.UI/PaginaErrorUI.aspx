<%@ Page Title="PaginaErrorUI" Language="C#" AutoEventWireup="true" MasterPageFile="~/Contenido/MasterPages/Lite.Master"
    CodeBehind="PaginaErrorUI.aspx.cs" Inherits="BPMO.SDNI.MapaSitio.UI.PaginaErrorUI" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        #dvContentImage
        {
            width: 645px;
            height: 376px;
            margin: 0 auto 0 auto;
            text-align: center;
        }
        #dvContentImage img { margin: 0px; padding: 0px; }
        #dvContentImage span
        {
        	text-transform: uppercase;
            font-weight: bold;
            color: #5C5E5D;
            font-size: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <div id="dvContentImage">
        <img src="../Contenido/Imagenes/error-01.png" alt="Estamos Trabajando para Usted" />
        <span>Estamos trabajando para usted</span>
    </div>
</asp:Content>
