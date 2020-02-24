//Satisface al CU068 - Mantenimiento Realizado Contra Programado

using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    /// <summary>
    /// Clase para el control de la vista del reporte
    /// </summary>
    public partial class MantenimientoRealizadoContraProgramadoUI : ReportPage, IMantenimientoRealizadoContraProgramadoVIS
    {

        #region Propiedades
        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private String nombreClase = typeof(MantenimientoRealizadoContraProgramadoUI).Name;
        /// <summary>
        /// Presentador para el reporte
        /// </summary>
        private ReporteMantenimientoRealizadoContraProgramadoPRE presentador = null;

        #region Buscador
        /// <summary>
        /// Catalogo para el buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Unidad
        }
        /// <summary>
        /// Identificador de la vista
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
        /// Objeto seleccionado en sesion
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
        /// Tipo de objeto a buscar
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
        /// Tipo de busqueda
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)ViewState["BUSQUEDA2"];
            }
            set
            {
                ViewState["BUSQUEDA2"] = value;
            }
        }
        /// <summary>
        /// Variable Libro Activos
        /// </summary>
        public string LibroActivos
        {
            get
            {
                string valor = null;
                if (this.hdnLibroActivos.Value.Trim().Length > 0)
                    valor = this.hdnLibroActivos.Value.Trim().ToUpper();
                return valor;

            }
            set
            {
                if (value != null)
                    this.hdnLibroActivos.Value = value.ToString();
                else
                    this.hdnLibroActivos.Value = string.Empty;
            }
        }
        /// <summary>
        /// Devuelve el Vin para busqueda
        /// </summary>
        public string NumeroVIN
        {
            get
            {
                string uiVIN = this.txtVin.Text;
                if (uiVIN.Trim().Length > 0)
                {
                    return uiVIN.ToUpper();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.txtVin.Text = value;
                }
                else
                {
                    this.txtVin.Text = String.Empty;
                }
            }
        }
        /// <summary>
        /// Devuelve la unidad operativa en sesion
        /// </summary>
        public int? UnidadOperativaId
        {
            get
            {
                return this.Master.UnidadOperativaID;
            }
        }
        #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get { return this.Master.SucursalID; }
            set { this.Master.SucursalID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String SucursalNombre
        {
            get { return this.Master.SucursalNombre; }
            set { this.Master.SucursalNombre = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ClienteID
        {
            get { return this.Master.ClienteID; }
            set { this.Master.ClienteID = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ClienteNombre
        {
            get { return this.Master.ClienteNombre; }
            set { this.Master.ClienteNombre = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? Anio
        {
            get
            {
                return this.Master.Anio;
            }
            set
            {
                this.Master.Anio = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesInicio
        {
            get
            {
                return this.Master.Mes;
            }
            set
            {
                this.Master.Mes = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesFin
        {
            get
            {
                if (this.ddlMesFin.SelectedIndex == -1 || this.ddlMesFin.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlMesFin.SelectedValue);
            }
            set
            {
                this.ddlMesFin.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlMesFin.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Vin de la unidad
        /// </summary>
        public String Vin
        {
            get { return this.txtVin.Text; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String IdentificadorReporte
        {
            get { return "PLEN.BEP.15.MODMTTO.CU068"; }
        } 
        #endregion

        #region Constructor
        /// <summary>
        /// Evento que se ejecuta al cargar la vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ReporteMantenimientoRealizadoContraProgramadoPRE(this);
            
            if (!this.IsPostBack)
            {
                this.Master.SucursalFiltroVisible = true;
                this.Master.ClienteFiltroVisible = true;

                this.Master.AnioEtiqueta = "Año";
                this.Master.AnioFiltroVisible = true;
                this.Master.AnioFiltroRequerido = true;

                this.Master.MesEtiqueta = "Mes Inicio";
                this.Master.MesFiltroVisible = true;
                this.Master.MesFiltroRequerido = true;

                this.FillMeses();
                this.presentador.PrepararBusqueda();
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Metodo de consulta para el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                if (this.Anio != null)
                    this.presentador.Consultar();
                else
                    this.MostrarMensaje("Debe Seleccionar un año:",ETipoMensajeIU.ADVERTENCIA);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase +": "+ ex.Message);
            }
        }

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
            this.RegistrarScript("Events", "BtnBuscar2('" + ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Registra una llamada al cliente JS
        /// </summary>
        /// <param name="key"></param>
        /// <param name="script"></param>
        private void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
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
        /// Prepara los campos del buscador
        /// </summary>
        public void PrepararBusqueda()
        {
            this.txtVin.Text = "";
        }

        /// <summary>
        /// Limpia la sesion
        /// </summary>
        public void LimpiarSesion()
        {
            
        }

        /// <summary>
        /// Carga los meses del reporte al control asignado
        /// </summary>
        private void FillMeses()
        {
            this.presentador.BindMeses();
        }

        // <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindMeses(ICollection items)
        {
            this.ddlMesFin.Items.Clear();
            this.ddlMesFin.DataSource = items;
            this.ddlMesFin.DataBind();
            this.ddlMesFin.ClearSelection();
        }
        #endregion

        #region Eventos
        
        /// <summary>
        /// Evento del boton para buscar unidades por vin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, EventArgs e)
        {
            if (txtVin.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("EquipoBepensa&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        /// <summary>
        /// Evento del buscador general
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click2(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion


    }
}