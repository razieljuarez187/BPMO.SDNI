<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Pagos.master" AutoEventWireup="true" CodeBehind="ConsultarPagosNoFacturadosUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.ConsultarPagosNoFacturadosUI" %>
<%@ Import Namespace="BPMO.SDNI.Facturacion.InfraestructuraPagos.BO" %>
<%--Satisface al caso de uso CU012 - Ver Pagos No Facturados--%>
<%-- Satisface a la solicitud de cambio SC0015 --%>
<%-- Satisface a la solicitud de cambio SC0027 --%>
<%-- Satisface a la solicitud de cambio SC0035 --%>

<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
<script type="text/javascript">
    // Inicializa el control de Información General
    initChild = function () {
                <%=ClientID %>_Inicializar();
            };
    $(document).ready(initChild);
    function <%= ClientID %>_Inicializar() {
        var mostrarDialogo = $("#<%= hdnMostrarDialogoPago.ClientID %>").val();
		if(mostrarDialogo=="1") MostrarDialogo(true);
    }

    function MostrarDialogo(mostrar) {              
	    $("#<%= hdnMostrarDialogoPago.ClientID %>").val("0");
		$("#DialogoCancelarPago").dialog({
				modal: true,
				width: 800,
				autoOpen:false,
				resizable: false,
				title: "Cancelar Solicitud de Pago"
			});
        
		$("#DialogoCancelarPago").parent().appendTo("form:first");
	    if(mostrar) {
		    $("#DialogoCancelarPago").dialog("open");
		    $("#<%= hdnMostrarDialogoPago.ClientID %>").val("1");
	    }
    }

    function confirmarCancelarPago() {
                var $div = $('<div title="Confirmación"></div>');
                $div.append('¿Desea cancelar el pago?');
                $("#dialog:ui-dialog").dialog("destroy");
                $($div).dialog({
                    closeOnEscape: true,
                    modal: true,
                    minWidth: 460,
                    close: function () { $(this).dialog("destroy"); },
                    buttons: {
                        SI: function () {
                            $(this).dialog("close");
                            __doPostBack("<%= btnCancelarPagoPendiente.UniqueID %>", "");
                        },
                        NO: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="childTitulo" runat="server">
    Consultar Pagos
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="childSubtitulo" runat="server">
    No facturados
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="childContent" runat="server">
	<asp:GridView runat="server" ID="grdPagos" AutoGenerateColumns="False"
		AllowPaging="True" EnableSortingAndPagingCallbacks="True"
		CssClass="Grid" OnPageIndexChanging="grdPagos_PageIndexChanging" OnRowDataBound="grdPagos_RowDataBound"
		Width="100%">
		<Columns>
			<%-- 0 --%>
			<asp:TemplateField HeaderText="#Contrato">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblFolio" Width="100%"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                <ItemStyle HorizontalAlign="Left" Width="80px" />
            </asp:TemplateField>
            <%-- 1 --%>
            <asp:TemplateField HeaderText="Cuenta Oracle / Cliente">
                <ItemTemplate>                    
                    <asp:Label runat="server" ID="lblCliente" Width="100%"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
			<%-- 2 --%>
            <asp:TemplateField HeaderText="Inicio Cto">
                <ItemTemplate>                    
                    <asp:Label runat="server" ID="lblFechaInicio" Width="100%"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                <ItemStyle HorizontalAlign="Left" Width="60px" />
            </asp:TemplateField>
            <%-- 3 --%>
            <asp:TemplateField HeaderText="Referencia">
                <ItemTemplate> 
                    <asp:Label runat="server" ID="lblReferencia" Width="100%"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                <ItemStyle HorizontalAlign="Left" Width="120px" />
            </asp:TemplateField>
			<%-- 4 --%>
			<asp:TemplateField HeaderText="Pago/Plazo">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblPagoPlazo" Width="100%"></asp:Label>
				</ItemTemplate>
				<HeaderStyle HorizontalAlign="Left" />
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<%-- 5 --%>
            <asp:TemplateField HeaderText="Demora">
				<ItemTemplate>
					<asp:Label runat="server" ID="lblDemora" Width="100%"></asp:Label>
				</ItemTemplate>
				<HeaderStyle HorizontalAlign="Left" />
				<ItemStyle HorizontalAlign="Left" />
			</asp:TemplateField>
			<%-- 6 --%>			
            <%-- SC0027, Adición de la columna de Total Factura para visualizar  --%>
            <asp:TemplateField HeaderText="Total Factura">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblTotalFactura" Width="100%"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
				<ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <%-- 7 --%>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="btnCancelar" CommandName="Cancelar" ImageUrl="~/Contenido/Imagenes/ESTATUS-NO-ICO.png"
                        ToolTip="Cancelar pago" 
                        ImageAlign="Middle" OnClick="btnCancelar_Click" />
                </ItemTemplate>
                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
            <%-- 8 --%>
			<asp:TemplateField>
			    <ItemTemplate>
            <asp:ImageButton runat="server" ID="btnMover" CommandName="Configurar" ImageUrl="~/Contenido/Imagenes/VER.png"
                        ToolTip="Mover a Pagos a Facturar" 
                        ImageAlign="Middle" OnClick="btnMover_Click" />				
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
    <!-- Diálogos -->
    <asp:Button ID="btnCancelarPagoPendiente" runat="server" Text="Button" OnClick="btnCancelarPagoPendiente_Click" style="display: none;" />
    <asp:HiddenField ID="hdnCodigoAutorizacion" runat="server" />
    <asp:HiddenField runat="server" ID="hdnMostrarDialogoPago" Value="0"/>
    <asp:HiddenField ID="hdnEstatusAutorizacion" runat="server" />
    <asp:HiddenField ID="hdnPagoACancelarID" runat="server" />
    <div id="DialogoCancelarPago" style="display: none">
	    <fieldset>
		    <legend>Cancelar Pago</legend>
		    <div class="dvIzquierda">
			    <table class="trAlinearDerecha" style="width:350px">
				    <tr>
					    <td class="tdCentradoVertical">
						    <span>*</span>Motivo
					    </td>
					    <td style="width: 20px;">
						    &nbsp;
					    </td>
					    <td class="tdCentradoVertical">
						    <asp:TextBox runat="server" ID="txtMotivoCancelacion" MaxLength="50" TextMode="MultiLine" Rows="3" Columns="20"
							    Width="200px"></asp:TextBox>
					    </td>
				    </tr>			
				</table>
		    </div>
		    <div class="dvDerecha">
			    <table class="trAlinearDerecha" style="width: 350px">
				    <tr>
					    <td class="tdCentradoVertical">
						    C&oacute;digo de Autorizaci&oacute;n</td>
					    <td style="width: 20px;">
						    &nbsp;</td>
					    <td class="tdCentradoVertical">
						    <asp:TextBox runat="server" ID="txtCancelaPagoCodigoAutorizacion" MaxLength="15"
							    Width="100px" Enabled="False"></asp:TextBox>
					    </td>
				    </tr>
				    </table>
				    <br />
                <asp:Button runat="server" Text="Solicitar" 
                    ID="btnSolicitarAutorizacion" CssClass="btnWizardContinuar" 
                    onclick="btnSolicitarAutorizacion_Click" OnClientClick="__blockUIModal();" />
				     <asp:button runat="server" ID="btnValidarAutorizacion" Text="Validar" 
							    onclick="btnValidarAutorizacion_Click" OnClientClick="__blockUIModal();" CssClass="btnWizardGuardar" /> 
                <asp:Button runat="server" ID="btnCancelarCambioTarifa" Text="Cancelar" 
                    CssClass="btnWizardCancelar" onclick="btnDescartarCancelacionPago_Click" OnClientClick="MostrarDialogo(false);"/>
		    </div>
	    </fieldset>
    </div>
	<asp:HiddenField runat="server" ID="hdnPMP" />
</asp:Content>
