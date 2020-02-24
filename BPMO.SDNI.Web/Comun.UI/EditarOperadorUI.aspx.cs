// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI
{
    public partial class EditarOperadorUI : System.Web.UI.Page, IEditarOperadorVIS
    {
        #region Atributos
        private string nombreClase = "EditarOperadorUI";
        private EditarOperadorPRE presentador;
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

        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoOperador"] != null)
                    return Session["UltimoObjetoOperador"];

                return null;
            }
            set
            {
                Session["UltimoObjetoOperador"] = value;
            }
        }

        public int? OperadorID
        {
            get { return this.ucDatosOperadorUI.OperadorID; }
            set
            {
                this.ucDatosOperadorUI.OperadorID = value;
                TextBox txtID = this.mOperador.Controls[0].FindControl("txtValue") as TextBox;
                if (value != null)
                    txtID.Text = value.ToString();
                else
                    txtID.Text = string.Empty;
            }
        }
        public int? CuentaClienteID
        {
            get { return this.ucDatosOperadorUI.CuentaClienteID; }
            set { this.ucDatosOperadorUI.CuentaClienteID = value; }
        }
        public string CuentaClienteNombre
        {
            get { return this.ucDatosOperadorUI.CuentaClienteNombre; }
            set { this.ucDatosOperadorUI.CuentaClienteNombre = value; }
        }
        public string Nombre
        {
            get { return this.ucDatosOperadorUI.Nombre; }
            set { this.ucDatosOperadorUI.Nombre = value; }
        }
        public int? AñosExperiencia
        {
            get { return this.ucDatosOperadorUI.AñosExperiencia; }
            set { this.ucDatosOperadorUI.AñosExperiencia = value; }
        }
        public DateTime? FechaNacimiento
        {
            get { return this.ucDatosOperadorUI.FechaNacimiento; }
            set { this.ucDatosOperadorUI.FechaNacimiento = value; }
        }
        public string DireccionCalle
        {
            get { return this.ucDatosOperadorUI.DireccionCalle; }
            set { this.ucDatosOperadorUI.DireccionCalle = value; }
        }
        public string DireccionCiudad
        {
            get { return this.ucDatosOperadorUI.DireccionCiudad; }
            set { this.ucDatosOperadorUI.DireccionCiudad = value; }
        }
        public string DireccionCP
        {
            get { return this.ucDatosOperadorUI.DireccionCP; }
            set { this.ucDatosOperadorUI.DireccionCP = value; }
        }
        public string DireccionEstado
        {
            get { return this.ucDatosOperadorUI.DireccionEstado; }
            set { this.ucDatosOperadorUI.DireccionEstado = value; }
        }
        public int? LicenciaTipoID
        {
            get { return this.ucDatosOperadorUI.LicenciaTipoID; }
            set { this.ucDatosOperadorUI.LicenciaTipoID = value; }
        }
        public string LicenciaNumero
        {
            get { return this.ucDatosOperadorUI.LicenciaNumero; }
            set { this.ucDatosOperadorUI.LicenciaNumero = value; }
        }
        public DateTime? LicenciaFechaExpiracion
        {
            get { return this.ucDatosOperadorUI.LicenciaFechaExpiracion; }
            set { this.ucDatosOperadorUI.LicenciaFechaExpiracion = value; }
        }
        public string LicenciaEstado
        {
            get { return this.ucDatosOperadorUI.LicenciaEstado; }
            set { this.ucDatosOperadorUI.LicenciaEstado = value; }
        }
        public bool? Estatus
        {
            get { return this.ucDatosOperadorUI.Estatus; }
            set 
            {
                this.ucDatosOperadorUI.Estatus = value;
                TextBox txtEstatus = this.mOperador.Controls[2].FindControl("txtValue") as TextBox;
                if (value != null && value == true)
                {
                    this.mOperador.Items[3].Text = "Desactivar";
                    txtEstatus.Text = "ACTIVO";
                }
                else
                {
                    this.mOperador.Items[3].Text = "Reactivar";
                    txtEstatus.Text = "INACTIVO";
                }
            }
        }

        public int? UC
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }
        public int? UUA
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUUA.Value))
                    id = int.Parse(this.hdnUUA.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUUA.Value = value.ToString();
                else
                    this.hdnUUA.Value = string.Empty;
            }
        }
        public DateTime? FC
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFC.Value))
                    temp = DateTime.Parse(this.hdnFC.Value.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFC.Value = value.Value.ToString();
                else
                    this.hdnFC.Value = string.Empty;
            }
        }
        public DateTime? FUA
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFUA.Value))
                    temp = DateTime.Parse(this.hdnFUA.Value.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFUA.Value = value.Value.ToString();
                else
                    this.hdnFUA.Value = string.Empty;
            }
        }

        public DateTime? FechaDesactivacion
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFechaDesactivacion.Value))
                    temp = DateTime.Parse(this.hdnFechaDesactivacion.Value.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFechaDesactivacion.Value = value.Value.ToString();
                else
                    this.hdnFechaDesactivacion.Value = string.Empty;
            }
        }
        public int? UsuarioDesactivacionID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioDesactivacionID.Value))
                    id = int.Parse(this.hdnUsuarioDesactivacionID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioDesactivacionID.Value = value.ToString();
                else
                    this.hdnUsuarioDesactivacionID.Value = string.Empty;
            }
        }
        public string MotivoDesactivacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.hdnMotivoDesactivacion.Value)) ? null : this.hdnMotivoDesactivacion.Value.Trim().ToUpper();
            }
            set
            {
                this.hdnMotivoDesactivacion.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        #endregion 

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new EditarOperadorPRE(this, this.ucDatosOperadorUI);

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
        #endregion Constructores

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
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleOperadorUI.aspx"));
        }
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/ConsultarOperadorUI.aspx"));
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
            if (Session["UltimoObjetoOperador"] != null)
                Session.Remove("UltimoObjetoOperador");
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
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Editar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al realizar la edición del operador", ETipoMensajeIU.ERROR, this.nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
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
        #endregion

    }
}