using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using System.Globalization;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class CambiarFrecuenciaContratosRDUI : Page, ICambiarFrecuenciaContratosRDVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador con los metodos principales
        /// </summary>
        private CambiarFrecuenciaContratosRDPRE presentador;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(CambiarFrecuenciaContratosRDUI).Name;
        #endregion
        #region Propiedades
        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoId
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnContratoId.Value) && !string.IsNullOrWhiteSpace(this.hdnContratoId.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnContratoId.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnContratoId.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        public string NumeroContrato
        {
            get
            {
                string numeroContrato = null;
                if(!String.IsNullOrEmpty(this.txtNumeroContrato.Text.Trim()))
                    numeroContrato = this.txtNumeroContrato.Text.Trim().ToUpper();
                return numeroContrato;
            }
            set
            {
                this.txtNumeroContrato.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// CuentaClienteID del Cliente
        /// </summary>
        public int? ClienteId
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnClienteId.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteId.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnClienteId.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnClienteId.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public string ClienteNombre
        {
            get
            {
                string nombreCliente = null;
                if(!String.IsNullOrEmpty(this.txtClienteNombre.Text.Trim()))
                    nombreCliente = this.txtClienteNombre.Text.Trim().ToUpper();
                return nombreCliente;
            }
            set
            {
                this.txtClienteNombre.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Id de la Sucursal
        /// </summary>
        public int? SucursalId
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnSucursalId.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalId.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnSucursalId.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnSucursalId.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                string nombreSucursal = null;
                if(!String.IsNullOrEmpty(this.txtSucursalNombre.Text.Trim()))
                    nombreSucursal = this.txtSucursalNombre.Text.Trim().ToUpper();
                return nombreSucursal;
            }
            set
            {
                this.txtSucursalNombre.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Fecha del Contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            set
            {
                this.txtFechaInicio.Text = value != null ? String.Format("{0:dd/MM/yyyy hh:ss}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Fecha de Promesa de Devolucion de la Unidad
        /// </summary>
        public DateTime? FechaPromesaDevolucion
        {
            set
            {
                this.txtFechaPromesaDevolucion.Text = value != null ? String.Format("{0:dd/MM/yyyy hh:ss}", value) : String.Empty;
            }
        }
        /// <summary>
        /// Dias Restantes para la Devolucion
        /// </summary>
        public int? DiasRestantes
        {
            get
            {
                if(!string.IsNullOrEmpty(this.txtDiasRestantes.Text) && !string.IsNullOrWhiteSpace(this.txtDiasRestantes.Text))
                {
                    int val = 0;
                    return Int32.TryParse(this.txtDiasRestantes.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.txtDiasRestantes.Text = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Lista de Pagos Enviados a Facturar
        /// </summary>
        public List<PagoUnidadContratoRDBO> ListaPagosFacturados
        {
            get
            {
                List<PagoUnidadContratoRDBO> pagosFacturados = (List<PagoUnidadContratoRDBO>)Session["ListaPagosFacturados"];
                if(pagosFacturados != null)
                    return pagosFacturados;
                return null;
            }
            set
            {
                if(value != null)
                    Session["ListaPagosFacturados"] = value;
                else
                    Session.Remove("ListaPagosFacturados");
            }
        }
        /// <summary>
        /// Frecuencia Actual del Contrato
        /// </summary>
        public EFrecuencia? FrecuenciaActual
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnFrecuenciaAnterior.Value) && !string.IsNullOrWhiteSpace(this.hdnFrecuenciaAnterior.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnFrecuenciaAnterior.Value, out val) ? (EFrecuencia?)val : null;
                }
                return null;
            }
            set
            {
                if(value.HasValue)
                {
                    this.txtFrecuenciaAnterior.Text = value.Value.ToString().ToUpper();
                    this.hdnFrecuenciaAnterior.Value = ((Int32?)value).Value.ToString();
                }
            }
        }
        /// <summary>
        /// Fecha que será asignada al Contrato
        /// </summary>
        public EFrecuencia? FrecuenciaNueva
        {
            get
            {
                if(!string.IsNullOrEmpty(this.ddlFrecuenciaFacturacion.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlFrecuenciaFacturacion.SelectedValue))
                {
                    int val = 0;
                    return Int32.TryParse(this.ddlFrecuenciaFacturacion.SelectedValue, out val) ? val != (-1) ? (EFrecuencia?)val : null : null;
                }
                return null;
            }
            set
            {
                if(value.HasValue)
                    this.ddlFrecuenciaFacturacion.SelectedValue = value.Value.ToString();
            }
        }
        /// <summary>
        /// Lista de Pagos Pendientes por Facturar
        /// </summary>
        public List<PagoUnidadContratoRDBO> ListaPagosFaltantes
        {
            get
            {
                List<PagoUnidadContratoRDBO> pagosFaltantes = (List<PagoUnidadContratoRDBO>)Session["ListaPagosFaltantes"];
                if(pagosFaltantes != null)
                    return pagosFaltantes;
                return null;
            }
            set
            {
                if(value != null)
                    Session["ListaPagosFaltantes"] = value;
                else
                    Session.Remove("ListaPagosFaltantes");
            }
        }
        /// <summary>
        /// Objeto que sera usado para modificar
        /// </summary>
        public ContratoRDBO ContratoOriginal
        {
            get
            {
                ContratoRDBO contratoRd = (ContratoRDBO)Session["ContratoOriginal"];
                if(contratoRd != null)
                    return contratoRd;
                return null;
            }
            set
            {
                if(value != null)
                    Session["ContratoOriginal"] = value;
                else
                    Session.Remove("ContratoOriginal");
            }
        }
        /// <summary>
        /// Objeto que conserva los Datos Originales del Contrato
        /// </summary>
        public ContratoRDBO ContratoAntiguo
        {
            get
            {
                ContratoRDBO contratoRd = (ContratoRDBO)Session["ContratoAntiguo"];
                if(contratoRd != null)
                    return contratoRd;
                return null;
            }
            set
            {
                if(value != null)
                    Session["ContratoAntiguo"] = value;
                else
                    Session.Remove("ContratoAntiguo");
            }
        }
        /// <summary>
        /// Cantidad de Pagos que se necesitan generar
        /// </summary>
        public int? CantidadPagosFaltantes
        {
            get
            {
                if(!string.IsNullOrEmpty(this.hdnDiasFacturar.Value) && !string.IsNullOrWhiteSpace(this.hdnDiasFacturar.Value))
                {
                    int val = 0;
                    return Int32.TryParse(this.hdnDiasFacturar.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set
            {
                this.hdnDiasFacturar.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture).Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Id del Usuario Logueado
        /// </summary>
        public int? UsuarioId
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if(masterMsj != null && masterMsj.Usuario != null && masterMsj.Usuario.Id != null)
                    return masterMsj.Usuario.Id;
                return null;
            }
        }
        /// <summary>
        /// Unidad Operativa del Usuario Logueadoo
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if(masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                return null;
            }
        }
        #endregion
        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new CambiarFrecuenciaContratosRDPRE(this);
                if(!IsPostBack)
                {
                    presentador.ValidarAcceso();
                    presentador.PrepararEdicion();
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion
        #region Eventos
        protected void grdPagosFacturados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType == DataControlRowType.DataRow)
                {
                    PagoUnidadContratoBO pago = (PagoUnidadContratoBO)e.Row.DataItem;

                    Label lblFechaVencimiento = e.Row.FindControl("lblFechaVencimiento") as Label;
                    if(lblFechaVencimiento != null)
                    {
                        string fecha = string.Empty;
                        if(pago.FechaVencimiento != null)
                        {
                            fecha = String.Format("{0:dd/MM/yyyy hh:ss}", pago.FechaVencimiento);
                        }
                        lblFechaVencimiento.Text = fecha;
                    }

                    Label lblEnviadoFacturacion = e.Row.FindControl("lblEnviadoFacturacion") as Label;
                    if(lblEnviadoFacturacion != null)
                    {
                        string enviado = String.Empty;
                        if(pago.EnviadoFacturacion != null)
                        {
                            enviado = pago.EnviadoFacturacion.Value ? "SI" : "NO"; 
                        }
                        lblEnviadoFacturacion.Text = enviado;
                    }

                    Label lblFrecuencia = e.Row.FindControl("lblFrecuencia") as Label;
                    if(lblFrecuencia != null)
                    {
                        string frecuencia = string.Empty;
                        if(pago.Tarifa.FrecuenciaID != null)
                        {
                            frecuencia = ((EFrecuencia)pago.Tarifa.FrecuenciaID).ToString().ToUpper();
                        }
                        lblFrecuencia.Text = frecuencia;
                    }
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdPagosFacturados_RowDataBound: " + ex.Message);
            }
        }
        protected void grdPagosFacturados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPagosFacturados.DataSource = ListaPagosFacturados;
                grdPagosFacturados.PageIndex = e.NewPageIndex;
                grdPagosFacturados.DataBind();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdPagosFacturados_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdPagosPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType == DataControlRowType.DataRow)
                {
                    PagoUnidadContratoBO pago = (PagoUnidadContratoBO)e.Row.DataItem;

                    Label lblFechaVencimiento = e.Row.FindControl("lblFechaVencimiento") as Label;
                    if(lblFechaVencimiento != null)
                    {
                        string fecha = string.Empty;
                        if(pago.FechaVencimiento != null)
                        {
                            fecha = String.Format("{0:dd/MM/yyyy hh:ss}", pago.FechaVencimiento);
                        }
                        lblFechaVencimiento.Text = fecha;
                    }
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".grdPagosPendientes_RowDataBound: " + ex.Message);
            }
        }
        protected void grdPagosPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPagosPendientes.DataSource = ListaPagosFaltantes;
                grdPagosPendientes.PageIndex = e.NewPageIndex;
                grdPagosPendientes.DataBind();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdPagosPendientes_PageIndexChanging: " + ex.Message);
            }
        }
        protected void ddlFrecuenciaFacturacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CalcularPagosPendientes(this.FrecuenciaNueva);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar la Frecuencia", ETipoMensajeIU.ERROR, nombreClase + ".ddlFrecuenciaFacturacion_SelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Ocurrio un problema al Cancelar la Accion", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var s = this.presentador.ValidarCampos();
                if(!String.IsNullOrEmpty(s))
                {
                    this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                this.RegistrarScript("ConfirmacionCambioFrecuencia", "confirmarCambioFrecuencia();");
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Ocurrio un problema al Cambiar la Frecuencia del Contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void btnTermino_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CambiarFrecuencia();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Ocurrio un problema al Cambiar la Frecuencia del Contrato", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion
        #region Metodos
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
        /// Limpia la sesion de la página
        /// </summary>
        public void LimpiarSesion()
        {
            this.Session.Remove("ContratoRDBO");
            this.Session.Remove("ListaPagosFacturados");
            this.Session.Remove("ListaPagosFaltantes");
            this.Session.Remove("ContratoOriginal");
            this.Session.Remove("ContratoAntiguo");
        }
        /// <summary>
        /// Coloca como inactivos los datos del contrato
        /// </summary>
        public void InactivarCamposIniciales()
        {
            this.txtNumeroContrato.Enabled = false;
            this.txtClienteNombre.Enabled = false;
            this.txtSucursalNombre.Enabled = false;
            this.txtFechaInicio.Enabled = false;
            this.txtFechaPromesaDevolucion.Enabled = false;
            this.txtDiasRestantes.Enabled = false;
            this.txtFrecuenciaAnterior.Enabled = false;
        }
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje que es desplegado</param>
        /// <param name="tipo">Tipo del mensaje que es desplegao</param>
        /// <param name="detalle">Detalle del mensaje que es desplegado</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
            if(tipo == ETipoMensajeIU.ERROR)
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        /// <summary>
        /// Obtiene el Paquete con el Contrato a Cambiar de Frecuencia
        /// </summary>
        /// <param name="key">Nombre con el que se identifica el objeto</param>
        /// <returns>Objeto Contrato que cambiara de Frecuencia</returns>
        public object ObtenerPaqueteNavegacion(string key)
        {
            ContratoRDBO contrato = (ContratoRDBO)Session["ContratoRDBO"];
            if(contrato == null)
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: No se encontro el Contrato que Cambiara de Frecuencia");
            //this.Session.Remove("ContratoRDBO");
            return contrato;
        }
        /// <summary>
        /// Determina si se habilita la opcion de Guardar
        /// </summary>
        /// <param name="permitir">True/False para Guardar</param>
        public void PermitirGuardar(bool permitir)
        {
            this.btnGuardar.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se habilita la opcion de Cancelar
        /// </summary>
        /// <param name="permitir">True/False para Cancelar</param>
        public void PermitirCancelar(bool permitir)
        {
            this.btnCancelar.Enabled = permitir;
        }
        /// <summary>
        /// Activa o desactiva la seleccion de frecuencia de Facturacion
        /// </summary>
        /// <param name="permitir">Determina se se puede o no selecciona la Frecuencia de Facturacion</param>
        public void PermitirFrecuencia(bool permitir)
        {
            this.ddlFrecuenciaFacturacion.Enabled = permitir;
            this.ddlFrecuenciaFacturacion.AutoPostBack = permitir;
        }
        /// <summary>
        /// Redirige la seccion de Consulta de Contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarCambioFrecuenciaRDUI.aspx"), true); ;
        }
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), true);
        }
        /// <summary>
        /// Presenta solo las Frecuencias de Facturacion Disponibles para Escoger
        /// </summary>
        /// <param name="frecuenciaFacturacion">Diccionario con las opciones de Frecuencia Disponibles</param>
        public void EstablecerFrecuenciasFacturacion(Dictionary<string, string> frecuenciaFacturacion)
        {
            if(ReferenceEquals(frecuenciaFacturacion, null))
                frecuenciaFacturacion = new Dictionary<string, string>();

            this.ddlFrecuenciaFacturacion.DataSource = frecuenciaFacturacion;
            this.ddlFrecuenciaFacturacion.DataValueField = "key";
            this.ddlFrecuenciaFacturacion.DataTextField = "value";
            this.ddlFrecuenciaFacturacion.DataBind();
        }
        /// <summary>
        /// Coloca la informacion inicial del contrato obtenido del paquete de Navegacion
        /// </summary>
        /// <param name="contratoBo">Contrato con las informacion inicial</param>
        public void EstablecerInformacionInicial(ContratoRDBO contratoBo)
        {
            if(contratoBo == null)
                throw new Exception("No se encuentra el contrato con la información inicial. ");
            this.ContratoId = contratoBo.ContratoID;
            this.SucursalId = contratoBo.Sucursal != null ? contratoBo.Sucursal.Id : null;
            this.FrecuenciaActual = contratoBo.FrecuenciaFacturacion;
        }
        /// <summary>
        /// Presenta en la Interfaz los pagos Enviados a Facturar
        /// </summary>
        /// <param name="pagosFacturados">Lista de Pagos Enviados a Facturar</param>
        public void EstablecerPagosFacturados(List<PagoUnidadContratoRDBO> pagosFacturados)
        {
            grdPagosFacturados.DataSource = pagosFacturados;
            grdPagosFacturados.DataBind();
        }
        /// <summary>
        /// Presenta en la Interfaz los pagos pendientes por enviar a Facturar
        /// </summary>
        /// <param name="pagosPendientes">Lista de Pagos que falta por enviar a Facturar</param>
        public void EstablecerPagosPendientes(List<PagoUnidadContratoRDBO> pagosPendientes)
        {
            grdPagosPendientes.DataSource = pagosPendientes;
            grdPagosPendientes.DataBind();
        }
        #endregion
    }
}