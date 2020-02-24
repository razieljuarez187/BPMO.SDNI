<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" 
    AutoEventWireup="true" CodeBehind="CerrarContratoRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.CerrarContratoRDUI" %>
    
<%-- Satisface al Caso de Uso CU013 - Cerrar Contrato de Renta Diaria--%>
<%@ Register Src="ucResumenContratoRDUI.ascx" TagName="ucResuContratoRDUI" TagPrefix="uc" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<%@ Register Src="~/Flota.UI/ucDatosGeneralesElementoUI.ascx" TagPrefix="uc" TagName="ucDatosGeneralesElementoUI" %>
<%@ Register Src="ucHerramientasRDUI.ascx" TagName="HerramientasRDUI" TagPrefix="uc" %>
<%@ Register Src="~/Contratos.RD.UI/ucTarifaRDUI.ascx" TagPrefix="uc" TagName="ucTarifaRDUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #BarraHerramientas
        {
            width: 832px !important;
        }
    </style>
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
    <script type="text/javascript">
        initChild = function () {
            <%= ucHerramientas.ClientID %>_Inicializar();    
            
            var fecha = $('#<%= txtFechaCierre.ClientID %>');
            if (fecha.length > 0) {
                fecha.datepicker({
                    yearRange: '-100:+10',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha de Cancelación",
                    showOn: 'button',
                    defaultDate: (fecha.val().length == 10) ? fecha.val() : new Date()
                });

                fecha.attr('readonly', true);
            }
                        
            var hora = $('#<%= txtHoraCierre.ClientID %>');
            if (hora.length > 0) {
                hora.timepicker({
                    showPeriod: true,
                    showLeadingZero: true
                });
            }
            hora.attr('readonly', true);        
        };
            
        $(document).ready(initChild);
    </script>
    <script type="text/javascript">
        function confirmarCierre() {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('¿Está seguro que desea cerrar el contrato?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack("<%= btnGuardar.UniqueID %>", "");
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="PaginaContenido">
    <!-- Barra de localización -->
    <div id="BarraUbicacion">
        <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CERRAR CONTRATO DE RENTA DIARIA</asp:Label>
    </div>
    <!--Navegación secundaria-->
    <div style="height: 80px;">
        <!-- Menú secundario -->
        <ul id="MenuSecundario" style="float: left; height: 64px;">
            <li class="MenuSecundarioSeleccionado">
                <asp:HyperLink ID="hlkConsulta" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarContratoRDUI.aspx">
                    CONSULTAR R.D.
                    <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.RD.UI/RegistrarContratoRDUI.aspx">
                    REGISTRAR RENTA R.D. 
                    <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                </asp:HyperLink>
            </li>
        </ul>
        <!-- Barra de herramientas -->
        <uc:HerramientasRDUI ID="ucHerramientas" runat="server" />
    </div>
    <div id="divInformacionGeneral" class="GroupBody">
		<div id="divInformacionGeneralHeader" class="GroupHeader">
			<span>Contrato De Renta Diaria</span>
			<div class="GroupHeaderOpciones Ancho2Opciones">
                <input type="button" class="btnWizardGuardar" value="Terminar" id="btnGuardarPrevio" onclick="javascript: confirmarCierre();" />
				<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
			</div>
		</div>
		<div id="divInformacionGeneralControles">
			<!--espacio uc informacion contrato -->
            <uc:ucResuContratoRDUI ID="ucResuContratoRD" runat="server" />
		</div>
	</div>
    <div class="GroupBody">
        <div class="GroupHeader">
            <asp:Label ID="lblTituloUnidad" runat="server" Text="INFORMACI&Oacute;N DE LA UNIDAD"></asp:Label>
        </div>
        <uc:ucDatosGeneralesElementoUI ID="ucDatosGeneralesElementoUI" runat="server" />
    </div>
    <div class="GroupBody">
        <div class="GroupHeader">
            <asp:Label ID="lblTituloEquipoAliado" runat="server" Text="INFORMACI&Oacute;N DE EQUIPOS ALIADOS"></asp:Label>
        </div>
        <uc:ucEquiposAliadosUnidadUI ID="ucEquiposAliadosUnidadUI" runat="server" />
    </div>
    <div class="GroupBody">
        <div class="GroupHeader">
            <asp:Label ID="lblImportes" runat="server" Text="IMPORTES KM Y HORAS"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom: 5px;">
            <tr>
                <td class="tdCentradoVertical" style="width: 200px">KM Entrega</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmEntrega" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 200px" align="right">Hrs Entrega</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsEqRefEntrega" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical">KM Recepci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmRecepcion" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Hrs Recepci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsEqRefRecepcion" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical">KM Recorrido</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmRecorrido" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Total Hrs</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtTotHrsEqRefrigeracion" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>        
            <tr>
                <td class="tdCentradoVertical">KM Libres</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmLibres" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right"><span>*</span>Hrs Libres</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsLibres" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>  
            <tr>
                <td class="tdCentradoVertical">KM Excedido</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtKmExcedido" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Hrs Excedidas</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtHrsExcedidas" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Tarifa KM Exc</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtTarifaKmExc" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Tarifa Hr Exc</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtTarifaHrExc" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Monto Total KM Exc</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtMontoTotKmExc" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">Monto Hrs</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtMontoHrsEqRef" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="GroupBody">
        <div class="GroupHeader">
            <asp:Label ID="lblCargosAdicionales" runat="server" Text="CARGOS ADICIONALES"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom: 5px;">
            <tr>
                <td class="tdCentradoVertical" style="width: 210px">Diferencia Combustible</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCombustibleSalida" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 230px" align="right"><span>*</span>Importe Unidad Combustible</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtImporteUnidadCombustible" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical">Importe Total Combustible</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtImporteTotCombustible" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right"><span>*</span>Cargo Abuso de Operaci&oacute;n</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCargoAbusoOperacion" runat="server" Width="175px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical" colspan="4"><span>*</span>Cargo por Disposici&oacute;n de Basura o Art&iacute;culos olvidados en el Veh&iacute;culo</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtCargoDispBasura" runat="server" Width="175px" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="GroupBody">
        <div class="GroupHeader">
            <asp:Label ID="lblDeposito" runat="server" Text="DEPÓSITO/REEMBOLSO"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom: 5px;">
            <tr>
                <td class="tdCentradoVertical" style="width: 210px">Importe Dep&oacute;sito</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:TextBox ID="txtImporteDeposito" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical"><span>*</span>Importe Reembolso</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtImporteRembolso" runat="server" Width="175px" CssClass="CampoNumero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 230px" align="right"><span>*</span>Persona Recibe Reembolso</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtPersonaRecibeRembolso" runat="server" Width="175px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="GroupBody">
        <div class="GroupHeader">
            <asp:Label ID="lblDiasRenta" runat="server" Text="DÍAS RENTA"></asp:Label>
        </div>
        <table class="trAlinearDerecha" style="margin-bottom:5px;">
            <tr>
                <td class="tdCentradoVertical" style="width: 240px">D&iacute;as Renta Programada</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtNumDiasRentProg" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" style="width: 200px" align="right">D&iacute;as Reales Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDiasRealesRenta" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>    
            <tr>
                <td class="tdCentradoVertical">D&iacute;as en Taller Durante Renta</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDiasUnidadTaller" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
                <td class="tdCentradoVertical" align="right">D&iacute;as Adicionales Cobro</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical">
                    <asp:TextBox ID="txtDiasAdicionalesCobro" runat="server" Width="175px" Enabled="False" CssClass="CampoNumeroEntero"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical">Monto Total D&iacute;as Adicionales</td>
                <td style="width: 20px;">&nbsp;</td>
                <td class="tdCentradoVertical" colspan="4">
                    <asp:TextBox ID="txtTotalDiasAdicionales" runat="server" Width="175px" Enabled="False" CssClass="CampoNumero"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDatosCierre" class="GroupBody">
		<div id="divDatosCierreHeader" class="GroupHeader">
			<span>DATOS DE CIERRE</span>
		</div>
        <div>
            <div class="dvIzquierda">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 175px;">
                            <span>*</span>Fecha Cierre
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtFechaCierre" runat="server" CssClass="CampoFecha"
                            MaxLength="11" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>Hora Cierre
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtHoraCierre" runat="server" CssClass="CampoHora"
                            MaxLength="7" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="dvDerecha" style="margin-bottom: 5px;">
                <table class="trAlinearDerecha">
                    <tr>
                        <td align="right" style="padding-top: 5px;">
                            <span>*</span>Observaciones
                        </td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtObservacionesCierre" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                            MaxLength="500" Style="float: left; max-width: 250px; min-width: 250px; max-height: 90px;
                            min-height: 90px;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
				<div class="ContenedorMensajes">
					<span class="Requeridos"></span>
					<br />
					<span class="FormatoIncorrecto"></span>
				</div>
			</div>

        </div>
	</div>
</div>
    <asp:HiddenField runat="server" ID="hdnContratoID" />
	<asp:HiddenField runat="server" ID="hdnEstatusID" />
	<asp:HiddenField runat="server" ID="hdnUnidadID" />
	<asp:HiddenField runat="server" ID="hdnEquipoID" />
	<asp:HiddenField runat="server" ID="hdnFUA" />
	<asp:HiddenField runat="server" ID="hdnUUA" />
    
	<asp:HiddenField runat="server" ID="hdnFechaContrato"/>
	<asp:HiddenField runat="server" ID="hdnFechaRecepcion" />
    
	<asp:Button ID="btnGuardar" runat="server" Text="Terminar" style="display: none;" OnClick="btnGuardar_Click" />
</asp:Content>
