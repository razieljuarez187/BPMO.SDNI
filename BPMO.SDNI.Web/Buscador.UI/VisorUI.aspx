<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisorUI.aspx.cs" Inherits="BPMO.SDNI.Buscador.UI.VisorUI" %>
<%-- 
	Satisface al CU083 - Consultar Historial de la Unidad
--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Visualizador</title>
	<asp:Literal ID="ltEstilo" runat="server" Text="<link  href='../CSS/EstiloDesarrollo.css' rel='Stylesheet' type='text/css'/>"></asp:Literal>
	<script src="../Contenido/Scripts/jquery-1.8.2.js" type="text/javascript"></script>
	<script src="../Contenido/Scripts/jquery.blockUI.js" type="text/javascript"></script>
	<script src="../Contenido/Scripts/jquery.jlabel-1.3.min.js" type="text/javascript"></script>
	<!-- PARCHE: Estos estilos son para solventar que el buscador acorte el tamaño del texto según el ancho configurado en el XML -->
	<style type="text/css">
		.Grid tbody tr td,
		.Grid tbody tr, th
		{ max-width: 200px; }
		.Grid tbody tr th input
		{ max-width: 160px; }
	</style>

	<script type="text/javascript">
		var esEnter = false;
		$(document).ready(function () {
			InitControls();
			$('form').keypress(function (e) {
				if (e.which == 13) {
					return false;
				}
			});
		});

		function InitControls() {
			$("#<%=grdBuscador.ClientID %>").width($("#<%=grdHeader.ClientID %>").width());
			$("input[type='text']").keypress(function (e) {
				if (e.which == 13) {
					CallSearch(this);
					esEnter = true;
				}
			});
			if (esEnter == true)
				esEnter = false;
		}

		function EventTxt() {
			$("input[type='text']").change(function () {
				if (esEnter == false) {
					CallSearch(this);
				} else {
					esEnter = false;
				}
			});
		}

		function CallSearch(text) {
			var atribtTxt = $(text).context.name.substring($(text).context.name.lastIndexOf("txt") + 3, $(text).context.name.length);
			var hdn = $("#<%=hdnFiltro.ClientID %>").val();
			//Buscar dentro del hdn la atrib y ponerle su valor
			var atribtFiltro = hdn.substring(0, hdn.indexOf(atribtTxt) + atribtTxt.length + 1);
			var valor = hdn.substring(hdn.indexOf(atribtTxt) + atribtTxt.length + 1, hdn.length);
			var resultCadena = valor.substring(valor.indexOf("'"), hdn.length);
			hdn = atribtFiltro + $(text).val() + resultCadena;
			$("#<%=hdnFiltro.ClientID %>").val(hdn);
			btnFiltro_Click();
		}

		function btnFiltro_Click() {
			$("#<%=btnFiltro.ClientID %>").click();
		}

		function JLabelTxt() {
			$(':text').jLabel({ speed: 500, opacity: 0.1 });
		}
		function EventImg() {
			//Valores de Retorno
			$("input[type='image']").click(function (e) {
				var value = $(this).context.name.substring($(this).context.name.lastIndexOf("imgBtn") + 6, $(this).context.name.length);
				$("#<%=hdnSelect.ClientID %>").val(value);
				setTimeout('btnSelect_Click()', 10);
			});
		}

		function btnSelect_Click() {
			$("#<%=btnSelect.ClientID %>").click();
		}
		function pageLoad() {
			$(':input').css({ 'border-radius': '5px' });
			$("span:contains('*')").css('font-size', '12px').addClass('ColorValidator');
			$('input[type="image"]').css({ 'border-width': '0px', 'color': 'transparent', 'width': '17px', 'border-color': 'transparent' });
			$('input[type="image"]').removeAttr('title');
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<asp:ScriptManager ID="smScriptManejador" runat="server">
	</asp:ScriptManager>
	<script src="../Contenido/Scripts/jPage.BlockUI.js" type="text/javascript"></script>
	<asp:UpdatePanel ID="UPContenedor" runat="server">
		<ContentTemplate>
			<div id="divMsj" style="display: none;">
				<br />
				<br />
			</div>
			<div id="DatosDelCliente" class="GroupBody" style="margin: 20px auto; width: 90%;">
				<div id="EncabezadoDatosDelCliente" class="GroupHeader" style="width: 100%; display: inline-block;">
					<asp:Label ID="lblTitulo" runat="server" Text="" Style="margin: 4px 15px 0; float: left;
						font-weight: bold"></asp:Label>
				</div>
				<div id="DatosClienteControles" style="min-height: 150px;">
					<asp:GridView ID="grdHeader" runat="server" AutoGenerateColumns="False" CellPadding="2"
						GridLines="None" CssClass="Grid" HorizontalAlign="Center" Width="100%" Style="border: none;
						background-color: transparent;">
						<RowStyle CssClass="GridRow" />
					</asp:GridView>
					<asp:GridView ID="grdBuscador" runat="server" CellPadding="4" GridLines="None" CssClass="Grid"
						HorizontalAlign="Center" AutoGenerateSelectButton="False" AutoGenerateColumns="false"
						OnSorting="grdBuscador_Sorting"  OnPageIndexChanging="grdBuscador_PageIndexChanging">
						<HeaderStyle CssClass="GridHeaderGral" />
						<EditRowStyle CssClass="GridAlternatingRow" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
						<RowStyle CssClass="GridRow" />
						<FooterStyle CssClass="GridFooter" />
						<SelectedRowStyle CssClass="GridSelectedRow" />
						<AlternatingRowStyle CssClass="GridAlternatingRow" />
						<EmptyDataTemplate>
							<b>No existen registros que cumplan con la condición solicitada, favor de corregir.</b>
						</EmptyDataTemplate>
					</asp:GridView>
				</div>
			</div>
			<div style="width: 100%; text-align: right;">
				<span style="margin-right: 40px;">(*) Campos requeridos o Formato incorrecto</span></div>
			<asp:Button ID="btnFiltro" runat="server" Text="Filtrar" OnClick="btnBuscarVisor_Click"
				Style="display: none;" ValidationGroup="Requeridos" />
			<asp:Button ID="btnSelect" runat="server" Text="Seleccionar" OnClick="btnSelect_Click"
				Style="display: none;" />
			<asp:HiddenField ID="hdnFiltro" runat="server" />
			<asp:HiddenField ID="hdnSelect" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
	</form>
</body>
</html>
