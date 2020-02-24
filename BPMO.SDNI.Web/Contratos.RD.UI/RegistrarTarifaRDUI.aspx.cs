// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class RegistrarTarifaRDUI : System.Web.UI.Page, IRegistrarTarifaRDVIS
    {
        #region Atributos
        private string NombreClase = "RegistrarTarifaRDUI";
        private RegistrarTarifaRDPRE presentador;
        public enum ECatalogoBuscador
        {
            Modelo,
            Cliente,
            Sucursal,
            SucursalNoAplica
        }

        #endregion

        #region Propiedades

        public int? UC
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Usuario != null && master.Usuario.Id != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
        public DateTime FC
        {
            get { return DateTime.Now; }
        }
        public int? UnidadOperativaID
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null &&
                    master.Adscripcion.UnidadOperativa.Id != null)
                    id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public decimal? PrecioCombustible
        {
            set
            {
                TextBox txtPrecioCombustible = mTarifa.Controls[0].FindControl("txtValue") as TextBox;
                if (txtPrecioCombustible != null)
                {
                    txtPrecioCombustible.Text = value != null ? String.Format("{0:#,##0.00##}", value) : string.Empty; //RI0062
                }
            }
        }
        public int? ModeloID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnModeloID.Value.Trim()))
                    id = int.Parse(this.hdnModeloID.Value.Trim());
                return id;
            }
            set { this.hdnModeloID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreModelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModelo.Text.Trim())) ? null : this.txtModelo.Text.ToUpper();
            }
            set { this.txtModelo.Text = value ?? String.Empty; }
        }
        public int? ClienteID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnClienteID.Value.Trim()))
                    id = int.Parse(this.hdnClienteID.Value.Trim());
                return id;
            }
            set { this.hdnClienteID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreCliente
        {
            get { return (String.IsNullOrEmpty(this.txtCliente.Text.Trim())) ? null : this.txtCliente.Text.ToUpper(); }
            set { this.txtCliente.Text = value ?? String.Empty; }
        }
        public int? TipoTarifaSeleccionada
        {
            get
            {
                int? id = null;
                if (ddlTipoTarifa.SelectedValue != "-1")
                    id = int.Parse(ddlTipoTarifa.SelectedValue.Trim());
                return id;
            }
        }
        public string MonedaSeleccionada
        {
            get
            {
                string codigoMoneda = null;
                if (ddlMoneda.SelectedValue != "-1")
                    codigoMoneda = this.ddlMoneda.SelectedValue.ToUpper();
                return codigoMoneda;
            }
        }
        public int? SucursalID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value.Trim()))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set { this.hdnSucursalID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreSucursal
        {
            get { return (String.IsNullOrEmpty(this.txtSucursal.Text.Trim())) ? null : this.txtSucursal.Text.ToUpper(); }
            set { this.txtSucursal.Text = value ?? String.Empty; }
        }
        public int? SucursalNoAplicaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalNoAplicaID.Value.Trim()))
                    id = int.Parse(this.hdnSucursalNoAplicaID.Value.Trim());
                return id;
            }
            set { this.hdnSucursalNoAplicaID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreSucursalNoAplica
        {
            get { return (String.IsNullOrEmpty(this.txtSucursalNoAplica.Text.Trim())) ? null : this.txtSucursalNoAplica.Text.ToUpper(); }
            set { this.txtSucursalNoAplica.Text = value ?? String.Empty; }
        }
        public List<SucursalBO> ListaSucursalSeleccionada
        {
            get
            {
                return Session["ListaSucursales"] != null
                           ? (List<SucursalBO>)Session["ListaSucursales"]
                           : new List<SucursalBO>();
            }
            set
            {
                List<SucursalBO> lst = value ?? new List<SucursalBO>();
                Session["ListaSucursales"] = lst;
                grdSucursales.DataSource = lst;
                grdSucursales.DataBind();
            }
        }
        public string Descripcion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtDescripcion.Text.Trim())) ? null : this.txtDescripcion.Text.Trim().ToUpper();
            }
            set
            {
                this.txtDescripcion.Text = value ?? String.Empty;
            }
        }
        public DateTime? Vigencia
        {
            get
            {
                if (string.IsNullOrEmpty(this.txtVigencia.Text.Trim())) return null;
                return Convert.ToDateTime(this.txtVigencia.Text.Trim());
            }
            set
            {
                this.txtVigencia.Text = value == null ? "" : ((DateTime)value).ToShortDateString();
            }
        }
        public bool? AplicarOtrasSucursales
        {
            get
            {
                return this.chkAplicarSucursales.Checked;
            }
            set { this.chkAplicarSucursales.Checked = (bool)(value ?? false); }
        }

        public string Observaciones
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservaciones.Text.Trim())) ? null : this.txtObservaciones.Text.Trim().ToUpper();
            }
            set
            {
                this.txtObservaciones.Text = value ?? String.Empty;
            }
        }

        #region Propiedades Buscador

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

        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new RegistrarTarifaRDPRE(this, this.ucTarifaRD);

                if (!this.IsPostBack)
                {
                    this.presentador.PrepararNuevo();

                    this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,500);");
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al crear la página:", ETipoMensajeIU.ERROR, this.NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void LimpiarSesion()
        {
            if (Session["ListaSucursales"] != null)
                Session.Remove("ListaSucursales");
            if(Session["TarifaBO"] != null)
                Session.Remove("TarifaBO");
        }
        public void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipos)
        {
            ddlTipoTarifa.Items.Clear();
            ddlTipoTarifa.DataSource = tipos;
            ddlTipoTarifa.DataTextField = "Value";
            ddlTipoTarifa.DataValueField = "Key";
            ddlTipoTarifa.DataBind();
            ddlTipoTarifa.SelectedValue = "-1";
        }
        public void EstablecerOpcionesMoneda(Dictionary<string, string> monedas)
        {
            ddlMoneda.Items.Clear();
            ddlMoneda.DataSource = monedas;
            ddlMoneda.DataTextField = "Value";
            ddlMoneda.DataValueField = "Key";
            ddlMoneda.DataBind();
            ddlMoneda.SelectedValue = "-1";
        }
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
        public void MostrarCliente(bool activo)
        {
            this.pnlDatosCliente.Visible = activo;
        }
        public void ModoEdicion(bool activo)
        {
            this.ddlMoneda.Enabled = activo;
            this.txtSucursal.Enabled = activo;
            this.txtSucursalNoAplica.Enabled = activo;
            this.ddlTipoTarifa.Enabled = activo;
            this.txtCliente.Enabled = activo;
            this.txtDescripcion.Enabled = activo;
            this.txtModelo.Enabled = activo;
            this.txtVigencia.Enabled = activo;
        }
        public void ModoRegistrarTarifa(bool activo)
        {
            this.ddlMoneda.Enabled = !activo;
            this.ddlTipoTarifa.Enabled = !activo;
            this.txtSucursal.Enabled = !activo;
            this.ibtnBuscarSucursal.Visible = !activo;
            this.ibtnBuscarSucursal.Enabled = !activo;
            this.txtModelo.Enabled = !activo;
            this.ibtnBuscarModelo.Visible = !activo;
            this.ibtnBuscarModelo.Enabled = !activo;
            this.txtSucursalNoAplica.Enabled = activo;
            this.ibtnBuscarSucursalNoAplica.Visible = activo;
            this.txtDescripcion.Enabled = !activo;
            this.txtCliente.Enabled = !activo;
            this.ibtnBuscarCliente.Visible = !activo;
            this.txtVigencia.Enabled = activo;
            this.pnlCapturaTarifas.Visible = activo;
            this.btnCapturarTarifas.Visible = !activo;
        }
        public void MostrarAplicarSucursal(bool activo)
        {
            this.pnlAplicarSucursales.Visible = activo;
        }
        public void MostrarCapturaTarifas(bool activo)
        {
            this.pnlCapturaTarifas.Visible = activo;
        }
        public void ModoConsulta(bool activo)
        {
            this.txtCliente.Enabled = !activo;
            this.txtDescripcion.Enabled = !activo;
            this.txtModelo.Enabled = !activo;
            this.txtVigencia.Enabled = !activo;
            this.ddlMoneda.Enabled = !activo;
            this.txtSucursal.Enabled = !activo;
            this.txtSucursalNoAplica.Enabled = !activo;
            this.ddlTipoTarifa.Enabled = !activo;
            this.grdSucursales.Enabled = !activo;
            this.btnAgregar.Enabled = !activo;
            this.btnCancelar.Enabled = !activo;
            this.btnCapturarTarifas.Enabled = !activo;
            this.btnGuardar.Enabled = !activo;
            this.ibtnBuscarCliente.Enabled = !activo;
            this.ibtnBuscarModelo.Enabled = !activo;
            this.ibtnBuscarCliente.Visible = !activo;
            this.ibtnBuscarModelo.Visible = !activo;
            this.chkAplicarSucursales.Enabled = !activo;
            this.ibtnBuscarSucursal.Visible = !activo;
            ibtnBuscarSucursalNoAplica.Visible = !activo;
        }
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarTarifasRDUI.aspx"));
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleTarifaRDUI.aspx"));
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void PermitirAgregarSucursales(bool activo)
        {
            pnlSeccionSucursal.Visible = activo;
            chkAplicarSucursales.Checked = !activo;
        }
        public void EstablecerPaqueteNavegacion(object tarifa)
        {
            Session["TarifaBO"] = tarifa;
        }
        public void MostrarVigencia(bool activo)
        {
            this.pnlVigencia.Visible = activo;
            txtVigencia.Enabled = activo;
        }
        public void MostrarObservaciones(bool activo)
        {
            this.pnlObservaciones.Visible = activo;
            this.txtObservaciones.Enabled = activo;
        }

        public void MostrarLeyendaSucursales(bool mostrar, string leyenda)
        {
            this.lblLeyendaSucursales.Text = leyenda;
            this.btnLeyendaSucursales.Visible = mostrar;
        }

        #region Métodos para el Buscador

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            Session_BOSelecto = null;
        }

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
        /// metodo para registrar script en el cliente
        /// </summary>
        /// <param name="key"> llave del script que se va a registrar</param>
        /// <param name="script">script que se va a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al Cancelar el Registro de la Tarifa", ETipoMensajeIU.ERROR, this.NombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Guardar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al Guardar la Tarifa", ETipoMensajeIU.ERROR, this.NombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
        protected void btnCapturarTarifas_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CapturarTarifas();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al momento de intentar configurar la opci&oacute;n de capturar tarifas", ETipoMensajeIU.ERROR, this.NombreClase + ".btnCapturarTarifas_Click: " + ex.Message);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarSucursalNoAplicar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al agregar la sucursal", ETipoMensajeIU.ERROR, this.NombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }
        protected void chkAplicarSucursales_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AplicarOtrasSucursales();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al realizar la acción", ETipoMensajeIU.ERROR, this.NombreClase + ".chkAplicarSucursales_CheckedChanged: " + ex.Message);
            }
        }
        protected void ddlTipoTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                    presentador.CambioTipoTarifa();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al momento de cambiar el Tipo de Tarifa", ETipoMensajeIU.ERROR, this.NombreClase + ".ddlTipoTarifa_SelectedIndexChanged:" + ex.Message);
            }
        }
        protected void grdSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdSucursales.DataSource = ListaSucursalSeleccionada;
                grdSucursales.PageIndex = e.NewPageIndex;
                grdSucursales.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al momento de realizar la paginaci&oacuten de las sucursales", ETipoMensajeIU.ERROR, this.NombreClase + ".grdSucursales_PageIndexChanging:" + ex.Message);
            }
        }
        protected void grdSucursales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                SucursalBO sucursal = ListaSucursalSeleccionada[index];
                this.presentador.QuitarSucursal(sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar el evento", ETipoMensajeIU.ERROR, NombreClase + ".grdSucursales_RowCommand:" + ex.Message);
            }
        }
        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string modelo = NombreModelo;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                NombreModelo = modelo;
                if (NombreModelo != null)
                {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    NombreModelo = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar el Modelo", ETipoMensajeIU.ERROR,
                                    this.NombreClase + ".txtModelo_TextChanged:" + ex.Message);
            }
        }
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cliente = NombreCliente;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Cliente);

                NombreCliente = cliente;
                if (NombreCliente != null)
                {
                    this.EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.Cliente);
                    NombreCliente = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar el Cliente", ETipoMensajeIU.ERROR,
                                    this.NombreClase + ".txtCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaCliente()) != null)
            {
                this.MostrarMensaje("Se requiere los siguientes datos " + s.Substring(2), ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }
            try
            {
                this.EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.Cliente);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar el cliente", ETipoMensajeIU.ERROR, this.NombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }
        protected void ibtnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaModelo()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);//RI0046
                return;
            }
            try
            {
                this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar el modelo", ETipoMensajeIU.ERROR, this.NombreClase + ".ibtnBuscarModelo_Click:" + ex.Message);
            }
        }
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sucursal = NombreSucursal;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                NombreSucursal = sucursal;
                if (NombreSucursal != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    NombreSucursal = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar la sucursal", ETipoMensajeIU.ERROR,
                                    this.NombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarSucursal_Click" + ex.Message);
            }
        }

        protected void txtSucursalNoAplica_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sucursal = NombreSucursalNoAplica;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.SucursalNoAplica);

                NombreSucursalNoAplica = sucursal;
                if (NombreSucursalNoAplica != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.SucursalNoAplica);
                    NombreSucursalNoAplica = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar la sucursal", ETipoMensajeIU.ERROR,
                                    this.NombreClase + ".txtSucursalNoAplica_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscarSucursalNoAplica_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.SucursalNoAplica);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarSucursalNoAplica_Click:" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Cliente:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.SucursalNoAplica:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
    }
}