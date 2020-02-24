//Satisface al CU075 - Catálogo de Equipo Aliado
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class EliminarEquipoAliadoUI : System.Web.UI.Page, IEliminarEquipoAliadoVIS
    {
        #region Atributos
        private EliminarEquipoAliadoPRE presentador;
        private const string nombreClase = "DetalleEquipoAliadoUI";
        #endregion

        #region Propiedades
        public int? EquipoAliadoID
        {
            get
            {
                TextBox txtCodigo = this.mnuEquipoAliado.Controls[0].FindControl("txtValue") as TextBox;
                int val = 0;
                if (!string.IsNullOrEmpty(txtCodigo.Text) && !string.IsNullOrWhiteSpace(txtCodigo.Text))
                    if (Int32.TryParse(txtCodigo.Text, out val))
                        return val;
                return null;
            }
            set
            {
                TextBox txtCodigo = this.mnuEquipoAliado.Controls[0].FindControl("txtValue") as TextBox;
                if (value != null)
                    txtCodigo.Text = value.Value.ToString();
                else
                    txtCodigo.Text = string.Empty;
            }
        }

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
                this.presentador = new EliminarEquipoAliadoPRE(this, this.ucucEquipoAliadoUI);
                if (!this.IsPostBack)
                {
					//Se valida el acceso a la vista
					this.presentador.ValidarAcceso(); //SC_0008
                    this.presentador.CargaInicial();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararVista()
        {
            TextBox txtCodigo = this.mnuEquipoAliado.Controls[0].FindControl("txtValue") as TextBox;
            txtCodigo.Text = string.Empty;
        }

        public object ObtenerDatosNavegacion()
        {
            return Session["EquipoAliadoBODetalle"];
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

		//SC_0008
		public void RedirigirSinPermisoAcceso()
		{
			this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
		}
        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarBaja();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Eliminar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar Eliminar el Equipo Aliado", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}