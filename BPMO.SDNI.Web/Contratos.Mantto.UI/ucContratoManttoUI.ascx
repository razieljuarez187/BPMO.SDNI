<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContratoManttoUI.ascx.cs"
    Inherits="BPMO.SDNI.Contratos.Mantto.UI.ucContratoManttoUI" %>
<%@ Import Namespace="BPMO.SDNI.Comun.BO" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.Mantto.BO" %>
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
        var fechaContrato = $('#<%= txtFechaContrato.ClientID %>');
        if (fechaContrato.length > 0) {
            if (fechaContrato.attr("disabled") != false && fechaContrato.attr("disabled") != "disabled") {
                fechaContrato.datepicker({
                    yearRange: '-10:+7',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha del contrato",
                    showOn: 'button',
                    defaultDate: (fechaContrato.val().length == 10) ? fechaContrato.val() : new Date()
                });

                fechaContrato.attr('readonly', true);
            }
        }
        
        var fechaInicioContrato = $('#<%= txtFechaInicioContrato.ClientID %>');
        if (fechaInicioContrato.length > 0) {
            if (fechaInicioContrato.attr("disabled") != false && fechaInicioContrato.attr("disabled") != "disabled") {
                fechaInicioContrato.datepicker({
                    yearRange: '-10:+7',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha de Inicio del contrato",
                    showOn: 'button',
                    defaultDate: (fechaInicioContrato.val().length == 10) ? fechaInicioContrato.val() : new Date()
                });

                fechaInicioContrato.attr('readonly', true);
            }
        }
    }
</script>
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

    function abrirDetalleRepresentantes(tipo) {
        var titulo = '';
        switch (tipo) {
            case 'OBLIGADO SOLIDARIO':
                titulo = 'REPRESENTANTES LEGALES DEL OBLIGADO SOLIDARIO';
                break;
            case 'AVAL':
                titulo = 'REPRESENTANTES LEGALES DEL AVAL';
                break;
        }

        $("#DialogRepresentantes").dialog({
            modal: true,
            width: 900,
            height: 400,
            resizable: false,
            title: titulo,
            buttons: {
                "Aceptar": function () {
                    $(this).dialog("destroy");
                }
            }
        });
        $("#DialogRepresentantes").parent().appendTo("form:first");
    }

    function abrirTarifasEquipoAliado() {
        $("#dvTarifaEquipoAliado").dialog({
            modal: true,
            width: 630,
            height: 270,
            resizable: false,
            close: function () { $(this).dialog("destroy"); }
        });
        $("#dvTarifaEquipoAliado").parent().appendTo("form:first");
    }
    
