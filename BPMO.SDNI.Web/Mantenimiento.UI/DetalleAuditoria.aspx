<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleAuditoria.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.DetalleAuditoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloIngresarUnidad.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
    <link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
        .Grid { border: none;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .btnRegistrarIngreso { background: #DE0814 !important; border: none !important; height: 30px; line-height:30px; color: White;font-weight: bold; text-decoration: none; text-transform: uppercase; width: 185px; cursor: pointer; }
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; }
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - AUDITOR&Iacute;A</asp:Label>
        </div>
        <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            
        </div>
         
        <div id="divInformacionGeneral" class="GroupBody form-two-columns-responsive" >
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>AUDITOR&Iacute;A DE SERVICIOS</span>
                <div class="GroupHeaderOpciones Ancho1Opciones">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnFinalizar_Click" />
                </div>
            </div>
            <div id="divInformacionGeneralControles" class="form-two-columns-responsive">

                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">FOLIO AUDITOR&Iacute;A</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtIdAuditoria" runat="server" Enabled="false" 
                                ReadOnly="true" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>  
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" Enabled="false" 
                                ReadOnly="true" CssClass="input-text-responsive"></asp:TextBox>  
                        </td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-label-responsive">FECHA AUDITOR&Iacute;A</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtFecha" runat="server" Enabled="false" 
                                ReadOnly="true" CssClass="input-text-responsive"></asp:TextBox>  
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">NOMBRE DE LOS T&Eacute;CNICOS</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtTecnicos" runat="server" TextMode="MultiLine" Rows="3" Columns="3"
                                Enabled="false" ReadOnly="true" CssClass="input-text-responsive"></asp:TextBox>  
                        </td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-label-responsive">FOLIO ORDEN SERVICIO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtIdOS" runat="server" Enabled="false" 
                                ReadOnly="true" CssClass="CampoNumeroEntero input-text-responsive"></asp:TextBox>  
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">TIPO MANTENIMIENTO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtTipoMantenimiento" runat="server" Enabled="false" 
                                ReadOnly="true" CssClass="input-text-responsive"></asp:TextBox>
                        </td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td colspan="2" class="tdCentradoVertical">
                            <asp:Button runat="server" ID="ibtDescargar" CssClass="btnImprimir" Text="Evidencia" ToolTip="DESCARGAR"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:UpdatePanel ID="UPActividades" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvActividadesAuditoria" runat="server" CssClass="Grid" 
                                       EnableViewState="False"  AutoGenerateColumns="False" 
                                            ShowHeaderWhenEmpty="True" CellPadding="1" OnRowDataBound="gvActividadesAuditoria_RowDataBound">  
                                        <Columns>
                                            <asp:TemplateField HeaderText="Actividades">
                                                <ItemTemplate>
                                                 <asp:Label ID="lbActividadAuditoria" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.DescriptorTrabajo.Nombre") %>'></asp:Label>                     
                                                </ItemTemplate>
                                                <ItemStyle CssClass="lbGridLeftAlign" />           
                                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Criterios">
                                                <ItemTemplate>
                                                 <asp:Label ID="lbCriterioAuditoria" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Criterio") %>'></asp:Label>                     
                                                </ItemTemplate>     
                                                <ItemStyle CssClass="lbGridLeftAlign" />         
                                                <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false" >
                                                <ItemTemplate>
                                                 <asp:Label ID="lbActividadID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PosicionTrabajoPaquete.ConfiguracionPosicionTrabajo.Id") %>'></asp:Label> 
                                                </ItemTemplate>              
                                            </asp:TemplateField>
                        
                                            <asp:TemplateField>
                                                <HeaderTemplate>Satisfactorio</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="chbxSatisfactorio" runat="server" GroupName="Calificacion" class="RadioResultado" Enabled="false"/>
                                                </ItemTemplate>   
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="radio-resultado"/>
                                            </asp:TemplateField>
                        
                                            <asp:TemplateField>
                                                <HeaderTemplate>Reparar</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="chbxReparar" runat="server" GroupName="Calificacion" class="RadioResultado" Enabled="false"/>
                                                </ItemTemplate>   
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="radio-resultado"/>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>Ajustado</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="chbxAjustado" runat="server" GroupName="Calificacion" class="RadioResultado" Enabled="false"/>
                                                </ItemTemplate>   
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="radio-resultado"/>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>Comentarios</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtComentarioActividad" runat="server" Style=" margin: 0;" ReadOnly="true" Enabled="false" Text='<%# DataBinder.Eval(Container.DataItem,"Comentarios") %>'></asp:TextBox>
                                                </ItemTemplate  >
                                                <HeaderStyle HorizontalAlign="Center" Width="10%"/>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="radio-resultado"/>
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
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="width:100% !important;text-align:left;" class="tdCentradoVertical input-label-responsive">OBSERVACIONES DE LA AUDITOR&Iacute;A:</td>
                    </tr>
                    <tr>
                        <td colspan="7" class="tdCentradoVertical input-group-responsive" style="width:100% !important;text-align:left;">
                            <asp:TextBox ID="txtObservaciones" CssClass="input-text-responsive full-width" runat="server" Rows="5" Columns="3" TextMode="MultiLine" ReadOnly="true" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div>
            <asp:Literal ID="ltEmbed" runat="server" />
        </div>
    </div>
    <asp:HiddenField ID="hdnLibroActivos" runat="server" />
</asp:Content>

