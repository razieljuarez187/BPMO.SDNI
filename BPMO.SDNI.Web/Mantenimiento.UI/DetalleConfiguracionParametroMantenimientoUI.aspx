<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleConfiguracionParametroMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.DetalleConfiguracionParametroMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         #ContenedorMenuSecundario,GroupSection { display: inline-block}
        .GroupSection { width: 400px; margin: 0 auto;}
        .GroupBody {max-width: 700px;}
        #divInformacionGeneral {margin: 0 auto;}
        #divInformacionGeneralControles {padding: 1em;}
        #divInformacionGeneralControles table { margin: 20px auto; }
        .input-find-responsive { min-width: 100px !important;}
        .input-dropdown-responsive { min-width: 156px !important;}
        .Grid { border: none; margin: 0 auto;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; padding: 0em 1em 0em 1em; width: 20px; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        #Parametros {width: 70%; margin: 0 auto;}
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
          }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="PaginaContenido">
    <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - DETALLE CONFIGURACIÓN MANTENIMIENTO</asp:Label>
        </div>

         <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarConfiguracionParametroMantenimientoUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarConfiguracionParametroMantenimientoUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>

            <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="float: right;">
                <asp:Menu runat="server" ID="mnuConfiguracionMantenimiento" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" OnMenuItemClick="menuSelecionado_MenuItemClick">
                    <Items>                     
                        <asp:MenuItem Text="Editar" Value="Editar" Selected="true"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarParametro"></asp:MenuItem>
                        <asp:MenuItem Text="Reactivar" Value="Reactivar" Enabled="False"></asp:MenuItem>
                    </Items>
                    <LevelSubMenuStyles><asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" /> </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado"/>
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                        <input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>                
            </div>
           
        </div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
             <span>DETALLE DE PARÁMETROS DE CONFIGURACIÓN</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">           
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditar_Click"/> 
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  CssClass="btnWizardCancelar" onclick="btnCancelar_Click"/>         
                </div>
            </div>
            <div id="divInformacionGeneralControles">

                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <asp:HiddenField ID="hdnConfiguracionID" runat="server" Visible="False" />
                        <td class="tdCentradoVertical input-label-responsive">MODELO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtModelo" runat="server" MaxLength="30" 
                                CssClass="input-find-responsive" Enabled="False"></asp:TextBox>
                            <asp:HiddenField ID="hdnModeloID" runat="server" Visible="False" />
                        </td>
                    </tr>
                </table>
            <fieldset id="Parametros">
            <legend>Parámetros</legend>
            <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">ESTADO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                             <asp:DropDownList ID="ddEstado" runat="server" 
                                CssClass="input-dropdown-responsive" AutoPostBack="True" Enabled="False">
                                 <asp:ListItem>ESTACIONADO</asp:ListItem>
                                 <asp:ListItem Selected="True">EN USO</asp:ListItem>
                            </asp:DropDownList>
                           <%-- <asp:CheckBox ID="chbxEnUso" runat="server" Enabled="False" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">TIPO DE MANTENIMIENTO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTipoMantenimiento" runat="server" 
                                CssClass="input-dropdown-responsive" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">UNIDAD DE MEDIDA</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddUnidadMedida" runat="server" 
                                CssClass="input-dropdown-responsive" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">INTERVALO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtIntervalo" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <asp:HiddenField ID="hdnEstatus" runat="server" Visible="False" />
                       
                </table>
                </fieldset>
                <br />
     <fieldset>
     <legend>Parámetros configurados para el modelo</legend>
     <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
            <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                <asp:GridView ID="gvConfiguraciones" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" OnRowCommand="gvConfiguraciones_RowCommand"
                    OnPageIndexChanging="gvConfiguraciones_PageIndexChanging" 
                    onselectedindexchanged="gvConfiguraciones_SelectedIndexChanged" 
                     onrowdatabound="gvConfiguraciones_RowDataBound"
                    ShowHeaderWhenEmpty="True">
                    <Columns>                   
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="TIPO MANTENIMIENTO">
                            <ItemTemplate>
                                    <asp:Label ID="lbTipoMantenimiento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimiento") %>' Width="150px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="ESTADO">
                            <ItemTemplate>
                                    <asp:Label ID="lbEstaUso" runat="server" Text='' Width="100px"></asp:Label>
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
                        <asp:TemplateField ItemStyle-Width="58px" HeaderText="&nbsp;Editar&nbsp;">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEditar" CommandName="Editar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png" ToolTip="" ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                     <EmptyDataTemplate>
                       <b id="GridVacio">No se han agregado configuraciones</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </fieldset>
    <br />
    
               
                <div class="ContenedorMensajes">
                                   
                </div>

            </div>


       
        </div>


          <!-- Cuerpo -->
</div>


</asp:Content>
