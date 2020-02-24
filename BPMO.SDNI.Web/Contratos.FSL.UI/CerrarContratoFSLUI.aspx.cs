//Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI {
	public partial class CerrarContratoFSLUI : System.Web.UI.Page, ICerrarContratoFSLVIS {

		#region Atributos

		private const string NombreClase = "CerrarContratoFSLUI";
		private CerrarContratoFSLPRE presentador;

		#endregion

		#region Constructores
		protected void Page_Load(object sender, EventArgs e) {
			try {
				ucInformacionGeneral.Presentador = new ucInformacionGeneralPRE(ucInformacionGeneral);
				ucInformacionPago.Presentador = new ucInformacionPagoPRE(ucInformacionPago);
				ucClienteContrato.Presentador = new ucClienteContratoPRE(ucClienteContrato);
				ucDatosRenta.Presentador = new ucDatosRentaPRE(ucDatosRenta);
				ucLineaContrato.Presentador = new ucLineaContratoFSLPRE(ucLineaContrato);
				ucHerramientas.Presentador = new ucHerramientasFSLPRE(ucHerramientas);
				ucFinalizacionContratoFSL.Presentador=new ucFinalizacionContratoFSLPRE(ucFinalizacionContratoFSL);
				presentador = new CerrarContratoFSLPRE(this, ucHerramientas.Presentador, ucInformacionGeneral.Presentador, ucClienteContrato.Presentador, ucDatosRenta.Presentador, ucInformacionPago.Presentador, ucLineaContrato.Presentador,ucFinalizacionContratoFSL.Presentador);

				if (!Page.IsPostBack) {					
					//Se valida el acceso a la vista
					presentador.ValidarAcceso();

					presentador.Inicializar();
				}
				
				ucDatosRenta.VerDetalleLineaContrato = VerDetalleLineaContrato_Click;
				ucLineaContrato.CancelarClick = CancelarLineaContrato_Click;
			} catch (Exception ex) {

				MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
									NombreClase + ".Page_Load: " + ex.Message);
			}
		}
		#endregion

		#region Propiedades
		/// <summary>
		/// Codigo del Paquete de Navegacion
		/// </summary>
		public string Codigo {
			get { return "ContratoFSLBO"; }
		}
		/// <summary>
		/// Codigo del Paquete de Navegacion del Contrato a Editar
		/// </summary>
		public string CodigoUltimoObjeto {
			get { return hdnCodigoUltimoContrato.Value; }
			set { hdnCodigoUltimoContrato.Value = value ?? string.Empty; }
		}
		/// <summary>
		/// Ultimo 
		/// </summary>
		public ContratoFSLBO UltimoObjeto {
			get { return Session[CodigoUltimoObjeto] as ContratoFSLBO; }
			set { Session[CodigoUltimoObjeto] = value; }
		}
		/// <summary>
		/// Identificador del Contrato
		/// </summary>
		public int? ContratoID {
			get {
				if (!string.IsNullOrEmpty(hdnContratoID.Value))
					return int.Parse(hdnContratoID.Value);
				return null;
			}
			set { hdnContratoID.Value = value != null ? value.ToString() : string.Empty; }
		}
		/// <summary>
		/// Estatus del Contrato
		/// </summary>
		public EEstatusContrato? Estatus {
			get {
				if (!string.IsNullOrEmpty(hdnEstatusContrato.Value))
					return (EEstatusContrato)int.Parse(hdnEstatusContrato.Value);
				return null;
			}
			set {
				hdnEstatusContrato.Value = value != null
											   ? Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture)
											   : string.Empty;
			}
		}
		/// <summary>
		/// Usuario de Ultima Modificacion
		/// </summary>
		public int? UUA {
			get {
				int? id = null;
				Site masterMsj = (Site)Page.Master;

				if (masterMsj != null && masterMsj.Usuario != null)
					id = masterMsj.Usuario.Id;
				return id;
			}
		}
		/// <summary>
		/// Fecha de Ultima Modificacion
		/// </summary>
		public DateTime? FUA {
			get { return DateTime.Now; }
		}
		/// <summary>
		/// Identificador de la Unidad Operativa del Contrato
		/// </summary>
		public int? UnidadOperativaContratoID {
			get {
				if (!string.IsNullOrEmpty(hdnUnidadOperativaContratoID.Value))
					return int.Parse(hdnUnidadOperativaContratoID.Value);
				return null;
			}
			set {
				if (value != null)
					hdnUnidadOperativaContratoID.Value = value.ToString();
				else {
					hdnUnidadOperativaContratoID.Value = string.Empty;
				}
			}
		}


		/// <summary>
		/// Devuelve el identificador del usuario que ha iniciado sesión en el sistema
		/// </summary>
		public int? UsuarioID {
			get {
				var master = (Site)Page.Master;
				if (master != null)
					if (master.Usuario != null && master.Usuario.Usuario != null)
						if (master.Usuario.Id.HasValue)
							return master.Usuario.Id.Value;
				return null;
			}
		}
		/// <summary>
		/// Devuelve el identificador de la unidad de adscripción desde la cual se ha iniciado sesión en el sistema
		/// </summary>
		public int? UnidadAdscripcionID {
			get {
				var master = (Site)Page.Master;
				if (master != null)
					if (master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null)
						if (master.Adscripcion.UnidadOperativa.Id.HasValue)
							return master.Adscripcion.UnidadOperativa.Id.Value;
				return null;
			}
		}

		public CierreAnticipadoContratoFSLBO DatosCierre{get { return ucFinalizacionContratoFSL.Presentador.InterfazUsuarioADatos(); }
		}
		#endregion

		#region Metodos
		public void LimpiarSesion() {
			Session.Remove(CodigoUltimoObjeto);
			ucLineaContrato.LimpiarSesion();
			ucClienteContrato.LimpiarSesion();
			ucDatosRenta.LimpiarSesion();
		}

		/// <summary>
		/// Regresa al Modulo de Consulta
		/// </summary>
		public void RegresarAConsultar() {
			ucLineaContrato.LimpiarSesion();
			Response.Redirect("ConsultarContratosFSLUI.aspx");
		}

		/// <summary>
		/// Este método despliega un mensaje en pantalla
		/// </summary>
		/// <param name="mensaje">Mensaje a desplegar</param>
		/// <param name="tipo">Tipo de mensaje a desplegar</param>
		/// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
		public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
			var master = (Site)Master;
			if (tipo == ETipoMensajeIU.ERROR) {
				if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
			} else {
				if (master != null) master.MostrarMensaje(mensaje, tipo);
			}
		}

		/// <summary>
		/// Redirecciona al Detalle del Contrato
		/// </summary>
		public void IrADetalleContrato() {
			Response.Redirect("DetalleContratoFSLUI.aspx");
		}


		/// <summary>
		/// Establece el Paquete de Navegación para el Detalle del Contrato Seleccionado
		/// </summary>
		/// <param name="Clave">Clave del Paquete</param>
		/// <param name="contrato">Identificador del Contrato Seleccionado</param>
		public void EstablecerPaqueteNavegacion(string Clave, ContratoBO contrato) {
			if (contrato != null && contrato.ContratoID != null) Session[Clave] = contrato;
			else {
				throw new Exception(NombreClase +
									".EstablecerPaqueteNavegacion: El contrato proporcionado no contiene un identificador.");
			}
		}

		public void RedirigirSinPermisoAcceso() {
			this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
		}

		public void PermitirRegistrar(bool status) {
			this.hlRegistroOrden.Enabled = status;
		}

		/// <summary>
		/// Cambia la vista de la interfaz a la información del contrato
		/// </summary>
		public void CambiaAContrato() {
			mvCU026.ActiveViewIndex = 0;
		}

		/// <summary>
		/// Cambia la vista de la interfaz al resumen del contrato
		/// </summary>
		public void CambiarALinea() {
			mvCU026.ActiveViewIndex = 1;
		}


		#endregion

		#region Eventos

		protected void btnCancelar_Click(object sender, EventArgs e)
		{
			try {
				presentador.RegresarADetalles();
			} catch (Exception ex) {
				MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click: " + ex.Message);
			}
		}

		protected void btnGuardar_Click(object sender, EventArgs e)
		{
			try {
				presentador.CerrarContrato();
			} catch (Exception ex) {
				MostrarMensaje("Inconsistencias al cerrar el contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardar_Click: " + ex.Message);
			}
			
		}

		protected void VerDetalleLineaContrato_Click(object sender, CommandEventArgs e) {
			var linea = e.CommandArgument as LineaContratoFSLBO;
			presentador.PrepararLinea(linea);
		}

		protected void CancelarLineaContrato_Click(object sender, EventArgs e) {
			mvCU026.ActiveViewIndex = 0;
		}
		#endregion

	}
}