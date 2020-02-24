//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ConsultarTramitesUI : System.Web.UI.Page,IConsultarTramitesVIS
    {
        #region Atributos
        private string nombreClase = "ConsultarTramitesUI";
        private ConsultarTramitesPRE presentador;

        public enum ECatalogoBuscador
        {
            Unidad
        }
        #endregion

        #region Propiedades
        public List<UnidadBO> Tramitables
        {
            get 
            {
                if (Session["unidades"] == null)
                    return new List<UnidadBO>();
                else
                    return (List<UnidadBO>)Session["unidades"];
            }
            set
            {
                Session["unidades"] = value;
            }
        }
        public string NumeroSerie
        {
            get { return (String.IsNullOrEmpty(this.txtNumeroSerie.Text.Trim())) ? null : this.txtNumeroSerie.Text.ToUpper(); }
            set { this.txtNumeroSerie.Text = value != null ? value : string.Empty; }
        }
        public int? TramitableID
        {
            get 
            { 
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTramitableID.Value.Trim()))
                    id = Convert.ToInt32(this.hdnTramitableID.Value.Trim());
                return id;
            }
            set
            {
                this.hdnTramitableID.Value = value != null ? value.ToString() : string.Empty;
            }
        }

        #region Propiedades Buscador
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

        #region SC0008
        public int? UsuarioId
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
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarTramitesPRE(this);
                if (!IsPostBack)
                {
                    presentador.Inicializar();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarDatos()
        {
            this.grdUnidades.DataSource = this.Tramitables;
            this.grdUnidades.DataBind();
        }
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/DetalleTramitesUI.aspx"));
        }
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }

        public void LimpiarSesion()
        {
            if (Session["unidades"] != null)
                Session.Remove("unidades");

        }

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

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
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
        #endregion

        #region SC0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.presentador.ConsultarTramitables();
        }

        protected void grdUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUnidades.DataSource = this.Tramitables;
                grdUnidades.PageIndex = e.NewPageIndex;
                grdUnidades.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdClientes_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdUnidades_RowCommand(object sender, GridViewCommandEventArgs e)
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
                            this.presentador.VerDetalles(index);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el cliente", ETipoMensajeIU.ERROR, this.nombreClase + ".grdClientes_RowCommand:" + ex.Message);
            }
        }

        #region Eventos Buscador
        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al buscar un cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }
        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.NumeroSerie;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.Unidad);
                this.NumeroSerie = numeroSerie;
                if (this.NumeroSerie != null)
                {
                    EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar la Unidad", ETipoMensajeIU.ERROR, nombreClase + ".txtNumeroSerie_TextChanged:" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
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