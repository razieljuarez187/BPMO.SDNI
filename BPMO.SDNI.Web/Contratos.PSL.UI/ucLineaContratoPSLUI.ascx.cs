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
    public partial class ucLineaContratoPSLUI : UserControl, IucLineaContratoPSLVIS {
        #region Atributos
        private ucLineaContratoPSLPRE presentador;
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
        /// Numero de Serie o código VIN
        /// </summary>
        public string VIN {
            get {
                return txtVIN.Text.Trim().ToUpper();
            }
            set {
                txtVIN.Text = value != null ? value.Trim() : string.Empty;
            }
        }
        /// <summary>
        /// Numero Económico
        /// </summary>
        public string NumeroEconocimico {
            get {
                return txtNumeroEconomico.Text.Trim().ToUpper();
            }
            set {
                txtNumeroEconomico.Text = value != null ? value.Trim() : string.Empty;
            }
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
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad
        /// </summary>
        public string Modelo {
            get {
                return (String.IsNullOrEmpty(this.txtModelo.Text)) ? null : this.txtModelo.Text.Trim().ToUpper();
            }
            set {
                this.txtModelo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la marca de la unidad
        /// </summary>
        public string Marca {
            get {
                return (String.IsNullOrEmpty(this.txtMarca.Text)) ? null : this.txtMarca.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtMarca.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
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
        /// Obtiene o establece el nombre de la sucursal de la unidad
        /// </summary>
        public string Sucursal {
            get {
                return (String.IsNullOrEmpty(this.txtUnidadSucursal.Text)) ? null : this.txtUnidadSucursal.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtUnidadSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número de las placas estatales de la unidad
        /// </summary>
        public string UnidadPlacaEstatal {
            get {
                return (String.IsNullOrEmpty(this.txtUnidadPlacasEstales.Text)) ? null : this.txtUnidadPlacasEstales.Text.Trim().ToUpper(); //RI0061
            }
            set {
                this.txtUnidadPlacasEstales.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Año del Unidad
        /// </summary>
        public int? Anio {
            get {
                int? value = null;

                if (!string.IsNullOrEmpty(txtAnio.Text.Trim()))
                    value = int.Parse(txtAnio.Text.Trim());

                return value;
            }
            set {
                txtAnio.Text = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Capacidad del tanque de combustible de la unidad
        /// </summary>
        public int? UnidadCapacidadTanque {
            get {
                if (!string.IsNullOrEmpty(this.txtUnidadCapacidadTanque.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadCapacidadTanque.Text)) {
                    int val;
                    return int.TryParse(this.txtUnidadCapacidadTanque.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtUnidadCapacidadTanque.Text = value.HasValue ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Monto de la tarifa
        /// </summary>
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
        /// Tarifa por Hr Adicional
        /// </summary>
        public decimal? TarifaHrAdicional {
            get {
                if (!String.IsNullOrEmpty(txtTarifaHoraAdicional.Text.Trim()))
                    return Decimal.Parse(txtTarifaHoraAdicional.Text.Trim().Replace(",", ""));
                return null;
            }
            set {
                this.txtTarifaHoraAdicional.Text = value != null ? String.Format("{0:#,##0.00}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Tarifa Turno ID
        /// </summary>
        public Enum TarifaTurno {
            get {
                Enum turno = null;
                if (this.ddlTarifaTurno.SelectedValue != "-1") {
                    switch (this.UnidadOperativaID) {
                        case (int)ETipoEmpresa.Construccion:
                            turno = (ETarifaTurnoConstruccion)Enum.Parse(typeof(ETarifaTurnoConstruccion), this.ddlTarifaTurno.SelectedValue.Trim());
                            break;
                        case (int)ETipoEmpresa.Generacion:
                            turno = (ETarifaTurnoGeneracion)Enum.Parse(typeof(ETarifaTurnoGeneracion), this.ddlTarifaTurno.SelectedValue.Trim());
                            break;
                        case (int)ETipoEmpresa.Equinova:
                            turno = (ETarifaTurnoEquinova)Enum.Parse(typeof(ETarifaTurnoEquinova), this.ddlTarifaTurno.SelectedValue.Trim());
                            break;
                        default:
                            turno = null;
                            break;
                    }
                    return turno;
                } else
                    return null;
            }
            set {
                if (this.UnidadOperativaID == (int)ETipoEmpresa.Generacion) {
                    if (value == null)
                        this.ddlTarifaTurno.SelectedIndex = 0;
                    else
                        this.ddlTarifaTurno.SelectedValue = ((int)(ETarifaTurnoGeneracion)value).ToString();
                }
                if (this.UnidadOperativaID == (int)ETipoEmpresa.Construccion) {
                    if (value == null)
                        this.ddlTarifaTurno.SelectedIndex = 0;
                    else
                        this.ddlTarifaTurno.SelectedValue = ((int)(ETarifaTurnoConstruccion)value).ToString();
                }
                if (this.UnidadOperativaID == (int)ETipoEmpresa.Equinova) {
                    if (value == null)
                        this.ddlTarifaTurno.SelectedIndex = 0;
                    else
                        this.ddlTarifaTurno.SelectedValue = ((int)(ETarifaTurnoEquinova)value).ToString();
                }
            }
        }
        /// <summary>
        /// Periodo
        /// </summary>
        public EPeriodosTarifa? PeriodoTarifa {
            get {
                EPeriodosTarifa? periodo = null;
                if (this.ddlPeriodoTarifa.SelectedValue != "-1")
                    periodo = (EPeriodosTarifa)Enum.Parse(typeof(EPeriodosTarifa), this.ddlPeriodoTarifa.SelectedValue.Trim());
                return periodo;
            }
            set { this.ddlPeriodoTarifa.SelectedValue = value != null ? ((int)value).ToString() : "-1"; }
        }
        /// <summary>
        /// Tipo Tarifa 
        /// </summary>
        public int? TipoTarifaID {
            get {
                int? id = null;
                if (ddlTipoTarifa.SelectedValue != "-1")
                    id = int.Parse(ddlTipoTarifa.SelectedValue.Trim());
                return id;
            }
            set { this.ddlTipoTarifa.SelectedValue = value != null ? value.ToString() : "-1"; }
        }
        ///// <summary>
        ///// Gastos de Maniobra
        ///// </summary>
        public decimal? Maniobra {
            get {
                if (!String.IsNullOrEmpty(txtManiobra.Text.Trim()))
                    return Decimal.Parse(txtManiobra.Text.Trim().Replace(",", ""));
                return null;
            }
            set {
                this.txtManiobra.Text = value != null ? String.Format("{0:#,##0.00}", value) : String.Empty;
            }
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
                if (!String.IsNullOrEmpty(hdnDuracionDiasPeriodo.Value.Trim()))
                    return Int32.Parse(hdnDuracionDiasPeriodo.Value.Trim());
                return null;
            }
            set {
                this.hdnDuracionDiasPeriodo.Value = value != null ? value.ToString() : "";
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

        /// <summary>
        /// ModuloID
        /// </summary>
        public int? ModuloID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnModuloID.Value.Trim()))
                    id = int.Parse(hdnModuloID.Value.Trim());
                return id;
            }
            set { this.hdnModuloID.Value = value != null ? value.ToString() : String.Empty; }
        }

        /// <summary>
        /// UsuarioID
        /// </summary>
        public int? UsuarioID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnUsuarioID.Value.Trim()))
                    id = int.Parse(hdnUsuarioID.Value.Trim());
                return id;
            }
            set { this.hdnUsuarioID.Value = value != null ? value.ToString() : String.Empty; }
        }

        /// <summary>
        /// CuentaClienteID
        /// </summary>
        public int? CuentaClienteID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnCuentaClienteID.Value.Trim()))
                    id = int.Parse(hdnCuentaClienteID.Value.Trim());
                return id;
            }
            set { this.hdnCuentaClienteID.Value = value != null ? value.ToString() : String.Empty; }
        }

        ///// <summary>
        ///// Porcentaje máximo de descuento 
        ///// </summary>
        public decimal? PorcentajeMaximoDescuentoTarifa {
            get {
                if (!String.IsNullOrEmpty(hdnPorcentajeMaxDescuentoTar.Value.Trim()))
                    return Decimal.Parse(hdnPorcentajeMaxDescuentoTar.Value.Trim());
                return null;
            }
            set {
                this.hdnPorcentajeMaxDescuentoTar.Value = value != null ? value.ToString() : "";
            }
        }

        ///// <summary>
        ///// Porcentaje de descuento aplicado a la tarifa 
        ///// </summary>
        public decimal? PorcentajeDescuentoTarifa {
            get {
                if (!String.IsNullOrEmpty(hdnPorcentajeDescuentoTar.Value.Trim()))
                    return Decimal.Parse(hdnPorcentajeDescuentoTar.Value.Trim());
                return null;
            }
            set {
                this.hdnPorcentajeDescuentoTar.Value = value != null ? value.ToString() : "";
            }
        }


        ///// <summary>
        ///// Porcentaje de descuento aplicado a la tarifa 
        ///// </summary>
        public decimal? TarifaConDescuento {
            get {
                if (!String.IsNullOrEmpty(hdnTarifaConDescuento.Value.Trim()))
                    return Decimal.Parse(hdnTarifaConDescuento.Value.Trim());
                return null;
            }
            set {
                this.hdnTarifaConDescuento.Value = value != null ? value.ToString() : "";
            }
        }

        /// <summary>
        /// Indica el valor de etiqueta
        /// </summary>
        public string TarifaEtiqueta {
            get {
                return string.IsNullOrEmpty(hdnTarifaEtiqueta.Value.Trim()) ? null : hdnTarifaEtiqueta.Value;
            }
            set {
                hdnTarifaEtiqueta.Value = value ?? "";
            }
        }

        /// <summary>
        /// Indica la modalidad en la cual se esta accediendo a la ventana
        /// </summary>
        public string ModoRegistro {
            get {
                return this.hdnModoRegistro.Value != null ? this.hdnModoRegistro.Value : string.Empty;
            }
            set {
                this.hdnModoRegistro.Value = value != null ? value : string.Empty;
            }
        }

        /// <summary>
        /// Indica si la línea de contrato se encuentra activa o no
        /// </summary>
        public bool? Activo {
            get {
                if (!string.IsNullOrEmpty(this.hdnActivo.Value) && !string.IsNullOrWhiteSpace(this.hdnActivo.Value)) {
                    bool val;
                    return Boolean.TryParse(this.hdnActivo.Value, out val) ? (bool?)val : null;
                }
                return null;
            }
            set { this.hdnActivo.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Indica si la línea de contrato se encuentra activa o no
        /// </summary>
        public bool Devuelta {
            get {
                if (!string.IsNullOrEmpty(this.hdnDevuelta.Value) && !string.IsNullOrWhiteSpace(this.hdnDevuelta.Value)) {
                    bool val;
                    return Boolean.TryParse(this.hdnDevuelta.Value, out val) ? (bool)val : false;
                }
                return false;
            }
            set { this.hdnDevuelta.Value = value.ToString(); }
        }

        /// <summary>
        /// LineaOrigenIntercambioID
        /// </summary>
        public int? LineaOrigenIntercambioID {
            get {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnLineaOrigenIntercambioID.Value.Trim()))
                    id = int.Parse(hdnLineaOrigenIntercambioID.Value.Trim());
                return id;
            }
            set { this.hdnLineaOrigenIntercambioID.Value = value != null ? value.ToString() : String.Empty; }
        }

        #region Tarifa Personalizada
        /// <summary>
        /// Obtiene o Establece el turno
        /// </summary>
        public string TarifaPersonalizadaTurno {
            get {
                if (this.ddlTarifaTurno.SelectedValue != "-1") {

                    return this.ddlTarifaTurno.SelectedItem.Text.Trim();
                } else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o Establece el tipo de tarifa
        /// </summary>
        public string TarifaPersonalizadaTipoTarifa {
            get {
                if (ddlTipoTarifa.SelectedValue != "-1")
                    return ddlTipoTarifa.SelectedItem.Text.Trim();
                return string.Empty;
            }
        }
        #endregion
        /// <summary>
        /// Obtiene o Establece el porcentaje del seguro de la UO
        /// </summary>
        public decimal? PorcentajeSeguro {
            get {
                if (!String.IsNullOrEmpty(txtPorcentajeSeguro.Text.Trim()))
                    return Decimal.Parse(txtPorcentajeSeguro.Text.Trim());
                return null;
            }
            set {
                this.txtPorcentajeSeguro.Text = value != null ? String.Format("{0:##0.00}", value) : string.Empty;
            }
        }
        public decimal? Seguro {
            get {
                decimal? seguro = null;
                if (!String.IsNullOrEmpty(this.txtSeguro.Text.Trim()))
                    seguro = decimal.Parse(this.txtSeguro.Text.Trim().Replace(",", ""));
                return seguro;
            }
            set { this.txtSeguro.Text = value != null ? string.Format("{0:#,##0.00}", value) : string.Empty; }
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
            Session.Remove("TarifaPersonalizaPSLEnviada");
            Session.Remove("TarifaPersonalizaPSLDevuelta");
        }
        /// <summary>
        /// Inicializa el Control
        /// </summary>
        public void Inicializar() {
            //Limpiar campos
            txtNumeroEconomico.Text = string.Empty;
            txtVIN.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtUnidadSucursal.Text = string.Empty;
            txtModelo.Text = string.Empty;
            txtUnidadCapacidadTanque.Text = string.Empty;
            txtAnio.Text = string.Empty;
            txtUnidadPlacasEstales.Text = string.Empty;
            txtTarifa.Text = string.Empty;
            txtTarifaHoraAdicional.Text = string.Empty;
            txtManiobra.Text = string.Empty;
            hdnTarifaPSLID.Value = string.Empty;
            hdnDuracionDiasPeriodo.Value = string.Empty;
            hdnMaximoHrsTurno.Value = string.Empty;
            txtSeguro.Text = string.Empty;


            //Habilitar y deshabilitar controles
            txtNumeroEconomico.Enabled = false;
            txtVIN.Enabled = false;
            txtMarca.Enabled = false;
            txtUnidadSucursal.Enabled = false;
            txtModelo.Enabled = false;
            txtUnidadCapacidadTanque.Enabled = false;
            txtAnio.Enabled = false;
            txtUnidadPlacasEstales.Enabled = false;
            ddlPeriodoTarifa.Enabled = false;
            txtTarifa.Enabled = false;
            txtTarifaHoraAdicional.Enabled = false;
            if (this.ModoRegistro == "REN" || this.ModoRegistro == "EDEC") {
                txtManiobra.Enabled = false;
                if (this.ModoRegistro == "REN")
                    ddlPeriodoTarifa.Enabled = true;
            }
            txtSeguro.Enabled = false;
            txtPorcentajeSeguro.Enabled = false;
        }

        /// <summary>
        /// Deshabilita los campos para la vista del detalle
        /// </summary>
        public void PrepararVistaDetalle(bool activar = false) {
            if (!activar) {
                this.ddlPeriodoTarifa.Enabled = activar;
                this.txtManiobra.Enabled = activar;
            }
            this.ddlTipoTarifa.Enabled = activar;
            this.ddlTarifaTurno.Enabled = activar;
            this.btnPersonalizarTarifa.Visible = activar;
            this.btnAgregar.Visible = activar;
        }

        /// <summary>
        /// Configura la interfaz en modo Consultar
        /// </summary>
        public void ConfigurarModoConsultar() {

            btnAgregar.Enabled = false;
            lblTitulo.Text = "UNIDAD EN RENTA";

            txtAnio.Enabled = false;

            txtModelo.Enabled = false;
            txtNumeroEconomico.Enabled = false;

            txtVIN.Enabled = false;

        }
        /// <summary>
        /// Configura la interfaz en modo Editar
        /// </summary>
        public void ConfigurarModoEditar() {

            btnAgregar.Enabled = true;
            lblTitulo.Text = "UNIDAD A RENTAR";

            txtAnio.Enabled = false;

            txtModelo.Enabled = false;
            txtNumeroEconomico.Enabled = false;

            txtVIN.Enabled = false;
        }
        public void EstablecerOpcionesTipoTarifa(Dictionary<int, string> tipos) {
            ddlTipoTarifa.Items.Clear();
            ddlTipoTarifa.DataSource = tipos;
            ddlTipoTarifa.DataTextField = "Value";
            ddlTipoTarifa.DataValueField = "Key";
            ddlTipoTarifa.DataBind();
            ddlTipoTarifa.SelectedValue = "-1";
        }
        /// <summary>
        /// Método que se utiliza para llenar el combo de los turnos de las tarifas, dependiendo de la empresa que tenga la sesión activa
        /// </summary>
        public void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turnos) {
            ddlTarifaTurno.Items.Clear();
            ddlTarifaTurno.DataSource = turnos;
            ddlTarifaTurno.DataTextField = "Value";
            ddlTarifaTurno.DataValueField = "Key";
            ddlTarifaTurno.DataBind();
            ddlTarifaTurno.SelectedValue = "-1";
        }

        /// <summary>
        /// Método encargado de realizar el llenado del combo con los valores de los períodos de las tarifas
        /// </summary>
        public void EstablecerOpcionesPeriodoTarifa(Dictionary<string, string> periodos) {
            ddlPeriodoTarifa.Items.Clear();
            ddlPeriodoTarifa.DataSource = periodos;
            ddlPeriodoTarifa.DataTextField = "Value";
            ddlPeriodoTarifa.DataValueField = "Key";
            ddlPeriodoTarifa.DataBind();
            ddlPeriodoTarifa.SelectedValue = "-1";
        }

        public void EstablecerPaqueteNavegacion(string key, object value) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(NombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(NombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key) {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(NombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }
       
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ucLineaContratoPSLPRE(this);
                if (!Page.IsPostBack) {
                    presentador.Inicializar();
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        protected void ddlPeriodoTarifa_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (ddlTarifaTurno.SelectedValue != "-1") {
                    this.presentador.VerificarDescuentoATarifa();
                    this.presentador.CalcularPeriodoTarifa();
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".ddlPeriodoTarifa_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void ddlTarifaTurno_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (ddlTarifaTurno.SelectedValue != "-1") {
                    this.presentador.VerificarDescuentoATarifa();
                    this.presentador.CalcularMaximoHrsTurno();
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".ddlTarifaTurno_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void ddlTipoTarifa_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (ddlTarifaTurno.SelectedValue != "-1") {
                    this.presentador.VerificarDescuentoATarifa();
                    this.presentador.CalcularTarifas();
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".ddlTipoTarifa_SelectedIndexChanged: " + ex.Message);
            }
        }

        /// <summary>
        /// Regresa a la pantalla principal del contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Argumento</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                this.LimpiarSesion();
                presentador.Inicializar();
                if (CancelarClick != null) CancelarClick.Invoke(sender, e);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Agrega a unidad a la línea del contrato
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Argumento</param>
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

        /// <summary>
        /// Despliega en pantalla los equipos aliados de la unidad seleccionada
        /// </summary>
        /// <param name="dt">Tabla con la información de los equipos aliados</param>
        public void EstablecerEquiposAliadoUnidad(System.Data.DataTable dt) {
            this.grdEquiposAliados.DataSource = dt;
            this.grdEquiposAliados.DataBind();
        }
        #region Personalizar Tarifas
        /// <summary>
        /// Dialog para personalizar la tarifa por descuento o para subir el monto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPersonalizarTarifa_Click(object sender, EventArgs e) {
            string mensaje = presentador.ValidarCamposPrevioPersonalizarTarifa();
            if (string.IsNullOrEmpty(mensaje)) {
                presentador.PrepararDatosTarifaPersonalizada();
                string refreshMetod = "function ActualizarTarifaPersonalizada() { $(\"input[name$='btnActualizaTarifa']\").click(); }";
                ScriptManager.RegisterStartupScript(this, typeof(UserControl), "ActualizarTarifaPersonalizada", refreshMetod, true);
                string openDialog = "showDialogModal('TarifaPersonalizadaPSLUI.aspx', 'Personalizar Tarifa', '980px', '350px', 'ActualizarTarifaPersonalizada()');";
                ScriptManager.RegisterStartupScript(this, typeof(ucLineaContratoPSLUI), "TarifaPersonalizadaDialog", openDialog, true);
            } else
                MostrarMensaje(mensaje, ETipoMensajeIU.ADVERTENCIA);
        }
        /// <summary>
        /// Refresca la información de la tarifa de la UI, después de personalizar la tarifa por medio de las UI Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizaTarifa_Click(object sender, EventArgs e) {
            try {
                presentador.ActualizarTarifaPersonalizada();
                presentador.CalcularSeguro(this.Tarifa);
            } catch (Exception ex) {
                MostrarMensaje("Guardado No Exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #endregion
    }
}