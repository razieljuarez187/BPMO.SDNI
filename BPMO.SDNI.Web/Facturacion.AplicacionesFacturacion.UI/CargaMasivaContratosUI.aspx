<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="CargaMasivaContratosUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI.CargaMasivaContratosUI" %>
<%--Satisface el caso de uso CU015 – Carga Masiva Facturación Contratos--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>  
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

        function BtnBuscar(guid, xml, senderId) {
            var width = ObtenerAnchoBuscador(xml);
            var height = "320px";

            if (!senderId)
                senderId = '<%= this.btnResult.ClientID %>';

            var sender = $("#" + senderId);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: sender,
                features: {
                    dialogWidth: width,
                    dialogHeight: height,
                    center: 'yes',
                    maximize: '0',
                    minimize: 'no'
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CONTRATOS - Carga Masiva Facturación Contratos</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div id="Navegacion" style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">                
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" NavigateUrl="~/Facturacion.AplicacionesFacturacion.UI/CargaMasivaContratosUI.aspx" runat="server"
                        CausesValidation="False">
                        CARGAR ARCHIVO
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
                        <td>Carga Masiva</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <asp:UpdatePanel ID="upCargaArchivo" runat="server">
                    <ContentTemplate>
                     <table class="trAlinearDerecha" style="margin: 20px auto; width: 506px;">
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span><label for="txtSucursal">Sucursal</label></td>
                        <td class="separadorCampo"></td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox ID="txtSucursal" runat="server" Width="275px" MaxLength="80" ValidationGroup="ActualizarRegistro" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                                OnClick="ibtnBuscarSucursal_Click" CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ActualizarRegistro" 
                                ErrorMessage="La sucursal es un dato requerido" Text="*" 
                                ControlToValidate="txtSucursal" Display="Dynamic" SetFocusOnError="True" 
                                ToolTip="La sucursal es un dato requerido"></asp:RequiredFieldValidator>           
                            <asp:RequiredFieldValidator ID="rfvtxtSucursal" runat="server" ValidationGroup="CargarArchivo" 
                                ErrorMessage="La sucursal es un dato requerido" Text="*" 
                                ControlToValidate="txtSucursal" Display="Dynamic" SetFocusOnError="True" 
                                ToolTip="La sucursal es un dato requerido"></asp:RequiredFieldValidator>
                                 <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />                                 
                        </td>
                    </tr>
                     <tr>
                        <td class="tdCentradoVertical"><span>*</span><label for="ddlDepartamento">Área/Departamento</label></td>
                        <td class="separadorCampo"></td>
                        <td class="tdCentradoVertical">
                            <asp:DropDownList ID="ddlDepartamento" DataValueField="Value" DataTextField="Text" runat="server">                                
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                         <td class="tdCentradoVertical"><span>*</span><label for="fuArchivo">Cargar Archivo:</label></td>
                         <td class="separadorCampo"></td>
                         <td class="tdCentradoVertical">
                             <asp:FileUpload ID="fuArchivo" runat="server" />
                             <asp:RequiredFieldValidator ID="rfvfuArchivo" runat="server" ValidationGroup="CargarArchivo" 
                                ErrorMessage="El archivo de carga es un dato requerido" Text="*" 
                                ControlToValidate="fuArchivo" Display="Dynamic" SetFocusOnError="True" 
                                ToolTip="El archivo de carga es un dato requerido"></asp:RequiredFieldValidator>
                             <asp:CustomValidator ID="cmvfuArchivo" runat="server" 
                                 ErrorMessage="El archivo proporcionado no tiene el formato correcto" 
                                 ControlToValidate="fuArchivo" onservervalidate="cmvfuArchivo_ServerValidate"
                                 Display="Dynamic" SetFocusOnError="True"  ValidationGroup="CargarArchivo"
                                 Text="**" ToolTip="El archivo proporcionado no tiene el formato correcto">
                             </asp:CustomValidator>                           
                         </td>
                    </tr>
                </table>
                    <asp:Button runat="server" ID="btnAceptar" ValidationGroup="CargarArchivo"  onclick="btnAceptar_Click" Text="Aceptar" CssClass="btnComando" style=" margin: 20px auto 0 auto; display: inherit;" OnClientClick=" if (Page_ClientValidate('CargarArchivo')) { __blockUI(); }" />
                    
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAceptar" />
                    </Triggers>
                </asp:UpdatePanel>         

                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>                          
            </div>
        </asp:Panel>

        <asp:GridView runat="server" ID="grdEventos" AutoGenerateColumns="False" Visible="false" AllowPaging="True" 
                    EnableSortingAndPagingCallbacks="True" CssClass="Grid97Percent" OnPageIndexChanging="grdEventos_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>Número Contrato</HeaderTemplate>
                            <ItemTemplate><span><%# DataBinder.Eval(Container.DataItem, "NumeroContrato")%></span></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Módelo Unidad</HeaderTemplate>
                            <ItemTemplate><span><%# DataBinder.Eval(Container.DataItem, "ModeloUnidad")%></span></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>VIN</HeaderTemplate>
                            <ItemTemplate><span><%# DataBinder.Eval(Container.DataItem, "VIN")%></span></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Número Factura</HeaderTemplate>
                            <ItemTemplate><span><%# DataBinder.Eval(Container.DataItem, "NoFactura")%></span></ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderTemplate>No. Renta Factura</HeaderTemplate>
                            <ItemTemplate><span><%# DataBinder.Eval(Container.DataItem, "NoRentaFactura")%></span></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Detalle</HeaderTemplate>
                            <ItemTemplate>
                                <span title='<%# DataBinder.Eval(Container.DataItem, "Detalle") %>'><pre style="white-space:pre-wrap"><%# DataBinder.Eval(Container.DataItem, "Detalle") %></pre></span>                   
                            </ItemTemplate>
                        </asp:TemplateField>
                                            
                    </Columns>                    
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>
    </div>    

    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" CausesValidation="false" UseSubmitBehavior="false" Style="display: none;" />
</asp:Content>
