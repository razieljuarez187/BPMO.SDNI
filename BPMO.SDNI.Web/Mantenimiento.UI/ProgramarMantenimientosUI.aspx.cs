// Satisface al caso de uso CU025
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class ProgramarMantenimientosUI : Page, IProgramarMantenimientosVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "ProgramarMantenimientosUI";
        /// <summary>
        /// Enumeración para controlar los objetos del buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal
        }
        /// <summary>
        /// Presentador de la UI
        /// </summary>
        private ProgramarMantenimientosPRE presentador;
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador de la unidad operativa
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
        /// Identificador del usuario
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                int? id = null;
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
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
                this.hdnSucursalID.Value = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal seleccionada para el filtro de consulta
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text) ? this.txtSucursal.Text.Trim().ToUpper() : string.Empty;
            }
            set
            {
                this.txtSucursal.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) ? value.Trim().ToUpper() : string.Empty;
            }
        }
        /// <summary>
        /// Identificador del taller
        /// </summary>
        public int? TallerID 
        { 
            get 
            {
                if (this.ddlTaller.Items != null && this.ddlTaller.Items.Count <= 0)
                    return null;
                else
                {
                    if (this.ddlTaller.SelectedIndex == 0 || this.ddlTaller.SelectedIndex == -1)
                        return null;
                }

                TallerBO taller = this.Talleres[this.ddlTaller.SelectedIndex - 1];
                return taller != null && taller.Id != null ? taller.Id : null;
            } 
        }
        /// <summary>
        /// Nombre del taller
        /// </summary>
        public string NombreTaller { get; set; }
        /// <summary>
        /// Determina si el taller es externo
        /// </summary>
        public bool? esExterno 
        {
            get
            {
                if (this.ddlTaller.Items != null && this.ddlTaller.Items.Count <= 0)
                    return null;
                else
                {
                    if (this.ddlTaller.SelectedIndex == 0 || this.ddlTaller.SelectedIndex == -1)
                        return null;
                }
                //AConfiguracionTallerBaseBO confTaller = this.Talleres[this.ddlTaller.SelectedIndex - 1];
                return false;
            } 
        }
        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de creación del registro
        /// </summary>
        public DateTime? FC { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de última actualización del registros
        /// </summary>
        public DateTime? FUA { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que creo el registro
        /// </summary>
        public int? UC { get; set; }
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que actualizó por última vez el registro
        /// </summary>
        public int? UUA { get { return this.UsuarioID; } }
        /// <summary>
        /// Listado de talleres
        /// </summary>
        public List<TallerBO> Talleres 
        {
            get
            {
                var listado = new List<TallerBO>();
                if (Session["ListadoTalleres"] != null)
                    listado = Session["ListadoTalleres"] as List<TallerBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoTalleres"] == null)
                    Session.Add("ListadoTalleres", value);
                else
                    Session["ListadoTalleres"] = value;
            }
        }
        /// <summary>
        /// Listado de citas de Mantenimiento
        /// </summary>
        public List<CitaMantenimientoBO> CitasMantenimiento
        {
            get
            {
                var listado = new List<CitaMantenimientoBO>();
                if (Session["ListadoCitasMantenimiento"] != null)
                    listado = Session["ListadoCitasMantenimiento"] as List<CitaMantenimientoBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoCitasMantenimiento"] == null)
                    Session.Add("ListadoCitasMantenimiento", value);
                else
                    Session["ListadoCitasMantenimiento"] = value;
            }
        }
        /// <summary>
        /// Listado de citas de Mantenimiento
        /// </summary>
        public List<DateTime> ListadoDiasInhabiles
        {
            get
            {
                var listado = new List<DateTime>();
                if (Session["ListadoDiasInhabiles"] != null)
                    listado = Session["ListadoDiasInhabiles"] as List<DateTime>;

                return listado;
            }
            set
            {
                if (Session["ListadoDiasInhabiles"] == null)
                    Session.Add("ListadoDiasInhabiles", value);
                else
                    Session["ListadoDiasInhabiles"] = value;
            }
        }
        #region PropiedadesDetalle
        /// <summary>
        /// Listado de Mantto de equipos aliados
        /// </summary>
        public List<MantenimientoProgramadoEquipoAliadoBO> ListadoManttoEquiposAliados
        {
            get
            {
                var listado = new List<MantenimientoProgramadoEquipoAliadoBO>();
                if (Session["ListadoEquiposAliados"] != null)
                    listado = Session["ListadoEquiposAliados"] as List<MantenimientoProgramadoEquipoAliadoBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoEquiposAliados"] == null)
                    Session.Add("ListadoEquiposAliados", value);
                else
                    Session["ListadoEquiposAliados"] = value;
            }
        }
        /// <summary>
        /// Listado de contactos del cliente
        /// </summary>
        public List<ContactoClienteBO> ListadoContactosCliente
        {
            get
            {
                var listado = new List<ContactoClienteBO>();
                if (Session["ListadoContactosCliente"] != null)
                    listado = Session["ListadoContactosCliente"] as List<ContactoClienteBO>;

                return listado;
            }
            set
            {
                if (Session["ListadoContactosCliente"] == null)
                    Session.Add("ListadoContactosCliente", value);
                else
                    Session["ListadoContactosCliente"] = value;
            }
        }
        /// <summary>
        /// Nombre del cliente de la unidad
        /// </summary>
        public string ClienteNombre 
        {
            get
            {
                return this.txtClienteNombre.Text;
            }
            set
            {
                this.txtClienteNombre.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Area/Departamento de la Unidad
        /// </summary>
        public string Area
        {
            get
            {
                return this.txtArea.Text;
            }
            set
            {
                this.txtArea.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// vin de la unidad
        /// </summary>
        public string VINUnidad
        {
            get
            {
                return this.txtVINUnidad.Text;
            }
            set 
            {
                this.txtVINUnidad.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Numero economico de la unidad
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                return this.txtNumeroEconomico.Text;
            }
            set
            {
                this.txtNumeroEconomico.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Placa estatal de la unidad
        /// </summary>
        public string PlacaEstatal
        {
            get
            {
                return this.txtPlacaEstatal.Text;
            }
            set
            {
                this.txtPlacaEstatal.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Placa Federal de la unidad
        /// </summary>
        public string PlacaFederal
        {
            get
            {
                return this.txtPlacaFederal.Text;
            }
            set
            {
                this.txtPlacaFederal.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Km del ultimo servicio de la unidad
        /// </summary>
        public int? KmUltimoServicio
        {
            get
            {
                int result = 0;
                if (int.TryParse(this.txtKMUltimoServicio.Text, out result))
                    return result;
                return null;
            }
            set
            {
                this.txtKMUltimoServicio.Text = value == null ? "" : value.ToString();
            }
        }
        /// <summary>
        /// Fecha del Ultimo Servicio de la Unidad
        /// </summary>
        public DateTime? FechaUltimoServicio
        {
            get
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(this.txtFechaUltimoServicio.Text, out result))
                    return result;
                return null;
            }
            set
            {
                this.txtFechaUltimoServicio.Text = value == null ? "" : value.ToString();
            }
        }
        /// <summary>
        /// Fecha Sugerida de Mantenimiento
        /// </summary>
        public DateTime? FechaSugerida
        {
            get
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(this.txtFechaSugerida.Text, out result))
                    return result;
                return null;
            }
            set
            {
                this.txtFechaSugerida.Text = value == null ? "" : value.ToString();
            }
        }
        /// <summary>
        /// Tipo Mantenimiento
        /// </summary>
        public string TipoMantenimiento
        {
            get
            {
                return this.txtTipoMantto.Text;
            }
            set
            {
                this.txtTipoMantto.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        public string NombreSucursalDetalle
        {
            get
            {
                return this.txtNombreSucursalDetail.Text;
            }
            set
            {
                this.txtNombreSucursalDetail.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Nombre del Taller
        /// </summary>
        public string NombreTallerDetalle
        {
            get
            {
                return this.txtNombreTallerDetail.Text;
            }
            set
            {
                this.txtNombreTallerDetail.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Estatus del Mantenimiento
        /// </summary>
        public string EstatusMantenimiento
        {
            get
            {
                return this.txtEstatusMantto.Text;
            }
            set
            {
                this.txtEstatusMantto.Text = String.IsNullOrEmpty(value) ? "" : value;
            }
        }
        /// <summary>
        /// Dias de retraso de ingreso de la unidad al taller
        /// </summary>
        public int? DiasRetraso
        {
            get
            {
                int result = 0;
                if (int.TryParse(this.txtDiasRetraso.Text, out result))
                    return result;
                return null;
            }
            set
            {
                this.txtDiasRetraso.Text = value == null ? "" : value.ToString();
            }
        }
        #endregion
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
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                presentador = new ProgramarMantenimientosPRE(this);
                if (!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Metodos
        public void PrepararNuevo()
        {

        }
        /// <summary>
        /// Redirige a la pagina informativa de falta de permisos para acceder
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            if (Session["CitaMantenimientoBO"] != null)
                Session.Remove("CitaMantenimientoBO");
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Limpia los objetos que se encuentren en la sesion generados por la interfaz
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["ListadoTalleres"] != null)
                Session.Remove("ListadoTalleres");
            if (Session["ListadoCitasMantenimiento"] != null)
                Session.Remove("ListadoCitasMantenimiento");
            if (Session["ListadoDiasInhabiles"] != null)
                Session.Remove("ListadoDiasInhabiles");
            if (Session["ListadoEquiposAliados"] != null)
                Session.Remove("ListadoEquiposAliados");
            if (Session["ListadoContactosCliente"] != null)
                Session.Remove("ListadoContactosCliente");
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
        /// <summary>
        /// Establece los talleres a los que tiene acceso de acuerdo a la sucursal
        /// </summary>
        /// <param name="talleres">Talleres Permitidos</param>
        public void EstablecerTalleres(List<TallerBO> talleres)
        {
            this.Talleres = talleres;
            List<ListItem> lista = Talleres.Select(x => new ListItem(x.Nombre, x.Id.ToString())).ToList();

            this.ddlTaller.Items.Clear();
            this.ddlTaller.DataSource = lista;
            this.ddlTaller.DataBind();

            this.ddlTaller.Items.Insert(0, new ListItem("SELECCIONE","0"));
        }
        /// <summary>
        /// Presentas las citas de mantenimiento en el calendario
        /// </summary>
        /// <param name="citasMantenimiento">Lista de Citas de Mantenimiento</param>
        public void EstablecerMantenimientos(List<CitaMantenimientoBO> citasMantenimiento, List<DateTime> diasInhabiles)
        {
            Session["ListadoCitasMantenimiento"] = citasMantenimiento;
            Session["ListadoDiasInhabiles"] = diasInhabiles;
            string json = this.ConvertirResultadoAJSON(citasMantenimiento);
            string jsonDias = this.ConvertirResultadoAJSON(diasInhabiles);

            this.RegistrarScript("Calendar", "var jsonEventos =" + json + "; var jsonDays = " + jsonDias + "; inicializeCalendar(jsonEventos, jsonDays);");
        }
        /// <summary>
        /// Presenta los equipos aliados en la interfaz
        /// </summary>
        public void EstablecerEquiposAliados()
        {           
            this.grvEquiposAliados.DataSource = this.ListadoManttoEquiposAliados;
            this.grvEquiposAliados.DataBind();

        }
        /// <summary>
        /// Presenta los contactos del cliente en la interfaz
        /// </summary>
        public void EstablecerContactosCliente()
        {
            List<DetalleContactoClienteBO> detalles = new List<DetalleContactoClienteBO>();
            this.ListadoContactosCliente.ForEach(x => { detalles.AddRange(x.Detalles); });
            
            this.grvContactoCliente.DataSource = detalles;
            this.grvContactoCliente.DataBind();

        }
        /// <summary>
        /// Convierte una lista de citas mantenimiento en JSON
        /// </summary>
        /// <param name="citasMantenimiento">Listado de citas de mantenimiento</param>
        /// <returns>Resultado en JSON</returns>
        private string ConvertirResultadoAJSON(List<CitaMantenimientoBO> citasMantenimiento)
        {
            try
            {
                #region Construcción del arreglo de eventos
                StringBuilder eventos = new StringBuilder();

                if (citasMantenimiento != null)
                {
                    var sinConfirmar = "SinConfirmarColor";
                    var recalendarizado = "RecalendarizadoColor";
                    var aTiempo = "ATiempoColor";
                    var pequenioRetraso = "PequenioRetrasoColor";
                    var retrasado = "RetrasadoColor";
                    foreach (CitaMantenimientoBO bo in citasMantenimiento)
                    {
                        StringBuilder evento = new StringBuilder();

                        if (bo.CitaMantenimientoID != null)
                            evento.AppendLine("id: " + bo.CitaMantenimientoID.ToString() + ",");
                        else
                            evento.AppendLine("id: " + bo.MantenimientoProgramado.MantenimientoProgramadoID.ToString() + ",");

                        if (!String.IsNullOrEmpty((bo.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroEconomico))
                        {
                            evento.AppendLine("title: '# Económico " + (bo.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroEconomico + "',");
                            evento.AppendLine("VIN_NUM: '"+(bo.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroEconomico+"',");
                        }
                        else
                        {
                            evento.AppendLine("title: 'VIN " + (bo.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroSerie + "',");
                            evento.AppendLine("VIN_NUM: '" + (bo.MantenimientoProgramado as MantenimientoProgramadoUnidadBO).Unidad.NumeroSerie + "',");
                        }

                        if (bo.FechaHoraCita != null)
                        {
                            evento.AppendLine("start: '" + bo.FechaHoraCita.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                            evento.AppendLine("end: '" + bo.FechaHoraCita.Value.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        }
                        else
                        {
                            if (bo.MantenimientoProgramado.Fecha != null)
                            {
                                evento.AppendLine("start: '" + bo.MantenimientoProgramado.Fecha.Value.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                                evento.AppendLine("end: '" + bo.MantenimientoProgramado.Fecha.Value.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss") + "',");
                            }
                        }

                        evento.AppendLine("allDay: false,");

                        if (bo.CitaMantenimientoID != null)
                            evento.AppendLine("programada: true,");
                        else
                            evento.AppendLine("programada: false,");

                        if (bo.MantenimientoProgramado.TipoMantenimiento != null)
                            evento.AppendLine("tipoMantenimiento: '" + bo.MantenimientoProgramado.TipoMantenimientoNombre + "',");
                        else
                            evento.AppendLine("tipoMantenimiento: 'Sin Nombre',");

                        #region Calculo Colores
                        if (bo.CitaMantenimientoID == null)
                        {
                            if (new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0) < bo.MantenimientoProgramado.Fecha.Value.Date)
                                evento.AppendLine("className: '" + sinConfirmar + "',");
                            else if (new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0) == bo.MantenimientoProgramado.Fecha.Value.Date)                
                                evento.AppendLine("className: '" + aTiempo + "',");
                            else
                            {
                                var tiempo = DateTime.Today - bo.MantenimientoProgramado.Fecha.Value.Date;
                                if (Math.Round(tiempo.TotalDays) < 15)
                                    evento.AppendLine("className: '" + pequenioRetraso + "',");
                                else
                                    evento.AppendLine("className: '" + retrasado + "',");

                            }
                           
                        }
                        else
                        {
                            if (bo.EstatusCita == EEstatusCita.RECALENDARIZADA)
                            {
                                if (new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0) <= bo.FechaHoraCita.Value.Date)
                                    evento.AppendLine("className: '" + recalendarizado + "',");
                                else
                                {
                                    if (new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0) < bo.MantenimientoProgramado.Fecha.Value.Date)
                                        evento.AppendLine("className: '" + aTiempo + "',");
                                    else
                                    {
                                        var tiempo = DateTime.Today - bo.MantenimientoProgramado.Fecha.Value.Date;
                                        if (Math.Round(tiempo.TotalDays) < 15)
                                            evento.AppendLine("className: '" + pequenioRetraso + "',");
                                        else
                                            evento.AppendLine("className: '" + retrasado + "',");

                                    }
                                }
                            }
                            else
                            {
                                if (new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0) < bo.MantenimientoProgramado.Fecha.Value.Date)
                                    evento.AppendLine("className: '" + aTiempo + "',");
                                else
                                {
                                    var tiempo = DateTime.Today - bo.FechaHoraCita.Value.Date;
                                    if (Math.Round(tiempo.TotalDays) < 15)
                                        evento.AppendLine("className: '" + pequenioRetraso + "',");
                                    else
                                        evento.AppendLine("className: '" + retrasado + "',");

                                }
                            }
                        }
                        #endregion

                        if (evento.ToString().Trim().Length > 0)
                        {
                            eventos.AppendLine("{");
                            eventos.AppendLine(evento.ToString().Substring(0, evento.Length - 3));
                            eventos.AppendLine("},");
                        }
                    }
                }
                #endregion

                StringBuilder json = new StringBuilder();
                json.AppendLine("[");

                if (eventos.ToString().Trim().Length > 0)
                    json.AppendLine(eventos.ToString().Substring(0, eventos.Length - 3));

                json.AppendLine("]");
                return json.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConvertirResultadoAJSON: " + ex.Message);
            }
        }
        /// <summary>
        /// Dias inhabiles del mes
        /// </summary>
        /// <param name="diasInhabiles">Dias inhabiles del taller</param>
        /// <returns>Dias inhabiles en JSON</returns>
        private string ConvertirResultadoAJSON(List<DateTime> diasInhabiles)
        {
            try
            {
                #region Construccion del arreglo de eventos
                StringBuilder eventos = new StringBuilder(); 
                foreach(var dia in diasInhabiles)
                {
                    eventos.Append("'"+dia.ToString("dd/MM/yyyy") + "',");
                }

                #endregion

                StringBuilder json = new StringBuilder();
                json.Append("[");

                if (eventos.ToString().Trim().Length > 0)
                    json.Append(eventos.ToString().Substring(0, eventos.Length - 1));

                json.Append("]");
                return json.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ConvertirResultadoAJSON: " + ex.Message);
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
        /// <summary>
        /// Evento Clic para la busqueda de los mantenimientos programados
        /// </summary>>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarMantenimientosProgramados();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Presenta el detalle de una cita de mantenimiento
        /// </summary>
        protected void btnDetalles_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.PresentarDetalles(int.Parse(this.hdnIdCita.Value), bool.Parse(this.hdnEsCita.Value));
                if (this.CitasMantenimiento != null && this.CitasMantenimiento.Any())
                    this.EstablecerMantenimientos(this.CitasMantenimiento, this.ListadoDiasInhabiles);
                this.RegistrarScript("PantallaDetalleCita", "PresentarDetalleCita();");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnDetalles_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Redirige a calendarizar o recalendarizar un mantenimiento para unidad
        /// </summary>
        protected void btnDetallesCita_Click(object sender, EventArgs e)
        {
            try
            {
                bool esCita = bool.Parse(this.hdnEsCita.Value);
                int id = int.Parse(this.hdnIdCita.Value);

                if (!esCita)
                {
                    Session["CitaMantenimientoBO"] = this.CitasMantenimiento.FirstOrDefault(x => x.MantenimientoProgramado.MantenimientoProgramadoID == id);
                    this.LimpiarSesion();
                    this.presentador.ValidarRegistro(false);
                    this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ProgramarCitaMantenimientoUI.aspx"));
                }
                else
                {
                    Session["CitaMantenimientoBO"] = this.CitasMantenimiento.FirstOrDefault(x => x.CitaMantenimientoID == id);
                    this.LimpiarSesion();
                    this.presentador.ValidarRegistro(true);
                    this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ReprogramarCitaMantenimientoUI.aspx"));
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al redirigir al Registro", ETipoMensajeIU.ERROR, nombreClase + ".btnDetallesCita_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento Clic
        /// </summary>>
        protected void btnRedirectContacto_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CitasMantenimiento != null && this.CitasMantenimiento.Any())
                    this.EstablecerMantenimientos(this.CitasMantenimiento, this.ListadoDiasInhabiles);
                this.RegistrarScript("IrContactos", "IrContacto();");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Cambio de Pagina
        /// </summary>
        protected void grvContactoCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                List<DetalleContactoClienteBO> detalles = new List<DetalleContactoClienteBO>();
                this.ListadoContactosCliente.ForEach(x => { detalles.AddRange(x.Detalles); });

                this.grvContactoCliente.DataSource = detalles;
                this.grvContactoCliente.PageIndex = e.NewPageIndex;
                this.grvContactoCliente.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #region Eventos para el buscador

        /// <summary>
        /// TextChanged acitva el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
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

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.CitasMantenimiento != null && this.CitasMantenimiento.Any() && this.ListadoDiasInhabiles != null && this.ListadoDiasInhabiles.Any())
                    this.EstablecerMantenimientos(this.CitasMantenimiento, this.ListadoDiasInhabiles);
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

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.CitasMantenimiento != null && this.CitasMantenimiento.Any() && this.ListadoDiasInhabiles != null && this.ListadoDiasInhabiles.Any())
                    this.EstablecerMantenimientos(this.CitasMantenimiento, this.ListadoDiasInhabiles);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, this.ToString() + ".ibtnBuscaSucursal_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Evento del boton del buscador de bepensa
        /// </summary>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }

                //Esto se hace aquí para que por cada postback no quite los eventos del calendario
                if (this.CitasMantenimiento != null && this.CitasMantenimiento.Any() && this.ListadoDiasInhabiles != null && this.ListadoDiasInhabiles.Any())
                    this.EstablecerMantenimientos(this.CitasMantenimiento, this.ListadoDiasInhabiles);
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