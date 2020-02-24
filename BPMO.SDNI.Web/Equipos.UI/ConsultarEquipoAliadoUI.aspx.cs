//Satisface al CU075 - Catálogo de Equipo Aliado
// Satisface a la SC0005
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ConsultarEquipoAliadoUI : Page, IConsultarEquipoAliadoVIS
    {
        #region Atributos
        private ConsultarEquipoAliadoPRE presentador = null;
        private const string nombreClase = "ConsultarEquipoAliadoUI";

        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal,
            Marca
        }
        #endregion

        #region Propiedades
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

        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                return null;
            }            
        }

        public int? SucursalID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value))
                    if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.Value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        public string SucursalNombre
        {
            get 
            {
                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                    return this.txtSucursal.Text.Trim().ToUpper();
                return null;
            }
            set 
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }

        public string NumeroSerie
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumeroSerie.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroSerie.Text))
                    return this.txtNumeroSerie.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtNumeroSerie.Text = value;
                else
                    this.txtNumeroSerie.Text = string.Empty;
            }
        }

        public string Marca
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtMarca.Text) && !string.IsNullOrWhiteSpace(this.txtMarca.Text))
                    return this.txtMarca.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtMarca.Text = value;
                else
                    this.txtMarca.Text = string.Empty;
            }
        }

        public int? MarcaID
        {
            get 
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnMarcaID.Value) && !string.IsNullOrWhiteSpace(this.hdnMarcaID.Value))
                    if (Int32.TryParse(this.hdnMarcaID.Value, out val))
                        return val;
                return null;
            }
            set 
            {
                if (value != null)
                    this.hdnMarcaID.Value = value.Value.ToString();
                else
                    this.hdnMarcaID.Value = string.Empty;
            }
        }

        public bool? ActivoOracle
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.ddlActivoOracle.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlActivoOracle.SelectedValue))
                {
                    if (Int32.TryParse(this.ddlActivoOracle.SelectedValue, out val))
                    {
                        switch(val)
                        {
                            case 1:
                                return true;
                                break;
                            case 2:
                                return false;
                                break;
                            default:
                                return null;
                            break;
                        }
                    }      
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (value.Value)
                        this.ddlActivoOracle.SelectedValue = "1";
                    else
                        this.ddlActivoOracle.SelectedValue = "2";
                }
                else
                    this.ddlActivoOracle.SelectedValue = "0";
            }
        }

        public int? TipoEquipoAliado
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlTipoEquipoAliado.SelectedValue) && !string.IsNullOrWhiteSpace(ddlTipoEquipoAliado.SelectedValue))
                {
                    int value;
                    if(Int32.TryParse(ddlTipoEquipoAliado.SelectedValue, out value))
                        if (value >= 0)
                            return value;
                }
                return null;
            }
            set { ddlTipoEquipoAliado.SelectedValue = value != null ? value.Value.ToString() : "-1"; }
        }

        public int? Estatus
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.ddlEstatusEquipo.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlEstatusEquipo.SelectedValue))
                    if (Int32.TryParse(this.ddlEstatusEquipo.SelectedValue, out val))
                        if(val >= 0)
                            return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlEstatusEquipo.SelectedValue = value.Value.ToString();
                else
                    this.ddlEstatusEquipo.SelectedValue = "-1";
            }
        }

        public int? UsuarioAutenticado
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

        public List<EquipoAliadoBO> Equipos 
        {
            get { return Session["listEquiposAliados"] != null ? Session["listEquiposAliados"] as List<EquipoAliadoBO> : null; }
            set { Session["listEquiposAliados"] = value; }
        }

        public int IndicePaginaResultado
        {
            get { return this.grdEquiposAliados.PageIndex; }
            set { this.grdEquiposAliados.PageIndex = value; }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Configuración de la unidad operativa que indica a qué libro corresponden los activos
        /// </summary>
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
        #endregion        

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarEquipoAliadoPRE(this);
                if (!IsPostBack)
                {
                    this.presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        
        public void PrepararBusqueda()
        {
            this.txtMarca.Text = string.Empty;
            this.txtNumeroSerie.Text = string.Empty;
            this.txtSucursal.Text = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.hdnUnidadOperativaID.Value = string.Empty;
            this.hdnMarcaID.Value = string.Empty;
            this.ddlTipoEquipoAliado.SelectedValue = "-1";
            this.ddlEstatusEquipo.SelectedValue = "-1";
            this.ddlActivoOracle.SelectedValue = "0";
        }

        public void CargarTiposEquipoAliado()
        {
            Type type = typeof(ETipoEquipoAliado);
            Array values = Enum.GetValues(typeof(ETipoEquipoAliado));
            ListItem item = new ListItem { Enabled = true, Selected = true, Text = "TODOS", Value = "-1" };
            ddlTipoEquipoAliado.Items.Add(item);
            foreach(int value in values)
            {
                var memInfo = type.GetMember(type.GetEnumName(value));
                var display = memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                if (display != null)
                {
                    ListItem itemenum = new ListItem(display.Description, value.ToString());
                    this.ddlTipoEquipoAliado.Items.Add(itemenum);
                }
            }

            //Se eliminan los items del ddlTipoEquipoAliado dependiendo de la unidad operativa
            switch (this.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Refrigerado).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Seco).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Construccion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Equinova).ToString()));
                    break;
                case (int)ETipoEmpresa.Construccion:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Refrigerado).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Seco).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Generacion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Equinova).ToString()));
                    break;
                case (int)ETipoEmpresa.Equinova:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Refrigerado).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Seco).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Generacion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Construccion).ToString()));
                    break;
                default:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Generacion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Construccion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Equinova).ToString()));
                    break;
            }

            ddlTipoEquipoAliado.DataBind();
        }

        public void CargarEstatusEquipos()
        {
            Array values = System.Enum.GetValues(typeof(EEstatusEquipoAliado));
            ListItem item = new ListItem { Enabled = true, Selected = true, Text = "TODOS", Value = "-1" };
            this.ddlEstatusEquipo.Items.Add(item);
            foreach (int value in values)
            {
                string display = Enum.GetName(typeof(EEstatusEquipoAliado), value);
                ListItem itemenum = new ListItem(display, value.ToString());
                this.ddlEstatusEquipo.Items.Add(itemenum);
            }                        
            this.ddlEstatusEquipo.DataBind();
        }

        public void LimpiarSesion()
        {
            if (Session["listEquiposAliados"] != null)
                Session.Remove("listEquiposAliados");
            if (Session["LastEquipoAliado"] != null)
                Session.Remove("LastEquipoAliado");
            if (Session["EquipoAliadoEditar"] != null)
                this.Session.Remove("EquipoAliadoEditar");
        }

        public void ActualizarResultado()
        {
            this.grdEquiposAliados.DataSource = this.Equipos;
            this.grdEquiposAliados.DataBind();
        }

        private void PrepararNuevaBusqueda()
        {
            this.hdnMarcaID.Value = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.txtMarca.Text = string.Empty;
            this.txtNumeroSerie.Text = string.Empty;
            this.txtSucursal.Text = string.Empty;
            this.ddlTipoEquipoAliado.SelectedValue = "-1";
            this.ddlEstatusEquipo.SelectedValue = "-1";
            this.ddlActivoOracle.SelectedValue = "0";
        }

        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleEquipoAliadoUI.aspx"));
        }

        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
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
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
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

        #region SC_0008
        public void PermitirRegistrar(bool permitir)
        {
            hlkRegistroActaNacimiento.Enabled = permitir;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar Equipos Aliados", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        protected void grdEquiposAliados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdEquiposAliados_PageIndexChanging:" + ex.Message);
            }
        }

        protected void grdEquiposAliados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        this.presentador.VerDetalles(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el equipo aliado", ETipoMensajeIU.ERROR, nombreClase + ".grdEquiposAliados_RowCommand:" + ex.Message);
            }
        }

        protected void grdEquiposAliados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    EquipoBO bo = (EquipoBO)e.Row.DataItem;
                    Label labelSucursalNombre = e.Row.FindControl("lblSucursal") as Label;
                    if (labelSucursalNombre != null)
                    {
                        string sucursalNombre = string.Empty;
                        if (bo.Sucursal != null)
                            if (bo.Sucursal.Nombre != null)
                            {
                                sucursalNombre = bo.Sucursal.Nombre;
                            }
                        labelSucursalNombre.Text = sucursalNombre;
                    }
                    Label labelMarca = e.Row.FindControl("lblMarca") as Label;
                    if (labelMarca != null)
                    {
                        string marca = string.Empty;
                        if (bo.Modelo != null)
                            if (bo.Modelo.Marca != null)
                            {
                                marca = bo.Modelo.Marca.Nombre;
                            }
                        labelMarca.Text = marca;
                    }
                    Label labelActivo = e.Row.FindControl("lblActivo") as Label;
                    if (labelActivo != null)
                    {
                        string activoOracle = string.Empty;
                        if (bo.EsActivo.HasValue)
                        {
                            if (bo.EsActivo.Value)
                                activoOracle = "SI";
                            else
                                activoOracle = "NO";
                        }
                        labelActivo.Text = activoOracle;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el equipo aliado", ETipoMensajeIU.ERROR, nombreClase + ".grdEquiposAliados_RowDataBound:" + ex.Message);
            }
        }

        #region Eventos para el Buscador
        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtNumeroSerie.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("EquipoBepensa&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        protected void txtMarca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreMarca = this.Marca;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Marca);

                this.Marca = nombreMarca;
                if (this.Marca != null)
                {
                    this.EjecutaBuscador("Marca", ECatalogoBuscador.Marca);
                    this.Marca = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una marca", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtMarca_TextChanged" + ex.Message);
            }
        }
        protected void ibtnBuscaMarca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Marca&hidden=0", ECatalogoBuscador.Marca);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaMarca_Click:" + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombre = nombreSucursal;
                if (SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Marca:
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}