// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI
{
    public partial class ConsultarOperadorUI : System.Web.UI.Page, IConsultarOperadorVIS
    {
        #region Atributos
        private ConsultarOperadorPRE presentador;
        private string nombreClase = "ConsultarOperadorUI";

        public enum ECatalogoBuscador
        {
            CuentaClienteIdealease
        }
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

        public int? CuentaClienteID
        {
            get
            {
                return !string.IsNullOrEmpty(hdnCuentaClienteID.Value.Trim())
                           ? (int?)int.Parse(hdnCuentaClienteID.Value.Trim())
                           : null;
            }
            set
            {
                hdnCuentaClienteID.Value = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        public string CuentaClienteNombre
        {
            get { return (String.IsNullOrEmpty(this.txtCuentaClienteNombre.Text.Trim())) ? null : this.txtCuentaClienteNombre.Text.Trim().ToUpper(); }
            set { this.txtCuentaClienteNombre.Text = value != null ? value : string.Empty; }
        }
        public string Nombre
        {
            get { return (String.IsNullOrEmpty(this.txtNombre.Text.Trim())) ? null : this.txtNombre.Text.Trim(); }
            set { this.txtNombre.Text = value != null ? value : string.Empty; }

        }
        public bool? Estatus
        {
            set
            {
                if (value != null)
                {
                    if (value == true)
                        this.ddlEstatus.SelectedValue = "ACTIVO";
                    if (value == false)
                        this.ddlEstatus.SelectedValue = "INACTIVO";
                }
                else
                    this.ddlEstatus.SelectedValue = "";
            }
            get
            {
                if (this.ddlEstatus.SelectedValue.Trim().ToUpper() == "ACTIVO")
                    return true;
                if (this.ddlEstatus.SelectedValue.Trim().ToUpper() == "INACTIVO")
                    return false;
                return null;
            }

        }
        public string LicenciaNumero
        {
            get { return (String.IsNullOrEmpty(this.txtLicenciaNumero.Text.ToUpper())) ? null : this.txtLicenciaNumero.Text.ToUpper(); }
            set { this.txtLicenciaNumero.Text = value != null ? value : string.Empty; }
        }

        public List<OperadorBO> Resultado
        {
            get
            {
                if (Session["ListaOperadores"] == null)
                    return new List<OperadorBO>();
                else
                    return (List<OperadorBO>)Session["ListaOperadores"];

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
                    objeto = (Session[nombreSession]);

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
                    objeto = (Session[ViewState_Guid]);

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

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ConsultarOperadorPRE(this);
                if (!Page.IsPostBack)
                {
                    this.presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistenacias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void EstablecerResultado(List<OperadorBO> resultado)
        {
            Session["ListaOperadores"] = resultado;

            this.grdOperadores.DataSource = resultado;
            this.grdOperadores.DataBind();
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
            this.hlkRegistrar.Enabled = permitir;
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleOperadorUI.aspx"));
        }

        public void LimpiarSesion()
        {
            if (Session["ListaOperadores"] != null)
                Session.Remove("ListaOperadores");
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
        /// <param name="tipoBusqueda">Nombre catalogo</param>
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
                this.presentador.ConsultarOperadores();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click: " + ex.Message);
            }
        }

        protected void grdOperadores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdOperadores.DataSource = this.Resultado;
                grdOperadores.PageIndex = e.NewPageIndex;
                grdOperadores.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdOperadores_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdOperadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;
            if (e.CommandName.ToString().Trim().ToUpper() == "PAGE") return;
            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        {
                            this.presentador.IrADetalle(index);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el operador", ETipoMensajeIU.ERROR, this.nombreClase + ".grdOperadores_RowCommand:" + ex.Message);
            }
        }

        protected void grdOperadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    OperadorBO bo = (OperadorBO)e.Row.DataItem;

                    Label lblFecha = e.Row.FindControl("lblFechaExpiracion") as Label;
                    if (lblFecha != null)
                    {
                        string fecha = string.Empty;
                        if (bo.Licencia != null && bo.Licencia.FechaExpiracion != null)
                            fecha = String.Format("{0:dd/MM/yyyy}", bo.Licencia.FechaExpiracion);
                        lblFecha.Text = fecha;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".grdOperadores_RowDataBound: " + ex.Message);
            }
        }

        #region Eventos para el Buscador
        protected void ibtnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutaBuscador("CuentaClienteIdealease&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al buscar un cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }
        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cuentaClienteNombre = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = cuentaClienteNombre;
                if (this.CuentaClienteNombre != null)
                {
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txbNombreCliente_TextChanged:" + ex.Message);
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