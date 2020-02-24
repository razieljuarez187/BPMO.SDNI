<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RealizarAuditoriaMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.RealizarAuditoriaMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ContenedorMenuSecundario,GroupSection { display: inline-block}
        .GroupSection { width: 750px; margin: 0 auto;}
        #divInformacionGeneral {margin: 0 auto;}
        #divInformacionGeneralControles {padding: 1em;}
        #divInformacionGeneralControles table { margin: 20px auto; }
        #gvrActividadesAuditoria { width: 100%; margin: 1em;}
        .ddTecnicos
        {
            height: 80px !important;
            width : 240px !important;
        }
        #ContenedorGrid 
        {
            max-height: 800px;
            overflow-y: scroll; 
        }
        .Grid td
        {
           vertical-align: middle;
        }
        .Grid { border: none; margin: 0 auto;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; padding: 0em 1em 0em 1em; width: 20px; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - iNICIAR AUDITORÍA</asp:Label>
        </div>
     <%--   <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="Li1">
                    <asp:HyperLink ID="hlkIngresoUnidad" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarUnidadUI.aspx"> 
                        INGRESAR UNIDAD
                        <img id="img2" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
                 <li id="Li2">
                    <asp:HyperLink ID="hlkIniciarAuditoria" runat="server" NavigateUrl="~/Mantenimiento.UI/RealizarAuditoriaMantenimientoUI.aspx"> 
                        REALIZAR AUDITORIA
                        <img id="img3" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>--%>
         <br />
         <br />
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
             <span>AUDITORÍA DE SERVICIOS</span>
                <div class="GroupHeaderOpciones Ancho3Opciones">
                    
                    <asp:Button ID="btnFinalizar" runat="server" Text="GUARDAR" CssClass="btnWizardTerminar" onclick="btnFinalizar_Click"/>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click"/>
                        <asp:Button ID="btnWizardImprimir" runat="server" Text="Imprimir" CssClass="btnWizardImprimir" OnClick="btnImprimirAuditoria_Click"/>                 
                </div>
            </div>
            <div id="divInformacionGeneralControles">

            <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span>FOLIO ORDEN SERVICIO</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFolioOS" runat="server" MaxLength="30" Width="240px" 
                                AutoPostBack="True" Enabled="False" ></asp:TextBox>  
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span>TIPO MANTENIMIENTO</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px; padding-right: 190px;">
                            <asp:TextBox ID="txtTipoMantenimiento" runat="server" Width="50px" 
                                MaxLength="30" Enabled="False" ReadOnly="True" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tdTecnicos">
                        <td class="" ><span>*</span>TÉCNICOS</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                          <asp:ListBox ID="ddlTecnicos" runat="server" Rows="5" CssClass="ddTecnicos" 
                                Enabled="False">
                          </asp:ListBox>  
                        </td>
                    </tr>
                </table>
                <div id ="ContenedorGrid">   
                <asp:UpdatePanel ID="UPActividades" runat="server">
                <ContentTemplate>
                <asp:GridView ID="grvActividadesAuditoria" runat="server" CssClass="Grid" 
                   EnableViewState="False"  AutoGenerateColumns="False" 
                        ShowHeaderWhenEmpty="True" CellPadding="1">  
                    <Columns>
                        <asp:TemplateField HeaderText="&nbsp;Actividades&nbsp;">
                            <ItemTemplate>
                             <asp:Label ID="lbActividadAuditoria" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.DescriptorTrabajo.Nombre") %>'></asp:Label>                     
                            </ItemTemplate>              
                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="&nbsp;Criterios&nbsp;">
                            <ItemTemplate>
                             <asp:Label ID="lbCriterioAuditoria" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Criterio") %>' ></asp:Label>                     
                            </ItemTemplate>              
                            <HeaderStyle HorizontalAlign="center" Width="25%" />
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" >
                            <ItemTemplate>
                             <asp:Label ID="lbActividadID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id") %>'></asp:Label> 
                            </ItemTemplate>              
                        </asp:TemplateField>
                        <asp:TemplateField>
                        <HeaderTemplate>&nbsp;SATISFACTORIO&nbsp;</HeaderTemplate>
                        <ItemTemplate>
                             <asp:RadioButton ID="chbxSatisfactorio" runat="server" GroupName="prueba" class="RadioResultado"
                             />
                          
                        </ItemTemplate>   
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                        <HeaderTemplate>&nbsp;REPARAR&nbsp;</HeaderTemplate>
                        <ItemTemplate>
                           
                            <asp:RadioButton ID="chbxReparar" runat="server" GroupName="prueba" class="RadioResultado"  />
                            
                        </ItemTemplate>   
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                        <HeaderTemplate>&nbsp;AJUSTADO&nbsp;</HeaderTemplate>
                        <ItemTemplate>
                            <asp:RadioButton ID="chbxAjustado" runat="server" GroupName="prueba" class="RadioResultado"  />
                           
                        </ItemTemplate>   
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>&nbsp;COMENTARIOS&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txbComentarioActividad" runat="server" Style=" margin: 0;" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </ItemTemplate  >
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
                </ContentTemplate>
                </asp:UpdatePanel>   
                </div>
                <br />
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical"><span>*</span>ADJUNTAR AUDITORÍA</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                        <asp:UpdatePanel ID="updPnEvidencias" runat="server" >
                                <ContentTemplate>
                                    <asp:FileUpload ID="fuplEvidenciaAuditoria" runat="server" Width="100%"/>
                               </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnFinalizar" />
                                </Triggers>
                                 <Triggers>
                                <asp:PostBackTrigger ControlID="btnWizardImprimir" />
                                </Triggers>
                            </asp:UpdatePanel> 
                            
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td><span>*</span>OBSERVACIONES DE LA AUDITORÍA:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txbObservaciones" runat="server" Rows="5" TextMode="MultiLine" 
                                Width="411px"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto">-</span>
                </div>



            </div>
        </div>


          <!-- Cuerpo -->
      


               

    </div>

</asp:Content>
