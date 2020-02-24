<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDetalleConfiguracionParametrosAuditoriaUI.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucDetalleConfiguracionParametrosAuditoriaUI" %>
 

                <%@ Register src="ucConfiguracionParametrosMantenimientoUI.ascx" tagname="ucConfiguracionParametrosMantenimientoUI" tagprefix="uc1" %>

 

                <table class="trAlinearDerecha table-responsive"> 
                    <uc1:ucConfiguracionParametrosMantenimientoUI ID="ucConfiguracionParametrosMantenimientoUI1" 
    runat="server" />                               
                      <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>NUM ACTIVIDADES ALEATORIAS</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtAleatorias" runat="server" CssClass="CampoNumeroEntero"></asp:TextBox>
                        </td>
                    </tr>             
                </table>
            
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto">-</span>                  
                </div>
            <fieldset>
              <legend>CONFIGURADOS</legend>
                  <asp:UpdatePanel ID="UpConfigurados" runat="server">
        <ContentTemplate>
            <div style="margin: 0 auto; width:80% !important; overflow: auto;">
                <asp:GridView ID="gvConfigurados" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" OnRowCommand="gvConfigurados_RowCommand"
                    OnPageIndexChanging="gvConfigurados_PageIndexChanging" 
                    onrowdatabound="gvConfigurados_RowDataBound" ShowHeaderWhenEmpty="True" 
                    PageSize="20">
                    <Columns>
                          <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;TIPO DE SERVICIO&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbTipoMantenimiento" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimiento") %>'></asp:Label> 
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="50%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                          <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;# OBLIGATORIAS&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbActividadesObligatorias" runat="server" Text=''></asp:Label> 
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="20%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;# ALEATORIAS&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbActividadesAleatorias" runat="server" Text=''></asp:Label> 
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="20%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="&nbsp;&nbsp;Quitar&nbsp;&nbsp;">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEliminar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="" ImageAlign="Middle" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"/>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>

                    </Columns>
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
            </fieldset>
               <br />
            <fieldset>
              <legend>ACTIVIDADES A AUDITAR</legend>
            <asp:UpdatePanel ID="UPContenedor" runat="server">           
        <ContentTemplate>
            <div style="margin: 0 auto; width:90% !important; overflow: auto;">
                <asp:GridView ID="gvActividades" runat="server" AutoGenerateColumns="False" CssClass="Grid" 
                    AllowPaging="True" EnableViewState="False" OnRowCommand="gvActividade_RowCommand"
                    OnPageIndexChanging="gvActividade_PageIndexChanging" 
                    onrowdatabound="gvActividade_RowDataBound" ShowHeaderWhenEmpty="True">
                    <Columns>
                         <asp:TemplateField HeaderText="&nbsp;&nbsp;Actividades&nbsp;&nbsp;">
                            <ItemTemplate>
                             <asp:Label ID="lbActividadAuditoria" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ConfiguracionPosicionTrabajo.DescriptorTrabajo.Nombre") %>'  Width="400px">></asp:Label>                     
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50%"/>              
                        </asp:TemplateField>
                          <asp:TemplateField Visible="false" >
                            <ItemTemplate>
                             <asp:Label ID="lbActividadID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ConfiguracionPosicionTrabajo.Id") %>'></asp:Label> 
                            </ItemTemplate>
                             <HeaderStyle HorizontalAlign="Center" Width="10%"/>              
                        </asp:TemplateField>
                          <asp:TemplateField>
                             <HeaderTemplate>&nbsp;&nbsp;CRITERIO&nbsp;&nbsp;</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txbCriterioActividad" runat="server"  style="width:100%; text-align: center !important;"></asp:TextBox>
                            </ItemTemplate  >
                             <HeaderStyle HorizontalAlign="Center" Width="40%"/>
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>        
                        <asp:TemplateField>
                        <HeaderTemplate>&nbsp;&nbsp;OBLIGATORIA&nbsp;&nbsp;</HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chbxObligatoria" runat="server" />
                        </ItemTemplate>   
                        </asp:TemplateField> 
                    </Columns>
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
                <br />   
            </fieldset>      
