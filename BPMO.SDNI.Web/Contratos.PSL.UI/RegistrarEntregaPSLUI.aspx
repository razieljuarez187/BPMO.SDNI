<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" 
AutoEventWireup="true" CodeBehind="RegistrarEntregaPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.RegistrarEntregaPSLUI" %>

<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionExcavadoraPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionExcavadoraPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionEntregaRecepcionPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionEntregaRecepcionPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionRetroExcavadoraPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionRetroExcavadoraPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionMotoNiveladoraPSLUI"  Src="~/Contratos.PSL.UI/ucPuntosVerificacionMotoNiveladoraPSLUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionPistolaNeumaticaPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionPistolaNeumaticaPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionMiniCargadorPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionMiniCargadorPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionMartilloHidraulicoPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionMartilloHidraulicoPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionSubArrendadoPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionSubArrendadoPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionTorresLuzPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionTorresLuzPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionPlataformaTijerasPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionPlataformaTijerasPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionVibroCompactadorPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionVibroCompactadorPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionMontaCargaPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionMontaCargaPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionCompresoresPortatilesPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionCompresoresPortatilesPSLUI.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet"
        type="text/css" />
    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <style type="text/css">
        .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
        }
        .style2
        {
            width: 335px;
        }
        .RBL label
        {
            display: block;
        }
        .GroupBody
        {
            display: inline-table;
            float: right;
            margin-right: 38px;
            margin-top: 10px;
        }
        #divInformacionGeneralControles table
        {
            margin: 0;
        }
        #divInformacionGeneralControles .dvCentro { text-align: center; width: 65%;  }
        #divInformacionGeneralControles .dvCentro .trAlinearDerecha { margin: 0px 0px 0px auto; }
        #divInformacionGeneralControles .dvCentro .trAlinearDerecha .tdCentradoVertical input { margin: 0px 5px 0px 15px !important; display: inline !important; vertical-align: middle !important; }
 
    </style>
    <script type="text/javascript">
        //Validar campos requeridos
        function ValidatePage(Texto) {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                MensajeGrowUI("Falta información necesaria para " + Texto, "4");
                return;
            }
        }

        initChild = function () {
            var fechaSalida = $('#<%= txtFechaSalida.ClientID %>');
            fechaSalida.attr('readonly', true);


            var horaSalida = $('#<%= txtHoraSalida.ClientID %>');
            if (horaSalida.length > 0) {
                horaSalida.timepicker({
                    showPeriod: true,
                    showLeadingZero: true
                });
            }
            horaSalida.attr('readonly', true);
        };
        function DialogoDetalleImagenesSeccion() {
            $("#dvDialogImagenesSecccion").dialog({
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
            $("#dvDialogImagenesSecccion").parent().appendTo("form:first");
        }

        $(document).ready(initChild);
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="PaginaContenido">
        
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - REGISTRAR CHECK LIST</asp:Label>
        </div>

        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 31px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarListadoVerificacionPSLUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        
        <div id="divInformacionGeneral" class="GroupBody">

            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>ENTREGA DE UNIDAD</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" 
                        CssClass="btnWizardTerminar" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                </div>
            </div>
            <div id="divInformacionGeneralControles">
                
                <%--Información general--%>
                <fieldset>
                    <legend>Información General</legend>
                    <asp:HiddenField runat="server" ID="hdnContratoID" />
                    <asp:HiddenField runat="server" ID="hdnLineaContratoID" />
                    <asp:HiddenField runat="server" ID="hdnFechaContrato" />
                    <asp:HiddenField runat="server" ID="hdnHoraContrato" />
                    <asp:HiddenField runat="server" ID="hdnEstatusContratoID" />
                    <asp:HiddenField runat="server" ID="hdnTipoCheckList" Value="0" />
                    <asp:HiddenField runat="server" ID="hdnTipoListadoVerificacionPSL" Value="0" />
                    <asp:HiddenField runat="server" ID="hdnTipoContrato" Value="0" />
                    <asp:HiddenField runat="server" ID="hdnArea" />
                    <div class="dvIzquierda">
                        <table class="trAlinearDerecha">
                            <tr>
                                <td class="tdCentradoVertical">
                                    # CONTRATO
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNumeroContrato"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    Cliente
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNombreCliente" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    <label># LICENCIA</label>
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNumeroLicencia" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="dvDerecha">
                        <table class="trAlinearDerecha">
                            <tr>
                                <td class="tdCentradoVertical">
                                    <label>ECODE</label>
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:HiddenField runat="server" ID="hdnUnidadID" />
                                    <asp:HiddenField runat="server" ID="hdnEquipoID" />                                    
                                    <asp:HiddenField runat="server" ID="hdnCapacidadTanque"/>
                                    <asp:TextBox runat="server" ID="txtNumeroEconomico" Width="180px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    Serie Unidad
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNumeroSerie" Width="180px"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td class="tdCentradoVertical">
                                    <label># Placas Estatales</label>
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtPlacasEstatales" Width="65px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <fieldset>
                        <legend>EQUIPOS ALIADOS DE LA UNIDAD</legend>
                        <uc:ucEquiposAliadosUnidadUI runat="server" ID="ucucEquiposAliadosUnidadUI" />
                    </fieldset>
                </fieldset>

                <%--Información de entrega--%>
                <fieldset>

                    <legend>INFORMACI&Oacute;N DE ENTREGA</legend>
                    
                    <div class="dvIzquierda">
                        <table class="trAlinearDerecha">
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span>*</span>Fecha
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtFechaSalida" Width="75px" MaxLength="11" CssClass="CampoFecha"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span>*</span>Usuario Entrega
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNombreUsuarioEntrega" Width="240px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span>*</span>Combustible
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtCombustibleSalida" Width="80px" 
                                        MaxLength="9" CssClass="CampoNumeroEntero" AutoPostBack="True"
                                        ontextchanged="txtCombustibleSalida_TextChanged">
                                    </asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="dvDerecha">
                        <table class="trAlinearDerecha">
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span>*</span>Hora
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtHoraSalida" Width="62px" MaxLength="7" CssClass="CampoHora"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span>*</span>Horómetro
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtHorometro" MaxLength="9" AutoPostBack="True"
                                        CssClass="CampoNumeroEntero" ontextchanged="txtHorometro_OnTextChanged" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr >
                                <%--<td class="tdCentradoVertical>
                                    Tanque Combustible
                                </td>--%>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtTanqueCobustible" Width="80px" MaxLength="9" CssClass="CampoNumeroEntero" Visible="false">
                                    </asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnHorometroAnterior"/>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <%--Check list, puntos de verificación--%>

                    <asp:MultiView runat="server" ID="mvPuntosVerificacion">
                         <asp:View runat="server" ID="vwPuntosVerificacionCompresoresPortatiles">
                            <uc:ucPuntosVerificacionCompresoresPortatilesPSLUI runat="server" ID="ucPuntosVerificacionCompresoresPortatilesPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionMontaCarga">
                            <uc:ucPuntosVerificacionMontaCargaPSLUI runat="server" ID="ucPuntosVerificacionMontaCargaPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionMotoNiveladora">
                            <uc:ucPuntosVerificacionMotoNiveladoraPSLUI runat="server" ID="ucPuntosVerificacionMotoNiveladoraPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionMiniCargador">
                            <uc:ucPuntosVerificacionMiniCargadorPSLUI runat="server" ID="ucPuntosVerificacionMiniCargadorPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionMartilloHidraulico">
                            <uc:ucPuntosVerificacionMartilloHidraulicoPSLUI runat="server" ID="ucPuntosVerificacionMartilloHidraulicoPSLUI" />
                        </asp:View> 
                        <asp:View runat="server" ID="mvPuntosVerificacionEntrega">
                            <uc:ucPuntosVerificacionEntregaRecepcionPSLUI runat="server" ID="ucPuntosVerificacionEntregaRecepcionPSLUI" />
                        </asp:View>
                         <asp:View runat="server" ID="vwPuntosVerificacionExcavadora">
                            <uc:ucPuntosVerificacionExcavadoraPSLUI runat="server" ID="ucPuntosVerificacionExcavadoraPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionSubArrendado">
                            <uc:ucPuntosVerificacionSubArrendadoPSLUI runat="server" ID="ucPuntosVerificacionSubArrendadoPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionTorresLuz">
                            <uc:ucPuntosVerificacionTorresLuzPSLUI runat="server" ID="ucPuntosVerificacionTorresLuzPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionPlataformaTijeras">
                            <uc:ucPuntosVerificacionPlataformaTijerasPSLUI runat="server" ID="ucPuntosVerificacionPlataformaTijerasPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionRetroExcavadora">
                            <uc:ucPuntosVerificacionRetroExcavadoraPSLUI runat="server" ID="ucPuntosVerificacionRetroExcavadoraPSLUI" />
                        </asp:View> 
                       <asp:View runat="server" ID="vwPuntosVerificacionVibroCompactador">
                            <uc:ucPuntosVerificacionVibroCompactadorPSLUI runat="server" ID="ucPuntosVerificacionVibroCompactadorPSLUI" />
                        </asp:View> 
                         <asp:View runat="server" ID="vwPuntosVerificacionPistolaNeumatica">
                            <uc:ucPuntosVerificacionPistolaNeumaticaPSLUI runat="server" ID="ucPuntosVerificacionPistolaNeumaticaPSLUI" />
                        </asp:View> 
                       
                    </asp:MultiView>

                </fieldset>

                <%--Documentos adjuntos al Check List--%>
                <div id="divDatosFinales">
                    <fieldset id="fsDocumentosAdjuntos">
                        <legend>Documentos Adjuntos al Check List</legend>
                        <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                    </fieldset>
                </div>

                <div id="divObservaciones">
                    <fieldset id="fsObservaciones">
                        <legend>Observaciones</legend>
                        <table class="trAlinearDerecha">
                        <tr>
                            <td align="right" style="padding-top: 5px; vertical-align: middle;">
                                Observaciones
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Width="750px"
                                    Height="45px" Style="max-width: 750px; min-width: 750px; max-height: 45px; min-height: 45px;">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </div>

            </div>

        </div>

    </div>

</asp:Content>
