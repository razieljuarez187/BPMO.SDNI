// Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class ucLlantaUI : System.Web.UI.UserControl, IucLlantaVIS
	{
		#region Atributos
        private ucLlantaPRE presentador;
		private string nombreClase = "ucLlantaUI";
        
        public enum ECatalogoBuscador
        {
            Llanta = 1,
            Sucursal = 2
        }
		#endregion Atributos

        #region Propiedades
        //Manejo del buscador en la página principal
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
                    objeto = (Session[nombreSession] as object);

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
                    objeto = (Session[ViewState_Guid] as object);

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
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }

        public int? LlantaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnLlantaId.Value))
                    id = int.Parse(this.hdnLlantaId.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnLlantaId.Value = value.ToString();
                else
                    this.hdnLlantaId.Value = string.Empty;
            }
        }
        public string Codigo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCodigo.Text)) ? null : this.txtCodigo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtCodigo.Text = value;
                else
                    this.txtCodigo.Text = string.Empty;
            }
        }
        public string Marca
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMarca.Text)) ? null : this.txtMarca.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMarca.Text = value;
                else
                    this.txtMarca.Text = string.Empty;
            }
        }
        public string Modelo
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtModelo.Text)) ? null : this.txtModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }
        public string Medida
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMedida.Text)) ? null : this.txtMedida.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtMedida.Text = value;
                else
                    this.txtMedida.Text = string.Empty;
            }
        }
        public decimal? Profundidad
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtProfundidad.Text))
                    temp = Decimal.Parse(this.txtProfundidad.Text.Trim().Replace(",","")); //RI0012
                return temp;
            }
            set
            {
                if (value != null)
					this.txtProfundidad.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtProfundidad.Text = string.Empty;
            }
        }
        public bool? Revitalizada
        {
            get
            {
                if (rbLlantaRevitalizada.SelectedValue == "") return null;
                return rbLlantaRevitalizada.SelectedValue == "1" ? true : false;
            }
            set
            {
                if (value == null) return;
                rbLlantaRevitalizada.SelectedValue = value == true ? "1" : "0";
            }
        }
        public bool? Stock
        {
            get
            {
                if (rbLlantaStock.SelectedValue == "") return null;
                return rbLlantaStock.SelectedValue == "1" ? true : false;
            }
            set
            {
                if (value == null) return;
                rbLlantaStock.SelectedValue = value == true ? "1" : "0";
            }
        }
        public int? EnllantableID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEnllantableId.Value))
                    id = int.Parse(this.hdnEnllantableId.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEnllantableId.Value = value.ToString();
                else
                    this.hdnEnllantableId.Value = string.Empty;
            }
        }
        public int? SucursalEnllantableID {
            get {
                int val;
                if (!string.IsNullOrEmpty(this.hdnSucursalEnllantableId.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalEnllantableId.Value))
                    if (Int32.TryParse(this.hdnSucursalEnllantableId.Value, out val))
                        return val;
                return null;
            }
            set {
                if (value != null)
                    this.hdnSucursalEnllantableId.Value = value.Value.ToString();
                else
                    this.hdnSucursalEnllantableId.Value = string.Empty;
            }
        }
        public string DescripcionEnllantable
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtDescripcionEnllantable.Text)) ? null : this.txtDescripcionEnllantable.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtDescripcionEnllantable.Text = value;
                else
                    this.txtDescripcionEnllantable.Text = string.Empty;
            }
        }
        public int? TipoEnllantable
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTipoEnllantable.Value))
                    id = int.Parse(this.hdnTipoEnllantable.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTipoEnllantable.Value = value.ToString();
                else
                    this.hdnTipoEnllantable.Value = string.Empty;
            }
        }
        public int? Posicion
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtPosicion.Text))
                    id = int.Parse(this.txtPosicion.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtPosicion.Text = value.ToString();
                else
                    this.txtPosicion.Text = string.Empty;
            }
        }
        public bool? EsRefaccion
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEsRefaccion.Value))
                    id = bool.Parse(this.hdnEsRefaccion.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEsRefaccion.Value = value.ToString();
                else
                    this.hdnEsRefaccion.Value = string.Empty;
            }
        }

        public List<ArchivoBO> ArchivosAdjuntos
        {
            get { return this.ucCatalogoDocumentos.NuevosArchivos; }
            set { this.ucCatalogoDocumentos.NuevosArchivos = value; }
        }
        
        public int? UC
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }
        public int? UUA
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUUA.Value))
                    id = int.Parse(this.hdnUUA.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUUA.Value = value.ToString();
                else
                    this.hdnUUA.Value = string.Empty;
            }
        }
        public string UsuarioCreacion
        {
            set
            {
                if (value != null)
                    this.txtUsuarioRegistro.Text = value;
                else
                    this.txtUsuarioRegistro.Text = string.Empty;
            }
        }
        public string UsuarioEdicion
        {
            set
            {
                if (value != null)
                    this.txtUsuarioEdicion.Text = value;
                else
                    this.txtUsuarioEdicion.Text = string.Empty;
            }
        }
		public DateTime? FC
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaRegistro.Text))
                    temp = DateTime.Parse(this.txtFechaRegistro.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaRegistro.Text = value.Value.ToString();
                else
                    this.txtFechaRegistro.Text = string.Empty;
            }
		}
		public DateTime? FUA
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaEdicion.Text))
                    temp = DateTime.Parse(this.txtFechaEdicion.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaEdicion.Text = value.Value.ToString();
                else
                    this.txtFechaEdicion.Text = string.Empty;
            }
        }

        public bool? Activo
        {
            get
            {
                if (rbLlantaActiva.SelectedValue == "") return null;
                return rbLlantaActiva.SelectedValue == "1";
            }
            set
            {
                if (value == null) return;
                rbLlantaActiva.SelectedValue = value == true ? "1" : "0";
            }
        }

        public bool BuscarSoloActivos
        {
            get
            {
                bool id = false;
                if (!String.IsNullOrEmpty(this.hdnBuscarSoloActivos.Value))
                    id = bool.Parse(this.hdnBuscarSoloActivos.Value.Trim());
                return id;
            }
        }
        public bool BuscarSoloStock
        {
            get
            {
                bool id = false;
                if (!String.IsNullOrEmpty(this.hdnBuscarSoloStock.Value))
                    id = bool.Parse(this.hdnBuscarSoloStock.Value.Trim());
                return id;
            }
        }

        public IucCatalogoDocumentosVIS VistaDocumentos
        {
            get 
            { 
                return this.ucCatalogoDocumentos; 
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
        public int? UnidadOperativaID {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? UsuarioAutenticado {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        #endregion

        #region Constructores
        public ucLlantaUI()
		{
			presentador = new ucLlantaPRE(this);
		}
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucLlantaPRE(this);
            ucCatalogoDocumentos.InicializarControl(null, null);
        }
		#endregion

		#region Métodos
        public void PrepararNuevo()
        {
            this.txtCodigo.Text = "";
            this.txtSucursal.Text = "";
            this.txtMarca.Text = "";
            this.txtMedida.Text = "";
            this.txtModelo.Text = "";
            this.txtProfundidad.Text = "";
            this.txtDescripcionEnllantable.Text = "";
            this.txtPosicion.Text = "";
            this.txtUsuarioRegistro.Text = "";
            this.txtUsuarioEdicion.Text = "";
            this.txtFechaEdicion.Text = "";
            this.txtFechaRegistro.Text = "";

            this.rbLlantaActiva.SelectedIndex = -1;
            this.rbLlantaRevitalizada.SelectedIndex = -1;
            this.rbLlantaStock.SelectedIndex = -1;

            this.hdnLlantaId.Value = "";
            this.hdnSucursalID.Value = "";
            this.hdnUC.Value = "";
            this.hdnUUA.Value = "";
            this.hdnEsRefaccion.Value = "";

            this.txtFechaEdicion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtUsuarioEdicion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.txtDescripcionEnllantable.Enabled = false;
            this.rbLlantaActiva.Enabled = false;
            this.rbLlantaStock.Enabled = false;
        }
        public void PrepararEdicion()
        {
            this.txtFechaEdicion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtUsuarioEdicion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;
            this.txtDescripcionEnllantable.Enabled = false;
            this.rbLlantaActiva.Enabled = false;
            this.rbLlantaStock.Enabled = false;
        }
        public void PrepararVisualizacion()
        {
            this.txtFechaEdicion.Enabled = false;
            this.txtFechaRegistro.Enabled = false;
            this.txtUsuarioEdicion.Enabled = false;
            this.txtUsuarioRegistro.Enabled = false;

            this.txtDescripcionEnllantable.Enabled = false;

            this.rbLlantaActiva.Enabled = false;
            this.rbLlantaStock.Enabled = false;
        }

        public void HabilitarModoEdicion(bool habilitar)
        {
            this.txtCodigo.Enabled = habilitar;            
            this.txtMarca.Enabled = habilitar;
            this.txtModelo.Enabled = habilitar;
            this.txtMedida.Enabled = habilitar;
            this.txtProfundidad.Enabled = habilitar;
            this.txtPosicion.Enabled = habilitar;
            this.rbLlantaRevitalizada.Enabled = habilitar;            
        }
        public void HabilitarProfundidad(bool habilitar)
        {
            this.txtProfundidad.Enabled = habilitar;
        }
        public void HabilitarPosicion(bool habilitar)
        {
            this.txtPosicion.Enabled = habilitar;
        }
        public void HabilitarCodigo(bool habilitar)
        {
            this.txtCodigo.Enabled = habilitar;
        }
        public void HabilitarSucursal(bool habilitar) {
            this.txtSucursal.Enabled = habilitar;
            this.ibtnBuscarSucursal.Visible = habilitar;
        }

        public void PermitirBusquedaCodigo(bool permitir, bool soloActivos, bool soloStock)
        {
            this.ibtnBuscaLlanta.Visible = permitir;
            this.txtCodigo.AutoPostBack = permitir;

            this.hdnBuscarSoloActivos.Value = soloActivos.ToString();
            this.hdnBuscarSoloStock.Value = soloStock.ToString();
        }

        public void MostrarStock(bool mostrar)
        {
            this.trStock.Visible = mostrar;
        }
        public void MostrarEnllantable(bool mostrar)
        {
            this.trDescripcionEnllantable.Visible = mostrar;
        }
        public void MostrarPosicion(bool mostrar)
        {
            this.trPosicion.Visible = mostrar;
        }
        public void MostrarActiva(bool mostrar)
        {
            this.trActiva.Visible = mostrar;
        }
        public void MostrarDatosRegistro(bool mostrar)
        {
            this.trFC.Visible = mostrar;
            this.trUC.Visible = mostrar;
        }
        public void MostrarDatosActualizacion(bool mostrar)
        {
            this.trFUA.Visible = mostrar;
            this.trUUA.Visible = mostrar;
        }

        public void OcultarDocumentos(bool ocultar)
        {
            this.dvDocumentos.Visible = !ocultar;
        }

		public void EstablecerTiposArchivo(List<TipoArchivoBO> tiposArchivo)
		{
			ucCatalogoDocumentos.TiposArchivo = tiposArchivo;
		}        
              


        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                ((HiddenField)this.Parent.FindControl("hdnTipoMensaje")).Value = ((int)tipo).ToString();
                ((HiddenField)this.Parent.FindControl("hdnMensaje")).Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)this.Parent.Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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
        #endregion

        #region Eventos
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Llanta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.LlantaID != null)
                {
                    string codigo = this.Codigo;
                    this.Session_BOSelecto = null;

                    this.DesplegarBOSelecto(ECatalogoBuscador.Llanta);

                    this.Codigo = codigo;
                    if (this.Codigo != null)
                    {
                        this.EjecutaBuscador("Llanta", ECatalogoBuscador.Llanta);
                        this.Codigo = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Llanta", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".txtCodigo_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// <summary>
        /// Buscar Llanta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaCodigo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Llanta&hidden=0", ECatalogoBuscador.Llanta);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Llanta", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".ibtnBuscaCodigo_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Busca sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    case ECatalogoBuscador.Llanta:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }
        #endregion
    }
}