</script>
<fieldset>
    <legend>Informaci&oacute;n General</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Fecha Contrato
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtFechaContrato" runat="server" MaxLength="11" CssClass="CampoFecha"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    Empresa
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtEmpresa" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdnEmpresaID" />
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px">
                    Domicilio Empresa
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDireccionEmpresa" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                        Enabled="false" MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px;
                        max-height: 90px; min-height: 90px;"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Sucursal
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical" style="width: 300px;">
                    <asp:TextBox ID="txtSucursal" runat="server" Width="250px" MaxLength="80" AutoPostBack="true"
                        OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                        ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                        OnClick="ibtnBuscaSucursal_Click" />
                    <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    Representante
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtRepresentante" runat="server" MaxLength="250" Width="250px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Moneda
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlMonedas" AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<fieldset>
    <legend>Datos del Cliente</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span><label>Cliente</label></td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:HiddenField runat="server" ID="hdnClienteID" />
                    <asp:HiddenField runat="server" ID="hdnTipoCuentaRegion" />
                    <asp:HiddenField runat="server" ID="hdnCuentaClienteID" />
                    <asp:TextBox ID="txtNombreCliente" runat="server" Width="250px" MaxLength="150" AutoPostBack="True"
                        OnTextChanged="txtNombreCliente_TextChanged"></asp:TextBox>
                    <asp:ImageButton ID="ibtnBuscarCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png"
                        ToolTip="Consultar Cliente" OnClick="ibtnBuscarCliente_Click" />
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><label>¿Es Físico?</label></td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:RadioButtonList ID="rbtEsFisico" runat="server" RepeatDirection="Horizontal"
                        Enabled="false" Style="margin-left: 0px;">
                        <asp:ListItem Value="True" Text="SI"></asp:ListItem>
                        <asp:ListItem Value="False" Text="NO"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><label>Cuenta Oracle</label></td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:TextBox runat="server" ID="txtNumeroCuentaOracle" Width="250px" Enabled="False"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td align="right" style="padding-top: 5px">
                    <span>*</span>Direcci&oacute;n
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:HiddenField runat="server" ID="hdnCalle" />
                    <asp:HiddenField runat="server" ID="hdnColonia" />
                    <asp:HiddenField runat="server" ID="hdnCiudad" />
                    <asp:HiddenField runat="server" ID="hdnEstado" />
                    <asp:HiddenField runat="server" ID="hdnMunicipio" />
                    <asp:HiddenField runat="server" ID="hdnPais" />
                    <asp:TextBox ID="txtDomicilioCliente" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                        MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
                        min-height: 90px;" Width="310px" Text="" Enabled="false"></asp:TextBox>
                    <asp:ImageButton ID="ibtnBuscarDirieccionCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png"
                        ToolTip="Consultar Direcci&oacute;n" OnClick="ibtnBuscarDirieccionCliente_Click">
                    </asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    C&oacute;digo Postal
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:TextBox ID="txtCodigoPostal" runat="server" Width="60px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pnlPersonasCliente" runat="server" Style="display: table; width: 100%;">
        <asp:Panel ID="pnlRepresentantesLegales" runat="server">
            <div style="display: table; margin: auto; width: 95%;">
                <asp:CheckBox runat="server" ID="cbSoloRepresentantes" Text="Solo Representantes Legales"
                    OnCheckedChanged="cbSoloRepresentantes_CheckedChanged" AutoPostBack="True" />
                <br />
                <asp:Label runat="server" ID="lblMensajeSoloRepresentantes" Font-Size="8pt" CssClass="ColorValidator"
                    Text="Esta opción solo despliega representantes legales en el contrato y sus anexos."></asp:Label>
            </div>
            <div style="display: inline;">
                <fieldset>
                    <legend>Representantes legales</legend>
                    <table style="margin-left: 0px; margin-right: auto;" id="tbRepresentanteLegal" runat="server">
                        <tr>
                            <td class="tdCentradoVertical" align="right" style="width: 210px;">
                                <span>*</span> Representante Legal&nbsp; &nbsp;
                            </td>
                            <td class="tdCentradoVertical" align="right">
                                <asp:UpdatePanel ID="updRepresentantesLegales" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlRepresentantesLegales" runat="server" Enabled="False" AppendDataBoundItems="true"
                                            Width="340px">
                                            <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical" align="right">
                                <asp:UpdatePanel ID="updAgregarRepresentante" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnAgregarRepresentante" CssClass="btnAgregarATabla" runat="server"
                                            Text="Agregar a Tabla" OnClick="btnAgregarRepresentante_Click" Enabled="False" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <div style="padding-top: 10px;">
                        <asp:GridView ID="grdRepresentantesLegales" runat="server" AutoGenerateColumns="False"
                            CssClass="Grid" PageSize="5" AllowPaging="True" AllowSorting="False" CellPadding="4"
                            GridLines="None" OnRowCommand="grdRepresentantesLegales_RowCommand" OnPageIndexChanging="grdRepresentantesLegales_PageIndexChanging">
                            <Columns>
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                            ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
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
                                <b>No se han asignado Representantes Legales.</b>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlObligadosSolidarios" runat="server">
            <div style="margin: auto; width: 95%;">
                <asp:CheckBox runat="server" ID="cbObligadosComoAvales" Text="Obligados Solidarios como Avales"
                    OnCheckedChanged="cbObligadosComoAvales_CheckedChanged" AutoPostBack="True" />
                <br />
                <asp:Label runat="server" ID="lblMensajeObligadosComoAvales" Font-Size="8pt" CssClass="ColorValidator"
                    Text="Esta opción hace que TODOS los obligados solidarios del contrato se usen como avales"></asp:Label>
                <br />
                <asp:Label runat="server" Font-Size="8pt" ID="lblMensajeOblS" CssClass="ColorValidator"
                    Text="En caso de no seleccionar Obligados Solidarios, El o los Representantes Legales del cliente tomarian dicha obligación."></asp:Label>
            </div>
            <fieldset>
                <legend>Obligados Solidarios</legend>
                <table style="margin-left: 0px; margin-right: auto;" id="tbObligadoSolidario" runat="server">
                    <tr>
                        <td class="tdCentradoVertical" align="right" style="width: 210px;">
                            <span>*</span> Obligado Solidario&nbsp; &nbsp;
                        </td>
                        <td style="padding-top: 5px;">
                            <asp:UpdatePanel ID="updObligadosSolidarios" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlObligadosSolidarios" runat="server" Enabled="False" Width="340px"
                                        AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlObligadosSolidarios_SelectedIndexChanged">
                                        <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlObligadosSolidarios" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:UpdatePanel ID="updAgregarObligado" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnAgregarObligadoSolidario" runat="server" CssClass="btnAgregarATabla"
                                        Enabled="False" OnClick="btnAgregarObligadoSolidario_Click" Text="Agregar a Tabla" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trRepresentantesObligado" runat="server">
                        <td align="right" style="width: 210px; padding-top: 5px;">
                            <span>*</span> Representantes Legales&nbsp; &nbsp;
                        </td>
                        <td colspan="3" style="padding-top: 5px;">
                            <asp:GridView runat="server" ID="grdRepresentantesObligadoSolidario" AutoGenerateColumns="False"
                                CssClass="Grid" Style="margin-left: 0px; margin-right: auto;" CellPadding="4"
                                GridLines="None" OnPageIndexChanging="grdRepresentantesObligadoSolidario_PageIndexChanging"
                                OnRowDataBound="grdRepresentantesObligadoSolidario_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            #</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRepresentanteOSID" Text='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                Width="30px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkRepOS" AutoPostBack="True" OnCheckedChanged="chkRepresentanteOS_CheckedChanged">
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <EditRowStyle CssClass="GridAlternatingRow" />
                                <PagerStyle CssClass="GridPager" />
                                <RowStyle CssClass="GridRow" />
                                <FooterStyle CssClass="GridFooter" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                            </asp:GridView>
                            <asp:HiddenField runat="server" ID="hdnRepresentanteObligadoSeleccionadoID" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grdObligadosSolidarios" runat="server" AutoGenerateColumns="false"
                    CssClass="Grid" PageSize="5" AllowPaging="True" CellPadding="4" GridLines="None"
                    Style="margin-top: 10px; max-width: 98%;" AllowSorting="False" OnRowCommand="grdObligadosSolidarios_RowCommand"
                    OnPageIndexChanging="grdObligadosSolidarios_PageIndexChanging" OnRowDataBound="grdObligadosSolidarios_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                        <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Justify">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.TipoObligado").ToString().ToUpper().Replace("0", "FÍSICO").Replace("0", "MORAL")%>
                            </ItemTemplate>
                            <ItemStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDireccion" Text='<%# ((PersonaBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="400px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                    ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>'
                                    ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver Detalles" CommandName="CMDDETALLE" CommandArgument='<%#Container.DataItemIndex%>'
                                    ImageAlign="Middle" /></ItemTemplate>
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
                        <b>No se han asignado Obligados Solidarios.</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnlAvales" runat="server">
            <fieldset>
                <legend>Avales</legend>
                <table style="margin-left: 0px; margin-right: auto;" id="tbAval" runat="server">
                    <tr>
                        <td class="tdCentradoVertical" align="right" style="width: 210px;">
                            <span>*</span> Aval&nbsp; &nbsp;
                        </td>
                        <td style="padding-top: 5px;">
                            <asp:UpdatePanel ID="updAvales" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlAvales" runat="server" Enabled="False" Width="340px" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlAvales_SelectedIndexChanged">
                                        <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlAvales" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:UpdatePanel ID="updAgregarAval" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnAgregarAval" runat="server" CssClass="btnAgregarATabla" Enabled="False"
                                        OnClick="btnAgregarAval_Click" Text="Agregar a Tabla" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trRepresentantesAval" runat="server">
                        <td class="tdCentradoVertical" align="right" style="width: 210px;">
                            <span>*</span> Representantes Legales&nbsp; &nbsp;
                        </td>
                        <td colspan="3" style="padding-top: 5px;">
                            <asp:GridView runat="server" ID="grdRepresentantesAval" AutoGenerateColumns="False"
                                CssClass="Grid" Style="margin-left: 0px; margin-right: auto;" CellPadding="4"
                                GridLines="None" OnPageIndexChanging="grdRepresentantesAval_PageIndexChanging"
                                OnRowDataBound="grdRepresentantesAval_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            #</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRepresentanteAvalID" Text='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                Width="30px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkRepAval" AutoPostBack="True" OnCheckedChanged="chkRepresentanteAval_CheckedChanged">
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <EditRowStyle CssClass="GridAlternatingRow" />
                                <PagerStyle CssClass="GridPager" />
                                <RowStyle CssClass="GridRow" />
                                <FooterStyle CssClass="GridFooter" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                            </asp:GridView>
                            <asp:HiddenField runat="server" ID="hdnRepresentanteAvalSeleccionadoID" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grdAvales" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                    PageSize="5" AllowPaging="True" AllowSorting="False" CellPadding="4" GridLines="None"
                    Style="margin-top: 10px; max-width: 98%;" OnRowCommand="grdAvales_RowCommand"
                    OnPageIndexChanging="grdAvales_PageIndexChanging" OnRowDataBound="grdAvales_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                        <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Justify">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.TipoAval").ToString().ToUpper().Replace("0", "FÍSICO").Replace("0", "MORAL")%>
                            </ItemTemplate>
                            <ItemStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblDireccion" Text='<%# ((PersonaBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="400px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                    ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>'
                                    ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver Detalles" CommandName="CMDDETALLE" CommandArgument='<%#Container.DataItemIndex%>'
                                    ImageAlign="Middle" /></ItemTemplate>
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
                        <b>No se han asignado Avales.</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </fieldset>
        </asp:Panel>
    </asp:Panel>
