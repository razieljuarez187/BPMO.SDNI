using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.MapaSitio.UI;
using System.Configuration;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class DetalleConfiguracionParametroMantenimientoUI : System.Web.UI.Page, IDetalleConfiguracionParametroMantenimientoVIS
    {

        #region Atributos
        private DetalleConfiguracionParametroMantenimientoPRE presentador = null;
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

        //public int? UC
        //{
        //    get
        //    {
        //        int? id = null;
        //        Site masterMsj = (Site)Page.Master;

        //        if (masterMsj.Usuario != null)
        //            id = masterMsj.Usuario.Id;
        //        return id;
        //    }
        //}


        #endregion

        #region Propiedades Interfaz
        
        /// <summary>
        /// Identificador de la configuracion de mantenimiento
        /// </summary>
        public int? ConfiguracionID
        {
            get
            {
                int? configuracionId = null;
                if (hdnConfiguracionID.Value != string.Empty)
                    configuracionId = int.Parse(hdnConfiguracionID.Value);
                return configuracionId = int.Parse(hdnConfiguracionID.Value);
                ;
            }
            set
            {
                if (value != null)
                    hdnConfiguracionID.Value = value.ToString();
                else
                    hdnConfiguracionID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Nombre del modelo
        /// </summary>
        public string Modelo
        {
            get
            {
                string modelo = string.Empty;
                if (txtModelo.Text != string.Empty)
                    modelo = txtModelo.Text;
                return modelo;
            }
            set
            {
                if (value != string.Empty)
                    txtModelo.Text = value;
                else
                    txtModelo.Text = string.Empty;
            }
        }
        /// <summary>
        /// Identificador del modelo
        /// </summary>
        public int? ModeloID
        {
            get
            {
                int? modeloId = null;
                if (hdnModeloID.Value != string.Empty)
                    modeloId = int.Parse(hdnModeloID.Value);
                return modeloId;
            }
            set
            {
                if (value != null)
                    hdnModeloID.Value = value.ToString();
                else
                    hdnModeloID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Tipo de mantenimiento de la configuracion
        /// </summary>
        private ETipoMantenimiento? tipoMantenimiento;

        public ETipoMantenimiento? TipoMantenimiento
        {
            get
            {
                tipoMantenimiento = (ETipoMantenimiento)Enum.Parse(typeof(ETipoMantenimiento), ddTipoMantenimiento.SelectedValue.ToString());
                return tipoMantenimiento;
            }
            set
            {
                if (value != null)
                {
                    if (value == ETipoMantenimiento.PMA)
                        ddTipoMantenimiento.SelectedIndex = 0;
                    if (value == ETipoMantenimiento.PMB)
                        ddTipoMantenimiento.SelectedIndex = 1;
                    if (value == ETipoMantenimiento.PMC)
                        ddTipoMantenimiento.SelectedIndex = 2;

                }
                else
                    ddTipoMantenimiento.SelectedValue.FirstOrDefault();
            }
        }
        /// <summary>
        /// Unidad de Medida de la configuracion
        /// </summary>
        private EUnidaMedida? unidadMendida;

        public EUnidaMedida? UnidadMedida
        {
            get
            {
                unidadMendida = (EUnidaMedida)Enum.Parse(typeof(EUnidaMedida), ddUnidadMedida.SelectedValue.ToString());
                return unidadMendida;
            }
            set
            {
                if (value != null)
                {
                    if (value == EUnidaMedida.Kilometros)
                        ddUnidadMedida.SelectedIndex = 0;
                    if (value == EUnidaMedida.Horas)
                        ddUnidadMedida.SelectedIndex = 1;
                    if (value == EUnidaMedida.Dias)
                        ddUnidadMedida.SelectedIndex = 2;
                }
                else
                    ddUnidadMedida.SelectedValue.FirstOrDefault();
            }
        }

        /// <summary>
        /// Tipo de uso o estado de la configuracion
        /// </summary>
        private bool enUso;

        public bool EnUso
        {
            get
            {
                if (ddEstado.SelectedValue == "EN USO")
                    enUso = true;
                else if (ddEstado.SelectedValue == "ESTACIONADO")
                    enUso = false;
                return enUso;
            }
            set
            {
                if (value == true)
                    ddEstado.SelectedValue = "EN USO";
                else if (value == false)
                    ddEstado.SelectedValue = "ESTACIONADO";
            }
        }

        /// <summary>
        /// Intervalo de la configuracion
        /// </summary>
        private int? intervalo;

        public int? Intervalo
        {
            get
            {
                if (txtIntervalo.Text != string.Empty)
                    intervalo = int.Parse(txtIntervalo.Text);
                return intervalo;
            }
            set
            {
                if (value != null)
                    txtIntervalo.Text = value.ToString();
                else
                    txtIntervalo.Text = string.Empty;
            }
        }
        /// <summary>
        /// Estatus de la configuracion en la base de datos 
        /// </summary>
        public bool Estatus
        {
            get
            {
                bool estatus = false;
                if (hdnEstatus.Value != string.Empty)
                    estatus = bool.Parse(hdnEstatus.Value);
                return estatus = bool.Parse(hdnEstatus.Value);
                
            }
            set
            {
                
                    hdnEstatus.Value = value.ToString();
               
            }
        }
        /// <summary>
        /// Control de GridView con las configuraciones de mantenimiento
        /// </summary>
        private GridView gridConfiguracionesMantenimiento;

        public GridView GridConfiguracionesMantenimiento
        {
            get
            {
                gridConfiguracionesMantenimiento = gvConfiguraciones;
                return gridConfiguracionesMantenimiento;
            }
            set
            {
                gridConfiguracionesMantenimiento = value;

            }
        }

        #endregion

        #region Varaibles Session

        /// <summary>
        /// Variable de Session con las configuraciones 
        /// </summary>
        public List<ConfiguracionMantenimientoBO> configuraciones
        {
            get
            {
                return Session["listaconfiguraciones"] as List<ConfiguracionMantenimientoBO>;
            }
            set
            {
                Session["listaconfiguraciones"] = value;
            }
        }
        /// <summary>
        /// Variable recibida para mostrar los detalles
        /// </summary>
        public ConfiguracionMantenimientoBO ConfiguracionRecibida
        {
            get
            {
                return Session["configuracionRecibida"] as ConfiguracionMantenimientoBO;
            }
            set
            {
                Session["configuracionRecibida"] = value;
            }
        }

        #endregion

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa los metodos y objetos principales 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nombreClase = this.GetType().Name;
                presentador = new DetalleConfiguracionParametroMantenimientoPRE(this);

                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.enlazarControles();
                    if (ConfiguracionRecibida != null)
                    {
                        if (ConfiguracionRecibida.Estatus == true)
                        {
                            this.presentador.ConsultarDetalle(ConfiguracionRecibida);
                            this.enlazarGrid();
                        }
                        else if (ConfiguracionRecibida.Estatus == false)
                        {
                            this.presentador.ConsultarDetalleInactivo(ConfiguracionRecibida);
                            this.btnEditar.Enabled = false;
                            this.mnuConfiguracionMantenimiento.Items[2].Enabled = true;
                            this.mnuConfiguracionMantenimiento.Items[0].Enabled = false;
                            this.mnuConfiguracionMantenimiento.Items[1].Enabled = false;
                            this.enlazarGrid();
                        }
                    }
                   
                }
                if (IsPostBack)
                {
                    this.enlazarGrid();
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la pagina", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load" + ex.Message);
            }


        }

        #endregion

        #region Metodos

        #region Accesos y Seguridad

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        /// <summary>
        /// Enlaza el gridView con las configuraciones
        /// </summary>
        private void enlazarGrid()
        {
            this.gvConfiguraciones.DataSource = configuraciones;
            this.gvConfiguraciones.DataBind();
        }

        #region Mensaje Sistema

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

        #region EnumToList

        private void enlazarControles()
        {
            var responseEnumTipo = Enum.GetNames(typeof(ETipoMantenimiento)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(ETipoMantenimiento), x) });
            ddTipoMantenimiento.DataSource = responseEnumTipo;
            ddTipoMantenimiento.DataTextField = "text";
            ddTipoMantenimiento.DataValueField = "value";
            ddTipoMantenimiento.DataBind();

            var responseEnumUnidad = Enum.GetNames(typeof(EUnidaMedida)).Select(x => new { text = x, value = (int)Enum.Parse(typeof(EUnidaMedida), x) });
            ddUnidadMedida.DataSource = responseEnumUnidad;
            ddUnidadMedida.DataTextField = "text";
            ddUnidadMedida.DataValueField = "value";
            ddUnidadMedida.DataBind();

        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos Grid

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //try
            //{
            //    gvConfiguraciones.PageIndex = e.NewPageIndex;
            //    DesplegarListaConfiguraciones();
            //}
            //catch (Exception ex)
            //{
            //    this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".gvConfiguraciones_PageIndexChanging:" + ex.Message);
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            try
            {
                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                    if (e.CommandName.Trim().Equals("Eliminar"))
                    {
                        this.configuraciones.Remove(configuraciones[index]);
                        this.gvConfiguraciones.DataSource = this.configuraciones;
                        this.gvConfiguraciones.DataBind();
                    }
                    if (e.CommandName.Trim().Equals("Editar"))
                    {
                        this.presentador.editarGrid(configuraciones[index]);
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al ejecutar evento: ", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConfiguraciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ConfiguracionMantenimiento = ((ConfiguracionMantenimientoBO)e.Row.DataItem);

                Label EnUso = e.Row.FindControl("lbEstaUso") as Label;

                if (ConfiguracionMantenimiento.EnUso == true)
                    EnUso.Text = "En Uso";
                else
                    EnUso.Text = "Estacionado";

            }
        }
        #endregion

        #region Guardar
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.GuardarConfiguracion();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al guardar los parametros de configuracion :", ETipoMensajeIU.ERROR, ex.ToString());
            }

        }



        protected void btnCancelar_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("~/Mantenimiento.UI/ConsultarConfiguracionParametroMantenimientoUI.aspx");

        }


        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.presentador.agregarAGrid();
        }

        #endregion

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            this.presentador.EstablecerSeguridad();
            this.presentador.redireccionar();
            Response.Redirect("~/Mantenimiento.UI/EditarConfiguracionParametroMantenimientoUI.aspx");
        }

        protected void menuSelecionado_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "Reactivar":
                    try
                    {
                        presentador.activarConfiguracion();
                        this.btnEditar.Enabled = true;
                        this.mnuConfiguracionMantenimiento.Items[2].Enabled = false;
                        (this.mnuConfiguracionMantenimiento.Items[0]).Enabled = true;
                    }
                    catch (Exception ex)
                    {

                        MostrarMensaje("Error al reactivar la configuracion : ", ETipoMensajeIU.ERROR, ex.ToString());
                    }

                    break;
                case "EliminarParametro":
                    try
                    {
                        this.presentador.redireccionar();
                        Response.Redirect("~/Mantenimiento.UI/EditarConfiguracionParametroMantenimientoUI.aspx?S=1");                  
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                    break;
                case "Editar":

                    this.presentador.EstablecerSeguridad();
                     this.presentador.redireccionar();
                     Response.Redirect("~/Mantenimiento.UI/EditarConfiguracionParametroMantenimientoUI.aspx");

                    break;
            }

        }

        

    }
}