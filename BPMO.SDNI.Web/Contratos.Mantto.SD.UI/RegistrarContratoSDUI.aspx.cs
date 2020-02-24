//Satisface al CU027 - Registrar Contrato de Mantenimiento
//Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
using System;
using System.Collections.Generic;
using System.Web.UI;

using System.Configuration;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Contratos.Mantto.BO;
using BPMO.SDNI.Contratos.Mantto.VIS;
using BPMO.SDNI.Contratos.Mantto.PRE;

namespace BPMO.SDNI.Contratos.Mantto.SD.UI
{
    public partial class RegistrarContratoSDUI : System.Web.UI.Page, IRegistrarContratoManttoVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase en la cual estamos trabajando
        /// </summary>
        private string nombreClase = "RegistrarContratoSDUI";
        /// <summary>
        /// presentador del UC de información general del contrato de renta diaria
        /// </summary>
        private RegistrarContratoManttoPRE presentador;
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
        /// Obtiene el nombre de la Unidad Operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public string UnidadOperativaNombre
        {
            get
            {
                Site masterMsj = (Site)Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Nombre : null;
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

        public int? ContratoID
        {
            get { return this.ucContratoUI.ContratoID; }
            set { this.ucContratoUI.ContratoID = value; }
        }
        public string NumeroContrato
        {
            get { return this.ucContratoUI.NumeroContrato; }
            set { this.ucContratoUI.NumeroContrato = value; }
        }
        public int? TipoContratoID
        {
            get { return (int)ETipoContrato.SD; }
        }
        public int? EstatusID
        {
            get { return this.ucContratoUI.EstatusID; }
            set { this.ucContratoUI.EstatusID = value; }
        }
        public DateTime? FC
        {
            get { return DateTime.Now; }
        }
        public int? UC
        {
            get { return this.UsuarioID; }
        }
        public DateTime? FUA
        {
            get { return this.FC; }
        }
        public int? UUA
        {
            get { return this.UC; }
        }

        public string RepresentanteEmpresa
        {
            get { return this.ucContratoUI.RepresentanteEmpresa; }
            set { this.ucContratoUI.RepresentanteEmpresa = value; }
        }

        public int? SucursalID
        {
            get { return this.ucContratoUI.SucursalID; }
            set { this.ucContratoUI.SucursalID = value; }
        }
        public string SucursalNombre
        {
            get { return ((IucContratoManttoVIS)this.ucContratoUI).SucursalNombre; }
            set { ((IucContratoManttoVIS)this.ucContratoUI).SucursalNombre = value; }
        }

        public DateTime? FechaContrato
        {
            get { return this.ucContratoUI.FechaContrato; }
            set { this.ucContratoUI.FechaContrato = value; }
        }
        public DateTime? FechaInicioContrato
        {
            get { return this.ucContratoUI.FechaInicioContrato; }
            set { this.ucContratoUI.FechaInicioContrato = value; }
        }
        public DateTime? FechaTerminacionContrato
        {
            get { return this.ucContratoUI.FechaTerminacionContrato; }
            set { this.ucContratoUI.FechaTerminacionContrato = value; }
        }
        public int? Plazo
        {
            get { return this.ucContratoUI.Plazo; }
            set { this.ucContratoUI.Plazo = value; }
        }

        public string CodigoMoneda
        {
            get { return this.ucContratoUI.CodigoMoneda; }
            set { this.ucContratoUI.CodigoMoneda = value; }
        }

        public int? ClienteID
        {
            get { return this.ucContratoUI.ClienteID; }
            set { this.ucContratoUI.ClienteID = value; }
        }
        public bool? ClienteEsFisica
        {
            get { return this.ucContratoUI.ClienteEsFisica; }
            set { this.ucContratoUI.ClienteEsFisica = value; }
        }
        public int? CuentaClienteID
        {
            get { return this.ucContratoUI.CuentaClienteID; }
            set { this.ucContratoUI.CuentaClienteID = value; }
        }
        public string CuentaClienteNombre
        {
            get { return this.ucContratoUI.CuentaClienteNombre; }
            set { this.ucContratoUI.CuentaClienteNombre = value; }
        }
        public int? CuentaClienteTipoID
        {
            get { return this.ucContratoUI.CuentaClienteTipoID; }
            set { this.ucContratoUI.CuentaClienteTipoID = value; }
        }
        public string ClienteDireccionCompleta
        {
            get { return this.ucContratoUI.ClienteDireccionCompleta; }
            set { this.ucContratoUI.ClienteDireccionCompleta = value; }
        }
        public string ClienteDireccionCalle
        {
            get { return this.ucContratoUI.ClienteDireccionCalle; }
            set { this.ucContratoUI.ClienteDireccionCalle = value; }
        }
        public string ClienteDireccionCodigoPostal
        {
            get { return this.ucContratoUI.ClienteDireccionCodigoPostal; }
            set { this.ucContratoUI.ClienteDireccionCodigoPostal = value; }
        }
        public string ClienteDireccionCiudad
        {
            get { return this.ucContratoUI.ClienteDireccionCiudad; }
            set { this.ucContratoUI.ClienteDireccionCiudad = value; }
        }
        public string ClienteDireccionEstado
        {
            get { return this.ucContratoUI.ClienteDireccionEstado; }
            set { this.ucContratoUI.ClienteDireccionEstado = value; }
        }
        public string ClienteDireccionMunicipio
        {
            get { return this.ucContratoUI.ClienteDireccionMunicipio; }
            set { this.ucContratoUI.ClienteDireccionMunicipio = value; }
        }
        public string ClienteDireccionPais
        {
            get { return this.ucContratoUI.ClienteDireccionPais; }
            set { this.ucContratoUI.ClienteDireccionPais = value; }
        }
        public string ClienteDireccionColonia
        {
            get { return this.ucContratoUI.ClienteDireccionColonia; }
            set { this.ucContratoUI.ClienteDireccionColonia = value; }
        }

        public List<RepresentanteLegalBO> RepresentantesLegales
        {
            get { return this.ucContratoUI.RepresentantesSeleccionados; }
            set { this.ucContratoUI.RepresentantesSeleccionados = value; }
        }
        public bool? SoloRepresentantes
        {
            get { return this.ucContratoUI.SoloRepresentantes; }
            set { this.ucContratoUI.SoloRepresentantes = value; }
        }
        public List<ObligadoSolidarioBO> ObligadosSolidarios
        {
            get { return this.ucContratoUI.ObligadosSolidariosSeleccionados; }
            set { this.ucContratoUI.ObligadosSolidariosSeleccionados = value; }
        }
        public bool? ObligadosComoAvales
        {
            get { return this.ucContratoUI.ObligadosComoAvales; }
            set { this.ucContratoUI.ObligadosComoAvales = value; }
        }
        public List<AvalBO> Avales
        {
            get { return this.ucContratoUI.AvalesSeleccionados; }
            set { this.ucContratoUI.AvalesSeleccionados = value; }
        }

        public List<LineaContratoManttoBO> LineasContrato
        {
            get { return this.ucContratoUI.LineasContrato; }
            set { this.ucContratoUI.LineasContrato = value; }
        }

        public string UbicacionTaller
        {
            get { return this.ucContratoUI.UbicacionTaller; }
            set { this.ucContratoUI.UbicacionTaller = value; }
        }
        public decimal? DepositoGarantia
        {
            get { return this.ucContratoUI.DepositoGarantia; }
            set { this.ucContratoUI.DepositoGarantia = value; }
        }
        public decimal? ComisionApertura
        {
            get { return this.ucContratoUI.ComisionApertura; }
            set { this.ucContratoUI.ComisionApertura = value; }
        }
        public int? IncluyeSeguroID
        {
            get { return this.ucContratoUI.IncluyeSeguroID; }
            set { this.ucContratoUI.IncluyeSeguroID = value; }
        }
        public int? IncluyeLavadoID
        {
            get { return this.ucContratoUI.IncluyeLavadoID; }
            set { this.ucContratoUI.IncluyeLavadoID = value; }
        }
        public int? IncluyePinturaRotulacionID
        {
            get { return this.ucContratoUI.IncluyePinturaRotulacionID; }
            set { this.ucContratoUI.IncluyePinturaRotulacionID = value; }
        }
        public int? IncluyeLlantasID
        {
            get { return this.ucContratoUI.IncluyeLlantasID; }
            set { this.ucContratoUI.IncluyeLlantasID = value; }
        }
        public string DireccionAlmacenaje
        {
            get { return this.ucContratoUI.DireccionAlmacenaje; }
            set { this.ucContratoUI.DireccionAlmacenaje = value; }
        }

        public List<DatoAdicionalAnexoBO> DatosAdicionales
        {
            get { return this.ucContratoUI.DatosAdicionales; }
            set { this.ucContratoUI.DatosAdicionales = value; }
        }

        public string Observaciones
        {
            get { return this.ucContratoUI.Observaciones; }
            set { this.ucContratoUI.Observaciones = value; }
        }
        public int? DireccionClienteID
        {
            get { return this.ucContratoUI.DireccionClienteID; }
            set { this.ucContratoUI.DireccionClienteID = value; }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new RegistrarContratoManttoPRE(this, this.ucContratoUI, this.ucContratoUI, this.ucCatalogoDocumentos);
                this.ucContratoUI.TipoContratoID = this.TipoContratoID;

                if (!this.IsPostBack)
                    this.presentador.PrepararNuevo();
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
        }

        public void PermitirGuardarBorrador(bool permitir)
        {
            this.btnGuardar.Enabled = permitir;
        }
        public void PermitirGuardarTerminado(bool permitir)
        {
            this.btnTermino.Enabled = permitir;
        }

        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.SD.UI/ConsultarContratoSDUI.aspx"));
        }
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.Mantto.SD.UI/DetalleContratoSDUI.aspx"));
        }

        public void LimpiarSesion()
        {

        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }

        /// <summary>
        /// Actualiza el listado de docuemntos en caso de ocurrir un error
        /// </summary>
        private void ActualizarDocumentos()
        {
            this.ucCatalogoDocumentos.InicializarControl(new List<ArchivoBO>(), new List<TipoArchivoBO>());
            this.ucCatalogoDocumentos.LimpiarSesion();
            this.ucCatalogoDocumentos.LimpiaCampos();
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
        #endregion

        #region Eventos
        protected void btnTermino_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.RegistrarTerminada();
            }
            catch (Exception ex)
            {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnTermino_Click:" + ex.Message);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.RegistrarBorrador();
            }
            catch (Exception ex)
            {
                this.ActualizarDocumentos();
                MostrarMensaje("Inconsistencia al guardar el contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnGuardar_Click:" + ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.CancelarRegistro();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al cancelar el registro del contrato.", ETipoMensajeIU.ERROR, nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        #endregion
    }
}