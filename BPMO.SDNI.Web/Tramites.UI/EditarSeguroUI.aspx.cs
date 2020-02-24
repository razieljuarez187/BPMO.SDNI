//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;
using BPMO.SDNI.Tramites.BO;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class EditarSeguroUI : System.Web.UI.Page, IEditarSeguroVIS
    {
        #region Atributos
        private EditarSeguroPRE presentador;
        private string nombreClase = "EditarSeguroUI";
        const string UrlDetalle = "DetalleSeguroUI.aspx";
        #endregion

        #region Propiedades
        public string NumeroPoliza
        {
            get
            {
                TextBox txtNumeroPoliza = this.mnuSeguro.Controls[1].FindControl("txtValue") as TextBox;

                if (!string.IsNullOrEmpty(txtNumeroPoliza.Text) && !string.IsNullOrWhiteSpace(txtNumeroPoliza.Text))
                    return txtNumeroPoliza.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                TextBox txtNumeroPoliza = this.mnuSeguro.Controls[1].FindControl("txtValue") as TextBox;
                if (value != null)
                    txtNumeroPoliza.Text = value;
                else
                    txtNumeroPoliza.Text = string.Empty;
            }
        }
        public string DescripcionTramitable
        {
            get
            {
                TextBox txtDescripcionTramitable = this.mnuSeguro.Controls[0].FindControl("txtValue") as TextBox;

                if (!string.IsNullOrEmpty(txtDescripcionTramitable.Text) && !string.IsNullOrWhiteSpace(txtDescripcionTramitable.Text))
                    return txtDescripcionTramitable.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                TextBox txtDescripcionTramitable = this.mnuSeguro.Controls[0].FindControl("txtValue") as TextBox;
                if (value != null)
                    txtDescripcionTramitable.Text = value;
                else
                    txtDescripcionTramitable.Text = string.Empty;
            }
        }

        #region SC_0008
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? UsuarioId
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
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                this.presentador = new EditarSeguroPRE(this, this.ucucSeguroUI);
                //Se valida el acceso a la vista
                this.presentador.ValidarAcceso();
                if (!this.IsPostBack)
                {
                    this.presentador.CargaInicial();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public object ObtenerDatosNavegacion()
        {
            return (SeguroBO)Session["EDITARSEGURO"];
        }
        public void EstablecerPaqueteNavegacion(string Clave, int? tramiteID)
        {
            SeguroBO seguro = (SeguroBO)this.presentador.InterfazUsuarioADato();

            if (seguro != null) Session[Clave] = seguro;
            else
            {
                throw new Exception(this.nombreClase + ".EstablecerPaqueteNavegacion: El seguro proporcionado no puede ser desplegado.");
            }
        }

        public void IrADetalle()
        {
            Response.Redirect(UrlDetalle);
        }

        public void LimpiarSesion()
        {
            if (Session["LastSeguro"] != null)
                Session.Remove("LastSeguro");
            if (Session["TramiteSeguro"] != null)
                Session.Remove("TramiteSeguro");
        }
        public void LimpiarSesionEditar()
        {
            if (Session["EDITARSEGURO"] != null)
                Session.Remove("EDITARSEGURO");
            if (Session["REGISTRARSEGURO"] != null)
                Session.Remove("REGISTRARSEGURO");
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

        #region SC0008

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion
        #endregion

        #region Eventos
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try 
            {
                this.presentador.Editar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el seguro", ETipoMensajeIU.ERROR, this.nombreClase + ".cmdGuardar_Click:" + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try 
            {
                SeguroBO seguro = (SeguroBO)this.presentador.InterfazUsuarioADato();
                if(seguro != null)
                    if(seguro.TramiteID.HasValue)
                        this.presentador.IrADetalle(seguro.TramiteID.Value);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el seguro", ETipoMensajeIU.ERROR, this.nombreClase + ".cmdCancelar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}