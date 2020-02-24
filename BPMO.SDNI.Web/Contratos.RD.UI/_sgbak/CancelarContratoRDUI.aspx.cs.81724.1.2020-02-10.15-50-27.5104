// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using System.Web.UI;

using System.Globalization;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class CancelarContratoRDUI : System.Web.UI.Page, ICancelarContratoRDVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "CancelarContratoRDUI";

        /// <summary>
        /// Presentador para Cancelar Contrato RD
        /// </summary>
        private CancelarContratoRDPRE presentador;
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
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value))
                    return int.Parse(this.hdnContratoID.Value);
                return null;
            }
            set { this.hdnContratoID.Value = value != null ? value.ToString() : string.Empty; }
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

        public DateTime? FechaContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFechaContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaContrato.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaContrato.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public DateTime? FechaPromesaDevolucion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFechaPromesaDevolucion.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaPromesaDevolucion.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaPromesaDevolucion.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaPromesaDevolucion.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        
        public DateTime? FechaDevolucion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFechaDevolucion.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaDevolucion.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaDevolucion.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaDevolucion.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public int? UnidadID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value))
                    return int.Parse(this.hdnUnidadID.Value);
                return null;
            }
            set { this.hdnUnidadID.Value = value != null ? value.ToString() : string.Empty; }
        }
        public int? EquipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value))
                    return int.Parse(this.hdnEquipoID.Value);
                return null;
            }
            set { this.hdnEquipoID.Value = value != null ? value.ToString() : string.Empty; }
        }
        public int? KmRecorrido
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnKmRecorrido.Value))
                    return int.Parse(this.hdnKmRecorrido.Value);
                return null;
            }
            set { this.hdnKmRecorrido.Value = value != null ? value.ToString() : string.Empty; }
        }

        public string ObservacionesCancelacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservacionesCierre.Text)) ? null : this.txtObservacionesCierre.Text.Trim().ToUpper(); 
            }
            set
            {
                this.txtObservacionesCierre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public DateTime? FechaCancelacion
        {
            get
            {
                DateTime? fecha = null;
                TimeSpan? hora = null;

                if (!string.IsNullOrEmpty(this.txtFechaCierre.Text) && !string.IsNullOrWhiteSpace(this.txtFechaCierre.Text))
                {
                    DateTime parse = new DateTime();
                    if (DateTime.TryParse(this.txtFechaCierre.Text, out parse))
                        fecha = parse;

                    if (!string.IsNullOrEmpty(this.txtHoraCierre.Text) && !string.IsNullOrWhiteSpace(this.txtHoraCierre.Text))
                    {
                        var time = DateTime.ParseExact(this.txtHoraCierre.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                        hora = time.TimeOfDay;
                    }
                }

                if (fecha.HasValue && hora.HasValue)
                    fecha = fecha.Value.Add(hora.Value);

                return fecha.HasValue ? fecha : null;
            }
            set
            {
                this.txtFechaCierre.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
                if (value != null && value.Value.TimeOfDay != null)
                {
                    DateTime d = new DateTime().Add(value.Value.TimeOfDay);
                    this.txtHoraCierre.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraCierre.Text = string.Empty;
            }
        }
        public string MotivoCancelacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMotivo.Text)) ? null : this.txtMotivo.Text.Trim().ToUpper();
            }
            set
            {
                this.txtMotivo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new CancelarContratoRDPRE(this, this.ucHerramientas, this.ucResuContratoRD, this.ucDatosGeneralesElementoUI, this.ucEquiposAliadosUnidadUI);
                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }

                this.txtObservacionesCierre.Attributes.Add("onkeyup", "checkText(this,500);");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Metodos
        public void PrepararEdicion()
        {
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

        public void RedirigirACerrar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/CerrarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirCancelar(bool permitir)
        {
            this.txtFechaCierre.Enabled = permitir;
            this.txtMotivo.Enabled = permitir;
            this.txtObservacionesCierre.Enabled = permitir;
            this.btnGuardar.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        /// <summary>
        /// Limpia las variables usadas para la edición de la session
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["UltimoObjetoContratoRD"] != null)
                Session.Remove("UltimoObjetoContratoRD");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null)
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
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar la cancelación del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarContrato();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click: " + ex.Message);
            }

        }
        #endregion
    }
}