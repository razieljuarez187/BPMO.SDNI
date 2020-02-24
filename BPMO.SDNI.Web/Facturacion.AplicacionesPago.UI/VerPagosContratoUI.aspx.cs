// Satisface al caso de uso CU038 - Ver Pagos de Contrato
// Satisface a la solicitud de cambios SC0008
// Satisface al Reporte de Inconsistencia RI0012

using System;
using System.Configuration;
using System.Web.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesPago.PRE;
using BPMO.SDNI.Facturacion.AplicacionesPago.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesPago.UI
{
    /// <summary>
    /// Clase que representa el formularion para ver los pagos de un contrato
    /// </summary>
    public partial class VerPagosContratoUI : Page, IVerPagosContratoVIS
    {
        #region [ Constantes ]
        /// <summary>
        /// Nombre de la variable de configuración para el formato del número de contrato
        /// </summary>
        private const String FORMATO_NUMERO_CONTRATO_INDEX = "FormatoNumeroContrato";
        #endregion

        #region Atributos
        /// <summary>
        /// Presentador de la UI
        /// </summary>
        private VerPagosContratoPRE presentador;
        /// <summary>
        /// Nombre del Interfaz de Usuario
        /// </summary>
        private const string nombreClase = "VerPagosContratoUI";
        #endregion
        
        #region Propiedades

        /// <summary>
        /// Usuario de Ultima Actualización
        /// </summary>
        public int? UsuarioId
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
        /// Referencia de Contrato Selecionada
        /// </summary>
        public string NumeroContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(txtFolioContrato.Text.Trim()))
                    return  txtFolioContrato.Text;

                return null;
            }
        }

        /// <summary>
        /// Unidad Operativa del Usuario
        /// </summary>
        public int? UnidadOperativaId
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id;
                return null;
            }
        }
        /// <summary>
        /// URL del Logotipo de la Unidad Operativa
        /// </summary>
        public string URLLogoEmpresa {
            get {
                Site masterMsj = (Site)Page.Master;
                if (masterMsj != null)
                    return masterMsj.URLLogoEmpresa;
                return null;
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
                presentador = new VerPagosContratoPRE(this);

                if (!Page.IsPostBack)
                {
                    presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistenacias al presentar la información", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load: " + ex.GetBaseException().Message);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Redirecciona al visor con el reporte de Historico de Pagos
        /// </summary>
        public void IrAImprimirHistorico()
        {
            Response.Redirect("../Buscador.UI/VisorReporteUI.aspx", true);
        }
        /// <summary>
        /// Establece el paquete de navegación a imprimir
        /// </summary>
        /// <param name="codigoNavegacion"></param>
        /// <param name="paqueteNavegacion"></param>
        public void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, object paqueteNavegacion)
        {
            Session["NombreReporte"] = codigoNavegacion;
            Session["DatosReporte"] = paqueteNavegacion;
        }
        /// <summary>
        /// Despliega el mensaje al usuario
        /// </summary>
        /// <param name="mensaje">Contenido del Mensaje</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Contenido interno del mensaje</param>
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
        /// Configura el Validador de Formato del Folio del Contrato
        /// </summary>
        public void ConfigurarValidadorFormato()
        {            
            String pattern = ConfigurationManager.AppSettings[VerPagosContratoUI.FORMATO_NUMERO_CONTRATO_INDEX];

            #region RI0012
            if (String.IsNullOrEmpty(pattern) || String.IsNullOrWhiteSpace(pattern))
                throw new NullReferenceException(String.Format("Verifique que en archivo de configuración del sitio este configurado la variable de configuración \"{0}\" con el formato de máscara para los folios de contrato", VerPagosContratoUI.FORMATO_NUMERO_CONTRATO_INDEX));
            #endregion

            revFolioContrato.ValidationExpression = pattern;
        }
        /// <summary>
        /// Redirige a la pagina Sin Permiso de Acceso
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), true);
        }
        /// <summary>
        /// Habilita o deshabilita el botón de Ver Pagos
        /// </summary>
        /// <param name="ver">Indica si se Habilia o no el Boton Ver Pagos</param>
        public void PermitirVerPagos(bool ver)
        {
            btnVerPagos.Enabled = ver;
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando se realiza la opcion de Visualizar los Pagos del Contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerPagos_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.ImprimirPagos();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconcistencias al Imprimir los Pagos del Contrato", ETipoMensajeIU.ERROR, nombreClase + ".btnVerHistorico_Click" + ex.Message);
            }
        } 
        #endregion
    }
}