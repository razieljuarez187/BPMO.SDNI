//Satisface al Caso de uso CU005 - Armar Paquetes Facturacion
using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.MapaSitio.UI;
using System.Collections.Generic;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    /// <summary>
    /// Forma que realiza el proceso de configuración, generación y envio de factura
    /// </summary>
    public partial class ConfigurarFacturacionUI : System.Web.UI.Page, IConfigurarFacturacionVIS
    {
        #region Constantes
        /// <summary>
        /// Clave del Guid asignado la instancia de la página
        /// </summary>
        private const String PAGEGUIDINDEX = "__PAGEGUID";

        /// <summary>
        /// Nombre de la variable que se recibe por GET o por POST para obtener el pago que se va a procesar
        /// </summary>
        private const String PAGO_UNIDAD_CONTRATO_ID = "PagoUnidadContratoID";

        /// <summary>
        /// Nombre de la variable que se envia por Sesión para obtener el Pago que se va a procesar
        /// </summary>
        private const String PAGO_UNIDAD_CONTRATO = "CU005_Pago";

        /// <summary>
        /// Nombre de la variable que se recibe por GET o por POST para obtener la página de consulta nueva
        /// </summary>
        private const String PAGINA_CONSULTA_NUEVA = "PaginaConsultaNueva";

        /// <summary>
        /// Nombre de la variable que se recibe por GET o por POST para obtener la página de cancelación
        /// </summary>
        private const String PAGINA_CANCELACION = "PaginaCancelacion";
        #endregion

        #region Atributos
        /// <summary>
        /// Id único global de la instancia del control
        /// </summary>        
        private Guid _GUID;

        /// <summary>
        /// Presentador para la vista que genera la factura 
        /// </summary>
        private ConfigurarFacturacionPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "ConfigurarFacturacionUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la forma de pago por default
        /// </summary>
        public String FormaPagoPorDefault
        {
            get
            {
                String value = ConfigurationManager.AppSettings["FormaPago"];
                if (value == null)
                    value = "CRÉDITO";

                return value;
            }
        }       

        /// <summary>
        /// Obtiene el tipo de tasaCambiariaPorDefault
        /// </summary>
        public String TipoTasaCambiarioPorDefault
        {
            get
            {
                String value = ConfigurationManager.AppSettings["TipoTasaCambiario"];
                if (value == null)
                    value = "User";

                return value;
            }
        }       

        /// <summary>
        /// Obtiene la unidad de medida por default
        /// </summary>
        public int? UnidadMedidaIDPorDefault
        {
            get
            {
                int result = 0;
                String value = ConfigurationManager.AppSettings["UnidadMedidaLineaFacturaID"];
                if (int.TryParse(value, out result))
                    return result;

                return null;
            }
        }

        /// <summary>
        /// Obtiene la forma de pago por default
        /// </summary>
        public String UnidadMedidaPorDefault
        {
            get
            {
                String value = ConfigurationManager.AppSettings["UnidadMedidaLineaFactura"];
                if (value == null)
                    value = "SERVICIO";

                return value;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa la página de consulta nuevo enviada
        /// </summary>
        protected String PaginaConsultaNueva
        {
            get
            {
                String url = this.Request[ConfigurarFacturacionUI.PAGINA_CONSULTA_NUEVA];
                if (String.IsNullOrEmpty(url))
                    url = "~/Facturacion.AplicacionesFacturacion.UI/ConsultarPagosFacturarUI.aspx";

                return url;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa la página de cancelación enviada
        /// </summary>
        protected String PaginaCancelacion
        {
            get
            {
                String url = this.Request["PaginaCancelacion"];
                if (String.IsNullOrEmpty(url))
                    url = "~/Facturacion.AplicacionesFacturacion.UI/ConsultarPagosFacturarUI.aspx";

                return url;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa la dirección para canultar nuevo
        /// </summary>
        protected String CurrentPaginaConsultaNueva
        {
            get
            {
                return this.hdfPaginaConsultarNuevo.Value;
            }
            set
            {
                this.hdfPaginaConsultarNuevo.Value = value;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa la dirección actual de cancelación
        /// </summary>
        protected String CurrentPaginaCancelacion
        {
            get
            {
                return this.hdfPaginaCancelacion.Value;
            }
            set
            {
                this.hdfPaginaCancelacion.Value = value;
            }
        }

        ///<summary>
        ///Obtiene un valor que representa un Id único global de la instancia del control
        ///</summary>
        ///<value>
        ///Objeto GUID con clave única de la instancia
        ///</value>
        internal Guid GUID
        {
            get
            {
                if (this._GUID == Guid.Empty)
                    this.RegisterGuid();

                return this._GUID;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ViewState_Guid {
            get {
                return this.GUID.ToString();
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)this.Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el nombre del usuario de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String UsuarioNombre 
        {
            get
            {
                var masterMsj = (Site)this.Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Usuario;

                return null;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)this.Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        /// <summary>
        /// Codigo de Moneda de la Empresa
        /// </summary>
        public String CodigoMonedaEmpresa
        {
            get
            {
                Site masterMsj = (Site)this.Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null && masterMsj.Adscripcion.UnidadOperativa.Empresa != null
                    && masterMsj.Adscripcion.UnidadOperativa.Empresa.Moneda != null ? masterMsj.Adscripcion.UnidadOperativa.Empresa.Moneda.Codigo : null;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador del pago que se esta solicitando a facturar
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? PagoUnidadContratoID
        {
            get
            {
                int result = 0;
                string nombreSession = String.Format("PAGOID_{0}", this.ViewState_Guid);            

                if (this.Request[ConfigurarFacturacionUI.PAGO_UNIDAD_CONTRATO_ID] != null &&
                   int.TryParse(this.Request[ConfigurarFacturacionUI.PAGO_UNIDAD_CONTRATO_ID], out result))
                {                    
                    this.Session[nombreSession] = result;
                    return result;
                }

                if (this.Session[ConfigurarFacturacionUI.PAGO_UNIDAD_CONTRATO] is PagoUnidadContratoBO)
                {
                    PagoUnidadContratoBO pago = (PagoUnidadContratoBO)this.Session[ConfigurarFacturacionUI.PAGO_UNIDAD_CONTRATO];                  
                    this.Session[nombreSession] = pago.PagoID;
                    return pago.PagoID;
                }

                if (this.Session[nombreSession] is Int32)
                    return (Int32)this.Session[nombreSession];

                return null;
            }
            internal set
            {
                string nombreSession = String.Format("PAGOID_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        public ETipoContrato? TipoPagoUnidadContrato
        {
            get
            {
                ETipoContrato? result = null;
                string nombreSession = String.Format("TIPO_PAGO_{0}", this.ViewState_Guid);

                if(this.Session[ConfigurarFacturacionUI.PAGO_UNIDAD_CONTRATO] is PagoUnidadContratoBO)
                {
                    var pago = (PagoUnidadContratoBO)this.Session[ConfigurarFacturacionUI.PAGO_UNIDAD_CONTRATO];
                    result = pago.ReferenciaContrato.TipoContratoID;
                    //if (pago.GetType() == typeof (PagoUnidadContratoFSLBO))
                    //    result = ((PagoUnidadContratoFSLBO) pago).TipoContrato;
                    //else
                    //{
                    //    if (pago.GetType() == typeof (PagoUnidadContratoRDBO))
                    //        result = ((PagoUnidadContratoRDBO) pago).TipoContrato;
                    //    else
                    //    {
                    //        if(pago.GetType() == typeof(PagoUnidadContratoManttoBO))
                    //            result = ((PagoUnidadContratoManttoBO)pago).TipoContrato;
                    //    }
                    //}

                    this.Session[nombreSession] = result;
                    return result;
                }

                return null;
            }
        }

        /// <summary>
        /// Pago en curso del cual se estar generando la prefactura
        /// </summary>
        private PagoUnidadContratoBO pagoActual;

        /// <summary>
        /// Obtiene o establece un valor que representa el pago en curso del cual se estar generando la prefactura
        /// </summary>
        /// <value>Objeto de tipo PagoUnidadContratoBO</value>
        public PagoUnidadContratoBO PagoActual
        {
            get
            {                
                if (this.pagoActual == null)
                {
                    string nombreSession = String.Format("PAGO_{0}", this.ViewState_Guid);
                    if (this.Session[nombreSession] is PagoUnidadContratoBO)
                        this.pagoActual = (PagoUnidadContratoBO)this.Session[nombreSession];
                }

                return this.pagoActual;
            }
            set
            {
                string nombreSession = String.Format("PAGO_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);

                this.pagoActual = null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la transaccion actual
        /// </summary>
        /// <value>Objeto de tipo TransaccionBO</value>
        public TransaccionBO TransaccionActual
        {
            get
            {
                TransaccionBO objeto = null;
                string nombreSession = String.Format("TRANSACCION_{0}", this.ViewState_Guid);
                if (this.Session[nombreSession] is TransaccionBO)
                    objeto = (TransaccionBO)this.Session[nombreSession];

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("TRANSACCION_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Obtiene un valor que representa la vista de costos adicionales asociada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucCostosAdicionalesVIS</value>
        public IucCostosAdicionalesVIS CostosAdicionalesView
        {
            get { return this.ucCostosAdicionalesUI; }
        }

        /// <summary>
        /// Obtiene un valor que representa la vista de herramientas adicionales asoiada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucHerramientasPrefacturaVIS</value>
        public IucHerramientasPrefacturaVIS HerramientasPrefacturaView
        {
            get { return this.ucHerramientas; }
        }

        /// <summary>
        /// Obtiene un valor que representa la vista de información adicional asociaada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucInformacionAdicionalVIS</value>
        public IucInformacionAdicionalVIS InformacionAdicionalView
        {
            get { return this.ucInformacionAdicionalUI; }
        }

        /// <summary>
        /// Obtiene un valor que representa la vista de información de cabezera asociada a la prefactura.
        /// </summary>
        /// <value>Objeto de tipo IucInformacionCabeceraVIS</value>
        public IucInformacionCabeceraVIS InformacionCabeceraView
        {
            get { return this.ucInformacionCabeceraUI; }
        }

        /// <summary>
        /// Obtiene un valor que representa la vista de lineas de factura asociada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucLineasFacturaVIS</value>
        public IucLineasFacturaVIS LineasFacturaView
        {
            get { return this.ucLineasFacturaUI; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string NombreCliente
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtNombreCliente.Text) && !String.IsNullOrWhiteSpace(this.txtNombreCliente.Text))
                    return this.txtNombreCliente.Text.Trim();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtNombreCliente.Text = value;
                else
                    this.txtNombreCliente.Text = null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de inicio de contrato del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public DateTime? FechaInicio
        {
            get
            {
                DateTime value = DateTime.MinValue;
                if (DateTime.TryParseExact(this.txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out value))
                    return value;

                return null;
            }
            set
            {
                if (value.HasValue)
                    this.txtFechaInicio.Text = value.Value.ToString("dd/MM/yyyy");
                else
                    this.txtFechaInicio.Text = null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la cuenta del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String NumeroCliente
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtCuentaCliente.Text))
                    return this.txtCuentaCliente.Text;

                return null;
            }
            set
            {
                this.txtCuentaCliente.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el sistema origen
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String SistemaOrigen
        {
            get
            {
                String value = ConfigurationManager.AppSettings["SistemaOrigen"];
                if (value == null)
                    value = "Idealease";

                return value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el RFC del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string RFCCliente
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtRFC.Text) && !String.IsNullOrWhiteSpace(this.txtRFC.Text))
                    return this.txtRFC.Text.Trim();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtRFC.Text = value;
                else
                    this.txtRFC.Text = null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la direccion del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string DireccionCliente
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtDireccionCliente.Text) && !String.IsNullOrWhiteSpace(this.txtDireccionCliente.Text))
                    return this.txtDireccionCliente.Text.Trim();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtDireccionCliente.Text = value;
                else
                    this.txtDireccionCliente.Text = null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el usuario generador del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string UsuarioGenerador
        {
            get
            {
                if (!String.IsNullOrEmpty(this.txtUsuarioGenerador.Text) && !String.IsNullOrWhiteSpace(this.txtUsuarioGenerador.Text))
                    return this.txtUsuarioGenerador.Text.Trim();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtUsuarioGenerador.Text = value;
                else
                    this.txtUsuarioGenerador.Text = null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa las observaciones agregadas a la prefactura a agregar
        /// </summary>
        /// <value>
        /// Objecto de tipo String
        /// </value>
        public string Observaciones
        {
            get
            {
                return !String.IsNullOrEmpty(this.txtObservaciones.Text) && !String.IsNullOrWhiteSpace(this.txtObservaciones.Text) ?
                    this.txtObservaciones.Text.Trim() :
                    null;
            }
            set
            {
                this.txtObservaciones.Text = value;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa la página actual visualizada
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? PaginaActual
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnPaginaActual.Value))
                    id = int.Parse(this.hdnPaginaActual.Value.Trim());
                return id;
            }
        }
        #endregion

        #region Constructores
        #endregion        

        #region Métodos

        /// <summary>
        /// Registra la clave única global del control en la página
        /// </summary>
        private void RegisterGuid()
        {
            string hiddenFieldValue = this.Request.Form[ConfigurarFacturacionUI.PAGEGUIDINDEX];
            if (hiddenFieldValue == null)
            {
                this._GUID = Guid.NewGuid();
                hiddenFieldValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._GUID.ToString()));
            }
            else
            {
                string guidValue = Encoding.UTF8.GetString(Convert.FromBase64String(hiddenFieldValue));
                this._GUID = new Guid(guidValue);
            }

            
            ScriptManager.RegisterHiddenField(this, ConfigurarFacturacionUI.PAGEGUIDINDEX, hiddenFieldValue);
        }

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            this.Session[key] = value;
        }

        /// <summary>
        /// Obtiene el valor de un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <returns>Valor de tipo objet dentro del paquete de navegación</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return this.Session[key];
        }

        /// <summary>
        /// Elimina un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (this.Session[key] != null)
                this.Session.Remove(key);
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Obtiene la carpeta raiz donde se encuentra la aplicación
        /// </summary>
        /// <returns>Dirección donde se encuentra la aplicación</returns>
        public string ObtenerCarpetaRaiz()
        {
            return this.Server.MapPath("~");
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                this.hdnTipoMensaje.Value = ((int) tipo).ToString();
                this.hdnMensaje.Value = mensaje;

                string botonID = "";
                botonID = this.btnConfirmarEnviarNoFacturado.UniqueID;

                this.RegistrarScript("Confirm", "abrirConfirmacion('" + mensaje + "', '" + botonID + "');");
            }
            else
            {
                Site masterMsj = (Site) Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Establece si se permite seleccionar la opción de Regresar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para permitir, false para denegar</param>
        public void PermitirRegresar(bool habilitar)
        {
            this.btnAnterior.Enabled = habilitar;
        }

        /// <summary>
        /// Establece si se permite seleccionar la opción de Continuar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para permitir, false para denegar</param>
        public void PermitirContinuar(bool habilitar)
        {
            this.btnContinuar.Enabled = habilitar;
        }

        /// <summary>
        /// Establece si se permite seleccionar la opción de Cancelar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para , false para denegar</param>
        public void PermitirCancelar(bool habilitar)
        {
            this.btnCancelar.Enabled = habilitar;
        }

        /// <summary>
        /// Establece si se permite seleccionar la opción de Terminar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para , false para denegar</param>
        public void PermitirTerminar(bool habilitar)
        {
            this.btnTerminar.Enabled = habilitar;
        }

        /// <summary>
        /// Establece si se permirte capturar datos dentro de una factura
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para , false para denegar</param>
        public void PertimirCapturar(bool habilitar)
        {
            this.ucInformacionCabeceraUI.Enabled = habilitar;
            this.ucLineasFacturaUI.Enabled = habilitar;
            this.ucCostosAdicionalesUI.Enabled = habilitar;
            this.ucInformacionAdicionalUI.Enabled = habilitar;
            this.txtObservaciones.Enabled = habilitar;
        }

        /// <summary>
        /// Establece si se permite ocultar la opción de Continuar
        /// </summary>
        /// <param name="ocultar">Valor booleano para establecer el comportamiento, true para ocultar, false para visualizar</param>
        public void OcultarContinuar(bool ocultar)
        {
            this.btnContinuar.Visible = !ocultar;
        }

        /// <summary>
        /// Establece si se permite ocultar la opción de Terminar
        /// </summary>
        /// <param name="ocultar">Valor booleano para establecer el comportamiento, true para ocultar, false para visualizar</param>
        public void OcultarTerminar(bool ocultar)
        {
            this.btnTerminar.Visible = !ocultar;
        }

        /// <summary>
        /// Establece la página a ser visualizada
        /// </summary>
        /// <param name="numeroPagina">Número de pagina a ser visualizada, comenzando por el número 1</param>
        public void EstablecerPagina(int numeroPagina)
        {
            this.mvCU005.SetActiveView((View)this.mvCU005.FindControl("vwPagina" + numeroPagina.ToString()));
            this.hdnPaginaActual.Value = numeroPagina.ToString();
        }

        /// <summary>
        /// Limpia datos de sesión
        /// </summary>
        private void LimpiarSesion()
        {
            this.PagoUnidadContratoID = null;
            this.PagoActual = null;
            this.TransaccionActual = null;

            this.ucCostosAdicionalesUI.LimpiarSesion();
            this.ucInformacionAdicionalUI.LimpiarSesion();
        }

        /// <summary>
        /// Proceso que prepara la visualización de una nueva factura
        /// </summary>
        public void PrepararNuevo()
        {            
            this.PagoActual = null;
            this.TransaccionActual = null;

            this.ucCostosAdicionalesUI.PrepararNuevo();
            this.ucHerramientas.PrepararNuevo();
            this.ucInformacionAdicionalUI.PrepararNuevo();
            this.ucInformacionCabeceraUI.PrepararNuevo();
            this.ucLineasFacturaUI.PrepararNuevo();

            if (this.PaginaConsultaNueva != null)
                this.CurrentPaginaConsultaNueva = this.PaginaConsultaNueva;

            if (this.PaginaCancelacion != null)
                this.CurrentPaginaCancelacion = this.PaginaCancelacion;
        }    
   
        /// <summary>
        /// Proceso que se realiza para redirigir a la ventana correspondiente para solicitar una nueva consulta
        /// </summary>
        public void RedirigirAConsulta()
        {
            if (!String.IsNullOrEmpty(this.CurrentPaginaConsultaNueva))
                this.Response.Redirect(this.CurrentPaginaConsultaNueva);
        }

        /// <summary>
        /// Proceso que se realiza para redirigir a la ventana correspondiente durante una cancelación
        /// </summary>
        public void RedirigirACancelacion()
        {
            if (!String.IsNullOrEmpty(this.CurrentPaginaCancelacion))
                this.Response.Redirect(this.CurrentPaginaCancelacion);
        }

        public bool UsoCFDIADatos() {
            bool tieneUso = false;
            if (ucLineasFacturaUI.UsoCFDIId != null){
                if (TransaccionActual.UsoCFDI == null) TransaccionActual.UsoCFDI = new UsoCFDIBO();
                TransaccionActual.UsoCFDI.Id = ucLineasFacturaUI.UsoCFDIId;
                TransaccionActual.UsoCFDI.NombreCorto = ucLineasFacturaUI.ClaveUsoCFDI;
                TransaccionActual.UsoCFDI.Nombre = ucLineasFacturaUI.DescripcionUsoCFDI;
                tieneUso = true;
            }
            return tieneUso;
        }
        #endregion
               
        #region Eventos
        /// <summary>
        /// Método delegado para el evento de carga de la página
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ConfigurarFacturacionPRE(this);

                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.presentador.PrepararNuevo();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento FormaPagoChanged del control "ucInformacionCabeceraUI"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void ucInformacionCabeceraUI_FormaPagoChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ValidarFormaPago(true);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de forma de pago", ETipoMensajeIU.ERROR, this.nombreClase + ".ucInformacionCabeceraUI_FormaPagoChanged:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento MonedaChanged del control "ucCostosAdicionalesUI"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void ucCostosAdicionalesUI_MonedaChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ValidarMonedaFacturacion(true);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar la moneda de facturación", ETipoMensajeIU.ERROR, this.nombreClase + ".ucCostosAdicionalesUI_MonedaChanged:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de click del botón "Continuar"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AvanzarPagina();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".btnContinuar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de click del botón "Terminar"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            try
            {
                if(UsoCFDIADatos())
                    this.presentador.RegistrarFacturacion();
                else
                    this.MostrarMensaje("El UsoCFDI es obligatorio.", ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al envio de Factura", ETipoMensajeIU.ERROR, this.nombreClase + ".btnTerminar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de click del botón "Cancelar"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.LimpiarSesion();
                this.presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de click del botón "Anterior"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.RetrocederPagina();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAnterior_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Método delegado para el evento de click del botón "Consulta Otro"
        /// </summary>
        /// <param name="sender">Objeto que desencadeno el evento</param>
        /// <param name="e">Argumento asociados al evento</param>
        protected void btnConsultarOtra_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarOtraFacturacion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, this.nombreClase + ".btnConsultarOtra_Click:" + ex.Message);
            }
        }

        protected void btnConfirmarEnviarNoFacturado_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConfirmarMoverPagoSinCredito();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al Enviar el Pago al Visor", ETipoMensajeIU.ERROR, this.nombreClase + ".btnConfirmarEnviarNoFacturado_OnClick:" + ex.Message);
            }
        }
        #endregion
    }
}
