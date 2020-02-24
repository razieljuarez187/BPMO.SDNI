<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ReprogramarCitaMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ReprogramarCitaMantenimientoUI" %>
<%@ Register Src="~/Mantenimiento.UI/ucDatosCitaMantenimientoUI.ascx" TagPrefix="uc" TagName="ucDatosCitaUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet"
        type="text/css" />
    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - REPROGRAMAR CITA DE MANTENIMIENTOS</asp:Label>
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
            <uc:ucDatosCitaUI ID="ucDatosCitaMantenimiento" runat="server" />
            <div id="divBoton" style="text-align: center; margin: 0 auto; display:none;">
                <asp:Button ID="btnActualizarMantto" runat="server" class="btnWizardGuardar"
                    Text="GUARDAR" onclick="btnActualizarMantto_Click" />
            </div>
        </div>
    </div>
    
    <asp:UpdatePanel ID="udpConfirmacion" runat="server">
        <ContentTemplate>
            <div id="dialogConfirmacion"  class="GroupBody" runat="server" style="display: none;">
                <div class="rowFull rowMargin">
                    <div class="rowFull">
                        ¿Desea reprogramar la cita con la nueva fecha seleccionada?
                    </div>                    
                </div>
                <br />
                <asp:Button ID="btnConfirmar" runat="server" Text="Button" OnClick="btnConfirmar_Click" Style="display: none;" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnActualizarMantto" EventName="" />
        </Triggers>
    
    </asp:UpdatePanel>

<script type="text/javascript">

    function PresentarDialogConfirm() {

        $("#<%= dialogConfirmacion.ClientID%>").dialog({
                autoOpen: true,
                closeOnEscape: false,
                modal: false,
                title: 'CONFIRMAR RECALENDARIZACIÓN',
                resizable: false,
                minWidth: 450,
                open: function () {
                },
                close: function () {
                    $(this).dialog("destroy");
                },
                buttons:
                [
                  {
                    text: "CANCELAR",
                    click: function () {
                        $(this).dialog("close");
                        }
                  },
                  {
                    text: "REPROGRAMAR",
                    click: function () {
                        __doPostBack("<%= btnConfirmar.UniqueID %>", "");
                        $(this).dialog("close");
                        __blockUI();
                      }
                  }
                ]              
            });
            $("#dialogConfirmacion").parent().appendTo("form:first");

    }

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
    initChild = function () {
        <%= ucDatosCitaMantenimiento.ClientID %>_Inicializar();
        ColocarBotones();
    };
    $(document).ready(initChild);

</script>

</asp:Content>
