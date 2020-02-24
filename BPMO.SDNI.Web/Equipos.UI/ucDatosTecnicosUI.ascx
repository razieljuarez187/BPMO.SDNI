<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosTecnicosUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucDatosTecnicosUI" %>
<fieldset class="ColumnaIzquierda">
    <legend>Odómetro</legend>
    <div style="display: inline-block;">
        <span style="float: none;">Km. Inicial</span>
        <asp:TextBox ID="txtKilometrosInicial" Width="70px" runat="server" CssClass="CampoNumeroEntero" style="float: none;" MaxLength="9"></asp:TextBox>
        <span style="float: none; padding-left: 6px;">Km. Final</span>
        <asp:TextBox ID="txtKilometrosFinal" Width="70px" runat="server" CssClass="CampoNumeroEntero" style="float: none;" MaxLength="9"></asp:TextBox>
    </div>
    <div style="margin-top: 5px;">
        <span>¿Activo?</span>&nbsp;&nbsp;
        <asp:RadioButton ID="rbKmActivoSi" runat="server" GroupName="KmActivo" Checked="true" Text="SI" />
        <asp:RadioButton ID="rbKmActivoNo" runat="server" GroupName="KmActivo" Text="NO" />
        <asp:Button ID="btnAgregarOdometro" runat="server" Text="Agregar" CssClass="btnAgregarMediano" onclick="btnAgregarOdometro_Click" />
    </div>
    <div class="Contenedor">
        <asp:GridView ID="grvOdometros" runat="server" CssClass="Grid" style="width: 100%;"
            AutoGenerateColumns="False" onrowcommand="grvOdometros_RowCommand" 
            onrowdatabound="grvOdometros_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Km Inicial</HeaderTemplate>
                    <ItemTemplate>
                        <%#string.Format("{0:#,##0}", DataBinder.Eval(Container, "DataItem.KilometrajeInicio"))%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Km Final</HeaderTemplate>
                    <ItemTemplate>
                        <%#string.Format("{0:#,##0}", DataBinder.Eval(Container, "DataItem.KilometrajeFin"))%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>¿Activo?</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Activo").ToString().Replace("True", "Si").Replace("False", "No")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                            ToolTip="Eliminar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
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
    </div>
</fieldset>
<fieldset class="ColumnaDerecha">
    <legend>Horómetro</legend>
    <div style="display: inline-block;">
        <span style="float: none;">Hr. Inicial</span>
        <asp:TextBox ID="txtHorasInicial" Width="70px" runat="server" CssClass="CampoNumeroEntero" style="float: none;" MaxLength="9"></asp:TextBox>
        <span style="float: none;">Hr. Final</span>
        <asp:TextBox ID="txtHorasFinal" Width="70px" runat="server" CssClass="CampoNumeroEntero" style="float: none;" MaxLength="9"></asp:TextBox>
    </div>    
    <div style="margin-top: 5px;">
        <span>¿Activo?</span>&nbsp;&nbsp;
        <asp:RadioButton ID="rbHrActivoSi" runat="server" GroupName="HrActivo" Checked="true" Text="SI" />
        <asp:RadioButton ID="rbHrActivoNo" runat="server" GroupName="HrActivo" Text="NO" />
        <asp:Button ID="btnAgregarHorometro" runat="server" Text="Agregar" CssClass="btnAgregarMediano" onclick="btnAgregarHorometro_Click" />
    </div>
    <div class="Contenedor">
        <asp:GridView ID="grvHorometros" runat="server" CssClass="Grid" style="width: 100%;"
            AutoGenerateColumns="False" onrowcommand="grvHorometros_RowCommand" 
            onrowdatabound="grvHorometros_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Hr Inicial</HeaderTemplate>
                    <ItemTemplate>
                        <%#string.Format("{0:#,##0}",  DataBinder.Eval(Container, "DataItem.HoraInicio"))%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Hr Final</HeaderTemplate>
                    <ItemTemplate>
                        <%# string.Format("{0:#,##0}", DataBinder.Eval(Container, "DataItem.HoraFin"))%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>¿Activo?</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Activo").ToString().Replace("True", "Si").Replace("False", "No")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png"
                            ToolTip="Eliminar" CommandName="Eliminar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
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
    </div>
</fieldset>

<div style="display: inline-block; margin-bottom: 5px;">
    <div class="ColumnaIzquierda" style="margin-top: 5px;">
        <div class="Etiqueta">
            <span>*</span><span>PBV Máx. Recomendado</span>
            <asp:TextBox ID="txtPBV" Width="115px" runat="server" CssClass="CampoNumero" MaxLength="13"></asp:TextBox>
        </div>
    </div>
    <div class="ColumnaDerecha" style="margin-top: 5px;">
        <div class="Etiqueta">
            <span>*</span><span>Capacidad de Tanque</span>
            <asp:TextBox ID="txtCapacidadTanque" Width="115px" runat="server" CssClass="CampoNumero" MaxLength="13"></asp:TextBox>
        </div>
    </div>
    <div class="ColumnaIzquierda" style="margin-top: 5px;">
        <div class="Etiqueta">
            <span>*</span><span>PBC Máx. Recomendado</span>
            <asp:TextBox ID="txtPBC" Width="115px" runat="server" CssClass="CampoNumero" MaxLength="13"></asp:TextBox>
        </div>
    </div>
    <div class="ColumnaDerecha" style="margin-top: 5px;">
        <div class="Etiqueta">
            <span>*</span><span>Rendimiento de Tanque</span>
            <asp:TextBox ID="txtRendimientoTanque" Width="115px" runat="server" CssClass="CampoNumero" MaxLength="13"></asp:TextBox>
        </div>
    </div>
</div>