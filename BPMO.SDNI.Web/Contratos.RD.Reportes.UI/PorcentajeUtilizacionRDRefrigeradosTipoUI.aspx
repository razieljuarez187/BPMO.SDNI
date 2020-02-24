<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="PorcentajeUtilizacionRDRefrigeradosTipoUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.Reportes.UI.PorcentajeUtilizacionRDRefrigeradosTipoUI" %>
<%--Satisface el CU027 – Reporte Porcentaje Utilización de RD Refrigerados por Tipo--%>

<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
<asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de Time Utilization Refrigerados Por Tipo</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <%--En esta sección se pone el título de la ventana de filtro--%>
    ¿Qúe Reporte de Time Utilization Refrigerados Por Tipo desea consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
</asp:Content>
