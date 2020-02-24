using System;
using System.Collections.Generic;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucCargosAdicionalesCierrePSLUI : UserControl, IucCargosAdicionalesCierrePSLVIS {
        #region Atributos
        private ucCargosAdicionalesCierrePSLPRE presentador;
        private const string NombreClase = "ucLineaContratoPSLUI";

        public enum ECatalogoBuscador {
            ProductoServicio = 1
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID {
            get {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidadOperativaID.Value.Trim()))
                    value = int.Parse(hdnUnidadOperativaID.Value.Trim());

                return value;
            }
            set {
                hdnUnidadOperativaID.Value = value != null ? value.Value.ToString("") : string.Empty;
            }
        }
        /// <summary>
        /// Identificador de la Unidad sobre la que se esta creando la línea del contrato
        /// </summary>
        public int? UnidadID {
            get {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidaID.Value.Trim()))
                    value = int.Parse(hdnUnidaID.Value.Trim());

                return value;
            }
            set {
                hdnUnidaID.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Identificador del Equipo de la Unidad
        /// </summary>
        public int? EquipoID {
            get {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnEquipoID.Value.Trim()))
                    value = int.Parse(hdnEquipoID.Value.Trim());

                return value;
            }
            set {
                hdnEquipoID.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Presentador del User Control para uso interno
        /// </summary>
        public ucCargosAdicionalesCierrePSLPRE Presentador {
            get { return presentador; }
            set { presentador = value; }
        }
        /// <summary>
        /// Obtiene o establece el identificador del modelo
        /// </summary>
        public int? ModeloID {
            get {
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value)) {
                    int val;
                    return Int32.TryParse(this.hdnModeloID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnModeloID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public int? SucursalID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value.Trim()))
                    id = int.Parse(this.hdnSucursalID.Value.Trim());
                return id;
            }
            set { this.hdnSucursalID.Value = value != null ? value.ToString() : String.Empty; }
        }
        /// <summary>
        /// Listado de Equipos Aliados
        /// </summary>
        public List<EquipoAliadoBO> ListadoEquiposAliados {
            get {
                if (Session["EquiposAliados_Unidad_LineaContrato"] == null)
                    Session["EquiposAliados_Unidad_LineaContrato"] = new List<EquipoAliadoBO>();

                return (List<EquipoAliadoBO>)Session["EquiposAliados_Unidad_LineaContrato"];
            }
            set {
                if (value != null)
                    Session["EquiposAliados_Unidad_LineaContrato"] = value;
                else
                    Session["EquiposAliados_Unidad_LineaContrato"] = new List<EquipoAliadoBO>();
            }
        }
        /// <summary>
        /// Obtiene o establece el código de la moneda
        /// </summary>
        public string CodigoMoneda {
            get {
                return (String.IsNullOrEmpty(this.hdnCodigoMoneda.Value)) ? null : this.hdnCodigoMoneda.Value.Trim().ToUpper();
            }
            set {
                this.hdnCodigoMoneda.Value = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Enumerador Área unidad
        /// </summary>
        public Enum EArea {
            get {
                Enum area = null;
                if (!string.IsNullOrEmpty(hdnArea.Value)) {
                    UnidadBO unidadBO = new UnidadBO();
                    Type TipoEnum = unidadBO.obtenerEAreas((ETipoEmpresa)this.UnidadOperativaID);
                    area = (Enum)Enum.ToObject(TipoEnum, (int)Convert.ChangeType(hdnArea.Value, typeof(int)));
                }
                return area;
            }
            set {
                hdnArea.Value = (Convert.ToInt32(value)).ToString();
            }
        }
        /// <summary>
        /// TarifaPSLID
        /// </summary>
        public int? TarifaPSLID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnTarifaPSLID.Value.Trim()))
                    id = int.Parse(hdnTarifaPSLID.Value.Trim());
                return id;
            }
            set { this.hdnTarifaPSLID.Value = value != null ? value.ToString() : String.Empty; }
        }
        ///// <summary>
        ///// Duración en días del periodo configurado para el turno de la tarifa
        ///// </summary>
        public int? DuracionDiasPeriodo {
            get {
                if (!String.IsNullOrEmpty(hdnDuracionMes.Value.Trim()))
                    return Int32.Parse(hdnDuracionMes.Value.Trim());
                return null;
            }
            set {
                this.hdnDuracionMes.Value = value != null ? value.ToString() : "";
            }
        }
        ///// <summary>
        ///// Máximo de horas configurado por turno 
        ///// </summary>
        public decimal? MaximoHrsTurno {
            get {
                if (!String.IsNullOrEmpty(hdnMaximoHrsTurno.Value.Trim()))
                    return Decimal.Parse(hdnMaximoHrsTurno.Value.Trim());
                return null;
            }
            set {
                this.hdnMaximoHrsTurno.Value = value != null ? value.ToString() : "";
            }
        }
        ///// <summary>
        ///// Factor por el cual se deberá multiplicar la tarifa para conocer su valor
        ///// </summary>
        public decimal? FactorTarifa {
            get {
                if (!String.IsNullOrEmpty(hdnFactorTurno.Value.Trim()))
                    return Decimal.Parse(hdnFactorTurno.Value.Trim());
                return null;
            }
            set {
                this.hdnFactorTurno.Value = value != null ? value.ToString() : "";
            }
        }

        /// <summary>
        /// Manejado de Eventos al hacer clic sobre el botón Agregar
        /// </summary>
        public EventHandler AgregarClick { get; set; }
        /// <summary>
        /// Manejador de Eventos al hacer clic sobre el botón Cancelar
        /// </summary>
        public EventHandler CancelarClick { get; set; }
        /// <summary>
        /// Línea de Contrato antes de Editarla
        /// </summary>
        public LineaContratoPSLBO UltimoObjeto {
            get { return Session["UltimaLineaContrato"] as LineaContratoPSLBO; }
            set { Session["UltimaLineaContrato"] = value; }
        }

        public string ModoRegistro { get; set; }

        #region Buscador
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto {
            get {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession]);

                return objeto;
            }
            set {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador {
            get {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid]);

                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ucLineaContratoPSLUI.ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ucLineaContratoPSLUI.ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion Buscador

        /**/

        /// <summary>
        /// Horas de Equipo Refrigeración en la entrega 
        /// </summary>
        public int? HrsEntrega {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtHrsEqRefEntrega.Text.Trim()))
                    num = int.Parse(this.txtHrsEqRefEntrega.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtHrsEqRefEntrega.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        public int? HrsRecepcion {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtHrsEqRefRecepcion.Text.Trim()))
                    num = int.Parse(this.txtHrsEqRefRecepcion.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtHrsEqRefRecepcion.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        /// <summary>
        /// Horas consumidas entre la salida y regreso de la unidad
        /// </summary>
        public int? HrsConsumidas {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtTotHrsEqRefrigeracion.Text.Trim()))
                    num = int.Parse(this.txtTotHrsEqRefrigeracion.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtTotHrsEqRefrigeracion.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        public int? TarifaHrsLibres {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtHrsLibres.Text.Trim()))
                    num = int.Parse(this.txtHrsLibres.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtHrsLibres.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        /// <summary>
        /// Horas excedidas según la tarifa y las horas consumidas
        /// </summary>
        public int? HrsExcedidas {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtHrsExcedidas.Text.Trim()))
                    num = int.Parse(this.txtHrsExcedidas.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtHrsExcedidas.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }

        public decimal? TarifaHrsExcedidas {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtTarifaHrExc.Text.Trim()))
                    num = decimal.Parse(this.txtTarifaHrExc.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtTarifaHrExc.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public decimal? MontoHrsExcedidas {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtMontoHrsEqRef.Text.Trim()))
                    num = decimal.Parse(this.txtMontoHrsEqRef.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtMontoHrsEqRef.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public decimal? ImporteUnidadCombustible {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtImporteUnidadCombustible.Text.Trim()))
                    num = decimal.Parse(this.txtImporteUnidadCombustible.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtImporteUnidadCombustible.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        /// <summary>
        /// porcentaje de combustible de una unidad para el checklist de entrega
        /// </summary>
        public decimal? DiferenciaCombustible {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtCombustibleSalida.Text.Trim()))
                    num = decimal.Parse(this.txtCombustibleSalida.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtCombustibleSalida.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public decimal? ImporteTotalCombustible {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtImporteTotCombustible.Text.Trim()))
                    num = decimal.Parse(this.txtImporteTotCombustible.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtImporteTotCombustible.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public decimal? CargoAbusoOperacion {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtCargoAbusoOperacion.Text.Trim()))
                    num = decimal.Parse(this.txtCargoAbusoOperacion.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtCargoAbusoOperacion.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public decimal? CargoDisposicionBasura {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtCargoDispBasura.Text.Trim()))
                    num = decimal.Parse(this.txtCargoDispBasura.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtCargoDispBasura.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public int? DiasRentaProgramada {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtNumDiasRentProg.Text.Trim()))
                    num = int.Parse(this.txtNumDiasRentProg.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtNumDiasRentProg.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? DiasEnTaller {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtDiasUnidadTaller.Text.Trim()))
                    num = int.Parse(this.txtDiasUnidadTaller.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtDiasUnidadTaller.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? DiasRealesRenta {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtDiasRealesRenta.Text.Trim()))
                    num = int.Parse(this.txtDiasRealesRenta.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtDiasRealesRenta.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? DiasAdicionales {
            get {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtDiasAdicionalesCobro.Text.Trim()))
                    num = int.Parse(this.txtDiasAdicionalesCobro.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtDiasAdicionalesCobro.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public decimal? MontoTotalDiasAdicionales {
            get {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtTotalDiasAdicionales.Text.Trim()))
                    num = decimal.Parse(this.txtTotalDiasAdicionales.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtTotalDiasAdicionales.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public int? ModuloID {
            get {
                return ((Site)Page.Master).ModuloID;
            }
        }

        /**/

        #endregion

        #region Metodos
        /// <summary>
        /// Muestra los mensajes en la UI
        /// </summary>
        /// <param name="mensaje">Mensaje a Mostrar</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Detalle o sub mensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null)
                    master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null)
                    master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        public void RegistrarScript(string key, string script) {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        /// <summary>
        /// Limpia la sesión del control
        /// </summary>
        public void LimpiarSesion() {
            Session.Remove("EquiposAliados_Unidad_LineaContrato");
            Session.Remove("TarifasAdicionalesLinea");
        }
        /// <summary>
        /// Inicializa el Control
        /// </summary>
        public void Inicializar() {

            hdnTarifaPSLID.Value = string.Empty;
            hdnDuracionMes.Value = string.Empty;
            hdnMaximoHrsTurno.Value = string.Empty;
            hdnFactorTurno.Value = string.Empty;

        }

        /// <summary>
        /// Deshabilita los campos para la vista del detalle
        /// </summary>
        public void PrepararVistaDetalle() {

        }

        /// <summary>
        /// Configura la interfaz en modo Consultar
        /// </summary>
        public void ConfigurarModoConsultar() {

            btnAgregar.Enabled = false;

        }
        /// <summary>
        /// Configura la interfaz en modo Editar
        /// </summary>
        public void ConfigurarModoEditar() {

            btnAgregar.Enabled = true;
        }
        public void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipos) {

        }
        /// <summary>
        /// Método que se utiliza para llenar el combo de los turnos de las tarifas, dependiendo de la empresa que tenga la sesión activa
        /// </summary>
        public void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turnos) {

        }

        /// <summary>
        /// Método encargado de realizar el llenado del combo con los valores de los períodos de las tarifas
        /// </summary>
        public void EstablecerOpcionesPeriodoTarifa(Dictionary<string, string> periodos) {

        }

        /// <summary>
        /// Despliega en pantalla los equipos aliados de la unidad seleccionada
        /// </summary>
        /// <param name="dt">Tabla con la información de los equipos aliados</param>
        public void EstablecerEquiposAliadoUnidad(System.Data.DataTable dt) {
            this.grdEquiposAliados.DataSource = dt;
            this.grdEquiposAliados.DataBind();
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucCargosAdicionalesCierrePSLPRE(this);
                if (!Page.IsPostBack) {
                    Presentador.Inicializar();

                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }


        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                this.LimpiarSesion();
                //presentador.Inicializar();
                if (CancelarClick != null) CancelarClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e) {
            string resultado = presentador.ValidarDatos();

            if (!string.IsNullOrEmpty(resultado)) {
                MostrarMensaje(resultado, ETipoMensajeIU.INFORMACION);
                return;
            }

            try {
                if (AgregarClick != null) AgregarClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregar_Click: " + ex.Message);
            }
        }


        #endregion
    }
}