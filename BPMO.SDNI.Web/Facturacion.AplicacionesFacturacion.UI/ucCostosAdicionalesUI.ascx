<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCostosAdicionalesUI.ascx.cs"
    Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ucCostosAdicionalesUI" %>

<%--Satisface el caso de uso CU005 – Armar Paquetes Facturacion--%>
<%--Satisface al Reporte de Inconsistencia RI0024--%>
<%--BEP1401 Satisface a la SC0034--%>
<script language="javascript" type="text/javascript">
    function BtnBuscarCostos(guid, xml) {
        var width = ObtenerAnchoBuscador(xml);

        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#<%=btnResultCostos.ClientID %>"),
            features: {
                dialogWidth: width,
                dialogHeight: '320px',
                center: 'yes',
                maximize: '0',
                minimize: 'no'
            }
        });
    }
</script>
<asp:Panel ID="pnlContenedor" runat="server" CssClass="Contenedor">
    <table class="trAlinearDerecha" style="margin: 0px auto; width: 700px; display: inherit;
        border: 1px solid transparent;">
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>Moneda
            </td>
            <td class="separadorCampo">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" colspan="5" style="width: 330px;">
                <%--SC0034, Deshabilitar el combo de Moneda y llenarlo con las monedas existentes--%>
                <asp:DropDownList ID="ddlMoneda" Width="200px" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlMoneda_SelectedIndexChanged">
                    <asp:ListItem Value="MXN" Text="PESOS"></asp:ListItem>
                    <asp:ListItem Value="-1" Text="MONEDA DE CONTRATO"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <table width="100%">
                    <tr>
                        <td style="white-space:nowrap">
                             Costos Adicionales
                        </td>
                        <td style="width:78%">
                            <hr style="width:100%; border-style:solid; border-width:thin" />
                        </td>
                    </tr>
                </table>
            </td>      
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                Concepto
            </td>
            <td class="separadorCampo">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:DropDownList ID="ddlConcepto" runat="server">
                </asp:DropDownList>
            </td>
            <td class="separadorCampo">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                Precio
            </td>
            <td class="separadorCampo">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtPrecio" Width="70px" CssClass="CampoMoneda"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtPrecio" runat="server" 
                    ValidationGroup="CostosAdicionales" Text="*" 
                    ToolTip="El precio unitario es requerido" 
                    ErrorMessage="El precio unitario es requerido" ControlToValidate="txtPrecio" 
                    Display="Dynamic"></asp:RequiredFieldValidator>

                <%--RI0024, Adición de Validador para que el precio sea mayor a cero--%>
                <asp:CompareValidator runat="server" 
                    ValidationGroup="CostosAdicionales" Text="*" 
                    ToolTip="El precio unitario debe de ser mayor a cero" 
                    ErrorMessage="El precio unitario debe de ser mayor a cero" ControlToValidate="txtPrecio"                     
                    Display="Dynamic" Operator="GreaterThan" Type="Double" 
                    ValueToCompare="0.0"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>Producto o Servicio</td>
            <td class="separadorCampo">&nbsp;</td>
            <td class="tdCentradoVertical" colspan="5">
                <asp:TextBox ID="txtClaveProductoServicio" runat="server" Columns="32" MaxLength="25"
                        OnTextChanged="txtClaveProductoServicio_TextChanged" AutoPostBack="True"></asp:TextBox>
                <asp:ImageButton ID="ibtnBuscarProductoServicio" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                        OnClick="ibtnBuscarProductoServicio_Click" Style="width: 17px" />
                <asp:TextBox ID="txtDescripcionProductoServicio" runat="server" Width="250px" MaxLength="100" ReadOnly="true"
                        Enabled="false" style="margin:0 10px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                Descripción
            </td>
            <td class="separadorCampo">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtDescripcion" Width="375px" MaxLength="500" TextMode="MultiLine" style="text-transform:uppercase; resize: none;"
                    Rows="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtDescripcion" runat="server" 
                    ValidationGroup="CostosAdicionales" Text="*" 
                    ToolTip="La descripción es requerida" 
                    ErrorMessage="La descripción es requerida" ControlToValidate="txtDescripcion" 
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="separadorCampo">&nbsp;</td>
            <td class="tdCentradoVertical">&nbsp;</td>
            <td class="separadorCampo">&nbsp;</td>
            <td class="tdCentradoVertical">&nbsp;</td>
        </tr>
        <tr>
            <td class="style1">&nbsp;</td>
            <td class="style1">&nbsp;</td>
            <td class="style1">&nbsp;</td>
            <td class="style1">&nbsp;</td>
            <td class="style1">&nbsp;</td>
            <td class="style1">&nbsp;</td>
            <td class="style1">
              <asp:Button ID="btnAgregar" runat="server" Text="Agregar" ValidationGroup="CostosAdicionales" CssClass="btnAgregarMediano"  OnClick="btnAgregar_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="grvCostosAdicionales" runat="server" CssClass="Grid97Percent" AutoGenerateColumns="False"
        OnRowCommand="grvCostosAdicionales_RowCommand" 
        onrowdatabound="grvCostosAdicionales_RowDataBound" 
        ShowHeaderWhenEmpty="True" 
        EmptyDataText="No han costos adicionales agregados">
        <EmptyDataTemplate>
            <span>No hay costos adicionales agregados</span>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="#">                
                <ItemTemplate>
                    <%# (++this.numeroCostosAdicionales) %>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>

             <asp:TemplateField HeaderText="Concepto">                
                <ItemTemplate>
                    <asp:Label ID="lblTipoRenglon" runat="server" Text=""></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Descripción">                
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Articulo.Nombre")%>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Precio">
                <ItemTemplate>
                    <%# string.Format("{0:#,##0.00}", DataBinder.Eval(Container.DataItem, "PrecioUnitario"))%>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>


            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                        ToolTip="Eliminar" CommandName="Eliminar" CommandArgument='<%# Bind("Id") %>' />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
        </Columns>
        <RowStyle CssClass="GridRow" />
        <HeaderStyle CssClass="GridHeader" />
        <FooterStyle CssClass="GridFooter" />
        <PagerStyle CssClass="GridPager" />
        <SelectedRowStyle CssClass="GridSelectedRow" />
        <AlternatingRowStyle CssClass="GridAlternatingRow" />
    </asp:GridView>
    <asp:Button ID="btnResultCostos" runat="server" Text="Button" OnClick="btnResultCostos_Click" Style="display: none;" />
    <%--Campos ocultos--%>
    <asp:HiddenField runat="server" ID="hdnProductoServicioId" Value="" />
</asp:Panel>
