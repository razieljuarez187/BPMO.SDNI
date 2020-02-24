<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionEntregaRecepcionPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionEntregaRecepcionPSLUI" %>
<div id="Div9" style="display: table; width: 100%">
              
    <fieldset>
        <legend>EXISTENCIAS</legend>
        <div id="dvOpciones" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            BANDAS
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneBandas" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            FILTRO ACEITE
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneFiltroAceite" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            FILTRO AGUA</td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneFiltroAgua" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            FILTRO COMBUSTIBLE
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneFiltroCombustible" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            FILTRO DE AIRE
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneFiltroAire" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            MANGUERAS</td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneMangueras" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>                       
        </div>
    </fieldset>
    <fieldset>
        <legend>MEDIDORES</legend>
        <div id="Div1" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            AMPERÍMETRO
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneAmperimetro" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            VOLTÍMETRO
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneVoltimetro" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            HORÓMETRO</td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneHorometro" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            MANÓMETRO
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneManometro" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            INTERRUPTOR
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneInterruptor" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>                       
        </div>
    </fieldset>
    <fieldset>
        <legend>MOTOR (NIVELES)</legend>
        <div id="Div2" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            ACEITE
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneNivelAceite" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            ANTICONGELANTE
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneNivelAnticongelante" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>                               
                </table>
            </div>                       
        </div>
    </fieldset>                
    <fieldset>
        <legend>VOLTAJE (OPERACIÓN)</legend>
        <div id="Div4" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>L1-N
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtVoltajeL1" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>L2-N
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtVoltajeL2" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>L3-N
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtVoltajeL3" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>L1-L2
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtVoltajeL1L2" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>     
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>L2-L3
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtVoltajeL2L3" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>     
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>L3-L1
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtVoltajeL3L1" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>                                   
                </table>
            </div>                       
        </div>
    </fieldset>
    <fieldset>
        <legend>ACCESORIOS</legend>
        <div id="Div3" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            CABLES (MTS)
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneCables" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            TRAMOS
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneTramos" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdCentradoVertical">
                            LÍNEAS
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneLineas" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            CALIBRES
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneCalibres" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            ZAPATAS
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneZapatas" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>                                    
                </table>
            </div>                       
        </div>
    </fieldset>
    <fieldset>
        <legend>BATERÍA</legend>
        <div id="Div5" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>CANTIDAD
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                                <asp:TextBox runat="server" ID="txtBateriaCantidad" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>PLACAS
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtBateriaPlacas" Width="150px" CssClass="CampoMonedaDosDecimales" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>MARCA
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtBateriaMarca" Width="150px"  MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>                                  
                </table>
            </div>                       
        </div>
    </fieldset> 
    <fieldset>
        <legend>DATOS REMOLQUE</legend>
        <div id="Div6" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>SUSPENSIÓN
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtSuspension" Width="240px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>GANCHO
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtGancho" Width="240px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>GATO DE NIVELACIÓN
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:TextBox runat="server" ID="txtGatoNivelacion" Width="240px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            <span>*</span>ARNÉS DE CONEXIÓN
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                                <asp:TextBox runat="server" ID="txtArnesConexion" Width="240px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>                                  
                </table>
            </div>                       
        </div>
    </fieldset> 
    <fieldset>
        <legend>LLANTAS</legend>
        <div id="Div7" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                                &nbsp;
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            D
                        </td>
                        <td class="tdCentradoVertical">
                            I
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            EJE 1
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneEje1LlantaD" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneEje1LlantaI" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            EJE 2
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneEje2LlantaD" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneEje2LlantaI" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            EJE 3
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneEje3LlantaD" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneEje3LlantaI" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            TAPAS LLUVIA
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneTapaLluviaLlantaD" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneTapaLluviaLlantaI" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>                                                                
                </table>
            </div>                       
        </div>
    </fieldset>
    <fieldset>
        <legend>LÁMPARAS</legend>
        <div id="Div8" style="display: table; width: 100% ">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            IZQUIERDA
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneLamparaIzquierda" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            DERECHA
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneLamparaDerecha" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            SEÑAL SATELITAL
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneSenalSatelital" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            DIODOS
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTieneDiodos" RepeatDirection="Horizontal">
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-SI-ICO.png" alt="SI" />' Value="True" />
                                <asp:ListItem Text='<img src="../Contenido/Imagenes/ESTATUS-NO-ICO.png" alt="NO" />' Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>                                   
                </table>
            </div>                       
        </div>
    </fieldset>  
</div>