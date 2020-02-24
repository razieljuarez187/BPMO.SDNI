//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class RegistrarLlantaUI : System.Web.UI.Page, IRegistrarLlantaVIS
	{
		#region Atributos

		private string nombreClase = "RegistrarLlantaUI";
		private RegistrarLlantaPRE presentador;

		#endregion Atributos

		#region Propiedades

		public bool? Activo { get { return true; } }

		public string Codigo
		{
			get
			{
				return this.ucLlanta.Codigo;
			}
			set
			{
				this.ucLlanta.Codigo = value;
			}
		}

		public DateTime? FC
		{
			get { return DateTime.Now; }
		}

		public DateTime? FUA
		{
			get { return this.FC; }
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

		public bool? Stock { get { return true; } }

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

		public int? UUA
		{
			get { return this.UC; }
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
        
        #endregion Propiedades

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
		{
            this.presentador = new RegistrarLlantaPRE(this, this.ucLlanta);

			if (!this.IsPostBack)
			{
				this.presentador.PrepararNuevo();
			}
		}

		#endregion Constructores

		#region Métodos

		public void EstablecerPaqueteNavegacion(string nombre, object valor)
		{
			Session[nombre] = valor;
		}

		public void RedirigirAConsulta()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarLlantaUI.aspx"));
		}
		public void RedirigirADetalles()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleLlantaUI.aspx"));
		}
        //SC_0008
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;
                this.RegistrarScript("Confirm", "abrirConfirmacion('" + mensaje + "');");
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

		#endregion Métodos

		#region Eventos

		protected void btnCancelar_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.CancelarRegistro();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cancelar el registro", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click:" + ex.Message);
			}
		}

		protected void btnNuevoRegistro_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.PrepararNuevo();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al limpiar los campos", ETipoMensajeIU.ERROR, this.nombreClase + ".btnNuevoRegistro_Click:" + ex.Message);
			}
		}

		protected void btnRegistrar_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.RegistrarLlanta();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al registrar una llanta", ETipoMensajeIU.ERROR, this.nombreClase + ".btnRegistrar_Click:" + ex.Message);
			}
		}

		#endregion Eventos
	}
}