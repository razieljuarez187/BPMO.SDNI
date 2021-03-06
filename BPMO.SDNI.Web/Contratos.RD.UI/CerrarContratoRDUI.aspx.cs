﻿// Satisface al CU013 - Cerrar Contrato Renta Diaria
using System;
using System.Collections.Generic;
using System.Web.UI;

using System.Globalization;
using System.Configuration;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class CerrarContratoRDUI : System.Web.UI.Page, ICerrarContratoRDVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "CerrarContratoRDUI";

        /// <summary>
        /// Presentador para Cancelar Contrato RD
        /// </summary>
        private CerrarContratoRDPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Usuario que se ha identificado en el Sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ModuloID"]))
                    return int.Parse(ConfigurationManager.AppSettings["ModuloID"]);

                return null;
            }
        }

        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoContratoRD"] != null)
                    return Session["UltimoObjetoContratoRD"];

                return null;
            }
            set
            {
                Session["UltimoObjetoContratoRD"] = value;
            }
        }

        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnContratoID.Value))
                    return int.Parse(this.hdnContratoID.Value);
                return null;
            }
            set { this.hdnContratoID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public int? EstatusID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value))
                    return int.Parse(this.hdnEstatusID.Value);
                return null;
            }
            set { this.hdnEstatusID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o estabece el usuario que actualiza por ultima vez el contrato
        /// </summary>
        public int? UUA
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUUA.Value) && !string.IsNullOrWhiteSpace(this.hdnUUA.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnUUA.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnUUA.Value = value.HasValue ? value.Value.ToString().Trim() : string.Empty; }
        }
        /// <summary>
        /// Obtiene o estabece la fecha de la ultima modificación del contrato
        /// </summary>
        public DateTime? FUA
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFUA.Value) && !string.IsNullOrWhiteSpace(this.hdnFUA.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFUA.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFUA.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
      
        public int? UnidadID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value))
                    return int.Parse(this.hdnUnidadID.Value);
                return null;
            }
            set { this.hdnUnidadID.Value = value != null ? value.ToString() : string.Empty; }
        }
        public int? EquipoID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value))
                    return int.Parse(this.hdnEquipoID.Value);
                return null;
            }
            set { this.hdnEquipoID.Value = value != null ? value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Kilometraje en la entrega de la unidad
        /// </summary>
        public int? KmEntrega
        {
            get
            {
                int? kmEntrega = null;
                if (!String.IsNullOrEmpty(this.txtKmEntrega.Text.Trim()))
                    kmEntrega = int.Parse(this.txtKmEntrega.Text.Trim().Replace(",", ""));
                return kmEntrega;
            }
            set { this.txtKmEntrega.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        /// <summary>
        /// Kilometraje en la recepción de la unidad
        /// </summary>
        public int? KmRecepcion
        {
            get
            {
                int? kmRecepcion = null;
                if (!String.IsNullOrEmpty(this.txtKmRecepcion.Text.Trim()))
                    kmRecepcion = int.Parse(this.txtKmRecepcion.Text.Trim().Replace(",", ""));
                return kmRecepcion;
            }
            set { this.txtKmRecepcion.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        /// <summary>
        /// kilometraje de recorrido entre la salida y regreso de la unidad
        /// </summary>
        public int? KmRecorrido
        {
            get
            {
                int? kmRecorrido = null;
                if (!String.IsNullOrEmpty(this.txtKmRecorrido.Text.Trim()))
                    kmRecorrido = int.Parse(this.txtKmRecorrido.Text.Trim().Replace(",", ""));
                return kmRecorrido;
            }
            set { this.txtKmRecorrido.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? TarifaKmLibres
        {
            get
            {
                int? tarifaKmLibres = null;
                if (!String.IsNullOrEmpty(this.txtKmLibres.Text.Trim()))
                    tarifaKmLibres = int.Parse(this.txtKmLibres.Text.Trim().Replace(",", ""));
                return tarifaKmLibres;
            }
            set { this.txtKmLibres.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? KmExcedido
        {
            get
            {
                int? kmExcedido = null;
                if (!String.IsNullOrEmpty(this.txtKmExcedido.Text.Trim()))
                    kmExcedido = int.Parse(this.txtKmExcedido.Text.Trim().Replace(",", ""));
                return kmExcedido;
            }
            set { this.txtKmExcedido.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public decimal? TarifaKmExcedido
        {
            get
            {
                decimal? tarifaKmExcedido = null;
                if (!String.IsNullOrEmpty(this.txtTarifaKmExc.Text.Trim()))
                    tarifaKmExcedido = decimal.Parse(this.txtTarifaKmExc.Text.Trim().Replace(",", ""));
                return tarifaKmExcedido;
            }
            set { this.txtTarifaKmExc.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        /// <summary>
        /// kilómetros excedidos con base en los recorridos y los kms libres de la tarifa
        /// </summary>
        public decimal? MontoKmExcedido
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtMontoTotKmExc.Text.Trim()))
                    num = decimal.Parse(this.txtMontoTotKmExc.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtMontoTotKmExc.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        /// <summary>
        /// Horas de Equipo Refrigeración en la entrega 
        /// </summary>
        public int? HrsEntrega
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtHrsEqRefEntrega.Text.Trim()))
                    num = int.Parse(this.txtHrsEqRefEntrega.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtHrsEqRefEntrega.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? HrsRecepcion
        {
            get
            {
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
        public int? HrsConsumidas
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtTotHrsEqRefrigeracion.Text.Trim()))
                    num = int.Parse(this.txtTotHrsEqRefrigeracion.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtTotHrsEqRefrigeracion.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? TarifaHrsLibres
        {
            get
            {
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
        public int? HrsExcedidas
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtHrsExcedidas.Text.Trim()))
                    num = int.Parse(this.txtHrsExcedidas.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtHrsExcedidas.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public decimal? TarifaHrsExcedidas
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtTarifaHrExc.Text.Trim()))
                    num = decimal.Parse(this.txtTarifaHrExc.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtTarifaHrExc.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public decimal? MontoHrsExcedidas
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtMontoHrsEqRef.Text.Trim()))
                    num = decimal.Parse(this.txtMontoHrsEqRef.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtMontoHrsEqRef.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public decimal? ImporteUnidadCombustible
        {
            get
            {
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
        public decimal? DiferenciaCombustible
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtCombustibleSalida.Text.Trim()))
                    num = decimal.Parse(this.txtCombustibleSalida.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtCombustibleSalida.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public decimal? ImporteTotalCombustible
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtImporteTotCombustible.Text.Trim()))
                    num = decimal.Parse(this.txtImporteTotCombustible.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtImporteTotCombustible.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public decimal? CargoAbusoOperacion
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtCargoAbusoOperacion.Text.Trim()))
                    num = decimal.Parse(this.txtCargoAbusoOperacion.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtCargoAbusoOperacion.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public decimal? CargoDisposicionBasura
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtCargoDispBasura.Text.Trim()))
                    num = decimal.Parse(this.txtCargoDispBasura.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtCargoDispBasura.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public decimal? ImporteDeposito
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtImporteDeposito.Text.Trim()))
                    num = decimal.Parse(this.txtImporteDeposito.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtImporteDeposito.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public decimal? ImporteReembolso
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtImporteRembolso.Text.Trim()))
                    num = decimal.Parse(this.txtImporteRembolso.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtImporteRembolso.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }
        public string PersonaRecibeReembolso
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtPersonaRecibeRembolso.Text)) ? null : this.txtPersonaRecibeRembolso.Text.Trim().ToUpper();
            }
            set
            {
                this.txtPersonaRecibeRembolso.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }

        public int? DiasRentaProgramada
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtNumDiasRentProg.Text.Trim()))
                    num = int.Parse(this.txtNumDiasRentProg.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtNumDiasRentProg.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? DiasEnTaller
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtDiasUnidadTaller.Text.Trim()))
                    num = int.Parse(this.txtDiasUnidadTaller.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtDiasUnidadTaller.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? DiasRealesRenta
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtDiasRealesRenta.Text.Trim()))
                    num = int.Parse(this.txtDiasRealesRenta.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtDiasRealesRenta.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public int? DiasAdicionales
        {
            get
            {
                int? num = null;
                if (!String.IsNullOrEmpty(this.txtDiasAdicionalesCobro.Text.Trim()))
                    num = int.Parse(this.txtDiasAdicionalesCobro.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtDiasAdicionalesCobro.Text = value != null ? String.Format("{0:#,##0}", value) : String.Empty; }
        }
        public decimal? MontoTotalDiasAdicionales
        {
            get
            {
                decimal? num = null;
                if (!String.IsNullOrEmpty(this.txtTotalDiasAdicionales.Text.Trim()))
                    num = decimal.Parse(this.txtTotalDiasAdicionales.Text.Trim().Replace(",", ""));
                return num;
            }
            set { this.txtTotalDiasAdicionales.Text = value != null ? String.Format("{0:#,##0.00##}", value) : String.Empty; }
        }

        public string ObservacionesCierre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservacionesCierre.Text)) ? null : this.txtObservacionesCierre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtObservacionesCierre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public DateTime? FechaCierre
        {
            get
            {
                DateTime? fecha = null;
                TimeSpan? hora = null;

                if (!string.IsNullOrEmpty(this.txtFechaCierre.Text) && !string.IsNullOrWhiteSpace(this.txtFechaCierre.Text))
                {
                    DateTime parse = new DateTime();
                    if (DateTime.TryParse(this.txtFechaCierre.Text, out parse))
                        fecha = parse;

                    if (!string.IsNullOrEmpty(this.txtHoraCierre.Text) && !string.IsNullOrWhiteSpace(this.txtHoraCierre.Text))
                    {
                        var time = DateTime.ParseExact(this.txtHoraCierre.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                        hora = time.TimeOfDay;
                    }
                }

                if (fecha.HasValue && hora.HasValue)
                    fecha = fecha.Value.Add(hora.Value);

                return fecha.HasValue ? fecha : null;
            }
            set
            {
                this.txtFechaCierre.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty;
                if (value != null && value.Value.TimeOfDay != null)
                {
                    DateTime d = new DateTime().Add(value.Value.TimeOfDay);
                    this.txtHoraCierre.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraCierre.Text = string.Empty;
            }
        }
        public DateTime? FechaContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFechaContrato.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaContrato.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaContrato.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public DateTime? FechaRecepcion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFechaRecepcion.Value) && !string.IsNullOrWhiteSpace(this.hdnFechaRecepcion.Value))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.hdnFechaRecepcion.Value, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.hdnFechaRecepcion.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new CerrarContratoRDPRE(this, this.ucHerramientas, this.ucResuContratoRD, this.ucDatosGeneralesElementoUI, this.ucEquiposAliadosUnidadUI);
                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion 

        #region Metodos
        public void PrepararEdicion()
        {
            this.ucDatosGeneralesElementoUI.EstablecerModoLectura();
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        public void RedirigirACancelar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/CancelarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleContratoRDUI.aspx"),false);
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarContratoRDUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        public void PermitirCerrar(bool permitir)
        {
            this.txtFechaCierre.Enabled = permitir;
            this.txtObservacionesCierre.Enabled = permitir;
            this.txtCargoAbusoOperacion.Enabled = permitir;
            this.txtCargoDispBasura.Enabled = permitir;
            this.txtHoraCierre.Enabled = permitir;
            this.txtImporteRembolso.Enabled = permitir;
            this.txtPersonaRecibeRembolso.Enabled = permitir;
            
            this.btnGuardar.Enabled = permitir;
        }
        public void PermitirRegistrar(bool permitir)
        {
            this.hlkRegistro.Enabled = permitir;
        }

        /// <summary>
        /// Limpia las variables usadas para la edición de la session
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["UltimoObjetoContratoRD"] != null)
                Session.Remove("UltimoObjetoContratoRD");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string msjDetalle = null)
        {
            var master = (Site)Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }
        #endregion

        #region Eventos
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarEdicion();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar el cierre del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CerrarContrato();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cerrar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click: " + ex.Message);
            }

        }
        #endregion
    }
}