<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTarifasFSLUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.FSL.UI.ucTarifasFSLUI" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.FSL.BO" %>
<%-- 
    Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing
    Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
    Satisface al caso de uso CU025 - Catalogo Tarifas Comerciales Renta FSL     
    Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
--%>
<style type="text/css">
    .tabla {
        width: 100%;
        border-collapse: collapse;
        border: 1px solid #000000;
        font-size: 12pt !important;
    }
    .tabla span {
        font-size: 12pt !important;
    }
    #divTipoCargo {
        margin: 0 auto;
        width: 100%;
    }
    #divTipoCargo > table {
        width: 100%;
    }
    .tdAlinearDerecha {
        text-align: right;
        width: 18%;
        vertical-align: middle;
    }
    .tdEspacioCentro {
        width: 2%;
    }
    .tdAlinearIzquierda {
        width: 30%;
    }
    #Configuracion {
        margin: 0 auto;
        width: 100%;
    }
    #Configuracion > fieldset {
        margin: 0 auto;
        width: 100%;
    }
    #Configuracion > fieldset > table {
        margin: 0 auto;
        width: 100%;
    }
    #AniosConfigurados {
        margin: 0 auto;
        width: 100%;
    }
    #AniosConfigurados > fieldset {
        margin: 0 auto;
        width: 100%;
    }
    .Grid
{
    border: 1px solid #5c5e5d;
    margin-top: 0px;
}
    .GridHeader{
    background-color: #5c5e5d;
    color: White;
    font-size: 1em;
    font-weight: bold;
    text-transform: uppercase;
}
.GridHeader,
.GridHeader th 
{ 
	border-color: #5c5e5d; 
}
.GridHeader a 
{
    color: White;
}
</style>
<asp:UpdatePanel ID="updListView" runat="server">
    <ContentTemplate>
        <div id="divTipoCargo" runat="server">
            <table>
                <tr>
                    <td class="tdAlinearDerecha">
                        <label>
                            CARGO POR
                        </label>
                    </td>
                    <td class="tdEspacioCentro">
                        &nbsp;
                    </td>
                    <td class="tdAlinearIzquierda">
                        <asp:DropDownList runat="server" ID="ddlTipoCargo" AutoPostBack="True"
                            onselectedindexchanged="ddlTipoCargo_SelectedIndexChanged">
                            <asp:ListItem Value="-1" Text="SELECCIONE" />
                            <asp:ListItem Value="0" Text="KMS" />
                            <asp:ListItem Value="1" Text="HRS" />
                        </asp:DropDownList>
                    </td>
                    <td style="width: 50%;">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div id="Configuracion" runat="server">
            <fieldset>
                <legend>
                    <label>
                        CONFIGURAR A&Ntilde;OS</label></legend>
                <table style="margin: 0 auto;">
                    <tbody>
                        <tr>
                            <td class="tdAlinearDerecha">
                                <label>
                                    A&Ntilde;O</label>
                            </td>
                            <td class="tdEspacioCentro">
                                &nbsp;
                            </td>
                            <td class="tdAlinearIzquierda">
                                <asp:DropDownList runat="server" ID="ddlAniosContrato" Width="40%" AutoPostBack="True"
                                    onselectedindexchanged="ddlAniosContrato_SelectedIndexChanged" />
                            </td>
                            <td colspan="3" style="text-align: center">
                                <asp:Button ID="btnGuardarAnio" runat="server" Text="GUARDAR" 
                                    CssClass="btnWizardGuardar" onclick="btnGuardarAnio_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdAlinearDerecha">
                                <label>
                                    FRECUENCIA</label>
                            </td>
                            <td class="tdEspacioCentro">
                                &nbsp;
                            </td>
                            <td class="tdAlinearIzquierda">
                                <asp:DropDownList runat="server" ID="ddlFrecuencia" Width="70%" />
                            </td>
                            <td class="tdAlinearDerecha">
                                <label>
                                    KM/HRS LIBRES</label>
                            </td>
                            <td class="tdEspacioCentro">
                                &nbsp;
                            </td>
                            <td class="tdAlinearIzquierda">
                                <asp:TextBox runat="server" ID="txtKmHrsLibres" Width="70%" CssClass="CampoNumeroEntero"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdAlinearDerecha"><span>KM/HR M&Iacute;NIMA</span></td>
                            <td class="tdEspacioCentro">&nbsp;</td>
                            <td class="tdAlinearIzquierda">
                                <asp:TextBox runat="server" ID="txtKmHrMinimo" Width="70%" CssClass="CampoNumeroEntero"></asp:TextBox>
                            </td>
                            <td class="tdAlinearDerecha"></td>
                            <td class="tdEspacioCentro">&nbsp;</td>
                            <td class="tdAlinearIzquierda"></td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: center;">
                                <label>RANGOS</label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: center;">
                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" 
                                    CssClass="btnAgregarATabla"  onclick="btnAgregar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table style="width: 100%;">
                                    <tbody>
                                        <tr>
                                            <td style="width: 8%; text-align: right; vertical-align: middle;">
                                                <label>
                                                    DE
                                                </label>
                                            </td>
                                            <td class="tdEspacioCentro">&nbsp;</td>
                                            <td style="width: 20%;">
                                                <asp:TextBox runat="server" ID="txtRangoInicial" CssClass="CampoNumeroEntero txtMoneda"
                                                    Width="90%"></asp:TextBox>
                                            </td>
                                            <td style="width: 20%;">
                                                <asp:DropDownList runat="server" ID="ddlRangoTiempo" Width="90%" AutoPostBack="True" 
                                                    onselectedindexchanged="ddlRangoTiempo_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="HASTA" />
                                                    <asp:ListItem Value="1" Text="EN ADELANTE"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 20%;">
                                                <asp:TextBox runat="server" ID="txtRangoFinal" CssClass="CampoNumeroEntero txtMoneda"
                                                    Width="90%"></asp:TextBox>
                                            </td>
                                            <td style="width: 8%; text-align: right; vertical-align: middle;">
                                                <label>
                                                    CARGO
                                                </label>
                                            </td>
                                            <td class="tdEspacioCentro" style="text-align: right; vertical-align: middle"><label>$</label></td>
                                            <td style="width: 20%;">
                                                <asp:TextBox runat="server" ID="txtCargo" CssClass="CampoMoneda txtMoneda" Width="90%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:GridView runat="server" ID="grvRangosConfigurados"
                                                    AutoGenerateColumns="False" Width="50%" CssClass="Grid" style="margin: 0 auto;" 
                                                    onrowcommand="grvRangosConfigurados_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="DE">
                                                             <ItemTemplate>
                                                                <label><%# !String.IsNullOrEmpty(hdnCargoKm.Value) ? hdnCargoKm.Value == "0" ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoInicial : ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoInicial : null%></label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="30%" HorizontalAlign="Right"/>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="HASTA">
                                                            <ItemTemplate>
                                                                <label><%# !String.IsNullOrEmpty(hdnCargoKm.Value) ? 
                                                                       hdnCargoKm.Value == "0" ? 
                                                                       ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal.ToString() : "EN ADELANTE"  : 
                                                                       ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal.ToString() : "EN ADELANTE" :
                                                                       null%></label>    
                                                            </ItemTemplate>
                                                            <ItemStyle Width="30%" HorizontalAlign="Right"/>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cargo">
                                                            <ItemTemplate>
                                                                <label><%# !String.IsNullOrEmpty(hdnCargoKm.Value) ? hdnCargoKm.Value == "0" ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoHr : ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoKm : null%></label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="30%" HorizontalAlign="Right"/>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                                                                    CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Center"/>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="GridHeader"/>
                                                    <EmptyDataTemplate>
                                                        <label>NO SE HAN AGREGADO RANGOS</label>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </fieldset>
        </div>
        <br/>
        <div id="AniosConfigurados" runat="server">
            <fieldset>
                <legend>A&Ntilde;OS CONFIGURADOS</legend>
                <asp:GridView runat="server" ID="grvAniosConfigurados" AutoGenerateColumns="False" CssClass="Grid" style="margin: 0 auto;" Width="50%">
                    <Columns>
                        <asp:TemplateField HeaderText="AÑO">
                            <ItemTemplate>
                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Año != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Año.ToString() : "N/A"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FRECUENCIA">
                            <ItemTemplate>
                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Frecuencia != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Frecuencia.ToString() : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KMS LIBRES">
                            <ItemTemplate>
                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).KmLibres != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).KmLibres.ToString() : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HRS LIBRES">
                            <ItemTemplate>
                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).HrLibres != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).HrLibres.ToString() : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CONFIGURADO">
                            <ItemTemplate>
                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Frecuencia == null ? "NO" : "SI"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridHeader"/>
                    <EmptyDataTemplate>
                        <label>NO EXISTEN TARIFAS PARA PRESENTAR</label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </fieldset>
        </div>
        <div id="ConsultaConfiguracion" runat="server">
            <fieldset>
                <legend><span>CONFIGURACI&Oacute;N DE A&Ntilde;OS</span></legend>
                <asp:Repeater runat="server" ID="rptConfiguracion" OnItemDataBound="rptConfiguracion_OnItemDataBound">
                    <ItemTemplate>
                        <table style="width: 80%; margin: 0 auto;">
                            <tbody>
                                <tr>
                                    <td class="tdAlinearDerecha"><span>A&Ntilde;O</span></td>
                                    <td class="tdEspacioCentro">&nbsp;</td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:TextBox runat="server" ID="txtAnioConsulta" Enabled="False" Width="20%"></asp:TextBox>
                                    </td>
                                    <td class="tdAlinearDerecha"><span>FRECUENCIA</span></td>
                                    <td class="tdEspacioCentro">&nbsp;</td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:TextBox runat="server" ID="txtFrecuenciaConsulta" Enabled="False" Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdAlinearDerecha"><span>KM/HRS LIBRES</span></td>
                                    <td class="tdEspacioCentro">&nbsp;</td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:TextBox runat="server" ID="txtKmLibresConsulta" Enabled="False" Width="50%"></asp:TextBox>
                                    </td>
                                    <td class="tdAlinearDerecha"><span>KM/HRS M&Iacute;NIMA</span></td>
                                    <td class="tdEspacioCentro">&nbsp;</td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:TextBox runat="server" ID="txtKmMinimosConsulta" Enabled="False" Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:GridView runat="server" ID="grvConsultaConfiguracion" AutoGenerateColumns="False"
                                         CssClass="Grid" style="margin: 0 auto;" Width="50%" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="DE">
                                                    <ItemTemplate>
                                                        <label><%# ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoInicial ?? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoInicial%></label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="33%" HorizontalAlign="Right"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="HASTA">
                                                    <ItemTemplate>
                                                        <label><%# ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal == null && ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal == null ?
                                                              "EN ADELANTE" :
                                                              ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal.ToString() : 
                                                              ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal.ToString() : ""%></label>    
                                                    </ItemTemplate>
                                                    <ItemStyle Width="33%" HorizontalAlign="Right"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cargo">
                                                    <ItemTemplate>
                                                        <label><%# ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoKm != null && ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoHr != null 
                                                               ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoInicial != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoKm : ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoHr
                                                               : ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoKm ?? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoHr%></label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="34%" HorizontalAlign="Right"/>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader"/>
                                            <EmptyDataTemplate><span>NO APLICA CONFIGURACIÓN</span></EmptyDataTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br/>
                    </ItemTemplate>
                </asp:Repeater>
            </fieldset>
        </div>
        <asp:HiddenField runat="server" ID="hdnCargoKm" Value="" />
        <asp:HiddenField runat="server" ID="hdnModoConsulta" Value="0"/>
        <asp:HiddenField runat="server" ID="hdnSinTarifas" Value="0"/>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlRangoTiempo"/>
        <asp:AsyncPostBackTrigger ControlID="ddlAniosContrato"/>
        <asp:AsyncPostBackTrigger ControlID="grvRangosConfigurados"/>
        <asp:AsyncPostBackTrigger ControlID="btnAgregar"/>
        <asp:AsyncPostBackTrigger ControlID="btnGuardarAnio"/>
    </Triggers>
</asp:UpdatePanel>
<asp:HiddenField runat="server" ID="hdnIdentificador" />
<asp:HiddenField runat="server" ID="hdnHabilitar" Value="true" />
<asp:HiddenField runat="server" ID="hdnPrimeraCarga" Value="true" />

<script type="text/javascript">
    function <%= ClientID %>_Inicializar() {
    }
</script>
