<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarTramitesUI.aspx.cs" Inherits="BPMO.SDNI.Tramites.UI.EditarTramitesUI" %>
<%@ Register src="ucTramiteTenenciaUI.ascx" tagname="ucTramiteTenenciaUI" tagprefix="uc1" %>
<%@ Register src="ucTramiteVerificacionUI.ascx" tagname="ucTramiteVerificacionUI" tagprefix="uc2" %>
<%@ Register src="ucTramitePlacalUI.ascx" tagname="ucTramitePlacalUI" tagprefix="uc3" %>
<%@ Register src="ucTramiteGPSUI.ascx" tagname="ucTramiteGPSUI" tagprefix="uc4" %>
<%@ Register src="ucTramiteFiltroAKUI.ascx" tagname="ucTramiteFiltroAKUI" tagprefix="uc5" %>
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
            if(typeof <%= ucTramiteVerificacionMecanico.ClientID %>_initPage == 'function'){
            <%= ucTramiteVerificacionMecanico.ClientID %>_initPage();
            }
            if(typeof <%= ucTramitePlacalEstatal.ClientID %>_initPage == 'function'){
            <%= ucTramitePlacalEstatal.ClientID %>_initPage();
            }
            if(typeof <%= ucTramitePlacalFederal.ClientID %>_initPage == 'function'){
            <%= ucTramitePlacalFederal.ClientID %>_initPage();
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
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">UNIDADES - EDITAR DE TR&Aacute;MITES</asp:Label>
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
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "NumeroSerie" ||  (string) Eval("Value") == "Modelo" || (string) Eval("Value") == "Marca" %>' CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>
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

        <asp:Panel runat="server" ID="pnlTenencia" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>Tenencia</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarTenencia_Click" />
                        <asp:Button ID="btnCancelarTenencia" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>
                <uc1:ucTramiteTenenciaUI ID="ucTramiteTenencia" runat="server" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlVerificacionAmbiental" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>Verificación Ambiental</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardarAmbiental" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarAmbiental_Click" />
                        <asp:Button ID="btnCancelarAmbiental" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>            
                <uc2:ucTramiteVerificacionUI ID="ucTramiteVerificacionAmbiental" runat="server" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlVerificacionMecanico" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>Verificación Físico - Mecánico</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardarMecanico" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarMecanico_Click" />
                        <asp:Button ID="btnCancelarMecanico" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>
                <uc2:ucTramiteVerificacionUI ID="ucTramiteVerificacionMecanico" runat="server" />  
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPlacaEstatal" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>Placa Estatal</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardarPlacaEstatal" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarPlacaEstatal_Click" />
                        <asp:Button ID="btnCancelarPlacaEstatal" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>            
                <uc3:ucTramitePlacalUI ID="ucTramitePlacalEstatal" runat="server" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPlacaFederal" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>Placa Federal</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardarPlacaFederal" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarPlacaFederal_Click" />
                        <asp:Button ID="btnCancelarPlacaFederal" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>            
                <uc3:ucTramitePlacalUI ID="ucTramitePlacalFederal" runat="server" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlGPS" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>GPS</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardarGPS" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarGPS_Click" />
                        <asp:Button ID="btnCancelarGPS" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>
                <uc4:ucTramiteGPSUI ID="ucTramiteGPS" runat="server" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlFiltro" Visible="false">
            <div class="GroupBody">
                <div class="GroupHeader">
                    <span>Filtro AK</span>
                    <div class="GroupHeaderOpciones Ancho2Opciones">
                        <asp:Button ID="btnGuardarFiltro" runat="server" Text="Guardar" CssClass="btnWizardGuardar" onclick="btnGuardarFiltro_Click" />
                        <asp:Button ID="btnCancelarFiltro" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                    </div>
                </div>
                <uc5:ucTramiteFiltroAKUI ID="ucTramiteFiltroAK" runat="server" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
