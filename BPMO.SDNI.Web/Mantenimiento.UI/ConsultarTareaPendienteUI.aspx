<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarTareaPendienteUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ConsultarTareaPendienteUI" %>
<%@ Register Src="~/Mantenimiento.UI/ucTareasPendientesUI.ascx" TagPrefix="uc" TagName="ucTareasPendientesUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../Contenido/Estilos/EstiloTareaPendiente.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/MantenimientoResponsive.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/bootstrap.1.8.2.css" rel="stylesheet" type="text/css" />
   <style type="text/css">
       .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table.table-responsive { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
         .Grid { border: none; margin: 0 auto; text-transform:uppercase}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; padding: 0em 1em 0em 1em; width: 20px; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .GridPager td {padding: 0 !important; width: 0px;}
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
    </script>    
    <script type="text/javascript">
        function BtnBuscar(guid, xml, sender) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#" + sender),
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="Label1" runat="server">CONSULTAR TAREAS PENDIENTES</asp:Label>
        </div>
         <!--Navegación secundaria-->
        <div style="height: 80px;">
        <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarTareaPendiente" runat="server" NavigateUrl="~/Mantenimiento.UI/ConsultarTareaPendienteUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistroTareaPendiente" runat="server" NavigateUrl="~/Mantenimiento.UI/RegistrarTareaPendienteUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/></asp:HyperLink>
                </li>
            </ul>
        </div>
        <!-- Cuerpo -->
        <div id="Div1" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>CONSULTAR TAREA</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="ControlesDatos" class="GroupContentCollapsable">
                <uc:ucTareasPendientesUI runat="server" ID="ucTareasPendientesUI" />
                <table id="tablaAdicional" class="trAlinearDerecha table-responsive" style="margin: 0px auto 10px auto; border: 1px solid transparent;">
                    <tr>
                    <td class="input-button-responsive">
                         <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btnComando" onclick="btnBuscar_Click" />
                    </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
        <div id="divGrid" style="margin: 0 auto; width:95% !important;">
                <asp:GridView ID="gvTareas" runat="server" AutoGenerateColumns="False" CssClass="Grid Table" 
                    AllowPaging="true" PageSize="10" EnableViewState="True" OnRowCommand="gvTareas_RowCommand"
                    EnableSortingAndPagingCallbacks="True" 
                    OnPageIndexChanging="gvTareas_PageIndexChanging" OnRowDataBound="gvTareas_RowDataBound">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="229px" HeaderText="NÚMERO DE SERIE">
                            <ItemTemplate>
                                    <asp:Label ID="lbNumeroSerie" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroSerie") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="180px" HeaderText="NÚMERO ECONÓMICO">
                            <ItemTemplate>
                                    <asp:Label ID="lbNumeroEconomico" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroEconomico") %>' Width="180px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="229px" HeaderText="MODELO">
                            <ItemTemplate>
                                    <asp:Label ID="lbModelo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %>' Width="229px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
					    <asp:TemplateField ItemStyle-Width="50px" HeaderText="VER">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

