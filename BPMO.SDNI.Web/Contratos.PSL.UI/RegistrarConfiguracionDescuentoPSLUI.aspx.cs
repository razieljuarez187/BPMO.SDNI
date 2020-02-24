using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class RegistrarConfiguracionDescuentoPSLUI : System.Web.UI.Page, IRegistrarConfiguracionDescuentoPSLVIS {

        #region Atributos
        private string nombreClase = "RegistrarConfiguracionDescuentoPSLUI";

        private RegistrarConfiguracionDescuentoPSLPRE presentador;

        public enum ECatalogoBuscador {
            Modelo,
            Sucursal,
            SucursalNoAplica
        }
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new RegistrarConfiguracionDescuentoPSLPRE(this, ucConfiguracionDescuentoPSLUI);
                if (!Page.IsPostBack) {
                    presentador.PrepararNuevo();
                    this.LimpiarSesion();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Retorna la fecha de creación del descuento
        /// </summary>
        public DateTime? FC {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// Retorna la fecha de la ultima modificación del descuento
        /// </summary>
        public DateTime? FUA {
            get { return FC; }
        }
        /// <summary>
        /// Retorna a el usuario que crea el descuento
        /// </summary>
        public int? UC {
            get {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// Retorna el usuario que actualiza por ultima vez el descuento
        /// </summary>
        public int? UUA {
            get { return UC; }
        }

        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID {
            get {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null) {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Prepara la interfaz para el registro de un nuevo descuento
        /// </summary>
        public void PrepararNuevo() {
            this.ucConfiguracionDescuentoPSLUI.PrepararNuevo();
        }

        /// <summary>
        /// Habilita o deshabilita el botón de guardar descuento terminado
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirGuardarTerminado(bool permitir) {
            this.btnGuardar.Enabled = permitir;
        }

        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Envía al usuario a la página de consulta de descuentos
        /// </summary>
        public void RedirigirAConsulta() {
            Response.Redirect("ConsultarConfiguracionDescuentoPSLUI.aspx", false);
        }

        // <summary>
        /// Envía al usuario a la página de detalle después del registro del Descuento
        /// </summary>
        public void RedirigirADetalles() {
            Response.Redirect("DetalleConfiguracionDescuentoPSLUI.aspx", false);
        }

        public void LimpiarSesion() {
            Session.Remove("ListaDescuentos");

            Session.Remove("Descuentos");

            Session.Remove("ListaSucursales");
        }

        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e) {
            this.presentador.GuardarRegistros();
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            LimpiarSesion();
            RedirigirAConsulta();
        }
    }
}