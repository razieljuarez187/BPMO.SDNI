// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI
{
    public partial class ucOperadorUI : System.Web.UI.UserControl, IucOperadorVIS
    {
        #region Atributos
        private string nombreClase = "ucOperadorUI";
        private ucOperadorPRE presentador;
        public enum ECatalogoBuscador
        {
            CuentaClienteIdealease
        }
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

        public int? OperadorID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnOperadorID.Value) && !string.IsNullOrWhiteSpace(this.hdnOperadorID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnOperadorID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnOperadorID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }
        public int? CuentaClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnCuentaClienteID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnCuentaClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnCuentaClienteID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }
        public string CuentaClienteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(txtCuentaClienteNombre.Text)) ? null : this.txtCuentaClienteNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtCuentaClienteNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string Nombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNombre.Text)) ? null : this.txtNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? AñosExperiencia
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtAniosExperiencia.Text) && !string.IsNullOrWhiteSpace(this.txtAniosExperiencia.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtAniosExperiencia.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtAniosExperiencia.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public DateTime? FechaNacimiento
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaNacimiento.Text) && !string.IsNullOrWhiteSpace(this.txtFechaNacimiento.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtFechaNacimiento.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaNacimiento.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public string DireccionCalle
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCalle.Text)) ? null : this.txtCalle.Text.Trim().ToUpper();
            }
            set
            {
                this.txtCalle.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string DireccionCiudad
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCiudad.Text)) ? null : this.txtCiudad.Text.Trim().ToUpper();
            }
            set
            {
                this.txtCiudad.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string DireccionCP
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCodigoPostal.Text)) ? null : this.txtCodigoPostal.Text.Trim().ToUpper();
            }
            set
            {
                this.txtCodigoPostal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string DireccionEstado
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstado.Text)) ? null : this.txtEstado.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstado.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? LicenciaTipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ddlTipoLicencia.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlTipoLicencia.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlTipoLicencia.SelectedValue, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.ddlTipoLicencia.SelectedValue = value.Value.ToString().Trim();
                }
            }
        }
        public string LicenciaNumero
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtLicenciaNumero.Text)) ? null : this.txtLicenciaNumero.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtLicenciaNumero.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public DateTime? LicenciaFechaExpiracion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtLicenciaFechaExpiracion.Text) && !string.IsNullOrWhiteSpace(this.txtLicenciaFechaExpiracion.Text))
                {
                    DateTime date;
                    return DateTime.TryParse(this.txtLicenciaFechaExpiracion.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtLicenciaFechaExpiracion.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public string LicenciaEstado
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtLicenciaEstadoExpedicion.Text)) ? null : this.txtLicenciaEstadoExpedicion.Text.Trim().ToUpper(); //RI0061
            }
            set
            {
                this.txtLicenciaEstadoExpedicion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public bool? Estatus
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstatus.Value))
                    id = bool.Parse(this.hdnEstatus.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstatus.Value = value.ToString();
                else
                    this.hdnEstatus.Value = string.Empty;
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
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucOperadorPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.txtAniosExperiencia.Text = "";
            this.txtCalle.Text = "";
            this.txtCiudad.Text = "";
            this.txtCodigoPostal.Text = "";
            this.txtCuentaClienteNombre.Text = "";
            this.txtEstado.Text = "";
            this.txtFechaNacimiento.Text = "";
            this.txtLicenciaEstadoExpedicion.Text = "";
            this.txtLicenciaFechaExpiracion.Text = "";
            this.txtLicenciaNumero.Text = "";
            this.txtNombre.Text = "";

            this.hdnCuentaClienteID.Value = "";
            this.hdnEstatus.Value = "";
            this.hdnOperadorID.Value = "";

            this.txtAniosExperiencia.Enabled = true;
            this.txtCalle.Enabled = true;
            this.txtCiudad.Enabled = true;
            this.txtCodigoPostal.Enabled = true;
            this.txtCuentaClienteNombre.Enabled = true;
            this.txtEstado.Enabled = true;
            this.txtFechaNacimiento.Enabled = true;
            this.txtLicenciaEstadoExpedicion.Enabled = true;
            this.txtLicenciaFechaExpiracion.Enabled = true;
            this.txtLicenciaNumero.Enabled = true;
            this.txtNombre.Enabled = true;
            this.ddlTipoLicencia.Enabled = true;
        }
        public void PrepararEdicion()
        {
            this.txtAniosExperiencia.Enabled = true;
            this.txtCalle.Enabled = true;
            this.txtCiudad.Enabled = true;
            this.txtCodigoPostal.Enabled = true;
            this.txtCuentaClienteNombre.Enabled = true;
            this.txtEstado.Enabled = true;
            this.txtFechaNacimiento.Enabled = true;
            this.txtLicenciaEstadoExpedicion.Enabled = true;
            this.txtLicenciaFechaExpiracion.Enabled = true;
            this.txtLicenciaNumero.Enabled = true;
            this.txtNombre.Enabled = true;
            this.ddlTipoLicencia.Enabled = true;
        }
        public void PrepararVisualizacion()
        {
            this.txtAniosExperiencia.Enabled = false;
            this.txtCalle.Enabled = false;
            this.txtCiudad.Enabled = false;
            this.txtCodigoPostal.Enabled = false;
            this.txtCuentaClienteNombre.Enabled = false;
            this.txtEstado.Enabled = false;
            this.txtFechaNacimiento.Enabled = false;
            this.txtLicenciaEstadoExpedicion.Enabled = false;
            this.txtLicenciaFechaExpiracion.Enabled = false;
            this.txtLicenciaNumero.Enabled = false;
            this.txtNombre.Enabled = false;
            this.ddlTipoLicencia.Enabled = false;
        }

        public void PermitirSeleccionarCuentaCliente(bool permitir)
        {
            this.ibtnBuscarCliente.Enabled = permitir;
            this.ibtnBuscarCliente.Visible = permitir;
            this.txtCuentaClienteNombre.Enabled = permitir;
        }

        public void LimpiarSesion()
        {
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
        protected void txtLicenciaFechaExpiracion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string s = string.Empty;
                if ((s = this.presentador.ValidarFechaExpiracionLicencia()) != null)
                {
                    this.txtLicenciaFechaExpiracion.Text = string.Empty;
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccionar la fecha", ETipoMensajeIU.ERROR, this.nombreClase + ".txtLicenciaFechaExpiracion_TextChanged:" + ex.Message);
            }
        }
        protected void txtOperadorFechaNacimiento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string s = string.Empty;
                if ((s = this.presentador.ValidarFechaNacimiento()) != null)
                {
                    this.txtFechaNacimiento.Text = string.Empty;
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al seleccioanr la fecha", ETipoMensajeIU.ERROR, this.nombreClase + ".txtOperadorFechaNacimiento_TextChanged:" + ex.Message);
            }
        }

        #region Eventos para el buscador
        protected void txtCuentaClienteNombre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string cuentaClienteNombre = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = cuentaClienteNombre;
                if (!string.IsNullOrEmpty(this.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = null;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string cuentaClienteNombre = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
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