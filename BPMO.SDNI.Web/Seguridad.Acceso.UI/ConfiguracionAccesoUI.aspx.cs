//Satisface al CU061 - Acceso al Sistema
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Seguridad.Acceso.PRE;
using BPMO.SDNI.Seguridad.Acceso.VIS;
using BPMO.Security.BO;

namespace BPMO.SDNI.Seguridad.Acceso.UI
{
    public partial class ConfiguracionAccesoUI : System.Web.UI.Page, IConfiguracionAccesoVIS
    {
        ConfiguracionAccesoPRE presentador=null;

        #region Propiedades
        public int? UnidadOperativa{
            get {
                int unidad;
                if (!int.TryParse(this.ddlUnidadOperativa.SelectedValue, out unidad)) {
                    return null;
                }
                return unidad;
            }
        }

        public UsuarioBO Usuario {
            get { return (UsuarioBO)Session["Usuario"]; }
        }
        public AdscripcionBO Adscripcion {
            get {
                return ((Site)this.Master).Adscripcion;
            }
            set {
                ((Site)this.Master).Adscripcion = value;
            }
        }

        public List<AdscripcionBO> Adscripciones {
            get {
                return this.Session["lstAdscripciones"] != null ? (List<AdscripcionBO>)this.Session["lstAdscripciones"] : null;
            }
            set {
                this.Session["lstAdscripciones"] = value;
            }
        }

        public List<UnidadOperativaBO> ListaUnidadesOperativas
        {
            get
            {
                return this.Session["lstUnidadesOperativas"] != null ? (List<UnidadOperativaBO>)this.Session["lstUnidadesOperativas"] : null;
            }
            set
            {
                this.Session["lstUnidadesOperativas"] = value;
            }
        }

        public List<ProcesoBO> ListadoProcesos {
            get {
                return ((Site)this.Master).ListadoProcesos;
            }
            set {
                ((Site)this.Master).ListadoProcesos = value;
            }
        }

        //RQM 14078, se agrega el valor intermedio para poder asignarle un valor fuera del valor del webConfig
          int? id = null;
           
        /// <summary>
        /// Identificador del Módulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (id != null)
                {
                    return id;
                }
                else{
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                        return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                    return null;
                }
            }
            set
            {
                //RQM 14150, se agrega el set para que permita en la configuración del modulo asignarle valor
                if (value != null)
                {
                    id = value;
                    ((Site) this.Master).ModuloID = value;
                }
                else
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                        id = int.Parse(ConfigurationManager.AppSettings["ModuloID"]);
                   
                }


            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new ConfiguracionAccesoPRE(this);
                if (!this.IsPostBack)
                {
                    this.presentador.ObtenerDatosAdscripcion();
                }
            }
            catch (Exception)
            {
                this.MostrarMensaje("Error inesperado", ETipoMensajeIU.ERROR, "Surgió un error inesperado, por favor pongase en contacto con el administrador del sistema");
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            Site master = (Site)this.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                master.MostrarMensaje(mensaje, tipo);
            }
        }
        public void EnviarAInicio() {
            this.Response.Redirect("~/MapaSitio.UI/MenuPrincipalUI.aspx?moduloid=" + this.UnidadOperativa.ToString());
        }        
        public void CargarDatosAdscripcion() {
            try
            {
                this.ddlUnidadOperativa.Items.Clear();
                var grupoUnidadOperativa = this.Adscripciones.GroupBy(ad => ad.UnidadOperativa.Id);
                foreach (var unidadID in grupoUnidadOperativa)
                {
                    if (unidadID.Key == null)
                        continue;

                    UnidadOperativaBO unidadBO = this.Adscripciones.FirstOrDefault(ad => ad.UnidadOperativa.Id == unidadID.Key.Value).UnidadOperativa;
                    if (unidadBO.Id != null && !string.IsNullOrWhiteSpace(unidadBO.Nombre))
                    {
                        ListItem unidadItem = new ListItem(unidadBO.Nombre, unidadBO.Id.ToString());
                        this.ddlUnidadOperativa.Items.Add(unidadItem);
                    }
                }

            }
            catch (Exception) {
                this.MostrarMensaje("Error al cargar listas de adscripciones",ETipoMensajeIU.ERROR,"Es posible que algunas unidades operativas no sean validas," +
                    "por favor contacte al administrador del sistema");
            }
        }

        public void EstablecerConfiguracionModulo(object configuracion)
        {
            Session["ConfiguracionModuloSDNI"] = configuracion;
        }
        /// <summary>
        /// Elimina permisos o autorizaciones de sesiones pasadas (Cambio adscripción)
        /// </summary>
        private void LimpiarSesionPrevia() {
            Session.Remove("SucursalesAutorizadas");
            Session.Remove("DirEmpresaRO");
        }
        #endregion

        #region Eventos
        protected void btnAceptar_Click(object sender, EventArgs e) {
            this.LimpiarSesionPrevia();
            presentador.SeleccionarAdscripcion();
        }
        #endregion
    }
}