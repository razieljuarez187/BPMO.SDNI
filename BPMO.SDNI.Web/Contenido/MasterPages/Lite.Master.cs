//Satisface al CU062 - Menú Principal
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BPMO.Basicos.BO;
using BPMO.Security.BO;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.MapaSitio.PRE;
using BPMO.SDNI.MapaSitio.VIS;

namespace BPMO.SDNI.MapaSitio.UI
{
    public partial class Lite : System.Web.UI.MasterPage,IMasterPageVIS
    {
        #region Atributos
        MasterPagePRE presentador = null;

        string paginaInicio = "~/MapaSitio.UI/MenuPrincipalUI.aspx";
        string paginaLogin = "~/Default.aspx";
        string paginaCambioAdscripcion = "~/Seguridad.Acceso.UI/ConfiguracionAccesoUI.aspx";
        #endregion

        #region Propiedades
        public List<DatosConexionBO> ListadoDatosConexion
        {
            get
            {
                return this.Session["DatosConexion"] != null ? (List<DatosConexionBO>)this.Session["DatosConexion"] : null;
            }
            set
            {
                this.Session["DatosConexion"] = value;
            }
        }

        public UsuarioBO Usuario
        {
            get
            {
                return this.Session["Usuario"] != null ? (UsuarioBO)this.Session["Usuario"] : null;
            }
            set
            {
                this.Session["Usuario"] = 2089;
            }
        }
        public AdscripcionBO Adscripcion
        {
            get
            {
                return this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
            }
            set
            {
                this.Session["Adscripcion"] = value;
            }
        }
        public List<ProcesoBO> ListadoProcesos
        {
            get
            {
                return this.Session["lstProcesos"] != null ? (List<ProcesoBO>)this.Session["lstProcesos"] : null;
            }
            set
            {
                this.Session["lstProcesos"] = value;
            }
        }
        public string Ambiente
        {
            get
            {
                return this.Session["EstiloCss"] != null ? this.Session["EstiloCss"].ToString() : null;
            }
            set
            {
                this.Session["EstiloCss"] = value;
            }
        }

        public int? NumeroFilas
        {
            get
            {
                return (this.Session["NUMERO_FILAS"] == null) ? null : (int?)this.Session["NUMERO_FILAS"];
            }
            set
            {
                if (value != null)
                    this.Session.Add("NUMERO_FILAS", value);
                else
                    this.Session.Remove("NUMERO_FILAS");
            }
        }
        public bool MostrarMenu
        {
            set
            {
                this.hdnMostrarMenu.Value = value.ToString();
            }
        }
        private string Logueo
        {
            get { return "~/Default.aspx"; }//return ConfigurationManager.AppSettings["Logueo"]; }
        }
        
        public string URLLogoEmpresa
        {
            get
            {
                string s = this.ResolveUrl("~/Contenido/Imagenes/LogoIdealease.png");

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO)
                    if (((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).URLLogoEmpresa != null)
                        return ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).URLLogoEmpresa;

                return s;
            }
        }
        public string DireccionCSS
        {
            get
            {
                string s = this.ResolveUrl("~/Contenido/Estilos/");

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO)
                    if (((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).DireccionCSS != null)
                        return ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).DireccionCSS;

                return s;
            }
        }
        public string NombreSistema
        {
            get
            {
                string s = "Sistema de Negocio de Idealease";

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO)
                    if (((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).NombreSistema != null)
                        return ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).NombreSistema;

                return s;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Métodos
        public void CargarProcesos() { }
        public void MenuPredeterminado() { }
        public void MostrarMensaje(string mensaje,ETipoMensajeIU tipo,string menssage = null) { }
        #region SC0008
        public void InicializarConfiguracionPrueba() { }
        #endregion SC0008
        #endregion
    }
}