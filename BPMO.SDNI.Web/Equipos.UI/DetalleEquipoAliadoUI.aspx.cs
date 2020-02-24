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
    public partial class DetalleEquipoAliadoUI : System.Web.UI.Page, IDetalleEquipoAliadoVIS
    {
        #region Atributos
        private DetalleEquipoAliadoPRE presentador;
        private const string nombreClase = "DetalleEquipoAliadoUI";
        #endregion

        #region Propiedades
        public int? EquipoAliadoID
        {
            get
            {
                TextBox txtCodigo = this.mnuBajaEquipoAliado.Controls[0].FindControl("txtValue") as TextBox;
                int val = 0;
                if (!string.IsNullOrEmpty(txtCodigo.Text) && !string.IsNullOrWhiteSpace(txtCodigo.Text))
                    if (Int32.TryParse(txtCodigo.Text, out val))
                        return val;
                return null;
            }
            set
            {
                TextBox txtCodigo = this.mnuBajaEquipoAliado.Controls[0].FindControl("txtValue") as TextBox;
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
                presentador = new DetalleEquipoAliadoPRE(this, this.ucucEquipoAliadoUI);
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
            TextBox txtCodigo = this.mnuBajaEquipoAliado.Controls[0].FindControl("txtValue") as TextBox;
            txtCodigo.Text = string.Empty;
        }

        public object ObtenerDatosNavegacion()
        {
            return Session["EquipoAliadoBODetalle"];
        }

		#region SC0008

		public void PermitirEliminar(bool permitir)
        {
            mnuBajaEquipoAliado.Items[2].Enabled = permitir;
        }
		public void PermitirEditar(bool permitir)
		{
			mnuBajaEquipoAliado.Items[1].Enabled = permitir;
			btnEditar.Enabled = permitir;
		}
		public void PermitirRegistrar(bool permitir)
		{
			hlkRegistroActaNacimiento.Enabled = permitir;

		}

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

		#endregion

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
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Editar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        protected void mnuBajaEquipoAliado_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                switch (e.Item.Value)
                {
                    case "EliminarEquipoALiado":
                        this.presentador.Eliminar();
                        break;
                    case "Editar":
                        this.presentador.Editar();
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