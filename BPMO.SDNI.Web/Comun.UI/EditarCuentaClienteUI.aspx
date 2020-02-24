<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarCuentaClienteUI.aspx.cs" Inherits="BPMO.SDNI.Comun.UI.EditarCuentaClienteUI" %>
<%@ Register Src="~/Comun.UI/ucDatosActaConstitutivaUI.ascx" TagPrefix="uc" TagName="ucDatosActaConstitutivaUI" %>
<%@ Register Src="~/Comun.UI/ucDatosRepresentanteLegalUI.ascx" TagName="ucDatosRepresentanteLegalUI"
    TagPrefix="UC1" %>
<%@ Register Src="~/Comun.UI/ucDatosObligadoSolidarioUI.ascx" TagName="ucDatosObligadoSolidarioUI"
    TagPrefix="UC2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Funcionalidad Deshabilitar Enter en cajas de texto-->
    <script src="<%=
Page.ResolveUrl("../Contenido/Scripts/jidealease.extension.js") %>" type="text/javascript"></script>
    <style>
        .no-close .ui-dialog-titlebar-close
        {
            display: none;
        }
        .etiquetaControl
        {
            max-width: 200px;
            min-width: 200px;
            width: 200px;
            text-align: right;
        }
        .control
        {
            max-width: 200px;
            min-width: 200px;
            width: 200px;
        }
        .espacio
        {
            max-width: 20px;
            min-width: 20px;
            width: 20px;
        }
        .Correo { text-transform: none !important}
        
        	.InputReset,
        select
        {
	        height: 25px;
	        width: 82%;
	        font-family : Century Gothic, Arial, Verdana, Serif; 
	
	        border-color:#d7d7d7; 
	        border-style:solid; 
	        border-width:1px;
	        border-radius:5px; -moz-border-radius:5px; -webkit-border-radius:5px; -khtml-border-radius:5px;
        }
        .InputReset
        {
	        padding-right:5px; padding-left:5px;
        }
        .InputReset,
        select[disabled]
        {
	        border-color:#d2d2d2; 
	        border-style:solid; 
	        border-width:1px;
        }
    </style>
    <link href="../Contenido/Estilos/EstiloCatCliente.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" id="JQuerySection">
        $(document).ready(function () {
            initChild();
        });
    </script>
    <script language="javascript" type="text/javascript" id="JavaScriptFunctions">
        initChild = function () {
            if (typeof MostrarOcultarRepresentantesObligados == 'function')
                MostrarOcultarRepresentantesObligados();

            ConfiguracionBarraHerramientas();

            //Formato de Fechas
            $('.CampoFecha').datepicker({
                yearRange: '-100:+10',
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha de compra",
                showOn: 'button'
            });

            $('.CampoFecha').attr('readonly', true);

            //Asignacion de Eventos
            $(".CURP").keyup(function () {
                this.value = this.value.toUpperCase();
            });
        };

        function DialogoDetalleObligado() {
            $("#DialogRepresentantesObligados").dialog({
                modal: true,
                width: 900,
                height: 400,
                resizable: false,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#DialogRepresentantesObligados").parent().appendTo("form:first");
        }
        //Método para cambiar etiquetas de formulario, según a la unidad operativa a la que pertenezca.
        function InicializarControlesEmpresas(json) {
            var obj = JSON.parse(json);
            document.getElementById("lblDiasUso").innerHTML = obj.CLI01;
            document.getElementById("lblTipoCuenta").innerHTML = obj.CLI02;
            document.getElementById("lblHorasUso").innerHTML = obj.CLI03;
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
        <!--Barra de Localización-->
        <div id="BarraUbicacion">
            <asp:Label ID="lblEncabezadoLeyenda" runat="server">CAT&Aacute;LOGOS - EDITAR CLIENTE</asp:Label>
        </div>
        <!--Menú secundario-->
        <div style="height: 80px;">
            <!-- Menú secundario -->
            <ul id="MenuSecundario">
                <li class="MenuSecundarioSeleccionado">
                    <asp:HyperLink ID="hlConsultar" runat="server" NavigateUrl="~/Comun.UI/ConsultarCuentaClienteUI.aspx">
						CONSULTAR
						<img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlRegistroOrden" runat="server" NavigateUrl="~/Comun.UI/RegistrarCuentaClienteUI.aspx">
						REGISTRAR
						<img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" />
                    </asp:HyperLink>
                </li>
            </ul>
            <div id="BarraHerramientas" style="float: right;">
                <asp:Menu runat="server" ID="mCuentaCliente" IncludeStyleBlock="False" Orientation="Horizontal"
                    CssClass="MenuPrimario">
                    <Items>
                        <asp:MenuItem Text="Cliente" Value="ClienteID" Enabled="False" Selectable="false">
                        </asp:MenuItem>
                        <asp:MenuItem Text="Editar" Value="Editar" Selected="true" NavigateUrl="#"></asp:MenuItem>
                    </Items>
                    <StaticItemTemplate>
                        <asp:Label runat="server" ID="lblOpcion" CssClass='<%# (string) Eval("Value") == "ClienteID" ? "Informacion" : string.Empty %>'
                            Text='<%# Eval("Text") %>'></asp:Label>
                        <asp:TextBox runat="server" ID="txtValue" Visible='<%# (string) Eval("Value") == "ClienteID" %>'
                            Style="width: 100px" CssClass="textBoxDisabled" ReadOnly="true"></asp:TextBox>
                    </StaticItemTemplate>
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle CssClass="SubMenuImpresion" Width="200px" BackColor="White" />
                    </LevelSubMenuStyles>
                    <DynamicHoverStyle CssClass="itemSeleccionado" />
                    <DynamicSelectedStyle CssClass="itemSeleccionado"></DynamicSelectedStyle>
                    <StaticSelectedStyle CssClass="itemSeleccionado"></StaticSelectedStyle>
                </asp:Menu>
                <div class="Ayuda" style="float: right">
                    <input id="btnAyuda" type="button" class="btnAyuda" onclick="ShowHelp();" />
                </div>
            </div>
            <div class="BarraNavegacionExtra">
                <input id="btnNuevoConsulta" type="button" value="Nueva Consulta" onclick="window.location='<%= Page.ResolveUrl("~/Comun.UI/ConsultarCuentaClienteUI.aspx") %>'" />
            </div>
        </div>
        <asp:MultiView runat="server" ID="mvwEditarCuentaCliente">
            <asp:View runat="server" ID="viewEdicion">
                <!--Edición del Cliente-->
                <div id="divInformacionGeneral" class="GroupBody">
                    <div id="divInformacionHeader" class="GroupHeader">
                        <span>Cliente</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnGuardar" runat="server" Text="Actualizar" OnClick="btnGuardar_Click"
                                CssClass="btnWizardGuardar" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                                CssClass="btnWizardCancelar" />
                        </div>
                    </div>
                    <div id="divInformacionClienteControles">
                        <table class="trAlinearDerecha">
                            <tr>
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <label>
                                        Nombre</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" colspan="4">
                                    <asp:TextBox ID="txtNombreCliente" runat="server" Enabled="false" Width="301px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <label>
                                        Tipo Contribuyente</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 230px;">
                                    <asp:TextBox ID="txtTipoContribuyente" runat="server" Enabled="false" Columns="10"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical" style="width: 200px; text-align: right;">
                                    <label>
                                        RFC</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox ID="txtRFC" runat="server" Enabled="false" Columns="16"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <label>
                                        Cuenta Oracle</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 230px;">
                                    <asp:TextBox runat="server" ID="txtNumeroCuentaOracle" Enabled="False" Width="95%" />
                                </td>
                                <td class="tdCentradoVertical" style="width: 200px; text-align: right;">
                                    <span>*</span><label id="lblTipoCuenta">Tipo de Cuenta</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:DropDownList runat="server" ID="ddlTipoCuenta" Enabled="False">
                                        <asp:ListItem Text="Seleccion una opción" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <span id="opcionalMoral">*</span><label>CURP</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 230px;">
                                    <asp:TextBox runat="server" ID="txtCURP" Columns="25" CssClass="CURP" MaxLength="18"></asp:TextBox>
                                </td>
                                <td class="tdCentradoVertical" style="width: 200px; text-align: right;">
                                    <span id="opcional"></span><label>CORREO</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" type="email" name="mail" ID="txtCorreo" 
                                        Columns="25" CssClass="Correo InputReset" 
                                        MaxLength="100" Width="178px"></asp:TextBox>
                                </td>
                                
                            </tr>
                             <tr>
                                <td class="tdCentradoVertical" style="width: 200px;">
                                    <span id="Span1"></span><label id="lblDiasUso">DIAS USO UNIDAD(MES)</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical" style="width: 230px;">
                                    <asp:TextBox runat="server" ID="txtDiasUso" Columns="25" CssClass="InputReset CampoNumeroEntero" 
                                        MaxLength="18" Width="31px"></asp:TextBox>
                                    <%--ComboBox para el campo Sector--%>
                                    <asp:DropDownList runat="server" ID="ddlSector" Columns="26" 
                                                      Enabled="true" Visible="false" OnSelectedIndexChanged="ddlSector_SelectedIndexChanged" >
                                        
                                    </asp:DropDownList>
                                </td>
                                <td class="tdCentradoVertical" style="width: 200px; text-align: right;">
                                    <span id="Span2"></span><label id="lblHorasUso">HORAS USO UNIDAD(DIA)</label>
                                </td>
                                <td style="width: 20px;">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical">
                                    <asp:TextBox runat="server" ID="txtHorasUso" Columns="25" CssClass="InputReset CampoNumeroEntero" 
                                        MaxLength="18" Width="30px" EnableTheming="True" 
                                        AutoPostBack="True" ontextchanged="txtHorasUso_TextChanged"></asp:TextBox>
                                       
                                    <asp:TextBox runat="server" ID="txtTelefonos" CssClass="InputReset CampoNumeroEntero" 
                                                 Columns="26" Enabled="False" Visible="False" MaxLength="10"></asp:TextBox>
                                   
                                 </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                
                                <td>
                                    <asp:Button ID="btnMas"  CssClass="btnAgregarATabla" runat="server" 
                                            Style="float: left; margin: 0 30px 10px 0" Visible="False" 
                                            onclick="btnMas_Click" Text="Agregar a tabla"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:GridView ID="grdvTelefonos" runat="server" AutoGenerateColumns="False"   Style="float: left;"
                                        AllowPaging="True" PageSize="5" Width="166px" 
                                        onrowdeleting="grdvTelefonos_OnRowDeleting" GridLines="None" 
                                        onpageindexchanging="grdvTelefonos_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField HeaderText="Teléfonos" DataField="Telefono">
                                            </asp:BoundField>
                                             <asp:CommandField ShowDeleteButton="True" ButtonType="Image" 
                                                DeleteImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" DeleteText="" /> 
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <EditRowStyle CssClass="GridAlternatingRow" />
                                        <PagerStyle CssClass="GridPager" />
                                        <RowStyle CssClass="GridRow" />
                                        <FooterStyle CssClass="GridFooter" />
                                        <SelectedRowStyle CssClass="GridSelectedRow" />
                                        <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Panel ID="pnlActaConstitutiva" runat="server" Visible="true">
                    <div id="divActaConstitutiva" class="GroupBody">
                        <div id="divActaConstitutivaHeader" class="GroupHeader">
                            <span>Acta Constitutiva</span>
                        </div>
                        <div id="divInformacionActaControles">                           
                            <uc:ucDatosActaConstitutivaUI runat="server" ID="ucDatosActaConstitutiva" />
                            <div id="ActasConstitutivas">
                                <asp:Button ID="btnAgregarActa" runat="server" Text="Actualizar" Style="float: right;
                                    margin: 0 30px 10px 0" OnClick="btnAgregarActa_Click" CssClass="btnWizardGuardar" />
                                <asp:Button ID="btnLimpiarActa" runat="server" Text="Limpiar" Style="float: right;
                                    margin: 0 10px 10px 0" OnClick="btnLimpiarActa_Click" CssClass="btnWizardCancelar" />
                                <asp:GridView ID="grvActasConstitutivas" runat="server" AutoGenerateColumns="false"
                                    CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                                    AllowSorting="True" OnRowCommand="grvActasConstitutivas_RowCommand" OnPageIndexChanging="grvActasConstitutivas_PageIndexChanging"
                                    Width="95%">
                                    <Columns>
                                        <asp:BoundField HeaderText="Numero Escritura" DataField="NumeroEscritura"></asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha Escritura" DataField="FechaEscritura" DataFormatString="{0:d}" />
                                        <asp:BoundField HeaderText="Notario" DataField="NombreNotario"></asp:BoundField>
                                        <asp:CheckBoxField HeaderText="Activo" DataField="Activo" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="ibtDetalle" ImageUrl="~/Contenido/Imagenes/VER.png"
                                                    ToolTip="Ver Detalles" CommandName="CMDDEDITAR" CommandArgument='<%# Eval("Id")%>' /></ItemTemplate>
                                            <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <EditRowStyle CssClass="GridAlternatingRow" />
                                    <PagerStyle CssClass="GridPager" />
                                    <RowStyle CssClass="GridRow" />
                                    <FooterStyle CssClass="GridFooter" />
                                    <SelectedRowStyle CssClass="GridSelectedRow" />
                                    <AlternatingRowStyle CssClass="GridAlternatingRow" />
                                </asp:GridView>
                            </div>                           
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlRegistroHacienda" runat="server" Visible="false">
                    <div id="divRegistroHacienda" class="GroupBody">
                        <div id="divRegistroHaciendaHeader" class="GroupHeader">
                            <span>Registro de Hacienda</span>
                        </div>
                        <div id="divInformacionHaciendaControles">
                            <table class="trAlinearDerecha">
                                <tr>
                                    <td class="tdCentradoVertical" style="width: 200px;">
                                        <span>*</span>Fecha de Registro
                                    </td>
                                    <td style="width: 20px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical" style="width: 230px;">
                                        <asp:TextBox ID="txtFechaRegistro" runat="server" CssClass="CampoFecha"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical" style="width: 200px; text-align: right;">
                                        <span>*</span>Giro de la Empresa
                                    </td>
                                    <td style="width: 20px;">
                                        &nbsp;
                                    </td>
                                    <td class="tdCentradoVertical">
                                        <asp:TextBox ID="txtGiroEmpresa" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
                <div id="divRepresentantesLegales" class="GroupBody" runat="server">
                    <div id="divRepresentantesLegalesHeader" class="GroupHeader">
                        <span>Representantes Legales</span>
                    </div>
                    <div id="divRepresentantesLegalesControles">
                        <asp:Panel runat="server" ID="pnlRepresentantesLegales">
                            <UC1:ucDatosRepresentanteLegalUI runat="server" ID="ucDatosRepresentanteLegal" />
                            <div class="dvOpciones">
                                <asp:Button ID="btnActualizarRepresentante" runat="server" Text="Actualizar" OnClick="btnActualizarRepresentante_Click"
                                    CssClass="btnWizardGuardar" Visible="false" />
                                <asp:Button ID="btnAgregarRepresentante" runat="server" CssClass="btnAgregarATabla"
                                    Text="Agregar a Tabla" OnClick="btnAgregarRepresentante_Click" Enabled="true" />
                                <asp:Button ID="btnCancelarRepresentante" runat="server" CssClass="btnWizardCancelar"
                                    Text="Cancelar" OnClick="btnCancelarRepresentante_Click" Visible="false" />
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="grdRepresentantesLegales" runat="server" AutoGenerateColumns="false"
                            CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                            AllowSorting="True" OnRowCommand="grdRepresentantesLegales_RowCommand" OnPageIndexChanging="grdRepresentantesLegales_PageIndexChanging"
                            Width="95%" OnRowDataBound="grdRepresentantesLegales_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                                <asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDireccion" runat="server" Text="<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="¿Depositario?" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.EsDepositario").ToString().ToUpper().Replace("TRUE","SI").Replace("FALSE","NO") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="110px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFolioRPPC" runat="server" Text="<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtEliminar" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                            CommandName="CMDELIMINAR" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                            CommandName="CMDEDITAR" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png" ToolTip="Editar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridHeader" />
                            <EditRowStyle CssClass="GridAlternatingRow" />
                            <PagerStyle CssClass="GridPager" />
                            <RowStyle CssClass="GridRow" />
                            <FooterStyle CssClass="GridFooter" />
                            <SelectedRowStyle CssClass="GridSelectedRow" />
                            <AlternatingRowStyle CssClass="GridAlternatingRow" />
                        </asp:GridView>
                    </div>
                </div>
                <div id="divObligadosSolidarios" class="GroupBody">
                    <div id="divObligadosSolidariosHeader" class="GroupHeader">
                        <span>Obligados Solidarios</span>
                    </div>
                    <div id="divObligadosSolidariosControles">
                        <asp:Panel ID="pnlObligadosSolidarios" runat="server">
                            <UC2:ucDatosObligadoSolidarioUI runat="server" ID="ucDatosObligadoSolidario" />
                            <div class="dvOpciones">
                                <asp:Button ID="btnActualizarObligado" runat="server" Text="Actualizar" OnClick="btnActualizarObligado_Click"
                                    CssClass="btnWizardGuardar" Visible="false" OnClientClick="window.mostrarConfirmacion='0';" />
                                <asp:Button ID="btnAgregarObligadoSolidario" runat="server" CssClass="btnAgregarATabla"
                                    Text="Agregar a Tabla" OnClick="btnAgregarObligadoSolidario_Click" Enabled="true"
                                    OnClientClick="window.mostrarConfirmacion='0';" />
                                <asp:Button ID="btnCancelarObligado" runat="server" Text="Cancelar" OnClick="btnCancelarObligado_Click"
                                    CssClass="btnWizardCancelar" Visible="false" OnClientClick="window.mostrarConfirmacion='0';" />
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="grdObligadosSolidarios" runat="server" AutoGenerateColumns="false"
                            CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
                            AllowSorting="True" OnRowCommand="grdObligadosSolidarios_RowCommand" OnPageIndexChanging="grdObligadosSolidarios_PageIndexChanging"
                            Width="95%" OnRowDataBound="grdObligadosSolidarios_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                                <asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDireccion" runat="server" Text="<%# ((BPMO.SDNI.Comun.BO.ObligadoSolidarioBO)Container.DataItem).DireccionPersona.Calle %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="300px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Teléfono" DataField="Telefono">
                                    <ItemStyle Width="140px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Tipo Obligado Solidario" ItemStyle-HorizontalAlign="Justify">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.TipoObligado").ToString().ToUpper().Replace("0", "FÍSICO").Replace("0", "MORAL")%>
                                    </ItemTemplate>
                                    <ItemStyle Width="110px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtEliminar" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                            CommandName="CMDELIMINAR" ImageUrl="~/Contenido/Imagenes/ELIMINAR-ICO.png" ToolTip="Eliminar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtEditar" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                            CommandName="CMDEDITAR" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png" ToolTip="Editar"
                                            OnClientClick="window.mostrarConfirmacion='0';" />
                                    </ItemTemplate>
                                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtDetalle" runat="server" CommandArgument="<%#Container.DataItemIndex%>"
                                            ToolTip="Ver Detalles" CommandName="CMDDETALLE" ImageUrl="~/Contenido/Imagenes/VER.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="17px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridHeader" />
                            <EditRowStyle CssClass="GridAlternatingRow" />
                            <PagerStyle CssClass="GridPager" />
                            <RowStyle CssClass="GridRow" />
                            <FooterStyle CssClass="GridFooter" />
                            <SelectedRowStyle CssClass="GridSelectedRow" />
                            <AlternatingRowStyle CssClass="GridAlternatingRow" />
                        </asp:GridView>
                    </div>
                </div>
                <div id="divObservaciones" runat="server" class="GroupBody">
                    <div id="div6" class="GroupHeader">
                        <span>Observaciones</span>
                    </div>
                    <div id="div7">
                        <table class="trAlinearDerecha">
                            <tr>
                                <td class="espacio">
                                    &nbsp;
                                </td>
                                <td class="tdCentradoVertical control">
                                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" 
                                     Width="890px" Height="122px" MaxLength="2000" ></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td class="espacio">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="viewRepresentantesObligados" runat="server">
                <div id="div1" class="GroupBody">
                    <div id="div2" class="GroupHeader">
                        <span>Representante Legal de Obligado Solidario</span>
                        <div class="GroupHeaderOpciones Ancho2Opciones">
                            <asp:Button ID="btnAgregarRepresentanteObligado" runat="server" CssClass="btnWizardGuardar"
                                Text="Agregar" OnClick="btnAgregarRepresentanteObligado_Click" />
                            <asp:Button ID="btnCancelarRepresentanteObligado" runat="server" CssClass="btnWizardCancelar"
                                Text="Cancelar" OnClick="btnCancelarRepresentanteObligado_Click" />
                        </div>
                    </div>
                    <div id="div3">
                        <UC1:ucDatosRepresentanteLegalUI runat="server" ID="ucDatosRepresentantesObligados" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    <div id="DialogRepresentantesObligados" style="display: none;" title="REPRESENTANTES LEGALES DEL OBLIGADO SOLIDARIO">
        <asp:GridView ID="grdRepresentantesObligados" runat="server" AutoGenerateColumns="false"
            CellPadding="4" GridLines="None" CssClass="Grid" PageSize="5" AllowPaging="True"
            AllowSorting="True" Width="95%" OnPageIndexChanging="grdRepresentantesObligados_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Nombre" DataField="Nombre"></asp:BoundField>
                <asp:TemplateField HeaderText="Dirección" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDireccion" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).DireccionPersona.Calle %>'></asp:Label></ItemTemplate>
                    <ItemStyle Width="300px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Folio RPPC" ItemStyle-HorizontalAlign="Justify">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFolioRPPC" Text='<%# ((BPMO.SDNI.Comun.BO.RepresentanteLegalBO)Container.DataItem).ActaConstitutiva.NumeroRPPC %>'></asp:Label></ItemTemplate>
                    <ItemStyle Width="150px" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridHeader" />
            <EditRowStyle CssClass="GridAlternatingRow" />
            <PagerStyle CssClass="GridPager" />
            <RowStyle CssClass="GridRow" />
            <FooterStyle CssClass="GridFooter" />
            <SelectedRowStyle CssClass="GridSelectedRow" />
            <AlternatingRowStyle CssClass="GridAlternatingRow" />
        </asp:GridView>
    </div>
</asp:Content>
