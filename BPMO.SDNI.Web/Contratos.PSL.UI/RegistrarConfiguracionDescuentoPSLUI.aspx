<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" validateRequest="false" enableEventValidation="false"  AutoEventWireup="true" CodeBehind="RegistrarConfiguracionDescuentoPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.RegistrarConfiguracionDescuentoPSLUI" %>

<%@  Register TagPrefix="uc" TagName="ucConfiguracionDescuentoPSLUI" Src="~/Contratos.PSL.UI/ucConfiguracionDescuentoPSLUI.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
<!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="../Contenido/Scripts/ObtenerFormatoImporte.js" type="text/javascript"></script>
    <style type="text/css">
        .ContenedorMensajes
        {
            margin-bottom: 10px !important;
            margin-top: -10px !important;
        }
        .style2
        {
            width: 335px;
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
                <%=ucConfiguracionDescuentoPSLUI.ClientID %>_Inicializar();

        $(document).ready(initChild);
    </script>

   <script type="text/javascript">
            //Validar campos requeridos

            function soloNumeros(e) {
                var key = window.event ? e.which : e.keyCode;
                if (key < 48 || key > 57) {
                    e.preventDefault();
                }
            }
          
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">

        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - REGISTRAR CONFIGURACIÓN DESCUENTO</asp:Label>
        </div>
        
           <div style="height: 80px;" id="ContenedorMenuSecundario">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" >
                    <asp:HyperLink ID="hlkConsultarDescuentosConfiguracion" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarConfiguracionDescuentoPSLUI.aspx" > 
                        CONSULTAR
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado" >
                    <asp:HyperLink ID="hlkConsultarConfiguracionDescuentos" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarConfiguracionDescuentoPSLUI.aspx"> 
                        REGISTRAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
        </div>

        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionGeneralHeader" class="GroupHeader">
                <span>REGISTRO DE DESCUENTO</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="TERMINAR" 
                        CssClass="btnWizardTerminar" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click" />
                </div>
            </div>
            
            <div id="divInformacionGeneralControles">
                <uc:ucConfiguracionDescuentoPSLUI runat="server" ID="ucConfiguracionDescuentoPSLUI"  />
            </div>

        </div>

    </div>
</asp:Content>
