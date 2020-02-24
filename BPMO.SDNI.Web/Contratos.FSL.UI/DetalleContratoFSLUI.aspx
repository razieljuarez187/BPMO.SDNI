<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="DetalleContratoFSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.DetalleContratoFSLUI" %>
    <%-- Satisface al Caso de Uso CU022 - Consultar Contrato Full Service Leasing--%>
<%@ Register Src="ucLineaContratoFSLUI.ascx" TagName="ucLineaContratoFSLUI" TagPrefix="uc" %>
<%@ Register Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" TagName="ucCatalogoDocumentosUI" TagPrefix="uc" %>
<%@ Register Src="ucHerramientasFSLUI.ascx" TagName="HerramientasFSLUI" TagPrefix="uc" %>
<%@ Register Src="ucInformacionGeneralUI.ascx" TagName="ucInformacionGeneralUI" TagPrefix="uc" %>
<%@ Register Src="ucInformacionPagoUI.ascx" TagName="ucInformacionPagoUI" TagPrefix="uc" %>
<%@ Register src="ucClienteContratoUI.ascx" tagName="ucClienteContratoUI" tagPrefix="uc" %>
<%@ Register src="ucDatosRentaUI.ascx" tagName="ucDatosRentaUI" tagPrefix="uc" %>
<%@ Register src="ucDatosAdicionalesAnexoUI.ascx" tagName="ucDatosAdicionalesUI" tagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ValidaEliminarContrato() {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('¿Desea Eliminar el Borrador del Contrato?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 460,
                minHeight: 250,
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack("<%= btnEliminarContratoBorrador.UniqueID %>", "");
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        initChild = function () {
            $("span:contains('*')").hide();
                
            <%= ucHerramientas.ClientID %>_Inicializar();
        };
            
        $(document).ready(initChild);
    </script>
    <style type="text/css">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONSULTAR DETALLES DE CONTRATO FULL SERVICE LEASING</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.FSL.UI/ConsultarContratosFSLUI.aspx">
                        CONSULTAR F.S.L.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.FSL.UI/RegistrarContratoFSLUI.aspx">
                        REGISTRAR RENTA F.S.L. 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <uc:herramientasfslui ID="ucHerramientas" runat="server" />
        </div>
        <asp:MultiView ID="mvCU022" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwContrato" runat="server">
                <div id="divInformacionGeneral" class="GroupBody">
                    <div id="divInformacionGeneralHeader" class="GroupHeader">
                        <span>Contrato DE Renta FULL SERVICE</span>
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
                    <fieldset>
                        <legend><span>CONFIGURACI&Oacute;N DE INPC</span></legend>
                        <table style="width: 100%;">
                            <tr>
                                <td class="tdAlinearDerecha"><span>TIPO INPC</span></td>
                                <td class="tdEspacioCentro"></td>
                                <td class="tdAlinearIzquierda">
                                    <asp:TextBox runat="server" ID="txtTipoInpc" Width="50%" Enabled="False"></asp:TextBox>
                                </td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td class="tdAlinearDerecha"><span>FECHA INICIO INPC</span></td>
                                <td class="tdEspacioCentro"></td>
                                <td class="tdAlinearIzquierda">
                                    <asp:TextBox runat="server" ID="txtFechaInicioINPC" CssClass="CampoFecha"  Width="50%" Enabled="False"></asp:TextBox>
                                </td>
                                <td class="tdAlinearDerecha"><span>VALOR de INPC</span></td>
                                <td class="tdEspacioCentro"></td>
                                <td class="tdAlinearIzquierda">
                                    <asp:TextBox runat="server" ID="txtValorINPC" CssClass="CampoFecha"  Width="50%" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsDatosAdicionales">
                        <legend>Datos Adicionales de Anexo</legend>
                        <uc:ucDatosAdicionalesUI runat="server" ID="ucDatosAdicionales" />
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
        </asp:MultiView>
    </div>
    <asp:Button runat="server" ID="btnEliminarContratoBorrador" onclick="btnEliminarContratoBorrador_Click" Style="display: none;"/>
    <asp:HiddenField runat="server" ID="hdnContratoID"/>
    <asp:HiddenField runat="server" ID="hdnEstatusContrato"/>
    <asp:HiddenField runat="server" ID="hdnUnidadOperativaContratoID"/>
</asp:Content>
