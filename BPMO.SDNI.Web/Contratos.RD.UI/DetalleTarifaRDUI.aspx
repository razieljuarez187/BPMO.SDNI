<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="DetalleTarifaRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.DetalleTarifaRDUI" %>

<%@ Register Src="~/Contratos.RD.UI/ucTarifaRDUI.ascx" TagPrefix="uc" TagName="ucTarifaRDUI" %>
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
            <asp:Label runat="server" ID="lblEncabezadoLeyenda">CAT&Aacute;LOGOS - CONSULTAR DETALLES DE TARIFA</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 65px;">
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink runat="server" ID="hlConsultar" NavigateUrl="~/Contratos.RD.UI/ConsultarTarifasRDUI.aspx">
                        CONSULTAR 
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hlRegistrar" NavigateUrl="RegistrarTarifaRDUI.aspx">
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
                <span>TARIFA</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button runat="server" ID="btnEditar" Text="Editar" OnClick="btnEditar_Click" CssClass="btnWizardEditar" />
                    <asp:Button ID="btnRegresar" runat="server" Text="REGRESAR" CssClass="btnWizardRegresar" onclick="btnRegresar_Click" />
                </div>
            </div>
            <fieldset id="fsDatosGenerales">
                <legend>Datos Generales de la tarifa</legend>
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">Sucursal</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtSucursal" Enabled="False" Width="250px"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnSucursalID" />
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Moneda</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtMoneda" Enabled="False"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnCodigoMoneda" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">Modelo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox runat="server" ID="txtModelo" Width="250px" Enabled="False"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnModeloID" />
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Tipo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtTipo" Enabled="False"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnTipo" />
                        </td>
                    </tr>                    
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">Descripci&oacute;n</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;" colspan="4">
                            <asp:TextBox runat="server" ID="txtDescripcion" MaxLength="150" Width="250px" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>                    
                    <asp:Panel runat="server" ID="pnlDatosCliente">
                        <tr>
                            <td class="tdCentradoVertical" style="width: 150px;">
                                <span>*</span>Cliente
                            </td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 320px;">
                                <asp:TextBox runat="server" ID="txtCliente" Width="250px" Enabled="False"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnClienteID" />
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                <span>*</span>Vigencia
                            </td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" style="width: 280px;">
                                <asp:TextBox runat="server" ID="txtVigencia" CssClass="CampoFecha" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px">
                                <span>*</span>Observaciones
                            </td>
                            <td style="width: 20px;">&nbsp;</td>
                            <td class="tdCentradoVertical" colspan="4">
                                <asp:TextBox runat="server" ID="txtObservaciones" Width="250px" TextMode="MultiLine" 
                                    style="max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </fieldset>
            <fieldset id="fsTarifas">
                <legend>Configuración de tarifas</legend>
                <uc:ucTarifaRDUI runat="server" ID="ucTarifaRD"/>
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
    <asp:HiddenField runat="server" ID="hdnTarifaID" />
</asp:Content>
