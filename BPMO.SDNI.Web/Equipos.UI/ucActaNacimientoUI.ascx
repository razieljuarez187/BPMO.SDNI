<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucActaNacimientoUI.ascx.cs" Inherits="BPMO.SDNI.Equipos.UI.ucActaNacimientoUI" %>
<div class="BarraEstaticaWizard" style="height:370px;">
    <span>Datos de Unidad</span>
    <div runat="server" id="divVIN">
        <br style="display: none;" /><asp:Label runat="server" ID="lblVIN">VIN</asp:Label>
        <br /><asp:TextBox ID="txtNumSerie" runat="server" Text="" Enabled="false"></asp:TextBox>
    </div>
    <span>Clave Activo Oracle</span>
    <br /><asp:TextBox ID="txtClaveOracle" runat="server" Text="" Enabled="false"></asp:TextBox>
    <div runat="server" id="divLeader">
        <br style="display: none;" /><asp:Label runat="server" ID="lblLeader">ID Leader</asp:Label>
        <br /><asp:TextBox ID="txtIDLeader" runat="server" Text="" Enabled="false"></asp:TextBox>
    </div>
    <div runat="server" id="divEconomico">
        <br style="display: none;" /><asp:Label runat="server" ID="lblEconomico"># Económico</asp:Label>
        <br /><asp:TextBox ID="txtNumEconomico" runat="server" Text="" Enabled="false"></asp:TextBox>
    </div>
    <span>Tipo Unidad</span>
    <br /><asp:TextBox ID="txtTipoUnidad" runat="server" Text="" Enabled="false"></asp:TextBox>
    <br /><span>Modelo</span>
    <br /><asp:TextBox ID="txtModelo" runat="server" Text="" Enabled="false"></asp:TextBox>
    <br /><span>Año</span>
    <br /><asp:TextBox ID="txtAnio" runat="server" Text="" Enabled="false"></asp:TextBox>
    <br /><span>Fecha Compra</span>
    <br /><asp:TextBox ID="txtFechaCompra" runat="server" Text="" CssClass="CampoFecha" Enabled="false"></asp:TextBox>
    <div runat="server" id="divMontoFactura">
        <br style="display: none;" /><asp:Label runat="server" ID="lblMontoFactura">Monto Factura</asp:Label>
        <br /><asp:TextBox ID="txtMontoFactura" runat="server" Text="" CssClass="CampoMoneda" Enabled="false"></asp:TextBox>
    </div>
</div>
<div class="GroupBody" style="float:left; width: 390px; margin: 0px 0px 10px 10px !important;">
    <div class="GroupHeader"><span>Datos Generales</span></div>
    <div style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px;">
        <div runat="server" id="divAreaDepartamento">
            <asp:Label runat="server" ID="lblAreaDepartamento">Área/Departamento</asp:Label>
            <br /><asp:TextBox ID="txtArea" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </div>
        <span>Propietario</span>
        <br /><asp:TextBox ID="txtPropietario" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        <br /><span>Cliente</span>
        <br /><asp:TextBox ID="txtCliente" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        <br /><span>Sucursal</span>
        <br /><asp:TextBox ID="txtSucursal" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
    </div>
</div>
<div class="GroupBody" style="float:right; width: 250px; margin: 0px 0px 10px 10px !important;" runat="server" id="divDatosTecnicos">
    <div class="GroupHeader"><span>Datos Técnicos</span></div>
    <div style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px;">
        <span>Odómetros</span>
        <asp:GridView ID="grdOdometros" runat="server" CssClass="Grid" style="width: 100%;" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Km Inicial</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.KilometrajeInicio")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Km Final</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.KilometrajeFin")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>¿Activo?</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Activo").ToString().Replace("True", "SI").Replace("False", "NO")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <center>La unidad no tiene odómetros</center>
            </EmptyDataTemplate>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
        <span>Horómetros</span>
        <asp:GridView ID="grdHorometros" runat="server" CssClass="Grid" style="width: 100%;" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Hr Inicial</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.HoraInicio")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Hr Final</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.HoraFin")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>¿Activo?</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Activo").ToString().Replace("True", "SI").Replace("False", "NO")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <center>La unidad no tiene horómetros</center>
            </EmptyDataTemplate>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
        <span>PBV Máx. Recomendado</span>
        <br /><asp:TextBox ID="txtPBV" runat="server" Text="" CssClass="CampoNumero" Enabled="false" Width="96%"></asp:TextBox>
        <br /><span>PBC Máx. Recomendado</span>
        <br /><asp:TextBox ID="txtPBC" runat="server" Text="" CssClass="CampoNumero" Enabled="false" Width="96%"></asp:TextBox>
        <br /><span>Capacidad de Tanque</span>
        <br /><asp:TextBox ID="txtCapacidadTanque" runat="server" Text="" CssClass="CampoNumero" Enabled="false" Width="96%"></asp:TextBox>
        <br /><span>Rendimiento de Tanque</span>
        <br /><asp:TextBox ID="txtRendimientoTanque" runat="server" Text="" CssClass="CampoNumero" Enabled="false" Width="96%"></asp:TextBox>
    </div>
