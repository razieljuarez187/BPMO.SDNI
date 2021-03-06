﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarConfiguracionParametroMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ConsultarConfiguracionParametroMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table.table-responsive { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
        .input-find-responsive { min-width: 100px !important;}
        .input-dropdown-responsive { min-width: 156px !important;}
        .Grid { border: none; margin: 0 auto; text-transform:uppercase}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; padding: 0em 1em 0em 1em; width: 20px; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .GridPager td {padding: 0 !important; width: 0px;}
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/bootstrap-1.8.2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/mantenimiento-responsive.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initScript();
        });

        function initScript() {
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
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

        <script type="text/javascript">
            function BtnBuscar(guid, xml) {
                var width = ObtenerAnchoBuscador(xml);

                $.BuscadorWeb({
                    xml: xml,
                    guid: guid,
                    btnSender: $("#<%=btnResult.ClientID %>"),
                    features: {
                        dialogWidth: width,
                        dialogHeight: '320px',
                        center: 'yes',
                        maximize: '0',
                        minimize: 'no'
                    }
                });
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div>
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="Label1" runat="server">MANTENIMIENTO - CONSULTAR CONFIGURACI&Oacute;N PAR&Aacute;METROS MANTENIMIENTO</asp:Label>
        </div>


        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarMantenimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarConfiguracionParametroMantenimientoUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistrarMantenimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarConfiguracionParametroMantenimientoUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>     
        </div>
        <!-- Cuerpo -->
        <div id="Div1" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>CONSULTAR CONFIGURACI&Oacute;N PAR&Aacute;METROS MANTENIMIENTO</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">MODELO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtModelo" runat="server" 
                                CssClass="input-find-responsive" AutoPostBack="True" 
                                ontextchanged="txtModelo_TextChanged" ></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarModelo" CommandName="VerModelo"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelos" CommandArgument='' OnClick="btnBuscarModelo_Click" />
                            <asp:HiddenField ID="hdnModeloID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">TIPO DE MANTENIMIENTO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTipoMantenimiento" runat="server" 
                                CssClass="input-dropdown-responsive">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">UNIDAD DE MEDIDA</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddUnidadMedida" runat="server" 
                                CssClass="input-dropdown-responsive">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">Estatus</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddlEstatus" runat="server" Width="155px">
                                <asp:ListItem Value="" Text="Todos" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

                             <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="OnClickBuscarConfiguraciones" CssClass="btnComando" />

                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
            <div style="margin: 0 auto; width:95% !important; overflow: auto;">
                <asp:GridView ID="gvConfiguraciones" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" OnRowCommand="gvConfiguraciones_RowCommand"
                    OnPageIndexChanging="gvConfiguraciones_PageIndexChanging" 
                    onrowdatabound="gvConfiguraciones_RowDataBound">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="MODELO">
                            <ItemTemplate>
                                    <asp:Label ID="lbModelo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %>' Width="120px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="ESTADO">
                            <ItemTemplate>
                                    <asp:Label ID="lbEnUso" runat="server" Text='' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="TIPO MANTENIMIENTO">
                            <ItemTemplate>
                                    <asp:Label ID="lbTipoMantenimiento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimiento") %>' Width="150px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="UNIDAD MEDIDA">
                            <ItemTemplate>
                                    <asp:Label ID="lbParametro" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UnidadMedida") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                         <asp:TemplateField ItemStyle-Width="100px" HeaderText="INTERVALO">
                            <ItemTemplate>
                                    <asp:Label ID="lbIntervalo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Intervalo") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="ESTATUS">
                            <ItemTemplate>
                                    <asp:Label ID="lbEstatus" runat="server" Text='' Width="55px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
					    <asp:TemplateField ItemStyle-Width="58px" HeaderText="&nbsp;VER&nbsp;&nbsp;">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Ver" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="" ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
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
            <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
            <asp:HiddenField ID="hdnLibroActivos" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
