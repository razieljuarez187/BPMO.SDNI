﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarConfiguracionParametrosAuditoriaUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.EditarConfiguracionParametrosAuditoriaUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ContenedorMenuSecundario,GroupSection { display: inline-block}
        .GroupSection { width: 1000px; margin: 0 auto;}
        .GroupBody {max-width: 1000px;}
        #divInformacionGeneral {margin: 0 auto;}
        #divInformacionGeneralControles {padding: 1em;}
        #divInformacionGeneralControles table { margin: 20px auto; }
        .input-find-responsive { min-width: 100px !important;}
        .input-dropdown-responsive { min-width: 156px !important;}
        .input-text-responsive {min-width: 20px; }
        .Grid { border: none; margin: 0 auto;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; padding: 0em 1em 0em 1em; width: 20px; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        .Obligatoria {margin-left: 25px;}
        .GridPager td {padding: 0 !important; width: 0px; margin: 0px !important;}
        .GridPager td table {margin: 0px !important;}
     
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

         <script type="text/javascript">
             function confirmarEliminacion(origen) {
                 var $div = $('<div title="Confirmación"></div>');
                 $div.append('¿Esta seguro que desea eliminar la Configuracion de esta Auditoria?');
                 $("#dialog:ui-dialog").dialog("destroy");
                 $($div).dialog({
                     closeOnEscape: true,
                     modal: true,
                     minWidth: 460,
                     close: function () { $(this).dialog("destroy"); },
                     buttons: {
                         Aceptar: function () {
                             $(this).dialog("close");
                             __doPostBack("<%= btnEliminar.UniqueID %>", "");
                             
                         },
                         Cancelar: function () {
                             $(this).dialog("close");
                         }
                     }
                 });
             }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="PaginaContenido">
 <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - EDITAR CONFIGURACIÓN AUDITORÍA</asp:Label>
        </div>

         <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarConfiguracionParametrosAuditoriaUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarConfiguracionParametrosAuditoriaUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas" style="float: right;">
                <asp:Menu runat="server" ID="mnuConfiguracionMantenimiento" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" OnMenuItemClick="menuSelecionado_MenuItemClick">
                    <Items>
                       <%-- <asp:MenuItem Text="Parametro Configuracion ID" Value="ConfiguracionAuditoriaID" Enabled="False" Selectable="false"></asp:MenuItem>--%>
                        <asp:MenuItem Text="Editar" Value="Editar" Selected="true" NavigateUrl="#" ></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarParametro" ></asp:MenuItem>
                    </Items>
                   <%-- <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "ConfiguracionAuditoriaID" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "ConfiguracionAuditoriaID" %>' Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>--%>
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
             <span>EDITAR CONFIGURACIONES AUDITORÍA</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">           
                    <asp:Button ID="btnRegistrar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnEditar_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CssClass="btnWizardCancelar" onclick="btnRegresar_Click"/>
                  <%--  <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                        CssClass="btnWizardBorrador" onclick="btnEliminar_Click"/>--%>
                </div>
             </div>

            <div id="divInformacionGeneralControles">
             
             
                <table class="trAlinearDerecha table-responsive"> 
                      <asp:HiddenField ID="hdnConfiguracionID" runat="server" Visible="False" />
                     <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" 
                                CssClass="input-text-responsive" Enabled="False"></asp:TextBox>
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>TALLER</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtTaller" runat="server" MaxLength="30" CssClass="input-text-responsive" Enabled="False"></asp:TextBox>
                            <asp:HiddenField ID="hdnTallerID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>MODELO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtModelo" runat="server" MaxLength="30" 
                                CssClass="input-find-responsive" Enabled="False"></asp:TextBox>
                            <asp:HiddenField ID="hdnModeloID" runat="server" Visible="False" />
                        </td>
                    </tr> 
                       <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>TIPO MANTENIMIENTO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtTipoMantenimiento" runat="server" MaxLength="30" CssClass="input-text-responsive" Enabled="False"></asp:TextBox>
                            <asp:HiddenField ID="hdnTipoMantenimeinto" runat="server" Visible="False" />
                        </td>
                    </tr>                              
                      <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>No. ACTIVIDADES ALEATORIAS</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtAleatorias" runat="server" CssClass="CampoNumeroEntero"></asp:TextBox>
                        </td>
                    </tr> 
                                
                </table>
                     <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                    style="display: none;" onclick="btnEliminar_Click" />
                    
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>              
                </div>
            <fieldset>
              <legend>CONFIGURADOS</legend>
                  <asp:UpdatePanel ID="UpConfigurados" runat="server">
        <ContentTemplate>
            <div style="margin: 0 auto; width:80% !important; overflow: auto;">
                <asp:GridView ID="gvConfigurados" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" OnRowCommand="gvConfigurados_RowCommand"
                    OnPageIndexChanging="gvConfigurados_PageIndexChanging" 
                    onrowdatabound="gvConfigurados_RowDataBound" ShowHeaderWhenEmpty="True" 
                    PageSize="20">
                    <Columns>
                          <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;TIPO DE SERVICIO&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbTipoMantenimiento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimiento") %>'></asp:Label> 
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="40%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                          <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;No. OBLIGATORIAS&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbActividadesObligatorias" runat="server" Text=''></asp:Label> 
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="30%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;No. ALEATORIAS&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbActividadesAleatorias" runat="server" Text=''></asp:Label> 
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="20%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="&nbsp;&nbsp;EDITAR&nbsp;&nbsp;">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Ver" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png" ToolTip="" ImageAlign="Middle" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"/>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
            </fieldset>
               <br />
            <fieldset>
              <legend>ACTIVIDADES A AUDITAR</legend>
            <asp:UpdatePanel ID="UPContenedor" runat="server">           
        <ContentTemplate>
            <div style="margin: 0 auto; width:90% !important; overflow: auto;">
                <asp:GridView ID="gvActividades" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" 
                    OnPageIndexChanging="gvActividade_PageIndexChanging" 
                    onrowdatabound="gvActividade_RowDataBound" ShowHeaderWhenEmpty="True">
                    <Columns>
                         <asp:TemplateField HeaderText="&nbsp;&nbsp;Actividades&nbsp;&nbsp;">
                            <ItemTemplate>
                             <asp:Label ID="lbActividadAuditoria" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ConfiguracionPosicionTrabajo.DescriptorTrabajo.Nombre") %>'  Width="400px">></asp:Label>                     
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50%"/>              
                        </asp:TemplateField>
                          <asp:TemplateField Visible="false" >
                            <ItemTemplate>
                             <asp:Label ID="lbActividadID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ConfiguracionPosicionTrabajo.Id") %>'></asp:Label> 
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Center" Width="10%"/>              
                        </asp:TemplateField>
                          <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;CRITERIO&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txbCriterioActividad" runat="server"  style="width:100%; text-align: left !important;" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="40%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>        
                        <asp:TemplateField>
                        <HeaderTemplate>&nbsp;&nbsp;OBLIGATORIA&nbsp;&nbsp;</HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chbxObligatoria" runat="server" CssClass="Obligatoria"/>
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
        </ContentTemplate>
    </asp:UpdatePanel>
                <br />   
            </fieldset>      
 

            </div>

       </div>  

                             

</div>
</asp:Content>
