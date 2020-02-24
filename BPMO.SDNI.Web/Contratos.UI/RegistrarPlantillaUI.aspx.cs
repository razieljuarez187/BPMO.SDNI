// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PRE;
using BPMO.SDNI.Contratos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.UI
{
    /// <summary>
    /// Página para el registro de los archivos que servirán de plantilla para los contratos
    /// </summary>
    public partial class RegistrarPlantillaUI : System.Web.UI.Page, IRegistrarPlantillaVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private RegistrarPlantillaPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "RegistrarPlantillaUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario.Id : null;
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
                           ? masterMsj.Adscripcion.UnidadOperativa.Id
                           : null;
            }
        }
        /// <summary>
        /// Obtiene o establece el modulo de contratos al que pertenece el archivo
        /// </summary>
        public int? TipoPlantilla
        {
            get
            {
                if (this.ddlModuloContratos.SelectedValue.CompareTo("-1") != 0)
                {
                    int val = 0;
                    if (Int32.TryParse(this.ddlModuloContratos.SelectedValue, out val))
                        return val;

                }
                return null;
            }
            set { this.ddlModuloContratos.SelectedValue = value.HasValue ? value.ToString() : "-1"; }
        }
        /// <summary>
        /// Obtiene le nombre del archivo seleccionado
        /// </summary>
        public string NombreArchivo
        {
            get { return this.uplArchivo.HasFile ? this.uplArchivo.FileName : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el tipo de archivo que corresponde al archivo seleccionado
        /// </summary>
        public object TipoArchivo
        {
            get
            {
                return Session["TipoArchivo"] as TipoArchivoBO;
            }
            set
            {
                Session["TipoArchivo"] = value;
            }
        }
        /// <summary>
        /// Obtiene la extencion del archivo seleccionado
        /// </summary>
        public string ExtencionArchivo
        {
            get { return this.uplArchivo.HasFile ? System.IO.Path.GetExtension(this.NombreArchivo).Replace(".", "") : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece los tipos de archivos permitidos para la carga
        /// </summary>
        public object TiposArchivo
        {
            get
            {
                return Session["TiposArchivos"] as List<TipoArchivoBO>;
            }
            set
            {
                Session["TiposArchivos"] = value;
            }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarPlantillaPRE(this);
                if (!Page.IsPostBack)
                    this.presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.ReestablecerControles();
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, string.Format("{0}.Page_Load:{1}{2}.", nombreClase, Environment.NewLine, ex.Message));
            }
        }        
        #endregion

        #region Métodos
        /// <summary>
        /// Obtiene el arreglo de bytes para el archivo seleccionado
        /// </summary>
        /// <returns>Arreglo de bytes</returns>
        public byte[] ObtenerArchivosBytes()
        {
            return this.uplArchivo.HasFile ? this.uplArchivo.FileBytes : null;
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Elimina de la Session las variables usadas en el caso de uso
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("TiposArchivos");
            Session.Remove("TipoArchivo");
        }

        public void PrepararNuevo()
        {
            this.ddlModuloContratos.Enabled = true;
        }
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), false);
        }
        /// <summary>
        /// Redirige a la página de consulta de documentos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.UI/ConsultarPlantillaUI.aspx"), false);
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// >Bloquea los botones de registro en la UI
        /// </summary>
        /// <param name="status">Estaus que se aplicará a los controles</param>
        public void BloquearRegistro(bool status)
        {
            this.hlRegistroOrden.Enabled = status;
        }
        /// <summary>
        /// Bloquea los botones de consulta en la UI
        /// </summary>
        /// <param name="status">Estatus que se aplicara a los controles</param>
        public void BloquearConsulta(bool status)
        {
            this.hlConsultar.Enabled = status;
        }
        /// <summary>
        /// Establece las opciones configuradas para los modulos de contratos
        /// </summary>
        /// <param name="opciones">Lista de opciones configuradas</param>
        public void EstablecerOpcionesTipoPlantilla(Dictionary<int, string> opciones)
        {
            this.ddlModuloContratos.Items.Clear();
            this.ddlModuloContratos.DataSource = opciones;
            this.ddlModuloContratos.DataTextField = "Value";
            this.ddlModuloContratos.DataValueField = "Key";
            this.ddlModuloContratos.DataBind();
            this.ddlModuloContratos.SelectedValue = "-1";
        }
        /// <summary>
        /// Reestablece los controles en caso de alguna inconsistencia
        /// </summary>
        private void ReestablecerControles()
        {
            this.btnCancelar.Enabled = true;
            this.btnGuardar.Enabled = false;
        }
        #endregion        

        #region Eventos
        /// <summary>
        /// Cancela el registro del archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el registro del archivo", ETipoMensajeIU.ERROR, string.Format("{0}.btnCancelar_Click:{1}{2}",nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Guarda el archivo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.uplArchivo.HasFile)
                    this.presentador.RegistrarArchivo();               
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el archivo", ETipoMensajeIU.ERROR, string.Format("{0}.btnGuardar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion        
    }
}