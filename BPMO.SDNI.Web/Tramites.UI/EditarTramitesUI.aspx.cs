//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class EditarTramitesUI : System.Web.UI.Page,IEditarTramitesVIS
    {
        #region Atributos
        private string nombreClase = "EditarTramitesUI";
        private EditarTramitesPRE presentador;
        #endregion
        #region Propiedades
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null)
                    id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? UsuarioId
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;

                if (master.Usuario != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
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
        public ETipoTramite? Tipo
        {
            get
            {
                ETipoTramite? tipo = null;
                if(Session["TipoTramite"]!=null)
                   tipo = (Session["TipoTramite"] as ETipoTramite?);

                return tipo;
            }
        }
        public ITramitable Tramitable
        {
            get
            {
                if (Session["Unidad"] == null)
                    return new TramitableProxyBO();
                else return (Session["Unidad"] as ITramitable);
            }
        }
        public object UltimoTramite
        {
            get
            {
                if (Session["Tramite"] == null)
                    return new object();
                else
                    return (Session["Tramite"] as object);
            }

        }
        public object Tramite
        {
            get
            {
                if (Session["NuevoTramite"] == null)
                    return new object();
                else
                    return (Session["NuevoTramite"] as object);
            }
            set
            {
                Session["NuevoTramite"] = value;
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
        public void LimpiarSesion()
        {
            if (Session["NuevoTramite"] != null)
                Session.Remove("NuevoTramite");
        }
        public void LimpiarSesionDatosNavegacion()
        {
            if (Session["Tramite"] != null)
                Session.Remove("Tramite");
            if (Session["TipoTramite"] != null)
                Session.Remove("TipoTramite");
            if (Session["Unidad"] != null)
                Session.Remove("Unidad");
        }
        public void EstablecerDatosNavegacion(string nombre, object objeto)
        {
            Session[nombre] = objeto;
        }
        public void MostrarTenencia()
        {
            this.pnlTenencia.Visible = true;
        }
        public void MostrarVerificacionAmbiental()
        {
            this.pnlVerificacionAmbiental.Visible = true;
        }
        public void MostrarVerificacionMecanico()
        {
            this.pnlVerificacionMecanico.Visible = true;
        }
        public void MostrarPlacaEstatal()
        {
            this.pnlPlacaEstatal.Visible = true;
        }
        public void MostrarPlacaFederal()
        {
            this.pnlPlacaFederal.Visible = true;
        }
        public void MostrarGPS()
        {
            this.pnlGPS.Visible = true;
        }
        public void MostrarFiltroAK()
        {
            this.pnlFiltro.Visible = true;
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/DetalleTramitesUI.aspx"), true);
        }

        #region SC0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion
        #endregion
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new EditarTramitesPRE(this,this.ucTramiteFiltroAK,this.ucTramiteGPS,this.ucTramitePlacalEstatal,this.ucTramitePlacalFederal,this.ucTramiteTenencia,this.ucTramiteVerificacionAmbiental,this.ucTramiteVerificacionMecanico);
                #region SC0008
                this.presentador.ValidarAcceso();
                #endregion
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "EditarTramites_" + DateTime.Now.Ticks.ToString(), "inicializarTramites();", true);
                if (!IsPostBack)
                {
                    presentador.Inicializar();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información",ETipoMensajeIU.ERROR,nombreClase +".Page_Load: " +ex.Message);
            }
        }

        protected void btnGuardarTenencia_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarTenencia();
        }

        protected void btnGuardarAmbiental_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarAmbiental();
        }

        protected void btnGuardarMecanico_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarMecanico();
        }

        protected void btnGuardarPlacaEstatal_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarPlacaEstatal();
        }

        protected void btnGuardarPlacaFederal_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarPlacaFederal();
        }

        protected void btnGuardarGPS_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarGPS();
        }

        protected void btnGuardarFiltro_Click(object sender, EventArgs e)
        {
            this.presentador.GuardarFiltro();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.presentador.RedirigirADetalle();
        }
        #endregion
    }
}