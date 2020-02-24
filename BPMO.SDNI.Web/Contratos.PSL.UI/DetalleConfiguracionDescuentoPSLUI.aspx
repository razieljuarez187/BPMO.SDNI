<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="DetalleConfiguracionDescuentoPSLUI.aspx.cs"
    Inherits="BPMO.SDNI.Contratos.PSL.UI.DetalleConfiguracionDescuentoPSLUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>"
        type="text/javascript"></script>
    <style type="text/css">
        .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
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
        #divInformacionGeneralControles table
        {
            margin: 0;
        }
        #divInformacionGeneralControles .dvCentro
        {
            text-align: center;
            width: 65%;
        }
        #divInformacionGeneralControles .dvCentro .trAlinearDerecha
        {
            margin: 0px 0px 0px auto;
        }
        #divInformacionGeneralControles .dvCentro .trAlinearDerecha .tdCentradoVertical input
        {
            margin: 0px 5px 0px 15px !important;
            display: inline !important;
            vertical-align: middle !important;
        }
        
        .btnWizardEditar 
        {
            width: 115px !important;
        }
    </style>
    <style type="text/css">
        .GroupSection
        {
            width: 650px;
            margin: 0px auto;
        }
        .GroupContentCollapsable table
        {
            margin: 20px auto;
            width: 506px;
        }
        .GroupContentCollapsable .btnComando
        {
            margin: 20px auto 0px auto;
            display: inherit;
        }
        .Grid
        {
            width: 90%;
            margin: 25px auto 15px auto;
        }
        .GridTabla
        {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - DETALLE CONFIGURACIÓN DESCUENTO </asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            
            
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarDescuentosConfiguracion" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarConfiguracionDescuentoPSLUI.aspx" ForeColor="White"> 
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkConsultarConfiguracionDescuentos" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarConfiguracionDescuentoPSLUI.aspx"> 
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionHeader" class="GroupHeader">
                <span>DETALLE DEL DESCUENTO</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btnWizardRegresar"
                                    OnClick="btnRegresar_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnEditar" Text="Editar" CssClass="btnWizardEditar"
                                    OnClick="btnEditar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divInformacionGeneralControles">
                <fieldset >
                    <legend>Catálogos</legend>
                    <div style="margin-left:auto; margin-right:auto; width:95%" class="GridTabla">
                    <div class="dvIzquierda">
                        <table class="trAlinearIzquierda">
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span></span>Cliente
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtCliente" runat="server" Width="202px" AutoPostBack="True"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnClienteID" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="dvDerecha">
                        <table class="trAlinearIzquierda">
                            <tr>
                                <td class="tdCentradoVertical">
                                    <span></span>Contacto Comercial
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 330px;">
                                    <asp:TextBox ID="txtContactoComercial" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    </div>
                    
                </fieldset>
                <fieldset style="padding-top: 10px; padding-bottom: 10px;" class="GridTabla">
                    <legend>DESCUENTOS POR SUCURSAL </legend>
                    <div style="margin-left:auto; margin-right:auto; width:95%;">
                    <asp:UpdatePanel ID="UPContenedor" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="grvConfiguracionDescuentos" AutoGenerateColumns="false"
                                PageSize="10" AllowPaging="true" AllowSorting="false" EnableSortingAndPagingCallbacks="true"
                                CssClass="Grid" OnPageIndexChanging="grvConfiguracionDescuentos_PageIndexChanging"
                                OnRowDataBound="grvConfiguracionDescuentos_RowDataBound" Width="850px">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sucursal</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Fecha Inicio</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFechaInicio" Text='<%# DataBinder.Eval(Container.DataItem,"FechaInicio") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Fecha Fin</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFechaFin" Text='<%# DataBinder.Eval(Container.DataItem,"FechaFin") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Máximo Descuento</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMaximoDescuento" Text='<%# DataBinder.Eval(Container.DataItem,"DescuentoMaximo") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Activo</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="ChkActivo" disabled Width="100%"></asp:CheckBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                </fieldset>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hdnModeloID" />
        <asp:HiddenField runat="server" ID="hdnSucursalID" />
        <asp:HiddenField runat="server" ID="hdnConfiguracionDescuentoID" />
        <asp:HiddenField runat="server" ID="hdnFechaInicio" />
        <asp:HiddenField runat="server" ID="hdnFechaFin" />
        <asp:HiddenField runat="server" ID="hdnDescuentoMaximo" />
    </div>
</asp:Content>
