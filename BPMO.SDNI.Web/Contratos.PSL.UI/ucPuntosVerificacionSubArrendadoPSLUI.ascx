<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionSubArrendadoPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionSubArrendadoPSLUI" %>
<div id="Div9" style="display: table; width: 100%">
    
    <fieldset>
        <div id="Div1" style="display: table; width: 100%">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            Niveles de fluidos
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtNivelesFluido" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Tapa de fluidos
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTapaFluidos" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Sistema eléctrico
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtSistemaElectrico" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Faros traseros
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtFarosTraseros" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Faros delanteros
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtFarosDelanteros" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Cuartos y direccionales
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtCuartosDireccionales" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Limpiaparabrisas
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtLimpiaparabrisas" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Batería
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtBateria" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Chasis
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtChasis" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Estabilizadores
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtEstabilizadores" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Zapata
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtZapata" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Bote trasero
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtBoteTrasero" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Bote delantero
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtBoteDelantero" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Brazo y pluma
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtBrazoPluma" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Contrapeso
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtContrapeso" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Gatos HD (Vástagos)
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtVastagos" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Tensión de cadena
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTensionCadena" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </fieldset>
    <fieldset>
    <legend>Equipos portátiles</legend>
        <div id="dvOpciones" style="display: table; width: 100%">
            <div class="dvCentro">
                <table class="trAlinearDerecha">
                    <tr>
                        <td class="tdCentradoVertical">
                            Nivel de Fluidos
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtNivelesFluidos" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Sistema de remolque
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtSistemaRemolque" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Ensamble de rueda
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtEnsambleRueda" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Estructura o chasis
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtEstructuraChasis" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Pintura
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtPintura" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Llantas
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtLlantas" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Sistema vibratorio
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtSistemaVibratorio" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Zapata o rodillo
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtZapataRodillo" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Asiento del operador
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtAsientoOperador" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Espejo retrovisor
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtEspejoRetrovisor" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Palancas de control
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtPalancasControl" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Tablero de instrumentos
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTableroInstrumentos" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Molduras y tolvas
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtMoldurasTolvas" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Aire Acondicionado
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtAireAcondicionado" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Cristales Laterales
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtCristalesLaterales" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Panorámico
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtPanoramico" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Puertas y Cerraduras
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtPuertasCerraduras" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Cofre de motor
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtCofreMotor" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Parrilla de radiador
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtParrillaRadiador" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Alarma de movimiento
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtAlarmaMovimiento" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Estéreo
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtEstereo" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Ventilador eléctrico
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtVentiladorElectrico" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Indicadores e Interruptores
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtIndicadoresInterruptores" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Pintura
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtPinturaEP" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Kit HD para martillo
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtKitMartillo" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Central de engrane
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtCentralEngrane" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Amperímetro
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtAmperimetro" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Voltímetro
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtVoltimetro" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Horómetro
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtHorometro" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Frecuentómetro
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtFrecuentometro" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Interruptor termomagnético
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtInterruptorTermomagnetico" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Manómetro de presión
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtManometroPresion" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Tipo de voltaje
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtTipoVoltaje" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Lámparas
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtLamparas" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdCentradoVertical">
                            Funcionamiento
                        </td>
                        <td style="width: 5px;">
                            &nbsp;
                        </td>
                        <td class="tdCentradoVertical">
                            <asp:RadioButtonList runat="server" ID="rbtFuncionamiento" RepeatDirection="Horizontal">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </fieldset>

    <div id="divObservaciones">
        <fieldset id="fsObservaciones">
            <legend>Comentarios generales</legend>
            <table class="trAlinearDerecha">
                <tr>
                    <td class="tdCentradoVertical">
                        <asp:TextBox runat="server" ID="txtComentariosGenerales" TextMode="MultiLine" Width="250px"
                                                Height="90px" 
                                                Style="max-width: 250px; min-width: 250px; max-height: 90px; min-height: 90px;"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</div>
