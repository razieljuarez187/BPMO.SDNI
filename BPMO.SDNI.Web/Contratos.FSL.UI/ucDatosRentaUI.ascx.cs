// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucDatosRentaUI : UserControl, IucDatosRentaVIS
    {
        #region Atributos

        private const string NombreClase = "ucDatosRentaUI";
        public enum ECatalogoBuscador
        {
            UnidadIdealease = 1
        }

        #endregion

        #region Propiedades
        /// <summary>
        /// Presentador de Datos de Renta
        /// </summary>
        internal ucDatosRentaPRE Presentador { get; set; }
        /// <summary>
        /// Manejador de Evento para ver el Detalle de la Linea de Contrato
        /// </summary>
        internal CommandEventHandler VerDetalleLineaContrato { get; set; }
        /// <summary>
        /// Manejador de Evento para remover una linea de contrato
        /// </summary>
        internal EventHandler RemoverLineaContrato { get; set; }
        /// <summary>
        /// Manejador de Evento para cambiar el plazo en meses
        /// </summary>
        internal EventHandler CambiarPlazoMeses { get; set; }
        /// <summary>
        /// Manejador de Evento para agregar una nueva unidad al contrato
        /// </summary>
        internal EventHandler AgregarUnidadClick { get; set; }
        /// <summary>
        /// Identificador de la unidad a Agregar
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int? id = null;

                if (!string.IsNullOrEmpty(hdnUnidadID.Value)) id = int.Parse(hdnUnidadID.Value);

                return id;
            }
            set
            {
                hdnUnidadID.Value = value == null ? string.Empty : value.ToString();
            }
        }
        /// <summary>
        /// Identificador de Equipo de la Unidad a Agregar
        /// </summary>
        public int? EquipoID
        {
            get
            {
                int? id = null;

                if (!string.IsNullOrEmpty(hdnEquipoID.Value)) id = int.Parse(hdnEquipoID.Value);

                return id;
            }
            set
            {
                hdnEquipoID.Value = value == null ? string.Empty : value.ToString();
            }
        }
        /// <summary>
        /// Numero de Serie (VIN) de la Unidad a Agregar
        /// </summary>
        public string NumeroSerie
        {
            get
            {
                return txtNumeroSerie.Text.Trim().ToUpper();
            }
            set
            {
                txtNumeroSerie.Text = value ?? string.Empty;
            }
        }
        /// <summary>
        /// Lineas del Contrato
        /// </summary>
        public List<LineaContratoFSLBO> LineasContrato
        {
            get
            {
                List<LineaContratoFSLBO> listValue;
                if (Session["LineasContratoSession"] != null)
                {
                    listValue = (List<LineaContratoFSLBO>)Session["LineasContratoSession"];
                }
                else
                    listValue = new List<LineaContratoFSLBO>();

                return listValue;
            }
            set
            {
                var listValue = new List<LineaContratoFSLBO>();
                if (value != null)
                {
                    listValue = value;
                }
                Session["LineasContratoSession"] = listValue;


                grdLineasContrato.DataSource = listValue;
                grdLineasContrato.DataBind();
            }
        }
        /// <summary>
        /// Plazo del Contrato en Años
        /// </summary>
        public int PlazoAnios
        {
            get
            {
                int? meses = PlazoMeses;
                double anios = 0;
                if (meses != null)
                {
                    if (meses > 0)
                    {
                        anios = meses.Value / 12d;

                        double aniosFxd = Math.Round(anios, MidpointRounding.AwayFromZero);

                        if (anios - aniosFxd > 0) anios += 0.5d;

                        anios = Math.Round(anios, MidpointRounding.AwayFromZero);
                    }
                }

                return Convert.ToInt32(anios);
            }
            set { txtPlazo.Text = value.ToString(CultureInfo.InvariantCulture); }
        }
        /// <summary>
        /// Plazo del Contrato en Meses
        /// </summary>
        public int? PlazoMeses
        {
            get
            {
                int? plazo = null;

                if (!string.IsNullOrEmpty(txtMeses.Text.Trim()))
                    plazo = int.Parse(txtMeses.Text.Trim());

                return plazo;
            }
            set
            {
                txtMeses.Text = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Ubicacion del Taller de Servicio
        /// </summary>
        public string UbicacionTaller
        {
            get
            {
                return txtUbicacionTaller.Text.Trim().ToUpper();
            }
            set
            {
                txtUbicacionTaller.Text = value;
            }
        }
        /// <summary>
        /// Tipo de Inclusion para el Seguro Seleccionado
        /// </summary>
        public ETipoInclusion? IncluyeSeguroSeleccionado
        {
            get
            {
                ETipoInclusion? seleccionado = null;

                if (ddlIncluyeSeguro.SelectedValue != "-1")
                    seleccionado = (ETipoInclusion)int.Parse(ddlIncluyeSeguro.SelectedValue);

                return seleccionado;
            }
        }
        /// <summary>
        /// Frecuencia de cobro del seguro
        /// </summary>
        public EFrecuenciaSeguro? FrecuenciaSeguro
        {
            get
            {
                if (ddlFrecuenciaCobroSeguro.SelectedIndex == 0) return null;
                EFrecuenciaSeguro? frecuencia;
                frecuencia = (EFrecuenciaSeguro)Convert.ToInt32(ddlFrecuenciaCobroSeguro.SelectedItem.Value);
                return frecuencia;
            }
        }
        /// <summary>
        /// Porcentaje Adicional que se le cobra al seguro
        /// </summary>
        public int? PorcentajeSeguro
        {
            get
            {
                int? porcentajeSeguro = null;
                if(!String.IsNullOrEmpty(txtPorcentajeSeguro.Text.Trim()))
                    porcentajeSeguro = int.Parse(txtPorcentajeSeguro.Text.Trim());
                return porcentajeSeguro;
            }
            set
            {
                this.txtPorcentajeSeguro.Text = value != null ? value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Tipo de Inclusion seleccionado para las llantas
        /// </summary>
        public ETipoInclusion? IncluyeLlantasSeleccionado
        {
            get
            {
                ETipoInclusion? seleccionado = null;

                if (ddlIncluyeLlantas.SelectedValue != "-1")
                    seleccionado = (ETipoInclusion)int.Parse(ddlIncluyeLlantas.SelectedValue);

                return seleccionado;
            }
        }
        /// <summary>
        /// Tipo de Inclusion seleccionado para el Lavado
        /// </summary>
        public ETipoInclusion? IncluyeLavadoSeleccionado
        {
            get
            {
                ETipoInclusion? seleccionado = null;

                if (ddlIncluyeLavado.SelectedValue != "-1")
                    seleccionado = (ETipoInclusion)int.Parse(ddlIncluyeLavado.SelectedValue);

                return seleccionado;
            }
        }
        /// <summary>
        /// Tipo de Inclusion seleccionado para la Pintura
        /// </summary>
        public ETipoInclusion? IncluyePinturaSeleccionado
        {
            get
            {
                ETipoInclusion? seleccionado = null;

                if (ddlIncluyePintura.SelectedValue != "-1")
                    seleccionado = (ETipoInclusion)int.Parse(ddlIncluyePintura.SelectedValue);

                return seleccionado;
            }
        }
        #region Propiedades Buscador
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
                    objeto = (Session[nombreSession]);

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
                    objeto = (Session[ViewState_Guid]);

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

        #region Metodos

        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = Presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", ClientID + "_Buscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            Presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }
        #endregion

        /// <summary>
        /// Habilita o deshabilita el agregar una unidad
        /// </summary>
        /// <param name="habilitar">Indica si habilita o deshabilita el boton</param>
        public void HabilitarAgregarUnidad(bool habilitar)
        {
            btnAgregarUnidad.Enabled = habilitar;
        }
        /// <summary>
        /// Configura la Fachada de acuerdo al modo de vista Consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {           
            const string Css = "textBoxDisabled";
            btnAgregarUnidad.Visible = false;
            ddlIncluyeLavado.Enabled = false;
            ddlIncluyeLlantas.Enabled = false;
            ddlIncluyePintura.Enabled = false;
            ddlIncluyeSeguro.Enabled = false;
            ddlFrecuenciaCobroSeguro.Enabled = false;
            txtPorcentajeSeguro.Enabled = false;
            ibtnEditarMeses.Visible = false;
            ibtnBuscarUnidad.Visible = false;
            txtMeses.ReadOnly = true;
            txtMeses.CssClass = string.Join(" ", new[] { txtMeses.CssClass, Css }); 
            txtNumeroSerie.Visible = false;
            txtPlazo.Enabled = false;
            txtUbicacionTaller.Enabled = false;
            lblVIN.Visible = false;
            lblTitulo.Text = "Unidades en Renta";

            // Boton Eliminar
            grdLineasContrato.Columns[grdLineasContrato.Columns.Count - 2].Visible = false;            
        }
        /// <summary>
        /// Configura la Fachada de acuerdo al modo de vista Editar
        /// </summary>
        public void ConfigurarModoEditar()
        {
            const string Css = "textBoxDisabled";
            btnAgregarUnidad.Visible = true;
            ibtnEditarMeses.Visible = true;
            ibtnBuscarUnidad.Visible = true;
            txtMeses.ReadOnly = false;
            txtMeses.CssClass = txtMeses.CssClass.Replace(Css, string.Empty);
            txtNumeroSerie.Visible = true;
            txtPlazo.Enabled = false;
            txtUbicacionTaller.Enabled = true;

            lblVIN.Visible = true;

            lblTitulo.Text = "Agregar Unidades en Renta";

            // Boton Eliminar
            grdLineasContrato.Columns[grdLineasContrato.Columns.Count - 2].Visible = true;
            //DropDowList de frecuencia de cobro del seguro
            ddlFrecuenciaCobroSeguro.Enabled = IncluyeSeguroSeleccionado != null && IncluyeSeguroSeleccionado == ETipoInclusion.NoIncluidoCargoCliente;
            txtPorcentajeSeguro.Enabled = IncluyeSeguroSeleccionado != null && IncluyeSeguroSeleccionado == ETipoInclusion.NoIncluidoCargoCliente;            
        }
        /// <summary>
        /// Este método despliega un mensaje en pantalla
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
        /// Determina si se puede seleccionar la frecuencia de cobro del seguro
        /// </summary>
        /// <param name="permitir">Bool que determina si se puede seleccionar</param>
        public void PermitirFrecuenciaSeguro(bool permitir)
        {
            ddlFrecuenciaCobroSeguro.Enabled = permitir;
            txtPorcentajeSeguro.Enabled = permitir;
            if (permitir == false)
                PorcentajeSeguro = null;
        }
        /// <summary>
        /// Carga las lista de opciones de Incluye Seguro a partir del listado de tipos inclusion proporcionado
        /// </summary>
        /// <param name="listado">Listado de Tipos de Inclusion</param>
        public void CargarListadoIncluyeSeguro(List<ETipoInclusion> listado)
        {
            List<KeyValuePair<int, string>> lista = CalcularListadoTipoInclusion(listado ?? new List<ETipoInclusion>());


            // Agregar el Item de fachada
            lista.Insert(0, new KeyValuePair<int, string>(-1, "Seleccione una opción"));
            //Limpiar el DropDownList Actual
            ddlIncluyeSeguro.Items.Clear();
            // Asignar Lista al DropDownList
            ddlIncluyeSeguro.DataTextField = "Value";
            ddlIncluyeSeguro.DataValueField = "Key";
            ddlIncluyeSeguro.DataSource = lista;
            ddlIncluyeSeguro.DataBind();
        }
        /// <summary>
        /// Carga la lista de valores de Frecuencias de Cobro del Seguro
        /// </summary>
        /// <param name="listado">Lista con los valores</param>
        public void CargarListadoFrecuenciaSeguro(List<EFrecuenciaSeguro> listado)
        {
            var lista = listado ?? new List<EFrecuenciaSeguro>();
            ddlFrecuenciaCobroSeguro.Items.Clear();
            foreach (var frecuenciaSeguro in lista)
            {
                var item = new ListItem
                {
                    Value = ((int)frecuenciaSeguro).ToString(CultureInfo.InvariantCulture),
                    Text = frecuenciaSeguro.ToString()
                };
                ddlFrecuenciaCobroSeguro.Items.Add(item);
            }
            ddlFrecuenciaCobroSeguro.DataBind();
            ddlFrecuenciaCobroSeguro.Items.Insert(0, new ListItem("SELECCIONE UNA OPCIÓN", "-1"));
        }
        /// <summary>
        /// Carga las lista de opciones de Incluye Llantas a partir del listado de tipos inclusion proporcionado
        /// </summary>
        /// <param name="listado">Listado de Tipos de Inclusion</param>
        public void CargarListadoIncluyeLlantas(List<ETipoInclusion> listado)
        {
            List<KeyValuePair<int, string>> lista = CalcularListadoTipoInclusion(listado ?? new List<ETipoInclusion>());

            // Agregar el Item de fachada
            lista.Insert(0, new KeyValuePair<int, string>(-1, "Seleccione una opción"));
            //Limpiar el DropDownList Actual
            ddlIncluyeLlantas.Items.Clear();
            // Asignar Lista al DropDownList
            ddlIncluyeLlantas.DataTextField = "Value";
            ddlIncluyeLlantas.DataValueField = "Key";
            ddlIncluyeLlantas.DataSource = lista;
            ddlIncluyeLlantas.DataBind();
        }
        /// <summary>
        /// Carga las lista de opciones de Incluye Lavado a partir del listado de tipos inclusion proporcionado
        /// </summary>
        /// <param name="listado">Listado de Tipos de Inclusion</param>
        public void CargarListadoIncluyeLavado(List<ETipoInclusion> listado)
        {
            List<KeyValuePair<int, string>> lista = CalcularListadoTipoInclusion(listado ?? new List<ETipoInclusion>());

            // Agregar el Item de fachada
            lista.Insert(0, new KeyValuePair<int, string>(-1, "Seleccione una opción"));
            //Limpiar el DropDownList Actual
            ddlIncluyeLavado.Items.Clear();
            // Asignar Lista al DropDownList
            ddlIncluyeLavado.DataTextField = "Value";
            ddlIncluyeLavado.DataValueField = "Key";
            ddlIncluyeLavado.DataSource = lista;
            ddlIncluyeLavado.DataBind();
        }
        /// <summary>
        /// Carga las lista de opciones de Incluye Pintura a partir del listado de tipos inclusion proporcionado
        /// </summary>
        /// <param name="listado">Listado de Tipos de Inclusion</param>
        public void CargarListadoIncluyePintura(List<ETipoInclusion> listado)
        {
            List<KeyValuePair<int, string>> lista = CalcularListadoTipoInclusion(listado ?? new List<ETipoInclusion>());

            // Agregar el Item de fachada
            lista.Insert(0, new KeyValuePair<int, string>(-1, "Seleccione una opción"));
            //Limpiar el DropDownList Actual
            ddlIncluyePintura.Items.Clear();
            // Asignar Lista al DropDownList
            ddlIncluyePintura.DataTextField = "Value";
            ddlIncluyePintura.DataValueField = "Key";
            ddlIncluyePintura.DataSource = lista;
            ddlIncluyePintura.DataBind();
        }
        /// <summary>
        /// Calcula la descripcion de los listados de tipo de inclusion
        /// </summary>
        /// <param name="listado">Listado de Tipos de Inclusion</param>
        /// <returns></returns>
        private List<KeyValuePair<int, string>> CalcularListadoTipoInclusion(IEnumerable<ETipoInclusion> listado)
        {
            var Lista = new List<KeyValuePair<int, string>>();

            if (listado != null)
                Lista.AddRange(from tipoInclusion in listado let tipo = ((DescriptionAttribute) tipoInclusion.GetType().GetField(tipoInclusion.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false)[0]).Description select new KeyValuePair<int, string>((int) tipoInclusion, tipo));

            return Lista;
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
        /// Establece el tipo de Inclusion para el Seguro
        /// </summary>
        /// <param name="tipo">Tipo de Inclusion proporcionado</param>
        public void EstablecerIncluyeSeguroSeleccionado(ETipoInclusion? tipo)
        {
            if (tipo != null)
                ddlIncluyeSeguro.SelectedValue = Convert.ToInt32(tipo).ToString(CultureInfo.InvariantCulture);
            else ddlIncluyeSeguro.SelectedIndex = 0;
        }
        /// <summary>
        /// Selecciona el valor configurado de la lista
        /// </summary>
        /// <param name="frecuenciaSeguro">Valor configurado</param>
        public void EstablecerFrecuenciaSeguro(EFrecuenciaSeguro? frecuenciaSeguro)
        {
            if (frecuenciaSeguro == null) ddlFrecuenciaCobroSeguro.SelectedIndex = 0;
            else
                ddlFrecuenciaCobroSeguro.SelectedValue = ((int)frecuenciaSeguro).ToString(CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Establece el tipo de Inclusion para el Lavado
        /// </summary>
        /// <param name="tipo">Tipo de Inclusion proporcionado</param>
        public void EstablecerIncluyeLlantasSeleccionado(ETipoInclusion? tipo)
        {
            if (tipo != null)
                ddlIncluyeLlantas.SelectedValue = Convert.ToInt32(tipo).ToString(CultureInfo.InvariantCulture);
            else ddlIncluyeLlantas.SelectedIndex = 0;
        }
        /// <summary>
        /// Establece el tipo de Inclusion para el Lavado
        /// </summary>
        /// <param name="tipo">Tipo de Inclusion proporcionado</param>
        public void EstablecerIncluyeLavadoSeleccionado(ETipoInclusion? tipo)
        {
            if (tipo != null)
                ddlIncluyeLavado.SelectedValue = Convert.ToInt32(tipo).ToString(CultureInfo.InvariantCulture);
            else ddlIncluyeLavado.SelectedIndex = 0;
        }
        /// <summary>
        /// Establece el tipo de Inclusion para la Pintura
        /// </summary>
        /// <param name="tipo">Tipo de Inclusion proporcionado</param>
        public void EstablecerIncluyePinturaSeleccionado(ETipoInclusion? tipo)
        {
            if (tipo != null)
                ddlIncluyePintura.SelectedValue = Convert.ToInt32(tipo).ToString(CultureInfo.InvariantCulture);
            else ddlIncluyePintura.SelectedIndex = 0;
        }
        /// <summary>
        /// Limpia los datos de la Sesion
        /// </summary>
        public void LimpiarSesion()
        {
            if (Session["LineasContratoSession"] != null)
                Session.Remove("LineasContratoSession");
        }
        #endregion Metodos

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucDatosRentaPRE(this);

                grdLineasContrato.PageSize = int.Parse(ConfigurationManager.AppSettings["NumberRows"]);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void btnAgregarUnidad_Click(object sender, EventArgs e)
        {
            try
            {
                if (AgregarUnidadClick != null) AgregarUnidadClick.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistancias al agregar la Unidad.", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregarUnidad_Click:" + ex.Message);
            }
        }

        protected void ibtnBuscarUnidad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarUnidad_Click: " + ex.Message);
            }
        }

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                NumeroSerie = numeroSerie;
                if (NumeroSerie != null)
                    EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }

        protected void txtMeses_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LineasContrato = null;
                Presentador.CalcularPlazoAnios();
                if(CambiarPlazoMeses != null) CambiarPlazoMeses.Invoke(sender,e);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al calcular el plazo en años.", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtMeses_TextChanged: " + ex.Message);
            }
        }

        protected void grdLineasContrato_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdLineasContrato.DataSource = LineasContrato;
                grdLineasContrato.PageIndex = e.NewPageIndex;
                grdLineasContrato.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, NombreClase + ".grdLineasContrato_PageIndexChanging: " + ex.Message);
            }
        }

        protected void grdLineasContrato_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if(e.CommandArgument == null) return;//RIG0001

                int index = 0;

                if (!Int32.TryParse(e.CommandArgument.ToString(), out index))//TODO:Documentar ri, esto es para evitar pase a tratar de convertir implicitamente y en directo el argumento a entero
                    return;

                LineaContratoFSLBO linea = LineasContrato[index];

                switch (eCommandNameUpper)
                {
                    case "CMDELIMINAR":
                        {
                            Presentador.RemoverLineaContrato(linea);
                            if(RemoverLineaContrato != null) RemoverLineaContrato.Invoke(sender, EventArgs.Empty);
                        }
                        break;
                    case "CMDDETALLES":
                        {
                            if (VerDetalleLineaContrato != null)
                            {
                                var c = new CommandEventArgs("LineaContrato", linea);

                                VerDetalleLineaContrato.Invoke(sender, c);
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al eliminar la unidad del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }

        protected void grdLineasContrato_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var linea = ((LineaContratoFSLBO)e.Row.DataItem);
                    var label = e.Row.FindControl("lblVIN") as Label;

                    var button = e.Row.FindControl("ibtnDetalles") as ImageButton;
                    if (button != null && linea.LineaContratoID == null) button.Visible = false;

                    // Numero de Serie
                    if (label != null)
                    {
                        if (linea.Equipo != null && linea.Equipo.NumeroSerie != null) label.Text = linea.Equipo.NumeroSerie;
                        else label.Text = string.Empty;
                    }

                    // Modelo
                    label = e.Row.FindControl("lblModelo") as Label;
                    if (label != null)
                    {
                        if (linea.Equipo != null && linea.Equipo.Modelo != null) label.Text = linea.Equipo.Modelo.Nombre;
                        else label.Text = string.Empty;
                    }

                    //Km Estimado Anual
                    label = e.Row.FindControl("lblKmEstimadoAnual") as Label;
                    if (label != null)
                    {
                        label.Text = linea.KmEstimadoAnual != null ? string.Format("{0:#,##0}",linea.KmEstimadoAnual) : string.Empty;
                    }

                    //Deposito en Garantia
                    label = e.Row.FindControl("lblDepositoGarantia") as Label;
                    if (label != null)
                    {
                        label.Text = linea.DepositoGarantia != null ? string.Format("{0:c4}", linea.DepositoGarantia.Value) : string.Empty;//RI_0013
                    }

                    //Comision por Apertura
                    label = e.Row.FindControl("lblComisionApertura") as Label;
                    if (label != null)
                    {
                        label.Text = linea.ComisionApertura != null ? string.Format("{0:c4}", linea.ComisionApertura.Value) : string.Empty;//RI_0013
                    }

                    //Cargo Fijo Mensual
                    label = e.Row.FindControl("lblCargoFijoMes") as Label;
                    if (label != null)
                    {
                        label.Text = linea.CargoFijoMensual != null ? string.Format("{0:c4}", linea.CargoFijoMensual.Value) : string.Empty;//RI_0013
                    }

                    //Activo
                    label = e.Row.FindControl("lblActiva") as Label;
                    if(label != null)
                    {
                        label.Text = linea.Activo != null ? linea.Activo.Value ? "SI" : "NO" : string.Empty;
                        label.ForeColor = linea.Activo != null && linea.Activo == false ? Color.Red : label.ForeColor;
                    }

                    //Cargo por KM
                    ImageButton img = e.Row.FindControl("ibtnCargoKM") as ImageButton;
                    if (img != null)
                    {
                        CargosAdicionalesFSLBO cargo = linea.Cobrable as CargosAdicionalesFSLBO;
                        if (cargo != null)
                        {
                            string cargoKm = string.Empty;
                            foreach (TarifaFSLBO tarifa in cargo.Tarifas)
                            {
                                cargoKm +=
                                    "<tr>" +
                                        "<td>" + (!string.IsNullOrEmpty(tarifa.Año.ToString()) ? tarifa.Año.ToString() : "N/A") + "</td>" +
                                        "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoKm) + "</td>" + //RI_0013
                                        "<td> Despues de: " + tarifa.KmLibres.ToString() + "</td>" +
                                        "<td>" + tarifa.Frecuencia + "</td>" +
                                    "</tr>";
                            }
                            cargoKm = "<table class=\"Grid\" width=\"100%\"><tr>" +
                                      "<th>Año</th>" +
                                      "<th colspan=\"3\">Cargo por KM</th>" +
                                      "</tr>" + cargoKm + "</table>";

                            img.OnClientClick = "$('#" + img.ClientID + "').click(function() { $('#" + ClientID + "_divCargos').html(''); $('#" + ClientID + "_divCargos').html('" + cargoKm + "'); $('#" + ClientID + "_divCargos').dialog({ modal:true, autoOpen:true, width: 800, title: 'Cargo por KM' }); }); return false;";
                        }
                    }

                    //Cargo por HR
                    img = e.Row.FindControl("ibtnCargoHR") as ImageButton;
                    if (img != null)
                    {
                        CargosAdicionalesFSLBO cargo = linea.Cobrable as CargosAdicionalesFSLBO;
                        if (cargo != null)
                        {
                            string cargoHr = string.Empty;
                            foreach (TarifaFSLBO tarifa in cargo.Tarifas)
                            {
                                cargoHr +=
                                    "<tr>" +
                                            "<td>" + (!string.IsNullOrEmpty(tarifa.Año.ToString()) ? tarifa.Año.ToString() : "N/A") + "</td>" +
                                            "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoHr) + "</td>" +//RI_0013
                                            "<td> Despues de: " + tarifa.HrLibres.ToString() + "</td>" +
                                            "<td>" + tarifa.Frecuencia + "</td>" +
                                    "</tr>";
                            }
                            cargoHr = "<table class=\"Grid\" width=\"100%\"><tr>" +
                                      "<th>Año</td>" +
                                      "<th colspan=\"3\">Cargo por Hora</th>" +
                                      "</tr>" + cargoHr + "</table>";

                            img.OnClientClick = "$('#" + img.ClientID + "').click(function() { $('#" + ClientID + "_divCargos').html(''); $('#" + ClientID + "_divCargos').html('" + cargoHr + "'); $('#" + ClientID + "_divCargos').dialog({ modal:true, autoOpen:true, width: 800, title: 'Cargo por Hora' }); }); return false;";
                        }
                    }

                    //Cargo por EA
                    img = e.Row.FindControl("ibtnCargoEA") as ImageButton;
                    if (img != null)
                    {
                        string cargos = string.Empty;
                        var cargo = linea.Cobrable as CargosAdicionalesFSLBO;
                        if (cargo != null)
                        {
                            foreach (CargoAdicionalEquipoAliadoBO caea in cargo.CargoAdicionalEquiposAliados)
                            {
                                cargos += (!string.IsNullOrEmpty(cargos))
                                              ? "<tr><td colspan=\"7\"><br/></td></tr>"//TODO: modifque el colspan de 5 a 7
                                              : string.Empty;
                                string cargoHr = string.Empty;
                                if (caea.Tarifas != null)
                                {
                                    foreach (TarifaFSLBO tarifa in caea.Tarifas)
                                    {
                                        cargoHr +=
                                            "<tr>" +
                                            "<td>" +
                                            (!string.IsNullOrEmpty(tarifa.Año.ToString()) ? tarifa.Año.ToString() : "N/A") +
                                            "</td>" +
                                            "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoKm) + "</td>" +//RI_0013
                                            "<td> Despues de: " + tarifa.KmLibres.ToString() + "</td>" +
                                            "<td>" + tarifa.Frecuencia + "</td>" +
                                            "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoHr) + "</td>" +//RI_0013
                                            "<td> Despues de: " + tarifa.HrLibres.ToString() + "</td>" +
                                            "<td>" + tarifa.Frecuencia + "</td>" +
                                            "</tr>";
                                    }
                                    cargoHr = "<tr>" +
                                              "<th>Año</td>" +
                                              "<th colspan=\"3\">Cargo por Km</th>" +
                                              "<th colspan=\"3\">Cargo por Hora</th>" +
                                          "</tr>" + cargoHr;
                                }
                                else
                                {
                                    cargoHr = "<tr>" +
                                              " <td colspan= \"7\">El equipo Aliado no cuenta con Tarifas</td>" +
                                              "</tr>";
                                }

                                cargos += "<tr>" +
                                                "<th colspan=\"7\"># VIN: " + caea.EquipoAliado.NumeroSerie + "</th>" +//TODO: modifque el colspan de 5 a 7
                                           "</tr>" + cargoHr;
                            }
                            cargos = "<table class=\"Grid\" width=\"100%\">" + cargos + "</table>";

                            img.OnClientClick = "$('#" + img.ClientID + "').click(function() { $('#" + ClientID + "_divCargos').html(''); $('#" + ClientID + "_divCargos').html('" + cargos + "'); $('#" + ClientID + "_divCargos').dialog({ modal:true, autoOpen:true, width: 800, title: 'Cargo por Equipos Aliados' }); }); return false;";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Se han encontrado Inconsistencias al presentar el detalle del contrato.",
                               ETipoMensajeIU.ERROR, NombreClase + ".grdLineasContrato_RowDataBound: " + ex.Message);
            }
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:

                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        protected void ddlIncluyeSeguro_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Presentador.CambiarInclusionSeguro(IncluyeSeguroSeleccionado);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Se han encontrado Inconsistencias al habilitar la Frecuencia de Cobro del Seguro", ETipoMensajeIU.ERROR, NombreClase + ".ddlIncluyeSeguro_OnSelectedIndexChanged: " + ex.Message);
            }
        }
        #endregion Eventos
    }
}
