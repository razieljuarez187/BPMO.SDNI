//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucTramiteGPSUI : System.Web.UI.UserControl,IucTramiteGPSVIS
    {
        #region Atributos
        private string nombreClase = "ucTramiteGPSUI";
        private ucTramiteGPSPRE presentador;
        #endregion

        #region Propiedades
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
        public GPSBO UltimoObjetoGPS
        {
            get
            {
                if ((GPSBO)Session["UltimoGPS"] == null)
                    return new GPSBO();
                else
                    return (GPSBO)Session["UltimoGPS"];
            }
            set
            {
                Session["UltimoGPS"] = value;
            }
        }
        public string Compania
        {
            get { return (String.IsNullOrEmpty(this.txtCompania.Text.Trim())) ? null : this.txtCompania.Text.Trim().ToUpper(); }
            set { this.txtCompania.Text = value != null ? value : string.Empty; }
        }
        public DateTime? FechaInstalacion
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaInstalacion.Text.Trim()))
                    fecha = DateTime.Parse(this.txtFechaInstalacion.Text.Trim());
                return fecha;
            }
            set
            {
                this.txtFechaInstalacion.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }

        }
        public string Numero
        {
            get { return (String.IsNullOrEmpty(this.txtNumeroID.Text.Trim())) ? null : this.txtNumeroID.Text.Trim().ToUpper(); }
            set { this.txtNumeroID.Text = value != null ? value : string.Empty; }
        }

        public ucTramiteGPSPRE Presentador
        {
            get { return presentador; }
            set { presentador = value; }
        }
        #endregion

        #region Métodos
        public void ModoEdicion(bool habilitar)
        {
            this.txtFechaInstalacion.Enabled = habilitar;
            this.txtNumeroID.Enabled = habilitar;
            this.txtCompania.Enabled = habilitar;
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
            if (Session["UltimoGPS"] != null)
                Session.Remove("UltimoGPS");
        }
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTramiteGPSPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al presentar la informacion.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion
    }
}