﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionRetroExcavadoraPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionRetroExcavadoraPSLUI" %>
<div id="dvOpciones" style="display: table; width: 100%">

        <div class="dvIzquierda">
            <%--1. Generales--%>
            <fieldset>
                <legend>1. EN GENERAL</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td class="tdCentradoVertical">
                            <asp:CheckBox ID="chbxPresionLlantas" Text="Presión de llantas" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBandaVentilador" Text="Banda del ventilador" runat="server"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxManguerasAbrazaderasAdmisionAire" 
                                Text="Mangueras y abrazaderas de admisión de aire" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCartuchoFiltroAire" Text="Cartucho del filtro de aire" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBoteDelantero" Text="Bote delantero" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBoteTrasero" Text="Bote trasero" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxZapatasEstabilizadores" 
                                Text="Zapatas de los estabilizadores" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCinturonSeguridad" Text="Cinturón de seguridad" 
                                runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxEspejoRetrovisor" Text="Espejo retrovisor" runat="server" />
                        </td>
                    </tr>
                    </table>
            </fieldset>
            
            <%--3. Lubricación--%>
            <fieldset>
                    <legend>3. Lubricación</legend>
                    <table class="trAlinearIzquierda">
                        <tr>
                            <td><asp:CheckBox ID="chbxBujesPasadoresCargador" 
                                    Text="Bujes y pasadores del cargador" runat="server" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chbxBujesPasadoresRetro" Text="Bujes y pasadores de la retro" 
                                    runat="server"/></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chbxAsientoOperador" Text="Asiento del operador" 
                                    runat="server"/></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </fieldset>

            <%--6. Controles--%>
            <fieldset>
                <legend>5. Controles</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td><asp:CheckBox ID="chbxPalancaTransito" Text="Palanca de tránsito" 
                                runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxJoystick" Text="Joystick" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxLucesAdvertencia" Text="Luces de advertencia" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxIndicadores" Text="Indicadores" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxTacometro" Text="Tacómetro" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxFrenoEstacionamiento" Text="Freno de estacionamiento" 
                                runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxVelocidadMinimaMotor" Text="Velocidad mínima del motor" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxVelocidadMaximaMotor" Text="Velocidad máxima del motor" runat="server"/></td>
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
                            <asp:CheckBox ID="chbxCombustible" Text="Combustible" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteMotor" Text="Aceite del motor" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxLiquidoRefrigerante" Text="Líquido refrigerante" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteHidraulico" Text="Aceite hidráulico" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteTransmision" Text="Aceite de transmisión" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteDiferencialTrasero" Text="Aceite diferencial trasero" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteDiferencialDel" 
                                Text="Aceite diferencial Del. (4x4)" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceitePlanetariosDel4x4" 
                                Text="Aceite planetarios Del. (4x4)" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBateria" 
                                Text="Batería" runat="server"/>
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
                        <asp:CheckBox ID="chbxLucesTrabajoDelanteras" Text="Luces de trabajo delantera" 
                            runat="server"/>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesTrabajoTraseras" Text="Luces de trabajo traseras" 
                            runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxClaxon" Text="Claxon" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAlarmaReversa" Text="Alarma de reversa" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesIntermitentes" Text="Luces intermitentes"
                            runat="server"/>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesDireccionales" Text="Luces de direccionales"
                            runat="server"/>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="chbxNeutralizadorTransmision" Text="Neutralizador de transmisión"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxDesacople4x4" Text="Desacople 4x4" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxBloqueoDiferencial" Text="Bloqueo del diferencial" 
                            runat="server"/>
                    </td>
                </tr>
            </table>
        </fieldset>
            
            <%--6. Miscelaneos--%>
            <fieldset>
                <legend>6. Misceláneos</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td><asp:CheckBox ID="chbxTapaCombustible" Text="Tapa de combustible" runat="server"/></td>
                    </tr>
                      <tr>
                        <td><asp:CheckBox ID="chbxTapaHidraulico" Text="Tapa del hidráulico" 
                                runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxCondicionAsiento" Text="Condición del asiento" 
                                runat="server"/></td>
                    </tr>

                      <tr>
                        <td><asp:CheckBox ID="chbxCondicionLlantas" Text="Condición de llantas" 
                                runat="server" /></td>
                    </tr>

                    <tr>
                        <td><asp:CheckBox ID="chbxCondicionPintura" Text="Condición de pintura" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxCondicionCalcas" Text="Condición de calcas" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxSimbolosSeguridadMaquina" Text="Símbolos de seguridad de la máquina" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxEstructuraChasis" Text="Estructura/Chasis" 
                                runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxAntenasMonitoreoSatelital" Text="Antenas monitoreo satelital" runat="server" /></td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    
</div>
