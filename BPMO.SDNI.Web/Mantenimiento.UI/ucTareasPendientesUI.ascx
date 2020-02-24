<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTareasPendientesUI.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucTareasPendientesUI" %>
<div id="contenido">
      <table class="trAlinearDerecha table-responsive" style="margin: 0px auto 10px auto; width: auto; border: 1px solid transparent;">
          <tr>
              <td class="tdCentradoVertical input-label-responsive">
                  <span>*</span>N&Uacute;MERO DE SERIE</td>
              <td class="input-space-responsive">&nbsp;</td>
              <td class="tdCentradoVertical input-group-responsive">
      <asp:TextBox ID="TextNumeroSerie" runat="server" OnTextChanged="textNumeroSerie_TextChanged" AutoPostBack="true" CssClass="input-find-responsive"></asp:TextBox>
      <asp:ImageButton runat="server" ID="ibtnBuscarNumeroSerie" CommandName="VerNumeroSerie"
          ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Número de Serie" CommandArgument=''
          OnClick="btnBuscarNumeroSerie_Click" Width="17px" CssClass="" />
              </td>
          </tr>
          <tr>
              <td class="tdCentradoVertical input-label-responsive">
                  N&Uacute;MERO ECON&Oacute;MICO
              </td>
              <td class="input-space-responsive">&nbsp;</td>
              <td class="tdCentradoVertical input-group-responsive">
               <asp:TextBox ID="TextNumeroEconomico" runat="server" OnTextChanged="textNumeroEconomico_TextChanged" AutoPostBack="true" CssClass="input-find-responsive"></asp:TextBox>
               <asp:ImageButton runat="server" ID="idbtnBuscarNumeroEconomico" CommandName="VerNumeroEconomico"
           ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Número Económico" CommandArgument=''
           OnClick="btnBuscarNumeroEconomico_Click" />
              </td>
          </tr>
          <tr>
              <td class="tdCentradoVertical input-label-responsive">
                  MODELO</td>
                  <td class="input-space-responsive">&nbsp;</td>
              <td class="tdCentradoVertical input-group-responsive">
      <asp:TextBox ID="TextModelo" runat="server" OnTextChanged="textModelo_TextChanged" AutoPostBack="true" CssClass="input-find-responsive"></asp:TextBox>
      <asp:ImageButton runat="server" ID="idBtnBuscarModelo" CommandName="VerModelo"
           ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelo" CommandArgument=''
           OnClick="btnBuscarModelo_Click" />
              </td>
          </tr>
          <asp:PlaceHolder ID="PlaceHolderDescripcionRow" Visible="true" runat="server">
             <tr>
              <td class="tdCentradoVertical input-label-responsive"> <span>*</span>
                  DESCRIPCI&Oacute;N </td>
                  <td class="input-space-responsive">&nbsp;</td>
              <td>
                  <asp:TextBox ID="TextDescripcion" runat="server" TextMode="multiline" Columns="50" Rows="10" Width="300px" MaxLength="500" CssClass="input-find-responsive"></asp:TextBox>
              </td>
          </tr>
          </asp:PlaceHolder>
          <tr>
            <td class="tdCentradoVertical input-label-responsive">ESTATUS</td>
            <td class="input-space-responsive">&nbsp;</td>
            <td style="text-align:left;">
                <asp:RadioButton ID="rbtnActivo" Text="Activo" runat="server" GroupName="EstatusTarea" />
                &nbsp;&nbsp;
                <asp:RadioButton ID="rbtnInactivo" Text="Inactivo" runat="server" GroupName="EstatusTarea" />
            </td>
        </tr>
      </table>
      <br />
    <asp:Button ID="btnResult" runat="server" Text="" OnClick="btnResult_Click" Style="display: none;" />
    <asp:HiddenField ID="hdnTareaPendienteID" runat="server" />
    <asp:HiddenField ID="hdnUnidadID" runat="server" />
    <asp:HiddenField ID="hdnModeloID" runat="server" />
    <asp:HiddenField runat="server" ID="hdnUnidadOperativaID"></asp:HiddenField>
    <asp:HiddenField ID="hdnTipoMensaje" runat="server" />
    <asp:HiddenField ID="hdnMensaje" runat="server" />
</div>