</div>
<div class="GroupBody" style="float:left; width: 250px; margin: 0px 0px 10px 10px !important;" runat="server" id="divNumerosSerie">
    <div class="GroupHeader"><span>Números de Serie</span></div>
    <div style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px;">
        <span>Radiador</span>
        <br /><asp:TextBox ID="txtRadiador" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        <br /><span>Post Enfriador</span>
        <br /><asp:TextBox ID="txtPostEnfriador" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Motor</legend>
            <span>Serie Motor</span>
            <br /><asp:TextBox ID="txtSerieMotor" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <span>Serie Turbo Cargador</span>
            <br /><asp:TextBox ID="txtSerieTurboCargador" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Serie Compresor de Aire</span>
            <br /><asp:TextBox ID="txtCompresorAire" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Serie ECM</span>
            <br /><asp:TextBox ID="txtECM" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Sistema Eléctrico</legend>
            <span>Serie Alternador</span>
            <br /><asp:TextBox ID="txtAlternador" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Serie Marcha</span>
            <br /><asp:TextBox ID="txtMarcha" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Serie Baterías</span>
            <br /><asp:TextBox ID="txtBaterias" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
        
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Transmisión</legend>
            <span>Serie</span>
            <br /><asp:TextBox ID="txtSerieTransmision" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Modelo</span>
            <br /><asp:TextBox ID="txtModeloTransmision" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Eje Dirección</legend>
            <span>Serie</span>
            <br /><asp:TextBox ID="txtSerieEjeDireccion" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Modelo</span>
            <br /><asp:TextBox ID="txtModeloEjeDireccion" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
        
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Eje Trasero Delantero</legend>
            <span>Serie</span>
            <br /><asp:TextBox ID="txtSerieEjeTraseroDelantero" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Modelo</span>
            <br /><asp:TextBox ID="txtModeloEjeTraseroDelantero" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Eje Trasero Trasero</legend>
            <span>Serie</span>
            <br /><asp:TextBox ID="txtSerieEjeTraseroTrasero" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Modelo</span>
            <br /><asp:TextBox ID="txtModeloEjeTraseroTrasero" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
    </div>
</div>
<div class="GroupBody" style="float:right; width: 390px; margin: 0px 0px 10px 10px !important;" runat="server" id="divLlantas">
    <div class="GroupHeader"><span>Llantas</span></div>
    <div style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px;">
        <asp:GridView ID="grdLlantas" runat="server" CssClass="Grid" style="width: 100%;"
            AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Código</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Codigo")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Marca</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Marca")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Modelo</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Modelo")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Medida</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Medida")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Profundidad</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Profundidad").ToString()%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Posición</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Posicion").ToString()%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <center>La unidad no tiene llantas</center>
            </EmptyDataTemplate>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
        <fieldset style="padding-left: 5px; padding-right: 5px;">
            <legend>Refacción</legend>
            <span>Código</span>
            <br /><asp:TextBox ID="txtRefaccionCodigo" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Marca</span>
            <br /><asp:TextBox ID="txtRefaccionMarca" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Modelo</span>
            <br /><asp:TextBox ID="txtRefaccionModelo" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Medida</span>
            <br /><asp:TextBox ID="txtRefaccionMedida" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
            <br /><span>Profundidad</span>
            <br /><asp:TextBox ID="txtRefaccionProfundidad" runat="server" Text="" Enabled="false" Width="96%" CssClass="CampoNumero"></asp:TextBox>
            <br /><span>Revitalizada</span>
            <br /><asp:TextBox ID="txtRefaccionRevitalizada" runat="server" Text="" Enabled="false" Width="96%"></asp:TextBox>
        </fieldset>
    </div>
</div>
<div class="GroupBody" style="float:right; width: 390px; margin: 0px 0px 10px 10px !important;" runat="server" id="divEquiposAliados">
    <div class="GroupHeader"><span>Equipos Aliados</span></div>
    <div style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px;">
        <asp:GridView ID="grdEquiposAliados" runat="server" CssClass="Grid" style="width: 100%;"
            AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate># Serie</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.NumeroSerie")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Año</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Anio")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Fabricante</HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.Fabricante")%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Tipo</HeaderTemplate>
                    <ItemTemplate>
                        <%# (DataBinder.Eval(Container, "DataItem.TipoEquipoServicio") != null) ? DataBinder.Eval(Container, "DataItem.TipoEquipoServicio.Nombre") : "" %>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <center>La unidad no tiene equipos aliados</center>
            </EmptyDataTemplate>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
    </div>
</div>

<div class="GroupBody" style="float:right; width: 250px; margin: 0px 0px 10px 10px !important;" runat="server" id="divNumerosSerieAdicional">
    <div class="GroupHeader"><span>Números Serie Adicionales</span></div>
    <div style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px;">
        <asp:GridView ID="grdNumerosSerie" runat="server" CssClass="Grid" style="width: 100%;" AutoGenerateColumns="False">
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
            </Columns>
            <EmptyDataTemplate>
                <center>No tiene números de serie</center>
            </EmptyDataTemplate>
            <RowStyle CssClass="GridRow" />
            <HeaderStyle CssClass="GridHeader" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>

    </div>

</div>
<asp:HiddenField runat="server" ID="hdnUnidadOperativaID"></asp:HiddenField>