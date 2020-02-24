//Satisface al CU075 - Catálogo de Equipo Aliado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class RegistrarEquipoAliadoUI : System.Web.UI.Page, IRegistrarEquipoAliadoVIS
    {
        #region Atributos
        private RegistrarEquipoAliadoPRE presentador;
        private const string nombreClase = "RegistrarEquipoAliadoUI"; 
        #endregion        

        #region Propiedades
        /// <summary>
        /// Contiene la lista de acciones a las cuales tiene acceso el usuario.
        /// </summary>
        public List<CatalogoBaseBO> ListaAcciones { get; set; }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new RegistrarEquipoAliadoPRE(this, this.ucucEquipoAliadoUI);
                if (!this.IsPostBack)
                    presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        
		#region Métodos

		#region SC008
		public void RedirigirSinPermisoAcceso()
		{
			this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
		}
		#endregion

        public void LimpiarSesion()
        {
            if (Session["EquipoAliadoBODetalle"] != null)
                this.Session.Remove("EquipoAliadoBODetalle");
            if (Session["EquipoAliadoEditar"] != null)
                this.Session.Remove("EquipoAliadoEditar");
            if (Session["LastEquipoAliado"] != null)
                this.Session.Remove("LastEquipoAliado");
            if (Session["listEquiposAliados"] != null)
                Session.Remove("listEquiposAliados");
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
                this.MostrarMensaje("Inconsistencia al guardar el equipo Aliado", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        #endregion

	
	}
}