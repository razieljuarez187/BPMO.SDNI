<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLineaContratoFSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.ucLineaContratoFSLUI" %>
<%@ Register Src="~/Contratos.FSL.UI/ucTarifasFSLUI.ascx" TagName="ucTarifasFSLUI" TagPrefix="uc" %>
<%@ Register Src="~/Contratos.FSL.UI/ucCargosAdicionalesEquiposAliados.ascx" TagName="ucCargosAdicionalesEquiposAliadosUI" TagPrefix="uc" %>
<%-- 
    Satisface al CU015 - Registrar Contrato Full Service Leasing
    Satisface al CU022 - Consultar Contratos Full Service Leasing
--%>
<div id="DatosOrden" class="GroupBody">
    <div id="divInformacionGeneral" class="GroupHeader">
        <asp:Label runat="server" ID="lblTitulo" Text="UNIDAD A RENTAR"></asp:Label>
        <div class="GroupHeaderOpciones Ancho2Opciones">
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btnWizardGuardar" ValidationGroup="Requeridos"
                OnClientClick="ValidatePage('agregar la Unidad al Contrato: Tarifas Adicionales a la Unidad');"
                OnClick="btnAgregar_Click" ViewStateMode="Enabled" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" CausesValidation="false" OnClick="btnCancelar_Click" />
        </div>
    </div>
    <br />
    <div id="divDatosUnidad">
        <table class="trAlinearDerecha" style="margin: 0 auto; width: 100%;">
            <tr>
                <td class="tdCentradoVertical" style="width: 190px;">VIN</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 250px;">
                    <asp:TextBox ID="txtVIN" runat="server" Columns="35"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 190px; text-align: right;"># Econ&oacute;mico</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtNumeroEconomico" runat="server" Columns="35"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Modelo</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtModelo" runat="server" Columns="35"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Año</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtAnio" runat="server" Columns="10" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">PBV Max. Recomendado</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtPBV" runat="server" Columns="35" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">PBC Max. Recomendado</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtPBC" runat="server" Columns="35" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Km Inicial</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmInicial" CssClass="CampoNumeroEntero" runat="server" Columns="35"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right"><asp:Label runat="server" ID="lblPolizaSeguro" Text="P&oacute;liza de Seguro"></asp:Label></td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtNumeroPoliza" Width="190px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Km Estimado Anual</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmEstimadoAnual" runat="server" CssClass="CampoNumeroEntero" Columns="35" MaxLength="9"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right"><span>*</span>Dep&oacute;sito Garant&iacute;a</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDepositoGarantia" runat="server" CssClass="CampoMoneda" Columns="35" MaxLength="13"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Comisi&oacute;n por Apertura
                </td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtComisionApertura" runat="server" CssClass="CampoMoneda" Columns="35" MaxLength="13"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right"><span>*</span>Cargo Fijo por Mes</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCargoFijoMes" runat="server" CssClass="CampoMoneda" Columns="35" MaxLength="13"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Tipo de Cotizaci&oacute;n</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlTipoCotizacion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCotizacion_SelectedIndexChanged">
                        <asp:ListItem Text="Seleccione una opción" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="tdCentradoVertical" align="right"><span>*</span>Producto o Servicio</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtClaveProductoServicio" runat="server" Columns="32" 
                        MaxLength="15" OnTextChanged="txtClaveProductoServicio_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <asp:ImageButton ID="ibtnBuscarProductoServicio" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                            OnClick="ibtnBuscarProductoServicio_Click" Style="width: 17px" />
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span><label>KM/HRS de Unidad</label></td>
                <td class="tdCentradoVertical" style="width: 20px">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlKMHRS" AutoPostBack="True" OnSelectedIndexChanged="ddlKMHRS_OnSelectedIndexChanged" Width="150px">
                        <asp:ListItem Text="SELECCIONE" Value="-1"/>
                        <asp:ListItem Text="KM" Value="0"/>
                        <asp:ListItem Text="HRS" Value="1"/>
                    </asp:DropDownList>
                </td>
                <td class="tdCentradoVertical" align="right">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDescripcionProductoServicio" runat="server" Columns="35" MaxLength="100" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="margin-left: 5px; margin-right: 5px;">
            <asp:UpdatePanel ID="updTarifas" runat="server">
                <ContentTemplate>
                    <uc:ucTarifasFSLUI ID="ucTarifasLinea" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTipoCotizacion" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlKMHRS" EventName="SelectedIndexChanged"/>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
    <div id="divDatosAdicionales">
        <table class="trAlinearDerecha" style="margin: 0 auto; width: 100%;">
            <tr>
                <td class="tdCentradoVertical" style="width: 190px;">¿Opci&oacute;n de Compra?</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:CheckBox ID="cbOpcionCompra" runat="server" AutoPostBack="True" OnCheckedChanged="cbOpcionCompra_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" style="width: 190px;">Moneda</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 250px;">
                    <asp:DropDownList ID="ddlMonedas" runat="server">
                        <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="tdCentradoVertical" style="width: 190px; text-align: right;">Importe de Compra</td>
                <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtImporteCompra" runat="server" CssClass="CampoMoneda" MaxLength="13"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divCargosAdicionalesEquiposAliados" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;">
        <asp:UpdatePanel ID="updCargosAdicionalesEquiposAliados" runat="server">
            <ContentTemplate>
                <uc:ucCargosAdicionalesEquiposAliadosUI ID="ucCargosEquiposAliados" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTipoCotizacion" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<asp:HiddenField ID="hdnUnidaID" runat="server" />
<asp:HiddenField ID="hdnEquipoID" runat="server" />
<asp:HiddenField ID="hdnPlazo" runat="server" />
<asp:HiddenField ID="hdnUnidadOperativaID" runat="server" />
<asp:HiddenField runat="server" ID="hdnProductoServicioId" Value="" />
<asp:Button runat="server" ID="btnResult" Style="display: none" OnClick="btnResult_Click" />