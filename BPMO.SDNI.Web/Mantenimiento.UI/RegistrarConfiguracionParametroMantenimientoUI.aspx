<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarConfiguracionParametroMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.RegistrarConfiguracionParametroMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         #ContenedorMenuSecundario,GroupSection { display: inline-block}
        .GroupSection { width: 700px; margin: 0 auto;}
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
        #GridVacio {margin: 0 auto}
         #Parametros {width: 70%; margin: 0 auto;}
         .btnQuitar {margin-left: 5px;}
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

<div id="PaginaContenido">
    <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - REGISTRAR CONFIGURACIÓN MANTENIMIENTO</asp:Label>
        </div>

        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" >
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarConfiguracionParametroMantenimientoUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistroActaNacimiento" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarConfiguracionParametroMantenimientoUI.aspx">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>     
        </div>

        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
             <span>PARÁMETROS DE MANTENIMIENTO</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">          
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" CssClass="btnWizardGuardar" onclick="btnGuardar_Click"/>  

                     <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click"/>           
                </div>
            </div>
            <div id="divInformacionGeneralControles">

            <table class="trAlinearDerecha table-responsive"> 
             <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>MODELO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtModelo" runat="server" MaxLength="30" 
                                CssClass="input-find-responsive"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarModelo" CommandName="VerModelo"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelos" CommandArgument='' OnClick="btnBuscarModelo_Click" />
                            <asp:HiddenField ID="hdnModeloID" runat="server" Visible="False" />
                        </td>
                    </tr>
           </table>

      <fieldset id="Parametros">
      <legend>PARÁMETROS</legend>
      <table class="trAlinearDerecha table-responsive">                
                     <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>ESTADO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                        <asp:DropDownList ID="ddEstado" runat="server" 
                                CssClass="input-dropdown-responsive">
                                 <asp:ListItem>ESTACIONADO</asp:ListItem>
                                 <asp:ListItem Selected="True">EN USO</asp:ListItem>
                            </asp:DropDownList>
                       <%--     <asp:CheckBox ID="chbxEnUso" runat="server" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>TIPO DE MANTENIMIENTO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTipoMantenimiento" runat="server" 
                                CssClass="input-dropdown-responsive">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>UNIDAD DE MEDIDA</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddUnidadMedida" runat="server" 
                                CssClass="input-dropdown-responsive">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>INTERVALO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtIntervalo" runat="server" CssClass="CampoNumeroEntero"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                    </tr>
                     <tr>
                        <td class="tdCentradoVertical input-label-responsive"></td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:Button ID="Button1" runat="server" Text="Agregar a Tabla" CssClass="btnAgregarATabla" 
                                onclick="Button1_Click"/>
                        </td>
                    </tr>                  
                </table>
                   <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
                </fieldset>
             



            </div>

             <br />
       
     <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
            <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                <asp:GridView ID="gvConfiguraciones" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" OnRowCommand="gvConfiguraciones_RowCommand"
                    OnPageIndexChanging="gvConfiguraciones_PageIndexChanging" 
                    onselectedindexchanged="gvConfiguraciones_SelectedIndexChanged" 
                    ShowHeaderWhenEmpty="True" onrowdatabound="gvConfiguraciones_RowDataBound">
                    <Columns>                   
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="&nbsp;TIPO MANTENIMIENTO&nbsp;">
                            <ItemTemplate>
                                    <asp:Label ID="lbTipoMantenimiento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimiento") %>' Width="150px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="&nbsp;ESTADO&nbsp;">
                            <ItemTemplate>
                                    <asp:Label ID="lbEstaUso" runat="server" Text='' Width="80px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="&nbsp;UNIDAD MEDIDA&nbsp;">
                            <ItemTemplate>
                                    <asp:Label ID="lbParametro" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UnidadMedida") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                         <asp:TemplateField ItemStyle-Width="100px" HeaderText="&nbsp;INTERVALO&nbsp;">
                            <ItemTemplate>
                                    <asp:Label ID="lbIntervalo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Intervalo") %>' Width="80px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
					    <asp:TemplateField ItemStyle-Width="58px" HeaderText="&nbsp;Quitar&nbsp; ">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnQuitar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="" ImageAlign="Middle" CssClass="btnQuitar"/>
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
                       <b id="GridVacio" style="text-align: center">NO SE HA AGREGADO NINGUNA CONFIGURACIÓN</b>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />

        </div>
          <!-- Cuerpo -->

   
</div>

 <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
 <asp:HiddenField ID="hdnLibroActivos" runat="server" />
</asp:Content>
