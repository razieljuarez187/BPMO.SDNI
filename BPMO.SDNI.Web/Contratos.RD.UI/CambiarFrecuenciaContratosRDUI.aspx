<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="CambiarFrecuenciaContratosRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.CambiarFrecuenciaContratosRDUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() { }
    </script>
    <style type="text/css">
        .tdRightCambioFrencuencia {
            width: 15%;
            text-align: right;
            vertical-align: middle;
        }
        .tdCenterCambioFrencuencia {
            width: 2%;
        }
        .tdLeftCambioFrencuencia {
            width: 33%;
        }
        .Grid
        {
            width: 70%;
            margin: 0 auto;
        }
    </style>
    <script type="text/javascript">
        function confirmarCambioFrecuencia() {
            var $div = $('<div title="Confirmación"></div>');
            $div.append('Al realizar un cambio de Frecuencia, no se podrán revertir los cambios realizos en caso de quedar pagos pendietes. <br/> ¿Desea realizar el cambio?');
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                closeOnEscape: true,
                modal: true,
                minWidth: 500,
                close: function () { $(this).dialog("destroy"); },
                buttons: {
                    Aceptar: function () {
                        $(this).dialog("close");
                        __doPostBack("<%= btnTermino.UniqueID %>", "");
                    },
                    Cancelar: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPCIONES - Cambiar Frecuencia de Contratos de RD</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div id="Navegacion" style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" NavigateUrl="~/Contratos.RD.UI/ConsultarCambioFrecuenciaRDUI.aspx"
                        runat="server" CausesValidation="False">
                        CONSULTAR
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas" style="width: 835px; float: right;">
                <div class="Ayuda" style="top: 0px;">
                    <input id="btnAyuda" type="button" onclick="ShowHelp();" class="btnAyuda" />
                </div>
            </div>
        </div>
        <!-- Cuerpo Contenido -->
        <div id="ContenedorContenido" class="GroupSection" style="margin: 0 auto; width: 70%">
            <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                <span>CAMBIAR FRECUENCIA DE FACTURACI&Oacute;N</span>
                <div class="GroupHeaderOpciones Ancho2Opciones">
                    <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" 
                        CssClass="btnWizardTerminar" onclick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" 
                        CssClass="btnWizardCancelar" onclick="btnCancelar_Click"/>
                </div>
            </div>
            <div class="GroupContentCollapsable">
                <fieldset  style="width:100%; margin: 0 auto;">
                    <legend>INFORMACI&Oacute;N DEL CONTRATO</legend>
                    <table>
                        <tbody>
                            <tr>
                                <td class="tdRightCambioFrencuencia">
                                    <label>CONTRATO</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtNumeroContrato" Width="50%"/>
                                    <asp:HiddenField runat="server" ID="hdnContratoId" Value=""/>
                                </td>
                                <td class="tdRightCambioFrencuencia">
                                    <label>FECHA INICIO</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtFechaInicio" Width="50%"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdRightCambioFrencuencia">
                                    <label>SUCURSAL</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtSucursalNombre" Width="50%"/>
                                    <asp:HiddenField runat="server" ID="hdnSucursalId" Value=""/>
                                </td>
                                <td class="tdRightCambioFrencuencia">
                                    <label>FECHA PROMESA</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtFechaPromesaDevolucion" Width="50%"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdRightCambioFrencuencia">
                                    <label>CLIENTE</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtClienteNombre" Width="90%"/>
                                    <asp:HiddenField runat="server" ID="hdnClienteId" Value=""/>
                                </td>
                                <td class="tdRightCambioFrencuencia">
                                    <label>D&Iacute;AS RESTANTES</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtDiasRestantes" Width="20%"/>
                                </td>
                            </tr>
                        </tbody>                        
                    </table>                    
                </fieldset>
                <fieldset>
                    <legend>ESTATUS DE PAGOS AL D&Iacute;A DE HOY</legend>
                     <asp:GridView runat="server" ID="grdPagosFacturados" 
                        AutoGenerateColumns="false" PageSize="10"
                        AllowPaging="true" AllowSorting="false" 
                        EnableSortingAndPagingCallbacks="true" CssClass="Grid" 
                        onrowdatabound="grdPagosFacturados_RowDataBound" 
                        onpageindexchanging="grdPagosFacturados_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="NumeroPago" HeaderText="# Pago">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Fecha Vencimiento
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFechaVencimiento" Text='<%# DataBinder.Eval(Container.DataItem,"FechaVencimiento") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    FACTURADO
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEnviadoFacturacion" Text='<%# DataBinder.Eval(Container.DataItem,"EnviadoFacturacion") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Center" Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    FRECUENCIA
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFrecuencia" Text='<%# DataBinder.Eval(Container.DataItem,"Tarifa.FrecuenciaID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="DiasFacturar" HeaderText="Días Factura">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                            </asp:BoundField>
                        </Columns>
                    <RowStyle CssClass="GridRow" />
                    <HeaderStyle CssClass="GridHeader" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                </asp:GridView>
                    <div style="margin: 0 auto; width:100%; text-align: center;">
                        <label style="color: Red;">TODOS LOS PAGOS DEBEN ESTAR FACTURADOS PARA REALIZAR EL CAMBIO DE FRECUENCIA</label>
                    </div>                    
                </fieldset>
                <fieldset>
                    <legend>PROYECCI&Oacute;N DE LOS SIGUIENTES PAGOS</legend>
                    <table  style="width:70%; margin: 0 auto;">
                         <tbody>
                            <tr>
                                <td class="tdRightCambioFrencuencia">
                                    <label>ACTUAL</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:TextBox runat="server" ID="txtFrecuenciaAnterior" Width="70%"/>
                                    <asp:HiddenField runat="server" ID="hdnFrecuenciaAnterior" />
                                </td>
                                <td class="tdRightCambioFrencuencia">
                                    <label>NUEVA</label>
                                </td>
                                <td class="tdCenterCambioFrencuencia">
                                    &nbsp;
                                </td>
                                <td class="tdLeftCambioFrencuencia">
                                    <asp:DropDownList runat="server" ID="ddlFrecuenciaFacturacion" Width="90%" AutoPostBack="true" 
                                        onselectedindexchanged="ddlFrecuenciaFacturacion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hdnDiasFacturar" />
                                </td>
                            </tr>
                         </tbody>
                         
                    </table>
                    <asp:UpdatePanel runat="server" ID="udpPagosFaltantes">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="grdPagosPendientes" 
                                AutoGenerateColumns="false" PageSize="10"
                                AllowPaging="true" AllowSorting="false" 
                                EnableSortingAndPagingCallbacks="true" style="width: 50%; margin: 0 auto;" 
                                onrowdatabound="grdPagosPendientes_RowDataBound" 
                                onpageindexchanging="grdPagosPendientes_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="NumeroPago" HeaderText="# Pago">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Fecha Vencimiento
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFechaVencimiento" Text='<%# DataBinder.Eval(Container.DataItem,"FechaVencimiento") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DiasFacturar" HeaderText="Días Factura">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle CssClass="GridRow" />
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <PagerStyle CssClass="GridPager" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlFrecuenciaFacturacion" />
                        </Triggers>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
        </div>
    </div>
    <asp:Button ID="btnTermino" runat="server" Text="Terminar" OnClick="btnTermino_Click" style="display: none;"/>
</asp:Content>
