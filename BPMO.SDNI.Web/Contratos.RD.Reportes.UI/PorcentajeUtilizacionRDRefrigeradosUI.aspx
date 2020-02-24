<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="PorcentajeUtilizacionRDRefrigeradosUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.Reportes.UI.PorcentajeUtilizacionRDRefrigeradosUI" %>
<%--Satisface el CU026 – Reporte Porcentaje Utilización de RD Refrigerados--%>

<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
<asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de Time Utilization Refrigerados</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    <%--En esta sección se pone el título de la ventana de filtro--%>
    ¿Qúe Reporte de Time Utilization Refrigerados desea consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
</asp:Content>