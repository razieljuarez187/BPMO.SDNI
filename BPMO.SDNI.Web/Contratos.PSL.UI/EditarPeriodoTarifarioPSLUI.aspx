<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarPeriodoTarifarioPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.EditarPeriodoTarifarioPSLUI" %>

<%@ Register Src="~/Contratos.PSL.UI/ucPeriodoTarifarioPSLUI.ascx" TagPrefix="uc" TagName="ucTarifaPSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloCatTarifas.css" rel="stylesheet" type="text/css" />
    <script src="../Contenido/Scripts/ObtenerFormatoImporte.js" type="text/javascript"></script>   
    <script language="javascript" type="text/javascript" id="JQuerySection">
        $(document).ready(function () {
            initChild();
            ConfiguracionBarraHerramientas();
        });
    </script>
    <script language="javascript" type="text/javascript" id="JavaScriptFunctions">
        initChild = function () {
            ConfiguracionBarraHerramientas();
           
        };
            
    </script>
    <style type="text/css">
        .style1
        {
            height: 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label runat="server" ID="lblEncabezadoLeyenda">CAT&Aacute;LOGOS - CONFIGURACI&Oacute;N DE PER&Iacute;ODOS TARIFARIOS</asp:Label>
        </div>
        <div style="height: 65px;"></div>
        <div id="divInformacionGeneral" class="GroupBody">
            <div id="divInformacionHeader" class="GroupHeader">
                <span>CONFIGURACI&Oacute;N DE PER&Iacute;ODOS TARIFARIOS</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click"
                        CssClass="btnWizardGuardar" />
                    <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" OnClick="btnCancelar_Click"
                        CssClass="btnWizardCancelar" />
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlCapturaTarifas">
                <fieldset id="fsTarifas">
                    <legend>PER&Iacute;ODOS</legend>
                    <table class="trAlinearDerecha">
                        <tr>
                             <td style="width: 200px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical" style="text-align: right;">
                                  <span>*</span><asp:Label runat="server" ID="lblTurno">Unidad Operativa</asp:Label>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td class="tdCentradoVertical" style="width: 400px;">
                               <asp:DropDownList runat="server" ID="ddlUnidadOperativa" Width="380px" Enabled="false"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <uc:ucTarifaPSLUI runat="server" ID="ucTarifaPSL" />
                </fieldset>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hdnUC" runat="server" />
    <asp:HiddenField ID="hdnFC" runat="server" />
    <asp:HiddenField ID="hdnUUA" runat="server" />
    <asp:HiddenField ID="hdnFUA" runat="server" />
   <asp:HiddenField runat="server" ID="hdnUnidadOperativaSeleccionadaID" />
</asp:Content>
