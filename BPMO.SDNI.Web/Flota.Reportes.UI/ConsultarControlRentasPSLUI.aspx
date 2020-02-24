<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ConsultarControlRentasPSLUI.aspx.cs" Inherits="BPMO.SDNI.Flota.Reportes.UI.ConsultarControlRentasPSLUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/EstiloIngresarUnidad.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/MantenimientoResponsive.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/bootstrap.1.8.2.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; } 
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
        .Grid { border: none;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .btnFooterToolbar { display: inline-block; font-size: 10px !important; padding: 10px 20px; cursor: pointer;
                            text-align: center; text-decoration: none; outline: none; color: #fff; background-color: #e9581b;
                            border: none; border-radius: 7px; box-shadow: 0 4px #999; }
        .btnFooterToolbar:hover {background-color: #d03f00}
        .btnFooterToolbar:active { background-color: #d03f00; box-shadow: 0 1px #666; transform: translateY(4px); }
        .btnRegistrarIngreso { background: #DE0814 !important; border: none !important; height: 30px; line-height:30px; color: White; font-weight: bold; text-decoration: none; text-transform: uppercase; width: 185px; cursor: pointer; }
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        .NoVisible { display:none; }
        .ConvertirMayus { text-transform: uppercase; }
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/bootstrap-1.8.2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/mantenimiento-responsive.js") %>" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () { initChild(); });

        function initChild() {
            inicializeHorizontalPanels();
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
        }
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td style='border: none; width:20px;'></td><td style='border: none; width:20px;'></td><td style='border: none; width:100% !important;' colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Contenido/Imagenes/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Contenido/Imagenes/plus.png");
            $(this).closest("tr").next().remove();
        });

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

        //Buscador Sucursal
        /*--------------------------------------------*/

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

        function RedireccionarPagina() {
            window.location = "../Flota.Reportes.UI/DescargarReporteExcel.aspx";            
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server"> 
    <div>
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label runat="server">Reportes - Centro de Control de Rentas</asp:Label>
        </div>
        <div>
            <%--<asp:Button ID="btnRegistrarIngreso" CssClass="btnRegistrarIngreso" runat="server" OnClick="OnclickAgregarIngreso" Text="Registrar Ingreso"></asp:Button>--%>
        </div>
        <!-- Cuerpo -->
        <br />
        <div id="Div1" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿Qué Filtros desea utilizar para aplicar en el reporte?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable ">
                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" 
                                CssClass="input-text-responsive" AutoPostBack="True" 
                                ontextchanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument='' OnClick="btnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">ESTATUS</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddEstatus" runat="server" CssClass="input-dropdown-responsive">
                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="input-button-responsive"><asp:Button runat="server" ID="btnConsultarReporte" Text="Buscar" OnClick="OnclickConsultarReporte" CssClass="btnComando" /></td>
                    </tr>
                </table>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <div style="width: 95% !important; overflow-x: scroll; overflow-y:hidden; margin-right: 20px; margin-left: 20px;">
                    <asp:GridView ID="gvUnidadesRentas" runat="server" AutoGenerateColumns="False" 
                        AllowPaging="True" EnableViewState="False" ShowHeaderWhenEmpty="True" OnPageIndexChanging="gvUnidadesRentas_PageIndexChanging"                        
                        CssClass="Grid" PageSize="20">
                            <Columns>
                                <asp:BoundField DataField="SUCURSAL" HeaderText="Sucursal" SortExpression="Sucursal">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Middle"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="ECODE" HeaderText="ECODE" SortExpression="ECODE">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCIÓN" SortExpression="Descripcion">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="#SERIE" HeaderText="# SERIE" SortExpression="#Serie">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ANIOUNIDAD" HeaderText="AÑO UNIDAD" SortExpression="AnioUnidad">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="ESTATUS" SortExpression="Estatus">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="¿SUBALQUILADO?" HeaderText="¿SUBALQUILADO?" SortExpression="Subalquilado">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="CLIENTE" HeaderText="CLIENTE" SortExpression="Cliente">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTRATO" HeaderText="CONTRATO" SortExpression="Contrato">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ASESOR" HeaderText="Asesor" SortExpression="Asesor">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHAVENCIMIENTO" HeaderText="FECHA VENCIMIENTO" SortExpression="FechaVencimiento">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="HOROMETRO" HeaderText="HORÓMETRO" SortExpression="Horometro">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>Sin registros disponibles</EmptyDataTemplate>
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
        <asp:Button ID="btnHSincronizarEstatus" runat="server" Text="Sincronizar" CssClass="NoVisible"/>

    <br />
    <div class="TextCenter" style="width:100%; overflow-x: scroll; overflow-y:hidden;">
        <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
        <%--<asp:HiddenField ID="hdnLibroActivos" runat="server" />--%>
        <asp:Button ID="btnExportar" CssClass="btnFooterToolbar" runat="server" Text="EXPORTAR REPORTE" OnClick="OnclickExportarReporteRentas" ToolTip="EXPORTAR REPORTE EN UN ARCHIVO DE EXCEL"/>
        <%--<asp:Button ID="btnEquiposProgramadosPendientes" CssClass="btnFooterToolbar" runat="server" Text="EQUIPOS PENDIENTES" OnClick="OnclickPendientesPorIngresar" ToolTip="EQUIPOS PROGRAMADOS PENDIENTES DE INGRESAR"/>
        <asp:Button ID="btnImprimirCalcomania" CssClass="btnFooterToolbar" runat="server" Text="IMPRIMIR CALCOMANIA" OnClick="OnclickImprimirCalcomania" ToolTip="GENERA LA CALCOMIA DEL MANTENIMIENTO"/>
        <asp:Button ID="btnPaseSalida" CssClass="btnFooterToolbar" runat="server" Text="PASE DE SALIDA" OnClick="OnclickPaseSalida" ToolTip="GENERA EL PASE DE SALIDA DE LA UNIDAD"/>
        <asp:Button ID="btnEnviarInformacionCliente" CssClass="btnFooterToolbar" runat="server" Text="ENVIAR INFORMACIÓN" OnClick="OnclickEnviarInformacionCliente" ToolTip="ENVIA LA INFORMACIÓN DEL MANTENIMIENTO AL CLIENTE"/>
        <asp:Button ID="btnTareasPendientes" CssClass="btnFooterToolbar" runat="server" Text="TAREAS PENDIENTES" Visible="false" OnClick="OnClickTareasPendientes" ToolTip="PRESENTA LAS TAREAS PENDIENTES PARA LA UNIDAD/MODELO"/>--%>
        <%--<a id="btnRedireccion" target="_blank" style="display:none;" href="ConsultarTareaPendienteUI.aspx">Link</a>--%>
        <%--<asp:HiddenField ID="hdnLeerInline" runat = "server" />
        <asp:HiddenField ID="hdnEsUnidad" runat="server" Value="" />--%>
        <asp:HiddenField ID="hdnTipoPermiso" runat="server" />
        <br />
        <br />
        <br />
    </div>
</asp:Content>
