using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.PRE;
using BPMO.SDNI.Contratos.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.UI
{
    public partial class CambiarContratoSucursalUI : Page, ICambiarContratoSucursalVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador con los metodos principales
        /// </summary>
        private CambiarContratoSucursalPRE presentador;
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string nombreClase = typeof(CambiarContratoSucursalUI).Name;
        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal = 0
        }
        #endregion
        #region Propiedades
        #region Propiedades Buscador
        public string ViewState_Guid
        {
            get
            {
                if(ViewState["GuidSession"] == null)
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
                if(Session[nombreSession] != null)
                    objeto = (Session[nombreSession]);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if(value != null)
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
                if(Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid]);

                return objeto;
            }
            set
            {
                if(value != null)
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
        #endregion Propiedades Buscador
        /// <summary>
        /// Id del Contrato
        /// </summary>
        public Int32? ContratoId
        {
            get
            {
                int? contratoId = null;
                if(!String.IsNullOrEmpty(this.hdnContratoId.Value) && !String.IsNullOrWhiteSpace(this.hdnContratoId.Value))
                    contratoId = Int32.Parse(this.hdnContratoId.Value);
                return contratoId;
            }
            set
            {
                this.hdnContratoId.Value = value != null ? value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        public string NumeroContrato
        {
            get
            {
                if(!String.IsNullOrEmpty(this.txtNumeroContrato.Text.Trim()) && !String.IsNullOrWhiteSpace(this.txtNumeroContrato.Text.Trim()))
                    return this.txtNumeroContrato.Text.Trim();
                return null;
            }
            set
            {
                this.txtNumeroContrato.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Tipo del Contrato: RD/FSL/CM/SD
        /// </summary>
        public ETipoContrato? TipoContrato
        {
            get
            {
                ETipoContrato? tipoContrato = null;
                if(!String.IsNullOrEmpty(this.hdnTipoContrato.Value) && !String.IsNullOrWhiteSpace(this.hdnTipoContrato.Value))
                    tipoContrato = (ETipoContrato?)Int32.Parse(this.hdnTipoContrato.Value);
                return tipoContrato;
            }
            set
            {
                this.hdnTipoContrato.Value = value != null ? ((Int32)value).ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Contrato Completo que sera Actualizado
        /// </summary>
        public ContratoBO ContratoConsultado
        {
            get
            {
                ContratoBO contrato = (ContratoBO)Session["ContratoBO_ContratoConsultado"];
                if(contrato != null)
                    return contrato;
                return null;
            }
            set
            {
                if(value != null)
                    Session["ContratoBO_ContratoConsultado"] = value;
                else
                    Session.Remove("ContratoBO_ContratoConsultado");
            }
        }
        /// <summary>
        /// Contrato de referencia para actualizar
        /// </summary>
        public ContratoBO ObjetoAnterior
        {
            get
            {
                ContratoBO contrato = (ContratoBO)Session["ContratoBO_ObjetoAnterior"];
                if(contrato != null)
                    return contrato;
                return null;
            }
            set
            {
                if(value != null)
                    Session["ContratoBO_ObjetoAnterior"] = value;
                else
                    Session.Remove("ContratoBO_ObjetoAnterior");
            }
        }
        /// <summary>
        /// Id de la Sucursal Original
        /// </summary>
        public int? SucursalIdAntigua
        {
            get
            {
                int? sucursalId = null;
                if(!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalId = int.Parse(this.hdnSucursalID.Value);
                return sucursalId;
            }
            set
            {
                this.hdnSucursalID.Value = value != null ? value.Value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Nombre de la Sucursal Original
        /// </summary>
        public string SucursalNombreAntigua
        {
            get
            {
                if(!String.IsNullOrEmpty(this.txtSucursal.Text.Trim()) && !String.IsNullOrWhiteSpace(this.txtSucursal.Text.Trim()))
                    return this.txtSucursal.Text.Trim();
                return null;
            }
            set
            {
                this.txtSucursal.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
            }
        }
        /// <summary>
        /// Id de la Sucursal Nueva
        /// </summary>
        public int? SucursalIdNueva
        {
            get
            {
                int? sucursalId = null;
                if(!String.IsNullOrEmpty(this.hdnSucursalIdNueva.Value))
                    sucursalId = int.Parse(this.hdnSucursalIdNueva.Value);
                return sucursalId;
            }
            set
            {
                this.hdnSucursalIdNueva.Value = value != null ? value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Nombre de la Sucursal Nueva
        /// </summary>
        public string SucursalNombreNueva
        {
            get
            {
                if(!String.IsNullOrEmpty(this.txtSucursalNombreNueva.Text.Trim()) && !String.IsNullOrWhiteSpace(this.txtSucursalNombreNueva.Text.Trim()))
                    return this.txtSucursalNombreNueva.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                this.txtSucursalNombreNueva.Text = !String.IsNullOrEmpty(value) ? value.ToUpper() : String.Empty;
            }
        }
        /// <summary>
        /// Observaciones sobre el cambio del Contrato
        /// </summary>
        public string Observaciones
        {
            get
            {
                if(!String.IsNullOrEmpty(this.txtObservaciones.Text.Trim()) && !String.IsNullOrWhiteSpace(this.txtObservaciones.Text.Trim()))
                    return this.txtObservaciones.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                this.txtObservaciones.Text = !String.IsNullOrEmpty(value) ? value : String.Empty;
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
                presentador = new CambiarContratoSucursalPRE(this);
                if(!IsPostBack)
                {
                    presentador.PrepararCambioSucursal();
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion
        #region Metodos
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
        /// Limpia la sesion de la página
        /// </summary>
        public void LimpiarSesion()
        {
            this.Session.Remove("ContratoBO_ContratoConsultado");
            this.Session.Remove("ContratoBO_ObjetoAnterior");
        }
        /// <summary>
        /// Redirecciona a la pantalla de consulta de contratos
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.UI/ConsultarContratoSucursalUI.aspx"), true);
        }
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), true);
        }
        /// <summary>
        /// Determina el Paquete que sera enviado a la interfaz de detalle
        /// </summary>
        /// <param name="nombre">Nombre del paquete</param>
        /// <param name="valor">Objeto que sera enviado</param>
        public ContratoBO ObtenerPaqueteNavegacion()
        {
            ContratoBO contrato = (ContratoBO)Session["ContratoProxyBO"];
            if(contrato == null)
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: No se encontro el Contrato que Cambiara de Sucursal");
            this.Session.Remove("ContratoProxyBO");
            return contrato;
        }
        /// <summary>
        /// Establece en la Interfaz el Objeto de Navegación
        /// </summary>
        /// <param name="objetoNavegacion">Contrato que cambiara de sucursal</param>
        public void EstablecerPaqueteNavegacion(ContratoBO objetoNavegacion)
        {
            if(objetoNavegacion == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El contrato que cambiara de sucursal no puede ser Nulo");
            this.ContratoId = objetoNavegacion.ContratoID != null ? objetoNavegacion.ContratoID : null;
            this.TipoContrato = objetoNavegacion.Tipo != null ? objetoNavegacion.Tipo : null;
        }
        /// <summary>
        /// Deshabilita los campos antiguos del Contrato
        /// </summary>
        public void DesactivarCamposIniciales()
        {
            this.txtNumeroContrato.Enabled = false;
            this.txtSucursal.Enabled = false;
        }
        /// <summary>
        /// Desactiva los objetos de la interfaz una vez realizado el cambio de sucursal
        /// </summary>
        public void PermitirGuardar(bool permitir)
        {
            this.btnGuardar.Enabled = permitir;
            this.btnCancelar.Enabled = permitir;
            this.ibtnBuscarSucursal.Enabled = permitir;
            this.txtSucursalNombreNueva.Enabled = permitir;
            this.txtObservaciones.Enabled = permitir;
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
        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }
        #endregion
        #endregion
        #region Eventos
        #region Eventos para el Buscador
        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSucursalNombreNueva_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = SucursalNombreNueva;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombreNueva = nombreSucursal;
                if(SucursalNombreNueva != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    SucursalNombreNueva = null;
                }
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursalNombreNueva_TextChanged" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch(ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Sucursal:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.CancelarCambio();
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
                presentador.CambiarSucursal();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Ocurrio un problema al Cambiar el Contrato de Sucursal", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion        
    }
}