using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BOF;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucHerramientasPSLUI : System.Web.UI.UserControl, IucHerramientasPSLVIS {
        #region Atributos

        /// <summary>
        /// Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private const string NombreClase = "ucHerramientasPSLUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// Presentador
        /// </summary>
        internal ucHerramientasPSLPRE Presentador { get; set; }

        /// <summary>
        /// Manejador de Eventos para Eliminar un Contrato Borrador
        /// </summary>
        internal EventHandler EliminarContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Editar un Contrato Borrador
        /// </summary>
        internal EventHandler EditarContratoRD { get; set; }

        /// <summary>
        /// Manejador de Eventos para Agregar documentos a un Contrato EnCurso o Cerrado
        /// </summary>
        internal EventHandler AgregarDocumentos { get; set; }

        /// <summary>
        /// Manejador de Eventos para Terminar un Contrato en Curso
        /// </summary>
        internal EventHandler CerrarContrato { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir la plantilla de contrato
        /// </summary>
        internal EventHandler ImprimirPlantilla { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el contrato RO
        /// </summary>
        internal EventHandler ImprimirContratoRO { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el pagare RO
        /// </summary>
        internal EventHandler ImprimirPagareRO { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el pagare RO
        /// </summary>
        internal EventHandler ImprimirPagareROC { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el contrato ROC
        /// </summary>
        internal EventHandler ImprimirContratoROC { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el chek list de contrato maestro
        /// </summary>
        internal EventHandler ImprimirContratoMaestro { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el chek list de entrega o recepción
        /// </summary>
        internal EventHandler ImprimirChkEntregaRecepcion { get; set; }

        /// <summary>
        /// Manejador de Evento para imprimir el chek list de entrega o recepción
        /// </summary>
        internal EventHandler ImprimirPlantillaCheckList { get; set; }
        /// <summary>
        /// Manejador de Evento para imprimir el chek list de entrega o recepción
        /// </summary>
        internal EventHandler ImprimirCabeceraCheckList { get; set; }

        /// <summary>
        /// Manejador de Eventos para Intercambiar una unidad en un Contrato en Curso
        /// </summary>
        internal EventHandler IntercambioUnidadContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Renovar un Contrato
        /// </summary>
        internal EventHandler RenovarContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Renovar un Contrato
        /// </summary>
        internal EventHandler ImprimirContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Editar un Contrato En Curso
        /// </summary>
        internal EventHandler EditarEnCurso { get; set; }

        /// <summary>
        /// Manejador de Eventos para Generar la Solicitud de Pago
        /// </summary>
        internal EventHandler GenerarSolicitudPago { get; set; }

        /// <summary>
        /// Numero de Contrato
        /// </summary>
        public string NumeroContrato {
            get {
                var txtNumeroContrato = mnContratos.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null && txtNumeroContrato.Text != null && !string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()))
                    return txtNumeroContrato.Text.Trim();
                return null;
            }
            set {
                var txtNumeroContrato = mnContratos.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null) {
                    txtNumeroContrato.Text = value ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public EEstatusContrato? EstatusContrato {
            get {

                if (hdnEstatusContrato != null && !string.IsNullOrEmpty(hdnEstatusContrato.Value)) {
                    var contrato = new ContratoPSLBOF();
                    contrato.EstatusText = hdnEstatusContrato.Value;
                    return contrato.Estatus;
                }
                return null;
            }
            set {
                var txtEstatus = mnContratos.Controls[mnContratos.Controls.Count - 1].FindControl("txtValue") as TextBox;
                if (txtEstatus != null) {
                    if (value != null) {
                        var contrato = new ContratoPSLBOF { Estatus = value };
                        txtEstatus.Text = contrato.EstatusText.ToUpper();
                        hdnEstatusContrato.Value = contrato.EstatusText.ToUpper();
                    }

                }
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID {
            get { return !string.IsNullOrEmpty(hdnContratoID.Value) ? (int?)int.Parse(hdnContratoID.Value) : null; }
            set {
                hdnContratoID.Value = (value != null) ? value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Manejador de Eventos para imprimir el pagaré
        /// </summary>
        internal EventHandler ImprimirPagare { get; set; }
        #endregion

        #region Constructor

        protected void Page_Load(object sender, EventArgs e) {
            try {
                Presentador = new ucHerramientasPSLPRE(this);
                #region Se prepara el user control para el ambiente de consulta
                this.ucucListadoPlantillasUI.Identificador = "ucContratosRentaDiaria";
                this.ucucListadoPlantillasUI.PermitirEliminar = false;
                this.ucucListadoPlantillasUI.PrepararEdicion(false);

                //RQM 15003
                this.btnPlantillas.Visible = false;
                this.ucucListadoPlantillasUI.Visible = false;

                #endregion
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Configura el Menú de acuerdo al Estatus del Contrato
        /// </summary>
        /// <param name="estatus">Estatus del Contrato</param>
        public void Configurar(EEstatusContrato? estatus) {
            foreach (MenuItem item in mnContratos.Items) {
                switch (item.Value) {
                    case "EditarContrato":
                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value == "EditarContratoRO") {
                                if (estatus == EEstatusContrato.EnCurso)
                                    innerItem.Enabled = true;
                                else
                                    innerItem.Enabled = false;
                                break;
                            }
                        }
                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value == "AgregarDocumentos") {
                                if (estatus != EEstatusContrato.EnCurso && estatus != EEstatusContrato.Cerrado) {
                                    innerItem.Enabled = false;
                                    break;
                                }
                            }
                        }
                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value == "RenovarContrato") {
                                if (estatus == EEstatusContrato.EnCurso)
                                    innerItem.Enabled = true;
                                else
                                    innerItem.Enabled = false;
                            }
                        }
                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value == "IntercambioUnidadContrato") {
                                if (estatus == EEstatusContrato.EnCurso || estatus == EEstatusContrato.PendientePorCerrar)
                                    innerItem.Enabled = true;
                                else
                                    innerItem.Enabled = false;
                                break;
                            }
                        }
                        break;
                    case "CerrarContrato":
                        if (estatus == EEstatusContrato.PendientePorCerrar)
                            item.Enabled = true;
                        else
                            item.Enabled = false;
                        break;
                    case "EliminarContrato":
                        if (estatus == EEstatusContrato.Borrador) item.Enabled = true;
                        break;
                    case "Impresion":

                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value.CompareTo("ImpContratoRO") == 0) {
                                innerItem.Enabled = false;
                                if (estatus != EEstatusContrato.Eliminado) {
                                    innerItem.Enabled = true;
                                    innerItem.Selectable = true;
                                    item.Enabled = true;
                                    item.Selectable = true;
                                    break;
                                }
                            }
                        }

                        break;
                    case "GenerarSolicitudPago":
                        if (estatus == EEstatusContrato.EnCurso)
                            item.Enabled = true;
                        break;
                    default:
                        item.Enabled = false; break;
                }
            }
        }

        /// <summary>
        /// Habilita o deshabilita las opciones del menú
        /// </summary>
        /// <param name="habilitar">indica si se se habilita o deshabilitan las opciones</param>
        public void HabilitarOpciones(bool habilitar) {
            if (habilitar) {
                Configurar(EstatusContrato);
            } else {
                foreach (MenuItem item in mnContratos.Items)
                    item.Enabled = false;
            }
        }

        public void HabilitarOpcionesEdicion() {
            foreach (MenuItem item in mnContratos.Items) {
                if (item.Value == "EditarContrato")
                    item.Enabled = true;
            }
        }

        public void DeshabilitarOpcionesEditarContratoPSL() {
            foreach (MenuItem item in mnContratos.Items) {
                switch (item.Value) {
                    case "EditarContrato":
                        item.Enabled = false;
                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value == "EditarContratoPSL") {
                                item.ChildItems.Remove(innerItem);
                                break;
                            }

                        }
                        break;
                }
            }
        }

        public void DeshabilitarOpcionesAgregarDoc() {
            foreach (MenuItem item in mnContratos.Items) {
                switch (item.Value) {
                    case "EditarContrato":
                        item.Enabled = false;
                        foreach (MenuItem innerItem in item.ChildItems) {
                            if (innerItem.Value == "AgregarDocumentos") {
                                item.ChildItems.Remove(innerItem);
                                break;
                            }

                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Marca como Seleccionado la Opción Editar Contrato
        /// </summary>
        public void MarcarOpcionEditarContrato() {
            MenuItem item = mnContratos.FindItem("EditarContrato");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
            item.ChildItems.Clear();
        }

        /// <summary>
        /// Marca como Seleccionado la Opción Agregar Documentos
        /// </summary>
        public void MarcarOpcionAgregarDocumentos() {
            MenuItem item = mnContratos.FindItem("EditarContrato");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
            item.ChildItems.Clear();
        }

        /// <summary>
        /// Ocultar el Numero de Contrato
        /// </summary>
        public void OcultarNoContrato() {
            MenuItem item = mnContratos.FindItem("Contrato");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta el Estatus del contrato
        /// </summary>
        public void OcultarEstatusContrato() {
            MenuItem item = mnContratos.FindItem("Estatus");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta el Menú de Impresión
        /// </summary>
        public void OcultarMenuImpresion() {
            MenuItem item = mnContratos.FindItem("Impresion");

            //if (item != null)
            //{
            //    mnContratos.Items.Remove(item);
            //}
        }

        /// <summary>
        /// Oculta la opción de Editar Contrato
        /// </summary>
        public void OcultarEditarContrato() {
            MenuItem item = mnContratos.FindItem("EditarContrato");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }


        /// <summary>
        /// Oculta la opción Cerrar Contrato
        /// </summary>
        public void OcultarCerrarContrato() {
            MenuItem item = mnContratos.FindItem("CerrarContrato");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta el menú de imprimir plantilla
        /// </summary>        
        public void OcultarImprimirPlantilla() {
            MenuItem item = mnContratos.FindItem("PlantillaContrato");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Deshabilita la opción de IMPRESION de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionImpresion() {
            MenuItem item = mnContratos.FindItem("Impresion");

            //if (item != null)
            //{
            //    item.Enabled = false;
            //}
        }

        /// <summary>
        /// Deshabilita la opción de CERRAR CONTRATO de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionCerrar() {
            MenuItem item = mnContratos.FindItem("CerrarContrato");

            if (item != null) {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción EDITAR de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionEditar() {
            MenuItem item = mnContratos.FindItem("EditarContrato");

            if (item != null) {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción ELIMINAR CONTRATO de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionBorrar() {
            MenuItem item = mnContratos.FindItem("EliminarContrato");

            if (item != null) {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción IMPRIMIR CONTRATO  de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarMenuImpresion() {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null) {
                item.Enabled = false;
            }

            item = mnContratos.FindItem("PlantillaContrato");

            if (item != null) {
                item.Enabled = false;
            }
        }

        #region CU012
        /// <summary>
        /// Oculta el menú de imprimir plantilla del check list
        /// </summary>        
        public void OcultarImprimirPlantillaCheckList() {
            MenuItem item = mnContratos.FindItem("PlantillaCheckList");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }
        #endregion

        public void MarcarOpcionCerrarContrato() {
            MenuItem item = mnContratos.FindItem("CerrarContrato");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
        }

        /// <summary>
        /// Oculta el botón para ver las plantillas de contratos
        /// </summary>
        public void OcultarPlantillas() {
            this.btnPlantillas.Visible = false;
        }

        #region SC0038
        /// <summary>
        /// Carga los archivos en el UC de plantillas
        /// </summary>
        /// <param name="resultado"></param>
        public void CargarArchivos(List<object> resultado) {
            this.ucucListadoPlantillasUI.Identificador = "ucContratosRentaOrdinario";
            this.ucucListadoPlantillasUI.PermitirEliminar = false;
            this.ucucListadoPlantillasUI.PrepararVista(false);
            this.ucucListadoPlantillasUI.Documentos = resultado;
            this.ucucListadoPlantillasUI.CargarElementosEncontrados(resultado);
        }
        #endregion

        /// <summary>
        /// Oculta el botón solicitud de pago
        /// </summary>
        public void OcultarSolicitudPago() {
            MenuItem item = mnContratos.FindItem("GenerarSolicitudPago");

            if (item != null) {
                mnContratos.Items.Remove(item);
            }
        }
        #endregion

        #region Eventos

        protected void mnContratos_MenuItemClick(object sender, MenuEventArgs e) {
            switch (e.Item.Value) {
                case "EliminarContrato":
                    if (EliminarContrato != null) EliminarContrato.Invoke(e, EventArgs.Empty);
                    break;
                case "CerrarContrato":
                    if (CerrarContrato != null) CerrarContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "EditarContratoRO":
                    if (EditarContratoRD != null) EditarContratoRD.Invoke(sender, EventArgs.Empty);
                    if (EditarEnCurso != null) EditarEnCurso.Invoke(sender, EventArgs.Empty);
                    break;
                case "AgregarDocumentos":
                    if (AgregarDocumentos != null) AgregarDocumentos.Invoke(sender, EventArgs.Empty);
                    break;
                case "PlantillaContrato":
                    if (ImprimirPlantilla != null) ImprimirPlantilla.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImpContratoRO":
                    //Imprimir contrato RO
                    if (ImprimirContratoRO != null)
                        ImprimirContratoRO.Invoke(sender, EventArgs.Empty);
                    //Imprimir contrato ROC
                    if (ImprimirContratoROC != null)
                        ImprimirContratoROC.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImpPagareRO":
                    //Imprimir pagare RO
                    if (ImprimirPagareRO != null)
                        ImprimirPagareRO.Invoke(sender, EventArgs.Empty);
                    //Imprimir pagare ROC
                    if (ImprimirPagareROC != null)
                        ImprimirPagareROC.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImpContratoM":
                    if (ImprimirContratoMaestro != null) ImprimirContratoMaestro.Invoke(sender, EventArgs.Empty);
                    break;
                case "ChkList":
                    if (ImprimirChkEntregaRecepcion != null) ImprimirChkEntregaRecepcion.Invoke(sender, EventArgs.Empty);
                    break;
                case "PlantillaCheckList":
                    if (ImprimirPlantillaCheckList != null) ImprimirPlantillaCheckList.Invoke(sender, EventArgs.Empty);
                    break;
                case "ChkListCabecera":
                    if (ImprimirCabeceraCheckList != null) ImprimirCabeceraCheckList.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImprimirPagare":
                    if (ImprimirPagare != null) ImprimirPagare.Invoke(sender, EventArgs.Empty);
                    break;
                case "IntercambioUnidadContrato":
                    if (IntercambioUnidadContrato != null) IntercambioUnidadContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "RenovarContrato":
                    if (RenovarContrato != null) RenovarContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "GenerarSolicitudPago":
                    if (GenerarSolicitudPago != null) GenerarSolicitudPago.Invoke(sender, EventArgs.Empty);
                    break;
            }
        }

        #endregion
    }
}