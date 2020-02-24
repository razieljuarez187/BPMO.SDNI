//Satisface al caso de uso CU003 - Consultar Contrato Renta Diaria
//Satisface al caso de uso CU014 - Imprimir Contrato de Renta Diaria
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class DetalleContratoRDUI : System.Web.UI.Page, IDetalleContratoRDVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Detalle de Contrato RD
        /// </summary>
        private DetalleContratoRDPRE presentador;

        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleContratoRDUI";

        #endregion

        #region Propiedades
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoContratoRD"] != null)
                    return Session["UltimoObjetoContratoRD"];

                return null;
            }
            set
            {
                Session["UltimoObjetoContratoRD"] = value;
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnContratoID.Value))
                    return int.Parse(hdnContratoID.Value);
                return null;
            }
            set
            {
                hdnContratoID.Value = value != null ? value.ToString() : null;
            }
        }
        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public int? EstatusID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value))
                    return int.Parse(this.hdnEstatusID.Value);
                return null;
            }
            set { this.hdnEstatusID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o estabece el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUUA.Value) && !string.IsNullOrWhiteSpace(this.hdnUUA.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnUUA.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUUA.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o estabece la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFUA.Value) && !string.IsNullOrWhiteSpace(this.hdnFUA.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFUA.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFUA.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Observaciones al eliminar un contrato RD en borrador
        /// </summary>
        public string Observaciones
        {
            get { return string.IsNullOrEmpty(txtboxObser.Text.Trim()) ? null : txtboxObser.Text.Trim(); }
            set { txtboxObser.Text = value ?? string.Empty; }
        }

        public bool? Cancelable
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnCancelable.Value))
                    id = bool.Parse(this.hdnCancelable.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                {
                    this.hdnCancelable.Value = value.ToString();
                }
                else
                {
                    this.hdnCancelable.Value = string.Empty;
                }
            }
        }
        public bool? Cerrable
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnCerrable.Value))
                    id = bool.Parse(this.hdnCerrable.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                {
                    this.hdnCerrable.Value = value.ToString();
                }
                else
                {
                    this.hdnCerrable.Value = string.Empty;
                }
            }
        }
        #endregion

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new DetalleContratoRDPRE(this, this.ucHerramientas, this.ucResuContratoRD, this.ucDatosGeneralesElementoUI, this.ucEquiposAliadosUnidadUI, this.ucCatalogoDocumentos, this.ucCatalogoDocumentosEntrega, this.ucCatalogoDocumentosRecepcion);

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();
                    this.presentador.RealizarPrimeraCarga();
                    //SC0038
                    this.presentador.CargarPlantillas();
                }

                this.txtboxObser.Attributes.Add("onkeyup", "checkText(this,250);");

                #region Asociacion Metodos Handlers
                #region SC0020
                ucResuContratoRD.RegresarConsultaFiltro = btnRegresarConsultaF_Click;
                #endregion SC0020
                ucHerramientas.EliminarContrato = EliminarContrato_Click;
                ucHerramientas.EditarContratoRD = EditarContratoRD_Click;
                ucHerramientas.AgregarDocumentos = AgregarDocumentosContratoRD_Click;
                ucHerramientas.CerrarContrato = CerrarContrato_Click;
                ucHerramientas.ImprimirContratoRD = ImprimirContrato_Click;
                ucHerramientas.ImprimirContratoMaestro = ImprimirContratoMaestro_Click;
                #region CU012
                ucHerramientas.ImprimirChkEntregaRecepcion = ImprimirChkListEntRec_Click;
                ucHerramientas.ImprimirCabeceraCheckList = ImprimirCabeceraChkListEntRec_Click;
                #endregion

	            ucHerramientas.ImprimirPagare = ImprimirPagare_Click;

	            #endregion
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        #endregion

        #region Métodos
        public void PrepararVisualizacion()
        {
            this.ucCatalogoDocumentos.Identificador = "documentosAdjuntos";
            this.ucCatalogoDocumentosEntrega.Identificador = "documentosEntrega";
            this.ucCatalogoDocumentosRecepcion.Identificador = "documentosRecepcion";

            this.ucCatalogoDocumentos.EstablecerModoEdicion(false);
            this.ucCatalogoDocumentosEntrega.EstablecerModoEdicion(false);
            this.ucCatalogoDocumentosRecepcion.EstablecerModoEdicion(false);

            this.ucHerramientas.OcultarImprimirPlantilla();
            this.ucHerramientas.OcultarImprimirPlantillaCheckList();//CU012

            this.ucDatosGeneralesElementoUI.EstablecerModoLectura();
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirAEditar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/EditarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarContratoRDUI.aspx"));
        }
        public void RedirigirAAgregarDocumentos()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/AgregarDocumentosContratoUI.aspx"));
        }
        public void RedirigirAImprimir()
        {
            this.Response.Redirect(this.ResolveUrl("../Buscador.UI/VisorReporteUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de cancelación del contrato
        /// </summary>
        public void RedirigirACancelacion()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/CancelarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de cierre del contrato
        /// </summary>
        public void RedirigirACierre()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/CerrarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirRegresar(bool permitir)
        {
            this.btnRegresar.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir)
        {
            this.hlRegistroOrden.Enabled = false;
        }
        public void PermitirEditar(bool permitir)
        {
            if (permitir == false)
                this.ucHerramientas.DeshabilitarOpcionEditar();
        }
        public void PermitirCerrar(bool permitir)
        {
            if (permitir == false)
                this.ucHerramientas.DeshabilitarOpcionCerrar();
        }
        public void PermitirEliminar(bool permitir)
        {
            if (permitir == false)
                this.ucHerramientas.DeshabilitarOpcionBorrar();
        }

        /// <summary>
        /// Limpia de la sesión los datos de impresión del reporte
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        #region SC0038
        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }
        #endregion
        #endregion

        #region Eventos

        protected void EliminarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtboxObser.Text = "";
                RegistrarScript(DateTime.Now.Ticks.ToString() + "mnContratos_MenuItemClick.EliminarContrato", "abrirDialogoEliminar();");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar eliminar el contrato.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void EditarContratoRD_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EditarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar editar el contrato.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void AgregarDocumentosContratoRD_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarDocumentos();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar agregar documentos al contrato.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void CerrarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CerrarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar cerrar el contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void btnEliminarContratoBorrador_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EliminarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar eliminar el contrato.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void ImprimirContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar imprimir el contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void ImprimirContratoMaestro_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirContratoMaestro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar imprimir el contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #region CU012
        /// <summary>
        /// Genera el reporte para la impresion del check list con todos los datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void ImprimirChkListEntRec_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirCheckList();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar impmrimir el checklist.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Genra el reporte para la impresión con los datos de cabecera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImprimirCabeceraChkListEntRec_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirCabeceraCheckList();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar imprimir el checklist.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion        
        
        #region SC0020

        protected void btnRegresarConsultaF_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Regresar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar regresar.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        #endregion

		protected void ImprimirPagare_Click(object sender, EventArgs e)
		{
			try
			{
				presentador.ImprimirPagare();
			}
			catch (Exception ex) { this.MostrarMensaje("Inconsistencias al intentar imprimir el pagaré.", ETipoMensajeIU.ERROR, "DetalleContratoFSLUI.btnImprimirPagare_Click: " + ex.Message); }
		}
        #endregion
    }
}