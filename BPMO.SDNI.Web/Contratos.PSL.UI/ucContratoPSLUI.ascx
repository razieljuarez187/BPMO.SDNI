<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContratoPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucContratoPSLUI" %>
<%@ Import Namespace="BPMO.SDNI.Comun.BO" %>


<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/ObtenerFormatoImporte.js" type="text/javascript"></script>  
<script type="text/javascript">
    // Inicializa el control de Información General
    function <%= ClientID %>_Inicializar(agregarevento, esrenovacion) {
        if (agregarevento == null || agregarevento== true) {
            
            if (esrenovacion == null || esrenovacion == false) {
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
            
        
                var fechainicioarrendamiento = $('#<%= txtCondicionesFechaInicioActual.ClientID %>');
                if (fechainicioarrendamiento.length > 0) {
                    fechainicioarrendamiento.datepicker({
                        yearRange: '-10:+10',
                        changeYear: true,
                        changeMonth: true,
                        showButtonPanel: true,
                        dateFormat: "dd/mm/yy",
                        buttonImage: '../Contenido/Imagenes/calendar.gif',
                        buttonImageOnly: true,
                        toolTipText: "Fecha de Inicio del Arrendamiento",
                        showOn: 'button',
                        defaultDate: (fechainicioarrendamiento.val().length == 10) ? fechainicioarrendamiento.val() : new Date()
                    });

                    fechainicioarrendamiento.attr('readonly', true);        
                }
            }
        
            var fechapropdevolucion = $('#<%= txtCondicionesFechaPromesaActual.ClientID %>');
            if (fechapropdevolucion.length > 0) {
                fechapropdevolucion.datepicker({
                    yearRange: '-10:+10',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha de Promesa de Devolución",
                    showOn: 'button',
                    defaultDate: (fechapropdevolucion.val().length == 10) ? fechapropdevolucion.val() : new Date()
                });

                fechapropdevolucion.attr('readonly', true);
            }

            var fechapagorenta = $('#<%= txtROCFechaPagoRenta.ClientID %>');
            if (fechapagorenta.length > 0) {
                fechapagorenta.datepicker({
                    yearRange: '-10:+10',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha de Pago de la Renta",
                    showOn: 'button',
                    defaultDate: (fechapagorenta.val().length == 10) ? fechapagorenta.val() : new Date()
                });

                fechapagorenta.attr('readonly', true);
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

    function abrirSubtotales() {
      $("#dialogSubtotales").dialog({
            modal: true,
            width: 750,
            height: 400,
            resizable: false,
            title: 'Resumen del Monto del Contrato',
            buttons: {
                "Aceptar": function () {
                    $(this).dialog("destroy");
                }
            }
        });
        $("#dialogSubtotales").parent().appendTo("form:first");
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
    <script type="text/javascript">
            function confirmarEliminarTarifas(ctrl,valAnterior, diasAnterior, modo) {
                var $div = $('<div title="Confirmación"></div>');
                var leyenda = 'eliminarán';
                if (modo != null && modo == 'REN')
                    leyenda = 'modificarán';
                $div.append('Actualmente existen unidades capturadas.<br />Si continúa se ' + leyenda +' las tarifas configuradas en ellas.<br />¿Desea continuar?');
                $("#dialog:ui-dialog").dialog("destroy");
                $($div).dialog({
                    closeOnEscape: true,
                    modal: true,
                    minWidth: 460,
                    close: function () { $(this).dialog("destroy"); },
                    buttons: {
                        Aceptar: function () {
                            $(this).dialog("close");
                            __doPostBack("<%= btnEliminarTarifas.UniqueID %>", "");
                        },
                        Cancelar: function () {
                            $(this).dialog("close");
                            var param = ctrl + "|" + valAnterior + "|" + diasAnterior;
                            document.getElementById('<%=hdnValoresCambiar.ClientID%>').value = param;
                            __doPostBack("<%= btnCancelarEliminarTarifas.UniqueID %>", "");
                        }
                    }
                });
            }
    </script>
<fieldset>
    <legend>Información General</legend>
    <asp:HiddenField runat="server" ID="hdnContratoID"/>
    <asp:HiddenField runat="server" ID="hdnEstatusContratoID"/>
    <asp:HiddenField runat="server" ID="hdnNumeroContrato"/>
    <asp:HiddenField runat="server" ID="hdnTipoContrato"/>
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
                    <asp:DropDownList ID="ddlSucursales" runat="server" Width="275px" 
                        AutoPostBack="true" onselectedindexchanged="ddlSucursales_SelectedIndexChanged">
                        <asp:ListItem Text="" Value="0"></asp:ListItem>
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
                <td class="tdCentradoVertical"><asp:TextBox ID="txtRepresentante" runat="server" MaxLength="250" Width="280px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px">Domicilio Empresa</td>
                <td style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDireccionEmpresa" runat="server" Rows="4" Columns="30" TextMode="MultiLine"
                                MaxLength="500" Style="float: left; max-width: 280px; min-width: 280px;
                                max-height: 70px; min-height: 70px;"></asp:TextBox>
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
                    <tr id="trAvales" runat="server">
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
    <legend>Condiciones de renta y facturación</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Ubicaci&oacute;n de Entrega del Bien</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesAreaOperacion"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Uso del Bien</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesMercancia"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Fecha Inicio Arrendamiento</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesFechaInicioActual" AutoPostBack="True" 
                    ontextchanged="txtCondicionesFechaInicioActual_TextChanged"></asp:TextBox>
                </td>
            </tr>           
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Fecha Devoluci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesFechaPromesaActual" AutoPostBack="True"
                        ontextchanged="txtCondicionesFechaPromesaActual_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Tasa inter&eacute;s</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtTasaInteres" AutoPostBack="True" CssClass="CampoNumero"
                        ontextchanged="txtTarifaInteres_TextChanged">0</asp:TextBox>&nbsp;&#37;
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
                <td class="tdCentradoVertical">D&iacute;as Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtCondicionesDiasRenta" MaxLength="10" Width="40px"></asp:TextBox>
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
                        <tr id="trAutorizadorCredito" runat="server">
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
</fieldset>

<fieldset id="opcionesROC" runat="server">
    <legend>Renta con Opci&Oacute;n a Compra</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical" style="width: 250px;"><span>*</span>Monto Total del Arrendamiento + IVA</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtMontoArrendamiento"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Fecha de pago de cada renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtROCFechaPagoRenta" AutoPostBack="True"
                        ontextchanged="txtROCFechaPagoRenta_TextChanged"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Plazo</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox runat="server" ID="txtPlazo" MaxLength="3" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Inversi&oacute;n Inicial</td>
                <td class="tdCentradoVertical" style="width: 5px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtInversionInicial" runat="server" Width="150px" MaxLength="10"></asp:TextBox>
                </td>
            </tr>            
        </table>
    </div>
</fieldset>

<fieldset>
    <legend>
        <asp:Label runat="server" ID="lblTitulo" Text="Agregar Unidades a Renta"></asp:Label></legend>
    <table class="trAlinearDerecha">
        <tr id="trAgregarUnidades" runat="server">
            <td class="tdCentradoVertical" style="width: 100px;">
                <span>*</span><asp:Label runat="server" ID="lblVIN" Text="SERIE UNIDAD"></asp:Label>
            </td>
            <td style="width: 5px;">
                &nbsp;
            </td>
            <td class="tdCentradoVertical" style="width: 330px;">
                <asp:TextBox ID="txtNumeroSerie" runat="server" Width="275px" AutoPostBack="true" OnTextChanged="txtNumeroSerie_TextChanged"></asp:TextBox>
                <asp:ImageButton ID="ibtnBuscarUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                    OnClick="ibtnBuscarUnidad_Click" Style="width: 17px" />
            </td>
            <td>
                <asp:UpdatePanel ID="updAgregarUnidad" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnAgregarUnidad" CssClass="btnAgregarATabla" runat="server" Text="Agregar Unidad"
                                    Enabled="False" OnClick="btnAgregarUnidad_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtNumeroSerie" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ibtnBuscarUnidad" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <div class="LineasContrato">
        <asp:UpdatePanel runat="server" ID="updLineasContratos" ChildrenAsTriggers="True">
            <ContentTemplate>
                <asp:GridView ID="grdLineasContrato" runat="server" AutoGenerateColumns="False" PageSize="10"
                    AllowPaging="True" CellPadding="4" GridLines="None" CssClass="Grid" Width="90%"
                    AllowSorting="True" OnRowDataBound="grdLineasContrato_RowDataBound" OnPageIndexChanging="grdLineasContrato_PageIndexChanging"
                    OnRowCommand="grdLineasContrato_RowCommand" >
                    <Columns>
                        <asp:TemplateField HeaderText="SERIE UNIDAD">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblVIN"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Justify" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modelo">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblModelo"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Justify" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblAnio"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Tipo Tarifa" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTipoTarifa"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Justify" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Turno" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTurno"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Justify" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Maniobra" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblManiobra"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                        </asp:TemplateField>                  
                        <asp:TemplateField HeaderText="ACTIVA" SortExpression="">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblActiva"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="6%" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                                    CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                            </ItemTemplate>
                            <ItemStyle Width="7px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
                                    CommandName="CMDDETALLES" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                            </ItemTemplate>
                            <ItemStyle Width="7px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ChkList">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnChkList" ImageUrl="~/contenido/Imagenes/contrato.png" runat="server" ToolTip="Imprimir"
                                    CommandName="CMDCHKLIST" CommandArgument='<%#Container.DataItemIndex%>' />
                            </ItemTemplate>
                        <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle"  />
                    </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center" />
                    <EditRowStyle CssClass="GridAlternatingRow" />
                    <PagerStyle CssClass="GridPager" />
                    <RowStyle CssClass="GridRow" />
                    <FooterStyle CssClass="GridFooter" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                    <EmptyDataTemplate>
                        <b>No se han asignado Unidades al Contrato.</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Button ID="btnVerSubtotales" runat="server" Text="Subtotales" 
        Style="float: right; margin: 10px 5px 10px 10px" CssClass="btnComando" 
        onclick="btnVerSubtotales_Click" />
    <asp:HiddenField runat="server" ID="hdnUnidadID" />
    <asp:HiddenField runat="server" ID="hdnEquipoID" />
</fieldset>

<fieldset>
    <legend>Cargos Adicionales</legend>
    <div class="dvIzquierda">
        <table class="trAlinearDerecha">
            <tr>
                <td class="tdCentradoVertical">Maniobra</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" style="width: 200px;">
                    <asp:TextBox runat="server" ID="txtManiobra" MaxLength="100" Width="100px" CssClass="CampoNumero"></asp:TextBox>
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
<asp:Button ID="btnEliminarTarifas" runat="server" Text="Button" OnClick="btnEliminarTarifas_Click" style="display: none;" />
<asp:Button ID="btnCancelarEliminarTarifas" runat="server" Text="Button" OnClick="btnCancelarEliminarTarifas_Click" style="display: none;" />
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

<div id="dialogSubtotales"  style="display: none;" title="MONTO DEL CONTRATO">                   
    <div id="divDetalle" class="equiposAliados">
        <br />
        <div id="divLstUnidades" class="equiposAliados">
            <asp:GridView ID="grdMontoUnidades" runat="server" AutoGenerateColumns="False" PageSize="10"
            AllowPaging="false" CellPadding="0" GridLines="None" CssClass="Grid" Width="100%" OnRowDataBound="grdMontoUnidades_RowDataBound" >
            <Columns>
                <asp:TemplateField HeaderText="SERIE UNIDAD">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSerie"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="20%" HorizontalAlign="Justify" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tarifa Base"  >
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTarifaBase"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tarifa Personalizada">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTarifa"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Maniobra">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblManiobra"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Seguro">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSeguro"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Monto a Cobrar" >
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblMonto"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50%" HorizontalAlign="Right" />
                </asp:TemplateField>    
            </Columns>
            <HeaderStyle CssClass="GridHeader" HorizontalAlign="Justify" />
            <EditRowStyle CssClass="GridAlternatingRow" />
            <PagerStyle CssClass="GridPager" />
            <RowStyle CssClass="GridRow" />
            <FooterStyle CssClass="GridFooter" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
            <EmptyDataTemplate>
                <b>No se han asignado Unidades al Contrato.</b>
            </EmptyDataTemplate>
        </asp:GridView>
            </div>
        <div id="FooterLstUnidades" class="equiposAliados">
        <table  style="width: 100%;  margin-right: 10px;">
            <tr>
                <td>                            
                </td>
                <td>                        
                </td>                       
                <td align="right">
                    Total Factura:
                    <asp:TextBox ID="txtSumaTotal" runat="server" Width="200px" ReadOnly="true" Style="text-align: right;">$0.00</asp:TextBox>
                </td>
            </tr>
        </table>
        </div>
    </div>           
</div>

<asp:HiddenField runat="server" ID="hdnValoresCambiar" Value="" />
<asp:HiddenField runat="server" ID="hdnProductoServicioId" Value="" />
<asp:HiddenField runat="server" ID="hdnFechaContrato" Value="" />
<asp:HiddenField runat="server" ID="hdnCondicionesFechaInicioActual" Value="" />
<asp:HiddenField runat="server" ID="hdnCondicionesFechaPromesaActual" Value="" />
<asp:HiddenField runat="server" ID="hdnCondicionesFechaPagoRenta" Value="" />
<asp:HiddenField runat="server" ID="hdnCondicionesMoneda" Value="" />
<asp:HiddenField runat="server" ID="hdnCondicionesFechaInicioArrendamiento" Value="" />
<asp:HiddenField runat="server" ID="hdnCondicionesFechaPromesaDevolucion" Value="" />
<asp:HiddenField runat="server" ID="hdnModoRegistro" Value="" />
<asp:HiddenField runat="server" ID="hdnIncluyeSD" Value="" />
<asp:HiddenField runat="server" ID="hdnPorcentajeSeguro" Value="" />
<asp:HiddenField runat="server" ID="hdnEsROC" Value="" />
