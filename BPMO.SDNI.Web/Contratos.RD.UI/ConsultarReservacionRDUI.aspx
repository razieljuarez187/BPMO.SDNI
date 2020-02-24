<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarReservacionRDUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.RD.UI.ConsultarReservacionRDUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>

    <link rel='stylesheet' type='text/css' href='../Contenido/Estilos/Tema.FullCalendar/fullcalendar.css' />
    <script type='text/javascript' src='../Contenido/Scripts/jquery.fullcalendar.js'></script>

    <style type="text/css">
        .GroupSection { width: 650px; margin: 0px auto; }
        .GroupContentCollapsable table { margin: 20px auto; width: 506px; }
        .GroupContentCollapsable .btnComando { margin: 20px auto 0px auto; display: inherit; }        
    </style>
    <script type="text/javascript">
        function inicializeCalendar(array) {
            $('#dvCalendarioCompleto').fullCalendar({
                theme: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                editable: false,
                events: array,
                eventClick: function (calEvent) {
                    $("#dialogReservacion").dialog({
                        autoOpen: true,
                        closeOnEscape: true,
                        modal: true,
                        title: calEvent.tipo + ': ' + calEvent.title + calEvent.activo,
                        resizable: false,
                        minWidth: 450,
                        open: function () {
                            $("#<%= lblFechaInicio.ClientID %>").text($.fullCalendar.formatDate(calEvent.start, 'dd/MM/yyyy hh:mm tt'));
                            $("#<%= lblFechaFin.ClientID %>").text($.fullCalendar.formatDate(calEvent.end, 'dd/MM/yyyy hh:mm tt'));
                            $("#<%= hdnReservacionID.ClientID %>").val(calEvent.id);
                            $("#<%= lblUsuarioReservoNombre.ClientID %>").text(calEvent.usuario);
                            $("#<%= lblClienteNombre.ClientID %>").text(calEvent.cliente);
                            $("#<%= lblObservaciones.ClientID %>").text(calEvent.observaciones);
                        },
                        close: function () {
                            $("#<%= lblFechaInicio.ClientID %>").text("");
                            $("#<%= lblFechaFin.ClientID %>").text("");
                            $("#<%= hdnReservacionID.ClientID %>").val("");
                            $("#<%= lblUsuarioReservoNombre.ClientID %>").text("");
                            $("#<%= lblClienteNombre.ClientID %>").text("");
                            $("#<%= lblObservaciones.ClientID %>").text("");
                            $(this).dialog("destroy"); 
                        },
                        buttons:
                        {
                            Detalles: function () {
                                __doPostBack("<%= btnDetalles.UniqueID %>", "");
                                $(this).dialog("close");
                            }
                        }
                    });
                    $("#dialogReservacion").parent().appendTo("form:first");
                }
            });
            
            if (array.length > 0)
                $('#dvCalendarioCompleto').show();
            else
                $('#dvCalendarioCompleto').hide();
        }
        function destroyCalendar() {
            $('#dvCalendarioCompleto').fullCalendar('destroy');
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            inicializeHorizontalPanels();

            /*Configuración de los campos de fecha*/
            $('.CampoFecha').each(function () {
                if ($(this).attr("disabled") != false && $(this).attr("disabled") != "disabled") {
                    $(this).datepicker({
                        yearRange: '-100:+10',
                        changeYear: true,
                        changeMonth: true,
                        dateFormat: "dd/mm/yy",
                        buttonImage: '../Contenido/Imagenes/calendar.gif',
                        buttonImageOnly: true,
                        toolTipText: "Fecha",
                        showOn: 'button'
                    });

                    $(this).attr('readonly', true);
                }
            });

            /*Configuración de los campos de hora*/
            if ($('#<%= txtHoraInicio.ClientID %>').length > 0) {
                $('#<%= txtHoraInicio.ClientID %>').timepicker({
                    showPeriod: true,
                    showLeadingZero: true
                });
            }
            $('#<%= txtHoraInicio.ClientID %>').attr('readonly', true);

            if ($('#<%= txtHoraFinal.ClientID %>').length > 0) {
                $('#<%= txtHoraFinal.ClientID %>').timepicker({
                    showPeriod: true,
                    showLeadingZero: true
                });
            }
            $('#<%= txtHoraFinal.ClientID %>').attr('readonly', true);
        }

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
        function BtnBuscar(guid, xml) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#<%=btnResult.ClientID %>"),
                features: {
                    dialogWidth: width,
                    dialogHeight: '280px',
                    center: 'yes',
                    maximize: '0',
                    minimize: 'no'
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!-- Barra de localización -->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - CONSULTAR RESERVACI&Oacute;N</asp:Label>
        </div>
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 64px;">
                <li id="ConsultarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkConsultar" runat="server" NavigateUrl="~/Contratos.RD.UI/ConsultarReservacionRDUI.aspx"
                        ForeColor="White">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li id="RegistrarCatalogo">
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.RD.UI/RegistrarReservacionRDUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
        <!-- Cuerpo -->
        <div id="Formulario" class="GroupSection">
            <div id="EncabezadoDatosCatalogo" class="GroupHeaderCollapsable">
                <table>
                    <tr>
                        <td>¿QU&Eacute; RESERVACI&Oacute;N DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical"># Reservaci&oacute;n</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumero" runat="server" MaxLength="30" Width="275px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical" style="width: 150px;">Sucursal</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" colspan="4">
                            <asp:TextBox ID="txtSucursal" runat="server" Width="250px" AutoPostBack="true" MaxLength="50" ToolTip="Sucursal" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar sucursales" CommandArgument='' OnClick="ibtnBuscaSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Cliente</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNombreCuentaCliente" runat="server" MaxLength="150" Width="275px"
                                AutoPostBack="True" OnTextChanged="txtNombreCuentaCliente_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarCliente" CommandName="VerCliente"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Clientes" CommandArgument=''
                                OnClick="ibtnBuscarCliente_Click" />
                            <asp:HiddenField runat="server" ID="hdnCuentaClienteID"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Modelo</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNombreModelo" runat="server" MaxLength="70" Width="275px" AutoPostBack="True" 
                                OnTextChanged="txtNombreModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarModelo" CommandName="VerModelo"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Modelos" CommandArgument=''
                                OnClick="ibtnBuscarModelo_Click" />
                            <asp:HiddenField runat="server" ID="hdnModeloID"/>
                        </td>
                    </tr>
                    <tr>
                        <td class = "tdCentradoVertical"># Econ&oacute;mico</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" MaxLength="70" Width="275px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td class="tdCentradoVertical">Fecha Inicio de Reservaci&oacute;n</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaInicio" runat="server" MaxLength="11" CssClass="CampoFecha" Width="90px"></asp:TextBox>
                            <asp:TextBox ID="txtHoraInicio" runat="server" MaxLength="7" CssClass="CampoHora" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td class="tdCentradoVertical">Fecha Fin de Reservaci&oacute;n</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtFechaFinal" runat="server" MaxLength="11" CssClass="CampoFecha" Width="90px"></asp:TextBox>
                            <asp:TextBox ID="txtHoraFinal" runat="server" MaxLength="7" CssClass="CampoHora" Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Usuario Reserv&oacute;</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:TextBox ID="txtUsuarioReservoNombre" runat="server" MaxLength="150" Width="275px" AutoPostBack="True" 
                                OnTextChanged="txtUsuarioReservoNombre_TextChanged"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscaUsuarioReservo" CommandName="VerUsuarioReservo"
                                ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar Empleados" CommandArgument=''
                                OnClick="ibtnBuscarUsuarioReservo_Click" />
                            <asp:HiddenField runat="server" ID="hdnUsuarioReservoID"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">Estatus</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlActivo" runat="server" Width="200px">
                                <asp:ListItem Value="True" Text="ACTIVA" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="False" Text="CANCELADA"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btnComando" OnClick="btnBuscar_Click" />
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>

        <div id="dvCalendarioCompleto">
        </div>
    </div>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    <div id="dialogReservacion" style="display: none;">
        Del 
        <asp:Label ID="lblFechaInicio" runat="server" Text=""></asp:Label>
         al 
        <asp:Label ID="lblFechaFin" runat="server" Text=""></asp:Label>
        <br />
        Cliente: <asp:Label ID="lblClienteNombre" runat="server" Text=""></asp:Label>
        <br />
        Reservado por: <asp:Label ID="lblUsuarioReservoNombre" runat="server" Text=""></asp:Label>
        <br />
        Observaciones: <asp:Label ID="lblObservaciones" runat="server" Text=""></asp:Label>
        <asp:HiddenField ID="hdnReservacionID" runat="server" />
        <asp:Button ID="btnDetalles" runat="server" Text="Button" OnClick="btnDetalles_Click" Style="display: none;" />
    </div>
</asp:Content>
