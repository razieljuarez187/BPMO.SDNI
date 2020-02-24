<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDatosCitaMantenimientoUI.ascx.cs" Inherits="BPMO.SDNI.Mantenimiento.UI.ucDatosCitaMantenimientoUI" %>
<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
<style type="text/css">
    .rowFull { width: 100%;}
    .rowMargin { width: 99%; margin-right:10px !important; margin-left:10px !important; }
    .rowHalve { width: 48%; display:inline-block;}
    .rowSpace { width: 2%; display:inline-block;}
    .AlinearDerecha { text-align: right;}
</style>
<script type="text/javascript">
// Inicializa el control de Información General
    function <%= ClientID %>_Inicializar() {
        var FechaCita = $('#<%= txtFechaCita.ClientID %>');
        if (FechaCita.length > 0) {
            FechaCita.datepicker({
                yearRange: '-10:+7',
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha de Cita de Mantenimiento",
                showOn: 'button',
                defaultDate: (FechaCita.val().length == 10) ? FechaCita.val() : new Date()
            });

            FechaCita.attr('readonly', true);
        }
         
        /*Configuración de los campos de hora*/
        $('.CampoHora').each(function () {
            if ($(this).length > 0) {
                $(this).timepicker({
                    showPeriod: true,
                    showLeadingZero: true,
                    showCloseButton: true
                });
            }
            $(this).attr('readonly', true);
            $('.ui-timepicker-close').click();
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
    function IrContacto() {
        document.getElementById("btnRedireccion").click();
    }
</script>
 <script type="text/javascript">
          function confirmarDiaHabil(origen) {
              var $div = $('<div title="Advertencia"></div>');
              var CodError = document.getElementById("MainContent_ucDatosCitaMantenimiento_LabelError").innerHTML;
              var CodError = CodError.split('\n').join('<br>');
              $div.append(CodError);
              $("#dialog:ui-dialog").dialog("destroy");
              $($div).dialog({
                  closeOnEscape: true,
                  modal: true,
                  minWidth: 460,
                  close: function () { $(this).dialog("destroy"); },
                  buttons: {
                      Aceptar: function () {
                          $(this).dialog("close");
                      },
                  }
              });
          }
    </script>
<div style="width: 90%; margin: 0 auto;">
    <asp:Label ID="LabelError" runat="server" Text="" style="display: none;"></asp:Label>
    <div id="dialogCitaMantenimiento" class="GroupBody" style="width: 100%;  margin: 0 auto;">
        <div id="divInformacionGeneralHeader" class="GroupHeader">
            <asp:Label runat="server" ID="lblTitulo" Text="Programación Matenimiento"></asp:Label>
            <div id="grpOpciones" class="GroupHeaderOpciones Ancho1Opciones">
                
            </div>
        </div>
        <div class="rowFull">
            <br />
            <fieldset style="width: 95%; margin: 0 auto;">
                <legend><span>INFORMACÍON DEL VEHÍCULO</span> </legend>
                <div class="rowFull">
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                CLIENTE:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtNombreCliente" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="rowSpace"></div>
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                ÁREA/DEPTO:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtArea" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="rowFull">
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                VIN:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtVINUnidad" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="rowSpace"></div>
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                PLACA ESTATAL:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtPlacaEstatal" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="rowFull">
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                # Económico:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtNumeroEconomico" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="rowSpace"></div>
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                Placa Federal:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtPlacaFederal" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="rowFull">
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                Tipo Mantto:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtTipoMantto" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="rowSpace"></div>
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <label>
                                Tiempo Mantto:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtTiempoMantenimiento" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="rowFull">
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <span>*</span>
                            <label>
                                Sucursal:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtNombreSucursalDetail" runat="server" Width="80%" OnTextChanged="txtNombreSucursalDetail_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="ibtnBuscarSucursal" CommandName="VerSucursal" ImageUrl="~/Contenido/Imagenes/Detalle.png" ToolTip="Consultar sucursales" CommandArgument='' OnClick="ibtnBuscaSucursal_Click" />
                            <asp:HiddenField ID="hdnSucursalID" runat="server" Visible="False" />
                        </div>
                    </div>
                    <div class="rowSpace"></div>
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <span>*</span>Taller:
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:DropDownList ID="ddlTaller" runat="server" Width="90%" AutoPostBack="true"
                                onselectedindexchanged="ddlTaller_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="rowFull">
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <span>*</span>
                            <label>
                                Fecha Cita:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtFechaCita" runat="server" Width="80%" AutoPostBack="True" 
                                ontextchanged="txtFechaCita_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <div class="rowSpace"></div>
                    <div class="rowHalve">
                        <div class="rowHalve AlinearDerecha">
                            <span>*</span>
                            <label>
                                Hora Cita:
                            </label>
                        </div>
                        <div class="rowSpace"></div>
                        <div class="rowHalve">
                            <asp:TextBox ID="txtHoraCita" runat="server" Width="90%" Class="CampoHora"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div style="width: 80%; margin: 0 auto;">
                    <fieldset>
                        <legend>Equipos Aliados</legend>
                        <asp:GridView ID="grvEquiposAliados" runat="server" AutoGenerateColumns="False" CssClass="Grid" Width="100%" Style="margin: 0 auto;">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        MARCA</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMarca" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.Modelo.Marca.Nombre") %>'
                                            Width="100%"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        MODELO</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblModelo" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.Modelo.Nombre") %>'
                                            Width="100%"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        AÑO</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblAnio" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.Anio") %>'
                                            Width="100%"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SERIE</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSerie" Text='<%# DataBinder.Eval(Container.DataItem,"EquipoAliado.NumeroSerie") %>'
                                            Width="100%"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Tipo Mantto</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblTipoMantto" Text='<%# DataBinder.Eval(Container.DataItem,"TipoMantenimientoNombre") %>'
                                            Width="100%"></asp:Label>
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
                        <div style="margin-top:10px"></div>
                    </fieldset>
                </div>
            </fieldset>
            <fieldset style="width: 95%; margin: 0 auto;">
                <legend><span>Información del Cliente</span> </legend>
                <div style="width: 95%; margin: 0 auto;">
                    <asp:UpdatePanel ID="udpContacto" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grvContactoCliente" runat="server" 
                                AutoGenerateColumns="False" CssClass="Grid" Width="100%" 
                                onrowcommand="grvContactoCliente_RowCommand" 
                                onrowdatabound="grvContactoCliente_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sucursal
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSucursalContacto" Text='<%# DataBinder.Eval(Container.DataItem,"ContactoCliente.Sucursal.Nombre") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Nombre</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblNombreContacto" Text='<%# DataBinder.Eval(Container.DataItem,"Nombre") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Direccion</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDireccion" Text='<%# DataBinder.Eval(Container.DataItem,"ContactoCliente.Direccion") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Telefono</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTelefono" Text='<%# DataBinder.Eval(Container.DataItem,"Telefono") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            E-MAIL</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCorreoElectronico" Text='<%# DataBinder.Eval(Container.DataItem,"Correo") %>'
                                                Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnSeleccionar" ImageUrl="~/Contenido/Imagenes/VER.png"
						                    ToolTip="Seleccionar" CommandName="CMDSELECT" CommandArgument='<%#Container.DataItemIndex%>'
						                    ImageAlign="Middle" />
                                        </ItemTemplate>
                                        <HeaderStyle />
                                        <ItemStyle HorizontalAlign="Center" Width="17px"/>
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnReloadContactos" />
                            <asp:AsyncPostBackTrigger ControlID="btnRedireccionContacto" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTaller" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div style="margin-top:10px">
                        <div style="width: 100%; margin:0 auto;text-align:center;">
                            <asp:Button ID="btnReloadContactos" runat="server" Text="RECARGAR" 
                                CssClass="btnComando" onclick="btnReloadContactos_Click" />
                            &nbsp;
                            <asp:Button ID="btnRedireccionContacto" runat="server" Text="AGREGAR" 
                                CssClass="btnComando" OnClientClick="IrContacto();"/>
                            <a id="btnRedireccion" target="_blank" style="display:none;" href="ConsultarContactoClienteUI.aspx">Link</a>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
        </div>
        <div class="ContenedorMensajes">
            <span class="Requeridos RequeridosFSL"></span>
            <br />
            <span class="FormatoIncorrecto FormatoIncorrectoFSL"></span>
        </div>
        <asp:HiddenField ID="hdnReservacionID" runat="server" />
        <asp:HiddenField ID="hdnEsExterno" runat="server" />
        <asp:HiddenField ID="hdnMantenimientoProgramadoID" runat="server" />
        <asp:HiddenField ID="hdnCitaMantenimientoID" runat="server" />
        <asp:HiddenField ID="hdnEstatusCita" runat="server" />
        <asp:HiddenField ID="hdnUnidadID" runat="server" />
        <asp:HiddenField ID="hdnEquipoID" runat="server" />
        <asp:HiddenField ID="hdnClienteID" runat="server" />
        <asp:HiddenField ID="hdnContactoClienteID" runat="server" />
        <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    </div>
</div>
