// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class EditarReservacionRDUI : System.Web.UI.Page, IEditarReservacionRDVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "EditarReservacionRDUI";
        /// <summary>
        /// presentador del UC de información general del contrato de renta diaria
        /// </summary>
        private EditarReservacionRDPRE presentador;
        #endregion

        #region Propiedades
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoReservacion"] != null)
                    return Session["UltimoObjetoReservacion"];

                return null;
            }
            set
            {
                Session["UltimoObjetoReservacion"] = value;
            }
        }

        public int? UsuarioID
        {
            get
            {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        public List<int?> SucursalesSeguridad
        {
            get
            {
                return this.ucReservacion.SucursalesSeguridad;
            }
            set
            {
                this.ucReservacion.SucursalesSeguridad = value;
            }
        }

        public int? ReservacionID
        {
            get
            {
                return this.ucReservacion.ReservacionID;
            }
            set
            {
                this.ucReservacion.ReservacionID = value;
            }
        }
        public DateTime? FC
        {
            get
            {
                return this.ucReservacion.FC;
            }
            set
            {
                this.ucReservacion.FC = value;
            }
        }
        public int? UC
        {
            get
            {
                return this.ucReservacion.UC;
            }
            set
            {
                this.ucReservacion.UC = value;
            }
        }
        public DateTime? FUA
        {
            get
            {
                return this.ucReservacion.FUA;
            }
            set
            {
                this.ucReservacion.FUA = value;
            }
        }
        public int? UUA
        {
            get
            {
                return this.ucReservacion.UUA;
            }
            set
            {
                this.ucReservacion.UUA = value;
            }
        }
        public bool? Activo
        {
            get
            {
                return this.ucReservacion.Activo;
            }
            set
            {
                this.ucReservacion.Activo = value;
            }
        }
        public int? TipoID
        {
            get
            {
                return this.ucReservacion.TipoID;
            }
            set
            {
                this.ucReservacion.TipoID = value;
            }
        }

        public string Numero
        {
            get
            {
                return this.ucReservacion.Numero;
            }
            set
            {
                this.ucReservacion.Numero = value;
                TextBox txtNumero = this.mReservacion.Controls[0].FindControl("txtValue") as TextBox;
                if (txtNumero != null)
                    txtNumero.Text = value != null ? value : string.Empty;
            }
        }
        public int? CuentaClienteID
        {
            get
            {
                return this.ucReservacion.CuentaClienteID;
            }
            set
            {
                this.ucReservacion.CuentaClienteID = value;
            }
        }
        public string CuentaClienteNombre
        {
            get
            {
                return this.ucReservacion.CuentaClienteNombre;
            }
            set
            {
                this.ucReservacion.CuentaClienteNombre = value;
            }
        }
        public int? ModeloID
        {
            get
            {
                return this.ucReservacion.ModeloID;
            }
            set
            {
                this.ucReservacion.ModeloID = value;
            }
        }
        public string ModeloNombre
        {
            get
            {
                return this.ucReservacion.ModeloNombre;
            }
            set
            {
                this.ucReservacion.ModeloNombre = value;
            }
        }

        public DateTime? FechaReservacionInicial
        {
            get
            {
                DateTime? fecha = this.ucReservacion.FechaReservacionInicial;
                TimeSpan? hora = this.ucReservacion.HoraReservacionInicial;
                DateTime? fechaFinal = null;

                if (fecha != null)
                {
                    if (hora != null)
                        fechaFinal = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora.Value.Hours, hora.Value.Minutes, hora.Value.Seconds);
                    else
                        fechaFinal = fecha;
                }

                return fechaFinal;
            }
            set
            {
                if (value != null)
                {
                    this.ucReservacion.FechaReservacionInicial = value.Value.Date;
                    this.ucReservacion.HoraReservacionInicial = value.Value.TimeOfDay;
                }
                else
                {
                    this.ucReservacion.FechaReservacionInicial = null;
                    this.ucReservacion.HoraReservacionInicial = null;
                }
            }
        }
        public DateTime? FechaReservacionFinal
        {
            get
            {
                DateTime? fecha = this.ucReservacion.FechaReservacionFinal;
                TimeSpan? hora = this.ucReservacion.HoraReservacionFinal;
                DateTime? fechaFinal = null;

                if (fecha != null)
                {
                    if (hora != null)
                        fechaFinal = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora.Value.Hours, hora.Value.Minutes, hora.Value.Seconds);
                    else
                        fechaFinal = fecha;
                }

                return fechaFinal;
            }
            set
            {
                if (value != null)
                {
                    this.ucReservacion.FechaReservacionFinal = value.Value.Date;
                    this.ucReservacion.HoraReservacionFinal = value.Value.TimeOfDay;
                }
                else
                {
                    this.ucReservacion.FechaReservacionFinal = null;
                    this.ucReservacion.HoraReservacionFinal = null;
                }
            }
        }

        public int? UnidadID
        {
            get
            {
                return this.ucReservacion.UnidadID;
            }
            set
            {
                this.ucReservacion.UnidadID = value;
            }
        }
        public string NumeroEconomico
        {
            get
            {
                return this.ucReservacion.NumeroEconomico;
            }
            set
            {
                this.ucReservacion.NumeroEconomico = value;
            }
        }

        public int? UsuarioReservoID
        {
            get
            {
                return this.ucReservacion.UsuarioReservoID;
            }
            set
            {
                this.ucReservacion.UsuarioReservoID = value;
            }
        }
        public string UsuarioReservoNombre
        {
            get
            {
                return this.ucReservacion.UsuarioReservoNombre;
            }
            set
            {
                this.ucReservacion.UsuarioReservoNombre = value;
            }
        }

        public string Observaciones
        {
            get
            {
                return this.ucReservacion.Observaciones;
            }
            set
            {
                this.ucReservacion.Observaciones = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el año de la unidad
        /// </summary>
        public int? AnioUnidad
        {
            get { return this.ucReservacion.UnidadAnio; }
            set { this.ucReservacion.UnidadAnio = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal
        /// </summary>
        public int? SucursalID //SC051
        {
            get { return this.ucReservacion.SucursalID; }
            set { this.ucReservacion.SucursalID = value; }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        public string SucursalNombre
        {
            get { return this.ucReservacion.SucursalNombre; }
            set { this.ucReservacion.SucursalNombre = value; }
        }//SC051
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new EditarReservacionRDPRE(this, this.ucReservacion);

                if (!this.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
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
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleReservacionRDUI.aspx"));
        }
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarReservacionRDUI.aspx"));
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
            if (Session["UltimoObjetoReservacion"] != null)
                Session.Remove("UltimoObjetoReservacion");
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
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Editar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar la reservación", ETipoMensajeIU.ERROR, this.nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}