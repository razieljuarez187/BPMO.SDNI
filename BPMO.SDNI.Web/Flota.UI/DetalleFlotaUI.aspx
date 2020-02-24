<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="DetalleFlotaUI.aspx.cs" Inherits="BPMO.SDNI.Flota.UI.DetalleFlotaUI" %>
<%@ Register Src="~/Flota.UI/ucEquiposAliadosUnidadUI.ascx" TagPrefix="uc" TagName="ucEquiposAliadosUnidadUI" %>
<%@ Register Src="~/Flota.UI/ucDatosGeneralesElementoUI.ascx" TagPrefix="uc" TagName="ucDatosGeneralesElementoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #MenuSecundario { float: left; height: 31px; }
        #BarraHerramientas { width: 835px; float: right; }
        .GroupBody { margin: 10px auto; width: 930px; }
        .GroupHeader { width: 100%; }
        #ControlesDatos { min-height: 120px; margin-top: 17px; position: inherit; }
    </style>
    <script type="text/javascript">
        initChild = function () {
            ConfiguracionBarraHerramientas();
        };
        $(document).ready(initChild);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">FLOTA - CONSULTAR DETALLES DE FLOTA DE RENTA DIARIA</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarActaNacimiento" runat="server" NavigateUrl="~/Flota.UI/ConsultarFlotaUI.aspx"> CONSULTAR<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
                </li>
            </ul>  
            <!-- Barra de herramientas -->
			<div id="BarraHerramientas">
                <asp:Menu runat="server" ID="mnuDetalleFlota" IncludeStyleBlock="False" Orientation="Horizontal" CssClass="MenuPrimario">
					<Items>
						<asp:MenuItem Text="# ECON&Oacute;MICO" Value="NumeroEconomico" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="# DE SERIE" Value="NumeroSerie" Enabled="False" Selectable="false">
                        </asp:MenuItem>
					</Items>
					<StaticItemTemplate>
						<asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "NumeroEconomico" || (string) Eval("Value") == "NumeroSerie" ? "Informacion" : string.Empty %>' Text='<%# Eval("Text") %>'></asp:Label>
						<asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "NumeroEconomico" || (string) Eval("Value") == "NumeroSerie" %>' Style="width: 150px" CssClass="textBoxDisabled" Enabled="false"></asp:TextBox>                       					
					</StaticItemTemplate>
                    <LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
					</LevelSubMenuStyles>
				</asp:Menu>
				<div class="Ayuda" style="float: right">
					<input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
				</div>
            </div>  
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Flota.UI/ConsultarFlotaUI.aspx") %>'" />
            </div>        
        </div>
        <div id="DatosCatalogo" class="GroupBody">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">                
                <asp:Label ID="lblTituloPaso" runat="server" Text="INFORMACI&Oacute;N GENERAL DE LA UNIDAD"></asp:Label>
                <div class="GroupHeaderOpciones Ancho1Opciones">
                    <asp:Button ID="btnRegresar" runat="server" Text="Anterior" CssClass="btnWizardRegresar" onclick="btnRegresar_Click" />
                </div>
            </div>          
            <div id="divDetalle">
                <br/>
                <asp:HiddenField ID="hdnUnidadID" runat="server" />
                <asp:HiddenField ID="hdnEquipoID" runat="server" />
                <uc:ucDatosGeneralesElementoUI runat="server" ID="ucucDatosGeneralesElementoUI" />                                
            </div>            
        </div>    
        <br />    
        <div id="DatosEquiposAliadosUnidad" class="GroupBody">
            <div id="Div1" class="GroupHeader">
                <asp:Label ID="Label1" runat="server" Text="EQUIPOS ALIADOS DE LA UNIDAD"></asp:Label>
            </div>          
            <div id="divEquipoALiado">               
                <uc:ucEquiposAliadosUnidadUI runat="server" ID="ucucEquiposAliadosUnidadUI" />
            </div>            
        </div>  
    </div>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
</asp:Content>