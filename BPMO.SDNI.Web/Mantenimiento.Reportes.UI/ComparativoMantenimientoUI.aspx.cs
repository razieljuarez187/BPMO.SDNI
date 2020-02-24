// Satisface al CU075 - Reporte Comparativo Mantenimiento
using System;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using System.Web.UI;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using BPMO.Primitivos.Enumeradores;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI {

    /// <summary>
    /// Clase que contiene los métodos para la presentación de la información de Envío de Correo de Servicio Realizado, al usuario.
    /// </summary>
    public partial class ComparativoMantenimientoUI : ReportPage, IComparativoMantenimientoVIS {
        
        #region Atributos

        /// <summary>
        /// Presentador que atiende las peticiones de la vista.
        /// </summary>
        private ComparativoMantenimientoPRE presentador;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private string nombreClase = typeof(ComparativoMantenimientoUI).Name;

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        public int? ModuloId {
             get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
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
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        public int? UnidadOperativaId {
            get {
                return this.Master.UnidadOperativaID;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        public int? UsuarioAutenticado {
            get {
                return Master.UsuarioID;
            }
        }

            #endregion

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Enumerador de Catálogos para el Buscador.
        /// </summary>
        public enum ECatalogoBuscador{
            Unidad
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

                #region Filtro Sucursal

        /// <summary>
        /// Obtiene o estable el Identificador de la Sucursal seleccionada.
        /// </summary>
        public int? SucursalId {
            get{ return this.Master.SucursalID; }
            set{ this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Obtiene o estable el Nombre de la Sucursal a buscar o la seleccionada.
        /// </summary>
        public string NombreSucursal {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        }

                #endregion

                #region Filtro Modelo

        /// <summary>
        /// Obtiene o estable el Identificador del Modelo seleccionado.
        /// </summary>
        public int? ModeloId {
            get{ return this.Master.ModeloID; }
            set{ this.Master.ModeloID = value; }
        }

        /// <summary>
        /// Obtiene o estable el Nombre del Modelo a buscar o el seleccionado.
        /// </summary>
        public string NombreModelo {
            get { return this.Master.ModeloNombre; }
            set { this.Master.ModeloNombre = value; }
        }

                #endregion

                #region Filtro Cliente Idealease

        /// <summary>
        /// Obtiene o estable el Identificador del Cliente Idealease seleccionado.
        /// </summary>
        public int? ClienteId {
            get{ return this.Master.ClienteID; }
            set{ this.Master.ClienteID = value; }
        }

        /// <summary>
        /// Obtiene o estable el Nombre del Cliente Idealease a buscar o el seleccionado.
        /// </summary>
        public string NombreCliente {
            get { return this.Master.ClienteNombre; }
            set { this.Master.ClienteNombre = value; }
        }

                #endregion

                #region Filtro Unidad

        /// <summary>
        /// Obtiene o estable el Número de Serie del Equipo a buscar o el seleccionado.
        /// </summary>
        public string VIN {
            get {
                string vin = null;
                if (txtVIN.Text.Trim().Length > 0)
                    vin = txtVIN.Text.ToUpper();
                return vin;
            }
            set {
                if (value != null)
                    txtVIN.Text = value.ToString().ToUpper();
                else
                    txtVIN.Text = string.Empty;
            }
        }

                #endregion

            #endregion

        /// <summary>
        /// Obtiene la Fecha de Inicio del reporte.
        /// </summary>
        public DateTime? FechaInicio {
            get {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicio.Text)) {
                    var dateTemp = DateTime.Parse(this.txtFechaInicio.Text);
                    fecha = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, 0, 0, 0);
                }
                return fecha;
            }
        }

        /// <summary>
        /// Obtiene la Fecha de Fin del reporte.
        /// </summary>
        public DateTime? FechaFin {
            get {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaFin.Text)) {
                    var dateTemp = DateTime.Parse(this.txtFechaFin.Text);
                    fecha = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, 23, 59, 59);
                }
                return fecha;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Método delegado para el evento de carga de la página.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ComparativoMantenimientoPRE(this);
            } catch(Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + " .Page_Load: " + ex.Message);
            }
            if(!IsPostBack){
                presentador.PrepararSeguridad();
                this.Master.SucursalFiltroVisible = true;
                this.Master.ClienteFiltroVisible = true;
                
                this.Master.ModeloFiltroVisible = true;
            }
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
            this.RegistrarScript("Events", "BtnBuscar2('" + ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo) {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

                #endregion

                #region Filtro Unidad

        /// <summary>
        /// Activa el llamado al Buscador de Unidades.
        /// </summary>
        /// <param name="campo">Nombre del campo que desencadenó el evento.</param>
        private void DesplegarBusquedaUnidad(string campo) { 
            if (VIN.Length < 1) {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + campo + ex.Message);
            }
        }

                #endregion

        /// <summary>
        /// Realiza la búsqueda de los Mantenimientos. Verifica que las Fecha de Inicio sea menor a la Fecha Fin.
        /// </summary>
        public override void Consultar() {
            try {
                if (ValidarFecha()) {
                    return;
                }
                presentador.BuscarMantenimientos();
            } catch (Exception ex) { }
        }

        /// <summary>
        /// Verifica que la Fecha de Fin sea mayor a la Fecha de Inicio o que al seleccionar una Fecha ambas sean obligatorias.
        /// </summary>
        /// <returns>Retorna True si la Fecha Fin es menor a la Fecha Inicio o solo una Fecha se haya seleccionado, en caso contrario 
        /// retorna False.</returns>
        private bool ValidarFecha() {
            if(FechaInicio != null || FechaFin != null){
                if (FechaInicio != null && FechaFin == null) {
                    MostrarMensaje("Error", ETipoMensajeIU.ERROR, "La Fecha Fin no puede ser nula.");
                    return true;
                }
                if (FechaInicio == null && reFechaFin != null) {
                    MostrarMensaje("Error", ETipoMensajeIU.ERROR, "La Fecha Inicio no puede ser nula.");
                    return true;
                }
                if (FechaFin.Value.Date < FechaInicio.Value.Date) {
                    MostrarMensaje("Error", ETipoMensajeIU.ERROR, "La Fecha Fin no puede ser menos a la Fecha Inicio.");
                    return true;
                }
            }
            return false;
        }

            #endregion

        /// <summary>
        /// Registra un Script en el cliente.
        /// </summary>
        /// <param name="key">Llave del Script.</param>
        /// <param name="script">Script a registrar.</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #endregion

        #region Eventos

            #region Form Búsqueda

                #region Buscador

        /// <summary>
        /// Establece el Objeto Seleccionado del Buscador.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnResult2_Click(object sender, EventArgs e) {
            try {
                switch (ViewState_Catalogo) {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + " .btnResult2_Click: " + ex.Message);
            }
         }

                #endregion

                #region Filtro Unidad

        /// <summary>
        /// Evento que activa el llamado al Buscador de Unidades.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void txtVIN_TextChanged(object sender, EventArgs e) {
            DesplegarBusquedaUnidad(" .txtVIN_TextChanged ");
        }

        /// <summary>
        /// Evento que activa el llamado al Buscador de Unidades.
        /// </summary>
        /// <param name="sender">Objeto que desencadenó el evento.</param>
        /// <param name="e">Argumento asociado al evento.</param>
        protected void btnBuscarVIN_Click(object sender, EventArgs e) { 
            DesplegarBusquedaUnidad(" .btnBuscarVin_Click ");
        }

                #endregion

            #endregion

        #endregion
    }
}