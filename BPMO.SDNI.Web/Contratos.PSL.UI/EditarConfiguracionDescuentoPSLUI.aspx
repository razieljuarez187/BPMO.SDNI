<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarConfiguracionDescuentoPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.EditarConfiguracionDescuentoPSLUI" validateRequest="false" enableEventValidation="false" %>
<%@ Register TagPrefix="uc" TagName="ucConfiguracionDescuentoPSLUI" Src="~/Contratos.PSL.UI/ucConfiguracionDescuentoPSLUI.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="../Contenido/Scripts/ObtenerFormatoImporte.js" type="text/javascript"></script>
    <style type="text/css">
        .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
        }
        .RBL label
        {
            display: block;
        }
        .GroupBody
        {
            display: inline-table;
            float: right;
            margin-right: 38px;
            margin-top: 10px;
        }
        #divInformacionGeneralControles table
        {
            margin: 0;
        }
        #divInformacionGeneralControles .dvCentro
        {
            text-align: center;
            width: 65%;
        }
        #divInformacionGeneralControles .dvCentro .trAlinearDerecha
        {
            margin: 0px 0px 0px auto;
        }
        #divInformacionGeneralControles .dvCentro .trAlinearDerecha .tdCentradoVertical input
        {
            margin: 0px 5px 0px 15px !important;
            display: inline !important;
            vertical-align: middle !important;
        }
    </style>
     <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
        .Grid { width: 90%; margin: 25px auto 15px auto; }
    </style>


    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        //Validar campos requeridos

       
    
          
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">

        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - EDITAR CONFIGURACIÓN DESCUENTO</asp:Label>
        </div>
        
        <!--Navegación secundaria-->
         <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            
            
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultarDescuentosConfiguracion" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarConfiguracionDescuentoPSLUI.aspx" ForeColor="White"> 
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkConsultarConfiguracionDescuentos" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarConfiguracionDescuentoPSLUI.aspx"> 
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>

        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>EDITAR DESCUENTO</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="TERMINAR" 
                        CssClass="btnWizardTerminar" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click"/>
                </div>
            
            </div>
             <div id="divInformacionGeneralControles">
             <uc:ucConfiguracionDescuentoPSLUI runat="server" ID="ucConfiguracionDescuentoPSLUI" />
             </div>
           
            
             
        </div>
       <asp:HiddenField runat="server" ID="hdnModeloID" />
                        <asp:HiddenField runat="server" ID="hdnSucursalID" />
                        <asp:HiddenField runat="server" ID="hdnConfiguracionDescuentoID" />
                     <asp:HiddenField runat="server" ID="hdnLstConfiguracionDescuento" />
            
          
    </div>
</asp:Content>
