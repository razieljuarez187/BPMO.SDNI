<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContratoRDUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.ucContratoRDUI" %>
<%@ Import Namespace="BPMO.SDNI.Comun.BO" %>

 <%--BEP1401 Satisface a la SC0034--%>
 <%-- Satisface a la solicitud de cambio SC0035 --%>

<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
        var FechaContrato = $('#<%= txtFechaContrato.ClientID %>');
        if (FechaContrato.length > 0) {
            FechaContrato.datepicker({
                yearRange: '-10:+7',
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha del contrato",
                showOn: 'button',
                defaultDate: (FechaContrato.val().length == 10) ? FechaContrato.val() : new Date()
            });

            FechaContrato.attr('readonly', true);
        }
        
        var fechapropdevolucion = $('#<%= txtCondicionesFechaDevolucion.ClientID %>');
        if (fechapropdevolucion.length > 0) {
            fechapropdevolucion.datepicker({
                yearRange: '-10:+10',
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha de expiración para la licencia del operador",
                showOn: 'button',
                defaultDate: (fechapropdevolucion.val().length == 10) ? fechapropdevolucion.val() : new Date()
            });

            fechapropdevolucion.attr('readonly', true);
        }
        
        /*Configuración de los campos de hora*/
        $('.CampoHora').each(function () {
            if ($(this).length > 0) {
                $(this).timepicker({
                    showPeriod: true,
                    showLeadingZero: true,
                    showCloseButton: true
                });
            }
            $(this).attr('readonly', true);
            $('.ui-timepicker-close').click();
        });
        
        var mostrarDialogo = $("#<%= hdnMostrarDialogoTarifa.ClientID %>").val();
		if(mostrarDialogo=="1") MostrarDialogo(true);

    }

    function MostrarDialogo(mostrar) {
            
	    
	    $("#<%= hdnMostrarDialogoTarifa.ClientID %>").val("0");
		$("#DialogoTarifaPersonal").dialog({
				modal: true,
				width: 800,
				autoOpen:false,
				resizable: false,
				title: "Personalizar Tarifa Renta Diaria"
			});
        
		$("#DialogoTarifaPersonal").parent().appendTo("form:first");
	    if(mostrar) {
		    $("#DialogoTarifaPersonal").dialog("open");
		    $("#<%= hdnMostrarDialogoTarifa.ClientID %>").val("1");
	    }
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
<fieldset>
    <legend>Información General</legend>
    <asp:HiddenField runat="server" ID="hdnContratoID"/>
    <asp:HiddenField runat="server" ID="hdnEstatusContratoID"/>
    <asp:HiddenField runat="server" ID="hdnNumeroContrato"/>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Fecha Contrato</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtFechaContrato" runat="server" MaxLength="11" CssClass="CampoFecha" AutoPostBack="True"
                        ontextchanged="txtFechaContrato_TextChanged"></asp:TextBox>
                </td>
            </tr>

            <%--SC0034, Campo de hora oculto --%>
            <tr style="display:none">
                <td class="tdCentradoVertical"><span>*</span>Hora Contrato</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical"><asp:TextBox ID="txtHoraContrato" runat="server" MaxLength="7" CssClass="CampoHora" AutoPostBack="True"
                    ontextchanged="txtFechaContrato_TextChanged"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Empresa</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtEmpresa" runat="server" Width="250px"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdnEmpresaID"/>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Sucursal</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 300px;">
                     <asp:DropDownList ID="ddlSucursales" runat="server" Width="300px" >
                                <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">Representante</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical"><asp:TextBox ID="txtRepresentante" runat="server" MaxLength="250" Width="250px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px">Domicilio Empresa</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDireccionEmpresa" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                                MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px;
                                max-height: 90px; min-height: 90px;"></asp:TextBox>
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
                <td class="tdCentradoVertical"><span>*</span>Cliente</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:HiddenField runat="server" ID="hdnClienteID" />
                    <asp:HiddenField runat="server" ID="hdnClienteEsFisico" />
                    <asp:HiddenField runat="server" ID="hdnTipoCuentaRegion" />                    
                    <asp:TextBox ID="txtNombreCliente" runat="server" Width="250px" MaxLength="150" AutoPostBack="True" OnTextChanged="txtNombreCliente_TextChanged"></asp:TextBox>
				    <asp:ImageButton ID="ibtnBuscarCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png" ToolTip="Consultar Cliente" OnClick="ibtnBuscarCliente_Click" />
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">RFC</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtRFCCliente" runat="server" Width="105px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Cuenta Oracle</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtNumeroCuentaCliente" runat="server" Width="105px" Visible="False"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtNumeroCuentaOracle" Width="105px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td align="right" style="padding-top: 5px"><span>*</span>Direcci&oacute;n</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:HiddenField runat="server" ID="hdnDireccionId" />
                    <asp:HiddenField runat="server" ID="hdnCalle" />
                    <asp:HiddenField runat="server" ID="hdnColonia" />                    
                    <asp:HiddenField runat="server" ID="hdnCiudad" />
                    <asp:HiddenField runat="server" ID="hdnEstado" />
                    <asp:HiddenField runat="server" ID="hdnMunicipio" />
                    <asp:HiddenField runat="server" ID="hdnPais" />
                    <asp:TextBox ID="txtDomicilioCliente" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
					MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
					min-height: 90px;" Width="310px" Text=""></asp:TextBox>
				<asp:ImageButton ID="ibtnBuscarDirieccionCliente" runat="server" ImageUrl="../Contenido/Imagenes/Detalle.png" 
                ToolTip="Consultar Direcci&oacute;n" OnClick="ibtnBuscarDirieccionCliente_Click"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">C&oacute;digo Postal</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCodigoPostal" runat="server" Width="60px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
  
  <asp:Panel ID="pnlPersonasCliente" runat="server" style="display: table; width: 100%;">
	      <div style="display: inline;" runat="server" id="pnlRepresentantes">
        <fieldset>
            <legend>Representantes legales</legend>
            <table style="margin-left: 0px; margin-right: auto;">
	        <tr id="trSoloRepresentantes" runat="server">
			<td  style="padding-top: 5px;" colspan="4">
                <asp:CheckBox runat="server" ID="cbSoloRepresentantes" Text="Solo Representantes Legales" OnCheckedChanged="cbSoloRepresentantes_CheckedChanged" AutoPostBack="True" />
			    <br />
				<asp:Label runat="server" ID="lblMensajeSoloRepresentantes" Font-Size="8pt" CssClass="ColorValidator" Text="Esta opción hace que TODOS los representantes legales del contrato se usen como avales" ></asp:Label>
			</td>
			<td class="tdCentradoVertical" align="right">
				&nbsp;</td>
		</tr>
                <tr id="trRepresentantes1" runat="server">
			        <td class="tdCentradoVertical" align="right" style="width: 210px;">
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
                    <td style="width: 5px;">&nbsp;</td>
                    <td align="right">
				        <asp:UpdatePanel ID="updAgregarRepresentante" runat="server">
					        <ContentTemplate>
						        <asp:Button ID="btnAgregarRepresentante" CssClass="btnAgregarATabla" runat="server" Text="Agregar a Tabla" OnClick="btnAgregarRepresentante_Click" Enabled="False" />
					        </ContentTemplate>
					        <Triggers>
						        <asp:AsyncPostBackTrigger ControlID="ibtnBuscarCliente" EventName="Click" />
						        <asp:AsyncPostBackTrigger ControlID="txtNombreCliente" EventName="TextChanged" />
					        </Triggers>
				        </asp:UpdatePanel>
			        </td>
		        </tr>
	        </table>
            <div style="padding-top: 15px;">
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
        </div>
        </fieldset>
    </div>
	
<asp:Panel ID="pnlAvales" runat="server">
            <fieldset>
                <legend>Avales</legend>
                <table style="margin-left: 0px; margin-right: auto;" id="tbAval" runat="server">
                    <tr>
			            <td class="tdCentradoVertical" align="right" style="width: 210px;">
				            <span>*</span> Aval&nbsp; &nbsp;
			            </td>
			            <td style="padding-top:5px;">
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
                        <td style="width: 5px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
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
                    <tr id="trRepresentantesAval" runat="server">
			            <td class="tdCentradoVertical" align="right" style="width: 210px;">
				            <span>*</span> Representantes Legales&nbsp; &nbsp;
			            </td>
			            <td colspan="3" style="padding-top:5px;">
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
				            </asp:GridView>
                            <asp:HiddenField runat="server" ID="hdnRepresentanteAvalSeleccionadoID"/>
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
            </fieldset>
        </asp:Panel>

  </asp:Panel>

</fieldset>

<fieldset>
    <legend>Datos del Operador</legend>
    <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical">
            <asp:HiddenField runat="server" ID="hdnOperadorID" />
            <asp:HiddenField runat="server" ID="hdnOperadorCuentaClienteID" />
            <span>*</span>Nombre</td>
            <td style="width: 5px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtOperadorNombre" MaxLength="100" Width="285px"
                    AutoPostBack="true" OnTextChanged="txtOperadorNombre_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarOperador" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                    ToolTip="Consultar Operadores" CommandArgument='' OnClick="ibtnBuscarOperador_Click" />
            </td>
            <td style="width: 5px;">&nbsp;</td>
            <td class="tdCentradoVertical">A&ntilde;os de Experiencia</td>
                <td style="width: 5px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtOperadorAniosExperiencia" MaxLength="2" Width="25px" CssClass="CampoNumeroEntero"></asp:TextBox>
            </td>
            <td style="width: 5px;">&nbsp;</td>
            <td class="tdCentradoVertical">Fecha de Nacimiento</td>
            <td style="width: 5px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtOperadorFechaNacimiento" 
                    CssClass="CampoFecha" MaxLength="10" Width="75px" AutoPostBack="True"
                    ontextchanged="txtOperadorFechaNacimiento_TextChanged" ></asp:TextBox>
            </td>
        </tr>
    </table> 
    <div class="dvIzquierda">
        <fieldset>
            <legend>Licencia</legend>
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical"><span>*</span># Licencia</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 270px;">
                        <asp:TextBox runat="server" ID="txtLicenciaNumero" MaxLength="20" Width="160px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Tipo Licencia</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 270px;">
                        <asp:DropDownList runat="server" ID="ddlTipoLicencia" 
                            ToolTip="Tipo de licencia del operador.">
                            <asp:ListItem Value="0">ESTATAL</asp:ListItem>
                            <asp:ListItem Selected="True" Value="1">FEDERAL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Fecha Expiraci&oacute;n</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 270px;">
                        <asp:TextBox runat="server" ID="txtLicenciaFechaExpiracion" 
                            CssClass="CampoFecha" MaxLength="10" Width="75px" AutoPostBack="True"
                            ontextchanged="txtLicenciaFechaExpiracion_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Estado Expedici&oacute;n</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 270px;">
                        <asp:TextBox runat="server" ID="txtLicenciaEstadoExpedicion" MaxLength="80" Width="150px"></asp:TextBox>
                    </td>
                </tr>
            </table>  
        </fieldset>  
    </div>
    <div class="dvDerecha">        
        <fieldset>
            <legend>Direcci&oacute;n</legend>
            <table class="trAlinearDerecha" style="margin-left: 15px; margin-right: auto;">
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Calle</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox runat="server" ID="txtOperadorCalle" MaxLength="100" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Ciudad</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox runat="server" ID="txtOperadorCiudad" MaxLength="100" Width="120px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>Estado</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox runat="server" ID="txtOperadorEstado" MaxLength="100" Width="120px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>CP</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 270px;">
                        <asp:TextBox runat="server" ID="txtOperadorCodigoPostal" MaxLength="10" Width="40px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</fieldset>
<fieldset>
    <legend>Informaci&oacute;n de la Unidad</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span># Econ&oacute;mico</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadNumeroEconomico" runat="server" MaxLength="100" AutoPostBack="True"
                        Width="200px" ontextchanged="txtUnidadNumeroEconomico_TextChanged"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="ibtnBuscarUnidad" 
                        CommandName="VerNumeroEconomico" ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                        ToolTip="Consultar Unidades" CommandArgument='' 
                        onclick="ibtnBuscarUnidad_Click"/>
                    <asp:HiddenField runat="server" ID="hdnUnidadID"/>
                    <asp:HiddenField runat="server" ID="hdnEquipoID"/>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Marca</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadMarca" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Modelo</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:HiddenField runat="server" ID="hdnModeloID"/>
                    <asp:TextBox ID="txtUnidadModelo" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">A&ntilde;o</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadAnio" runat="server" Width="31px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Sucursal</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadSucursal" runat="server" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Capacidad Tanque</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadCapacidadTanque" runat="server" Width="110px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"># Serie</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadNumeroSerie" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Placas Federales</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadPlacasFederales" runat="server" Width="110px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Placas Estatales</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadPlacasEstales" runat="server" Width="110px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Capacidad de Carga (PBC)</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadCapacidadCarga" runat="server" Width="110px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Rendimiento Tanque Combustible</td>
                <td style="width: 5px;">&nbsp;-</td>
                <td class="tdCentradoVertical" style="width: 280px;">
                    <asp:TextBox ID="txtUnidadRendimientoTanque" runat="server" Width="110px" CssClass="CampoNumero"></asp:TextBox> KM
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Producto o Servicio</td>
                <td class="tdCentradoVertical" style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtClaveProductoServicio" runat="server" Columns="30" MaxLength="15"
                            ontextchanged="txtClaveProductoServicio_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:ImageButton ID="ibtnBuscarProductoServicio" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                            OnClick="ibtnBuscarProductoServicio_Click" Style="width: 17px" 
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDescripcionProductoServicio" runat="server" Width="250px" MaxLength="100" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <fieldset>
        <legend>Equipos Aliados</legend>
        <div id="divAliados">
            <asp:GridView ID="grdEquiposAliados" runat="server" autoGenerateColumns="False" AllowPaging="True" PageSize="10"
                AllowSorting="false"  EnableSortingAndPagingCallbacks="True" CssClass="Grid">
                <Columns>
                    <asp:BoundField DataField="NumeroSerie" HeaderText="# Serie" SortExpression="NumeroVIN">
                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Anio" HeaderText="A&ntilde;o" SortExpression="Anio">
                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                        <ItemStyle HorizontalAlign="Left" Width="20px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Dimensiones" HeaderText="Dimensiones" SortExpression="Dimensiones">
                        <HeaderStyle HorizontalAlign="Left" Width="180px" />
                        <ItemStyle HorizontalAlign="Left" Width="180px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PBV" HeaderText="PBV" SortExpression="PBV">
                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PBC" HeaderText="PBC" SortExpression="PBC">
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
    <fieldset>
        <legend>Informaci&oacute;n del seguro</legend>
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">Compa&ntilde;&iacute;a</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUnidadCompaniaSeguro" runat="server" Width="220px"></asp:TextBox>
                </td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical"># P&oacute;liza</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtUnidadNumeroPoliza" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">Importe Deducible</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:HiddenField runat="server" ID="hdnUnidadPorcentajeDeducible"/>
                    <asp:TextBox ID="txtUnidadImporteDeducible" runat="server" Width="115px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </fieldset>
</fieldset>
<fieldset>
    <legend>Condiciones de renta</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">Destino/&Aacute;rea Operaci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesAreaOperacion"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Mercanc&iacute;a a transportar</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesMercancia"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Motivo de Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlCondicionesMotivoRenta">
                        <asp:ListItem Value=" ">SELECCIONE UNA OPCIÓN</asp:ListItem>
                        <asp:ListItem Value="0">SUSTITUCION TEMPORAL</asp:ListItem>
                        <asp:ListItem Value="1">UNIDAD EXTRA</asp:ListItem>
                        <asp:ListItem Value="2">DEMOSTRACIÓN</asp:ListItem>
                        <asp:ListItem Value="3">MATERIAL PELIGROSO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Fecha Devoluci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesFechaDevolucion" AutoPostBack="True"
                        ontextchanged="txtCondicionesFechaDevolucion_TextChanged"></asp:TextBox>
                </td>
            </tr>

            <%--SC0034, Campo de hora oculto --%>
            <tr style="display:none">
                <td class="tdCentradoVertical"><span>*</span>Hora Devoluci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesHoraDevolucion" AutoPostBack="True" CssClass="CampoHora" 
                        ontextchanged="txtFechaContrato_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px">El Vehículo ser&aacute; devuelto a</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesDireccionRegreso"
                    Rows="5" Columns="30" TextMode="MultiLine" MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px;
                    max-height: 90px; min-height: 90px;"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Moneda</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlMonedas" AutoPostBack="True" AppendDataBoundItems="true" 
                        onselectedindexchanged="ddlMonedas_SelectedIndexChanged"/>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Frecuencia Facturaci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlFrecuenciaFacturacion">
                        <asp:ListItem Value="2">DIARIA</asp:ListItem>
                        <asp:ListItem Value="3">SEMANAL</asp:ListItem>
                        <asp:ListItem Value="4">QUINCENAL</asp:ListItem>
                        <asp:ListItem Value="0">MENSUAL</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Bit&aacute;cora Viaje</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlBitacoraViaje">
                        <asp:ListItem Value="1">SI</asp:ListItem>
                        <asp:ListItem Selected="True" Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">D&iacute;as Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesDiasRenta" MaxLength="3" Width="20px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>D&iacute;as Primera Factura</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtDiasFacturar" MaxLength="3" Width="20px" 
                        AutoPostBack="True" ontextchanged="txtDiasFacturar_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Lector Kilometraje</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:DropDownList runat="server" ID="ddlLectorKilometraje">
                        <asp:ListItem Value="0">ODOMETRO</asp:ListItem>
                        <asp:ListItem Value="1">HUBODOMETRO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Forma Pago</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:UpdatePanel ID="updFormaPago" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlFormaPago" AutoPostBack="true" onselectedindexchanged="ddlFormaPago_SelectedIndexChanged">
                                <asp:ListItem Value="0">CRÉDITO</asp:ListItem>
                                <asp:ListItem Selected="True" Value="1">CONTADO</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlFormaPago" EventName="SelectedIndexChanged"/>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>                
            </tr>
            <tr>
                <td colspan="3">
                    <table runat="server" id="tblPagoCredito" class="trAlinearDerecha">
                        <tr runat="server" id="trPagoCredito">
                            <td class="tdCentradoVertical" style="width: 185px;"><span>*</span>Tipo Autorizaci&oacute;n de Cr&eacute;dito</td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" align="left">
                                <asp:DropDownList runat="server" ID="ddlTipoPagoCredito" Visible="False" AutoPostBack="true" onselectedindexchanged="ddlTipoPagoCredito_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="x">SELECCIONE UNA OPCIÓN</asp:ListItem>
                                    <asp:ListItem Value="0">ORDEN DE COMPRA</asp:ListItem>
                                    <asp:ListItem Value="1">CONFIRMACIÓN DE RENTA</asp:ListItem>
                                    <asp:ListItem Value="2">RENTA DIRECTA</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical"><span>*</span>Autorizador del Cr&eacute;dito</td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" align="left">
                                <asp:TextBox runat="server" ID="txtPersonaAutorizaCredito" MaxLength="120"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trAutorizaOrdenCompra" visible="False">
                            <td class="tdCentradoVertical"><span>*</span>Orden de Compra</td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" align="left">
                                <asp:TextBox runat="server" ID="txtPersonaAutorizaOrdenCompra" MaxLength="120"></asp:TextBox>
                            </td>
                        </tr>
                    </table> 
                </td>
            </tr>            
            <tr style="margin-top: 5px">
                <td class="tdCentradoVertical">Importe Dep&oacute;sito</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesImporteDeposito" MaxLength="10" Width="200px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</fieldset>

<fieldset>
    <legend>Tarifas</legend>    
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Descripci&oacute;n</td>
                <td style="width: 20px">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:HiddenField runat="server" ID="hdnTarifaID"/>
                    <asp:HiddenField runat="server" ID="hdnTarifaCodigoMoneda"/>
                    <asp:HiddenField runat="server" ID="hdnTarifaSucursalID"/>
                    <asp:HiddenField runat="server" ID="hdnTarifaModeloID"/>
                    <asp:HiddenField runat="server" ID="hdnTarifaUnidadOperativaID"/>
                    <asp:HiddenField runat="server" ID="hdnTarifaCuentaClienteID"/>
                    <asp:TextBox runat="server" ID="txtTarifaDescripcion" MaxLength="80" Width="200px" AutoPostBack="true" OnTextChanged="txtTarifaDescripcion_TextChanged"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="ibtnBuscarTarifa" 
                        CommandName="VerNumeroEconomico" ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                        ToolTip="Consultar tarifas" CommandArgument='' 
                        onclick="ibtnBuscarTarifa_Click"/>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Tarifa diaria</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtTarifaDiaria" MaxLength="15" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">KM Libres</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtTarifaKMLibres" MaxLength="5" Width="50px" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="tdCentradoVertical">Tarifa KM Adicional</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtTarifaKMAdicional" MaxLength="15" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
			<tr>
				<td class="tdCentradoVertical">
					&nbsp;
				</td>
				<td style="width: 20px;">
					&nbsp;
				</td>
				<td class="tdCentradoVertical">
					<asp:Button runat="server" ID="btnPersonalizarTarifa" Text="Personalizar" CssClass="btnWizardEditar" OnClick="btnPersonalizarTarifa_Click" />
				</td>
			</tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
        <tr>
            <td class="tdCentradoVertical">Tipo de Tarifa</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:DropDownList runat="server" ID="ddlTipoTarifa">
                    <asp:ListItem Value="0">LOCAL</asp:ListItem>
                    <asp:ListItem Value="1">NACIONAL</asp:ListItem>
                    <asp:ListItem Value="2">ESPECIAL</asp:ListItem>
                    <asp:ListItem Value="3">PERSONALIZADA</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Capacidad de Carga</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtTarifaCarga" MaxLength="100" Width="100px" CssClass="CampoNumeroEntero"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Horas Libres</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtTarifaHorasLibres" MaxLength="5" Width="50px" CssClass="CampoNumeroEntero"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical">Tarifa Hora Adicional</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtTarifaHoraAdicional" MaxLength="15" Width="100px" CssClass="CampoNumero"></asp:TextBox>
            </td>
        </tr>
    </table>
    </div>
</fieldset>
<fieldset>
    <legend>Cargos Adicionales</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">Por D&iacute;a de posesión</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 200px;">
                    <asp:TextBox runat="server" ID="txtCargosHoraPosesion" MaxLength="100" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Por Alteraci&oacute;n de medidor de kilometraje</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 200px;">
                    <asp:TextBox runat="server" ID="txtCargosAlteracionMedidor" MaxLength="100" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">% facturaci&oacute;n posterior a <asp:Label runat="server" ID="lblDiasPago"></asp:Label> d&iacute;as de la fecha de facturaci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 150px;">
                    <asp:TextBox runat="server" ID="txtCargosPorcentajeFacturacion" MaxLength="100" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Por Entrega Inpuntual</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 150px;">
                    <asp:TextBox runat="server" ID="txtCargosEntregaInpuntual" MaxLength="100" Width="100px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>    
</fieldset>
<fieldset>
    <legend>Observaciones</legend>
    <table class="trAlinearDerecha">
        <tr>
            <td align="right" style="padding-top: 5px">Observaci&oacute;n</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical">
                <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Width="750px" Height="90px"
                        Style="max-width: 750px; min-width: 750px; max-height: 90px; min-height: 90px;">
                    </asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />

<!-- Dialogos -->
<asp:HiddenField ID="hdnCodigoAutorizacion" runat="server" />
<asp:HiddenField runat="server" ID="hdnMostrarDialogoTarifa" Value="0"/>
<asp:HiddenField ID="hdnEstatusAutorizacion" runat="server" />
<div id="DialogoTarifaPersonal" style="display: none">
	<fieldset>
		<legend>Tarifas</legend>
		<div class="dvIzquierda">
			<table class="trAlinearDerecha" style="width:350px">
				<tr>
					<td class="tdCentradoVertical">
						Tarifa diaria
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTarifaDiaria" MaxLength="15"
							Width="100px" CssClass="CampoNumero"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="tdCentradoVertical">
						KM Libres
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaKmsLibres" MaxLength="5" Width="50px"
							CssClass="CampoNumeroEntero"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="tdCentradoVertical">
						Tarifa KM Adicional
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTarifaKmAdicional" MaxLength="15"
							Width="100px" CssClass="CampoNumero"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="tdCentradoVertical">
						C&oacute;digo de Autorizaci&oacute;n</td>
					<td style="width: 20px;">
						&nbsp;</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaCodigoAutorizacion" MaxLength="15"
							Width="100px" Enabled="False"></asp:TextBox>
					</td>
				</tr>
				</table>
		</div>
		<div class="dvDerecha">
			<table class="trAlinearDerecha" style="width: 350px">
				<tr>
					<td class="tdCentradoVertical">
						Tipo de Tarifa
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:DropDownList runat="server" ID="ddlTarifaPersonalizadaTipoTarifa" Enabled="False">
							<asp:ListItem Value="0">LOCAL</asp:ListItem>
							<asp:ListItem Value="1">NACIONAL</asp:ListItem>
							<asp:ListItem Value="2">ESPECIAL</asp:ListItem>
							<asp:ListItem Value="3">PERSONALIZADA</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="tdCentradoVertical">
						Capacidad de Carga
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaCapacidadCarga" MaxLength="100"
							Width="100px" CssClass="CampoNumeroEntero"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="tdCentradoVertical">
						Horas Libres
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaHrsLibres" MaxLength="5" Width="50px"
							CssClass="CampoNumeroEntero"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td class="tdCentradoVertical">
						Tarifa Hora Adicional
					</td>
					<td style="width: 20px;">
						&nbsp;
					</td>
					<td class="tdCentradoVertical">
						<asp:TextBox runat="server" ID="txtTarifaPersonalizadaTarifaHrAdicional" MaxLength="15"
							Width="100px" CssClass="CampoNumero"></asp:TextBox>
					</td>
				</tr>
				</table>
				<br />
            <asp:Button runat="server" Text="Solicitar" 
                ID="btnSolicitarAutorizacion" CssClass="btnWizardContinuar" 
                onclick="btnSolicitarAutorizacion_Click" />
				 <asp:button runat="server" ID="btnValidarAutorizacion" Text="Validar" 
							onclick="btnValidarAutorizacion_Click" CssClass="btnWizardGuardar" /> 
            <asp:Button runat="server" ID="btnCancelarCambioTarifa" Text="Cancelar" 
                CssClass="btnWizardCancelar" onclick="btnCancelarCambioTarifa_Click" OnClientClick="MostrarDialogo(false);"/>
		</div>
	</fieldset>
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
<asp:HiddenField runat="server" ID="hdnProductoServicioId" Value="" />