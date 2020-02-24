//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class DetalleTramitesUI : System.Web.UI.Page, IDetalleTramitesVIS
    {
        #region Atributos
        private string nombreClase = "DetalleTramitesUI";
        private DetalleTramitesPRE presentador;
        #endregion

        #region Propiedades
        public string NumeroSerie
        {
            get
            {
                TextBox txtNumeroSerie = mTramites.Controls[0].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtNumeroSerie.Text.Trim()))
                    return txtNumeroSerie.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                TextBox txtNumeroSerie = mTramites.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroSerie != null)
                {
                    if (value != null)
                        txtNumeroSerie.Text = value.ToString();
                    else txtNumeroSerie.Text = string.Empty;
                }
            }
        }
        public string Modelo
        {
            get
            {
                TextBox txtModelo = mTramites.Controls[1].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtModelo.Text.Trim()))
                    return txtModelo.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                TextBox txtModelo = mTramites.Controls[1].FindControl("txtValue") as TextBox;
                if (txtModelo != null)
                {
                    if (value != null)
                        txtModelo.Text = value.ToString();
                    else txtModelo.Text = string.Empty;
                }
            }
        }
        public string Marca
        {
            get
            {
                TextBox txtMarca = mTramites.Controls[2].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtMarca.Text.Trim()))
                    return txtMarca.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                TextBox txtMarca = mTramites.Controls[2].FindControl("txtValue") as TextBox;
                if (txtMarca != null)
                {
                    if (value != null)
                        txtMarca.Text = value.ToString();
                    else txtMarca.Text = string.Empty;
                }
            }
        }
        public ITramitable Tramitable
        {
            get
            {
                if (Session["DatosTramitable"] == null)
                    return new TramitableProxyBO();
                else
                    return (ITramitable)Session["DatosTramitable"];
            }
            set
            {
                Session["DatosTramitable"] = value;
            }
        }
        public TenenciaBO Tenencia
        {
            get
            {
                if (Session["Tenencia"] == null)
                    return new TenenciaBO();
                else return (TenenciaBO)Session["Tenencia"];
            }
            set
            {
                Session["Tenencia"] = value;
            }
        }
        public VerificacionBO VerificacionAmbiental
        {
            get
            {
                if (Session["VerificacionAmbiental"] == null)
                    return new VerificacionBO();
                else
                    return (VerificacionBO)Session["VerificacionAmbiental"];
            }
            set
            {
                Session["VerificacionAmbiental"] = value;
            }

        }
        public VerificacionBO VerificacionMecanica
        {
            get
            {
                if (Session["VerificacionMecanica"] == null)
                    return new VerificacionBO();
                else
                    return (VerificacionBO)Session["VerificacionMecanica"];
            }
            set
            {
                Session["VerificacionMecanica"] = value;
            }
        }
        public PlacaEstatalBO PlacaEstatal
        {
            get
            {
                if (Session["PlacaEstatal"] == null)
                    return new PlacaEstatalBO();
                else
                    return (PlacaEstatalBO)Session["PlacaEstatal"];
            }
            set
            {
                Session["PlacaEstatal"] = value;
            }
        }
        public PlacaFederalBO PlacaFederal
        {
            get
            {
                if (Session["PlacaFederal"] == null)
                    return new PlacaFederalBO();
                else
                    return (PlacaFederalBO)Session["PlacaFederal"];
            }
            set
            {
                Session["PlacaFederal"] = value;
            }

        }
        public GPSBO GPS
        {
            get
            {
                if (Session["GPS"] == null)
                    return new GPSBO();
                else
                    return (GPSBO)Session["GPS"];
            }
            set
            {
                Session["GPS"] = value;
            }
        }
        public FiltroAKBO FiltroAK
        {
            get
            {
                if (Session["Filtro"] == null)
                    return new FiltroAKBO();
                else
                    return (FiltroAKBO)Session["Filtro"];
            }
            set
            {
                Session["Filtro"] = value;
            }

        }
        public SeguroBO Seguro
        {
            get
            {
                if (Session["SeguroTramiteDetalle"] == null)
                    return new SeguroBO();
                else
                    return Session["SeguroTramiteDetalle"] as SeguroBO;
            }
            set
            {
                Session["SeguroTramiteDetalle"] = value;
            }

        }
        public string Poliza
        {
            get { return (String.IsNullOrEmpty(this.txtPoliza.Text.Trim())) ? null : this.txtPoliza.Text.Trim().ToUpper(); }
            set { this.txtPoliza.Text = value != null ? value : string.Empty; }
        }
        public string Aseguradora
        {
            get { return (String.IsNullOrEmpty(this.txtAseguradora.Text.Trim())) ? null : this.txtAseguradora.Text.Trim().ToUpper(); }
            set { this.txtAseguradora.Text = value != null ? value : string.Empty; }
        }
        public DateTime? FechaInicial
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicial.Text.Trim()))
                    fecha = Convert.ToDateTime(this.txtFechaInicial.Text);
                return fecha;
            }
            set
            {
                this.txtFechaInicial.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public DateTime? FechaFinal
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaFinal.Text.Trim()))
                    fecha = Convert.ToDateTime(this.txtFechaFinal.Text);
                return fecha;
            }
            set
            {
                this.txtFechaFinal.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public List<TenenciaBO> Tenencias
        {
            get
            {
                if (Session["ListaTenencias"] == null)
                    return new List<TenenciaBO>();
                else return (List<TenenciaBO>)Session["ListaTenencias"];
            }
            set
            {
                Session["ListaTenencias"] = value;
            }
        }
        public List<VerificacionBO> VerificacionesAmbientales
        {
            get
            {
                if (Session["ListaVerificacionAmbiental"] == null)
                    return new List<VerificacionBO>();
                else
                    return (List<VerificacionBO>)Session["ListaVerificacionAmbiental"];
            }
            set
            {
                Session["ListaVerificacionAmbiental"] = value;
            }

        }
        public List<VerificacionBO> VerificacionesMecanicas
        {
            get
            {
                if (Session["ListaVerificacionMecanica"] == null)
                    return new List<VerificacionBO>();
                else
                    return (List<VerificacionBO>)Session["ListaVerificacionMecanica"];
            }
            set
            {
                Session["ListaVerificacionMecanica"] = value;
            }
        }
        public List<PlacaEstatalBO> PlacasEstatales
        {
            get
            {
                if (Session["ListaPlacaEstatal"] == null)
                    return new List<PlacaEstatalBO>();
                else
                    return (List<PlacaEstatalBO>)Session["ListaPlacaEstatal"];
            }
            set
            {
                Session["ListaPlacaEstatal"] = value;
            }
        }
        public List<PlacaFederalBO> PlacasFederales
        {
            get
            {
                if (Session["ListaPlacaFederal"] == null)
                    return new List<PlacaFederalBO>();
                else
                    return (List<PlacaFederalBO>)Session["ListaPlacaFederal"];
            }
            set
            {
                Session["ListaPlacaFederal"] = value;
            }

        }
        public List<GPSBO> ListaGPS
        {
            get
            {
                if (Session["ListaGPS"] == null)
                    return new List<GPSBO>();
                else
                    return (List<GPSBO>)Session["ListaGPS"];
            }
            set
            {
                Session["ListaGPS"] = value;
            }
        }
        public List<FiltroAKBO> FiltroAKs
        {
            get
            {
                if (Session["ListaFiltro"] == null)
                    return new List<FiltroAKBO>();
                else
                    return (List<FiltroAKBO>)Session["ListaFiltro"];
            }
            set
            {
                Session["ListaFiltro"] = value;
            }

        }

        #region SC_0008
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? UsuarioId
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
        #endregion

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new DetalleTramitesPRE(this, this.ucTramiteFiltroAK, this.ucTramiteGPS, this.ucTramitePlacaEstatal, this.ucTramitePlacaFederal, this.ucTramiteTenencia, this.ucTramiteVerificacionAmbiental, this.ucTramiteVerificacionMecanica);
                if (!IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    presentador.Inicializar();
                }
                this.DesactivarBotones();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            Site master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);

            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        public void EstablecerDatosNavegacion(string nombre, object unidad, string nombreTramite, object tramite, string nombreTipo, ETipoTramite tipo)
        {
            Session[nombre] = unidad;
            Session[nombreTramite] = tramite;
            Session[nombreTipo] = tipo;
        }
        public void EstablecerDatosNavegacionSeguro(string nombre, object unidad)
        {
            Session[nombre] = unidad;
        }

        public void LimpiarSesion()
        {
            if (Session["GPS"] != null)
                Session.Remove("GPS");
            if (Session["Tenencia"] != null)
                Session.Remove("Tenencia");
            if (Session["VerificacionAmbiental"] != null)
                Session.Remove("VerificacionAmbiental");
            if (Session["VerificacionMecanica"] != null)
                Session.Remove("VerificacionMecanica");
            if (Session["PlacaEstatal"] != null)
                Session.Remove("PlacaEstatal");
            if (Session["PlacaFederal"] != null)
                Session.Remove("PlacaFederal");
            if (Session["Filtro"] != null)
                Session.Remove("Filtro");
            if (Session["ListaTenencias"] != null)
                Session.Remove("ListaTenencias");
            if (Session["ListaVerificacionAmbiental"] != null)
                Session.Remove("ListaVerificacionAmbiental");
            if (Session["ListaVerificacionMecanica"] != null)
                Session.Remove("ListaVerificacionMecanica");
            if (Session["ListaPlacaEstatal"] != null)
                Session.Remove("ListaPlacaEstatal");
            if (Session["ListaPlacaFederal"] != null)
                Session.Remove("ListaPlacaFederal");
            if (Session["ListaGPS"] != null)
                Session.Remove("ListaGPS");
            if (Session["ListaFiltro"] != null)
                Session.Remove("ListaFiltro");
        }

        public void RedirigirARegistrar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/RegistrarTramitesUI.aspx"));
        }
        public void RedigirAEdicion()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/EditarTramitesUI.aspx"));
        }
        public void RedirigirARegistrarSeguro()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/RegistrarSeguroUI.aspx"));
        }
        public void RedirigirAEditarSeguro()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/EditarSeguroUI.aspx"));
        }

        public void EstablecerPagina(int numeroPagina)
        {
            this.mvCU087.SetActiveView((View)this.mvCU087.FindControl("vwPagina" + numeroPagina.ToString()));
            this.hdnPagina.Value = numeroPagina.ToString();
        }

        public void MostrarTenencias()
        {
            this.grdTenencias.DataSource = this.Tenencias;
            this.grdTenencias.DataBind();
        }
        public void MostrarGPSs()
        {
            this.grdGPS.DataSource = this.ListaGPS;
            this.grdGPS.DataBind();
        }
        public void MostrarFiltros()
        {
            this.grdFiltro.DataSource = this.FiltroAKs;
            this.grdFiltro.DataBind();
        }
        public void MostrarPlacasFederales()
        {
            this.grdPlacaFederal.DataSource = this.PlacasFederales;
            this.grdPlacaFederal.DataBind();
        }
        public void MostrarPlacaEstatales()
        {
            this.grdPlacaEstatal.DataSource = this.PlacasEstatales;
            this.grdPlacaEstatal.DataBind();
        }
        public void MostrarVerificacionAmbiental()
        {
            grdVerificacionAmbiental.DataSource = this.VerificacionesAmbientales;
            grdVerificacionAmbiental.DataBind();
        }
        public void MostrarVerificacionMecanico()
        {
            grdVerificacionMecanico.DataSource = this.VerificacionesMecanicas;
            grdVerificacionMecanico.DataBind();
        }

        public void DesactivarBotones()
        {
            if (this.Tenencia == null || this.Tenencia.TramiteID == null)
            {
                this.btnEditarTenencia.Enabled = false;
                this.btnVerTenencia.Enabled = false;
            }
            if (this.VerificacionAmbiental == null || this.VerificacionAmbiental.TramiteID == null)
            {
                this.btnEditarVerificacionAmbiental.Enabled = false;
                this.btnVerVerificacionAmbiental.Enabled = false;
            }
            if (this.VerificacionMecanica == null || this.VerificacionMecanica.TramiteID == null)
            {
                this.btnEditarVerificacionMecanico.Enabled = false;
                this.btnVerVerificacionMecanica.Enabled = false;
            }
            if (this.PlacaEstatal == null || this.PlacaEstatal.TramiteID == null)
            {
                this.btnEditarPlacaEstatal.Enabled = false;
                this.btnVerPlacaEstatal.Enabled = false;
            }
            if (this.PlacaFederal == null || this.PlacaFederal.TramiteID == null)
            {
                this.btnEditarPlacaFederal.Enabled = false;
                this.btnVerPlacaFederal.Enabled = false;
            }            
            else if (!this.PlacaFederal.Activo.Value)
            {
                this.btnEditarPlacaFederal.Enabled = false;
            }            
            if (this.GPS == null || this.GPS.TramiteID == null)
            {
                this.btnEditarGPS.Enabled = false;
                this.btnVerGPS.Enabled = false;
            }
            if (this.FiltroAK == null || this.FiltroAK.TramiteID == null)
            {
                this.btnEditarFiltro.Enabled = false;
                this.btnVerFiltro.Enabled = false;
            }
            if (this.Poliza == null && this.Aseguradora == null && this.FechaInicial == null && this.FechaFinal == null)
            {
                this.btnEditarSeguro.Enabled = false;
            }
        }

        #region SC0008
        public void PermitirEditar(bool permitir)
        {
            this.btnEditarTenencia.Enabled = permitir;
            this.btnEditarVerificacionAmbiental.Enabled = permitir;
            this.btnEditarVerificacionMecanico.Enabled = permitir;
            this.btnEditarPlacaEstatal.Enabled = permitir;
            this.btnEditarPlacaFederal.Enabled = permitir;
            this.btnEditarGPS.Enabled = permitir;
            this.btnEditarFiltro.Enabled = permitir;
            this.btnEditarSeguro.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir)
        {
            this.btnRegistrarNuevoTenencia.Enabled = permitir;
            this.btnRegistrarNuevoVerificacionAmbiental.Enabled = permitir;
            this.btnRegistrarVerificacionMecanico.Enabled = permitir;
            this.btnRegistrarPlacaEstatal.Enabled = permitir;
            this.btnRegistrarPlacaFederal.Enabled = permitir;
            this.btnRegistrarGPS.Enabled = permitir;
            this.btnRegistrarFiltro.Enabled = permitir;
            this.btnRegistrarSeguro.Enabled = permitir;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnRegistrarNuevoVerificacionAmbiental_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.VERIFICACION_AMBIENTAL);
        }

        protected void btnEditarVerificacionAmbiental_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.VERIFICACION_AMBIENTAL);
        }

        protected void btnVerVerificacionAmbiental_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaVerificacionAmbiental();
            if (this.VerificacionesAmbientales.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerVerificacionAmbiental_" + DateTime.Now.Ticks.ToString(), "DialogVerificacionAmbiental();", true);
            }
        }

        protected void btnRegistrarVerificacionMecanico_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.VERIFICACION_FISICO_MECANICA);
        }

        protected void btnEditarVerificacionMecanico_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.VERIFICACION_FISICO_MECANICA);
        }

        protected void btnVerVerificacionMecanica_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaVerificacionMecanica();
            if (this.VerificacionesMecanicas.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerVerificacionMecanica_" + DateTime.Now.Ticks.ToString(), "DialogMecanico();", true);
            }
        }

        protected void btnRegistrarPlacaEstatal_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.PLACA_ESTATAL);
        }

        protected void btnEditarPlacaEstatal_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.PLACA_ESTATAL);
        }

        protected void btnVerPlacaEstatal_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaPlacaEstatal();
            if (this.PlacasEstatales.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerPlacaEstatal_" + DateTime.Now.Ticks.ToString(), "DialogPlacaEstatal();", true);
            }
        }

        protected void btnRegistrarPlacaFederal_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.PLACA_FEDERAL);
        }

        protected void btnEditarPlacaFederal_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.PLACA_FEDERAL);
        }

        protected void btnVerPlacaFederal_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaPlacaFederal();
            if (this.PlacasFederales.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerPlacaFederal_" + DateTime.Now.Ticks.ToString(), "DialogPlacaFederal();", true);
            }
        }

        protected void btnRegistrarGPS_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.GPS);
        }

        protected void btnEditarGPS_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.GPS);
        }

        protected void btnVerGPS_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaGPS();
            if (this.ListaGPS.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerGPS_" + DateTime.Now.Ticks.ToString(), "DialogGPS();", true);
            }
        }

        protected void btnRegistrarFiltro_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.FILTRO_AK);
        }

        protected void btnEditarFiltro_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.FILTRO_AK);
        }

        protected void btnVerFiltro_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaFiltro();
            if (this.FiltroAKs.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerFiltro_" + DateTime.Now.Ticks.ToString(), "DialogFiltro();", true);
            }
        }

        protected void btnTenencia_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(0);
        }

        protected void btnVerificacionAmbiental_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(1);
        }

        protected void btnVerificacionMecanica_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(2);
        }

        protected void btnPlacaEstatal_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(3);
        }

        protected void btnPlacaFederal_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(4);
        }

        protected void btnGPS_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(5);
        }

        protected void btnFiltro_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(6);
        }
        protected void grdTenencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTenencias.DataSource = this.Tenencias;
                grdTenencias.PageIndex = e.NewPageIndex;
                grdTenencias.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdTenencias_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdVerificacionAmbiental_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdVerificacionAmbiental.DataSource = this.VerificacionesAmbientales;
                grdVerificacionAmbiental.PageIndex = e.NewPageIndex;
                grdVerificacionAmbiental.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdVerificacionAmbiental_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdVerificacionMecanico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdVerificacionMecanico.DataSource = this.VerificacionesMecanicas;
                grdVerificacionMecanico.PageIndex = e.NewPageIndex;
                grdVerificacionMecanico.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdVerificacionMecanico_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdPlacaEstatal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPlacaEstatal.DataSource = this.PlacasEstatales;
                grdPlacaEstatal.PageIndex = e.NewPageIndex;
                grdPlacaEstatal.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdPlacaEstatal_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdPlacaFederal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPlacaFederal.DataSource = this.PlacasFederales;
                grdPlacaFederal.PageIndex = e.NewPageIndex;
                grdPlacaFederal.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdPlacaFederal_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdGPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdGPS.DataSource = this.ListaGPS;
                grdGPS.PageIndex = e.NewPageIndex;
                grdGPS.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdGPS_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdFiltro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFiltro.DataSource = this.FiltroAKs;
                grdFiltro.PageIndex = e.NewPageIndex;
                grdFiltro.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdFiltro_PageIndexChanging: " + ex.Message);
            }
        }

        protected void btnVerTenencia_Click(object sender, EventArgs e)
        {
            presentador.MostrarListaTenencias();
            if (this.Tenencias.Count != 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "IbtnVerTenencia_" + DateTime.Now.Ticks.ToString(), "DialogTenencia();", true);
            }
        }

        protected void btnEditarTenencia_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEdicion(ETipoTramite.TENENCIA);
        }

        protected void btnRegistrarNuevoTenencia_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrar(ETipoTramite.TENENCIA);
        }

        protected void btnSeguro_Click(object sender, EventArgs e)
        {
            presentador.EstablecerPagina(7);
        }

        protected void btnRegistrarSeguro_Click(object sender, EventArgs e)
        {
            presentador.RedirigirARegistrarSeguro();
        }

        protected void btnEditarSeguro_Click(object sender, EventArgs e)
        {
            presentador.RedirigirAEditarSeguro();
        }
        #endregion
    }
}