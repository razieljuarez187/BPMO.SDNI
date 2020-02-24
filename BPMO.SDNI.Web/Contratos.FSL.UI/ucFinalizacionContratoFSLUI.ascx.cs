//Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucFinalizacionContratoFSLUI : System.Web.UI.UserControl, IucFinalizacionContratoFSLVIS
    {

        #region Atributos

        internal ucFinalizacionContratoFSLPRE Presentador;
        #endregion

        #region Propiedades

        public DateTime? FechaFinContrato
        {
            get
            {
                if (string.IsNullOrEmpty(hdnFechaFinContrato.Value))
                    return null;
                return Convert.ToDateTime(hdnFechaFinContrato.Value);
            }
            set { hdnFechaFinContrato.Value = value == null ? "" : ((DateTime)value).ToShortDateString(); }
        }

        public DateTime? FechaCierre
        {
            get
            {
                if (string.IsNullOrEmpty(txtFechaCierre.Text.Trim()))
                    return null;
                return Convert.ToDateTime(txtFechaCierre.Text.Trim());
            }
            set { txtFechaCierre.Text = value == null ? "" : ((DateTime)value).ToShortDateString(); }
        }

        public string ObservacionesCierre
        {
            get
            {
                if (string.IsNullOrEmpty(txtObservacionesCierre.Text))
                    return null;
                return txtObservacionesCierre.Text.Trim().ToUpper();
            }
            set { txtObservacionesCierre.Text = value ?? ""; }
        }

        public decimal? Penalizacion
        {
            get
            {
                if (string.IsNullOrEmpty(txtPenalizacion.Text))
                    return null;
                return Convert.ToDecimal(txtPenalizacion.Text.Trim().Replace(",", ""));
            }
            set { txtPenalizacion.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; }
        }

        public DateTime? FechaInicioContrato
        {
            get
            {
                if (string.IsNullOrEmpty(hdnFechaInicioContrato.Value))
                    return null;
                return Convert.ToDateTime(hdnFechaInicioContrato.Value);
            }
            set { hdnFechaInicioContrato.Value = value == null ? "" : ((DateTime)value).ToShortDateString(); }
        }

        public bool? ModoEdicion
        {
            get
            {
                if (string.IsNullOrEmpty(hdnModoEdicion.Value))
                    return null;
                return Convert.ToBoolean(hdnModoEdicion.Value);
            }
            set { hdnModoEdicion.Value = value == null ? "" : value.ToString(); }
        }

        public decimal? Mensualidad
        {
            get
            {
                if (string.IsNullOrEmpty(hdnMensualidad.Value))
                    return null;
                return Convert.ToDecimal(hdnMensualidad.Value);
            }
            set { hdnMensualidad.Value = value == null ? "" : value.ToString(); }
        }

        public int? Plazo
        {
            get
            {
                if (string.IsNullOrEmpty(hdnPlazoMeses.Value))
                    return null;
                return Convert.ToInt32(hdnPlazoMeses.Value);
            }
            set { hdnPlazoMeses.Value = value == null ? "" : value.ToString(); }
        }

        public decimal? PorcentajePenalizacion
        {
            set { hdnPorcentajePenalizacion.Value = value == null ? "" : value.ToString(); }
            get
            {
                if (string.IsNullOrEmpty(hdnPorcentajePenalizacion.Value)) return null;
                return Convert.ToDecimal(hdnPorcentajePenalizacion.Value.Trim());
            }
        }

        public string MotivoCierreAnticipado
        {
            get
            {
                if (!string.IsNullOrEmpty(txtMotivo.Text.Trim()))
                    return txtMotivo.Text.Trim().ToUpper();
                return null;
            }
            set { txtMotivo.Text = value ?? ""; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            Presentador = new ucFinalizacionContratoFSLPRE(this);

            if (!this.IsPostBack)
                this.txtObservacionesCierre.Attributes.Add("onkeyup", "checkText(this,500);");
        }
        #endregion

        #region Métodos

        public void ObservacionObligatoria(bool obligatorio)
        {
            if (obligatorio) lblObservaciones.Text = "*";
            else lblObservaciones.Text = "";
        }

        public void MostrarPenalizacion(bool mostrar)
        {
            trPenalizacion.Visible = mostrar;
        }

        public void MostrarMotivos(bool mostrar)
        {
            trMotivo.Visible = mostrar;
        }

        /// <summary>
        /// Despliega mensajes en la Interfaz de Usuario
        /// </summary>
        /// <param name="mensaje">Mensaje a Desplegar</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Detalle o submensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// configura la interfaz de usuario para el modo edición
        /// </summary>
        public void ConfigurarModoEdicion()
        {
            txtObservacionesCierre.Enabled = true;
            txtMotivo.Enabled = true;
        }

        /// <summary>
        /// Configura la interfaz de usuario para el modo consulta
        /// </summary>
        public void ConfigurarModoConsulta()
        {
            txtObservacionesCierre.Enabled = false;
            txtMotivo.Enabled = false;
        }

        #endregion

        #region Eventos
        protected void txtFechaCierre_TextChanged(object sender, EventArgs e)
        {
            Presentador.ValidarFechaCierre();
        }
        #endregion

    }
}