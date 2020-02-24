<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTramitesActivosUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucTramitesActivosUI" %>
<%@ Register src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" tagname="ucCatalogoDocumentosUI" tagprefix="uc1" %>
<div id="dvTramitesActivos">
    <table class="trAlinearDerecha" style="margin: 0px auto 10px auto;">
        <asp:ListView ID="lvTramites" runat="server" onitemdatabound="lvTramites_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td class="tdCentradoVertical" style="width: 230px;">
                        <span><asp:Label ID="lblTramite" runat="server"><%# DataBinder.Eval(Container, "DataItem.Tipo").ToString().Replace("_", " ").Replace("N ", "# ")%></asp:Label></span>
                    </td>
                    <td class="tdCentradoVertical" style="width: 16px;">
                        <asp:Image ID="imgEstatus" ImageUrl="~/Contenido/Imagenes/exito.png" runat="server" style="width: 16px;" />
                    </td>
                    <td class="tdCentradoVertical" style="width: 280px;">
                        <asp:TextBox ID="txbResultado" Enabled="false" Width="275px" Text='<%# DataBinder.Eval(Container, "DataItem.Resultado") %>' AutoPostBack="True" runat="server" OnTextChanged="txbResultado_OnTextChanged"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
       
           
       
        <tr id="trTramites" runat="server">
            <td colspan="3" align="center">
                <br />
                <asp:Button ID="btnTramites" runat="server" Text="Catálogo de Trámites" 
                    CssClass="btnLibrePositivo" onclick="btnTramites_Click" />
            </td>
        </tr>
    </table>
</div>
<uc1:ucCatalogoDocumentosUI ID="ucCatalogoDocumentosTramites" runat="server"/>
<asp:HiddenField ID="hdnTramitableID" runat="server" />
<asp:HiddenField ID="hdnTipoTramitable" runat="server" />
<asp:HiddenField ID="hdnDescripcionEnllantable" runat="server" />
<asp:HiddenField ID="hdnNumeroPedimento" runat="server" />
<asp:HiddenField ID="hdnCambioPedimento" runat="server" />

