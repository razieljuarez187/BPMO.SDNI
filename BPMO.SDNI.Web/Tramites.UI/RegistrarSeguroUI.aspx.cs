//Esta clase satisface al CU085 - CATALOGO INFORMACION SEGURO DE LA UNIDAD
using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Tramites.PRE;
using BPMO.SDNI.Tramites.VIS;

namespace BPMO.SDNI.Tramites.UI
{
    public partial class RegistrarSeguroUI : System.Web.UI.Page, IRegistrarSeguroVIS
    {
        #region Atributos
        private RegistrarSeguroPRE presentador;
        private string nombreClase = "RegistrarSeguroUI";
        #endregion

        #region Propiedades
        public string VIN
        {
            get
            {
                if(this.Session["VINSEGURO"] != null)
                    return ((string)this.Session["VINSEGURO"]).ToUpper();
                else
                {
                    this.Session["VINSEGURO"] = string.Empty;
                    return (string)this.Session["VINSEGURO"];
                }
            }
            set
            {
                if (value != null)
                {
                    this.Session["VINSEGURO"] = value;
                    this.hdnVIN.Value = (string)this.Session["VINSEGURO"];
                }
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
        #endregion
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarSeguroPRE(this, this.ucucSeguroUI);
                //SC0008
                this.presentador.ValidarAcceso();

                if (!this.IsPostBack)
                    this.presentador.PrepararNuevo();
            
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void LimpiarSesion()
        {

        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
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
                this.presentador.Registrar();                
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el seguro", ETipoMensajeIU.ERROR, this.nombreClase + ".cmdGuardar_Click:" + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsultarSeguroUI.aspx", false);
        }
        #endregion
    }
}