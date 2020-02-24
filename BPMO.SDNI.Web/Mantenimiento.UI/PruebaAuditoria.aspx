<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="PruebaAuditoria.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.PruebaAuditoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="txtFolio" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="Redireccionar" runat="server" Text="Button" 
        onclick="Redireccionar_Click" />
</asp:Content>
