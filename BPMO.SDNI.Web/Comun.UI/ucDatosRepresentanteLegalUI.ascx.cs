//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI {
    public partial class ucDatosRepresentanteLegalUI : System.Web.UI.UserControl, IucDatosRepresentanteLegalVIS {
        #region Atributos

        private ucDatosRepresentanteLegalPRE presentador;
        private List<CatalogoBaseBO> listaAcciones;
        private int? unidadOperativaId;
        #endregion Atributos

        #region Propiedades

        public string RFC {
            get { return this.txtRFC.Text; }
            set { this.txtRFC.Text = value; }
        }

        public List<CatalogoBaseBO> ListaAcciones {
            get { return this.listaAcciones; }
            set { this.listaAcciones = value; }
        }

        public ActaConstitutivaBO ActaConstitutiva {
            get { return datosActaConstitutivaUI.ObtenerActaConstitutiva(); }
            set { datosActaConstitutivaUI.MostrarDatosActaConstitutiva(value); }
        }

        public bool? Depositario {
            get {
                if (!ddlDepositario.Visible) return false;
                if (this.ddlDepositario.SelectedValue == "1")
                    return true;
                if (this.ddlDepositario.SelectedValue == "2")
                    return false;
                return null;
            }
            set {
                if (value == true)
                    this.ddlDepositario.SelectedIndex = this.ddlDepositario.Items.IndexOf(this.ddlDepositario.Items.FindByValue("1"));
                if (value == false)
                    this.ddlDepositario.SelectedIndex = this.ddlDepositario.Items.IndexOf(this.ddlDepositario.Items.FindByValue("2"));
                if (value == null)
                    this.ddlDepositario.SelectedIndex = this.ddlDepositario.Items.IndexOf(this.ddlDepositario.Items.FindByValue("0"));
            }
        }

        public string Direccion {
            get { return (String.IsNullOrEmpty(this.txtDireccionRepresentante.Text.Trim())) ? null : this.txtDireccionRepresentante.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtDireccionRepresentante.Text = value;
                else
                    this.txtDireccionRepresentante.Text = string.Empty;
            }
        }

        public string Nombre {
            get { return (String.IsNullOrEmpty(this.txtNombreRepresentante.Text.Trim())) ? null : this.txtNombreRepresentante.Text.ToUpper(); }
            set {
                if (value != null)
                    this.txtNombreRepresentante.Text = value;
                else
                    this.txtNombreRepresentante.Text = string.Empty;
            }
        }

        public int? RepresentanteID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnRepresentanteID.Value.Trim()))
                    id = int.Parse(this.hdnRepresentanteID.Value.Trim());

                return id;
            }
            set { this.hdnRepresentanteID.Value = value != null ? value.ToString() : string.Empty; }
        }

        public string Telefono {
            get { return (String.IsNullOrEmpty(this.txtTelefonoRepresentante.Text.Trim())) ? null : this.txtTelefonoRepresentante.Text; }
            set {
                if (value != null)
                    this.txtTelefonoRepresentante.Text = value;
                else
                    this.txtTelefonoRepresentante.Text = string.Empty;
            }
        }

        public int? UnidadOperatiaId {
            get {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
            }
            set { this.unidadOperativaId = value; }
        }

        #endregion

        #region Constructores

        public ucDatosRepresentanteLegalUI() {
            presentador = new ucDatosRepresentanteLegalPRE(this);
        }
        protected void Page_Load(object sender, EventArgs e) {
            presentador = new ucDatosRepresentanteLegalPRE(this);
        }

        #endregion

        #region Métodos

        public void HabilitarCampos(bool habilitar, bool? veactivo = true) {
            this.txtDireccionRepresentante.Enabled = habilitar;
            this.txtNombreRepresentante.Enabled = habilitar;
            this.txtTelefonoRepresentante.Enabled = habilitar;
            this.ddlDepositario.Enabled = habilitar;
            this.datosActaConstitutivaUI.HabilitarCampos(habilitar, veactivo);
            this.txtRFC.Enabled = habilitar;
            this.trRfc.Visible = habilitar && (this.unidadOperativaId != (int)ETipoEmpresa.Idealease);
            if (this.ListaAcciones != null) EstablecerAcciones(this.ListaAcciones, habilitar);

        }

        public void MostrarDatosRepresentante(RepresentanteLegalBO representante) {
            presentador.MostrarDatosRepresentanteLegal(representante);
        }

        public void MostrarDepositario(bool mostrar) {
            lblDepositario.Visible = mostrar;
            asteriscoDepositario.Visible = mostrar;
            ddlDepositario.Visible = mostrar;
        }

        public void PrepararNuevo() {
            this.presentador.PrepararNuevo();
        }

        public string ValidarActaConstitutiva(bool? validarfc = false, bool? representanteLegal = false) {
            return datosActaConstitutivaUI.ValidarCampos(validarfc, representanteLegal);
        }

        public void ValidarEscrituraRepresentante(int? UnidadOperativaID = 84) {
            switch (UnidadOperativaID) {
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                case (int)ETipoEmpresa.Construccion:
                    datosActaConstitutivaUI.ValidarEscrituraRepresentante(true);
                    break;
                case (int)ETipoEmpresa.Idealease:
                    datosActaConstitutivaUI.ValidarEscrituraRepresentante(true);
                    break;
            }
        }

        #endregion


        public void EstablecerAcciones(List<CatalogoBaseBO> lstAcciones, bool habilitar) {
            ETipoEmpresa empresa = (ETipoEmpresa)this.UnidadOperatiaId;
            this.datosActaConstitutivaUI.UnidadOperativaID = this.UnidadOperatiaId;
            datosActaConstitutivaUI.ListaAcciones = this.ListaAcciones;
            if (empresa != ETipoEmpresa.Idealease)
                this.trRfc.Visible = true;
            else
                this.trRfc.Visible = false;
        }

    }
}