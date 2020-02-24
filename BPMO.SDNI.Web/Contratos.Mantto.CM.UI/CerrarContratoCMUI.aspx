<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CerrarContratoCMUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.Mantto.CM.UI.CerrarContratoCMUI" %>

<%@ Register Src="~/Contratos.Mantto.UI/ucContratoManttoUI.ascx" TagPrefix="uc" TagName="ucContrato" %>
<%@ Register Src="~/Contratos.Mantto.CM.UI/ucHerramientasCMUI.ascx" TagPrefix="uc"
    TagName="HerramientasCMUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #BarraHerramientas
        {
            width: 832px !important;
        }
    </style>
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet"
        type="text/css" />
    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
    <script type="text/javascript">
        initChild = function () {
             $("#divInformacionGeneralControles span:contains('*')").hide();
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
        };
            
        $(document).ready(initChild);
    </script>
    <script type="text/javascript">
        function confirmarCancelacion() {
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CERRAR CONTRATO DE MANTENIMIENTO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsulta" runat="server" NavigateUrl="~/Contratos.Mantto.CM.UI/ConsultarContratoCMUI.aspx">
                        CONSULTAR C.M.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.Mantto.CM.UI/RegistrarContratoCMUI.aspx">
                        REGISTRAR RENTA C.M. 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <uc:HerramientasCMUI ID="ucHerramientas" runat="server" />
        </div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>Contrato De Mantenimiento</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <input type="button" class="btnWizardGuardar" value="Cerrar" id="btnGuardarPrevio"
                        onclick="javascript: confirmarCancelacion();" runat="server" />
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btnWizardRegresar"
                        OnClick="btnRegresar_Click" />
                </div>
            </div>
            <div id="divInformacionGeneralControles">
                <uc:ucContrato ID="ucContratoUI" runat="server" />
            </div>
        </div>
        <div id="divDatosCierre" class="GroupBody">
            <div id="divDatosCierreHeader" class="GroupHeader">
                <span>DATOS DE CIERRE</span>
            </div>
            <div>
                <div class="dvIzquierda" style="margin-bottom: 5px;">
                    <table class="trAlinearDerecha">
                        <tr>
                            <td class="tdCentradoVertical">
                                <span>*</span>Fecha Cierre
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtFechaCierre" runat="server" CssClass="CampoFecha" MaxLength="11"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trMotivo" runat="server" visible="True">
                            <td class="tdCentradoVertical">
                                <span>*</span>Motivo
                            </td>
                            <td style="width: 5px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox ID="txtMotivo" runat="server" Width="250px" Style="max-width: 250px;
                                    min-width: 250px;" MaxLength="150"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="dvDerecha">
                    <table class="trAlinearDerecha">
                        <tr>
                            <td align="right" style="padding-top: 5px">
                                <span>
                                    <asp:Label runat="server" ID="lblObservaciones" Text=""></asp:Label></span>Observaciones
                            </td>
                            <td style="width: 5px;">
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
    </div>
    <asp:HiddenField runat="server" ID="hdnFUA" />
    <asp:HiddenField runat="server" ID="hdnUUA" />
    <asp:Button ID="btnGuardar" runat="server" Text="Cancelar" Style="display: none;"
        OnClick="btnGuardar_Click" />
</asp:Content>
