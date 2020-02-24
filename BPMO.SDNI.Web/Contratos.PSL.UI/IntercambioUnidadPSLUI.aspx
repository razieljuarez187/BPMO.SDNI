<%@ Page Title="" Language="C#" MasterPageFile="~/Contenido/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="IntercambioUnidadPSLUI.aspx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.IntercambioUnidadPSLUI" %>
<%@ Register TagPrefix="uc" TagName="HerramientasPSLUI" Src="~/Contratos.PSL.UI/ucHerramientasPSLUI.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucCatalogoDocumentosUI" Src="~/Comun.UI/ucCatalogoDocumentosUI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        #BarraHerramientas
        {
            width: 832px !important;
        }
    </style>
<link href="../Contenido/Estilos/EstiloContratoFSL.css" rel="stylesheet" type="text/css" />
<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>
<script type="text/javascript">
 // Inicializa la page
    $(document).ready(function () { initChild(); });

        function initChild() {
            initPage();
           

//            <%= ucHerramientas.ClientID %>_Inicializar();
        }

        function initPage() {
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
</script>
<script type="text/javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="PaginaContenido">
    <!-- Barra de localización -->
    <div id="BarraUbicacion">
        <asp:Label ID="lblEncabezadoLeyenda" runat="server">OPERACI&Oacute;N - INTERCAMBIO DE UNIDAD DE CONTRATO</asp:Label>
    </div>
    <!--Navegación secundaria-->
    <div style="height: 80px;">
        <!-- Menú secundario -->
        <ul id="MenuSecundario" style="float: left; height: 64px;">
            <li class="MenuSecundarioSeleccionado">
                <asp:HyperLink ID="hlkConsulta" runat="server" NavigateUrl="~/Contratos.PSL.UI/ConsultarContratoROUI.aspx">
                    CONSULTAR R.0.
                    <img id="imgConsultaCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección" /> 
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hlkRegistro" runat="server" NavigateUrl="~/Contratos.PSL.UI/RegistrarContratoROUI.aspx">
                    REGISTRAR R.0. 
                    <img id="imgRegistroCatalogo" src="<%= Page.ResolveUrl("~/Contenido/Imagenes/SelectorBlanco.png") %>" alt="selección"/>
                </asp:HyperLink>
            </li>
        </ul>
        <!-- Barra de herramientas -->
        <uc:HerramientasPSLUI ID="ucHerramientas" runat="server" Visible="True"/>
    </div>
    <div id="divIntercambioCleinteGroup" class="GroupBody">
        <div id="divIntercambioCliente" class="GroupHeader">
            <span>¿Qué unidad desea intercambiar?</span>
            <div class="GroupHeaderOpciones Ancho2Opciones">                
                 <asp:Button ID="btnGuardar" runat="server" Text="Terminar" CssClass="btnWizardTerminar"  OnClick="btnGuardar_Click"/> 
                 <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btnWizardCancelar"  OnClick="btnCancelar_Click"/> 
            </div>
        </div>
        <div class="dvIzquierda">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical">SERIE UNIDAD</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 300px;">
                        <asp:DropDownList ID="ddlUnidadesSerie" Width="200px" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="ddlUnidadesSerie_SelectedIndexChanged" > 
                            <asp:ListItem Value="x" Text="Seleccionar"></asp:ListItem>
                        </asp:DropDownList>                       
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">ECODE</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtECODECliente" runat="server" Width="250px" Enabled="False"></asp:TextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">MODELO</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 300px;">
                        <asp:TextBox ID="txtModeloCliente" runat="server" Width="250px" MaxLength="80" Enabled="False"></asp:TextBox>                      
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">HORÓMETRO</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtHorometroCliente" runat="server" Width="250px" Enabled="False"></asp:TextBox>                      
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">LITROS DE COMBUSTIBLE</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtProcentajeUnidadCliente" runat="server" Width="250px" Enabled="False"></asp:TextBox>                      
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click" Style="display: none;" />
    <div id="div1" class="GroupBody">
        <div id="div2" class="GroupHeader">
            <span>¿Por qué unidad será intercambiada?</span>
        </div>
        <div class="dvIzquierda">
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical">SERIE UNIDAD</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 300px;">
                        <asp:TextBox ID="txtNumeroSerie" runat="server" Width="250px" AutoPostBack="true" OnTextChanged="txtNumeroSerie_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscarUnidad" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                    OnClick="ibtnBuscarUnidad_Click" Style="width: 17px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">MODELO</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical" style="width: 300px;">
                       <asp:TextBox ID="txtModelo" runat="server" Width="250px" MaxLength="80" AutoPostBack="True" ToolTip="Modelo de la unidad" 
                                OnTextChanged="txtModelo_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="ibtnBuscaModelo" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png" 
                                ToolTip="Consultar modelo de la unidad" OnClick="ibtnBuscaModelo_Click" />                           
                    </td>
                </tr>
                <tr>
                    <td class="tdCentradoVertical">ECODE</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtEcode" runat="server" Width="250px" Enabled="False"></asp:TextBox>
                       <%-- <asp:HiddenField runat="server" ID="hdnEmpresaID"/>--%>
                    </td>
                </tr> 
                <tr>
                    <td class="tdCentradoVertical">HORÓMETRO</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtHorometroIntercambio" runat="server" Width="250px" Enabled="False"></asp:TextBox>                           
                    </td>

                </tr>      
                 <tr>
                    <td class="tdCentradoVertical">LITROS DE COMBUSTIBLE</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtPorcentajeIntercambio" runat="server" runat="server" Width="250px" Enabled="False" ></asp:TextBox>
                    </td>
                </tr>         
                <tr>
                    <td class="tdCentradoVertical"><span>*</span>FECHA INTERCAMBIO</td>
                    <td style="width: 5px;">&nbsp;</td>
                    <td class="tdCentradoVertical">
                        <asp:TextBox ID="txtFechaIntercambio" runat="server" MaxLength="11" CssClass="CampoFecha" AutoPostBack="True"
                                    ></asp:TextBox>   <%--ontextchanged="txtFechaContrato_TextChanged"--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>

     <div id="div3" class="GroupBody">
        <div id="div4" class="GroupHeader">
            <span>Documentos adjuntos al intercambio</span>
        </div>
        <div class="dvIzquierda">
             <fieldset id="fsDocumentosAdjuntos">
                            <legend></legend>
                            <uc:ucCatalogoDocumentosUI ID="ucCatalogoDocumentos" runat="server" />
                        </fieldset>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnContratoID" />
	<asp:HiddenField runat="server" ID="hdnEstatusID" />
	<asp:HiddenField runat="server" ID="hdnUnidadID" />
	<asp:HiddenField runat="server" ID="hdnEquipoID" />
	<asp:HiddenField runat="server" ID="hdnFUA" />
	<asp:HiddenField runat="server" ID="hdnUUA" /> 
    <asp:HiddenField runat="server" ID="hdnSucursalID"/>
	<asp:HiddenField runat="server" ID="hdnFechaIntercambio"/>
    <asp:HiddenField runat="server" ID="hdnModeloID"  />
    <asp:HiddenField runat="server" ID="hdnIntercambioUnidadID" />
	<asp:HiddenField runat="server" ID="hdnntercambioEquipoID" />
    <asp:HiddenField runat="server" ID="hdnTipoContrato" />
	
</asp:Content>
