<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ProgramarMantenimientosUI.aspx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ProgramarMantenimientosUI" %>
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
        .SinConfirmarColor { background-color : White; color: Black; }
        .RecalendarizadoColor { background-color : Yellow; color: Black; }
        .ATiempoColor { background-color : Green; }
        .PequenioRetrasoColor { background-color : Orange; }
        .RetrasadoColor { background-color : Red; }
        .rowFull { width: 100%;}
        .rowMargin { width: 99%; margin-right:10px; margin-top:0; margin-bottom:0px; margin-left:10px; }
        .rowHalve { width: 48%; display:inline-block;}
    </style>
    <script type="text/javascript">
        function inicializeCalendar(array, days) {
            var dateToday = new Date();
            dateToday = dateToday.format("dd/MM/yyyy");
            $('#dvCalendarioCompleto').fullCalendar({
                theme: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                editable: false,
                events: array,
                timeFormat: 'H(:mm)',
                dayRender: function (date, cell) {
                    var daDate = $.fullCalendar.formatDate(date, 'dd/MM/yyyy');
                    if (daDate == dateToday) {
                        cell.css("background", "Aqua");
                    }
                    else { }
                    if (days != null) {
                        if (days.length > 0) {
                            if (days.some(function (e) {
                                var inhabilDate = e;
                                return inhabilDate == daDate; 
                                })) {
                                cell.css("background", "Grey");
                            }
                        }
                    }
                },
                eventClick: function (calEvent) {
                    $("#dialogDetalle").dialog({
                        autoOpen: true,
                        closeOnEscape: false,
                        modal: true,
                        title: 'CITA DE MANTENIMIENTO',
                        resizable: false,
                        minWidth: 450,
                        open: function () {
                            $("#<%= lblFechaCita.ClientID %>").text($.fullCalendar.formatDate(calEvent.start, 'dd/MM/yyyy hh:mm tt'));
                            $("#<%= lblVINNumEconomico.ClientID %>").text(calEvent.VIN_NUM);
                            $("#<%= lblTipoMantenimiento.ClientID %>").text(calEvent.tipoMantenimiento);
                            $("#<%= hdnIdCita.ClientID %>").val(calEvent.id);
                            $("#<%= hdnEsCita.ClientID %>").val(calEvent.programada);
                        },
                        close: function () {
                            $("#<%= lblFechaCita.ClientID %>").text("");
                            $("#<%= lblVINNumEconomico.ClientID %>").text("");
                            $("#<%= lblTipoMantenimiento.ClientID %>").text("");
                            $("#<%= hdnIdCita.ClientID %>").val("");
                            $("#<%= hdnEsCita.ClientID %>").val("");
                            $(this).dialog("destroy");
                        },
                        buttons:
                        {
                            DETALLES: function () {
                                __doPostBack("<%= btnDetalles.UniqueID %>", "");
                                $(this).dialog("close");
                            }
                        }
                    });
                    $("#dialogDetalle").parent().appendTo("form:first");
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
        function PresentarDetalleCita() {
            $("#dialogCitaMantenimiento").dialog({
                autoOpen: true,
                closeOnEscape: false,
                modal: false,
                title: 'CITA DE MANTENIMIENTO',
                resizable: false,
                minWidth: 950,
                open: function () {
                },
                close: function () {
                    $(this).dialog("destroy");
                },
                buttons:
                [
                  {
                    text: "CANCELAR",
                    click: function () {
                        $(this).dialog("close");
                        }
                  },
                  {
                    text: "PROGRAMAR",
                    click: function () {
                        __doPostBack("<%= btnDetallesCita.UniqueID %>", "");
                        $(this).dialog("close");
                        __blockUI();
                      }
                  }
                ]              
            });
            $("#dialogCitaMantenimiento").parent().appendTo("form:first");
        }
        function IrContacto() {
            document.getElementById("btnRedireccion").click();
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
                    dialogHeight: '300px',
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
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">MANTENIMIENTO - PROGRAMAR MANTENIMIENTOS</asp:Label>
        </div>
        <div style="height: 32px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario" style="float: left; height: 32px;">
                <li id="RegistrarCatalogo" class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Mantenimiento.UI/ProgramarMantenimientosUI.aspx">
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
                        <td>¿QU&Eacute; MANTENIMIENTOS DESEA CONSULTAR?</td>
                        <td>
                            <img id="img1" class="imgMenu" src="../Contenido/Imagenes/FlechaAbajo.png" alt="Click para Ocultar/Mostrar" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GroupContentCollapsable">
                <table class="trAlinearDerecha">
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
                        <td class="tdCentradoVertical">Taller</td>
                        <td style="width: 20px;">&nbsp;</td>
                        <td class="tdCentradoVertical" style="width: 320px;">
                            <asp:DropDownList ID="ddlTaller" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div style="text-align:center;">
                    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btnComando" OnClick="btnBuscar_Click" />
                </div >
                <div class="ContenedorMensajes">
                    <span class="Requeridos"></span>
                    <br />
                    <span class="FormatoIncorrecto"></span>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="udpCalendar" runat="server">
            <ContentTemplate>
                <div id="dvCalendarioCompleto">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    <div id="dialogDetalle" class="GroupBody" style="display: none;">
        Fecha de la Cita: <asp:Label ID="lblFechaCita" runat="server"></asp:Label>
        <br />
        VIN/Número Económico: <asp:Label ID="lblVINNumEconomico" runat="server"></asp:Label>
        <br />
        Tipo Mantto: <asp:Label ID="lblTipoMantenimiento" runat="server"></asp:Label>
        <asp:HiddenField ID="hdnIdCita" runat="server"/>
        <asp:HiddenField ID="hdnEsCita" runat="server"/>
        <asp:Button ID="btnDetalles" runat="server" Text="Button" OnClick="btnDetalles_Click" Style="display: none;" />
    </div>
    <asp:UpdatePanel ID="udpDetalleCita" runat="server" style="width: 100%;">
        <ContentTemplate>
            <div id="dialogCitaMantenimiento" class="GroupBody" style="width: 100%;display: none;">
                <div class="rowFull" style="text-align:center;">
                    <h3><span>Programación Matenimiento</span></h3>
                </div>
                <br />
                <div class="rowMargin">
                    <fieldset>
                        <legend>
                            <span>Información del Vehiculo</span>
                        </legend>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        VIN:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtVINUnidad" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        KM Último Servicio:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtKMUltimoServicio" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        # Económico:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtNumeroEconomico" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Fecha Último Servicio:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtFechaUltimoServicio" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Placa Estatal:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtPlacaEstatal" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Placa Federal:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtPlacaFederal" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Tipo Mantto:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtTipoMantto" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Fecha Sugerida:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtFechaSugerida" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Sucursal:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtNombreSucursalDetail" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Taller:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtNombreTallerDetail" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        CLIENTE:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtClienteNombre" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        ÁREA:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtArea" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div style="width: 80%; margin: 0 auto;">
                            <fieldset>
                                <legend>Equipos Aliados</legend>
                                <asp:GridView ID="grvEquiposAliados" runat="server" AutoGenerateColumns="False" CssClass="Grid" Width="100%" style="margin: 0 auto;">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>MARCA</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.Modelo.Marca.Nombre") %>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>MODELO</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.Modelo.Nombre") %>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>AÑO</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAnio" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.Anio") %>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>SERIE</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSerie" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.NumeroSerie") %>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="30%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Tipo Mantto</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTipoMantto" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimientoNombre") %>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="GridRow" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <FooterStyle CssClass="GridFooter" />
                                    <PagerStyle CssClass="GridPager" />
                                    <SelectedRowStyle CssClass="GridSelectedRow" />
                                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                    <EmptyDataTemplate>
                                        <label>NO SE ENCUENTRAN EQUIPOS ALIADOS</label>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </fieldset>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <span>Información de Mantenimiento</span>
                        </legend>
                        <div class="rowFull rowMargin">
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Estatus:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtEstatusMantto" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rowHalve">
                                <div class="rowHalve">
                                    <label>
                                        Días Retraso:
                                    </label>
                                </div>
                                <div class="rowHalve">
                                    <asp:TextBox ID="txtDiasRetraso" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <span>Información del Cliente</span>
                        </legend>
                        <div style="width: 80%; margin: 0 auto;">
                            <asp:GridView ID="grvContactoCliente" runat="server" 
                                AutoGenerateColumns="False" CssClass="Grid" width="100%" 
                                onpageindexchanging="grvContactoCliente_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>SUCURSAL</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSucursal" Text='<%# DataBinder.Eval(Container.DataItem,"ContactoCliente.Sucursal.Nombre") %>' Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Nombre</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"Nombre") %>' Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Direccion</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"ContactoCliente.Direccion") %>' Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Telefono</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAnio" Text='<%# DataBinder.Eval(Container.DataItem,"Telefono") %>' Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>E-MAIL</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSerie" Text='<%# DataBinder.Eval(Container.DataItem,"Correo") %>' Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%"/>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="GridRow" />
                                <HeaderStyle CssClass="GridHeader" />
                                <FooterStyle CssClass="GridFooter" />
                                <PagerStyle CssClass="GridPager" />
                                <SelectedRowStyle CssClass="GridSelectedRow" />
                                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                <EmptyDataTemplate>
                                    <label>NO SE ENCUENTRAN ASIGNADOS CONTACTOS DEL CLIENTE</label>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <div style="text-align:center;">
                                <asp:Button runat="server" ID="btnRedirectContacto" Text="AGREGAR" CssClass="btnComando" OnClick="btnRedirectContacto_Click" />
                                <a id="btnRedireccion" target="_blank" style="display:none;" href="ConsultarContactoClienteUI.aspx">Link</a>
                            </div>
                        </div>
                    </fieldset>
                    <br />                   
                </div>
                <asp:HiddenField ID="hdnReservacionID" runat="server" />
                <asp:Button ID="btnDetallesCita" runat="server" Text="Button" OnClick="btnDetallesCita_Click" Style="display: none;" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDetalles" EventName="" />
            <asp:AsyncPostBackTrigger ControlID="btnRedirectContacto" EventName="" />
        </Triggers>
    </asp:UpdatePanel>    
</asp:Content>
