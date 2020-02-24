//Satisface al CU016 - Realizar Auditoria.
//Satisface al CU020 - Imprimir Auditoria Realizada

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Servicio.Procesos.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class RealizarAuditoriaMantenimientoUI : System.Web.UI.Page, IRealizarAuditoriaMantenimientoVIS
    {
        #region Atributos
        private RealizarAuditoriaMantenimientoPRE presentador = null;
        private string nombreClase;
        #endregion

        #region Propiedades

        #region propiedades idealise
        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// identificador de la unidad operativa desde el cual se esta accesando
        /// </summary>
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }
        
        #endregion

        #region Propiedades Interfaz

        /// <summary>
        /// Identificador de la Orden de servicio
        /// </summary>
        public int? OrdenServicioID
        {
            get
            {
                int? ordenID = null;
                if (this.txtFolioOS.Text.Trim().Length > 0)
                    ordenID = int.Parse(this.txtFolioOS.Text.Trim().ToUpper());
                return ordenID;
            }
            set
            {
                if (value != null)
                    this.txtFolioOS.Text = value.ToString();
                else
                    this.txtFolioOS.Text = String.Empty;
            }
        }
        /// <summary>
        /// Tipo de Mantenimiento de la Orden de servicio (A,B,C)
        /// </summary>
        public string TipoMantenimiento
        {
            get
            {
                string tipoMantenimiento = string.Empty;
                if (this.txtTipoMantenimiento.Text != null)
                    tipoMantenimiento = this.txtTipoMantenimiento.Text;
                return tipoMantenimiento;
            }
            set
            {
                if (value != string.Empty)
                    this.txtTipoMantenimiento.Text = value;
                else
                    this.txtTipoMantenimiento.Text = string.Empty;
            }
        }
        /// <summary>
        /// Tecnicos asignados a la Orden de servicio en el paquete de mantenimiento
        /// </summary>
        public List<TecnicoBO> Tecnicos
        {
            get
            {
                List<TecnicoBO> tecnicos = new List<TecnicoBO>();
                if (this.ddlTecnicos.Items.Count > 0)
                {
                    for (int i = 0; i < ddlTecnicos.Items.Count; i++)
                    {
                        var item = new TecnicoBO
                        {
                            Id = int.Parse(ddlTecnicos.Items[i].Value),
                            Empleado = new Basicos.BO.EmpleadoBO
                            {
                                NombreCompleto = ddlTecnicos.Items[i].Text
                            }

                        };
                        tecnicos.Add(item);
                    }
                }
                return tecnicos;
            }
            set
            {
                if (value != null)
                    foreach (TecnicoBO item in value)
                    {
                        ListItem tecnico = new ListItem();
                        tecnico.Text = item.Empleado.NombreCompleto;
                        tecnico.Value = item.Id.ToString();
                        this.ddlTecnicos.Items.Add(tecnico);
                    }
                else
                    this.ddlTecnicos.Items.Clear();
            }
        }

        private GridView actividadesAuditoria;
        /// <summary>
        /// Grid con las actividades de paquete de mantenimiento
        /// </summary>
        public GridView ActividadesAuditoria
        {
            get
            {
                actividadesAuditoria = grvActividadesAuditoria;
                return actividadesAuditoria;
            }
            set
            {
                actividadesAuditoria = value;

            }
        }

        private FileUpload evidencia;
        /// <summary>
        /// Archivo de evidencia de la auditoria
        /// </summary>
        public FileUpload Evidencia
        {
            get
            {
                evidencia = this.fuplEvidenciaAuditoria;
                return evidencia;
            }
            set
            {
                evidencia = value;
            }
        }

        private string observaciones;
        /// <summary>
        /// Observaciones de la auditoria
        /// </summary>
        public string Observaciones
        {
            get
            {
                string observacion = string.Empty;
                if (txbObservaciones.Text != null)
                    observacion = txbObservaciones.Text;
                return observacion;
            }
            set
            {
                if (value != string.Empty)
                    this.txbObservaciones.Text = value;
                else
                    this.txbObservaciones.Text = string.Empty;
            }
        }
        
        #endregion  

        #region Variables Session
        /// <summary>
        /// Variable de session recibida desde modulo de recepcion de unidades CU009
        /// </summary>
        public OrdenServicioBO MantenimientoRecibido
        {
            get { return Session["MantenimientoAuditoria"] != null ? Session["MantenimientoAuditoria"] as OrdenServicioBO : null; }
            set { Session["MantenimientoAuditoria"] = value; }
        }
        /// <summary>
        /// Variable de session de la auditoria realizada
        /// </summary>
        public AuditoriaMantenimientoBO Resultado
        {
            get { return Session["Auditoria"] != null ? Session["Auditoria"] as AuditoriaMantenimientoBO : null; }
            set { Session["Auditoria"] = value; }
        }
        /// <summary>
        /// Variable de session para mantener informacion del Grid
        /// </summary>
        public List<DetalleAuditoriaMantenimientoBO> DetalleAuditoria
        {
            get { return Session["DetalleAuditoria"] as List<DetalleAuditoriaMantenimientoBO>; }
            set { Session["DetalleAuditoria"] = value; }
        }
        /// <summary>
        /// variable de session del Archivo de evidencia 
        /// </summary>
        public FileUpload ArchivoEvidencia
        {
            get { return Session["ArchivoEvidencia"] as FileUpload; }
            set { Session["ArchivoEvidencia"] = value; }
        }

        #endregion
        #endregion

        #region Constructores
        /// <summary>
        /// Carga Inicial de la pagina 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                nombreClase = this.GetType().Name;
                presentador = new RealizarAuditoriaMantenimientoPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la pagina", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load" + ex.Message);
            }

            if (!IsPostBack)
            {
                this.presentador.ValidarAcceso();

                if (MantenimientoRecibido != null)
                {
                    try
                    {
                        presentador.Consultar();
                    }
                    catch (Exception ex)
                    {
                        this.MostrarMensaje("Error al consultar la Orden de Servicio a Auditar", ETipoMensajeIU.ERROR, this.nombreClase + ".Consultar :" + ex.GetBaseException().Message);
                    }

                }

            }

            #region PersistenciaFileUpload


            // almacena el objeto FileUpload en session.
            // "ArchivoEvidencia" es el ID de el control FileUpload
            // esta condicion ocurre cuando se carga por primara vez el archivo.
            if (ArchivoEvidencia == null && this.fuplEvidenciaAuditoria.HasFile == true)
            {
                ArchivoEvidencia = this.fuplEvidenciaAuditoria;
            }
            // esta condicion ocurrira em el siguente postback      
            else if (ArchivoEvidencia != null && this.fuplEvidenciaAuditoria.HasFile == false)
            {
                this.fuplEvidenciaAuditoria = ArchivoEvidencia;
            }
            //  when Session will have File but user want to change the file 
            //  cuando session tenga el archivo pero el usuario cambia el archivo
            // i.e. quere subier otro archivo con el mismo control
            // entonces se acutaliza la session con el nuevo archvo
            else if (this.fuplEvidenciaAuditoria.HasFile == true)
            {
                ArchivoEvidencia = this.fuplEvidenciaAuditoria;
            }

            #endregion

            #region Persistencia del Grid

            if (Session["DetalleAuditoria"] == null)
                Session["DetalleAuditoria"] = new List<DetalleAuditoriaMantenimientoBO>();

            if (IsPostBack)
            {
                grvActividadesAuditoria.DataSource = DetalleAuditoria;
                grvActividadesAuditoria.DataBind();
            }

            #endregion

        }
        
        #endregion

        #region Métodos

        #region Accesos y Seguridad

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        #region Limpiar Session
        private void limpiarSession()
        {
            Session.Remove("DetalleAuditoria");
            Session.Remove("Auditoria");
            Session.Remove("MantenimientoAuditoria");
            Session.Remove("ArchivoEvidencia");
            Session.Remove("Resultado");

        }  
        #endregion
 
        #region MensajeSistema

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }
       
        
        #endregion

        #region Leer configuracio de extenciones permitidas

        public List<string> ObtenerConfiguracionFormatos()
        {
            List<string> list = new List<string>();
            string FormatosValidos = ConfigurationManager.AppSettings["FormatosValidos"];
              
                if (null != FormatosValidos)
                {
                    string[] extenciones = FormatosValidos.Split(',');
                    foreach (var item in extenciones)
	                {
		             list.Add(item);
	                }
                }
                return list;
        }

        #endregion

        #region Imprimir Auditoria
        
        /// <summary>
        /// Redirige a la impresion de la auditoria
        /// </summary>
        public void RedirigirAImprimir()
        {
            Response.Write("<script type='text/javascript'>window.open('../Buscador.UI/VisorReporteUI.aspx');</script>");
        }

        /// <summary>
        /// Limpia los datos en sesion de la auditoria
        /// </summary>
        public void LimpiarSessionAuditoria()
        {
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
        }

        /// <summary>
        /// Establece el nombre del reporte y los datos en sesion
        /// </summary>
        /// <param name="key">Nombre del reporte</param>
        /// <param name="value">Datos para la impresion del reporte</param>
        public void EstablecerValoresImpresion(string key, object value)
        {
            Session["NombreReporte"] = key;
            Session["DatosReporte"] = value;
        }
        #endregion

        #endregion

        #region Eventos

        #region Evento Guardar Auditoria
        /// <summary>
        /// Evento del boton Finalizar Audiditoria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                int error = this.presentador.GuardarAuditoria();

                if (error == 0)
                {
                    this.limpiarSession();
                    Response.Redirect("~/Mantenimiento.UI/RegistrarUnidadUI.aspx");
                }
                else if (error == 1)
                {
                    this.MostrarMensaje("Todas las actividades de la auditoria deben tener una calificación", ETipoMensajeIU.ADVERTENCIA);
                }
                else if (error == 2)
                {
                    this.MostrarMensaje("Debe adjuntar un documento de evidencia a la auditoria", ETipoMensajeIU.ADVERTENCIA);
                }
                else if (error == 3)
                {
                    this.MostrarMensaje("Debe llenar el campo de observaciones de auditoria", ETipoMensajeIU.ADVERTENCIA);
                }
                else if (error == 4)
                {
                    this.MostrarMensaje("El formato del documento de evidencia es incorrecto", ETipoMensajeIU.ADVERTENCIA);
                }


            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al guardar la Auditoria", ETipoMensajeIU.ERROR, string.Format("{0}.btnFinalizar_Click:{1}{2}", nombreClase, Environment.NewLine, ex.GetBaseException().Message));
            }


        }
        #endregion

        #region Evento Cancelar
        /// <summary>
        /// Evento del boton Cancelar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.limpiarSession();
            Response.Redirect("~/Mantenimiento.UI/RegistrarUnidadUI.aspx");
        }  
        #endregion

        #region Evento Imprimir Auditoria
        /// <summary>
        /// Evento del boton Imprimir Auditoria 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImprimirAuditoria_Click(object sender, EventArgs e)
        {
            this.presentador.ImprimirAuditoria();
        }
        #endregion

        #endregion

    }
}