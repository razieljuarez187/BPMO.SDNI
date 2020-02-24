<%@ Control Language="C#" CodeBehind="ucConfiguracionDescuentoPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucConfiguracionDescuentoPSLUI" %>
<link href="../Contenido/Estilos/Tema.JqueryUI/jquery.ui.timepicker.css" rel="Stylesheet" type="text/css"/>
<script src="../Contenido/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
<script src="../Contenido/Scripts/jquery.ui.timepicker-es.js" type="text/javascript"></script>

<style type="text/css">
    .Grid
    {
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
    }
    .guardarGrid
    {
        width: auto;
    }
    
    .btnWizardGuardar
    {
    }
    .no-close .ui-dialog-titlebar-close
    {
        display: none;
    }
    
    input[type='checkbox'] + label
    {
        vertical-align: middle;
        margin-left: 5px;
    }
    
</style>

    <script type="text/javascript">
        $(document).ready(function () { initChild(); });

        function initChild() {
            initPage(); inicializeHorizontalPanels();
        }

       

        function initPage() {
           

            var dateFormat = "dd/mm/yy",
            from = $(".CampoFechaInicio").datepicker({
                yearRange: '-100:+10',
                changeYear: true,
                changeMonth: true,
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha de inicio de descuento",
                showOn: 'button',
                minDate: 0
            })

            to = $(".CampoFechaFinal").datepicker({
                yearRange: '-100:+10',
                changeYear: true,
                changeMonth: true,
                buttonImage: '../Contenido/Imagenes/calendar.gif',
                buttonImageOnly: true,
                toolTipText: "Fecha final de descuento",
                showOn: 'button',
                minDate: 0

            }).on("change", function () {
                from.datepicker("option", "maxDate", getDate(this));
            })

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                }
                catch (error) {
                    date = null;
                }

                return date;
            }

            $('.CampoFechaInicio').attr('readonly', true);
            $('.CampoFechaFinal').attr('readonly', true);
        

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
    </script>    

    <%--Modal buscador--%>
    <script type="text/javascript">
        function BtnBuscar(guid, xml) {
            var width = ObtenerAnchoBuscador(xml);

            $.BuscadorWeb({
                xml: xml,
                guid: guid,
                btnSender: $("#<%=btnResult.ClientID %>"),
                features: {
                    dialogWidth: width,
                    dialogHeight: '310px',
                    center: 'yes',
                    maximize: '0',
                    minimize: 'no'
                }
            });
        }
    </script>

    <%--modal Aviso--%>
    <script language="javascript" type="text/javascript">
        function abrirConfirmacion(msg) {
            var $div = $('<div title="Confirmación"></div>');
            $div.append(msg);
            $("#dialog:ui-dialog").dialog("destroy");
            $($div).dialog({
                dialogClass: "no-close",
                closeOnEscape: false,
                modal: true,
                minWidth: 460,
                close: function () {
                    $(this).dialog("close");
                },
                buttons:
                [
                  {
                    text: "CANCELAR",
                    click: function () {
                        __doPostBack("<%= CancelarCheckbox.UniqueID %>", "");
                        $(this).dialog("close");
                        __blockUI();
                        }
                  },
                  {
                    text: "ACEPTAR",
                    click: function () {
                        __doPostBack("<%= GuardarEnGrid.UniqueID %>", "");
                        $(this).dialog("close");
                        __blockUI();
                      }
                  }
                ]
            });
        }
    </script>
    
