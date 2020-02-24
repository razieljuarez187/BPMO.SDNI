//Satisface al CU082 - Registrar Movimiento de Flota
using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.UI
{
    public partial class RegistrarAltaUnidadUI : System.Web.UI.Page, IRegistrarAltaUnidadVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private RegistrarAltaUnidadPRE presentador = null;
        /// <summary>
        /// Nombre de la clase que se usará para los mensajes de error
        /// </summary>
        private string nombreClase = "RegistrarAltaUnidadUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del modulo en el que se encuentra trabajando con el fin de obtener las configuraciones necesarias
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
        /// Obtieen eo establece el identificador de la unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int val = 0;
                if (Int32.TryParse(this.hdnUnidadID.Value, out val))
                    return val;
                return null;
            }
            set
            {
                this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }
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
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la empresa
        /// </summary>
        public int? EmpresaID
        {
            get
            {
                int val = 0;
                if (Int32.TryParse(this.hdnEmpresaID.Value, out val))
                    return val;
                return null;
            }
            set
            {
                this.hdnEmpresaID.Value = value.HasValue ? value.Value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la empresa
        /// </summary>
        public string NombreEmpresa
        {
            get
            {
                return (String.IsNullOrEmpty(txtEmpresa.Text)) ? null : this.txtEmpresa.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEmpresa.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                           ? value.Trim().ToUpper()
                                           : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el domicilio de la sucursal
        /// </summary>
        public string DomicilioSucursal
        {
            get { return this.txtDireccionSucursal.Text.Trim().ToUpper(); }
            set
            {
                this.txtDireccionSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad
        /// </summary>
        public string VIN
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoNumSerie.Text)) ? null : this.txtEstaticoNumSerie.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoNumSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string ClaveActivoOracle
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoClaveOracle.Text)) ? null : this.txtEstaticoClaveOracle.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtEstaticoClaveOracle.Text = value;
                else
                    this.txtEstaticoClaveOracle.Text = string.Empty;
            }
        }
        public int? LiderID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtEstaticoIDLeader.Text))
                    id = int.Parse(this.txtEstaticoIDLeader.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoIDLeader.Text = value.ToString();
                else
                    this.txtEstaticoIDLeader.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoNumEconomico.Text)) ? null : this.txtEstaticoNumEconomico.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoNumEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del tipo de unidad
        /// </summary>
        public string TipoUnidadNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoTipoUnidad.Text)) ? null : this.txtEstaticoTipoUnidad.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtEstaticoTipoUnidad.Text = value;
                else
                    this.txtEstaticoTipoUnidad.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del tipo de unidad
        /// </summary>
        public int? TipoUnidadId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTipoUnidadID.Value))
                    id = int.Parse(this.hdnTipoUnidadID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTipoUnidadID.Value = value.ToString();
                else
                    this.hdnTipoUnidadID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad
        /// </summary>
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoModelo.Text)) ? null : this.txtEstaticoModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtEstaticoModelo.Text = value;
                else
                    this.txtEstaticoModelo.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del modelo de la unidad
        /// </summary>
        public int? ModeloId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnModeloID.Value))
                    id = int.Parse(this.hdnModeloID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }

        public int? Anio
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtEstaticoAnio.Text))
                    id = int.Parse(this.txtEstaticoAnio.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoAnio.Text = value.ToString();
                else
                    this.txtEstaticoAnio.Text = string.Empty;
            }
        }
        public DateTime? FechaCompra
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtEstaticoFechaCompra.Text))
                    temp = DateTime.Parse(this.txtEstaticoFechaCompra.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoFechaCompra.Text = value.ToString();
                else
                    this.txtEstaticoFechaCompra.Text = string.Empty;
            }
        }

        public decimal? MontoFactura
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtEstaticoMontoFactura.Text))
                    temp = decimal.Parse(this.txtEstaticoMontoFactura.Text.Trim().Replace(",", "")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoMontoFactura.Text = string.Format("{0:#,##0.0000}", value);//RI0012
                else
                    this.txtEstaticoMontoFactura.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el folio de la factura de compra de la unidad
        /// </summary>
        public string FolioFactura
        {
            get { return this.txtFolioFacturaCompra.Text.Trim().ToUpper(); }
            set { this.txtFolioFacturaCompra.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece las observaciones de alta de la unidad
        /// </summary>
        public string Observaciones
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservaciones.Text)) ? null : this.txtObservaciones.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtObservaciones.Text = value;
                else
                    this.txtObservaciones.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la referencía al objeto
        /// </summary>
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoExpediente"] != null)
                    return Session["UltimoObjetoExpediente"];

                return null;
            }
            set
            {
                Session["UltimoObjetoExpediente"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad esta disponible
        /// </summary>
        public bool? EstaDisponible
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstaDisponible.Value))
                    id = bool.Parse(this.hdnEstaDisponible.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstaDisponible.Value = value.ToString();
                else
                    this.hdnEstaDisponible.Value = string.Empty;

                Image img = this.mFlota.Controls[3].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad se encuentra en un contrato
        /// </summary>
        public bool? EstaEnContrato
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstaEnContrato.Value))
                    id = bool.Parse(this.hdnEstaEnContrato.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstaEnContrato.Value = value.ToString();
                else
                    this.hdnEstaEnContrato.Value = string.Empty;

                Image img = this.mFlota.Controls[4].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        /// <summary>
        /// Obtiene o establece si la unidad cuenta con equipos aliados
        /// </summary>
        public bool? TieneEquipoAliado
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnTieneEquipoAliado.Value))
                    id = bool.Parse(this.hdnTieneEquipoAliado.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTieneEquipoAliado.Value = value.ToString();
                else
                    this.hdnTieneEquipoAliado.Value = string.Empty;

                Image img = this.mFlota.Controls[5].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas de la unidad
        /// </summary>
        public string NumeroPlaca
        {
            get
            {
                var txt = this.mFlota.Controls[6].FindControl("txtValue") as TextBox;
                return (String.IsNullOrEmpty(txt.Text)) ? null : txt.Text.Trim().ToUpper();
            }
            set
            {
                var txt = this.mFlota.Controls[6].FindControl("txtValue") as TextBox;
                txt.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Load de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarAltaUnidadPRE(this);
                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,350);");
                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la página
                    this.presentador.ValidarAcceso();
                    //Se prepara la página para la edición del contrato
                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, string.Format("{0}.Page_Load:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }

        }
        #endregion

        #region Métodos
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }
        /// <summary>
        /// Redirige a la página de permiso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.LimpiarSesion();
            this.LimpiarPaqueteNavegacion("UnidadExpedienteBO");
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Redirige a la página de detalle del expediente de la unidad
        /// </summary>
        public void RedirigirAExpediente()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/DetalleExpedienteUnidadUI.aspx"));
        }

        public void RedirigirAConsultaDeUnidades()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx"), false);
        }

        public void RedirigirAConsultaDeEquipoAliado()
        {
            throw new NotImplementedException();
        }

        public void LimpiarSesion()
        {
            if (Session["UltimoObjetoExpediente"] != null)
                Session.Remove("UltimoObjetoExpediente");
        }

        /// <summary>
        /// Habilita o deshabilita el botón de registro
        /// </summary>
        /// <param name="status">Estatus que se desea aplicar a los controles</param>
        public void PermitirRegistrar(bool status)
        {
            this.btnGuardar.Enabled = status;
        }

        /// <summary>
        /// Reestablece los controles a la configuración inicial
        /// </summary>
        private void ReestablecerControles()
        {
            this.btnCancelar.Enabled = true;
            this.btnGuardar.Enabled = true;
        }

        public void PermitirConsultar(bool status)
        {
            this.hlkConsultar.Enabled = status;
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el movimiento de la sucursal", ETipoMensajeIU.ERROR, string.Format("{0}.btnCancelar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.RealizarAltaUnidadFlota();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar el movimiento de la sucursal", ETipoMensajeIU.ERROR, string.Format("{0}.btnGuardar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion
    }
}