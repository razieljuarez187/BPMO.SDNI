<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPuntosVerificacionPistolaNeumaticaPSLUI.ascx.cs" Inherits="BPMO.SDNI.Contratos.PSL.UI.ucPuntosVerificacionPistolaNeumaticaPSLUI" %>


<style type="text/css">
    .style1
    {
        height: 26px;
    }
</style>


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
                    <td class="style1">
                        <asp:CheckBox ID="chbxCondicionLubricador" Text="Condición del lubricador" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCondicionBujes" Text="Condición de bujes" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxCondicionPica" Text="Condición de la pica" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxAjustePresionCompresor" Text="Ajuste de presión del compresor (Max. 90 PSI)"
                            runat="server" />
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
                        <asp:CheckBox ID="chbxCondicionPintura" Text="Condición de pintura" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbxEstructura" Text="Estructura" runat="server" />
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
                        <asp:CheckBox ID="chbxNivelAceiteLubricador" Text="Nivel de aceite en el lubricador"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</div>
