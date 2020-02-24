<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="ContratoSDRegistradoUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.Mantto.SD.Reportes.UI.ContratoSDRegistradoUI" %>
<%--Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados--%>

<asp:Content ID="childHead" ContentPlaceHolderID="childHead" runat="server">
    <%--En esta sección se ponen los scripts o estilos adicionales--%>
</asp:Content>

<asp:Content ID="encabezadoLeyenda" ContentPlaceHolderID="encabezadoLeyenda" runat="server">   
   <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte Contratos de Servicio Dedicado Registrados</asp:Label>
</asp:Content>

<asp:Content ID="navegacionSecundaria" ContentPlaceHolderID="navegacionSecundaria" runat="server">
    <%--En esta sección se pone los submenus que van debajo del menú principal--%>
</asp:Content>

<asp:Content ID="encabezadoFiltrosReporte" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <%--En esta sección se pone el título de la ventana de filtro--%>
    ¿Qúe reporte de Contratos de Servicio Dedicado Registrados desea consultar?
</asp:Content>

<asp:Content ID="filtrosAdicionalesArriba" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">    
    <%--En esta sección se ponen los filtros adicionales que estarán arriba de los filtros predefinidos--%>
</asp:Content>

<%--Se define las filas que se agregarán a la tabla de filtro--%>
<asp:Content ID="filtrosAdicionalesAbajo" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">   
</asp:Content>