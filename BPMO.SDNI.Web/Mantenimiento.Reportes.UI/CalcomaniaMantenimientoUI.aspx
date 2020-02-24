<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="CalcomaniaMantenimientoUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.CalcomaniaMantenimientoUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
    <style type="text/css">
    .GroupSection { width: 650px; margin: 0px auto; }
    .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
    .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }
    .Grid { width: 90%; margin: 25px auto 15px auto; }  
    .GridPager table { margin: 0px !important; width: 5px !important; }
</style>
<script type="text/javascript">
    $(document).ready(function () { initChild(); });

    function initChild() {
        inicializeHorizontalPanels();
    }

    function inicializeHorizontalPanels() {
        $(".GroupHeaderCollapsable").click(function () {
            $(this).next(".GroupContentCollapsable").slideToggle(500);
            if ($(this).find(".imgMenu").attr("src") == "../Contenido/Imagenes/FlechaArriba.png")
                $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaAbajo.png");
            else
                $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaArriba.png");
            return false;
        });
    }
    </script>
<script type="text/javascript">
    function BtnBuscarUnidad(guid, xml, sender) {
        var width = ObtenerAnchoBuscador(xml);

        $.BuscadorWeb({
            xml: xml,
            guid: guid,
            btnSender: $("#" + sender),
            features: {
                dialogWidth: width,
                dialogHeight: '320px',
                center: 'yes',
                maximize: '0',
                minimize: 'no'
            }
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="encabezadoLeyenda" runat="server">
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de calcomanías sobre mantenimientos registrados</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    ¿Qué calcomanía de mantenimiento desea consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">

    <tr>
        <td class="tdCentradoVertical">
            # Econ&oacute;mico
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="txtNumeroEconomico" runat="server" MaxLength="50" Width="275px" 
            AutoPostBack="true" OnTextChanged="txtNumeroEconomico_TextChanged"></asp:TextBox>
            <asp:ImageButton runat="server" ID="btnBuscarNumEconomico" CommandName="VerNumEconomico"
                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Num. Económico"
                CommandArgument='' onclick="btnBuscarNumEconomico_Click" />
        </td>
    </tr>
    <tr>
        <td><asp:HiddenField ID="hdnLibroActivos" runat="server" /></td>
    </tr>

    <asp:Button ID="btnResult2" runat="server" Text="Button" OnClick="btnResult2_Click" Style="display: none;" />


    
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ResultadosAbajo" runat="server">

 <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvUnidades" runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="false"
                EnableSortingAndPagingCallbacks="true" CssClass="Grid" OnPageIndexChanging="gvUnidades_PageIndexChanging"
                OnRowCommand="gvUnidades_RowCommand" >
                    <Columns>
                        <asp:BoundField DataField="OrdenID" HeaderText="OrdenServicio" SortExpression="Modelo">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Serie" HeaderText="VIN/Número Económico" SortExpression="Modelo">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="NombreCompleto" HeaderText="Cliente" SortExpression="Modelo">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnVer" CommandName="Detalles" ImageUrl="~/Contenido/Imagenes/VER.png"
                                    ToolTip="Ver reporte" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                    ImageAlign="Middle" />
                            </ItemTemplate>
                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                    </Columns>
                   
                   <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />

                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>


