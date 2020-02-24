//Satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucSeguroDetalleUI : System.Web.UI.UserControl, IucSeguroDetalleVIS
    {
        #region Atributos
        private ucSeguroDetallePRE presentador;
        private string nombreClase = "ucSeguroViewUI";
        private bool? activo;
        #endregion

        #region Propiedades
        public string VIN
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtVIN.Text) && !string.IsNullOrWhiteSpace(this.txtVIN.Text))
                    return this.txtVIN.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtVIN.Text = value.ToString();
                else
                    this.txtVIN.Text = string.Empty;
            }
        }
        public string Modelo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text))
                    return this.txtModelo.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }
        public string NumeroPoliza
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumPoliza.Text) && !string.IsNullOrWhiteSpace(this.txtNumPoliza.Text))
                    return this.txtNumPoliza.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtNumPoliza.Text = value;
                else
                    this.txtNumPoliza.Text = string.Empty;
            }
        }
        public string Aseguradora
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtAseguradora.Text) && !string.IsNullOrWhiteSpace(this.txtAseguradora.Text))
                    return this.txtAseguradora.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtAseguradora.Text = value;
                else
                    this.txtAseguradora.Text = string.Empty;
            }
        }
        public string Contacto
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtContacto.Text) && !string.IsNullOrWhiteSpace(this.txtContacto.Text))
                    return this.txtContacto.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtContacto.Text = value;
                else
                    this.txtContacto.Text = string.Empty;
            }
        }
        public decimal? PrimaAnual
        {
            get
            {
                decimal prima;
                if (!string.IsNullOrEmpty(this.txtPrimaAnual.Text) && !string.IsNullOrWhiteSpace(this.txtPrimaAnual.Text))
                    if (Decimal.TryParse(this.txtPrimaAnual.Text.Trim().Replace(",",""), out prima)) //RI0012
                        return prima;

                return null;
            }
            set
            {
                if (value != null)
                    this.txtPrimaAnual.Text = string.Format("{0:#,##0.0000}",value); //RI0012
                else
                    this.txtPrimaAnual.Text = string.Empty;
            }
        }
        public decimal? PrimaSemestral
        {
            get
            {
                decimal prima;
                if (!string.IsNullOrEmpty(this.txtPrimaSemestral.Text) && !string.IsNullOrWhiteSpace(this.txtPrimaSemestral.Text))
                    if (Decimal.TryParse(this.txtPrimaSemestral.Text.Trim().Replace(",",""), out prima)) //RI0012
                        return prima;

                return null;
            }
            set
            {
                if (value != null)
					this.txtPrimaSemestral.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtPrimaSemestral.Text = string.Empty;
            }
        }
        public DateTime? VigenciaInicial
        {
            get
            {
                DateTime date;
                if (!string.IsNullOrEmpty(this.txtVigenciaInicial.Text) && !string.IsNullOrWhiteSpace(this.txtVigenciaInicial.Text))
                    if (DateTime.TryParse(this.txtVigenciaInicial.Text.Trim().ToUpper(), out date))
                        return date;

                return null;
            }
            set
            {
                if (value != null)
                    this.txtVigenciaInicial.Text = value.Value.ToShortDateString();
                else
                    this.txtVigenciaInicial.Text = string.Empty;
            }
        }
        public DateTime? VigenciaFinal
        {
            get
            {
                DateTime date;
                if (!string.IsNullOrEmpty(this.txtVigenciaFinal.Text) && !string.IsNullOrWhiteSpace(this.txtVigenciaFinal.Text))
                    if (DateTime.TryParse(this.txtVigenciaFinal.Text, out date))
                        return date;

                return null;
            }
            set
            {
                if (value != null)
                    this.txtVigenciaFinal.Text = value.Value.ToShortDateString();
                else
                    this.txtVigenciaFinal.Text = string.Empty;
            }
        }
        public string Observaciones
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtObservacion.Text) && !string.IsNullOrWhiteSpace(this.txtObservacion.Text))
                    return this.txtObservacion.Text.Trim().ToUpper();
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    this.txtObservacion.Text = value;
                else
                    this.txtObservacion.Text = string.Empty;
            }
        }
        public bool? Activo
        {
            get
            {
                return this.activo;
            }
            set
            {
                this.activo = value;
            }
        }
        public DateTime? FC
        {
            get { return DateTime.Today; }
        }
        public DateTime? FUA
        {
            get { return this.FC; }
        }
        public int? UC
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        public int? UUA
        {
            get { return this.UC; }
        }
        public int? TramiteID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTramiteID.Value) && !string.IsNullOrWhiteSpace(this.hdnTramiteID.Value))
                    if (Int32.TryParse(this.hdnTramiteID.Value.Trim().ToUpper(), out val))
                        return val;

                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTramiteID.Value = value.ToString();
                else
                    this.hdnTramiteID.Value = string.Empty;
            }
        }
        public ETipoTramite? TipoTramite
        {
            get { return ETipoTramite.SEGURO; }
        }
        public int? TramitableID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTramitableID.Value) && !string.IsNullOrWhiteSpace(this.hdnTramitableID.Value))
                    if (Int32.TryParse(this.hdnTramitableID.Value.Trim().ToUpper(), out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTramitableID.Value = value.ToString();
                else
                    this.hdnTramitableID.Value = string.Empty;
            }
        }
        public ETipoTramitable? TipoTramitable
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTipoTramitable.Value) && !string.IsNullOrWhiteSpace(this.hdnTipoTramitable.Value))
                    if (Int32.TryParse(this.hdnTipoTramitable.Value.Trim().ToUpper(), out val))
                        return (ETipoTramitable)val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTipoTramitable.Value = value.ToString();
                else
                    this.hdnTipoTramitable.Value = string.Empty;
            }
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
            set { Session["Deducibles"] = value; }
        }
        public List<EndosoBO> Endosos
        {
            get
            {
                if (Session["Endosos"] == null)
                    return new List<EndosoBO>();
                return (List<EndosoBO>)this.Session["Endosos"];
            }
            set { Session["Endosos"] = value; }
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
            set { Session["Siniestros"] = value; }
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
					this.txtPrimaAnualTotal.Text = string.Format("{0:#,##0.0000}", value); //RI0012;
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
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucSeguroDetallePRE(this);
        }
        #endregion

        #region Métodos
        private void PrepararNuevo()
        {
            this.txtAseguradora.Text = string.Empty;
            this.txtContacto.Text = string.Empty;
            this.txtModelo.Text = string.Empty;
            this.txtNumPoliza.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.txtPrimaAnual.Text = string.Empty;
            this.txtPrimaSemestral.Text = string.Empty;
            this.txtVigenciaFinal.Text = string.Empty;
            this.txtVigenciaInicial.Text = string.Empty;
            this.txtPrimaAnualTotal.Text = string.Empty;
            this.txtSumaEndosos.Text = string.Empty;
            this.txtVIN.Text = string.Empty;
            this.grdDeducibles.DataSource = null;
            this.grdEndosos.DataSource = null;
            this.grdSiniestros.DataSource = null;
        }
        public void PrepararVista()
        {
            this.txtAseguradora.Enabled = false;
            this.txtContacto.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtNumPoliza.Enabled = false;
            this.txtObservacion.Enabled = false;
            this.txtPrimaAnual.Enabled = false;
            this.txtPrimaSemestral.Enabled = false;
            this.txtVigenciaFinal.Enabled = false;
            this.txtVigenciaInicial.Enabled = false;
            this.txtSumaEndosos.Enabled = false;
            this.txtPrimaAnualTotal.Enabled = false;
            this.txtVIN.Enabled = false;
            this.grdDeducibles.Enabled = false;            
            this.grdEndosos.Enabled = false;
            this.grdSiniestros.Enabled = false;
        }

        public void ActualizarLista()
        {
            this.grdDeducibles.DataSource = this.Deducibles;
            this.grdDeducibles.DataBind();
            this.grdEndosos.DataSource = this.Endosos;
            this.grdEndosos.DataBind();
            this.grdSiniestros.DataSource = this.Siniestros;
            this.grdSiniestros.DataBind();
        }

        public void DatoAInterfazUsuario(object obj)
        {
            if (this.presentador == null)
                this.presentador = new ucSeguroDetallePRE(this);

            this.presentador.DatoAInterfazUsuario(obj);
        }
        public object InterfazUsuarioADato()
        {
            return this.presentador.InterfazUsuarioADato();
        }

        public void LimpiarSesion()
        {
            if (Session["Deducibles"] != null)
                Session.Remove("Deducibles");

            if (Session["Endosos"] != null)
                Session.Remove("Endosos");

            if (Session["Siniestros"] != null)
                Session.Remove("Siniestros");
        }
        #endregion

        #region Eventos

        #endregion
    }
}