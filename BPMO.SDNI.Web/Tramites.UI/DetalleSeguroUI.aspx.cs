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
    public partial class DetalleSeguroUI : System.Web.UI.Page, IDetalleSeguroVIS
    {
        #region Atributos
        private DetalleSeguroPRE presentador;
        private const string nombreClase = "DetalleSeguroUI";
        #endregion        

        #region Propiedades
        public int? TramiteID
        {
            get
            {
                if (((SeguroBO)Session[this.Codigo]).TramiteID.HasValue)
                    return ((SeguroBO)Session[this.Codigo]).TramiteID.Value;
                return null;
            }
        }

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

        public string Codigo
        {
            get
            {
                return "TRAMITESEGURO";
            }
        }

        public SeguroBO Seguro
        {
            get
            {
                if (Session[this.Codigo + "Detalle"] is SeguroBO)
                    return Session[Codigo + "Detalle"] as SeguroBO;

                return null;
            }
            set
            {
                Session[this.Codigo + "Detalle"] = value;
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
                presentador = new DetalleSeguroPRE(this, this.ucSeguroViewUI1);

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        public object ObtenerDatosNavegacion()
        {
            if (((SeguroBO)Session[this.Codigo]) != null)
                return ((SeguroBO)Session[this.Codigo]);
            return null;
        }

        public void EstablecerPaqueteNavegacion(string Clave)
        {
            SeguroBO seguro = (SeguroBO)this.presentador.InterfazUsuarioADato();
            if (seguro != null) Session[Clave] = seguro;
            else
            {
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El seguro proporcionado no pertence al listado de seguros registrados.");
            }
            Response.Redirect("EditarSeguroUI.aspx");
        }

        public void LimpiarSesion()
        {
            string key = this.Codigo + "Detalle";
            if(this.Session[key] != null)
                Session.Remove(key);
            if (Session["TramiteSeguro"] != null)
                Session.Remove("TramiteSeguro");
        }

        #region SC0008
        public void PermitirEditar(bool permitir)
        {
            this.mnuSeguro.Items[2].Enabled = permitir;
            this.btnEditar.Enabled = permitir;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try 
            {
                presentador.IrAEdicion();
            }
            catch(Exception ex) 
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".cmdEditar_Click: " + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsultarSeguroUI.aspx", false);
        }

        protected void mnuSeguro_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                switch (e.Item.Value)
                {
                    case "Editar":
                        presentador.IrAEdicion();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar dar de baja el equipo aliado", ETipoMensajeIU.ERROR, nombreClase + ".mnuBajaEquipoAliado_MenuItemClick:" + ex.Message);
            }
        }
        #endregion
    }
}