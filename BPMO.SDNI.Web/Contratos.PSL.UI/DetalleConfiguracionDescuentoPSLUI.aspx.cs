using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class DetalleConfiguracionDescuentoPSLUI : System.Web.UI.Page, IDetalleConfiguracionDescuentoPSLVIS {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para los errores
        /// </summary>
        private string nombreClase = "DetalleConfiguracionDescuentoPSLUI";
        /// <summary>
        /// Presentador del detalle
        /// </summary>
        private DetalleConfiguracionDescuentoPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o regresa el ID de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null &&
                    master.Adscripcion.UnidadOperativa.Id != null)
                    id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        /// <summary>
        /// Obtiene o regresa el ID del cliente
        /// </summary>
        public int? ClienteID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnClienteID.Value.Trim()))
                    id = int.Parse(hdnClienteID.Value.Trim());
                return id;
            }
            set { hdnClienteID.Value = value != null ? value.ToString() : String.Empty; }
        }
        /// <summary>
        /// Obtiene o regresa el ID del modelo
        /// </summary>
        public int? ModeloID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnModeloID.Value.Trim()))
                    id = int.Parse(hdnModeloID.Value.Trim());
                return id;
            }
            set { hdnModeloID.Value = value != null ? value.ToString() : String.Empty; }
        }
        /// <summary>
        /// Obtiene o regresa el ID del sucursal
        /// </summary>
        public int? SucursalID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnSucursalID.Value.Trim()))
                    id = int.Parse(hdnSucursalID.Value.Trim());
                return id;
            }
            set { hdnSucursalID.Value = value != null ? value.ToString() : String.Empty; }
        }
        /// <summary>
        /// Obtiene o regresa el ID del descuento
        /// </summary>
        public int? ConfiguracionDescuentoID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnConfiguracionDescuentoID.Value.Trim()))
                    id = int.Parse(hdnConfiguracionDescuentoID.Value.Trim());
                return id;
            }
            set { hdnConfiguracionDescuentoID.Value = value != null ? value.ToString() : String.Empty; }
        }
        /// <summary>
        /// Obtiene o regresa el ID del usuario
        /// </summary>
        public int? UsuarioID {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Usuario != null && master.Usuario.Id != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// Obtiene o regresa el nombre del cliente
        /// </summary>
        public string Cliente {
            get {
                return (String.IsNullOrEmpty(this.txtCliente.Text)) ? null : this.txtCliente.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o regresa el nombre del contacto comercial
        /// </summary>
        public string ContactoComercial {
            get {
                return (String.IsNullOrEmpty(this.txtContactoComercial.Text)) ? null : this.txtContactoComercial.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtContactoComercial.Text = value;
                else
                    this.txtContactoComercial.Text = string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o regresa el resultado que esta en la tabla actualmente
        /// </summary>
        public List<ConfiguracionDescuentoBO> Resultado {
            get { return Session["listConfiguracionDescuentoBO"] != null ? Session["listConfiguracionDescuentoBO"] as List<ConfiguracionDescuentoBO> : null; }
            set { Session["listConfiguracionDescuentoBO"] = value; }
        }
        /// <summary>
        /// Obtiene o regresa el índice de la pagina de resultado
        /// </summary>
        public int IndicePaginaResultado {
            get { return this.grvConfiguracionDescuentos.PageIndex; }
            set { this.grvConfiguracionDescuentos.PageIndex = value; }
        }


        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new DetalleConfiguracionDescuentoPSLPRE(this);
                if (!IsPostBack) {
                    presentador.ValidarAcceso();
                    presentador.RealizarPrimeraCarga();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al crear la página", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Actualiza el resultado de la tabla
        /// </summary>
        public void ActualizarResultado() {
            this.grvConfiguracionDescuentos.DataSource = this.Resultado;
            this.grvConfiguracionDescuentos.DataBind();
        }
        /// <summary>
        /// Muestra el mensaje 
        /// </summary>
        /// <param name="mensaje">mensaje a enviar al usuario</param>
        /// <param name="tipo">tipo de mensaje</param>
        /// <param name="detalle">detalle</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Método que bloquea el botón dependiendo del permiso del usuari
        /// </summary>
        /// <param name="activo"></param>
        public void PermitirEditar(bool activo) {
            this.btnEditar.Enabled = activo;

        }
        /// <summary>
        /// Método para redirigir a la pagina de sin acceso 
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Habilita o Deshabilita los inputs en el detalle
        /// </summary>
        /// <param name="activo"></param>
        public void ModoDetalle(bool activo) {
            txtCliente.Enabled = !activo;
            txtContactoComercial.Enabled = !activo;


        }
        /// <summary>
        /// Limpia la sesión del objeto de descuento
        /// </summary>
        public void LimpiarSesion() {
            if (Session["ConfiguracionDescuentoBO"] != null)
                Session.Remove("ConfiguracionDescuentoBO");
        }
        /// <summary>
        /// Obtiene la sesión del objeto de descuento
        /// </summary>
        /// <returns>Sesión de descuento</returns>
        public object ObtenerDatosNavegacion() {
            return (object)Session["ConfiguracionDescuentoBO"];
        }
        /// <summary>
        /// Estable la variable de sesión de descuento
        /// </summary>
        /// <param name="configuracionDescuento">Objeto de descuento</param>
        public void EstablecerDatosNavegacion(object configuracionDescuento) {
            Session["ConfiguracionDescuentoBO"] = configuracionDescuento;
        }
        /// <summary>
        /// Redirige a editar
        /// </summary>
        public void RedirigirAEditar() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/EditarConfiguracionDescuentoPSLUI.aspx"));
        }
        #region SC0024
        /// <summary>
        /// SC0024 Retorna a la página de consulta
        /// </summary>
        public void RegresarAConsultar() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarConfiguracionDescuentoPSLUI.aspx"));
        }

        #endregion


        #endregion

        #region Eventos Botones
        /// <summary>
        /// Evento que se realizara al dar click al botón de editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">evento del botón</param>
        protected void btnEditar_Click(object sender, EventArgs e) {
            try {
                presentador.IrAEditar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al intentar editar el descuento", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Evento que se realizara al dar click al botón de regresar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">evento del botón</param>
        protected void btnRegresar_Click(object sender, EventArgs e) {
            try {
                this.presentador.RetrocederPagina();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnRegresar_Click:" + ex.Message);
            }
        }
        #endregion

        #region Eventos Tabla
        protected void grvConfiguracionDescuentos_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                ConfiguracionDescuentoBO Descuento = (ConfiguracionDescuentoBO)e.Row.DataItem;

                Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                if (labelSucursalNombre != null) {
                    string SucursalNombre = string.Empty;
                    if (Descuento.Sucursal != null)
                        if (Descuento.Sucursal.Nombre != null) {
                            SucursalNombre = Descuento.Sucursal.Nombre;
                        }
                    labelSucursalNombre.Text = SucursalNombre;
                }
                Label labelModeloNombre = e.Row.FindControl("lblModelo") as Label;
                if (labelModeloNombre != null) {
                    string ModeloNombre = string.Empty;
                    if (Descuento.Modelo != null)
                        if (Descuento.Modelo.Nombre != null) {
                            ModeloNombre = Descuento.Modelo.Nombre;
                        }
                    labelModeloNombre.Text = ModeloNombre;
                }

                Label labelFechaInicioNombre = e.Row.FindControl("lblFechaInicio") as Label;
                if (labelFechaInicioNombre != null) {
                    string FechaInicioNombre = string.Empty;
                    if (Descuento.FechaInicio != null)
                        if (Descuento.FechaInicio != null) {

                            DateTime FechaInicio = Convert.ToDateTime(Descuento.FechaInicio);
                            FechaInicioNombre = string.Format(FechaInicio.ToShortDateString(), "dd/mm/aaaa");
                        }
                    labelFechaInicioNombre.Text = FechaInicioNombre;
                }
                Label labelFechaFinNombre = e.Row.FindControl("lblFechaFin") as Label;
                if (labelFechaFinNombre != null) {
                    string FechaFinNombre = string.Empty;
                    if (Descuento.FechaFin != null)
                        if (Descuento.FechaFin != null) {
                            DateTime FechaFin = Convert.ToDateTime(Descuento.FechaFin);
                            FechaFinNombre = string.Format(FechaFin.ToShortDateString(), "dd/mm/aaaa");
                        }
                    labelFechaFinNombre.Text = FechaFinNombre;
                }
                Label labelDescuentoMaximoNombre = e.Row.FindControl("lblMaximoDescuento") as Label;
                if (labelDescuentoMaximoNombre != null) {

                    string DescuentoMaximoNombre = string.Empty;

                    var desc = Math.Round((decimal)Descuento.DescuentoMaximo, 2);

                    if (Descuento.DescuentoMaximo != null)

                        if (Descuento.DescuentoMaximo != null) {

                            DescuentoMaximoNombre = desc.ToString();
                        }

                    labelDescuentoMaximoNombre.Text = DescuentoMaximoNombre;
                }

                CheckBox CheckBoxActivo = e.Row.FindControl("ChkActivo") as CheckBox;
                if (CheckBoxActivo != null) {
                    if (Descuento.Estado != false) {
                        CheckBoxActivo.Checked = true;
                    } else {
                        CheckBoxActivo.Checked = false;
                    }

                }
            }
        }

        protected void grvConfiguracionDescuentos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_PageIndexChanging:" + ex.Message);
            }
        }
        #endregion

    }
}