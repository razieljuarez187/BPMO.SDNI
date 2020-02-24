//Satisface al CU029 - Consultar Contratos de Mantenimiento
//Satisface al caso de uso CU095 - Imprimir Pagaré CM
//Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.PRE;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.Mantto.CM.UI
{
    public partial class DetalleContratoCMUI : System.Web.UI.Page, IDetalleContratoManttoVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Detalle de Contrato de Mantenimiento
        /// </summary>
        private DetalleContratoManttoPRE presentador;

        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleContratoCMUI";

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
        public int? EstatusID
        {
            get { return this.ucContratoUI.EstatusID; }
            set { this.ucContratoUI.EstatusID = value; }
        }
        public DateTime? FC
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFC.Value))
                    temp = DateTime.Parse(this.hdnFC.Value);
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFC.Value = value.Value.ToString();
                else
                    this.hdnFC.Value = string.Empty;
            }
        }
        public int? UC
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }
        public DateTime? FUA
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFUA.Value))
                    temp = DateTime.Parse(this.hdnFUA.Value);
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFUA.Value = value.Value.ToString();
                else
                    this.hdnFUA.Value = string.Empty;
            }
        }
        public int? UUA
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUUA.Value))
                    id = int.Parse(this.hdnUUA.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUUA.Value = value.ToString();
                else
                    this.hdnUUA.Value = string.Empty;
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

        public List<RepresentanteLegalBO> RepresentantesLegales
        {
            get { return this.ucContratoUI.RepresentantesSeleccionados; }
            set 
            { 
                this.ucContratoUI.RepresentantesSeleccionados = value;
                this.ucContratoUI.ActualizarRepresentantesLegales();
            }
        }
        public bool? SoloRepresentantes
        {
            get { return this.ucContratoUI.SoloRepresentantes; }
            set { this.ucContratoUI.SoloRepresentantes = value; }
        }
        public List<ObligadoSolidarioBO> ObligadosSolidarios
        {
            get { return this.ucContratoUI.ObligadosSolidariosSeleccionados; }
            set 
            { 
                this.ucContratoUI.ObligadosSolidariosSeleccionados = value;
                this.ucContratoUI.ActualizarObligadosSolidarios();
            }
        }
        public bool? ObligadosComoAvales
        {
            get { return this.ucContratoUI.ObligadosComoAvales; }
            set { this.ucContratoUI.ObligadosComoAvales = value; }
        }
        public List<AvalBO> Avales
        {
            get { return this.ucContratoUI.AvalesSeleccionados; }
            set 
            { 
                this.ucContratoUI.AvalesSeleccionados = value;
                this.ucContratoUI.ActualizarAvales();
            }
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

        public List<DatoAdicionalAnexoBO> DatosAdicionales
        {
            get { return this.ucContratoUI.DatosAdicionales; }
            set 
            { 
                this.ucContratoUI.DatosAdicionales = value;
                this.ucContratoUI.ActualizarDatosAdicionales();
            }
        }

        public string Observaciones
        {
            get { return this.ucContratoUI.Observaciones; }
            set { this.ucContratoUI.Observaciones = value; }
        }
        public int? DireccionClienteID
        {
            get { return this.ucContratoUI.DireccionClienteID; }
            set { this.ucContratoUI.DireccionClienteID = value; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new DetalleContratoManttoPRE(this, this.ucHerramientas, this.ucContratoUI, this.ucContratoUI, this.ucCatalogoDocumentos);

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();

                }

                this.presentador.CargarPlantillas();

                this.txtObservacionesEliminacion.Attributes.Add("onkeyup", "checkText(this,250);");
                
                this.ucHerramientas.EliminarContrato = this.btnPrepararEliminarContrato_Click;
                this.ucHerramientas.EditarContrato = this.btnEditarContrato_Click;
                this.ucHerramientas.AgregarDocumentos = this.btnAgregarDocumentosContrato_Click;
                this.ucHerramientas.CerrarContrato = this.btnCerrarContrato_Click;
                this.ucHerramientas.CancelarContrato = this.btnCancelarContrato_Click;
                this.ucHerramientas.ImprimirContrato = this.btnImprimirContrato_Click;
                this.ucHerramientas.ImprimirManualOperaciones = this.btnImprimirManualOperaciones_Click;
                this.ucHerramientas.ImprimirAnexoA = this.btnImprimirAnexoA_Click;
                this.ucHerramientas.ImprimirAnexoB = this.btnImprimirAnexoB_Click;
                this.ucHerramientas.ImprimirAnexoC = this.btnImprimirAnexoC_Click;
                this.ucHerramientas.ImprimirTodo = this.btnImprimirTodo_Click;
				this.ucHerramientas.ImprimirPagare = this.btnImprimirPagare_Click;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararVisualizacion()
        {
            this.ucCatalogoDocumentos.Identificador = "documentosAdjuntos";

            this.ucCatalogoDocumentos.EstablecerModoEdicion(false);
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

        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirAEditar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/EditarContratoCMUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/ConsultarContratoCMUI.aspx"));
        }
        public void RedirigirAAgregarDocumentos()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/AgregarDocumentosContratoUI.aspx"));
        }
        public void RedirigirAImprimir()
        {
            this.Response.Redirect(this.ResolveUrl("../Buscador.UI/VisorReporteUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de cancelación del contrato
        /// </summary>
        public void RedirigirACancelacion()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/CancelarContratoCMUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de cierre del contrato
        /// </summary>
        public void RedirigirACierre()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/CerrarContratoCMUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirRegresar(bool permitir)
        {
            this.btnRegresar.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = false;
        }
        public void PermitirEditar(bool permitir)
        {
            if (permitir == false)
                this.ucHerramientas.DeshabilitarOpcionEditar();
        }
        public void PermitirCerrar(bool permitir)
        {
            if (permitir == false)
                this.ucHerramientas.DeshabilitarOpcionCerrar();
        }
        public void PermitirEliminar(bool permitir)
        {
            if (permitir == false)
                this.ucHerramientas.DeshabilitarOpcionBorrar();
        }

        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }

        /// <summary>
        /// Limpia de la sesión los datos de impresión del reporte
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");

            //TODO: La sesión del lastobject no? UltimoObjetoContratoCM
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        protected void btnPrepararEliminarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtObservacionesEliminacion.Text = "";
                RegistrarScript(DateTime.Now.Ticks.ToString() + "mnContratos_MenuItemClick.EliminarContrato", "abrirDialogoEliminar();");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar eliminar el contrato.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEliminarContrato_Click: " + ex.Message);
            }
        }
        protected void btnEditarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EditarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar editar el contrato.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEditarContrato_Click: " + ex.Message);
            }
        }
        protected void btnAgregarDocumentosContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarDocumentos();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar agregar documentos al contrato.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarDocumentosContrato_Click: " + ex.Message);
            }
        }
        protected void btnCerrarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CerrarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar cerrar el contrato", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCerrarContrato_Click: " + ex.Message);
            }
        }
        protected void btnCancelarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar cerrar el contrato", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelarContrato_Click: " + ex.Message);
            }
        }
        protected void btnEliminarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EliminarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar eliminar el contrato.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEliminarContrato_Click: " + ex.Message);
            }
        }
        protected void btnImprimirContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar imprimir el contrato", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirContrato_Click: " + ex.Message);
            }
        }
        protected void btnImprimirManualOperaciones_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirManualOperaciones();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar imprimir el manual de operaciones", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirManualOperaciones_Click: " + ex.Message);
            }
        }
        protected void btnImprimirAnexoA_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirAnexoA();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar imprimir el anexo a.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirAnexoA_Click: " + ex.Message);
            }
        }
        protected void btnImprimirAnexoB_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirAnexoB();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar imprimir el anexo b.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirAnexoB_Click: " + ex.Message);
            }
        }
        protected void btnImprimirAnexoC_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirAnexoC();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar imprimir el anexo c.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirAnexoC_Click: " + ex.Message);
            }
        }
        protected void btnImprimirTodo_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ImprimirTodo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar imprimir el contrato, sus anexos y el manual de operaciones.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirTodo_Click: " + ex.Message);
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
                this.MostrarMensaje("Inconsistencias al intentar regresar.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnRegresar_Click: " + ex.Message);
            }
        }

		protected void btnImprimirPagare_Click(object sender, EventArgs e)
		{
			try
			{
				presentador.ImprimirPagare();
			}
			catch (Exception ex) { this.MostrarMensaje("Inconsistencias al intentar imprimir el pagaré.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnImprimirPagare_Click: " + ex.Message); }
		}
        #endregion
    }
}