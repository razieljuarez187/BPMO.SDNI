<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSeguroDetalleUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucSeguroDetalleUI" %>

<fieldset id="generales">
    <legend>Generales</legend>
    <table class="trAlinearDerecha" style="margin: 0px auto;">
        <tr>
            <td class="tdCentradoVertical">VIN</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtVIN" runat="server" ToolTip="Número de Serie" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Modelo</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtModelo" runat="server" ToolTip="Modelo" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"># P&oacute;liza</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtNumPoliza" runat="server" ToolTip="Numero de poliza" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Aseguradora</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtAseguradora" runat="server" ToolTip="Aseguradora" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Tel&eacute;fono</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtContacto" runat="server" ToolTip="Contacto" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Prima Anual</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtPrimaAnual" runat="server" CssClass="CampoMoneda" ToolTip="Prima Anual" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Suma Endosos</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtSumaEndosos" runat="server" CssClass="CampoMoneda" ToolTip="Suma de Endosos" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">PRIMA ANUAL TOTAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtPrimaAnualTotal" runat="server" CssClass="CampoMoneda" ToolTip="Prima Anual Total" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">PRIMA SEMESTRAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtPrimaSemestral" runat="server" CssClass="CampoMoneda" ToolTip="Prima Semestral" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">VIGENCIA INICIAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtVigenciaInicial" runat="server" CssClass="CampoFecha" ToolTip="Vigencia Inicial" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">VIGENCIA FINAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtVigenciaFinal" runat="server" CssClass="CampoFecha" ToolTip="Vigencia Final" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">OBSERVACIONES</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtObservacion" runat="server" ToolTip="Observaciones" TextMode="MultiLine" Width="270px" MaxLength="500"
                style="max-width: 270px; min-width: 270px; max-height:50px; min-height:50px"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset> 

<div id="tabs">
    <ul>
		<li><a href="#tabs-1">Endosos</a></li>
		<li><a href="#tabs-2">Deducibles</a></li>
		<li><a href="#tabs-3">Siniestros</a></li>
	</ul>
    <div id="tabs-1">
        <div id="endosos" align="center">
            <asp:GridView ID="grdEndosos" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField HeaderText="EndosoID" DataField="EndosoID" visible="false" />
                    <asp:BoundField HeaderText="Motivo" DataField="Motivo" />
                    <asp:BoundField HeaderText="Importe" DataField="Importe" 
						DataFormatString="{0:#,##0.0000}" >
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <RowStyle CssClass="GridRow" />
                <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                <PagerStyle CssClass="GridPager" />
                <SelectedRowStyle CssClass="GridSelectedRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
            </asp:GridView>
        </div>
    </div>
    <div id="tabs-2">
        <div id="deducibles" align="center">
            <asp:GridView ID="grdDeducibles" runat="server" AutoGenerateColumns="False"  
                CssClass="Grid" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField HeaderText="DeducibleID" DataField="DeducibleID" visible="false" />
                    <asp:BoundField HeaderText="CONCEPTO" DataField="Concepto" />
                    <asp:BoundField HeaderText="PORCENTAJE" DataField="Porcentaje" 
						DataFormatString="{0:n2}" >
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    </Columns>
                <RowStyle CssClass="GridRow" />
                <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                <PagerStyle CssClass="GridPager" />
                <SelectedRowStyle CssClass="GridSelectedRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
            </asp:GridView>
        </div>
    </div>            
    <div id="tabs-3">
        <div id="siniestros" align="center">
            <asp:GridView ID="grdSiniestros" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="True" AllowSorting="True">
                <Columns>
                    <asp:BoundField HeaderText="SiniestroID" DataField="SiniestroID" visible="false" />
                    <asp:BoundField HeaderText="#" DataField="Numero" >
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:d}">
                        <ItemStyle Width="95px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Descripción" DataField="Descripcion">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Estatus" DataField="Estatus" />
                </Columns>
                <RowStyle CssClass="GridRow" />
                <HeaderStyle CssClass="GridHeader" />
                <FooterStyle CssClass="GridFooter" />
                <PagerStyle CssClass="GridPager" />
                <SelectedRowStyle CssClass="GridSelectedRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
            </asp:GridView>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdnTramiteID" runat="server" />
<asp:HiddenField ID="hdnTramitableID" runat="server" />
<asp:HiddenField ID="hdnTipoTramitable" runat="server" />
