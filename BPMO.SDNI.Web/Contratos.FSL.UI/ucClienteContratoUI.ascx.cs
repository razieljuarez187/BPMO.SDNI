// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
// Satisface al caso de uso CU001 - Calcular Monto a Facturar FSL
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucClienteContratoUI : UserControl, IucClienteContratoVIS
    {
        #region Atributos

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucClienteContratoUI";

        /// <summary>
        /// Enumerador de Busqueda por Catalogos
        /// </summary>
        public enum ECatalogoBuscador
        {
            CuentaClienteIdealease = 0,
            DireccionCliente = 1
        }

        #endregion

        #region Propiedades
        /// <summary>
        /// Presentador de Cliente Contrato
        /// </summary>
        internal ucClienteContratoPRE Presentador { get; set; }

        #region Propiedades Buscador

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

        public ECatalogoBuscador ViewState_Catalogo
        {
            get { return (ECatalogoBuscador)ViewState["BUSQUEDA"]; }
            set { ViewState["BUSQUEDA"] = value; }
        }

        #endregion

        /// <summary>
        /// Unidad Opertiva cargada de Sesion
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null &&
                    (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id, Nombre = masterMsj.Adscripcion.UnidadOperativa.Nombre };
                return null;
            }
        }

        /// <summary>
        /// Representantes Legales asignados al Contrato
        /// </summary>
        public List<RepresentanteLegalBO> RepresentantesLegalesContrato
        {
            get
            {
                List<RepresentanteLegalBO> listValue;
                if (Session["RepresentantesLegalesContrato"] != null)
                    listValue = (List<RepresentanteLegalBO>)Session["RepresentantesLegalesContrato"];
                else
                    listValue = new List<RepresentanteLegalBO>();

                return listValue;
            }
            set
            {
                List<RepresentanteLegalBO> listValue = value ?? new List<RepresentanteLegalBO>();

                Session["RepresentantesLegalesContrato"] = listValue;

                grdRepresentantesLegales.DataSource = listValue;
                grdRepresentantesLegales.DataBind();
            }
        }

        /// <summary>
        /// Obligados Solidarios Agregados al contrato
        /// </summary>
		public List<ObligadoSolidarioBO> ObligadosSolidariosContrato
        {
            get
            {
                List<ObligadoSolidarioBO> listValue;
                if (Session["ObligadosSolidariosContrato"] != null)
                    listValue = (List<ObligadoSolidarioBO>)Session["ObligadosSolidariosContrato"];
                else
                    listValue = new List<ObligadoSolidarioBO>();

                return listValue;
            }
            set
            {
                List<ObligadoSolidarioBO> listValue = value ?? new List<ObligadoSolidarioBO>();

                Session["ObligadosSolidariosContrato"] = listValue;

                grdObligadosSolidarios.DataSource = listValue;
                grdObligadosSolidarios.DataBind();
            }
        }

        /// <summary>
        /// Nombre del Cliente del Contrato
        /// </summary>
        public string NombreCuentaCliente
        {
            get { return txtNombreCliente.Text.Trim().ToUpper(); }
            set { txtNombreCliente.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Identificador del Cuenta Cliente del Contrato
        /// </summary>
        public int? CuentaClienteID
        {
            get
            {
                int? id = null;

                if (!string.IsNullOrEmpty(hdnCuentaClienteID.Value.Trim()))
                    id = int.Parse(hdnCuentaClienteID.Value.Trim());

                return id;
            }
            set { hdnCuentaClienteID.Value = value == null ? string.Empty : value.ToString(); }
        }

        /// <summary>
        /// Identificador del Cliente del Contrato
        /// </summary>
        public int? ClienteID
        {
            get
            {
                int? id = null;

                if (!string.IsNullOrEmpty(hdnClienteID.Value.Trim())) id = int.Parse(hdnClienteID.Value.Trim());

                return id;
            }
            set { hdnClienteID.Value = value == null ? string.Empty : value.ToString(); }
        }

        /// <summary>
        /// Listado de Representantes Legales del Cliente
        /// </summary>
        public List<RepresentanteLegalBO> ListadoRepresentantesLegales
        {
            get
            {
                if (Session["ListadoRepresentantesLegales"] != null)
                    return (List<RepresentanteLegalBO>) Session["ListadoRepresentantesLegales"];
                
                return new List<RepresentanteLegalBO>();
            }
            set
            {
                List<RepresentanteLegalBO> listValue = value ?? new List<RepresentanteLegalBO>();

                // Clonar la Lista para no afectar la lista original
                var lista = new List<RepresentanteLegalBO>(listValue);
                Session["ListadoRepresentantesLegales"] = new List<RepresentanteLegalBO>(listValue);
                // Agregar el SucursalBO de fachada
                lista.Insert(0, new RepresentanteLegalBO { Id = 0, Nombre = "Seleccione una opción" });
                //Limpiar el DropDownList Actual
                ddlRepresentantesLegales.Items.Clear();
                // Asignar Lista al DropDownList
                ddlRepresentantesLegales.DataTextField = "Nombre";
                ddlRepresentantesLegales.DataValueField = "Id";
                ddlRepresentantesLegales.DataSource = lista;
                ddlRepresentantesLegales.DataBind();
            }
        }

        /// <summary>
        /// Representante Legal seleccionado para agregar al contrato
        /// </summary>
        public RepresentanteLegalBO RepresentanteLegalSeleccionado
        {
            get
            {
                RepresentanteLegalBO seleccionado = null;

                if (ddlRepresentantesLegales.SelectedValue != "0")
                    seleccionado = new RepresentanteLegalBO
                    {
                        Id = int.Parse(ddlRepresentantesLegales.SelectedValue),
                        Nombre = ddlRepresentantesLegales.SelectedItem.Text
                    };

                return seleccionado;
            }
        }

        /// <summary>
        ///  Listado de Obligados Solidarios del Cliente
        /// </summary>
        public List<ObligadoSolidarioBO> ListadoObligadosSolidarios
        {
            get { return (List<ObligadoSolidarioBO>)ddlObligadosSolidarios.DataSource; }
            set
            {
                List<ObligadoSolidarioBO> listValue = value ?? new List<ObligadoSolidarioBO>();

                // Clonar la Lista para no afectar la lista original
                var lista = new List<ObligadoSolidarioBO>(listValue);
                // Agregar el SucursalBO de fachada
                lista.Insert(0, new ObligadoSolidarioProxyBO { Id = 0, Nombre = "Seleccione una opción" });//SC0005
                //Limpiar el DropDownList Actual
                ddlObligadosSolidarios.Items.Clear();
                // Asignar Lista al DropDownList
                ddlObligadosSolidarios.DataTextField = "Nombre";
                ddlObligadosSolidarios.DataValueField = "Id";
                ddlObligadosSolidarios.DataSource = lista;
                ddlObligadosSolidarios.DataBind();
            }
        }

        /// <summary>
        /// Obligado Solidario seleccionado a agregar al contrato
        /// </summary>
        public ObligadoSolidarioBO ObligadoSolidarioSeleccionado
        {
            get
            {
				if (ddlObligadosSolidarios.SelectedValue != "0")
                    return new ObligadoSolidarioProxyBO//SC0005
                    {
                        Id = int.Parse(ddlObligadosSolidarios.SelectedValue),
                        Nombre = ddlObligadosSolidarios.SelectedItem.Text
                    };

                return null;
            }
        }

        /// <summary>
        /// Indica si la Interfaz de Usuario esta en modo Editar, en caso contrato esta en modo consultar
        /// </summary>
        public bool ModoEditar {
            get
            {
                if (!string.IsNullOrEmpty(hdnModo.Value) && bool.Parse(hdnModo.Value.ToLower()))
                    return true;

                return false;
            }
        }
        public string Calle
        {
            get { return (String.IsNullOrEmpty(hdnCalle.Value.Trim())) ? null : hdnCalle.Value; }
            set { hdnCalle.Value = value ?? String.Empty; }
        }
        public string Direccion
        {
            get { return (String.IsNullOrEmpty(txtDomicilioCliente.Text.Trim())) ? null : txtDomicilioCliente.Text.ToUpper(); }
            set { txtDomicilioCliente.Text = value ?? String.Empty; }
        }
        public string Colonia
        {
            get { return (String.IsNullOrEmpty(hdnColonia.Value.Trim())) ? null : hdnColonia.Value.ToUpper(); }
            set { hdnColonia.Value = value ?? String.Empty; }
        }

		public List<RepresentanteLegalBO> RepresentantesObligado
		{
			get { return (List<RepresentanteLegalBO>)Session["RepresentantesObligSolidario"]; }
			set
			{
				List<RepresentanteLegalBO> listValue = value ?? new List<RepresentanteLegalBO>();
				// Clonar la Lista para no afectar la lista original
				var lista = new List<RepresentanteLegalBO>(listValue);
				//Limpiar el DropDownList Actual
				grdRepresentantesObligadoSolidario.DataSource = null;
				Session["RepresentantesObligSolidario"] = null;
				grdRepresentantesObligadoSolidario.DataSource = lista;
				Session["RepresentantesObligSolidario"] = lista;
				grdRepresentantesObligadoSolidario.DataBind();
			}
		}
	    public string CodigoPostal
        {
            get { return (String.IsNullOrEmpty(hdnCodigoPostal.Value.Trim())) ? null : hdnCodigoPostal.Value; }
            set { hdnCodigoPostal.Value = value ?? String.Empty; }
        }
        public string Ciudad
        {
            get { return (String.IsNullOrEmpty(hdnCiudad.Value.Trim())) ? null : hdnCiudad.Value.ToUpper(); }
            set { hdnCiudad.Value = value ?? String.Empty; }
        }
        public string Estado
        {
            get { return (String.IsNullOrEmpty(hdnEstado.Value.Trim())) ? null : hdnEstado.Value.ToUpper(); }
            set { hdnEstado.Value = value ?? String.Empty; }
        }
        public string Municipio
        {
            get { return (String.IsNullOrEmpty(hdnMunicipio.Value.Trim())) ? null : hdnMunicipio.Value.ToUpper(); }
            set { hdnMunicipio.Value = value ?? String.Empty; }
        }
        public string Pais
        {
            get { return (String.IsNullOrEmpty(hdnPais.Value.Trim())) ? null : hdnPais.Value.ToUpper(); }
            set { hdnPais.Value = value ?? String.Empty; }
        }
        /// <summary>
        /// Numero de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        public string ClienteNumeroCuenta
        {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text) ? null : this.txtNumeroCuentaOracle.Text; }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }

        #region SC0005
        public List<RepresentanteLegalBO> RepresentantesObligadosSolidario
        {
            get
            {
                if (Session["ListaRepresentantesObligadosSolidario"] == null)
                    return new List<RepresentanteLegalBO>();
                return (List<RepresentanteLegalBO>)Session["ListaRepresentantesObligadosSolidario"];
            }
            set
            {
                Session["ListaRepresentantesObligadosSolidario"] = value;
            }
        }
        #endregion

        #region SC0007
        public bool? EsFisico
        {
            get
            {
                return hdnEsFisico.Value.ToLower() == true.ToString().ToLower();
            }
            set
            {
                if (value == null || value == false) hdnEsFisico.Value = "false";
                else
                    hdnEsFisico.Value = "true";
            }
        }

        public bool? SoloRepresentantes
        {
            get
            {
                return cbSoloRepresentantes.Checked;
            }
            set
            {
                cbSoloRepresentantes.Checked = value == true;
            }
        }
        #endregion SC0007

		/// <summary>
		/// Obtiene el identificador del aval seleccionado
		/// </summary>
		public int? AvalSeleccionadoID
		{
			get
			{
				if (!string.IsNullOrEmpty(this.ddlAvales.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlAvales.SelectedValue))
				{
					if (System.String.Compare(this.ddlAvales.SelectedValue, "0", System.StringComparison.Ordinal) != 0)
					{
						int val = 0;
						return Int32.TryParse(this.ddlAvales.SelectedValue, out val) ? (int?)val : null;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Obtiene o establece los avales del cliente que han sido seleccionados
		/// </summary>
		public List<AvalBO> AvalesSeleccionados
		{
			get
			{
				List<AvalBO> lst = new List<AvalBO>();
				if (Session["AvalesContratoFSL"] != null)
					lst = (List<AvalBO>)Session["AvalesContratoFSL"];

				return lst;
			}
			set
			{
				List<AvalBO> lst = value ?? new List<AvalBO>();
				Session["AvalesContratoFSL"] = lst;
			}
		}

		/// <summary>
		/// Obtiene o establece el listado de avales del cliente seleccionado
		/// </summary>
		public List<AvalBO> AvalesTotales
		{
			get
			{
				if (Session["ListadoAvalesFSL"] != null)
					return (List<AvalBO>)Session["ListadoAvalesFSL"];

				return new List<AvalBO>();
			}
			set
			{
				List<AvalBO> lst = value ?? new List<AvalBO>();

				//Se sube a la sesión
				Session["ListadoAvalesFSL"] = new List<AvalBO>(lst);
				//Se asigna en el dropdownlist
				this.ddlAvales.Items.Clear();
				this.ddlAvales.Items.Add(new ListItem("Seleccione una opción", "0"));
				this.ddlAvales.DataTextField = "Nombre";
				this.ddlAvales.DataValueField = "Id";
				this.ddlAvales.DataSource = lst;
				this.ddlAvales.DataBind();
			}
		}

		public int? RepresentanteAvalSeleccionadoID
		{
			get
			{
				if (!string.IsNullOrEmpty(this.hdnRepresentanteAvalSeleccionadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnRepresentanteAvalSeleccionadoID.Value))
				{
					int val = 0;
					return Int32.TryParse(this.hdnRepresentanteAvalSeleccionadoID.Value, out val) ? (int?)val : null;
				}
				return null;
			}
		}

        public int? DireccionClienteID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnDireccionClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnDireccionClienteID.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnDireccionClienteID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { hdnDireccionClienteID.Value = value == null ? string.Empty : value.ToString(); }
        }

		public bool? ObligadosComoAvales
		{
			get
			{
				return this.cbObligadosComoAvales.Checked;
			}
			set
			{
				if (value != null)
					this.cbObligadosComoAvales.Checked = value.Value;
				else
					this.cbObligadosComoAvales.Checked = false;
			}
		}

		public List<RepresentanteLegalBO> RepresentantesAvalSeleccionados
		{
			get { return (List<RepresentanteLegalBO>)Session["RepresentantesAvalFSL"]; }
			set
			{
				List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

				Session["RepresentantesAvalFSL"] = lst;
			}
		}

		public List<RepresentanteLegalBO> RepresentantesAvalTotales
		{
			get
			{
				if (Session["ListaRepresentantesAvalFSL"] == null)
					return new List<RepresentanteLegalBO>();
				return (List<RepresentanteLegalBO>)Session["ListaRepresentantesAvalFSL"];
			}
			set
			{
				List<RepresentanteLegalBO> lst = value ?? new List<RepresentanteLegalBO>();

				Session["ListaRepresentantesAvalFSL"] = lst;
				this.grdRepresentantesAval.DataSource = lst;
				this.grdRepresentantesAval.DataBind();
			}
		}
        #endregion Propiedades

        #region Eventos
        #region SC0005
        protected void chkRepresentanteOS_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				CheckBox chk = (CheckBox)sender;
				GridViewRow row = (GridViewRow)chk.Parent.Parent;
				Label lbl = (Label)row.FindControl("lblRepresentanteOSID");
				int id;
				if (Int32.TryParse(lbl.Text, out id))
				{
					bool valor = chk.Checked;
					List<RepresentanteLegalBO> reps = RepresentantesObligado;
					int index = reps.FindIndex(x => x.Id != null && x.Id.Value == id);
					reps[index].Activo = valor;
					RepresentantesObligado = reps;
				}
			}
			catch (Exception ex)
			{
				MostrarMensaje("Ocurrío una inconsistencia al momento de intentar cambiar el valor del Representante legal", ETipoMensajeIU.ERROR, NombreClase + ".chkRepresentanteOS_CheckedChanged" + ex.Message);
			}
		}

		protected void grdRepresentantesObligados_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
            grdRepresentantesObligados.DataSource = RepresentantesObligadosSolidario;
			grdRepresentantesObligados.PageIndex = e.NewPageIndex;
            grdRepresentantesObligados.DataBind();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);
		}

		protected void grdRepresentantesObligadoSolidario_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					RepresentanteLegalBO persona = (RepresentanteLegalBO)e.Row.DataItem;
					var chk = e.Row.FindControl("chkRepOS") as CheckBox;
					if (chk != null)
					{
					    if (persona.Activo.HasValue)
							chk.Checked = persona.Activo.Value;
					}
					var lbl = e.Row.FindControl("lblRepresentanteOSID") as Label;
					if (lbl != null)
					{
						string id = string.Empty;
						if (persona != null)
							if (persona.Id.HasValue)
								id = persona.Id.Value.ToString(CultureInfo.InvariantCulture);
						lbl.Text = id;
					}
				}
			}
			catch (Exception ex)
			{
				MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".grdRepresentantesObligadoSolidario_RowDataBound: " + ex.Message);
			}
		}

		protected void grdRepresentantesObligadoSolidario_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				grdRepresentantesObligadoSolidario.DataSource = RepresentantesObligado;
				grdRepresentantesObligadoSolidario.PageIndex = e.NewPageIndex;
				grdRepresentantesObligadoSolidario.DataBind();
			}
			catch (Exception ex)
			{
				MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, NombreClase + ".grdRepresentantesObligadoSolidario_PageIndexChanging: " + ex.Message);
			}
		}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucClienteContratoPRE(this);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
                               NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void grdObligadosSolidarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdObligadosSolidarios.DataSource = ObligadosSolidariosContrato;
                grdObligadosSolidarios.PageIndex = e.NewPageIndex;
                grdObligadosSolidarios.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR,
                               NombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdObligadosSolidarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
			try
			{
				string eCommandNameUpper = e.CommandName.ToUpper();
				if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;

				switch (eCommandNameUpper.Trim())
				{
					case "CMDELIMINAR":
						int index = Convert.ToInt32(e.CommandArgument);
						ObligadoSolidarioBO obligado = ObligadosSolidariosContrato[index];

						Presentador.RemoverObligadoSolidarioContrato(obligado);
						break;
					case "CMDDETALLE":
						int indexd = 0;
						if (Int32.TryParse(e.CommandArgument.ToString(), out indexd))
						{
							if (indexd > ObligadosSolidariosContrato.Count || indexd < 0)
							{
								MostrarMensaje("No se encontró el detalle del Obligado Solidario seleccionado", ETipoMensajeIU.INFORMACION);
								return;
							}
							ObligadoSolidarioBO oblig = ObligadosSolidariosContrato[indexd];
                            RepresentantesObligadosSolidario = null;//SC0005
							Presentador.MostrarDetalleObligado(oblig);
							ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "DialogoDetalleObligado();", true);
						}
						else
							MostrarMensaje("No se encontró el detalle del Obligado Solidario seleccionado", ETipoMensajeIU.INFORMACION);
						break;
				}

			}
			catch (Exception ex)
			{
				MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".grdObligadosSolidarios_RowCommand: " + ex.Message);
			}
        }

        protected void grdObligadosSolidarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
			try
			{
				if (e.Row.RowType != DataControlRowType.DataRow) return;
				ObligadoSolidarioBO bo = e.Row.DataItem != null ? (ObligadoSolidarioBO)e.Row.DataItem : new ObligadoSolidarioProxyBO();
				if (!bo.TipoObligado.HasValue)
					e.Row.FindControl("ibtDetalle").Visible = false;
				else if (bo.TipoObligado == ETipoObligadoSolidario.Fisico)
					e.Row.FindControl("ibtDetalle").Visible = false;
			}
			catch (Exception ex)
			{
				MostrarMensaje("", ETipoMensajeIU.ERROR, NombreClase + ".grdObligadosSolidarios_RowDataBound" + ex.Message);
			}
        }

        protected void btnAgregarObligadoSolidario_Click(object sender, EventArgs e)
        {
			try
			{
                ObligadoSolidarioBO oblig = Presentador.ConsultarObligadoSolidario(ObligadoSolidarioSeleccionado);//SC0005
			    if (oblig != null)
			    {
			        if (oblig.TipoObligado.HasValue)
			        {
                        if (oblig.TipoObligado.Value == ETipoObligadoSolidario.Fisico)
                        {
                            Presentador.AgregarObligadoSolidarioContrato(ObligadoSolidarioSeleccionado);
                            ddlObligadosSolidarios.SelectedValue = "0";
                            MostrarRepresentantesObligados(false);
                        }
                        else if (ObtenerRepresentantesObligadoSeleccionados() != null)//SC0005
                        {
                            if (ObtenerRepresentantesObligadoSeleccionados().Count > 0)
                            {
                                Presentador.AgregarObligadoSolidarioContrato(ObligadoSolidarioSeleccionado, ObtenerRepresentantesObligadoSeleccionados());
                                ddlObligadosSolidarios.SelectedValue = "0";
                                MostrarRepresentantesObligados(false);
                            }
                            else
                            {
                                MostrarMensaje("Es necesario seleccionar al menos un representante legal, para el obligado solidarío.", ETipoMensajeIU.ADVERTENCIA);
                            }
                        }
			        }
			    }
			}
			catch (Exception ex)
			{
				MostrarMensaje("Inconsistencias al agregar un Obligado Solidario al contrato", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".btnAgregarObligadoSolidario_Click: " + ex.Message);
			}

			ddlObligadosSolidarios.SelectedValue = "0";
			MostrarRepresentantesObligados(false);
        }

        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
			try
			{
                grdRepresentantesLegales.DataSource = RepresentantesLegalesContrato;
                grdRepresentantesLegales.PageIndex = e.NewPageIndex;
                grdRepresentantesLegales.DataBind();
			}
			catch (Exception ex)
			{
                MostrarMensaje("Inconsistencias al cambiar de pagina en los datos del representante legal", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".grdRepresentantesLegales_PageIndexChanging: " + ex.Message);
			}
        }

        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);
                RepresentanteLegalBO representante = RepresentantesLegalesContrato[index];

                Presentador.RemoverRepresentanteLegalContrato(representante);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR,
                               NombreClase + ".grdRepresentantesLegales_RowCommand: " + ex.Message);
            }
        }

        protected void grdRepresentantesLegales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var label = e.Row.FindControl("lblTipoPersona") as Label;
                    if (label != null)
                    {
                        var persona = (PersonaBO) e.Row.DataItem;
                        string tipo = string.Empty;
                        if (persona.TipoPersona.HasValue)
                            tipo =
                                ((DescriptionAttribute)
                                 persona.TipoPersona.Value.GetType()
                                        .GetField(persona.TipoPersona.Value.ToString())
                                        .GetCustomAttributes(typeof (DescriptionAttribute), false)[0]).Description;
                        label.Text = tipo;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR,
                               NombreClase + ".grdRepresentantesLegales_RowDataBound: " + ex.Message);
            }
        }

        protected void btnAgregarRepresentante_Click(object sender, EventArgs e)
        {
            try
            {
                Presentador.AgregarRepresentanteLegalContrato(RepresentanteLegalSeleccionado);
                ddlRepresentantesLegales.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al agregar un Representante Legal al contrato",
                               ETipoMensajeIU.ADVERTENCIA,
                               NombreClase + ".btnAgregarRepresentante_Click: " + ex.Message);
            }
        }

        protected void txtNombreCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreCuentaCliente = NombreCuentaCliente;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                NombreCuentaCliente = nombreCuentaCliente;
                if (NombreCuentaCliente != null)
                {
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA,
                               NombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }

        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA,
                               NombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.DireccionCliente:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }

        protected void ibtnBuscarDirieccionCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(this.hdnClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteID.Value))
                    EjecutaBuscador("DireccionCuentaClienteIdealease", ECatalogoBuscador.DireccionCliente);
                else
                    this.MostrarMensaje("Por favor seleccione un cliente previamente a consultar sus direcciones.", ETipoMensajeIU.ADVERTENCIA, null);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar las direcciones del Cliente", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarDirieccionCliente_Click:" + ex.Message);
            }
        }

		protected void ddlObligadosSolidarios_SelectedIndexChanged(object sender, EventArgs e)
		{
		    try
		    {
		        MostrarRepresentantesObligados(false);
		        Presentador.ObtenerRepresentanteOS(ObligadoSolidarioSeleccionado);
		    }
		    catch (Exception ex)
		    {
                MostrarMensaje("Inconsistencias al cambiar de pagina en los datos de Obligados Solidarios", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ddlObligadosSolidarios_SelectedIndexChanged: " + ex.Message);
		    }

		}

        protected void cbSoloRepresentantes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Presentador.ConfigurarSoloRepresentantes();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".cbSoloRepresentantes_CheckedChanged: " + ex.Message);
            }
        }

		protected void cbObligadosComoAvales_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				this.Presentador.ConfigurarObligadosComoAvales();

				this.ddlAvales.SelectedIndex = -1;
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, "ucClienteContratoUI.cbObligadosComoAvales_CheckedChanged: " + ex.Message);
			}
		}

		protected void ddlAvales_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.Presentador.SeleccionarAval();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al seleccionar el aval", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ddlAvales_SelectedIndexChanged: " + ex.Message);
			}
		}

		protected void btnAgregarAval_Click(object sender, EventArgs e)
		{
			try
			{
				this.Presentador.AgregarAval();

				this.ddlAvales.SelectedIndex = -1;
				this.RepresentantesAvalSeleccionados = null;
				this.RepresentantesAvalTotales = null;
				this.MostrarRepresentantesAval(false);
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al agregar un Aval al contrato", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".btnAgregarAval_Click: " + ex.Message);
			}
		}

		protected void grdRepresentantesAval_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					RepresentanteLegalBO persona = (RepresentanteLegalBO)e.Row.DataItem;
					var chk = e.Row.FindControl("chkRepAval") as CheckBox;

					if (chk != null)
						chk.Checked = this.RepresentantesAvalSeleccionados != null && this.RepresentantesAvalSeleccionados.Exists(p => p.Id == persona.Id);
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias listar los representantes legales del aval", ETipoMensajeIU.ERROR, NombreClase + ".grdRepresentantesAval_RowDataBound: " + ex.Message);
			}
		}
		
		protected void grdRepresentantesAval_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				this.grdRepresentantesAval.DataSource = this.RepresentantesAvalTotales;
				this.grdRepresentantesAval.PageIndex = e.NewPageIndex;
				this.grdRepresentantesAval.DataBind();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al cambiar la página de los obligados solidarios", ETipoMensajeIU.ERROR, NombreClase + ".grdObligadosSolidarios_PageIndexChanging: " + ex.Message);
			}
		}

		protected void grdAvales_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				this.grdAvales.DataSource = this.AvalesSeleccionados;
				this.grdAvales.PageIndex = e.NewPageIndex;
				this.grdAvales.DataBind();
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al cambiar la página de los avales", ETipoMensajeIU.ERROR, NombreClase + ".grdAvaless_PageIndexChanging: " + ex.Message);
			}
		}
		
		protected void grdAvales_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
				int index = Convert.ToInt32(e.CommandArgument);

				switch (e.CommandName.ToUpper().Trim())
				{
					case "CMDELIMINAR":
						this.Presentador.QuitarAval(index);
						break;
					case "CMDDETALLE":
						this.Presentador.PrepararVisualizacionRepresentantesAval(index);
						break;
				}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al ejecutar la acción sobre el aval", ETipoMensajeIU.ERROR, NombreClase + ".grdAvales_RowCommand: " + ex.Message);
			}
		}
		
		protected void grdAvales_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType != DataControlRowType.DataRow) return;
				AvalBO bo = e.Row.DataItem != null ? (AvalBO)e.Row.DataItem : new AvalProxyBO();
				if (!bo.TipoAval.HasValue)
					e.Row.FindControl("ibtDetalle").Visible = false;
				else if (bo.TipoAval == ETipoAval.Fisico)
					e.Row.FindControl("ibtDetalle").Visible = false;
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al desplegar los avales", ETipoMensajeIU.ERROR, NombreClase + ".grdAvales_RowDataBound: " + ex.Message);
			}
		}

		protected void chkRepresentanteAval_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				//Se obtiene el id de los controles
				CheckBox chk = (CheckBox)sender;
				GridViewRow row = (GridViewRow)chk.Parent.Parent;
				Label lbl = (Label)row.FindControl("lblRepresentanteAvalID");

				int id;
				if (Int32.TryParse(lbl.Text, out id))
				{
					this.hdnRepresentanteAvalSeleccionadoID.Value = id.ToString();

					if (chk.Checked)
						this.Presentador.AgregarRepresentanteAval();
					else
						this.Presentador.QuitarRepresentanteAval();

					this.hdnRepresentanteAvalSeleccionadoID.Value = string.Empty;
				}
				else
					throw new Exception("No se encontró el ID del representante legal del aval o tiene un dato inválido.");
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencias al seleccionar el representante legal para el aval", ETipoMensajeIU.ERROR, NombreClase + ".chkRepresentanteAval_CheckedChanged: " + ex.Message);
			}
		}
        #endregion

        #region Metodos

		private List<RepresentanteLegalBO> ObtenerRepresentantesObligadoSeleccionados()
		{
			List<RepresentanteLegalBO> lista = new List<RepresentanteLegalBO>();
			if (RepresentantesObligado != null)
			{
				foreach (RepresentanteLegalBO rep in RepresentantesObligado)
				{
					if (rep.Activo.HasValue)
						if (rep.Activo.Value)
							lista.Add(rep);
				}
			}

			return lista;
		}

        #region Métodos para el Buscador

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = Presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", ClientID + "_Buscar('" + ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            Presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }

        #endregion

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof (Page), key, script, true);
        }

        /// <summary>
        /// Habilita el Listado de Obligados Solidarios
        /// </summary>
        /// <param name="habilitar">Indica si habilita el Listado</param>
        public void HabilitarListadoObligadosSolidarios(bool habilitar)
        {
            ddlObligadosSolidarios.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita la funcionalidad agregar de Obligados Solidarios
        /// </summary>
        /// <param name="habilitar">Indica si habilita la funcionalidad</param>
        public void HabilitarAgregarObligadoSolidario(bool habilitar)
        {
            btnAgregarObligadoSolidario.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita el Listado de Representantes Legales
        /// </summary>
        /// <param name="habilitar">Indica si habilita el Listado</param>
        public void HabilitarListadoRepresentantesLegales(bool habilitar)
        {
            ddlRepresentantesLegales.Enabled = habilitar;
        }

        /// <summary>
        /// Habilita la funcionalidad agregar de Representantes Legales
        /// </summary>
        /// <param name="habilitar">Indica si habilita la funcionalidad</param>
        public void HabilitarAgregarRepresentanteLegal(bool habilitar)
        {
            btnAgregarRepresentante.Enabled = habilitar;
        }
        /// <summary>
        /// Habilita la funcionalidad de buscar las direcciones del cliente
        /// </summary>
        public void HabilitarConsultaDireccionCliente(bool habilitar)
        {
                ibtnBuscarDirieccionCliente.Enabled = habilitar;
                ibtnBuscarDirieccionCliente.Visible = habilitar;
        }
        /// <summary>
        /// Configura la interfaz en modo consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {
            ibtnBuscarCliente.Visible = false;
            ibtnBuscarCliente.Enabled = false;
            txtNombreCliente.Enabled = false;
            txtNumeroCuentaOracle.Enabled = false;
            ibtnBuscarDirieccionCliente.Visible = false;
            ibtnBuscarDirieccionCliente.Enabled = false;
            txtDomicilioCliente.Enabled = false;
            btnAgregarObligadoSolidario.Visible = false;
            ddlObligadosSolidarios.Visible = false;
            btnAgregarRepresentante.Visible = false;
            ddlRepresentantesLegales.Visible = false;
            grdObligadosSolidarios.Columns[grdObligadosSolidarios.Columns.Count - 2].Visible = false;
            grdRepresentantesLegales.Columns[grdRepresentantesLegales.Columns.Count - 1].Visible = false;
            hdnModo.Value = false.ToString().ToLower();
            cbSoloRepresentantes.Enabled = false;
	        cbObligadosComoAvales.Enabled = false;
	        btnAgregarAval.Visible = false;
			grdAvales.Columns[grdAvales.Columns.Count - 2].Visible = false;
	        ddlAvales.Visible = false;
        }

        /// <summary>
        /// Configura la interfaz en modo Editar
        /// </summary>
        public void ConfigurarModoEditar()
        {
            ibtnBuscarCliente.Visible = true;
            ibtnBuscarCliente.Enabled = true;
            txtNombreCliente.Enabled = true;
            txtNumeroCuentaOracle.Enabled = false;
            btnAgregarObligadoSolidario.Visible = true;
            ddlObligadosSolidarios.Visible = true;
            btnAgregarRepresentante.Visible = true;
            ddlRepresentantesLegales.Visible = true;
            grdObligadosSolidarios.Columns[grdObligadosSolidarios.Columns.Count - 1].Visible = true;
            grdRepresentantesLegales.Columns[grdRepresentantesLegales.Columns.Count - 1].Visible = true;
            hdnModo.Value = true.ToString().ToLower();
            if(EsFisico != true)
            cbSoloRepresentantes.Enabled = true;
			cbObligadosComoAvales.Enabled = true;
			btnAgregarAval.Visible = true;
			grdAvales.Columns[grdAvales.Columns.Count - 2].Visible = true;
			ddlAvales.Visible = true;
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site) Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Limpia los datos de la sesion
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("RepresentantesLegalesContrato");
            Session.Remove("ObligadosSolidariosContrato");
            Session.Remove("ListaRepresentantesObligadosSolidario");
			Session.Remove("AvalesContratoFSL");
			Session.Remove("ListadoAvalesFSL");
			Session.Remove("RepresentantesAvalFSL");
			Session.Remove("ListaRepresentantesAvalFSL");
        }

	    public void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes)
	    {
			grdRepresentantesObligados.DataSource = representantes;
            RepresentantesObligadosSolidario = representantes;//SC0005
			grdRepresentantesObligados.DataBind();
	    }

	    public void MostrarRepresentantesObligados(bool mostrar)
	    {
		    tdGridRepresentantes.Visible = mostrar;
		    tdRepLegalObligado.Visible = mostrar;
        }

        #region SC0007
        /// <summary>
        /// Habilita o inhabilitar el campo de solo representantes legales
        /// </summary>
        /// <param name="habilitar"></param>
        public void HabilitarSoloRepresentantes(bool habilitar)
        {
            cbSoloRepresentantes.Enabled = habilitar;
        }

        /// <summary>
        /// Configura la UI para Clientes Persona Moral
        /// </summary>
        public void ConfigurarClienteMoral()
        {
            HabilitarAgregarRepresentanteLegal(true);
            HabilitarListadoRepresentantesLegales(true);
            HabilitarSoloRepresentantes(true);
            trRepresentantes1.Visible = true;
            trRepresentantes2.Visible = true;
            trRepresentantes3.Visible = true;
            grdRepresentantesLegales.Visible = true;
            lblMensajeOblS.Visible = true;
        }

        /// <summary>
        /// Configura la UI para Clientes Persona Fisica
        /// </summary>
        public void ConfigurarClienteFisico()
        {
            HabilitarAgregarRepresentanteLegal(false);
            HabilitarListadoRepresentantesLegales(false);
            HabilitarSoloRepresentantes(false);
            trRepresentantes1.Visible = false;
            trRepresentantes2.Visible = false;
            trRepresentantes3.Visible = false;
            grdRepresentantesLegales.Visible = false;
            lblMensajeOblS.Visible = false;
        }

        /// <summary>
        /// Muestra u Oculta los controles de los obligados solidario
        /// </summary>
        /// <param name="p">indica si e</param>
        public void MostrarObligadosSolidarios(bool p)
        {
            tblObligadosSolidarios.Visible = p;
            grdObligadosSolidarios.Visible = p;
        }
        #endregion SC0007

		public void ActualizarAvales()
		{
			this.grdAvales.DataSource = this.AvalesSeleccionados;
			this.grdAvales.DataBind();
		}

		public void MostrarAvales(bool mostrar)
		{
			this.pnlAvales.Visible = mostrar;
		}

		public void PermitirAgregarAvales(bool permitir)
		{
			this.btnAgregarAval.Enabled = permitir;
		}

		public void PermitirSeleccionarAvales(bool permitir)
		{
			this.ddlAvales.Enabled = permitir;
		}
		public void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes)
		{
			this.grdRepresentantesDialog.DataSource = representantes;
			this.grdRepresentantesDialog.DataBind();

			this.RegistrarScript("DetalleObligado", "abrirDetalleRepresentantes('AVAL');");
		}

		public void MostrarRepresentantesAval(bool mostrar)
		{
			this.tdGridRepresentantesAvales.Visible = mostrar;
			this.tdRepLegalAval.Visible = mostrar;
		}

		public void HabilitarObligadosComoAvales(bool habilitar)
		{
			cbObligadosComoAvales.Enabled = habilitar;
		}
        #endregion Metodos
    }
}