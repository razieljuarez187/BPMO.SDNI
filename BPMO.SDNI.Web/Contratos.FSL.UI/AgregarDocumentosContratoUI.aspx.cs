// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
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
    public partial class AgregarDocumentosContratoUI : System.Web.UI.Page, IAgregarDocumentosContratoVIS
    {
        #region Atributos
        private AgregarDocumentosContratoPRE presentador;
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
                    return (EEstatusContrato) int.Parse(hdnEstatusContrato.Value);
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
        /// Usuario de Ultima Modificacion
        /// </summary>
        public int? UUA
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site) Page.Master;

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
        #endregion

        #region Métodos

        public void LimpiarSesion()
        {
            Session.Remove(CodigoUltimoObjeto);
            ucCatalogoDocumentos.LimpiarSesion();
        }

        /// <summary>
        /// Regresa al Modulo de Consulta
        /// </summary>
        public void RegresarAConsultar()
        {
            ucCatalogoDocumentos.LimpiarSesion();

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

        /// <summary>
        /// Redirecciona al Detalle del Contrato
        /// </summary>
        public void IrADetalleContrato()
        {
            Response.Redirect("DetalleContratoFSLUI.aspx");
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
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                ucCatalogoDocumentos.presentador = new ucCatalogoDocumentosPRE(ucCatalogoDocumentos);
                ucHerramientas.Presentador = new ucHerramientasFSLPRE(ucHerramientas);

                presentador = new AgregarDocumentosContratoPRE(this, ucHerramientas.Presentador, ucCatalogoDocumentos.presentador);               

                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso(); //SC_0008
                    presentador.Inicializar();
                }                
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            presentador.ActualizarDocumentos();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            presentador.RegresarADetalles();
        }
        #endregion
    }
}