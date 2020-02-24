//Satisface al caso de uso CU003 - Consultar Contrato Renta Diaria
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
// Satisface al CU013 - Cerrar Contrato Renta Diaria
//Satisface al CU002 - Editar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.RD.PRE;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class ucHerramientasRDUI : System.Web.UI.UserControl, IucHerramientasRDVIS
    {
        #region Atributos

        /// <summary>
        /// Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private const string NombreClase = "ucHerramientasRDUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// Presentador
        /// </summary>
        internal ucHerramientasRDPRE Presentador { get; set; }

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
        /// Manejeador de Evento para imprimir la plantilla de contrato
        /// </summary>
        internal EventHandler ImprimirPlantilla { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el contrato RD
        /// </summary>
        internal EventHandler ImprimirContratoRD { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el chek list de contrato maestro
        /// </summary>
        internal EventHandler ImprimirContratoMaestro { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el chek list de entrega o recepcion
        /// </summary>
        internal EventHandler ImprimirChkEntregaRecepcion { get; set; }
        #region CU012
        /// <summary>
        /// Manejeador de Evento para imprimir el chek list de entrega o recepcion
        /// </summary>
        internal EventHandler ImprimirPlantillaCheckList { get; set; }
        /// <summary>
        /// Manejeador de Evento para imprimir el chek list de entrega o recepcion
        /// </summary>
        internal EventHandler ImprimirCabeceraCheckList { get; set; }
        #endregion
        
        /// <summary>
        /// Numero de Contrato
        /// </summary>
        public string NumeroContrato
        {
            get
            {
                var txtNumeroContrato = mnContratos.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null && txtNumeroContrato.Text != null && !string.IsNullOrEmpty(txtNumeroContrato.Text.Trim()))
                    return txtNumeroContrato.Text.Trim();
                return null;
            }
            set
            {
                var txtNumeroContrato = mnContratos.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null)
                {
                    txtNumeroContrato.Text = value ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public EEstatusContrato? EstatusContrato
        {
            get
            {

                var txtEstatus = mnContratos.Controls[5].FindControl("txtValue") as TextBox;
                if (txtEstatus != null && txtEstatus.Text != null && !string.IsNullOrEmpty(txtEstatus.Text.Trim()))
                {
                    var contrato = new ContratoRDBOF { EstatusText = txtEstatus.Text };
                    return contrato.Estatus;
                }
                return null;
            }
            set
            {
                var txtEstatus = mnContratos.Controls[5].FindControl("txtValue") as TextBox;
                if (txtEstatus != null)
                {
                    if (value != null)
                    {
                        var contrato = new ContratoRDBOF { Estatus = value };
                        txtEstatus.Text = contrato.EstatusText.ToUpper();
                    }
                    else
                        txtEstatus.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID
        {
            get { return !string.IsNullOrEmpty(hdnContratoID.Value) ? (int?)int.Parse(hdnContratoID.Value) : null; }
            set
            {
                hdnContratoID.Value = (value != null) ? value.ToString() : string.Empty;
            }
        }

		/// <summary>
		/// Manejador de Eventos para imprimir el pagaré
		/// </summary>
		internal EventHandler ImprimirPagare { get; set; }
        #endregion

        #region Constructor

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucHerramientasRDPRE(this);
                #region SC0038 se prepara el user control para el ambiente de consulta
                this.ucucListadoPlantillasUI.Identificador = "ucContratosRentaDiaria";
                this.ucucListadoPlantillasUI.PermitirEliminar = false;
                this.ucucListadoPlantillasUI.PrepararEdicion(false);
                #endregion
            }
            catch (Exception ex)
            {
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
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Configura el Menu de acuerdo al Estatus del Contrato
        /// </summary>
        /// <param name="estatus">Estatus del Contrato</param>
        public void Configurar(EEstatusContrato? estatus)
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                item.Enabled = false;
                switch (item.Value)
                {
                    case "EditarContrato":
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "EditarContratoRD")
                            {
                                if (estatus != EEstatusContrato.Borrador)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                            }
                        }
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "AgregarDocumentos")
                            {
                                if (estatus != EEstatusContrato.EnCurso && estatus != EEstatusContrato.Cerrado)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                            }
                        }
                        if (item.ChildItems.Count <= 0)
                            item.Enabled = false;
                        break;
                    case "CerrarContrato":
                        if (estatus == EEstatusContrato.EnPausa || estatus == EEstatusContrato.PendientePorCerrar) item.Enabled = true;
                        break;
                    case "EliminarContrato":
                        if (estatus == EEstatusContrato.Borrador) item.Enabled = true;
                        break;
                    case "Impresion":
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value.CompareTo("ChkListCabecera") == 0)
                            {
                                if (estatus != EEstatusContrato.EnCurso && estatus != EEstatusContrato.PendientePorCerrar)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                            }
                        }
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value.CompareTo("ChkList") == 0)
                            {
                                if (estatus != EEstatusContrato.EnCurso && estatus != EEstatusContrato.PendientePorCerrar && estatus != EEstatusContrato.Cancelado && estatus != EEstatusContrato.Cerrado)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                            }
                        }
                        if (item.ChildItems.Count <= 0)
                            item.Enabled = false;
                        break;
                    default:
                        item.Enabled = false; break;
                }
            }
        }

        /// <summary>
        /// Habilita o deshabilita las opciones del menu
        /// </summary>
        /// <param name="habilitar">indica si se se habilita o deshabilitan las opciones</param>
        public void HabilitarOpciones(bool habilitar)
        {
            if (habilitar)
            {
                Configurar(EstatusContrato);
            }
            else
            {
                foreach (MenuItem item in mnContratos.Items)
                    item.Enabled = false;
            }
        }

        public void HabilitarOpcionesEdicion()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                if (item.Value == "EditarContrato")
                    item.Enabled = true;
            }
        }

        public void DeshabilitarOpcionesEditarContratoRD()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                switch (item.Value)
                {
                    case "EditarContrato":
                        item.Enabled = false;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "EditarContratoRD")
                            {
                                item.ChildItems.Remove(innerItem);
                                break;
                            }

                        }
                        break;
                }
            }
        }

        public void DeshabilitarOpcionesAgregarDoc()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                switch (item.Value)
                {
                    case "EditarContrato":
                        item.Enabled = false;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "AgregarDocumentos")
                            {
                                item.ChildItems.Remove(innerItem);
                                break;
                            }

                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Marca como Seleccionado la Opcion Editar Contrato
        /// </summary>
        public void MarcarOpcionEditarContrato()
        {
            MenuItem item = mnContratos.FindItem("EditarContrato");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
            item.ChildItems.Clear();
        }

        /// <summary>
        /// Marca como Seleccionado la Opcion Agregar Documentos
        /// </summary>
        public void MarcarOpcionAgregarDocumentos()
        {
            MenuItem item = mnContratos.FindItem("EditarContrato");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
            item.ChildItems.Clear();
        }

        /// <summary>
        /// Ocultar el Numero de Contrato
        /// </summary>
        public void OcultarNoContrato()
        {
            MenuItem item = mnContratos.FindItem("Contrato");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta el Estatus del contrato
        /// </summary>
        public void OcultarEstatusContrato()
        {
            MenuItem item = mnContratos.FindItem("Estatus");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta el Menu de Impresion
        /// </summary>
        public void OcultarMenuImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta la opcion de Editar Contrato
        /// </summary>
        public void OcultarEditarContrato()
        {
            MenuItem item = mnContratos.FindItem("EditarContrato");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta la opcion Eliminar Contrato
        /// </summary>
        public void OcultarEliminarContrato()
        {
            MenuItem item = mnContratos.FindItem("EliminarContrato");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta la opcion Cerrar Contrato
        /// </summary>
        public void OcultarCerrarContrato()
        {
            MenuItem item = mnContratos.FindItem("CerrarContrato");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Oculta el menu de imprimir plantilla
        /// </summary>        
        public void OcultarImprimirPlantilla()
        {
            MenuItem item = mnContratos.FindItem("PlantillaContrato");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }

        /// <summary>
        /// Deshabilita la opción de IMPRESION de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
            {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción de CERRAR CONTRATO de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionCerrar()
        {
            MenuItem item = mnContratos.FindItem("CerrarContrato");

            if (item != null)
            {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción EDITAR de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionEditar()
        {
            MenuItem item = mnContratos.FindItem("EditarContrato");

            if (item != null)
            {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción ELIMINAR CONTRATO de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionBorrar()
        {
            MenuItem item = mnContratos.FindItem("EliminarContrato");

            if (item != null)
            {
                item.Enabled = false;
            }
        }

        /// <summary>
        /// Deshabilita la opción IMPRIMIR CONTRATO  de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarMenuImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
            {
                item.Enabled = false;
            }

            item = mnContratos.FindItem("PlantillaContrato");

            if (item != null)
            {
                item.Enabled = false;
            }
        }
        
        #region CU012
        /// <summary>
        /// Oculta el menu de imprimir plantilla del check list
        /// </summary>        
        public void OcultarImprimirPlantillaCheckList()
        {
            MenuItem item = mnContratos.FindItem("PlantillaCheckList");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }
        #endregion
        
        public void MarcarOpcionCerrarContrato()
        {
            MenuItem item = mnContratos.FindItem("CerrarContrato");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
        }

        /// <summary>
        /// Oculta el botón para ver las plantillas de contratos
        /// </summary>
        public void OcultarPlantillas()
        {
            this.btnPlantillas.Visible = false;
        }

        #region SC0038
        /// <summary>
        /// Carga los archivos en el UC de plantillas
        /// </summary>
        /// <param name="resultado"></param>
        public void CargarArchivos(List<object> resultado)
        {
            this.ucucListadoPlantillasUI.Identificador = "ucContratosRentaDiaria";
            this.ucucListadoPlantillasUI.PermitirEliminar = false;
            this.ucucListadoPlantillasUI.PrepararVista(false);
            this.ucucListadoPlantillasUI.Documentos = resultado;
            this.ucucListadoPlantillasUI.CargarElementosEncontrados(resultado);
        }
        #endregion
        #endregion

        #region Eventos

        protected void mnContratos_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "EliminarContrato":
                    if (EliminarContrato != null) EliminarContrato.Invoke(e, EventArgs.Empty);
                    break;
                case "CerrarContrato":
                    if (CerrarContrato != null) CerrarContrato.Invoke(sender, EventArgs.Empty);
                    break;               
                case "EditarContratoRD":
                    if (EditarContratoRD != null) EditarContratoRD.Invoke(sender, EventArgs.Empty);
                    break;
                case "AgregarDocumentos":
                    if (AgregarDocumentos != null) AgregarDocumentos.Invoke(sender, EventArgs.Empty);
                    break;
                case "PlantillaContrato":
                    if (ImprimirPlantilla != null) ImprimirPlantilla.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImpContratoRD":
                    if (ImprimirContratoRD != null) ImprimirContratoRD.Invoke(sender, EventArgs.Empty);
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
            }
        }

        #endregion
    }
}