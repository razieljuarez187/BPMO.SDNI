<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClienteContratoUI.ascx.cs"
	Inherits="BPMO.SDNI.Contratos.FSL.UI.ucClienteContratoUI" %>
<%@ Import Namespace="BPMO.SDNI.Comun.BO" %>
<%--
	Satisface al Caso de Uso CU022 - Consultar Contratos Full Service Leasing
	Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
	Satisface al caso de uso CU093 - Imprimir Pagaré Contrato FSL
--%>
<script type="text/javascript">
	function <%= ClientID %>_Buscar(guid, xml) {
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

	 function DialogoDetalleObligado() {
			$("#DialogRepresentantesObligados").dialog({
				modal: true,
				width: 900,
				height: 400,
				resizable: false,
				buttons: {
					"Aceptar": function () {
						$(this).dialog("close");
					}
				}
			});
			$("#DialogRepresentantesObligados").parent().appendTo("form:first");
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
</script>
<div id="divDatosClienteControles">
	<table style="width: 95%; margin: 5px auto;">
		<tr>
			<td style="padding-top: 5px;">
				<span>*</span>Cliente
			</td>
			<td style="width: 5px;">
				&nbsp;
			</td>
			<td>
				<asp:TextBox ID="txtNombreCliente" runat="server" Width="275px" AutoPostBack="True"
					OnTextChanged="txtNombreCliente_TextChanged"></asp:TextBox>
				<asp:ImageButton ID="ibtnBuscarCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png"
					OnClick="ibtnBuscarCliente_Click" />
			</td>
			<td align="right" style="padding-top: 5px;">
				<span>*</span>Dirección&nbsp;&nbsp;
			</td>
			<td class="tdCentradoVertical" align="right">
				<asp:TextBox ID="txtDomicilioCliente" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
					MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
					min-height: 90px;" Width="310px" Text="" Enabled="false"></asp:TextBox>
				<asp:ImageButton ID="ibtnBuscarDirieccionCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png"
					OnClick="ibtnBuscarDirieccionCliente_Click"></asp:ImageButton>
			</td>
		</tr>
        <tr>
            <td style="padding-top: 5px;"><label>Cuenta Oracle</label></td>
            <td style="width: 5px;">&nbsp;</td>
            <td><asp:TextBox runat="server" ID="txtNumeroCuentaOracle" Width="150px"></asp:TextBox></td>
            <td align="right" style="padding-top: 5px;">&nbsp;</td>
            <td class="tdCentradoVertical" align="right">&nbsp;</td>
        </tr>
		<tr id="trRepresentantes1" runat="server">
			<td  style="padding-top: 5px;" colspan="4">
                <asp:CheckBox runat="server" ID="cbSoloRepresentantes" Text="Solo Representantes Legales" OnCheckedChanged="cbSoloRepresentantes_CheckedChanged" AutoPostBack="True" />
			    <br />
				<asp:Label runat="server" ID="lblMensajeSoloRepresentantes" Font-Size="8pt" CssClass="ColorValidator" Text="Esta opción solo despliega representantes legales en el contrato y sus anexos." ></asp:Label>
			</td>
			<td class="tdCentradoVertical" align="right">
				&nbsp;</td>
		</tr>
		<tr id="trRepresentantes2" runat="server">
			<td colspan="4" class="tdCentradoVertical" align="right" style="width: 161px;">
				<span>*</span> Representante Legal&nbsp; &nbsp;
			</td>
			<td class="tdCentradoVertical" align="right">
				<asp:UpdatePanel ID="updRepresentantesLegales" runat="server">
					<ContentTemplate>
						<asp:DropDownList ID="ddlRepresentantesLegales" runat="server" Enabled="False" Width="340px">
							<asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
						</asp:DropDownList>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr id="trRepresentantes3" runat="server">
            <td colspan="5" align="right">
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
	<asp:GridView ID="grdRepresentantesLegales" runat="server" AutoGenerateColumns="False"
		CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
		AllowSorting="True" OnRowDataBound="grdRepresentantesLegales_RowDataBound" OnRowCommand="grdRepresentantesLegales_RowCommand"
		OnPageIndexChanging="grdRepresentantesLegales_PageIndexChanging">
		<Columns>
			<asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
			<asp:TemplateField HeaderText="¿Es Depositario?" ItemStyle-HorizontalAlign="Justify">
				<ItemTemplate>
					<asp:Label runat="server" ID="cbEsDepositario" Text='<%# ((bool)DataBinder.Eval(Container.DataItem, "EsDepositario") == true)? "SI":"NO" %>'
						Width="100%"></asp:Label>
				</ItemTemplate>
				<ItemStyle Width="150" HorizontalAlign="Justify" />
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
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
	
	<table style="width: 95%; margin: 5px auto;" id="tblObligadosSolidarios" runat="server">
			<tr>
			<td style="padding-top:5px;" colspan="5">
				 <asp:CheckBox runat="server" ID="cbObligadosComoAvales" Text="Obligados Solidarios como Avales" OnCheckedChanged="cbObligadosComoAvales_CheckedChanged" AutoPostBack="True" />
	            <br />
	            <asp:Label runat="server" ID="lblMensajeObligadosComoAvales" Font-Size="8pt" CssClass="ColorValidator" 
                    Text="Esta opción hace que TODOS los obligados solidarios del contrato se usen como avales" ></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="padding-top:5px;" colspan="7">
				<asp:Label runat="server" Font-Size="8pt" ID="lblMensajeOblS" CssClass="ColorValidator" Text="En caso de no seleccionar Obligados Solidarios, El o los Representantes Legales del cliente tomarian dicha obligación."></asp:Label></td>
		</tr>
		<tr>
			<td style="padding-top:5px; width: 200px;">
				<span>*</span>Obligado Solidario
			</td>
			<td style="width: 5px;">
				&nbsp;
			</td>
			<td style="padding-top:5px;">
				<asp:UpdatePanel ID="updObligadosSolidarios" runat="server">
					<ContentTemplate>
						<asp:DropDownList ID="ddlObligadosSolidarios" runat="server" Enabled="False" Width="250px"
							AutoPostBack="true" OnSelectedIndexChanged="ddlObligadosSolidarios_SelectedIndexChanged">
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
			<td style="padding-top: 5px; width: 100px;" runat="server" id="tdRepLegalObligado" visible="False">
				<span>*</span>Representantes Legales
			</td>
			<td style="width: 5px;">
				&nbsp;
			</td>
			<td runat="server" id="tdGridRepresentantes" visible="False">
				<asp:GridView runat="server" ID="grdRepresentantesObligadoSolidario" AutoGenerateColumns="False"
				 CssClass="Grid" style="margin-left: 0px; margin-right: auto;" CellPadding="4" GridLines="None"
					OnPageIndexChanging="grdRepresentantesObligadoSolidario_PageIndexChanging" OnRowDataBound="grdRepresentantesObligadoSolidario_RowDataBound">
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
								<asp:CheckBox runat="server" ID="chkRepOS" Checked='<%#((RepresentanteLegalBO)Container.DataItem).Activo == true %>'
									AutoPostBack="True" OnCheckedChanged="chkRepresentanteOS_CheckedChanged"></asp:CheckBox>
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
			</td>
		</tr>
		<tr>
			<td colspan="7" align="right">
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
	</table>	
	<asp:GridView ID="grdObligadosSolidarios" runat="server" AutoGenerateColumns="false"
		CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True" style="margin-top: 10px; max-width: 98%;"
		AllowSorting="True" OnRowCommand="grdObligadosSolidarios_RowCommand" OnPageIndexChanging="grdObligadosSolidarios_PageIndexChanging"
		OnRowDataBound="grdObligadosSolidarios_RowDataBound">
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
					<asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.ObligadoSolidarioBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label>
				</ItemTemplate>
				<ItemStyle Width="400px" />
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
						CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' ImageAlign="Middle" />
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
	<asp:Panel ID="pnlAvales" runat="server">
           
                <table style="width: 95%; margin: 5px auto;" id="tbAval" runat="server">
                    <tr>
			            <td style="padding-top:5px; width: 200px;">
				            <span>*</span> Aval&nbsp; &nbsp;
			            </td>
						<td style="width: 5px;">
						&nbsp;
						</td>
			            <td style="padding-top:5px;">
				            <asp:UpdatePanel ID="updAvales" runat="server">
					            <ContentTemplate>
						            <asp:DropDownList ID="ddlAvales" runat="server" Enabled="False" Width="250px" AppendDataBoundItems="true"
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
						<td style="padding-top: 5px;width: 100px;" runat="server" id="tdRepLegalAval" visible="False">
				            <span>*</span>Representantes Legales
			            </td>
						<td style="width: 5px;">
				&nbsp;
			</td>
						<td runat="server" id="tdGridRepresentantesAvales" Visible="False">
						
				            <asp:GridView runat="server" ID="grdRepresentantesAval" AutoGenerateColumns="False" 
                                CssClass="Grid" style="margin-left: 0px; margin-right: auto;" CellPadding="4" GridLines="None" 
					            OnPageIndexChanging="grdRepresentantesAval_PageIndexChanging" OnRowDataBound="grdRepresentantesAval_RowDataBound">
					            <Columns>
						            <asp:TemplateField>
							            <HeaderTemplate>#</HeaderTemplate>
							            <ItemTemplate>
								            <asp:Label runat="server" ID="lblRepresentanteAvalID" Text='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Width="30px"></asp:Label>
							            </ItemTemplate>
						            </asp:TemplateField>
						            <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
						            <asp:TemplateField>
							            <ItemTemplate>
								            <asp:CheckBox runat="server" ID="chkRepAval" AutoPostBack="True" OnCheckedChanged="chkRepresentanteAval_CheckedChanged"></asp:CheckBox>
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
										<EmptyDataTemplate>
			<b>NO SE HAN ASIGNADO AVALES.</b>
		</EmptyDataTemplate>
				            </asp:GridView>
                            <asp:HiddenField runat="server" ID="hdnRepresentanteAvalSeleccionadoID"/>
                        </td>
                    </tr>
					<tr>
						<td colspan="7" align="right">
		                				            <asp:UpdatePanel ID="updAgregarAval" runat="server">
					            <ContentTemplate>
						            <asp:Button ID="btnAgregarAval" runat="server" CssClass="btnAgregarATabla"
							            Enabled="False" OnClick="btnAgregarAval_Click" Text="Agregar a Tabla" />
					            </ContentTemplate>
					            <Triggers>
						            <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
						            <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
					            </Triggers>
				            </asp:UpdatePanel>
	                </td>
					</tr>
				</table>
	            <asp:GridView ID="grdAvales" runat="server" AutoGenerateColumns="false" CssClass="Grid" PageSize="5" AllowPaging="True"
		            AllowSorting="False" CellPadding="4" GridLines="None" style="margin-top: 10px; max-width: 98%;"
                    OnRowCommand="grdAvales_RowCommand" OnPageIndexChanging="grdAvales_PageIndexChanging" OnRowDataBound="grdAvales_RowDataBound">
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
					            <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
						            CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' ImageAlign="Middle" />
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

        </asp:Panel>
</div>
<asp:HiddenField runat="server" ID="hdnCuentaClienteID" />
<asp:HiddenField runat="server" ID="hdnClienteID" />
<asp:HiddenField runat="server" ID="hdnModo" />
<asp:HiddenField runat="server" ID="hdnCalle" />
<asp:HiddenField runat="server" ID="hdnColonia" />
<asp:HiddenField runat="server" ID="hdnCodigoPostal" />
<asp:HiddenField runat="server" ID="hdnCiudad" />
<asp:HiddenField runat="server" ID="hdnEstado" />
<asp:HiddenField runat="server" ID="hdnMunicipio" />
<asp:HiddenField runat="server" ID="hdnPais" />
<asp:HiddenField runat="server" ID="hdnEsFisico" />
<asp:HiddenField runat="server" ID="hdnDireccionClienteID" />

<asp:Button runat="server" ID="btnResult" Style="display: none;" OnClick="btnResult_Click" />

<div id="DialogRepresentantesObligados" style="display: none;" title="REPRESENTANTES LEGALES DEL OBLIGADO SOLIDARIO">
	<asp:GridView ID="grdRepresentantesObligados" runat="server" AutoGenerateColumns="false"
		CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
		AllowSorting="True" OnPageIndexChanging="grdRepresentantesObligados_PageIndexChanging"
		Width="95%">
		<Columns>
			<asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
			<asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label></ItemTemplate>
				<ItemStyle Width="300px" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblFolioRPPC" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>'></asp:Label></ItemTemplate>
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