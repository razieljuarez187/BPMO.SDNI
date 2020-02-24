<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="DetallePeriodoTarifarioPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.DetallePeriodoTarifarioPSLUI" %>

<%@ Register Src="~/Contratos.PSL.UI/ucPeriodoTarifarioPSLUI.ascx" TagPrefix="uc" TagName="ucPeriodoTarifarioPSLUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/EstiloCatTarifas.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" id="JQuerySection">
        $(document).ready(function () {
            initChild();
            ConfiguracionBarraHerramientas();
        });

        function initChild() {
            $("span:contains('*')").text("");
        }
        
    </script>
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
                    <asp:Button runat="server" ID="btnEditar" Text="Editar" OnClick="btnEditar_Click" CssClass="btnWizardEditar" />
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
                               <asp:DropDownList runat="server" Enabled="false" ID="ddlUnidadOperativa" Width="380px" AutoPostBack="True" OnSelectedIndexChanged="ddlUnidadOperativa_SelectedIndexChanged">
                                    </asp:DropDownList>
                            </td>
                        </tr>        
                    </table>
                    <uc:ucPeriodoTarifarioPSLUI runat="server" ID="ucPeriodoTarifarioPSL" />
                </fieldset>
            </asp:Panel>
            <fieldset id="fsDatosSistema" style="display:none;">
                <legend>Datos del Sistema</legend>
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical" style="width: 180px;">Fecha Registro</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 200px;">
                            <asp:TextBox runat="server" ID="txtFechaRegistro" Enabled="False" CssClass="CampoFecha" Width="180px"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Usuario Registro</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtUsuarioRegistro" Enabled="False" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 180px;">Fecha Modificación</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 200px;">
                            <asp:TextBox runat="server" ID="txtFechaModificacion" Enabled="False" CssClass="CampoFecha" Width="180px"></asp:TextBox>
                        </td>
                        <td class="tdCentradoVertical" style="text-align: right;">Usuario Modificación</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 280px;">
                            <asp:TextBox runat="server" ID="txtUsuarioModificacion" Enabled="False" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 180px;">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" colspan="4">
                            <asp:TextBox runat="server" ID="txtEstatus" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnPeriodoTarifarioPSLID" />
    <asp:HiddenField runat="server" ID="hdnUnidadOperativaSeleccionadaID" />
</asp:Content>
