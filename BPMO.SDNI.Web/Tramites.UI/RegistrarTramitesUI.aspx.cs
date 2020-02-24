//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Comun.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class RegistrarTramitesUI : System.Web.UI.Page, IRegistrarTramitesVIS
    {

        #region Atributo
        private readonly string nombreClase = "RegistrarTramitesUI.cs";
        private RegistrarTramitesPRE presentador = null;
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
            get { return this.UC; }
        }
        public ETipoTramite? TipoTramite
        {
            get
            {
                return Session["TipoTramite"] as ETipoTramite?;
            }
            set
            {
                Session["TipoTramite"] = value;
            }
        }
        public List<TipoArchivoBO> TiposArchivo
        {
            get 
            {
                if (Session["ListaArchivosTramites"] == null)
                    return new List<TipoArchivoBO>();
                else
                    return Session["ListaArchivosTramites"] as List<TipoArchivoBO>;
            }
            set
            {
                Session["ListaArchivosTramites"] = value;
            }
        }
        public Object Tramite
        {
            get
            {
                if(Session["Tramite"]!=null)
                return Session["Tramite"] as Object;
                return null;
            }
            set
            {
                Session["Tramite"] = value;
            }
        }
        public ITramitable Tramitable
        {
            get
            {
                if (Session["Tramitable"] == null)
                    return new TramitableProxyBO();

                return (ITramitable)Session["Tramitable"];
            }
            set
            {
                Session["Tramitable"] = value;
            }
        }
        public string NumSerie
        {
            get
            {
                var numVIN = mnTramites.Controls[0].FindControl("txtValue") as TextBox;
                if (numVIN != null && numVIN.Text != null && !string.IsNullOrEmpty(numVIN.Text.Trim()))
                    return numVIN.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                var txtNumeroContrato = mnTramites.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroContrato != null)
                {
                    txtNumeroContrato.Text = value ?? string.Empty;
                }
            }
        }
        public string Modelo
        {
            get
            {
                TextBox txtModelo = mnTramites.Controls[1].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtModelo.Text.Trim()))
                    return txtModelo.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                TextBox txtModelo = mnTramites.Controls[1].FindControl("txtValue") as TextBox;
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
                TextBox txtMarca = mnTramites.Controls[2].FindControl("txtValue") as TextBox;
                if (String.IsNullOrEmpty(txtMarca.Text.Trim()))
                    return txtMarca.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                TextBox txtMarca = mnTramites.Controls[2].FindControl("txtValue") as TextBox;
                if (txtMarca != null)
                {
                    if (value != null)
                        txtMarca.Text = value.ToString();
                    else txtMarca.Text = string.Empty;
                }
            }
        }
        public DateTime? FC
        {
            get { return DateTime.Today; }
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
                Site master = (Site)Page.Master;
                if (master.Usuario.Id != null)
                    id = master.Usuario.Id;

                return id;
            }
        }
        public int? UUA
        {
            get { return this.UC; }
        }

        #endregion

        #region Métodos
        public void CambiarControl()
        {
            switch (TipoTramite)
            {
                case ETipoTramite.FILTRO_AK:
                    mvTramites.SetActiveView(TramiteFiltroAK);
                    ucTramiteFiltroAK.Presentador.Inicializar();
                    break;

                case ETipoTramite.GPS:
                    this.mvTramites.SetActiveView(TramiteGPS);
                    ucTramiteGPS.Presentador.Inicializar();
                    break;

                case ETipoTramite.PLACA_FEDERAL:
                    this.mvTramites.SetActiveView(TramitePlaca);
                    ucTramitePlacal.Presentador.Inicializar(TipoTramite);
                    break;
                case ETipoTramite.PLACA_ESTATAL:
                    this.mvTramites.SetActiveView(TramitePlacaEstatal);
                    this.UcTramitePlacalEstatal.Presentador.Inicializar(TipoTramite);
                    break;

                case ETipoTramite.TENENCIA:
                    this.mvTramites.SetActiveView(TramiteTenencia);
                    ucTramiteTenencia.Presentador.Inicializar();
                    ucTramiteTenencia.Presentador.ModoEdicion(true);
                    ucTramiteTenencia.Presentador.EstablecerTiposdeArchivo(this.TiposArchivo);
                    break;

                case ETipoTramite.VERIFICACION_FISICO_MECANICA:
                    this.mvTramites.SetActiveView(TramiteVerificacion);
                    ETipoVerificacion tipo = ETipoVerificacion.FISICO_MECANICO;
                    ucTramiteVerificacion.Presentador.Inicializar(tipo);
                    ucTramiteVerificacion.Presentador.ModoEdicion(true);
                    ucTramiteVerificacion.Presentador.LimpiarSesion();
                    ucTramiteVerificacion.Presentador.EstablecerTiposdeArchivo(this.TiposArchivo);

                    break;
                case ETipoTramite.VERIFICACION_AMBIENTAL:
                    this.mvTramites.SetActiveView(TramiteVerificacionAmbiental);
                    ETipoVerificacion tipoA = ETipoVerificacion.AMBIENTAL;
                    ucTramiteVerificacionAmbiental.Presentador.Inicializar(tipoA);
                    ucTramiteVerificacionAmbiental.Presentador.ModoEdicion(true);
                    ucTramiteVerificacionAmbiental.Presentador.LimpiarSesion();
                    ucTramiteVerificacionAmbiental.Presentador.EstablecerTiposdeArchivo(this.TiposArchivo);
                    
                    break;

                default:
                    break;
            }
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            Site master = (Site)this.Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        public void LimpiarSesionTramite()
        {
            Session.Remove("TipoTramite");
            if (Session["ListaArchivosTramites"] != null)
                Session.Remove("ListaArchivosTramites");
        }
        public void LimpiarSesionTramitable()
        {
            Session.Remove("Tramitable");
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Tramites.UI/DetalleTramitesUI.aspx"));
        }
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
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
                ucTramiteFiltroAK.Presentador = new ucTramiteFiltroAKPRE(ucTramiteFiltroAK);
                ucTramiteGPS.Presentador = new ucTramiteGPSPRE(ucTramiteGPS);
                ucTramitePlacal.Presentador = new ucTramitePlacaPRE(ucTramitePlacal);
                ucTramiteTenencia.Presentador = new ucTramiteTenenciaPRE(ucTramiteTenencia);
                ucTramiteVerificacion.Presentador = new ucTramiteVerificacionPRE(ucTramiteVerificacion);
                ucTramiteVerificacionAmbiental.Presentador = new ucTramiteVerificacionPRE(ucTramiteVerificacionAmbiental);
                UcTramitePlacalEstatal.Presentador = new ucTramitePlacaPRE(UcTramitePlacalEstatal);



                presentador = new RegistrarTramitesPRE(this, ucTramiteFiltroAK.Presentador, ucTramiteGPS.Presentador,
                    ucTramitePlacal.Presentador, ucTramiteTenencia.Presentador, ucTramiteVerificacion.Presentador, ucTramiteVerificacionAmbiental.Presentador, UcTramitePlacalEstatal.Presentador);

                #region SC0008
                this.presentador.ValidarAcceso();
                #endregion

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RegistrarTramites_" + DateTime.Now.Ticks.ToString(), "inicializarTramites();", true);
                if (!Page.IsPostBack)
                {
                    presentador.EstablecerListaArchivos();
                    CambiarControl();

                    presentador.AsignarNumVIN(Tramitable);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.presentador.RedirigirADetalle();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            this.presentador.RegistrarTramite(this.TipoTramite.Value);

        }
        #endregion
    }
}