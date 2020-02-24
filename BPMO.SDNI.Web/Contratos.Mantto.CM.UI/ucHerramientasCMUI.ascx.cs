//Satisface al CU029 - Consultar Contratos de Mantenimiento
//Satisface al CU095 - Imprimir Pagaré Contrato CM
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BOF;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.UI
{
    public partial class ucHerramientasCMUI : System.Web.UI.UserControl, IucHerramientasManttoVIS
    {
        #region Atributos

        /// <summary>
        /// Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private string nombreClase = "ucHerramientasCMUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// Presentador
        /// </summary>
        internal ucHerramientasManttoPRE Presentador { get; set; }

        /// <summary>
        /// Manejador de Eventos para Eliminar un Contrato Borrador
        /// </summary>
        internal EventHandler EliminarContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Editar un Contrato Borrador
        /// </summary>
        internal EventHandler EditarContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Agregar documentos a un Contrato EnCurso o Cerrado
        /// </summary>
        internal EventHandler AgregarDocumentos { get; set; }

        /// <summary>
        /// Manejador de Eventos para Terminar un Contrato en Curso
        /// </summary>
        internal EventHandler CerrarContrato { get; set; }

        /// <summary>
        /// Manejador de Eventos para Cancelar un Contrato en Curso
        /// </summary>
        internal EventHandler CancelarContrato { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el contrato RD
        /// </summary>
        internal EventHandler ImprimirContrato { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el manual de operaciones
        /// </summary>
        internal EventHandler ImprimirManualOperaciones { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el anexo a
        /// </summary>
        internal EventHandler ImprimirAnexoA { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el anexo b
        /// </summary>
        internal EventHandler ImprimirAnexoB { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el anexo c
        /// </summary>
        internal EventHandler ImprimirAnexoC { get; set; }

        /// <summary>
        /// Manejeador de Evento para imprimir el contrato, anexo y manual de operaciones
        /// </summary>
        internal EventHandler ImprimirTodo { get; set; }
		/// <summary>
		/// Manejador de Eventos para imprimir el pagaré
		/// </summary>
		internal EventHandler ImprimirPagare { get; set; }

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
                    var contrato = new ContratoManttoBOF { EstatusText = txtEstatus.Text };
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
                        var contrato = new ContratoManttoBOF { Estatus = value };
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

        #endregion

        #region Constructor

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucHerramientasManttoPRE(this);

                this.ucucListadoPlantillasUI.Identificador = "ucContratosMantenimiento";
                this.ucucListadoPlantillasUI.PermitirEliminar = false;
                this.ucucListadoPlantillasUI.PrepararEdicion(false);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load: " + ex.Message);
            }
        }

        #endregion

        #region Métodos
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
                    case "Editar":
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "EditarContrato")
                                if (estatus != EEstatusContrato.Borrador)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                        }
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "AgregarDocumentos")
                                if (estatus != EEstatusContrato.EnCurso && estatus != EEstatusContrato.Cerrado)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                        }
                        if (item.ChildItems.Count <= 0)
                            item.Enabled = false;
                        break;
                    case "Cerrar":                        
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "CerrarContrato")
                                if (estatus != EEstatusContrato.EnCurso)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                        }
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "CancelarContrato")
                                if (estatus != EEstatusContrato.Borrador)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                        }
                        if (item.ChildItems.Count <= 0)
                            item.Enabled = false;
                        break;
                    case "EliminarContrato":
                        if (estatus == EEstatusContrato.Borrador) item.Enabled = true;
                        break;
                    case "Impresion":
                        item.Enabled = true;
                        break;
                    default:
                        item.Enabled = false; 
                        break;
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
                Configurar(EstatusContrato);
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
                if (item.Value == "Editar")
                    item.Enabled = true;
            }
        }

        /// <summary>
        /// Marca como Seleccionado la Opcion Editar Contrato
        /// </summary>
        public void MarcarOpcionEditarContrato()
        {
            MenuItem item = mnContratos.FindItem("Editar");
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
            MenuItem item = mnContratos.FindItem("Editar");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
            item.ChildItems.Clear();
        }
        public void MarcarOpcionCerrarContrato()
        {
            MenuItem item = mnContratos.FindItem("Cerrar");
            item.Enabled = true;
            item.Selectable = true;
            item.Selected = true;
            item.ChildItems.Clear();
        }
        public void MarcarOpcionCancelarContrato()
        {
            MenuItem item = mnContratos.FindItem("Cerrar");
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
                mnContratos.Items.Remove(item);
        }
        /// <summary>
        /// Oculta el Estatus del contrato
        /// </summary>
        public void OcultarEstatusContrato()
        {
            MenuItem item = mnContratos.FindItem("Estatus");

            if (item != null)
                mnContratos.Items.Remove(item);
        }
        /// <summary>
        /// Oculta el Menu de Impresion
        /// </summary>
        public void OcultarMenuImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
                mnContratos.Items.Remove(item);
        }
        /// <summary>
        /// Oculta la opcion de Editar Contrato
        /// </summary>
        public void OcultarEditarContrato()
        {
            MenuItem item = mnContratos.FindItem("Editar");

            if (item != null)
                mnContratos.Items.Remove(item);
        }
        /// <summary>
        /// Oculta la opcion Eliminar Contrato
        /// </summary>
        public void OcultarEliminarContrato()
        {
            MenuItem item = mnContratos.FindItem("EliminarContrato");

            if (item != null)
                mnContratos.Items.Remove(item);
        }
        /// <summary>
        /// Oculta la opcion Cerrar Contrato
        /// </summary>
        public void OcultarCerrarContrato()
        {
            MenuItem item = mnContratos.FindItem("Cerrar");

            if (item != null)
                mnContratos.Items.Remove(item);
        }
        /// <summary>
        /// Oculta el botón para ver las plantillas de contratos
        /// </summary>
        public void OcultarPlantillas()
        {
            this.btnPlantillas.Visible = false;
        }

        /// <summary>
        /// Deshabilita la opción de IMPRESION de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
                item.Enabled = false;
        }
        /// <summary>
        /// Deshabilita la opción de CERRAR CONTRATO de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionCerrar()
        {
            MenuItem item = mnContratos.FindItem("Cerrar");

            if (item != null)
                item.Enabled = false;
        }
        /// <summary>
        /// Deshabilita la opción EDITAR de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionEditar()
        {
            MenuItem item = mnContratos.FindItem("Editar");

            if (item != null)
                item.Enabled = false;
        }
        /// <summary>
        /// Deshabilita la opción ELIMINAR CONTRATO de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionBorrar()
        {
            MenuItem item = mnContratos.FindItem("EliminarContrato");

            if (item != null)
                item.Enabled = false;
        }
        /// <summary>
        /// Deshabilita la opción IMPRIMIR CONTRATO  de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarMenuImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
                item.Enabled = false;
        }
        public void DeshabilitarOpcionesEditarContrato()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                switch (item.Value)
                {
                    case "Editar":
                        item.Enabled = false;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "EditarContrato")
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
                    case "Editar":
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
        /// Carga los archivos en el UC de plantillas
        /// </summary>
        /// <param name="resultado"></param>
        public void CargarArchivos(List<object> resultado)
        {
            this.ucucListadoPlantillasUI.Identificador = "ucContratosMantenimiento";
            this.ucucListadoPlantillasUI.PermitirEliminar = false;
            this.ucucListadoPlantillasUI.PrepararVista(false);
            this.ucucListadoPlantillasUI.Documentos = resultado;
            this.ucucListadoPlantillasUI.CargarElementosEncontrados(resultado);
        }

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
                case "CancelarContrato":
                    if (CancelarContrato != null) CancelarContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "EditarContrato":
                    if (EditarContrato != null) EditarContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "AgregarDocumentos":
                    if (AgregarDocumentos != null) AgregarDocumentos.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImprimirContrato":
                    if (ImprimirContrato != null) ImprimirContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImprimirManualOperaciones":
                    if (ImprimirManualOperaciones != null) ImprimirManualOperaciones.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImprimirAnexoA":
                    if (ImprimirAnexoA != null) ImprimirAnexoA.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImprimirAnexoB":
                    if (ImprimirAnexoB != null) ImprimirAnexoB.Invoke(sender, EventArgs.Empty);
                    break;
                case "ImprimirAnexoC":
                    if (ImprimirAnexoC != null) ImprimirAnexoC.Invoke(sender, EventArgs.Empty);
                    break;
				case "ImprimirPagare":
					if (ImprimirPagare != null) ImprimirPagare.Invoke(sender, EventArgs.Empty);
					break;
                case "ImprimirTodo":
                    if (ImprimirTodo != null) ImprimirTodo.Invoke(sender, EventArgs.Empty);
                    break;
            }
        }

        #endregion
    }
}