<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSiniestroSeguroUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucSiniestroSeguroUI" %>
<script type="text/javascript">
    $(document).ready(function () {
        initChild();
    });
    function initChild() {
        if (!$('#<%=txtFecha.ClientID %>').is(':disabled')) {
            $('#<%=txtFecha.ClientID %>').datepicker({ yearRange: '-100:+0',
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha del siniestro",
                showOn: 'button'
            });
        }
        $('.CampoFecha').attr('readonly', true);
    }
</script>
<fieldset>
    <legend>Siniestro</legend>
    <table id="siniestro" class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>#
            </td>
            <td style="width: 20px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtNumero" runat="server" CssClass="CampoNumeroEntero" ToolTip="Número de siniestro"
                    MaxLength="9" Width="275px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>FECHA
            </td>
            <td style="width: 20px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtFecha" runat="server" CssClass="CampoFecha" ToolTip="Fecha del siniestro"
                    Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>DESCRIPCI&Oacute;N
            </td>
            <td style="width: 20px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtDescripcion" runat="server" ToolTip="Descripción" MaxLength="250"
                    Width="275px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>ESTATUS
            </td>
            <td style="width: 20px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtEstatus" runat="server" ToolTip="Estatus" MaxLength="30" Width="275px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnAgregar" runat="server" Text="AGREGAR" CssClass="btnAgregarATabla"
                    OnClick="btnAgregar_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<div id="siniestros">
    <asp:UpdatePanel runat="server" ID="updGirdSiniestros" ChildrenAsTriggers="True">
        <ContentTemplate>
            <asp:GridView ID="grdSiniestros" runat="server" AutoGenerateColumns="False" CssClass="Grid"
                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="grdSiniestros_PageIndexChanging"
                OnRowCommand="grdSiniestros_RowCommand" align="center" OnRowDataBound="grdSiniestros_RowDataBound"
                Style="margin-top: 0px">
                <Columns>
                    <asp:BoundField HeaderText="SiniestroID" DataField="SiniestroID" Visible="false" />
                    <asp:BoundField HeaderText="#" DataField="Numero">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:d}">
                        <ItemStyle Width="95px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Descripción" ItemStyle-HorizontalAlign="Justify">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDescripcion" Text='<%# DataBinder.Eval(Container, "DataItem.Descripcion").ToString()%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estatus" ItemStyle-HorizontalAlign="Justify">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEstatusSiniestro" Text='<%#DataBinder.Eval(Container,"DataItem.Estatus").ToString() %>'></asp:Label>
                            <asp:TextBox runat="server" ID="txtEstatusSiniestro" ReadOnly="True" MaxLength="30"
                                Text='<%#DataBinder.Eval(Container,"DataItem.Estatus").ToString() %>' Visible="False"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                CommandName="Eliminar" CommandArgument='<%#Container.DataItemIndex%>' ToolTip="Eliminar" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtEditar" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png"
                                CommandName="Editar" CommandArgument='<%#Container.DataItemIndex%>' ToolTip="Editar" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtAceptar" ImageUrl="~/Contenido/Imagenes/GUARDAR-ICO.png"
                                CommandName="Aceptar" CommandArgument='<%#Container.DataItemIndex%>' ToolTip="Aceptar"
                                Visible="False" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:HiddenField ID="hdnTramiteID" runat="server" />
<asp:HiddenField ID="hdnSiniestroID" runat="server" />
