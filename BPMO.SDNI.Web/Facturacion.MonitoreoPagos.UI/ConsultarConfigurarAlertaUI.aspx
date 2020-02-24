<%@ Page Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarConfigurarAlertaUI.aspx.cs" Inherits="BPMO.SDNI.Facturacion.MonitoreoPagos.UI.ConsultarConfigurarAlertaUI" %>

<%--
Satisface el caso de uso CU009 – Configuración Notificación de facturación
Satisface la solicitud de cambio SC0008
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .Grid { width: 90%; margin: 25px auto 15px auto; }
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
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CONFIGURACIÓN DE NOTIFICACIÓN DE ALERTA - CONSULTAR EMPLEADO PARA RECIBIR NOTIFICACIÓN</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Facturacion.MonitoreoPagos.UI/ConsultarConfigurarAlertaUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlkRegistrar" runat="server" NavigateUrl="~/Facturacion.MonitoreoPagos.UI/RegistrarConfigurarAlertaUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/></asp:HyperLink>
                </li>
            </ul>
        </div>

        <!--Filtros de la consulta-->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿QUÉ CONFIGURACIÓN DE NOTIFICACIÓN DE ALERTA DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                     <tr>
        <td class="tdCentradoVertical">
            SUCURSAL
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="style1">
            <asp:TextBox ID="txtSucursal" runat="server" Width="275px" MaxLength="80" AutoPostBack="true" OnTextChanged="txtSucursal_TextChanged">
            </asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument=''
                OnClick="ibtnBuscarSucursal_Click" CausesValidation="False" />
            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
            <asp:HiddenField ID="hdnClaveSucursal" runat="server" Visible="False" />
         
        </td>
    </tr>
    <tr>
         <td class="tdCentradoVertical">
            EMPLEADO
        </td>
        <td style="width: 20px;">&nbsp;</td>
        <td class="style1">
            <asp:TextBox ID="txtEmpleado" runat="server" MaxLength="30" Width="275px"
                AutoPostBack="True" OnTextChanged="txtNombreEmpleado_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="ibtnBuscarEmpleado" CommandName="VerEmpleado"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Empleados" CommandArgument=''
                OnClick="ibtnBuscarEmpleado_Click" CausesValidation="False" />
            
            <asp:HiddenField ID="hdnEmpleadoID" runat="server" Visible="False" />
        </td>
    </tr>     
                    <tr>
                        <td class="tdCentradoVertical">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList runat="server" ID="ddlEstatus"  Width="150px">                                
                                <asp:ListItem Text="ACTIVO" Value="True" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="INACTIVO" Value="False"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar"  OnClick="btnBuscar_Click" CssClass="btnComando" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <%--SC0008 - Actualización de datos del grid para soporte de configuraciones de facturistas--%>
                <asp:GridView runat="server" ID="grdConfiguracionesAlerta" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EnableSortingAndPagingCallbacks="True" CssClass="Grid" OnPageIndexChanging="grdConfiguracionesAlerta_PageIndexChanging"
                    OnRowCommand="grdConfiguracionesAlerta_RowCommand" 
                    onrowdatabound="grdConfiguracionesAlerta_RowDataBound">
                    <Columns>
                        <asp:TemplateField>                          
                            <ItemTemplate>
                                <asp:Image ID="imgTipoConfiguracion" ImageAlign="AbsMiddle"  ImageUrl="~/Contenido/Imagenes/ico.usuario.png" ToolTip="Configuración Manual" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Sucursal</HeaderTemplate>
                            <ItemTemplate>                                
								<asp:Label ID="lblSucursalNombre" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Sucursal.Nombre") %>'></asp:Label>
                                <asp:HiddenField ID="hdfSucursalID" Value='<%# DataBinder.Eval(Container.DataItem, "Sucursal.Id") %>' runat="server" />
                                <asp:HiddenField ID="hdfPerfil" Value='<%# Container.DataItem is BPMO.SDNI.Facturacion.MonitoreoPagos.BO.ConfiguracionAlertaPerfilBO ? DataBinder.Eval(Container.DataItem, "Perfil.Id") : "" %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Nombre del Empleado</HeaderTemplate>
                            <ItemTemplate>                                
								<asp:Label ID="lblEmpleadoNombreCompleto" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Empleado.NombreCompleto") %>'></asp:Label>
                                <asp:HiddenField ID="hdfEmpleadoID" Value='<%# DataBinder.Eval(Container.DataItem, "Empleado.Id") %>' runat="server" />
                             </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>Correo Electrónico</HeaderTemplate>
                            <ItemTemplate>                                
                                <asp:Label ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Empleado.Email")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Número de Días para Recibir Notificación">                            
                            <ItemTemplate>
                                <asp:Label ID="lblNumeroDias" runat="server" Text='<%# Bind("NumeroDias") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" />
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" CausesValidation="false" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver detalles" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ConfiguracionAlertaID") %>'
                                    ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="GridRow"/>                    
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" CausesValidation="false" UseSubmitBehavior="false" Style="display: none;" />
</asp:Content>
