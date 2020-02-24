//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.PRE;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucTramiteFiltroAKUI : System.Web.UI.UserControl,IucTramiteFiltroAKVIS
    {
        #region Atributos
        private string nombreClase="ucTramiteFiltroAKUI";
        private ucTramiteFiltroAKPRE presentador;
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
        public FiltroAKBO UltimoObjetoFiltroAK
        {
            get
            {
                if ((FiltroAKBO)Session["UltimoFiltro"] == null)
                    return new FiltroAKBO();
                else
                    return (FiltroAKBO)Session["UltimoFiltro"];
            }
            set
            {
                Session["UltimoFiltro"] = value;
            }
        }
        public string NumeroSerie
        {
            get
            {
	            return (String.IsNullOrEmpty(this.txtNumeroSerie.Text.Trim())) ? null : this.txtNumeroSerie.Text.Trim().ToUpper();
            }
            set
            {
	            this.txtNumeroSerie.Text = value ?? string.Empty;
            }
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

        public ucTramiteFiltroAKPRE Presentador
        {
            get { return presentador; }
            set { presentador = value; }
        }
        #endregion

        #region Métodos
        public void ModoEdicion(bool habilitar)
        {
            this.txtFechaInstalacion.Enabled = habilitar;
            this.txtNumeroSerie.Enabled = habilitar;
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
            if (Session["UltimoFiltro"] != null)
                Session.Remove("UltimoFiltro");
        }
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTramiteFiltroAKPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al presentar la informacion.",ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion
    }
}