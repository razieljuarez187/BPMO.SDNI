//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.UI
{
    public partial class ConsultarFlotaUI : System.Web.UI.Page, IConsultarFlotaVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de la página
        /// </summary>
        private ConsultarFlotaPRE presentador = null;

        /// <summary>
        /// Nombre de la clase para usar en los mensajes
        /// </summary>
        private const string nombreClase = "FlotaUI";

        /// <summary>
        /// Enumeración para el control del buscador de bepensa
        /// </summary>
        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal,
            Marca,
            Modelo,
            ModeloEquipoAliado
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el usuario autenticado en el sistema
        /// </summary>
        public UsuarioBO Usuario
        {
            get
            {
                Site masterMsj = (Site) Page.Master;

                return masterMsj != null && masterMsj.Usuario != null ? masterMsj.Usuario : new UsuarioBO();
            }
        }

        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                Site masterMsj = (Site) Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null &&
                       masterMsj.Adscripcion.UnidadOperativa != null
                           ? masterMsj.Adscripcion.UnidadOperativa
                           : new UnidadOperativaBO();
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
                           ? (Int32.TryParse(this.hdnSucursalID.Value, out val) ? (int?) val : null)
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

        /// <summary>
        /// Obtiene o establece el número de serie de la unidad que se desea consultar
        /// </summary>
        public string NumeroSerie
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtNumeroSerie.Text) &&
                       !string.IsNullOrWhiteSpace(this.txtNumeroSerie.Text)
                           ? this.txtNumeroSerie.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtNumeroSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                               ? value.Trim().ToUpper()
                                               : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el número económico de la unidad que se desea consultar
        /// </summary>
        public string NumeroEconomico
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtNumeroEconomico.Text.Trim().ToUpper()) &&
                       !string.IsNullOrWhiteSpace(this.txtNumeroEconomico.Text.Trim().ToUpper())
                           ? this.txtNumeroEconomico.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtNumeroEconomico.Text = string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                   ? value.Trim().ToUpper()
                                                   : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador de la marca de la unidad que se desea consultar
        /// </summary>
        public int? MarcaID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnMarcaID.Value) && !string.IsNullOrWhiteSpace(this.hdnMarcaID.Value)
                           ? (Int32.TryParse(this.hdnMarcaID.Value, out val) ? (int?) val : null)
                           : null;
            }
            set { this.hdnMarcaID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la marca de la unidad seleccionado para el filtro de cosnulta
        /// </summary>
        public string MarcaNombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtMarca.Text) && !string.IsNullOrWhiteSpace(this.txtMarca.Text)
                           ? this.txtMarca.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtMarca.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                         ? value.Trim().ToUpper()
                                         : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el identificador del modelo de la unidad que se desea consultar
        /// </summary>
        public int? ModeloID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnModeloID.Value) &&
                       !string.IsNullOrWhiteSpace(this.hdnModeloID.Value)
                           ? (Int32.TryParse(this.hdnModeloID.Value, out val) ? (int?) val : null)
                           : null;
            }
            set { this.hdnModeloID.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }        

        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad seleccionada para el filtro de consulta
        /// </summary>
        public string ModeloNombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text)
                           ? this.txtModelo.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtModelo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                          ? value.Trim().ToUpper()
                                          : string.Empty;
            }
        }
        
        #region SC0019
        /// <summary>
        /// SC0019
        /// Obtiene o establece el identificador del modelo del equipo aliado de la unidad que se desea consultar
        /// </summary>
        public int? ModeloEAID
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.hdnModeloEAID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloEAID.Value)
                           ? Int32.TryParse(this.hdnModeloEAID.Value, out val) ? (int?)val : null
                           : null;
            }
            set { this.hdnModeloEAID.Value = value.HasValue ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// SC0019
        /// Obtiene o establece el nombre del equipo aliado de la unidad que se desea cosnultar
        /// </summary>
        public string ModeloEANombre
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtModeloEquipoAliado.Text) && !string.IsNullOrWhiteSpace(this.txtModeloEquipoAliado.Text)
                           ? this.txtModeloEquipoAliado.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtModeloEquipoAliado.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                          ? value.Trim().ToUpper()
                                          : string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// Obtiene o establece el año de la unidad que se desea consultar
        /// </summary>
        public int? Anio
        {
            get
            {
                int val;
                return !string.IsNullOrEmpty(this.txtAnio.Text) && !string.IsNullOrWhiteSpace(this.txtAnio.Text)
                           ? (Int32.TryParse(this.txtAnio.Text, out val) ? (int?) val : null)
                           : null;
            }
            set { this.txtAnio.Text = value.HasValue ? value.ToString() : string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece el número de placas de la unidad que se desea consultar
        /// </summary>
        public string Placas
        {
            get
            {
                return !string.IsNullOrEmpty(this.txtNumeroPlaca.Text) &&
                       !string.IsNullOrWhiteSpace(this.txtNumeroPlaca.Text)
                           ? this.txtNumeroPlaca.Text.Trim().ToUpper()
                           : string.Empty;
            }
            set
            {
                this.txtNumeroPlaca.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                               ? value.Trim().ToUpper()
                                               : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece el indice para el control del grid de resultados de la consulta
        /// </summary>
        public int IndicePaginaResultado
        {
            get { return this.grdElementosFlota.PageIndex; }
            set { this.grdElementosFlota.PageIndex = value; }
        }

        /// <summary>
        /// Obtiene o establece los resultados de la consulta
        /// </summary>
        public List<ElementoFlotaBO> Resultado
        {
            get
            {
                return Session["FlotaEncontrada"] != null
                           ? Session["FlotaEncontrada"] as List<ElementoFlotaBO>
                           : new List<ElementoFlotaBO>();
            }
            set { Session["FlotaEncontrada"] = value; }
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
            get { return (ECatalogoBuscador) ViewState["BUSQUEDA"]; }
            set { ViewState["BUSQUEDA"] = value; }
        }

        #endregion

        #endregion

        #region Métodos

        ///<summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            Site masterMsj = (Site) Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        /// <summary>
        /// Remueve la información almacenada en la session para la interfaz de consultar Flota
        /// </summary>
        public void LimpiarSesion()
        {
            Session.Remove("FlotaEncontrada");
            Session.Remove("DetalleFlotaUI");
            Session.Remove("EquiposAliadosUnidad");
        }

        /// <summary>
        /// Limpia el grid de elementos de flota
        /// </summary>
        public void LimpiarFlotaEncontrada()
        {
            this.LimpiarSesion();
            this.CargarElementosFlotaEncontrados(new List<ElementoFlotaBO>());
        }

        /// <summary>
        /// Carga y despliega los elementos de flota que cumplen con los filtros de busqueda en el Grid de Detalles.
        /// </summary>
        /// <param name="elementos">Lista con los elementos de flota que cumplen con los filtros proporcionados para la consulta.</param>
        public void CargarElementosFlotaEncontrados(List<ElementoFlotaBO> elementos)
        {
            List<ElementoFlotaBO> lista = elementos ?? new List<ElementoFlotaBO>();

            grdElementosFlota.DataSource = lista;
            grdElementosFlota.DataBind();
        }

        /// <summary>
        /// Actualiza la información que se  visualiza en el grid de detalles
        /// </summary>
        public void ActualizarResultado()
        {
            this.grdElementosFlota.DataSource = this.Resultado;
            this.grdElementosFlota.DataBind();
        }

        /// <summary>
        /// Establece la información del paquete de navegación para el detalle de la flota
        /// </summary>
        /// <param name="Clave">Clave del paquete</param>
        /// <param name="UnidadID">Identificador de la undia que se desea ver a detalle</param>
        public void EstablecerPaqueteNavegacion(string Clave, int? UnidadID)
        {
            ElementoFlotaBO elementoBO = this.Resultado.Find(p => p.Unidad.UnidadID.HasValue && p.Unidad.UnidadID.Value == UnidadID);
            if (elementoBO != null)
            {
                Session[Clave] = elementoBO;
                #region SC0019
                Dictionary<string,object> paqueteNavegacion = new Dictionary<string, object>();
                paqueteNavegacion.Add("ObjetoFiltro", this.presentador.InterfazUsuarioADatoNavegacion());
                paqueteNavegacion.Add("PagActGrid", this.IndicePaginaResultado);
                paqueteNavegacion.Add("Bandera", true);
                Session["FiltrosFlota"] = paqueteNavegacion;
                #endregion
            }

            else            
                throw new Exception(this.ToString() + ".EstablecerPaqueteNavegacion: La unidad seleccionada no se encuentra dentro del listado de unidades encintradas.");            
        }
        #region SC019
        /// <summary>
        /// SC0019
        /// Obtiene el paquete de navegación donde se ha guardado la consulta original
        /// </summary>
        /// <returns></returns>
        public object ObtenerPaqueteNavegacion()
        {
            if (Session["FiltrosFlota"] != null)
                return Session["FiltrosFlota"] as object;
            return null;
        }
        /// <summary>
        /// SC0019
        /// Remueve de la session el paquete de navegación anterior
        /// </summary>
        public void LimpiarPaqueteNavegacion()
        {
            Session.Remove("ElementosFiltro");
        }
        #endregion
        /// <summary>
        /// Redirige a la pantalla de detalle de unidad
        /// </summary>
        public void IrADetalle()
        {
            Response.Redirect("DetalleFlotaUI.aspx");
        }

        /// <summary>
        /// Redirige a la pantalla de página sin acceso
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        /// <summary>
        /// Habilita o deshabilita el campo del modelo segun sea requerido
        /// </summary>
        /// <param name="estado"></param>
        public void HabilitarModelo(bool estado)
        {
            this.txtModelo.Enabled = estado;
            this.ibtnBuscaModelo.Enabled = estado;
        }

        public void BloquearConsulta(bool estado)
        {
            this.txtAnio.Enabled = estado;
            this.txtMarca.Enabled = estado;
            this.txtModelo.Enabled = estado;
            this.txtNumeroEconomico.Enabled = estado;
            this.txtNumeroPlaca.Enabled = estado;
            this.txtNumeroSerie.Enabled = estado;
            this.txtSucursal.Enabled = estado;
        }        

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
        /// Load de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ConsultarFlotaPRE(this);
                if (!IsPostBack)
                {
                    this.presentador.PrepararBusqueda();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta la busqueda de la flota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarFlota();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al consultar actas de nacimiento", ETipoMensajeIU.ERROR, nombreClase + ".btnBuscar_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar una unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtNumeroSerie.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroSerie.Text))
                {
                    this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
                }
                else
                {
                    this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);                    
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un número de serie", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtNumeroSerie.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }
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
                    if (this.UnidadOperativa.Id.Value != null && this.SucursalNombre != null)
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
        /// <summary>
        /// TextChanged activa el llamado al Buscador para la Marca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMarca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.MarcaID = null;
                string marcaNombre = this.MarcaNombre;
                this.Session_BOSelecto = null;

                if (!string.IsNullOrEmpty(this.txtMarca.Text) && !string.IsNullOrWhiteSpace(this.txtMarca.Text))
                {
                    this.DesplegarBOSelecto(ECatalogoBuscador.Marca);

                    this.MarcaNombre = marcaNombre;
                    if (this.MarcaNombre != null)
                    {
                        this.EjecutaBuscador("Marca", ECatalogoBuscador.Marca);
                        this.MarcaNombre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtMarca_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Marca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaMarca_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Marca&hidden=0", ECatalogoBuscador.Marca);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Marca", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaMarca_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// TextChanged activa el llamado al Buscador para el Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.ModeloID = null;
                int? marcaId = this.MarcaID;
                string modeloNombre = this.ModeloNombre;
                this.Session_BOSelecto = null;

                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text))
                {
                    this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                    this.MarcaID = marcaId;
                    this.ModeloNombre = modeloNombre;
                    if (this.ModeloNombre != null && this.MarcaID != null)
                    {
                        this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                        this.ModeloNombre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtModelo_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// Buscar Modelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaModelo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaModelo_Click:" + ex.Message);
            }
        }
        #region SC0019
        /// <summary>
        /// SC0019
        /// Buscar el modelo del equipo aliado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtModeloEquipoAliado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.ModeloEAID = null;
                string modeloNombre = this.ModeloEANombre;
                this.Session_BOSelecto = null;

                if (!string.IsNullOrEmpty(this.txtModeloEquipoAliado.Text) && !string.IsNullOrWhiteSpace(this.txtModeloEquipoAliado.Text))
                {
                    this.DesplegarBOSelecto(ECatalogoBuscador.ModeloEquipoAliado);

                    this.ModeloEANombre = modeloNombre;
                    if (this.ModeloEANombre != null)
                    {
                        this.EjecutaBuscador("Modelo", ECatalogoBuscador.ModeloEquipoAliado);
                        this.ModeloEANombre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtModeloEquipoAliado_TextChanged:" + ex.Message);
            }
        }
        /// <summary>
        /// SC0019
        /// Buscar el modelo del equipo aliado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscarModeloEQA_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.ModeloEquipoAliado);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscarModeloEQA_Click:" + ex.Message);
            }
        }
        #endregion
        /// <summary>
        /// Cambiar página en el grid de elemntos de flota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdElementosFlota_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.presentador.CambiarPaginaResultado(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, this.ToString() + ".grdElementosFlota_PageIndexChanging:" + ex.Message);
            }
        }       
        /// <summary>
        /// Ejecuta el buscador de bepensa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Marca:
                    case ECatalogoBuscador.Modelo:
                    case ECatalogoBuscador.Sucursal:
                    case ECatalogoBuscador.ModeloEquipoAliado://SC0019
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click" + ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta los comandos del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdElementosFlota_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim().ToUpper())
                {
                    case "DETALLES":
                        int? unidadID = (e.CommandArgument != null) ? (int?)Convert.ToInt32(e.CommandArgument) : null;
                        this.presentador.IrADetalle(unidadID);
                        break;
                    case "PAGE":
                        break;
                    default:
                        {
                            MostrarMensaje("Comando no encontrado", ETipoMensajeIU.ERROR, "El comando no está especificado en el sistema");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al intentar realizar una acción sobre una unidad en la flota", ETipoMensajeIU.ERROR, nombreClase + ".grdElementosFlota_RowCommand" + ex.Message);
            }
        }
        #endregion
    }
}