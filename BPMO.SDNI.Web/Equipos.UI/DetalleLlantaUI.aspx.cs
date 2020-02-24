//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class DetalleLlantaUI : System.Web.UI.Page, IDetalleLlantaVIS
	{
		#region Atributos

		private string nombreClase = "DetalleLlantaUI";
		private DetalleLlantaPRE presentador;

		#endregion Atributos

		#region Propiedades

		public bool Actualizada
		{
			get
			{
				return Session["LlantaActualizada"] != null;
			}
		}

		public int? LlantaID
		{
			get
			{
				return this.ucLlanta.LlantaID;
			}
			set
			{
				this.ucLlanta.LlantaID = value;
			}
		}
		public string Codigo
		{
			get
			{
				return this.ucLlanta.Codigo;
			}
			set
			{
				this.ucLlanta.Codigo = value;

				TextBox txtCodigoLlanta = mBajaLlanta.Controls[0].FindControl("txtValue") as TextBox;
				txtCodigoLlanta.Text = value;
			}
		}
		public string Marca
		{
			get
			{
				return this.ucLlanta.Marca;
			}
			set
			{
				this.ucLlanta.Marca = value;
			}
		}
		public string Modelo
		{
			get
			{
				return this.ucLlanta.Modelo;
			}
			set
			{
				this.ucLlanta.Modelo = value;
			}
		}
		public string Medida
		{
			get
			{
				return this.ucLlanta.Medida;
			}
			set
			{
				this.ucLlanta.Medida = value;
			}
		}
		public decimal? Profundidad
		{
			get
			{
				return this.ucLlanta.Profundidad;
			}
			set
			{
				this.ucLlanta.Profundidad = value;
			}
		}
		public bool? Revitalizada
		{
			get
			{
				return this.ucLlanta.Revitalizada;
			}
			set
			{
				this.ucLlanta.Revitalizada = value;
			}
		}

		public bool? Stock
		{
			get
			{
				return this.ucLlanta.Stock;
			}
			set
			{
				this.ucLlanta.Stock = value;
			}
		}
		public string DescripcionEnllantable
		{
			get
			{
				return this.ucLlanta.DescripcionEnllantable;
			}
			set
			{
				this.ucLlanta.DescripcionEnllantable = value;
			}
		}

		public List<ArchivoBO> ArchivosAdjuntos
		{
			get { return this.ucLlanta.ArchivosAdjuntos; }
			set { this.ucLlanta.ArchivosAdjuntos = value; }
		}

		public DateTime? FC
		{
			set
			{
				this.ucLlanta.FC = value;
			}
		}
		public DateTime? FUA
		{
			set
			{
				this.ucLlanta.FUA = value;
			}
		}
		public int? UC
		{
			get { return this.ucLlanta.UC; }
			set { this.ucLlanta.UC = value; }
		}
		public int? UUA
		{
			get
			{
				return this.ucLlanta.UUA;
			}
			set
			{
				this.ucLlanta.UUA = value;
			}
		}

		public string UsuarioCreacion
		{
			set
			{
				this.ucLlanta.UsuarioCreacion = value.ToString();
			}
		}
		public string UsuarioEdicion
		{
			set
			{
				this.ucLlanta.UsuarioEdicion = value.ToString();
			}
		}
		public bool? Activo
		{
			get
			{
				return this.ucLlanta.Activo;
			}
			set
			{
				this.ucLlanta.Activo = value;
			}
		}

		#region SC_0008
		public int? UnidadOperativaID
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
		public int? UsuarioID
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

        public int? SucursalID {
            get {
                return this.ucLlanta.SucursalID;
            }
            set {
                this.ucLlanta.SucursalID = value;
            }
        }
        public string SucursalNombre {
            get {
                return this.ucLlanta.SucursalNombre;
            }
            set {
                this.ucLlanta.SucursalNombre = value;
            }
        }
        
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				this.presentador = new DetalleLlantaPRE(this, this.ucLlanta);
				
                if (!IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    this.presentador.RealizarPrimeraCarga();
                }
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
			}
		}
		#endregion

		#region Métodos

		public void PermitirEliminar(bool permitir)
		{
			this.mBajaLlanta.Items[2].Enabled = permitir;
		}
		public void PermitirEditar(bool permitir)
		{
			this.mBajaLlanta.Items[1].Enabled = permitir;
			this.btnEditar.Enabled = permitir;
		}
		//SC_0008
		public void PermitirRegistrar(bool permitir)
		{
			this.hlRegistroOrden.Enabled = permitir;
		}

		public LlantaBO ObtenerDatosNavegacion()
		{
			return Session["LlantaBO"] as LlantaBO;
		}

		public void EstablecerDatosNavegacion(string nombre, object valor)
		{
			Session[nombre] = valor;
		}

		public void Recargar()
		{
			Session.Remove("LlantaActualizada");
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleLlantaUI.aspx"));
		}

		public void RedirigirAEdicion()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/EditarLlantaUI.aspx"));
		}
		public void RedirigirAPopUp()
		{
			RegistrarScript("EliminarLlanta", "showDialogModal('EliminarLlantaUI.aspx', '955px', '450px', undefined);  __doPostBack('',''); ");
			Session["UsuarioIDEliminar"] = UsuarioID;
			Session["UnidadOperativaIDEliminar"]=UnidadOperativaID;
			Session["UCEliminar"] = UC;
			Session["LLantaIDEliminar"] = LlantaID;
		}
        //SC_0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

		public void LimpiarSesion()
		{

			if (Session["UsuarioIDEliminar"] != null)
				this.Session.Remove("UsuarioIDEliminar");
			if (Session["UnidadOperativaIDEliminar"] != null)
				this.Session.Remove("UnidadOperativaIDEliminar");
			if (Session["UCEliminar"] != null)
				this.Session.Remove("UCEliminar");
			if (Session["LLantaIDEliminar"] != null)
				this.Session.Remove("LLantaIDEliminar");
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

		/// <summary>
		/// Registra Script en el cliente
		/// </summary>
		/// <param name="key">Key del script</param>
		/// <param name="script">script a registrar</param>
		private void RegistrarScript(string key, string script)
		{
			ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
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
				this.MostrarMensaje("Inconsistencia al cambiar a edición", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEditar_Click:" + ex.Message);
			}
		}

		protected void mBajaLlanta_MenuItemClick(object sender, MenuEventArgs e)
		{
			switch (e.Item.Value)
			{
				case "EliminarLlanta":
					this.presentador.Eliminar();

					this.mBajaLlanta.Items[1].Selected = true;
					break;
				case "Editar":
					this.presentador.Editar();
					break;
			}
		}

		#endregion
	}
}