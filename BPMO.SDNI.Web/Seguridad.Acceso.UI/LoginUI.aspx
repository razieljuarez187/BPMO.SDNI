<%@ Page Title="LoginUI" Language="C#" AutoEventWireup="true" MasterPageFile="~/Contenido/MasterPages/Lite.Master"
    CodeBehind="LoginUI.aspx.cs" Inherits="BPMO.SDNI.Seguridad.Acceso.UI.LoginUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function __showLoading() {
            $('#<%= this.lblMensajeError.ClientID %>').text("");

            if (Page_ClientValidate()) {
                $("#trLoading").show();
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dvCajaLibreGrande">
        <div class="Header"> </div>
        <div class="Content">           
            <asp:Panel ID="pnlMensajeError" CssClass="Error" runat="server">
                <asp:Label ID="lblMensajeError" runat="server"></asp:Label>
            </asp:Panel>
            <table>
                <tr>
                    <td>USUARIO</td>
                    <td>
                        <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ErrorMessage="*" ControlToValidate="txtUsuario"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>CONTRASEÑA</td>
                    <td>
                        <asp:TextBox ID="txtContrasenia" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvContrasenia" runat="server" ErrorMessage="*" ControlToValidate="txtContrasenia"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trLoading" style="display:none">
                    <td colspan="2" style="text-transform:none; color:#9F6000">
                        <asp:Panel ID="pnlLoading" runat="server" style="text-align:center">
                            <asp:Image ID="imgLoading" ImageAlign="AbsMiddle" ImageUrl="~/Contenido/Imagenes/Cargando.gif" runat="server" />
                            Por favor espere un momento
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnAceptar" runat="server" Text="" CssClass="btnEntrar" OnClick="btnAceptar_Click" OnClientClick="__showLoading();" />
        </div>
        <div class="Footer"> </div>
    </div>
</asp:Content>
