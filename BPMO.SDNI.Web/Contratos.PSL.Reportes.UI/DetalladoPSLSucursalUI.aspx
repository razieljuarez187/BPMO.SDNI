<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="DetalladoPSLSucursalUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.Reportes.UI.DetalladoPSLSucursalUI" %>

<asp:Content ID="childHead" ContentPlaceHolderID="childHead" runat="server">
    <%--En esta sección se ponen los scripts o estilos adicionales--%>
</asp:Content>

<asp:Content ID="cEncabezadoLeyenda" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <%--En esta sección se pone la leyenda del encabezado --%>
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte Detallado por Sucursal</asp:Label>
</asp:Content>

<asp:Content ID="cNavegacionSecundaria" ContentPlaceHolderID="navegacionSecundaria" runat="server">
    <%--En esta sección se pone los submenus que van debajo del menú principal--%>
</asp:Content>

<asp:Content ID="cEncabezadoFiltrosReporte" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <%--En esta sección se pone el título de la ventana de filtro--%>
    ¿Qué Reporte Detallado por Sucursal desea consultar?
</asp:Content>

<asp:Content ID="cFiltrosAdicionalesArriba" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
    <%--En esta sección se ponen los filtros adicionales que estarán arriba de los filtros predefinidos--%>
</asp:Content>

<asp:Content ID="cFiltrosAdicionalesAbajo" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
    <%--En esta sección se ponen los filtros adicionales que estarán abajo de los filtros predefinidos--%>
</asp:Content>

<asp:Content ID="cResultadosAbajo" ContentPlaceHolderID="ResultadosAbajo" runat="server">
    <%--En esta sección se ponen los resultados --%>
</asp:Content>
