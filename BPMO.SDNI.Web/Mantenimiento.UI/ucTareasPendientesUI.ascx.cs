//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.UI
{
    /// <summary>
    /// Clase para el control de la UI general de tareas pendientes
    /// </summary>
    public partial class ucTareasPendientesUI : System.Web.UI.UserControl, IucTareaPendienteVIS
    {
        #region Atributos
        /// <summary>
        /// Presentador general de tarea pendiente
        /// </summary>
        ucTareaPendientePRE presentador;

        /// <summary>
        /// Nombre de clase
        /// </summary>
        private const string nombreClase = "ucTareaPendienteUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador de la tarea pendiente
        /// </summary>
        public int? TareaPendienteID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnTareaPendienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnTareaPendienteID.Value))
                {
                    if (Int32.TryParse(this.hdnTareaPendienteID.Value, out val))
                        return val;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTareaPendienteID.Value = value.Value.ToString();
                else
                    this.hdnTareaPendienteID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador del modelo
        /// </summary>
        public int? ModeloID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value))
                    if (Int32.TryParse(this.hdnModeloID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.Value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnUnidadID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadID.Value))
                    if (Int32.TryParse(this.hdnUnidadID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadID.Value = value.Value.ToString();
                else
                    this.hdnUnidadID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la unidad operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnUnidadOperativaID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadOperativaID.Value))
                {
                    if (Int32.TryParse(this.hdnUnidadOperativaID.Value, out val))
                        return val;
                    else
                    {
                        Site masterMsj = (Site)Page.Master;

                        if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                            return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    }
                }
                else
                {
                    Site masterMsj = (Site)Page.Master;

                    if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                        return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadOperativaID.Value = value.Value.ToString();
                else
                    this.hdnUnidadOperativaID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el numero de serie de la unidad
        /// </summary>
        public string NumeroSerie
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TextNumeroSerie.Text) && !string.IsNullOrWhiteSpace(this.TextNumeroSerie.Text))
                    return this.TextNumeroSerie.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.TextNumeroSerie.Text = value;
                else
                    this.TextNumeroSerie.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el numero economico de la unidad
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TextNumeroEconomico.Text) && !string.IsNullOrWhiteSpace(this.TextNumeroEconomico.Text))
                    return this.TextNumeroEconomico.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.TextNumeroEconomico.Text = value;
                else
                    this.TextNumeroEconomico.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el modelo de la unidad
        /// </summary>
        public string Modelo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TextModelo.Text) && !string.IsNullOrWhiteSpace(this.TextModelo.Text))
                    return this.TextModelo.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                if (value != null)
                    this.TextModelo.Text = value;
                else
                    this.TextModelo.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la descripcion de la tarea pendiente
        /// </summary>
        public string Descripcion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TextDescripcion.Text) && !string.IsNullOrWhiteSpace(this.TextDescripcion.Text))
                    return this.TextDescripcion.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                if (value != null)
                    this.TextDescripcion.Text = value;
                else
                    this.TextDescripcion.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o estable el estatus de la Tarea
        /// </summary>
        public bool? Activo {
            get {
                if (this.rbtnActivo.Checked) return true;
                else if (this.rbtnInactivo.Checked) return false;
                else return null;
            }
            set {
                if (value != null) {
                    if ((bool)value) this.rbtnActivo.Checked = true;
                    else this.rbtnInactivo.Checked = true;
                } else {
                    this.rbtnActivo.Checked = false;
                    this.rbtnInactivo.Checked = false;
                }
            }
        }

        /// <summary>
        /// Obtiene la fecha de creación
        /// </summary>
        public DateTime? FC
        {
            get { return DateTime.Now; }
            set { }
        }

        /// <summary>
        /// Obtiene la fecha de ultima actualizacion
        /// </summary>
        public DateTime? FUA
        {
            get { return this.FC; }
        }

        /// <summary>
        /// Obtiene el identificador del usuario creador
        /// </summary>
        public int? UC
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        /// <summary>
        /// Obtiene el identificador del usuario ultima actualizacion
        /// </summary>
        public int? UUA
        {
            get { return this.UC; }
        }

        /// <summary>
        /// Obtiene el usuario
        /// </summary>
        public UsuarioBO Usuario
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario;
                return null;
            }
        }

        /// <summary>
        /// Obtiene la unidad operativa
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                {
                    this.UnidadOperativaID = masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };
                }
                return null;
            }
        }

        #region Propiedades buscador
        /// <summary>
        /// Manejo del buscador en la pagina principal
        /// </summary>
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

        /// <summary>
        /// Objeto seleccionado del buscador
        /// </summary>
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

        /// <summary>
        /// Objeto para buscar
        /// </summary>
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

        /// <summary>
        /// Tipo catalogo de busqueda
        /// </summary>
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

        /// <summary>
        /// Enum de catalogos para buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            UnidadIdealease,
            Modelo
        }
        #endregion
        #endregion

        #region Metodos

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucTareaPendientePRE(this);
        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Deshabilita campos de edicion para detalle
        /// </summary>
        public void DeshabilitarCamposEdicion()
        {
            this.TextDescripcion.Enabled = false;
        }

        /// <summary>
        /// Habilita o inhabilita la edición de los campos de estatus
        /// </summary>
        /// <param name="permitir">Indica si permitirá manipulación o no</param>
        public void PermitirEdicionEstatus(bool permitir) {
            this.rbtnActivo.Enabled = permitir;
            this.rbtnInactivo.Enabled = permitir;
        }

        /// <summary>
        /// Limpia los datos en sesión
        /// </summary>
        public void LimpiarSesion()
        {
            Session["TareaPendienteBO"] = null;
        }

        /// <summary>
        /// Prepara la vista inicial de la UI
        /// </summary>
        public void PrepararVista() {
            this.TextNumeroSerie.Enabled = false;
            this.TextNumeroEconomico.Enabled = false;
            this.TextModelo.Enabled = false;
            this.ibtnBuscarNumeroSerie.Visible = false;
            this.idbtnBuscarNumeroEconomico.Visible = false;
            this.idBtnBuscarModelo.Visible = false;
            this.TextDescripcion.Enabled = false;
            this.rbtnActivo.Enabled = false;
            this.rbtnInactivo.Enabled = false;
        }

        /// <summary>
        /// Redirige a la UI de consulta
        /// </summary>
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/ConsultarTareaPendienteUI.aspx"));
        }

        /// <summary>
        /// Obtiene los datos para mostrar en la UI
        /// <returns>Objeto por desplegar en la UI</returns>
        /// </summary>
        public object ObtenerDatosNavegacion()
        {
            return Session["TareaPendienteBO"];
        }

        /// <summary>
        /// Redirige a la UI de detalles
        /// </summary>
        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Mantenimiento.UI/EditarTareaPendienteUI.aspx"));
        }

        /// <summary>
        /// Establece datos por desplegar en la UI
        /// <param name="nombre">Identificador de objeto</param>
        /// <param name="valor">Valor por desplegar</param>
        /// </summary>
        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento para los buscadores
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:                     
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;

                    case ECatalogoBuscador.Modelo:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el boton buscar numero de serie
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnBuscarNumeroSerie_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarNumeroSerie_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el boton buscar numero economico
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnBuscarNumeroEconomico_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarNumeroEconomico_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el boton buscar modelo
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarModelo_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el cambio de texto en el campo numero de serie
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void textNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.NumeroSerie;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                this.NumeroSerie = numeroSerie;
                if (this.NumeroSerie != null)
                {
                    this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
                    this.NumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el número de serie", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".textNumeroSerie_TextChanged" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el cambio de texto en el campo numero economico
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void textNumeroEconomico_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroEconomico = this.NumeroEconomico;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                this.NumeroEconomico = numeroEconomico;
                if (this.NumeroEconomico != null)
                {
                    this.EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
                    this.NumeroEconomico = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el número económico", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".textNumeroEconomico_TextChanged" + ex.Message);
            }
        }

        /// <summary>
        /// Evento para el cambio de texto en el campo modelo
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void textModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string modelo = this.Modelo;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.Modelo = modelo;
                if (this.Modelo != null)
                {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.Modelo = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".textModelo_TextChanged" + ex.Message);
            }
        }

        #endregion
    }
}