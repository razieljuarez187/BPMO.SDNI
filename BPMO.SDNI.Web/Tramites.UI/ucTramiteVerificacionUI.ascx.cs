//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Comun.VIS;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class ucTramiteVerificacionUI : System.Web.UI.UserControl,IucTramiteVerificacionVIS
    {
        #region Atributos
        private ucTramiteVerificacionPRE presentador;
        private string nombreClase = "ucTramiteVarificacionUI";
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
        public List<ArchivoBO> Archivos
        {
            get
            {
                if (ucCatalogoDocumentosVerificacion.ObtenerArchivos() != null)
                    return ucCatalogoDocumentosVerificacion.ObtenerArchivos();
                else
                    return new List<ArchivoBO>();
            }
            set { ucCatalogoDocumentosVerificacion.EstablecerListasArchivos(value, null); }
        }
        public string Folio
        {
            get
            {
                return (String.IsNullOrEmpty(txtFolio.Text.Trim())) ? null : this.txtFolio.Text.Trim().ToUpper();
            }
            set
            {
                this.txtFolio.Text = value != null ? value : string.Empty;
            }
        }
        public DateTime? FechaInicio
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(txtFechaInicio.Text.Trim()))
                    fecha = DateTime.Parse(this.txtFechaInicio.Text.Trim());
                return fecha;
            }
            set
            {
                this.txtFechaInicio.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public DateTime? FechaFinal
        {
            get
            {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(this.txtFechaFinal.Text.Trim()))
                    fecha = DateTime.Parse(this.txtFechaFinal.Text.Trim());
                return fecha;
            }
            set
            {
                this.txtFechaFinal.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public ETipoVerificacion? TipoTramite
        {
            get
            {
                ETipoVerificacion? tipo = null;
                if (!String.IsNullOrEmpty(this.hdnTipoVerificacion.Value.Trim()))
                    tipo = (ETipoVerificacion)Convert.ToInt16(this.hdnTipoVerificacion.Value.Trim());

                return tipo;

            }
            set
            {
                if (value == null)
                    this.hdnTipoVerificacion.Value = string.Empty;
                else
                {
                    this.hdnTipoVerificacion.Value = ((int)value).ToString();
                }
            }
        }
        public VerificacionBO UltimoObjetoVerificacion
        {
            get
            {
                if ((VerificacionBO)Session["UltimaVerificacion"+this.TipoTramite] == null)
                    return new VerificacionBO();
                else
                    return (VerificacionBO)Session["UltimaVerificacion"+this.TipoTramite];
            }
            set
            {
                Session["UltimaVerificacion"+TipoTramite] = value;
            }
        }
        public List<TipoArchivoBO> TiposArchivo
        {
            get { return ucCatalogoDocumentosVerificacion.TiposArchivo; }
            set { ucCatalogoDocumentosVerificacion.TiposArchivo = value; }
        }
        public IucCatalogoDocumentosVIS VistaDocumentos
        {
            get { return this.ucCatalogoDocumentosVerificacion; }
        }

        public ucTramiteVerificacionPRE Presentador
        {
            get { return presentador; }
            set { presentador = value; }
        }
        #endregion

        #region Métodos
        public void ModoEdicion(bool habilitar)
        {
            this.txtFechaInicio.Enabled = habilitar;
            this.txtFolio.Enabled = habilitar;
            this.txtFechaFinal.Enabled = habilitar;
            ucCatalogoDocumentosVerificacion.EstablecerModoEdicion(habilitar);
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
            if (Session["UltimaVerificacion"+this.TipoTramite] != null)
                Session.Remove("UltimaVerificacion"+this.TipoTramite);
        }
        public void EstablecerIdentificadorListaArchivos(string identificador)
        {
            this.ucCatalogoDocumentosVerificacion.Identificador = identificador;
        }
        public void EstablecerTipoAdjunto(ETipoAdjunto tipo)
        {
            ucCatalogoDocumentosVerificacion.TipoAdjunto = tipo;
        }
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTramiteVerificacionPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al presentar la información. ", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion
    }
}