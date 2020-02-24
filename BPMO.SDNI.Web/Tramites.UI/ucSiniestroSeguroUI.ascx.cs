//Satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucSiniestroSeguroUI : System.Web.UI.UserControl, IucSiniestroSeguroVIS
    {
        #region Atributos
        private ucSiniestroSeguroPRE presentador;
        private string nombreClase = "ucSiniestroSeguroUI";
        #endregion 

        #region Propiedades
        public CommandEventHandler Editar_Siniestro { get; set; }
        public CommandEventHandler Siniestro_Editado { get; set; }
        public int? SiniestroID
        {
            get
            {
                int id;
                if (!string.IsNullOrEmpty(this.hdnSiniestroID.Value) && !string.IsNullOrWhiteSpace(this.hdnSiniestroID.Value))
                    if (Int32.TryParse(this.hdnSiniestroID.Value, out id))
                        return id;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSiniestroID.Value = value.ToString();
                else
                    this.hdnSiniestroID.Value = string.Empty;
            }
        }

        public string Numero
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumero.Text) && !string.IsNullOrWhiteSpace(this.txtNumero.Text))
                    return this.txtNumero.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtNumero.Text = value;
                else
                    this.txtNumero.Text = string.Empty;
            }
        }

        public DateTime? Fecha
        {
            get
            {
                DateTime date;
                if(!string.IsNullOrEmpty(this.txtFecha.Text) && !string.IsNullOrWhiteSpace(this.txtFecha.Text))
                    if(DateTime.TryParse(this.txtFecha.Text, out date))
                        return date;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtFecha.Text = value.Value.ToShortDateString();
                else
                    this.txtFecha.Text = string.Empty;
            }
        }

        public string Descripcion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtDescripcion.Text) && !string.IsNullOrWhiteSpace(this.txtDescripcion.Text))
                    return this.txtDescripcion.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtDescripcion.Text = value;
                else
                    this.txtDescripcion.Text = string.Empty;
            }
        }

        public string Estatus
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEstatus.Text) && !string.IsNullOrWhiteSpace(this.txtEstatus.Text))
                    return this.txtEstatus.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtEstatus.Text = value;
                else
                    this.txtEstatus.Text = string.Empty;
            }
        }

        public int IndicePaginaResultado
        {
            get { return this.grdSiniestros.PageIndex; }
            set { this.grdSiniestros.PageIndex = value; }
        }

        public List<SiniestroBO> Siniestros
        {
            get
            {
                if ((List<SiniestroBO>)this.Session["Siniestros"] == null)
                    return new List<SiniestroBO>();
                else
                    return (List<SiniestroBO>)this.Session["Siniestros"];
            }
            set { this.Session["Siniestros"] = value; }
        }

        public List<SiniestroBO> SiniestrosBorrados
        {
            get
            {
                if ((List<SiniestroBO>)this.Session["SiniestrosBorrados"] == null)
                    return new List<SiniestroBO>();
                else
                    return (List<SiniestroBO>)this.Session["SiniestrosBorrados"];
            }
            set { this.Session["SiniestrosBorrados"] = value; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucSiniestroSeguroPRE(this);
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.hdnSiniestroID.Value = string.Empty;
            this.hdnTramiteID.Value = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtEstatus.Text = string.Empty;
            this.txtFecha.Text = string.Empty;
            this.txtNumero.Text = string.Empty;

            this.txtDescripcion.Enabled = true;
            this.txtEstatus.Enabled = true;
            this.txtFecha.Enabled = true;
            this.txtNumero.Enabled = true;

            this.txtDescripcion.Visible = true;
            this.txtEstatus.Visible = true;
            this.txtFecha.Visible = true;
            this.txtNumero.Visible = true;

            this.grdSiniestros.Columns[8].Visible = false;
            this.grdSiniestros.Columns[6].Visible = false;
            this.grdSiniestros.Columns[7].Visible = true;
            this.grdSiniestros.Columns[5].Visible = true;
        }
        public void PrepararEdicion()
        {
            this.hdnSiniestroID.Value = string.Empty;
            this.hdnTramiteID.Value = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtEstatus.Text = string.Empty;
            this.txtFecha.Text = string.Empty;
            this.txtNumero.Text = string.Empty;

            this.txtDescripcion.Enabled = true;
            this.txtEstatus.Enabled = true;
            this.txtFecha.Enabled = true;
            this.txtNumero.Enabled = true;

            this.txtDescripcion.Visible = true;
            this.txtEstatus.Visible = true;
            this.txtFecha.Visible = true;
            this.txtNumero.Visible = true;

            this.grdSiniestros.Columns[4].Visible = true;
            this.grdSiniestros.Columns[5].Visible = false;
            this.grdSiniestros.Columns[7].Visible = true;
            this.grdSiniestros.Columns[6].Visible = false;
            this.grdSiniestros.Columns[8].Visible = false;
        }
        public void PrepararVista()
        {
            this.hdnSiniestroID.Value = string.Empty;
            this.hdnTramiteID.Value = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtEstatus.Text = string.Empty;
            this.txtFecha.Text = string.Empty;
            this.txtNumero.Text = string.Empty;

            this.txtDescripcion.Enabled = false;
            this.txtEstatus.Enabled = false;
            this.txtFecha.Enabled = false;
            this.txtNumero.Enabled = false;

            this.txtDescripcion.Visible = true;
            this.txtEstatus.Visible = true;
            this.txtFecha.Visible = true;
            this.txtNumero.Visible = true;
        }
        public void ActualizarLista()
        {
            this.grdSiniestros.DataSource = this.Siniestros;
            this.grdSiniestros.DataBind();
        }
        public void ReestablecerColumnas()
        {

            this.grdSiniestros.Columns[1].Visible = true;
            this.grdSiniestros.Columns[2].Visible = true;
            this.grdSiniestros.Columns[3].Visible = true;
            this.grdSiniestros.Columns[4].Visible = true;
            this.grdSiniestros.Columns[5].Visible = true;
            this.grdSiniestros.Columns[6].Visible = true;
            this.grdSiniestros.Columns[7].Visible = false;
        }
        #region SC0004
        public void ColumnasModoRegistrar()
        {
            
            this.grdSiniestros.Columns[1].Visible = true;
            this.grdSiniestros.Columns[2].Visible = true;
            this.grdSiniestros.Columns[3].Visible = true;
            this.grdSiniestros.Columns[4].Visible = true;
            this.grdSiniestros.Columns[5].Visible = true;
            this.grdSiniestros.Columns[6].Visible = false;
            this.grdSiniestros.Columns[7].Visible = false;
        }
        public void HabilitarControles()
        {
            this.btnAgregar.Enabled = true;
            this.txtDescripcion.Enabled = true;
            this.txtEstatus.Enabled = true;
            this.txtFecha.Enabled = true;
            this.txtNumero.Enabled = true;
            
        }
        public void DeshabilitarControles()
        {
            this.btnAgregar.Enabled = false;
            this.txtDescripcion.Enabled = false;
            this.txtEstatus.Enabled = false;
            this.txtFecha.Enabled = false;
            this.txtNumero.Enabled = false;
        }
        #endregion
        public void LimpiarSesion()
        {
            if ((List<SiniestroBO>)this.Session["Siniestros"] != null)
                Session.Remove("Siniestros");
            if ((List<SiniestroBO>)this.Session["SiniestrosBorrados"] != null)
                Session.Remove("SiniestrosBorrados");
        }
        public void LimpiarCampos()
        {
            this.txtDescripcion.Text = string.Empty;
            this.txtEstatus.Text = string.Empty;
            this.txtFecha.Text = string.Empty;
            this.txtNumero.Text = string.Empty;
        }

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
        #endregion

        #region Eventos
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarElemento();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el deducible, veririque por favor", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }

        protected void grdSiniestros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grdSiniestros_PageIndexChanging:" + ex.Message);
            }
        }

        protected void grdSiniestros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;

                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        this.presentador.RemoverElemento(index);
                        break;

                        #region CU0004

                    case "Editar":
                        Label lbl = this.grdSiniestros.Rows[index].FindControl("lblEstatusSiniestro") as Label;
                        lbl.Visible = false;
                        TextBox txt =  this.grdSiniestros.Rows[index].FindControl("txtEstatusSiniestro") as TextBox;             
                        txt.ReadOnly = false;
                        txt.Visible = true;
                        ImageButton ibtn = this.grdSiniestros.Rows[index].FindControl("ibtAceptar") as ImageButton;
                        ibtn.Visible = true;
                        this.grdSiniestros.Columns[5].Visible = false;
                        this.grdSiniestros.Columns[6].Visible = false;
                        this.grdSiniestros.Columns[7].Visible = true;
                        this.DeshabilitarControles();
                        if (Editar_Siniestro != null)
                        {
                            Editar_Siniestro.Invoke(sender,null);
                        }
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "grdSiniestros_RowCommand_Editar" + DateTime.Now.Ticks.ToString(), "$('#tabs').tabs({active:2});", true);
                        break;
                    case "Aceptar":
                        TextBox txtSiniestro = this.grdSiniestros.Rows[index].FindControl("txtEstatusSiniestro") as TextBox;
                        this.Estatus = txtSiniestro.Text;
                        this.presentador.ActualizarEstatus(index);
                        if (Siniestro_Editado != null)
                        {
                            Siniestro_Editado.Invoke(sender, null);
                        }
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "grdSiniestros_RowCommand_Aceptar" + DateTime.Now.Ticks.ToString(), "$('#tabs').tabs({active:2});", true);
                        break;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdSiniestros_RowCommand: " + ex.Message);
            }
        }
        #region SC0004
        protected void grdSiniestros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    SiniestroBO bo = e.Row.DataItem != null ? (SiniestroBO)e.Row.DataItem : new SiniestroBO();
                    if (bo.SiniestroID != null)
                    {
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/BORRAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEditar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/EDITAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtAceptar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/GUARDAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).Visible = true;
                        ((ImageButton)e.Row.FindControl("ibtEditar")).Visible = true;
                        ((ImageButton)e.Row.FindControl("ibtAceptar")).Visible = false;
                    }
                    else
                    {
                        ((ImageButton)e.Row.FindControl("ibtEliminar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ELIMINAR-ICO.png");
                        ((ImageButton)e.Row.FindControl("ibtEditar")).ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/EDITAR-ICO.png");
                        ((ImageButton) e.Row.FindControl("ibtEliminar")).Visible = true;
                        ((ImageButton)e.Row.FindControl("ibtEditar")).Visible = false;
                        ((ImageButton)e.Row.FindControl("ibtAceptar")).Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al desplegar los Siniestros", ETipoMensajeIU.ERROR, this.nombreClase + ".grdSiniestros_RowDataBound" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}