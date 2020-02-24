<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarRecepcionPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.RegistrarRecepcionPSLUI" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionEntregaRecepcionPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionEntregaRecepcionPSLUI.ascx"  %>
<%@ Register TagPrefix="uc" TagName="ucPuntosVerificacionExcavadoraPSLUI" Src="~/Contratos.PSL.UI/ucPuntosVerificacionExcavadoraPSLUI.ascx"  %>
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
    <link href="../Contenido/Estilos/EstiloRecepcionUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />

    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>

    <style type="text/css">
        .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
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
        
        .tdCentradoVertical input, .tdCentradoVertical img, .tdCentradoVertical span
        {
            vertical-align: auto;
        }
        
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

        function initChild() {
            setWizardStep($("#<%=hdnPaginaActual.ClientID %>").val());

            var fechaSalida = $('#<%= txtFechaRecepcion.ClientID %>');

            fechaSalida.attr('readonly', true);
            var horaSalida = $('#<%= txtHoraRecepcion.ClientID %>');
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

        $(document).ready(function () {
            initChild();
        });

        function obtieneFechaRecepcion() {
            var fechaRecibe = $('#<%= txtFechaRecepcion.ClientID %>');
            var date = new Date();
            fechaRecibe.datepicker({
                    maxDate: new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0),
                    yearRange: '-1:+1',
                    changeYear: true,
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: "dd/mm/yy",
                    buttonImage: '../Contenido/Imagenes/calendar.gif',
                    buttonImageOnly: true,
                    toolTipText: "Fecha Recepción",
                    showOn: 'button',
                    defaultDate: (fechaRecibe.val().length == 10) ? fechaRecibe.val() : new Date()
                });
                fechaRecibe.attr('readonly', true);
        }
        
    </script>

    <script language="javascript" type="text/javascript">
        function setWizardStep(stepNumber) {
            $(".WizardSteps ul li").removeClass("ActualStep");
            $(".WizardSteps ul li").removeClass("PastStep");

            if (stepNumber != "" && $("#wizard-step-" + stepNumber) != null) {
                $("#wizard-step-" + stepNumber).addClass("ActualStep");
            }
            //CAMBIE EL OPERADOR <= POR <
            if (stepNumber < 0 || stepNumber == "") {
                $(".BarraEstaticaWizard").hide();
                $(".GroupBody").css({ 'margin-right': '0px', 'margin-top': '30px' });
            }
            else {
                $(".BarraEstaticaWizard").show();
                $(".GroupBody").css({ 'margin-right': '38px', 'margin-top': '10px' });
            }
            // cambio > por >=, 0
            for (var i = stepNumber - 1; i >= 0; i--) {
                $("#wizard-step-" + i).addClass("PastStep");
            }

            $("#<%=lblTituloPaso.ClientID %>").text("RECEPCI&Oacute;N DE UNIDAD");
            if (stepNumber == 0) $("#<%=lblTituloPaso.ClientID %>").text("DATOS GENERALES");
            if (stepNumber == 1) {
                $("#<%=lblTituloPaso.ClientID %>").text("HORÓMETRO");
                obtieneFechaRecepcion();
            }
            if (stepNumber == 2) $("#<%=lblTituloPaso.ClientID %>").text("CUESTIONARIO");
            if (stepNumber == 3) $("#<%=lblTituloPaso.ClientID %>").text("DOCUMENTOS");

            if (stepNumber == "" || stepNumber < 0 || stepNumber == 4) {
                $(".WizardSteps ul li").unbind("click");
            }
            else {
                $(".WizardSteps ul li").bind('click', function () { jumpToWizardStep(this); });
                $("#wizard-step-4").unbind("click");
            }
        }

        function jumpToWizardStep(liControl) {
            var stepNumber = $(liControl).attr('id').replace("wizard-step-", "");
            $("#<%=hdnPaginaBrinco.ClientID %>").val(stepNumber);
            $("#<%=btnBrincarPagina.ClientID %>").click();
        }
    </script>

    <%--    SCXXXXX--%>
    <script type="text/javascript">
          function confirmarLavadoExitoso(origen) {
              var $div = $('<div title="Confirmación"></div>');
              var OSLavadoID = document.getElementById("MainContent_LabelOsId").innerHTML;
              $div.append('SE HA GENERADO '+OSLavadoID);           
              $("#dialog:ui-dialog").dialog("destroy");
              $($div).dialog({
                  closeOnEscape: true,
                  modal: true,
                  minWidth: 460,
                  open: function(event, ui) { $(".ui-dialog-titlebar-close", ui.dialog).hide();},
                  close: function () { $(this).dialog("destroy"); },
                  buttons: {
                      Aceptar: function () {
                          $(this).dialog("close");
                          __doPostBack("<%= btnImprimir.UniqueID %>", "");

                      },
                  }
              });
          }
    </script>

    <script type="text/javascript">
          function confirmarLavadoError(origen) {
              var $div = $('<div title="Confirmación"></div>');
              var CodError = document.getElementById("MainContent_LabelError").innerHTML;
              $div.append(CodError);
              $("#dialog:ui-dialog").dialog("destroy");
              $($div).dialog({
                  closeOnEscape: true,
                  modal: true,
                  minWidth: 460,
                  open: function(event, ui) { $(".ui-dialog-titlebar-close", ui.dialog).hide();},
                  close: function () { $(this).dialog("destroy"); },
                  buttons: {
                      Aceptar: function () {
                          $(this).dialog("close");
                          __doPostBack("<%= btnImprimir.UniqueID %>", "");

                      },
                  }
              });
          }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="PaginaContenido">

        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - REGISTRAR  CHECK LIST</asp:Label>
        </div>

        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 31px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarListadoVerificacionUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
            </ul>

            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="WizardSteps">
                    <ul>
                        <li id="wizard-step-0">1.GENERALES</li>
                        <li id="wizard-step-1">2.HORÓMETRO</li>
                        <li id="wizard-step-2">3.CUESTIONARIO</li>
                        <li id="wizard-step-3">4.DOCUMENTOS</li>
                    </ul>
                </div>
                <div class="Ayuda" style="top: 0;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>

        </div>

        <%-- SCXXXXX--%>
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" style="display: none;" />
        <asp:Label ID="LabelError" runat="server" Text="" style="display: none;"></asp:Label>
        <asp:Label ID="LabelOsId" runat="server" Text="" style="display: none;"></asp:Label>

        <div id="dvDatosRegistro" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="RECEPCI&Oacute;N DE UNIDAD"></asp:Label>
                <div class="GroupHeaderOpciones Ancho3Opciones">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btnWizardContinuar"
                        ValidationGroup="Obligatorios" onclick="btnContinuar_Click" />
                    <asp:Button ID="btnTerminar" runat="server" Text="Terminar" CssClass="btnWizardTerminar"
                        ValidationGroup="Obligatorios" onclick="btnTerminar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btnWizardAtras"
                        ValidationGroup="Obligatorios" onclick="btnAnterior_Click" />
                </div>
            </div>

            <div id="divInformacionGeneralControles">
                <asp:MultiView ID="mvCU077" runat="server" ActiveViewIndex="0">
                    
                    <%--Información general--%>
                    <asp:View ID="vwPagina0" runat="server">
                        <fieldset>
                            <legend>Información General</legend>
                            <asp:HiddenField runat="server" ID="hdnContratoID" />
                            <asp:HiddenField runat="server" ID="hdnLineaContratoID" />
                            <asp:HiddenField runat="server" ID="hdnFechaContrato" />
                            <asp:HiddenField runat="server" ID="hdnHoraContrato" />
                            <asp:HiddenField runat="server" ID="hdnTipoContrato" />
                            <asp:HiddenField runat="server" ID="hdnEstatusContratoID" />
                            <asp:HiddenField runat="server" ID="hdnTipoCheckList" Value="0" />
                            <asp:HiddenField runat="server" ID="hdnTipoListadoVerificacionPSL" Value="0" />
                            <asp:HiddenField runat="server" ID="hdnArea" />
                            <div class="dvIzquierda">
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            # CONTRATO
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtNumeroContrato"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            Cliente
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtNombreCliente" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            # lICENCIA
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtNumeroLicencia" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="dvDerecha">
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            Ecode
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
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
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtNumeroSerie" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            # Placas Estatales
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtPlacasEstatales" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <fieldset style="text-align: center">
                                <legend>EQUIPOS ALIADOS DE LA UNIDAD</legend>
                                <uc:ucEquiposAliadosUnidadUI runat="server" ID="ucucEquiposAliadosUnidadUI" />
                            </fieldset>
                        </fieldset>
                    </asp:View>

                    <%--Horómetro--%>
                    <asp:View ID="vwPagina1" runat="server">
                        <asp:HiddenField runat="server" ID="hdnCheckListEntregaID" />
                        <div class="divContenedor">
                            <fieldset class="ColumnaIzquierda">
                                <legend>Información DE ENTREGA</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            USUARIO ENTREG&Oacute;
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtUsuarioEntrego" Width="240px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            FECHA
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtFechaEntrega" Width="75px" MaxLength="11" CssClass="CampoFecha"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            HORA
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtHoraEntrega" Width="62px" MaxLength="7" CssClass="CampoHora"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            HORÓMETRO
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtHorometroEntrega" MaxLength="9" CssClass="CampoNumeroEntero"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            COMBUSTIBLE
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtCombustibleEntrega" Width="80px" MaxLength="9"
                                                CssClass="CampoNumeroEntero" ></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset class="ColumnaDerecha">
                                <legend>Información DE RECEPCI&Oacute;n</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            USUARIO RECIBE
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtUsuarioRecibe" Width="240px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            <span>*</span>FECHA
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtFechaRecepcion" Width="75px" MaxLength="11" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            <span>*</span>HORA
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtHoraRecepcion" Width="62px" MaxLength="7" CssClass="CampoHora"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            <span>*</span>HORÓMETRO
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtHorometro" MaxLength="9" CssClass="CampoNumeroEntero"
                                                AutoPostBack="True" ontextchanged="txtHorometro_OnTextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            <span>*</span>COMBUSTIBLE
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td class="tdCentradoVertical" style="float: left">
                                            <asp:TextBox runat="server" ID="txtCombustible" Width="80px" MaxLength="9" CssClass="CampoNumeroEntero"
                                                AutoPostBack="True" ontextchanged="txtCombustible_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </asp:View>

                    <asp:View ID="vwPagina2" runat="server">
                        <asp:MultiView ID="mvPuntosVerificacion" runat="server">
                            <asp:View ID="vwCompresoresPortatiles" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionCompresoresPortatilesPSLUI runat="server" ID="ucucPuntosVerificacionCompresoresPortatilesE" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionCompresoresPortatilesPSLUI runat="server" ID="ucucPuntosVerificacionCompresoresPortatilesR" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwMontaCarga" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMontaCargaPSLUI runat="server" ID="ucucPuntosVerificacionMontaCargaEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMontaCargaPSLUI runat="server" ID="ucucPuntosVerificacionMontaCargaRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwMotoNiveladora" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMotoNiveladoraPSLUI runat="server" ID="ucucPuntosVerificacionMotoNiveladoraEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMotoNiveladoraPSLUI runat="server" ID="ucucPuntosVerificacionMotoNiveladoraRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwMiniCargador" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMiniCargadorPSLUI runat="server" ID="ucucPuntosVerificacionMiniCargadorEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMiniCargadorPSLUI runat="server" ID="ucucPuntosVerificacionMiniCargadorRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwMartilloHidraulico" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMartilloHidraulicoPSLUI runat="server" ID="ucucPuntosVerificacionMartilloHidraulicoEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionMartilloHidraulicoPSLUI runat="server" ID="ucucPuntosVerificacionMartilloHidraulicoRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwEntregaRecepcion" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionEntregaRecepcionPSLUI runat="server" ID="ucucPuntosVerificacionEntregaRecepcionEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionEntregaRecepcionPSLUI runat="server" ID="ucucPuntosVerificacionEntregaRecepcionRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwExcavadora" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionExcavadoraPSLUI runat="server" ID="ucucPuntosVerificacionExcavadoraEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionExcavadoraPSLUI runat="server" ID="ucucPuntosVerificacionExcavadoraRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwSubArrendado" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionSubArrendadoPSLUI runat="server" ID="ucucPuntosVerificacionSubArrendadoEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionSubArrendadoPSLUI runat="server" ID="ucucPuntosVerificacionSubArrendadoRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwTorresLuz" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionTorresLuzPSLUI runat="server" ID="ucucPuntosVerificacionTorresLuzEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionTorresLuzPSLUI runat="server" ID="ucucPuntosVerificacionTorresLuzRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwPlataformaTijeras" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionPlataformaTijerasPSLUI runat="server" ID="ucPuntosVerificacionPlataformaTijerasE" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionPlataformaTijerasPSLUI runat="server" ID="ucPuntosVerificacionPlataformaTijerasR" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwRetroExcavadora" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionRetroExcavadoraPSLUI runat="server" ID="ucucPuntosVerificacionRetroExcavadoraEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionRetroExcavadoraPSLUI runat="server" ID="ucucPuntosVerificacionRetroExcavadoraRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                       
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwVibroCompactador" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionVibroCompactadorPSLUI runat="server" ID="ucucPuntosVerificacionVibroCompactadorEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionVibroCompactadorPSLUI runat="server" ID="ucucPuntosVerificacionVibroCompactadorRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                            <asp:View ID="vwPistolaNeumatica" runat="server">
                                <%--Cuestionario--%>
                                <div class="divContenedor">
                                    <fieldset class="ColumnaIzquierda">
                                        <legend>Información DE ENTREGA</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionPistolaNeumaticaPSLUI runat="server" ID="ucucPuntosVerificacionPistolaNeumaticaEntrega" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset class="ColumnaDerecha">
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <table class="trAlinearIzquierda">
                                            <tr>
                                                <td class="tdCentradoVertical">
                                                    <uc:ucPuntosVerificacionPistolaNeumaticaPSLUI runat="server" ID="ucucPuntosVerificacionPistolaNeumaticaRecepcion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </asp:View>
                        </asp:MultiView>

                        <%--Observaciones--%>
                        <div class="divContenedor">
                            <fieldset class="ColumnaIzquierda">
                                <legend>Observaciones de ENTREGA</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtObservacionEntrega" TextMode="MultiLine" Width="250px"
                                                Height="90px" Style="max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;"> 
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset class="ColumnaDerecha">
                                <legend>Observaciones de RECEPCIÓN</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtObservacionRecepcion" TextMode="MultiLine" Width="250px"
                                                Height="90px" 
                                                Style="max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>

                    </asp:View>
                    

                    <%--Documentos--%>
                    <asp:View ID="vwPagina3" runat="server">
                        <fieldset>
                            <legend>Información DE ENTREGA</legend>
                            <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                        </fieldset>
                        <fieldset>
                            <legend>Información DE RECEPCI&Oacute;N</legend>
                            <uc:ucCatalogoDocumentosUI ID="UcCatalogoDocumentosUI1" runat="server" />
                        </fieldset>
                    </asp:View>

                </asp:MultiView>
            </div>

        </div>

    </div>

    <asp:HiddenField ID="hdnPaginaActual" runat="server" />
    <asp:HiddenField ID="hdnPaginaBrinco" runat="server" />
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnKilometrajeDiario" runat="server" />
    <asp:Button ID="btnBrincarPagina" runat="server" Text="" Style="display: none;" OnClick="btnBrincarPagina_Click" />
    <div class="ContenedorMensajes">
        <span class="Requeridos RequeridosPSL"></span>
        <br />
        <span class="FormatoIncorrecto FormatoIncorrectoPSL"></span>
    </div>

</asp:Content>
