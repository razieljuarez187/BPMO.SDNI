<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuscadorUI.aspx.cs" Inherits="BPMO.SDNI.Buscador.UI.BuscadorUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Buscador</title>
    <asp:Literal ID="ltEstilo" runat="server" Text="<link  href='../CSS/EstiloDesarrollo.css' rel='Stylesheet' type='text/css'/>"></asp:Literal>
    <script src="../Contenido/Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.jlabel-1.3.min.js" type="text/javascript"></script>
    <style type="text/css">
        .BotonReset
        {
            margin: 3px 15px 0;
            float: right;
            font-weight: bold;
            border: none;
            background-color: #5c5e5d;
            color: White;
            padding: 3px;     
            text-transform:uppercase;
            font-size:11px;      
        }        
        .BotonReset:hover
        {
            opacity:0.7;
            color:Yellow;
            cursor: pointer;
        }
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
            var arrHeader = $("#<%=grdHeader.ClientID%>").find("th").toArray();
            if ($("#<%=grdBuscador.ClientID%>")[0].rows.length > 1) {
                var arrBusca = $("#<%=grdBuscador.ClientID%>").find("tr:first");
                var tr = $("<tr/>");
                jQuery.each(arrHeader, function (index, value) {
                    var txt = $(value).find("input[type = 'text']");
                    txt.css("width", value.clientWidth <= 100 ? '50%' : '85%');
                    tr.append(value);
                });
                arrBusca.before(tr);
            } else {
                jQuery.each(arrHeader, function (index, value) {
                    var txt = $(value).find("input[type = 'text']");
                    txt.css("width", value.clientWidth <= 100 ? '50%' : '85%');
                });
            }
            $("input[type='text']").keypress(function (e) {
                if (e.which == 13) {
                    CallSearch(this);
                    esEnter = true;
                }
            });
            if (esEnter == true) esEnter = false;
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
            var atribtTxt = $(text).context.name.substring($(text).context.name.lastIndexOf("txt") + 3, $(text).context.name.length) + "'";
            var hdn = $("#<%=hdnFiltro.ClientID %>").val();
            //Buscar dentro del hdn la atrib y ponerle su valor
            var atribtFiltro = hdn.substring(0, hdn.indexOf(atribtTxt) + atribtTxt.length);
            var valor = hdn.substring(hdn.indexOf(atribtTxt) + atribtTxt.length, hdn.length);
            var resultCadena = valor.substring(valor.indexOf("'"), hdn.length);
            hdn = atribtFiltro + $(text).val() + resultCadena;
            $("#<%=hdnFiltro.ClientID %>").val(hdn);
            btnFiltro_Click();
        }
        function btnFiltro_Click() {
            $("#<%=btnFiltro.ClientID %>").click();
        }
        function JLabelTxt() {
            try {
                $(':text').jLabel({ speed: 500, opacity: 0.1 });
            } catch (ex) { }
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
        function closeParentUI() {
            $("#hdnShowBuscador", parent.document).val("0");
            if (parent.btnTrigger !== undefined && parent.btnTrigger !== null && parent.btnTrigger.length > 0)
                $(parent.btnTrigger, parent.document)[0].click();
            else
                $("input[name$='btnResult']", parent.document)[0].click();
        }
        function pageLoad() {
            $(':input').css({ 'border-radius': '5px' });
            $("span:contains('*')").css('font-size', '12px').addClass('ColorValidator');
            $("#txtAux").focus();
            $(':input[type="text"]:first').focus();
            $("#txtAux").hide();
        }
        function LimpiarText() {
            var hdn = $("#<%=hdnFiltro.ClientID %>").val();
            $("#<%=grdHeader.ClientID%>").find(":text").each(function () {
                $text = $(this).val();
                if ($text.length > 0) {
                    hdn = hdn.replace("'" + $text + "'", "''");
                    $(this).val('');
                }
            });
            $("#<%=hdnFiltro.ClientID %>").val(hdn);
        }
    </script>
</head>
<body style="font-size: 11px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smScriptManejador" runat="server" />
    <script src="../Contenido/Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="UPContenedor" runat="server">
        <ContentTemplate>
            <div id="divMsj" style="display: none; text-align: center; color: Red">
                <br />
                <br />
            </div>
            <div id="DatosDelCliente" class="GroupBody" style="margin: 20px auto; width: 95%;">
                <div id="EncabezadoDatosDelCliente" class="GroupHeader" style="width: 100%;">
                    <asp:Label ID="lblTitulo" runat="server" Text="" Style="margin: 4px 15px 0; float: left;
                        font-weight: bold"></asp:Label>
                    <asp:Button ID="btnReiniciarFiltro" runat="server" Text="Reiniciar busqueda" CssClass="BotonReset"
                        OnClick="btnReiniciarFiltro_Click" />
                    <asp:CheckBox ID="cbBuscaEnBD" runat="server" Text="Filtrar sobre consulta" Style="margin: 4px 15px 0;
                        float: right;" Visible="False" />
                </div>
                <div id="DatosClienteControles" style="min-height: 150px; height: 95%; overflow: auto;">
                    <asp:GridView ID="grdHeader" runat="server" AutoGenerateColumns="False" CellPadding="2"
                        GridLines="None" CssClass="Grid" HorizontalAlign="Center" Width="100%" Style="border: none;
                        background-color: transparent;">
                        <RowStyle CssClass="GridRow" />
                    </asp:GridView>
                    <asp:GridView ID="grdBuscador" runat="server" CellPadding="4" GridLines="None" CssClass="Grid"
                        HorizontalAlign="Center" AutoGenerateSelectButton="False" AutoGenerateColumns="false"
                        OnSorting="grdBuscador_Sorting" Width="100%" OnPageIndexChanging="grdBuscador_PageIndexChanging">
                        <HeaderStyle CssClass="GridHeaderGral" />
                        <EditRowStyle CssClass="GridAlternatingRow" />
                        <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                        <RowStyle CssClass="GridRow" />
                        <FooterStyle CssClass="GridFooter" />
                        <SelectedRowStyle CssClass="GridSelectedRow" />
                        <AlternatingRowStyle CssClass="GridAlternatingRow" />
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <div style="width: 100%; text-align: right;">
                <span style="margin-right: 40px;">(*) Campos requeridos o Formato incorrecto</span></div>
            <asp:Button ID="btnFiltro" runat="server" Text="Filtrar" OnClick="btnBuscar_Click"
                Style="display: none;" ValidationGroup="Requeridos" />
            <asp:Button ID="btnSelect" runat="server" Text="Seleccionar" OnClick="btnSelect_Click"
                Style="display: none;" />
            <asp:HiddenField ID="hdnFiltro" runat="server" />
            <asp:HiddenField ID="hdnSelect" runat="server" />
            <input id="txtAux" type="text" title=" " />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
