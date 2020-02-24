<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="DetalleTarifaPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.DetalleTarifaPSLUI" %>

<%@ Register Src="~/Contratos.PSL.UI/ucTarifaPSLUI.ascx" TagPrefix="uc" TagName="ucTarifaPSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloCatTarifas.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" id="JQuerySection">
        $(document).ready(function () {
            initChild();
            ConfiguracionBarraHerramientas();
        });

        function initChild() {
            $("span:contains('*')").text("");
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label runat="server" ID="lblEncabezadoLeyenda">CAT&Aacute;LOGOS - CONSULTAR DETALLES DE LISTA DE PRECIOS</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 65px;">
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink runat="server" ID="hlConsultar" NavigateUrl="~/Contratos.PSL.UI/ConsultarTarifaPSLUI.aspx">
                        CONSULTAR 
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hlRegistrar" NavigateUrl="RegistrarTarifaPSLUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de Herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mTarifa" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" OnMenuItemClick="mTarifa_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Precio Combustible" Value="PrecioCombustible" Enabled="False" Selectable="false"> </asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="Editar"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "PrecioCombustible" ? "Informacion" : string.Empty %>'
                            Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "PrecioCombustible" %>' Style="width: 150px" CssClass="textBoxDisabled" ReadOnly="true">
                            
                        </asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionHeader" class="GroupHeader">
                <span>LISTA DE PRECIOS</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button runat="server" ID="btnEditar" Text="Editar" OnClick="btnEditar_Click" CssClass="btnWizardEditar" />
                    <asp:Button ID="btnRegresar" runat="server" Text="REGRESAR" CssClass="btnWizardRegresar" onclick="btnRegresar_Click" />
                </div>
            </div>
            <fieldset id="fsDatosGenerales">
                <legend>Datos Generales de la LISTA DE PRECIOS</legend>
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">Sucursal</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtSucursal" Enabled="False" Width="250px"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnSucursalID" />
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Turno</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtTarifaTurno" Enabled="False"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnTarifaTurno" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">Modelo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtModelo" Width="250px" Enabled="False"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnModeloID" />
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Moneda</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtMoneda" Enabled="False"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnCodigoMoneda" />
                        </td>
                    </tr>                    
                        <tr>
                            <td class="tdCentradoVertical" style="width: 150px;">Per&iacute;odo</td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 320px;">
                                <asp:TextBox runat="server" ID="txtPeriodoTarifa" MaxLength="150" Width="250px" Enabled="False"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnPeriodoTarifa" />
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">Tipo Tarifa</td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 280px;">
                                <asp:TextBox runat="server" ID="txtTipo" Enabled="False"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnTipo" />
                            </td>
                        </tr>                    
                </table>
            </fieldset>
            <fieldset id="fsTarifas">
                <legend>Configuración de tarifas</legend>
                <uc:ucTarifaPSLUI runat="server" ID="ucTarifaPSL"/>
            </fieldset>
            <fieldset id="fsDatosSistema">
                <legend>Datos del Sistema</legend>
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 180px;">Fecha Registro</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 200px;">
                            <asp:TextBox runat="server" ID="txtFechaRegistro" Enabled="False" CssClass="CampoFecha" Width="180px"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Usuario Registro</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtUsuarioRegistro" Enabled="False" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 180px;">Fecha Modificación</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 200px;">
                            <asp:TextBox runat="server" ID="txtFechaModificacion" Enabled="False" CssClass="CampoFecha" Width="180px"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Usuario Modificación</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtUsuarioModificacion" Enabled="False" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 180px;">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" colspan="4">
                            <asp:TextBox runat="server" ID="txtEstatus" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnTarifaPSLID" />
</asp:Content>
