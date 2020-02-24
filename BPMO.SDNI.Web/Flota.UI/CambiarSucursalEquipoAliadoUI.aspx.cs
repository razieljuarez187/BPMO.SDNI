//Satisface al CU082 - Registrar Movimiento de Flota
using System;
using System.Configuration;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.UI
{
    public partial class CambiarSucursalEquipoAliadoUI : System.Web.UI.Page, ICambiarSucursalEquipoAliadoVIS
    {

        #region Atributos
        private CambiarSucursalEquipoAliadoPRE presentador;
        private string nombreClase = "CambiarSucursalEquipoAliadoUI";
        public enum ECatalogoBuscador
        {
            Sucursal,
            EquipoAliado
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del modulo en el que se encuentra trabajando con el fin de obtener las configuraciones necesarias para editar el contrato
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        public EquipoAliadoBO UltimoObjeto
        {
            get
            {
                if ((EquipoAliadoBO)Session["UltimoObjetoExpediente"] == null)
                    return new EquipoAliadoBO();
                else
                    return (EquipoAliadoBO)Session["UltimoObjetoExpediente"];
            }
            set
            {
                Session["UltimoObjetoExpediente"] = value;
            }
        }

        public int? UnidadOperativaID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnUnidadOperativaID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadOperativaID.Value))
                {
                    if (Int32.TryParse(this.hdnUnidadOperativaID.Value, out val))
                        return val;
                    else
                    {
                        Site masterMsj = (Site)Page.Master;

                        if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                            return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    }
                }
                else
                {
                    Site masterMsj = (Site)Page.Master;

                    if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                        return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadOperativaID.Value = value.Value.ToString();
                else
                    this.hdnUnidadOperativaID.Value = string.Empty;
            }
        }

        public int? EmpresaActualID
        {
            get
            {
                var val = 0;
                if (Int32.TryParse(this.hdnEmpresaActualID.Value, out val))
                    return val;
                return null;
            }
            set { this.hdnEmpresaActualID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public string NombreEmpresaActual
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEmpresaActual.Text) && !string.IsNullOrWhiteSpace(this.txtEmpresaActual.Text))
                    return this.txtEmpresaActual.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtEmpresaActual.Text = value;
                else
                    this.txtEmpresaActual.Text = string.Empty;
            }
        }

        public string DomicilioSucursalActual
        {
            get { return this.txtDomicilioActual.Text.Trim().ToUpper(); }
            set
            {
                this.txtDomicilioActual.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
            }
        }

        public int? SucursalActualID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnSucursalActualID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalActualID.Value))
                    if (Int32.TryParse(this.hdnSucursalActualID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalActualID.Value = value.Value.ToString();
                else
                    this.hdnSucursalActualID.Value = string.Empty;
            }
        }

        public string SucursalActualNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtSucursalActual.Text) && !string.IsNullOrWhiteSpace(this.txtSucursalActual.Text))
                    return this.txtSucursalActual.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtSucursalActual.Text = value;
                else
                    this.txtSucursalActual.Text = string.Empty;
            }
        }

        public int? EquipoAliadoID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnEquipoAliadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoAliadoID.Value))
                    if (Int32.TryParse(this.hdnEquipoAliadoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoAliadoID.Value = value.Value.ToString();
                else
                    this.hdnEquipoAliadoID.Value = string.Empty;
            }
        }

        public int? EquipoID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value))
                    if (Int32.TryParse(this.hdnEquipoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoID.Value = value.Value.ToString();
                else
                    this.hdnEquipoID.Value = string.Empty;
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

        public string Modelo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text))
                    return this.txtModelo.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }

        public int? ModeloID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value))
                    if (Int32.TryParse(this.hdnModeloID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.Value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }

        public string AnioModelo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtAnioModelo.Text) && !string.IsNullOrWhiteSpace(this.txtAnioModelo.Text))
                    return this.txtAnioModelo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtAnioModelo.Text = value;
                else
                    this.txtAnioModelo.Text = string.Empty;
            }
        }

        public decimal? PBV
        {
            get
            {
                decimal val;
                if (!string.IsNullOrEmpty(this.txtPBV.Text) && !string.IsNullOrWhiteSpace(this.txtPBV.Text))
                    if (Decimal.TryParse(this.txtPBV.Text.Trim().Replace(",", ""), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtPBV.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtPBV.Text = string.Empty;
            }
        }

        public decimal? PBC
        {
            get
            {
                decimal val;
                if (!string.IsNullOrEmpty(this.txtPBC.Text) && !string.IsNullOrWhiteSpace(this.txtPBC.Text))
                    if (Decimal.TryParse(this.txtPBC.Text.Trim().Replace(",", ""), out val)) //RI0012
                        return val;

                return null;
            }
            set
            {
                if (value != null)
                    this.txtPBC.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtPBC.Text = string.Empty;
            }
        }

        public string TipoEquipoNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTipoEquipo.Text) && !string.IsNullOrWhiteSpace(this.txtTipoEquipo.Text))
                    return this.txtTipoEquipo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtTipoEquipo.Text = value;
                else
                    this.txtTipoEquipo.Text = string.Empty;
            }
        }

        public int? TipoEquipoID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnTipoEquipoAliadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnTipoEquipoAliadoID.Value))
                    if (Int32.TryParse(this.hdnTipoEquipoAliadoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTipoEquipoAliadoID.Value = value.Value.ToString();
                else
                    this.hdnTipoEquipoAliadoID.Value = string.Empty;
            }
        }

        public string OracleID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnOracleID.Value) && !string.IsNullOrWhiteSpace(this.hdnOracleID.Value))
                    return this.hdnOracleID.Value.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.hdnOracleID.Value = value;
                    this.txtOracleID.Text = value;
                }
                else
                {
                    this.hdnOracleID.Value = string.Empty;
                    this.txtOracleID.Text = string.Empty;
                }
            }
        }

        public int? EquipoLiderID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnLiderID.Value))
                    id = int.Parse(this.hdnLiderID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnLiderID.Value = value.ToString();
                else
                    this.hdnLiderID.Value = string.Empty;
            }
        }

        public DateTime? FC
        {
            get { return DateTime.Now; }
        }

        public DateTime? FUA
        {
            get { return this.FC; }
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

        public int? UUA
        {
            get { return this.UC; }
        }
        /// <summary>
        /// Usuario del Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
            }
        }
        /// <summary>
        /// Unidad Operativa de Configurada
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                {
                    this.UnidadOperativaID = masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                }
                return null;
            }
        }

        public int? HorasIniciales
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.txtHorasIniciales.Text) && !string.IsNullOrWhiteSpace(this.txtHorasIniciales.Text))
                    if (Int32.TryParse(this.txtHorasIniciales.Text.Trim().Replace(",", ""), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtHorasIniciales.Text = string.Format("{0:#,##0}", value); //RI0012
                else
                    this.txtHorasIniciales.Text = string.Empty;
            }
        }

        public int? EstatusID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value) && !string.IsNullOrWhiteSpace(this.hdnEstatusID.Value))
                    if (Int32.TryParse(this.hdnEstatusID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnEstatusID.Value = value.Value.ToString();
                else
                    this.hdnEstatusID.Value = string.Empty;
            }
        }

        public string EstatusNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEstatusEquipo.Text) && !string.IsNullOrWhiteSpace(this.txtEstatusEquipo.Text))
                    return this.txtEstatusEquipo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtEstatusEquipo.Text = value;
                else
                    this.txtEstatusEquipo.Text = string.Empty;
            }
        }

        public int? SucursalDestinoID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnSucursalDestinoID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalDestinoID.Value))
                    if (Int32.TryParse(this.hdnSucursalDestinoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalDestinoID.Value = value.Value.ToString();
                else
                    this.hdnSucursalDestinoID.Value = string.Empty;
            }
        }

        public string SucursalDestinoNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtSucursalDestino.Text) && !string.IsNullOrWhiteSpace(this.txtSucursalDestino.Text))
                    return this.txtSucursalDestino.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtSucursalDestino.Text = value;
                else
                    this.txtSucursalDestino.Text = string.Empty;
            }
        }

        public int? EmpresaDestinoID
        {
            get
            {
                var val = 0;
                if (Int32.TryParse(this.hdnEmpresaDestinoID.Value, out val))
                    return val;
                return null;
            }
            set { this.hdnEmpresaDestinoID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public string NombreEmpresaDestino
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEmpresaDestino.Text) && !string.IsNullOrWhiteSpace(this.txtEmpresaDestino.Text))
                    return this.txtEmpresaDestino.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtEmpresaDestino.Text = value;
                else
                    this.txtEmpresaDestino.Text = string.Empty;
            }
        }

        public string DomicilioSucursalDestino
        {
            get { return this.txtDomicilioDestino.Text.Trim().ToUpper(); }
            set
            {
                this.txtDomicilioDestino.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                    ? value.Trim().ToUpper()
                                                    : string.Empty;
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

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new CambiarSucursalEquipoAliadoPRE(this);

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }

        }
        #endregion

        #region Métodos
        public void PrepararVista()
        {
            this.txtAnioModelo.Text = string.Empty;
            this.txtEmpresaActual.Text = string.Empty;
            this.txtDomicilioActual.Text = string.Empty;
            this.txtDomicilioDestino.Text = string.Empty;
            this.txtEmpresaDestino.Text = string.Empty;
            this.txtEstatusEquipo.Text = string.Empty;
            this.txtHorasIniciales.Text = string.Empty;
            this.txtMarca.Text = string.Empty;
            this.txtModelo.Text = string.Empty;
            this.txtNumeroSerie.Text = string.Empty;
            this.txtOracleID.Text = string.Empty;
            this.txtPBC.Text = string.Empty;
            this.txtPBV.Text = string.Empty;
            this.txtSucursalActual.Text = string.Empty;
            this.txtTipoEquipo.Text = string.Empty;
            this.hdnEquipoAliadoID.Value = string.Empty;
            this.hdnEquipoID.Value = string.Empty;
            this.hdnLiderID.Value = string.Empty;
            this.hdnModeloID.Value = string.Empty;
            this.hdnOracleID.Value = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.hdnTipoEquipoAliadoID.Value = string.Empty;
            this.hdnUnidadOperativaID.Value = string.Empty;

            this.txtAnioModelo.Enabled = false;
            this.txtEmpresaActual.Enabled = false;
            this.txtDomicilioActual.Enabled = false;
            this.txtDomicilioDestino.Enabled = false;
            this.txtEmpresaDestino.Enabled = false;
            this.txtEstatusEquipo.Enabled = false;
            this.txtHorasIniciales.Enabled = false;
            this.txtMarca.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtOracleID.Enabled = false;
            this.txtPBC.Enabled = false;
            this.txtPBV.Enabled = false;
            this.txtSucursalActual.Enabled = false;
            this.txtTipoEquipo.Enabled = false;
        }

        public void RedirigirAConsultarSeguimiento()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx"), false);
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.LimpiarSesion();
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }

        /// <summary>
        /// Habilita o deshabilita los controles de consulta
        /// </summary>
        /// <param name="status">Estatus que se desea proporcionar a los controles</param>
        public void PermitirConsultar(bool status)
        {
            this.hlkConsultarFlota.Enabled = status;
        }
        /// <summary>
        /// Habilita o deshabilita los controles de registro
        /// </summary>
        /// <param name="status">Estatus que se desea aplicar a los controles</param>
        public void PermitirRegistrar(bool status)
        {
            this.btnGuardar.Enabled = status;
        }

        public void LimpiarSesion()
        {
            if (Session["UltimoObjetoExpediente"] != null)
                this.Session.Remove("UltimoObjetoExpediente");
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
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
        #region Eventos del Buscador
        protected void txtEquipoAliado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.NumeroSerie;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.EquipoAliado);

                this.NumeroSerie = numeroSerie;
                if (this.NumeroSerie != null)
                {
                    this.EjecutaBuscador("EquipoAliado", ECatalogoBuscador.EquipoAliado);
                    this.NumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtEquipoAliado_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscaEquipoAliado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("EquipoAliado&hidden=0", ECatalogoBuscador.EquipoAliado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Equipo Aliado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaEquipoAliado_Click:" + ex.Message);
            }
        }
        protected void txtSucursalDestino_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = SucursalDestinoNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalDestinoNombre = nombreSucursal;
                if (SucursalDestinoNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    SucursalDestinoNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursalDestino_TextChanged" + ex.Message);
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
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.EquipoAliado:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CambiarSucursal();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar el movimiento de la unidad", ETipoMensajeIU.ERROR, string.Format("{0}.btnGuardar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el movimiento de la unidad", ETipoMensajeIU.ERROR, string.Format("{0}.btnCancelar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion
    }
}