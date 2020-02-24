<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionMartilloHidraulicoPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionMartilloHidraulicoPSLUI" %>



<div id="dvOpciones" style="display: table; width: 100%">

        <div class="dvIzquierda">
            <%--1. Generales--%>
            <fieldset>
                <legend>1. EN GENERAL</legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td class="tdCentradoVertical">
                            <asp:CheckBox ID="chbxCondicionMangueras" Text="Condición de mangueras" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxTaponesManguerasAlimentacion" Text="Tapones de las mangueras de alimentación" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxCondicionBujes" 
                                Text="Condición de bujes" runat="server" 
                                />
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:CheckBox ID="chbxCondicionPasadores" 
                                Text="Condición de pasadores" runat="server" />
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:CheckBox ID="chbxCondicionPica" 
                                Text="Condición de la pica" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxTorqueTornillosBaseMartillo" Text="Torque de tornillos de la base del martillo" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxGraseraManual" 
                                Text="Grasera Manual" runat="server" />
                        </td>
                    </tr>
                                      
                   
                </table>
            </fieldset>
            
            <%--3. Lubricación--%>
            <fieldset>
                    <legend>3. Misceláneos</legend>
                    <table class="trAlinearIzquierda">
                        <tr>
                            <td><asp:CheckBox ID="chbxCondicionPintura" 
                                    Text="Condición de pintura" runat="server" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chbxCalcas" Text="Condición de calcas" 
                                    runat="server" /></td>
                        </tr>
                          <tr>
                            <td><asp:CheckBox ID="chbxSimbolosSeguridadMaquina" Text="Símbolos de seguridad de la máquina" 
                                    runat="server" /></td>
                        </tr>
                          <tr>
                            <td><asp:CheckBox ID="chbxEstructura" Text="Estructura" 
                                    runat="server" /></td>
                        </tr>
                                                
                    </table>
                </fieldset>

       
        </div>

        <div class="dvDerecha">            
            <%--2. Niveles de fluidos--%>
            <fieldset>
                <legend>2. Lubricación </legend>
                <table class="trAlinearIzquierda">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chbxBujes" 
                                Text="Bujes" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:CheckBox ID="chbxPasadores" 
                                Text="Pasadores" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:CheckBox ID="chbxPica" 
                                Text="Pica" runat="server" />
                        </td>
                    </tr>
                    </table>
            </fieldset>

        
            
         
        </div>
    </div>