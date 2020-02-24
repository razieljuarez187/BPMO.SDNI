//Satisface el caso de uso CU009 – Configuración Notificación de facturación

using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.MonitoreoPagos.PRE;
using BPMO.SDNI.Facturacion.MonitoreoPagos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.UI
{
    /// <summary>
    /// Forma que realiza el proceso de registra un nuevo registro
    /// </summary>
    public partial class RegistrarConfigurarAlertaUI : System.Web.UI.Page, IRegistrarConfigurarAlertaVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de para registrar registro
        /// </summary>
        private RegistrarConfigurarAlertaPRE presentador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "RegistrarConfigurarAlertaUI";

        #endregion

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
        /// Obtiene o establece un valor que representa el nombre corto de 
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ClaveSucursal
        {
            get
            {
                return this.ucConfigurarAlerta.ClaveSucursal;
            }
            set
            {
                this.ucConfigurarAlerta.ClaveSucursal = value;
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
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>
        public void PrepararNuevo()
        {
            this.ucConfigurarAlerta.PrepararNuevo();
        }

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        public void EstablecerPaqueteNavegacion(string clave, object valor)
        {
            if (string.IsNullOrEmpty(clave) || string.IsNullOrWhiteSpace(clave))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (clave == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            this.Session[clave] = valor;
        }

        /// <summary>
        /// Obtiene el valor de un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <returns>Valor de tipo objet dentro del paquete de navegación</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return this.Session[key];
        }

        /// <summary>
        /// Elimina un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (this.Session[key] != null)
                this.Session.Remove(key);
        }

        /// <summary>
        /// Realiza la dirección al visor correspondiente para realiza una nueva consulta de registros
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Facturacion.MonitoreoPagos.UI/ConsultarConfigurarAlertaUI.aspx"));
        }

        /// <summary>
        /// Despliega el Detalle del registro Seleccionado
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Facturacion.MonitoreoPagos.UI/DetalleConfigurarAlertaUI.aspx"));
        }

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {            
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
                this.presentador = new RegistrarConfigurarAlertaPRE(this, this.ucConfigurarAlerta);

                if (!this.IsPostBack)
                    this.presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se solicita el guardado de los datos capturados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Registrar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al registrar un registro", ETipoMensajeIU.ERROR, nombreClase + ".btnRegistrar_Click:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se solicita la cancelación del nuevo registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}