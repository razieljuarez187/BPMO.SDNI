//Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class ConsultarLlantaUI : System.Web.UI.Page, IConsultarLlantaVIS
	{
		#region Atributos

		private string nombreClase = "ConsultarLlantaUI";
		private ConsultarLlantaPRE presentador = null;

        /// <summary>
        /// Enumerador de Catálogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador {            
            UnidadIdealease = 1,
            Sucursal = 2
        }

		#endregion

		#region Propiedades

		public bool? Activo
		{
			get
			{
				bool? activo = null;
				if (ddlActivo.SelectedIndex > 0)
					activo = Convert.ToBoolean(this.ddlActivo.SelectedValue);
				return activo;
			}
			set
			{
				if (value == null)
					this.ddlActivo.SelectedValue = value.ToString();
				else
					this.ddlActivo.SelectedIndex = 0;
			}
		}

		public string Codigo
		{
			get
			{
				string codigo = null;
				if (this.txtCodigo.Text.Trim().Length > 0)
					codigo = this.txtCodigo.Text.Trim();
				return codigo;
			}

			set
			{
				if (value != null)
					this.txtCodigo.Text = value.ToString();
				else
					this.txtCodigo.Text = string.Empty;
			}
		}

		public bool? EnStock
		{
			get
			{
				bool? enStock = null;
				if (this.ddlEnStock.SelectedIndex > 0)
					enStock = Convert.ToBoolean(this.ddlEnStock.SelectedValue);
				return enStock;
			}
			set
			{
				if (value != null)
					this.ddlEnStock.SelectedValue = value.ToString();
				else
					this.ddlEnStock.SelectedIndex = 0;
			}
		}

		public int IndicePaginaResultado
		{
			get { return this.grvLlantas.PageIndex; }
			set { this.grvLlantas.PageIndex = value; }
		}

		public string Medida
		{
			get
			{
				string medida = null;
				if (this.txtMedida.Text.Trim().Length > 0)
					medida = this.txtMedida.Text.Trim();
				return medida;
			}

			set
			{
				if (value != null)
					this.txtMedida.Text = value.ToString();
				else
					this.txtMedida.Text = string.Empty;
			}
		}

		public string NumeroSerie
		{
			get
			{
				string serie = null;
				if (this.txtNumeroSerie.Text.Trim().Length > 0)
					serie = this.txtNumeroSerie.Text.Trim();
				return serie;
			}
			set
			{
				if (value != null)
					this.txtNumeroSerie.Text = value.ToString();
				else
					this.txtNumeroSerie.Text = string.Empty;
			}
		}

		public List<LlantaBO> Resultado
		{
			get { return Session["listLlantas"] != null ? Session["listLlantas"] as List<LlantaBO> : null; }
			set { Session["listLlantas"] = value; }
		}

		public bool? Revitalizada
		{
			get
			{
				bool? revitalizada = null;
				if (this.ddlRevitalizada.SelectedIndex >= 0)
					revitalizada = Convert.ToBoolean(this.ddlRevitalizada.SelectedValue);
				return revitalizada;
			}
			set
			{
				if (value != null)
					this.ddlRevitalizada.SelectedValue = value.ToString();
				else
					this.ddlRevitalizada.SelectedIndex = 0;
			}
		}

		public ETipoEnllantable? TipoEnllantable
		{
			get
			{
				ETipoEnllantable? tipo = null;
				if (!string.IsNullOrEmpty(hdnTipoEnllantable.Value))
				{
					tipo = (ETipoEnllantable)Convert.ToInt32(hdnTipoEnllantable.Value);
				}
				return tipo;
			}
			set { }
		}

		/// <summary>
		/// Identificador de Equipo de la Unidad a Agregar
		/// </summary>
		public int? UnidadID
		{
			get
			{
				int? id = null;

				if (!string.IsNullOrEmpty(hdnUnidadID.Value)) id = int.Parse(hdnUnidadID.Value);

				return id;
			}
			set
			{
				hdnUnidadID.Value = value == null ? string.Empty : value.ToString();
			}
        }

        public int? SucursalID {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value))
                    if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                        return val;
                return null;
            }
            set {
                if (value != null)
                    this.hdnSucursalID.Value = value.Value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }
        public string SucursalNombre {
            get {
                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                    return this.txtSucursal.Text.Trim().ToUpper();
                return null;
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
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

		#region Propiedades Buscador

		public ECatalogoBuscador ViewState_Catalogo
		{
			get
			{
				return (ECatalogoBuscador)ViewState["BUSQUEDA"];
			}
			set
			{
				ViewState["BUSQUEDA"] = value;
			}
		}

		public string ViewState_Guid
		{
			get
			{
				if (ViewState["GuidSession"] == null)
				{
					Guid guid = Guid.NewGuid();
					ViewState["GuidSession"] = guid.ToString();
				}
				return ViewState["GuidSession"].ToString();
			}
		}

		protected object Session_BOSelecto
		{
			get
			{
				object objeto = null;
				string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
				if (Session[nombreSession] != null)
					objeto = (Session[nombreSession]);

				return objeto;
			}
			set
			{
				string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
				if (value != null)
					Session[nombreSession] = value;
				else
					Session.Remove(nombreSession);
			}
		}

		protected object Session_ObjetoBuscador
		{
			get
			{
				object objeto = null;
				if (Session[ViewState_Guid] != null)
					objeto = (Session[ViewState_Guid]);

				return objeto;
			}
			set
			{
				if (value != null)
					Session[ViewState_Guid] = value;
				else
					Session.Remove(ViewState_Guid);
			}
		}

		#endregion 

		#endregion

		#region Constructores

		protected void Page_Load(object sender, EventArgs e)
		{

			try
			{
				presentador = new ConsultarLlantaPRE(this);
				if (!IsPostBack)
				{
					this.presentador.PrepararBusqueda();
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
			}

			
		}

		#endregion 

		#region Métodos

		public void ActualizarResultado()
		{
			this.grvLlantas.DataSource = this.Resultado;
			this.grvLlantas.DataBind();
		}

		public void EstablecerPaqueteNavegacion(string nombre, object valor)
		{
			Session[nombre] = valor;
		}

		public void LimpiarSession()
		{
			if (Session["listLlantas"] != null)
				Session.Remove("listLlantas");
		}

		/// <summary>
		/// Este método despliega un mensaje en pantalla
		/// </summary>
		/// <param name="mensaje">Mensaje a desplegar</param>
		/// <param name="tipo">Tipo de mensaje a desplegar</param>
		/// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
		public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
		{
			Site masterMsj = (Site)Page.Master;
			if (tipo == ETipoMensajeIU.ERROR)
				masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
			else
				masterMsj.MostrarMensaje(mensaje, tipo);
		}

		public void PrepararBusqueda()
		{
			this.txtCodigo.Text = "";
			this.txtMedida.Text = "";
			this.txtNumeroSerie.Text = "";
			this.ddlActivo.SelectedIndex = 0;
			this.ddlEnStock.SelectedIndex = 0;
			this.ddlRevitalizada.SelectedIndex = 0;
		}

		public void RedirigirADetalles()
		{
			this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleLlantaUI.aspx"));
		}
        
        #region SC_0008
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistroActaNacimiento.Enabled = permitir;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        /// <summary>
		/// Registra Script en el cliente
		/// </summary>
		/// <param name="key">Key del script</param>
		/// <param name="script">script a registrar</param>
		private void RegistrarScript(string key, string script)
		{
			ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        #region Métodos para el Buscador

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }

        #endregion 

		#endregion 

		#region Eventos

		protected void btnBuscar_Click(object sender, EventArgs e)
		{
			try
			{
				this.presentador.Consultar();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al consultar llantas", ETipoMensajeIU.ERROR, this.nombreClase + ".btnBuscar_Click:" + ex.Message);
			}
		}

		protected void grvLlantas_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				this.presentador.CambiarPaginaResultado(e.NewPageIndex);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".grvLlantas_PageIndexChanging:" + ex.Message);
			}
		}

		protected void grvLlantas_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int index;

			if (e.CommandName.ToString().ToUpper() == "PAGE") return;

			try
			{
				index = Convert.ToInt32(e.CommandArgument.ToString());
				switch (e.CommandName.Trim())
				{
					case "Detalles":
						this.presentador.VerDetalles(index);
						break;
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al ejecutar la acción sobre la llanta", ETipoMensajeIU.ERROR, this.nombreClase + ".grvLlantas_RowCommand:" + ex.Message);
			}
		}
        
        #region Eventos para el Buscador

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                NumeroSerie = numeroSerie;
                if (NumeroSerie != null)
                    EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }
        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarUnidad_Click: " + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e) {
            try {
                string sucursal = SucursalNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombre = sucursal;
                if (SucursalNombre != null)
                    EjecutaBuscador("Sucursal", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged: " + ex.Message);
            }
        }
        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        #endregion

        #endregion


    }
}