<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="VerPagosContratoUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesPago.UI.VerPagosContratoUI" %>
<%-- Satisface a la Solicitud de Cambios SC0008--%>
<%-- Satisface al caso de uso CU038 - Ver Pagos de Contrato--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <style type="text/css">
        #BarraHerramientas { width: 832px !important;}          
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
    </style>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">       
    <div id="PaginaContenido">
        <!-- Barra de localizacion -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">Pagos de Contrato</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <a href="VerPagosContratoUI.aspx">
                        Consultar
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/> 
                    </a>
                </li>  
            </ul>            
        </div>
        
        <div id="Formulario" class = "GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>Contrato a Consultar</td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical"><label for='<%= txtFolioContrato.ClientID %>'># Contrato</label></td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtFolioContrato" MaxLength="50" Width="275px" ValidationGroup="validate"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="revFolioContrato" ControlToValidate="txtFolioContrato" ValidationGroup="validate" ErrorMessage="**" EnableClientScript="false"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFolioContrato" ValidationGroup="validate" ControlToValidate="txtFolioContrato" ErrorMessage="*" EnableClientScript="false"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnVerPagos"  onclick="btnVerPagos_Click" Text="Ver" CssClass="btnComando" ValidationGroup="validate" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
