using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class RegistrarContratoROCUI : Page, IRegistrarContratoROCVIS {
        #region Atributos
        /// <summary>
        /// Presentador de la página de registro
        /// </summary>
        private RegistrarContratoROCPRE presentador;
        /// <summary>
        /// Nombre de la página que  se esta desplegando
        /// </summary>
        private const string nombreClase = "RegistrarContratoROCUI";
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
        /// Obtiene o establece el Cliente ID
        /// </summary>
        public int? ClienteID {
            get { return this.ucInformacionGeneralPSL.ClienteID; }
            set { this.ucInformacionGeneralPSL.ClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece el RFC del cliente
        /// </summary>
        public string ClienteRFC {
            get { return this.ucInformacionGeneralPSL.ClienteRFC; }
            set { this.ucInformacionGeneralPSL.ClienteRFC = value; }
        }
        /// <summary>
        /// Obtiene o establece EsFisica
        /// </summary>
        public bool? ClienteEsFisica {
            get { return this.ucInformacionGeneralPSL.ClienteEsFisica; }
            set { this.ucInformacionGeneralPSL.ClienteEsFisica = value; }
        }
        /// <summary>
        /// Obtiene o establece el Numero de Cuenta
        /// </summary>
        public string CuentaClienteNumeroCuenta {
            get { return this.ucInformacionGeneralPSL.CuentaClienteNumeroCuenta; }
            set { this.ucInformacionGeneralPSL.CuentaClienteNumeroCuenta = value; }
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
        /// Obtiene o establece la Dirección completa del cliente
        /// </summary>
        public string ClienteDireccionCompleta {
            get { return this.ucInformacionGeneralPSL.ClienteDireccionCompleta; }
            set { this.ucInformacionGeneralPSL.ClienteDireccionCompleta = value; }
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
        /// Clave Producto/Servicio SAT predeterminada
        /// </summary>
        public string ClaveProductoSATPredeterminado {
            get {
                return (ConfigurationManager.AppSettings["ClaveProductoServicioPSL"] != null && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ClaveProductoServicioPSL"].ToString())
                    ? ConfigurationManager.AppSettings["ClaveProductoServicioPSL"].ToString() : string.Empty);
            }
        }
        /// <summary>
        /// Líneas del Contrato
        /// </summary>
        public List<LineaContratoPSLBO> LineasContrato {
            get { return this.ucInformacionGeneralPSL.LineasContrato; }
            set { this.ucInformacionGeneralPSL.LineasContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece la configuración de días a cobrar
        /// </summary>
        public bool? IncluyeSD {
            get { return this.ucInformacionGeneralPSL.IncluyeSD; }
            set { this.ucInformacionGeneralPSL.IncluyeSD = value; }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accediendo
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }
        #region Campos de Renta con Opcion a Compra
        /// <summary>
        /// Obtiene o establece el Monto Total del Arrendamiento
        /// </summary>
        public decimal? MontoTotalArrendamiento {
            get { return this.ucInformacionGeneralPSL.MontoTotalArrendamiento; }
            set { this.ucInformacionGeneralPSL.MontoTotalArrendamiento = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de promesa de devolución de la unidad
        /// </summary>
        public DateTime? FechaPagoRenta {
            get { return this.ucInformacionGeneralPSL.FechaPagoRenta; }
            set { this.ucInformacionGeneralPSL.FechaPagoRenta = value; }
        }
        /// <summary>
        /// Plazo
        /// </summary>
        public int? Plazo {
            get { return this.ucInformacionGeneralPSL.Plazo; }
            set { this.ucInformacionGeneralPSL.Plazo = value; }
        }
        /// <summary>
        /// Obtiene o establece la Inversion Inicial
        /// </summary>
        public decimal? InversionInicial {
            get { return this.ucInformacionGeneralPSL.InversionInicial; }
            set { this.ucInformacionGeneralPSL.InversionInicial = value; }
        }
        #endregion
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new RegistrarContratoROCPRE(this, this.ucInformacionGeneralPSL, this.ucCatalogoDocumentos, this.ucLineaContratoPSLUI);
                if (!Page.IsPostBack)
                    presentador.PrepararNuevo();
                ucInformacionGeneralPSL.AgregarUnidadClick = AgregarUnidad_Click;
                ucLineaContratoPSLUI.AgregarClick = AgregarLineaContrato_Click;
                ucLineaContratoPSLUI.CancelarClick = CancelarLineaContrato_Click;
                ucInformacionGeneralPSL.RemoverLineaContrato = RemoverLineaContrato_Click;
                ucInformacionGeneralPSL.VerDetalleLineaContrato = VerDetalle_Click;
                ucInformacionGeneralPSL.EliminarTarifaLineasClick = btnEliminarTarifas_Click;
                ucInformacionGeneralPSL.MostrarControlesROC(true);
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
        /// Habilita o deshabilita el botón de guardar borrador
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirGuardarBorrador(bool permitir) {
            this.btnGuardar.Enabled = permitir;
        }
        /// <summary>
        /// Habilita o deshabilita el botón de guardar contrato terminado
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirGuardarTerminado(bool permitir) {
            this.btnTermino.Enabled = permitir;
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
        #endregion

        #region Eventos
        protected void btnActualizarPrevio_Click(object sender, EventArgs e) {
            try {
                string origen = ((Button)sender).ID;

                if (this.presentador.UnidadTieneReservacion())
                    this.RegistrarScript("UnidadTieneReservacion", "confirmarRentaUnidadReservada('" + origen + "');");
                else {
                    switch (origen) {
                        case "btnTerminoPrevio":
                            this.presentador.RegistrarTerminada();
                            break;
                        case "btnGuardarPrevio":
                            this.presentador.RegistrarBorrador();
                            break;
                    }
                }
            } catch (Exception ex) {
                this.ActualizarDocumentos();
                this.MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnActualizarPrevio_Click:" + ex.Message);
            }
        }

        protected void btnTermino_Click(object sender, EventArgs e) {
            try {
                presentador.RegistrarTerminada();
            } catch (Exception ex) {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnTermino_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                presentador.RegistrarBorrador();
            } catch (Exception ex) {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }

        }
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                presentador.CancelarRegistro();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
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
                MostrarMensaje("Inconsistencias al desplegar el detalle de la unidad a rentar.", ETipoMensajeIU.ERROR, nombreClase + ".VerDetalle_Click: " + ex.Message);
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
