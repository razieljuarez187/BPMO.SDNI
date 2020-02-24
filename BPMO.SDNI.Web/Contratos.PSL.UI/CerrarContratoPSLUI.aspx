<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CerrarContratoPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.CerrarContratoPSLUI" %>

<%-- Satisface al Caso de Uso CU013 - Cerrar Contrato de Renta Diaria--%>
<%@ Register Src="ucResumenContratoPSLUI.ascx" TagName="ucResuContratoPSLUI" TagPrefix="uc" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<%@ Register Src="ucHerramientasPSLUI.ascx" TagName="HerramientasPSLUI" TagPrefix="uc" %>
<%@ Register Src="~/Contratos.PSL.UI/ucTarifaPSLUI.ascx" TagPrefix="uc" TagName="ucTarifaPSLUI" %>
<%@ Register Src="~/Contratos.PSL.UI/ucContratoPSLUI.ascx" TagPrefix="uc" TagName="ucInformacionGeneralPSLUI" %>
<%@ Register Src="~/Contratos.PSL.UI/ucLineaContratoPSLUI.ascx" TagPrefix="uc" TagName="ucLineaContratoPSLUI" %>
<%@ Register Src="~/Contratos.PSL.UI/ucCargosAdicionalesCierrePSLUI.ascx" TagPrefix="uc" TagName="ucCargosAdicionalesCierrePSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #BarraHerramientas
        {
            width: 832px !important;
        }
        .Grid
        {
            width: 90%;
            margin: 15px auto 15px auto;
        }
    </style>
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet"
        type="text/css" />
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server"></asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsulta" runat="server">
                    CONSULTAR <asp:Label runat="server" ID="lblTipoContrato"></asp:Label>
                    <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistro" runat="server">
                    REGISTRAR <asp:Label runat="server" ID="lblTipoContrato2"></asp:Label>
                    <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <uc:HerramientasPSLUI ID="ucHerramientas" runat="server" />
        </div>
        <asp:MultiView ID="mvRQMCIERRE" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwCargosAdicionales" runat="server">
                <div id="divInformacionGeneral" class="GroupBody">
                    <div id="divInformacionGeneralHeader" class="GroupHeader">
                        <span><asp:Label runat="server" ID="lblTipoContrato3"></asp:Label></span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <input type="button" class="btnWizardGuardar" value="Terminar" id="btnGuardarPrevio"
                                onclick="javascript: confirmarCierre();" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"
                                OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                    <div id="divInformacionGeneralControles">
                        <!--espacio uc informacion contrato -->
                        <uc:ucResuContratoPSLUI ID="ucResuContratoPSL" runat="server" />
                    </div>
                </div>
                <div class="GroupBody">
                    <div class="GroupHeader">
                        <asp:Label ID="LabelCargos" runat="server" Text="SECCIONES ADICIONALES PARA CARGOS"></asp:Label>
                    </div>
                    <asp:UpdatePanel runat="server" ID="updLineasContratos" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <asp:GridView ID="grdLineasContrato" runat="server" AutoGenerateColumns="False" PageSize="10"
                                AllowPaging="True" CellPadding="4" GridLines="None" CssClass="Grid" Width="90%"
                                AllowSorting="True" OnRowDataBound="grdLineasContrato_RowDataBound" OnPageIndexChanging="grdLineasContrato_PageIndexChanging"
                                OnRowCommand="grdLineasContrato_RowCommand">
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
                                            <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                                ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>'
                                                Width="17px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="7px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                                ToolTip="Ver Detalles" CommandName="CMDDETALLES" CommandArgument='<%#Container.DataItemIndex%>'
                                                Width="17px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="7px" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                <div class="GroupBody" id="depositoReembolso" runat="server">
                    <div class="GroupHeader">
                        <asp:Label ID="lblDeposito" runat="server" Text="DEPÓSITO/REEMBOLSO"></asp:Label>
                    </div>
                    <table class="trAlinearDerecha" style="margin-bottom: 5px;">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 210px">
                                Importe Dep&oacute;sito
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical" colspan="4">
                                <asp:TextBox ID="txtImporteDeposito" runat="server" Width="175px" Enabled="False"
                                    CssClass="CampoNumero"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical">
                                Importe Reembolso
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtImporteRembolso" runat="server" Width="175px" CssClass="CampoNumero"></asp:TextBox>
                            </td>
                            <td class="tdCentradoVertical" style="width: 230px" align="right">
                                Persona Recibe Reembolso
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtPersonaRecibeRembolso" runat="server" Width="175px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divDatosCierre" class="GroupBody" runat="server">
                    <div id="divDatosCierreHeader" class="GroupHeader">
                        <span>DATOS DE CIERRE</span>
                    </div>
                    <div style="display: flex;">
                        <div class="dvIzquierda" style="width: 49%">
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td class="tdCentradoVertical" style="width: 175px;">
                                        <span>*</span>Fecha Cierre
                                    </td>
                                    <td style="width: 20px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical">
                                        <asp:TextBox ID="txtFechaCierre" runat="server" CssClass="CampoFecha" MaxLength="11"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        <span>*</span>Hora Cierre
                                    </td>
                                    <td style="width: 20px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical">
                                        <asp:TextBox ID="txtHoraCierre" runat="server" CssClass="CampoHora" MaxLength="7"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="dvDerecha" style="margin-bottom: 5px; width:49% ">
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td align="right" style="padding-top: 5px;">
                                        <span>*</span>Observaciones
                                    </td>
                                    <td style="width: 20px;">
                                        &nbsp;
                                    </td>
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
            </asp:View>
            <asp:View ID="vwCargosAdicionalesCierre" runat="server">
                <div id="divCargosAdicionales" class="GroupBody">
                    <div id="div4">
                        <uc:ucCargosAdicionalesCierrePSLUI ID="ucCargosAdicionalesCierrePSLUI" runat="server" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:HiddenField runat="server" ID="hdnContratoID" />
    <asp:HiddenField runat="server" ID="hdnEstatusID" />
    <asp:HiddenField runat="server" ID="hdnUnidadID" />
    <asp:HiddenField runat="server" ID="hdnEquipoID" />
    <asp:HiddenField runat="server" ID="hdnFUA" />
    <asp:HiddenField runat="server" ID="hdnUUA" />
    <asp:HiddenField runat="server" ID="hdnFechaContrato" />
    <asp:HiddenField runat="server" ID="hdnFechaRecepcion" />
    <asp:HiddenField runat="server" ID="hdnTipoContrato" Value="0" />
    <asp:Button ID="btnGuardar" runat="server" Text="Terminar" Style="display: none;"
        OnClick="btnGuardar_Click" />
</asp:Content>
