//Satisface al caso de uso CU098 - Configurar Autorizadores para los Contratos
using System;
using System.Web.UI;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI
{
    public partial class RegistrarAutorizadorUI : System.Web.UI.Page, IRegistrarAutorizadorVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de para registrar autorizador
        /// </summary>
        private RegistrarAutorizadorPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "RegistrarAutorizadorUI";
       
        #endregion

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

        public int? AutorizadorID
        {
            get { return this.ucAutorizador.AutorizadorID; }
            set { this.ucAutorizador.AutorizadorID = value; }
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
        public bool? Estatus
        {
            get { return this.ucAutorizador.Estatus; }
            set { this.ucAutorizador.Estatus = value; }
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
            get { return DateTime.Now; }
        }
        public int? UC
        {
            get { return this.UsuarioID; }
        }
        public DateTime? FUA
        {
            get { return this.FC; }
        }

        public int? UUA
        {
            get { return this.UC; }
        }
        #endregion 

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarAutorizadorPRE(this, this.ucAutorizador);

                if (!this.IsPostBack)
                    this.presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
        }

        /// <summary>
        /// Establece el Paquete de Navegacion para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="value">Valor del paquete</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }


        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/ConsultarAutorizadorUI.aspx"));
        }
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleAutorizadorUI.aspx"));
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void LimpiarSesion()
        {
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
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Registrar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al registrar un autorizador", ETipoMensajeIU.ERROR, nombreClase + ".btnRegistrar_Click:" + ex.Message);
            }
        }

        #endregion 
    }
}