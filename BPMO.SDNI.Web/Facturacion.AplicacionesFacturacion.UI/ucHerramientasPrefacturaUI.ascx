<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHerramientasPrefacturaUI.ascx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ucHerramientasPrefacturaUI" %>
<%--Satisface el caso de uso CU005 – Armar Paquetes Facturacion--%>
<div id="BarraHerramientas" style="float: right;">
    <asp:Menu runat="server" ID="mnContratos" IncludeStyleBlock="False"
        Orientation="Horizontal" CssClass="MenuPrimario" 
       >
        <Items>
            <asp:MenuItem Text="# Contrato" Value="Contrato" Enabled="False"></asp:MenuItem>
             <asp:MenuItem Text="# Pago" Value="Pago" Enabled="False"></asp:MenuItem>            
        </Items>
        <StaticItemTemplate>
            <asp:Label runat="server" ID="lblOpcion" Text='<%# Eval("Text") %>' CssClass='Informacion' ></asp:Label>
            <asp:TextBox runat="server" ID="txtValue"  Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
        </StaticItemTemplate>

        <DynamicHoverStyle CssClass="itemSeleccionado"/>
        <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
        <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
    </asp:Menu>
</div>