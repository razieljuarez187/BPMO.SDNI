using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class DetalleContratoROCUI : Page, IDetalleContratoROCVIS {
        #region Atributos
        /// <summary>
        /// Presentador de la página de registro
        /// </summary>
        private DetalleContratoROCPRE presentador;
        /// <summary>
        /// Nombre de la página que  se esta desplegando
        /// </summary>
        private const string nombreClase = "DetalleContratoROCUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID {
            get { return this.ucInformacionGeneralPSL.ContratoID; }
            set { this.ucInformacionGeneralPSL.ContratoID = value; }
        }
        /// <summary>
        /// Objeto en sesión del último contrato consultado
        /// </summary>
        public object UltimoObjeto {
            get {
                if (Session["UltimoObjetoContratoRO"] != null)
                    return Session["UltimoObjetoContratoRO"];
                return null;
            }
            set {
                Session["UltimoObjetoContratoRO"] = value;
            }
        }
        /// <summary>
        /// Retorna la fecha de creación del contrato
        /// </summary>
        public DateTime? FC {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// Retorna la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA {
            get { return FC; }
        }
        /// <summary>
        /// Retorna a el usuario que crea el contrato
        /// </summary>
        public int? UC {
            get {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// Retorna el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA {
            get { return UC; }
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
            get {
                return (string.IsNullOrEmpty(this.ucInformacionGeneralPSL.DescripcionProductoServicio)) ? null
                  : this.ucInformacionGeneralPSL.DescripcionProductoServicio.Trim().ToUpper();
            }
            set { this.ucInformacionGeneralPSL.DescripcionProductoServicio = (value != null) ? value.ToUpper() : string.Empty; }
        }

        /// <summary>
        /// Líneas del Contrato
        /// </summary>
        public List<LineaContratoPSLBO> LineasContrato {
            get { return this.ucInformacionGeneralPSL.LineasContrato; }
            set { this.ucInformacionGeneralPSL.LineasContrato = value; }
        }
        public ETipoContrato? TipoContrato {
            get {
                return ucInformacionGeneralPSL.TipoContrato;
            }
            set { ucInformacionGeneralPSL.TipoContrato = value; }
        }
        #region CheckList
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la línea del contrato
        /// </summary>
        public int? LineaContratoID {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnLineaContratoID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnLineaContratoID.Value)
                           ? (Int32.TryParse(this.hdnLineaContratoID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set {
                this.hdnLineaContratoID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece l tipo de check list que se va a registrar
        /// </summary>
        public int? TipoListadoVerificacionPSL {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnTipoListadoVerificacionPSL.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnTipoListadoVerificacionPSL.Value)
                           ? (Int32.TryParse(this.hdnTipoListadoVerificacionPSL.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnTipoListadoVerificacionPSL.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        #endregion
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new DetalleContratoROCPRE(this, this.ucInformacionGeneralPSL, this.ucCatalogoDocumentos, this.ucLineaContratoPSLUI, this.ucHerramientas);
                if (!Page.IsPostBack) {
                    presentador.ValidarAcceso();

                    presentador.RealizarPrimeraCarga();
                }
                ucInformacionGeneralPSL.MostrarControlesROC(true);
                ucLineaContratoPSLUI.CancelarClick = CancelarLineaContrato_Click;
                ucInformacionGeneralPSL.VerDetalleLineaContrato = VerDetalle_Click;
                ucInformacionGeneralPSL.ImprimirChkEntregaRecepcion = ImprimirChkListEntRec_Click;

                #region Asociacion Metodos Handlers
                this.ucHerramientas.IntercambioUnidadContrato = IntercambiarUnidadContrato_Click;
                this.ucHerramientas.RenovarContrato = btnRenovar_Click;
                this.ucHerramientas.CerrarContrato = CerrarContrato_Click;
                this.ucHerramientas.EliminarContrato = EliminarContrato_Click;
                this.ucHerramientas.ImprimirContratoROC = ImprimirContrato_Click;
                this.ucHerramientas.ImprimirPagareROC = ImprimirPagareROC_Click;
                this.ucHerramientas.EditarEnCurso = btnEditarEnCurso_Click;
                this.ucHerramientas.GenerarSolicitudPago = btnGenerarSolicitud_Click;
                #endregion

            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la interfaz para el registro de un nuevo contrato
        /// </summary>
        public void PrepararNuevo() {
            this.ucInformacionGeneralPSL.PrepararNuevo();
            this.ucCatalogoDocumentos.LimpiaCampos();
        }

        /// <summary>
        /// Preparar la interfaz para los documentos
        /// </summary>
        public void PrepararVisualizacion() {
            this.ucCatalogoDocumentos.Identificador = "documentosAdjuntos";

            this.ucCatalogoDocumentos.EstablecerModoEdicion(false);
        }

        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta() {
            Response.Redirect("ConsultarContratoROCUI.aspx", false);
        }
        /// <summary>
        /// Envía al usuario a la página de detalle después del registro del contrato
        /// </summary>
        public void RedirigirADetalles() {
            Response.Redirect("DetalleContratoROCUI.aspx", false);
        }

        /// <summary>
        /// Envía al usuario a la página de intercambio de unidad del contrato
        /// </summary>
        public void RedirigirAIntercambioUnidad() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/IntercambioUnidadPSLUI.aspx"));
        }

        /// <summary>
        /// Establece el Paquete de Navegación para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="value">Valor del paquete</param>
        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }


        /// <summary>
        /// Guarda en la variable de Session un paquete para su posterior consulta
        /// </summary>
        /// <param name="key">Clave del paquete de navegación</param>
        /// <param name="value">Paquete que se desea guardar en la session</param>
        public void EstablecerPaqueteNavegacionReporte(string key, object value) {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
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
        /// Limpia las variables usadas para el registro de la sesión
        /// </summary>
        public void LimpiarSesion() {
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucInformacionGeneralPSL.LimpiarSesion();
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
        /// Actualiza el listado de documentos en caso de ocurrir un error
        /// </summary>
        private void ActualizarDocumentos() {
            this.ucCatalogoDocumentos.InicializarControl(new List<ArchivoBO>(), new List<TipoArchivoBO>());
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucCatalogoDocumentos.LimpiaCampos();
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
        /// Redirige a la pantalla de Edición
        /// </summary>
        public void RedirigirAEdicion() {
            this.RedirigirAEdicion(false);
        }

        /// <summary>
        /// Redirige a la pantalla de Edición
        /// </summary>
        public void RedirigirAEdicion(bool lEnCurso) {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/EditarContratoROCUI.aspx" + (lEnCurso ? "?S=1" : "")));
        }

        /// <summary>
        /// Obtiene los datos de navegación de la sesión
        /// </summary>
        /// <returns>objeto con información de los datos consultados</returns>
        public object ObtenerDatosNavegacion() {
            return Session["ContratoPSLBO"];
        }

        /// <summary>
        /// Redirige a la pantalla de Renovar
        /// </summary>
        public void RedirigirARenovar() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/RenovarContratoROCUI.aspx"));
        }

        /// <summary>
        /// Redirige al cierre de contrato
        /// </summary>
        public void RedirigirACierre() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/CerrarContratoPSLUI.aspx"));
        }

        /// <summary>
        /// Redirige a la página de impresión del reporte
        /// </summary>
        public void RedirigirAImprimirContrato() {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", false);
        }

        #endregion

        #region Eventos

        protected void IntercambiarUnidadContrato_Click(object sender, EventArgs e) {
            try {
                this.presentador.IntercambiarUnidadContrato();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al intentar intercambiar la unidad en el contrato contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e) {
            try {
                presentador.Editar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        protected void btnEditarEnCurso_Click(object sender, EventArgs e) {
            try {
                presentador.Editar(true);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        protected void btnRenovar_Click(object sender, EventArgs e) {
            try {
                presentador.Renovar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, nombreClase + ".btnRenovar_Click:" + ex.Message);
            }
        }

        protected void btnGenerarSolicitud_Click(object sender, EventArgs e) {
            try {
                presentador.GeneraSolicitudPago();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al generar la solicitud de pago", ETipoMensajeIU.ERROR, nombreClase + ".btnGenerarSolicitud_Click:" + ex.Message);
            }
        }

        protected void btnEliminarContratoBorrador_Click(object sender, EventArgs e) {
            try {
                this.presentador.EliminarContrato();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al intentar eliminar el contrato.", ETipoMensajeIU.ERROR, ex.Message);
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
        /// Evento para Ver los Detalles de Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VerDetalle_Click(object sender, CommandEventArgs e) {
            try {
                var linea = e.CommandArgument as LineaContratoPSLBO;
                presentador.PrepararLinea(linea);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar el detalle del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Método disparado desde el evento evento cerrar contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del evento</param>
        protected void CerrarContrato_Click(object sender, EventArgs e) {
            try {
                presentador.CerrarContrato();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar la asignación de la unidad al contrato.", ETipoMensajeIU.ERROR, nombreClase + ".CancelarLineaContrato_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Método disparado desde el evento Eliminar contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del evento</param>
        protected void EliminarContrato_Click(object sender, EventArgs e) {
            try {
                this.txtboxObser.Text = "";
                RegistrarScript(DateTime.Now.Ticks.ToString() + "mnContratos_MenuItemClick.EliminarContrato", "abrirDialogoEliminar();");
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al intentar eliminar el contrato.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Permite habilitar o deshabilitar el botón de registrar
        /// </summary>
        /// <param name="permitir">Indica si se habilita o deshabilita el botón</param>
        public void PermitirRegistrar(bool permitir) {
            this.hlRegistroOrden.Enabled = false;
        }

        /// <summary>
        /// Permite habilitar o deshabilitar el botón de editar
        /// </summary>
        /// <param name="permitir">Indica si se habilita o deshabilita el botón</param>
        public void PermitirEditar(bool permitir) {
            this.btnEditar.Enabled = permitir;
            this.btnEditar.Visible = permitir;
        }

        /// <summary>
        /// Permite habilitar o deshabilitar el botón de generar pago
        /// </summary>
        /// <param name="permitir">Indica si se habilita o deshabilita el botón</param>
        public void PermitirGenerarPago(bool permitir) {
            if (!permitir)
                this.ucHerramientas.OcultarSolicitudPago();
        }

        /// <summary>
        /// Método disparado desde el evento evento cerrar contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del evento</param>
        protected void ImprimirContrato_Click(object sender, EventArgs e) {
            try {
                presentador.ImprimirContratoROC();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al imprimir el Contrato.", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirContrato_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Método disparado para imprimir el pagare
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del evento</param>
        protected void ImprimirPagareROC_Click(object sender, EventArgs e) {
            try {
                presentador.ImprimirPagareROC();
            }
            catch (Exception ex) {
                MostrarMensaje("Inconsistencias al imprimir el Pagaré.", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirPagareROC_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtene la ruta del directorio donde se encuentran los XML de los reportes
        /// </summary>
        /// <returns>Ruta de </returns>
        public XmlDocument ObtenerXmlReporte() {
            try {
                string rutaXml = ConfigurationManager.AppSettings["UbicacionXML"];
                string xmlLayout = "BPMO.SDNI.Reportes.Contrato.Pagare.xml";

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(rutaXml + xmlLayout);

                return xDoc;
            }
            catch (Exception ex) {
                throw new Exception("No se puede cargar la plantilla del reporte: " + ex.Message);
            }
        }
        #region CheckList 
        /// <summary>
        /// Genera el reporte para la impresion del check list con todos los datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void ImprimirChkListEntRec_Click(object sender, CommandEventArgs e) {
            try {
                LineaContratoPSLBO lineaContratoBO = e.CommandArgument as LineaContratoPSLBO;
                LineaContratoID = lineaContratoBO.LineaContratoID;
                if (lineaContratoBO.ListadosVerificacion.Count == 1)
                    TipoListadoVerificacionPSL = 0;
                else
                    TipoListadoVerificacionPSL = 1;

                this.presentador.ImprimirCheckList();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al intentar imprimir el checklist.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        public void RedirigirAImprimir() {
            this.Response.Redirect(this.ResolveUrl("../Buscador.UI/VisorReporteUI.aspx"));
        }
        #endregion CheckList
        #endregion
    }
}
