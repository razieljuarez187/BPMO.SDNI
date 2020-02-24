<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarEntregaUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.RegistrarEntregaUI" %>

<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
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
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarListadoVerificacionUI.aspx">
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
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" CssClass="btnWizardTerminar"
                        OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="divInformacionGeneralControles">
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
                                    Operador
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNombreOperador" Width="200px"></asp:TextBox>
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
                                    <label># Econ&oacute;mico</label>
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
                                    # Serie
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtNumeroSerie" Width="110px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    <label># Placas Federales</label>
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtPlacasFederales" Width="65px"></asp:TextBox>
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
                                        ontextchanged="txtCombustibleSalida_TextChanged"></asp:TextBox>
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
                                    <span>*</span>KM Tablero
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtKilometrajeSalida" MaxLength="9" CssClass="CampoNumeroEntero" AutoPostBack="True" OnTextChanged="txtKilometrajeSalida_OnTextChanged"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnKilometrajeAnterior"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical">
                                    Hrs Refrigeración
                                </td>
                                <td style="width: 5px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtHoraRefrigeracion" MaxLength="9" CssClass="CampoNumeroEntero" AutoPostBack="True" OnTextChanged="txtHoraRefrigeracion_OnTextChanged"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnHorometroAnterior"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dvOpciones" style="display: table; width: 100% ">
                        <div class="dvIzquierda">
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Interior Limpio
                                    </td>
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtLimpiezaInteriosCaja" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Golpes En General
                                    </td>
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtGolpesGeneral" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="dvDerecha">
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td class="tdCentradoVertical">
                                        Documentaci&oacute;n Completa
                                    </td>
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
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
                                    <td style="width: 5px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical">
                                        <asp:RadioButtonList runat="server" ID="rbtBateriasCorrectas" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="SI" Value="true" />
                                            <asp:ListItem Text="NO" Value="false" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Observaciones de LA Informaci&oacute;n DE ENTREGA</legend>
                    <table class="trAlinearDerecha">
                        <tr>
                            <td align="right" style="padding-top: 5px; vertical-align: middle">
                                Observaci&oacute;n Documentaci&oacute;n
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox runat="server" ID="txtObservacionesDocumentacion" TextMode="MultiLine"
                                    Width="750px" Height="45px" Style="max-width: 750px; min-width: 750px; max-height: 45px;
                                    min-height: 45px;">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px; vertical-align: middle">
                                Observaci&oacute;n Bater&iacute;as
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox runat="server" ID="txtObservacionesBaterias" TextMode="MultiLine" Width="750px"
                                    Height="45px" Style="max-width: 750px; min-width: 750px; max-height: 45px; min-height: 45px;">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Llantas</legend>
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
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:HiddenField runat="server" ID="hdnRefaccionID" />
                                    <asp:TextBox runat="server" ID="txtRefaccion"></asp:TextBox>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td align="right" style="padding-top: 5px; vertical-align: middle">
                                    Es Correcta
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
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
                            <td align="right" style="padding-top: 5px; vertical-align: middle;">
                                Observaci&oacute;n Llanta
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical">
                                <asp:TextBox runat="server" ID="txtObservacionesLlanta" TextMode="MultiLine" Width="750px"
                                    Height="45px" Style="max-width: 750px; min-width: 750px; max-height: 45px; min-height: 45px;">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Unidad</legend>
                    <div id="dvCamioncitos" style="text-align: center;;
                        width: 900px; height: 200px;">
                        <img src="../Contenido/Imagenes/camion-24-PV2.png" style="float: left; height: 170px;
                            width: 250px;" />
                        <img src="../Contenido/Imagenes/camion-24-PV4.png" style="float: left; height: 170px;
                            width: 165px; margin-left: 20px;" />
                        <img src="../Contenido/Imagenes/camion-24-PV3.png" style="float: left; height: 170px;
                            width: 165px; margin-left: 20px;" />
                        <img src="../Contenido/Imagenes/camion-24-PV.png" style="float: right; height: 170px;
                            width: 250px;" />
                    </div>
                    <fieldset>
                        <legend>Observaci&oacute;n</legend>
                        <table class="trAlinearDerecha">
                            <tr>
                                <td align="right" style="padding-top: 5px; vertical-align: middle">
                                    Secci&oacute;n
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:DropDownList ID="ddlSeccionesunidad" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td align="right" style="padding-top: 5px; vertical-align: middle">
                                    Observaci&oacute;N
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtObservacionesSeccion" TextMode="MultiLine" Width="350px"
                                        Height="85px" Style="max-width: 350px; min-width: 350px; max-height: 85px; min-height: 85px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        CssClass="ColorValidator" ControlToValidate="txtObservacionesSeccion" ValidationGroup="addSeccion"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:Button ID="btnAgregarSeccion" runat="server" Text="Agregar Secci&oacute;n" ToolTip="Agregar observaciones de la sección"
                                        CssClass="btnAgregarATabla" ValidationGroup="addSeccion" OnClick="btnAgregarSeccion_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:UpdatePanel ID="updPnimagenes" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td align="right" style="padding-top: 5px">
                                            Archivo
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="uplArchivoImagen" runat="server" Width="580px" />
                                            <asp:RequiredFieldValidator ID="rfvUplArchivo" runat="server" ErrorMessage="*" CssClass="ColorValidator"
                                                ControlToValidate="uplArchivoImagen" ValidationGroup="FileUploadImagen"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 5px;">
                                            &nbsp;
                                        </td>
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
            </div>
            <div id="divDatosFinales">
                <fieldset id="fsDocumentosAdjuntos">
                    <legend>Documentos Adjuntos al Check List</legend>
                    <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                </fieldset>
            </div>
        </div>
    </div>
    <div id="dvDialogImagenesSecccion" style="display: none;" title="IMAGENES DE LA SECCIÓN">
        <asp:GridView ID="grdImagenesDialog" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            PageSize="15" CellPadding="4" GridLines="None" CssClass="Grid" Width="95%" Style="margin: 10px auto;">
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
            </Columns>
            <HeaderStyle CssClass="GridHeader" />
            <EditRowStyle CssClass="GridAlternatingRow" />
            <PagerStyle CssClass="GridPager" />
            <RowStyle CssClass="GridRow" />
            <FooterStyle CssClass="GridFooter" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
            <EmptyDataTemplate>
                <b>No se han agregado imagenes a la sección.</b>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <div class="ContenedorMensajes">
        <span class="Requeridos RequeridosFSL"></span>
        <br />
        <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
    </div>
</asp:Content>
