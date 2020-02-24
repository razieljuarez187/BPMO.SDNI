<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarTramitesUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.RegistrarTramitesUI" %>
<%@ Register Src="ucTramiteFiltroAKUI.ascx" TagName="ucTramiteFiltroAKUI" TagPrefix="uc1" %>
<%@ Register Src="ucTramiteGPSUI.ascx" TagName="ucTramiteGPSUI" TagPrefix="uc2" %>
<%@ Register Src="ucTramitePlacalUI.ascx" TagName="ucTramitePlacalUI" TagPrefix="uc3" %>
<%@ Register Src="ucTramiteTenenciaUI.ascx" TagName="ucTramiteTenenciaUI" TagPrefix="uc4" %>
<%@ Register Src="ucTramiteVerificacionUI.ascx" TagName="ucTramiteVerificacionUI" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloTramites.css" rel="stylesheet" type="text/css" />
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%=Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        function inicializarTramites() {
            if(typeof <%= ucTramiteTenencia.ClientID %>_initPage == 'function'){
            <%= ucTramiteTenencia.ClientID %>_initPage();
            }
            if(typeof <%= ucTramiteVerificacionAmbiental.ClientID %>_initPage == 'function'){
            <%= ucTramiteVerificacionAmbiental.ClientID %>_initPage();
            }
            if(typeof <%= ucTramiteVerificacion.ClientID %>_initPage == 'function'){
            <%= ucTramiteVerificacion.ClientID %>_initPage();
            }
            if(typeof <%= UcTramitePlacalEstatal.ClientID %>_initPage == 'function'){
            <%= UcTramitePlacalEstatal.ClientID %>_initPage();
            }
            if(typeof <%= ucTramitePlacal.ClientID %>_initPage == 'function'){
            <%= ucTramitePlacal.ClientID %>_initPage();
            }
            if(typeof <%= ucTramiteGPS.ClientID %>_initPage == 'function'){
            <%= ucTramiteGPS.ClientID %>_initPage();
            }
            if(typeof <%= ucTramiteFiltroAK.ClientID %>_initPage == 'function'){
            <%= ucTramiteFiltroAK.ClientID %>_initPage();
            } 
        }
   
        initChild = function () {
            ConfiguracionBarraHerramientas();
        }
        $(document).ready(function () {
            initChild();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Cuerpo -->
    <div id="PaginaContenido">
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - REGISTRAR TR&Aacute;MITES</asp:Label>
        </div>
        <div style="height: 80px;">
			<ul id="MenuSecundario">
				<li>
					<asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Tramites.UI/ConsultarTramitesUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
					</asp:HyperLink>
				</li>
                <li class="MenuSecundarioSeleccionado">
                    <a href="#">
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </a>
                </li>
			</ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas" style="float: right;">
                <asp:Menu runat="server" ID="mnTramites" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="# Serie" Value="NumSerie" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Modelo" Value="Modelo" Selectable="false"></asp:MenuItem>
                        <asp:MenuItem Text="Marca" Value="Marca" Selectable="false"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                         <asp:Label runat="server" ID="lblOpcion" CssClass='<%# ((string) Eval("Value") == "NumSerie" ||  (string) Eval("Value") == "Modelo" || (string) Eval("Value") == "Marca") ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
                         <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "NumSerie"  ||  (string) Eval("Value") == "Modelo" || (string) Eval("Value") == "Marca"%>'
                            Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                    <LevelSubMenuStyles><asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" /> </LevelSubMenuStyles>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Tramites.UI/ConsultarTramitesUI.aspx") %>'" />
           </div>
        </div>

        <asp:MultiView ID="mvTramites" runat="server" ActiveViewIndex="0">
            <asp:View ID="TramiteFiltroAK" runat="server">
                <div id="divTramiteFiltroAK" class="GroupBody">
                    <div class="GroupHeader">
                        <span>Trámite filtro AK</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarFiltro" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarFiltro" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc1:ucTramiteFiltroAKUI ID="ucTramiteFiltroAK" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="TramiteGPS" runat="server">
                <div id="divTramiteGPS" class="GroupBody">
                    <div class="GroupHeader">
                        <span>Trámite GPS</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarGPS" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarGPS" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc2:ucTramiteGPSUI ID="ucTramiteGPS" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="TramitePlaca" runat="server">
                <div id="divTramitePlacal" class="GroupBody">
                    <div class="GroupHeader">
                        <span>Trámite Placa Federal</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarPlacaFederal" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarPlacaFederal" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc3:ucTramitePlacalUI ID="ucTramitePlacal" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="TramitePlacaEstatal" runat="server">
                <div id="divTramitePlaca" class="GroupBody">
                    <div class="GroupHeader">
                        <span>Trámite Placa Estatal</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarPlacaEstatal" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarPlacaEstatal" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc3:ucTramitePlacalUI ID="UcTramitePlacalEstatal" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="TramiteTenencia" runat="server">
                <div id="divTramiteTenencia" class="GroupBody">
                    <div class="GroupHeader">
                        <span>Trámite Tenencia</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarTenencia" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarTenencia" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc4:ucTramiteTenenciaUI ID="ucTramiteTenencia" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="TramiteVerificacion" runat="server">
                <div id="divTramiteVerificacion" class="GroupBody">
                <div class="GroupHeader">
                        <span>Trámite Verificación Fisico - Mecánico</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarMecanico" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarMecanico" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc5:ucTramiteVerificacionUI ID="ucTramiteVerificacion" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="TramiteVerificacionAmbiental" runat="server">
                <div id="divTramiteVerificacionAmbiental" class="GroupBody">
                <div class="GroupHeader">
                        <span>Trámite Verificación Ambiental</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarAmbiental" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelarAmbiental" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                        </div>
                    </div>
                    <uc5:ucTramiteVerificacionUI ID="ucTramiteVerificacionAmbiental" runat="server" />
                    <div class="ContenedorMensajes">
                        <span class="Requeridos"></span>
                        <br />
                        <span class="FormatoIncorrecto"></span>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