</fieldset>
<fieldset>
    <legend>Condiciones de Renta</legend>
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical">
                <span>*</span>Plazo
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 120px;">
                <asp:TextBox ID="txtPlazo" runat="server" MaxLength="2" Width="60px" CssClass="CampoNumeroEntero"
                    AutoPostBack="true" OnTextChanged="txtPlazo_TextChanged"></asp:TextBox>
                MESES
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 120px; text-align: right;">
                <span>*</span>Fecha Inicio
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 171px;">
                <asp:TextBox ID="txtFechaInicioContrato" runat="server" MaxLength="11" CssClass="CampoFecha"
                    AutoPostBack="true" OnTextChanged="txtFechaInicioContrato_TextChanged"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" style="width: 170px; text-align: right;">
                Fecha Terminaci&oacute;n
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtFechaFinalizacion" runat="server" MaxLength="11" Enabled="false"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <legend>Unidades del Contrato</legend>
    <table id="tbSeleccionarUnidad" runat="server" class="trAlinearDerecha" style="margin-left: 0px;
        margin-right: auto;">
        <tr>
            <td class="tdCentradoVertical" style="width: 170px;">
                <span>*</span>VIN
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtUnidadVIN" runat="server" Width="200px" Enabled="false" AutoPostBack="True"
                    OnTextChanged="txtUnidadNumeroEconomico_TextChanged"></asp:TextBox>
                <asp:ImageButton runat="server" ID="ibtnBuscarUnidad" CommandName="VerNumeroEconomico"
                    ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Unidades" CommandArgument=''
                    OnClick="ibtnBuscarUnidad_Click" />
                <asp:HiddenField runat="server" ID="hdnUnidadID" />
                <asp:HiddenField runat="server" ID="hdnEquipoID" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlLineaContrato" runat="server">
        <div class="dvIzquierda">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical">
                        # Econ&oacute;mico
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical" style="width: 260px;">
                        <asp:TextBox ID="txtUnidadNumeroEconomico" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Marca
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadMarcaNombre" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Modelo
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadModeloNombre" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Año
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadAnio" runat="server" Width="30px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Capacidad Tanque
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadCapacidadTanque" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Km Estimado Anual
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadKmEstimadoAnual" runat="server" Width="110px" MaxLength="9" CssClass="CampoNumeroEntero"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Kilometros Incluidos
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtKilometrosLibres" runat="server" Width="110px" MaxLength="10" CssClass="CampoNumero"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Costo Km Recorrido
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadCostoKmRecorrido" runat="server" Width="110px" MaxLength="10" CssClass="CampoNumero"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Periodo KM Incluido
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:DropDownList runat="server" ID="ddlPeriodoTarifaKM"/>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Producto o Servicio</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtClaveProductoServicio" runat="server" Columns="30" MaxLength="15"
                                ontextchanged="txtClaveProductoServicio_TextChanged" Enabled="False" AutoPostBack="true"></asp:TextBox>
                        <asp:ImageButton ID="ibtnBuscarProductoServicio" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                                OnClick="ibtnBuscarProductoServicio_Click" Style="width: 17px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">&nbsp;</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtDescripcionProductoServicio" runat="server" Width="200px" MaxLength="100" ReadOnly="true" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="dvDerecha">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical">
                        Sucursal
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadSucursalNombre" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Placas Federales
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadPlacaFederal" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Placas Estatales
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadPlacaEstatal" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Capacidad Carga (PBC)
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadCapacidadCarga" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        Rendimiento Tanque
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadRendimientoTanque" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                        km
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Cargo Fijo Mensual
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtUnidadCargoFijoMensual" runat="server" Width="110px" MaxLength="10" CssClass="CampoNumero"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Horas Incluidas
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtHorasLibres" runat="server" Width="110px" MaxLength="10" CssClass="CampoNumero"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Costo Hora Refrigerada
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtCostoHoraRefrigerada" runat="server" Width="110px" MaxLength="10" CssClass="CampoNumero"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">
                        <span>*</span>Periodo HRS Incluidas
                    </td>
                    <td style="width: 5px;">
                        &nbsp;
                    </td>
                    <td class="tdCentradoVertical">
                        <asp:DropDownList runat="server" ID="ddlPeriodoTarifaHRS"/>
                    </td>
                </tr>
            </table>
        </div>
        <fieldset style="display: inline;">
            <legend>Equipos Aliados</legend>
            <asp:GridView runat="server" ID="grdUnidadEquiposAliados" AutoGenerateColumns="False"
                AllowPaging="false" CssClass="Grid" CellPadding="4" GridLines="None" 
                onrowcommand="grdUnidadEquiposAliados_RowCommand" >
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            #</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEquipoAliadoID" Text='<%# DataBinder.Eval(Container.DataItem, "EquipoAliado.EquipoAliadoID") %>'
                                Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="# Serie">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EquipoAliado.NumeroSerie")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Año">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EquipoAliado.Anio")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dimensiones">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EquipoAliado.Dimension")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PBV">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EquipoAliado.PBV")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PBC">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EquipoAliado.PBC")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Modelo">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.EquipoAliado.Modelo.Nombre")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            ¿Mantenimiento?</HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkMantenimiento" Checked='<%# DataBinder.Eval(Container.DataItem, "Mantenimiento") %>' Enabled="False" OnCheckedChanged="chkMantenimiento_CheckedChanged"></asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                ToolTip="Ver Detalles" CommandName="CMDDETALLES" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EquipoAliado.EquipoAliadoID") %>'
                                Width="17px" />
                        </ItemTemplate>
                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <b>La unidad no cuenta con equipos aliados</b></EmptyDataTemplate>
                <HeaderStyle CssClass="GridHeader" />
                <EditRowStyle CssClass="GridAlternatingRow" />
                <PagerStyle CssClass="GridPager" />
                <RowStyle CssClass="GridRow" />
                <FooterStyle CssClass="GridFooter" />
                <SelectedRowStyle CssClass="GridSelectedRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
            </asp:GridView>
        </fieldset>
        <div style="width: 98%; text-align: right;">
            <asp:Button ID="btnCancelarLineaContrato" runat="server" CssClass="btnWizardCancelar"
                OnClick="btnCancelarLineaContrato_Click" Text="Cancelar" />
            <asp:Button ID="btnActualizarLineaContrato" runat="server" CssClass="btnWizardGuardar"
                OnClick="btnActualizarLineaContrato_Click" Text="Actualizar" />
            <asp:Button ID="btnAgregarLineaContrato" runat="server" CssClass="btnAgregarATabla"
                OnClick="btnAgregarLineaContrato_Click" Text="Agregar a Tabla" />
        </div>
    </asp:Panel>
    <asp:GridView ID="grdLineasContrato" runat="server" AutoGenerateColumns="False" AllowPaging="false"
        CssClass="Grid" Style="margin-top: 10px; margin-bottom: 10px; width: 96%;" CellPadding="4"
        GridLines="None" OnPageIndexChanging="grdLineasContrato_PageIndexChanging" OnRowCommand="grdLineasContrato_RowCommand"
        OnRowDataBound="grdLineasContrato_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    VIN</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblVIN" Text='<%# DataBinder.Eval(Container.DataItem, "Equipo.NumeroSerie") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Modelo</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem, "Equipo.Modelo.Nombre") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Cargo Fijo Mensual</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblCargoFijoMensual"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Costo Km Recorrido</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblCostoKmRecorrido"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="KmEstimadoAnual" HeaderText="Km Estimado Anual" DataFormatString="{0:#,##0}">
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                        ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="ibtEditar" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png"
                        ToolTip="Editar" CommandName="CMDEDITAR" CommandArgument='<%#Container.DataItemIndex%>' />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                        ToolTip="Ver Detalles" CommandName="CMDDETALLES" CommandArgument='<%#Container.DataItemIndex%>'
                        Width="17px" />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <b>El contrato no tiene unidades</b></EmptyDataTemplate>
        <HeaderStyle CssClass="GridHeader" />
        <EditRowStyle CssClass="GridAlternatingRow" />
        <PagerStyle CssClass="GridPager" />
        <RowStyle CssClass="GridRow" />
        <FooterStyle CssClass="GridFooter" />
        <SelectedRowStyle CssClass="GridSelectedRow" />
        <AlternatingRowStyle CssClass="GridAlternatingRow" />
    </asp:GridView>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Ubicaci&oacute;n Taller Servicio
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUbicacionTallerServicio" runat="server" MaxLength="300" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Dep&oacute;sito Garant&iacute;a
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDepositoGarantia" runat="server" MaxLength="10" Width="200px"
                        CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>Comisi&oacute;n Apertura
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtComisionApertura" runat="server" MaxLength="10" Width="200px"
                        CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px">
                    <span>*</span>Direcci&oacute;n de Almacenaje
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDireccionAlmacenaje" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                        MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
                        min-height: 90px;"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical" style="width: 170px;">
                    <span>*</span>¿Incluye Seguro?
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlIncluyeSeguro" runat="server" Width="240px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>¿Incluye Lavado?
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlIncluyeLavado" runat="server" Width="240px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>¿Incluye Pintura?
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlIncluyeRotulacionPintura" runat="server" Width="240px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">
                    <span>*</span>¿Incluye Llantas?
                </td>
                <td style="width: 5px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList ID="ddlIncluyeLlantas" runat="server" Width="240px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hdnLineaContratoID" />
    <asp:HiddenField runat="server" ID="hdnCobrableID" />
    <asp:HiddenField runat="server" ID="hdnLineaContratoIndex" />
    <asp:HiddenField runat="server" ID="hdnModoEdicionLineaContrato" />
