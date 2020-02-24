// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BOF;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Collections.Generic;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucHerramientasFSLUI : UserControl, IucHerramientasFSLVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase para agregar al mensaje de las excepciones
        /// </summary>
        private const string NombreClase = "ucHerramientasFSLUI";
        #endregion

        #region Propiedades

        /// <summary>
        /// Presentador
        /// </summary>
        internal ucHerramientasFSLPRE Presentador { get; set; }
        /// <summary>
        /// Manejador de Eventos para Eliminar un Contrato Borrador
        /// </summary>
        internal EventHandler EliminarContrato { get; set; }
        #region SC0002
        /// <summary>
        /// Manejador de Eventos para Editar un Contrato Borrador
        /// </summary>
        internal EventHandler EditarContratoFSL { get; set; }
        /// <summary>
        /// Manejador de Eventos para Agregar documentos a un Contrato EnCurso o Cerrado
        /// </summary>
        internal EventHandler AgregarDocumentos { get; set; }
        #endregion
        /// <summary>
        /// Manejador de Eventos para Redirigir a Modificar Lineas del Contrato
        /// </summary>
        internal EventHandler ModificarUnidadesContratoFSL { get; set; }
        /// <summary>
        /// Manejador de Eventos para Terminar un Contrato en Curso
        /// </summary>
        internal EventHandler CerrarContrato { get; set; }
        /// <summary>
        /// Manejeador de Evento para imprimir la Constancia de Bienes
        /// </summary>
        internal EventHandler ImprimirConstanciaBienes { get; set; }
        /// <summary>
        /// Manejeador de Evento para imprimir el Manual de Operaciones
        /// </summary>
        internal EventHandler ImprimirManualOperaciones { get; set; }
        /// <summary>
        /// Manejador de Evento para Imprimir el Anexo A
        /// </summary>
        internal EventHandler ImprimirAnexoA { get; set; }
        /// <summary>
        /// Manejador de Evento para Imprimir el Anexo B
        /// </summary>
        internal EventHandler ImprimirAnexoB { get; set; }
        /// <summary>
        /// Manejador de Evento para Imprimir el Anexo C
        /// </summary>
        internal EventHandler ImprimirAnexoC { get; set; }
        /// <summary>
        /// Manejador de Evento para Imprimir el Contrato Maestro
        /// </summary>
        internal EventHandler ImprimirContratoMaestro { get; set; }
        /// <summary>
        /// Manejador de Eventos para Imprimir Todos los Anexos del Contrato
        /// </summary>
        internal EventHandler ImprimirAnexosContrato { get; set; }
        /// <summary>
        /// Manejador de Eventos para Imprimir El Formato de Contrato de Clientes (Persona Fisica)
        /// </summary>
        internal EventHandler ImprimirFormatoPersonaFisica { get; set; }
        /// <summary>
        /// Manejador de Eventos para Imprimir El Formato de Contrato de Clientes (Persona Moral)
        /// </summary>
        internal EventHandler ImprimirFormatoPersonaMoral { get; set; }
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
                    var contrato = new ContratoFSLBOF { EstatusText = txtEstatus.Text };
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
                        var contrato = new ContratoFSLBOF { Estatus = value };
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
            get { return !string.IsNullOrEmpty(hdnContratoID.Value) ? (int?) int.Parse(hdnContratoID.Value) : null; }
            set
            {
                hdnContratoID.Value = (value != null)? value.ToString() : string.Empty;
            }
        }
        #endregion

        #region Metodos
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
                    #region SC0002
                    case "EditarContrato":
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if(innerItem.Value == "ModificarUnidadesContratoFSL")
                                if (estatus != EEstatusContrato.EnCurso)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                            if (innerItem.Value == "EditarContratoFSL")
                                if (estatus != EEstatusContrato.Borrador)
                                {
                                    item.ChildItems.Remove(innerItem);
                                    break;
                                }
                        }
                        break;
                    #endregion
                    case "CerrarContrato":
                        if (estatus == EEstatusContrato.EnCurso) item.Enabled = true;
                        break;
                    case "EliminarContrato":
                        if (estatus == EEstatusContrato.Borrador) item.Enabled = true;
                        break;
                    case "Impresion":
                        if (estatus == EEstatusContrato.Borrador || estatus == EEstatusContrato.EnCurso) item.Enabled = true;
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

        #region SC0002
        public void HabilitarOpcionesEdicion()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                if (item.Value != "EditarContrato")
                    item.Enabled = false;
            }
        }

        public void DeshabilitarOpcionesEditarContratoFSL()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                switch (item.Value)
                {
                    #region SC0002
                    case "EditarContrato":
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "EditarContratoFSL")
                            {
                                item.ChildItems.Remove(innerItem);
                                break;
                            }
                                
                        }
                        break;
                    #endregion
                }
            }
        }

        public void DeshabilitarOpcionesAgregarDoc()
        {
            foreach (MenuItem item in mnContratos.Items)
            {
                switch (item.Value)
                {
                    #region SC0002
                    case "EditarContrato":
                        item.Enabled = true;
                        foreach (MenuItem innerItem in item.ChildItems)
                        {
                            if (innerItem.Value == "AgregarDocumentos")
                            {
                                item.ChildItems.Remove(innerItem);
                                break;
                            }

                        }
                        break;
                    #endregion
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
        #endregion

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
        /// Oculta el menu de impresion de Formatos de Contrato
        /// </summary>        
        public void OcultarFormatosContrato()
        {
            MenuItem item = mnContratos.FindItem("FormatoContrato");

            if (item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }
        #region SC_0008
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
        /// 
        /// </summary>
        public void DeshabilitarMenuImpresion()
        {
            MenuItem item = mnContratos.FindItem("Impresion");

            if (item != null)
            {
                item.Enabled = false;
            }

            item = mnContratos.FindItem("FormatoContrato");

            if (item != null)
            {
                item.Enabled = false;
            }
        }

        #endregion
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
            this.ucucListadoPlantillasUI.Identificador = "ucContratosFSL";
            this.ucucListadoPlantillasUI.PermitirEliminar = false;
            this.ucucListadoPlantillasUI.PrepararVista(false);
            this.ucucListadoPlantillasUI.Documentos = resultado;
            this.ucucListadoPlantillasUI.CargarElementosEncontrados(resultado);
        }
        #endregion
        /// <summary>
        /// Deshabilita la opción de MODIFICAR UNIDADES de la barra de herramientas a petición
        /// </summary>
        public void DeshabilitarOpcionesModificarUnidadesContrato()
        {
            MenuItem item = mnContratos.FindItem("ModificarUnidadesContratoFSL");

            if(item != null)
            {
                item.Enabled = false;
            }
        }
        /// <summary>
        /// Oculta la opcion MODIFICAR UNIDADES
        /// </summary>
        public void OcultarModificarUnidadesContrato()
        {
            MenuItem item = mnContratos.FindItem("ModificarUnidadesContratoFSL");

            if(item != null)
            {
                mnContratos.Items.Remove(item);
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucHerramientasFSLPRE(this);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

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
                #region SC0002
                case "EditarContratoFSL":
                    if (EditarContratoFSL != null) EditarContratoFSL.Invoke(sender, EventArgs.Empty);                    
                    break;
                case "AgregarDocumentos":
                    if (AgregarDocumentos != null) AgregarDocumentos.Invoke(sender, EventArgs.Empty);
                    break;
                #endregion
                case "ModificarUnidadesContratoFSL":
                    if(ModificarUnidadesContratoFSL != null) ModificarUnidadesContratoFSL.Invoke(sender, EventArgs.Empty);
                    break;
                case "ConstanciaBienes":
                    if(ImprimirConstanciaBienes != null) ImprimirConstanciaBienes.Invoke(sender, EventArgs.Empty);
                    break;
                case "ManualOperaciones":
                    if(ImprimirManualOperaciones != null) ImprimirManualOperaciones.Invoke(sender, EventArgs.Empty);
                    break;
                case "AnexoA":
                    if (ImprimirAnexoA != null) ImprimirAnexoA.Invoke(sender, EventArgs.Empty);
                    break;
                case "AnexoC":
                    if(ImprimirAnexoC != null) ImprimirAnexoC.Invoke(sender, EventArgs.Empty);
                    break;
                case "ContratoMaestro":
                    if(ImprimirContratoMaestro != null) ImprimirContratoMaestro.Invoke(sender, EventArgs.Empty);
                    break;
                case "Anexos":
                    if(ImprimirAnexosContrato != null) ImprimirAnexosContrato.Invoke(sender, EventArgs.Empty);
                    break;
                case "PersonaFisica":
                    if (ImprimirFormatoPersonaFisica != null) ImprimirFormatoPersonaFisica.Invoke(sender, EventArgs.Empty);
                    break;
                case "PersonaMoral":
                    if (ImprimirFormatoPersonaMoral != null) ImprimirFormatoPersonaMoral.Invoke(sender, EventArgs.Empty);
                    break;
				case "ImprimirPagare":
					if (ImprimirPagare != null) ImprimirPagare.Invoke(sender, EventArgs.Empty);
					break;
                
            }
        }
        #endregion
    }
}