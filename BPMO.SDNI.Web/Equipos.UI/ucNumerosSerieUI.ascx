<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNumerosSerieUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucNumerosSerieUI" %>
<div style="display: inline-block;">
    <div class="ColumnaIzquierda" style="margin-bottom: 5px;">
        <div class="Etiqueta"><span>*</span><span>Radiador</span></div>  
        <asp:TextBox ID="txtRadiador" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
    </div>
    <div class="ColumnaDerecha" style="margin-bottom: 5px;">
        <div class="Etiqueta"><span>*</span><span>Post Enfriador</span></div>  
        <asp:TextBox ID="txtPostEnfriador" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
    </div>    

    <div style="display: inline-block; width: 100%;">
        <fieldset class="ColumnaIzquierda" style="margin-bottom: 5px;">
            <legend>Motor</legend>
             <div class="Etiqueta"><span>*</span><span>Serie Motor</span></div>  
            <asp:TextBox ID="txtSerieMotor" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
            <br />
            <div class="Etiqueta"><span>*</span><span>Serie Turbo Cargador</span></div>  
            <asp:TextBox ID="txtSerieTurboCargador" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
            <br />
            <div class="Etiqueta"><span>*</span><span>Serie Compresor de Aire</span></div>  
            <asp:TextBox ID="txtCompresorAire" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
            <br />
            <div class="Etiqueta"><span>*</span><span>Serie ECM</span></div>  
            <asp:TextBox ID="txtECM" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
        </fieldset>
        <fieldset class="ColumnaDerecha" style="margin-bottom: 5px;">
            <legend>Sistema Eléctrico</legend>
            <div class="Etiqueta"><span>*</span><span>Serie Alternador</span></div>  
            <asp:TextBox ID="txtAlternador" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
            <br />
            <div class="Etiqueta"><span>*</span><span>Serie Marcha</span></div>  
            <asp:TextBox ID="txtMarcha" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
            <br />
            <div class="Etiqueta"><span>*</span><span>Serie Baterías</span></div>  
            <asp:TextBox ID="txtBaterias" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
        </fieldset>
    </div>

    <fieldset class="ColumnaIzquierda" style="margin-bottom: 5px;">
        <legend>Transmisión</legend>
        <div class="Etiqueta"><span>*</span><span>Serie</span></div>  
        <asp:TextBox ID="txtSerieTransmision" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
        <br />
        <div class="Etiqueta"><span>*</span><span>Modelo</span></div>  
        <asp:TextBox ID="txtModeloTransmision" Width="325px" runat="server" MaxLength="250"></asp:TextBox>
    </fieldset>
    <fieldset class="ColumnaDerecha" style="margin-bottom: 5px;">
        <legend>Eje Dirección</legend>
        <div class="Etiqueta"><span>*</span><span>Serie</span></div>  
        <asp:TextBox ID="txtSerieEjeDireccion" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
        <br />
        <div class="Etiqueta"><span>*</span><span>Modelo</span></div>  
        <asp:TextBox ID="txtModeloEjeDireccion" Width="325px" runat="server" MaxLength="250"></asp:TextBox>
    </fieldset>

    <fieldset class="ColumnaIzquierda" style="margin-bottom: 5px;">
        <legend>Eje Trasero Delantero</legend>
        <div class="Etiqueta"><span>*</span><span>Serie</span></div>  
        <asp:TextBox ID="txtSerieEjeTraseroDelantero" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
        <br />
        <div class="Etiqueta"><span>*</span><span>Modelo</span></div>  
        <asp:TextBox ID="txtModeloEjeTraseroDelantero" Width="325px" runat="server" MaxLength="250"></asp:TextBox>
    </fieldset>
    <fieldset class="ColumnaDerecha" style="margin-bottom: 5px;">
        <legend>Eje Trasero Trasero</legend>
        <div class="Etiqueta"><span>*</span><span>Serie</span></div>  
        <asp:TextBox ID="txtSerieEjeTraseroTrasero" Width="325px" runat="server" MaxLength="150"></asp:TextBox>
        <br />
        <div class="Etiqueta"><span>*</span><span>Modelo</span></div>  
        <asp:TextBox ID="txtModeloEjeTraseroTrasero" Width="325px" runat="server" MaxLength="250"></asp:TextBox>
    </fieldset>

    <fieldset class="ColumnaIzquierda">
    <legend>Numeros Serie Adicionales</legend>
    <div style="display: inline-block;">
        <span style="float: none;">Nombre</span>
        <asp:TextBox ID="txtNombre" Width="70px" runat="server" style="float: none;" MaxLength="150"></asp:TextBox>
        <span style="float: none; padding-left: 6px;">Serie</span>
        <asp:TextBox ID="txtSerie" Width="70px" runat="server" style="float: none;" MaxLength="150"></asp:TextBox>
    </div>
    
    <div style="margin-top: 5px;">
        <asp:Button ID="btnAgregarNumeroSerie" runat="server" Text="Agregar" CssClass="btnAgregarMediano" onclick="btnAgregarNumeroSerie_Click" />
    </div>

    <div class="Contenedor">
        <asp:GridView ID="grvNumeroSerie" runat="server" CssClass="Grid" style="width: 100%;"
            AutoGenerateColumns="False" onrowcommand="grvNumeroSerie_RowCommand" 
            onrowdatabound="grvNumeroSerie_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Nombre</HeaderTemplate>
                    <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.Nombre")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Serie</HeaderTemplate>
                    <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.NumeroSerie")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
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

</div>
