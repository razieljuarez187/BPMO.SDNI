<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ProgramarCitaMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ProgramarCitaMantenimientoUI" %>
<%@ Register Src="~/Mantenimiento.UI/ucDatosCitaMantenimientoUI.ascx" TagPrefix="uc" TagName="ucDatosCitaUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="PaginaContenido">
    <!-- Barra de localización -->
    <div id="BarraUbicacion">
        <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - PROGRAMAR CITA DE MANTENIMIENTOS</asp:Label>
    </div>
    <div style="height: 32px;">
        <!-- Menú secundario -->
        <ul id="MenuSecundario" style="float: left; height: 32px;">
            <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Mantenimiento.UI/ProgramarMantenimientosUI.aspx"> REGRESAR <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /></asp:HyperLink>
            </li>
        </ul>
    </div>
    <!-- Cuerpo -->
    <div id="Formulario">
        <br />
        <uc:ucDatosCitaUI ID="ucDatosCitaMantenimiento" runat="server" />
        <div id="divBoton"  class="Ancho3Opciones" style="text-align: center;margin: 0 auto; display: none;">
            <asp:Button ID="btnRegistrarMantto" runat="server" class="btnWizardGuardar"
                Text="GUARDAR" onclick="btnRegistrarMantto_Click"/>
        </div>
        <a id="btnRedireccionConsulta" target="_self" style="display:none;" href="ProgramarMantenimientosUI.aspx">Link</a>
    </div>
</div>
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
    function ColocarBotones(){
        var btnGuardar = $("#divBoton").html();
        $("#grpOpciones").html(btnGuardar);
    }
    function Redireccion(){
        $("#<%= btnRegistrarMantto.ClientID %>").css("display", "none");
        __blockUI();
        document.getElementById("btnRedireccionConsulta").click();
    }
    initChild = function () {
        <%= ucDatosCitaMantenimiento.ClientID %>_Inicializar();
        ColocarBotones();
    };
    $(document).ready(initChild);
</script>
</asp:Content>
