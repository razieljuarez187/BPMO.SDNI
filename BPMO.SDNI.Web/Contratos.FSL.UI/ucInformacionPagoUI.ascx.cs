// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
// Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing  
using System;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucInformacionPagoUI : UserControl, IucInformacionPagoVIS
    {
        #region Atributos

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucInformacionPagoUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// Presentador de Informacion de Pago
        /// </summary>
        internal ucInformacionPagoPRE Presentador { get; set; }

        /// <summary>
        /// Manejador del Evento cuando cambia la fecha de Inicio
        /// </summary>
        public EventHandler CambiarFechaInicioContrato { get; set; }
        
        /// <summary>
        /// Fecha de Inicio de Contrato
        /// </summary>
        public DateTime? FechaInicioContrato
        {
            get
            {
                DateTime? inicioContrato = null;

                if (!string.IsNullOrEmpty(txtFechaInicio.Text.Trim()))
                    inicioContrato = DateTime.Parse(txtFechaInicio.Text.Trim());

                return inicioContrato;
            }
            set
            {
                txtFechaInicio.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        /// <summary>
        /// Fecha de Terminacion del Contrato
        /// </summary>
        public DateTime? FechaTerminacionContrato
        {
            get
            {
                DateTime? finContrato = null;

                if (!string.IsNullOrEmpty(txtFechaFin.Text.Trim()))
                    finContrato = DateTime.Parse(txtFechaFin.Text.Trim());

                return finContrato;
            }
            set
            {
                txtFechaFin.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }        

        #region Cuenta Bancaria
        ///// <summary>
        ///// Nombre del Beneficiario de la Cuenta Bancaria Seleccionada
        ///// </summary>
        //public string Beneficiario
        //{
        //    get
        //    {
        //        return txtBeneficiario.Text.Trim();
        //    }
        //    set
        //    {
        //        txtBeneficiario.Text = value;
        //    }
        //}
        ///// <summary>
        ///// Lugar de la Cuenta Bancaria
        ///// </summary>
        //public string Lugar
        //{
        //    get
        //    {
        //        return txtLugar.Text.Trim();
        //    }
        //    set
        //    {
        //        txtLugar.Text = value;
        //    }
        //}
        ///// <summary>
        ///// Banco al que pertenece la cuenta bancaria
        ///// </summary>
        //public string Banco
        //{
        //    get
        //    {
        //        return txtBanco.Text.Trim();
        //    }
        //    set
        //    {
        //        txtBanco.Text = value;
        //    }

        //}
        ///// <summary>
        ///// Cuenta Bancaria Seleccionada
        ///// </summary>
        //public Facade.SDNI.BO.Banco CuentaBancariaSeleccionada
        //{
        //    get
        //    {
        //        return ddlCuentaBancaria.SelectedIndex != 0 ? new Facade.SDNI.BO.Banco() : null;
        //    }
        //}

        ///// <summary>
        ///// Listado de Cuentas Bancarias
        ///// </summary>
        //public List<Facade.SDNI.BO.Banco> ListadoCuentasBancarias
        //{
        //    get
        //    {
        //        return (List<Facade.SDNI.BO.Banco>)ddlCuentaBancaria.DataSource;
        //    }
        //    set
        //    {
        //        List<Facade.SDNI.BO.Banco> listValue = value ?? new List<Facade.SDNI.BO.Banco>();

        //        // Clonar la Lista para no afectar la lista original
        //        var lista = new List<Facade.SDNI.BO.Banco>(listValue);
        //        // Agregar el SucursalBO de fachada
        //        //Limpiar el DropDownList Actual
        //        ddlCuentaBancaria.Items.Clear();
        //        // Asignar Lista al DropDownList
        //        ddlCuentaBancaria.DataTextField = "NumeroCuenta";
        //        ddlCuentaBancaria.DataValueField = "Id";
        //        ddlCuentaBancaria.DataSource = lista;
        //        ddlCuentaBancaria.DataBind();
        //    }
        //}
        #endregion

        /// <summary>
        /// Pago Total del Contrato
        /// </summary>
        public decimal? Total
        {
            set
            {
				txtTotal.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI0012
            }
        }

        /// <summary>
        /// Pago Mensual del Contrato
        /// </summary>
        public decimal? Mensualidad
        {
            set
            {
				txtMensualidad.Text = value != null ? string.Format("{0:#,##0.0000}", value) : string.Empty; //RI0012
            }
        }

        /// <summary>
        /// Dias para realizar el Pago
        /// </summary>
        public int? DiasPago
        {
            get
            {
                if (txtDiasPago.Text == null || string.IsNullOrEmpty(txtDiasPago.Text.Trim()))
                    return null;
                return int.Parse(txtDiasPago.Text.Trim().Replace(",","")); //RI0012
            }
            set
            {
				txtDiasPago.Text = (value != null) ? string.Format("{0:#,##0}", value) : string.Empty; //RI0012
            }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Configura la Fachada de acuerdo al modo de vista Editar
        /// </summary>
        public void ConfigurarModoEditar()
        {
            ddlCuentaBancaria.Enabled = true;
            txtBanco.Enabled = false;
            txtBeneficiario.Enabled = false;
            txtFechaFin.Enabled = false;
            txtFechaInicio.Enabled = true;
            txtLugar.Enabled = false;
            txtMensualidad.Enabled = false;
            txtTotal.Enabled = false;
            txtDiasPago.Enabled = true;
        }

        /// <summary>
        /// Configura la Fachada de acuerdo al modo de vista Consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {
            ddlCuentaBancaria.Enabled = false;
            txtBanco.Enabled = false;
            txtBeneficiario.Enabled = false;
            txtFechaFin.Enabled = false;
            txtFechaInicio.Enabled = false;
            txtLugar.Enabled = false;
            txtFechaInicio.Enabled = false;
            txtMensualidad.Enabled = false;
            txtTotal.Enabled = false;
            txtDiasPago.Enabled = false;
        }

        #region Cuenta Bancaria
        ///// <summary>
        ///// Establece la Cuenta Bancaria Seleccionada
        ///// </summary>
        ///// <param name="Id"></param>
        //public void EstablecerCuentaBancariaSeleccionada(int? Id)
        //{
        //    if (Id != null)
        //        ddlCuentaBancaria.SelectedValue = Id.Value.ToString(CultureInfo.InvariantCulture);
        //    else
        //        ddlCuentaBancaria.SelectedIndex = 0;
        //}
        #endregion

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
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

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucInformacionPagoPRE(this);

                txtFechaFin.Attributes.Add("readonly", "true");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void txtFechaInicio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (CambiarFechaInicioContrato != null) CambiarFechaInicioContrato.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar la fecha de inicio.", ETipoMensajeIU.ERROR, NombreClase + ".txtFechaInicio_TextChanged:" + ex.Message);
            }
        }

        #endregion
    }
}