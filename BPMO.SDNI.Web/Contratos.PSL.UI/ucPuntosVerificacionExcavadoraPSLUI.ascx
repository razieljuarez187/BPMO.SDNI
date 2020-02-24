<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionExcavadoraPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionExcavadoraPSLUI" %>

<div id="dvOpciones" style="display: table; width: 100%">
    <div class="dvIzquierda">
        <%--1. Generales--%>
        <fieldset>
            <legend>1. EN GENERAL</legend>
            <table class="trAlinearIzquierda">
                <tr>
                    <td class="tdCentradoVertical">
                        <asp:CheckBox ID="chbxZapatas" Text="Zapatas" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxBrazoPluma" Text="Brazo y pluma" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxContrapeso" Text="Contrapeso" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxVastagosGatos" Text="Vástagos de gatos" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxTensionCadena" Text="Tensión de cadena" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxRodillosTransito" Text="Rodillos de tránsito" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxEspejosRetrovisores" Text="Espejos retrovisores" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCristalesCabina" Text="Cristales de cabina" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxPuertasCerraduras" Text="Puertas y cerraduras" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxBisagrasCofreMotor" Text="Bisagras cofre del motor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxBalancinBote" Text="Balancín del bote (H)" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <%--3. Lubricación--%>
        <fieldset>
            <legend>3. Lubricación</legend>
            <table class="trAlinearIzquierda">
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxPasadoresBoom" Text="Pasadores boom" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxPasadoresBrazo" Text="Pasadores brazo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxPasadoresBote" Text="Pasadores bote" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxTornamesa" Text="Tornamesa" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCentralEngrase" Text="Central de engrase" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <%--6. Controles--%>
        <fieldset>
            <legend>5. Controles</legend>
            <table class="trAlinearIzquierda">
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxJoystick" Text="Joysticks" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesAdvertencia" Text="Luces de advertencia" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxIndicadores" Text="Indicadores" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxPalancaBloqueoPilotaje" Text="Palanca bloqueo pilotaje" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAireAcondicionado" Text="Aire acondicionado" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAutoaceleracion" Text="Autoaceleración" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxVelocidadMinimaMotor" Text="Velocidad mínima del motor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxVelocidadMaximaMotor" Text="Velocidad máxima del motor" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div class="dvDerecha">
        <%--2. Niveles de fluidos--%>
        <fieldset>
            <legend>2. Niveles de fluidos </legend>
            <table class="trAlinearIzquierda">
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCombustible" Text="Combustible" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAceiteMotor" Text="Aceite del motor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAceiteHidraulico" Text="Aceite hidráulico" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLiquidoRefrigerante" Text="Líquido refrigerante" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxReductorEngranesTransito" Text="Reductor de engranes de tránsito"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxReductorSwing" Text="Reductor del swing" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxBateria" Text="Batería" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <%--4. Funciones eléctricas--%>
        <fieldset>
            <legend>4. Funciones eléctricas </legend>
            <table class="trAlinearIzquierda">
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesTrabajo" Text="Luces de trabajo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLamparasTablero" Text="Lámparas del tablero" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxInterruptorDesconexion" Text="Interruptor de desconexión" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAlarmaReversa" Text="Alarma de reversa" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxHorometro" Text="Horómetro" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLimpiaparabrisas" Text="Limpiaparabrisas" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <%--6. Miscelaneos--%>
        <fieldset>
            <legend>6. Misceláneos</legend>
            <table class="trAlinearIzquierda">
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxTapaCombustible" Text="Tapa de combustible" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCondicionAsiento" Text="Condición del asiento" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCondicionPintura" Text="Condición de pintura" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCondicionCalcas" Text="Condición de calcas" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxSimbolosSeguridadMaquina" Text="Símbolos de seguridad de la máquina"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxEstructuraChasis" Text="Estructura/Chasis" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAntenasMonitoreoSatelital" Text="Antenas monitoreo satelital"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</div>
