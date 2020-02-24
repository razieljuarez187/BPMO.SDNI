// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PRE;
using BPMO.SDNI.Contratos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.UI
{
    /// <summary>
    /// Página para la cosnulta de los archivos que servirán de plantilla para los contratos
    /// </summary>
    public partial class ConsultarPlantillaUI : System.Web.UI.Page, IConsultarPlantillaVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private ConsultarPlantillaPRE presentador = null;
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "ConsultarPlantillaUI";
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
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado en el sistema
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
        /// Obtiene o establece el nombre del archivo
        /// </summary>
        public string Nombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtNombreDocumento.Text) && !string.IsNullOrWhiteSpace(this.txtNombreDocumento.Text)
                           ? this.txtNombreDocumento.Text
                           : string.Empty;
            }
            set
            {
                this.txtNombreDocumento.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                            ? value.Trim()
                                            : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece los 
        /// </summary>
        public bool? Estatus
        {
            get
            {
                if (this.ddlEstatusDocumento.SelectedValue.CompareTo("-1") != 0)
                {
                    bool val;
                    return Boolean.TryParse(this.ddlEstatusDocumento.SelectedValue, out val) ? (bool?)val : null;
                }
                return null;
            }
            set
            {
                this.ddlEstatusDocumento.SelectedValue = value.HasValue
                                                   ? value.ToString()
                                                   : "-1";
            }
        }

        public int IndicePaginaResultado
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Asignamos el identifocador para el User Control de archivos
                this.ucucListadoPlantillasUI.Identificador = "ucucListadoPlantillasUI";
                this.ucucListadoPlantillasUI.EliminarArchivo = this.btnEliminar_Click;
                this.presentador = new ConsultarPlantillaPRE(this, this.ucucListadoPlantillasUI);
                if (!IsPostBack)
                {
                    this.ucucListadoPlantillasUI.Identificador = "ucucListadoPlantillasUI";
                    this.presentador.PrepararBusqueda();
                }                
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, string.Format("{0}.Page_Load:{1}{2}.", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion

        #region Métodos
        ///<summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Limpia de Session los valores a usar en la página
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove(this.ucucListadoPlantillasUI.ToString());
        }
        /// <summary>
        /// Limpia el grid con los resultados encontrados
        /// </summary>
        public void LimpiarDocumentosEncontrados()
        {
            this.LimpiarSesion();

            this.presentador.CargarElementosEncontrados((new List<PlantillaBO>()).ConvertAll(x => (object)x));
        }

        public void ActualizarResultado()
        {
            throw new NotImplementedException();
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
        /// <summary>
        /// Redirige a la página de registro de documentos
        /// </summary>
        public void RedirigirARegistro()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.UI/RegistrarPlantillaUI.aspx"), false);
        }
        /// <summary>
        /// Habilita o deshabilita el control para registrar
        /// </summary>
        /// <param name="status"></param>
        public void PermitirRegistrar(bool status)
        {
            this.hlRegistroOrden.Enabled = status;
        }
        /// <summary>
        /// Habilita o deshabilita los controles para eliminar
        /// </summary>
        /// <param name="status"></param>
        public void PermitirEliminar(bool status)
        {
            this.ucucListadoPlantillasUI.PermitirEliminar = false;
            this.ucucListadoPlantillasUI.PrepararVista(false);
        }
        #endregion
        
        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Consultar();
            }catch(Exception ex)
            {
                this.MostrarMensaje("Inconsictencia al intentar consultar los documentos", ETipoMensajeIU.ERROR, string.Format("{0}.btnBuscar_Click{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.EliminarArchivo();
            }catch(Exception ex)
            {
                this.MostrarMensaje("Inconsictencia al intentar eliminar el documento", ETipoMensajeIU.ERROR, string.Format("{0}.btnEliminar_Click{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        #endregion
    }
}