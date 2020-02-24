<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContactoCliente.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucContactoCliente" %>
<script type="text/javascript">
    function BtnBuscar(guid, xml) {
        var width = ObtenerAnchoBuscador(xml);
        
        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#<%=btnResult.ClientID %>"),
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
<fieldset style="min-width: 100% !important;">
    <legend>Contacto Cliente</legend>
    <table class="trAlinearDerecha table-responsive" style="margin: 0px auto 10px auto; width: 100%; border: 1px solid transparent;">
        <tr>
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>Cliente</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="input-text-responsive" AutoPostBack="true" OnTextChanged="txtNombreCliente_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="btnBuscarCliente" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="CONSULTAR CLIENTES" OnClick="btnBuscarCliente_Click" />
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>Sucursal del cliente</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtSucursal" runat="server" CssClass="input-text-responsive" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="btnBuscarSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="CONSULTAR SUCURSALES" OnClick="btnBuscarSucursal_Click" />
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>Direcci&oacute;n de la sucursal del Cliente:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtDireccion" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr style="display:none;">
            <td>
                <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
                <asp:HiddenField ID="hdnLibroActivos" runat="server" />
            </td>
        </tr>
    </table>
    <div class="ContenedorMensajes">
        <span class="Requeridos"></span>
    </div>
</fieldset>
<fieldset>
    <legend>Detalles de Contacto Cliente</legend>
    <table class="trAlinearDerecha table-responsive" style="margin: 0px auto 10px auto; width: auto; border: 1px solid transparent;">
        <tr id="trNombre" runat="server">
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>Nombre Contacto/Puesto: </td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtNombreContacto" runat="server" CssClass="input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr id="trTelefono" runat="server">
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>Tel&eacute;fono:</td>




            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>
            </td>
        </tr>
        <tr id="trCorreo" runat="server">
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>Correo:</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:TextBox ID="txtCorreo" runat="server" 
                    CssClass="input-text-responsive Correo InputReset" type="email" name="email"></asp:TextBox>
            </td>
        </tr>
        <tr id="trEnviarCorreo" runat="server">
            <td class="tdCentradoVertical input-label-responsive"><span>*</span>¿Env&iacute;ar correo?</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:CheckBox ID="cbEnvioCorreo" runat="server"/>
            </td>
        </tr>
        <tr id="trAgregarATabla" runat="server">
            <td class="tdCentradoVertical input-label-responsive"></td>
            <td class="input-space-responsive">&nbsp;</td>
            <td class="tdCentradoVertical input-group-responsive">
                <asp:Button ID="btnAgregarATabla" runat="server" Text="Agregar a Tabla" CssClass="btnAgregarATabla" 
                    OnClick="AgregarATabla_Click"/>
            </td>
        </tr>
    </table>
    <div id="divCamposRequeridosDetalles" class="ContenedorMensajes" runat="server">
        <span class="Requeridos" runat="server"></span>
    </div>
    <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
            <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="true" PageSize="10" EnableViewState="False" OnRowCommand="gvDetalles_RowCommand"
                    OnPageIndexChanging="gvDetalles_PageIndexChanging" ShowHeaderWhenEmpty="True">
                    <Columns>                   
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="NOMBRE/PUESTO">
                            <ItemTemplate>
                                    <asp:Label ID="lbNombre" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Nombre") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="TEL&Eacute;FONO">
                            <ItemTemplate>
                                    <asp:Label ID="lbTelefono" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Telefono") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="CORREO">
                            <ItemTemplate>
                                    <asp:Label ID="lbCorreo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Correo") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="50px" HeaderText="ENV&Iacute;AR CORREO">
                            <ItemTemplate>
                                <asp:Label ID="lbEnvioCorreo" runat="server" Width="60px"><%# DataBinder.Eval(Container, "DataItem.RecibeCorreoElectronico").ToString().Replace("True", "SI").Replace("False", "NO") %></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
					    <asp:TemplateField ItemStyle-Width="58px" HeaderText="&nbsp;Eliminar&nbsp;" >
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEliminar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="" ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="58px" HeaderText="&nbsp;Editar&nbsp;">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEditar" CommandName="Editar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png" ToolTip="" ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                        <EmptyDataTemplate>
                        <b id="GridVacio">No se han agregado detalles de contacto cliente</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </fieldset>