//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucAsignacionEquiposAliadosUI : System.Web.UI.UserControl, IucAsignacionEquiposAliadosVIS
    {
        #region Atributos
        private ucAsignacionEquiposAliadosPRE presentador;
        private string nombreClase = "ucAsignacionEquiposAliadosUI";

        public enum ECatalogoBuscador
        {
            EquipoAliado
        }
        #endregion

        #region Propiedades
        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set
            {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }

        public string EquipoAliadoNumeroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEquipoAliado.Text)) ? null : this.txtEquipoAliado.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtEquipoAliado.Text = value;
                else
                    this.txtEquipoAliado.Text = string.Empty;
            }
        }
        public int? EquipoAliadoId
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEquipoAliadoID.Value))
                    id = int.Parse(this.hdnEquipoAliadoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoAliadoID.Value = value.ToString();
                else
                    this.hdnEquipoAliadoID.Value = string.Empty;
            }
        }

        public List<EquipoAliadoBO> EquiposAliados
        {
            get
            {
                if ((List<EquipoAliadoBO>)Session["ListaEquiposAliados"] == null)
                    return new List<EquipoAliadoBO>();
                else
                    return (List<EquipoAliadoBO>)Session["ListaEquiposAliados"];
            }
            set
            {
                Session["ListaEquiposAliados"] = value;
            }
        }
        public List<EquipoAliadoBO> UltimoEquiposAliados
        {
            get
            {
                if ((List<EquipoAliadoBO>)Session["LastListaEquiposAliados"] == null)
                    return new List<EquipoAliadoBO>();
                else
                    return (List<EquipoAliadoBO>)Session["LastListaEquiposAliados"];
            }
            set
            {
                Session["ListaEquiposAliados"] = value;
            }
        }

        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)this.Parent.Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioAutenticado.Value))
                    id = int.Parse(this.hdnUsuarioAutenticado.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioAutenticado.Value = value.ToString();
                else
                    this.hdnUsuarioAutenticado.Value = string.Empty;
            }
        }
        public List<int?> SucursalesSeguridad
        {
            get
            {
                if ((List<int?>)Session["ListaSucursalesSeguridad_ucAsignacion"] == null)
                    return new List<int?>();
                else
                    return (List<int?>)Session["ListaSucursalesSeguridad_ucAsignacion"];
            }
            set
            {
                Session["ListaSucursalesSeguridad_ucAsignacion"] = value;
            }
        }

        #region SC0002

        private GridView gridAliados;

        public GridView GridAliados
        {
            get
            {
                gridAliados = this.grvEquiposAliados;
                return gridAliados;
            }
            set
            {
                this.grvEquiposAliados = value;
            }
        }

        public bool? habilitarCheckEntrada 
        {
            get
            {
                bool? chx = false;
                if ((bool?)Session["ActivarCheckAliado"] == null)
                    return chx;
                else
                    return (bool?)Session["ActivarCheckAliado"];
            }
            set
            {
                Session["ActivarCheckAliado"] = value;
            }
        }

        public bool? habilitarBotonEliminar { get; set; }

        #endregion
   

        
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucAsignacionEquiposAliadosPRE(this);
           
        }
        #endregion

        #region Métodos

        public void PrepararNuevo()
        {
            this.PrepararNuevoEquipoAliado();

            this.grvEquiposAliados.DataSource = null;
            this.grvEquiposAliados.DataBind();
        }
        public void PrepararNuevoEquipoAliado()
        {
            this.txtEquipoAliado.Text = "";
            this.hdnEquipoAliadoID.Value = "";
        }

        public void HabilitarModoEdicion(bool habilitar)
        {
            this.btnAgregar.Enabled = habilitar;
            this.ibtnBuscaEquipoAliado.Enabled = habilitar;
            this.txtEquipoAliado.Enabled = habilitar;

            this.grvEquiposAliados.Enabled = habilitar;
        }

        public void ActualizarEquiposAliados()
        {
            this.grvEquiposAliados.DataSource = this.EquiposAliados;
            this.grvEquiposAliados.DataBind();
        }

        public void LimpiarSesion()
        {
            if (Session["ListaEquiposAliados"] != null)
                Session.Remove("ListaEquiposAliados");
            if (Session["LastListaEquiposAliados"] != null)
                Session.Remove("LastListaEquiposAliados");
            if (Session["ListaSucursalesSeguridad_ucAsignacion"] != null)
                Session.Remove("ListaSucursalesSeguridad_ucAsignacion");
            if (Session["ActivarCheckAliado"] != null)
                Session.Remove("ActivarCheckAliado");
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion

        #region REQ 13285 Métodos relacionados con las acciones dependiendo de la unidad operativa.
        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa Generación.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa)
        {
            //Visibilidad de la columna Hrs Iniciales
            if (tipoEmpresa == ETipoEmpresa.Generacion)
            {
                this.grvEquiposAliados.Columns[6].Visible = false;
            }
            
        }

        #endregion
        #endregion

        #region Eventos
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarEquipoAliado();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el equipo aliado a la unidad", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }

        protected void grvEquiposAliados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName.ToString().ToUpper() == "PAGE") return;

            try
            {
                index = Convert.ToInt32(e.CommandArgument.ToString());
                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        this.presentador.QuitarEquipoAliado(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre el equipo aliado", ETipoMensajeIU.ERROR, this.nombreClase + ".grvEquiposAliados_RowCommand:" + ex.Message);
            }
        }

        #region SC0002
        protected void grvEquiposAliados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var equipoAliado = ((EquipoAliadoBO)e.Row.DataItem);
                CheckBox chbxEntra = (CheckBox)e.Row.FindControl("chbxEntraMantenimiento");
                ImageButton botonEliminar = (ImageButton)e.Row.FindControl("imgDelete");

                if (this.habilitarCheckEntrada == true)
                {
                    if(equipoAliado.EntraMantenimiento == true)
                    chbxEntra.Checked = true;
                    if (equipoAliado.EntraMantenimiento == false)
                    chbxEntra.Checked = false;
                }
                else if (this.habilitarCheckEntrada == false)
                {
                    //chbxEntra.Enabled = false;
                    chbxEntra.Checked = false;
                }
            }
                
        }

        protected void grvEquiposAliados_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }
        
        #endregion

        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Equipo Aliado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtEquipoAliado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.EquipoAliadoNumeroSerie;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.EquipoAliado);

                this.EquipoAliadoNumeroSerie = numeroSerie;
                if (this.EquipoAliadoNumeroSerie != null)
                {
                    this.EjecutaBuscador("EquipoAliado", ECatalogoBuscador.EquipoAliado);
                    this.EquipoAliadoNumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Tipo de Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtEquipoAliado_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaEquipoAliado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("EquipoAliado&hidden=0", ECatalogoBuscador.EquipoAliado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Equipo Aliado", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaEquipoAliado_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.EquipoAliado:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }
        #endregion

        protected void chbxEntraMantenimiento_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Entrar = (CheckBox)sender;
            GridViewRow rowSelected = Entrar.Parent.Parent as GridViewRow;

            if (habilitarCheckEntrada == false && Entrar.Checked == true)
            {
                Entrar.Checked = false;
                this.MostrarMensaje("No se puede poner un Equipo Aliado en mantenimiento si la unidad no entra a mantenimiento", ETipoMensajeIU.ADVERTENCIA);
            }
        }

       
        #endregion
    }
}