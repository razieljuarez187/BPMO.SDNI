//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class EditarLlantaUI : System.Web.UI.Page, IEditarLlantaVIS
	{
		#region Atributos

		private string nombreClase = "EditarLlantaUI";
		private EditarLlantaPRE presentador;

		#endregion Atributos

		#region Propiedades

		public bool? Activo
		{
			get { return this.ucLlanta.Activo; }
			set { this.ucLlanta.Activo = value; }
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

				TextBox txtCodigoLlanta = mEditarLlanta.Controls[0].FindControl("txtValue") as TextBox;
				txtCodigoLlanta.Text = value;
			}
		}

		public LlantaBO DatosNavegacion
		{
			get
			{
				if (Session["LlantaBO"] != null) return Session["LlantaBO"] as LlantaBO;
				return null;
			}
			set { Session["LlantaBO"] = value; }
		}

		public int? EnllantableID
		{
			get
			{
				return (this.ucLlanta.EnllantableID);
			}
			set
			{
				this.ucLlanta.EnllantableID = value;
			}
		}

		public bool? EsRefaccion
		{
			get { return this.ucLlanta.EsRefaccion; }
			set { this.ucLlanta.EsRefaccion = value; }
		}

		public DateTime? FC
		{
			get { return this.ucLlanta.FC; }
			set { this.ucLlanta.FC = value; }
		}

		public DateTime? FUA
		{
			get { return DateTime.Now; }
		}

		public int? LlantaID
		{
			get
			{
				return (this.ucLlanta.LlantaID);
			}
			set
			{
				this.ucLlanta.LlantaID = value;
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

		public int? Posicion
		{
			get
			{
				return (this.ucLlanta.Posicion);
			}
			set
			{
				this.ucLlanta.Posicion = value;
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
			get { return this.ucLlanta.Stock; }
			set { this.ucLlanta.Stock = value; }
		}

		public int? TipoEnllantable
		{
			get
			{
				return (this.ucLlanta.TipoEnllantable);
			}
			set
			{
				this.ucLlanta.TipoEnllantable = value;
			}
		}

		public int? UC
		{
			get { return this.ucLlanta.UC; }
			set { this.ucLlanta.UC = value; }
		}

		public LlantaBO UltimoObjetoLlanta
		{
			get
			{
				if ((LlantaBO)Session["UltimoObjetoLlanta"] == null)
					return new LlantaBO();
				else
					return (LlantaBO)Session["UltimoObjetoLlanta"];
			}
			set
			{
				Session["UltimoObjetoLlanta"] = value;
			}
		}

		public int? UUA
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

        #region SC008

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

		#endregion SC008

		#endregion Propiedades

		#region Constructor

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				this.presentador = new EditarLlantaPRE(this, this.ucLlanta);

				if (!this.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    this.presentador.RealizarPrimeraCarga();
                    this.presentador.PreparaEdicion();
                }
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + "Page_Load" + ex.Message);
			}
		}

		#endregion

		#region Métodos

		public void LimpiarSesion()
		{
			if (Session["UltimoObjetoLlanta"] != null)
				Session.Remove("UltimoObjetoLlanta");
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

		public void PrepararEdicion()
		{
		}

        //SC_0008
        public void PermitirRegistrar(bool permitir)
        {
            this.hlRegistroOrden.Enabled = permitir;
        }

		public void RedirigirAConsulta()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarLlantaUI.aspx"));
		}
		public void RedirigirADetalle()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleLlantaUI.aspx"));
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
				this.presentador.CancelarEdicion();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
			}
		}

		protected void btnEditar_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.EditarLlanta();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al realizar la edición de la llanta", ETipoMensajeIU.ERROR, this.nombreClase + ".btnEditar_Click:" + ex.Message);
			}
		}

		#endregion Eventos
	}
}