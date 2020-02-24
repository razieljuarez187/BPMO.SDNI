// Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI {
    public partial class ucDatosObligadoSolidarioUI : System.Web.UI.UserControl, IucDatosObligadoSolidarioVIS {
        #region Atributos

        private string nombreClase = "DatosObligadoSolidarioUI";
        private ucDatosObligadoSolidarioPRE presentador;
        private List<CatalogoBaseBO> listaAcciones;
        private int? unidadOperativa;
        #endregion

        #region Propiedades

        public List<CatalogoBaseBO> ListaAcciones {
            get { return listaAcciones; }
            set { this.listaAcciones = value; }
        }

        public int? UnidadOperativaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
            set {
                this.unidadOperativa = value;
            }
        }

        public ActaConstitutivaBO ActaConstitutiva {
            get { return ucDatosActaConstitutivaUI.ObtenerActaConstitutiva(); }
            set { ucDatosActaConstitutivaUI.MostrarDatosActaConstitutiva(value); }
        }

        public string Direccion {
            get { return (String.IsNullOrEmpty(this.txtDireccion.Text.Trim())) ? null : this.txtDireccion.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtDireccion.Text = value;
                else
                    this.txtDireccion.Text = string.Empty;
            }
        }

        public EventHandler hdlAgregarRepresentante { get; set; }

        public string Nombre {
            get { return (String.IsNullOrEmpty(this.txtNombre.Text.Trim())) ? null : this.txtNombre.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtNombre.Text = value;
                else
                    this.txtNombre.Text = string.Empty;
            }
        }

        public int? ObligadoID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnObligadoID.Value.Trim()))
                    id = int.Parse(this.hdnObligadoID.Value.Trim());

                return id;
            }
            set {
                this.hdnObligadoID.Value = value != null ? value.ToString() : string.Empty;
            }
        }

        public List<RepresentanteLegalBO> RepresentantesInactivos {
            get {
                if (Session["ListaRepInactivos"] == null) return new List<RepresentanteLegalBO>();
                return Session["ListaRepInactivos"] as List<RepresentanteLegalBO>;
            }
            set {
                Session["ListaRepInactivos"] = value;
            }
        }

        public List<RepresentanteLegalBO> RepresentantesLegales {
            get {
                if (Session["ListaRepLegalesObSolidarios"] == null)
                    return new List<RepresentanteLegalBO>();
                return (List<RepresentanteLegalBO>)Session["ListaRepLegalesObSolidarios"];
            }
            set {
                Session["ListaRepLegalesObSolidarios"] = value;
            }
        }

        public string Telefono {
            get { return (String.IsNullOrEmpty(this.txtTelefono.Text.Trim())) ? null : this.txtTelefono.Text; }
            set {
                if (value != null)
                    this.txtTelefono.Text = value;
                else
                    this.txtTelefono.Text = string.Empty;
            }
        }

        public ETipoObligadoSolidario? TipoObligadoSolidario {
            get {
                if (string.IsNullOrEmpty(ddlTipoObligado.SelectedValue)) return null;
                return (ETipoObligadoSolidario)Convert.ToInt32(ddlTipoObligado.SelectedValue);
            }
            set {

                if (value == null) ddlTipoObligado.SelectedValue = "";
                else ddlTipoObligado.SelectedValue = ((int)value).ToString();
            }
        }

        public string RFC {
            get { return this.txtRFC.Text; }
            set { this.txtRFC.Text = value; }
        }

        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                ddlTipoObligado.Items.Add(new ListItem("Seleccione una opción", ""));
                foreach (int valor in Enum.GetValues(typeof(ETipoObligadoSolidario))) {
                    ddlTipoObligado.Items.Add(new ListItem(Enum.GetName(typeof(ETipoObligadoSolidario), valor), valor.ToString()));
                }
            }
            presentador = new ucDatosObligadoSolidarioPRE(this);

            if (hdnConfirmarEliminar.Value == "1") {
                if (ddlTipoObligado.SelectedValue == "")
                    presentador.PrepararNuevo();
                if (ddlTipoObligado.SelectedValue == ((int)ETipoObligadoSolidario.Fisico).ToString())
                    presentador.MostrarDatos(new ObligadoSolidarioFisicoBO { DireccionPersona = new DireccionPersonaBO() });
                if (ddlTipoObligado.SelectedValue == ((int)ETipoObligadoSolidario.Moral).ToString())
                    presentador.MostrarDatos(new ObligadoSolidarioMoralBO { DireccionPersona = new DireccionPersonaBO(), ActaConstitutiva = new ActaConstitutivaBO() });
                presentador.EliminarRepresentantes();
                hdnConfirmarEliminar.Value = "0";
            }
        }
        #endregion

        #region Metodos

        public void ActualizarRepresentantesLegales() {
            grdRepresentantesLegales.DataSource = RepresentantesLegales.Count > 0 ? (from representante in RepresentantesLegales where representante.Activo == true select representante).ToList() : RepresentantesLegales;
            if (this.UnidadOperativaId == (int)ETipoEmpresa.Construccion || this.UnidadOperativaId == (int)ETipoEmpresa.Generacion || this.UnidadOperativaId == (int)ETipoEmpresa.Equinova)
                grdRepresentantesLegales.Columns[2].Visible = false;

            grdRepresentantesLegales.DataBind();
        }

        public void HabilitarCampos(bool habilitar) {
            txtDireccion.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            ddlTipoObligado.Enabled = habilitar;
            btnAgregarRepresentanteObligado.Enabled = habilitar;
            grdRepresentantesLegales.Enabled = habilitar;
            txtRFC.Enabled = habilitar;
        }

        public void ModoCreacion() {
            ddlTipoObligado.Enabled = true;
            RepresentantesLegales = null;
            ActualizarRepresentantesLegales();
        }

        public void ModoEdicion() {
            ddlTipoObligado.Enabled = false;
        }

        public void MostrarDatos(ObligadoSolidarioBO obligado) {
            this.presentador.MostrarDatos(obligado);
        }

        public ObligadoSolidarioBO ObtenerDatos() {
            if (this.ValidarDatos() == null)
                return this.presentador.ObtenerDatos();

            return null;
        }

        public void PrepararNuevo() {
            this.presentador.PrepararNuevo();
        }

        public string ValidarDatos() {
            return this.presentador.ValidarDatos();
        }

        #endregion Metodos

        #region Eventos

        protected void btnAgregarRepresentanteObligado_Click(object sender, EventArgs e) {
            if (hdlAgregarRepresentante != null)
                hdlAgregarRepresentante.Invoke(sender, e);
        }

        protected void grdRepresentantesLegales_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            grdRepresentantesLegales.PageIndex = e.NewPageIndex;
            ActualizarRepresentantesLegales();
        }

        protected void grdRepresentantesLegales_RowCommand(object sender, GridViewCommandEventArgs e) {
            string eCommandNameUpper = e.CommandName.ToUpper();
            if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
            int index = Convert.ToInt32(e.CommandArgument);
            RepresentanteLegalBO representante = this.RepresentantesLegales[index];

            presentador.QuitarRepresentanteLegal(representante);
            ActualizarRepresentantesLegales();
        }

        #region SC0007

        public string ValidarActaConstitutiva(bool? validarEscritura = true, bool? obligadoSolidario = false) {
            return ucDatosActaConstitutivaUI.ValidarCampos(validarEscritura, obligadoSolidario);
        }

        #endregion SC0007

        public void EstablecerAcciones(bool mostrar) {
            if (this.UnidadOperativaId == (int)ETipoEmpresa.Idealease)
                this.trRfc.Visible = false;
            if (this.UnidadOperativaId != (int)ETipoEmpresa.Idealease)
                this.ucDatosActaConstitutivaUI.HabilitarCampos(mostrar);
        }

        #endregion
    }
}