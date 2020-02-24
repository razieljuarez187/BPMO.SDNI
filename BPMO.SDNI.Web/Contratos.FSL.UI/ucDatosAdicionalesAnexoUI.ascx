<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosAdicionalesAnexoUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.ucDatosAdicionalesAnexoUI" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.FSL.BO" %>
<%-- Satisface al caso de uso CU015 - Registrar Contrato FULL SERVICE LEASING --%>
<%-- Satisface al Caso de Uso CU022 - Consultar Contrato Full Service Leasing --%>
<%-- Satisface al caso de uso CU023 - Editar Contrato FULL SERVICE LEASING --%>
<script type="text/javascript">
    function <%= ClientID %>_Inicializar() {
        $('#<%= txtDescripcion.ClientID %>').blur(<%= ClientID %>_RecortarDescripcion);
        $('#<%= txtDescripcion.ClientID %>').keyup(<%= ClientID %>_RecortarDescripcion);
    }

    function <%= ClientID %>_RecortarDescripcion() {
        var Descripcion = $('#<%= txtDescripcion.ClientID %>');


        if ($.trim(Descripcion.val()).length > <%= txtDescripcion.MaxLength %>)
            Descripcion.val($.trim(Descripcion.val()).substring(0, <%= txtDescripcion.MaxLength %> - 1));
    }

</script>
<div id="divDetalle" runat="server">
    <table class="trAlinearDerecha" style="width: 100%;">
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>Título
            </td>
            <td style="width: 5px;">&nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtTitulo" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
            </td>
            <td style="width: 5px;">&nbsp;
            </td>
            <td align="right">
                <asp:CheckBox ID="cbEsObservacion" runat="server" />
            </td>
            <td style="width: 5px;">&nbsp;
            </td>
            <td class="tdCentradoVertical">
                ¿Es Observación?
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px;">
                <span>*</span>Descripción</td>
            <td style="width: 5px;">&nbsp;
            </td>
            <td class="tdCentradoVertical" colspan="5">
                <asp:TextBox ID="txtDescripcion" runat="server" Width="350px" MaxLength="300" TextMode="MultiLine" Rows="3" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="7" align="right" style="padding-right:20px;">
                <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btnWizardGuardar" OnClick="btnActualizar_Click"></asp:Button>
                <asp:Button ID="btnAgregarDatoAdicional" runat="server" Text="Agregar a Tabla" CssClass="btnAgregarATabla" OnClick="btnAgregarDatoAdicional_Click"></asp:Button>
                <asp:Button ID="btnCancelarDatoAdicional" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelarDatoAdicional_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnDatoAdicionalID" runat="server" />
    <asp:HiddenField ID="hdnModoConsultar" runat="server" />
</div>
        <asp:GridView ID="grdDatosAdicionales" runat="server" AutoGenerateColumns="False"
            CellPadding="4" GridLines="None" CssClass="Grid" PageSize="10" AllowPaging="True"
            AllowSorting="True" OnRowCommand="grdDatosAdicionales_RowCommand"
            OnPageIndexChanging="grdDatosAdicionales_PageIndexChanging" Width="95%" Style="margin: 10px auto;">
            <Columns>
                <asp:TemplateField HeaderText="Título" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTitulo" Text='<%# DataBinder.Eval(Container.DataItem, "Titulo") %>'
                            Width="100%"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="250" HorizontalAlign="Justify" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descripción" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDescripcion" Text='<%# ((DatoAdicionalAnexoBO)Container.DataItem).Descripcion.Substring(0, ((((DatoAdicionalAnexoBO)Container.DataItem).Descripcion.Length >100)? 100: ((DatoAdicionalAnexoBO)Container.DataItem).Descripcion.Length)) %>'
                            Width="100%"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="350" HorizontalAlign="Justify" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Observación">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="cbObservacion" Checked='<%#((DatoAdicionalAnexoBO)Container.DataItem).EsObservacion == true %>' Enabled="False" Width="17px" />
                    </ItemTemplate>
                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                            CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
                    </ItemTemplate>
                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="ibtEditar" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png" ToolTip="Editar"
                            CommandName="CMDEDITAR" CommandArgument='<%#Container.DataItemIndex%>'/>
                    </ItemTemplate>
                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
                            CommandName="CMDDETALLES" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                    </ItemTemplate>
                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridHeader" />
            <EditRowStyle CssClass="GridAlternatingRow" />
            <PagerStyle CssClass="GridPager" />
            <RowStyle CssClass="GridRow" />
            <FooterStyle CssClass="GridFooter" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
            <EmptyDataTemplate>
                <b>No se han agregado Datos Adicionales.</b>
            </EmptyDataTemplate>
        </asp:GridView>
<asp:HiddenField ID="hdnContratoID" runat="server" />