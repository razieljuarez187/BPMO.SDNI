<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConfiguracionParametrosMantenimientoUI.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucConfiguracionParametrosMantenimientoUI" %>
                   <script type="text/javascript">
                       function BtnBuscar(guid, xml) {
                           var width = ObtenerAnchoBuscador(xml);

                           $.BuscadorWeb({
                                xml: xml,
                                guid: guid,
                                btnSender: $("#<%=btnResult.ClientID %>"),
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
                <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
                <asp:HiddenField ID="hdnLibroActivos" runat="server" />
                     <tr>
                        <td class="tdCentradoVertical input-label-responsive">SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" 
    MaxLength="30" CssClass="input-text-responsive" AutoPostBack="True" 
    ontextchanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument='' OnClick="btnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">TALLER</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTalleres" runat="server" 
    CssClass="input-dropdown-responsive" Enabled="False"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">MODELO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtModelo" runat="server" MaxLength="30" 
                                CssClass="input-find-responsive" 
    AutoPostBack="True" ontextchanged="txtModelo_TextChanged" ></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarModelo" CommandName="VerModelo"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelos" CommandArgument='' OnClick="btnBuscarModelo_Click" />
                            <asp:HiddenField ID="hdnModeloID" runat="server" Visible="False" />
                        </td>
                    </tr>
              
               