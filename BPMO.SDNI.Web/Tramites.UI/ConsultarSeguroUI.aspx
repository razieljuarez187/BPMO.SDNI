<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarSeguroUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.ConsultarSeguroUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .Grid { width: 90%; margin: 25px auto 15px auto; }
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
     <script src="<%=Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>

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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - CONSULTAR SEGUROS</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Tramites.UI/ConsultarSeguroUI.aspx">
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
            </ul>
        </div>       
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿QUÉ SEGURO DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">VIN</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtVIN" runat="server" Width="275px" AutoPostBack="true" ToolTip="Número de serie" ontextchanged="txtVIN_TextChanged" MaxLength="100"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Buscar Unidad" 
                                onclick="ibtnBuscaUnidad_Click" />
                        </td>                    
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical"># DE P&Oacute;LIZA</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroPoliza" runat="server" Width="301px" ToolTip="Número de poliza" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">ASEGURADORA</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtAseguradora" runat="server" Width="301px" ToolTip="Nombre Aseguradora" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical"><label>¿VENCIDOS?</label></td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList runat="server" ID="ddlProximaVencer" Width="30%">
                                <asp:ListItem Text="TODOS" Value="-1"/>
                                <asp:ListItem Text="NO" Value="0"/>
                                <asp:ListItem Text="SI" Value="1"/>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btnComando" ToolTip="Consultar seguros" onclick="btnBuscar_Click" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <asp:GridView ID="grdSeguros" runat="server" AutoGenerateColumns="False" 
            CssClass="Grid" AllowPaging="True" 
            EnableSortingAndPagingCallbacks="True" onrowdatabound="grdSeguros_RowDataBound" 
            onpageindexchanging="grdSeguros_PageIndexChanging" 
            onrowcommand="grdSeguros_RowCommand" align="center">
            <Columns>
                <asp:BoundField HeaderText="TramiteID" DataField="TramiteID" Visible="false"/>
                <asp:TemplateField HeaderText="VIN" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblVIN" Text="" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="170" HorizontalAlign="Justify" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modelo" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblModelo" Text="" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="200" HorizontalAlign="Justify" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="# De Póliza" DataField="NumeroPoliza" ItemStyle-Width="100px"/>
                <asp:BoundField HeaderText="Aseguradora" DataField="Aseguradora"/>
                <asp:BoundField HeaderText="Vencimiento" DataField="VigenciaFinal" DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="Right" Width="110px"/>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Días" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblProximoVencer" Text="" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60" HorizontalAlign="Center" />
                </asp:TemplateField>                        
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnVer" runat="server" 
                            CommandArgument='<%#DataBinder.Eval(Container.DataItem,"TramiteID") %>' 
                            CommandName="Detalles" ImageAlign="Middle" 
                            ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver detalles" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="17px" />
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
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
    <asp:HiddenField ID="hdnVIN" runat="server" /> 
    <asp:Button ID="btnResult" runat="server" Text="" onclick="btnResult_Click" style="display: none;" />
</asp:Content>
