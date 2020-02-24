<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarMasterFacturacionUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ConsultarMasterFacturacionUI" %>
<%-- Satisface a la solicitud de cambio SC0001 --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="">
        $(document).ready(function () {
            var strHtml = $('#<%= hdnHtml.ClientID %>').val();

            var visor = window.open();
            var visorDocument = visor.document;
            visorDocument.open();
            visorDocument.write(strHtml);
            visorDocument.close();

            //window.location.replace('<%= Page.ResolveUrl(ConfigurationManager.AppSettings["Logueo"]) %>');
            history.back(1);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hdnHtml"/>    
</asp:Content>
