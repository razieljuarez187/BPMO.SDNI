//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;
using System.Drawing;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ConsultarSeguroUI : System.Web.UI.Page, IConsultarSeguroVIS
    {
        #region Atributos
        private ConsultarSeguroPRE presentador;
        private string nombreClase = "ConsultarSeguroUI";
        public enum ECatalogoBuscador
        {
            Unidad,
            Marca,
            Modelo,
            TipoUnidad,
            Distribuidor,
            Motorizacion,
            Aplicacion
        }
        #endregion

        #region Propiedades
        public string VIN
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtVIN.Text) && !string.IsNullOrWhiteSpace(this.txtVIN.Text))
                    return this.txtVIN.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtVIN.Text = value;
                else
                    this.txtVIN.Text = string.Empty;
            }
        }

        public string NumeroPoliza
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumeroPoliza.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroPoliza.Text))
                    return this.txtNumeroPoliza.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtNumeroPoliza.Text = value;
                else
                    this.txtNumeroPoliza.Text = string.Empty;
            }
        }

        public string Aseguradora
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtAseguradora.Text) && !string.IsNullOrWhiteSpace(this.txtAseguradora.Text))
                    return this.txtAseguradora.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtAseguradora.Text = value;
                else
                    this.txtAseguradora.Text = string.Empty;
            }
        }

        public int? TramitableID 
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrEmpty(this.hdnVIN.Value) && !string.IsNullOrWhiteSpace(this.hdnVIN.Value))
                    if (Int32.TryParse(this.hdnVIN.Value, out id))
                        return id;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnVIN.Value = value.Value.ToString();
                else
                    this.hdnVIN.Value = string.Empty;
            }
        }

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

        public List<SeguroBO> Seguros 
        { 
            get 
            {
                if ((List<SeguroBO>)this.Session["Seguros"] == null)
                    return new List<SeguroBO>();
                else
                    return (List<SeguroBO>)this.Session["Seguros"];
            } 
            set 
            {
                this.Session["Seguros"] = value;
            } 
        }

        public Boolean? Vencido
        {
            get
            {
                if(this.ddlProximaVencer.SelectedValue == "-1") return null;
                return this.ddlProximaVencer.SelectedValue == "1";
            }
            set
            {
                if(value != null)
                {
                    this.ddlProximaVencer.SelectedValue = value.Value ? "1" : "0";
                }
                else
                    this.ddlProximaVencer.SelectedIndex = 0;
            }
        }

        #region SC0008
        public int? UsuarioId
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
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            this.presentador = new ConsultarSeguroPRE(this);
            if (!this.IsPostBack)
                this.presentador.PrepararBusqueda();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }        
        #endregion

        #region Métodos
        public void PrepararBusqueda()
        {
            this.txtAseguradora.Text = string.Empty;
            this.txtNumeroPoliza.Text = string.Empty;
            this.txtVIN.Text = string.Empty;

            this.grdSeguros.DataSource = null;
            this.grdSeguros.DataBind();

            this.LimpiarSesion();
        }

        public void LimpiarSesion()
        {
            if ((List<SeguroBO>)this.Session["Seguros"] != null)
                Session.Remove("Seguros");
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        public void ActualizarLista()
        {
            this.grdSeguros.DataSource = null;
            this.grdSeguros.DataSource = this.Seguros;
            this.grdSeguros.DataBind();
        }

        public void EstablecerPaqueteNavegacion(string Clave, int? tramiteID)
        {
            SeguroBO seguro = this.Seguros.FirstOrDefault(x => x.TramiteID.Value == tramiteID.Value);

            if (seguro != null) Session[Clave] = seguro;
            else
            {
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: El seguro proporcionado no pertence al listado de seguros encontrados.");
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

        public void IrADetalle()
        {
            const string Url = "DetalleSeguroUI.aspx";
            Response.Redirect(Url);
        }

        #region SC0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try 
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar los seguros", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }

        protected void grdSeguros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    SeguroBO seguro = (SeguroBO)e.Row.DataItem;

                    Label label = e.Row.FindControl("lblModelo") as Label;
                    if (label != null)
                    {
                        string tipo = string.Empty;
                        if (((UnidadBO)seguro.Tramitable) != null)
                            if (((UnidadBO)seguro.Tramitable).Modelo != null)
                                label.Text = ((UnidadBO)seguro.Tramitable).Modelo.Nombre;
                    }

                    Label labelVIN = e.Row.FindControl("lblVIN") as Label;
                    if (labelVIN != null)
                    {
                        string tipo = string.Empty;
                        if (((UnidadBO)seguro.Tramitable) != null)
                            if (((UnidadBO)seguro.Tramitable).DescripcionTramitable != null)
                                labelVIN.Text = ((UnidadBO)seguro.Tramitable).DescripcionTramitable;
                    }

                    Label lblProximoVencer = e.Row.FindControl("lblProximoVencer") as Label;
                    if(lblProximoVencer != null)
                    {
                        int? dias = seguro.DiasParaVencimiento();
                        if(dias == null)
                            lblProximoVencer.Text = String.Empty;
                        else
                            lblProximoVencer.Text = seguro.DiasParaVencimiento().ToString();

                        bool? proximoVencer = seguro.ProximoAVencer();
                        if(proximoVencer != null)
                        {
                            var celda = e.Row.Cells[e.Row.Cells.Count - 2];
                            celda.BackColor = proximoVencer.Value ? Color.FromArgb(222, 8, 20) : e.Row.Cells[e.Row.Cells.Count - 1].BackColor;
                            lblProximoVencer.ForeColor = proximoVencer.Value ? Color.White : e.Row.Cells[e.Row.Cells.Count - 3].ForeColor;
                        }
                        else
                        {
                            var celda = e.Row.Cells[e.Row.Cells.Count - 2];
                            celda.BackColor = e.Row.Cells[e.Row.Cells.Count - 1].BackColor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar los seguros", ETipoMensajeIU.ERROR, this.nombreClase + ".grdSeguros_DataBound:" + ex.Message);
            }
        }

        protected void grdSeguros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try 
            {
                this.grdSeguros.DataSource = this.Seguros;
                this.grdSeguros.PageIndex = e.NewPageIndex;
                this.grdSeguros.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".grdSeguros_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdSeguros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim())
                {
                    case "Detalles":
                        {
                            int? tramiteID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                            presentador.IrADetalle(tramiteID);
                            break;
                        }
                    case "Page":
                        break;
                    default:
                        {
                            this.MostrarMensaje("Comando no encontrado", ETipoMensajeIU.ERROR,"El comando no está especificado en el sistema");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al intentar realizar una acción sobre un seguro:", ETipoMensajeIU.ERROR, this.nombreClase + ".grdSeguros_RowCommand: " + ex.Message);
            }
        }

        protected void txtVIN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.txtVIN.Text.Trim().CompareTo("") != 0 || this.txtVIN.Text.Trim().CompareTo("") != 0)
                {
                    string serieUnidad = this.VIN;
                    this.Session_BOSelecto = null;

                    this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                    this.VIN = serieUnidad;
                    if (this.VIN != null)
                    {
                        this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.Unidad);
                        this.VIN = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txbUnidad_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscaUnidad_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaUnidad()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaUnidad_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:                        
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Marca:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.TipoUnidad:
                    case ECatalogoBuscador.Aplicacion:
                    case ECatalogoBuscador.Distribuidor:
                    case ECatalogoBuscador.Motorizacion:
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
    }
}