<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="SistemasRevisadosUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.Reportes.UI.SistemasRevisadosUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="childHead" runat="server">
    <style type="text/css">
    .Grid { width: 90%; margin: 25px auto 15px auto; }
</style>
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
    <asp:Label ID="lblEncabezadoLeyenda" runat="server">REPORTES - Reporte de sistemas revisados</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navegacionSecundaria" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="encabezadoFiltrosReporte" runat="server">
    ¿Qué reporte de sistemas revisados deseas consultar?
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="filtrosAdicionalesArriba" runat="server">

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="filtrosAdicionalesAbajo" runat="server">
    <tr>
        <td class="tdCentradoVertical">
            N&Uacute;MERO DE SERIE
        </td>
        <td style="width: 20px;">
            &nbsp;
        </td>
        <td class="tdCentradoVertical" style="width: 320px;">
            <asp:TextBox ID="TextNumeroSerie" runat="server" MaxLength="30" Width="275px" AutoPostBack="True"></asp:TextBox>
            <asp:ImageButton runat="server" ID="btnBuscarNumeroSerie" CommandName="VerNumeroSerie" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                ToolTip="Consultar Numero de Serie" CommandArgument='' OnClick="btnBuscarNumeroSerie_Click"/>
        </td>
    </tr>
    <tr>
          <td class="tdCentradoVertical">
             <span>*</span> FECHA INICIO     
          </td>
          <td class="separadorCampo"></td>
          <td class="tdCentradoVertical">
               <asp:TextBox ID="TextFechaInicio" runat="server" CssClass="CampoFecha"></asp:TextBox>
          </td>                          
    </tr>
     <tr>
          <td class="tdCentradoVertical">
             <span>*</span> FECHA FIN     
          </td>
          <td class="separadorCampo"></td>
          <td class="tdCentradoVertical">
               <asp:TextBox ID="TextFechaFin" runat="server" CssClass="CampoFecha"></asp:TextBox>
          </td>                          
    </tr>
    <asp:Button ID="btnResult2" runat="server" Text="Button" OnClick="btnResult2_Click" Style="display: none;" />
    <asp:HiddenField ID="hdnUnidadID" runat="server" />
    <asp:HiddenField ID="hdnUnidadOperativaID" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />    
</asp:Content>

