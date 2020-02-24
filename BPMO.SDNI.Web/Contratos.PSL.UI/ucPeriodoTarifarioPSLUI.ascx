<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPeriodoTarifarioPSLUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPeriodoTarifarioPSLUI" %>
<%@ Import Namespace="BPMO.SDNI.Comun.BO" %>
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
    <asp:UpdatePanel runat="server" ID="udpPeriodoTarifarioPSL">
        <ContentTemplate>
            <table class="trAlinearDerecha" style="margin-left: auto; width: 95%; margin-right: auto;">
                 <tr>
                    <td colspan="7" style="text-align: center">
                        <asp:CheckBox ID="chbxIncluyeSD" Text="  Incluye Sábados y Domingos" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span><asp:Label runat="server" ID="lblDuracionSemana">Duración Semana</asp:Label>
                    <td style="width: 2%;">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                        <asp:TextBox runat="server" ID="txtDuracionSemana" MaxLength="2" CssClass="CampoNumeroEntero" Width="15%"></asp:TextBox>
                    </td>
                    <td class="tdCentradoVertical tdRightAling"><span>*</span><asp:Label runat="server" ID="lblDuracionMes">Duración Mes</asp:Label></td>
                    <td style="width: 2%">&nbsp;</td>
                    <td class="tdCentradoVertical tdCenterAling">
                       <asp:TextBox runat="server" ID="txtDuracionMes" MaxLength="2" CssClass="CampoNumeroEntero" Width="15%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <table style="width: 100%; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td class="tdCentradoVertical tdRightAling" style="width: 17%;"><label>Inicio Período Día</label></td>
                                <td style="width: 13%;">
                                    <asp:TextBox runat="server" ID="txtInicioPeriodoDia" CssClass="campoNumero" Text="1" Width="90%" Enabled="false"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical tdRightAling" style="width: 20%;"><span>*</span><label>Inicio Período Semana</label></td>
                                <td style="width: 13%;">
                                    <asp:TextBox runat="server" ID="txtInicioPeriodoSemana" MaxLength="2" CssClass="CampoNumeroEntero" Width="90%"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical tdRightAling" style="width: 17%;"><span>*</span><label>Inicio Período Mes</label></td>
                                <td style="width: 13%;">
                                    <asp:TextBox runat="server" ID="txtInicioPeriodoMes" MaxLength="2" CssClass="CampoNumeroEntero" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" ID="trGridRangos" style="margin-top: 20px">
                    <td colspan="6">
                        <fieldset id="fsTarifas">
                        <legend>TURNOS</legend>
                        <table style="width: 95%; margin-left: auto; margin-right: auto; margin-top: 20px;">
                            <tr>
                                <td style="width: 6%;"><span>*</span><label>TURNO</label></td>
                                 <td style="width: 18%;">
                                    <asp:DropDownList runat="server" ID="ddlTurnoTarifa" Width="90%"></asp:DropDownList>
                                </td>
                                <td class="tdCentradoVertical tdRightAling" style="width: 12%;"><span>*</span><label>Máximo Horas Día</label></td>
                                <td style="width: 5%;">
                                    <asp:TextBox runat="server" ID="txtMaximoHorasDia" CssClass="CampoNumeroEntero" MaxLength="3" Width="90%"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical tdRightAling" style="width: 14%;"><span>*</span><label>Máximo Horas Semana</label></td>
                                <td style="width: 5%;">
                                    <asp:TextBox runat="server" ID="txtMaximoHorasSemana" CssClass="CampoNumeroEntero" MaxLength="3" Width="90%"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical tdRightAling" style="width: 12%;"><span>*</span><label>Máximo Horas Mes</label></td>
                                <td style="width: 5%;">
                                    <asp:TextBox runat="server" ID="txtMaximoHorasMes" CssClass="CampoNumeroEntero" MaxLength="3" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" style="text-align: center">
                                    <asp:Button runat="server" ID="btnAgregarHorasTurno" Text="Agregar" 
                                        CssClass="btnAgregarATabla" onclick="btnAgregarHorasTurno_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:GridView runat="server" ID="grvTurnos" AutoGenerateColumns="False" CssClass="Grid"
                                        OnRowCommand="grvTurnos_RowCommand" Width="90%" Style="margin: 0 auto;" 
                                        OnRowDatabound="grvTurnos_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Turno">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTurno" Width="100%"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="22%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Máximo Horas Día">
                                                <ItemTemplate>
                                                    <label>
                                                        <%#  ((DetalleHorasTurnoTarifaBO)DataBinder.GetDataItem(Container)).Dia%></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="22%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Máximo Horas Semana">
                                                <ItemTemplate>
                                                    <label>
                                                        <%# ((DetalleHorasTurnoTarifaBO)DataBinder.GetDataItem(Container)).Semana%></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="23%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Máximo Horas Mes">
                                                <ItemTemplate>
                                                    <label>
                                                        <%# ((DetalleHorasTurnoTarifaBO)DataBinder.GetDataItem(Container)).Mes%></label>
                                                </ItemTemplate>
                                                <ItemStyle Width="23%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                                        ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>'
                                                        Width="17px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" Width="100%" VerticalAlign="Middle"/>
                                        <EmptyDataTemplate>
                                            <div style="text-align: center;">
                                                <label>NO SE HAN CONFIGURADO TURNOS</label>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="hdnModoEdicion" Value="0" />
                         </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
