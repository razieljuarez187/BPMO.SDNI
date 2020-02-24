<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarConfiguracionSistemaUnidadUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.RegistrarConfiguracionSistemaUnidadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { width: 750px; margin: 0 auto;}
        #divInformacionGeneralControles {padding: 1em;}
        #divInformacionGeneralControles table { margin: 20px auto; }
        .td-finder { float: left; white-space: nowrap;}
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
            ConfiguracionBarraHerramientas();
        }
        
        </script>
        
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

   <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
			<asp:Label ID="lblEncabezadoLeyenda" runat="server">REGISTRAR CONFIGURACI&Oacute;N DE SISTEMA DE UNIDAD</asp:Label>
		</div>
        <!--Navegación secundaria-->
		<div style="height: 80px;">
			<!-- Menú secundario -->
			<ul id="MenuSecundario" class="menuCompuesto">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarConfiguracionSistemaUnidadUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistarConfiguracion" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarConfiguracionSistemaUnidadUI.aspx">
                        REGISTRAR 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas-->
			<div id="BarraHerramientas" style="width:100%">
				<asp:Menu runat="server" ID="mConfiguracion" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario" OnMenuItemClick="mConfiguracion_MenuItemClick">
                    <Items>
                        <asp:MenuItem Text="Editar" Value="EditarConfiguracion"></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarConfiguracion"></asp:MenuItem>
                    </Items>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnHelp" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
				<input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Mantenimiento.UI/ConsultarConfiguracionSistemaUI.aspx") %>'" />
            </div>
        </div>
        <!-- Cuerpo -->
        <div id="divInformacionGeneral" class="GroupBody" >
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>REGISTRAR CONFIGURACI&Oacute;N DE SISTEMA DE UNIDAD</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
					<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnFinalizar_Click" />
			    </div>
            </div>
            <div id="divInformacionGeneralControles">
                 <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>CLAVE</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtClave" runat="server" MaxLength="3" CssClass="input-find-responsive"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>NOMBRE DEL SISTEMA DE UNIDAD</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="30" CssClass="input-find-responsive"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"></td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:Button ID="btnAgregarATabla" runat="server" Text="Agregar a Tabla" CssClass="btnAgregarATabla" 
                                OnClick="AgregarATabla_Click"/>
                        </td>
                    </tr>
                </table>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
                <br />
                <asp:UpdatePanel ID="UPContenedor" runat="server">
                    <ContentTemplate>
                        <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                            <asp:GridView ID="gvConfiguraciones" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                                AllowPaging="true" PageSize="10" EnableViewState="False" OnRowCommand="gvConfiguraciones_RowCommand"
                                OnPageIndexChanging="gvConfiguraciones_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                <Columns>                   
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="CLAVE">
                                        <ItemTemplate>
                                                <asp:Label ID="lbNombre" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Clave") %>' Width="229px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left"/>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="NOMBRE DEL SISTEMA DE UNIDAD">
                                        <ItemTemplate>
                                                <asp:Label ID="lbEstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Nombre") %>' Width="229px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left"/>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
					                <asp:TemplateField ItemStyle-Width="58px" HeaderText="&nbsp;Eliminar&nbsp; ">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnEliminar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="" ImageAlign="Middle" />
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
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
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnLibroActivos" runat="server" />
</asp:Content>
