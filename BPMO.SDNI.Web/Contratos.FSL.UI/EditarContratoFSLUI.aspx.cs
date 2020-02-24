// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
//Satisface al Caso de Uso CU026 - Registrar Terminación de Contrato Full Service Leasing
//Satisface al Reporte de Inconsistencia RI0008
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    /// <summary>
    /// UI de la Edicion de Contratos
    /// </summary>
    public partial class EditarContratoFSLUI : Page, IEditarContratoFSLVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador de la Edicion de Contrato
        /// </summary>
        private EditarContratoFSLPRE presentador;
        /// <summary>
        /// Nombre de la Clase
        /// </summary>
        private const string NombreClase = "EditarContratoFSLUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// Codigo del Paquete de Navegacion
        /// </summary>
        public string Codigo
        {
            get { return "ContratoFSLBO"; }
        }
        /// <summary>
        /// Codigo del Paquete de Navegacion del Contrato a Editar
        /// </summary>
        public string CodigoUltimoObjeto
        {
            get { return hdnCodigoUltimoContrato.Value; }
            set { hdnCodigoUltimoContrato.Value = value ?? string.Empty; }
        }
        /// <summary>
        /// Ultimo 
        /// </summary>
        public ContratoFSLBO UltimoObjeto
        {
            get { return Session[CodigoUltimoObjeto] as ContratoFSLBO; }
            set { Session[CodigoUltimoObjeto] = value; }
        }
        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnContratoID.Value))
                    return int.Parse(hdnContratoID.Value);
                return null;
            }
            set { hdnContratoID.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public EEstatusContrato? Estatus
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnEstatusContrato.Value))
                    return (EEstatusContrato)int.Parse(hdnEstatusContrato.Value);
                return null;
            }
            set
            {
                hdnEstatusContrato.Value = value != null
                                               ? Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture)
                                               : string.Empty;
            }
        }
        /// <summary>
        /// Usuario de Creación
        /// </summary>
        public int? UC
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnUC.Value))
                    return int.Parse(this.hdnUC.Value);
                return null;
            }
            set { this.hdnUC.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Fecha de Creación
        /// </summary>
        public DateTime? FC
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnFC.Value))
                    return DateTime.Parse(this.hdnFC.Value);
                return null;
            }
            set { this.hdnFC.Value = value != null ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Usuario de Ultima Modificacion
        /// </summary>
        public int? UUA
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// Fecha de Ultima Modificacion
        /// </summary>
        public DateTime? FUA
        {
            get { return DateTime.Now; }
        }
        /// <summary>
        /// Identificador de la Unidad Operativa del Contrato
        /// </summary>
        public int? UnidadOperativaContratoID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnUnidadOperativaContratoID.Value))
                    return int.Parse(hdnUnidadOperativaContratoID.Value);
                return null;
            }
            set
            {
                if (value != null)
                    hdnUnidadOperativaContratoID.Value = value.ToString();
                else
                {
                    hdnUnidadOperativaContratoID.Value = string.Empty;
                }
            }
        }

        #region SC_0008
        /// <summary>
        /// Devuelve el identificador del usuario que ha iniciado sesión en el sistema
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var master = (Site)Page.Master;
                if (master != null)
                    if (master.Usuario != null && master.Usuario.Usuario != null)
                        if (master.Usuario.Id.HasValue)
                            return master.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Devuelve el identificador de la unidad de adscripción desde la cual se ha iniciado sesión en el sistema
        /// </summary>
        public int? UnidadAdscripcionID
        {
            get
            {
                var master = (Site)Page.Master;
                if (master != null)
                    if (master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null)
                        if (master.Adscripcion.UnidadOperativa.Id.HasValue)
                            return master.Adscripcion.UnidadOperativa.Id.Value;
                return null;
            }
        }
        #endregion

        #region CU026
        /// <summary>
        /// Fecha de cierre del contrato
        /// </summary>
        public DateTime? FechaCierre
        {
            get
            {
                if (string.IsNullOrEmpty(hdnFechaCierre.Value))
                    return null;
                return Convert.ToDateTime(hdnFechaCierre.Value);
            }
            set
            {
                hdnFechaCierre.Value = value != null ? ((DateTime)FechaCierre).ToString() : "";
            }
        }
        /// <summary>
        /// Usuario que realiza el cierre del contrato
        /// </summary>
        public int? UsuarioCierre
        {
            get
            {
                if (string.IsNullOrEmpty(hdnUsuarioCierre.Value)) return null;
                return Convert.ToInt32(hdnUsuarioCierre.Value);
            }
            set { hdnUsuarioCierre.Value = value == null ? "" : value.ToString(); }
        }

        /// <summary>
        /// Observaciones del cierre del contrato
        /// </summary>
        public string ObservacionesCierre
        {
            get
            {
                if (string.IsNullOrEmpty(hdnObservacionesCierre.Value)) return null;
                return hdnObservacionesCierre.Value;
            }
            set { hdnObservacionesCierre.Value = value ?? string.Empty; }
        }
        /// <summary>
        /// Monto de Penalización
        /// </summary>
        public decimal? CantidadPenalizacion
        {
            get
            {
                if (string.IsNullOrEmpty(hdnCantidadPenalizacion.Value)) return null;
                return Convert.ToDecimal(hdnCantidadPenalizacion.Value);
            }
            set
            {
                hdnCantidadPenalizacion.Value = value == null ? "" : value.ToString();
            }
        }
        /// <summary>
        /// Motivo de Cierre
        /// </summary>
        public string MotivoCierre
        {
            get
            {
                if (string.IsNullOrEmpty(hdnMotivoCierre.Value)) return null;
                return hdnMotivoCierre.Value;
            }
            set { hdnMotivoCierre.Value = value ?? string.Empty; }
        }
        #endregion

        /// <summary>
        /// Determina si se cobrara INPC fijo o elegido por el usuario
        /// </summary>
        public bool? InpcFijo
        {
            get
            {
                if(ddlTipoInpc.SelectedIndex == 0) return null;
                return ddlTipoInpc.SelectedIndex == 2;
            }
            set
            {
                if(value == null)
                    ddlTipoInpc.SelectedIndex = 0;
                else
                    ddlTipoInpc.SelectedIndex = value.Value ? 2 : 1;
            }
        }
        /// <summary>
        /// Fecha en la que inicia el Cobro del INPC
        /// </summary>
        public DateTime? FechaInicioInpc
        {
            get
            {
                if(ddlAnioInicioInpc.SelectedIndex == 0) return null;
                if(ucInformacionPago.FechaInicioContrato == null) return null;
                var fechaContrato = ucInformacionPago.FechaInicioContrato;
                return fechaContrato.Value.AddYears(Int32.Parse(ddlAnioInicioInpc.SelectedValue) - 1);
            }
            set
            {
                if(value == null)
                    ddlAnioInicioInpc.SelectedIndex = 0;
                else
                {
                    var años = value.Value.Year - ucInformacionPago.FechaInicioContrato.Value.Year;
                    ddlAnioInicioInpc.SelectedIndex = años + 1;
                }
            }
        }
        /// <summary>
        /// Valor del INPC Fijo que se cobrará
        /// </summary>
        public decimal? ValorInpc
        {
            get
            {
                decimal valor;
                if(String.IsNullOrEmpty(txtValorInpc.Text.Trim())) return null;
                if(Decimal.TryParse(txtValorInpc.Text.Trim(), out valor)) { return valor; }
                return null;
            }
            set
            {
                txtValorInpc.Text = value != null ? value.ToString() : "";
            }
        }
        /// <summary>
        /// Inpc Configurado para el Contrato
        /// </summary>
        public INPCContratoBO InpcContrato
        {
            get
            {
                if(Session["InpcContrato"] == null) return null;
                return (INPCContratoBO)Session["InpcContrato"];
            }
            set
            {
                if(value == null)
                {
                    if(Session["InpcContrato"] == null) return;
                    Session.Remove("InpcContrato");
                }
                else
                    Session["InpcContrato"] = value;
            }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Carga Inicial de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucInformacionGeneral.Presentador = new ucInformacionGeneralPRE(ucInformacionGeneral);
                ucInformacionPago.Presentador = new ucInformacionPagoPRE(ucInformacionPago);
                ucClienteContrato.Presentador = new ucClienteContratoPRE(ucClienteContrato);
                ucCatalogoDocumentos.presentador = new ucCatalogoDocumentosPRE(ucCatalogoDocumentos);
                ucDatosRenta.Presentador = new ucDatosRentaPRE(ucDatosRenta);
                ucLineaContrato.Presentador = new ucLineaContratoFSLPRE(ucLineaContrato);
                ucHerramientas.Presentador = new ucHerramientasFSLPRE(ucHerramientas);
                ucDatosAdicionales.Presentador = new ucDatosAdicionalesAnexoPRE(ucDatosAdicionales);

                presentador = new EditarContratoFSLPRE(this, ucHerramientas.Presentador, ucInformacionGeneral.Presentador, ucClienteContrato.Presentador, ucDatosRenta.Presentador, ucInformacionPago.Presentador, ucLineaContrato.Presentador, ucCatalogoDocumentos.presentador, ucDatosAdicionales.Presentador);

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    presentador.Inicializar();
                }

                //SC0038
                this.presentador.CargarPlantillas();

                ucInformacionPago.CambiarFechaInicioContrato = FechaInicioContrato_Change;
                ucDatosRenta.CambiarPlazoMeses = PlazoMeses_Change;
                ucDatosRenta.VerDetalleLineaContrato = VerDetalle_Click;
                ucDatosRenta.AgregarUnidadClick = AgregarUnidad_Click;
                ucLineaContrato.AgregarClick = AgregarLineaContrato_Click;
                ucLineaContrato.CancelarClick = CancelarLineaContrato_Click;
                ucDatosRenta.RemoverLineaContrato = RemoverLineaContrato_Click;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR,
                                    NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Cambia la vista de la interfaz al resumen del contrato
        /// </summary>
        public void CambiarALinea()
        {
            mvCU023.ActiveViewIndex = 1;
        }
        /// <summary>
        /// Limpia la Session del Usuario
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove(CodigoUltimoObjeto);
            ucLineaContrato.LimpiarSesion();
            ucCatalogoDocumentos.LimpiarSesion();
            ucClienteContrato.LimpiarSesion();
            ucDatosRenta.LimpiarSesion();
        }

        /// <summary>
        /// Cambia la vista de la interfaz a la informacion del contrato
        /// </summary>
        public void CambiaAContrato()
        {
            mvCU023.ActiveViewIndex = 0;
        }
        /// <summary>
        /// Cambia la Vista de la interfaz a la informacion del INPC
        /// </summary>
        public void CambiarAINPC()
        {
            mvCU023.SetActiveView(vwConfiguracionINPC);
        }
        /// <summary>
        /// Regresa al Modulo de Consulta
        /// </summary>
        public void RegresarAConsultar()
        {
            ucCatalogoDocumentos.LimpiarSesion();
            ucLineaContrato.LimpiarSesion();

            Response.Redirect("ConsultarContratosFSLUI.aspx");
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
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
        /// Carga el Listado de Tipos de Archivo
        /// </summary>
        /// <param name="tipos"></param>
        public void CargarTiposArchivos(List<TipoArchivoBO> tipos)
        {
            ucCatalogoDocumentos.TiposArchivo = tipos;
        }

        /// <summary>
        /// Redirecciona al Detalle del Contrato
        /// </summary>
        public void IrADetalleContrato()
        {
            Response.Redirect("DetalleContratoFSLUI.aspx");
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
        /// Establece el Paquete de Navegacion para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="contrato">Identificador del Contrato Seleccionado</param>
        public void EstablecerPaqueteNavegacion(string Clave, ContratoBO contrato)
        {
            if (contrato != null && contrato.ContratoID != null) Session[Clave] = contrato;
            else
            {
                throw new Exception(NombreClase +
                                    ".EstablecerPaqueteNavegacion: El contrato proporcionado no contiene un identificador.");
            }
        }

        #region SC_0008
        /// <summary>
        /// Redirige a la página de aviso por fata de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        /// <summary>
        /// Permite habilitar o deshabilitar controles de usuario de acuerdo alos permisos de seguridad
        /// </summary>
        /// <param name="status">Estatus que se desea tengan los controles</param>
        public void PermitirRegistrar(bool status)
        {
            hlRegistroOrden.Enabled = false;
        }
        #endregion

        #region SC0038
        /// <summary>
        /// Obtiene las plantillas del Contrato
        /// </summary>
        /// <param name="key">Clave de la Plantilla</param>
        /// <returns></returns>
        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }
        #endregion

        #region RI0008
        /// <summary>
        /// Permite guardar con estatus en Curso un Contrato
        /// </summary>
        /// <param name="permitir"></param>
        public void PermitirGuardarEnCurso(bool permitir)
        {
            btnTermino.Enabled = permitir;
        }
        #endregion
        /// <summary>
        /// Años Configurados para el Contrato.
        /// </summary>
        /// <param name="listaAnios">Lista de Años configurados</param>
        public void PresentarAniosConfigurados(Dictionary<String, String> listaAnios)
        {
            ddlAnioInicioInpc.Items.Clear();

            if(listaAnios == null)
                listaAnios = new Dictionary<string, string>();

            ddlAnioInicioInpc.DataSource = listaAnios;
            ddlAnioInicioInpc.DataValueField = "key";
            ddlAnioInicioInpc.DataTextField = "value";
            ddlAnioInicioInpc.DataBind();

            ddlAnioInicioInpc.Items.Insert(0, new ListItem("SELECCIONE", "0"));
        }
        /// <summary>
        /// Activa o desactiva los controles de seleccion de INPC
        /// </summary>
        /// <param name="permitir">Determina el estado de los controles</param>
        public void PermitirControlesINPC(bool permitir)
        {
            ddlAnioInicioInpc.Enabled =
                txtValorInpc.Enabled = permitir;
        }
        /// <summary>
        /// Activa o desactiva el control de Valor de INPC
        /// </summary>
        /// <param name="permitir">Determina el estado de los controles</param>
        public void PermitirValorINPC(bool permitir)
        {
            txtValorInpc.Enabled = permitir;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento Guardar Contrato en Borrador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarContratoBorrador();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Evento Guardar Contrato en Curso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTermino_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarContratoTermino();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnTermino_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Evento Cancelar Edición de Contrato 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.RegresarADetalles();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento de Cambio en la Fecha de Inicio del Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FechaInicioContrato_Change(object sender, EventArgs e)
        {
            try
            {
                presentador.CalcularFechaFinContrato();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar la fecha de inicio del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".FechaInicioContrato_Change: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento de Cambio en el Plazo del Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlazoMeses_Change(object sender, EventArgs e)
        {
            try
            {
                presentador.CalcularFechaFinContrato();
                presentador.CalcularTotales();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al realizar el cambio del plazo.", ETipoMensajeIU.ERROR, NombreClase + ".PlazoMeses_Change: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento para Ver los Detalles de Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VerDetalle_Click(object sender, CommandEventArgs e)
        {
            try
            {
                var linea = e.CommandArgument as LineaContratoFSLBO;
                presentador.PrepararLinea(linea);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al desplegar el detalle de la unidad a rentar.", ETipoMensajeIU.ERROR, NombreClase + ".VerDetalle_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo disparado desde el evento evento AgregarClick del Control ucLineaContratoFSLUI
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void AgregarLineaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.AgregarLineaContrato();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al agregar la unidad al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".AgregarLineaContrato_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo disparado desde el evento evento AgregarClick del Control ucLineaContratoFSLUI
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parametros del evento</param>
        protected void CancelarLineaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CancelarLineaContrato();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar la asingación de la unidad al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".CancelarLineaContrato_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Metodo Disparado desde el evento AgregarUnidadClick para Agregar una unidad al Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AgregarUnidad_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.PrepararNuevaLinea();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al preparar los datos de la unidad a agregar al contrato.", ETipoMensajeIU.ERROR, NombreClase + ".AgregarUnidad_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Evento para remover un detalle o linea de contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemoverLineaContrato_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CalcularTotales();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al remover la unidad del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".RemoverLineaContrato_Click: " + ex.Message);
            }
        }
        /// <summary>
        /// Habre la seccion de Configuracion de INPC
        /// </summary>
        protected void btnConfigurarINPC_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.PresentarConfiguracionINPC();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la Configuración del INPC.", ETipoMensajeIU.ERROR, NombreClase + ".btnConfigurarINPC_OnClick: " + ex.Message);
            }
        }
        /// <summary>
        /// Guarda la Configuracion del INPC
        /// </summary>
        protected void btnGuardarINPC_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarConfiguracionINPC(true);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Guardar el Cambio en la Configuración del INPC.", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardarINPC_OnClick: " + ex.Message);
            }
        }
        /// <summary>
        /// Restablece y no Guarda la Configuracion de INPC Actual
        /// </summary>
        protected void btnCancelarINPC_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarConfiguracionINPC(false);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cancelar el Cambio en la Configuración del INPC.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelarINPC_OnClick: " + ex.Message);
            }
        }
        /// <summary>
        /// Cambia la Seleccion del Tipo de INPC
        /// </summary>
        protected void ddlTipoInpc_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                presentador.CambiarSeleccionTipoINPC();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cambiar la Configuracion del INPC.", ETipoMensajeIU.ERROR, NombreClase + ".ddlTipoInpc_OnSelectedIndexChanged: " + ex.Message);
            }
        }
        #endregion
    }
}
