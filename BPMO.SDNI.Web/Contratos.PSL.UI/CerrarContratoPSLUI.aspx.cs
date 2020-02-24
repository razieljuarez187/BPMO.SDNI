
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.PRE;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.PSL.UI
{
    public partial class CerrarContratoPSLUI : System.Web.UI.Page, ICerrarContratoPSLVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "CerrarContratoPSLUI";

        /// <summary>
        /// Presentador para Cancelar Contrato PSL
        /// </summary>
        private CerrarContratoPSLPRE presentador;
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
                return ((Site)Page.Master).ModuloID;
            }
        }

        public object UltimoObjeto
        {
            get
            {
                if (Session["UltimoObjetoContratoPSL"] != null)
                    return Session["UltimoObjetoContratoPSL"];

                return null;
            }
            set
            {
                Session["UltimoObjetoContratoPSL"] = value;
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
        /// Obtiene o establece el usuario que actualiza por ultima vez el contrato
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
        /// Obtiene o establece la fecha de la ultima modificación del contrato
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

        /// <summary>
        /// Líneas del Contrato
        /// </summary>
        public List<LineaContratoPSLBO> LineasContrato
        {
            get
            {
                List<LineaContratoPSLBO> listValue;
                if (Session["LineasContratoSession"] != null)
                {
                    listValue = (List<LineaContratoPSLBO>)Session["LineasContratoSession"];
                }
                else
                    listValue = new List<LineaContratoPSLBO>();

                return listValue;
            }
            set
            {
                var listValue = new List<LineaContratoPSLBO>();
                if (value != null)
                {
                    listValue = value;
                }
                Session["LineasContratoSession"] = listValue;

                grdLineasContrato.DataSource = listValue;
                grdLineasContrato.DataBind();
            }
        }

        /// <summary>
        /// Manejador de Evento para ver el Detalle de cargos adicionales
        /// </summary>
        internal CommandEventHandler VerCargosAdicionalesCierre { get; set; }

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

        /// <summary>
        /// Obtiene o establece l tipo de contrato que se va a registrar
        /// </summary>
        public int? TipoContrato
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnTipoContrato.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnTipoContrato.Value)
                           ? (Int32.TryParse(this.hdnTipoContrato.Value, out val) ? (int?)val : null)
                           : null;
            }
            set { this.hdnTipoContrato.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new CerrarContratoPSLPRE(this, this.ucHerramientas, this.ucResuContratoPSL, this.ucCargosAdicionalesCierrePSLUI);
                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                    this.grdLineasContrato.Columns[7].Visible = false;
                }
                this.VerCargosAdicionalesCierre = VerCargosAdicionales_Click;

                this.ucCargosAdicionalesCierrePSLUI.AgregarClick = AgregarCargoCierre_Click;
                this.ucCargosAdicionalesCierrePSLUI.CancelarClick = CancelarAgregarCargoCierre_Click;
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
            this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/CancelarContratoPSLUI.aspx"));
        }
        /// <summary>
        /// Envía al usuario a la página de detalle del contrato
        /// </summary>
        public void RedirigirADetalles()
        {
            switch (this.TipoContrato)
            {
                case (int)ETipoContrato.ROC:
                    this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleContratoROCUI.aspx"), false);
                    break;
                default:
                    this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/DetalleContratoROUI.aspx"), false);
                    break;
            }
        }
        /// <summary>
        /// Envía al usuario a la página de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            switch(this.TipoContrato)
            {
                case (int)ETipoContrato.ROC:
                    this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarContratoROCUI.aspx"));
                    break;
                default:
                    this.Response.Redirect(this.ResolveUrl("~/Contratos.PSL.UI/ConsultarContratoROUI.aspx"));
                    break;
            }
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
            if (Session["UltimoObjetoContratoPSL"] != null)
                Session.Remove("UltimoObjetoContratoPSL");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
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

        public void EstablecerEtiquetas()
        {
            switch (this.TipoContrato)
            {
                case (int)ETipoContrato.RO:
                    this.lblTipoContrato.Text = " R.O.";
                    this.lblTipoContrato2.Text = " R.O.";
                    this.lblTipoContrato3.Text = "Contrato De Renta Ordinaria";
                    this.lblEncabezadoLeyenda.Text = "OPERACIÓN - CERRAR CONTRATO RO";
                    this.hlkConsulta.NavigateUrl = "~/Contratos.PSL.UI/ConsultarContratoROUI.aspx";
                    this.hlkRegistro.NavigateUrl = "~/Contratos.PSL.UI/RegistrarContratoROUI.aspx";
                        break;
                case (int)ETipoContrato.ROC:
                        this.lblTipoContrato.Text = " R.O.C.";
                        this.lblTipoContrato2.Text = " R.O.C.";
                        this.lblTipoContrato3.Text = "Contrato De Renta con Opción a Compra";
                        this.lblEncabezadoLeyenda.Text = "OPERACIÓN - CERRAR CONTRATO ROC";
                    this.hlkConsulta.NavigateUrl = "~/Contratos.PSL.UI/ConsultarContratoROCUI.aspx";
                    this.hlkRegistro.NavigateUrl = "~/Contratos.PSL.UI/RegistrarContratoROCUI.aspx";
                        break;
                default:
                        this.lblTipoContrato.Text = " R.E.";
                        this.lblTipoContrato2.Text = " R.E.";
                        this.lblTipoContrato3.Text = "Contrato De Renta Extraordinaria";
                        this.lblEncabezadoLeyenda.Text = "OPERACIÓN - CERRAR CONTRATO RE";
                        this.hlkConsulta.NavigateUrl = "~/Contratos.PSL.UI/ConsultarContratoROUI.aspx";
                        this.hlkRegistro.NavigateUrl = "~/Contratos.PSL.UI/RegistrarContratoROUI.aspx";
                        break;

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

        protected void grdLineasContrato_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LineaContratoPSLBO linea = ((LineaContratoPSLBO)e.Row.DataItem);
                    var label = e.Row.FindControl("lblVIN") as Label;

                    var button = e.Row.FindControl("ibtnDetalles") as ImageButton;
                    button.Visible = true;

                    // Numero de Serie
                    if (label != null)
                    {
                        if (linea.Equipo != null && linea.Equipo.NumeroSerie != null) label.Text = linea.Equipo.NumeroSerie;
                        else label.Text = string.Empty;
                    }

                    // Modelo
                    label = e.Row.FindControl("lblModelo") as Label;
                    if (label != null)
                    {
                        if (linea.Equipo != null && linea.Equipo.Modelo != null) label.Text = linea.Equipo.Modelo.Nombre;
                        else label.Text = string.Empty;
                    }

                    //Anio
                    label = e.Row.FindControl("lblAnio") as Label;
                    if (label != null)
                    {
                        label.Text = linea.Equipo.Anio != null ? linea.Equipo.Anio.ToString() : string.Empty;
                    }

                    //Tipo Tarifa
                    label = e.Row.FindControl("lblTipoTarifa") as Label;
                    if (label != null)
                    {
                        label.Text = linea.TipoTarifa != null ? linea.TipoTarifa.ToString() : string.Empty;
                    }

                    //Turno
                    label = e.Row.FindControl("lblTurno") as Label;
                    if (label != null)
                    {
                        label.Text = string.Empty;

                        if (linea.Cobrable != null && ((TarifaContratoPSLBO)linea.Cobrable).TarifaTurno != null)
                        {
                            Type type = ((TarifaContratoPSLBO)linea.Cobrable).obtenerETarifaTurno(this.UnidadOperativaID.Value);
                            var memInfo = type.GetMember(type.GetEnumName(((TarifaContratoPSLBO)linea.Cobrable).TarifaTurno));
                            var display = memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;
                            label.Text = display.Description;
                        }
                    }

                    //Maniobra
                    label = e.Row.FindControl("lblManiobra") as Label;
                    if (label != null)
                    {
                        label.Text = string.Empty;
                        if (linea.Cobrable != null && ((TarifaContratoPSLBO)linea.Cobrable).Maniobra != null)
                            label.Text = string.Format("{0:c2}", ((TarifaContratoPSLBO)linea.Cobrable).Maniobra.Value);
                    }

                    //Activo
                    label = e.Row.FindControl("lblActiva") as Label;
                    if (label != null)
                    {
                        label.Text = linea.Activo != null ? linea.Activo.Value ? "SI" : "NO" : string.Empty;
                        label.ForeColor = linea.Activo != null && linea.Activo == false ? Color.Red : label.ForeColor;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Se han encontrado Inconsistencias al presentar el detalle del contrato.",
                               ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_RowDataBound: " + ex.Message);
            }
        }

        protected void grdLineasContrato_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdLineasContrato.DataSource = LineasContrato;
                grdLineasContrato.PageIndex = e.NewPageIndex;
                grdLineasContrato.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdLineasContrato_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if (e.CommandArgument == null) return;

                int index = 0;

                if (!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;

                LineaContratoPSLBO linea = LineasContrato[index];

                switch (eCommandNameUpper)
                {
                    case "CMDDETALLES":
                        {
                            if (VerCargosAdicionalesCierre != null)
                            {
                                var c = new CommandEventArgs("LineaContrato", linea);

                                VerCargosAdicionalesCierre.Invoke(sender, c);
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al eliminar la unidad del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento para Ver la captura de cargos adicionales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VerCargosAdicionales_Click(object sender, CommandEventArgs e)
        {
            try
            {
                var linea = e.CommandArgument as LineaContratoPSLBO;
                presentador.PrepararCargoAdicional(linea);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al desplegar el detalle del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click: " + ex.Message);
            }
        }


        /// <summary>
        /// Evento para Ver la captura de cargos adicionales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AgregarCargoCierre_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.AgregarCargoCierre();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al desplegar el detalle del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento para Ver la captura de cargos adicionales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelarAgregarCargoCierre_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.PrepararCierre();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al desplegar el detalle del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click: " + ex.Message);
            }
        }


        /// <summary>
        /// Cambia la la Vista de la interfaz a la información de la cargos adicionales
        /// </summary>
        public void CambiarACargoAdicional()
        {
            mvRQMCIERRE.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Cambia la la Vista de la interfaz a la información del cierre
        /// </summary>
        public void CambiarACierre()
        {
            mvRQMCIERRE.ActiveViewIndex = 0;
        }

        #endregion
    }
}
