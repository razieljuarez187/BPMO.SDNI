<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConfiguracionAccesoUI.aspx.cs" Inherits="BPMO.SDNI.Seguridad.Acceso.UI.ConfiguracionAccesoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="BarraUbicacion">
        <asp:Label ID="lblEncabezadoLeyenda" runat="server">SELECCI&Oacute;N DE ADSCRIPCI&Oacute;N</asp:Label>
    </div>
    <div id="dvCajaLibreMediana">
        <div class="Header"> </div>
        <div class="Content">
            <h1>SELECCIONA TU UNIDAD OPERATIVA</h1>
            <table>
                <tr>
                    <td>Unidad Operativa</td>
                    <td>
                        <asp:DropDownList ID="ddlUnidadOperativa" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Button runat="server" ID="btnAceptar" CssClass="btnEntrar" Text="" onclick="btnAceptar_Click"/>
        </div>
        <div class="Footer"> </div>        
    </div>
</asp:Content>
