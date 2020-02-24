<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="DollarUtilizationUI.aspx.cs" Inherits="BPMO.SDNI.Flota.Reportes.UI.DollarUtilizationUI" %>
<%--Satisface el CU024 - Reporte de Dollar Utilization--%>

<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
    <%--En esta sección se ponen los scripts o estilos adicionales--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de Dollar Utilization</asp:Label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
    <%--En esta sección se pone los submenus que van debajo del menú principal--%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <%--En esta sección se pone el título de la ventana de filtro--%>
    ¿Qúe reporte de Dollar Utilization desea consultar?
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
    <%--En esta sección se ponen los filtros adicionales que estarán arriba de los filtros predefinidos--%>
</asp:Content>

<%--Se define las filas que se agregarán a la tabla de filtro--%>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
</asp:Content>
