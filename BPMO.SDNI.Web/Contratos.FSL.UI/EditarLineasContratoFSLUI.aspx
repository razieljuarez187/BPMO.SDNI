<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarLineasContratoFSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.FSL.UI.EditarLineasContratoFSLUI" %>
<%@ Import Namespace="BPMO.SDNI.Contratos.FSL.BO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GroupSection {
            width: 90%;
            margin: 0 auto;
        }
        .GroupContentCollapsable {
            margin: 0 auto;
        }
        .fieldset {
            width: 100%;
        }
        #datosGeneralesContrato, #lineasContrato, #tblDatosUnidad, #tblTipoCargo, #tblEquiposAliados {
            width: 98%;
            margin: 0 auto;
        }
        .Grid {
            margin: 0 auto;
        }
        .tdAlinearDerecha {
            text-align: right;
            width: 18%;
            vertical-align: middle;
        }
        .tdEspacioCentro {
            width: 2%;
        }
        .tdAlinearIzquierda {
            width: 30%;
            vertical-align: middle;
        }
        #Configuracion {
            margin: 0 auto;
            width: 98%;
        }
        #Configuracion > fieldset {
            margin: 0 auto;
            width: 98%;
        }
        #Configuracion > fieldset > table {
            margin: 0 auto;
            width: 98%;
        }
        #AniosConfigurados {
            margin: 0 auto;
            width: 98%;
        }
        #AniosConfigurados > fieldset {
            margin: 0 auto;
            width: 98%;
        }
    </style>
    <script type="text/javascript">
        var dialogUnidad;
        function <%= ClientID %>_Buscar(guid, xml) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#<%=btnResult.ClientID %>"),
                features: {
                    dialogWidth: width,
                    dialogHeight: '280px'
                }
            });
        }
        initChild = function() {
            dialogUnidad = $("#<%= ClientID %>_divCargos").dialog({
                width:950,
                resizable:false,
                position:"center",
                draggable: false,
                modal: true,
                autoOpen:false,
                closeOnEscape:false
            });
        };
        $(document).ready(initChild);
    </script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
         <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - MODIFICAR LINEAS DE CONTRATOS DE FSL</asp:Label>
        </div>
        <!--Navegación secundaria-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Contratos.FSL.UI/ConsultarContratosFSLUI.aspx">
                        CONSULTAR F.S.L.
                        <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Contratos.FSL.UI/RegistrarContratoFSLUI.aspx">
                        REGISTRAR RENTA F.S.L. 
                        <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                    </asp:HyperLink>
                </li>
            </ul>
            <!-- Barra de herramientas -->
            <div id="BarraHerramientas" style="float: left; width: 835px;">
                <div class="Ayuda" style="top:0px; float: right;">        
                    <input id="Button1" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
        </div>
        <asp:MultiView runat="server" ID="mvCambioUnidades" ActiveViewIndex="0">
            <asp:View runat="server" ID="vwInformacionGeneral">
                <div id="Formulario" class="GroupSection" style="margin-top: 25px;">
                    <div id="EncabezadoDatosCatalogo" class="GroupHeader">
                        <span>MODIFICACI&Oacute;N DE UNIADES EN CONTRATO FSL</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btnWizardGuardar" OnClick="btnGuardar_OnClick" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelar_OnClick" />
                        </div>
                    </div>
                    <div class="GroupContentCollapsable">
                        <fieldset style="width: 100%;">
                            <legend>
                                <span>Datos del Contrato</span>
                            </legend>
                            <table id="datosGeneralesContrato">
                                <tbody>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>Número de Contrato:</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtNumeroContrato"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label>Fecha de Contrato:</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtFechaInicioContrato"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>Cliente:</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda" colspan="4">
                                            <asp:TextBox runat="server" ID="txtClienteNombre" Width="50%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <label>Unidades en el Contrato</label>
                            </legend>
                            <table id="lineasContrato">
                                <tbody>
                                    <tr>
                                        <td class="tdAlinearDerecha">
                                            <asp:Label runat="server" ID="lblVIN" Text="VIN"></asp:Label>
                                        </td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda">
                                            <asp:TextBox ID="txtNumeroSerie" runat="server" Width="80%" OnTextChanged="txtNumeroSerie_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnBuscarUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" OnClick="ibtnBuscarUnidad_OnClick" style="width: 17px" />
                                        </td>
                                        <td colspan="3">
                                            <asp:UpdatePanel ID="updAgregarUnidad" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAgregarUnidad" CssClass="btnAgregarATabla" runat="server" Text="Agregar Unidad" Enabled="True" OnClick="btnAgregarUnidad_OnClick" />
                                                    
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtNumeroSerie" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ibtnBuscarUnidad" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnResult" EventName="Click"/>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br/>
                            <asp:UpdatePanel runat="server" ID="updLineasContratos" ChildrenAsTriggers="True">
                                <ContentTemplate>
                                    <asp:GridView ID="grdLineasContrato" runat="server" AutoGenerateColumns="False" PageSize="10"
                                        AllowPaging="False" CellPadding="4" GridLines="None" CssClass="Grid" Width="98%" AllowSorting="False" 
                                        OnRowDataBound="grdLineasContrato_OnRowDataBound" 
                                        OnPageIndexChanging="grdLineasContrato_OnPageIndexChanging"
                                        OnRowCommand="grdLineasContrato_OnRowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="VIN">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblVIN"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Justify" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modelo">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblModelo"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Justify" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estimado Anual">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblKmEstimadoAnual"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Depósito Garantía" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDepositoGarantia"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comisión Apertura" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblComisionApertura"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo Fijo x Mes" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCargoFijoMes"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Opción Compra">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="cbOpcionCompra" Checked='<%#((LineaContratoFSLBO)Container.DataItem).ConOpcionCompra == true %>' Enabled="False" Width="17px" />
                                                </ItemTemplate>
                                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo KM" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtnCargoKM" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Ver Cargo por KM">
                                                    </asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo HR" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtnCargoHR" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Ver Cargo por Hora">
                                                    </asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargos EA" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtnCargoEA" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Ver Cargo por Equipo Aliado">
                                                    </asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                                                        CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="ibtnDetalles" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="Ver Detalles"
                                                        CommandName="CMDDETALLES" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center" />
                                        <EditRowStyle CssClass="GridAlternatingRow" />
                                        <PagerStyle CssClass="GridPager" />
                                        <RowStyle CssClass="GridRow" />
                                        <FooterStyle CssClass="GridFooter" />
                                        <SelectedRowStyle CssClass="GridSelectedRow" />
                                        <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                        <EmptyDataTemplate>
                                            <b>No se han asignado Unidades al Contrato.</b>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </div>
                </div>
            </asp:View>
            <asp:View runat="server" ID="vwLineasContrato">
                <div id="divLineaContrato" class="GroupSection" style="margin-top: 25px;">
                    <div id="encabezadoLineaContrato" class="GroupHeader">
                        <span>AGREGAR DATOS DE UNIDAD</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarLinea" runat="server" Text="Guardar" CssClass="btnWizardGuardar" OnClick="btnGuardarLinea_OnClick" />
                            <asp:Button ID="btnCancelarLinea" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCancelarLinea_OnClick" />
                        </div>
                    </div>
                    <div class="GroupContentCollapsable">
                        <fieldset style="width: 100%;">
                            <legend><label>GENERAL</label></legend>
                            <table id="tblDatosUnidad">
                                <tbody>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>VIN</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtVinUnidad" Width="80%"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label># ECON&Oacute;MICO</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtNumeroEconomico" Width="80%"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>MODELO</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtNombreModelo" Width="80%"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label>A&Ntilde;O</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtAnio" Width="40%"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>PBV MAX. RECOMENDADO</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtPBV" Width="50%"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label>PBC MAX. RECOMENDADO</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtPBC" Width="50%"></asp:TextBox></td>
                                    </tr>
                                     <tr>
                                        <td class="tdAlinearDerecha"><label>KM INICIAL</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtKmInicial" Width="50%"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label>P&Oacute;LIZA DE SEGURO</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtPolizaSeguro" Width="80%"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>*KM ESTIMADO ANUAL</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtKmEstimadoAnual" Width="80%" CssClass="CampoNumeroEntero"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label>*DEP&Oacute;SITO GARANT&Iacute;A</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtDepositoGarantia" Width="80%" CssClass="CampoMoneda"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>*COMISI&Oacute;N POR APERTURA</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtComisionApertura" Width="80%" CssClass="CampoMoneda"></asp:TextBox></td>
                                        <td class="tdAlinearDerecha"><label>*CARGO FIJO POR MES</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtCargoFijo" Width="80%" CssClass="CampoMoneda"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>*TIPO COTIZACI&Oacute;N</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:DropDownList runat="server" ID="ddlTipoCotizacion" Width="80%" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoCotizacion_OnSelectedIndexChanged"/></td>
                                        <td class="tdAlinearDerecha"><label>*Producto o Servicio</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda">
                                            <asp:TextBox ID="txtClaveProductoServicio" runat="server" Width="80%" MaxLength="15" OnTextChanged="txtClaveProductoServicio_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnBuscarProductoServicio" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" OnClick="ibtnBuscarProductoServicio_Click" Style="width: 17px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>&nbsp;</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda">&nbsp;</td>
                                        <td class="tdAlinearDerecha"><label>&nbsp;</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox ID="txtDescripcionProductoServicio" runat="server" Width="80%" MaxLength="100" ReadOnly="true" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center">
                                            <asp:Button runat="server" ID="btnConfigurarTarifasUnidad" CssClass="btnWizardEditar" Text="EDITAR TARIFA" OnClick="btnConfigurarTarifasUnidad_OnClick"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>¿OPCI&Oacute;N DE COMPRA?</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda" colspan="4">
                                            <asp:CheckBox runat="server" ID="cbOpcionCompra" OnCheckedChanged="cbOpcionCompra_OnCheckedChanged" AutoPostBack="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdAlinearDerecha"><label>MONEDA</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:DropDownList runat="server" ID="ddlMonedaCompra" Width="50%"/></td>
                                        <td class="tdAlinearDerecha"><label>IMPORTE COMPRA</label></td>
                                        <td class="tdEspacioCentro">&nbsp;</td>
                                        <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtImporteCompra" Width="50%" CssClass="CampoMoneda"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend><span>EQUIPOS ALIADOS</span></legend>
                            <table id="tblEquiposAliados">
                                <tr>
                                    <td class="tdAlinearDerecha">
                                        <span>VIN</span>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:TextBox runat="server" ID="txtVinEquipoAliado" Width="80%" OnTextChanged="txtVinEquipoAliado_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:ImageButton ID="ibtBuscarEquipoAliado" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" OnClick="ibtBuscarEquipoAliado_OnClick" style="width: 17px" />
                                    </td>
                                    <td colspan="3">
                                        <asp:UpdatePanel ID="udpAgregarEquipoAliado" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAgregarEquipoAliado" CssClass="btnAgregarATabla" runat="server" Text="Agregar Equipo" Enabled="True" OnClick="btnAgregarEquipoAliado_OnClick" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtVinEquipoAliado" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ibtBuscarEquipoAliado" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnResult" EventName="Click"/>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView runat="server" ID="grvEquiposAliados" AllowPaging="false"
                                AutoGenerateColumns="False" CssClass="Grid" Width="70%"
                                OnRowDataBound="grvEquiposAliados_OnRowDataBound"
                                OnRowCommand="grvEquiposAliados_OnRowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="VIN">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblVIN"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" HorizontalAlign="Justify" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Modelo">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblModelo"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" HorizontalAlign="Justify" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TARIFA CAPTURADA">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="cbTarifaCapturada"/>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                                                CommandName="CMDELIMINAR" CommandArgument='<%#((CargoAdicionalEquipoAliadoBO)Container.DataItem).EquipoAliado.EquipoAliadoID %>' Width="17px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnConfigurarTarifasEquipoAliado" CssClass="btnWizardEditar" Text="EDITAR TARIFA"
                                                CommandName="CMDEDITARTARIFA" CommandArgument='<%#((CargoAdicionalEquipoAliadoBO)Container.DataItem).EquipoAliado.EquipoAliadoID %>'/>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center" />
                                <EditRowStyle CssClass="GridAlternatingRow" />
                                <PagerStyle CssClass="GridPager" />
                                <RowStyle CssClass="GridRow" />
                                <FooterStyle CssClass="GridFooter" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                <EmptyDataTemplate>
                                    <b>NO SE HAN ASIGNADO EQUIPOS ALIADOS A LA UNIDAD</b>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </fieldset>
                    </div>
                </div>
            </asp:View>
            <asp:View runat="server" ID="vwConfiguracionTarifa">
                <asp:UpdatePanel ID="updListView" runat="server">
            <ContentTemplate>
                <div id="divTarifas" class="GroupSection"  style="margin-top: 25px;">
                    <div id="encabezadoTarifas" class="GroupHeader">
                        <asp:Label runat="server" ID="lblEncabezadoTarifas">CONFIGURAR TARIFAS</asp:Label>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardarTarifa" runat="server" Text="Guardar" CssClass="btnWizardGuardar" OnClick="btnGuardarTarifa_OnClick" />
                            <asp:Button ID="btnCacelarTarifa" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" OnClick="btnCacelarTarifa_OnClick" />
                        </div>
                    </div>
                    <div class="GroupContentCollapsable">
                        <div id="divTipoCargo" runat="server">
                            <table id="tblTipoCargo">
                                <tr runat="server" ID="trAplicarCargos">
                                    <td class="tdAlinearDerecha">
                                        <span>NO APLICAR CARGOS ADICIONALES</span>
                                    </td>
                                    <td class="tdEspacioCentro">
                                        &nbsp;
                                    </td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:CheckBox runat="server" ID="cbNoAplicarCargos" AutoPostBack="True" OnCheckedChanged="cbNoAplicarCargos_OnCheckedChanged"/>
                                    </td>
                                    <td style="width: 50%;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdAlinearDerecha">
                                        <span>CARGO POR</span>
                                    </td>
                                    <td class="tdEspacioCentro">
                                        &nbsp;
                                    </td>
                                    <td class="tdAlinearIzquierda">
                                        <asp:DropDownList runat="server" ID="ddlTipoCargo" AutoPostBack="True"
                                            onselectedindexchanged="ddlTipoCargo_OnSelectedIndexChanged">
                                            <asp:ListItem Value="-1" Text="SELECCIONE" />
                                            <asp:ListItem Value="0" Text="KMS" />
                                            <asp:ListItem Value="1" Text="HRS" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 50%;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="Configuracion">
                            <fieldset>
                                <legend>
                                    <label>CONFIGURAR A&Ntilde;OS</label>
                                </legend>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="tdAlinearDerecha">
                                                <label>
                                                    A&Ntilde;O</label>
                                            </td>
                                            <td class="tdEspacioCentro">
                                                &nbsp;
                                            </td>
                                            <td class="tdAlinearIzquierda">
                                                <asp:DropDownList runat="server" ID="ddlAniosContrato" Width="40%" AutoPostBack="True"
                                                    onselectedindexchanged="ddlAniosContrato_OnSelectedIndexChanged" />
                                            </td>
                                            <td colspan="3" style="text-align: center">
                                                <asp:Button ID="btnGuardarAnio" runat="server" Text="GUARDAR" CssClass="btnWizardGuardar" OnClick="btnGuardarAnio_OnClick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdAlinearDerecha">
                                                <label>
                                                    FRECUENCIA</label>
                                            </td>
                                            <td class="tdEspacioCentro">
                                                &nbsp;
                                            </td>
                                            <td class="tdAlinearIzquierda">
                                                <asp:DropDownList runat="server" ID="ddlFrecuencia" Width="70%" />
                                            </td>
                                            <td class="tdAlinearDerecha">
                                                <label>
                                                    KM/HRS LIBRES</label>
                                            </td>
                                            <td class="tdEspacioCentro">
                                                &nbsp;
                                            </td>
                                            <td class="tdAlinearIzquierda">
                                                <asp:TextBox runat="server" ID="txtKmHrsLibres" Width="70%" CssClass="CampoNumeroEntero txtMoneda"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdAlinearDerecha"><span>KM/HR M&Iacute;NIMA</span></td>
                                            <td class="tdEspacioCentro">&nbsp;</td>
                                            <td class="tdAlinearIzquierda"><asp:TextBox runat="server" ID="txtKmHrMinimo" Width="70%" CssClass="CampoNumeroEntero"></asp:TextBox></td>
                                            <td class="tdAlinearDerecha"></td>
                                            <td class="tdEspacioCentro">&nbsp;</td>
                                            <td class="tdAlinearIzquierda"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="text-align: center;">
                                                <label>RANGOS</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="text-align: center;">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btnAgregarATabla" OnClick="btnAgregar_OnClick"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <table style="width: 100%;">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 8%; text-align: right; vertical-align: middle;">
                                                                <label>
                                                                    DE
                                                                </label>
                                                            </td>
                                                            <td class="tdEspacioCentro">&nbsp;</td>
                                                            <td style="width: 20%;">
                                                                <asp:TextBox runat="server" ID="txtRangoInicial" CssClass="CampoNumeroEntero txtMoneda"
                                                                    Width="90%"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <asp:DropDownList runat="server" ID="ddlRangoTiempo" Width="90%" AutoPostBack="True" 
                                                                    onselectedindexchanged="ddlRangoTiempo_OnSelectedIndexChanged">
                                                                    <asp:ListItem Value="0" Text="HASTA" />
                                                                    <asp:ListItem Value="1" Text="EN ADELANTE"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <asp:TextBox runat="server" ID="txtRangoFinal" CssClass="CampoNumeroEntero txtMoneda"
                                                                    Width="90%"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 8%; text-align: right; vertical-align: middle;">
                                                                <label>
                                                                    CARGO
                                                                </label>
                                                            </td>
                                                            <td class="tdEspacioCentro" style="text-align: right; vertical-align: middle"><label>$</label></td>
                                                            <td style="width: 20%;">
                                                                <asp:TextBox runat="server" ID="txtCargo" CssClass="CampoMoneda txtMoneda" Width="90%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="8">
                                                                <asp:GridView runat="server" ID="grvRangosConfigurados"
                                                                    AutoGenerateColumns="False" Width="50%" CssClass="Grid"
                                                                    onrowcommand="grvRangosConfigurados_OnRowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="DE">
                                                                             <ItemTemplate>
                                                                                <label><%# !String.IsNullOrEmpty(hdnCargoKm.Value) ? hdnCargoKm.Value == "0" ? 
                                                                                       ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoInicial : 
                                                                                       ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoInicial : null%></label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="30%" HorizontalAlign="Right"/>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="HASTA">
                                                                            <ItemTemplate>
                                                                                <label><%# !String.IsNullOrEmpty(hdnCargoKm.Value) ? hdnCargoKm.Value == "0" ? 
                                                                                       ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).HrRangoFinal.ToString() : "EN ADELANTE" : 
                                                                                       ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal != null ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).KmRangoFinal.ToString() : "EN ADELANTE" : null%></label>    
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="30%" HorizontalAlign="Right"/>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Cargo">
                                                                            <ItemTemplate>
                                                                                <label><%# !String.IsNullOrEmpty(hdnCargoKm.Value) ? hdnCargoKm.Value == "0" ? ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoHr : ((RangoTarifaFSLBO)DataBinder.GetDataItem(Container)).CargoKm : null%></label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="30%" HorizontalAlign="Right"/>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField >
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton runat="server" ID="ibtEliminar" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar"
                                                                                    CommandName="CMDELIMINAR" CommandArgument='<%#Container.DataItemIndex%>' Width="17px" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="10%" HorizontalAlign="Center"/>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="GridHeader"/>
                                                                    <EmptyDataTemplate>
                                                                        <label>NO SE HAN AGREGADO RANGOS</label>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </div>
                        <div id="AniosConfigurados">
                            <fieldset>
                                <legend>A&Ntilde;OS CONFIGURADOS</legend>
                                <asp:GridView runat="server" ID="grvAniosConfigurados" AutoGenerateColumns="False" CssClass="Grid" Width="50%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="AÑO">
                                            <ItemTemplate>
                                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Año != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Año.ToString() : "N/A"%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FRECUENCIA">
                                            <ItemTemplate>
                                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Frecuencia != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Frecuencia.ToString() : ""%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="KMS LIBRES">
                                            <ItemTemplate>
                                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).KmLibres != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).KmLibres.ToString() : ""%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HRS LIBRES">
                                            <ItemTemplate>
                                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).HrLibres != null ? ((TarifaFSLBO)DataBinder.GetDataItem(Container)).HrLibres.ToString() : ""%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CONFIGURADO">
                                            <ItemTemplate>
                                                <%# ((TarifaFSLBO)DataBinder.GetDataItem(Container)).Frecuencia == null ? "NO" : "SI"%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridHeader"/>
                                    <EmptyDataTemplate>
                                        <label>NO EXISTEN TARIFAS PARA PRESENTAR</label>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlRangoTiempo"/>
                <asp:AsyncPostBackTrigger ControlID="ddlAniosContrato"/>
                <asp:AsyncPostBackTrigger ControlID="grvRangosConfigurados"/>
                <asp:AsyncPostBackTrigger ControlID="btnAgregar"/>
                <asp:AsyncPostBackTrigger ControlID="btnGuardarAnio"/>
            </Triggers>
        </asp:UpdatePanel>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:HiddenField runat="server" ID="hdfClienteID" />
    <asp:Button runat="server" ID="btnResult" Style="display: none" OnClick="btnResult_OnClick" />
    <div id="<%= ClientID %>_divCargos">
    </div>
    <div id="<%= ClientID %>_divConfirmacion" style="display: none">
    </div>
    <asp:HiddenField runat="server" ID="hdnLineaUnidadId"/>
    <asp:HiddenField runat="server" ID="hdnLineaEquipoId"/>
    <asp:HiddenField runat="server" ID="hdnProductoServicioId" />
    <asp:HiddenField runat="server" ID="hdnCargoKm" Value="" />
    <asp:HiddenField runat="server" ID="hdnTipoEquipo" Value=""/>
    <asp:HiddenField runat="server" ID="hdnUnidadId" Value=""/>
    <asp:HiddenField runat="server" ID="hdnEquipoAliadoId" Value=""/>
    <script type="text/javascript">
        function AbrirTarifa() {
            dialogUnidad.parent().appendTo($("form:first"));
            dialogUnidad.dialog("open");
        }
        function CerrarTarifa() {
            $("#<%=ClientID %>_divCargos").dialog("close");
        }
    </script>
</asp:Content>
