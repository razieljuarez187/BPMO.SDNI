// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Registro Configuración Sistema de 
    /// Unidad Idealease, al usuario.
    /// </summary>
    public partial class RegistrarConfiguracionSistemaUnidadUI : System.Web.UI.Page, IRegistrarConfiguracionSistemaUnidadVIS {

        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista Registro Configuración Sistema de Unidad Idealease.
        /// </summary>
        private RegistrarConfiguracionSistemaUnidadPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(RegistrarConfiguracionSistemaUnidadUI).Name;

            #region Seguridad

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
        public int? UnidadOperativaID {
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

            #endregion

            #region Form Registro

        /// <summary>
        /// Obtiene o establece un valor que representa la Clave de la Configuración Sistema de Unidad Idealease.
        /// </summary>
        private string Clave{
             get {
                String clave = null;
                if (this.txtClave.Text.Trim().Length > 0)
                    clave = this.txtClave.Text.Trim().ToUpper();
                return clave;
            }
            set {
                if (value != null)
                    this.txtClave.Text = value.ToString();
                else
                    this.txtClave.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre de la Configuración Sistema de Unidad Idealease.
        /// </summary>
        private string Nombre{
             get {
                String nombre = null;
                if (this.txtNombre.Text.Trim().Length > 0)
                    nombre = this.txtNombre.Text.Trim().ToUpper();
                return nombre;
            }
            set {
                if (value != null)
                    this.txtNombre.Text = value.ToString();
                else
                    this.txtNombre.Text = String.Empty;
            }
        }

            #endregion 

            #region Grid Configuraciones

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Configuraciones Sistemas de Unidad Idealease a registrar.
        /// </summary>
        public List<ConfiguracionSistemaUnidadBO> Configuraciones {
            get { return Session["configuracionesAGuardar"] as List<ConfiguracionSistemaUnidadBO>; }
            set { Session["configuracionesAGuardar"] = value; }
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
                presentador = new RegistrarConfiguracionSistemaUnidadPRE(this);
                if (!IsPostBack) {
                    Session["recargarAuditorias"] = false;
                    presentador.PrepararSeguridad();
                    Configuraciones = new List<ConfiguracionSistemaUnidadBO>();
                }
            }catch(Exception ex){
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + " .Page_Load: " + ex.Message);
            }
            CargarConfiguraciones();
        }

        #endregion

        #region Métodos

            #region Form Registro

        /// <summary>
        /// Verifica que no exista una Configuración Sistema de Unidad Idealease con la misma clave en las Configuraciones 
        /// previamente agregadas.
        /// </summary>
        /// <param name="configuracion">El Filtro ConfiguracíonSistemaUnidadBO.</param>
        /// <returns>Retorna True si encontró coincidencias, en caso contrario retornal False.</returns>
        private bool ValidarExistencia(ConfiguracionSistemaUnidadBO configuracion){
            return Configuraciones.Find(x => x.Clave.Equals(configuracion.Clave)) != null;
        }

            #endregion

            #region Grid Configuraciones

        /// <summary>
        /// Realiza la vinculación de la lista de Configuraciones Sistema de Unidad Idealease agregadas.
        /// </summary>
        public void CargarConfiguraciones() {
            gvConfiguraciones.DataSource = Configuraciones;
            gvConfiguraciones.DataBind();
        }

            #endregion

        /// <summary>
        /// Bloquea los Campos Clave y Nombre de la Configuración Sistema de Unidad Idealease.
        /// </summary>
        public void BloquearCampos() {
            txtNombre.ReadOnly = true;
            txtNombre.Enabled = false;
            txtClave.ReadOnly = true;
            txtClave.Enabled = false;
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Eliminación Configuración Sistema de Unidad Idealease.
        /// </summary>
        private void RedirigirAEliminarConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/EliminarConfiguracionSistemaUnidadUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Edición Configuración Sistema de Unidad Idealease.
        /// </summary>
        private void RedirigirAEditarConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/EditarConfiguracionSistemaUnidadUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Configuraciones Sistemas de Unidad Idealease.
        /// </summary>
        private void RedirigirAConsultaConfiguracionSistemaUnidad() {
            Response.Redirect(ResolveUrl("~/Mantenimiento.UI/ConsultarConfiguracionSistemaUnidadUI.aspx"));
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

        #endregion

        #region Eventos

            #region Form Registro

        /// <summary>
        /// Agrega una Configuración Sistema de Unidad a la tabla de Configuraciones. Verifica que la Clave tenga 3 carácteres.
        /// Verifica que la Configuración Sistema de Unidad Idealease no se haya guardado con anterioridad.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void AgregarATabla_Click(object sender, EventArgs e) {
            if (Nombre == null || Clave == null) {
                MostrarMensaje("Algunos campos son obligatorios", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            if (this.Clave.Length < 3) {
                MostrarMensaje("Error al agregar la configuración", ETipoMensajeIU.ERROR, "El tamaño de la clave no puede ser menor a 3");
                return;
            }
            ConfiguracionSistemaUnidadBO configuracion = new ConfiguracionSistemaUnidadBO() {
                Nombre = this.Nombre.ToUpper(),
                Clave = this.Clave.ToUpper()
            };
            if (ValidarExistencia(configuracion)) {
                MostrarMensaje("Esta configuración ya fue agregada con anterioridad", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            Configuraciones.Add(configuracion);
            CargarConfiguraciones();
            Nombre = "";
            Clave = "";    
        }

            #endregion

            #region Grid Configuraciones

        /// <summary>
        /// Despliega las Configuraciones Sistema de Unidad Idealease agregadas, en su página correspondiente.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvConfiguraciones_PageIndexChanging(object sender, GridViewPageEventArgs e){
            try {
                this.gvConfiguraciones.PageIndex = e.NewPageIndex;
                CargarConfiguraciones();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene la Configuración Sistema de Unidad Idealease seleccionada. Carga sus detalles para Edición o Eliminación 
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void gvConfiguraciones_RowCommand(object sender, GridViewCommandEventArgs e) { 
            int index;

            try {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString())) {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    ConfiguracionSistemaUnidadBO configuracion = this.Configuraciones[index];
                    switch(e.CommandName.Trim()) {
                        case "Eliminar":
                            Configuraciones.Remove(configuracion);
                            CargarConfiguraciones();
                            break;
                        case "Editar":
                            Nombre = configuracion.Nombre;
                            Clave = configuracion.Clave;
                            Configuraciones.Remove(configuracion);
                            CargarConfiguraciones();
                        break;
                    }
                }
            }
            catch (Exception er) { }
        }

            #endregion

        /// <summary>
        /// Realiza la redirección al visor seleccionado.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void mConfiguracion_MenuItemClick(object sender, MenuEventArgs e) {
            Session["configuracionSeleccionada"] = null;
            switch (e.Item.Value) {
                case "EliminarConfiguracion":
                    RedirigirAEliminarConfiguracionSistemaUnidad();
                    break;
                case "EditarConfiguracion":
                    RedirigirAEditarConfiguracionSistemaUnidad();
                    break;
            }
        }

        /// <summary>
        /// Registra la lista de Configuraciones Sistemas de Unidad Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnFinalizar_Click(object sender, EventArgs e) {
            if (Configuraciones.Count == 0) {
                MostrarMensaje("No se encontraron Configuraciones de Sistema de Unidad para guardar!", ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            
            presentador.Agregar();
        }

        /// <summary>
        /// Realiza la redirección al visor de Consulta Configuraciones Sistemas de Unidad Idealease.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            Configuraciones = new List<ConfiguracionSistemaUnidadBO>();
            RedirigirAConsultaConfiguracionSistemaUnidad();
        }

        #endregion

    }
}