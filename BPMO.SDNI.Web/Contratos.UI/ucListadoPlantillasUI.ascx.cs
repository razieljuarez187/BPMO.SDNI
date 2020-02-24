// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PRE;
using BPMO.SDNI.Contratos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.UI
{
    public partial class ucListadoPlantillasUI : System.Web.UI.UserControl, IucListadoPlantillasVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "ucListadoPlantillasUI";
        /// <summary>
        /// Presentador de la página
        /// </summary>
        private ucListadoPlantillasPRE presentador = null;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador del control
        /// </summary>
        public string Identificador
        {
            get
            {
                return !string.IsNullOrEmpty(this.hdnIdentificador.Value) && !string.IsNullOrWhiteSpace(this.hdnIdentificador.Value)
                           ? this.hdnIdentificador.Value
                           : string.Empty;
            }
            set
            {
                this.hdnIdentificador.Value = !string.IsNullOrEmpty(this.hdnIdentificador.Value) && !string.IsNullOrWhiteSpace(this.hdnIdentificador.Value)
                                              ? this.hdnIdentificador.Value : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el listado de archivos encontrados
        /// </summary>
        public List<object> Documentos
        {
            get
            {
                List<PlantillaBO> lista = null;
                if (Session[this.Identificador] != null)
                    lista = new List<PlantillaBO>();

                return (List<object>)(Session[this.Identificador] ?? lista);
            }
            set { Session[this.Identificador] = value; }
        }
        /// <summary>
        /// Manejador de Eventos para Eliminar un archivo
        /// </summary>
        internal EventHandler EliminarArchivo { get; set; }
        /// <summary>
        /// Obtiene o establece el indice de la página de resultados
        /// </summary>
        public int IndicePaginaResultado
        {
            get { return this.grdArchivos.PageIndex; }
            set { this.grdArchivos.PageIndex = value; }
        }
        /// <summary>
        /// Obtiene o establece si se puede ejecutar la accion de borrado en el control
        /// </summary>
        public bool? PermitirEliminar
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnModoEdicion.Value))
                    id = bool.Parse(this.hdnModoEdicion.Value.Trim());
                return id;
            }
            set { this.hdnModoEdicion.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del archivo que se va a eliminar
        /// </summary>
        public int? ArchivoEliminarID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnArchivoEliminar.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnArchivoEliminar.Value)
                           ? (Int32.TryParse(this.hdnArchivoEliminar.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnArchivoEliminar.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de la página web
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucListadoPlantillasPRE(this);
        }
        #endregion

        # region Métodos
        /// <summary>
        /// Este método despliega un mensaje en pantalla
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
        /// <summary>
        /// Carga y despliega los archivos que cumplen con los filtros de búsqueda en el grid de detalles
        /// </summary>
        /// <param name="elementos">Lista de archivos que cumplen con los filtros proporcionados para la consulta</param>
        public void CargarElementosEncontrados(List<object> elementos)
        {
            this.grdArchivos.DataSource = elementos;
            this.grdArchivos.DataBind();
        }
        /// <summary>
        /// Actualiza la informaciónq eu se visualiza en el grid de detalles
        /// </summary>
        public void ActualizarResultado()
        {
            this.grdArchivos.DataSource = this.Documentos;
            this.grdArchivos.DataBind();
        }
        /// <summary>
        /// Prepara el control para el modo de visualización
        /// </summary>
        /// <param name="status">status aplicado al control</param>
        public void PrepararVista(bool status)
        {
            this.grdArchivos.Columns[2].Visible = status;
        }
        /// <summary>
        /// Prepara el control para el modo de edición
        /// </summary>
        /// <param name="status">Estatus aplicado al control</param>
        public void PrepararEdicion(bool status)
        {
            this.grdArchivos.Columns[2].Visible = status;
        }
        #endregion

        #region Eventos
        protected void grdArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                PlantillaBO archivoBO = (PlantillaBO)e.Row.DataItem;
                Label lblExtension = e.Row.FindControl("lblExtension") as Label;
                if (lblExtension != null)
                {
                    string extension = string.Empty;
                    if (archivoBO.TipoArchivo != null)
                        if (archivoBO.TipoArchivo.Extension != null)
                        {
                            extension = archivoBO.TipoArchivo.Extension;
                        }
                    lblExtension.Text = extension;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (archivoBO.Id != null)
                    {
                        ImageButton imgBtn = (ImageButton)e.Row.FindControl("ibtDescargar");
                        imgBtn.OnClientClick = "javascript:window.open('../Contratos.UI/hdlrDescargarPlantilla.ashx?archivoID=" + archivoBO.Id + "'); return false;";
                    }

                    if (this.PermitirEliminar != null)
                    {
                        if (this.PermitirEliminar == false)
                        {
                            ImageButton imgBtn = (ImageButton)e.Row.FindControl("ibtEliminar");
                            imgBtn.Visible = false;
                        }
                    }

                    if (archivoBO.Activo.HasValue)
                        if (archivoBO.Activo == false)
                        {
                            ImageButton imgBtn = (ImageButton)e.Row.FindControl("ibtEliminar");
                            imgBtn.Visible = false;
                            ImageButton imgBtnd = (ImageButton)e.Row.FindControl("ibtDescargar");
                            imgBtnd.Visible = false;
                        }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio alguna inconcistencia al intentar ejecutar la acción", ETipoMensajeIU.ERROR, string.Format("{0}.grdArchivos_RowDataBound:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void grdArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim())
                {
                    case "eliminar":
                        var index = Convert.ToInt32(e.CommandArgument);
                        if (this.Documentos != null)
                            if (this.Documentos.Count > 0)
                                this.ArchivoEliminarID = ((PlantillaBO)this.Documentos[index]).Id;

                        if (this.EliminarArchivo != null) EliminarArchivo.Invoke(sender, EventArgs.Empty);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Ocurrio alguna inconcistencia al intentar ejecutar la acción", ETipoMensajeIU.ERROR, string.Format("{0}.grdArchivos_RowCommand{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }

        protected void grdArchivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
                if (this.PermitirEliminar.HasValue)
                    if (!this.PermitirEliminar.Value)
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "Detalles", "DialogoDetallePlantillas();", true);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grdListadosVerificacion_PageIndexChanging:" + ex.Message);
            }
        }
        #endregion
    }
}