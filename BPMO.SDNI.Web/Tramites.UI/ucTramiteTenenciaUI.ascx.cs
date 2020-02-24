//Satisface al CU087 – Catálogo Tramite Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.VIS;
using BPMO .SDNI.Tramites.PRE;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Tramites.UI
{
    
    public partial class ucTramiteTenenciaUI : System.Web.UI.UserControl,IucTramiteTenenciaVIS
    {
        #region Atributos
        private ucTramiteTenenciaPRE presentador;
        private string nombreClase = "ucTramiteTenenciaUI";
        #endregion

        #region Propiedades
        public int? UC
        {
            get 
            {
                int? id = null;
                Site master = (Site)Page.Master;
                    if(master.Usuario!=null)
                        id=master.Usuario.Id;
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
                if (ucCatalogoDocumentosTenencia.ObtenerArchivos() != null)
                    return ucCatalogoDocumentosTenencia.ObtenerArchivos();
                else
                    return new List<ArchivoBO>();
            }
            set { ucCatalogoDocumentosTenencia.EstablecerListasArchivos(value,null); }
        }
       
		public Decimal? Importe
        {
            get {

                Decimal? importe = null;
                if (!String.IsNullOrEmpty(txtImporte.Text.Trim()))
                {
                    try
                    {
                        importe = Convert.ToDecimal(txtImporte.Text.Trim().Replace(",","")); //RI0012
                    }
                    catch (Exception ex)
                    {
                        this.MostrarMensaje("Verifique que el valor de importe sea un número", ETipoMensajeIU.INFORMACION);
                    }
                }
                return importe;
            }
            set
            {
				this.txtImporte.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty;  //RI0012
            }
        }
        public string Folio
        {
            get { return (String.IsNullOrEmpty(txtFolio.Text.Trim())) ? null : txtFolio.Text.Trim().ToUpper(); }
            set { this.txtFolio.Text = value != null ? value : string.Empty; }
        }
        public DateTime? FechaPago
        {
            get {
                DateTime? fecha = null;
                if (!String.IsNullOrEmpty(txtFechaPago.Text.Trim()))
                    fecha = DateTime.Parse(txtFechaPago.Text.Trim());
                return fecha;
            }
            set
            {
                this.txtFechaPago.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
            
        }
        public TenenciaBO UltimoObjetoTenencia
        {
            get {
                if ((TenenciaBO)Session["UltimaTenencia"] == null)
                    return new TenenciaBO();
                else
                    return (TenenciaBO)Session["UltimaTenencia"];
            }
            set
            {
                Session["UltimaTenencia"] = value;
            }
        }
        public List<TipoArchivoBO> TiposArchivo
        {
            get { return ucCatalogoDocumentosTenencia.TiposArchivo; }
            set { ucCatalogoDocumentosTenencia.TiposArchivo = value;}
        }
        public IucCatalogoDocumentosVIS VistaDocumentos
        {
            get { return this.ucCatalogoDocumentosTenencia; }
        }

        public ucTramiteTenenciaPRE Presentador
        {
            get { return presentador; }
            set { presentador = value; }
        }
        #endregion

        #region Métodos
        
        public void ModoEdicion(bool habilitar)
        {
            this.txtFechaPago.Enabled = habilitar;
            this.txtFolio.Enabled = habilitar;
            this.txtImporte.Enabled = habilitar;
            ucCatalogoDocumentosTenencia.EstablecerModoEdicion(habilitar);
        }
        public void MostrarMensaje(string mensaje,ETipoMensajeIU tipo, string detalle=null)
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
            if (Session["UltimaTenencia"] != null)
                Session.Remove("UltimaTenencia");
        }
        public void EstablecerIdentificadorListaArchivos(string identificador)
        {
            this.ucCatalogoDocumentosTenencia.Identificador = identificador;
        }
        public void EstablecerTipoAdjunto(ETipoAdjunto tipo)
        {
            ucCatalogoDocumentosTenencia.TipoAdjunto = tipo;
        }
        #endregion
        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTramiteTenenciaPRE(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al intentar Presentar la informacion", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion
    }
}