// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class EditarPeriodoTarifarioPSLUI : System.Web.UI.Page, IEditarPeriodoTarifarioPSLVIS {
        #region Atributos
        private string NombreClase = "EditarPeriodoTarifarioPSLUI";
        private EditarPeriodoTarifarioPSLPRE presentador;

        #endregion

        #region Propiedades

        public int? UnidadOperativaID {
            get {
                Site master = (Site)Page.Master;
                return master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null
                        ? master.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }

        public int? UnidadOperativaSeleccionada {
            get {
                int? id = null;
                if (this.ddlUnidadOperativa.SelectedValue != "-1")
                    id = int.Parse(ddlUnidadOperativa.SelectedValue.Trim());
                return id;
            }
            set {
                this.hdnUnidadOperativaSeleccionadaID.Value = value.ToString();
            }
        }

        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }

        /// <summary>
        /// Ultimo objeto al que se ha hecho referencia
        /// </summary>
        public object UltimoObjeto {
            get {
                if (Session["UltimoObjetoPeriodoTarifario"] != null)
                    return Session["UltimoObjetoPeriodoTarifario"];

                return null;
            }
            set {
                Session["UltimoObjetoPeriodoTarifario"] = value;
            }
        }

        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }

        public int? UnidadOperativaSeleccionadaID {
            get {
                string UnidadOperativa = string.IsNullOrEmpty(this.hdnUnidadOperativaSeleccionadaID.Value) ? this.UnidadOperativaID.ToString() : this.hdnUnidadOperativaSeleccionadaID.Value;
                return Convert.ToInt32(UnidadOperativa);
            }
            set {
                this.hdnUnidadOperativaSeleccionadaID.Value = value.ToString();
            }
        }

        public UsuarioBO Usuario {
            get { return (UsuarioBO)Session["Usuario"]; }
        }

        /// <summary>
        /// Obtiene o estabece la fecha de creación del contrato
        /// </summary>
        public DateTime? FC {
            get {
                if (!string.IsNullOrEmpty(this.hdnFC.Value) && !string.IsNullOrWhiteSpace(this.hdnFC.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFC.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFC.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA {
            get {
                if (!string.IsNullOrEmpty(this.hdnFUA.Value) && !string.IsNullOrWhiteSpace(this.hdnFUA.Value)) {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFUA.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFUA.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el usuario que crea el contrato
        /// </summary>
        public int? UC {
            get {
                if (!string.IsNullOrEmpty(this.hdnUC.Value) && !string.IsNullOrWhiteSpace(this.hdnUC.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUC.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUC.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o establece el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA {
            get {
                if (!string.IsNullOrEmpty(this.hdnUUA.Value) && !string.IsNullOrWhiteSpace(this.hdnUUA.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnUUA.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUUA.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new EditarPeriodoTarifarioPSLPRE(this, this.ucTarifaPSL);

                if (!this.IsPostBack) {
                    this.presentador.ValidarAcceso();
                    this.presentador.RealizarPrimeraCarga();

                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al crear la página:", ETipoMensajeIU.ERROR, this.NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void LimpiarSesion() {
            if (Session["PeriodoTarifarioPSLBO"] != null)
                Session.Remove("PeriodoTarifarioPSLBO");
        }

        public object ObtenerDatosNavegacion() {
            return (object)Session["PeriodoTarifarioPSLBO"];
        }

        /// <summary>
        /// Método encargado de realizar el llenado del combo con los valores de los períodos de las tarifas
        /// </summary>
        public void EstablecerOpcionesUnidadesOperativas(Dictionary<int, string> unidadesOperativas) {
            this.ddlUnidadOperativa.Items.Clear();
            this.ddlUnidadOperativa.DataSource = unidadesOperativas;
            this.ddlUnidadOperativa.DataTextField = "Value";
            this.ddlUnidadOperativa.DataValueField = "Key";
            this.ddlUnidadOperativa.DataBind();
            this.ddlUnidadOperativa.SelectedValue = this.UnidadOperativaSeleccionadaID.ToString();
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        public void ModoEdicion(bool activo) {


        }

        public void ModoConsulta(bool activo) {
            this.btnCancelar.Enabled = !activo;
            this.btnGuardar.Enabled = !activo;
            this.ddlUnidadOperativa.Enabled = false;
        }

        public void RedirigirADetalle() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetallePeriodoTarifarioPSLUI.aspx"));
        }

        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void EstablecerPaqueteNavegacion(object tarifa) {
            Session["PeriodoTarifarioPSLBO"] = tarifa;
        }

        #region Métodos para el Buscador
        /// <summary>
        /// metodo para registrar script en el cliente
        /// </summary>
        /// <param name="key"> llave del script que se va a registrar</param>
        /// <param name="script">script que se va a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                this.presentador.Cancelar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al Cancelar el Registro de la Tarifa", ETipoMensajeIU.ERROR, this.NombreClase + ".btnCancelar_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                this.presentador.Guardar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al Guardar la Tarifa", ETipoMensajeIU.ERROR, this.NombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e) {
            try {
                this.presentador.AgregarTurnoTarifa();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar la sucursal", ETipoMensajeIU.ERROR, this.NombreClase + ".btnAgregar_Click:" + ex.Message);
            }
        }
        #endregion
    }
}