<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarRecepcionUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.RegistrarRecepcionUI" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
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
            if (stepNumber == 1) $("#<%=lblTituloPaso.ClientID %>").text("kILOMETRAJE");
            if (stepNumber == 2) $("#<%=lblTituloPaso.ClientID %>").text("CUESTIONARIO");
            if (stepNumber == 3) $("#<%=lblTituloPaso.ClientID %>").text("LLANTAS");
            if (stepNumber == 4) $("#<%=lblTituloPaso.ClientID %>").text("SECCIONES");
            if (stepNumber == 5) $("#<%=lblTituloPaso.ClientID %>").text("DOCUMENTOS");

            if (stepNumber == "" || stepNumber < 0 || stepNumber == 6) {
                $(".WizardSteps ul li").unbind("click");
            }
            else {
                $(".WizardSteps ul li").bind('click', function () { jumpToWizardStep(this); });
                $("#wizard-step-6").unbind("click");
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
                        <li id="wizard-step-1">2.kILOMETRAJE</li>
                        <li id="wizard-step-2">3.CUESTIONARIO</li>
                        <li id="wizard-step-3">4.LLANTAS</li>
                        <li id="wizard-step-4">5.SECCIONES</li>
                        <li id="wizard-step-5">6.DOCUMENTOS</li>
                    </ul>
                </div>
                <div class="Ayuda" style="top: 0;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
       <%-- SCXXXXX--%>
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" style="display: none;" onclick="btnImprimir_Click"  />
        <asp:Label ID="LabelError" runat="server" Text="" style="display: none;"></asp:Label>
        <asp:Label ID="LabelOsId" runat="server" Text="" style="display: none;"></asp:Label>


        <div id="dvDatosRegistro" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="RECEPCI&Oacute;N DE UNIDAD"></asp:Label>
                <div class="GroupHeaderOpciones Ancho3Opciones">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btnWizardContinuar"
                        OnClick="btnContinuar_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnTerminar" runat="server" Text="Terminar" CssClass="btnWizardTerminar"
                        OnClick="btnTerminar_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"
                        OnClick="btnCancelar_Click" />
                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btnWizardAtras"
                        OnClick="btnAnterior_Click" ValidationGroup="Obligatorios" />
                </div>
            </div>
            <div id="dvNotaCancelar" runat="server" class="Nota">
                Nota: El kilometraje de recepción de la unidad es igual al kilometraje de entrega, Al guardar el Check List se procederá a cancelar el contrato.
            </div>
            <div id="divInformacionGeneralControles">                
                <asp:MultiView ID="mvCU077" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPagina0" runat="server">
                        <fieldset>
                            <legend>Información General</legend>
                            <asp:HiddenField runat="server" ID="hdnContratoID" />
                            <asp:HiddenField runat="server" ID="hdnLineaContratoID" />
                            <asp:HiddenField runat="server" ID="hdnFechaContrato" />
                            <asp:HiddenField runat="server" ID="hdnHoraContrato" />
                            <asp:HiddenField runat="server" ID="hdnEstatusContratoID" />
                            <asp:HiddenField runat="server" ID="hdnTipoCheckList" Value="0" />
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
                                            Operador
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtNombreOperador" Width="200px"></asp:TextBox>
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
                                            # Econ&oacute;mico
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
                                            # Serie
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtNumeroSerie" Width="180px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCentradoVertical">
                                            # Placas Federales
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtPlacasFederales" Width="65px"></asp:TextBox>
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
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtUsuarioEntrego" Width="240px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        FECHA
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtFechaEntrega" Width="75px" MaxLength="11" CssClass="CampoFecha"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        HORA
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtHoraEntrega" Width="62px" MaxLength="7" CssClass="CampoHora"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        KM TABLERO
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtKilometrajeEntrega" MaxLength="9" CssClass="CampoNumeroEntero"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        HRS REFRIGERACI&Oacute;N
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtHorasRefrigeracionEntrega" MaxLength="9" CssClass="txtMoneda"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        COMBUSTIBLE
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtCombustibleEntrega" Width="80px" MaxLength="9"
                                            CssClass="CampoNumeroEntero"></asp:TextBox>
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
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtUsuarioRecibe" Width="240px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        <span>*</span>FECHA
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtFechaRecepcion" Width="75px" MaxLength="11" CssClass="CampoFecha"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        <span>*</span>HORA
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtHoraRecepcion" Width="62px" MaxLength="7" CssClass="CampoHora"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        <span>*</span>KM TABLERO
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtKilometraje" MaxLength="9" CssClass="CampoNumeroEntero"
                                            AutoPostBack="True" OnTextChanged="txtKilometraje_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        <span>*</span>HRS REFRIGERACI&Oacute;N
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtHorasRefrigeracion" MaxLength="9" CssClass="CampoNumeroEntero" 
                                        OnTextChanged="txtHorasRefrigeracion_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        <span>*</span>COMBUSTIBLE
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical" style="float: left">
                                        <asp:TextBox runat="server" ID="txtCombustible" Width="80px" MaxLength="9" 
                                            CssClass="CampoNumeroEntero" AutoPostBack="True"
                                            ontextchanged="txtCombustible_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        </div>
                    </asp:View>
                    <asp:View ID="vwPagina2" runat="server">
                        <div class="divContenedor">
                        <fieldset class="ColumnaIzquierda">
                            <legend>Información DE ENTREGA</legend>
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Interior Limpio
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtInteriorLimpioEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Vestiduras Limpias
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtVestidurasLimpiasEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Llave Original
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtLLaveOriginalEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Estereo y Bocinas
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtEstereoBocinasEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Extinguidor
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtExtinguidorEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Tres Reflejantes
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtTresReflejantesEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Limpieza Interior Caja
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtLimpiezaInteriorCajaEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Golpes En General
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGolpesGeneralEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Documentaci&oacute;n Completa
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtDocumentacionCompletaEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Tapetes
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtTapetesEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Encendedor
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtEncendedorEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Alarmas De Reversa
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtAlarmasReversaEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Gato y Llave
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGatoEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Espejos Completos
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtEspejosCompletosEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        GPS
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGPSEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Bater&iacute;as Correctas
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtBateriasCorrectasEntrega" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="True" />
                                            <asp:ListItem Text="NO" Value="False" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Observaciones de ENTREGA</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td align="right" style="padding-top: 5px;">
                                            Observaci&oacute;n Documentaci&oacute;n
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtObservacionDocumentacionEntrega" TextMode="MultiLine"
                                                Width="250px" Height="90px" Style="max-width: 250px;
                                                min-width: 250px; max-height: 90px; min-height: 90px;"> 
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding-top: 5px;">
                                            <asp:GridView ID="grdImagenesDialog" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CellPadding="4" CssClass="Grid" GridLines="None" 
                                                PageSize="15" Style="margin: 10px auto;" Width="95%">
                                                <Columns>
                                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="NOMBRE">
                                                    <ItemStyle VerticalAlign="Middle" Width="100%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Extensión
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExtension" runat="server" 
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"TipoArchivo.Extension") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
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
                                                    <b>No se han agregado archivos.</b>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            Observaci&oacute;n Bater&iacute;as
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtObservacionesBateriasEntrega" TextMode="MultiLine"
                                                Width="250px" Height="90px" Style="max-width: 250px;
                                                min-width: 250px; max-height: 90px; min-height: 90px;">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </fieldset>
                        <fieldset class="ColumnaDerecha">
                            <legend>Información DE RECEPCI&Oacute;N</legend>
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Interior Limpio
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtInteriorLimpio" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Vestiduras Limpias
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtVestidurasLimpias" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Llave Original
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtLLaveOriginal" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Estereo y Bocinas
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtEstereoBocinas" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Extinguidor
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtExtinguidor" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Tres Reflejantes
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtTresReflejantes" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Limpieza Interior Caja
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtLimpiezaInteriorCaja" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Golpes En General
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGolpesGeneral" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Documentaci&oacute;n Completa
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtDocumentacionCompleta" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Tapetes
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtTapetes" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Encendedor
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtEncendedor" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Alarmas De Reversa
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtAlarmasReversa" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Gato y Llave
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGato" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Espejos Completos
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtEspejosCompletos" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        GPS
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGPS" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Bater&iacute;as Correctas
                                    </td>
                                    <td style="width: 20px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtBateriasCorrectas" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Observaciones de RECEPCI&Oacute;N</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td align="right" style="padding-top: 5px;">
                                            Observaci&oacute;n Documentaci&oacute;n
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtObservacionesDocumentacion" TextMode="MultiLine"
                                                Width="250px" Height="90px" Style="max-width: 250px; min-width: 250px; max-height: 90px;
                                                min-height: 90px;">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding-top: 5px;">
                                            Observaci&oacute;n Bater&iacute;as
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:TextBox runat="server" ID="txtObservacionesBaterias" TextMode="MultiLine" Width="250px"
                                                Height="90px" Style="max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </fieldset>
                        </div>
                    </asp:View>
                    <asp:View ID="vwPagina3" runat="server">
                        <div class="divContenedor">
                        <fieldset class="ColumnaIzquierda">
                            <legend>Información DE ENTREGA</legend>
                            <asp:GridView ID="grdLlantasEntrega" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                EnableSortingAndPagingCallbacks="True" CssClass="Grid" OnRowDataBound="grdLlantasEntrega_RowDataBound"
                                Enabled="False">
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <HeaderTemplate>
                                            #</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLlantaID" Text='<%# DataBinder.Eval(Container.DataItem,"Llanta.LlantaID") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Posici&oacute;n</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPosicionLlanta" Text='<%# DataBinder.Eval(Container.DataItem,"Llanta.Posicion") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            C&oacute;digo</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCodigoLlanta" Text='<%# DataBinder.Eval(Container.DataItem,"Llanta.Codigo") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Es Correcta</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:RadioButtonList runat="server" ID="rbtEstadoLlanta" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="SI" Value="True" />
                                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="GridRow" />
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <PagerStyle CssClass="GridPager" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                            </asp:GridView>
                            <fieldset>
                                <legend>Refacci&oacute;n</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td align="right" style="padding-top: 5px; vertical-align: middle">
                                            C&oacute;digo
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:HiddenField runat="server" ID="hdnRefaccionID" />
                                            <asp:TextBox runat="server" ID="txtRefaccionEntrega"></asp:TextBox>
                                        </td>
                                        <td align="right" style="padding-top: 5px; vertical-align: middle">
                                            Es Correcta
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical" style="vertical-align: middle;">
                                            <asp:RadioButtonList runat="server" ID="rbtRefaccionEntrega" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="SI" Value="True" />
                                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td align="right" style="padding-top: 5px; width: 100px;">
                                        Observaci&oacute;n Llanta
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:TextBox runat="server" ID="txtObservacionesLlantaEntrega" TextMode="MultiLine"
                                            Width="270px" Height="90px" Style="max-width: 270px;
                                            min-width: 270px; max-height: 90px; min-height: 90px;">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="ColumnaDerecha">
                            <legend>Información DE RECEPCI&Oacute;n</legend>
                            <asp:GridView ID="grdLlantas" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                EnableSortingAndPagingCallbacks="True" CssClass="Grid">
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <HeaderTemplate>
                                            #</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblLlantaID" Text='<%# DataBinder.Eval(Container.DataItem,"LlantaID") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Posici&oacute;n</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPosicionLlanta" Text='<%# DataBinder.Eval(Container.DataItem,"Posicion") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            C&oacute;digo</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCodigoLlanta" Text='<%# DataBinder.Eval(Container.DataItem,"Codigo") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Es Correcta</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:RadioButtonList runat="server" ID="rbtEstadoLlanta" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="SI" Value="true" />
                                                <asp:ListItem Text="NO" Value="false" Selected="True" />
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="GridRow" />
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <PagerStyle CssClass="GridPager" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                            </asp:GridView>
                            <fieldset>
                                <legend>Refacci&oacute;n</legend>
                                <table class="trAlinearDerecha">
                                    <tr>
                                        <td align="right" style="padding-top: 5px; vertical-align: middle">
                                            C&oacute;digo
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical">
                                            <asp:HiddenField runat="server" ID="hdnRecepcionRefaccionID" />
                                            <asp:TextBox runat="server" ID="txtRefaccion"></asp:TextBox>
                                        </td>
                                        <td align="right" style="padding-top: 5px; vertical-align: middle">
                                            Es Correcta
                                        </td>
                                        <td style="width: 5px;">&nbsp;</td>
                                        <td class="tdCentradoVertical" style="vertical-align: middle;">
                                            <asp:RadioButtonList runat="server" ID="rbtRefaccionCorrecta" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="SI" Value="true" />
                                                <asp:ListItem Text="NO" Value="false" Selected="True" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td align="right" style="padding-top: 5px; width: 100px;">
                                        Observaci&oacute;n Llanta
                                    </td>
                                    <td style="width: 5px;">&nbsp;</td>
                                    <td class="tdCentradoVertical">
                                        <asp:TextBox runat="server" ID="txtObservacionesLlanta" TextMode="MultiLine" Width="270px"
                                            Height="90px" Style="max-width: 270px; min-width: 270px; max-height: 90px; min-height: 90px;">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        </div>
                    </asp:View>
                    <asp:View ID="vwPagina4" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Unidad</legend>
                                    <div id="dvCamioncitos" style="text-align: center; line-height: 100px;
                                        width: 900px; height: 200px;">
                                        <img src="../Contenido/Imagenes/camion-24-PV2.png" style="float: left; height: 170px;
                                            width: 250px;" alt="Lateral Pasajero" />
                                        <img src="../Contenido/Imagenes/camion-24-PV4.png" style="float: left; height: 170px;
                                            width: 165px; margin-left: 20px;" alt="Trasera" />
                                        <img src="../Contenido/Imagenes/camion-24-PV3.png" style="float: left; height: 170px;
                                            width: 165px; margin-left: 20px;" alt="Frontal" />
                                        <img src="../Contenido/Imagenes/camion-24-PV.png" style="float: right; height: 170px;
                                            width: 250px;" alt="Lateral Conductor" />
                                    </div>
                                    <fieldset>
                                        <legend>Información DE ENTREGA</legend>
                                        <fieldset>
                                            <legend>Observaciones Secci&oacute;n de la Unidad</legend>
                                            <asp:GridView ID="grdObservacionesSeccionesEntrega" runat="server" AutoGenerateColumns="False"
                                                HorizontalAlign="Center" Aliggn="Center" AllowPaging="True" PageSize="10" CellPadding="4"
                                                GridLines="None" CssClass="Grid" Width="95%" Style="margin: 10px auto;" 
                                                OnRowCommand="grdObservacionesSeccionesEntrega_RowCommand" 
                                                onpageindexchanging="grdObservacionesSeccionesEntrega_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Secci&oacute;n" DataField="Numero" SortExpression="NUMERO">
                                                        <ItemStyle Width="50px" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Observaci&oacute;n" DataField="Observacion" SortExpression="OBSERVACION">
                                                        <ItemStyle Width="100%" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png"
                                                                ToolTip="Ver Detalles" CommandName="detalle" CommandArgument='<%#Container.DataItemIndex%>'
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
                                                    <b>No se han agregado observaciones a la unidad.</b>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </fieldset>
                                        <fieldset>
                                            <legend>Imagenes</legend>
                                            <asp:GridView ID="grdImagenesSecciones" runat="server" AutoGenerateColumns="False"
                                                AllowPaging="True" PageSize="15" CellPadding="4" GridLines="None" CssClass="Grid"
                                                Width="95%" Style="margin: 10px auto;" 
                                                onrowdatabound="grdImagenesSecciones_RowDataBound" 
                                                onpageindexchanging="grdImagenesSecciones_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="NOMBRE">
                                                        <ItemStyle Width="100%" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Extensión</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblExtension" Text='<%# DataBinder.Eval(Container.DataItem,"TipoArchivo.Extension") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="ibtDescargar" CommandName="descargar" ImageUrl="~/Contenido/Imagenes/DESCARGAR-ICO.png"
                                                                ToolTip="Descargar" CommandArgument='<%#Container.DataItemIndex%>' Visible='<%# (int?)DataBinder.Eval(Container,"DataItem.Id") != null ? true:false %>' />
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
                                                    <b>No se han agregado archivos.</b>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </fieldset>
                                    </fieldset>
                                    <fieldset>
                                        <legend>Información DE RECEPCI&Oacute;N</legend>
                                        <fieldset>
                                            <legend>Observaci&oacute;n</legend>
                                            <table class="trAlinearDerecha" style="width: 98%;">
                                                <tr>
                                                    <td align="right" style="padding-top: 5px; width: 75px;">
                                                        Secci&oacute;n
                                                    </td>
                                                    <td style="width: 5px;">&nbsp;</td>
                                                    <td align="right" style="padding-top: 5px;">
                                                        <asp:DropDownList ID="ddlSeccionesunidad" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 10px;">&nbsp;</td>
                                                    <td align="right" style="padding-top: 5px;">
                                                        Observaci&oacute;N
                                                    </td>
                                                    <td style="width: 5px;">&nbsp;</td>
                                                    <td class="tdCentradoVertical">
                                                        <asp:TextBox runat="server" ID="txtObservacionesSeccion" TextMode="MultiLine" Width="350px"
                                                            Height="85px" Style="max-width: 350px; min-width: 350px; max-height: 85px; min-height: 85px;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                                            CssClass="ColorValidator" ControlToValidate="txtObservacionesSeccion" ValidationGroup="addSeccion"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td align="right" style="padding-top: 10px;">
                                                        <asp:Button ID="btnAgregarSeccion" runat="server" Text="Agregar Secci&oacute;n" ToolTip="Agregar observaciones de la sección"
                                                            CssClass="btnAgregarATabla" ValidationGroup="addSeccion" OnClick="btnAgregarSeccion_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:UpdatePanel ID="updPnimagenes" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table style="width: 98%;">
                                                        <tr>
                                                            <td align="right" style="padding-top: 5px">
                                                                Archivo
                                                            </td>
                                                            <td style="width: 5px;">&nbsp;</td>
                                                            <td>
                                                                <asp:FileUpload ID="uplArchivoImagen" runat="server" Width="580px" />
                                                                <asp:RequiredFieldValidator ID="rfvUplArchivo" runat="server" ErrorMessage="*" CssClass="ColorValidator"
                                                                    ControlToValidate="uplArchivoImagen" ValidationGroup="FileUploadImagen"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td style="width: 5px;">&nbsp;</td>
                                                            <td class="tdCentradoVertical">
                                                                <asp:Button ID="btnAgregarImagen" runat="server" Text="Agregar Imag&eacute;n" CssClass="btnAgregarATabla"
                                                                    ToolTip="Agregar imagén a la sección" ValidationGroup="FileUploadImagen" OnClick="btnAgregarImagen_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnAgregarImagen" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grdArchivosImagen" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                                            CellPadding="4" GridLines="None" CssClass="Grid" Width="95%" Style="margin: 10px auto;"
                                                            OnPageIndexChanging="grdArchivos_PageIndexChanging" OnRowCommand="grdArchivos_RowCommand"
                                                            OnRowDataBound="grdArchivos_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="NOMBRE">
                                                                    <ItemStyle Width="100%" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        Extensión</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblExtension" Text='<%# DataBinder.Eval(Container.DataItem,"TipoArchivo.Extension") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="ibtEliminar" CommandName="eliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                                                            ToolTip="Eliminar" CommandArgument='<%#Container.DataItemIndex%>' OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="ibtDescargar" CommandName="descargar" ImageUrl="~/Contenido/Imagenes/DESCARGAR-ICO.png"
                                                                            ToolTip="Descargar" CommandArgument='<%#Container.DataItemIndex%>' Visible='<%# (int?)DataBinder.Eval(Container,"DataItem.Id") != null ? true:false %>' />
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
                                                                <b>No se han agregado archivos.</b>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset>
                                            <legend>Observaciones Secci&oacute;n de la Unidad</legend>
                                            <asp:GridView ID="grdObservacionesSecciones" runat="server" AutoGenerateColumns="False"
                                                AllowPaging="True" PageSize="5" CellPadding="4" GridLines="None" CssClass="Grid"
                                                Width="95%" Style="margin: 10px auto;" 
                                                OnRowCommand="grdObservacionesSecciones_RowCommand" 
                                                onpageindexchanging="grdObservacionesSecciones_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Secci&oacute;n" DataField="Numero" SortExpression="NUMERO">
                                                        <ItemStyle Width="50px" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Observaci&oacute;n" DataField="Observacion" SortExpression="OBSERVACION">
                                                        <ItemStyle Width="100%" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="ibtEliminar" CommandName="eliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                                                ToolTip="Eliminar" CommandArgument='<%#Container.DataItemIndex%>' OnClientClick="return confirm('¿Está seguro que desea eliminar esta observaci&oacute;n?');" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png"
                                                                ToolTip="Ver Detalles" CommandName="detalle" CommandArgument='<%#Container.DataItemIndex%>'
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
                                                    <b>No se han agregado observaciones a la unidad.</b>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </fieldset>
                                    </fieldset>
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnContinuar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="vwPagina5" runat="server">
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
    <div id="dvDialogImagenesSecccion" style="display: none;" title="IMAGENES DE LA SECCIÓN">
    </div>
    <asp:HiddenField ID="hdnPaginaActual" runat="server" />
    <asp:HiddenField ID="hdnPaginaBrinco" runat="server" />
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnKilometrajeDiario" runat="server" />
    <asp:Button ID="btnBrincarPagina" runat="server" Text="" OnClick="btnBrincarPagina_Click"
        Style="display: none;" />
    <div class="ContenedorMensajes">
        <span class="Requeridos RequeridosFSL"></span>
        <br />
        <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
    </div>
</asp:Content>
