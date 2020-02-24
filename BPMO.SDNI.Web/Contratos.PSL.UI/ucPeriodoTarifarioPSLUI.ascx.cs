using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucPeriodoTarifarioPSLUI : UserControl, IucPeriodoTarifarioPSLVIS {
        #region Atributos
        private string nombreClase = "ucPeriodoTarifarioPSLUI";
        private ucPeriodoTarifarioPSLPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Determina si la interfaz se encuentra en Modo se Consulta
        /// </summary>
        private bool? EsModoEdicion {
            get {
                if (String.IsNullOrEmpty(hdnModoEdicion.Value)) return null;
                return this.hdnModoEdicion.Value == "1";
            }
            set {
                if (value != null)
                    this.hdnModoEdicion.Value = value.Value ? "1" : "0";
                else
                    this.hdnModoEdicion.Value = "0";
            }
        }

        /// <summary>
        /// Indica si la configuración de período tarifario incluye Sábados y Domingos
        /// </summary>
        public bool? IncluyeSD {
            get {
                return this.chbxIncluyeSD.Checked;
            }
            set {
                this.chbxIncluyeSD.Checked = value ?? false;
            }
        }

        /// <summary>
        /// Determina la UO que se está configurando
        /// </summary>
        public int? UnidadOperativaID {
            get {
                int? intRet = null;
                if (Session["PeriodoTarifarioPSLBO"] != null) {
                    var objRet = (DiaPeriodoTarifaBO)Session["PeriodoTarifarioPSLBO"];
                    intRet = objRet.UnidadOperativaID;
                } else {
                    Site master = (Site)Page.Master;
                    intRet = master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null
                        ? master.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
                }
                return intRet;
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


        public int? TurnoTarifaID {
            get {
                int? id;
                if (this.ddlTurnoTarifa.SelectedValue != "-1")
                    id = int.Parse(ddlTurnoTarifa.SelectedValue.Trim());
                else
                    id = null;
                return id;
            }

            set {
                this.ddlTurnoTarifa.SelectedValue = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número de días que dura una semana
        /// </summary>
        public int? DiasDuracionSemana {
            get {
                if (String.IsNullOrEmpty(txtDuracionSemana.Text)) return null;
                return Int32.Parse(txtDuracionSemana.Text.Trim());
            }
            set {
                txtDuracionSemana.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número de días que dura un mes
        /// </summary>
        public int? DiasDuracionMes {
            get {
                if (String.IsNullOrEmpty(txtDuracionMes.Text)) return null;
                return Int32.Parse(txtDuracionMes.Text.Trim());
            }
            set {
                txtDuracionMes.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número de días a partir de los cuales se considera como período diario
        /// </summary>
        public int? InicioPeriodoDia {
            get {
                if (String.IsNullOrEmpty(txtInicioPeriodoDia.Text)) return null;
                return Int32.Parse(txtInicioPeriodoDia.Text.Trim());
            }
            set {
                txtInicioPeriodoDia.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número de días a partir de los cuales se considera como período semanal
        /// </summary>
        public int? InicioPeriodoSemana {
            get {
                if (String.IsNullOrEmpty(txtInicioPeriodoSemana.Text)) return null;
                return Int32.Parse(txtInicioPeriodoSemana.Text.Trim());
            }
            set {
                txtInicioPeriodoSemana.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número de días a partir de los cuales se considera como período mensual
        /// </summary>
        public int? InicioPeriodoMes {
            get {
                if (String.IsNullOrEmpty(txtInicioPeriodoMes.Text)) return null;
                return Int32.Parse(txtInicioPeriodoMes.Text.Trim());
            }
            set {
                txtInicioPeriodoMes.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// lista de Horas Turno configuradas
        /// </summary>
        public List<DetalleHorasTurnoTarifaBO> listHorasTurno {
            get {
                if (Session["HorasTurno"] == null) return null;
                return (List<DetalleHorasTurnoTarifaBO>)Session["HorasTurno"];
            }
            set {
                Session["HorasTurno"] = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el turno de la tarifa
        /// </summary>
        public Enum TarifaTurno {
            get {
                Enum tarifaTurno = null;
                switch (this.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Generacion:
                        tarifaTurno = ETarifaTurnoGeneracion;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        tarifaTurno = ETarifaTurnoConstruccion;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        tarifaTurno = ETarifaTurnoEquinova;
                        break;
                }
                return tarifaTurno;
            }
            set {
                switch (this.UnidadOperativaID) {
                    case (int)ETipoEmpresa.Generacion:
                        this.ddlTurnoTarifa.SelectedIndex = (int)(ETarifaTurnoGeneracion)value;
                        break;
                    case (int)ETipoEmpresa.Construccion:
                        this.ddlTurnoTarifa.SelectedIndex = (int)(ETarifaTurnoConstruccion)value;
                        break;
                    case (int)ETipoEmpresa.Equinova:
                        this.ddlTurnoTarifa.SelectedIndex = (int)(ETarifaTurnoEquinova)value;
                        break;
                }
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Construcción
        /// </summary>
        public ETarifaTurnoConstruccion? ETarifaTurnoConstruccion {
            get {
                ETarifaTurnoConstruccion? eTarifaTurno = null;
                eTarifaTurno = (ETarifaTurnoConstruccion)Enum.Parse(typeof(ETarifaTurnoConstruccion), ddlTurnoTarifa.SelectedIndex.ToString());
                return eTarifaTurno;
            }
            set {
                ddlTurnoTarifa.SelectedIndex = (int)(ETarifaTurnoConstruccion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Generación
        /// </summary>
        public ETarifaTurnoGeneracion? ETarifaTurnoGeneracion {
            get {
                ETarifaTurnoGeneracion? eTarifaTurno = null;
                eTarifaTurno = (ETarifaTurnoGeneracion)Enum.Parse(typeof(ETarifaTurnoGeneracion), ddlTurnoTarifa.SelectedIndex.ToString());
                return eTarifaTurno;
            }
            set {
                ddlTurnoTarifa.SelectedIndex = (int)(ETarifaTurnoGeneracion)value;
            }
        }

        /// <summary>
        /// Enumerador Turno de Tarifa para Equinova
        /// </summary>
        public ETarifaTurnoEquinova? ETarifaTurnoEquinova {
            get {
                ETarifaTurnoEquinova? eTarifaTurno = null;
                eTarifaTurno = (ETarifaTurnoEquinova)Enum.Parse(typeof(ETarifaTurnoEquinova), ddlTurnoTarifa.SelectedIndex.ToString());
                return eTarifaTurno;
            }
            set {
                ddlTurnoTarifa.SelectedIndex = (int)(ETarifaTurnoEquinova)value;
            }
        }

        /// <summary>
        /// Determina el número máximo de horas que debe trabajar para el período diario
        /// </summary>
        public int? MaximoHorasDia {
            get {
                if (String.IsNullOrEmpty(txtMaximoHorasDia.Text)) return null;
                return Int32.Parse(txtMaximoHorasDia.Text.Trim());
            }
            set {
                txtMaximoHorasDia.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número máximo de horas que debe trabajar para el período semanal
        /// </summary>
        public int? MaximoHorasSemana {
            get {
                if (String.IsNullOrEmpty(txtMaximoHorasSemana.Text)) return null;
                return Int32.Parse(txtMaximoHorasSemana.Text.Trim());
            }
            set {
                txtMaximoHorasSemana.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Determina el número máximo de horas que debe trabajar para el período mensual
        /// </summary>
        public int? MaximoHorasMes {
            get {
                if (String.IsNullOrEmpty(txtMaximoHorasMes.Text)) return null;
                return Int32.Parse(txtMaximoHorasMes.Text.Trim());
            }
            set {
                txtMaximoHorasMes.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucPeriodoTarifarioPSLPRE(this);
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
        }

        /// <summary>
        /// Limpia los campos donde se agregan los máximos de horas
        /// </summary>
        public void LimpiarCamposHorasTurno() {
            this.ddlTurnoTarifa.SelectedValue = "-1";
            this.txtMaximoHorasDia.Text = string.Empty;
            this.txtMaximoHorasSemana.Text = string.Empty;
            this.txtMaximoHorasMes.Text = string.Empty;
        }

        /// <summary>
        /// Método que se utiliza para llenar el combo de los turnos de las tarifas, dependiendo de la empresa que tenga la sesión activa
        /// </summary>
        public void EstablecerOpcionesTurnoTarifa(Dictionary<string, string> turnos) {
            this.ddlTurnoTarifa.Items.Clear();
            this.ddlTurnoTarifa.DataSource = turnos;
            this.ddlTurnoTarifa.DataTextField = "Value";
            this.ddlTurnoTarifa.DataValueField = "Key";
            this.ddlTurnoTarifa.DataBind();
            this.ddlTurnoTarifa.SelectedValue = "-1";
        }

        /// <summary>
        /// Coloca los controles en Modo de Edicion
        /// </summary>
        /// <param name="activo">Boolean que determina si colocar en TRUE el Modo de Edicion</param>
        public void ModoEdicion(bool activo) {
            this.EsModoEdicion = activo;
            this.chbxIncluyeSD.Enabled = activo;
            this.txtDuracionSemana.Enabled = activo;
            this.txtDuracionMes.Enabled = activo;

            this.txtInicioPeriodoDia.Enabled = false;
            this.txtInicioPeriodoSemana.Enabled = activo;
            this.txtInicioPeriodoMes.Enabled = activo;

            this.ddlTurnoTarifa.Enabled = activo;
            this.txtMaximoHorasDia.Enabled = activo;
            this.txtMaximoHorasSemana.Enabled = activo;
            this.txtMaximoHorasMes.Enabled = activo;

            this.btnAgregarHorasTurno.Enabled = activo;
        }

        /// <summary>
        /// Presenta en Pantalla los Rangos de la Tarifa
        /// </summary>
        /// <param name="listaHorasTurno">Rangos que Pertenecen a la Tarifa</param>
        public void PresentarHorasTurno(List<DetalleHorasTurnoTarifaBO> listaHorasTurno) {
            if (listaHorasTurno == null)
                listaHorasTurno = new List<DetalleHorasTurnoTarifaBO>();

            this.grvTurnos.DataSource = listaHorasTurno;
            this.grvTurnos.DataBind();
        }

        /// <summary>
        /// Método que se utiliza para llenar el combo de los turnos de las tarifas, dependiendo de la empresa que tenga la sesión activa
        /// </summary>
        public void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turnos) {
            ddlTurnoTarifa.Items.Clear();
            ddlTurnoTarifa.DataSource = turnos;
            ddlTurnoTarifa.DataTextField = "Value";
            ddlTurnoTarifa.DataValueField = "Key";
            ddlTurnoTarifa.DataBind();
            ddlTurnoTarifa.SelectedValue = "-1";
        }

        #endregion
        #region Eventos
        protected void grvTurnos_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if (e.CommandArgument == null) return;

                int index;

                if (!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;
                if (listHorasTurno == null) return;
                var rangoTarifa = listHorasTurno[index];

                switch (eCommandNameUpper) {
                    case "CMDELIMINAR":
                        listHorasTurno.Remove(rangoTarifa);
                        PresentarHorasTurno(listHorasTurno);
                        break;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al eliminar el rango de la Tarifa", ETipoMensajeIU.ERROR, this.nombreClase + ".grvTurnos_RowCommand: " + ex.Message);
            }
        }


        protected void btnAgregarHorasTurno_Click(object sender, EventArgs e) {
            try {
                presentador.AgregarHorasTurnoAPeriodoTarifario();
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al agregar un Rango", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarRango_Click: " + ex.Message);
            }
        }


        protected void grvTurnos_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    DetalleHorasTurnoTarifaBO horasTurno = (DetalleHorasTurnoTarifaBO)e.Row.DataItem;

                    #region Agregar el nombre del turno a la tabla
                    Label labelTurno = e.Row.FindControl("lblTurno") as Label;
                    if (labelTurno != null) {
                        string tipoAutorizacion = string.Empty;
                        if (horasTurno.TurnoTarifa != null) {
                            switch (this.UnidadOperativaID) {
                                case (int)ETipoEmpresa.Generacion:
                                case (int)ETipoEmpresa.Construccion:
                                case (int)ETipoEmpresa.Equinova:
                                    Type type = this.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) :
                                        this.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) : typeof(ETarifaTurnoEquinova);
                                    var memInfo = type.GetMember(type.GetEnumName(horasTurno.TurnoTarifa));
                                    var display = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                                    if (display != null) {
                                        tipoAutorizacion = display.Description.ToUpper();
                                    }
                                    break;
                            }
                        }
                        labelTurno.Text = tipoAutorizacion.Replace("_", " ");
                    }
                    #endregion

                    if (this.EsModoEdicion != null) {
                        var boton = e.Row.FindControl("ibtEliminar") as ImageButton;
                        if (boton != null) {
                            boton.Enabled = this.EsModoEdicion.Value;
                        }
                    }
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".grvTurnos_RowDataBound: Inconsistencias al agregar los Rangos: " + ex.Message);
            }
        }
        #endregion
    }
}