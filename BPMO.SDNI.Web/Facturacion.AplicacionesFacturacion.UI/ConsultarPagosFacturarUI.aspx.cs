// Satisface al caso de uso CU006 - Ver Histórico de Pagos
// Satisface al caso de uso CU004 - Consulta de Pagos a Facturar
// Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using System.Drawing;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    public partial class ConsultarPagosFacturarUI : Page, IConsultarPagosFacturarVIS
    {
        #region Atributos

        #region CU004 Consulta de Pagos a Facturar
        /// <summary>
        /// Master de la Pagina
        /// </summary>
        private IPagosMasterVIS master; 
        #endregion

        #region CU006 Ver Historico de Pagos

        /// <summary>
        /// Presentador de la UI
        /// </summary>
        private ConsultarPagosFacturarPRE Presentador;
        /// <summary>
        /// Nombre de la clase de la pagina
        /// </summary>
        private const string NombreClase = "ConsultarPagosFacturarUI"; 
        
        #endregion
        #endregion
        
        #region Propiedades

        #region CU006 - Ver Historico de Pagos
        /// <summary>
        /// Unidad Operativa del Usuario
        /// </summary>
        public UnidadOperativaBO UnidadOperativa { get { return new UnidadOperativaBO { Id = master.UnidadOperativaID }; } }

        /// <summary>
        /// Usuario de Ultima Actualización
        /// </summary>
        public int? UC { get { return master.UsuarioID; } }

        /// <summary>
        /// Referencia de Contrato Selecionada
        /// </summary>
        public ReferenciaContratoBO ReferenciaContratoSeleccionada { get; set; }
        
        #endregion

        #region CU004 - Consulta de Pagos a Facturar
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
        
        public List<PagoUnidadContratoBO> UltimosPagos
        {
            get { return (List<PagoUnidadContratoBO>)Session["UltimosPagos"]; }
            set { Session["UltimosPagos"] = value; }
        }

        public List<PagoContratoPSLBO> UltimosPagosPSL
        {
            get { return (List<PagoContratoPSLBO>)Session["UltimosPagos"]; }
            set { Session["UltimosPagos"] = value; }
        }
        #endregion

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
        /// URL del Logotipo de la Unidad Operativa
        /// </summary>
        public string URLLogoEmpresa {
            get { return ((Pagos)Page.Master).URLLogoEmpresa; }
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

        #endregion

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                master = Page.Master as IPagosMasterVIS;
                Presentador = new ConsultarPagosFacturarPRE(this);
                if (master != null) master.AccionConsultar = Consultar;
                if (!Page.IsPostBack)
                {
                    Presentador.PrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
                    NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        #endregion

        #region Metodos

        #region CU004 Consulta de Pagos a Facturar
        /// <summary>
        /// Realiza la acción de consultar los pagos a facturar
        /// </summary>
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

            var columnas = grdPagos.Columns.Cast<DataControlField>();
            foreach(var columna in columnas.Where(x=>x.HeaderText.ToUpper() == "VENCIMIENTO"))
            {
                if (ETipoEmpresa.Idealease == (ETipoEmpresa)this.UnidadOperativaID)
                {
                    if (PagosConsultados.Any(y => y.FechaVencimiento != null) &&
                        PagosConsultados.Any(x => new DateTime(x.FechaVencimiento.Value.Year, x.FechaVencimiento.Value.Month, x.FechaVencimiento.Value.Day, 0, 0, 0) < DateTime.Today))
                        columna.HeaderStyle.BackColor = Color.Red;
                    else
                        grdPagos.CssClass = "Grid";
                }
                else
                {
                    if (PagosConsultadosPSL.Any(y => y.FechaVencimiento != null) &&
                       PagosConsultadosPSL.Any(x => new DateTime(x.FechaVencimiento.Value.Year, x.FechaVencimiento.Value.Month, x.FechaVencimiento.Value.Day, 0, 0, 0) < DateTime.Today))
                        columna.HeaderStyle.BackColor = Color.Red;
                    else
                        grdPagos.CssClass = "Grid";
                }
            }
        }        
        /// <summary>
        /// Redirecciona a Configurar Facturación (CU005)
        /// </summary>
        public void IrAConfigurarFacturacion(bool pagoPSL)
        {
            if(pagoPSL)
                Response.Redirect(ResolveUrl("~/Facturacion.AplicacionesFacturacion.UI/ConfigurarFacturacionPSLUI.aspx"), true);
            else
                Response.Redirect(ResolveUrl("~/Facturacion.AplicacionesFacturacion.UI/ConfigurarFacturacionUI.aspx"), true);
        }
        /// <summary>
        /// Establece el codigo de navegacion para el modulo de configuración de facturación
        /// </summary>        
        /// <param name="paqueteNavegacion"></param>
        public void EstablecerPaqueteNavegacionFacturacion(object paqueteNavegacion)
        {
            Session["CU005_Pago"] = paqueteNavegacion;
        }
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
            master.MarcarPagosFacturar();
        }
        /// <summary>
        /// Limpia la session del modulo.
        /// </summary>
        public void LimpiarSession()
        {
            PagosConsultados = null;
            Session.Remove("UltimosPagos");
            Session.Remove("NombreReporte");
            Session.Remove("DatosReporte");
            Session.Remove("CU005_Pago");
        }
        /// <summary>
        /// Permite la generacion del historico de pagos
        /// </summary>
        /// <param name="permitir">Indica si se permite o no la generación</param>
        public void PermitirHistorico(bool permitir)
        {
            hdnPODHP.Visible = permitir;
        }
        /// <summary>
        /// Redirecciona a PaginaSinAccesoUI en caso no se tengan permisos para la pagina
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Permite redireccionar a la configuración de facturación
        /// </summary>
        /// <param name="permitir">Indica se se permite o no la generación</param>
        public void PermitirFacturar(bool permitir)
        {
            hdnPCF.Visible = permitir;
        }
        /// <summary>
        /// Permite la cancelación del pago
        /// </summary>
        /// <param name="permitir">Indica se se permite o no visualizar la columna de cancelación de pago</param>
        public void PermitirCancelarPago(bool permitir)
        {
            this.grdPagos.Columns[6].Visible = permitir;
        }
        #endregion

        #region CU006 Ver Historico de Pagos
        public void IrAImprimirHistorico()
        {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", true);
        }

        public void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, object paqueteNavegacion)
        {
            Session["NombreReporte"] = codigoNavegacion;
            Session["DatosReporte"] = paqueteNavegacion;
        }

        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
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

        #endregion
        #endregion

        #region Eventos

        #region CU006 Ver CHistorico de Pagos
        protected void btnVerHistorico_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as ImageButton;
                if (btn != null)
                {
                    int pagoID = int.Parse(btn.CommandArgument);
                    Presentador.ImprimirHistoricoPagos(pagoID);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al Imprimir el Histórico de Pagos.", ETipoMensajeIU.ERROR, NombreClase + ".btnVerHistorico_Click" + ex.Message);
            }
        } 
        #endregion

        #region CU004 Consulta de Pagos a Facturar
        protected void btnConfigurar_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as ImageButton;
                if (btn != null)
                {
                    Int32 pagoId = int.Parse(btn.CommandArgument);
                    switch(this.UnidadOperativaID)
                    {
                        case (int)ETipoEmpresa.Generacion:
                        case (int)ETipoEmpresa.Construccion:
                        case (int)ETipoEmpresa.Equinova:
                            this.ConfigurarPSL(pagoId);
                            break;
                        case (int)ETipoEmpresa.Idealease:
                        default:
                            this.Configurar(pagoId);
                            break;
                    }                      
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al obtener el pago a presentar.", ETipoMensajeIU.ERROR, NombreClase + ".btnConfigurar_Click" + ex.Message);
            }
        }
        /// <summary>
        /// Envía al Wizard de facturación de Idealease
        /// </summary>
        /// <param name="pagoId">Identificador del pago</param>
        protected void Configurar(int pagoId)
        {
            try
            {
                var pagoSeleccionado = PagosConsultados.Where(p => p.PagoID == pagoId).FirstOrDefault();

                if (Presentador.EsPagoValido(pagoSeleccionado))
                    Presentador.ConfigurarFacturacion(pagoId);
                else
                    MostrarMensaje("Este pago no se puede enviar a facturar porque existen pagos vencidos que no han sido enviados", ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al obtener el pago a presentar.", ETipoMensajeIU.ERROR, NombreClase + ".btnConfigurar_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Envía al Wizard de facturación de generación y construcción
        /// </summary>
        /// <param name="pagoId">Identificador del pago</param>
        protected void ConfigurarPSL(int pagoId)
        {
            try
            {
                var pagoSeleccionado = PagosConsultadosPSL.Where(p => p.PagoContratoID == pagoId).FirstOrDefault();
                if (Presentador.EsPagoValido(pagoSeleccionado))
                    Presentador.ConfigurarFacturacionPSL(pagoId);
                else
                    MostrarMensaje("Este pago no se puede enviar a facturar porque existen pagos vencidos que no han sido enviados", ETipoMensajeIU.ADVERTENCIA);
              
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al obtener el pago a presentar.", ETipoMensajeIU.ERROR, NombreClase + ".btnConfigurar_Click" + ex.Message);
            }
        }
        #endregion
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
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, NombreClase + ".grdPagos_PageIndexChanging: " + ex.Message);
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
                    var lblReferencia = e.Row.FindControl("lblReferencia") as Label;
                    var lblFechaInicio = e.Row.FindControl("lblFechaInicio") as Label;
                    var lblPlazoPago = e.Row.FindControl("lblPagoPlazo") as Label;
                    var lblFechaVencimiento = e.Row.FindControl("lblFechaVencimiento") as Label;
                    var btnVer = e.Row.FindControl("btnVer") as ImageButton;
                    var btnConfigurar = e.Row.FindControl("btnConfigurar") as ImageButton;
                    var btnCancelar = e.Row.FindControl("btnCancelar") as ImageButton;     

                    if (e.Row.DataItem is PagoContratoPSLBO)
                    {
                        var bof = e.Row.DataItem as PagoContratoPSLBO;
                        if (bof != null)
                        {
                            if (lblFolio != null)
                                lblFolio.Text = bof.ReferenciaContrato.FolioContrato;
                            if (lblCliente != null)
                                lblCliente.Text = bof.ReferenciaContrato.CuentaCliente.Nombre;
                            if (lblReferencia != null)
                                lblReferencia.Text = bof.Referencia;
                            if (lblFechaInicio != null)
                                lblFechaInicio.Text = bof.ReferenciaContrato.FechaInicio.Value.ToShortDateString();
                            if (lblPlazoPago != null)
                                lblPlazoPago.Text = string.Format("{0:00}/{1:00}", bof.NumeroPago, bof.ReferenciaContrato.Plazo);
                            if (lblFechaVencimiento != null)
                                lblFechaVencimiento.Text = bof.FechaVencimiento.Value.ToShortDateString();
                            if (btnVer != null)
                                btnVer.CommandArgument = bof.PagoContratoID.ToString();
                            if (btnConfigurar != null)
                                btnConfigurar.CommandArgument = bof.PagoContratoID.ToString();
                            if (btnCancelar != null)
                            {
                                //Validar la visualización por permiso si es el último pago
                                if (bof.UltimoPago.HasValue && bof.UltimoPago.Value)
                                {
                                    btnCancelar.Visible = true;
                                    btnCancelar.CommandArgument = bof.PagoContratoID.ToString();
                                }
                                else
                                    btnCancelar.Visible = false;
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
                                lblCliente.Text = bof.ReferenciaContrato.CuentaCliente.Nombre;
                            if (lblReferencia != null)
                                lblReferencia.Text = bof.Referencia;
                            if (lblFechaInicio != null)
                                lblFechaInicio.Text = bof.ReferenciaContrato.FechaInicio.Value.ToShortDateString();
                            if (lblPlazoPago != null)
                                lblPlazoPago.Text = string.Format("{0:00}/{1:00}", bof.NumeroPago, bof.ReferenciaContrato.Plazo);
                            if (lblFechaVencimiento != null)
                                lblFechaVencimiento.Text = bof.FechaVencimiento.Value.ToShortDateString();
                            if (btnVer != null)
                                btnVer.CommandArgument = bof.PagoID.ToString();
                            if (btnConfigurar != null)
                                btnConfigurar.CommandArgument = bof.PagoID.ToString();
                            //Visibilidad del botón cancelar por default no aplica para Idealease
                            if (btnCancelar != null)
                                btnCancelar.Visible = false;
                        }
                    }
                    if (btnConfigurar != null) btnConfigurar.Visible = hdnPODHP.Visible;

                    if (btnVer != null) btnVer.Visible = hdnPCF.Visible;

                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, NombreClase + ".grdPagos_RowDataBound: " + ex.Message);
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

        #endregion
    }
}