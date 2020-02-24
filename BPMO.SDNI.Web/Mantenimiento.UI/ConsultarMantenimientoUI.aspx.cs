// Satisface al CU062 - Obtener Orden de Servicio Idealease
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Primitivos.Enumeradores;
using System.Configuration;
using System.Data;
using BPMO.SDNI.Mantenimiento.BO;
using System.Drawing;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.Basicos.BO;
using BPMO.Generales.BR;
using System.Collections;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Consulta Mantenimientos Idealease, al usuario.
    /// </summary>
    public partial class ConsultarMantenimientoUI : System.Web.UI.Page, IConsultarMantenimientoVIS {

        #region Propiedades

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private ConsultarMantenimientoPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarMantenimientoUI).Name;

        /// <summary>
        /// Obtiene o establece un valor que representa un bit para determinar si es un Mantenimiento Unidad o un Mantenimiento Equipo Aliado.
        /// </summary>
        public bool EsUnidad {
            get { return (bool)Session["esUnidad"]; }
            set { Session["esUnidad"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Mantenimiento Unidad Idealease seleccionada.
        /// </summary>
        public MantenimientoUnidadBO MantenimientoUnidadEncontrada {
            get { return Session["unidadEdicionEncontrada"] as MantenimientoUnidadBO; }
            set { Session["unidadEdicionEncontrada"] = value; } 
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Unidad Idealease seleccionada.
        /// </summary>
        public BPMO.SDNI.Equipos.BO.UnidadBO UnidadEncontrada {
            get { return Session["unidadEdicionEncontrada"] as BPMO.SDNI.Equipos.BO.UnidadBO; }
            set { Session["unidadEdicionEncontrada"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Mantenimiento Equipo Aliado Idealease seleccionado.
        /// </summary>
        public MantenimientoEquipoAliadoBO MantenimientoEquipoAliadoEncontrado{
            get { return Session["unidadEdicionEncontrada"] as MantenimientoEquipoAliadoBO; }
            set { Session["unidadEdicionEncontrada"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Equipo Aliado seleccionado.
        /// </summary>
        public EquipoAliadoBO EquipoAliadoEncontrado {
            get { return Session["unidadEdicionEncontrada"] as EquipoAliadoBO; }
            set { Session["unidadEdicionEncontrada"] = value; }
        }
            
            #region Form Búsqueda

                #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;
                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        public string LibroActivos {
            get {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor; 
            }
            set {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }
                #endregion

                #region Buscador

        /// <summary>
        /// Enumerador de Catálogos para el Buscador.
        /// </summary>
        public enum ECatalogoBuscador {
            Unidad,
            Modelo
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Enumerador que contiene los buscadores existentes en la UI.
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo {
            get { return (ECatalogoBuscador)ViewState["BUSQUEDA"]; }
            set { ViewState["BUSQUEDA"] = value; }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI.
        /// </summary>
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Objeto con la información de filtrado para el buscador.
        /// </summary>
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

        /// <summary>
        /// Obtiene o establece un valor que representa el Objeto que fue seleccionado del buscador.
        /// </summary>
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

                #endregion

                #region Filtro Unidad

        /// <summary>
        /// Obtiene o establece un valor que representa el Número de Serie de la Unidad o el Equipo Aliado
        /// </summary>
        public string NumeroVIN {
            get {
                string uiVIN = this.txtVIN.Text;
                if (uiVIN.Trim().Length > 0) {
                    return uiVIN.ToUpper();
                }
                return null;
            }
            set {
                if (value != null) {
                    this.txtVIN.Text = value;
                } else {
                    this.txtVIN.Text = String.Empty;
                }
            }
        }

                #endregion

                #region Filtro Modelo

        /// <summary>
        /// Obtiene o establece un valor que representa el ModeloBO seleccionado.
        /// </summary>
        public ModeloBO ModeloSeleccionado {
            get { return (ModeloBO)Session["modeloSeleccionado"]; }
            set { Session["modeloSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre del Modelo seleccionado.
        /// </summary>
        public string ModeloNombre {
            get {
                string uiModelo = this.txtModelo.Text;
                if (uiModelo.Trim().Length > 0) {
                    return uiModelo.ToUpper();
                }

                return null;
            }
            set {
                if (value != null) {
                    this.txtModelo.Text = value;
                } else {
                    this.txtModelo.Text = String.Empty;
                }
            }
        }

                #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el Número Económico de la Unidad.
        /// </summary>
        public string NumeroEconomico {
            get {
                string uiNumEco = this.txtNumeroEconomico.Text;
                if (uiNumEco.Trim().Length > 0) {
                    return uiNumEco.ToUpper();
                }

                return null;
            }
            set {
                if (value != null) {
                    this.txtNumeroEconomico.Text = value;
                } else {
                    this.txtNumeroEconomico.Text = String.Empty;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Folio de la Orden de Servicio.
        /// </summary>
        public int? FolioOrdenServicio {
            get {
                String uiOrdenServicioId = this.txtNumeroOrdenServicio.Text;
                if (!String.IsNullOrEmpty(uiOrdenServicioId.Trim())) {
                    return int.Parse(uiOrdenServicioId);
                }

                return null;
            }
            set {
                if (value != null) {
                    this.txtNumeroOrdenServicio.Text = value.ToString();
                } else {
                    this.txtNumeroOrdenServicio.Text = String.Empty;
                }
            }
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Obtiene o establece un valor que representa el índice del Mantenimiento Idealease seleccionado.
        /// </summary>
        public int Index {
            get { return Int32.Parse(Session["indexMantenimientoSeleccionado"].ToString()); }
            set { Session["indexMantenimientoSeleccionado"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el listado de Mantenimientos Unidades Idealease encontradas.
        /// </summary>
        public List<MantenimientoUnidadBO> MantenimientosUnidad {
            get { return Session["listaEdicionOrdenesServicio"] as List<MantenimientoUnidadBO>; }
            set { Session["listaEdicionOrdenesServicio"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de los Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease.
        /// </summary>
        public DataSet EquiposAliadosMantenimiento {
            get { return Session["equiposAliadosMantenimiento"] as DataSet; }
            set { Session["equiposAliadosMantenimiento"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el listado de Mantenimientos Equipos Aliados Idealease encontradas.
        /// </summary>
        public List<MantenimientoEquipoAliadoBO> MantenimientosEquipoAliado {
            get { return Session["listaEdicionOrdenesServicio"] as List<MantenimientoEquipoAliadoBO>; }
            set { Session["listaEdicionOrdenesServicio"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de los Mantenimientos Idealease encontrados.
        /// </summary>
        public DataSet DataSource  {
            get { return Session["listMantenimientosBOF"] as DataSet; }
            set { Session["listMantenimientosBOF"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa un diccionario de datos de la Orden de Servicio Idealease seleccionada.
        /// </summary>
        public Dictionary<string, string> MantenimientoToHash {
            get { return Session["mantenimientoHash"] as Dictionary<string, string>; }
            set { Session["mantenimientoHash"] = value; }
        }

            #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConsultarMantenimientoPRE(this);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }

            if (!IsPostBack) {
                this.presentador.PrepararBusqueda();
                LimpiarSesionBusqueda();
            }

            IniciarVariablesSession();
            this.LoadDataSource();
        }

        #endregion

        #region Métodos

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda) {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Desplegar la información del Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

                #endregion

                #region Unidad

        /// <summary>
        /// Activa el llamado al Buscador de Unidades.
        /// </summary>
        /// <param name="evento">Nombre del campo que desencadenó el evento.</param>
        private void DesplegarBusquedaVIN(string evento) { 
            if (txtVIN.Text.Length < 1) {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try {
                this.EjecutaBuscador("EquipoBepensa&hidden=0", ECatalogoBuscador.Unidad);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + evento + ex.Message);
            }
        }

                #endregion

                #region Modelo

        /// <summary>
        /// Activa el llamado al Buscador de Modelos.
        /// </summary>
        private void DesplegarBusquedaModelo() { 
            try {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarModelo_Click" + ex.Message);
            }
        }

                #endregion

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Realiza la vinculación de la lista de Mantenimientos Idealease encontrados con la UI.
        /// </summary>
        private void LoadDataSource() {
            this.gvOrdenesServicio.DataSource = DataSource.Tables[0];
            this.gvOrdenesServicio.DataBind();
        }

        /// <summary>
        /// Construye el diccionario de datos de los Mantenimientos Idealease encontrados.
        /// </summary>
        public void CargarListaMantenimientos() {
            DataSet list = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("NumeroSerie");
            table.Columns.Add("NumeroEconomico");
            table.Columns.Add("Modelo");
            table.Columns.Add("OrdenServicio");
            string numeroSerie = "", numeroEconomico = "", modelo = "", ordenServicio = "";
            if (this.EsUnidad) {
                foreach (MantenimientoUnidadBO mantenimiento in this.MantenimientosUnidad) {
                    BPMO.SDNI.Equipos.BO.UnidadBO unidad = mantenimiento.IngresoUnidad.Unidad;
                    numeroSerie = unidad.NumeroSerie != null ? unidad.NumeroSerie : "";
                    numeroEconomico = unidad.NumeroEconomico != null ? unidad.NumeroEconomico : "";
                    modelo = unidad.Modelo != null && unidad.Modelo.Nombre != null ? unidad.Modelo.Nombre : "" ;
                    ordenServicio = mantenimiento.OrdenServicio.Id != null ? ((int)mantenimiento.OrdenServicio.Id).ToString() : "";
                    DataRow row = table.NewRow();
                    row.ItemArray = new object[] { numeroSerie, numeroEconomico, modelo, ordenServicio };
                    table.Rows.Add(row);
                }
            } else {
                foreach (MantenimientoEquipoAliadoBO mantenimiento in this.MantenimientosEquipoAliado) {
                    EquipoAliadoBO equipoAliado = mantenimiento.IngresoEquipoAliado.EquipoAliado;
                    numeroSerie = equipoAliado.NumeroSerie != null ? equipoAliado.NumeroSerie : "";
                    numeroEconomico = "";
                    modelo = equipoAliado.Modelo != null && equipoAliado.Modelo.Nombre != null ? equipoAliado.Modelo.Nombre : "";
                    ordenServicio = mantenimiento.OrdenServicio.Id != null ? ((int)mantenimiento.OrdenServicio.Id).ToString() : "";
                    DataRow row = table.NewRow();
                    row.ItemArray = new object[] { numeroSerie, numeroEconomico, modelo, ordenServicio };
                    table.Rows.Add(row);
                }
            }
            list.Tables.Add(table);
            this.DataSource = list;
            this.LoadDataSource();
        }

        /// <summary>
        /// Realiza la redirección al visor de Detalle Mantenimiento Idealease.
        /// </summary>
        public void RedirigirAEdicion(){
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/DetalleMantenimientoUI.aspx"));
        }

            #endregion

        /// <summary>
        /// Establece una nueva instancia de la Unidad E-Lider seleccionada, el tipo de Unidad E-Lider y la lista de Mantenimientos Unidad Idealease encontradas.
        /// </summary>
        private void IniciarVariablesSession() {
            if(Session["esUnidad"] == null){
                Session["esUnidad"] = true;
            }
            if(Session["unidadEdicionEncontrada"] == null){
                Session["unidadEdicionEncontrada"] = new BPMO.SDNI.Equipos.BO.UnidadBO();
            }
            if(Session["listaEdicionOrdenesServicio"] == null){
                Session["listaEdicionOrdenesServicio"]= new List<MantenimientoUnidadBO>();
            }
        }

        /// <summary>
        /// Prepara la Vista para una nueva búsqueda.
        /// </summary>
        public void PrepararBusqueda() {
            this.txtVIN.Text = "";
            this.txtNumeroEconomico.Text = "";
            this.txtModelo.Text = "";
            this.txtNumeroOrdenServicio.Text = "";
            this.ModeloSeleccionado = new ModeloBO();
        }

        /// <summary>
        /// Establece una instancia del diccionario de datos del Mantenimiento seleccionado, la lista de Mantenimientos Idealease encontrados y la 
        /// Lista de Mantenimientos Equipos Aliados Idealease del Mantenimiento Unidad Idealease.
        /// </summary>
        public void LimpiarSesion() {
            Session["equiposAliadosMantenimiento"] = null;
            Session["mantenimientoHash"] = null;
            if(DataSource == null) {
                DataSet dataSet = new DataSet();
                DataTable dataTable = new DataTable();
                dataTable.NewRow();
                dataSet.Tables.Add(dataTable);
                this.DataSource = dataSet;
            }
        }

        /// <summary>
        /// Establece una nueva instancia del Tipo de Undad E-Lider, la Unidad E-Lider seleccionada, el índice del Mantenimiento seleccionado
        /// la Lista de Mantenimiento Unidad y Mantenimientos Equipos Aliados Idealease.
        /// </summary>
        private void LimpiarSesionBusqueda() {
            Session["esUnidad"] = null;
            Session["unidadEdicionEncontrada"] = null;
            Session["indexMantenimientoSeleccionado"] = null;
            if (Session["recargarOrdenesServicioIdealease"] == null) {
                Session["listaEdicionOrdenesServicio"] = null;
                Session["listMantenimientosBOF"] = null;
                if(DataSource == null) {
                    DataSet dataSet = new DataSet();
                    DataTable dataTable = new DataTable();
                    dataTable.NewRow();
                    dataSet.Tables.Add(dataTable);
                    this.DataSource = dataSet;
                }
            } else {
                Session["recargarOrdenesServicioIdealease"] = null;
            }            
        }

        /// <summary>
        /// Valida que al menos un campo del Formulario del buscador tenga valor.
        /// </summary>
        /// <returns>Retorna True si todos los campos no tienen valor, en caso contrario retorna False.</returns>
        private bool ValidarForm(){
            return NumeroVIN == null && 
                ModeloNombre == null  &&
                FolioOrdenServicio == null && 
                NumeroEconomico == null;
        }

        /// <summary>
        /// Despliega el Número de Serie, Número Económico y el Modelo del Equipo E-Lider.
        /// </summary>
        public void CargarDatosMantenimientoEncontrado() {
            String numeroEconomico = "", numeroSerie = "", modelo = "";

            if (this.EsUnidad) {
                numeroSerie = UnidadEncontrada.NumeroSerie != null ? UnidadEncontrada.NumeroSerie : "";
                numeroEconomico = UnidadEncontrada.NumeroEconomico != null ? UnidadEncontrada.NumeroEconomico : "";
                modelo = UnidadEncontrada.Modelo != null && UnidadEncontrada.Modelo.Id != null ? UnidadEncontrada.Modelo.Nombre : "";
            } else {
                numeroSerie = EquipoAliadoEncontrado.NumeroSerie != null ? EquipoAliadoEncontrado.NumeroSerie : "";
                numeroEconomico = "";
                modelo = EquipoAliadoEncontrado.Modelo != null && EquipoAliadoEncontrado.Modelo.Id != null ? EquipoAliadoEncontrado.Modelo.Nombre : "";
            }
            
            this.txtNumeroEconomico.Text = numeroEconomico;
            this.txtVIN.Text = numeroSerie;
            this.txtModelo.Text = modelo;
        }

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null) {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Registra un Script en el cliente.
        /// </summary>
        /// <param name="key">Llave del Script.</param>
        /// <param name="script">Script a registrar.</param>
        private void RegistrarScript(string key, string script){
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

            #region Seguridad

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

            #endregion

        #endregion

        #region Eventos

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Establece el Objeto Seleccionado del Buscador.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Modelo:
                        BPMO.Servicio.Catalogos.BO.ModeloBO modeloBO = (BPMO.Servicio.Catalogos.BO.ModeloBO)this.Session_BOSelecto;
                        this.ModeloSeleccionado = modeloBO;
                        this.ModeloNombre = ModeloSeleccionado.Nombre;
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

                #endregion

                #region Filtro Unidad

        /// <summary>
        /// Evento que activa el llamado al Buscador Unidades.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarVin_Click(object sender, EventArgs e) {
            DesplegarBusquedaVIN(".btnBuscarVin_Click");
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador Unidades.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtVIN_TextChanged(object sender, EventArgs e) {
            DesplegarBusquedaVIN(".txtVIN_TextChanged");
        }

                #endregion

                #region Filtro Modelo

        /// <summary>
        /// Evento que activa el llamado al Buscador Modelos.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        public void btnBuscarModelo_Click(object sender, EventArgs e) {
            DesplegarBusquedaModelo();
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador Modelos.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtModelo_TextChanged(object sender, EventArgs e) { 
            try {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtModelo_TextChanged" + ex.Message);
            }
        }

                #endregion

        /// <summary>
        /// Realiza la consulta de Mantenimientos Idealease. Si no se encontraron resultados el sistema despliega un mensaje de error.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void OnClickBuscarUnidades(object sender, EventArgs e) {
            if(!ValidarForm()){
                if (ModeloNombre != null && ModeloNombre.Trim() != null && !ModeloNombre.Equals("")) {
                    if (ModeloSeleccionado == null || ModeloSeleccionado.Id == null) {
                        DesplegarBusquedaModelo();
                        return;
                    }else if(!ModeloNombre.Equals(ModeloSeleccionado.Nombre)){
                        DesplegarBusquedaModelo();
                        return;
                    }
                } else if (ModeloSeleccionado.Id != null && ModeloSeleccionado.Id > 0){
                    ModeloSeleccionado = new ModeloBO();
                }
                //if (FolioOrdenServicio != null && FolioOrdenServicio > 0) {
                //    presentadorFiltroOS.MostrarDatosInterfaz();
                //    this.CargarListaMantenimientos();
                //} else {
                    presentador.MostrarDatosInterfaz();
                //}
            }else{
                MostrarMensaje("Es necesario especificar al menos un filtro de búsqueda", ETipoMensajeIU.ADVERTENCIA);
            }
        }

            #endregion

            #region Grid Resultado Búsqueda

        /// <summary>
        /// Despliega los Mantenimientos Idealease encontrados en su página correspondiente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void grvOrdenesServicio_PageIndexChanging(Object sender, GridViewPageEventArgs e) {
            try {
                gvOrdenesServicio.PageIndex = e.NewPageIndex;
                this.LoadDataSource();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el Mantenimiento Idealease seleccionado, carga sus detalles completos y redirecciona al visor de Detalle Mantenimiento Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void grvOrdenesServicio_RowCommand(Object sender, GridViewCommandEventArgs e) {
            int index;

            try {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    this.Index = index;
                    if (e.CommandName.Trim().Equals("Editar")) {
                        if (EsUnidad) {
                            presentador.ConsultarMantenimientoUnidadCompleto();
                        }else{
                            presentador.ConsultarMantenimientoEquipoCompleto();
                        }
                        RedirigirAEdicion();
                    }
                }
            } catch (Exception er) 
            {
                MostrarMensaje("Error al consultar el mantenimiento", ETipoMensajeIU.ERROR, er.Message);
            }

        }

            #endregion

        #endregion

    }
}