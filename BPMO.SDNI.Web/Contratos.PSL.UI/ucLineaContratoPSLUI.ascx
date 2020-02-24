<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLineaContratoPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucLineaContratoPSLUI" %>
<%@ Register Src="~/Contratos.FSL.UI/ucTarifasFSLUI.ascx" TagName="ucTarifasFSLUI" TagPrefix="uc" %>
<%@ Register Src="~/Contratos.FSL.UI/ucCargosAdicionalesEquiposAliados.ascx" TagName="ucCargosAdicionalesEquiposAliadosUI" TagPrefix="uc" %>

<style type="text/css">    
    #divDatosUnidadGrl fieldset { display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosUnidadGrl fieldset fieldset{ display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosUnidadGrl .trAlinearDerecha { margin: 5px auto 0px auto; width: 98%; }
    #divDatosUnidadGrl .dvIzquierda { float: left; width: 49%; }
    #divDatosUnidadGrl .dvIzquierda .trAlinearDerecha { margin: 0px 0px 0px auto; }
    #divDatosUnidadGrl .dvDerecha { float: right; width: 49%; }
    #divDatosUnidadGrl .dvDerecha .trAlinearDerecha { margin: 0px auto 0px 0px; }
    #divDatosUnidadGrl fieldset fieldset .equiposAliados{ width: 96%; margin-top: 5px; margin-bottom: 13px; margin-left: 5%; margin-right: 5%; }
     
    #divDatosTarifa fieldset { display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosTarifa .trAlinearDerecha { margin: 5px auto 0px auto; width: 98%; }
    #divDatosTarifa .dvIzquierda { float: left; width: 49%; }
    #divDatosTarifa .dvIzquierda .trAlinearDerecha { margin: 0px 0px 0px auto; }
    #divDatosTarifa .dvDerecha { float: right; width: 49%; }
    #divDatosTarifa .dvDerecha .trAlinearDerecha { margin: 0px auto 0px 0px; }
    
    #divDatosCargos fieldset { display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosCargos .trAlinearDerecha { margin: 5px auto 0px auto; width: 98%; }
    #divDatosCargos .dvIzquierda { float: left; width: 49%; }
    #divDatosCargos .dvIzquierda .trAlinearDerecha { margin: 0px 0px 0px auto; }
    #divDatosCargos .dvDerecha { float: right; width: 49%; }
    #divDatosCargos .dvDerecha .trAlinearDerecha { margin: 0px auto 0px 0px; }
    .Grid
    {
        text-align: center;
    }    
