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
    public partial class ucEndosoSeguroUI : System.Web.UI.UserControl, IucEndosoSeguroVIS
    {
        #region Atributos
        private ucEndosoSeguroPRE presentador;
        private string nombreClase = "ucEndosoSeguroUI";
        #endregion 

        #region Propiedades
        public int? EndosoID
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrEmpty(this.hdnEndosoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEndosoID.Value))
                    if (Int32.TryParse(this.hdnEndosoID.Value, out id))
                        return id;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnEndosoID.Value = value.ToString();
                else
                    this.hdnEndosoID.Value = string.Empty;
            }
        }

        public string Motivo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtMotivo.Text) && !string.IsNullOrWhiteSpace(this.txtMotivo.Text))
                    return this.txtMotivo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtMotivo.Text = value;
                else
                    this.txtMotivo.Text = string.Empty;
            }
        }

        public decimal? Importe
        {
            get
            {
                decimal importe;
                if (!string.IsNullOrEmpty(this.txtImporte.Text) && !string.IsNullOrWhiteSpace(this.txtImporte.Text))
                    if(Decimal.TryParse(this.txtImporte.Text.Trim().Replace(",",""), out importe)) //RI0012
                        return importe;
                return null;
            }
            set
            {
                if (value != null)
					this.txtImporte.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtImporte.Text = string.Empty;
            }
        }

        public decimal? PrimaAnual
        {
            get
            {
                decimal prima;
                if (!string.IsNullOrEmpty(this.hdnPrimaAnual.Value) && !string.IsNullOrWhiteSpace(this.hdnPrimaAnual.Value))
                    if (Decimal.TryParse(this.hdnPrimaAnual.Value, out prima))
                        return prima;

                return null;
            }
            set
            {
                if (value != null)
                    this.hdnPrimaAnual.Value = value.Value.ToString();
                else
                    this.hdnPrimaAnual.Value = string.Empty;
            }
        }

        public List<EndosoBO> Endosos 
        {
            get 
            {
                if ((List<EndosoBO>)this.Session["Endosos"] == null)
                    return new List<EndosoBO>();
                else
                    return (List<EndosoBO>)this.Session["Endosos"];
            }
            set { this.Session["Endosos"] = value;  }
        }

        public List<EndosoBO> EndososBorrados
        {
            get
            {
                if ((List<EndosoBO>)this.Session["EndososBorrados"] == null)
                    return new List<EndosoBO>();
                else
                    return (List<EndosoBO>)this.Session["EndososBorrados"];
            }
            set { this.Session["EndososBorrados"] = value; }
        }

        public decimal? PrimaAnualTotal
        {
            get
            {
                decimal val = 0;
                if (!string.IsNullOrEmpty(this.txtPrimaAnualTotal.Text) && !string.IsNullOrWhiteSpace(this.txtPrimaAnualTotal.Text))
                    if (Decimal.TryParse(this.txtPrimaAnualTotal.Text.Trim().Replace(",",""), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
					this.txtPrimaAnualTotal.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtPrimaAnualTotal.Text = string.Empty;
            }
        }
        
        public decimal? TotalEndosos
        {
            get
            {
                decimal val = 0;
                if (!string.IsNullOrEmpty(this.txtSumaEndosos.Text) && !string.IsNullOrWhiteSpace(this.txtSumaEndosos.Text))
                    if (decimal.TryParse(this.txtSumaEndosos.Text.Trim().Replace(",",""), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
					this.txtSumaEndosos.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtSumaEndosos.Text = string.Empty;
            }
        }

        public int IndicePaginaResultado
        {
            get { return this.grdEndosos.PageIndex; }
            set { this.grdEndosos.PageIndex = value; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucEndosoSeguroPRE(this);
        }
        #endregion

        #region Métodos
        #region SC0004
        public void DeshabilitarControles()
        {
            this.txtImporte.Enabled = false;
            this.txtMotivo.Enabled = false;
            this.btnAgregar.Enabled = false;
            this.grdEndosos.Enabled = false;
        }
        public void HabilitarControles()
        {
            this.txtImporte.Enabled = true;
            this.txtMotivo.Enabled = true;
            this.btnAgregar.Enabled = true;
            this.grdEndosos.Enabled = true;
        }
        #endregion
        public void PrepararNuevo()
        {
            this.txtImporte.Text = string.Empty;
            this.txtMotivo.Text = string.Empty;
            this.hdnEndosoID.Value = string.Empty;
            this.hdnTramiteID.Value = string.Empty;
            this.Endosos = new List<EndosoBO>();
            this.grdEndosos.DataSource = null;
            this.grdEndosos.DataBind();

            this.txtImporte.Visible = true;
            this.txtMotivo.Visible = true;
            this.txtPrimaAnualTotal.Enabled = false;
            this.txtSumaEndosos.Enabled = false;
            this.txtImporte.Enabled = true;
            this.txtMotivo.Enabled = true;

            this.txtPrimaAnualTotal.Text = string.Empty;
            this.txtPrimaAnualTotal.Enabled = false;

            this.txtSumaEndosos.Text = string.Empty;
            this.txtSumaEndosos.Enabled = false;
        }
        public void PrepararEdicion()
        {
            this.txtImporte.Text = string.Empty;
            this.txtMotivo.Text = string.Empty;
            this.hdnEndosoID.Value = string.Empty;
            this.hdnTramiteID.Value = string.Empty;

            this.txtImporte.Visible = true;
            this.txtMotivo.Visible = true;

            this.txtImporte.Enabled = true;
            this.txtMotivo.Enabled = true;
        }
        public void PrepararVista()
        {
            this.txtImporte.Text = string.Empty;
            this.txtMotivo.Text = string.Empty;

            this.txtImporte.Enabled = false;
            this.txtMotivo.Enabled = false;
            this.txtPrimaAnualTotal.Enabled = false;
            this.txtSumaEndosos.Enabled = false;
            this.txtImporte.Visible = false;
            this.txtMotivo.Visible = false;
        }
        public void ActualizarLista()
        {
            this.grdEndosos.DataSource = this.Endosos;
            this.grdEndosos.DataBind();
        }
        /// <summary>
        /// Habilita y deshabilita la visibilidad de los controles que muestran el total de importes para endosos
        /// </summary>
        private void MostrarSumas()
        {
            if (this.Endosos != null)
            {
                if (this.Endosos.Count > 0)
                    this.tbSumatoriasEndosos.Visible = true;
                else
                    this.tbSumatoriasEndosos.Visible = false;
            }
            else
                this.tbSumatoriasEndosos.Visible = false;
        }

        public void LimpiarSesion()
        {
            if ((List<EndosoBO>)this.Session["Endosos"] != null)
                Session.Remove("Endosos");
            if ((List<EndosoBO>)this.Session["EndososBorrados"] != null)
                Session.Remove("EndososBorrados");
        }

        public void LimpiarCampos()
        {
            this.txtImporte.Text = string.Empty;
            this.txtMotivo.Text = string.Empty;
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
                this.presentador.AgregarItem();
                this.MostrarSumas();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al agregar el deducible, veririque por favor", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }        

        protected void grdEndosos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.nombreClase + ".grdEndosos_PageIndexChanging:" + ex.Message);
            }
        }

        protected void grdEndosos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;

                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.ToString())
                {
                    case "Eliminar":
                        EndosoBO deducible = this.Endosos[index];
                        this.presentador.RemoverElemento(index);
                        this.MostrarSumas();
                        break;
                }

            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdEndosos_RowCommand: " + ex.Message);
            }
        }
        #endregion
    }
}