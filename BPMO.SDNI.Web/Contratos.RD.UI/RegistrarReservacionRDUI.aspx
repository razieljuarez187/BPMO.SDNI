<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarReservacionRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.RegistrarReservacionRDUI" %>
<%@ Register Src="~/Contratos.RD.UI/ucReservacionRDUI.ascx" TagName="ucReservacionUI" TagPrefix="ucReservacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; position: inherit; border: 1px solid transparent; }
        #ControlesDatos fieldset { display: inherit; width: 96%; margin-top: 10px; margin-bottom: 13px; margin-left: 2%; margin-right: 2%; }
    </style>
    <script type="text/javascript">
        initChild = function () {
            <%= ucReservacion.ClientID %>_Inicializar();
        };
        $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - REGISTRAR RESERVACI&Oacute;N</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarReservacionRDUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.RD.UI/RegistrarReservacionRDUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
        </div>

        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="REGISTRAR RESERVACIÓN"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btnWizardGuardar" OnClick="btnRegistrar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <ucReservacion:ucReservacionUI ID="ucReservacion" runat="server" />
            </div>
            <div class="ContenedorMensajes">
                <span class="Requeridos"></span>
                <br />
                <span class="FormatoIncorrecto"></span>
            </div>            
        </div>
        <asp:GridView ID="grvReservaciones" runat="server" AutoGenerateColumns="false" CssClass="Grid"
            Style="width: 90%; margin: 25px auto 15px auto;"
            AllowPaging="false" AllowSorting="false" EnableSortingAndPagingCallbacks="false">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Fecha Inicial</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFechaInicial" Text='<%# DataBinder.Eval(Container.DataItem,"FechaInicial", "{0:dd/MM/yyyy hh:mm tt}") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="90px" />
                    <ItemStyle HorizontalAlign="Right" Width="90px" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Fecha Final</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFechaFinal" Text='<%# DataBinder.Eval(Container.DataItem,"FechaFinal", "{0:dd/MM/yyyy hh:mm tt}") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="90px" />
                    <ItemStyle HorizontalAlign="Right" Width="90px" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Cliente</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblClienteNombre" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente.Nombre") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Modelo</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblModeloNombre" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo.Nombre") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate># Económico</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUnidadEconomico" Text='<%# DataBinder.Eval(Container.DataItem,"Unidad.NumeroEconomico") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Usuario</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUsuarioNombre" Text='<%# DataBinder.Eval(Container.DataItem,"UsuarioReservo.Nombre") %>' Width="100%">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
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
</asp:Content>
