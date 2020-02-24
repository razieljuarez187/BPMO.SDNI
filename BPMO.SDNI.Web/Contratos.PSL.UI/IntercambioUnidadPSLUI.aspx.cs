using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class IntercambioUnidadPSLUI : System.Web.UI.Page, IIntercambioUnidadPSLVIS {
        #region Atributos
        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "IntercambioUnidadPSLUI";

        /// <summary>
        /// Presentador para Intercambiar Unidad
        /// </summary>
        private IntercambioUnidadPSLPRE presentador;

        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador {
            Unidad,
            Modelo
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Unidad Operativa de Configurada
        /// </summary>
        public UnidadOperativaBO UnidadOperativa {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
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
        /// Usuario del Sistema
        /// </summary>
        public UsuarioBO Usuario {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
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
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        public object UltimoObjeto {
            get {
                if (Session["UltimoContratoPSLBO"] != null)
                    return Session["UltimoContratoPSLBO"];

                return null;
            }
            set {
                if (value != null)
                    Session["UltimoContratoPSLBO"] = value;
                else
                    Session.Remove("UltimoContratoPSLBO");
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value))
                    return int.Parse(this.hdnContratoID.Value);
                return null;
            }
            set { this.hdnContratoID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public int? EstatusID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value))
                    return int.Parse(this.hdnEstatusID.Value);
                return null;
            }
            set { this.hdnEstatusID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o estabece el usuario que actualiza por ultima vez el contrato
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
        /// Retorna la fecha de intercambio de la unidad del contrato
        /// </summary>
        public DateTime? FC {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Retorna a el usuario que crea el contrato originalmente
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
        /// Obtiene o estabece la fecha de la ultima modificación del contrato
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

        public int? UnidadID {
            get {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value))
                    return int.Parse(this.hdnUnidadID.Value);
                return null;
            }
            set { this.hdnUnidadID.Value = value != null ? value.ToString() : string.Empty; }
        }
        public int? EquipoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value))
                    return int.Parse(this.hdnEquipoID.Value);
                return null;
            }
            set { this.hdnEquipoID.Value = value != null ? value.ToString() : string.Empty; }
        }

        public DateTime? FechaIntercambio {
            get {
                if (!string.IsNullOrEmpty(this.txtFechaIntercambio.Text) && !string.IsNullOrWhiteSpace(this.txtFechaIntercambio.Text)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaIntercambio.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaIntercambio.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }


        /// <summary>
        /// Ecode de la unidad que tiene el cliente
        /// </summary>
        public string ECodeCliente {
            get { return string.IsNullOrEmpty(txtECODECliente.Text.Trim()) ? null : txtECODECliente.Text.Trim().ToUpper(); }
            set { txtECODECliente.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Modelo de la unidad que tiene el cliente
        /// </summary>
        public string ModeloCliente {
            get { return string.IsNullOrEmpty(txtModeloCliente.Text.Trim()) ? null : txtModeloCliente.Text.Trim().ToUpper(); }
            set { txtModeloCliente.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Horometro de la unidad que tiene el cliente
        /// </summary>
        public string HorometroUnidadCliente {
            get { return string.IsNullOrEmpty(txtHorometroCliente.Text.Trim()) ? null : txtHorometroCliente.Text.Trim().ToUpper(); }
            set { txtHorometroCliente.Text = value ?? string.Empty; }
        }



        /// <summary>
        /// Porcentaje de combustible de la unidad que tiene el cliente
        /// </summary>
        public string PorcentajeUnidadCliente {
            get { return string.IsNullOrEmpty(txtProcentajeUnidadCliente.Text.Trim()) ? null : txtProcentajeUnidadCliente.Text.Trim().ToUpper(); }
            set { txtProcentajeUnidadCliente.Text = value != null ? String.Format("{0:###0.00##}", value) : string.Empty; }

        }

        /// <summary>
        /// Horómetro de la unidad a intercambiar
        /// </summary>
        public string HorometroUnidadIntercambio {
            get { return string.IsNullOrEmpty(txtHorometroIntercambio.Text.Trim()) ? null : txtHorometroIntercambio.Text.Trim().ToUpper(); }
            set { txtHorometroIntercambio.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Porcentaje de combustible de la unidad a intercambiar
        /// </summary>
        public string PorcentajeCombustibleIntercambio {
            get { return string.IsNullOrEmpty(txtPorcentajeIntercambio.Text.Trim()) ? null : txtPorcentajeIntercambio.Text.Trim().ToUpper(); }
            set { txtPorcentajeIntercambio.Text = txtPorcentajeIntercambio.Text = value != null ? String.Format("{0:###0.00##}", value) : string.Empty; }
        }
        /// <summary>
        /// Lista de unidades obtenidas del contrato y línea contrato
        /// </summary>
        public List<UnidadBO> lstUnidades {
            get {
                if (Session["unidades"] != null)
                    return (List<UnidadBO>)Session["unidades"];
                else
                    return new List<UnidadBO>();
            }
            set { Session["unidades"] = value; }
        }

        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usuario autenticado tiene permiso de acceder
        /// </summary>
        public List<SucursalBO> SucursalesAutorizadas {
            get {
                if (Session["SucursalesAutorizadas"] != null)
                    return Session["SucursalesAutorizadas"] as List<SucursalBO>;

                return null;
            }
            set {
                if (value != null && value.Count > 0) {
                    Session["SucursalesAutorizadas"] = value;
                } else {
                    Session.Remove("SucursalesAutorizadas");
                }
            }
        }

        /// <summary>
        /// Numero de Serie (VIN) de la Unidad que se le mandará al cliente
        /// </summary>
        public string NumeroSerie {
            get {
                return txtNumeroSerie.Text.Trim().ToUpper();
            }
            set {
                txtNumeroSerie.Text = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID {
            get {
                int val;
                return !string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value)
                           ? (Int32.TryParse(this.hdnSucursalID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set {
                this.hdnSucursalID.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }

        /// <summary>
        /// Modelo de la unidad que va para el cliente
        /// </summary>
        public string ModeloNombre {
            get { return string.IsNullOrEmpty(txtModelo.Text.Trim()) ? null : txtModelo.Text.Trim().ToUpper(); }
            set { txtModelo.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Ecode de la unidad que va para el cliente
        /// </summary>
        public string ECode {
            get { return string.IsNullOrEmpty(txtEcode.Text.Trim()) ? null : txtEcode.Text.Trim().ToUpper(); }
            set { txtEcode.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Identificador del modelo de la unidad que va para el cliente
        /// </summary>
        public int? ModeloID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnModeloID.Value))
                    id = int.Parse(this.hdnModeloID.Value.Trim());
                return id;
            }
            set {
                if (value != null)
                    this.hdnModeloID.Value = value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }

        public int? IntercambioUnidadID {
            get {
                if (!string.IsNullOrEmpty(this.hdnIntercambioUnidadID.Value))
                    return int.Parse(this.hdnIntercambioUnidadID.Value);
                return null;
            }
            set { this.hdnIntercambioUnidadID.Value = value != null ? value.ToString() : string.Empty; }
        }
        public int? IntercambioEquipoID {
            get {
                if (!string.IsNullOrEmpty(this.hdnntercambioEquipoID.Value))
                    return int.Parse(this.hdnntercambioEquipoID.Value);
                return null;
            }
            set { this.hdnntercambioEquipoID.Value = value != null ? value.ToString() : string.Empty; }
        }

        public ETipoContrato? TipoContrato {
            get {
                ETipoContrato? tipoContrato = null;
                if (this.hdnTipoContrato != null)
                    tipoContrato = (ETipoContrato)Enum.Parse(typeof(ETipoContrato), this.hdnTipoContrato.Value);
                return tipoContrato;
            }
            set {

                if (value == null)
                    this.hdnTipoContrato.Value = string.Empty;
                else
                    this.hdnTipoContrato.Value = ((int)value).ToString();
            }
        }

        #region Propiedades para el Buscador
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto {
            get {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador {
            get {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new IntercambioUnidadPSLPRE(this, this.ucCatalogoDocumentos, this.ucHerramientas);
                if (!Page.IsPostBack) {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                    this.HabilitarBotonTerminar(false);
                }
            } catch (Exception ex) {
                this.ActualizarDocumentos();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Actualiza el listado de documentos en caso de ocurrir un error
        /// </summary>
        private void ActualizarDocumentos() {
            this.ucCatalogoDocumentos.InicializarControl(new List<ArchivoBO>(), new List<TipoArchivoBO>());
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucCatalogoDocumentos.LimpiaCampos();
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

        public void RedirigirACancelar() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleContratoROUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/RegistrarEntregaPSLUI.aspx"), false);
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarContratoROUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirIntercambiar(bool permitir) {

        }

        /// <summary>
        /// Limpia las variables usadas para la edición de la session
        /// </summary>
        public void LimpiarSesion() {
            if (Session["UltimoContratoPSLBO"] != null)
                Session.Remove("UltimoContratoPSLBO");
        }

        /// <summary>
        /// Limpia los campos del buscador
        /// </summary>
        public void LimpiarCampos() {
            this.txtNumeroSerie.Text = "";
            this.txtModelo.Text = "";
            this.txtEcode.Text = "";
            this.ucCatalogoDocumentos.LimpiaCampos();
        }

        /// <summary>
        /// Carga y despliega el listado de series
        /// </summary>
        /// <param name="listado"></param>
        public void CargarSerie(Dictionary<string, object> listadoEquipos) {

            this.ddlUnidadesSerie.Items.Clear();
            this.ddlUnidadesSerie.Items.Add(new ListItem() { Value = "-1", Text = "Seleccionar" });

            foreach (KeyValuePair<string, object> equipo in listadoEquipos)
                this.ddlUnidadesSerie.Items.Add(new ListItem() { Text = equipo.Key, Value = equipo.Value.ToString() });
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null) {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        public void HabilitarBotonTerminar(bool habilitar)
        {
            this.btnGuardar.Enabled = habilitar;
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {

            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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

        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                presentador.CancelarRegistro();
            } catch (Exception ex) {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                presentador.RegistrarIntercambio();
            } catch (Exception ex) {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al intercambiar la undidad.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }

        }

        protected void ddlUnidadesSerie_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                int? valor = null;

                if (this.ddlUnidadesSerie.SelectedIndex > 0) {
                    valor = int.Parse(this.ddlUnidadesSerie.SelectedValue);

                    this.hdnEquipoID.Value = valor.ToString();
                    this.EquipoID = valor;

                    this.presentador.ObtenerDatosUnidad();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al seleccionar la unidad", ETipoMensajeIU.ERROR, nombreClase + ".ddlUnidadesSerie_SelectedIndexChanged:" + ex.Message);
            }
        }

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e) {
            try {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                NumeroSerie = numeroSerie;
                if (NumeroSerie != null)
                    EjecutaBuscador("UnidadIdealeaseSimple", ECatalogoBuscador.Unidad);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }

        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e) {
            try {
                if (!string.IsNullOrWhiteSpace(this.NumeroSerie))
                    EjecutaBuscador("UnidadIdealeaseSimple&hidden=0", ECatalogoBuscador.Unidad);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarUnidad_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModelo_TextChanged(object sender, EventArgs e) {
            try {
                string modeloNombre = this.ModeloNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.ModeloNombre = modeloNombre;
                if (this.ModeloNombre != null) {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtModelo_TextChanged:" + ex.Message);
            }
        }

        /// <summary>
        /// Buscar Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaModelo_Click(object sender, ImageClickEventArgs e) {
            try {
                if (!(string.IsNullOrWhiteSpace(this.ModeloNombre)))
                    this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
                else
                    MostrarMensaje("Ingrese un parámetro de búsqueda.", ETipoMensajeIU.INFORMACION);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaModelo_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Unidad:
                    case ECatalogoBuscador.Modelo:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
    }
}
