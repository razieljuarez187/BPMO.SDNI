//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
//Esta clase satisface los requerimientos especificados en el caso de uso CU008 Consultar Entrega Recepción de Unidad de Renta Diaria
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.UI
{
    public partial class DetalleFlotaUI : System.Web.UI.Page, IDetalleFlotaVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador para el detalle de los elementos de flota
        /// </summary>
        private DetalleFlotaPRE presentador;
        /// <summary>
        /// Retorna el nombre de la clase
        /// </summary>
        private const string nombreClase = "DetalleFlotaUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public UsuarioBO Usuario
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario : new UsuarioBO();
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa : new UnidadOperativaBO();
            }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad desplegada
        /// </summary>
        public int? UnidadID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnUnidadID.Value, out val) ? (int?) val : null;
                }
                return null;
            }
            set { this.hdnUnidadID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del equipo de la unidad desplegada
        /// </summary>
        public int? EquipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnEquipoID.Value, out val) ? (int?) val : null;
                }
                return null;
            }
            set { this.hdnEquipoID.Value = value.HasValue ? value.Value.ToString().Trim().ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad desplegada
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                var txtNumeroEconomico = mnuDetalleFlota.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroEconomico != null && txtNumeroEconomico.Text != null && !string.IsNullOrEmpty(txtNumeroEconomico.Text.Trim()))
                    return txtNumeroEconomico.Text.Trim();
                return null;
            }
            set 
            {
                var txtNumeroEconomico = mnuDetalleFlota.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumeroEconomico != null)
                {
                    txtNumeroEconomico.Text = value ?? string.Empty;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece el número de serie de la unidad desplegada
        /// </summary>
        public string NumeroSerie
        {
            get
            {
                var txtNumeroSerie = mnuDetalleFlota.Controls[1].FindControl("txtValue") as TextBox;
                if (txtNumeroSerie != null && txtNumeroSerie.Text != null && !string.IsNullOrEmpty(txtNumeroSerie.Text.Trim()))
                    return txtNumeroSerie.Text.Trim();
                return null;
            }
            set
            {
                var txtNumeroSerie = mnuDetalleFlota.Controls[1].FindControl("txtValue") as TextBox;
                if (txtNumeroSerie != null)
                {
                    txtNumeroSerie.Text = value ?? string.Empty;
                }
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new DetalleFlotaPRE(this, this.ucucDatosGeneralesElementoUI, this.ucucEquiposAliadosUnidadUI);
                if (!Page.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.presentador.Inicializar();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Habilita lso ocntroles en modo lectura
        /// </summary>
        public void InicializarControles()
        {
            this.ucucDatosGeneralesElementoUI.EstablecerModoLectura();
        }
        /// <summary>
        /// Retorna a la página de consulta
        /// </summary>
        public void RegresarAConsultar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/ConsultarFlotaUI.aspx"));
        }
        /// <summary>
        /// Redirige a la pantalla de página sin acceso
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)            
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);            
            else            
                if (master != null) master.MostrarMensaje(mensaje, tipo);            
        }
        /// <summary>
        /// Obtiene el objeto que se desea presentar en la interfaz de usuario
        /// </summary>
        /// <returns>Objeto guardado en sesion para su visualización a detalle</returns>
        public object ObtenerDatosNavegacion()
        {
            return Session["DetalleFlotaUI"];
        }
        /// <summary>
        /// Establece el objeto que se desea presentar en la interfaz de usaurio
        /// </summary>
        /// <param name="nombre">Clave con la cual se asigan el paquete en session</param>
        /// <param name="valor">Objeto que es guardado en session</param>
        public void EstablecerDatosNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }
        /// <summary>
        /// Limpia las variables de session usadas para la carga de la información
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("FlotaEncontrada");
            Session.Remove("DetalleFlotaUI");
            Session.Remove("EquiposAliadosUnidad");
        }
        /// <summary>
        /// CU008
        /// Deshabilita el botón de regreso a consulta con filtro en caso de que el detalle sea llamado desde el registrar
        /// </summary>
        public void PermitirRegresar(bool permiso)
        {
            if (permiso == true)
                this.btnRegresar.Enabled = true;
            else
                this.btnRegresar.Enabled = false;

        }
        /// <summary>
        /// CU008
        /// Obtiene los filtros iniciales de consulta en caso de que la llamada sea realizada desde la consulta
        /// </summary>
        /// <returns>Objeto con los filtros iniciales de consulta</returns>
        public object ObtenerFiltrosConsulta()
        {
            if (Session["FiltrosFlota"] != null)
                return Session["FiltrosFlota"] as object;
            return null;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// SC0019
        /// Redirige a la página de consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.RetrocederPagina();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnAnterior_Click:" + ex.Message);
            }
        }
        #endregion
    }
}