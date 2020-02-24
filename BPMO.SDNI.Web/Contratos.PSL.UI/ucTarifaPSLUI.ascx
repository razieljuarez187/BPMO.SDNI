<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTarifaPSLUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.PSL.UI.ucTarifaPSLUI" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.PSL.BO" %>
<style type="text/css">
    .Grid {
        border: 1px solid #5c5e5d;
        margin-top: 0px;
    }
    .GridHeader {
        background-color: #5c5e5d;
        color: White;
        font-size: 1em;
        font-weight: bold;
        text-transform: uppercase;
    }
    .GridHeader, .GridHeader th {
        border-color: #5c5e5d;
    }
    .GridHeader a {
        color: White;
    }
    .tdRightAling {
        width: 23%;
        text-align: right;
    }
    .tdCenterAling {
        width: 25%;
    }
</style>
<div>
    <asp:UpdatePanel runat="server" ID="udpTarifaPSL">
        <ContentTemplate>
            <table class="trAlinearDerecha" style="margin-left: auto; width: 70%; margin-right: auto;">
                <tr>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span><asp:Label runat="server" ID="lblTarifa">Tarifa</asp:Label>
                    <td style="width: 2%;">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                        <asp:TextBox runat="server" ID="txtTarifa" MaxLength="15" Width="98%" onkeypress="return ImporteTarifa(event, this);"></asp:TextBox>
                    </td>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span><asp:Label runat="server" ID="lblTarifaHrAdicional">Tarifa Hr Adicional </asp:Label></td>
                    <td style="width: 2%">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                       <asp:TextBox runat="server" ID="txtTarifaHrAdicional" MaxLength="9" Width="98%" onkeypress="return ImporteTarifaHRAdicional(event, this);"></asp:TextBox>
                    </td>
                </tr>            
               <tr>
               <td colspan="6"></td>
               </tr>
                <tr runat="server" ID="trGridRangos">
                    <td colspan="6">
                        <table style="width: 100%; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td colspan="7" style="text-align: center"><label>RANGOS</label></td>
                            </tr>
                            <tr>
                                <td style="width: 4%;"><label>DE</label></td>
                                <td style="width: 1%;">&nbsp;</td>
                                <td style="width: 22%;">
                                    <asp:TextBox runat="server" ID="txtRangoInicial" CssClass="CampoNumero" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 22%;">
                                    <asp:DropDownList runat="server" ID="ddlRangoTarifa" AutoPostBack="True" 
                                        Width="90%" onselectedindexchanged="ddlRangoTarifa_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="HASTA"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="EN ADELANTE"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 22%;">
                                    <asp:TextBox runat="server" ID="txtRangoFinal" CssClass="CampoNumero" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 7%;"><label>COSTO</label></td>
                                <td style="width: 22%;">
                                    <asp:TextBox runat="server" ID="txtCobroKmHr" CssClass="CampoMoneda" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7" style="text-align: center">
                                    <asp:Button runat="server" ID="btnAgregarRango" Text="Agregar" 
                                        CssClass="btnAgregarATabla" onclick="btnAgregarRango_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:GridView runat="server" ID="grvRangos" AutoGenerateColumns="False" CssClass="Grid"
                                        OnRowCommand="grvRangos_RowCommand" Width="80%" Style="margin: 0 auto;" 
                                        OnRowDatabound="grvRangos_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="DE">
                                                <ItemTemplate>
                                                    <label>
                                                        <%#  ((RangoTarifaPSLBO)DataBinder.GetDataItem(Container)).RangoInicial %></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HASTA">
                                                <ItemTemplate>
                                                    <label>
                                                        <%#  ((RangoTarifaPSLBO)DataBinder.GetDataItem(Container)).RangoFinal%></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo">
                                                <ItemTemplate>
                                                    <label>
                                                        <%# ((RangoTarifaPSLBO)DataBinder.GetDataItem(Container)).Cargo %></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                                        ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>'
                                                        Width="17px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" VerticalAlign="Middle"/>
                                        <EmptyDataTemplate>
                                            <div style="text-align: center;">
                                                <label>NO SE HAN AGREGADO RANGOS</label>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="hdnModoConsulta" Value="0" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdnCrearRangos" Value="0"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
