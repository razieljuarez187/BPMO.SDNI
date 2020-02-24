//Satisface el caso de uso CU009 – Configuración Notificación de facturación
//Satisface a la solicitud de cambios SC0008

using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.MonitoreoPagos.PRE;
using BPMO.SDNI.Facturacion.MonitoreoPagos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.UI
{
    /// <summary>
    /// Forma que realiza el proceso de visualización de un registro
    /// </summary>
    public partial class DetalleConfigurarAlertaUI : System.Web.UI.Page, IDetalleConfigurarAlertaVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "DetalleConfigurarAlertaUI";

        /// <summary>
        /// Presentador para la vista que visualiza los campos de un registro
        /// </summary>
        private DetalleConfigurarAlertaPRE presentador;

        #endregion Atributos

        #region Propiedades
        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)this.Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)this.Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ConfiguracionAlertaID
        {
            get
            {
                return this.ucConfigurarAlerta.ConfiguracionAlertaID;
            }
            set
            {
                this.ucConfigurarAlerta.ConfiguracionAlertaID = value;
            }
        }

        #region SC0008
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? PerfilID
        {
            get
            {
                return this.ucConfigurarAlerta.PerfilID;
            }
            set
            {
                this.ucConfigurarAlerta.PerfilID = value;
            }
        }
        #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get
            {
                return this.ucConfigurarAlerta.SucursalID;
            }
            set
            {
                this.ucConfigurarAlerta.SucursalID = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalNombre
        {
            get
            {
                return this.ucConfigurarAlerta.SucursalNombre;
            }
            set
            {
                this.ucConfigurarAlerta.SucursalNombre = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? EmpleadoID
        {
            get
            {
                return this.ucConfigurarAlerta.EmpleadoID;
            }
            set
            {
                this.ucConfigurarAlerta.EmpleadoID = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre completo del empleado a quien se le asigna la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string EmpleadoNombre
        {
            get
            {
                return this.ucConfigurarAlerta.EmpleadoNombre;
            }
            set
            {
                this.ucConfigurarAlerta.EmpleadoNombre = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la cuenta de correo electrónico que tiene actualmente el empleado donde se enviarán las notificaciones
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string CorreoElectronico
        {
            get
            {
                return this.ucConfigurarAlerta.CorreoElectronico;
            }
            set
            {
                this.ucConfigurarAlerta.CorreoElectronico = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el número de días para recibir la notificación
        /// </summary>
        /// <value>Objeto de tipo Int16</value>
        public short? NumeroDias
        {
            get
            {
                return this.ucConfigurarAlerta.NumeroDias;
            }
            set
            {
                this.ucConfigurarAlerta.NumeroDias = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el estatus de la configuración
        /// </summary>
        /// <value>Objeto de tipo Nullable de Boolean</value>
        public bool? Estatus
        {
            get
            {
                return this.ucConfigurarAlerta.Estatus;
            }
            set
            {
                this.ucConfigurarAlerta.Estatus = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de creación del registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de DateTime</value>
        public DateTime? FC
        {
            get
            {
                return this.ucConfigurarAlerta.FC;
            }
            set
            {
                this.ucConfigurarAlerta.FC = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de última actualización del registros
        /// </summary>
        /// <value>Objeto de tipo Nullable de DateTime</value>
        public DateTime? FUA
        {
            get
            {
                return this.ucConfigurarAlerta.FUA;
            }           
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que creo el registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UC
        {
            get
            {
                return this.ucConfigurarAlerta.UC;
            }
            set
            {
                this.ucConfigurarAlerta.UC = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que actualizó por última vez el registro
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? UUA
        {
            get
            {
                return this.ucConfigurarAlerta.UUA;
            }            
        }

        /// <summary>
        /// Obtiene o estable el objeto de donde se esta obteniendo los datos a mostrar en el visor
        /// </summary>
        /// <value>Objeto de tipo Object</value>
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoConfigurarAlerta"] != null)
                    return Session["UltimoObjetoConfigurarAlerta"];

                return null;
            }
            set
            {
                Session["UltimoObjetoConfigurarAlerta"] = value;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza el proceso de inicialización del visor
        /// </summary>
        public void PrepararVisualizacion()
        {
            this.ucConfigurarAlerta.PrepararVisualizacion();
        }

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            this.Session[key] = value;
        }

        /// <summary>
        /// Obtiene el valor de un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <returns>Valor de tipo objet dentro del paquete de navegación</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return this.Session[key];
        }

        /// <summary>
        /// Elimina un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (this.Session[key] != null)
                this.Session.Remove(key);
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente para realizar la edición del registro actual
        /// </summary>
        public void RedirigirAEditar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Facturacion.MonitoreoPagos.UI/EditarConfigurarAlertaUI.aspx"), false);
        }

        /// <summary>
        /// Realiza la dirección al visor correspondiente para realiza una nueva consulta de registros
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Facturacion.MonitoreoPagos.UI/ConsultarConfigurarAlertaUI.aspx"), false);
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o denegar la acción de regresar al visor anterior
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        public void PermitirRegresar(bool permitir)
        {
            this.btnRegresar.Visible = permitir;
        }

        // <summary>
        /// Asigna el estatus correspondiente para permitir o denegar la acción invocar el visor de registra un nuevo elemento
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        /// <summary>
        /// Asigna el estatus correspondiente para permitir o denegar la acción invocar el visor de editar el registro en curso
        /// </summary>
        /// <param name="permitir">bandera que define la operación a realizar, true para permitir, false para denegar</param>
        public void PermitirEditar(bool permitir)
        {
            this.btnEditar.Enabled = permitir;            
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["UltimoObjetoConfigurarAlerta"] != null)
                Session.Remove("UltimoObjetoConfigurarAlerta");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)this.Page.Master;
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
        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new DetalleConfigurarAlertaPRE(this, this.ucConfigurarAlerta);

                if (!IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();
                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se que recibe el aviso de resultado del buscador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Regresar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnRegresar_Click:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se requiere redirigir a la forma de edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.IrAEditar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar editar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.GetBaseException().Message);
            }
        }
       
        #endregion
    }
}