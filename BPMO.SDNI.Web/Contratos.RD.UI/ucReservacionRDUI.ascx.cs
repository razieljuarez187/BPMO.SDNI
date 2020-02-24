// Satisface al CU004 - Catálogo de Reservaciones de Unidades de Renta Diaria
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class ucReservacionRDUI : System.Web.UI.UserControl, IucReservacionRDVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ucReservacionRDUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal,//SC051
            CuentaClienteIdealease,
            Modelo,
            UnidadDisponibleReservacion
        }
        /// <summary>
        /// presentador del UC de información general del contrato de renta diaria
        /// </summary>
        private ucReservacionRDPRE presentador;
        #endregion

        #region Propiedades
        public int? ReservacionID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnReservacionID.Value))
                    id = int.Parse(this.hdnReservacionID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnReservacionID.Value = value.ToString();
                else
                    this.hdnReservacionID.Value = string.Empty;
            }
        }
        public DateTime? FC
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFC.Value))
                    temp = DateTime.Parse(this.hdnFC.Value);
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFC.Value = value.Value.ToString();
                else
                    this.hdnFC.Value = string.Empty;
            }
        }
        public int? UC
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUC.Value))
                    id = int.Parse(this.hdnUC.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUC.Value = value.ToString();
                else
                    this.hdnUC.Value = string.Empty;
            }
        }
        public DateTime? FUA
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.hdnFUA.Value))
                    temp = DateTime.Parse(this.hdnFUA.Value);
                return temp;
            }
            set
            {
                if (value != null)
                    this.hdnFUA.Value = value.Value.ToString();
                else
                    this.hdnFUA.Value = string.Empty;
            }
        }
        public int? UUA
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUUA.Value))
                    id = int.Parse(this.hdnUUA.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUUA.Value = value.ToString();
                else
                    this.hdnUUA.Value = string.Empty;
            }
        }
        public bool? Activo
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnActivo.Value))
                    id = bool.Parse(this.hdnActivo.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                {
                    this.hdnActivo.Value = value.ToString();
                    if (value == true)
                        this.txtActivo.Text = "Activa";
                    else
                        this.txtActivo.Text = "Cancelada";
                }
                else
                {
                    this.hdnActivo.Value = string.Empty;
                    this.txtActivo.Text = string.Empty;
                }
            }
        }
        public int? UnidadOperativaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUOID.Value))
                    id = int.Parse(this.hdnUOID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUOID.Value = value.ToString();
                else
                    this.hdnUOID.Value = string.Empty;
            }
        }
        public List<int?> SucursalesSeguridad
        {
            get
            {
                if ((List<int?>)Session["ListaSucursalesSeguridad_ucReservacion"] == null)
                    return new List<int?>();
                else
                    return (List<int?>)Session["ListaSucursalesSeguridad_ucReservacion"];
            }
            set
            {
                Session["ListaSucursalesSeguridad_ucReservacion"] = value;
            }
        }
        public int? TipoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTipoID.Value))
                    id = int.Parse(this.hdnTipoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTipoID.Value = value.ToString();
                else
                    this.hdnTipoID.Value = string.Empty;
            }
        }

        public string Numero
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumero.Text)) ? null : this.txtNumero.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumero.Text = value;
                else
                    this.txtNumero.Text = string.Empty;
            }
        }
        public int? CuentaClienteID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnCuentaClienteID.Value))
                    id = int.Parse(this.hdnCuentaClienteID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnCuentaClienteID.Value = value.ToString();
                else
                    this.hdnCuentaClienteID.Value = string.Empty;
            }
        }
        public string CuentaClienteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNombreCuentaCliente.Text)) ? null : this.txtNombreCuentaCliente.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNombreCuentaCliente.Text = value;
                else
                    this.txtNombreCuentaCliente.Text = string.Empty;
            }
        }
        public int? ModeloID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnModeloID.Value))
                    id = int.Parse(this.hdnModeloID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNombreModelo.Text)) ? null : this.txtNombreModelo.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNombreModelo.Text = value;
                else
                    this.txtNombreModelo.Text = string.Empty;
            }
        }

        public DateTime? FechaReservacionInicial
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaInicio.Text) && !string.IsNullOrWhiteSpace(this.txtFechaInicio.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaInicio.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaInicio.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public TimeSpan? HoraReservacionInicial
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraInicio.Text) && !string.IsNullOrWhiteSpace(this.txtHoraInicio.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraInicio.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraInicio.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraInicio.Text = string.Empty;
            }
        }
        public DateTime? FechaReservacionFinal
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFechaFinal.Text) && !string.IsNullOrWhiteSpace(this.txtFechaFinal.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtFechaFinal.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtFechaFinal.Text = value.HasValue ? value.Value.ToShortDateString() : string.Empty; }
        }
        public TimeSpan? HoraReservacionFinal
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtHoraFinal.Text) && !string.IsNullOrWhiteSpace(this.txtHoraFinal.Text))
                {
                    var time = DateTime.ParseExact(this.txtHoraFinal.Text, "hh:mm tt", CultureInfo.InvariantCulture);
                    return time.TimeOfDay;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime d = new DateTime().Add(value.Value);
                    this.txtHoraFinal.Text = string.Format("{0:hh:mm tt}", d).ToUpper().Replace(".", "").Replace("A M", "AM").Replace("P M", "PM").Trim();
                }
                else
                    this.txtHoraFinal.Text = string.Empty;
            }
        }

        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroEconomico.Text)) ? null : this.txtNumeroEconomico.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtNumeroEconomico.Text = value;
                else
                    this.txtNumeroEconomico.Text = string.Empty;
            }
        }
        public int? UnidadID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUnidadID.Value))
                    id = int.Parse(this.hdnUnidadID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadID.Value = value.ToString();
                else
                    this.hdnUnidadID.Value = string.Empty;
            }
        }
        public string UnidadSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadSerie.Text)) ? null : this.txtUnidadSerie.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidadSerie.Text = value;
                else
                    this.txtUnidadSerie.Text = string.Empty;
            }
        }
        public int? UnidadAnio
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.txtUnidadAnio.Text))
                    id = int.Parse(this.txtUnidadAnio.Text.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.txtUnidadAnio.Text = value.ToString();
                else
                    this.txtUnidadAnio.Text = string.Empty;
            }
        }
        public string UnidadPlacaEstatal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadPlacaEstatal.Text)) ? null : this.txtUnidadPlacaEstatal.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidadPlacaEstatal.Text = value;
                else
                    this.txtUnidadPlacaEstatal.Text = string.Empty;
            }
        }
        public string UnidadPlacaFederal
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadPlacaFederal.Text)) ? null : this.txtUnidadPlacaFederal.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidadPlacaFederal.Text = value;
                else
                    this.txtUnidadPlacaFederal.Text = string.Empty;
            }
        }
        public decimal? UnidadCapacidadCarga
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadCapacidadCarga.Text))
                    temp = Decimal.Parse(this.txtUnidadCapacidadCarga.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadCapacidadCarga.Text = string.Format("{0:#,##0.0000}", value);
                else
                    this.txtUnidadCapacidadCarga.Text = string.Empty;
            }
        }
        public string UnidadMarcaNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadMarcaNombre.Text)) ? null : this.txtUnidadMarcaNombre.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidadMarcaNombre.Text = value;
                else
                    this.txtUnidadMarcaNombre.Text = string.Empty;
            }
        }
        public decimal? UnidadCapacidadTanque
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadCapacidadTanque.Text))
                    temp = Decimal.Parse(this.txtUnidadCapacidadTanque.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadCapacidadTanque.Text = string.Format("{0:#,##0.0000}", value);
                else
                    this.txtUnidadCapacidadTanque.Text = string.Empty;
            }
        }
        public decimal? UnidadRendimientoTanque
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtUnidadRendimientoTanque.Text))
                    temp = Decimal.Parse(this.txtUnidadRendimientoTanque.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtUnidadRendimientoTanque.Text = string.Format("{0:#,##0.0000}", value);
                else
                    this.txtUnidadRendimientoTanque.Text = string.Empty;
            }
        }
        public string UnidadEstatusOperacion
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadEstatusOperacion.Text)) ? null : this.txtUnidadEstatusOperacion.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidadEstatusOperacion.Text = value;
                else
                    this.txtUnidadEstatusOperacion.Text = string.Empty;
            }
        }
        public string UnidadEstatusMantenimiento
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUnidadEstatusMantenimiento.Text)) ? null : this.txtUnidadEstatusMantenimiento.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUnidadEstatusMantenimiento.Text = value;
                else
                    this.txtUnidadEstatusMantenimiento.Text = string.Empty;
            }
        }
        public DateTime? UnidadFechaPlaneadaLiberacion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtUnidadFechaPlaneadaLiberacion.Text) && !string.IsNullOrWhiteSpace(this.txtUnidadFechaPlaneadaLiberacion.Text))
                {
                    DateTime date = new DateTime();
                    return DateTime.TryParse(this.txtUnidadFechaPlaneadaLiberacion.Text, out date) ? (DateTime?)date : null;
                }
                return null;
            }
            set { this.txtUnidadFechaPlaneadaLiberacion.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public object UnidadEquiposAliados
        {
            get
            {
                if (Session["ListadoUnidadEquiposAliados"] != null)
                    return Session["ListadoUnidadEquiposAliados"];

                return null;
            }
            set
            {
                Session["ListadoUnidadEquiposAliados"] = value;
                this.grvUnidadEquiposAliados.DataSource = value;
                this.grvUnidadEquiposAliados.DataBind();
            }
        }

        public int? UsuarioReservoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUsuarioReservoID.Value))
                    id = int.Parse(this.hdnUsuarioReservoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUsuarioReservoID.Value = value.ToString();
                else
                    this.hdnUsuarioReservoID.Value = string.Empty;
            }
        }
        public string UsuarioReservoNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtUsuarioReservoNombre.Text)) ? null : this.txtUsuarioReservoNombre.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtUsuarioReservoNombre.Text = value;
                else
                    this.txtUsuarioReservoNombre.Text = string.Empty;
            }
        }

        public string Observaciones
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservaciones.Text)) ? null : this.txtObservaciones.Text.Trim().ToUpper();
            }
            set
            {
                if (value != null)
                    this.txtObservaciones.Text = value;
                else
                    this.txtObservaciones.Text = string.Empty;
            }
        }

        #region Propiedades para el Buscador
        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

                return objeto;
            }
            set
            {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set
            {
                ViewState["BUSQUEDA"] = value;
            }
        }
        #endregion

        #region SC051
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal de la cual se desean filtrar las unidades
        /// </summary>
        public int? SucursalID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnSucursalID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value)
                           ? (Int32.TryParse(this.hdnSucursalID.Value, out val) ? (int?)val : null)
                           : null;
            }
            set
            {
                this.hdnSucursalID.Value = value.HasValue
                                               ? value.Value.ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text)
                           ? this.txtSucursal.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
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
                this.presentador = new ucReservacionRDPRE(this);

                this.txtObservaciones.Attributes.Add("onkeyup", "checkText(this,300);");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear de página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.txtFechaFinal.Text = "";
            this.txtFechaInicio.Text = "";
            this.txtHoraFinal.Text = "";
            this.txtHoraInicio.Text = "";
            this.txtNombreCuentaCliente.Text = "";
            this.txtNombreModelo.Text = "";
            this.txtNumero.Text = "";
            this.txtNumeroEconomico.Text = "";
            this.txtObservaciones.Text = "";
            this.txtUnidadAnio.Text = "";
            this.txtUnidadCapacidadCarga.Text = "";
            this.txtUnidadCapacidadTanque.Text = "";
            this.txtUnidadEstatusMantenimiento.Text = "";
            this.txtUnidadEstatusOperacion.Text = "";
            this.txtUnidadFechaPlaneadaLiberacion.Text = "";
            this.txtUnidadMarcaNombre.Text = "";
            this.txtUnidadPlacaEstatal.Text = "";
            this.txtUnidadPlacaFederal.Text = "";
            this.txtUnidadRendimientoTanque.Text = "";
            this.txtUnidadSerie.Text = "";
            this.txtUsuarioReservoNombre.Text = "";
            this.txtUsuarioReservoNombre.Text = "";
            this.txtSucursal.Text = string.Empty;//SC051

            this.hdnActivo.Value = "";
            this.hdnCuentaClienteID.Value = "";
            this.hdnFC.Value = "";
            this.hdnFUA.Value = "";
            this.hdnModeloID.Value = "";
            this.hdnReservacionID.Value = "";
            this.hdnUC.Value = "";
            this.hdnUnidadID.Value = "";
            this.hdnUOID.Value = "";
            this.hdnUsuarioReservoID.Value = "";
            this.hdnUUA.Value = "";
            this.hdnSucursalID.Value = string.Empty;

            this.grvUnidadEquiposAliados.DataSource = null;
            this.grvUnidadEquiposAliados.DataBind();
        }

        public void HabilitarModoEdicion(bool habilitar)
        {
            this.txtFechaFinal.Enabled = habilitar;
            this.txtFechaInicio.Enabled = habilitar;
            this.txtHoraFinal.Enabled = habilitar;
            this.txtHoraInicio.Enabled = habilitar;
            this.txtNombreCuentaCliente.Enabled = habilitar;
            this.txtNombreModelo.Enabled = habilitar;
            this.txtNumeroEconomico.Enabled = habilitar;
            this.txtObservaciones.Enabled = habilitar;
            this.txtSucursal.Enabled = habilitar;//SC051
        }
        public void HabilitarCuentaCliente(bool habilitar)
        {
            this.txtNombreCuentaCliente.Enabled = habilitar;
            this.ibtnBuscarCliente.Enabled = habilitar;
        }
        public void HabilitarModelo(bool habilitar)
        {
            this.txtNombreModelo.Enabled = habilitar;
            this.ibtnBuscarModelo.Enabled = habilitar;
        }
        public void HabilitarUnidad(bool habilitar)
        {
            this.txtNumeroEconomico.Enabled = habilitar;
            this.ibtnBuscarUnidad.Enabled = habilitar;
        }
        /// <summary>
        /// Habilita las opciones para la selección de la sucursal
        /// </summary>
        /// <param name="habilitar">Estatus a aplicar a los controles</param>
        public void HabilitarSucursal(bool habilitar)//SC051
        {
            this.txtSucursal.Enabled = habilitar;
            this.ibtnBuscarSucursal.Enabled = habilitar;
        }

        public void MostrarDetalleUnidad(bool mostrar)
        {
            this.pnlDetalleUnidad.Visible = mostrar;
            this.pnlDetalleUnidadEquiposAliados.Visible = mostrar;
        }
        public void MostrarUsuarioReservo(bool mostrar)
        {
            this.pnlUsuarioReservo.Visible = mostrar;
        }
        public void MostrarNumeroReservacion(bool mostrar)
        {
            this.pnlNumero.Visible = mostrar;
        }
        public void MostrarActivo(bool mostrar)
        {
            this.pnlActivo.Visible = mostrar;
        }

        public void LimpiarSesion()
        {
            if (Session["ListadoUnidadEquiposAliados"] != null)
                Session.Remove("ListadoUnidadEquiposAliados");
            if (Session["ListaSucursalesSeguridad_ucReservacion"] != null)
                Session.Remove("ListaSucursalesSeguridad_ucReservacion");
        }

        /// <summary>
        /// Despliega un mensaje en pantalla
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

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
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
        #endregion
        #endregion

        #region Eventos
        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.presentador.SeleccionarFecha();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar la fecha", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtFecha_TextChanged:" + ex.Message);
            }
        }

        #region Eventos para el Buscador
        protected void txtNombreCuentaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = nombreCuentaCliente;
                if (!string.IsNullOrEmpty(this.CuentaClienteNombre) && !string.IsNullOrWhiteSpace(this.CuentaClienteNombre))
                    EjecutaBuscador("CuentaClienteIdealease", ECatalogoBuscador.CuentaClienteIdealease);

                this.CuentaClienteNombre = null;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNombreCliente_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.CuentaClienteID = null;

                string nombreCuentaCliente = this.CuentaClienteNombre;
                Session_BOSelecto = null;

                EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarCliente_Click:" + ex.Message);
            }
        }

        protected void txtNombreModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string modelo = this.ModeloNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.ModeloNombre = modelo;
                if (this.ModeloNombre != null)
                {
                    string s;
                    if ((s = this.presentador.ValidarCamposModeloDisponibleReservacion()) != null)
                    {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }

                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar el Modelo", ETipoMensajeIU.ERROR, this.nombreClase + ".txtNombreModelo_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposModeloDisponibleReservacion()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar el modelo", ETipoMensajeIU.ERROR, this.nombreClase + ".ibtnBuscarModelo_Click:" + ex.Message);
            }
        }

        protected void txtNumeroEconomico_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroEconomico = this.NumeroEconomico;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.UnidadDisponibleReservacion);

                this.NumeroEconomico = numeroEconomico;
                if (this.NumeroEconomico != null)
                {
                    string s;
                    if ((s = this.presentador.ValidarCamposUnidadDisponibleReservacion()) != null)
                    {
                        this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                        return;
                    }

                    this.EjecutaBuscador("UnidadDisponibleReservacion", ECatalogoBuscador.UnidadDisponibleReservacion);
                    this.NumeroEconomico = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Iconsistencias al buscar la Unidad para reservar", ETipoMensajeIU.ERROR, this.nombreClase + ".txtNumeroEconomico_TextChanged:" + ex.Message);
            }
        }
        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposUnidadDisponibleReservacion()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("UnidadDisponibleReservacion", ECatalogoBuscador.UnidadDisponibleReservacion);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al buscar la Unidad para reservar", ETipoMensajeIU.ERROR, this.nombreClase + ".ibtnBuscarUnidad_Click:" + ex.Message);
            }
        }

        #region SC051
        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Session_BOSelecto = null;
                this.SucursalID = null;

                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                {
                    string sucursalNombre = this.SucursalNombre;

                    this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                    this.SucursalNombre = sucursalNombre;
                    if (this.UnidadOperativaID.Value != null && this.SucursalNombre != null)
                    {
                        this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                        this.SucursalNombre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.ToString() + ".ibtnBuscaSucursal_Click:" + ex.Message);
            }
        }
        #endregion
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.CuentaClienteIdealease:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.UnidadDisponibleReservacion:
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}