//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI
{
    public partial class EditarAutorizadorUI : System.Web.UI.Page, IEditarAutorizadorVIS
    {
        #region Atributos

        private string nombreClase = "EditarAutorizadorUI";
        private EditarAutorizadorPRE presentador;

        #endregion Atributos

        #region Propiedades
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoAutorizador"] != null)
                    return Session["UltimoObjetoAutorizador"];

                return null;
            }
            set
            {
                Session["UltimoObjetoAutorizador"] = value;
            }
        }

        public int? AutorizadorID
        {
            get { return this.ucAutorizador.AutorizadorID; }
            set
            {
                this.ucAutorizador.AutorizadorID = value;
                TextBox txtAutorizadorID = this.mEditarAutorizador.Controls[0].FindControl("txtValue") as TextBox;
                txtAutorizadorID.Text = value.ToString();
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el registro del autorizador
        /// </summary>
        public string SucursalNombre
        {
            get { return this.ucAutorizador.SucursalNombre; }
            set { this.ucAutorizador.SucursalNombre = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean registrar el autorizador
        /// </summary>
        public int? SucursalID
        {
            get { return this.ucAutorizador.SucursalID; }
            set { this.ucAutorizador.SucursalID = value; }
        }
        /// <summary>
        /// Tipo de Autorización 
        /// </summary>
        public Enum TipoAutorizacion
        {
            get { return this.ucAutorizador.TipoAutorizacion; }
            set { this.ucAutorizador.TipoAutorizacion = value; }
        }
        /// <summary>
        /// Nombre del Empleado
        /// </summary>
        public string EmpleadoNombre
        {
            get { return this.ucAutorizador.EmpleadoNombre; }
            set { this.ucAutorizador.EmpleadoNombre = value; }
        }
        /// <summary>
        /// Identificador del Empleado
        /// </summary>
        public int? EmpleadoID
        {
            get { return this.ucAutorizador.EmpleadoID; }
            set { this.ucAutorizador.EmpleadoID = value; }
        }
        public string Email
        {
            get { return this.ucAutorizador.Email; }
            set { this.ucAutorizador.Email = value; }
        }
        public string Telefono
        {
            get { return this.ucAutorizador.Telefono; }
            set { this.ucAutorizador.Telefono = value; }
        }
        public bool? Estatus
        {
            get { return this.ucAutorizador.Estatus; }
            set
            {
                this.ucAutorizador.Estatus = value;
                TextBox txtEstatus = this.mEditarAutorizador.Controls[2].FindControl("txtValue") as TextBox;
                if (value != null && value == true)
                    txtEstatus.Text = "ACTIVO";
                else
                    txtEstatus.Text = "INACTIVO";
            }
        }
        /// <summary>
        /// Obtiene  o establece el Tipo de Notificación del autorizador
        /// </summary>
        public bool? SoloNotificacion
        {
            get
            {
                return this.ucAutorizador.SoloNotificacion;
            }
            set
            {
                this.ucAutorizador.SoloNotificacion = value;
            }
        }

        public DateTime? FC
        {
            get { return this.ucAutorizador.FC; }
            set { this.ucAutorizador.FC = value; }
        }
        public DateTime? FUA
        {
            get { return this.ucAutorizador.FUA; }
            set { this.ucAutorizador.FUA = value; }
        }
        public int? UC
        {
            get { return this.ucAutorizador.UC; }
            set { this.ucAutorizador.UC = value; }
        }
        public int? UUA
        {
            get { return this.ucAutorizador.UUA; }
            set { this.ucAutorizador.UUA = value; }
        }
        #endregion 

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new EditarAutorizadorPRE(this, this.ucAutorizador);

                if (!this.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + "Page_Load" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararEdicion()
        {

        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleAutorizadorUI.aspx"));
        }
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/ConsultarAutorizadorUI.aspx"));
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        public void LimpiarSesion()
        {
            if (Session["UltimoObjetoAutorizador"] != null)
                Session.Remove("UltimoObjetoAutorizador");
        }

        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
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
                this.MostrarMensaje("Inconsistencia al cancelar la edición", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Editar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al realizar la edición del autorizador", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        #endregion Eventos
    }
}