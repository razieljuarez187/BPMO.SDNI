<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCargosAdicionalesCierrePSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucCargosAdicionalesCierrePSLUI" %>

<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagName="ucEquiposAliadosLineasUI" TagPrefix="uc" %>

<style type="text/css">    
    #divDatosUnidadGrl fieldset { display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosUnidadGrl fieldset fieldset{ display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    #divDatosUnidadGrl .trAlinearDerecha { margin: 5px auto 0px auto; width: 98%; }
    #divDatosUnidadGrl .dvIzquierda { float: left; width: 49%; }
    #divDatosUnidadGrl .dvIzquierda .trAlinearDerecha { margin: 0px 0px 0px auto; }
    #divDatosUnidadGrl .dvDerecha { float: right; width: 49%; }
    #divDatosUnidadGrl .dvDerecha .trAlinearDerecha { margin: 0px auto 0px 0px; }
    
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
            width: 90%;
            margin: 15px auto 15px auto;
        }
    
</style>
<div id="DatosOrden">
    <div id="divInformacionGeneral" class="GroupHeader">
        
        <div class="GroupHeaderOpciones Ancho2Opciones">
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btnWizardGuardar" ValidationGroup="Requeridos"
                OnClientClick="ValidatePage('agregar la Unidad al Contrato: Tarifas Adicionales a la Unidad');"
                OnClick="btnAgregar_Click" ViewStateMode="Enabled" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" CausesValidation="false" OnClick="btnCancelar_Click" />
        </div>
    </div>

    <div style="margin-top: 15px; margin-bottom: 15px;">
        <div class="GroupHeader">
            <asp:Label ID="lblTituloEquipoAliado" runat="server" Text="INFORMACI&Oacute;N DE EQUIPOS ALIADOS"></asp:Label>
        </div>
        <div id="divAliados">
            <div id="div1" class="equiposAliados">
                <asp:GridView ID="grdEquiposAliados" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                    PageSize="10" AllowSorting="false" EnableSortingAndPagingCallbacks="True" CssClass="Grid">
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
        </div>
    </div>

    <div id="importesKmHrs" runat="server">
        <div class="GroupHeader">
            <asp:Label ID="lblImportes" runat="server" Text="IMPORTES HORAS"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom: 5px;">
            <tr>
                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" style="width: 200px" align="right">Hrs Entrega</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsEqRefEntrega" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" align="right">Hrs Recepci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsEqRefRecepcion" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" align="right">Total Hrs</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtTotHrsEqRefrigeracion" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>        
            <tr>
                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" align="right"><span>*</span>Hrs Libres</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsLibres" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>  
            <tr>                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" align="right">Hrs Excedidas</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsExcedidas" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" align="right">Tarifa Hr Exc</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtTarifaHrExc" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    &nbsp;</td>
                <td class="tdCentradoVertical" align="right">Monto Hrs</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtMontoHrsEqRef" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="cargosAdicionales" runat="server">
        <div class="GroupHeader">
            <asp:Label ID="lblCargosAdicionales" runat="server" Text="CARGOS ADICIONALES"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom: 5px;">
            <tr>
                <td class="tdCentradoVertical" style="width: 210px">Diferencia Combustible</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCombustibleSalida" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 230px" align="right"><span>*</span>Importe Unidad Combustible</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtImporteUnidadCombustible" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical">Importe Total Combustible</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtImporteTotCombustible" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Cargo Abuso de Operaci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCargoAbusoOperacion" runat="server" Width="175px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical" colspan="4">Cargo por Disposici&oacute;n de Basura o Art&iacute;culos olvidados en el Veh&iacute;culo</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCargoDispBasura" runat="server" Width="175px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="diasRenta" runat="server">
        <div class="GroupHeader">
            <asp:Label ID="lblDiasRenta" runat="server" Text="DÍAS RENTA"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom:5px;">
            <tr>
                <td class="tdCentradoVertical" style="width: 240px">D&iacute;as Renta Programada</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtNumDiasRentProg" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 200px" align="right">D&iacute;as Reales Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDiasRealesRenta" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical">D&iacute;as en Taller Durante Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDiasUnidadTaller" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">D&iacute;as Adicionales Cobro</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDiasAdicionalesCobro" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Monto Total D&iacute;as Adicionales</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:TextBox ID="txtTotalDiasAdicionales" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
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
<asp:HiddenField ID="hdnDuracionMes" runat="server" />
<asp:HiddenField ID="hdnMaximoHrsTurno" runat="server" />
<asp:HiddenField ID="hdnFactorTurno" runat="server" />