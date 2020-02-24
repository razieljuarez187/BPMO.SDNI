<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionMotoNiveladoraPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionMotoNiveladoraPSLUI" %>

<div id="dvOpciones" style="display: table; width: 100%">

        <div class="dvIzquierda">
            <%--1. Generales--%>
            <fieldset>
                <legend>1. EN GENERAL</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td class="tdCentradoVertical">
                            <asp:CheckBox ID="chbxPresionLlantas" Text="Presi&oacute;n de llantas" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBandaVentilador" Text="Banda del ventilador" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxManguerasAbrazaderasAdmisionAire" 
                                Text="Mangueras y Abrazaderas de Admisi&oacute;n de Aire" runat="server" 
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCartuchoFiltroAire" Text="Cartucho del Filtro de Aire" 
                                runat="server"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCuchilla" Text="Cuchilla" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAjusteGiracirculoCuchilla" 
                                Text="Ajuste del giracirculo de cuchilla" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxRipperEscarificador" 
                                Text="Ripper/Escarificador" runat="server"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCinturonSeguridad" Text="Cinturón de seguridad" runat="server"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxEspejosRetrovisores" Text="Espejos retrovisores" runat="server"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </fieldset>
            
            <%--3. Lubricación--%>
            <fieldset>
                    <legend>3. Lubricación</legend>
                    <table class="trAlinearIzquierda">
                        <tr>
                            <td><asp:CheckBox ID="chbxArticulacionesCuchilla" 
                                    Text="Articulaciones de cuchilla" runat="server" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chbxArticulacionesRipper" Text="Articulaciones de ripper" 
                                    runat="server" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chbxArticulacionesEscarificador" Text="Articulaciones de escarificador" 
                                    runat="server" /></td>
                        </tr>
                         <tr>
                            <td><asp:CheckBox ID="chbxArticulacionesDireccion" Text="Articulaciones de direcci&oacute;n" 
                                    runat="server" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chbxArticulacionesChasis" Text="Articulaci&oacute;n de chasis" 
                                    runat="server" /></td>
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
                        <td><asp:CheckBox ID="chbxPalancasFuncionesHidraulicos" 
                                Text="Palancas de funciones hidr&aacute;ulicos" runat="server" 
                                /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxLucesAdvertencia" Text="Luces de advertencia" 
                                runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxIndicadores" Text="Indicadores" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxTacometro" Text="Tac&oacute;metro" runat="server" 
                                /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxFrenoEstacionamiento" Text="Freno de estacionamiento" 
                                runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxVelocidadMinimaMotor" Text="Velocidad m&iacute;nima del motor" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxVelocidadMaximaMotor" Text="Velocidad m&aacute;xima del motor" runat="server" /></td>
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
                            <asp:CheckBox ID="chbxLiquidoRefrigerante" Text="L&iacute;quido refrigerante" runat="server"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteHidraulico" Text="Aceite hidr&aacute;ulico" runat="server" 
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteTransmision" Text="Aceite de transmisi&oacute;n"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteDiferencial" 
                                Text="Aceite diferencial " runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteMandosFinales" 
                                Text="Aceite mandos finales" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteTandem" 
                                Text="Aceite tandem" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteCajaEngranesGiracirculo" 
                                Text="Aceite caja engranes giracirculo" runat="server" 
                                />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:CheckBox ID="chbxBateria" 
                                Text="Bater&iacute;a" runat="server" />
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
                            runat="server" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesTrabajoTraseras" Text="Luces de trabajo traseras" 
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxClaxon" Text="Claxon" runat="server"
                            />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAlarmaReversa" Text="Alarma de reversa" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesIntermitentes" Text="Luces intermitentes"
                            runat="server" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="chbxLucesDireccionales" Text="Luces de direccionales"
                            runat="server" />
                    </td>
                </tr>
                 </table>
        </fieldset>
            
            <%--6. Miscelaneos--%>
            <fieldset>
                <legend>6. Miscelaneos</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td><asp:CheckBox ID="chbxTapaCombustible" Text="Tapa de combustible" runat="server"  /></td>
                    </tr>
                      <tr>
                        <td><asp:CheckBox ID="chbxTapaHidraulico" Text="Tapa del hidr&aacute;ulico" 
                                runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxCondicionAsiento" Text="Condici&oacute;n del asiento" 
                                runat="server" /></td>
                    </tr>

                      <tr>
                        <td><asp:CheckBox ID="chbxCondicionLlantas" Text="Condici&oacute;n de llantas" 
                                runat="server" /></td>
                    </tr>

                    <tr>
                        <td><asp:CheckBox ID="chbxCondicionPintura" Text="Condición de pintura" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxCondicionCalcas" Text="Condición de calcas" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxSimbolosSeguridadMaquina" Text="Símbolos de seguridad de la m&aacute;quina" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxEstructuraChasis" Text="Estructura/Chasis" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chbxAntenasMonitoreoSatelital" Text="Antenas monitoreo satelital" runat="server" /></td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    