<div style="width:100%; overflow:hidden;">
<fieldset style="padding:5px;" class="Grid">
    <legend>Catálogos</legend>
    <div class="dvIzquierda">
        <table class="trAlinearIzquierda">
            <tr>
                <td class="tdCentradoVertical" >
                    <span>*</span>Cliente
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:TextBox ID="txtCliente" runat="server" Width="202px" AutoPostBack="True" OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                    <asp:ImageButton ID="ibtnBuscaCliente" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                        OnClick="ibtnBuscaCliente_Click" />
                   
                </td>
            </tr>
        </table>
        <table class="trAlinearIzquierda">
           <tr>
                <td class="tdCentradoVertical" colspan="1" style="width:233px;">
                    <asp:CheckBox ID="chbxTodasSucursales" Text="Todas las sucursales"
                        runat="server" AutoPostBack="True" oncheckedchanged="chbxTodasSucursales_CheckedChanged1" />
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearIzquierda">
            <tr>
                <td class="tdCentradoVertical" >
                    <span>*</span>Contacto Comercial
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:TextBox ID="txtContactoComercial" runat="server" Width="200px" MaxLength="250"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<fieldset style="padding:5px;" class="Grid">
    <legend>Información del Descuento </legend>
    <div class="dvIzquierda">
        <table class="trAlinearIzquierda">
            <tr>
                <td class="tdCentradoVertical" >
                    <span>*</span>Sucursal
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
                <td class="txtFechaVigencia" style="width: 330px;">
                    <asp:TextBox ID="txtSucursal" runat="server" Width="200px" AutoPostBack="True" OnTextChanged="txtSucursal_TextChanged"></asp:TextBox>
                    <asp:ImageButton ID="ibtnBuscaSucursal" runat="server" ImageUrl="~/Contenido/Imagenes/Detalle.png"
                        OnClick="ibtnBuscaSucursal_Click" />
                    <asp:HiddenField ID="hdnSucursalID" runat="server" />
                </td>
            </tr>
            <tr id="trtFechaInicio" runat="server">
                <td class="tdCentradoVertical">
                    <span>*</span>Fecha Inicio
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:TextBox ID="txtFechaInicio" runat="server" MaxLength="30" Width="95px" ValidationGroup="FormatoValido"
                        CssClass="CampoFechaInicio"></asp:TextBox>
                </td>
            </tr>

            <tr id="trFechaFinal" runat="server">
                <td class="tdCentradoVertical" >
                    <span>*</span>Fecha Fin
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
                
                <td class="tdCentradoVertical" style="width: 330px;">
                    <asp:TextBox ID="txtFechaFin" runat="server" MaxLength="30" Width="95px" ValidationGroup="FormatoValido"
                        CssClass="CampoFechaFinal"></asp:TextBox>
                </td>
            </tr>
           
        </table>
    </div>
    <div class="dvDerecha">
        <table class="trAlinearIzquierda">
            <tr>
                <td class="tdCentradoVertical" >
                    <span>*</span>Máximo descuento
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
                <td class="tdCentradoVertical" style="width: 330px;">
                  <asp:TextBox ID="txtMaximoDescuento" runat="server" Width="200px" MaxLength="6" 
                        onkeypress="return descuentoMaximo(event, this);"  ></asp:TextBox> 
              
                 
                </td>
            </tr>
            <tr>
                <td class="tdCentradoVertical" >
                    <asp:CheckBox ID="chbxActivo" Text="Activo" runat="server" />
                </td>
                <td style="width: 10px;">
                    &nbsp;
                </td>
            </tr>      
          
        </table>
    </div>
    <div id="dvBotones" runat="server" style="margin-top:120px; margin-bottom:10px;">
        <table class="trAlinearIzquierda" style="margin: 0 auto; text-align: center;">
            
            <tr style="width: 100%">
                <td class="tdCentradoVertical">
                    <asp:Button Text="Agregar" runat="server" ID="btnAgregar" CssClass="btnWizardGuardar"
                        OnClick="btnAgregar_Click" />
                </td>
                <td class="tdCentradoVertical">
                    <asp:Button Text="Cancelar" runat="server" ID="btnCancelar" CssClass="btnWizardCancelar"
                        OnClick="btnCancelar_Click" />
                </td>
                <td class="tdCentradoVertical">
                    <asp:Button Text="Actualizar" runat="server" ID="btnActualizar" CssClass="btnWizardEditar"
                        Visible="False" OnClick="btnActualizar_Click" />
                </td>
            </tr>
         
        </table>
    </div>
</fieldset>
 <fieldset style="padding-top:10px; padding-bottom:10px;" class="Grid">
 <legend>DESCUENTOS POR SUCURSAL </legend>
     <div style="margin-left:auto; margin-right:auto; width:850px;">
      <asp:UpdatePanel ID="UPContenedor" runat="server" >
            <ContentTemplate>
       <asp:GridView ID="grvDescuentos" runat="server" autoGenerateColumns="False" 
           AllowPaging="True" PageSize="10"
                        AllowSorting="false"  EnableSortingAndPagingCallbacks="True" 
            OnRowCommand="grvDescuentos_RowCommand" 
           OnRowDataBound="grvDescuentos_RowDataBound" width="850px"
           onpageindexchanging="grvDescuentos_PageIndexChanging" 
           onselectedindexchanged="grvDescuentos_SelectedIndexChanged">

       <Columns>
                <asp:BoundField DataField="Sucursal" HeaderText="Sucursal" SortExpression="Nombre">
                    <HeaderStyle HorizontalAlign="Left"  />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:BoundField>

                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha inicial" SortExpression="FechaInicio">
                    <HeaderStyle HorizontalAlign="Left"  />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:BoundField>

                <asp:BoundField DataField="FechaFin" HeaderText="Fecha final" SortExpression="FechaFin">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:BoundField>

            
                <asp:BoundField DataField="DescuentoMaximo" HeaderText="Máximo Descuento" SortExpression="DescuentoMaximo">
                    <HeaderStyle HorizontalAlign="Left"  />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:BoundField>

                <asp:TemplateField>
                        <HeaderTemplate>
                            Activo
                            </HeaderTemplate>
                    <ItemTemplate>
                            <asp:Checkbox runat="server" ID="ChkActivo"  disabled Width="100%"></asp:Checkbox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:TemplateField>

            <asp:TemplateField >
                <ItemTemplate>
                    <asp:ImageButton ID="imgDelete"  runat="server" ImageUrl="~/Contenido/Imagenes/EDITAR-ICO.png"
                        ToolTip="Editar" CommandName="Editar" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
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
                    <EmptyDataTemplate>
			            <b>No se han asignado datos.</b>
		            </EmptyDataTemplate>
    </asp:GridView>
       </ContentTemplate>
        </asp:UpdatePanel>
     </div>
    
     
   
   
    </fieldset>
<asp:Button Text="GuardarEnGrid" runat="server" ID="GuardarEnGrid" 
                               onclick="btnAceptarModal_Click" Visible="False" 
                  />
                  <asp:Button Text="CancelarCheckbox" runat="server" ID="CancelarCheckbox" 
                               onclick="btnCancelarModal_Click" Visible="False" 
                  />
 <asp:HiddenField ID="hdnClienteID" runat="server" />
<asp:Button ID="btnResult" runat="server" Text="Button" OnClick="btnResult_Click"
    Style="display: none;" />
<asp:HiddenField ID="hdnUsuarioAutenticado" runat="server" />
<asp:HiddenField ID="hdnTipoMensaje" runat="server" />
<asp:HiddenField ID="hdnMensaje" runat="server" />
<asp:HiddenField ID="hdnLlenarGrid" runat="server" />
<asp:HiddenField ID="hdnAccion" runat="server" />
<asp:HiddenField ID="hdnIndex" runat="server" />



</div>

    
    
    
