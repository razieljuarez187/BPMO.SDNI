// Esta clase satisface los requerimientos del CU028 - Editar Contrato de Mantenimiento
// Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
// Satisface a la solución de la RI0008
using System;
using System.Collections.Generic;
using System.Configuration;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.UI
{
    public partial class EditarContratoCMUI : System.Web.UI.Page, IEditarContratoManttoVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la  página
        /// </summary>
        private EditarContratoManttoPRE presentador = null;
        /// <summary>
        /// Nombre de la clase que se usará para lso mensajes de error
        /// </summary>
        private const string nombreClase = "EditarContratoCMUI";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la página de editar contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new EditarContratoManttoPRE(this, this.ucHerramientas, this.ucucContratoManttoUI, this.ucucContratoManttoUI, this.ucCatalogoDocumentos);
                this.ucucContratoManttoUI.TipoContratoID = this.TipoContrato.HasValue ? this.TipoContrato.Value : (int)ETipoContrato.CM;
                if (!Page.IsPostBack)
                {
                    //identificador único para el user control
                    this.ucCatalogoDocumentos.Identificador = new object().GetHashCode().ToString();
                    //Se valida el acceso a la página
                    this.presentador.ValidarAcceso();
                    //Se prepara la página para la edición del contrato
                    this.presentador.PrepararEdicion();
                    //Cargamos las plantillas definidas para el tipo de contrato
                    this.presentador.CargarPlantillas();
                }
            }
            catch (Exception ex)
            {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, string.Format("{0}.Page_Load:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del modulo en el que se encuentra trabajando con el fin de obtener las configuraciones necesarias para editar el contrato
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
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
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
                           ? masterMsj.Adscripcion.UnidadOperativa.Id
                           : null;
            }
        }
        /// <summary>
        /// Obtiene el nombre de la Unidad Operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public string UnidadOperativaNombre
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Nombre : null;
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal para la cual se esta elaborando el contrato
        /// </summary>
        public int? SucursalID
        {
            get { return this.ucucContratoManttoUI.SucursalID; }
            set { this.ucucContratoManttoUI.SucursalID = value.HasValue ? (int?)value.Value : null; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal en la cual se esta elaborando el contrato
        /// </summary>
        public string SucursalNombre
        {
            get { return ((IucContratoManttoVIS)this.ucucContratoManttoUI).SucursalNombre; }
            set { ((IucContratoManttoVIS)this.ucucContratoManttoUI).SucursalNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get { return this.ucucContratoManttoUI.ContratoID; }
            set { this.ucucContratoManttoUI.ContratoID = value; }
        }
        /// <summary>
        /// Obtiene o establece el número del contrato con el cual se esta trabajando
        /// </summary>
        public string NumeroContrato
        {
            get { return this.ucucContratoManttoUI.NumeroContrato; }
            set { this.ucucContratoManttoUI.NumeroContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de contrato que se esta editando
        /// </summary>
        public int? TipoContrato
        {
            get
            {
                return this.ucucContratoManttoUI.TipoContratoID.HasValue ? this.ucucContratoManttoUI.TipoContratoID.Value : (int)ETipoContrato.CM;
            }
            set { this.ucucContratoManttoUI.TipoContratoID = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador para el cliente al cual se se esta elaborando el contrato
        /// </summary>
        public int? ClienteID
        {
            get { return this.ucucContratoManttoUI.ClienteID; }
            set { this.ucucContratoManttoUI.ClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece si la cuenta del clietne es del tipo de regimén fisicó o no
        /// </summary>
        public bool? CuentaClienteFisica
        {
            get { return this.ucucContratoManttoUI.ClienteEsFisica; }
            set { this.ucucContratoManttoUI.ClienteEsFisica = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la cuenta del cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get { return this.ucucContratoManttoUI.CuentaClienteID; }
            set { this.ucucContratoManttoUI.CuentaClienteID = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la cuenta del cliente
        /// </summary>
        public string CuentaClienteNombre
        {
            get { return this.ucucContratoManttoUI.CuentaClienteNombre; }
            set { this.ucucContratoManttoUI.CuentaClienteNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de la cuenta del cliente
        /// </summary>
        public int? CuentaClienteTipoID
        {
            get { return this.ucucContratoManttoUI.CuentaClienteTipoID; }
            set { this.ucucContratoManttoUI.CuentaClienteTipoID = value; }
        }
        /// <summary>
        /// Obtiene o establece la dirección completa del cliente
        /// </summary>
        public string ClienteDireccionCompleta
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionCompleta; }
            set { this.ucucContratoManttoUI.ClienteDireccionCompleta = value; }
        }
        /// <summary>
        /// Obtiene o establece la calle en la dirección del cliente
        /// </summary>
        public string ClienteDireccionCalle
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionCalle; }
            set { this.ucucContratoManttoUI.ClienteDireccionCalle = value; }
        }
        /// <summary>
        /// Obtiene o establece el código postal en la dirección del cliente
        /// </summary>
        public string ClienteDireccionCodigoPostal
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionCodigoPostal; }
            set { this.ucucContratoManttoUI.ClienteDireccionCodigoPostal = value; }
        }
        /// <summary>
        /// Obtiene o establece el 
        /// </summary>
        public string ClienteDireccionCiudad
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionCiudad; }
            set { this.ucucContratoManttoUI.ClienteDireccionCiudad = value; }
        }
        /// <summary>
        /// Obtiene o establece el estado en la dirección del cliente
        /// </summary>
        public string ClienteDireccionEstado
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionEstado; }
            set { this.ucucContratoManttoUI.ClienteDireccionEstado = value; }
        }
        /// <summary>
        /// Obtiene o establece el municipio en la dirección del cliente
        /// </summary>
        public string ClienteDireccionMunicipio
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionMunicipio; }
            set { this.ucucContratoManttoUI.ClienteDireccionMunicipio = value; }
        }
        /// <summary>
        /// Obtiene o establece el pais en la dirección del cliente
        /// </summary>
        public string ClienteDireccionPais
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionPais; }
            set { this.ucucContratoManttoUI.ClienteDireccionPais = value; }
        }
        /// <summary>
        /// Obtiene o establece la colonia en la dirección del cliente
        /// </summary>
        public string ClienteDireccionColonia
        {
            get { return this.ucucContratoManttoUI.ClienteDireccionColonia; }
            set { this.ucucContratoManttoUI.ClienteDireccionColonia = value; }
        }
        /// <summary>
        /// Obtiene o establece el código de la moneda para el contrato
        /// </summary>
        public string DivisaID
        {
            get { return this.ucucContratoManttoUI.CodigoMoneda; }
            set { this.ucucContratoManttoUI.CodigoMoneda = value; }
        }
        /// <summary>
        /// Obtiene o establece el estatus del contrato que se esta editando
        /// </summary>
        public int? EstatusContrato
        {
            get { return this.ucucContratoManttoUI.EstatusID; }
            set { this.ucucContratoManttoUI.EstatusID = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha del contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            get { return this.ucucContratoManttoUI.FechaContrato; }
            set { this.ucucContratoManttoUI.FechaContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de inicio de contrato
        /// </summary>
        public DateTime? FechaInicioContrato
        {
            get { return this.ucucContratoManttoUI.FechaInicioContrato; }
            set { this.ucucContratoManttoUI.FechaInicioContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece el plazo del contrato
        /// </summary>
        public int? Plazo
        {
            get { return this.ucucContratoManttoUI.Plazo; }
            set { this.ucucContratoManttoUI.Plazo = value; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de fin del contrato
        /// </summary>
        public DateTime? FechaTerminacionContrato
        {
            get { return this.ucucContratoManttoUI.FechaTerminacionContrato; }
            set { this.ucucContratoManttoUI.FechaTerminacionContrato = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre del representante de la empresa
        /// </summary>
        public string RepresentanteEmpresa
        {
            get { return this.ucucContratoManttoUI.RepresentanteEmpresa; }
            set { this.ucucContratoManttoUI.RepresentanteEmpresa = value; }
        }
        /// <summary>
        /// Obtiene o establece si el contrato solo cuenta con representantes legales
        /// </summary>
        public bool? SoloRepresentantes
        {
            get { return this.ucucContratoManttoUI.SoloRepresentantes; }
            set { this.ucucContratoManttoUI.SoloRepresentantes = value; }
        }
        /// <summary>
        /// Obtiene o establece si en el pagaré del contrato los avales son iguales a los obligados solidarios del contrato
        /// </summary>
        public bool? ObligadosComoAvales
        {
            get { return this.ucucContratoManttoUI.ObligadosComoAvales; }
            set { this.ucucContratoManttoUI.ObligadosComoAvales = value; }
        }
        /// <summary>
        /// Obtiene o establece la ubicación de los talleres donde se dará el mantenimiento
        /// </summary>
        public string UbicacionTaller
        {
            get { return this.ucucContratoManttoUI.UbicacionTaller; }
            set { this.ucucContratoManttoUI.UbicacionTaller = value; }
        }
        /// <summary>
        /// Obtiene o establece el deposito en garantia para el contrato
        /// </summary>
        public decimal? DepositoGarantia
        {
            get { return this.ucucContratoManttoUI.DepositoGarantia; }
            set { this.ucucContratoManttoUI.DepositoGarantia = value; }
        }
        /// <summary>
        /// Obtiene o establece la comision por apertura del contrato
        /// </summary>
        public decimal? ComisionApertura
        {
            get { return this.ucucContratoManttoUI.ComisionApertura; }
            set { this.ucucContratoManttoUI.ComisionApertura = value; }
        }
        /// <summary>
        /// Obtiene o establece el repsonsable del seguro de las unidades en el contrato
        /// </summary>
        public int? IncluyeSeguro
        {
            get { return this.ucucContratoManttoUI.IncluyeSeguroID; }
            set { this.ucucContratoManttoUI.IncluyeSeguroID = value; }
        }
        /// <summary>
        /// Obtiene o establece quien es el responsable del lavado de las unidades en el contrato
        /// </summary>
        public int? IncluyeLavado
        {
            get { return this.ucucContratoManttoUI.IncluyeLavadoID; }
            set { this.ucucContratoManttoUI.IncluyeLavadoID = value; }
        }
        /// <summary>
        /// Obtiene o establece quien es el responsable de la pintura y rotulación de las unidades
        /// </summary>
        public int? IncluyePinturaRotulación
        {
            get { return this.ucucContratoManttoUI.IncluyePinturaRotulacionID; }
            set { this.ucucContratoManttoUI.IncluyePinturaRotulacionID = value; }
        }
        /// <summary>
        /// Obtiene o establece quien es el responsable de 
        /// </summary>
        public int? IncluyeLlantas
        {
            get { return this.ucucContratoManttoUI.IncluyeLlantasID; }
            set { this.ucucContratoManttoUI.IncluyeLlantasID = value; }
        }
        /// <summary>
        /// Obtiene o establece la dirección del alamacenaje de las unidades del contrato
        /// </summary>
        public string DireccionAlmacenaje
        {
            get { return this.ucucContratoManttoUI.DireccionAlmacenaje; }
            set { this.ucucContratoManttoUI.DireccionAlmacenaje = value; }
        }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas al contrato
        /// </summary>
        public string Observaciones
        {
            get { return this.ucucContratoManttoUI.Observaciones; }
            set { this.ucucContratoManttoUI.Observaciones = value; }
        }
        /// <summary>
        /// Obtiene o establece a los representantes legales del cliente para el contrato
        /// </summary>
        public List<object> RepresentatesLegales
        {
            get
            {
                return this.ucucContratoManttoUI.RepresentantesSeleccionados != null ? this.ucucContratoManttoUI.RepresentantesSeleccionados.ConvertAll(x => (object)x) : null;
            }
            set
            {
                this.ucucContratoManttoUI.RepresentantesSeleccionados = value != null ? value.ConvertAll(x => (RepresentanteLegalBO)x) : new List<RepresentanteLegalBO>();
                this.ucucContratoManttoUI.ActualizarRepresentantesLegales();
            }
        }
        /// <summary>
        /// Obtiene o establece a los obligados solidarios para el contrato
        /// </summary>
        public List<object> ObligadosSolidarios
        {
            get
            {
                return this.ucucContratoManttoUI.ObligadosSolidariosSeleccionados != null
                           ? this.ucucContratoManttoUI.ObligadosSolidariosSeleccionados.ConvertAll(x => (object)x)
                           : null;
            }
            set
            {
                this.ucucContratoManttoUI.ObligadosSolidariosSeleccionados = value != null
                                                                                 ? value.ConvertAll(
                                                                                     x => (ObligadoSolidarioBO)x)
                                                                                 : new List<ObligadoSolidarioBO>();
                this.ucucContratoManttoUI.ActualizarObligadosSolidarios();
            }
        }
        /// <summary>
        /// Obtiene o establece a los avales para el pagaré correspondiente al contrato
        /// </summary>
        public List<object> Avales
        {
            get
            {
                return this.ucucContratoManttoUI.AvalesSeleccionados != null
                           ? this.ucucContratoManttoUI.AvalesSeleccionados.ConvertAll(x => (object)x)
                           : null;
            }
            set
            {
                this.ucucContratoManttoUI.AvalesSeleccionados = value != null
                                                                    ? value.ConvertAll(x => (AvalBO)x)
                                                                    : new List<AvalBO>();
                this.ucucContratoManttoUI.ActualizarAvales();
            }
        }
        /// <summary>
        /// Obtiene o establece las lineas de contrato
        /// </summary>
        public List<object> LineasContrato
        {
            get
            {
                return this.ucucContratoManttoUI.LineasContrato != null
                           ? this.ucucContratoManttoUI.LineasContrato.ConvertAll(x => (object)x)
                           : null;
            }
            set
            {
                this.ucucContratoManttoUI.LineasContrato = value != null
                                                               ? value.ConvertAll(x => (LineaContratoManttoBO)x)
                                                               : new List<LineaContratoManttoBO>();
                this.ucucContratoManttoUI.ActualizarLineasContrato();
            }
        }
        /// <summary>
        /// Obtiene o establece los datos adiconales agregados al contrato
        /// </summary>
        public List<object> DatosAdicionales
        {
            get
            {
                return this.ucucContratoManttoUI.DatosAdicionales != null
                           ? this.ucucContratoManttoUI.DatosAdicionales.ConvertAll(x => (object)x)
                           : null;
            }
            set
            {
                this.ucucContratoManttoUI.DatosAdicionales = value != null
                                                                 ? value.ConvertAll(x => (DatoAdicionalAnexoBO)x)
                                                                 : new List<DatoAdicionalAnexoBO>();
                this.ucucContratoManttoUI.ActualizarDatosAdicionales();
            }
        }
        /// <summary>
        /// Obtiene o establece la referencía al objeto inicial que se esta editando
        /// </summary>
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoContratoMantto"] != null)
                    return Session["UltimoObjetoContratoMantto"];

                return null;
            }
            set
            {
                Session["UltimoObjetoContratoMantto"] = value;
            }
        }
        /// <summary>
        /// Obtiene la fecha de registro del contrato
        /// </summary>
        public DateTime? FC
        {
            get
            {
                var contrato = (ContratoManttoBO)this.UltimoObjeto;
                return contrato != null ? contrato.FC : null;
            }
        }
        /// <summary>
        /// Obtiene el usuario que creo el contrato
        /// </summary>
        public int? UC
        {
            get
            {
                var contrato = (ContratoManttoBO)this.UltimoObjeto;
                return contrato != null ? contrato.UC : null;
            }
        }
        /// <summary>
        /// Identificador de la Dirección del Cliente
        /// </summary>
        public int? DireccionClienteID
        {
            get { return this.ucucContratoManttoUI.DireccionClienteID; }
            set { this.ucucContratoManttoUI.DireccionClienteID = value; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Borra de la variable de session los paquetes usados en el caso de uso
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("UltimoObjetoContratoMantto");
        }
        /// <summary>
        /// Reestablece la configuración inicial de los controles de guardado en caso de algun error
        /// </summary>
        private void ReestablecerControles()
        {
            this.btnCancelar.Enabled = true;
            this.btnGuardarPrevio.Enabled = false;
            this.btnTerminoPrevio.Enabled = false;
        }
        /// <summary>
        /// Guarda en la variable de session un paquete para su uso en otra página
        /// </summary>
        /// <param name="key">Clave del paquete de navegación</param>
        /// <param name="value">Paquete que se desea subir a la variable de session</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }
        /// <summary>
        /// Recupera un paquete de navegación de la variable de session
        /// </summary>
        /// <param name="key">Clave del paquete de navegación que de desea obtener</param>
        /// <returns>Paquete de navegación recueprado</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) && string.IsNullOrWhiteSpace(key))
                throw new Exception(string.Format("{0}.ObtenerPaqueteNavegacion:{1}{2}", nombreClase, Environment.NewLine, "Es necesario especificar la clave del paquete que se desea recuperar"));

            return Session[key];
        }
        /// <summary>
        /// Elimina de la variable de session un paquete
        /// </summary>
        /// <param name="key">Clave del paquete que se desea eliminar</param>
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) && string.IsNullOrWhiteSpace(key))
                throw new Exception(string.Format("{0}.LimpiarPaqueteNavegacion:{1}{2}", nombreClase, Environment.NewLine, "Es necesario especificar la clave del paquete que se desea eliminar"));

            Session.Remove(key);
        }
        /// <summary>
        /// Obtiene el listado de documentos predeterminados para el contrato
        /// </summary>
        /// <param name="key">Clave del paquete en la session</param>
        /// <returns>Lista con los documentos plantilla</returns>
        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }
        /// <summary>
        /// Habilita o deshabilita el acceso a la página de registro
        /// </summary>
        /// <param name="status">Estatus que se desea aplicar a los controles</param>
        public void PermitirRegistrar(bool status)
        {
            this.hlRegistroOrden.Enabled = status;
        }
        /// <summary>
        /// Habilita o deshailita los controles para el modo borrador
        /// </summary>
        /// <param name="status">Estatus del control</param>
        public void HabilitarModoBorrador(bool status)
        {
            this.btnGuardarPrevio.Enabled = status;
        }
        /// <summary>
        /// Habilita o deshabilita los controles para el modo Termiando
        /// </summary>
        /// <param name="status">Estatus del control</param>
        public void HabilitarModoTerminado(bool status)
        {
            this.btnTerminoPrevio.Enabled = status;
        }
        /// <summary>
        /// Redirige a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/DetalleContratoCMUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de consultar contrato
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/ConsultarContratoCMUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de permiso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.LimpiarSesion();
            this.LimpiarPaqueteNavegacion("ContratoManttoBO");
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }
        /// <summary>
        /// Despliega un mensaje en pantalla
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

        #region RI0008
        /// <summary>
        /// Permite Guardar el Contrato como Terminado (En Curso)
        /// </summary>
        /// <param name="permitir">Indica si se permitira Guardar el Contrato En curso</param>
        public void PermitirGuardarTerminado(bool permitir)
        {
            this.btnTerminoPrevio.Enabled = permitir;
        }
        #endregion
        #endregion

        #region Eventos
        /// <summary>
        /// Guarda el contrato con estatus en borrador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarPrevio_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EditarBorrador();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar el registro del contrato.", ETipoMensajeIU.ERROR, string.Format("{0}.btnGuardarPrevio_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Guarda el contrato con estatus en curso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminoPrevio_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EditarTerminada();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar el registro del contrato.", ETipoMensajeIU.ERROR, string.Format("{0}.btnTerminoPrevio_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Cancela la edición del contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, string.Format("{0}.btnCancelar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion
    }
}