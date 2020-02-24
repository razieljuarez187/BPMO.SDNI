// Esta clase satisface los requerimientos del CU028 - Editar Contrato de Mantenimiento
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.Mantto.SD.UI
{
    public partial class AgregarDocumentosContratoUI : System.Web.UI.Page, IAgregarDocumentosContratoVIS
    {
        #region Atributos
        private AgregarDocumentosContratoPRE presentador;
        private string nombreClase = "AgregarDocumentosContratoUI";
        #endregion

        #region constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new AgregarDocumentosContratoPRE(this, this.ucHerramientas, this.ucCatalogoDocumentos);
                if (!Page.IsPostBack)
                {
                    //asignamos el identificador del user control
                    this.ucCatalogoDocumentos.Identificador = new object().GetHashCode().ToString();
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Propiedades
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
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value) && !string.IsNullOrWhiteSpace(this.hdnContratoID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnContratoID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnContratoID.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
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
        /// Obtiene o estabece el estatus del contrato
        /// </summary>
        public int? EstatusID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value) && !string.IsNullOrWhiteSpace(this.hdnEstatusID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnEstatusID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnEstatusID.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }

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
        #endregion

        #region Métodos
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
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.SD.UI/DetalleContratoSDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.SD.UI/ConsultarContratoSDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        /// <summary>
        /// Actualiza el listado de docuemntos en caso de ocurrir un error
        /// </summary>
        private void ActualizarDocumentos()
        {
            this.ucCatalogoDocumentos.InicializarControl(new List<ArchivoBO>(), new List<TipoArchivoBO>());
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucCatalogoDocumentos.LimpiaCampos();
        }

        /// <summary>
        /// Limpia las variables usadas para la edición de la session
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["ContratoManttoBO"] != null)
                Session.Remove("ContratoManttoBO");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ActualizarDocumentos();
            }
            catch (Exception ex)
            {
                this.ActualizarDocumentos();
                this.MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar la edición del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        #endregion
    }
}