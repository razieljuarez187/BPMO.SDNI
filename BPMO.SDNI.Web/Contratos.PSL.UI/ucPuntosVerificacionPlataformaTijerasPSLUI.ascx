<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionPlataformaTijerasPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionPlataformaTijerasPSLUI" %>

<div id="dvOpciones" style="display: table; width: 100%">

        <div class="dvIzquierda">
            <%--1. Generales--%>
            <fieldset>
                <legend>1. CONDICIONES GENERALES</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td class="tdCentradoVertical">
                            <asp:CheckBox ID="chbxConjuntoBarandillas" Text="Conjunto de barandillas" runat="server" 
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxPlataforma" Text="Plataforma" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBrazosTijera" 
                                Text="Brazos de tijera" runat="server" 
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxPasadoresPivoteTijera" Text="Pasadores de pivote de tijera" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCilindroElevador" Text="Cilindro elevador" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxChasis" Text="Chasis" runat="server"
                                />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxConjuntoNeumaticosRuedas" 
                                Text="Conjunto de neumáticos/ruedas" runat="server"
                                />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAlmohadillasDesgasteDeslizantes" 
                                Text="Almohadillas de desgaste deslizantes" runat="server"
                                />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCilindroDireccion" Text="Cilindro de dirección" runat="server"
                                />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:CheckBox ID="chbxBarrasDireccion" Text="Barras de dirección" runat="server"
                                />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxFuncionamientoControlesTierra" 
                                Text="Funcionamiento de controles de tierra" runat="server"
                                />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxFuncionamientoControlesPlataforma" 
                                Text="Funcionamiento de controles de plataforma" runat="server"
                                />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxParosEmergencia" Text="Faros de emergencia" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxVelocidadTransitoPlataformaRetraida" 
                                Text="Velocidad de tránsito con plataforma retraída" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxVelocidadTransitoPlataformaExtendida" 
                                Text="Velocidad de tránsito con plataforma extendida" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxPruebaSwitchesLimitePothole" 
                                Text="Prueba de los switches de límite del pothole" runat="server" />
                        </td>
                    </tr>
             
                  
                    
                </table>
            </fieldset>
            
            <%--3. Lubricación--%>
            <fieldset>
                    <legend>3. Lubricación</legend>
                    <table class="trAlinearIzquierda">
                        <tr>
                            <td><asp:CheckBox ID="chbxPivotesDireccion" 
                                    Text="Pivotes de la dirección" runat="server" /></td>
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
                            <asp:CheckBox ID="chbxBateria" Text="Batería" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteHidraulico" Text="Aceite hidráulico" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxReductoresTransito" Text="Reductores de tránsito"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxAceiteMotor" Text="Aceite de motor (Si aplica)" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxRefrigerante" Text="Refrigerante (Si aplica)" runat="server" />
                        </td>
                    </tr>
                    </table>
            </fieldset>

           
        </div>
    </div>
    
