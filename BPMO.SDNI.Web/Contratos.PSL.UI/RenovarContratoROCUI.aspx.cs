using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class RenovarContratoROCUI : System.Web.UI.Page, IRenovarContratoROCVIS {
        #region Atributos
        /// <summary>
        /// Presentador de la página de edición
        /// </summary>
        private RenovarContratoROCPRE presentador;
        /// <summary>
        /// Nombre de la página que  se esta desplegando
        /// </summary>
        private const string nombreClase = "RenovarContratoROCUI";
        #endregion

        #region Propiedades
        public object UltimoObjeto {
            get {
                if (Session["UltimoObjetoContratoROC"] != null)
                    return Session["UltimoObjetoContratoROC"];

                return null;
            }
            set {
                Session["UltimoObjetoContratoROC"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID {
            get { return this.ucInformacionGeneralPSL.ContratoID; }
            set { this.ucInformacionGeneralPSL.ContratoID = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de creación del contrato
        /// </summary>
        public DateTime? FC {
            get {
                if (!string.IsNullOrEmpty(this.hdnFC.Value) && !string.IsNullOrWhiteSpace(this.hdnFC.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFC.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFC.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA {
            get {
                if (!string.IsNullOrEmpty(this.hdnFUA.Value) && !string.IsNullOrWhiteSpace(this.hdnFUA.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFUA.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFUA.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el usuario que crea el contrato
        /// </summary>
        public int? UC {
            get {
                if (!string.IsNullOrEmpty(this.hdnUC.Value) && !string.IsNullOrWhiteSpace(this.hdnUC.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUC.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUC.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA {
            get {
                if (!string.IsNullOrEmpty(this.hdnUUA.Value) && !string.IsNullOrWhiteSpace(this.hdnUUA.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUUA.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUUA.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el estatus del contrato
        /// </summary>
        public int? EstatusID {
            get { return ucInformacionGeneralPSL.EstatusID; }
            set { this.ucInformacionGeneralPSL.EstatusID = value; }
        }
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Obtiene la el nombre de la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public string UnidadOperativaNombre {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                    ? masterMsj.Adscripcion.UnidadOperativa.Nombre : string.Empty;
            }
        }
        /// <summary>
        /// Fecha en la que se ejecuta el contrato
        /// </summary>
        public DateTime? FechaContrato {
            get {
                DateTime? date = null;
                if (this.ucInformacionGeneralPSL.FechaContrato.HasValue)
                    date = this.ucInformacionGeneralPSL.FechaContrato;

                return date.HasValue ? date : null;
            }
            set { this.ucInformacionGeneralPSL.FechaContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato
        /// </summary>
        public string NumeroContrato {
            get { return this.ucInformacionGeneralPSL.NumeroContrato; }
            set { this.ucInformacionGeneralPSL.NumeroContrato = value; }
        }
        /// <summary>
        /// Obtiene la sucursal seleccionada
        /// </summary>
        public SucursalBO SucursalSeleccionada {
            get { return this.ucInformacionGeneralPSL.SucursalSeleccionada; }
        }
        /// <summary>
        /// Obtiene o establece el número de cuenta del cliente
        /// </summary>
        public int? CuentaClienteID {
            get { return this.ucInformacionGeneralPSL.CuentaClienteID; }
            set { this.ucInformacionGeneralPSL.CuentaClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del cliente
        /// </summary>
        public string CuentaClienteNombre {
            get { return this.ucInformacionGeneralPSL.CuentaClienteNombre; }
            set { this.ucInformacionGeneralPSL.CuentaClienteNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de cuenta del cliente
        /// </summary>
        public int? CuentaClienteTipoID {
            get { return this.ucInformacionGeneralPSL.CuentaClienteTipoID; }
            set { this.ucInformacionGeneralPSL.CuentaClienteTipoID = value; }
        }
        /// <summary>
        /// Id de la Dirección del Cliente
        /// </summary>
        public int? ClienteDireccionId {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionClienteID; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece la calle de la dirección del cliente
        /// </summary>
        public string ClienteDireccionCalle {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionCalle; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionCalle = value; }
        }
        /// <summary>
        /// Obtiene o establece el código postal de la dirección del cliente
        /// </summary>
        public string ClienteDireccionCodigoPostal {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionCodigoPostal; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionCodigoPostal = value; }
        }
        /// <summary>
        /// Obtiene o establece la ciudad de la dirección del cliente
        /// </summary>
        public string ClienteDireccionCiudad {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionCiudad; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionCiudad = value; }
        }
        /// <summary>
        /// Obtiene o establece el estado para la dirección del cliente
        /// </summary>
        public string ClienteDireccionEstado {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionEstado; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionEstado = value; }
        }
        /// <summary>
        /// Obtiene o establece el municipio para la dirección del cliente
        /// </summary>
        public string ClienteDireccionMunicipio {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionMunicipio; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionMunicipio = value; }
        }
        /// <summary>
        /// Obtiene o establece el país para la dirección del cliente
        /// </summary>
        public string ClienteDireccionPais {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionPais; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionPais = value; }
        }
        /// <summary>
        /// Obtiene o establece la colonia para al dirección del cliente
        /// </summary>
        public string ClienteDireccionColonia {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionColonia; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionColonia = value; }
        }
        /// <summary>
        /// Obtiene o establece los representantes legales seleccionados para el contrato
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesLegales {
            get { return this.ucInformacionGeneralPSL.RepresentantesSeleccionados; }
            set { this.ucInformacionGeneralPSL.RepresentantesSeleccionados = value; }
        }

        /// <summary>
        /// Obtiene o establece si el contrato solo tendrá representantes legales
        /// </summary>
        public bool? SoloRepresentantes {
            get { return ucInformacionGeneralPSL.SoloRepresentantes; }
            set { ucInformacionGeneralPSL.SoloRepresentantes = value; }
        }

        /// <summary>
        /// Obtiene o establece los avales seleccionados para el contrato
        /// </summary>
        public List<AvalBO> Avales {
            get { return this.ucInformacionGeneralPSL.AvalesSeleccionados; }
            set { this.ucInformacionGeneralPSL.AvalesSeleccionados = value; }
        }
        /// <summary>
        /// Código de la moneda seleccionada
        /// </summary>
        public string CodigoMoneda {
            get { return this.ucInformacionGeneralPSL.CodigoMoneda; }
            set { this.ucInformacionGeneralPSL.CodigoMoneda = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de autorización para el pago a crédito
        /// </summary>
        public int? TipoConfirmacionID {
            get { return this.ucInformacionGeneralPSL.TipoConfirmacionID; }
            set { this.ucInformacionGeneralPSL.TipoConfirmacionID = value; }
        }
        /// <summary>
        /// Obtiene o establece la forma de pago del contrato
        /// </summary>
        public int? FormaPagoID {
            get { return this.ucInformacionGeneralPSL.FormaPagoID; }
            set { this.ucInformacionGeneralPSL.FormaPagoID = value; }
        }
        /// <summary>
        /// Obtiene o establece la frecuencia de facturación para el contrato
        /// </summary>
        public int? FrecuenciaFacturacionID {
            get { return this.ucInformacionGeneralPSL.FrecuenciaFacturacionID; }
        }
        /// <summary>
        /// Nombre de la persona que autoriza el pago a crédito
        /// </summary>
        public string AutorizadorTipoConfirmacion {
            get { return this.ucInformacionGeneralPSL.AutorizadorTipoConfirmacion; }
            set { this.ucInformacionGeneralPSL.AutorizadorTipoConfirmacion = value; }
        }
        /// <summary>
        /// Obtiene o establece el autorizador de la orden de compra
        /// </summary>
        public string AutorizadorOrdenCompra {
            get { return this.ucInformacionGeneralPSL.AutorizadorOrdenCompra; }
            set { this.ucInformacionGeneralPSL.AutorizadorOrdenCompra = value; }
        }
        /// <summary>
        /// Obtiene o establece la mercancía que se va a transportar en la unidad
        /// </summary>
        public string MercanciaTransportar {
            get { return this.ucInformacionGeneralPSL.MercanciaTransportar; }
            set { this.ucInformacionGeneralPSL.MercanciaTransportar = value; }
        }
        /// <summary>
        /// Obtiene o establece el destino o área de operación para la unidad
        /// </summary>
        public string DestinoAreaOperacion {
            get { return this.ucInformacionGeneralPSL.DestinoAreaOperacion; }
            set { this.ucInformacionGeneralPSL.DestinoAreaOperacion = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de inicio de arrendamiento
        /// </summary>
        public DateTime? FechaInicioArrendamiento {
            get {
                DateTime? date = null;
                if (this.ucInformacionGeneralPSL.FechaInicioArrendamiento.HasValue)
                    date = this.ucInformacionGeneralPSL.FechaInicioArrendamiento;

                return date.HasValue ? date : null;
            }
            set { this.ucInformacionGeneralPSL.FechaInicioArrendamiento = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPromesaDevolucion {
            get {
                DateTime? date = null;
                if (this.ucInformacionGeneralPSL.FechaPromesaDevolucion.HasValue)
                    date = this.ucInformacionGeneralPSL.FechaPromesaDevolucion;

                return date.HasValue ? date : null;
            }
            set { this.ucInformacionGeneralPSL.FechaPromesaDevolucion = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de inicio actual
        /// </summary>
        public DateTime? FechaInicioActual {
            get {
                DateTime? date = null;
                if (this.ucInformacionGeneralPSL.FechaInicioActual.HasValue)
                    date = this.ucInformacionGeneralPSL.FechaInicioActual;

                return date.HasValue ? date : null;
            }
            set { this.ucInformacionGeneralPSL.FechaInicioActual = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución actual
        /// </summary>
        public DateTime? FechaPromesaActual {
            get {
                DateTime? date = null;
                if (this.ucInformacionGeneralPSL.FechaPromesaActual.HasValue)
                    date = this.ucInformacionGeneralPSL.FechaPromesaActual;

                return date.HasValue ? date : null;
            }
            set { this.ucInformacionGeneralPSL.FechaPromesaActual = value; }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas al contrato
        /// </summary>
        public string Observaciones {
            get { return this.ucInformacionGeneralPSL.Observaciones; }
            set { this.ucInformacionGeneralPSL.Observaciones = value; }
        }
        /// <summary>
        /// Número de días que se cobrarán en la primera Factura
        /// </summary>
        public Int32? DiasFacturar {
            get { return this.ucInformacionGeneralPSL.DiasFacturar; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID {
            get { return this.ucInformacionGeneralPSL.UnidadID; }
            set { this.ucInformacionGeneralPSL.UnidadID = value; }
        }
        /// <summary>
        /// obtiene o establece el identificador del equipo
        /// </summary>
        public int? EquipoID {
            get { return this.ucInformacionGeneralPSL.EquipoID; }
            set { this.ucInformacionGeneralPSL.EquipoID = value; }
        }
        /// <summary>
        /// Identificador de Producto o Servicio (SAT)
        /// </summary>
        public int? ProductoServicioId {
            get { return this.ucInformacionGeneralPSL.ProductoServicioId; }
            set { this.ucInformacionGeneralPSL.ProductoServicioId = value; }
        }
        /// <summary>
        /// Clave de Producto o Servicio (SAT)
        /// </summary>
        public string ClaveProductoServicio {
            get { return this.ucInformacionGeneralPSL.ClaveProductoServicio; }
            set { this.ucInformacionGeneralPSL.ClaveProductoServicio = value; }
        }
        /// <summary>
        /// Descripción de Producto o Servicio (SAT)
        /// </summary>
        public string DescripcionProductoServicio {
            get { return this.ucInformacionGeneralPSL.DescripcionProductoServicio; }
            set { this.ucInformacionGeneralPSL.DescripcionProductoServicio = value; }
        }
        /// <summary>
        /// Líneas del Contrato
        /// </summary>
        public List<LineaContratoPSLBO> LineasContrato {
            get { return this.ucInformacionGeneralPSL.LineasContrato; }
            set { this.ucInformacionGeneralPSL.LineasContrato = value; }
        }
        /// <summary>
        /// Tipo de contrato
        /// </summary>
        public ETipoContrato? TipoContrato {
            get {
                return ucInformacionGeneralPSL.TipoContrato;
            }
            set { ucInformacionGeneralPSL.TipoContrato = value; }
        }
        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new RenovarContratoROCPRE(this, this.ucInformacionGeneralPSL, this.ucCatalogoDocumentos, this.ucHerramientas, this.ucLineaContratoPSLUI);
                if (!Page.IsPostBack) {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();
                    presentador.RealizarPrimeraCarga();
                }
                ucInformacionGeneralPSL.MostrarControlesROC(true);
                ucInformacionGeneralPSL.AgregarUnidadClick = AgregarUnidad_Click;
                ucLineaContratoPSLUI.AgregarClick = AgregarLineaContrato_Click;
                ucLineaContratoPSLUI.CancelarClick = CancelarLineaContrato_Click;
                ucInformacionGeneralPSL.RemoverLineaContrato = RemoverLineaContrato_Click;
                ucInformacionGeneralPSL.VerDetalleLineaContrato = VerDetalle_Click;
                ucInformacionGeneralPSL.EliminarTarifaLineasClick = btnEliminarTarifas_Click;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararRenovacion() {
        }

        /// <summary>
        /// Establece la Sucursal Seleccionada
        /// </summary>
        /// <param name="Id"></param>
        public void EstablecerSucursalSeleccionada(int? Id) {
            this.ucInformacionGeneralPSL.EstablecerSucursalSeleccionada(Id);
        }

        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        /// <summary>
        /// Habilita o deshabilita el botón de guardar contrato renovado
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirGuardarRenovacion(bool permitir) {
            this.btnRenovar.Enabled = permitir;
        }

        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleContratoROCUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarContratoROCUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirRegistrar(bool permitir) {
            this.hlRegistroOrden.Enabled = permitir;
        }

        /// <summary>
        /// Actualiza el listado de documentos en caso de ocurrir un error
        /// </summary>
        private void ActualizarDocumentos() {
            this.ucCatalogoDocumentos.InicializarControl(new List<ArchivoBO>(), new List<TipoArchivoBO>());
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucCatalogoDocumentos.LimpiaCampos();
        }

        /// <summary>
        /// Limpia las variables usadas para la edición de la sesión
        /// </summary>
        public void LimpiarSesion() {
            if (Session["UltimoObjetoContratoRO"] != null)
                Session.Remove("UltimoObjetoContratoRO");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null) {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        /// <summary>
        /// Cambia la la Vista de la interfaz a la información de la línea contrato
        /// </summary>
        public void CambiarALinea() {
            mvCU015.ActiveViewIndex = 1;
        }
        /// <summary>
        /// Cambia la Vista de la interfaz a la información del contrato
        /// </summary>
        public void CambiaAContrato() {
            mvCU015.ActiveViewIndex = 0;
        }
        /// <summary>
        /// Carga el Listado de Tipos de Archivo
        /// </summary>
        /// <param name="tipos"></param>
        public void CargarTiposArchivos(List<TipoArchivoBO> tipos) {
            ucCatalogoDocumentos.TiposArchivo = tipos;
        }
        #endregion

        #region Eventos
        protected void btnRenovar_Click(object sender, EventArgs e) {
            try {
                this.presentador.RenovarTerminada();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnTermino_Click:" + ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                presentador.CancelarEdicion();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento para Ver los Detalles de Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VerDetalle_Click(object sender, CommandEventArgs e) {
            try {
                var linea = e.CommandArgument as LineaContratoPSLBO;
                presentador.PrepararLinea(linea);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar el detalle de la unidad a rentar.", ETipoMensajeIU.ERROR, nombreClase + ".VerDetalle_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Método disparado desde el evento evento AgregarClick del Control ucLineaContratoFSLUI
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del evento</param>
        protected void AgregarLineaContrato_Click(object sender, EventArgs e) {
            try {
                presentador.AgregarLineaContrato();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al agregar la unidad al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".AgregarLineaContrato_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Método disparado desde el evento evento AgregarClick del Control ucLineaContratoFSLUI
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del evento</param>
        protected void CancelarLineaContrato_Click(object sender, EventArgs e) {
            try {
                presentador.CancelarLineaContrato();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar la asignación de la unidad al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".CancelarLineaContrato_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Método Disparado desde el evento AgregarUnidadClick para Agregar una unidad al Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AgregarUnidad_Click(object sender, EventArgs e) {
            try {
                presentador.PrepararNuevaLinea();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al preparar los datos de la unidad a agregar al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".AgregarUnidad_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento para remover un detalle o línea de contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoverLineaContrato_Click(object sender, EventArgs e) {
            try {
                presentador.CalcularTotales();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al remover la unidad del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".RemoverLineaContrato_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Método Disparado desde el evento EliminarTarifaLineasClick para Eliminar tarifas de las líneas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarTarifas_Click(object sender, EventArgs e) {
            try {
                presentador.EliminarTarifaLineasContrato();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al eliminar las tarifas de las líneas.", ETipoMensajeIU.ERROR, nombreClase + ".btnEliminarTarifas_Click: " + ex.Message);
            }
        }
        #endregion
    }
}