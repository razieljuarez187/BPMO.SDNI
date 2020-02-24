<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarContratoFSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.RegistrarContratoFSLUI" %>


<%-- Satisface al Caso de Uso CU022 - Consultar Contrato Full Service Leasing--%>
<%-- Satisface al caso de uso CU015 - Registrar Contrato FULL SERVICE LEASING --%>
<%@ Register Src="ucLineaContratoFSLUI.ascx" TagName="ucLineaContratoFSLUI" TagPrefix="uc" %>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosUI" TagPrefix="uc" %>
<%@ Register Src="ucInformacionGeneralUI.ascx" TagName="ucInformacionGeneralUI" TagPrefix="uc" %>
<%@ Register Src="ucInformacionPagoUI.ascx" TagName="ucInformacionPagoUI" TagPrefix="uc" %>
<%@ Register src="ucClienteContratoUI.ascx" tagName="ucClienteContratoUI" tagPrefix="uc" %>
<%@ Register src="ucDatosRentaUI.ascx" tagName="ucDatosRentaUI" tagPrefix="uc" %>
<%@ Register Src="ucDatosAdicionalesAnexoUI.ascx" TagName="ucDatosAdicionalesUI" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
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
        .tdAlinearDerecha {
        text-align: right;
        width: 18%;
        vertical-align: middle;
        }
        .tdEspacioCentro {
            width: 2%;
        }
        .tdAlinearIzquierda {
            width: 30%;
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

            initChild = function() {
                <%= ucInformacionGeneral.ClientID %>_Inicializar();
                <%= ucInformacionPago.ClientID %>_Inicializar();
                <%= ucDatosRenta.ClientID %>_Inicializar();
                <%= ucDatosAdicionales.ClientID %>_Inicializar();
            };

            $(document).ready(initChild);
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - REGISTRAR CONTRATO FULL SERVICE LEASING</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.FSL.UI/ConsultarContratosFSLUI.aspx">
                        CONSULTAR F.S.L.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.FSL.UI/RegistrarContratoFSLUI.aspx">
                        REGISTRAR RENTA F.S.L.
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
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
        <asp:MultiView ID="mvCU015" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwContrato" runat="server">
                <div id="divInformacionGeneral" class="GroupBody">
                    <div id="divInformacionGeneralHeader" class="GroupHeader">
                        <span>Contrato DE Renta FULL SERVICE</span>
                        <div class="GroupHeaderOpciones Ancho3Opciones">
                            <asp:Button ID="btnTermino" runat="server" Text="Terminar" CssClass="btnWizardTerminar" OnClick="btnTermino_Click" />
                            <asp:Button ID="btnGuardar" runat="server" Text="Borrador" CssClass="btnWizardBorrador" OnClick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                    <div id="divInformacionGeneralControles">
                        <uc:ucInformacionGeneralUI runat="server" ID="ucInformacionGeneral" />
                    </div>
                </div>
                <div id="divDatosCliente" class="GroupBody">
                    <div id="divDatosClienteHeader" class="GroupHeader">
                        <span>DATOS DE CLIENTE</span>
                    </div>
                    <div id="divDatosClienteControles">
                        <uc:ucClienteContratoUI runat="server" ID="ucClienteContrato" />
                    </div>    
                </div>
                <uc:ucDatosRentaUI runat="server" ID="ucDatosRenta" />
                <div id="divDatosFinales" class="GroupBody">
                    <uc:ucInformacionPagoUI runat="server" ID="ucInformacionPago"/>
                    <div style="width: 100%; text-align: center;">
                        <asp:Button runat="server" ID="btnConfigurarINPC" Text="Configurar INPC" CssClass="btnWizardEditar" OnClick="btnConfigurarINPC_OnClick"/>
                    </div>
                    <fieldset id="fsDatosAdicionales">
                        <legend>Datos Adicionales de Anexo A</legend>
                        <uc:ucDatosAdicionalesUI ID="ucDatosAdicionales" runat="server" />
                    </fieldset>
                    <fieldset id="fsDocumentosAdjuntos">
                        <legend>Documentos Adjuntos al Contrato</legend>
                        <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="vwLineaContrato" runat="server">
                <uc:ucLineaContratoFSLUI ID="ucLineaContrato" runat="server" />
            </asp:View>
            <asp:View ID="vwConfiguracionINPC" runat="server">
                <div class="GroupBody">
                    <div class="GroupHeader">
                        <span>CONFIGURACI&Oacute;N DE INPC</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                             <asp:Button ID="btnGuardarINPC" runat="server" Text="GUARDAR" CssClass="btnWizardGuardar" OnClick="btnGuardarINPC_OnClick" />
                             <asp:Button ID="btnCancelarINPC" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelarINPC_OnClick" />
                        </div>
                    </div>
                    <div>
                        <fieldset style="width: 80%; margin: 0 auto;">
                            <legend>
                                <span>CONFIGURAR</span>
                            </legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td class="tdAlinearDerecha"><span>Tipo INPC</span></td>
                                    <td class="tdEspacioCentro"></td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:DropDownList runat="server" ID="ddlTipoInpc" Width="50%" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoInpc_OnSelectedIndexChanged">
                                            <asp:ListItem Text="SELECCIONE" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="AUTOM&Aacute;TICO" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="FIJO" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td class="tdAlinearDerecha"><span>A&Ntilde;O DE INICIO DE INPC</span></td>
                                    <td class="tdEspacioCentro"></td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:DropDownList runat="server" ID="ddlAnioInicioInpc" Width="50%"/>
                                    </td>
                                    <td class="tdAlinearDerecha"><span>FACTOR INPC</span></td>
                                    <td class="tdEspacioCentro"></td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:TextBox runat="server" ID="txtValorInpc" Width="20%" CssClass="CampoNumero"></asp:TextBox>
                                        <asp:RegularExpressionValidator runat="server" ID="rexValorInpc" ControlToValidate="txtValorInpc" ValidationExpression="^\d+\.?\d{0,2}$"
                                        Display="Dynamic" ErrorMessage="2 decimales máximo" ForeColor="RED"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    <div class="ContenedorMensajes">
        <span class="Requeridos RequeridosFSL"></span>
        <br />
        <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
    </div>
    <asp:HiddenField ID="hdnClave" runat="server" />
</asp:Content>
