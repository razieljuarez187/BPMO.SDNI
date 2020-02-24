<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarActaNacimientoUI.aspx.cs" Inherits="BPMO.SDNI.Equipos.UI.RegistrarActaNacimientoUI" %>
<%@ Register Src="~/Equipos.UI/ucDatosGeneralesUI.ascx" TagName="ucDatosGeneralesUI" TagPrefix="ucPagina1" %>
<%@ Register Src="~/Equipos.UI/ucDatosTecnicosUI.ascx" TagName="ucDatosTecnicosUI" TagPrefix="ucPagina2" %>
<%@ Register Src="~/Equipos.UI/ucNumerosSerieUI.ascx" TagName="ucNumerosSerieUI" TagPrefix="ucPagina3" %>
<%@ Register Src="~/Equipos.UI/ucAsignacionLlantasUI.ascx" TagName="ucAsignacionLlantasUI" TagPrefix="ucPagina4" %>
<%@ Register Src="~/Equipos.UI/ucAsignacionEquiposAliadosUI.ascx" TagName="ucAsignacionEquiposAliadosUI" TagPrefix="ucPagina5" %>
<%@ Register Src="~/Tramites.UI/ucTramitesActivosUI.ascx" TagName="ucTramitesActivosUI" TagPrefix="ucPagina6" %>
<%@ Register Src="~/Equipos.UI/ucResumenActaNacimientoUI.ascx" TagName="ucResumenActaNacimientoUI" TagPrefix="ucPagina7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../Contenido/Estilos/EstiloActaNacimiento.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Contenido/Scripts/ObtenerFormatoImporte.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            initChild();
           
        });
        function initChild() {
            
            setWizardStep($("#<%=hdnPaginaActual.ClientID %>").val());

            if (!$("#<%=txtProximoServicio.ClientID %>").is(':disabled')) {
                $("#<%=txtProximoServicio.ClientID %>").datepicker({
                    showOn: "button",
                    buttonImage: "../Contenido/Imagenes/calendar.gif",
                    buttonImageOnly: true
                });
            }
            $("#<%=txtProximoServicio.ClientID %>").attr('readonly', true);
            $("#<%=btnContinuar.ClientID %>").click(function() {
               return ValidarCampos();
           });

           $('.CampoFecha').datepicker({ 
               yearRange: '-100:+10',
               changeYear: true,
               changeMonth: true,
               dateFormat: "dd/mm/yy",
               buttonImage: '../Contenido/Imagenes/calendar.gif',
               buttonImageOnly: true,
               toolTipText: "Fecha de compra",
               showOn: 'button',
              
           });
           $('.CampoFecha').attr('readonly', true);

          

           $('.txtMontoArrendamiento').on("change", function () {

               if ($(".txtMontoArrendamiento").val().length >= 1) {
                   $(".ddlMonedas").attr("disabled", false);
                   $(".ddlMonedas").css("background", "#FFFFFF");
               } else {
                   $(".ddlMonedas").attr("disabled", "disabled");
                   $(".ddlMonedas").css("background", "#d2d2d2");
               }
           });

       }

       function HabilitaFechasDesflote(Habilitar) {
           $('.CampoFechas').datepicker({ yearRange: '-100:+10',
               changeYear: true,
               changeMonth: true,
               dateFormat: "dd/mm/yy",
               buttonImage: '../Contenido/Imagenes/calendar.gif',
               buttonImageOnly: true,
               toolTipText: "Fecha de compra",
               showOn: 'button',
               disabled: Habilitar
           });
           $('.CampoFechas').attr('readonly', true);
       }
        //RI0001
        function ValidarCampos() {
            if ($("#<%=hdnPaginaActual.ClientID%>").length) {
                if (!isNaN($("#<%=hdnPaginaActual.ClientID%>").val())) {
                    var pagina = parseInt($("#<%=hdnPaginaActual.ClientID%>").val());
                        if (pagina == 0) {
                            var s = "";
                            if ($("#<%=txtUnidad.ClientID%>").val().trim() != "") {
                                if ($("#<%=txtLiderID.ClientID%>").val().trim() == "") {

                                    if ($("#<%=txtTipoUnidad.ClientID%>").val().trim() != "") {
                                        if ($("#<%=hdnTipoUnidadID.ClientID %>").val().trim() == "") {
                                            s += ", Tipo de Unidad no válido";
                                        }
                                    } else {
                                        s += ", Tipo de Unidad";
                                    }
                                    if ($("#<%=txtMarca.ClientID%>").val().trim() != "") {
                                        if ($("#<%=hdnMarcaID.ClientID%>").val().trim() == "") {
                                            s += ", Marca no válida";
                                        }
                                    } else {
                                        s += ", Marca";
                                    }
                                    if ($("#<%=txtModelo.ClientID%>").val().trim() != "") {
                                        if ($("#<%=hdnModeloID.ClientID %>").val().trim() == "") {
                                            s += ", Modelo no válida";
                                        }
                                    } else {
                                        s += ", Modelo";
                                    }
                                    if ($("#<%=txtDistribuidor.ClientID %>").val().trim() != "") {
                                        if ($("#<%=hdnDistribuidorID.ClientID %>").val().trim() == "") {
                                            s += ", Fabricante no válido";
                                        }
                                    } else {
                                        s += ", Fabricante";
                                    }
                                    if ($("#<%=txtMotorizacion.ClientID%>").val().trim() != "") {
                                        if ($("#<%=hdnMotorizacionID.ClientID%>").val().trim() == "") {
                                            s += ", Motorización no válido";
                                        }
                                    } else {
                                        s += ", Motorización";
                                    }
                                    if ($("#<%=txtAplicacion.ClientID%>").val().trim() != "") {
                                        if ($("#<%=hdnAplicacionID.ClientID%>").val().trim() == "") {
                                            s += ", Aplicación no válida";
                                        }
                                    } else {
                                        s += ", Aplicación";
                                    }
                                    if ($("#<%=txtProximoServicio.ClientID %>").val().trim() == "") {
                                        s += ", Fecha de Próximo Servicio";
                                    }
                                    if ($("#<%=txtKMProximoServicio.ClientID %>").val().trim() == "") {
                                        s += ", Próximo Servicio";
                                    }
                                    if ($("#<%=txtKMInicial.ClientID %>").val().trim() == "") {
                                        s += ", KM Inicial";
                                    }
                                    if ($("#<%=txtHRSInicial.ClientID %>").val().trim() == "") {
                                        s += ", Hrs Inicial";
                                    }
                                    if ($("#<%=txtCombustibleTotal.ClientID %>").val().trim() == "") {
                                        s += ", Combustible Consumido Total";
                                    }
                                    if (s.trim() != "") {
                                        MensajeGrowUI("Falta informacion necesaria para continuar con el registro" + s, "4");
                                        return false;
                                    } else {
                                        return true;
                                    }
                                } else {
                                    return true;
                                }
                            } else {
                                MensajeGrowUI("Debe Seleccionar la unidad antes de Continuar.", "4");
                                return false;
                            }
                        }
                    return true;
                }
                    } else {
                    MensajeError("Inconsistencias al obtener el valor de la página a mostrar", " RegistrarActaNacimientoUI.aspx:ValidarCampos()");
                    return false;
                }
            return false;
        }
    </script>
    <script type="text/javascript">

        function InicializarControlesEmpresas(valores) {
            /// <summary>
            /// Inicializa los objetos o controles de la página por empresa, que se van a mostrar o no.
            /// </summary>
            /// <param name="valores">Hijo o child de la pestaña a ocultar</param>   

            var lstElementos = valores.split(',');
            for (var i = 0; i < lstElementos.length; i++) {
               document.getElementById("tabsContent").children[lstElementos[i]].style.display = "none";
           }
        }

        function BtnBuscar(guid, xml, sender) {
            setWizardStep($("#<%=hdnPaginaActual.ClientID %>").val());
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#" + sender),
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
    <script language="javascript" type="text/javascript">
        function setWizardStep(stepNumber) {
            $(".WizardSteps ul li").removeClass("ActualStep");
            $(".WizardSteps ul li").removeClass("PastStep");

            if (stepNumber != "" && $("#wizard-step-" + stepNumber) != null) {
                $("#wizard-step-" + stepNumber).addClass("ActualStep");
            }
            
            if (stepNumber <= 0 || stepNumber == "") {
                $(".BarraEstaticaWizard").hide();
                $(".GroupBody").css({'margin-right':'0px', 'margin-top':'30px'});
            }
            else {
                $(".BarraEstaticaWizard").show();
                $(".GroupBody").css({ 'margin-right': '38px', 'margin-top': '10px' });
            }

            for (var i = stepNumber - 1; i > 0; i--) {
                $("#wizard-step-" + i).addClass("PastStep");
            }

            $("#<%=lblTituloPaso.ClientID %>").text("Registrar Acta de Nacimiento");
            if (stepNumber == 0) $("#<%=lblTituloPaso.ClientID %>").text("Selección de la Unidad");
            if (stepNumber == 1) $("#<%=lblTituloPaso.ClientID %>").text("Datos Generales");
            if (stepNumber == 2) $("#<%=lblTituloPaso.ClientID %>").text("Datos Técnicos");
            if (stepNumber == 3) $("#<%=lblTituloPaso.ClientID %>").text("Números de Serie");
            if (stepNumber == 4) $("#<%=lblTituloPaso.ClientID %>").text("Llantas");
            if (stepNumber == 5) $("#<%=lblTituloPaso.ClientID %>").text("Equipos Aliados");
            if (stepNumber == 6) $("#<%=lblTituloPaso.ClientID %>").text("Trámites");
            if (stepNumber == 7) $("#<%=lblTituloPaso.ClientID %>").text("Fin del Registro");

            if (stepNumber == "" || stepNumber == 0 || stepNumber == 7) {
                $(".WizardSteps ul li").unbind("click");
            }
            else {
                $(".WizardSteps ul li").bind('click', function () { jumpToWizardStep(this); });
                $("#wizard-step-7").unbind("click");
            }            
        }

        function jumpToWizardStep(liControl) {
            var stepNumber = $(liControl).attr('id').replace("wizard-step-", "");
            $("#<%=hdnPaginaBrinco.ClientID %>").val(stepNumber);
            $("#<%=btnBrincarPagina.ClientID %>").click();
        }
    </script>
    <script language="javascript" type="text/javascript">
        function setNumeroEconomico(control) {
            $("#<%=txtEstaticoNumEconomico.ClientID %>").val($(control).val());
        }
    </script>
    <script language="javascript" type="text/javascript">
        function abrirConfirmacion(msg, botonID) {
            var $div = $('<div title="Confirmación"></div>');
            $div.append(msg);
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack(botonID, "");
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>

     <script type="text/javascript">
          function confirmarPaqueteServicio(origen) {
              var $div = $('<div title="Confirmación"></div>');
              $div.append("Registro exitoso <br>")
              var CodError = document.getElementById("MainContent_LabelError").innerHTML;
              var CodError = CodError.split('\n').join('<br>');
              $div.append(CodError);
              $("#dialog:ui-dialog").dialog("destroy");
              $($div).dialog({
                  closeOnEscape: true,
                  modal: true,
                  minWidth: 460,
                  close: function () { $(this).dialog("destroy"); },
                  buttons: {
                      Aceptar: function () {
                          $(this).dialog("close");
                      }
                  }
              });
          }
    </script>

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Label ID="LabelError" runat="server" Text="" style="display: none;"></asp:Label>
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - REGISTRAR ACTA DE NACIMIENTO</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 65px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/ConsultarActaNacimientoUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Equipos.UI/RegistrarActaNacimientoUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="WizardSteps">
                    <ul id="tabsContent">
                        <li id="wizard-step-1">1.DATOS GENERALES</li>
                        <li id="wizard-step-2">2.DATOS TÉCNICOS</li>
                        <li id="wizard-step-3">3.NÚMEROS DE SERIE</li>
                        <li id="wizard-step-4">4.LLANTAS</li>
                        <li id="wizard-step-5">5.EQUIPOS ALIADOS</li>
                        <li id="wizard-step-6">6.TRÁMITES</li>
                        <li id="wizard-step-7">7.FIN DE REGISTRO</li>
                    </ul>
                </div>
                <div class="Ayuda" style="top: 0px;">                    
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoRegistro" type="button" value="REINICIAR REGISTRO"
                    onclick="window.location='<%= Page.ResolveUrl("~/Equipos.UI/RegistrarActaNacimientoUI.aspx") %>'" />
            </div>
        </div>

        <div class="BarraEstaticaWizard">
            <span>Datos de Unidad</span>
            <br /><span id="RE01" runat="server">VIN</span>
            <br /><asp:TextBox ID="txtEstaticoNumSerie" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE02" runat="server">Clave Activo Oracle</span>
            <br /><asp:TextBox ID="txtEstaticoClaveOracle" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE03" runat="server">ID Leader</span>
            <br /><asp:TextBox ID="txtEstaticoIDLeader" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE04" runat="server"># Económico</span>
            <br /><asp:TextBox ID="txtEstaticoNumEconomico" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE05" runat="server">Tipo Unidad</span>
            <br /><asp:TextBox ID="txtEstaticoTipoUnidad" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE07" runat="server">Modelo</span>
            <br /><asp:TextBox ID="txtEstaticoModelo" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE08" runat="server">Año</span>
            <br /><asp:TextBox ID="txtEstaticoAnio" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE09" runat="server">Fecha Compra</span>
            <br /><asp:TextBox ID="txtEstaticoFechaCompra" runat="server" Enabled="false"></asp:TextBox>
            <br /><span id="RE12" runat="server">Monto Factura</span>
            <br /><asp:TextBox ID="txtEstaticoMontoFactura" runat="server" Enabled="false"></asp:TextBox>
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho4Opciones">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btnWizardContinuar" onclick="btnContinuar_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnTerminar" runat="server" Text="Terminar" CssClass="btnWizardTerminar" onclick="btnTerminar_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnBorrador" runat="server" Text="Borrador" CssClass="btnWizardBorrador" onclick="btnBorrador_Click" ValidationGroup="Obligatorios" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click"  />
                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" CssClass="btnWizardAtras" onclick="btnAnterior_Click" ValidationGroup="Obligatorios" />
                </div>
            </div>
            <div id="ControlesDatos">
                <asp:MultiView ID="mvCU077" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPagina0" runat="server">
                        <table class="trAlinearDerecha" style="margin: 0px auto; width: 530px; display: inherit; border: 1px solid transparent;">
                        <tr runat="server" id="trAccesorio">
                            <td class="tdCentradoVertical"><asp:Label ID="lblAccesorio" runat="server">Accesorio</asp:Label></td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 330px;">
                            <asp:CheckBox runat="server" ID="cbAccesorio"/>
                            </td>
                        </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblVIN" runat="server">VIN</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtUnidad" runat="server" Width="275px" AutoPostBack="true" ontextchanged="txtUnidad_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaUnidad_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><asp:Label ID="lblActivoOracle" runat="server">Clave Activo Oracle</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtClaveActivoOracle" runat="server" Width="301px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"> <asp:Label ID="lblLider" runat="server">ID Lider</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtLiderID" runat="server" Width="301px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblNumEcono" runat="server"># Económico</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtNumeroEconomico" runat="server" Width="301px" MaxLength="10" onchange="javascript: setNumeroEconomico(this);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblTipoUnidad" runat="server">Tipo Unidad</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtTipoUnidad" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtTipoUnidad_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaTipoUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaTipoUnidad_Click" />
                                    <asp:HiddenField ID="hdnTipoUnidadID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblMarca" runat="server">Marca</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtMarca" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtMarca_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaMarca" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaMarca_Click" />
                                    <asp:HiddenField ID="hdnMarcaID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblModelo" runat="server">Modelo</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px; vertical-align: middle;">
                                    <asp:TextBox ID="txtModelo" runat="server" Width="175px" AutoPostBack="True" ontextchanged="txtModelo_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaModelo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaModelo_Click" />
                                    
                                    <asp:HiddenField ID="hdnModeloID" runat="server" />
                                    <asp:Label ID="lblAnio" runat="server"><span>Año</span></asp:Label>
                                    <asp:TextBox ID="txtAnio" runat="server" Width="45px" MaxLength="4"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revAnio" runat="server" ControlToValidate="txtAnio"
                                        ErrorMessage="Formato incorrecto" ValidationExpression="^[123][0-9]{3}$"
                                        ValidationGroup="Obligatorios" Display="Dynamic" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><asp:Label ID="lblFechaCompra" runat="server">Fecha Compra</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtFechaCompra" runat="server" Width="95px" CssClass="CampoFechas"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblFabricante" runat="server">Fabricante</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtDistribuidor" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtDistribuidor_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaDistribuidor" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaDistribuidor_Click" />
                                    <asp:HiddenField ID="hdnDistribuidorID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><asp:Label ID="lblMontoFactura" runat="server">Monto Factura</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtMontoFactura" runat="server" Width="301px" CssClass="CampoMoneda"></asp:TextBox>
                                </td>
                            </tr>

                            <tr id="trFechaInicioDepreciacion" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblInicioDepreciacion" runat="server">Fecha Inicio Depreciación</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtFechaInicioDepreciacion" runat="server" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFechas" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trFechaDesflote" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblFechaDesflote" runat="server">Fecha Ideal de Desflote</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                <asp:TextBox ID="txtFechaDesflote" runat="server" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFechas" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trTasaDepreciacion" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblTasaDepreciacion" runat="server">Tasa Depreciación</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                <asp:TextBox ID="txtTasaDepreciacion" runat="server" MaxLength="9" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" ClientIDMode="Static" CssClass="CampoNumero" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trVidaUtil" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblVidaUtil" runat="server">Vida Util(Años)</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                <asp:TextBox ID="txtVidaUtil" runat="server" MaxLength="9" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" ClientIDMode="Static" CssClass="CampoNumeroEntero" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trPorcentajeVR" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblPorcentajeValorRes" runat="server">Porcentaje Valor Residual</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                <asp:TextBox ID="txtPorcentajeVR" runat="server" MaxLength="9" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" ClientIDMode="Static" CssClass="CampoNumero" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trValorResidual" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblValorResidual" runat="server">Valor Residual</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                <asp:TextBox ID="txtValorResidual" runat="server" MaxLength="9" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" ClientIDMode="Static" CssClass="CampoNumero" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trSaldoDepreciar" runat="server">
                                <td class="tdCentradoVertical"><asp:Label ID="lblSaldoDepreciar" runat="server">Saldo por Depreciar</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                <asp:TextBox ID="txtSaldoDepreciar" runat="server" MaxLength="9" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" ClientIDMode="Static" CssClass="CampoNumero" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>

                            <tr><td colspan="3">&nbsp;</td></tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblMotorizacion" runat="server">Motorización</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtMotorizacion" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtMotorizacion_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaMotorizacion" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaMotorizacion_Click" />
                                    <asp:HiddenField ID="hdnMotorizacionID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblAplicacion" runat="server">Aplicación</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtAplicacion" runat="server" Width="275px" AutoPostBack="True" ontextchanged="txtAplicacion_TextChanged"></asp:TextBox>
                                    <asp:ImageButton ID="ibtnBuscaAplicacion" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" onclick="ibtnBuscaAplicacion_Click" />
                                    <asp:HiddenField ID="hdnAplicacionID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblFechaProximoServicio" runat="server">Próximo Servicio</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtProximoServicio" runat="server" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFechas"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revFechaProximoServicio" runat="server" ControlToValidate="txtProximoServicio"
                                        Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                        ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblKmProximoServicio" runat="server">Próximo Servicio</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtKMProximoServicio" runat="server" MaxLength="9" Width="195px"
                                        CausesValidation="True" ValidationGroup="FormatoValido" ClientIDMode="Static" CssClass="CampoNumeroEntero"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" id="trKMInicial">
                                <td class="tdCentradoVertical"><span>*</span><asp:Label ID="lblKMInicial" runat="server">Inicial (Km)</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtKMInicial" runat="server" MaxLength="9" Width="195px" CausesValidation="True"
                                        ValidationGroup="FormatoValido" CssClass="CampoNumeroEntero"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span id="spanHrsInicial" runat="server">*</span><asp:Label ID="lblHrsInicial" runat="server">Inicial (Hrs)</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtHRSInicial" runat="server" MaxLength="9" Width="195px" CausesValidation="True"
                                        ValidationGroup="FormatoValido" CssClass="CampoNumeroEntero"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical"><span id="spanCombustibleTotal" runat="server">*</span><asp:Label ID="lblCombustibleTotal" runat="server">Combustible Total</asp:Label></td>
                                <td style="width: 20px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtCombustibleTotal" runat="server" MaxLength="9" Width="195px" CausesValidation="True"
                                        ValidationGroup="FormatoValido" CssClass="CampoNumeroEntero"></asp:TextBox>
                                </td>
                            </tr>


                        </table>
                    </asp:View>
                    <asp:View ID="vwPagina1" runat="server">
                        <ucPagina1:ucDatosGeneralesUI ID="ucDatosGeneralesUI" runat="server" />
                    </asp:View>
                    <asp:View ID="vwPagina2" runat="server">
                        <ucPagina2:ucDatosTecnicosUI ID="ucDatosTecnicosUI" runat="server" />
                    </asp:View>
                    <asp:View ID="vwPagina3" runat="server">
                        <ucPagina3:ucNumerosSerieUI ID="ucNumerosSerieUI" runat="server" />
                    </asp:View>
                    <asp:View ID="vwPagina4" runat="server">
                        <ucPagina4:ucAsignacionLlantasUI ID="ucAsignacionLlantasUI" runat="server" />
                    </asp:View>
                    <asp:View ID="vwPagina5" runat="server">
                        <ucPagina5:ucAsignacionEquiposAliadosUI ID="ucAsignacionEquiposAliadosUI" runat="server" />
                    </asp:View>
                    <asp:View ID="vwPagina6" runat="server">
                        <ucPagina6:ucTramitesActivosUI ID="ucTramitesActivosUI" runat="server" />
                    </asp:View>
                    <asp:View ID="vwPagina7" runat="server">
                        <ucPagina7:ucResumenActaNacimientoUI ID="ucResumenActaNacimientoUI" runat="server" />
                    </asp:View>
                </asp:MultiView>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
    </div>
    
    <asp:HiddenField ID="hdnEquipoId" runat="server" />
    <asp:HiddenField ID="hdnUnidadId" runat="server" />
    <asp:HiddenField ID="hdnEstatusUnidad" runat="server" />
    <asp:HiddenField ID="hdnPaginaActual" runat="server" />
    <asp:HiddenField ID="hdnPaginaBrinco" runat="server" />
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnLibroActivos" runat="server" />

    <asp:Button ID="btnResult" runat="server" Text="" onclick="btnResult_Click" style="display: none;" />
    <asp:Button ID="btnBrincarPagina" runat="server" Text="" onclick="btnBrincarPagina_Click" style="display: none;" />
</asp:Content>
