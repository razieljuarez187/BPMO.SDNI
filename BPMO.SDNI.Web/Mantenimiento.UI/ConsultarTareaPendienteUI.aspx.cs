//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Mantenimiento.BOF;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// Clase para el control de la UI de consultar tarea pendiente
    /// </summary>
    public partial class ConsultarTareaPendienteUI : System.Web.UI.Page, IConsultarTareaPendienteVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de consultar tareas pendientes
        /// </summary>
        private ConsultarTareaPendientePRE presentador = null;

        /// <summary>
        /// Nombre de clase
        /// </summary>
        private const string nombreClase = "ConsultarTareaPendienteUI";
        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o establece una lista de tareas pendientes
        /// </summary>
        public List<BO.TareaPendienteBO> Tareas
        {
            get { return Session["listTareas"] != null ? Session["listTareas"] as List<TareaPendienteBO> : null; }
            set { Session["listTareas"] = value; }
        }

        /// <summary>
        /// Obtiene o establece un indice de pagina
        /// </summary>
        public int IndicePaginaResultado
        {
            get { return this.gvTareas.PageIndex; }
            set { this.gvTareas.PageIndex = value; }
        }
        #endregion

        #region Metodos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarTareaPendientePRE(this, this.ucTareasPendientesUI);
                this.ucTareasPendientesUI.PlaceHolderDescripcionRow.Visible = false;
                if (!IsPostBack)
                {
                    this.presentador.ValidarAcceso();


                    this.Tareas = null;
                    ActualizarResultado();

                    if (Session["RecargarTareas"] == null)
                    {
                        Session["PaginaActual"] = null;
                        if (Tareas == null)
                        {
                            Tareas = new List<TareaPendienteBO>();
                        }
                        else
                        {
                            ActualizarResultado();
                        }
                        if (Session["TareaPendienteBOF"] != null)
                        {
                            var tarea = Session["TareaPendienteBOF"] as TareaPendienteBOF;
                            this.ucTareasPendientesUI.NumeroEconomico = tarea.NumeroEconomico;
                            this.ucTareasPendientesUI.NumeroSerie = tarea.Serie;
                            this.ucTareasPendientesUI.Modelo = tarea.Modelo;
                            this.ucTareasPendientesUI.ModeloID = tarea.ModeloID;
                            this.ucTareasPendientesUI.UnidadID = tarea.UnidadID;
                            this.presentador.Consultar();
                        }
                    }
                    else
                    {
                        if (Session["PaginaActual"] != null)
                        {
                            this.presentador.CambiarPaginaResultado((int)Session["PaginaActual"]);
                        }
                        Session["RecargarTareas"] = null;
                        ActualizarResultado();
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// Redirecciona a PaginaSinAccesoUI en caso no se tengan permisos para la pagina
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Muestra un mensaje en la UI
        /// <param name="mensaje">Mensaje por desplegar</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="msjDetalle">Detalle del mensaje</param>
        /// </summary>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }

        /// <summary>
        /// Actualiza el resultado en la UI
        /// </summary>
        public void ActualizarResultado()
        {
            this.gvTareas.DataSource = this.Tareas;
            this.gvTareas.DataBind();
        }

        /// <summary>
        /// Establece datos por desplegar en la UI
        /// <param name="nombre">Identificador de objeto</param>
        /// <param name="valor">Valor por desplegar</param>
        /// </summary>
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }

        /// <summary>
        /// Redirige a la UI de detalles
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/EditarTareaPendienteUI.aspx"));
        }

        /// <summary>
        /// Limpia los datos en sesion
        /// </summary>
        public void LimpiarSesion()
        {
            Session["listTareas"] = null;
            Session["PaginaActual"] = null;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento para el boton buscar
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Consultar();
                Session["TareaPendienteBOF"] = null;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar tareas pendientes", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el cambio de pagina del grid
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void gvTareas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".gvTareas_PageIndexChanging:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el detalle del grid
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void gvTareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;
            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString()) + (((GridView)sender).PageIndex * ((GridView)sender).PageSize);
                switch (e.CommandName.Trim())
                {
                    case "Ver":
                        Session["PaginaActual"] = ((GridView)sender).PageIndex;
                        this.presentador.VerDetalles(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre la tarea", ETipoMensajeIU.ERROR, nombreClase + ".gvTareas_RowCommand:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el bind de datos al grid
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void gvTareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TareaPendienteBO bo = (TareaPendienteBO)e.Row.DataItem;
                    Label labelNumeroSerie = e.Row.FindControl("lblNumeroSerie") as Label;
                    if (labelNumeroSerie != null)
                    {
                        string numeroSerie = string.Empty;
                        if (bo.Unidad != null)
                            if (bo.Unidad.NumeroSerie != null)
                            {
                                numeroSerie = bo.Unidad.NumeroSerie;
                            }
                        labelNumeroSerie.Text = numeroSerie;
                    }
                    Label labelNumeroEconomico = e.Row.FindControl("lblNumeroEconomico") as Label;
                    if (labelNumeroEconomico != null)
                    {
                        string numeroEconomico = string.Empty;
                        if (bo.Unidad != null)
                            if (bo.Unidad.NumeroEconomico != null)
                            {
                                numeroEconomico = bo.Unidad.NumeroEconomico;
                            }
                        labelNumeroEconomico.Text = numeroEconomico;
                    }
                    Label labelModelo = e.Row.FindControl("lblModelo") as Label;
                    if (labelModelo != null)
                    {
                        string modelo = string.Empty;
                        if (bo.Modelo != null)
                            if (bo.Modelo.Nombre != null)
                            {
                                modelo = bo.Modelo.Nombre;
                            }
                        labelModelo.Text = modelo;
                    }                
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre la tarea pendiente", ETipoMensajeIU.ERROR, nombreClase + ".gvTareas_RowDataBound:" + ex.Message);
            }
        }
    }
        #endregion
}