<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarConfiguracionParametroMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.EditarConfiguracionParametroMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ContenedorMenuSecundario,GroupSection { display: inline-block}
        .GroupSection { width: 400px; margin: 0 auto;}
        .GroupBody {max-width: 700px;}
        .GroupHeader span,.GroupHeaderCollapsable span { margin-top: 2px;}
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

          
         <script type="text/javascript">
             function confirmarEliminacion(origen) {
                 var $div = $('<div title="Confirmación"></div>');
                 $div.append('¿Esta seguro que desea eliminar este Parametro de Configuracion?');
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - EDITAR CONFIGURACIÓN MANTENIMIENTO</asp:Label>
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
                <asp:Menu runat="server" ID="mnuConfiguracionMantenimiento" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario" >
                    <Items>
                       <%-- <asp:MenuItem Text="Configuracion MantenimientoID" Value="ConfiguracionMantenimientoID" Enabled="False" Selectable="false"></asp:MenuItem>--%>
                        <asp:MenuItem Text="Editar" Value="Editar" Selected="true" NavigateUrl="#" ></asp:MenuItem>
						<asp:MenuItem Text="Eliminar" Value="EliminarParametro" ></asp:MenuItem>
                    </Items>
                   <%-- <StaticItemTemplate>
                       <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "ConfiguracionMantenimientoID" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "ConfiguracionMantenimientoID" %>' Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
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
             <span>
                 <asp:Label ID="lbEstado" runat="server" Text=""></asp:Label></span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnActualizar" runat="server" Text="GUARDAR" CssClass="btnWizardGuardar" onclick="btnActualizar_Click"/>
                    <asp:Button ID="btnRegresar" runat="server" Text="CANCELAR" CssClass="btnWizardCancelar" onclick="btnRegresar_Click"/>           
                    <asp:Button ID="btnConfirmEliminar" runat="server" Text="Confirmar" 
                        CssClass="btnWizardGuardar"  onclientclick="window.mostrarConfirmacion='0'" onclick="btnEliminarConfirm_Click" Visible="False"/>          
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
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>ESTADO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                           <asp:DropDownList ID="ddEstado" runat="server" 
                                CssClass="input-dropdown-responsive">
                                 <asp:ListItem>ESTACIONADO</asp:ListItem>
                                 <asp:ListItem Selected="True">EN USO</asp:ListItem>
                            </asp:DropDownList>
                           <%-- <asp:CheckBox ID="chbxEnUso" runat="server" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>TIPO DE MANTENIMIENTO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTipoMantenimiento" runat="server" 
                                CssClass="input-dropdown-responsive" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>UNIDAD DE MEDIDA</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddUnidadMedida" runat="server" 
                                CssClass="input-dropdown-responsive" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>INTERVALO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtIntervalo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>ACTIVO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:CheckBox ID="chbxActivo" runat="server" Enabled="False" />
                        </td>
                    </tr>
                </table>

                 <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                    style="display: none;" onclick="btnEliminar_Click" />
                </fieldset>
               
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>                 
                </div>

            </div>


       
        </div>

</div>
          <!-- Cuerpo -->

</asp:Content>
