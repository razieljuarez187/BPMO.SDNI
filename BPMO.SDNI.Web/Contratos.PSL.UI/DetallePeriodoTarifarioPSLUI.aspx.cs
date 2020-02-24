// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class DetallePeriodoTarifarioPSLUI : Page, IDetallePeriodoTarifarioPSLVIS {
        #region Atributos

        private string nombreClase = "DetallePeriodoTarifarioPSLUI";
        private DetallePeriodoTarifarioPSLPRE presentador;
        #endregion

        #region Propiedades
        public int? UnidadOperativaID {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null &&
                    master.Adscripcion.UnidadOperativa.Id != null)
                    id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }

        public int? UnidadOperativaSeleccionadaID {
            get {
                string UnidadOperativa = string.IsNullOrEmpty(this.hdnUnidadOperativaSeleccionadaID.Value) ? this.UnidadOperativaID.ToString() : this.hdnUnidadOperativaSeleccionadaID.Value;
                return Convert.ToInt32(UnidadOperativa);
            }
        }
        public int? UsuarioID {
            get {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Usuario != null && master.Usuario.Id != null)
                    id = master.Usuario.Id;
                return id;
            }
        }

        public UsuarioBO Usuario {
            get { return (UsuarioBO)Session["Usuario"]; }
        }

        public string Estatus {
            set { txtEstatus.Text = value ?? string.Empty; }
        }

        public DateTime? FechaRegistro {
            set { txtFechaRegistro.Text = value.ToString() ?? String.Empty; }
        }

        public DateTime? FechaModificacion {
            set { txtFechaModificacion.Text = value.ToString() ?? String.Empty; }
        }

        public string UsuarioRegistro {
            set { txtUsuarioRegistro.Text = value ?? String.Empty; }
        }

        public string UsuarioModificacion {
            set { txtUsuarioModificacion.Text = value ?? String.Empty; }
        }

        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new DetallePeriodoTarifarioPSLPRE(this, this.ucPeriodoTarifarioPSL);
                if (!IsPostBack) {
                    presentador.ValidarAcceso();
                    presentador.RealizarPrimeraCarga();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al crear la página", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
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

        public void PermitirEditar(bool activo) {
            this.btnEditar.Enabled = activo;
        }

        public void RedirigirSinPermisoAcceso() {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void ModoDetalle(bool activo) {
            txtEstatus.Enabled = !activo;
            txtFechaModificacion.Enabled = !activo;
            txtFechaRegistro.Enabled = !activo;
            txtUsuarioModificacion.Enabled = !activo;
            txtUsuarioRegistro.Enabled = !activo;
        }
        public void LimpiarSesion() {
            if (Session["PeriodoTarifarioPSLBO"] != null)
                Session.Remove("PeriodoTarifarioPSLBO");
        }
        public object ObtenerDatosNavegacion() {
            if (Session["PeriodoTarifarioPSLBO"] != null)
                return (object)Session["PeriodoTarifarioPSLBO"];
            else
                return new DiaPeriodoTarifaBO() { UnidadOperativaID = this.UnidadOperativaID };
        }
        public void EstablecerDatosNavegacion(object tarifa) {
            Session["PeriodoTarifarioPSLBO"] = tarifa;
        }
        public void RedirigirAEditar() {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/EditarPeriodoTarifarioPSLUI.aspx"));
        }
        #endregion

        #region Eventos
        protected void btnEditar_Click(object sender, EventArgs e) {
            try {
                presentador.IrAEditar();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al intentar editar la tarifa", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento que controla la selección del DropDownList de Sector.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidadOperativa_SelectedIndexChanged(object sender, EventArgs e) {
            Session["PeriodoTarifarioPSLBO"] = new DiaPeriodoTarifaBO() { UnidadOperativaID = Convert.ToInt32(ddlUnidadOperativa.SelectedValue) };
            this.hdnUnidadOperativaSeleccionadaID.Value = ddlUnidadOperativa.SelectedValue;
            this.presentador.RealizarPrimeraCarga();
        }
        #endregion
    }
}