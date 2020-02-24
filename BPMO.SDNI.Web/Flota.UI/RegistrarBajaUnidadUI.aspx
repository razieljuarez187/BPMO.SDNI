<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarBajaUnidadUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.RegistrarBajaUnidadUI" %>
<%--Satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA--%>
<%--Satisface la solicitud de cambio SC0006--%>

<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>}
    <script type="text/javascript">
        $(document).ready(function() {            
            Sys.Application.add_load(function(){
                createDatePickers();
            });
        });

        function createDatePickers() {
            //Formato de Fechas
            $(".CampoFecha").datepicker({
                yearRange: '-100:+10',
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '<%= this.ResolveUrl("~/Contenido/Imagenes/calendar.gif") %>',
                buttonImageOnly: true,
                toolTipText: "Fecha de compra",
                showOn: 'button'
            })
            .attr('readonly', true);

            $(".CampoFecha:enabled").datepicker("enable");
            $(".CampoFecha:disabled").datepicker("disable");
        }
    </script>
    <link href="../Contenido/Estilos/EstiloActaNacimiento.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*Estilos para alinear verticalmente los elementos del menú con una imagen*/
        div.MenuPrimario ul:first-child li:nth-of-type(7) a input[type="text"]
        {
            margin-top: -4px !important;
        }
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a, 
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a, 
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a
        {
            display: table !important;
        }
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a span, 
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a span, 
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a span, 
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a img, 
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a img, 
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a img
        {
            display: table-cell !important;
            vertical-align: middle;
        }
        div.MenuPrimario ul:first-child li:nth-of-type(4).static a img, 
        div.MenuPrimario ul:first-child li:nth-of-type(5).static a img, 
        div.MenuPrimario ul:first-child li:nth-of-type(6).static a img
        {
            padding-left: 3px;
        }
        /*Estilo para hacer que el primer elemento del menú, al seleccionarse, se ve igual que al seleccionar otro*/
        div.MenuPrimario ul:first-child li:nth-of-type(1).Seleccionado a
        {
            color: White !important;
        }
        /*Estilo para marcar como deshabilitado el menu*/
        div.MenuPrimario ul:first-child li:nth-of-type(1).itemDeshabilitado a { color: Silver !important; }
        /*Estilo para alargar la barra lateral del acta de nacimiento para soportar un campo más*/
        .BarraEstaticaWizard { height: 425px; }
        /*Estilos de sucursal*/
        .ContenedorMovimiento fieldset, .ContenedorSiniestro fieldset { width: 100%; margin-right: auto; margin-left: auto; float: none; display: inherit; }
        .ContenedorMovimiento, .ContenedorSiniestro
        {
            float: none;
            display: inherit;
            width: 95%;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
        }
        .ContenedorObservaciones
        {
            float: none;
            display: inherit;
            width: 665px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 15px;
        }
    </style>
    <script type="text/javascript">
        initChild = function () {
            ConfiguracionBarraHerramientas();
        };
        $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - BAJA DE UNIDAD</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 65px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx"> CONSULTAR UNIDAD <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li id="Movimiento">
                    <asp:HyperLink ID="hlkMovimiento" runat="server" NavigateUrl="~/Flota.UI/CambiarSucursalEquipoAliadoUI.aspx"> MOVER EQ. ALIADO <img id="img1" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mFlota" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="Expediente" Value="Expediente" Selectable="False" Enabled="False">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Movimientos" Value="Movimientos" Selectable="true" Selected="True"
                            NavigateUrl="#"></asp:MenuItem>
                        <asp:MenuItem Text="Reporte" Value="Reportes" Selectable="False" Enabled="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Disponible" Value="Disponible" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Contrato" Value="Contrato" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Eq. Aliado" Value="EquipoAliado" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Placas" Value="Placas" Enabled="False" Selectable="false"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "Placas" || (string) Eval("Value") == "Disponible"|| (string) Eval("Value") == "Contrato"|| (string) Eval("Value") == "EquipoAliado" ? "Informacion" : string.Empty %>'
                            Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "Placas" %>'
                            Style="width: 100px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>
                        <asp:Image ID="imgEstatus" ImageUrl="~/Contenido/Imagenes/ESTATUS-NO-ICO.png" Visible='<%# (string) Eval("Value") == "Disponible" || (string) Eval("Value") == "Contrato" || (string) Eval("Value") == "EquipoAliado" %>'
                            runat="server" />
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx") %>'" />
            </div>
        </div>
        <div class="BarraEstaticaWizard">
            <span>Datos de Unidad</span>
            <br />
            <span>VIN</span>
            <br />
            <asp:TextBox ID="txtEstaticoNumSerie" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Clave Activo Oracle</span>
            <br />
            <asp:TextBox ID="txtEstaticoClaveOracle" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>ID Leader</span>
            <br />
            <asp:TextBox ID="txtEstaticoIDLeader" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span># Económico</span>
            <br />
            <asp:TextBox ID="txtEstaticoNumEconomico" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Tipo Unidad</span>
            <br />
            <asp:TextBox ID="txtEstaticoTipoUnidad" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Modelo</span>
            <br />
            <asp:TextBox ID="txtEstaticoModelo" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Año</span>
            <br />
            <asp:TextBox ID="txtEstaticoAnio" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Fecha Compra</span>
            <br />
            <asp:TextBox ID="txtEstaticoFechaCompra" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Monto Factura</span>
            <br />
            <asp:TextBox ID="txtEstaticoMontoFactura" runat="server" Enabled="false"></asp:TextBox>
            <br />
            <span>Folio Factura</span>
            <br />
            <asp:TextBox ID="txtFolioFacturaCompra" runat="server" Enabled="false"></asp:TextBox>
        </div>
        <!--Cuerpo de la página-->
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <span>BAJA DE UNIDAD</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" CssClass="btnWizardTerminar"
                        OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" CssClass="btnWizardCancelar"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <asp:HiddenField ID="hdnUnidadID" runat="server" Visible="False" />
                <asp:HiddenField ID="hdnEstaDisponible" runat="server" />
                <asp:HiddenField ID="hdnEstaEnContrato" runat="server" />
                <asp:HiddenField ID="hdnTieneEquipoAliado" runat="server" />
                <asp:Panel ID="pnlContenidoMovimiento" CssClass="ContenedorMovimiento" runat="server">
                    <fieldset>
                        <legend>Sucursal</legend>
                        <asp:Panel ID="pnlSucursal" runat="server">                        
                        <table class="trAlinearDerecha" style="width: 690px; margin: 0px auto">
                            <tr>
                                <td class="tdCentradoVertical" style="width: 75px;">Sucursal</td>
                                <td style="width: 5px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 275px;">
                                    <asp:TextBox ID="txtSucursal" runat="server" Width="240px" MaxLength="80" Enabled="False"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical" style="text-align: right;">Empresa</td>
                                <td style="width: 5px;">&nbsp;</td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtEmpresa" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 5px; text-align: right; width: 75px; vertical-align:top">Domicilio</td>
                                <td style="width: 5px;">&nbsp;</td>
                                <td class="tdCentradoVertical" colspan="4">
                                    <asp:TextBox ID="txtDireccionSucursal" runat="server" Rows="5" Columns="30" TextMode="MultiLine"
                                        Enabled="false" MaxLength="500" Style="float: left; max-width: 590px; min-width: 590px;
                                        max-height: 40px; min-height: 40px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>  
                        <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        <asp:HiddenField runat="server" ID="hdnEmpresaID" />           
                        </asp:Panel>
                    </fieldset>
                </asp:Panel>

                <%--SC0006 - Adición de controles para datos de siniestro--%>
                <asp:Panel ID="pnlContenedorSiniestro" CssClass="ContenedorSiniestro" runat="server"> 
                    <fieldset>
                    <legend>
                        <asp:CheckBox ID="chkSiniestro" Text="¿Siniestro?" runat="server" 
                            oncheckedchanged="chkSiniestro_CheckedChanged" AutoPostBack="True" 
                            TextAlign="Left" /></legend>
                        <asp:Panel ID="pnlSiniestro" Enabled="false" runat="server">                        
                        <table class="trAlinearDerecha" style="width: 690px; margin: 0px auto">
                            <tr>
                                <td class="tdCentradoVertical" style="width: 115px;">Fecha Siniestro</td>
                                <td style="width: 5px;">&nbsp;</td>
                                <td class="tdCentradoVertical" style="width: 220px;">
                                    <asp:TextBox ID="txtFechaSiniestro" runat="server" CssClass="CampoFecha"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical" style="text-align: right;">Fecha Dictamen</td>
                                <td style="width: 5px;">&nbsp;</td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtFechaDictamen" runat="server" CssClass="CampoFecha"></asp:TextBox>
                                </td>
                            </tr>                        
                            <tr>
                                <td class="tdCentradoVertical" style="width: 115px; text-align: right; vertical-align:top;">Observaciones</td>
                                <td style="width: 5px;">&nbsp;</td>
                                <td class="tdCentradoVertical" colspan="4">
                                    <asp:TextBox runat="server" ID="txtObservacionesSiniestro" TextMode="MultiLine" Width="100%" MaxLength="700"
                                        Height="90px" Style="max-height: 90px; min-height: 90px;">
                                    </asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="pnlContenedorObservaciones" CssClass="ContenedorObservaciones" runat="server">
                    <asp:Panel ID="pnlObservaciones" runat="server">
                    <span>*</span>Observaci&oacute;n
                    <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Width="655px"
                        Height="90px" Style="max-width: 655px; min-width: 655px; max-height: 90px; min-height: 90px;">
                    </asp:TextBox>
                    <br />
                    <br />
                     <asp:Panel ID="pnlCentroCostos" runat="server">
                        <span>*</span>Centro de Costos
                        <asp:TextBox runat="server" ID="txtCentroCostos" Width="240px">
                        </asp:TextBox>
                    </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <%--Documentos adjuntos al Check List--%>
                <asp:Panel ID="pnlContenedorDocumentos" runat="server">
                    <asp:Panel ID="pnlDocumentos" runat="server">
                        <fieldset id="fsDocumentosAdjuntos">
                            <legend>Documentos Adjuntos a la Baja</legend>
                            <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                        </fieldset>
                    </asp:Panel>
                </asp:Panel>
            </div>
            <div class="ContenedorMensajes">
                <span class="Requeridos"></span>
                <br />
                <span class="FormatoIncorrecto"></span>
            </div>
        </div>
    </div>
</asp:Content>
