//Satisface al caso de uso CU062 - Menú Principal
//Satisface a la Solicitud de Cambio SC0001
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;

using System.Linq;
using System.Xml.Linq;

using BPMO.Security.BO;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.MapaSitio.PRE;
using BPMO.SDNI.MapaSitio.VIS;

namespace BPMO.SDNI.MapaSitio.UI
{
    public partial class Site : System.Web.UI.MasterPage, IMasterPageVIS
    {
        #region Atributos
        MasterPagePRE presentador = null;

        string paginaInicio = "~/MapaSitio.UI/MenuPrincipalUI.aspx";
        string paginaLogin = "~/Seguridad.Acceso.UI/LoginUI.aspx";
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
                this.Session["Usuario"] = value;
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
            get { return ConfigurationManager.AppSettings["Logueo"]; } //return "~/Seguridad.Acceso.UI/LoginUI.aspx"; }//
        }

        public string URLLogoEmpresa
        {
            get
            {
                string s = this.ResolveUrl("~/Contenido/Imagenes/LogoBepensaMotriz.png");

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO)
                {
                    string config = ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).URLLogoEmpresa;
                    if (!string.IsNullOrEmpty(config) && !string.IsNullOrWhiteSpace(config))
                        return config;
                }

                return s;
            }
        }
        public string DireccionCSS
        {
            get
            {
                string s = this.ResolveUrl("~/Contenido/Estilos/");

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO)
                {
                    string config = ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).DireccionCSS;
                    if (!string.IsNullOrEmpty(config) && !string.IsNullOrWhiteSpace(config))
                        return config;
                }

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
                else
                {
                    if (this.Session["ModuloID"] != null)
                        return Convert.ToInt32(this.Session["ModuloID"]);
                    else
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                            return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);
                    }

                    return null;
                }
            }
            set
            {
                //RQM 14150, se agrega el set para que permita en la configuración del modulo asignarle valor
                if (value != null)
                {
                    id = value;
                    //Se guarda en sesión el valor del módulo id
                    this.Session["ModuloID"] = value;
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
        /// <summary>
        /// Método manejador del evento init de la página
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EvenArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //Código de prueba borrar y habilitar el de abajo al final
            if (!IsPostBack)
            {
                //this.InicializarConfiguracionPrueba();

                //Registramos el primer post
                Session["UltimoPost"] = DateTime.Now;

                this.LeerYSubirAmbiente();
            }
            this.InicializarConfiguracion();
        }
        /// <summary>
        /// Método manejador del evento load de la master principal
        /// </summary>
        /// <param name="sender">Representa el objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Representa un objeto de tipo EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.AsignarMenuSeleccionado();
                this.VerificaSession();
                this.VerificarCambioAdscripcion();
                this.LeerYSubirNumeroFilas();
                if (this.Usuario != null && this.ListadoDatosConexion != null && this.ListadoDatosConexion.Count > 0)
                {
                    this.EvitarCache();
                    this.presentador = new MasterPagePRE(this);
                    if (!IsPostBack)
                    {
                        hdnIniContFinSession.Value = (int.Parse(ConfigurationManager.AppSettings["timeoutIniContador"].ToString()) * 60).ToString();
                        this.Session.Remove("ProcesoSeleccionado");
                        if (bool.Parse(this.hdnMostrarMenu.Value))
                        {
                            if (this.ListadoProcesos == null)
                            {
                                this.presentador.ObtenerProcesos();
                            }
                            else
                            {
                                this.CargarProcesos();
                            }
                        }
                    }
                }
                else
                {
                    //Checar este código al final, sustituir la ubicación de la página de login
                    this.Response.Redirect(this.Logueo);
                }

            }
            catch (Exception)
            {
                this.MostrarMensaje("Error inesperado", ETipoMensajeIU.ERROR, "Surgió un error inesperado, por favor contacte a su administrador de sistemas.");
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Asigna el valor del menú seleccionado 
        /// </summary>
        private void AsignarMenuSeleccionado()
        {
            try
            {
                if (this.Request.QueryString["MenuSeleccionado"] != null)
                {
                    this.hdnMenuSeleccionado.Value = this.Request.QueryString["MenuSeleccionado"];
                    this.Session["MenuSeleccionado"] = this.hdnMenuSeleccionado.Value;
                    var posicion = this.Request.Url.ToString().IndexOf("MenuSeleccionado");
                    if (posicion != -1)
                    {
                        this.Response.Redirect(this.Request.Url.ToString().Substring(0, posicion - 1));
                    }
                }
                else
                {
                    this.hdnMenuSeleccionado.Value = this.Session["MenuSeleccionado"] != null ? this.Session["MenuSeleccionado"].ToString() : string.Empty;
                }
            }
            catch (Exception)
            {

                this.MostrarMensaje("Error al asignar el menú seleccionado", ETipoMensajeIU.INFORMACION);
            }
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">1: Error, 2: Advertencia, 3: Información</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null)
        {
            string sError = string.Empty;
            if (tipoMensaje == ETipoMensajeIU.ERROR)
            {
                if (this.hdnMensaje == null)
                    sError += " , hdnDetalle";
                this.hdnDetalle.Value = msjDetalle;
            }
            if (hdnMensaje == null)
                sError += " , hdnMensaje";
            if (hdnTipoMensaje == null)
                sError += " , hdnTipoMensaje";
            if (sError.Length > 0)
                throw new Exception("No se pudo desplegar correctamente el error. No se encontró el control: " + sError.Substring(2) + " en la MasterPage.");

            this.hdnMensaje.Value = mensaje;
            this.hdnTipoMensaje.Value = ((int)tipoMensaje).ToString();
        }

        //Método de prueba borrar al final
        public void InicializarConfiguracionPrueba()
        {
            this.presentador = new MasterPagePRE(this);
            this.presentador.ObtenerDatosDeConexion();
            if (this.Ambiente == null)
            {
                this.ltEstilo.Text = "<link href='" + this.ResolveUrl("~/Contenido/Estilos/EstiloDesarrollo.css") + "' rel='Stylesheet' type='text/css'/>";
            }
            else
            {
                this.ltEstilo.Text = "<link rel='stylesheet' type='text/css' href='" + this.ResolveUrl("~/Contenido/Estilos/" + this.Ambiente + ".css") + "'/>";
            }
            if (this.Usuario != null && this.Usuario.Usuario != null)
                this.lblNombre.Text = this.Usuario.Usuario.Trim();
            else
                this.lblNombre.Text = "";
            if (this.Adscripcion != null)
            {
                if (this.Adscripcion.UnidadOperativa != null)
                    this.lblAdscripcion.Text = this.Adscripcion.UnidadOperativa.Nombre;
                if (this.Adscripcion.Sucursal != null)
                    this.lblAdscripcion.Text += " | " + this.Adscripcion.Sucursal.Nombre;
            }
        }
        /// <summary>
        /// Inicializa las variables de configuración del sistema
        /// </summary>
        private void InicializarConfiguracion()
        {
            try
            {
                //Conexiones
                if (this.ListadoDatosConexion == null)
                {
                    this.presentador = new MasterPagePRE(this);
                    this.presentador.ObtenerDatosDeConexion();
                }

                //Asignación de estilos con base en la configuración del módulo y el ambiente
                if (this.Ambiente == null)
                    this.ltEstilo.Text = "<link href='" + this.DireccionCSS + "EstiloDesarrollo.css" + "' rel='Stylesheet' type='text/css'/>";
                else
                    this.ltEstilo.Text = "<link rel='stylesheet' type='text/css' href='" + this.DireccionCSS + this.Ambiente + ".css" + "'/>";

                //Asignación del logo del sistema
                this.imgLogo.ImageUrl = this.URLLogoEmpresa;

                //Asignación del nombre del sistema
                this.ltTitle.Text = this.NombreSistema;

                //Asignación de información de logueo
                if (this.Usuario != null)
                {
                    string id = this.Usuario.Id != null ? this.Usuario.Id.ToString() : "";
                    string nombreUsuario = this.Usuario.Usuario;
                    if (string.IsNullOrEmpty(id))
                        throw new NullReferenceException("Usuario no válido");
                    else
                        this.lblNombre.Text = this.Usuario.Usuario;
                }
                else
                {
                    this.lblNombre.Text = "";
                }

                //Asignación de información de la adscripción
                if (this.Adscripcion != null)
                {
                    this.lblAdscripcion.Text = this.Adscripcion.UnidadOperativa.Nombre;
                    if (this.Adscripcion.Sucursal != null)
                        this.lblAdscripcion.Text += " | " + this.Adscripcion.Sucursal.Nombre;
                }
            }
            catch (Exception ex)
            {

                this.MostrarMensaje("No se pudo inicializar la configuración correctamente", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Carga los procesos a los que el usuario tiene acceso
        /// </summary>
        public void CargarProcesos()
        {
            try
            {
                #region SC0008
                List<string> procesosRaiz = presentador.ObtenerProcesoRaiz(this.ListadoProcesos);
                
                this.mnuPrincipal.Items.Clear();

                MenuItem Inicio = new MenuItem
                {
                    Text = "Inicio",
                    Value = "Inicio",
                    ToolTip = "Ir a la página de inicio".ToUpper(),
                    NavigateUrl = this.ResolveUrl(this.paginaInicio)
                };
                this.mnuPrincipal.Items.AddAt(0, Inicio);
                #endregion SC0008

                foreach (string proceso in procesosRaiz)
                {
                    if (!string.IsNullOrEmpty(proceso))
                    {
                        MenuItem elementoPadre = new MenuItem(proceso, proceso);
                        elementoPadre.ToolTip = "MENÚ " + proceso;
                        this.mnuPrincipal.Items.Add(elementoPadre);
                        this.AgregarSubMenusRecursivo(ref elementoPadre, this.ListadoProcesos);
                    }
                }

                MenuItem mnuOpciones = mnuPrincipal.Items.Cast<MenuItem>().FirstOrDefault(item => item.Value.ToLower() == "opciones");

                bool existeOpciones = mnuOpciones != null;

                if (!existeOpciones)
                {
                    mnuOpciones = new MenuItem
                    {
                        Text = "Opciones",
                        ToolTip = "Menú opciones".ToUpper()
                    };
                }
                if (this.Adscripcion != null)
                    {
                        var cambiarAdscripcion = new MenuItem
                        {
                            Text = "Cambiar Adscripción",
                            Value = "Adscripcion",
                            NavigateUrl = this.ResolveUrl(this.paginaCambioAdscripcion),
                            ToolTip = "Ir a la página de configuración".ToUpper()
                        };
                        mnuOpciones.ChildItems.AddAt(0,cambiarAdscripcion);
                    }
                    if (mnuOpciones.ChildItems.Count > 0 && !existeOpciones)
                    {
                        this.mnuPrincipal.Items.Add(mnuOpciones);
                    }
                
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al cargar los procesos", ETipoMensajeIU.ERROR,
                    "Error al cargar los procesos correspondientes al usuario.");
                //throw ex;
            }
        }

        /// <summary>
        /// Agrega los submenús y las elementos hijos de los menús padre
        /// </summary>
        /// <param name="mnuPadre">Elemento padre</param>
        /// <param name="listaProcesos">lista de procesos</param>
        private void AgregarSubMenusRecursivo(ref MenuItem mnuPadre, List<ProcesoBO> listaProcesos)
        {
            foreach (var proceso in listaProcesos)
            {
                if (proceso.ProcesoPadre == mnuPadre.Value && proceso.NombreCorto != mnuPadre.Value && proceso.MenuPrincipal == true)
                {
                    MenuItem elementoHijo = new MenuItem
                    {
                        Text = proceso.UI == "#" ? proceso.Titulo + "  »" : proceso.Titulo,
                        Value = proceso.UI,
                        NavigateUrl = proceso.UI != "#" ? proceso.Ruta : "",
                        ToolTip = proceso.Titulo.ToUpper(),
                    };
                    mnuPadre.ChildItems.Add(elementoHijo);
                    this.AgregarSubMenusRecursivo(ref elementoHijo, listaProcesos);
                }
            }
        }
        /// <summary>
        /// <summary>
        /// Construye un ménu con opciones predeterminadas
        /// </summary>
        public void MenuPredeterminado()
        {
            this.mnuPrincipal.Items.Clear();

            MenuItem Inicio = new MenuItem
            {
                Text = "Inicio",
                Value = "Inicio",
                ToolTip = "Ir a la página de inicio".ToUpper(),
                NavigateUrl = this.ResolveUrl(this.paginaInicio)
            };
            this.mnuPrincipal.Items.AddAt(0, Inicio);

            MenuItem mnuOpciones = new MenuItem
            {
                Text = "Opciones",
                ToolTip = "Menú opciones".ToUpper()
            };

            if (this.Adscripcion != null)
            {
                MenuItem cambiarAdscripcion = new MenuItem
                {
                    Text = "Cambiar Adscripción",
                    Value = "Adscripcion",
                    NavigateUrl = this.ResolveUrl(this.paginaCambioAdscripcion),
                    ToolTip = "Ir a la página de configuración".ToUpper()
                };
                mnuOpciones.ChildItems.Add(cambiarAdscripcion);
            }
            if (mnuOpciones.ChildItems.Count > 0)
            {
                this.mnuPrincipal.Items.Add(mnuOpciones);
            }
        }

        /// <summary>
        /// Este método permite evitar el almacenamiento en cache de la página
        /// </summary>
        private void EvitarCache()
        {
            this.Response.AddHeader("Cache-Control", "no-cache");
            this.Response.AddHeader("Pragma", "no-cache");
            this.Response.Expires = 0;
            this.Response.Cache.SetNoStore();
        }

        /// <summary>
        /// Verifica si el último Post registrado en la página ha superado el timeout de la session
        /// si así fue invoca a finalizar la sesión del usuario
        /// </summary>
        protected void VerificaSession()
        {
            if (Session["UltimoPost"] == null)
            {
                FinalizarSession();
            }
            else
            {
                DateTime ultimoPost = ((DateTime)Session["UltimoPost"]).AddMinutes(Session.Timeout);
                if (DateTime.Compare(ultimoPost, DateTime.Now) < 0)
                {
                    FinalizarSession();
                }
                else
                {
                    Session["UltimoPost"] = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// Finaliza la sesion del usuario
        /// </summary>
        private void FinalizarSession()
        {
            this.Session.RemoveAll();
            //TODO: Eudaldo > modificar código
            this.Response.Redirect(this.Logueo);
        }
        /// <summary>
        /// Verifica si la adscripción servicio ha sido cambiada
        /// </summary>
        protected void VerificarCambioAdscripcion()
        {
            hdnContadorSession.Value = (Session.Timeout * 60).ToString();
            if (string.IsNullOrEmpty(hdnSessionKey.Value))
            {
                if (Adscripcion != null)
                    hdnSessionKey.Value = Adscripcion.GetHashCode().ToString();
            }
            else
            {
                if (hdnSessionKey.Value != Adscripcion.GetHashCode().ToString())
                {
                    Response.Redirect(this.ResolveUrl(this.paginaInicio) + "?pkt=1", true);//Response.Redirect("~/Catalogos.UI/default.aspx?pkt=1", true);
                }
            }
        }

        /// <summary>
        /// Lee el número de filas del web.config y lo sube a session
        /// </summary>
        private void LeerYSubirNumeroFilas()
        {
            string numberRows = ConfigurationManager.AppSettings["NumberRows"];
            int rows;
            bool esNumero = int.TryParse(numberRows, out rows);
            if (!esNumero)
                rows = 10;
            this.NumeroFilas = rows;
        }

        private void LeerYSubirAmbiente()
        {
            #region Se Obtiene el XML de las Conexiones según el ambiente

            XDocument xmlDocumento = null;
            try
            {
                string rutaXMLConexiones = ConfigurationManager.AppSettings["XMLConexiones"];
                if (rutaXMLConexiones.StartsWith("~/"))
                    rutaXMLConexiones = this.Server.MapPath(rutaXMLConexiones);

                xmlDocumento = XDocument.Load(rutaXMLConexiones);
            }
            catch
            {
                throw new ArgumentNullException("No se pudo cargar el archivo de configuración (Conexiones.xml).");
            }
            string ambienteId = ConfigurationManager.AppSettings["Ambiente"].ToString();
            if (string.IsNullOrEmpty(ambienteId))
                throw new ArgumentNullException("No existe configuración para el ambiente en el web.config.");

            #endregion

            XElement Ambiente = xmlDocumento.Root.Elements("Ambiente").FirstOrDefault(a => a.Attribute("id").Value == ambienteId);
            if (Ambiente != null)
                this.Ambiente = Ambiente.Attribute("Estilo").Value;
            else
                this.Ambiente = null;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método manejador del evento click del bóton lkCerrarSesión
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EventArgs</param>
        protected void lkbCerrarSesion_Click(object sender, EventArgs e)
        {
            FinalizarSession();
        }

        protected void btnCheckListQuick_Click(object sender, EventArgs e)
        {
            switch (this.Adscripcion.UnidadOperativa.Id.Value)
            {
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Construccion:
                case (int)ETipoEmpresa.Equinova:
                    this.Response.Redirect("~/Contratos.PSL.UI/ConsultarListadoVerificacionPSLUI.aspx", true);
                    break;
                default:
                    this.Response.Redirect("~/Contratos.RD.UI/ConsultarListadoVerificacionUI.aspx", true);
                    break;
            }
        }
        #endregion
    }
}