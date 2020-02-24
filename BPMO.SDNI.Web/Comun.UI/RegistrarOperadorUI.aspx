<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarOperadorUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.RegistrarOperadorUI" %>
<%@ Register TagPrefix="uc" TagName="ucOperadorUI" Src="~/Comun.UI/ucOperadorUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 64px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 761px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 10px; position: inherit; border: 1px solid transparent; }
    </style>

    <script type="text/javascript">
        //Validar campos requeridos
        function ValidatePage(Texto) {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                MensajeGrowUI("Falta información necesaria para " + Texto, "4");
                return;
            }
        }

        initChild = function () {
            <%= ucDatosOperadorUI.ClientID %>_Inicializar();
        };
        $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CAT&Aacute;LOGOS - REGISTRAR OPERADOR</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li>
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarOperadorUI.aspx"> CONSULTAR <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlRegistroOperador" runat="server" NavigateUrl="~/Comun.UI/RegistrarOperadorUI.aspx"> REGISTRAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/></asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <asp:Label ID="lblTituloPaso" runat="server" Text="DATOS GENERALES"></asp:Label>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardTerminar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_Click" />
                </div>
            </div>
            <div id="ControlesDatos">
                <uc:ucOperadorUI ID="ucDatosOperadorUI" runat="server" />
                <div style="Text-align: right; margin-top: 10px; margin-bottom: 10px; width: 98%;">
                    <asp:Button ID="btnAgregarOperador" runat="server" CssClass="btnAgregarATabla" Text="Agregar a Tabla"
                        Enabled="true" onclick="btnAgregarOperador_Click" />
                </div>
                <asp:GridView ID="grdOperadores" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" 
                    AllowPaging="True" AllowSorting="False" style="width: 96%; margin-left: auto; margin-right: auto; margin-bottom: 10px;"
                    onpageindexchanging="grdOperadores_PageIndexChanging" onrowcommand="grdOperadores_RowCommand" onrowdatabound="grdOperadores_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                        <asp:BoundField DataField="AñosExperiencia" HeaderText="Años experiencia" />
                        <asp:TemplateField>
                            <HeaderTemplate>Tipo licencia</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem, "Licencia.Tipo")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate># Licencia</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem, "Licencia.Numero")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Fecha expiración</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblFechaExpiracion" Text='<%# DataBinder.Eval(Container.DataItem,"Licencia.FechaExpiracion") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="90px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Estado Expedición</HeaderTemplate>
                            <ItemTemplate><%# DataBinder.Eval(Container.DataItem, "Licencia.Estado")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                                    ToolTip="Eliminar" CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' />
                            </ItemTemplate>
                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridHeader" />
                    <EditRowStyle CssClass="GridAlternatingRow" />
                    <PagerStyle CssClass="GridPager" />
                    <RowStyle CssClass="GridRow" />
                    <FooterStyle CssClass="GridFooter" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="ContenedorMensajes">
        <span class="Requeridos"></span>
        <br />
        <span class="FormatoIncorrecto"></span>
    </div>
</asp:Content>
