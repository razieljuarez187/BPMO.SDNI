<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSeguroUI.ascx.cs" Inherits="BPMO.SDNI.Tramites.UI.ucSeguroUI" %>
<%@ Register Src="~/Tramites.UI/ucEndosoSeguroUI.ascx" TagPrefix="uc" TagName="ucEndosoSeguroUI" %>
<%@ Register Src="~/Tramites.UI/ucSiniestroSeguroUI.ascx" TagPrefix="uc" TagName="ucSiniestroSeguroUI" %>
<%@ Register Src="~/Tramites.UI/ucDeducibleSeguroUI.ascx" TagPrefix="uc" TagName="ucDeducibleSeguroUI" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#tabs").tabs();
        initPage();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onRequestEnd)
    });

    function ActivarSiniestro() {
        $("#tabs").tabs("option","active",2);
    }
    function initPage() {
        if (!$('#<%=txtVigenciaInicial.ClientID %>').is(':disabled') && !$('#<%=txtVigenciaFinal.ClientID %>').is(':disabled')) {
            $('#<%=txtVigenciaInicial.ClientID %>').datepicker({
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "VIGENCIA INICIAL",
                showOn: 'button'
            });
            $('#<%=txtVigenciaFinal.ClientID %>').datepicker({
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "VIGENCIA FINAL",
                showOn: 'button'
            });   
        }
        $('.CampoFecha').attr('readonly', true);
    }

    function onRequestStart() {
    }

    function onRequestEnd() {
        if (!$('#<%=txtVigenciaInicial.ClientID %>').is(':disabled') && !$('#<%=txtVigenciaFinal.ClientID %>').is(':disabled')) {
            $('#<%=txtVigenciaInicial.ClientID %>').datepicker({
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "VIGENCIA INICIAL",
                showOn: 'button'
            });
            $('#<%=txtVigenciaFinal.ClientID %>').datepicker({
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "VIGENCIA FINAL",
                showOn: 'button'
            });
        }
        $("#tabs").tabs();
        $('.CampoFecha').attr('readonly', true);
    } 
</script>

<fieldset id="generales">
    <legend>GENERALES</legend>
    <table class="trAlinearDerecha" style="margin: 0px auto;">
        <tr>
            <td class="tdCentradoVertical"><span>*</span>VIN</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtVIN" runat="server" ToolTip="Número de Serie" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>MODELO</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtModelo" runat="server" ToolTip="Modelo" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span># P&Oacute;LIZA</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtNumPoliza" runat="server" ToolTip="Numero de poliza" Width="270px" MaxLength="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>ASEGURADORA</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtAseguradora" runat="server" ToolTip="Aseguradora" Width="270px" MaxLength="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>TEL&Eacute;FONO</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtContacto" runat="server" ToolTip="Contacto" Width="270px" MaxLength="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>PRIMA ANUAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtPrimaAnual" runat="server" CssClass="CampoMoneda" ToolTip="Prima Anual" Width="95px" MaxLength="13"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>PRIMA SEMESTRAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtPrimaSemestral" runat="server" CssClass="CampoMoneda" ToolTip="Prima Semestral" Width="95px" MaxLength="13"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>VIGENCIA INICIAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtVigenciaInicial" runat="server" CssClass="CampoFecha" ToolTip="Vigencia Inicial" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>VIGENCIA FINAL</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtVigenciaFinal" runat="server" CssClass="CampoFecha" ToolTip="Vigencia Final" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdCentradoVertical"><span>*</span>OBSERVACIONES</td>
            <td style="width: 20px;">&nbsp;</td>
            <td class="tdCentradoVertical" style="width: 280px;">
                <asp:TextBox ID="txtObservacion" runat="server" ToolTip="Observaciones" MaxLength="250" Width="270px" style="max-width: 270px; min-width: 270px;"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>
<div id="tabs">
    <ul>
		<li><a href="#tabs-1">Endosos</a></li>
		<li><a href="#tabs-2">Deducibles</a></li>
		<li><a href="#tabs-3">Siniestros</a></li>
	</ul>
    <div id="tabs-1">
        <uc:ucEndosoSeguroUI runat="server" ID="ucucEndosoSeguroUI" />
    </div>
    <div id="tabs-2">
        <uc:ucDeducibleSeguroUI runat="server" ID="ucucDeducibleSeguroUI" /> 
    </div>
    <div id="tabs-3">
        <uc:ucSiniestroSeguroUI runat="server" ID="ucucSiniestroSeguroUI" />
    </div>
</div>

<asp:HiddenField ID="hdnTramiteID" runat="server" />
<asp:HiddenField ID="hdnTramitableID" runat="server" />
<asp:HiddenField ID="hdnTipoTramitable" runat="server" />
<asp:HiddenField ID="hdnModo" runat="server" />