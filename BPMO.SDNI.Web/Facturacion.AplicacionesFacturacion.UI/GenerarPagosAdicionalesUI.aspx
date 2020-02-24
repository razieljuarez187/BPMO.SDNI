<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="GenerarPagosAdicionalesUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.GenerarPagosAdicionalesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>"
        type="text/javascript"></script>
    <style>
        .alinearDerecha{
            width: 30%;
            text-align: right;
        }
        .separador{
            width: 5%;
        }
        .alinearIzquierda{
            width: 65%;
             text-align: left;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            inicializeHorizontalPanels();
        }

        function inicializeHorizontalPanels() {
            $(".GroupHeaderCollapsable").click(function () {
                $(this).next(".GroupContentCollapsable").slideToggle(500);
                if ($(this).find(".imgMenu").attr("src") == "../Contenido/Imagenes/FlechaArriba.png")
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaAbajo.png");
                else
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaArriba.png");
                return false;
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">PAGOS CONTRATOS - Generador de Pagos Adcionales para Contratos</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div id="Navegacion" style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" NavigateUrl="~/Facturacion.AplicacionesFacturacion.UI/GenerarPagosAdicionalesUI.aspx"
                        runat="server" CausesValidation="False">
                        <span>GENERAR PAGO</span>
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas" style="width: 835px; float: right;">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlFormulario" CssClass="GroupSection GroupSectionFix" runat="server">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>
                            Generador Pagos Adicionales
                        </td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <asp:UpdatePanel ID="udpGenerarPagos" runat="server">
                    <ContentTemplate>
                        <table style="margin: 20px auto; width: 500px;">
                            <tr>
                                <td class="alinearDerecha">
                                    <span>*</span><lable>Sucursal</lable>
                                </td>
                                <td class="separador">
                                </td>
                                <td class="alinearIzquierda">
                                    <asp:DropDownList runat="server" ID="ddlSucursal" DataValueField="Value" DataTextField="Text" ValidationGroup="Requeridos">
                                    </asp:DropDownList>
                                    <asp:RangeValidator runat="server" ID="rgvSucursal" ControlToValidate="ddlSucursal" Type="Integer"
                                        MinimumValue="0" MaximumValue="100" ValidationGroup="Requeridos" Display="Dynamic"
                                        ErrorMessage="La sucursal es un dato requerido" Text="*" SetFocusOnError="True"
                                        ToolTip="La sucursal es un dato requerido" />
                                </td>
                            </tr>
                            <tr>
                                <td class="alinearDerecha">
                                    <span>*</span><lable>Departamento</lable>
                                </td>
                                <td class="separador">
                                </td>
                                <td class="alinearIzquierda">
                                    <asp:DropDownList runat="server" ID="ddlDepartamento" DataValueField="Value" DataTextField="Text" ValidationGroup="Requeridos">
                                    </asp:DropDownList>
                                    <asp:RangeValidator runat="server" ID="rgvDepartamento" ControlToValidate="ddlDepartamento"  Type="Integer"
                                        MinimumValue="0" MaximumValue="3" ValidationGroup="Requeridos" Display="Dynamic"
                                        ErrorMessage="El Departamento es un dato requerido" Text="*" SetFocusOnError="True"
                                        ToolTip="El Departamento es un dato requerido" />
                                </td>
                            </tr>
                            <tr>
                                <td class="alinearDerecha">
                                    <span>*</span><lable>Número Contrato</lable>
                                </td>
                                <td class="separador">
                                </td>
                                <td class="alinearIzquierda">
                                    <asp:TextBox runat="server" ID="txtNumeroContrato" Width="50%" MaxLength="25" ValidationGroup="Requeridos"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvNumeroContrato" ControlToValidate="txtNumeroContrato"
                                        ValidationGroup="Requeridos" Display="Dynamic" ErrorMessage="El Número de Contrato es un dato requerido"
                                        Text="*" SetFocusOnError="True" ToolTip="El Número de Contrato es un dato requerido" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical" colspan="3">
                                    <asp:Button runat="server" ID="btnAceptar" ValidationGroup="Requeridos" Text="Aceptar" 
                                    CssClass="btnComando" OnClientClick=" if (Page_ClientValidate('Requeridos')) { __blockUI(); }" 
                                    style=" margin: 20px auto 0 auto; display: inherit;" onclick="btnAceptar_Click"/>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="hdnContratoID" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
