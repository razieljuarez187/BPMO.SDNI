using System;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.Primitivos.Enumeradores;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using System.Globalization;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI {
    public partial class ReporteIngresoRentasUI : ReportPage, IReporteIngresoRentasVIS {

        /// <summary>
        /// Formato por default aplicado para los controles de fechas
        /// </summary>
        private const String DATETIME_FORMAT = "dd/MM/yyyy";

        #region Atributos
        private ReporteIngresoRentasPRE presentador;
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String nombreClase = typeof(ReporteIngresoRentasUI).Name;
        #endregion

        #region Propiedades
        public int? SucursalID {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        public string Sucursal {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        }

        public DateTime? FechaInicio {
            get {
                DateTime result;
                if (!String.IsNullOrEmpty(txtFechaInicio.Text) && DateTime.TryParseExact(this.txtFechaInicio.Text, DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out result)) {
                    return result;
                }
                return null;
            }
            set { this.txtFechaInicio.Text = value != null ? value.GetValueOrDefault().ToString(DATETIME_FORMAT) : string.Empty; }
        }

        public DateTime? FechaFin {
            get {
                DateTime result;
                if (!String.IsNullOrEmpty(txtFechaFin.Text) && DateTime.TryParseExact(this.txtFechaFin.Text, DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out result)) {
                    return result;
                }
                return null;
            }
            set { this.txtFechaFin.Text = value != null ? value.GetValueOrDefault().ToString(DATETIME_FORMAT) : string.Empty; }
        }

        public string URLImage {
            get {
                return Master.URLLogoEmpresa;
            }
        }

        public AdscripcionBO Adscripcion {
            get {
                return this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
            }
        }
        #endregion

        #region Metodos
        protected void Page_Load(object sender, EventArgs e) {
            this.presentador = new ReporteIngresoRentasPRE(this);
            if (!this.IsPostBack) {
                //Se definen los filtros Visibles                
                this.Master.SucursalFiltroVisible = true;               
            }
        }

        public override void Consultar() {
            try {
                if (!Fechas()) {
                    this.MostrarMensaje("Rango de fechas incorrecto. Se deben de capturar ambos campos de fechas.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                if (!ValidarRangoFechas()) {
                    this.MostrarMensaje("Rango de fechas incorrecto. La fecha final es menor que la fecha inicial.", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }
                this.presentador.Consultar();
            }
            catch(Exception e) {
                this.MostrarMensaje(e.Message, ETipoMensajeIU.ADVERTENCIA);
            }
        }

        /// <summary>
        /// Valida los rangos de fechas de dos campos de texto
        /// </summary>
        /// <param name="input1">Campo de texto 1</param>
        /// <param name="input2">Campo de texto 2</param>
        /// <returns>Devuelve true si las fechas de los campos son válidos</returns>
        private bool ValidarRangoFechas() {
            DateTime val = DateTime.MinValue;
            if (this.FechaInicio.HasValue && this.FechaFin.HasValue)
                return FechaInicio.Value <= FechaFin.Value;

            return true;
        }

        private bool Fechas() {
            bool valido = true;
            if (FechaInicio != null) {
                if (FechaFin == null)
                    valido = false;
            }
            else if (FechaFin != null) {
                valido = false;
            }

            return valido;
        }
        #endregion
    }
}