<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarUnidadUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimimento.UI.RegistrarUnidadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/EstiloIngresarUnidad.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/MantenimientoResponsive.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%= Page.ResolveUrl("~/Contenido/Estilos/bootstrap.1.8.2.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GroupSection { max-width: 650px; min-width:100px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; } 
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; }
        .Grid { border: none;}
        .ChildGrid { margin: 0px; padding: 0px; border: none;}
        .btnFooterToolbar { display: inline-block; font-size: 10px !important; padding: 10px 20px; cursor: pointer;
                            text-align: center; text-decoration: none; outline: none; color: #fff; background-color: #e9581b;
                            border: none; border-radius: 7px; box-shadow: 0 4px #999; }
        .btnFooterToolbar:hover {background-color: #d03f00}
        .btnFooterToolbar:active { background-color: #d03f00; box-shadow: 0 1px #666; transform: translateY(4px); }
        .btnRegistrarIngreso { background: #DE0814 !important; border: none !important; height: 30px; line-height:30px; color: White; font-weight: bold; text-decoration: none; text-transform: uppercase; width: 185px; cursor: pointer; }
        .Grid th, .ChildGrid th { font-size : 12px !important; text-align: center; }
        .Grid td, .ChildGrid td { font-size : 11px !important; text-align: left; vertical-align: middle; }
        .td-finder { float: left; white-space: nowrap;}
        .ChildGrid td { border: solid 1px #cccccc; border-top: none; }
        .NoVisible { display:none; }
        .ConvertirMayus { text-transform: uppercase; }
    </style>
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/bootstrap-1.8.2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("../Contenido/Scripts/mantenimiento-responsive.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        var dialog = null;
        var dialogUnidades = null;
        var dialogCancelacion = null;
        var dialogMantenimientosPendientes = null;
        var dialogCombustibleSalida = null;

        $(document).ready(function () {
            initChild();
            inicializeHorizontalPanels();
            cloneMenu();
            loadMenuPrincipalSelected();
            listenClickMenuResponsive();
            setInterval(initSincronizerEstatus, 300000);
        });

        function initSincronizerEstatus() {
            if (!AnyDialogIsOpen()) {
                $("#<%=btnHSincronizarEstatus.ClientID %>").click();
            }
        }

        function IrTareasPendientes() {
            document.getElementById("btnRedireccionVerTareas").click();
        }

        function IrRegistrarTarea() {
            document.getElementById("btnRedireccionRegistraTarea").click();
        }

        function AnyDialogIsOpen() {
            var isOpen = false;

            if (dialog != null) {
                isOpen = $(dialog).dialog("isOpen");
            }
            if (dialogUnidades!= null && isOpen == false) {
                isOpen = $(dialogUnidades).dialog("isOpen");
            }
            if (dialogCancelacion!= null && isOpen == false) {
                isOpen = $(dialogCancelacion).dialog("isOpen");
            }
            if (dialogMantenimientosPendientes != null && isOpen == false) {
                isOpen = $(dialogMantenimientosPendientes).dialog("isOpen");
            }
            if (dialogCombustibleSalida != null && isOpen == false) {
                isOpen = $(dialogCombustibleSalida).dialog("isOpen");
            }
            if (isOpen == false && $(".blockUI").length > 0) {
                isOpen = true;
            }

            return isOpen;
        }

        function OnChangeCalc(input) {
            var btnToClick = $(input).next();
            $(btnToClick).click();
        }

        function ValidarCampos() {
            var s = "";

            if ($("#<%=txtInventario.ClientID%>").val().trim() != "" || $("#<%= hdnValidaCampoPSL.ClientID %>").val() == "0") {
                if ($("#<%=txtDescripcionFalla.ClientID%>").val().trim() != "") {
                    if ($("#<%=txtCodigoFalla.ClientID%>").val().trim() != "" || $("#<%= hdnValidaCampoPSL.ClientID %>").val() == "0") {
                        if ($("#<%= hdnEsUnidad.ClientID %>").val().trim() == "Unidad") {
                            if ($("#<%=txtCombustible.ClientID%>").val().trim() != "") {
                                if ($("#<%=txtCombustibleTotal.ClientID%>").val().trim() != "" || $("#<%= hdnValidaCampoPSL.ClientID %>").val() == "0") {
                                    return true;
                                }
                                else {
                                    s += ", Combustible Total";
                                }
                            } else {
                                s += ", Combustible de Entrada";
                            }
                        }
                        else {
                            return true;
                        }
                    } else {
                        s += ", Codigos de Falla";
                    }
                }
                else {
                    s += ", Descripcion de Falla";
                }
            } else {
                s += ", Inventario";
            }

            if (s.trim() != "") {
                MensajeGrowUI("Falta información necesaria para continuar con el registro" + s, "4");
                return false;
            } else {
                return true;
            }
        }

        function initChild() {
            if (!$("#<%=txtFechaInicio.ClientID %>").is(':disabled')) {
                $("#<%=txtFechaInicio.ClientID %>").datepicker({
                    showOn: "button",
                    buttonImage: "../Contenido/Imagenes/calendar.gif",
                    buttonImageOnly: true
                });
            }

            if (!$("#<%=txtFechaFin.ClientID %>").is(':disabled')) {
                $("#<%=txtFechaFin.ClientID %>").datepicker({
                    showOn: "button",
                    buttonImage: "../Contenido/Imagenes/calendar.gif",
                    buttonImageOnly: true
                });
            }
            $("#<%=txtFechaInicio.ClientID %>").attr('readonly', true);
            $("#<%=txtFechaFin.ClientID %>").attr('readonly', true);
            
            $("#<%=btnGuardarOS.ClientID %>").click(function () {
                return ValidarCampos();
            });
        }

        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td style='border: none; width:20px;'></td><td style='border: none; width:20px;'></td><td style='border: none; width:100% !important;' colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Contenido/Imagenes/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Contenido/Imagenes/plus.png");
            $(this).closest("tr").next().remove();
        });

        function inicializeHorizontalPanels() {
            $(".GroupHeaderCollapsable").click(function () {
                $(this).next(".GroupContentCollapsable").slideToggle(500);
                if ($(this).find(".imgMenu").attr("src") == "../Contenido/Imagenes/FlechaArriba.png")
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaAbajo.png");
                else
                    $(this).find(".imgMenu").attr("src", "../Contenido/Imagenes/FlechaArriba.png");
                return false;
            });
        }

        //Buscador Sucursal
        /*--------------------------------------------*/

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
        function IrImprimirCalcomania() {
            document.getElementById("btnRedireccionCalcomania").click();
        }

        function IrImprimirPaseSalida() {
            document.getElementById("btnRedireccionPaseSalida").click();
        }
        function IrEnviarServicioRealizado(ConPendientes) {
            if (ConPendientes == "1")
                document.getElementById("btnRedireccionaEnviarEmailCon").click();
            else
                document.getElementById("btnRedireccionaEnviarEmailSin").click();
        }
        function IrExportarReporte() {
            document.getElementById("btnRedireccionaExportar").click();
        }
        /*--------------------------------------------*/

        //Dialogo OS
        /*--------------------------------------------*/


        function AbrirDialogo(esUnidad, capacidadTanque) {
            if (esUnidad != null) {
                $("#<%= hdnEsUnidad.ClientID %>").val(esUnidad);
            }
            if (capacidadTanque != null) {
                $("#<%= hdnCapacidadTanque.ClientID %>").val(capacidadTanque);
            }
            dialog = $('#dialogo-os').dialog({
                width: 'auto',
                height: 'auto',
                modal: false,
                autoOpen: false,
                resizable: false,
                draggable: false,
                zIndex: 800,
                close: function () {
                    $("#<%= hdnEsUnidad.ClientID %>").val("");
                    $("#<%= hdnCapacidadTanque.ClientID %>").val("");
                    $(this).dialog('close');
                }
            });
            $(dialog).parent().appendTo("#contenedor-dialogo-os");
            $(dialog).dialog("open");
        }

        function AbrirDialogoUnidades() {
            dialogUnidades = $('#dialogo-unidades').dialog({
                width: '700',
                height: '320',
                modal: false,
                autoOpen: false,
                resizable: false,
                draggable: false,
                close: function () {
                    $(this).dialog('close');
                }
            });
            $(dialogUnidades).parent().appendTo("#contenedor-dialogo-unidades");
            $(dialogUnidades).dialog("open");
        }

        function AbrirDialogoMantenimientosPendientes() {
            dialogMantenimientosPendientes = $('#dialogo-mantenimientosPendientes').dialog({
                width: '700',
                height: '320',
                modal: false,
                autoOpen: false,
                resizable: false,
                draggable: false,
                close: function () {
                    $(this).dialog('close');
                }
            });
            $(dialogMantenimientosPendientes).parent().appendTo("#contenedor-dialogo-mantenimientosPendientes");
            $(dialogMantenimientosPendientes).dialog("open");
        }

        function AbrirDialogoCancelacion() {
            dialogCancelacion = $('#dialogo-cancelar-ingreso').dialog({
                width: 'auto',
                height: 'auto',
                modal: false,
                autoOpen: false,
                resizable: false,
                draggable: false,
                close: function () {
                    $(this).dialog('close');
                }
            });
            $(dialogCancelacion).parent().appendTo("#contenedor-dialogo-cancelar-ingreso");
            $(dialogCancelacion).dialog("open");
        }

        function AbrirDialogoCombustibleSalida() {
            dialogCombustibleSalida = $('#dialogo-combustibe-salida').dialog({
                width: '380',
                height: 'auto',
                modal: false,
                autoOpen: false,
                resizable: false,
                draggable: false,
                close: function () {
                    $(this).dialog('close');
                }
            });
            $(dialogCombustibleSalida).parent().appendTo("#contenedor-dialogo-combustibe-salida");
            $(dialogCombustibleSalida).dialog("open");
        }

        function CerrarDialogoCombustible() {
            $(dialogCombustibleSalida).parent().appendTo("#contenedor-dialogo-combustibe-salida");
            $(dialogCombustibleSalida).dialog("close");
        }

        /*--------------------------------------------*/
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server"> 
    <div>
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label runat="server">MANTENIMIENTO - Ingresar Unidad a Taller</asp:Label>
        </div>
        <div>
            <asp:Button ID="btnRegistrarIngreso" CssClass="btnRegistrarIngreso" runat="server" OnClick="OnclickAgregarIngreso" Text="Registrar Ingreso"></asp:Button>
        </div>
        <!-- Cuerpo -->
        <div id="Div1" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>DETALLE INGRESO A TALLER</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable ">
                <table class="trAlinearDerecha table-responsive">
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">SUCURSAL</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtSucursal" runat="server" MaxLength="30" 
                                CssClass="input-text-responsive" AutoPostBack="True" 
                                ontextchanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnBuscarSucursal" CommandName="VerSucursal"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Sucursales" CommandArgument='' OnClick="btnBuscarSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>TALLER</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddTalleres" runat="server" CssClass="input-dropdown-responsive"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">ESTATUS</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:DropDownList ID="ddEstatus" runat="server" CssClass="input-dropdown-responsive">
                                <%--<asp:ListItem Value="0" Text="EN REGISTRO" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="2" Text="ASIGNADA"></asp:ListItem>
                                <asp:ListItem Value="4" Text="EN PROCESO"></asp:ListItem>
                                <asp:ListItem Value="6" Text="FACTURADO"></asp:ListItem>
                                <asp:ListItem Value="14" Text="TERMINADO"></asp:ListItem>
                                <asp:ListItem Value="59" Text="POR AUTORIZAR"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">ORDEN SERVICIO</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtOrdenServicio" runat="server" MaxLength="10"
                                CausesValidation="True" ValidationGroup="FormatoValido" CssClass="input-text-responsive" />
                            <asp:RegularExpressionValidator ID="revOrdenServicio" runat="server" ControlToValidate="txtOrdenServicio"
                                Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="\d{10}"
                                ValidationGroup="FormatoValido" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive">VIN</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtNumeroSerie" runat="server" CssClass="input-text-responsive" MaxLength="30" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>FECHA INICIO</td>
                        <td class="input-space-responsive" >&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtFechaInicio" runat="server"
                                CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFecha input-date-responsive"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revFechaInicio" runat="server" ControlToValidate="txtFechaInicio"
                                Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical input-label-responsive"><span>*</span>FECHA FIN</td>
                        <td class="input-space-responsive">&nbsp;</td>
                        <td class="tdCentradoVertical input-group-responsive">
                            <asp:TextBox ID="txtFechaFin" runat="server"
                                CausesValidation="True" ValidationGroup="FormatoValido" CssClass="CampoFecha input-date-responsive"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revFechaFin" runat="server" ControlToValidate="txtFechaFin"
                                Display="Dynamic" ErrorMessage="Formato inválido" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d"
                                ValidationGroup="Obligatorios" CssClass="ColorValidator">**</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="input-button-responsive"><asp:Button runat="server" ID="btnBuscarMantenimientos" Text="Buscar" OnClick="OnclickBuscarMantenimientos" CssClass="btnComando" /></td>
                    </tr>
                </table>
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="UPContenedor" runat="server">
            <ContentTemplate>
            <div style="margin-left:20px; margin-right:20px; width:95% !important; overflow: auto;">
                <asp:GridView ID="gvIngresoUnidades" runat="server" AutoGenerateColumns="False" 
                    AllowPaging="true" PageSize="10"
                        DataKeyNames="Guid" EnableViewState="false" ShowHeaderWhenEmpty="true" OnPageIndexChanging="grvRegistro_PageIndexChanging"
                        OnRowCommand="grvRegistro_RowCommand" onrowdatabound="OnRowDataBound" 
                    CssClass="Grid">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGuid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Guid") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSeleccionar" runat="server" AutoPostBack="true" OnCheckedChanged="gvIngresoUnidadesCheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.MantenimientoUnidadId") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCapacidadTanque" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Unidad.CaracteristicasUnidad.CapacidadTanque") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <img alt = "" style="cursor: pointer"  src="../Contenido/Imagenes/plus.png" />
                                        <div style="display: none;">  
				                        <asp:GridView ID="gvAliados" runat="server" AutoGenerateColumns="false"
                                         OnRowCommand="grvRegistroEquipoAliado_RowCommand" DataKeyNames="Guid" EnableViewState="false"
                                         onrowdatabound="OnRowDataBoundEquipoAliado" onprerender="gvAliados_PreRender"
                                          CssClass="ChildGrid" ShowHeader="False">  
					                        <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAliadoGuid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Guid") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.MantenimientoEquipoAliadoId") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
						                        <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEVin" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.IngresoEquipoAliado.EquipoAliado.NumeroSerie") %>' Width="100px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblENumEcono" runat="server"  Text='' Width="100px" CssClass="ConvertirMayus"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEModelo" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.IngresoEquipoAliado.EquipoAliado.Modelo.Nombre") %>' Width="100px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblECliente" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Unidad.Cliente.Nombre") %>'  Width="100px" CssClass="ConvertirMayus"></asp:Label>
                                                </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEControlista" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Controlista.NombreCorto") %>'  Enabled="false" Width="150px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEOperador" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Operador") %>'  Enabled="false" Width="80px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtEObservaciones" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.IngresoEquipoAliado.ObservacionesOperador") %>' Width="80px" CssClass="ConvertirMayus"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div style="width:62px;"><asp:ImageButton runat="server" ID="btnIngresarEA" CommandName="IngresarEquipoAliado" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="INGRESAR" ImageAlign="Middle" /></div>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEFechaHOra" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.MantenimientoProgramado.Fecha") %>' Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEKms" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.MantenimientoProgramado.Km") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label  ID="lblEHorometro" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.MantenimientoProgramado.Horas") %>' Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblETipoServicio" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.MantenimientoProgramado.TipoMantenimiento") %>' Width="58px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:Label runat="server"  ID="lblEFechaHoraCalc" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.IngresoEquipoAliado.FechaIngreso") %>' Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEKmCalc" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.KilometrajeEntrada") %>' 
                                                        Width="50px" onchange="OnChangeCalc(this)"></asp:TextBox>
                                                        <asp:Button  ID="btnEKmCalc" runat="server" CssClass="NoVisible" OnClick="btnEKmCalc_Click" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEHorometroCalc" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.HorasEntrada") %>' 
                                                        Width="50px" onchange="OnChangeCalc(this)"></asp:TextBox>
                                                        <asp:Button  ID="btnEHorasCalc" runat="server" CssClass="NoVisible" OnClick="btnEHrsCalc_Click" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblETipoServicioCalc" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.TipoMantenimiento") %>' Width="57px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server"  ID="lblEDiasDiferencia" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.DiasDiferencia") %>' Width="73px"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEOrdenServicio" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.OrdenServicio.Id") %>' Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label  ID="lblEEstatus"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoAliado.OrdenServicio.Estatus.Nombre") %>' Width="100px" CssClass="ConvertirMayus"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" ID="btnInMantEA" CommandName="MantenimientoEquipoAliado" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="PREORDEN" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="45px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>AUDITOR&Iacute;A</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" ID="btnIniciarAuditoriaEA" CommandName="AuditoriaEquipoAliado" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="AUDITORIA" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="66px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Editar OS</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" ID="btnEditarOsEA" CommandName="EditarOsEquipoAliado" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="EDITAR OS" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
					                        </Columns>  
                                        </asp:GridView>  
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="VIN/# SERIE">
                                <ItemTemplate>
                                    <div class="td-finder">
                                        <asp:TextBox runat="server" ID="txtVin" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Vin") %>' Width="80px"  CssClass="ConvertirMayus"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="btnBuscarVin" CommandName="Vin" ImageUrl="~/Contenido/Imagenes/find.png"
                                            ToolTip="Consultar Unidad" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageAlign="Middle" /> 
                                    </div> 
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="# ECON&Oacute;MICO">
                                <ItemTemplate>
                                    <div class="td-finder">
                                        <asp:TextBox runat="server"  ID="txtNumeroEconomico" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.NumeroEconomico") %>' Width="80px"  CssClass="ConvertirMayus"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="btnBuscarNumEco" CommandName="NumeroEconomico" ImageUrl="~/Contenido/Imagenes/find.png"
                                            ToolTip="Consultar Unidad" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageAlign="Middle" />
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="MODELO">
                                <ItemTemplate>
                                    <div class="td-finder">
                                        <asp:TextBox ID="txtModelo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Unidad.Modelo.Nombre") %>' Width="80px" CssClass="ConvertirMayus"></asp:TextBox>
                                        <asp:ImageButton  runat="server" ID="btnBuscarModelo" CommandName="Modelo" ImageUrl="~/Contenido/Imagenes/find.png"
                                            ToolTip="Consultar Unidad" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageAlign="Middle" />
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                                <ItemStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="CLIENTE">
                                <ItemTemplate>
                                    <div class="td-finder">
                                        <asp:TextBox ID="txtCliente" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Unidad.Cliente.Nombre") %>' Width="80px" CssClass="ConvertirMayus"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="btnBuscarCliente" CommandName="Cliente" ImageUrl="~/Contenido/Imagenes/find.png"
                                            ToolTip="Consultar Unidad" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageAlign="Middle" />
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="150px">
                                <HeaderTemplate>CONTROLISTA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblControlista" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Controlista.NombreCorto") %>' Width="150px" CssClass="ConvertirMayus"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>    
                            <asp:TemplateField ItemStyle-Width="150px">
                                <HeaderTemplate>OPERADOR</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtOperador" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.Operador") %>' Width="80px" CssClass="ConvertirMayus"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="150px">
                                <HeaderTemplate>REPORTE OPERADOR</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtObservaciones" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.ObservacionesOperador") %>' Width="80px" Wrap="false" CssClass="ConvertirMayus"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>INGRESAR</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnIngresar" CommandName="Ingresar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="INGRESAR" ImageAlign="Middle" />
                                </ItemTemplate>
                                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px">
                                <HeaderTemplate>FECHA Y HORA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server"  ID="lblFechaHora" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.MantenimientoProgramado.Fecha") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>KMS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblKm" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.MantenimientoProgramado.Km") %>' Width="50px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>HRS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblHorometro" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.MantenimientoProgramado.Horas") %>' Width="50px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>TIPO SERVICIO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTipoServicio" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.MantenimientoProgramado.TipoMantenimiento") %>' CssClass="ConvertirMayus"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="80px">
                                <HeaderTemplate>FECHA Y HORA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFechaHoraCalc" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.IngresoUnidad.FechaIngreso") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>KMS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="lblKmCalc" AutoPostBack="true" OnTextChanged="OnTextChangedKm" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.KilometrajeEntrada") %>' Width="50px"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>HRS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="lblHorometroCalc" AutoPostBack="true" OnTextChanged="OnTextChangedHoras" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.HorasEntrada") %>' Width="50px"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>TIPO SERVICIO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTipoServicioCalc" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.TipoMantenimiento") %>' CssClass="ConvertirMayus"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>D&Iacute;AS DIFERENCIA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDiasDiferencia" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.DiasDiferencia") %>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>ORDEN SERVICIO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOrdenServicio" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.OrdenServicio.Id") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>ESTATUS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEstatus" Text='<%# DataBinder.Eval(Container.DataItem,"MantenimientoUnidad.OrdenServicio.Estatus.Nombre") %>' Width="100px" CssClass="ConvertirMayus"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="40px">
                                <HeaderTemplate>IMPRESO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkImpresoMantto" Enabled="false"  />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="40px">
                                <HeaderTemplate>INCLUIR TAREAS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkAgregarTareas" Enabled="false"  />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
						    <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>INICIAR MANT.</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnInMant" CommandName="Mantenimiento" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="PREORDEN" ImageAlign="Middle" />
                                </ItemTemplate>
                                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="66px">
                                <HeaderTemplate>AUDITOR&Iacute;A</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnIniciarAuditoria" CommandName="Auditoria" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="AUDITORIA" ImageAlign="Middle" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="25px">
                                <HeaderTemplate>Ver OS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnEditarOs" CommandName="EditarOs" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="EDITAR OS" ImageAlign="Middle" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>Sin registros disponibles</EmptyDataTemplate>
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
        <asp:Button ID="btnHSincronizarEstatus" runat="server" Text="Sincronizar" OnClick="btn_SincronizarEstatus_Click" CssClass="NoVisible"/>
   
    <div id="contenedor-dialogo-unidades">
        <div id="dialogo-unidades" title="Unidades" style="display: none;">
        <br />
            <asp:GridView ID="gvUnidades" runat="server" AutoGenerateColumns="false" CellPadding="4" GridLines="None"
                Width="100%" HorizontalAlign="Center" CssClass="Grid" OnRowCommand="grvUnidadesSeleccion_RowCommand" 
                OnPageIndexChanging="grvUnidades_PageIndexChanging" AllowPaging="true" PageSize="5">
                    <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>VIN</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" id="lblNumeroSerieResultado" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroSerie") %>' Width="100%"></asp:Label>    
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>SUCURSAL</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"Sucursal.Nombre") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate># ECON&Oacute;MICO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroEconomico") %>' Width="100px"></asp:Label>    
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>CLIENTE</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente.Nombre") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>&Aacute;REA/DEPTO.</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Area") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>ESTATUS</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EstatusActual") %>' Width="100%"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="50px">
                                <HeaderTemplate>SELECCIONAR</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnSeleccionarUnidad" CommandName="Unidad" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"NumeroSerie") %>' ImageUrl="~/Contenido/Imagenes/VER.png" ToolTip="SELECCIONAR" ImageAlign="Middle" />
                                </ItemTemplate>
                                <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridHeaderGral" />
                    <EditRowStyle CssClass="GridAlternatingRow" />
                    <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                    <RowStyle CssClass="GridRow" />
                    <FooterStyle CssClass="GridFooter" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                    <EmptyDataTemplate>
                        <b>No existen registros que cumplan con la condición solicitada, favor de corregir.</b>
                    </EmptyDataTemplate>
            </asp:GridView>
	    </div>
    </div>
    <div id="contenedor-dialogo-os">
        <div id="dialogo-os" title="ORDEN SERVICIO" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="trAlinearDerecha table-responsive">
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                TIPO ORDEN SERVICIO
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:DropDownList ID="ddTipoOrdenServicio" runat="server" CssClass="input-dropdown-responsive ConvertirMayus">
                                    <asp:ListItem Value="2" Selected="true">
                                        Preventivo
                                    </asp:ListItem>
                                    <asp:ListItem Value="1">
                                        Correctivo
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                <span id="idObligadoInventario" runat="server">*</span>INVENTARIO
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtInventario" Text='<%# Eval("Inventario") %>' runat="server" TextMode="multiline" Columns="30" Rows="2"  ValidationGroup="FormatoValido" CausesValidation="True" CssClass="ConvertirMayus"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                <span>*</span>COMBUSTIBLE ENTRADA
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtCombustible" runat="server" Text='<%# Eval("Combustible") %>' AutoPostBack="true" OnTextChanged="OnClickChangeImage" CssClass="CampoNumeroEntero" ValidationGroup="FormatoValido" CausesValidation="True"></asp:TextBox>
                                <label id="lblCombustibleEntrada" runat="server" class="input-label-responsive">LITROS</label> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                            </td>
                            <td class="input-space-responsive" >&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:Image ID="imgCombustible" runat="server" Height="150px" CssClass="img-responsive" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                <span id="idObligadoCombustibleTotal" runat="server">*</span>COMBUSTIBLE TOTAL 
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtCombustibleTotal" runat="server" Text='<%# Eval("CombustibleTotal") %>' CssClass="CampoNumeroEntero ConvertirMayus" ValidationGroup="FormatoValido" CausesValidation="True"></asp:TextBox>
                                <label id="lblCombustibleTotal" runat="server" class="input-label-responsive">LITROS</label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                <span>*</span>DESCRIPCI&Oacute;N FALLA
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtDescripcionFalla" Text='<%# Eval("DescripcionFalla") %>' TextMode="multiline" Columns="30" Rows="2" runat="server" CssClass="ConvertirMayus"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                <span id="idCodigoFalla" runat="server">*</span>C&Oacute;DIGOS FALLA
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtCodigoFalla" runat="server" Text='<%# Eval("CodigosFalla") %>'  TextMode="multiline" Columns="30" Rows="2" CssClass="ConvertirMayus"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="input-button-responsive">
                                <asp:Button ID="btnCancelarOS" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" onclick="OnclickCancelarOrdenServicio"  />
                                    <asp:Button ID="btnGuardarOS" runat="server" Text="Guardar" CssClass="btnWizardTerminar" onclick="OnclickGuardarOrderServicio" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnValidaCampoPSL" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="contenedor-dialogo-cancelar-ingreso">
        <div id="dialogo-cancelar-ingreso" title="CANCELAR INGRESO" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table class="TextCenter">
                        <tr class="TextCenter">
                            <td colspan="2">
                                <asp:Label ID="Label6" runat="server">Motivo de Cancelaci&oacute;n</asp:Label>
                            </td>
                        </tr>
                        <tr class="TextCenter">
                            <td style="text-align:center;" colspan="2">
                                <asp:TextBox ID="txtMotivoCancelacion" Text='<%# Eval("MotivoCancelacion") %>' TextMode="multiline" Columns="30" Rows="2" runat="server" CssClass="ConvertirMayus"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="TextCenter">
                            <td>
                                <asp:Button ID="btnCancelarCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar" />
                            </td>
                            <td>
                                <asp:Button ID="btnAceptarCancelar" runat="server" Text="Aceptar" CssClass="btnWizardTerminar" onclick="OnclickCancelarIngreso" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="contenedor-dialogo-mantenimientosPendientes">
        <div id="dialogo-mantenimientosPendientes" title="MANTENIMIENTOS PROGRAMADOS PENDIENTES" style="display: none;">
        <br />
            <asp:GridView ID="grvMantenimientosPendientes" runat="server" AutoGenerateColumns="false" CellPadding="4" GridLines="None"
                Width="100%" HorizontalAlign="Center" CssClass="Grid" OnPageIndexChanging="grvMantenimientosPendientes_PageIndexChanging" 
                AllowPaging="true" PageSize="2">
                    <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>VIN</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroSerie") %>' Width="150px"></asp:Label>    
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                             <asp:TemplateField>
                                <HeaderTemplate># ECON&Oacute;MICO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumeroEconomico") %>' Width="100px"></asp:Label>    
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>MODELO</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Modelo") %>' Width="150px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>CLIENTE</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Cliente") %>' Width="150px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>FECHA</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label10" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Fecha") %>' Width="100px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="GridHeaderGral" />
                    <EditRowStyle CssClass="GridAlternatingRow" />
                    <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                    <RowStyle CssClass="GridRow" />
                    <FooterStyle CssClass="GridFooter" />
                    <SelectedRowStyle CssClass="GridSelectedRow" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
            </asp:GridView>
        </div>
    </div>
    <div id="contenedor-dialogo-combustibe-salida">
        <div id="dialogo-combustibe-salida" title="RREGISTRAR COMBUSTIBLE DE SALIDA" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <table class="TextCenter">
                        <tr>
                            <td class="tdCentradoVertical input-label-responsive">
                                <span>*</span>COMBUSTIBLE SALIDA
                            </td>
                            <td class="input-space-responsive">&nbsp;</td>
                            <td class="tdCentradoVertical input-group-responsive">
                                <asp:TextBox ID="txtCombustibleSalida" runat="server" Text='<%# Eval("CombustibleSalida") %>' CssClass="CampoNumeroEntero" ValidationGroup="FormatoValido" CausesValidation="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tdCentradoVertical input-label-responsive">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="input-button-responsive">
                                <asp:Button ID="btnCancelarCombustible" runat="server" Text="Cancelar" 
                                    CssClass="btnWizardCancelar" onclick="btnCancelarCombustible_Click" />
                                 <asp:Button ID="btnAceptarCombustible" runat="server" Text="Guardar" CssClass="btnWizardTerminar" OnClick="OnclickCombustibleSalida" />
                                 <a id="btnRedireccionCalcomania" target="_blank" style="display:none;" href="../Buscador.UI/VisorReporteUI.aspx">Link</a>
                                 <a id="btnRedireccionPaseSalida" target="_blank" style="display:none;" href="../Buscador.UI/VisorReporteUI.aspx?rpt=CU051">Link</a>
                                 <a id="btnRedireccionaEnviarEmailCon" target="_blank" style="display:none;" href="<%= Page.ResolveUrl("~/Mantenimiento.UI/EnviarCorreoServicioRealizadoUI.aspx?p=true")%>">Link Enviar 1</a>
                                 <a id="btnRedireccionaEnviarEmailSin" target="_blank" style="display:none;" href="<%= Page.ResolveUrl("~/Mantenimiento.UI/EnviarCorreoServicioRealizadoUI.aspx?p=false")%>">Link Enviar 2</a>
                                 <a id="btnRedireccionaExportar" target="_blank" style="display:none;" href="../Buscador.UI/ExportaFormatoUI.aspx?formato=xlsx">Export</a>
                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
    <div class="TextCenter" style="width:100%; overflow-x: scroll; overflow-y:hidden;">
        <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
        <asp:HiddenField ID="hdnLibroActivos" runat="server" />
        <asp:Button ID="btnCancelarIngreso" CssClass="btnFooterToolbar" runat="server" Text="CANCELAR INGRESO" OnClick="OnclickAbrirMotivoCancelacion" ToolTip="CANCELA EL INGRESO DE LA UNIDAD AL TALLER"/>
        <asp:Button ID="btnEquiposProgramadosPendientes" CssClass="btnFooterToolbar" runat="server" Text="EQUIPOS PENDIENTES" OnClick="OnclickPendientesPorIngresar" ToolTip="EQUIPOS PROGRAMADOS PENDIENTES DE INGRESAR"/>
        <asp:Button ID="btnImprimirFormatos" CssClass="btnFooterToolbar" runat="server" 
            Text="IMPRIMIR FORMATOS" ToolTip="GENERA LA CALCOMIA DEL MANTENIMIENTO" 
            onclick="btnImprimirFormatos_Click"/>
        <asp:Button ID="btnTareasPendientes" CssClass="btnFooterToolbar" runat="server" Text="TAREAS PENDIENTES" Visible="false" OnClick="OnClickTareasPendientes" ToolTip="PRESENTA LAS TAREAS PENDIENTES PARA LA UNIDAD/MODELO"/>
        <asp:Button ID="btnRegistrarTarea" CssClass="btnFooterToolbar" runat="server" 
            Text="CAPTURAR TAREA" Visible="false" ToolTip="REGISTRA TAREAS PARA LA UNIDAD" 
            onclick="btnRegistrarTarea_Click"/>
        <asp:Button ID="btnExportarMantenimientos" runat="server" Text="EXPORTAR" CssClass="btnFooterToolbar" 
            ToolTip="GENERA UN ARCHIVO DE EXCEL CON LOS RESULTADOS" onclick="btnExportarMantenimientos_Click"/>
        <a id="btnRedireccionVerTareas" target="_blank" style="display:none;" href="ConsultarTareaPendienteUI.aspx">Link</a>
        <a id="btnRedireccionRegistraTarea" target="_blank" style="display:none;" href="RegistrarTareaPendienteUI.aspx">Link</a>
        <asp:HiddenField ID="hdnLeerInline" runat = "server" />
        <asp:HiddenField ID="hdnEsUnidad" runat="server" Value="" />
        <asp:HiddenField ID="hdnCapacidadTanque" runat="server" />
        <br />
        <br />
        <br />
    </div>
</asp:Content>
