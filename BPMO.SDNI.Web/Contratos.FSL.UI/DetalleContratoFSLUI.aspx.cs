// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
//Satisface al caso de uso CU026 - Registrar Terminación de Contrato Full Service Leasing
//Satisface al caso de uso CU093 - Imprimir Pagaré Contrato FSL
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
    public partial class DetalleContratoFSLUI : Page, IDetalleContratoFSLVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Detalle de Contrato FSL
        /// </summary>
        private DetalleContratoFSLPRE presentador;

        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "DetalleContratoFSLUI";

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
            set
            {
                hdnContratoID.Value = value != null ? value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Contrato consultado
        /// </summary>
        public ContratoFSLBO Contrato
        {
            get
            {
                if (Session[Codigo] is ContratoFSLBO)
                    return Session[Codigo] as ContratoFSLBO;

                return null;
            }
            set { Session[Codigo] = value; }
        }

        /// <summary>
        /// Estatus del Contrato
        /// </summary>
        public EEstatusContrato? EstatusContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnEstatusContrato.Value))
                    return (EEstatusContrato)int.Parse(hdnEstatusContrato.Value);
                return null;
            }
            set
            {
                hdnEstatusContrato.Value = value != null ? Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }

        /// <summary>
        /// Usuario de Ultima Modificacion
        /// </summary>
        public int? UUA
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
        /// Fecha de Ultima Modificacion
        /// </summary>
        public DateTime? FUA
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Identificador de Unidad Operativa del Contrato
        /// </summary>
        public int? UnidadOperativaContratoID
        {
            get
            {
                return !string.IsNullOrEmpty(hdnUnidadOperativaContratoID.Value)
                           ? (int?)int.Parse(hdnUnidadOperativaContratoID.Value)
                           : null;
            }
            set { hdnUnidadOperativaContratoID.Value = (value != null) ? value.ToString() : string.Empty; }
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

        /// <summary>
        /// Fecha en que se empieza a Aplicar el INPC del Contrato
        /// </summary>
        public DateTime? FechaInicioINPC
        {
            get
            {
                DateTime fecha;
                if (String.IsNullOrEmpty(txtFechaInicioINPC.Text.Trim())) return null;
                if (DateTime.TryParse(txtFechaInicioINPC.Text.Trim(), out fecha))
                {
                    return fecha;
                }
                return null;
            }
            set { txtFechaInicioINPC.Text = value != null ? value.Value.ToShortDateString() : ""; }
        }
        /// <summary>
        /// Determina si el INPC del Contrato es Fijo o Calculado por el Sistema
        /// </summary>
        public bool? InpcFijo
        {
            get
            {
                if (String.IsNullOrEmpty(txtTipoInpc.Text.Trim())) return null;
                return txtTipoInpc.Text == "FIJO";
            }
            set
            {
                txtTipoInpc.Text = value != null ? value.Value ? "FIJO" : "AUTOMÁTICO" : "";
            }
        }
        /// <summary>
        /// Valor del INPC que se aplicara al Contrato
        /// </summary>
        public decimal? ValorInpc
        {
            get
            {
                decimal valor;
                if (String.IsNullOrEmpty(txtValorINPC.Text.Trim())) return null;
                if (Decimal.TryParse(txtValorINPC.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set
            {
                txtValorINPC.Text = value != null ? value.ToString() : "";
            }
        }
        #endregion Propiedades

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Inicializacion Presentadores Controles de Usuario
                ucInformacionGeneral.Presentador = new ucInformacionGeneralPRE(ucInformacionGeneral);
                ucInformacionPago.Presentador = new ucInformacionPagoPRE(ucInformacionPago);
                ucClienteContrato.Presentador = new ucClienteContratoPRE(ucClienteContrato);
                ucCatalogoDocumentos.presentador = new ucCatalogoDocumentosPRE(ucCatalogoDocumentos);
                ucDatosRenta.Presentador = new ucDatosRentaPRE(ucDatosRenta);
                ucLineaContrato.Presentador = new ucLineaContratoFSLPRE(ucLineaContrato);
                ucHerramientas.Presentador = new ucHerramientasFSLPRE(ucHerramientas);
                ucDatosAdicionales.Presentador = new ucDatosAdicionalesAnexoPRE(ucDatosAdicionales);
                presentador = new DetalleContratoFSLPRE(this, ucHerramientas.Presentador, ucInformacionGeneral.Presentador, ucClienteContrato.Presentador, ucDatosRenta.Presentador, ucInformacionPago.Presentador, ucLineaContrato.Presentador, ucDatosAdicionales.Presentador);
                #endregion

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008

                    presentador.Inicializar();
                }

                //SC0038
                this.presentador.CargarPlantillas();

                #region Asociacion Metodos Handlers
                ucDatosRenta.VerDetalleLineaContrato = VerDetalleLineaContrato_Click;
                ucLineaContrato.CancelarClick = CancelarLineaContrato_Click;
                ucHerramientas.EliminarContrato = EliminarContrato_Click;
                #region SC0002
                ucHerramientas.EditarContratoFSL = EditarContratoFSL_Click;
                ucHerramientas.AgregarDocumentos = AgregarDocumentosContratoFSL_Click;
                #endregion
                ucHerramientas.CerrarContrato = CerrarContrato_Click;
                ucHerramientas.ModificarUnidadesContratoFSL = ModificarUnidadesContratoFSL_Click;
                ucHerramientas.ImprimirConstanciaBienes = ImprimirConstanciaBienes_Click;
                ucHerramientas.ImprimirManualOperaciones = ImprimirManualOperaciones_Click;
                ucHerramientas.ImprimirAnexoA = ImprimirAnexoA_Click;
                ucHerramientas.ImprimirAnexoB = ImprimirAnexoB_Click;
                ucHerramientas.ImprimirAnexoC = ImprimirAnexoC_Click;
                ucHerramientas.ImprimirContratoMaestro = ImprimirContratoMaestro_Click;
                ucHerramientas.ImprimirAnexosContrato = ImprimirAnexosContrato_Click;
                this.ucHerramientas.ImprimirPagare = this.btnImprimirPagare_Click;
                #endregion
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
        /// Inicializa los controles que contienen los datos del contrato
        /// </summary>
        public void InicializarControles()
        {
            ucInformacionPago.ConfigurarModoConsultar();
            ucInformacionGeneral.ConfigurarModoConsultar();
            ucClienteContrato.ConfigurarModoConsultar();
            ucDatosRenta.ConfigurarModoConsultar();
            ucLineaContrato.ConfigurarModoConsultar();
            ucCatalogoDocumentos.EstablecerModoEdicion(false);
            ucHerramientas.OcultarFormatosContrato();
            ucDatosAdicionales.ConfigurarModoConsultar();
        }

        /// <summary>
        /// Cambia la vista de la interfaz al resumen del contrato
        /// </summary>
        public void CambiarALinea()
        {
            mvCU022.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Cambia la vista de la interfaz a la informacion del contrato
        /// </summary>
        public void CambiarAContrato()
        {
            mvCU022.ActiveViewIndex = 0;
        }

        /// <summary>
        /// Retorna a la pagina de Consulta de Contrato FSL
        /// </summary>
        public void RegresarAConsultar()
        {
            Session.Remove(Codigo);
            ucLineaContrato.LimpiarSesion();
            Response.Redirect("ConsultarContratosFSLUI.aspx");
        }

        /// <summary>
        /// Despliega los datos del Contrato en la Interfaz de Usuario
        /// </summary>
        /// <param name="contrato"></param>
        public void DatosAInterfazUsuario(ContratoFSLBO contrato)
        {
            ucCatalogoDocumentos.EstablecerListasArchivos(contrato.DocumentosAdjuntos, new List<TipoArchivoBO>());
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
        ///  Establece el paquete navegacion de UltimoContrato para Edicion.
        /// </summary>
        /// <param name="clave">Clave del paquete de navegacion</param>
        /// <param name="contrato">Contrato a editar</param>
        public void EstablecerPaqueteNavegacionEditar(string clave, ContratoFSLBO contrato)
        {
            if (!String.IsNullOrEmpty(clave)) Session[clave] = contrato;
            else
                throw new Exception(NombreClase +
                                    ".EstablecerPaqueteNavegacionEditar: No se ha proporcionado la clave para el paquete de navegación del contrato a editar.");
        }

        /// <summary>
        /// Redirecciona a la pantalla de edicion
        /// </summary>
        public void IrAEditar()
        {
            Response.Redirect("EditarContratoFSLUI.aspx");
        }
        /// <summary>
        /// Redirige a la Interfaz para Modificar las Lineas de Contrato
        /// </summary>
        public void IrAModificarLineas()
        {
            Response.Redirect("EditarLineasContratoFSLUI.aspx", true);
        }

        #region SC0002
        /// <summary>
        /// Redirecciona a la pantalla de agregar documento
        /// </summary>
        public void IrAAgregarDocs()
        {
            Response.Redirect("AgregarDocumentosContratoUI.aspx");
        }
        #endregion

        /// <summary>
        /// Establece el Paquete de Navegacion para las impresiones
        /// </summary>
        /// <param name="codigoNavegacion"></param>
        /// <param name="DatosReporte"></param>
        public void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, Dictionary<string, object> DatosReporte)
        {
            Session["NombreReporte"] = codigoNavegacion;
            Session["DatosReporte"] = DatosReporte;
        }
        /// <summary>
        /// Despliega el visor de impresiones
        /// </summary>
        public void IrAImprimir()
        {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", true);
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

        #region SC_0008
        /// <summary>
        /// Permite habilitar o deshabilitar controles de usuario de acuerdo alos permisos de seguridad
        /// </summary>
        /// <param name="status">Estatus que se desea tengan los controles</param>
        public void PermitirRegistrar(bool status)
        {
            hlRegistroOrden.Enabled = false;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion

        #region CU026
        public void IrACerrarContrato()
        {
            Response.Redirect("CerrarContratoFSLUI.aspx");
        }
        #endregion

        #region SC0038
        public List<object> ObtenerPlantillas(string key)
        {
            return (List<object>)Session[key];
        }
        #endregion
        #endregion

        #region Eventos

        protected void VerDetalleLineaContrato_Click(object sender, CommandEventArgs e)
        {
            var linea = e.CommandArgument as LineaContratoFSLBO;
            presentador.PrepararLinea(linea);
        }

        protected void CancelarLineaContrato_Click(object sender, EventArgs e)
        {
            CambiarAContrato();
        }

        protected void EliminarContrato_Click(object sender, EventArgs e)
        {
            RegistrarScript("mnContratos_MenuItemClick.EliminarContrato", "ValidaEliminarContrato();");
        }

        #region SC0002
        protected void EditarContratoFSL_Click(object sender, EventArgs e)
        {
            // Editar Contrato
            presentador.EditarContrato();
        }

        protected void AgregarDocumentosContratoFSL_Click(object sender, EventArgs e)
        {
            // Editar Contrato
            presentador.AgregarDocumentos();
        }
        #endregion

        protected void ModificarUnidadesContratoFSL_Click(object sender, EventArgs e)
        {
            presentador.ModificarUnidadesContrato();
        }

        protected void CerrarContrato_Click(object sender, EventArgs e)
        {
            presentador.CerrarContrato();
        }

        protected void btnEliminarContratoBorrador_Click(object sender, EventArgs e)
        {
            presentador.EliminarContrato();
        }

        protected void ImprimirConstanciaBienes_Click(object sender, EventArgs e)
        {
            presentador.ImprimirConstanciaBienes();
        }

        protected void ImprimirManualOperaciones_Click(object sender, EventArgs e)
        {
            presentador.ImprimirManualOperaciones();
        }

        protected void ImprimirAnexoA_Click(object sender, EventArgs e)
        {
            presentador.ImprimirAnexoA();
        }

        protected void ImprimirContratoMaestro_Click(object sender, EventArgs e)
        {
            presentador.ImprimirContratoMaestro();
        }

        protected void ImprimirAnexosContrato_Click(object sender, EventArgs e)
        {
            presentador.ImprimirAnexosContrato();
        }

        protected void ImprimirAnexoB_Click(object sender, EventArgs e)
        {
            Response.Redirect("../MapaSitio.UI/PaginaError.aspx");
        }

        protected void ImprimirAnexoC_Click(object sender, EventArgs e)
        {
            presentador.ImprimirAnexoC();
        }

        protected void btnImprimirPagare_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.ImprimirPagare();
            }
            catch (Exception ex) { this.MostrarMensaje("Inconsistencias al intentar imprimir el pagaré.", ETipoMensajeIU.ERROR, "DetalleContratoFSLUI.btnImprimirPagare_Click: " + ex.Message); }
        }
        #endregion
    }
}