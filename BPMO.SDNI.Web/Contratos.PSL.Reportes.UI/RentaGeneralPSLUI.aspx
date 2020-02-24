<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="RentaGeneralPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.Reportes.UI.RentaGeneralPSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
    <%--En esta sección se ponen los scripts o estilos adicionales--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte General de Rentas</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
    <%--En esta sección se pone los submenus que van debajo del menú principal--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <%--En esta sección se pone el título de la ventana de filtro--%>
    ¿Qué filtros desea utilizar para aplicar en el Reporte General de Rentas?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
    <%--En esta sección se ponen los filtros adicionales que estarán arriba de los filtros predefinidos--%>
</asp:Content>
<%--Se define las filas que se agregarán a la tabla de filtro--%>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ResultadosAbajo" runat="server">
</asp:Content>
