//Satisface al CU030 - Registrar Terminación de Contrato de Mantenimiento
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;

using System.Globalization;

using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;

namespace BPMO.SDNI.Contratos.Mantto.CM.UI
{
    public partial class CancelarContratoCMUI : System.Web.UI.Page, ICancelarContratoManttoVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "CancelarContratoCMUI";

        /// <summary>
        /// Presentador para Cancelar Contrato Mantto
        /// </summary>
        private CancelarContratoManttoPRE presentador;

        #endregion

        #region Propiedades

        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

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
        /// <summary>
        /// Obtiene o estable el ultimo objeto
        /// </summary>
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoContratoCM"] != null)
                    return Session["UltimoObjetoContratoCM"];
                return null;
            }
            set
            {
                Session["UltimoObjetoContratoCM"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get { return this.ucContratoUI.ContratoID; }
            set { this.ucContratoUI.ContratoID = value; }
        }
        public string NumeroContrato
        {
            get { return this.ucContratoUI.NumeroContrato; }
            set { this.ucContratoUI.NumeroContrato = value; }
        }
        public int? TipoContratoID
        {
            get { return (int)ETipoContrato.CM; }
            set { }
        }
        /// <summary>
        /// Obtiene o estable el Estatus del Contrato
        /// </summary>
        public int? EstatusID
        {
            get { return this.ucContratoUI.EstatusID; }
            set { this.ucContratoUI.EstatusID = value; }
        }
        /// <summary>
        /// Obtiene o estabece el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnUUA.Value) && !string.IsNullOrWhiteSpace(this.hdnUUA.Value))
                    return int.TryParse(this.hdnUUA.Value, out val) ? (int?)val : null;
                return null;
            }
            set
            {
                this.hdnUUA.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty;
            }
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
            set
            {
                this.hdnFUA.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }
        public string RepresentanteEmpresa
        {
            get { return this.ucContratoUI.RepresentanteEmpresa; }
            set { this.ucContratoUI.RepresentanteEmpresa = value; }
        }
        public int? SucursalID
        {
            get { return this.ucContratoUI.SucursalID; }
            set { this.ucContratoUI.SucursalID = value; }
        }
        public string SucursalNombre
        {
            get { return ((IucContratoManttoVIS)this.ucContratoUI).SucursalNombre; }
            set { ((IucContratoManttoVIS)this.ucContratoUI).SucursalNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha del contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            get { return this.ucContratoUI.FechaContrato; }
            set { this.ucContratoUI.FechaContrato = value; }
        }
        public DateTime? FechaInicioContrato
        {
            get { return this.ucContratoUI.FechaInicioContrato; }
            set { this.ucContratoUI.FechaInicioContrato = value; }
        }
        public DateTime? FechaTerminacionContrato
        {
            get { return this.ucContratoUI.FechaTerminacionContrato; }
            set { this.ucContratoUI.FechaTerminacionContrato = value; }
        }
        public int? Plazo
        {
            get { return this.ucContratoUI.Plazo; }
            set { this.ucContratoUI.Plazo = value; }
        }
        public string CodigoMoneda
        {
            get { return this.ucContratoUI.CodigoMoneda; }
            set { this.ucContratoUI.CodigoMoneda = value; }
        }
        public int? ClienteID
        {
            get { return this.ucContratoUI.ClienteID; }
            set { this.ucContratoUI.ClienteID = value; }
        }
        public bool? ClienteEsFisica
        {
            get { return this.ucContratoUI.ClienteEsFisica; }
            set { this.ucContratoUI.ClienteEsFisica = value; }
        }
        public int? CuentaClienteID
        {
            get { return this.ucContratoUI.CuentaClienteID; }
            set { this.ucContratoUI.CuentaClienteID = value; }
        }
        public string CuentaClienteNombre
        {
            get { return this.ucContratoUI.CuentaClienteNombre; }
            set { this.ucContratoUI.CuentaClienteNombre = value; }
        }
        public int? CuentaClienteTipoID
        {
            get { return this.ucContratoUI.CuentaClienteTipoID; }
            set { this.ucContratoUI.CuentaClienteTipoID = value; }
        }
        public string ClienteDireccionCompleta
        {
            get { return this.ucContratoUI.ClienteDireccionCompleta; }
            set { this.ucContratoUI.ClienteDireccionCompleta = value; }
        }
        public string ClienteDireccionCalle
        {
            get { return this.ucContratoUI.ClienteDireccionCalle; }
            set { this.ucContratoUI.ClienteDireccionCalle = value; }
        }
        public string ClienteDireccionCodigoPostal
        {
            get { return this.ucContratoUI.ClienteDireccionCodigoPostal; }
            set { this.ucContratoUI.ClienteDireccionCodigoPostal = value; }
        }
        public string ClienteDireccionCiudad
        {
            get { return this.ucContratoUI.ClienteDireccionCiudad; }
            set { this.ucContratoUI.ClienteDireccionCiudad = value; }
        }
        public string ClienteDireccionEstado
        {
            get { return this.ucContratoUI.ClienteDireccionEstado; }
            set { this.ucContratoUI.ClienteDireccionEstado = value; }
        }
        public string ClienteDireccionMunicipio
        {
            get { return this.ucContratoUI.ClienteDireccionMunicipio; }
            set { this.ucContratoUI.ClienteDireccionMunicipio = value; }
        }
        public string ClienteDireccionPais
        {
            get { return this.ucContratoUI.ClienteDireccionPais; }
            set { this.ucContratoUI.ClienteDireccionPais = value; }
        }
        public string ClienteDireccionColonia
        {
            get { return this.ucContratoUI.ClienteDireccionColonia; }
            set { this.ucContratoUI.ClienteDireccionColonia = value; }
        }
        public List<LineaContratoManttoBO> LineasContrato
        {
            get { return this.ucContratoUI.LineasContrato; }
            set
            {
                this.ucContratoUI.LineasContrato = value;
                this.ucContratoUI.ActualizarLineasContrato();
            }
        }
        public string UbicacionTaller
        {
            get { return this.ucContratoUI.UbicacionTaller; }
            set { this.ucContratoUI.UbicacionTaller = value; }
        }
        public decimal? DepositoGarantia
        {
            get { return this.ucContratoUI.DepositoGarantia; }
            set { this.ucContratoUI.DepositoGarantia = value; }
        }
        public decimal? ComisionApertura
        {
            get { return this.ucContratoUI.ComisionApertura; }
            set { this.ucContratoUI.ComisionApertura = value; }
        }
        public int? IncluyeSeguroID
        {
            get { return this.ucContratoUI.IncluyeSeguroID; }
            set { this.ucContratoUI.IncluyeSeguroID = value; }
        }
        public int? IncluyeLavadoID
        {
            get { return this.ucContratoUI.IncluyeLavadoID; }
            set { this.ucContratoUI.IncluyeLavadoID = value; }
        }
        public int? IncluyePinturaRotulacionID
        {
            get { return this.ucContratoUI.IncluyePinturaRotulacionID; }
            set { this.ucContratoUI.IncluyePinturaRotulacionID = value; }
        }
        public int? IncluyeLlantasID
        {
            get { return this.ucContratoUI.IncluyeLlantasID; }
            set { this.ucContratoUI.IncluyeLlantasID = value; }
        }
        public string DireccionAlmacenaje
        {
            get { return this.ucContratoUI.DireccionAlmacenaje; }
            set { this.ucContratoUI.DireccionAlmacenaje = value; }
        }
        public string Observaciones
        {
            get { return this.ucContratoUI.Observaciones; }
            set { this.ucContratoUI.Observaciones = value; }
        }
        /// <summary>
        /// Obtiene o establece las observaciones de cancelación del contrato
        /// </summary>
        public string ObservacionesCancelacion
        {
            get
            {
                return (string.IsNullOrEmpty(this.txtObservacionesCierre.Text))
                    ? null
                    : this.txtObservacionesCierre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtObservacionesCierre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                    ? value.Trim().ToUpper()
                    : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece la fecha y hora de cancelación del contrato
        /// </summary>
        public DateTime? FechaCancelacion
        {
            get
            {
                if (string.IsNullOrEmpty(txtFechaCierre.Text.Trim()))
                    return null;
                return Convert.ToDateTime(txtFechaCierre.Text.Trim());
            }
            set
            {
                txtFechaCierre.Text = value == null ? "" : ((DateTime)value).ToShortDateString();
            }
        }
        /// <summary>
        /// Obtiene o establece el motivo de cancelación
        /// </summary>
        public string MotivoCancelacion
        {
            get
            {
                return (string.IsNullOrEmpty(this.txtMotivo.Text)) ? null :
                this.txtMotivo.Text.Trim().ToUpper();
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
                presentador = new CancelarContratoManttoPRE(this, this.ucHerramientas, this.ucContratoUI, this.ucContratoUI);

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la página
                    this.presentador.ValidarAcceso();
                    //Se prepara la página para la cancelación
                    this.presentador.RealizarPrimeraCarga();
                }
                this.txtObservacionesCierre.Attributes.Add("onkeyup", "checkText(this,500);");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void DeshabilitarBotonCancelar()
        {
            this.btnGuardarPrevio.Disabled = true;
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
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/CerrarContratoCMUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/DetalleContratoCMUI.aspx"));
        }

        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/ConsultarContratoCMUI.aspx"));
        }
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
            if (Session["UltimoObjetoContratoCM"] != null)
                Session.Remove("UltimoObjetoContratoCM");
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

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Regresar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar regresar.", ETipoMensajeIU.ERROR, nombreClase + ".btnRegresar_Click: " + ex.Message);
            }
        }
        #endregion
    }
}