</style>

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
    <div id="divDatosUnidadGrl" >
        <fieldset>
            <legend>INFORMACIÓN DE LA UNIDAD</legend>
            <div id="divDatosUnidad">
                <table class="trAlinearDerecha" style="margin: 0 auto; width: 100%;">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 120px;">ECODE</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 250px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" Columns="35"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="width: 190px; text-align: right;">SERIE UNIDAD</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtVIN" runat="server" Columns="35"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Marca</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtMarca" runat="server" Columns="35"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" align="right">Sucursal</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtUnidadSucursal" runat="server" Columns="35"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Modelo</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtModelo" runat="server" Columns="35"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" align="right">Capacidad Tanque</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtUnidadCapacidadTanque" runat="server" Columns="25" CssClass="CampoNumero"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdCentradoVertical">Año</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtAnio" runat="server" Columns="10" CssClass="CampoNumeroEntero"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" align="right">Placas Estatales</td>
                        <td class="tdCentradoVertical" style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtUnidadPlacasEstales" runat="server" Columns="25" ></asp:TextBox>
                        </td>
                    </tr>   
                </table>
            </div>
            <br />
            <fieldset>
                <legend>Equipos Aliados</legend>
                <div id="divAliados" class="equiposAliados">
                     <asp:GridView ID="grdEquiposAliados" runat="server" autoGenerateColumns="False" AllowPaging="True" PageSize="10"
                        AllowSorting="false"  EnableSortingAndPagingCallbacks="True" CssClass="Grid">
                        <Columns>
                            <asp:BoundField DataField="NumeroSerie" HeaderText="# Serie" SortExpression="NumeroVIN">
                                <HeaderStyle HorizontalAlign="Left" Width="180px" />
                                <ItemStyle HorizontalAlign="Left" Width="180px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Anio" HeaderText="A&ntilde;o" SortExpression="Anio">
                                <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Dimensiones" HeaderText="Dimensiones" SortExpression="Dimensiones">
                                <HeaderStyle HorizontalAlign="Left" Width="180px" />
                                <ItemStyle HorizontalAlign="Left" Width="180px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PBV" HeaderText="PBV" SortExpression="PBV">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PBC" HeaderText="FT" SortExpression="PBC">
                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" SortExpression="Modelo">
                                <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="GridRow" />
                        <HeaderStyle CssClass="GridHeader" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="GridPager" />
                        <SelectedRowStyle CssClass="GridSelectedRow" />
                        <AlternatingRowStyle CssClass="GridAlternatingRow" />
                        <EmptyDataTemplate>
			                <b>No se han asignado Equipos Aliados a la Unidad.</b>
		                </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </fieldset>
    </div>
    <br />
    <div id="divDatosTarifa">
        <fieldset>
            <legend>Tarifas</legend>    
            <div class="dvIzquierda">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">PERIODO</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:DropDownList runat="server" ID="ddlPeriodoTarifa" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodoTarifa_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdCentradoVertical"><span>*</span>Turno</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:DropDownList runat="server" ID="ddlTarifaTurno" AutoPostBack="true" OnSelectedIndexChanged="ddlTarifaTurno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Tarifa</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtTarifa" MaxLength="15" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                        </td>
                    </tr>
			        <tr>
                        <td class="tdCentradoVertical">&nbsp;</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtSeguro" MaxLength="15" Width="100px" Visible="false" CssClass="CampoNumero" readonly="true"></asp:TextBox>
                              </td>
                    </tr>
                </table>
            </div>
            <div class="dvDerecha">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span>Tipo Tarifa</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:DropDownList runat="server" ID="ddlTipoTarifa" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoTarifa_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>               
                    <tr>
                        <td class="tdCentradoVertical">Tarifa Hora Adicional</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtTarifaHoraAdicional" MaxLength="15" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
				        <td class="tdCentradoVertical">
					        &nbsp;Porcentaje Seguro</td>
				        <td style="width: 20px;">
					        &nbsp;
				        </td>
				        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtPorcentajeSeguro" MaxLength="15" Width="100px" readonly="true"
                                CssClass="CampoNumero"></asp:TextBox>&nbsp;&#37;
				        </td>
			        </tr>
                    <tr>
                        <td class="tdCentradoVertical"></td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" align="right">
                              <asp:Button runat="server" ID="btnPersonalizarTarifa" Text="Personalizar" CssClass="btnWizardEditar" OnClick="btnPersonalizarTarifa_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
    <br />
    <div id="divDatosCargos">
        <fieldset>
            <legend>Cargos Adicionales</legend>
            <div class="dvIzquierda">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">Maniobra</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 200px;">
                            <asp:TextBox runat="server" ID="txtManiobra" MaxLength="100" Width="100px" CssClass="CampoNumero" onkeypress="return ImporteTarifaHRAdicional(event, this);"></asp:TextBox>
                        </td>
                    </tr>            
                </table>
            </div>
        </fieldset>
    </div>
</div>
<asp:HiddenField ID="hdnUnidaID" runat="server" />
<asp:HiddenField ID="hdnEquipoID" runat="server" />
<asp:HiddenField ID="hdnPlazo" runat="server" />
<asp:HiddenField ID="hdnUnidadOperativaID" runat="server" />
<asp:HiddenField ID="hdnModeloID" runat="server" />
<asp:HiddenField ID="hdnSucursalID" runat="server" />
<asp:HiddenField ID="hdnCodigoMoneda" runat="server" />
<asp:HiddenField ID="hdnArea" runat="server" />
<asp:HiddenField ID="hdnTarifaPSLID" runat="server" />
<asp:HiddenField ID="hdnDuracionDiasPeriodo" runat="server" />
<asp:HiddenField ID="hdnMaximoHrsTurno" runat="server" />
<asp:HiddenField ID="hdnActivo" runat="server" />
<asp:HiddenField ID="hdnDevuelta" runat="server" />
<asp:HiddenField ID="hdnLineaOrigenIntercambioID" runat="server" />
<asp:HiddenField ID="hdnModuloID" runat="server" />
<asp:HiddenField ID="hdnUsuarioID" runat="server" />
<asp:HiddenField ID="hdnCuentaClienteID" runat="server" />
<asp:HiddenField ID="hdnPorcentajeMaxDescuentoTar" runat="server" />
<asp:HiddenField ID="hdnPorcentajeDescuentoTar" runat="server" />
<asp:HiddenField ID="hdnTarifaConDescuento" runat="server" />
<asp:HiddenField ID="hdnTarifaEtiqueta" runat="server" />
<asp:HiddenField ID="hdnModoRegistro" runat="server" Value="" />
<asp:HiddenField ID="hdnShowDialogModal" runat="server" value="0"/>
<asp:Button ID="btnActualizaTarifa" runat="server" OnClick="btnActualizaTarifa_Click" Text="btnActualizaTarifa" Style="display: none;" />