// Construcción de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucTarifaPSLUI : UserControl, IucTarifaPSLVIS {
        #region Atributos
        private string nombreClase = "ucTarifaPSLUI";
        private ucTarifaPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Determina si la interfaz se encuentra en Modo se Consulta
        /// </summary>
        private bool? EsModoConsulta {
            get {
                if (String.IsNullOrEmpty(hdnModoConsulta.Value)) return null;
                return this.hdnModoConsulta.Value == "1";
            }
            set {
                if (value != null)
                    this.hdnModoConsulta.Value = value.Value ? "1" : "0";
                else
                    this.hdnModoConsulta.Value = "0";
            }
        }
        public decimal? Tarifa {
            get {
                decimal? tarifa = null;
                if (!String.IsNullOrEmpty(this.txtTarifa.Text.Trim()))
                    tarifa = decimal.Parse(this.txtTarifa.Text.Trim().Replace(",", ""));
                return tarifa;
            }
            set { this.txtTarifa.Text = value != null ? String.Format("{0:#,##0.00}", value) : String.Empty; }
        }
        /// <summary>
        /// Rangos de la Tarifa de Renta Diaria
        /// </summary>
        public List<RangoTarifaPSLBO> RangosTarifa {
            get {
                if (Session["RangosTarifaPSL"] == null) return null;
                return (List<RangoTarifaPSLBO>)Session["RangosTarifaPSL"];
            }
            set {
                Session["RangosTarifaPSL"] = value;
            }
        }

        /// <summary>
        /// Rango Inicial
        /// </summary>
        public int? RangoInicial {
            get {
                if (String.IsNullOrEmpty(txtRangoInicial.Text)) return null;
                return Int32.Parse(txtRangoInicial.Text.Trim());
            }
            set {
                txtRangoInicial.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Rango Final
        /// </summary>
        public int? RangoFinal {
            get {
                if (String.IsNullOrEmpty(txtRangoFinal.Text)) return null;
                return Int32.Parse(txtRangoFinal.Text.Trim());
            }
            set {
                txtRangoFinal.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Costo Rango
        /// </summary>
        public decimal? CostoRango {
            get {
                if (String.IsNullOrEmpty(txtCobroKmHr.Text.Trim())) return null;
                return Decimal.Parse(txtCobroKmHr.Text.Trim());
            }
            set {
                txtCobroKmHr.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina si sera el Rango Final de la Tarifa
        /// </summary>
        public bool? EsRangoFinal {
            get {
                return this.ddlRangoTarifa.SelectedIndex == 1;
            }
            set {
                if (value != null) {
                    this.ddlRangoTarifa.SelectedIndex = value.Value ? 1 : 0;
                } else
                    this.ddlRangoTarifa.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Determina si se pueden Crear Rangos
        /// </summary>
        public bool? CrearRangos {
            get {
                if (String.IsNullOrEmpty(hdnCrearRangos.Value)) return null;
                return this.hdnCrearRangos.Value == "1";
            }
            set {
                if (value != null)
                    this.hdnCrearRangos.Value = value.Value ? "1" : "0";
                else
                    this.hdnCrearRangos.Value = "0";
            }
        }
        ///// <summary>
        ///// Tarifa por Hr Adicional / Modo Antiguo
        ///// </summary>
        public decimal? TarifaHrAdicional {
            get {
                if (!String.IsNullOrEmpty(txtTarifaHrAdicional.Text.Trim()))
                    return Decimal.Parse(txtTarifaHrAdicional.Text.Trim());
                return null;
            }
            set {
                this.txtTarifaHrAdicional.Text = value != null ? value.ToString() : "";
            }
        }

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucTarifaPSLPRE(this);
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".Page_Load: Error al inicializar el control de usuario: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Limpia la Session activa
        /// </summary>
        public void LimpiarSesion() {
            if (Session["UltimaTarifa"] != null)
                Session.Remove("UltimaTarifa");
            if (Session["RangosTarifaPSL"] != null)
                Session.Remove("RangosTarifaPSL");
        }
        /// <summary>
        /// Coloca los controles en Modo de Edicion
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edicion</param>
        public void ModoEdicion(bool activo) {
            this.EsModoConsulta = !activo;
            this.txtRangoInicial.Enabled = activo;
            this.txtRangoFinal.Enabled = activo;
            this.ddlRangoTarifa.Enabled = activo;
            this.txtCobroKmHr.Enabled = activo;
            this.btnAgregarRango.Enabled = activo;
            this.txtTarifa.Enabled = activo;
            this.txtTarifaHrAdicional.Enabled = activo;
            //this.txtTarifaHrAdicional.Enabled = activo;
        }
        /// <summary>
        /// Presenta en Pantalla los Rangos de la Tarifa
        /// </summary>
        /// <param name="rangoTarifa">Rangos que Pertenecen a la Tarifa</param>
        public void PresentarRangos(List<RangoTarifaPSLBO> rangoTarifa) {
            if (rangoTarifa == null)
                rangoTarifa = new List<RangoTarifaPSLBO>();

            this.grvRangos.DataSource = rangoTarifa;
            this.grvRangos.DataBind();
        }

        public void PermitirRangoInicial(bool permitir) {
            this.txtRangoInicial.Enabled = permitir;
        }

        public void PermiritRangoCargo(bool permitir) {
            this.ddlRangoTarifa.Enabled = permitir;
        }

        public void PermitirRangoFinal(bool permitir) {
            this.txtRangoFinal.Enabled = permitir;
        }

        public void PermitirCargoAdicional(bool permitir) {
            this.txtCobroKmHr.Enabled = permitir;
        }

        public void PermitirAgregarRangos(bool permitir) {
            this.btnAgregarRango.Enabled = permitir;
        }

        /// <summary>
        /// Mantiene el modo Actual de las Tarifas, sin configuracion de rangos
        /// </summary>
        /// <param name="modoAntiguo">Determina si se utiliza el modo antiguo</param>
        public void ModoAntiguo(bool modoAntiguo) {
            this.trGridRangos.Visible = false;
        }

        #endregion
        #region Eventos
        protected void grvRangos_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if (e.CommandArgument == null) return;
                if (this.EsModoConsulta.Value) return;

                int index;

                if (!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;
                if (RangosTarifa == null) return;
                var rangoTarifa = RangosTarifa[index];

                switch (eCommandNameUpper) {
                    case "CMDELIMINAR":
                        if (RangosTarifa.Count > 1 && (index + 1) != RangosTarifa.Count)
                            throw new Exception("Debe Eliminarse el Último Rango Primero");

                        RangosTarifa.Remove(rangoTarifa);
                        PresentarRangos(RangosTarifa);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al eliminar el rango de la Tarifa", ETipoMensajeIU.ERROR, this.nombreClase + ".grvRangos_RowCommand: " + ex.Message);
            }
        }
        protected void btnAgregarRango_Click(object sender, EventArgs e) {
            try {
                presentador.AgregarRangoATarifa();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar un Rango", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarRango_Click: " + ex.Message);
            }
        }
        protected void grvRangos_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    if (this.EsModoConsulta != null) {
                        var boton = e.Row.FindControl("ibtEliminar") as ImageButton;
                        if (boton != null) {
                            boton.Enabled = !this.EsModoConsulta.Value;
                        }
                    }
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".grvRangos_RowDataBound: Inconsistencias al agregar los Rangos: " + ex.Message);
            }
        }
        protected void ddlRangoTarifa_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                this.RangoFinal = null;
                this.PermitirRangoFinal(this.ddlRangoTarifa.SelectedIndex == 0);
                if (!this.EsRangoFinal.Value)
                    this.txtRangoFinal.Focus();
                else
                    this.txtCobroKmHr.Focus();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al cambiar el Rango", ETipoMensajeIU.ERROR, this.nombreClase + ".ddlRangoTarifa_SelectedIndexChanged: " + ex.Message);
            }
        }
        #endregion
    }
}