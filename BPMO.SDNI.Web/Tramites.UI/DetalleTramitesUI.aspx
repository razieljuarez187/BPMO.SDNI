<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleTramitesUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.DetalleTramitesUI" %>
<%@ Register src="ucTramiteTenenciaUI.ascx" tagname="ucTramiteTenenciaUI" tagprefix="uc1" %>
<%@ Register src="ucTramiteVerificacionUI.ascx" tagname="ucTramiteVerificacionUI" tagprefix="uc2" %>
<%@ Register src="ucTramitePlacalUI.ascx" tagname="ucTramitePlacalUI" tagprefix="uc3" %>
<%@ Register src="ucTramiteGPSUI.ascx" tagname="ucTramiteGPSUI" tagprefix="uc4" %>
<%@ Register src="ucTramiteFiltroAKUI.ascx" tagname="ucTramiteFiltroAKUI" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloTramites.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%=Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript" id= "funciones">
        $(document).ready(function () {
            initChild();
        });
        function initChild() {
            var vista = $("#<%= hdnPagina.ClientID %>").val();
            if (vista == "0") {
                $("#<%=btnTenencia.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "1") {
                $("#<%=btnVerificacionAmbiental.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "2") {
                $("#<%=btnVerificacionMecanica.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "3") {
                $("#<%=btnPlacaEstatal.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "4") {
                $("#<%=btnPlacaFederal.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "5") {
                $("#<%=btnGPS.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "6") {
                $("#<%=btnFiltro.ClientID %>").addClass("btnTabSeleccionado");
            }
            if (vista == "7") {
                $("#<%=btnSeguro.ClientID %>").addClass("btnTabSeleccionado");
            }

            $("span:contains('*')").hide();

            ConfiguracionBarraHerramientas();
        }
        $('.Informacion').parent().parent().addClass('Informacion');
            function DialogTenencia(){
                $("#dialogTenencia").dialog({
                    modal: true,
                    width: 620,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogTenencia").parent().appendTo("form:first");
           }
           function DialogVerificacionAmbiental(){
                $("#dialogVerificacionAmbiental").dialog({
                    modal: true,
                    width: 620,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogVerificacionAmbiental").parent().appendTo("form:first");
           }
           function DialogMecanico(){
                $("#dialogMecanico").dialog({
                    modal: true,
                    width: 620,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogMecanico").parent().appendTo("form:first");
           }
           function DialogPlacaEstatal(){
                $("#dialogPlacaEstatal").dialog({
                    modal: true,
                    width: 620,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogPlacaEstatal").parent().appendTo("form:first");
           }
           function DialogPlacaFederal(){
                $("#dialogPlacaFederal").dialog({
                    modal: true,
                    width: 730,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogPlacaFederal").parent().appendTo("form:first");
           }
           function DialogGPS(){
                $("#dialogGPS").dialog({
                    modal: true,
                    width: 620,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogGPS").parent().appendTo("form:first");
           }
           function DialogFiltro(){
                $("#dialogFiltro").dialog({
                    modal: true,
                    width: 700,
                    height: 400,
                    resizable: false,
                    buttons: {
                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogFiltro").parent().appendTo("form:first");
            }        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - DETALLE DE TR&Aacute;MITES</asp:Label>
        </div>
        <div style="height: 80px;">
			<ul id="MenuSecundario">
				<li class="MenuSecundarioSeleccionado">
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Tramites.UI/ConsultarTramitesUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
			</ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mTramites" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="# Serie" Value="NumeroSerie" Enabled="False" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Modelo" Value="Modelo" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Marca" Value="Marca" Selectable="false"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# ((string) Eval("Value") == "NumeroSerie" ||  (string) Eval("Value") == "Modelo" || (string) Eval("Value") == "Marca") ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "NumeroSerie" ||  (string) Eval("Value") == "Modelo" || (string) Eval("Value") == "Marca"%>' CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" /> 
                    </LevelSubMenuStyles>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
             </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Tramites.UI/ConsultarTramitesUI.aspx") %>'" />
            </div>
        </div>

        <div id="EncabezadoDatos" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
            <div id="OpcionesTabs" class="ui-tabs-nav ui-widget-header ui-corner-all">
                <asp:Button runat="server" Text="TENENCIA" ID="btnTenencia" CssClass="btnTab" onclick="btnTenencia_Click"/>
                <asp:Button runat="server" Text="VERIFICACIÓN AMBIENTAL" ID="btnVerificacionAmbiental" CssClass="btnTab" Width="180" onclick="btnVerificacionAmbiental_Click"/>
                <asp:Button runat="server" Text="VERIFICACION FÍSICO-MECÁNICO" ID="btnVerificacionMecanica" CssClass="btnTab" Width="220" onclick="btnVerificacionMecanica_Click"/>
                <asp:Button runat="server" Text="PLACA ESTATAL" ID="btnPlacaEstatal" CssClass="btnTab" onclick="btnPlacaEstatal_Click"/>
                <asp:Button runat="server" Text="PLACA FEDERAL" ID="btnPlacaFederal" CssClass="btnTab" onclick="btnPlacaFederal_Click" />
                <asp:Button runat="server" Text="GPS" ID="btnGPS" CssClass="btnTab" onclick="btnGPS_Click"/>
                <asp:Button runat="server" Text="FILTRO AK" ID="btnFiltro" CssClass="btnTab" onclick="btnFiltro_Click"/>
                <asp:Button runat="server" Text="SEGURO" ID="btnSeguro" CssClass="btnTab" onclick="btnSeguro_Click"/>
            </div>
        <div id="DatosCatalogo" class="ui-tabs-panel ui-widget-content ui-corner-bottom">
            <div id="ControlesDatos">
                <asp:MultiView ID="mvCU087" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwPagina0" runat="server">                        
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>Tenencia</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarTenencia" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarTenencia_Click" />
                                    <asp:Button ID="btnRegistrarNuevoTenencia" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarNuevoTenencia_Click" />
                                    <asp:Button OnClick="btnVerTenencia_Click" ID="btnVerTenencia" runat="server" CssClass="btnWizardHistorial" Text="Historial"/>
                                </div>
                            </div>                        
                            <uc1:ucTramiteTenenciaUI ID="ucTramiteTenencia" runat="server"/>                      
                            <div id="dialogTenencia" style="display:none;" title="Historial de Tenencias">
                                <asp:GridView ID="grdTenencias" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdTenencias_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Importe" DataField="Importe">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Folio" DataField="Folio" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Pago</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaPago" Text='<%# DataBinder.Eval(Container.DataItem,"FechaPago","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Activo</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate>
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
                    </asp:View>
                    <asp:View ID="vwPagina1" runat="server">
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>VERIFICACIÓN AMBIENTAL</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarVerificacionAmbiental" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarVerificacionAmbiental_Click" />
                                    <asp:Button ID="btnRegistrarNuevoVerificacionAmbiental" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarNuevoVerificacionAmbiental_Click" />
                                    <asp:Button OnClick="btnVerVerificacionAmbiental_Click" ID="btnVerVerificacionAmbiental" runat="server" Text="Historial" CssClass="btnWizardHistorial" />
                                </div>
                            </div>
                            <uc2:ucTramiteVerificacionUI ID="ucTramiteVerificacionAmbiental" runat="server" />
                            <div id="dialogVerificacionAmbiental" style="display:none;" title="Historial de Verificaciones Ambientales">
                                <asp:GridView ID="grdVerificacionAmbiental" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdVerificacionAmbiental_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Folio" DataField="Folio"></asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Vigencia Inicial</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaInicial" Text='<%# DataBinder.Eval(Container.DataItem,"VigenciaInicial","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Vigencia Final</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaFinal" Text='<%# DataBinder.Eval(Container.DataItem,"VigenciaFinal","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Activo</HeaderTemplate><ItemTemplate>
                                            <asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate>
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
                    </asp:View>
                    <asp:View ID="vwPagina2" runat="server">                        
                        <div class="GroupBody">
                            <div id="div1" class="GroupHeader">
                                <span>VERIFICACIÓN FÍSICO - MECÁNICO</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarVerificacionMecanico" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarVerificacionMecanico_Click" />
                                    <asp:Button ID="btnRegistrarVerificacionMecanico" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarVerificacionMecanico_Click" />
                                    <asp:Button OnClick="btnVerVerificacionMecanica_Click" ID="btnVerVerificacionMecanica" runat="server" CssClass="btnWizardHistorial" Text="Historial"/>
                                </div>
                            </div>
                            <uc2:ucTramiteVerificacionUI ID="ucTramiteVerificacionMecanica" runat="server" />  
                            <div id="dialogMecanico" style="display:none;" title="Historial de Verificaciones F&iacute;sico-Mec&aacute;nicas">
                                <asp:GridView ID="grdVerificacionMecanico" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdVerificacionMecanico_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Folio" DataField="Folio"></asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Vigencia Inicial</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaInicial" Text='<%# DataBinder.Eval(Container.DataItem,"VigenciaInicial","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Vigencia Final</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaFinal" Text='<%# DataBinder.Eval(Container.DataItem,"VigenciaFinal","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Activo</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate>
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
                    </asp:View>
                    <asp:View ID="vwPagina3" runat="server">
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>PLACA ESTATAL</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarPlacaEstatal" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarPlacaEstatal_Click" />
                                    <asp:Button ID="btnRegistrarPlacaEstatal" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarPlacaEstatal_Click" />
                                    <asp:Button OnClick="btnVerPlacaEstatal_Click" ID="btnVerPlacaEstatal" runat="server" Text="Historial" CssClass="btnWizardHistorial"/>
                                </div>
                            </div>
                            <uc3:ucTramitePlacalUI ID="ucTramitePlacaEstatal" runat="server" />
                            <div id="dialogPlacaEstatal" style="display:none;" title="Historial de Placas Estatales">
                                <asp:GridView ID="grdPlacaEstatal" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdPlacaEstatal_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="#" DataField="Numero"></asp:BoundField>
                                        <asp:TemplateField><HeaderTemplate>Activo</HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate></asp:TemplateField>
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
                    </asp:View>
                    <asp:View ID="vwPagina4" runat="server">
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>PLACA FEDERAL</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarPlacaFederal" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarPlacaFederal_Click" />
                                    <asp:Button ID="btnRegistrarPlacaFederal" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarPlacaFederal_Click" />
                                    <asp:Button OnClick="btnVerPlacaFederal_Click" ID="btnVerPlacaFederal" runat="server" Text="Historial" CssClass="btnWizardHistorial"/>
                                </div>
                            </div>
                            <uc3:ucTramitePlacalUI ID="ucTramitePlacaFederal" runat="server" />
                            <div id="dialogPlacaFederal" style="display:none;" title="Historial de Placas Federales">
                                <asp:GridView ID="grdPlacaFederal" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdPlacaFederal_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="#" DataField="Numero"></asp:BoundField>
                                        <asp:BoundField HeaderText="# Gu&iacute;a" DataField="NumeroGuia" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Env&iacute;o Documentos</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaEnvioDocumentos" Text='<%# DataBinder.Eval(Container.DataItem,"FechaEnvioDocumentos","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha Recepci&oacute;n</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaRecepcion" Text='<%# DataBinder.Eval(Container.DataItem,"FechaRecepcion","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField><HeaderTemplate>Activo</HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate></asp:TemplateField>
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
                    </asp:View>
                    <asp:View ID="vwPagina5" runat="server">
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>GPS</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarGPS" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarGPS_Click" />
                                    <asp:Button ID="btnRegistrarGPS" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarGPS_Click" />
                                    <asp:Button OnClick="btnVerGPS_Click" ID="btnVerGPS" runat="server" Text="Historial" CssClass="btnWizardHistorial"/>
                                </div>
                            </div>
                            <uc4:ucTramiteGPSUI ID="ucTramiteGPS" runat="server" />
                            <div id="dialogGPS" style="display:none;" title="Historial de GPS">
                                <asp:GridView ID="grdGPS" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdGPS_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="#" DataField="Numero"></asp:BoundField>
                                        <asp:BoundField HeaderText="Compañ&iacute;a" DataField="Compania" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha de Instalaci&oacute;n</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaInstalacion" Text='<%# DataBinder.Eval(Container.DataItem,"FechaInstalacion","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField><HeaderTemplate>Activo</HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate></asp:TemplateField>
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
                    </asp:View>
                    <asp:View ID="vwPagina6" runat="server">
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>FILTRO AK</span>
                                <div class="GroupHeaderOpciones Ancho3Opciones">
                                    <asp:Button ID="btnEditarFiltro" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarFiltro_Click" />
                                    <asp:Button ID="btnRegistrarFiltro" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarFiltro_Click" />
                                    <asp:Button OnClick="btnVerFiltro_Click" ID="btnVerFiltro" runat="server" Text="Historial" CssClass="btnWizardHistorial"/>
                                </div>
                            </div>
                            <uc5:ucTramiteFiltroAKUI ID="ucTramiteFiltroAK" runat="server" />
                            <div id="dialogFiltro" style="display:none;" title="Historial de Filtros AK">
                                <asp:GridView ID="grdFiltro" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnPageIndexChanging="grdFiltro_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="# Serie" DataField="NumeroSerie"></asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Fecha de Instalaci&oacute;n</HeaderTemplate>
                                            <ItemTemplate><asp:Label runat="server" ID="lblFechaInstalacion" Text='<%# DataBinder.Eval(Container.DataItem,"FechaInstalacion","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField><HeaderTemplate>Activo</HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblActivo" Text='<%# DataBinder.Eval(Container.DataItem,"Activo").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>' Width="100%"></asp:Label></ItemTemplate></asp:TemplateField>
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
                    </asp:View>
                    <asp:View ID="vwPagina7" runat="server">
                        <div class="GroupBody">
                            <div class="GroupHeader">
                                <span>SEGURO</span>
                                <div class="GroupHeaderOpciones Ancho2Opciones">
                                    <asp:Button ID="btnEditarSeguro" runat="server" Text="Editar" CssClass="btnWizardEditar" onclick="btnEditarSeguro_Click" />
                                    <asp:Button ID="btnRegistrarSeguro" runat="server" Text="Nuevo" CssClass="btnWizardGuardar" onclick="btnRegistrarSeguro_Click" />
                                </div>
                            </div>                        
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td># Póliza</td>
                                    <td><asp:TextBox runat="server" ID="txtPoliza" Enabled="false" Columns="23"></asp:TextBox></td>
                                    <td align="right">Aseguradora</td>
                                    <td><asp:TextBox runat="server" ID="txtAseguradora" Enabled="false" Columns="23"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Fecha Inicial</td>
                                    <td><asp:TextBox runat="server" ID="txtFechaInicial" CssClass="CampoFecha" Enabled="false"></asp:TextBox></td>
                                    <td align="right">Fecha Final</td>
                                    <td >
                                        <asp:TextBox runat="server" ID="txtFechaFinal" CssClass="CampoFecha" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
        </div>
    </div>
    <asp:HiddenField runat="server" Value="0" ID="hdnPagina"/>
</asp:Content>