</fieldset>
<fieldset id="fsDatosAdicionales" runat="server">
    <legend>Datos Adicionales</legend>
    <table class="trAlinearDerecha" style="width: 100%;" id="tbDatoAdicional" runat="server">
        <tr>
            <td class="tdCentradoVertical" style="width: 160px;">
                <span>*</span>T&iacute;tulo
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox ID="txtTitulo" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
            </td>
            <td class="tdCentradoVertical" style="text-align: right; width: 160px;">
                <span>*</span>Descripci&oacute;n
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" rowspan="3">
                <asp:TextBox ID="txtDescripcion" runat="server" Width="350px" MaxLength="300" TextMode="MultiLine"
                    Rows="3" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
                    min-height: 90px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">
                <asp:CheckBox ID="cbEsObservacion" runat="server" />
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                ¿Es Observaci&oacute;n?
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="7" align="right" style="padding-right: 20px;">
                <asp:Button ID="btnAgregarDatoAdicional" runat="server" Text="Agregar a Tabla" CssClass="btnAgregarATabla"
                    OnClick="btnAgregarDatoAdicional_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <asp:GridView ID="grdDatosAdicionales" runat="server" AutoGenerateColumns="False"
        CellPadding="4" GridLines="None" CssClass="Grid" PageSize="10" AllowPaging="True"
        AllowSorting="True" OnRowCommand="grdDatosAdicionales_RowCommand" OnPageIndexChanging="grdDatosAdicionales_PageIndexChanging"
        Width="95%" Style="margin: 10px auto;">
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
                    <asp:CheckBox runat="server" ID="cbObservacion" Checked='<%#((DatoAdicionalAnexoBO)Container.DataItem).EsObservacion == true %>'
                        Enabled="False" Width="17px" />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                        ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
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
</fieldset>
<fieldset>
    <legend>Observaciones</legend>
    <table class="trAlinearDerecha">
        <tr>
            <td align="right" style="padding-top: 5px">
                Observaci&oacute;n
            </td>
            <td style="width: 20px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Width="750px"
                    Height="90px" Style="max-width: 750px; min-width: 750px; max-height: 90px; min-height: 90px;">
                </asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>
