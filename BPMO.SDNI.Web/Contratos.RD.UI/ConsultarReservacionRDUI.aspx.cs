// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.Globalization;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class ConsultarReservacionRDUI : System.Web.UI.Page, IConsultarReservacionRDVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ConsultarReservacionRDUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal,//SC_0051
            CuentaClienteIdealease,
            Modelo,
            Empleado
        }
        /// <summary>
        /// presentador del UC de información general del contrato de renta diaria
        /// </summary>
        private ConsultarReservacionRDPRE presentador;
        #endregion

        #region Propiedades
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        public int? UsuarioID
        {
            get
            {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        public string Numero
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumero.Text)) ? null : this.txtNumero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumero.Text = value;
                else
                    this.txtNumero.Text = string.Empty;
            }
        }
        public string CuentaClienteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNombreCuentaCliente.Text)) ? null : this.txtNombreCuentaCliente.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNombreCuentaCliente.Text = value;
                else
                    this.txtNombreCuentaCliente.Text = string.Empty;
            }
        }
        public int? CuentaClienteID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnCuentaClienteID.Value))
                    id = int.Parse(this.hdnCuentaClienteID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnCuentaClienteID.Value = value.ToString();
                else
                    this.hdnCuentaClienteID.Value = string.Empty;
            }
        }
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNombreModelo.Text)) ? null : this.txtNombreModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNombreModelo.Text = value;
                else
                    this.txtNombreModelo.Text = string.Empty;
            }
        }
        public int? ModeloID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnModeloID.Value))
                    id = int.Parse(this.hdnModeloID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroEconomico.Text)) ? null : this.txtNumeroEconomico.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumeroEconomico.Text = value;
                else
                    this.txtNumeroEconomico.Text = string.Empty;
            }
        }

        public DateTime? FechaReservacionInicial
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaInicio.Text) && !string.IsNullOrWhiteSpace(this.txtFechaInicio.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaInicio.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaInicio.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public TimeSpan? HoraReservacionInicial
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraInicio.Text) && !string.IsNullOrWhiteSpace(this.txtHoraInicio.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraInicio.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraInicio.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraInicio.Text = string.Empty;
            }
        }
        public DateTime? FechaReservacionFinal
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaFinal.Text) && !string.IsNullOrWhiteSpace(this.txtFechaFinal.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaFinal.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaFinal.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public TimeSpan? HoraReservacionFinal
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraFinal.Text) && !string.IsNullOrWhiteSpace(this.txtHoraFinal.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraFinal.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraFinal.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraFinal.Text = string.Empty;
            }
        }

        public int? UsuarioReservoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioReservoID.Value))
                    id = int.Parse(this.hdnUsuarioReservoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioReservoID.Value = value.ToString();
                else
                    this.hdnUsuarioReservoID.Value = string.Empty;
            }
        }
        public string UsuarioReservoNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUsuarioReservoNombre.Text)) ? null : this.txtUsuarioReservoNombre.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUsuarioReservoNombre.Text = value;
                else
                    this.txtUsuarioReservoNombre.Text = string.Empty;
            }
        }

        public bool? Activo
        {
            get
            {
                bool? activo = null;
                if (this.ddlActivo.SelectedIndex > -1)
                    activo = Convert.ToBoolean(this.ddlActivo.SelectedValue);
                return activo;
            }
            set
            {
                if (value != null)
                    this.ddlActivo.SelectedValue = value.ToString();
                else
                    this.ddlActivo.SelectedIndex = -1;
            }
        }

        public List<ReservacionRDBO> Resultado
        {
            get
            {
                if (Session["ListadoReservaciones"] != null)
                    return Session["ListadoReservaciones"] as List<ReservacionRDBO>;

                return null;
            }
        }

        #region Propiedades para el Buscador
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
        #endregion

        #region SC_0051
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnSucursalID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value)
                           ? (Int32.TryParse(this.hdnSucursalID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnSucursalID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text)
                           ? this.txtSucursal.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                            ? value.Trim().ToUpper()
                                            : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o estable la lista de sucursales a las que el usaurio autenticado tiene permiso de acceder
        /// </summary>
        public List<object> SucursalesAutorizadas
        {
            get
            {
                if (Session["SucursalesAutRD"] != null)
                    return Session["SucursalesAutRD"] as List<object>;
                else
                {
                    var lstRet = new List<SucursalBO>();
                    return lstRet.ConvertAll(x => (object)x);
                }
            }

            set { Session["SucursalesAutRD"] = value; }
        }
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarReservacionRDPRE(this);
                if (!this.IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void EstablecerResultado(List<ReservacionRDBO> resultado)
        {
            Session["ListadoReservaciones"] = resultado;
            string json = this.ConvertirResultadoAJSON(resultado);

            this.RegistrarScript("Calendar", "var jsonEventos =" + json + "; inicializeCalendar(jsonEventos);");
        }

        private string ConvertirResultadoAJSON(List<ReservacionRDBO> resultado)
        {
            try
            {
                #region Construcción del arreglo de eventos
                StringBuilder eventos = new StringBuilder();

                if (resultado != null)
                {
                    foreach (ReservacionRDBO bo in resultado)
                    {
                        StringBuilder evento = new StringBuilder();

                        if (bo.ReservacionID != null)
                            evento.AppendLine("id: " + bo.ReservacionID.ToString() + ",");

                        if (bo.Tipo != null)
                        {
                            switch (bo.Tipo)
                            {
                                case ETipoReservacion.MODELO:
                                    if (bo.Modelo != null && bo.Modelo.Nombre != null)
                                        evento.AppendLine("title: '" + bo.Modelo.Nombre + "',");
                                    break;
                                case ETipoReservacion.UNIDAD:
                                    if (bo.Unidad != null && bo.Unidad.NumeroEconomico != null)
                                        evento.AppendLine("title: '# Económico " + bo.Unidad.NumeroEconomico + "',");
                                    else
                                    {
                                        if (bo.Unidad != null && bo.Unidad.NumeroSerie != null)
                                            evento.AppendLine("title: 'VIN " + bo.Unidad.NumeroSerie + "',");
                                        else
                                            evento.AppendLine("title: 'Unidad sin # Económico',");
                                    }
                                    break;
                            }

                            evento.AppendLine("tipoId: " + ((int)bo.Tipo).ToString() + ",");
                            evento.AppendLine("tipo: '" + bo.Tipo.ToString() + "',");
                        }

                        if (bo.FechaInicial != null)
                            evento.AppendLine("start: '" + bo.FechaInicial.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        if (bo.FechaFinal != null)
                            evento.AppendLine("end: '" + bo.FechaFinal.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        if (bo.UsuarioReservo != null && bo.UsuarioReservo.Nombre != null)
                            evento.AppendLine("usuario: '" + bo.UsuarioReservo.Nombre + "',");
                        if (bo.Cliente != null && bo.Cliente.Nombre != null)
                            evento.AppendLine("cliente: '" + bo.Cliente.Nombre + "',");
                        if (bo.Observaciones != null)
                            evento.AppendLine("observaciones: '" + bo.Observaciones + "',");

                        if (bo.Activo != null && bo.Activo == false)
                        {
                            evento.AppendLine("className: 'TipoElemento3',"); 
                            evento.AppendLine("activo: ' (CANCELADA)',");
                        }
                        else
                        {
                            if (bo.Tipo != null && bo.Tipo == ETipoReservacion.MODELO)
                                evento.AppendLine("className: 'TipoElemento1',");
                            if (bo.Tipo != null && bo.Tipo == ETipoReservacion.UNIDAD)
                                evento.AppendLine("className: 'TipoElemento2',");
                            evento.AppendLine("activo: '',");
                        }                            
                        
                        if (evento.ToString().Trim().Length > 0)
                        {
                            eventos.AppendLine("{");
                            eventos.AppendLine(evento.ToString().Substring(0, evento.Length - 3));
                            eventos.AppendLine("},");
                        }
                    }
                }
                #endregion
                
                StringBuilder json = new StringBuilder();
                json.AppendLine("[");

                if (eventos.ToString().Trim().Length > 0)
                    json.AppendLine(eventos.ToString().Substring(0, eventos.Length - 3));

                json.AppendLine("]");
                return json.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConvertirResultadoAJSON: " + ex.Message);
            }
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }
        
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleReservacionRDUI.aspx"));
        }

        public void LimpiarSesion()
        {
            if (Session["ListadoReservaciones"] != null)
                Session.Remove("ListadoReservaciones");
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

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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
        #endregion
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarReservaciones();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click: " + ex.Message);
            }
        }
        protected void btnDetalles_Click(object sender, EventArgs e)
        {
            try
            {
                int? id = (this.hdnReservacionID.Value.Trim().CompareTo("") != 0) ? (int?)Convert.ToInt32(this.hdnReservacionID.Value) : null;
                this.presentador.IrADetalle(id);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar la reservación", ETipoMensajeIU.ERROR, nombreClase + ".btnDetalles_Click: " + ex.Message);
            }
        }

        #region Eventos para el buscador
        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrEmpty(this.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = null;

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ERROR, nombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ERROR, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void txtNombreModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string modelo = this.ModeloNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.ModeloNombre = modelo;
                if (this.ModeloNombre != null)
                {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar el Modelo", ETipoMensajeIU.ERROR, this.nombreClase + ".txtNombreModelo_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar el modelo", ETipoMensajeIU.ERROR, this.nombreClase + ".ibtnBuscarModelo_Click:" + ex.Message);
            }
        }
        
        protected void txtUsuarioReservoNombre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string usuario = this.UsuarioReservoNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Empleado);

                this.UsuarioReservoNombre = usuario;
                if (this.UsuarioReservoNombre != null)
                {
                    this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);
                    this.UsuarioReservoNombre = null;
                }

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar el Usuario", ETipoMensajeIU.ERROR, this.nombreClase + ".txtUsuarioReservoNombre_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarUsuarioReservo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Empleado", ECatalogoBuscador.Empleado);

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar el usuario", ETipoMensajeIU.ERROR, this.nombreClase + ".ibtnBuscarUsuarioReservo_Click:" + ex.Message);
            }
        }
        #region SC_0051
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Session_BOSelecto = null;
                this.SucursalID = null;

                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                {
                    string sucursalNombre = this.SucursalNombre;

                    this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                    this.SucursalNombre = sucursalNombre;
                    if (this.UnidadOperativaID.Value != null && this.SucursalNombre != null)
                    {
                        this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                        this.SucursalNombre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.ToString() + ".ibtnBuscaSucursal_Click:" + ex.Message);
            }
        }
        #endregion
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.Empleado:
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.Resultado != null)
                    this.EstablecerResultado(this.Resultado);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}