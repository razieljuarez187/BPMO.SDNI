//Satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucDeducibleSeguroUI : System.Web.UI.UserControl, IucDeducibleSeguroVIS
    {
        #region Atributos
        private ucDeducibleSeguroPRE presentador;
        private string nombreClase = "ucDeducibleSeguroUI";
        #endregion        

        #region Propiedades
        public int? DeducibleID 
        {
            get 
            {
                int id;
                if (!string.IsNullOrEmpty(this.hdnDeducibleID.Value) && !string.IsNullOrWhiteSpace(this.hdnDeducibleID.Value))
                    if (Int32.TryParse(this.hdnDeducibleID.Value, out id))
                        return id;
                return null;
            }
            set 
            {
                if (value != null)
                    this.hdnDeducibleID.Value = value.ToString();
                else
                    this.hdnDeducibleID.Value = string.Empty;
            }
        }

        public string Concepto 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtConcepto.Text) && !string.IsNullOrWhiteSpace(this.txtConcepto.Text))
                    return this.txtConcepto.Text.Trim().ToUpper();
                return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtConcepto.Text = value;
                else
                    this.txtConcepto.Text = string.Empty;
            } 
        }

        public decimal? Porcentaje 
        { 
            get
            {
                decimal porcentaje;
                if (!string.IsNullOrEmpty(this.txtPorcentaje.Text) && !string.IsNullOrWhiteSpace(this.txtPorcentaje.Text))
                    if (Decimal.TryParse(this.txtPorcentaje.Text.Trim().Replace(",",""), out porcentaje)) //RI0012
                        return porcentaje;
                return null;
            }
            set
            {
				if (value != null)
					this.txtPorcentaje.Text = string.Format("{0:#,##0.00}", value);//RI0012
				else
					this.txtPorcentaje.Text = string.Empty;
            }
        }

        public int IndicePaginaResultado
        {
            get { return this.grdDeducibles.PageIndex; }
            set { this.grdDeducibles.PageIndex = value; }
        }

        public List<DeducibleBO> DeduciblesBorrados
        {
            get
            {
                if ((List<DeducibleBO>)this.Session["DeduciblesBorrados"] == null)
                    return new List<DeducibleBO>();
                else
                    return (List<DeducibleBO>)this.Session["DeduciblesBorrados"];
            }
            set { this.Session["DeduciblesBorrados"] = value; }
        }

        public List<DeducibleBO> Deducibles
        {
            get
            {
                if ((List<DeducibleBO>)this.Session["Deducibles"] == null)
                    return new List<DeducibleBO>();
                else
                    return (List<DeducibleBO>)this.Session["Deducibles"];
            }
            set{ this.Session["Deducibles"] = value; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucDeducibleSeguroPRE(this);
        }
        #endregion

        #region Métodos
        #region SC0004
        public void DeshabilitarControles()
        {
            this.txtConcepto.Enabled = false;
            this.txtPorcentaje.Enabled = false;
            this.btnAgregar.Enabled = false;
            this.grdDeducibles.Enabled = false;
        }
        public void HabilitarControles()
        {
            this.txtConcepto.Enabled = true;
            this.txtPorcentaje.Enabled = true;
            this.btnAgregar.Enabled = true;
            this.grdDeducibles.Enabled = true;
        }
        #endregion
        public void  PrepararNuevo()
        {
 	        this.txtConcepto.Text = string.Empty;
            this.txtPorcentaje.Text = string.Empty;            
            this.Deducibles = new List<DeducibleBO>();
            this.grdDeducibles.DataSource = null;
            this.grdDeducibles.DataBind();

            this.txtConcepto.Visible = true;
            this.txtPorcentaje.Visible = true;
            this.txtConcepto.Enabled = true;
            this.txtPorcentaje.Enabled = true;
        }
        public void  PrepararEdicion()
        {
 	        this.txtConcepto.Text = string.Empty;
            this.txtPorcentaje.Text = string.Empty;            
            this.txtConcepto.Enabled = true;
            this.txtPorcentaje.Enabled = true;
            this.txtConcepto.Visible = true;
            this.txtPorcentaje.Visible = true;
        }
        public void  PrepararVista()
        {
 	        this.txtConcepto.Text = string.Empty;
            this.txtPorcentaje.Text = string.Empty;

            this.txtConcepto.Enabled = false;
            this.txtPorcentaje.Enabled = false;

            this.txtConcepto.Visible = false;
            this.txtPorcentaje.Visible = false;
        }
        public void ActualizarLista()
        {
            this.grdDeducibles.DataSource = this.Deducibles;
            this.grdDeducibles.DataBind();
        }
        public void LimpiarCampos()
        {
            this.txtConcepto.Text = string.Empty;
            this.txtPorcentaje.Text = string.Empty;
        }
        public void LimpiarSesion()
        {
            if ((List<DeducibleBO>)this.Session["Deducibles"] != null)
                Session.Remove("Deducibles");
            if ((List<DeducibleBO>)this.Session["DeduciblesBorrados"] != null)
                Session.Remove("DeduciblesBorrados");
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

        protected void grdDeducibles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;

                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        DeducibleBO deducible = this.Deducibles[index];
                        presentador.RemoverElemento(index);
                        break;
                }
                
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdDeducibles_RowCommand: " + ex.Message);
            }
        }

        protected void grdDeducibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grdDeducibles_PageIndexChanging:" + ex.Message);
            }
        }
        #endregion
    }
}