<asp:HiddenField runat="server" ID="hdnContratoID" />
<asp:HiddenField runat="server" ID="hdnTipoContratoID" />
<asp:HiddenField runat="server" ID="hdnEstatusContratoID" />
<asp:HiddenField runat="server" ID="hdnNumeroContrato" />
<asp:HiddenField runat="server" ID="hdnFechaFinalizacion" />
<asp:HiddenField runat="server" ID="hdnUsuarioFinalizacionID" />
<asp:HiddenField runat="server" ID="hdnObservacionesFinalizacion" />
<asp:HiddenField runat="server" ID="hdnMotivoFinalizacion" />
<asp:HiddenField runat="server" ID="hdnModoEdicionContrato" />
<asp:HiddenField runat="server" ID="hdnDireccionClienteID" />
<asp:HiddenField runat="server" ID="hdnProductoServicioId" Value="" />
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click"
    Style="display: none;" />
<div id="dvTarifaEquipoAliado" title="TARIFA EQUIPO ALIADO" style="display: none;">
    <table>
        <asp:HiddenField runat="server" ID="hdnEquipoAliadoID" />
        <tr>
            <td>¿Mantenimiento?</td>
            <td>&nbsp;</td>
            <td><asp:CheckBox runat="server" ID="chkMantenimientoEA"/> </td>            
            <td>
                &nbsp;
            </td>
            <td>
                Cargo Fijo Mensual
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCargoFijoMensualEA" CssClass="CampoMoneda txtMoneda"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Kilometros Incluidos
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtKilometrosLibresEA" CssClass="CampoNumeroEntero" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                Horas Incluidas
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtHorasLibresEA" CssClass="CampoNumeroEntero" ></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>            
        </tr>
        <tr>
            <td>
                Costo Kilometro
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCostoKilometroEA" CssClass="CampoMoneda txtMoneda" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                Costo Hora Refrigerada
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCostoHoraRefrigeradaEA" CssClass="CampoMoneda txtMoneda" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Per&iacute;odo KM Incluido
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPeriodoTarifaKMEA"/>
            </td>
            <td>
                &nbsp;
            </td>
            <td>Per&iacute;odo HRS Incuidas</td>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPeriodoTarifaHRSEA"/>
            </td>
        </tr>
    </table>
    <div style="text-align: center">
        <asp:Button runat="server" ID="cmdAceptarTarifaEA" OnClick="cmdAceptarTarifaEA_Click"
            CssClass="btnComando" Text="Aceptar" />
    </div>
</div>
<div id="DialogRepresentantes" style="display: none;" title="REPRESENTANTES LEGALES">
    <asp:GridView ID="grdRepresentantesDialog" runat="server" AutoGenerateColumns="false"
        CellPadding="4" CssClass="Grid" AllowPaging="False" AllowSorting="False" Width="95%">
        <Columns>
            <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
            <asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblDireccion" Text='<%# ((RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label></ItemTemplate>
                <ItemStyle Width="300px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblFolioRPPC" Text='<%# ((RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>'></asp:Label></ItemTemplate>
                <ItemStyle Width="150px" />
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="GridHeader" />
        <EditRowStyle CssClass="GridAlternatingRow" />
        <PagerStyle CssClass="GridPager" />
        <RowStyle CssClass="GridRow" />
        <FooterStyle CssClass="GridFooter" />
        <SelectedRowStyle CssClass="GridSelectedRow" />
        <AlternatingRowStyle CssClass="GridAlternatingRow" />
    </asp:GridView>
</div>
