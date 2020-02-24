//Satisface al caso de uso CU003 - Consultar Contrato Renta Diaria
//Satisface a la solicitud de cambio SC0035
using System;
using System.Configuration;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI {
    public partial class ucResumenContratoPSLUI : System.Web.UI.UserControl, IucResumenContratoPSLVIS {
        #region Atributos

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucResumenContrato";

        #endregion

        #region Propiedades

        /// <summary>
        /// Presentador de Resumen Contrato
        /// </summary>
        internal ucResumenContratoPSLPRE Presentador { get; set; }

        /// <summary>
        /// Unidad Operativa del Usuario
        /// </summary>
        public UnidadOperativaBO UnidadOperativa {
            get {
                var masterMsj = (Site)Page.Master;
                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa;
                
                return null;
            }
        }

        /// <summary>
        /// Identificador del Modulo
        /// </summary>
        public int? ModuloID {
            get {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        /// <summary>
        /// Obtiene la Sucursal Seleccionada
        /// </summary>
        public string NombreSucursal {
            get {
                return txtSucursal.Text.Trim().ToUpper();
            }
            set {
                txtSucursal.Text = (value != null) ? value.Trim() : null;
            }
        }

        /// <summary>
        /// Identificador de la Empresa
        /// </summary>
        public int? EmpresaID {
            get {
                int? id = null;

                if (!string.IsNullOrEmpty(hdnEmpresaID.Value)) id = int.Parse(hdnEmpresaID.Value);

                return id;
            }
            set {
                hdnEmpresaID.Value = value == null ? null : value.ToString();
            }
        }

        /// <summary>
        /// Nombre de la Empresa
        /// </summary>
        public string NombreEmpresa {
            get {
                return txtEmpresa.Text.Trim().ToUpper();
            }
            set {
                txtEmpresa.Text = (value != null) ? value.Trim() : null;
            }
        }

        /// <summary>
        /// Fecha de Contrato
        /// </summary>
        public DateTime? FechaContrato {
            get {
                DateTime? fechaContrato = null;

                if (!string.IsNullOrEmpty(txtFechaContrato.Text.Trim()))
                    fechaContrato = DateTime.Parse(txtFechaContrato.Text.Trim());

                return fechaContrato;
            }
            set {
                txtFechaContrato.Text = (value != null) ? value.Value.ToString("dd/MM/yyyy") : null;
            }
        }

        /// <summary>
        /// Nombre del Cliente del Contrato
        /// </summary>
        public string NombreCuentaCliente {
            get { return txtCliente.Text.Trim().ToUpper(); }
            set { txtCliente.Text = value ?? null; }
        }

        /// <summary>
        /// RFC del Cliente del Contrato
        /// </summary>
        public string RFCCliente {
            get {
                return txtRFC.Text.Trim().ToUpper();
            }
            set {
                txtRFC.Text = (value != null) ? value.Trim() : null;
            }
        }

        /// <summary>
        /// Dirección del Cliente del Contrato
        /// </summary>
        public string DireccionCliente {
            get {
                return txtDireccion.Text.Trim().ToUpper();
            }
            set {
                txtDireccion.Text = (value != null) ? value.Trim() : null;
            }
        }

        /// <summary>
        /// Usuario de Logueado en el Sistema
        /// </summary>
        public UsuarioBO Usuario {
            get {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
                return null;
            }
        }

        #region SC0020

        /// <summary>
        /// Manejador de Evento para redirección a consultar con Filtros
        /// </summary>
        internal EventHandler RegresarConsultaFiltro { get; set; }

        #endregion SC0020

        /// <summary>
        /// Numero de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        public String NumeroCuentaCliente {
            get { return String.IsNullOrEmpty(this.txtNumeroCuentaOracle.Text) ? null : this.txtNumeroCuentaOracle.Text; }
            set { this.txtNumeroCuentaOracle.Text = value ?? String.Empty; }
        }

        #region SC0035
        /// <summary>
        /// Fecha de cierre de contrato
        /// </summary>
        public DateTime? FechaCierreContrato {
            get {
                DateTime? fechaCierreContrato = null;
                if (!string.IsNullOrEmpty(txtFechaCierreContrato.Text.Trim()))
                    fechaCierreContrato = DateTime.Parse(txtFechaCierreContrato.Text.Trim());

                return fechaCierreContrato;
            }
            set {
                txtFechaCierreContrato.Text = (value != null) ? value.Value.ToString("dd/MM/yyyy h:mm tt") : null;
            }
        }
        /// <summary>
        /// Frecuencia de facturación del contrato.
        /// </summary>
        public EFrecuencia? FrecuenciaFacturacion {
            get {
                EFrecuencia? frecuencia = null;
                if (!String.IsNullOrEmpty(this.txtFrecuenciaFacturacion.Text.Trim()))
                    frecuencia = (EFrecuencia)Enum.Parse(typeof(EFrecuencia), this.txtFrecuenciaFacturacion.Text.Trim());
                return frecuencia;
            }
            set {
                if (value == null)
                    this.txtFrecuenciaFacturacion.Text = string.Empty;
                else
                    this.txtFrecuenciaFacturacion.Text = value.ToString();
            }
        }
        #endregion

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e) {
            try {
                Presentador = new ucResumenContratoPSLPRE(this);
                this.txtFechaCierreContrato.Visible = false;
                this.lblFechaCierreContrato.Visible = false;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            var master = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR) {
                if (master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            } else {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        #endregion
    }
}