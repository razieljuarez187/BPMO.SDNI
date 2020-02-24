//Satisface al CU068 - Catalogo de Clientes
using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI {
    public partial class ucDatosActaConstitutivaUI : System.Web.UI.UserControl, IucDatosActaConstitutivaVIS {
        #region Atributos

        private ucDatosActaConstitutivaPRE presentador;
        private int? unidadOprativaId;
        private List<CatalogoBaseBO> listAcciones;
        #endregion

        #region Propiedades

        public List<CatalogoBaseBO> ListaAcciones {
            get { return this.listAcciones; }
            set { this.listAcciones = value; }
        }

        public int? ActaId {
            get {
                int id;
                if (int.TryParse(this.hdnActaId.Value, out id))
                    return id;
                else
                    return null;
            }
            set {
                if (value != null)
                    this.hdnActaId.Value = value.ToString();
                else
                    this.hdnActaId.Value = string.Empty;
            }
        }
        public DateTime? FechaEscritura {
            get {
                if (string.IsNullOrEmpty(txtFechaEscrituraCliente.Text.Trim())) return null;
                return Convert.ToDateTime(txtFechaEscrituraCliente.Text.Trim());
            }
            set {
                txtFechaEscrituraCliente.Text = value == null ? "" : ((DateTime)value).ToShortDateString();
            }
        }

        public DateTime? FechaRPPC {
            get {
                if (string.IsNullOrEmpty(txtFechaRPPCCliente.Text.Trim())) return null;
                return Convert.ToDateTime(txtFechaRPPCCliente.Text.Trim());
            }
            set {
                txtFechaRPPCCliente.Text = value == null ? "" : ((DateTime)value).ToShortDateString();
            }
        }

        public string LocalidadNotaria {
            get { return string.IsNullOrEmpty(txtLocalidadNotariaCliente.Text.Trim()) ? null : txtLocalidadNotariaCliente.Text.Trim().ToUpper(); }
            set { txtLocalidadNotariaCliente.Text = value ?? ""; }
        }

        public string LocalidadRPPC {
            get { return string.IsNullOrEmpty(txtLocalidadRPPCCliente.Text.Trim()) ? null : txtLocalidadRPPCCliente.Text.Trim().ToUpper(); }
            set { txtLocalidadRPPCCliente.Text = value ?? ""; }
        }

        public string NombreNotario {
            get { return string.IsNullOrEmpty(txtNombreNotarioCliente.Text.Trim()) ? null : txtNombreNotarioCliente.Text.Trim().ToUpper(); }
            set { txtNombreNotarioCliente.Text = value ?? ""; }
        }

        public string NumeroEscritura {
            get {
                return string.IsNullOrEmpty(txtNumeroEscrituraCliente.Text.Trim()) ? null : txtNumeroEscrituraCliente.Text.Trim().ToUpper();
            }
            set {
                txtNumeroEscrituraCliente.Text = value ?? "";
            }
        }

        public string NumeroNotaria {
            get { return string.IsNullOrEmpty(txtNumeroNotariaCliente.Text.Trim()) ? null : txtNumeroNotariaCliente.Text.Trim().ToUpper(); }
            set { txtNumeroNotariaCliente.Text = value ?? ""; }
        }

        public string NumeroRPPC {
            get { return string.IsNullOrEmpty(txtNumeroFolioCliente.Text.Trim()) ? null : txtNumeroFolioCliente.Text.Trim().ToUpper(); }
            set { txtNumeroFolioCliente.Text = value ?? ""; }
        }
        public bool? Activo {
            get {
                return this.chkActivo.Checked;
            }
            set {
                if (value.HasValue)
                    this.chkActivo.Checked = value.Value;
                else
                    this.chkActivo.Checked = false;
            }
        }

        public int? UnidadOperativaID {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
            set { this.unidadOprativaId = value; }
        }
        #endregion

        #region Constructores

        public ucDatosActaConstitutivaUI() {
            presentador = new ucDatosActaConstitutivaPRE(this);
        }
        protected void Page_Load(object sender, EventArgs e) {
            presentador = new ucDatosActaConstitutivaPRE(this);
        }

        #endregion

        #region Métodos

        public void HabilitarCampos(bool habilitar, bool? veractivo = true) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperativaID;

            switch (empresa) {
                case ETipoEmpresa.Generacion:
                    txtFechaEscrituraCliente.Enabled = habilitar;
                    txtNumeroEscrituraCliente.Enabled = habilitar;

                    txtFechaRPPCCliente.Visible = false;
                    txtLocalidadNotariaCliente.Visible = false;
                    txtLocalidadRPPCCliente.Visible = false;
                    txtNombreNotarioCliente.Visible = false;
                    txtNumeroFolioCliente.Visible = false;
                    txtNumeroNotariaCliente.Visible = false;
                    this.trSeccion1.Visible = false;
                    this.trSeccion2.Visible = false;
                    this.trSeccion3.Visible = false;
                    this.lblNumEscritura.InnerText = "# Escritura";
                    this.lblFechaEscritura.InnerText = "Fecha Escritura";
                    this.chkActivo.Enabled = habilitar;
                    this.trSeccion4.Visible = veractivo.Value;
                    break;
                case ETipoEmpresa.Equinova:
                    txtFechaEscrituraCliente.Enabled = habilitar;
                    txtNumeroEscrituraCliente.Enabled = habilitar;

                    txtFechaRPPCCliente.Visible = false;
                    txtLocalidadNotariaCliente.Visible = false;
                    txtLocalidadRPPCCliente.Visible = false;
                    txtNombreNotarioCliente.Visible = false;
                    txtNumeroFolioCliente.Visible = false;
                    txtNumeroNotariaCliente.Visible = false;
                    this.trSeccion1.Visible = false;
                    this.trSeccion2.Visible = false;
                    this.trSeccion3.Visible = false;
                    this.lblNumEscritura.InnerText = "# Escritura";
                    this.lblFechaEscritura.InnerText = "Fecha Escritura";
                    this.chkActivo.Enabled = habilitar;
                    this.trSeccion4.Visible = veractivo.Value;
                    break;
                case ETipoEmpresa.Construccion:
                    txtFechaEscrituraCliente.Enabled = habilitar;
                    txtNumeroEscrituraCliente.Enabled = habilitar;

                    txtFechaRPPCCliente.Visible = false;
                    txtLocalidadNotariaCliente.Visible = false;
                    txtLocalidadRPPCCliente.Visible = false;
                    txtNombreNotarioCliente.Visible = false;
                    txtNumeroFolioCliente.Visible = false;
                    txtNumeroNotariaCliente.Visible = false;
                    this.trSeccion1.Visible = false;
                    this.trSeccion2.Visible = false;
                    this.trSeccion3.Visible = false;
                    this.lblNumEscritura.InnerText = "# Escritura";
                    this.lblFechaEscritura.InnerText = "Fecha Escritura";
                    this.chkActivo.Enabled = habilitar;
                    this.trSeccion4.Visible = veractivo.Value;
                    break;
                case ETipoEmpresa.Idealease:
                    txtFechaEscrituraCliente.Enabled = habilitar;
                    txtFechaRPPCCliente.Enabled = habilitar;
                    txtLocalidadNotariaCliente.Enabled = habilitar;
                    txtLocalidadRPPCCliente.Enabled = habilitar;
                    txtNombreNotarioCliente.Enabled = habilitar;
                    txtNumeroEscrituraCliente.Enabled = habilitar;
                    txtNumeroFolioCliente.Enabled = habilitar;
                    txtNumeroNotariaCliente.Enabled = habilitar;
                    this.chkActivo.Enabled = habilitar;
                    break;
            }
        }

        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        public void MostrarDatosActaConstitutiva(ActaConstitutivaBO actaConstitutiva) {
            presentador.MostrarDatosActaConstitutiva(actaConstitutiva);
        }

        public ActaConstitutivaBO ObtenerActaConstitutiva() {
            return presentador.ObtenerActaConstitutiva();
        }

        public string ValidarCampos(bool? validarEscritura = false, bool? representanteLegal = false) {
            return presentador.ValidarCampos(validarEscritura, representanteLegal);
        }
        public void ValidarEscrituraRepresentante(bool lMostrar) {
            this.lblNumEscritura.Visible = lMostrar;
            this.txtNumeroEscrituraCliente.Visible = lMostrar;
            this.lblFechaEscritura.Visible = lMostrar;
            this.txtFechaEscrituraCliente.Visible = lMostrar;
        }
        public void LimpiarCampos() {
            this.presentador.LimpiarCampos();
        }

        #endregion
    }
}