//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.PRE;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucTramitePlacaUI : System.Web.UI.UserControl, IucTramitePlacaVIS
    {
        #region Atributos
        private string nombreClase = "ucTramitePlacaUI";
        private ucTramitePlacaPRE presentador;
        #endregion
        #region Propiedades
        public ETipoTramite? tipo
        {
            get
            {
                ETipoTramite? tipo = null;
                if (!String.IsNullOrEmpty(this.hdnTipoPlaca.Value.Trim()))
                {
                    tipo = (ETipoTramite)Convert.ToInt16(this.hdnTipoPlaca.Value.Trim());
                }
                return tipo;
            }
            set
            {

                if (value != null)
                {
                    hdnTipoPlaca.Value = ((int)value).ToString();
                }
                else
                    hdnTipoPlaca.Value = string.Empty;
            }
        }
        public int? UC
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master.Usuario != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
        public int? UUA
        {
            get { return this.UC; }
        }
        public DateTime? FC
        {
            get { return DateTime.Today; }
        }
        public DateTime? FUA
        {
            get
            {
                return this.FC;
            }
        }
        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimaPlaca" + this.tipo] == null)
                    return null;
                else
                    return (object)Session["UltimaPlaca" + this.tipo];
            }
            set
            {
                Session["UltimaPlaca" + this.tipo] = value;
                if (value != null)
                    this.trActivo.Visible = true;
            }
        }
        public string Numero
        {
            get
            {
                string numero = null;
                if (tipo == ETipoTramite.PLACA_ESTATAL)
                {
                    if (!String.IsNullOrEmpty(this.txtNumeroPlacaEstatal.Text.Trim()))
                        numero = this.txtNumeroPlacaEstatal.Text.Trim().ToUpper();
                }
                if (tipo == ETipoTramite.PLACA_FEDERAL)
                {
                    if (!String.IsNullOrEmpty(this.txtNumeroPlacaFederal.Text.Trim()))
                        numero = this.txtNumeroPlacaFederal.Text.Trim().ToUpper();
                }
                return numero;
            }
            set
            {
                if (tipo == ETipoTramite.PLACA_FEDERAL)
                {
                    this.txtNumeroPlacaFederal.Text = value != null ? value : string.Empty;
                }
                if (tipo == ETipoTramite.PLACA_ESTATAL)
                {
                    this.txtNumeroPlacaEstatal.Text = value != null ? value : string.Empty;
                }
            }
        }
        public string NumeroGuia
        {
            get { return (String.IsNullOrEmpty(this.txtNumeroGuia.Text.Trim())) ? null : this.txtNumeroGuia.Text.Trim().ToUpper(); }
            set { this.txtNumeroGuia.Text = value != null ? value : string.Empty; }
        }
        public DateTime? FechaEnvio
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaEnvio.Text.Trim()))
                    fecha = DateTime.Parse(this.txtFechaEnvio.Text.Trim());
                return fecha;
            }
            set
            {
                this.txtFechaEnvio.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }

        }
        public DateTime? FechaRecepcion
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaRecepcion.Text.Trim()))
                    fecha = DateTime.Parse(this.txtFechaRecepcion.Text.Trim());
                return fecha;
            }
            set
            {
                this.txtFechaRecepcion.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }

        }

        public ucTramitePlacaPRE Presentador
        {
            get { return presentador; }
            set { presentador = value; }
        }
        public bool? Activo
        {
            get
            {
                return this.chkActivo.Checked;
            }
            set
            {
                if (value.HasValue)
                    this.chkActivo.Checked = value.Value;
                else
                    this.chkActivo.Checked = false;
            }
        }
        #endregion

        #region Métodos
        public void ModoEdicion(bool habilitar)
        {
            this.txtFechaEnvio.Enabled = habilitar;
            this.txtFechaRecepcion.Enabled = habilitar;
            this.txtNumeroPlacaEstatal.Enabled = habilitar;
            this.txtNumeroPlacaFederal.Enabled = habilitar;
            this.txtNumeroGuia.Enabled = habilitar;
            this.chkActivo.Enabled = habilitar;
        }
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            }
            else
            {
                Site master = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    master.MostrarMensaje(mensaje, tipo, detalle);
                else
                    master.MostrarMensaje(mensaje, tipo);
            }

        }
        public void LimpiarSesion()
        {
            if (Session["UltimaPlaca" + this.tipo] != null)
                Session.Remove("UltimaPlaca" + this.tipo);
            this.trActivo.Visible = false;
        }
        public void PlacaEstatal()
        {
            this.divPlacaFederal.Visible = false;
            this.divPlacaEstatal.Visible = true;
        }
        public void PlacaFederal()
        {
            this.divPlacaFederal.Visible = true;
            this.divPlacaEstatal.Visible = false;
        }
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTramitePlacaPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Incosistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

    }
}