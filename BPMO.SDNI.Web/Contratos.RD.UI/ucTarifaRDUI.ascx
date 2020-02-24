<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTarifaRDUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.RD.UI.ucTarifaRDUI" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.RD.BO" %>
<style>
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
    <asp:UpdatePanel runat="server" ID="udpTarifaRD">
        <ContentTemplate>
            <table class="trAlinearDerecha" style="margin-left: auto; width: 70%; margin-right: auto;">
                <tr>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span>Capacidad Carga</td>
                    <td style="width: 2%;">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                        <asp:TextBox runat="server" ID="txtCapacidadCarga" CssClass="CampoNumeroEntero" MaxLength="9" Width="98%"></asp:TextBox>
                    </td>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span>Tarifa Diaria</td>
                    <td style="width: 2%">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                        <asp:TextBox runat="server" ID="txtTarifaDiaria" CssClass="CampoNumero" MaxLength="13" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr runat="server" ID="trCobroRangos">
                    <td class="tdCentradoVertical tdRightAling"><span>*</span>COBRO POR</td>
                    <td style="width: 2%;">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                        <asp:DropDownList runat="server" ID="ddlTipoCargo" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoCargo_SelectedIndexChanged"  Width="98%">
                            <asp:ListItem Value="-1" Text="SELECCIONE"></asp:ListItem>
                            <asp:ListItem Value="0" Text="KILOMETROS"></asp:ListItem>
                            <asp:ListItem Value="1" Text="HORAS"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdCentradoVertical tdRightAling" style="text-align: right;">&nbsp;</td>
                    <td style="width: 2%;">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">&nbsp;</td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span>Kilometros Libres</td>
                    <td style="width: 2%">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling"><asp:TextBox runat="server" ID="txtKmLibres" CssClass="CampoNumeroEntero" MaxLength="9" Width="98%"></asp:TextBox></td>
                    <td class="tdCentradoVertical tdRightAling" style="text-align: right; width: 180px;"><span>*</span>Horas Libres</td>
                    <td style="width: 2%;">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling"><asp:TextBox runat="server" ID="txtHrLibres" CssClass="CampoNumero" MaxLength="13" Width="98%"></asp:TextBox></td>
                </tr>
                <tr runat="server" ID="trAdicionales">
                    <td class="tdCentradoVertical tdRightAling"><span>*</span>Tarifa Km Adicional</td>
                    <td style="width: 2%">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling"><asp:TextBox runat="server" ID="txtTarifaKmAdicional" CssClass="CampoNumero" MaxLength="15" Width="98%"></asp:TextBox></td>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span>Tarifa Hr Adicional</td>
                    <td style="width: 2%">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling"><asp:TextBox runat="server" ID="txtTarifaHrAdicional" CssClass="CampoNumero" MaxLength="15" Width="98%"></asp:TextBox></td>
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
                                                        <%# CargoPorKm != null ? !CargoPorKm.Value ? ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).HrRangoInicial : ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).KmRangoInicial : ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).KmRangoInicial%></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="HASTA">
                                                <ItemTemplate>
                                                    <label>
                                                        <%# CargoPorKm != null ? !CargoPorKm.Value ? ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).HrRangoFinal : ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).KmRangoFinal : ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).KmRangoFinal%></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo">
                                                <ItemTemplate>
                                                    <label>
                                                        <%# CargoPorKm != null ? !CargoPorKm.Value ? ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).CargoHr : ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).CargoKm : ((RangoTarifaRDBO)DataBinder.GetDataItem(Container)).CargoKm%></label>
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
