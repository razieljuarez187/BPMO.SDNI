//Satisface al caso de uso CU012 - Ver Pagos No Facturados
//Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    public partial class ConsultarPagosNoFacturadosUI : Page, IConsultarPagosNoFacturadosVIS
    {
        #region Atributos

        /// <summary>
        /// Master del la Pagina
        /// </summary>
        private IPagosMasterVIS master;

        /// <summary>
        /// Presentador de la UI
        /// </summary>
        private ConsultarPagosNoFacturadosPRE Presentador;

        /// <summary>
        /// Nombre de la clase de la pagina
        /// </summary>
        private const string NombreClase = "ConsultarPagosNoFacturadosUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// DepartamentoSeleccionado
        /// </summary>
        public EDepartamento? DepartamentoSeleccionado
        {
            get { return master.DepartamentoSeleccionado; }
        }

        /// <summary>
        /// Filtro por Fecha Inicial del Vencimiento
        /// </summary>
        public DateTime? FechaVencimientoInicio
        {
            get { return master.FechaVencimientoInicio; }
            set { master.FechaVencimientoInicio = value; }
        }

        /// <summary>
        /// Filtro por Fecha Final del Vencimiento
        /// </summary>
        public DateTime? FechaVencimientoFin
        {
            get { return master.FechaVencimientoFin; }
            set { master.FechaVencimientoFin = value; }
        }

        /// <summary>
        /// Listado de Pagos Consultados
        /// </summary>
        public List<PagoUnidadContratoBO> PagosConsultados
        {
            get { return master.PagosConsultados; }
            set { master.PagosConsultados = value; }
        }

        /// <summary>
        /// Listado de Pagos Consultados PSL
        /// </summary>
        public List<PagoContratoPSLBO> PagosConsultadosPSL
        {
            get { return master.PagosConsultadosPSL; }
            set { master.PagosConsultadosPSL = value; }
        }

        /// <summary>
        /// Nombre de la Sucursal Seleccionada
        /// </summary>
        public string SucursalNombre {
            get { return master.SucursalNombre; }
        }

        /// <summary>
        /// Sucursal Seleccionada
        /// </summary>
        public int? SucursalSeleccionadaID {
            get { return master.SucursalSeleccionadaID; }
        }

        /// <summary>
        /// Sucursales permitidas para el usuario
        /// </summary>
        public List<SucursalBO> SucursalesUsuario
        {
            get { return master.SucursalesUsuario; }
            set { master.SucursalesUsuario = value; }
        }

        /// <summary>
        /// Identificador de la Unidad Operativa del Usuario
        /// </summary>
        public int? UnidadOperativaID
        {
            get { return master.UnidadOperativaID; }
        }

        /// <summary>
        /// Identificador del Usuario en Session
        /// </summary>
        public int? UsuarioID
        {
            get { return master.UsuarioID; }
        }

        #region SC0035
        /// <summary>
        /// Número de contrato seleccionado.
        /// </summary>
        public string NumeroContrato
        {
            get { return master.NumeroContrato; }
            set { master.NumeroContrato = value; }
        }
        /// <summary>
        /// Identificador de la Cuenta Cliente
        /// </summary>
        public int? CuentaClienteID
        {
            get { return master.CuentaClienteID; }
            set { master.CuentaClienteID = value; }
        }
        /// <summary>
        /// Nombre del Cliente seleccionado.
        /// </summary>
        public string NombreCuentaCliente
        {
            get { return master.NombreCuentaCliente; }
            set { master.NombreCuentaCliente = value; }
        }
        /// <summary>
        /// Numero económico o vin a consultar.
        /// </summary>
        public string VinNumeroEconomico
        {
            get { return master.VinNumeroEconomico; }
            set { master.VinNumeroEconomico = value; }
        }
        #endregion

        #region Cancelación de pagos

        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Pagos)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Adscripción de la UOID
        /// </summary>
        public AdscripcionBO Adscripcion
        {
            get
            {
                return master.Adscripcion;
            }
        }
        /// <summary>
        /// Identificador del Pago a cancelar
        /// </summary>
        public int? PagoACancelarID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnPagoACancelarID.Value) && !string.IsNullOrWhiteSpace(this.hdnPagoACancelarID.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnPagoACancelarID.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnPagoACancelarID.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o Establece el código de autorización
        /// </summary>
        public string CancelaPagoCodigoAutorizacion
        {
            get
            {
                return string.IsNullOrEmpty(hdnCodigoAutorizacion.Value.Trim()) ? null : hdnCodigoAutorizacion.Value;
            }
            set
            {
                hdnCodigoAutorizacion.Value = value ?? "";
                txtCancelaPagoCodigoAutorizacion.Text = "";
            }
        }
        /// <summary>
        /// Obtiene o Establece el motivo de la cancelación
        /// </summary>
        public string MotivoCancelarPago
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtMotivoCancelacion.Text)) ? null : this.txtMotivoCancelacion.Text.Trim().ToUpper();
            }
            set
            {
                this.txtMotivoCancelacion.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        #endregion
        #endregion Propiedades

        #region Métodos

        /// <summary>
        /// Establece como seleccionado el marcador de Pagos a Facturar
        /// </summary>
        public void MarcarPagosFacturar()
        {
            master.MarcarPagosFacturar();
        }
        /// <summary>
        /// Marca como seleccionado el Marcador de Pagos no Facturados
        /// </summary>
        public void MarcarPagoNoFacturados()
        {
            master.MarcarPagoNoFacturados();
        }
        /// <summary>
        /// Limpia la session del modulo.
        /// </summary>
        public void LimpiarSession()
        {
            PagosConsultados = null;
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
            Session.Remove("CU005_Pago");
        }

        public void Consultar()
        {
            Presentador.Consultar();
        }

        /// <summary>
        /// Carga en la interfaz de usuario los pagos consultados
        /// </summary>
        public void CargarPagosConsultados()
        {
            if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.UnidadOperativaID)
                grdPagos.DataSource = PagosConsultados;
            else
                grdPagos.DataSource = PagosConsultadosPSL;
            grdPagos.DataBind();
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo)
        {
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle)
        {
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
        /// Redirecciona a PaginaSinAccesoUI en caso no se tengan permisos para la pagina
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Permite Habilitar Mover los pagos no facturados a la bandeja de pagos por facturar
        /// </summary>
        /// <param name="permitir"></param>
        public void PermitirMover(bool permitir)
        {
            grdPagos.Columns[8].Visible = permitir;
        }
        /// <summary>
        /// Permite la cancelación del pago
        /// </summary>
        /// <param name="permitir">Indica se se permite o no visualizar la columna de cancelación de pago</param>
        public void PermitirCancelarPago(bool permitir)
        {
            grdPagos.Columns[7].Visible = permitir;
        }
        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }

        /// <summary>
        /// Habilita o deshabilita la opción de Validar Código de Autorización
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirValidarCodigoAutorizacion(bool permitir)
        {
            btnValidarAutorizacion.Enabled = permitir;

            txtCancelaPagoCodigoAutorizacion.Enabled = permitir;
            txtCancelaPagoCodigoAutorizacion.ReadOnly = !permitir;

        }

        /// <summary>
        /// Habilita o deshabilita la opción de Solicitar Código de Autorización
        /// </summary>
        /// <param name="permitir">Parámetro que controla el estatus del control</param>
        public void PermitirSolicitarCodigoAutorizacion(bool permitir)
        {

            btnSolicitarAutorizacion.Enabled = permitir;

            txtMotivoCancelacion.Enabled = permitir;
            txtMotivoCancelacion.ReadOnly = !permitir;
        }
        /// <summary>
        /// Se actualiza la cantidad de pagos Facturados/NoFacturados cuando se envía un pago a Por-Facturar
        /// </summary>
        public void ActualizarMarcadoresEnviarAFacturar() {
            master.TotalFacturar++;
            master.TotalNoFacturado--;
        }
        #endregion Métodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                master = Page.Master as IPagosMasterVIS;
                Presentador = new ConsultarPagosNoFacturadosPRE(this);
                master.AccionConsultar = Consultar;

                if (!Page.IsPostBack)
                    Presentador.PrimeraCarga();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
                    NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void grdPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.UnidadOperativaID)
                    grdPagos.DataSource = PagosConsultados;
                else
                    grdPagos.DataSource = PagosConsultadosPSL;
                grdPagos.PageIndex = e.NewPageIndex;
                grdPagos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR,
                    NombreClase + ".grdPagos_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var lblFolio = e.Row.FindControl("lblFolio") as Label;
                    var lblCliente = e.Row.FindControl("lblCliente") as Label;                    
                    var lblFechaInicio = e.Row.FindControl("lblFechaInicio") as Label;
                    var lblReferencia = e.Row.FindControl("lblReferencia") as Label;
                    var lblPlazoPago = e.Row.FindControl("lblPagoPlazo") as Label;
                    var lblDemora = e.Row.FindControl("lblDemora") as Label;
                    var lblTotalFactura = e.Row.FindControl("lblTotalFactura") as Label;
                    var btnMover = e.Row.FindControl("btnMover") as ImageButton;
                    var btnCancelar = e.Row.FindControl("btnCancelar") as ImageButton; 

                    if (e.Row.DataItem is PagoContratoPSLBO)
                    {
                        var bof = e.Row.DataItem as PagoContratoPSLBO;
                        if (bof != null)
                        {
                            if (lblFolio != null)
                                lblFolio.Text = bof.ReferenciaContrato.FolioContrato;
                            if (lblCliente != null)
                                lblCliente.Text = bof.ReferenciaContrato.CuentaCliente.Numero + ": " + bof.ReferenciaContrato.CuentaCliente.Nombre;
                            if (lblFechaInicio != null)
                                lblFechaInicio.Text = bof.ReferenciaContrato.FechaInicio.Value.ToShortDateString();
                            if (lblReferencia != null)
                                lblReferencia.Text = bof.Referencia;
                            if (lblPlazoPago != null)
                                lblPlazoPago.Text = string.Format("{0:00}/{1:00}", bof.NumeroPago, bof.ReferenciaContrato.Plazo);
                            if (lblDemora != null)
                            {
                                TimeSpan? ts = DateTime.Today - bof.FechaVencimiento;
                                lblDemora.Text = ((TimeSpan)ts).Days.ToString();
                            }
                            if (lblTotalFactura != null)
                                lblTotalFactura.Text = string.Format("{0:#,##0.00} {1}", bof.TotalFactura.GetValueOrDefault(), bof.Divisa.MonedaDestino.Codigo);
                            if (btnMover != null)
                                btnMover.CommandArgument = bof.PagoContratoID.ToString();
                            if (btnCancelar != null)
                            {
                                //Validar la visualización por permiso si es el último pago
                                if (bof.UltimoPago.HasValue && bof.UltimoPago.Value)
                                {
                                    btnCancelar.Visible = true;
                                    btnCancelar.CommandArgument = bof.PagoContratoID.ToString();
                                }
                            }
                        }
                    }
                    else
                    {                        
                        var bof = e.Row.DataItem as PagoUnidadContratoBO;
                        if (bof != null)
                        {
                            if (lblFolio != null)
                                lblFolio.Text = bof.ReferenciaContrato.FolioContrato;
                            if (lblCliente != null)
                                lblCliente.Text = bof.ReferenciaContrato.CuentaCliente.Numero + ": " + bof.ReferenciaContrato.CuentaCliente.Nombre;
                            if (lblFechaInicio != null)
                                lblFechaInicio.Text = bof.ReferenciaContrato.FechaInicio.Value.ToShortDateString();
                            if (lblReferencia != null)
                                lblReferencia.Text = bof.Referencia;
                            if (lblPlazoPago != null)
                                lblPlazoPago.Text = string.Format("{0:00}/{1:00}", bof.NumeroPago, bof.ReferenciaContrato.Plazo);
                            if (lblDemora != null)
                            {
                                TimeSpan? ts = DateTime.Today - bof.FechaVencimiento;
                                lblDemora.Text = ((TimeSpan)ts).Days.ToString();
                            }
                            if (lblTotalFactura != null)
                                lblTotalFactura.Text = string.Format("{0:#,##0.00} {1}", bof.TotalFactura.GetValueOrDefault(), bof.Divisa.MonedaDestino.Codigo);
                            if (btnMover != null)
                                btnMover.CommandArgument = bof.PagoID.ToString();
                            //Visibilidad del botón cancelar por default no aplica para Idealease
                            if (btnCancelar != null)
                                btnCancelar.Visible = false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR,
                    NombreClase + ".grdPagos_RowDataBound: " + ex.Message);
            }
        }

        protected void btnMover_Click(object sender, EventArgs e) {
            try {
                var btn = sender as ImageButton;
                if (btn != null) {
                    int pagoID = int.Parse(btn.CommandArgument);
                    Presentador.MoverAFacturar(pagoID);
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al Mover el Pago.", ETipoMensajeIU.ERROR, NombreClase + ".btnMover_Click" + ex.Message);
            }
        }

        #region Cancelar pagos a facturar
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as ImageButton;
                if (btn != null)
                {
                    Int32 pagoId = int.Parse(btn.CommandArgument);
                    switch (this.UnidadOperativaID)
                    {
                        case (int)ETipoEmpresa.Generacion:
                        case (int)ETipoEmpresa.Construccion:
                        case (int)ETipoEmpresa.Equinova:
                            Presentador.PrepararDialogo();
                            Presentador.EstablecerPagoACancelar(pagoId);
                            this.RegistrarScript("CancelarSolicitudPago", "confirmarCancelarPago();");
                            break;
                        case (int)ETipoEmpresa.Idealease:
                        default:
                            throw new Exception("No es permitido elimina pagos en esta unidad operativa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al procesar el pago a cancelar.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click" + ex.Message);
            }
        }
        protected void btnValidarAutorizacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presentador.ValidarCodigoAutorizacion(txtCancelaPagoCodigoAutorizacion.Text.Trim()))
                {
                    Presentador.CancelarPago();
                    Presentador.PrepararDialogo();
                    RegistrarScript(null, "MostrarDialogo(false);");
                }
                else
                    MostrarMensaje("El c&oacute;digo de autorizaci&oacute;n es inv&aacute;lido, por favor verifique", ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un Error al validar el código de autorización", ETipoMensajeIU.INFORMACION);
            }
        }

        protected void btnSolicitarAutorizacion_Click(object sender, EventArgs e)
        {
            try
            {
                PermitirSolicitarCodigoAutorizacion(false);
                Presentador.SolicitarAutorizacion();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al solicitar el código de autorización", ETipoMensajeIU.ERROR, ex.Message);
                Presentador.PrepararDialogo();
                hdnMostrarDialogoPago.Value = "0";
            }
        }

        protected void btnDescartarCancelacionPago_Click(object sender, EventArgs e)
        {
            Presentador.PrepararDialogo();
            hdnMostrarDialogoPago.Value = "0";

        }

        protected void btnCancelarPagoPendiente_Click(object sender, EventArgs e)
        {
            try
            {
                RegistrarScript(null, "MostrarDialogo(true);");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar el pago.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelarPagoPendiente_Click:" + ex.Message);
            }
        }
        #endregion

        #endregion Eventos

    }
}