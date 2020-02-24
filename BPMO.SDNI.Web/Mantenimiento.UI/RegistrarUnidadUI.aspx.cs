// Satisface al CU001 - Ingresar Unidad a Taller
// Satisface al CU002 - Calcular Proximo Mantenimiento
// Satisface al CU009 - Iniciar Mantenimiento
// Satisface al CU016 - Realizar Auditoria
// Satisface al CU062 - Ver Orden Servicio Idealease
// Satisface al CU064 - Enviar Correo Servicio Realizado
// Satisface el CU063 - Administrar Tareas Pendientes
// Satisface a la SC0002
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using System.Data;
using BPMO.SDNI.Mantenimiento.RPT;
using System.IO;
using DevExpress.XtraPrinting;
using BPMO.SDNI.Mantenimiento.BOF;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Mantenimimento.UI
{
    public partial class RegistrarUnidadUI : System.Web.UI.Page, IRegisitrarUnidadVIS
    {
        #region Propiedades

        private RegistrarUnidadPRE presentador = null;
        private SincronizaEstatusPRE sincronizarEstatusPre = null;
        private string nombreClase = "RegistrarUnidadUI";
        
        /// <summary>
        /// Enum de buscador general
        /// </summary>
        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal,
            Cliente,
            Modelo
        }
        
        /// <summary>
        /// Enum para especificar el tipo de servicio
        /// </summary>
        public enum ETipoServicio
        {
            Unidad,
            EquipoAliado
        }

        #region Filtros Busqueda

        public int? SucursalID
        {
            get
            {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        public string SucursalNombre
        {
            get
            {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

        public int? TallerID
        {
            get
            {
                int? tallerID = null;
                if (!String.IsNullOrEmpty(this.ddTalleres.SelectedValue))
                    tallerID = int.Parse(this.ddTalleres.SelectedValue);
                return tallerID;
            }
        }

        /// <summary>
        /// Unidad Operativa donde se registrará la OS
        /// </summary>
        public int? UnidadOperativaDestinoID {
            get {
                int? uoID = null;
                if (!String.IsNullOrEmpty(this.ddTalleres.SelectedValue)) {
                    uoID = this.Talleres[this.ddTalleres.SelectedIndex].UnidadOperativa.Id;
                }
                return uoID;
            }
        }

        /// <summary>
        /// Sucursal donde se registrará la OS
        /// </summary>
        public int? SucursalDestinoID {
            get {
                int? sucID = null;
                if (!String.IsNullOrEmpty(this.ddTalleres.SelectedValue)) {
                    sucID = this.Talleres[this.ddTalleres.SelectedIndex].Sucursal.Id;
                }
                return sucID;
            }
        }

        public int? Estatus
        {
            get
            {
                int? estatus = null;
                if (!String.IsNullOrEmpty(this.ddEstatus.SelectedValue))
                    estatus = int.Parse(this.ddEstatus.SelectedValue);
                return estatus;
            }
        }
        // Identificador de Orden Servicio
        public int? OrdenServicioID {
            get {
                int id;
                bool esNumero = int.TryParse(this.txtOrdenServicio.Text.Trim(), out id);
                if (esNumero == false) {
                    return null;
                }
                return id;
            }
        }
        // Número de Serie Unidad / VIN
        public string NumeroSerie {
            get {
                return (String.IsNullOrWhiteSpace(this.txtNumeroSerie.Text)) ? null : this.txtNumeroSerie.Text.Trim().ToUpper();
            }
        }

        public DateTime? FechaInicio
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicio.Text))
                {
                    var dateTemp = DateTime.Parse(this.txtFechaInicio.Text);
                    fecha = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, 0, 0, 0);
                }

                return fecha;
            }
        }

        public DateTime? FechaFin
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaFin.Text))
                {
                    var dateTemp = DateTime.Parse(this.txtFechaFin.Text);
                    fecha = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, 23, 59, 59);
                }
                return fecha;
            }
        }

        #endregion

        #region Buscador General

        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }

        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set
            {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }

        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }

        public ETipoServicio StateOrdenServicio
        {
            get
            {
                return (ETipoServicio)Session["TIPOORDENSERVICIO"];
            }
            set
            {
                Session["TIPOORDENSERVICIO"] = value;
            }
        }

        #endregion

        #region Formulario Orden Servicio
        public string DescripcionFalla { get { return this.txtDescripcionFalla.Text; } }

        public int CombustibleEntrada { get { return !string.IsNullOrEmpty(this.txtCombustible.Text) ? int.Parse(this.txtCombustible.Text) : 0; } }

        public int CombustibleSalida { get { return !string.IsNullOrEmpty(this.txtCombustibleSalida.Text) ? int.Parse(this.txtCombustibleSalida.Text) : 0; } }

        public int CombustibleTotal { get { return !string.IsNullOrEmpty(this.txtCombustibleTotal.Text) ? int.Parse(this.txtCombustibleTotal.Text) : 0; } }

        public string Inventario { get { return this.txtInventario.Text; } }

        public string CodigosFalla { get { return this.txtCodigoFalla.Text; } }

        public string MotivoCancelacion { get { return this.txtMotivoCancelacion.Text; } }
        
        #endregion
        
        public List<MantenimientoBOF> Mantenimientos
        {
            get { return Session["listMantenimientos"] as List<MantenimientoBOF>; }
            set { Session["listMantenimientos"] = value; }
        }

        /// <summary>
        /// Lista de Tareas Pendientes de
        /// </summary>
        public List<TareaPendienteBO> ListaTareasPendientes {
            get {
                return (Session["lstTareasPendientes"] == null) ? null : (List<TareaPendienteBO>)Session["lstTareasPendientes"];
            }
            set {
                if (value != null) Session["lstTareasPendientes"] = value;
                else Session.Remove("lstTareasPendientes");
            }
        }

        public List<UnidadBO> ResultadoBusquedaUnidades
        {
            get { return Session["listResultadoBusquedaUnidades"] as List<UnidadBO>; }
            set { Session["listResultadoBusquedaUnidades"] = value; }
        }

        public List<BPMO.Servicio.Catalogos.BO.AdscripcionServicioBO> Talleres
        {
            get { return Session["listTalleres"] as List<BPMO.Servicio.Catalogos.BO.AdscripcionServicioBO>; }
            set { Session["listTalleres"] = value; }
        }

        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        public int? UC
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }

        public int? TipoServicioCorrectivo
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TipoServicioCorrectivo"]))
                    return int.Parse(ConfigurationManager.AppSettings["TipoServicioCorrectivo"]);

                return null;
            }
        }

        public GridView GvMantenimientos {
            get 
            {
                return this.gvIngresoUnidades;
            } 
        }

        public GridView GvUnidadesSeleccion
        {
            get 
            {
                return this.gvUnidades;
            }
        }

        public GridViewRow RowEnSesion
        {
            get
            {
                return this.RecuperarEnSesion("RowMantenimimento") as GridViewRow;
            }
        }

        public DropDownList DdTalleres { get { return this.ddTalleres; } }

        public DropDownList DdTipoOrdenServicio { get { return this.ddTipoOrdenServicio; } }

        public System.Web.UI.WebControls.Image ImgCombustible 
        {
            get { return this.imgCombustible; }
        }

        public String ClienteNombre
        {
            get
            {
                String clienteNombre = null;
                GridViewRow row = (GridViewRow)this.RowEnSesion;
                if (row != null)
                {
                    TextBox txtCliente = row.FindControl("txtCliente") as TextBox;
                    clienteNombre = txtCliente.Text.Trim().ToUpper();
                }
                return clienteNombre;
            }
        }

        public String ModeloNombre
        {
            get
            {
                String modeloNombre = null;
                GridViewRow row = (GridViewRow)this.RowEnSesion;
                if (row != null)
                {
                    TextBox txtModelo = row.FindControl("txtModelo") as TextBox;
                    modeloNombre = txtModelo.Text.Trim().ToUpper();
                }
                return modeloNombre;
            }
        }

        private AdscripcionBO Adscripcion
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj.Adscripcion;
            }
        }

        public int IndicePaginaResultado
        {
            get { return this.gvIngresoUnidades.PageIndex; }
            set { this.gvIngresoUnidades.PageIndex = value; }
        }

        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }

        public string RootPath
        {
            get
            {
                return this.Server.MapPath("~");
            }
        }

        /// <summary>
        /// Determina si esta habilitado poder leer del archivo InLine
        /// </summary>
        public bool? LeerInLine
        {
            get
            {
                bool value;
                if (String.IsNullOrEmpty(this.hdnLeerInline.Value))
                    return null;
                if (bool.TryParse(this.hdnLeerInline.Value.Trim(), out value))
                    return value;
                return null;
            }
            set
            {
                this.hdnLeerInline.Value = value != null ? (value == true ? "true" : "false") : String.Empty;
            }
        }
        /// <summary>
        /// Configuraciones de mantenimiento por modelo consultadas
        /// </summary>
        public List<ConfiguracionMantenimientoBO> ListaConfiguracionesMantenimiento
        {
            get
            {
                if (this.Session["ListaConfiguraciones"] == null)
                    return null;
                else
                    return (List<ConfiguracionMantenimientoBO>)this.Session["ListaConfiguraciones"];
            }
            set
            {
                if (value == null)
                    this.Session.Remove("ListaConfiguraciones");
                else
                    this.Session["ListaConfiguraciones"] = value;
            }
        }
        /// <summary>
        /// Reporte en formato Base64 para exportaciones
        /// </summary>
        public byte[] Reporte64 {
            set {
                if (value != null)
                    this.Session["DocumentoBase64"] = value;
                else
                    this.Session.Remove("DocumentoBase64");
            }
        }
        #endregion

        #region Constructores

        /// <summary>
        /// Evento que se ejecuta al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarUnidadPRE(this);
                this.sincronizarEstatusPre = new SincronizaEstatusPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }

            if (Session["listMantenimientos"] == null)
            {
                Session["listMantenimientos"] = new List<MantenimientoBOF>();
            }

            if (!this.IsPostBack)
            {
                this.presentador.ValidarAcceso();
                this.presentador.InicializaModulo();
                this.LimpiarSesion();
            }

            this.presentador.CargarDatos();
            DesplegarListaMantenimientosProgramadosPendientes();
            //Asignación de campo oculto que indica si los campos para generación y construcción se validan
            this.hdnValidaCampoPSL.Value = (ETipoEmpresa)this.UnidadOperativaId == ETipoEmpresa.Idealease ? "1" : "0";
        }

        #endregion

        #region Métodos

        public void PrepararBusqueda()
        {
            this.txtSucursal.Text = "";
            this.hdnSucursalID.Value = "";
        }
        
        public bool IsUnidad()
        {
            return this.StateOrdenServicio == ETipoServicio.Unidad;
        }

        /// <summary>
        /// Limpia los registros de ingreso de la sesion
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["listResultadoBusquedaUnidades"] != null)
                Session.Remove("listResultadoBusquedaUnidades");
            if (Session["mantenimientosProgramadosPendientes"] != null) {
                Session.Remove("mantenimientosProgramadosPendientes");
            }
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

        /// <summary>
        /// Carga la lista de talleres
        /// </summary>
        public void CargarTalleres()
        {
            AdscripcionBO adscripcionFiltro = new AdscripcionBO() {
                UnidadOperativa = this.Adscripcion.UnidadOperativa,
                Sucursal = new SucursalBO() { Id = this.SucursalID }
            };
            this.Talleres = this.presentador.ConsultarTalleres(adscripcionFiltro);
            this.ddTalleres.DataValueField = "Id";
            this.ddTalleres.DataTextField = "Nombre";
            this.ddTalleres.DataSource = this.Talleres.Select(a => a.Taller).ToList();
            this.ddTalleres.DataBind();
        }

        /// <summary>
        /// Carga los estatus en el registro de ingresos
        /// </summary>
        public void CargarEstatus()
        {
            #region[Integración de genéración y construcción]
            if ((ETipoEmpresa)this.UnidadOperativaId == ETipoEmpresa.Idealease)
            {
                //Carga de estatus para idealease
                foreach (var value in Enum.GetValues(typeof(EEstatusMantenimiento)))
                {
                    ListItem itemenum = new ListItem(Enum.GetName(typeof(EEstatusMantenimientoPSL), value).Replace("_", " "), ((int)(EEstatusMantenimiento)value).ToString());
                    ddEstatus.Items.Add(itemenum);
                }
                this.lblCombustibleEntrada.Visible = false;
                this.lblCombustibleTotal.Visible = false;
            }
            else
            {
                //Carga de estatus para generación y construcción
                foreach (var value in Enum.GetValues(typeof(EEstatusMantenimientoPSL)))
                {
                    ListItem itemenum = new ListItem(Enum.GetName(typeof(EEstatusMantenimientoPSL), value).Replace("_", " "), ((int)(EEstatusMantenimientoPSL)value).ToString());
                    ddEstatus.Items.Add(itemenum);
                }

                //Se agrega un nuevo elemento al listado de tipo orden de servicio
                ListItem itemNuevo = new ListItem("DAÑOS", "3");
                ddTipoOrdenServicio.Items.Add(itemNuevo);

                this.idObligadoInventario.Visible = false;
                this.idCodigoFalla.Visible = false;
                this.idObligadoCombustibleTotal.Visible = false;
                this.lblCombustibleEntrada.Visible = true;
                this.lblCombustibleTotal.Visible = true;
            }
            #endregion
        }

        /// <summary>
        /// Muestra el resultado de la busqueda de unidades
        /// </summary>
        public void MostrarResultadoUnidades()
        {
            this.RegistrarScript("PopUnidades", "AbrirDialogoUnidades();");
        }

        /// <summary>
        /// Crea el cabecero personalizado
        /// </summary>
        public void CrearCabeceraComplemento()
        {
            if (this.gvIngresoUnidades.Controls.Count > 0)
            {
                TableCell thProgramacion = new TableHeaderCell();
                thProgramacion.HorizontalAlign = HorizontalAlign.Center;
                thProgramacion.ColumnSpan = 4;
                thProgramacion.BackColor = Color.FromArgb(92, 94, 93);
                thProgramacion.ForeColor = Color.White;
                thProgramacion.BorderColor = Color.White;
                thProgramacion.Font.Bold = true;
                thProgramacion.Text = "PROGRAMACI&Oacute;N";

                TableCell thEntrada = new TableHeaderCell();
                thEntrada.HorizontalAlign = HorizontalAlign.Center;
                thEntrada.ColumnSpan = 4;
                thEntrada.BackColor = Color.FromArgb(92, 94, 93);
                thEntrada.ForeColor = Color.White;
                thEntrada.BorderColor = Color.White;
                thEntrada.Font.Bold = true;
                thEntrada.Text = "DATOS DE ENTRADA";

                TableCell thVacio = new TableHeaderCell();
                thVacio.ColumnSpan = 10;
                thVacio.BackColor = Color.White;
                thVacio.BorderColor = Color.White;

                GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                row.Cells.Add(thVacio);
                row.Cells.Add(thProgramacion);
                row.Cells.Add(thEntrada);
                row.BackColor = Color.White;
                row.BorderColor = Color.White;

                Table table = ((Table)this.gvIngresoUnidades.Controls[0]);
                if (table.Rows.Count > 0)
                {
                    table.Rows.AddAt(0, row);
                }
            }
        }

        public void CargarDatos()
        {
            this.presentador.CargarDatos();
        }

        #region Privados

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), key, script, true);
        }

        public void PonerEnSesion(string key, object value)
        {
            Session[key] = value;
        }

        private void RemoverEnSesion(string key)
        {
            if (Session[key] != null)
                Session.Remove(key);
        }

        private object RecuperarEnSesion(string key)
        {
            return Session[key];
        }

        #endregion

        /// <summary>
        /// Redirige a la pagina informativa de falta de permisos para acceder
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            try
            {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
            }
            catch (Exception ex)
            {

                this.MostrarMensaje("Error al consultar unidades", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            var row = this.RowEnSesion as GridViewRow;
            switch (catalogo)
            {
                case ECatalogoBuscador.Unidad:
                case ECatalogoBuscador.Sucursal:
                    this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
                    break;
                case ECatalogoBuscador.Cliente:
                    ClienteBO cliente = (ClienteBO)this.Session_BOSelecto;
                    if (row != null) 
                    {
                        string guid = ((Label)row.FindControl("lblGuid")).Text;
                        this.presentador.Consultar("Cliente", cliente.Id.ToString(), guid);
                    }
                    break;
                case ECatalogoBuscador.Modelo:
                    BPMO.Servicio.Catalogos.BO.ModeloBO modelo = (BPMO.Servicio.Catalogos.BO.ModeloBO)this.Session_BOSelecto;
                    if (row != null && modelo != null)
                    {
                        string guid = ((Label)row.FindControl("lblGuid")).Text;
                        this.presentador.Consultar("Modelo", modelo.Id.ToString(), guid);
                    }
                    break;
            }
            this.Session_BOSelecto = null;
        }

        /// <summary>
        /// Guarda la orden de servicio de una unidad
        /// </summary>
        private void GuardarOrdenServicioUnidad()
        {
            try
            {
                this.presentador.RegistrarOrdenServicio(ETipoEquipo.Unidad);
                this.RemoverEnSesion("RowMantenimimento");
                this.presentador.CargarDatos();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al guardar la orden de servicio", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Guarda la orden de servicio de un equipo aliado
        /// </summary>
        private void GuardarOrdenServicioEquipoAliado()
        {
            try
            {
                this.presentador.RegistrarOrdenServicio(ETipoEquipo.EquipoAliado);
                this.presentador.CargarDatos();
                this.RemoverEnSesion("RowMantenimimento");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al guardar la orden de servicio", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Limpia los datos de los campos para la orden de servicio
        /// </summary>
        private void LimpiarDialogoOrdenServicio()
        {
            var vacio = string.Empty;
            this.txtInventario.Text = vacio;
            this.txtCombustible.Text = vacio;
            this.txtCombustibleTotal.Text = vacio;
            this.presentador.CargarImagenDefecto();
            this.txtDescripcionFalla.Text = vacio;
            this.txtCodigoFalla.Text = vacio;
        }

        #region CU017
        /// <summary>
        /// Redirige all UI para imprimir la calcomania
        /// </summary>
        public void redirigeCU017()
        {
            this.RegistrarScript("IrImprimirCalco", "IrImprimirCalcomania();");
        }
        #endregion

        #region CU051
        /// <summary>
        /// Identificador del CU051
        /// </summary>
        public string IdentificadorReporteCU051
        {
            get { return "CU051"; }
        }
        #endregion

        #region CU=!%
        /// <summary>
        /// Redirige all UI para imprimir la calcomania
        /// </summary>
        public void redirigePaseSalida()
        {
            this.RegistrarScript("IrImprimirPase", "IrImprimirPaseSalida();");
        }
        #endregion

        #region CU064
        /// <summary>
        /// Redirige all UI para enviar correo
        /// </summary>
        public void redirigeCU064(bool imprimirPendientes = false)
        {
            string _p = imprimirPendientes ? "1" : "0";
            this.RegistrarScript("IrEnviarServicioRealizado", "IrEnviarServicioRealizado(" + _p +");");
        }
        #endregion

        #region CU063
        /// <summary>
        /// Redirige al UI para consultar tareas pendientes
        /// </summary>
        public void RedirigeCU063()
        {
            this.RegistrarScript("IrTareasPendientes", "IrTareasPendientes();");
        }
        #endregion

        /// <summary>
        /// Despliega el visor de los formatos a imprimir
        /// </summary>
        public void IrAImprimir()
        {
            const string Url = "../Buscador.UI/VisorReporteUI.aspx";
            Response.Redirect(Url, true);
        }
        /// <summary>
        /// Establece el Paquete de Navegacion para imprimir los Formatos del Contrato
        /// </summary>
        /// <param name="Clave">Clave del Paquete de Navegacion</param>
        /// <param name="DatosReporte">Datos del Reporte</param>
        /// <param name="ConfigurarNombreSesion">Indica si se configurará el nombre que tendrá la variable de sesión</param>
        public void EstablecerPaqueteNavegacionImprimir(string Clave, Dictionary<string, object> DatosReporte, bool ConfigurarNombreSesion = false) {
            if (ConfigurarNombreSesion)
                Session["rpt." + Clave] = DatosReporte;
            else {
                Session["NombreReporte"] = Clave;
                Session["DatosReporte"] = DatosReporte;
            }
        }

        public DataSet MantenimientosProgramadosPendientes
        {
            get { return Session["mantenimientosProgramadosPendientes"] as DataSet; }
            set { Session["mantenimientosProgramadosPendientes"] = value; }
        }

        public void DesplegarListaMantenimientosProgramadosPendientes()
        {
            CargarMantenimientosProgramadosPendientes();
            if (MantenimientosProgramadosPendientes != null && MantenimientosProgramadosPendientes.Tables[0].Rows.Count > 0)
            {
                RegistrarScript("PopMantenimientosPendientes", "AbrirDialogoMantenimientosPendientes();");
            }
        }

        private void CargarMantenimientosProgramadosPendientes()
        {
            grvMantenimientosPendientes.DataSource = MantenimientosProgramadosPendientes;
            grvMantenimientosPendientes.DataBind();
        }

        protected void grvMantenimientosPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grvMantenimientosPendientes.PageIndex = e.NewPageIndex;
                this.CargarMantenimientosProgramadosPendientes();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvMantenimientosPendientes_PageIndexChanging:" + ex.Message);
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSucursal = (TextBox)sender;
            if (txtSucursal.Text != "")
            {
                try
                {
                    this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
                }
                catch (Exception ex)
                {
                    this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Evento que sincroniza el estatus de la orden de servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SincronizarEstatus_Click(object sender, EventArgs e)
        {
            sincronizarEstatusPre.SincronizaEstatus();
        }

        #region Ingreso

        /// <summary>
        /// Evento para agregar una nueva fila de ingreso
        /// </summary>
        protected void OnclickAgregarIngreso(Object sender, EventArgs e)
        {
            this.presentador.AgregarNuevaFila();
        }

        /// <summary>
        /// Evento para cancelar el ingreso de la undad
        /// </summary>
        protected void OnclickCancelarIngreso(Object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMotivoCancelacion.Text))
                this.MostrarMensaje("Es necesario escribir un Motivo de Cancelación", ETipoMensajeIU.ADVERTENCIA, null);
            else
                this.presentador.CancelarIngreso();
        }

        protected void OnclickCombustibleSalida(Object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCombustibleSalida.Text))
                    this.MostrarMensaje("Es necesario ingresar el combustible de salida", ETipoMensajeIU.ADVERTENCIA, null);
                else
                    this.RegistrarScript("PopCerrar", "CerrarDialogoCombustible();");
                this.presentador.ImprimirCalcomania();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al imprimir la calcomanía. ", ETipoMensajeIU.ERROR, this.nombreClase + ".OnclickCombustibleSalida:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para cancelar el ingreso de la undad
        /// </summary>
        protected void OnclickAbrirMotivoCancelacion(Object sender, EventArgs e)
        {
            int countSeleccionados = 0;
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked)
                    {
                        countSeleccionados += 1;
                    }
                }
            }

            if (countSeleccionados == 0)
            {
                this.MostrarMensaje("Selecciona un registro para continuar.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                if (countSeleccionados > 1)
                {
                    this.MostrarMensaje("Favor de seleccionar un registro a la vez.", ETipoMensajeIU.ADVERTENCIA, null);
                }
                else
                {
                    this.txtMotivoCancelacion.Text = "";
                    this.RegistrarScript("PopCancelar", "AbrirDialogoCancelacion();");
                }
            }
        }

        /// <summary>
        /// Evento para cancelar el ingreso de la undad
        /// </summary>
        protected void ProcesoEnviarInfoCliente() {
            int countSeleccionados = 0;
            GridViewRow rowSeleccionado = null;
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked)
                    {
                        countSeleccionados += 1;
                        rowSeleccionado = row;
                    }
                }
            }

            if (countSeleccionados == 0)
            {
                this.MostrarMensaje("Selecciona un registro para continuar.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            else
            {
                if (countSeleccionados > 1)
                {
                    this.MostrarMensaje("Favor de seleccionar un registro a la vez.", ETipoMensajeIU.ADVERTENCIA, null);
                }
                else
                {
                    string guid = ((Label)rowSeleccionado.FindControl("lblGuid")).Text;
                    MantenimientoBOF mantenimiento = this.Mantenimientos.Where(x => x.Guid == guid).FirstOrDefault();
                    bool finalizado = this.presentador.validarEnviarInformacionCliente(mantenimiento);
                    if (finalizado == true)
                    {
                        this.PonerEnSesion("RowMantenimimento", rowSeleccionado);
                        this.presentador.EnviarInformacionCliente();

                    }
                    else if (finalizado == false)
                    {
                        this.MostrarMensaje("No se ha finalizado el mantenimiento de la unidad, Genere su pase de salida", ETipoMensajeIU.ADVERTENCIA);
                    }
                   
                }
            }
        }

        /// <summary>
        /// Evento para obtener los mantenimientos programados pendientes
        /// </summary>
        protected void OnclickPendientesPorIngresar(object sender, EventArgs e) {
            this.presentador.ObtenerUnidadesPendientesPorEntrar();
        }

        /// <summary>
        /// Evento para generar calcomanía y/o Pase de Salida
        /// </summary>
        protected void btnImprimirFormatos_Click(object sender, EventArgs e) {
            this.ProcesoImprimirCalcomania();
        }

        /// <summary>
        /// Impresión de la calcomanía
        /// </summary>
        private void ProcesoImprimirCalcomania() {
            int countSeleccionados = 0;
            GridViewRow rowSeleccionado = null;
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked) {
                        countSeleccionados += 1;
                        rowSeleccionado = row;
                    }
                }
            }

            if (countSeleccionados == 0) {
                this.MostrarMensaje("Selecciona un registro para continuar.", ETipoMensajeIU.ADVERTENCIA, null);
            } else {
                if (countSeleccionados > 1) {
                    this.MostrarMensaje("Favor de seleccionar un registro a la vez.", ETipoMensajeIU.ADVERTENCIA, null);
                } else {
                    string guid = ((Label)rowSeleccionado.FindControl("lblGuid")).Text;
                    MantenimientoBOF mantenimiento = this.Mantenimientos.Where(x => x.Guid == guid).FirstOrDefault();
                    var mantenimientoUnidad = mantenimiento.MantenimientoUnidad;

                    if (!mantenimiento.RealizarAuditoria || this.presentador.CuentaConAuditoria(mantenimientoUnidad)) {
                        if (mantenimientoUnidad.OrdenServicio != null && mantenimientoUnidad.OrdenServicio.Estatus != null && (mantenimientoUnidad.OrdenServicio.Estatus.Nombre == "ASIGNADA" || mantenimientoUnidad.OrdenServicio.Estatus.Nombre == "FACTURADA")) {
                            this.PonerEnSesion("RowMantenimimento", rowSeleccionado);
                            var Combustible = this.presentador.validarCalcomania(mantenimientoUnidad);
                            if (Combustible == true) {
                                // Ya se generó la calcomanía, imprimir solo el Pase de Salida
                                if (mantenimientoUnidad.FechaSalida == null)
                                    this.ProcesoImprimirPaseSalida();
                                else
                                    this.ProcesoEnviarInfoCliente();
                            } else if (Combustible == false) {
                                this.txtCombustibleSalida.Text = "";
                                this.RegistrarScript("PopCombustibleSalida", "AbrirDialogoCombustibleSalida();");
                            }
                        } else {
                            this.MostrarMensaje("No se puede generar la calcomania de mantenimiento si la orden no esta Facturada o Asignada", ETipoMensajeIU.ADVERTENCIA);
                        }
                    } else {
                        this.MostrarMensaje("Es necesario realizar la auditoria para poder finaliza la Orden.", ETipoMensajeIU.ADVERTENCIA, null);
                    }
                }
            }
        }

        /// <summary>
        /// Impresión del Pase de Salida
        /// </summary>
        private void ProcesoImprimirPaseSalida() {
            int countSeleccionados = 0;
            GridViewRow rowSeleccionado = null;
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked) {
                        countSeleccionados += 1;
                        rowSeleccionado = row;
                    }
                }
            }

            if (countSeleccionados == 0) {
                this.MostrarMensaje("Selecciona un registro para continuar.", ETipoMensajeIU.ADVERTENCIA, null);
            } else {
                if (countSeleccionados > 1) {
                    this.MostrarMensaje("Favor de seleccionar un registro a la vez.", ETipoMensajeIU.ADVERTENCIA, null);
                } else {
                    this.PonerEnSesion("RowMantenimimento", rowSeleccionado);
                    this.presentador.GenerarPaseSalida();
                }
            }
        }

        /// <summary>
        /// Evento para buscar mantenimientos segun fitros aplicados
        /// </summary>
        protected void OnclickBuscarMantenimientos(Object sender, EventArgs e)
        {
            if (this.ddTalleres.SelectedItem != null)
            {
                try
                {
                    this.presentador.CargarMantenimientos();
                }
                catch (Exception ex)
                {

                    this.MostrarMensaje("Error al consultar los Mantenimientos :", ETipoMensajeIU.ERROR, ex.Message);
                }
                
            }
            else
            {
                this.MostrarMensaje("Es necesario seleccionar un Taller", ETipoMensajeIU.ADVERTENCIA, null);
            }
        }

        /// <summary>
        /// Evento al ingresar km en el inicio del mantenimiento
        /// </summary>
        protected void OnTextChangedKm(Object sender, EventArgs e)
        {
            TextBox txtKm = (TextBox)sender;
            if (txtKm.Text != "")
            {
                try
                {
                    int? km = int.Parse(txtKm.Text);
                    var gridRow = txtKm.Parent.Parent as GridViewRow;
                    string guid = ((Label)gridRow.FindControl("lblGuid")).Text;
                    this.presentador.CalcularMantenimientoReal(ETipoEquipo.Unidad, guid, km);
                    this.presentador.CargarDatos();
                }
                catch (Exception ex)
                {
                    this.MostrarMensaje("Error al calcular Mantenimiento Real", ETipoMensajeIU.ERROR, ex.Message);
                }
            }
        }

        /// <summary>
        /// Evento al ingresar km en el inicio del mantenimiento
        /// </summary>
        protected void OnTextChangedHoras(Object sender, EventArgs e)
        {
            TextBox txtHoras = (TextBox)sender;
            if (txtHoras.Text != "")
            {
                try
                {
                    int? hrs = int.Parse(txtHoras.Text);
                    var gridRow = txtHoras.Parent.Parent as GridViewRow;
                    string guid = ((Label)gridRow.FindControl("lblGuid")).Text;
                    this.presentador.CalcularMantenimientoReal(ETipoEquipo.Unidad, guid, null, hrs);
                    this.presentador.CargarDatos();
                }
                catch (Exception ex)
                {
                    this.MostrarMensaje("Favor de ingresar únicamente números enteros", ETipoMensajeIU.ERROR, ex.Message);
                }
            }
        }

        /// <summary>
        /// Evento para calcular mantenimiento real en grid de equipos aliados, unicamente con km
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEKmCalc_Click(object sender, EventArgs e)
        {
            GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
            TextBox txtEKmCalc = row.FindControl("txtEKmCalc") as TextBox;
            txtEKmCalc.Text = this.presentador.ClearValue(txtEKmCalc.Text);
            int km = 0;
            if (int.TryParse(txtEKmCalc.Text, out km))
            {
                try
                {
                    string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
                    this.presentador.CalcularMantenimientoReal(ETipoEquipo.EquipoAliado, guid, km);
                    this.presentador.CargarDatos();
                }
                catch (Exception ex)
                {
                    this.MostrarMensaje("Error al calcular Mantenimiento Real", ETipoMensajeIU.ERROR, ex.Message);
                }
            }
        }

        /// <summary>
        /// Evento para calcular mantenimiento real en grid de equipos aliados, con km y hrs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEHrsCalc_Click(object sender, EventArgs e)
        {
            GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
            TextBox txtEHorometroCalc = row.FindControl("txtEHorometroCalc") as TextBox;
            txtEHorometroCalc.Text = this.presentador.ClearValue(txtEHorometroCalc.Text);
            int hrs = 0;
            if (int.TryParse(txtEHorometroCalc.Text, out hrs))
            {
                try
                {
                    string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;
                    this.presentador.CalcularMantenimientoReal(ETipoEquipo.EquipoAliado, guid, null, hrs);
                    this.presentador.CargarDatos();
                }
                catch (Exception ex)
                {
                    this.MostrarMensaje("Error al calcular Mantenimiento Real", ETipoMensajeIU.ERROR, ex.Message);
                }
            }
        }

        /// <summary>
        /// Evento que ocurre sobre el grid de unidades a nivel de tuplas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var mantenimiento = ((MantenimientoBOF)e.Row.DataItem);
                if (mantenimiento.Detalles != null
                    && mantenimiento.Detalles.Count > 0)
                {
                    GridView gvAliados = e.Row.FindControl("gvAliados") as GridView;
                    gvAliados.DataSource = mantenimiento.Detalles;
                    gvAliados.DataBind();
                }

                TextBox lblKmCalc = e.Row.FindControl("lblKmCalc") as TextBox;
                TextBox lblHorometroCalc = e.Row.FindControl("lblHorometroCalc") as TextBox;

                if (mantenimiento.LeerInline)
                {
                    lblKmCalc.Enabled = lblHorometroCalc.Enabled = false;
                }
                else
                {
                    if (mantenimiento.MantenimientoUnidad != null && mantenimiento.MantenimientoUnidad.OrdenServicio != null && mantenimiento.MantenimientoUnidad.OrdenServicio.Id != null)
                    {
                        lblKmCalc.Enabled = false;
                        lblHorometroCalc.Enabled = false;
                    }
                    else
                    {
                        var permitirKm = this.presentador.IngresarMedida(mantenimiento.MantenimientoUnidad.IngresoUnidad.Unidad, EUnidaMedida.Kilometros);
                        var permitirHrs = this.presentador.IngresarMedida(mantenimiento.MantenimientoUnidad.IngresoUnidad.Unidad, EUnidaMedida.Horas);

                        lblKmCalc.Enabled = permitirKm == null ? false : permitirKm.Value;
                        lblHorometroCalc.Enabled = permitirHrs == null ? false : permitirHrs.Value;
                    }                    
                }

                ImageButton btnIngresar = e.Row.FindControl("btnIngresar") as ImageButton;
                btnIngresar.Enabled = mantenimiento.MantenimientoUnidad.MantenimientoUnidadId == null;

                ImageButton btnEditarOs = e.Row.FindControl("btnEditarOs") as ImageButton;
                btnEditarOs.Enabled = mantenimiento.MantenimientoUnidad.OrdenServicio.Id != null;

                ImageButton btnIniciarMante = e.Row.FindControl("btnInMant") as ImageButton;
                btnIniciarMante.Enabled = mantenimiento.MantenimientoUnidad.OrdenServicio.Id == null;

                if (mantenimiento.MantenimientoUnidad.OrdenServicio != null && mantenimiento.MantenimientoUnidad.OrdenServicio.Id != null)
                {
                    mantenimiento.RealizarAuditoria = this.presentador.RealizarAuditoria(mantenimiento.MantenimientoUnidad);
                }

                if(mantenimiento.MantenimientoUnidad.TipoServicio != null && mantenimiento.MantenimientoUnidad.TipoServicio.Id != null)
                {
                    if (mantenimiento.MantenimientoUnidad.TipoServicio.Id == this.TipoServicioCorrectivo)
                        mantenimiento.RealizarAuditoria = false;
                }

                ImageButton btnAuditoria = e.Row.FindControl("btnIniciarAuditoria") as ImageButton;
                btnAuditoria.Enabled = mantenimiento.RealizarAuditoria;

                CheckBox chkImpresionFinalizada = e.Row.FindControl("chkImpresoMantto") as CheckBox;
                chkImpresionFinalizada.Checked = mantenimiento.MantenimientoUnidad.FechaSalida.HasValue;
            }
        }

        /// <summary>
        /// Evento que ocurre sobre el grid de unidades al cambiar un checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIngresoUnidadesCheckedChanged(Object sender, EventArgs e) {
            int countSeleccionados = 0;
            GridViewRow rowSeleccionado = null;
                        
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked)
                    {
                        countSeleccionados += 1;
                        rowSeleccionado = row;
                    }
                    // Desmarcar Enviar Tareas Pendientes
                    CheckBox chkAgregarTareas = row.Cells[25].FindControl("chkAgregarTareas") as CheckBox;
                    chkAgregarTareas.Enabled = chkAgregarTareas.Checked = false;
                }
            }

            if (countSeleccionados == 1) {
                this.ListaTareasPendientes = null;
                this.PonerEnSesion("RowMantenimimento", rowSeleccionado);
                object lblOS = rowSeleccionado.Cells[22].FindControl("lblOrdenServicio");
                if (lblOS != null && !string.IsNullOrWhiteSpace((lblOS as Label).Text)) {
                    this.btnTareasPendientes.Visible = false;
                    this.btnRegistrarTarea.Visible = true;
                } else {
                    this.presentador.VerificarTareasPendientes();
                }
            } else {
                this.btnTareasPendientes.Visible = false;
                this.btnRegistrarTarea.Visible = false;
            }
        }

        /// <summary>
        /// Habilita el boton de tareas pendientes
        /// </summary>
        public void HabilitarTareasPendientes() {
            this.btnTareasPendientes.Visible = true;
            this.btnRegistrarTarea.Visible = false;
            // Si tiene tareas habilitar el campo
            CheckBox chkIncluirTP = this.RowEnSesion.Cells[25].FindControl("chkAgregarTareas") as CheckBox;
            chkIncluirTP.Enabled = true;
        }

        /// <summary>
        /// Evento del botón de tareas pendientes
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// </summary>
        protected void OnClickTareasPendientes(object sender, EventArgs e)
        {
            this.RedirigeCU063();
        }

        /// <summary>
        /// Evento del botón Registrar Tarea
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarTarea_Click(object sender, EventArgs e) {
            #region Conteo de selección
            int countSeleccionados = 0;
            GridViewRow rowSeleccionado = null;
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows) {
                if (row.RowType == DataControlRowType.DataRow) {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSeleccionar") as CheckBox);
                    if (chkRow.Checked) {
                        countSeleccionados += 1;
                        rowSeleccionado = row;
                    }
                }
            }

            if (countSeleccionados == 0) {
                this.MostrarMensaje("Selecciona un registro para continuar.", ETipoMensajeIU.ADVERTENCIA, null);
            } else if (countSeleccionados > 1) {
                this.MostrarMensaje("Selecciona un solo registro para esta acción.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            #endregion /Conteo Selección

            string guid = ((Label)rowSeleccionado.FindControl("lblGuid")).Text;
            MantenimientoUnidadBO mantenimiento = this.Mantenimientos.FirstOrDefault(m => m.Guid == guid).MantenimientoUnidad;
            if (String.IsNullOrEmpty(mantenimiento.IngresoUnidad.Unidad.NumeroSerie) && mantenimiento.IngresoUnidad.Unidad.Modelo == null)
                return;
            if (String.IsNullOrEmpty(mantenimiento.IngresoUnidad.Unidad.NumeroSerie) && mantenimiento.IngresoUnidad.Unidad.Modelo != null && mantenimiento.IngresoUnidad.Unidad.Modelo.Id != null)
                return;

            TareaPendienteBOF tareaPendiente = new TareaPendienteBOF() {
                UnidadID = mantenimiento.IngresoUnidad.Unidad.UnidadID,
                Serie = mantenimiento.IngresoUnidad.Unidad.NumeroSerie,
                NumeroEconomico = mantenimiento.IngresoUnidad.Unidad.NumeroEconomico,
                ModeloID = mantenimiento.IngresoUnidad.Unidad.Modelo.Id,
                Modelo = mantenimiento.IngresoUnidad.Unidad.Modelo.Nombre,
            };
            this.PonerEnSesion("TareaPendienteBOF", tareaPendiente);
            this.RegistrarScript("IrRegistrarTarea", "IrRegistrarTarea();");
        }

        /// <summary>
        /// Evento que ocurre sobre el grid de equipos aliados a nivel de tuplas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowDataBoundEquipoAliado(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var mantenimiento = ((MantenimientoBOF)e.Row.DataItem);

                TextBox lblKmCalc = e.Row.FindControl("txtEKmCalc") as TextBox;
                TextBox lblHorometroCalc = e.Row.FindControl("txtEHorometroCalc") as TextBox;

                int? km = String.IsNullOrEmpty(lblKmCalc.Text) ? null : (Int32?)Int32.Parse(lblKmCalc.Text);
                int? hrs = String.IsNullOrEmpty(lblHorometroCalc.Text) ? null : (Int32?)Int32.Parse(lblHorometroCalc.Text);

                ImageButton btnIngresar = e.Row.FindControl("btnIngresarEA") as ImageButton;
                ImageButton btnInMant = e.Row.FindControl("btnInMantEA") as ImageButton;
                ImageButton btnAuditoria = e.Row.FindControl("btnIniciarAuditoriaEA") as ImageButton;
                ImageButton btnEditarOs = e.Row.FindControl("btnEditarOsEA") as ImageButton;

                lblKmCalc.Text = km == null ? "" : km.Value.ToString();
                lblHorometroCalc.Text = hrs == null ? "" : hrs.Value.ToString();

                if (mantenimiento.MantenimientoAliado.IngresoEquipoAliado.EquipoAliado.EntraMantenimiento == false)
                {
                    lblKmCalc.Enabled = lblHorometroCalc.Enabled = false;
                    btnIngresar.Enabled = btnInMant.Enabled = btnAuditoria.Enabled = btnEditarOs.Enabled = false;
                }
                else
                {
                    if (mantenimiento.MantenimientoAliado != null && mantenimiento.MantenimientoAliado.OrdenServicio != null && mantenimiento.MantenimientoAliado.OrdenServicio.Id != null)
                    {
                        lblKmCalc.Enabled = lblHorometroCalc.Enabled = false;
                    }
                    else
                    {
                        var permitirKm = this.presentador.IngresarMedida(mantenimiento.MantenimientoUnidad.IngresoUnidad.Unidad, EUnidaMedida.Kilometros);
                        var permitirHrs = this.presentador.IngresarMedida(mantenimiento.MantenimientoUnidad.IngresoUnidad.Unidad, EUnidaMedida.Horas);

                        lblKmCalc.Enabled = permitirKm == null ? false : permitirKm.Value;
                        lblHorometroCalc.Enabled = permitirHrs == null ? false : permitirHrs.Value;
                    }

                    btnIngresar.Enabled = mantenimiento.MantenimientoAliado.MantenimientoEquipoAliadoId == null;
                    btnInMant.Enabled = mantenimiento.MantenimientoAliado.OrdenServicio.Id == null;
                    btnAuditoria.Enabled = mantenimiento.RealizarAuditoria == null ? false : mantenimiento.RealizarAuditoria;
                    btnEditarOs.Enabled = mantenimiento.MantenimientoAliado.OrdenServicio == null ? false : mantenimiento.MantenimientoAliado.OrdenServicio.Id != null;
                }
            }
        }
        
        /// <summary>
        /// Evento para desplegar resultado de buscador general
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                this.DesplegarBOSelecto(ViewState_Catalogo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }
        
        /// <summary>
        /// Eventos para los botones del grid de mantenimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvRegistro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    if (e.CommandName.Trim() == "Vin" || e.CommandName.Trim() == "NumeroEconomico" || e.CommandName.Trim() == "Modelo" ||
                        e.CommandName.Trim() == "Cliente" || e.CommandName.Trim() == "Ingresar" || e.CommandName.Trim() == "Mantenimiento" ||
                        e.CommandName.Trim() == "Auditoria" || e.CommandName.Trim() == "EditarOs")
                    { }
                    else { return; }
                    //index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    index = Convert.ToInt32(e.CommandArgument.ToString());
                    GridViewRow row = this.gvIngresoUnidades.Rows[index];
                    string guid = ((Label)row.FindControl("lblGuid")).Text;
                    
                    switch (e.CommandName.Trim())
                    {
                        case "Vin":
                            TextBox txtVin = row.FindControl("txtVin") as TextBox;
                            string vin = txtVin.Text;
                            this.PonerEnSesion("RowMantenimimento", row);
                            this.presentador.Consultar("NumeroSerie", vin, guid);
                            break;
                        case "NumeroEconomico":
                            TextBox txtNumeroEconomico = row.FindControl("txtNumeroEconomico") as TextBox;
                            this.PonerEnSesion("RowMantenimimento", row);
                            this.presentador.Consultar("NumeroEconomico", txtNumeroEconomico.Text, guid);
                            break;
                        case "Modelo":
                            TextBox txtModelo = row.FindControl("txtModelo") as TextBox;
                            if (txtModelo.Text.Length < 1)
                            {
                                this.MostrarMensaje("Es necesario un nombre del Modelo.", ETipoMensajeIU.ADVERTENCIA);
                                return;
                            }
                            try
                            {
                                this.PonerEnSesion("RowMantenimimento", row);
                                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
                            }
                            catch (Exception ex)
                            {
                                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarClientes_Click" + ex.Message);
                            }
                            break;
                        case "Cliente":
                            TextBox txtCliente = row.FindControl("txtCliente") as TextBox;
                            if (txtCliente.Text.Length < 1)
                            {
                                this.MostrarMensaje("Es necesario un nombre del Cliente.", ETipoMensajeIU.ADVERTENCIA);
                                return;
                            }
                            try
                            {
                                this.PonerEnSesion("RowMantenimimento", row);
                                this.EjecutaBuscador("Cliente&hidden=0", ECatalogoBuscador.Cliente);
                            }
                            catch (Exception ex)
                            {
                                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ":" + ex.Message);
                            }
                            break;
                        case "Ingresar":
                            this.PonerEnSesion("RowMantenimimento", row);
                            if (this.presentador.ValidarIngresoUnidad())
                            {
                                this.presentador.IngresarUnidad();
                            }
                            this.RemoverEnSesion("RowMantenimimento");
                            break;
                        case "Mantenimiento":
                            TextBox tempVin = row.FindControl("txtVin") as TextBox;
                            string numeroSerie = tempVin.Text;
                            this.PonerEnSesion("RowMantenimimento", row);
                            if (this.presentador.ValidarInicioMantenimientoUnidad())
                            {
                                string capacidadTanque = ((Label)row.FindControl("lblCapacidadTanque")).Text;
                                decimal capacidad = 0;
                                if (!decimal.TryParse(capacidadTanque, out capacidad))
                                    capacidad = 0;

                                this.LimpiarDialogoOrdenServicio();
                                if (this.LeerInLine.Value)
                                {
                                    MantenimientoBOF mantenimiento = this.Mantenimientos.Where(x => x.Guid == guid).FirstOrDefault();
                                    if (mantenimiento.MantenimientoUnidad.IngresoUnidad.Unidad.Modelo.Marca.Id == this.presentador.MarcaInternationalID)
                                    {
                                        this.txtCombustibleTotal.Enabled = false;
                                        this.txtCombustibleTotal.Text = this.presentador.LeerArchivo(RegistrarUnidadPRE.TipoLectura.COMBUSTIBLE, numeroSerie).ToString();
                                    }
                                    else
                                    {
                                        this.txtCombustibleTotal.Enabled = true;
                                        this.txtCombustibleTotal.Text = "";
                                    }
                                }
                                else
                                {
                                    this.txtCombustibleTotal.Enabled = true;
                                    this.txtCombustibleTotal.Text = "";
                                }
                                this.RegistrarScript("Pop", "AbrirDialogo('Unidad','" + capacidad +"');");
                                this.StateOrdenServicio = ETipoServicio.Unidad;
                            }
                            break;
                        case "Auditoria":
                            if (this.presentador.ValidarAuditoria(guid))
                            {
                                this.presentador.RealizarAuditoria(guid);
                                this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/RealizarAuditoriaMantenimientoUI.aspx"));
                            }
                            else 
                            {
                                return;
                            }
                            break;
                        case "EditarOs":
                            this.presentador.VerOrdenServicio(guid);
                            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/DetalleMantenimientoUI.aspx"));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el Registro de la Unidad", ETipoMensajeIU.ERROR, this.nombreClase + ".grvRegistro_RowCommand:" + ex.Message);
            }
        }

        /// <summary>
        /// Eventos grid de Equipos Aliados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvRegistroEquipoAliado_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int index;

            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    GridViewRow row = ((GridView)sender).Rows[index];
                    string guid = ((Label)row.FindControl("lblAliadoGuid")).Text;

                    switch (e.CommandName.Trim())
                    {
                        case "IngresarEquipoAliado":
                            this.PonerEnSesion("RowMantenimimento", row);
                            if (this.presentador.ValidarIngresoEquipoAliado())
                            {
                                this.presentador.IngresarEquipoAliado();
                            }
                            this.RemoverEnSesion("RowMantenimimento");
                            break;
                        case "MantenimientoEquipoAliado":
                            this.PonerEnSesion("RowMantenimimento", row);
                            if (this.presentador.ValidarInicioMantenimientoAliado())
                            {
                                this.LimpiarDialogoOrdenServicio();
                                this.RegistrarScript("Pop", "AbrirDialogo('EquipoAliado','0');");
                                this.StateOrdenServicio = ETipoServicio.EquipoAliado;
                            }
                            break;
                        case "EditarOsEquipoAliado":
                            this.presentador.VerOrdenServicioAliado(guid);
                            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/DetalleMantenimientoUI.aspx"));
                            break;

                            
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el Registro del Equipo Aliado", ETipoMensajeIU.ERROR, this.nombreClase + ".grvRegistroEquipoAliado_RowCommand:" + ex.Message);
            }
        }
        
        /// <summary>
        /// Evento para cambiar el paginado de resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvRegistro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvIngresoUnidades.PageIndex = e.NewPageIndex;
                this.presentador.CargarDatos();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Eventos grid de Seleccion de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvUnidadesSeleccion_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            int index;
            string numeroSerie = "";
            try
            {
                numeroSerie = e.CommandArgument.ToString();
                switch (e.CommandName.Trim())
                {
                    case "Unidad":
                        this.presentador.EstablecerUnidadSeleccion(numeroSerie);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el Registro del Equipo Aliado", ETipoMensajeIU.ERROR, this.nombreClase + ".grvUnidadesSeleccion_RowCommand:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para cambiar el paginado de las unidades consultadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUnidades.PageIndex = e.NewPageIndex;
                this.presentador.CargarGridViewUnidades();
                this.RegistrarScript("Pop", "AbrirDialogoUnidades();");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grvActasNacimiento_PageIndexChanging:" + ex.Message);
            }
        }

        protected void OnClickChangeImage(object sender, EventArgs e)
        {
            try
            {
                decimal capacidadTanque = 0;
                decimal.TryParse(this.hdnCapacidadTanque.Value, out capacidadTanque);
                if (capacidadTanque != 0)
                {
                    if (this.CombustibleEntrada > capacidadTanque)
                        this.MostrarMensaje("El combustible de entrada no puede ser mayor a " + capacidadTanque, ETipoMensajeIU.ADVERTENCIA);
                    else
                        this.presentador.CargarImagen();
                }
                else
                {
                    this.presentador.CargarImagen();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio un error al validar la cantidad del combustible de entrada", ETipoMensajeIU.ERROR, nombreClase + "OnClickChangeImage: " + ex.Message);
            }
            this.RegistrarScript("PopOs", "AbrirDialogo();");
        }

        #endregion

        #region Orden de Servicio

        /// <summary>
        /// Cancela el registro de la orden de servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnclickCancelarOrdenServicio(object sender, EventArgs e)
        {
            this.LimpiarDialogoOrdenServicio();
            this.RemoverEnSesion("RowMantenimimento");
            this.RemoverEnSesion("TareaPendienteBOF");
        }

        /// <summary>
        /// Registra 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnclickGuardarOrderServicio(object sender, EventArgs e)
        {

            switch (this.StateOrdenServicio)
            {
                case ETipoServicio.Unidad:
                    this.GuardarOrdenServicioUnidad();
                    break;
                case ETipoServicio.EquipoAliado:
                    this.GuardarOrdenServicioEquipoAliado();
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Evento para limpiar comas en grid aliado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAliados_PreRender(object sender, EventArgs e)
        {
          
            foreach (GridViewRow row in this.gvIngresoUnidades.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    GridView gvAliados = row.FindControl("gvAliados") as GridView;
                    foreach (GridViewRow rowAliado in gvAliados.Rows)
                    {
                        TextBox lblKmCalc = rowAliado.FindControl("txtEKmCalc") as TextBox;
                        TextBox lblHorometroCalc = rowAliado.FindControl("txtEHorometroCalc") as TextBox;
                        TextBox txtEObservaciones = rowAliado.FindControl("txtEObservaciones") as TextBox;
                        

                        lblKmCalc.Text = this.presentador.ClearValue(lblKmCalc.Text);
                        lblHorometroCalc.Text = this.presentador.ClearValue(lblHorometroCalc.Text);
                        txtEObservaciones.Text = this.presentador.ClearValue(txtEObservaciones.Text);

                    }
                   
                }
            }

        }
        /// <summary>
        /// Exporta los resultados de la consulta a un archivo de Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportarMantenimientos_Click(object sender, EventArgs e) {
            try {
                if (this.Mantenimientos == null)
                    this.MostrarMensaje("No existen mantenimientos a exportar. Primero realice una búsqueda.", ETipoMensajeIU.ADVERTENCIA, null);

                this.presentador.ExportarMantenimientos();
                
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al exportar.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnExportarMantenimientos_Click" + ex.Message);
            }
        }
        /// <summary>
        /// Invoca al proceso de descarga del archivo Excel
        /// </summary>
        public void RedirigeExportarReporte() {
            this.RegistrarScript("IrExportarReporteDoc", "IrExportarReporte();");
        }
        #endregion

        protected void btnCancelarCombustible_Click(object sender, EventArgs e)
        {
            this.RegistrarScript("PopCerrar", "CerrarDialogoCombustible();");
        }
    }
}
