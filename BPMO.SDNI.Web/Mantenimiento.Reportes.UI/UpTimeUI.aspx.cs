//Satisface al CU069 - Reporte de UpTime
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using System.Collections;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    public partial class UpTimeUI : ReportPage, IUpTimeVIS
    {

        #region Constantes
        /// <summary>
        /// Nombre de la clase en curso
        /// </summary>
        private static readonly String NombreClase = typeof(UpTimeUI).Name;

        /// <summary>
        /// Enum catálogo buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal,
            Cliente
        }

        #endregion

        #region Atributos

        UpTimePRE prensentador;

        #endregion

        #region Propiedades

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
        /// Identificador de la Sucursal
        /// </summary>
        public int? SucursalID
        {
            get
            {
                return this.Master.SucursalID;
            }
            set
            {
                this.Master.SucursalID = value;
            }
        }

        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        public string SucursalNombre
        {
            get
            {
                return this.Master.SucursalNombre;
            }
            set
            {
                this.Master.SucursalNombre = value;
            }
        }

        /// <summary>
        /// Identificador de Cliente
        /// </summary>
        public int? ClienteID
        {
            get { return this.Master.ClienteID; }
            set { this.Master.ClienteID = value; }
        }

        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public string ClienteNombre
        {
            get { return this.Master.ClienteNombre; }
            set { this.Master.ClienteNombre = value; }
        }

        /// <summary>
        /// Serie de la unidad
        /// </summary>
        public string VIN
        {
            get
            {
                String numeroVIN = null;
                if (this.txtNumVin.Text.Trim().Length > 0)
                    numeroVIN = this.txtNumVin.Text.Trim().ToUpper();
                return numeroVIN;

            }
            set
            {
                if (value != null)
                    this.txtNumVin.Text = value.ToString();
                else
                    this.txtNumVin.Text = String.Empty;
            }
        }

        /// <summary>
        /// Identificador del Enumerador de Area/Departamento
        /// </summary>
        public int? Area
        {
            get
            {
                if (!this.Master.Departamento.HasValue)
                {
                    return null;
                }
                return (int)this.Master.Departamento;
            }
            set
            {
                if (value == null)
                {
                    this.Master.Departamento = null;
                }
                else
                    this.Master.Departamento = Master.Departamento.Value;
            }
        }

        /// <summary>
        /// Filtro Año
        /// </summary>
        public Int32? Anio {
            get {
                return this.Master.Anio;
            }
            set {
                this.Master.Anio = value;
            }
        }

        /// <summary>
        /// Filtro mes
        /// </summary>
        public Int32? Mes {
            get {
                return this.Master.Mes;
            }
            set {
                this.Master.Mes = value;
            }
        }

        #region Propiedades para el Buscador
        public string ViewState_Guid
        {
            get
            {
                if (ViewState["GuidSession"] == null)
                {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = this.Master.ViewState_Guid.ToString();
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

        #region Constructor

        /// <summary>
        /// Evento iniciado con la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.prensentador = new UpTimePRE(this);
                if (!this.IsPostBack) {
                    this.prensentador.ValidarAcceso();

                    this.Master.SucursalFiltroVisible = true;
                    this.Master.ClienteFiltroVisible = true;
                    this.Master.DepartamentoEtiqueta = "TIPO DE CONTRATO";
                    this.Master.DepartamentoFiltroVisible = true;
                    this.Master.MesEtiqueta = "MES";
                    this.Master.MesFiltroVisible = true;
                    this.Master.MesFiltroRequerido = true;
                    this.Master.AnioEtiqueta = "AÑO";
                    this.Master.AnioFiltroVisible = true;
                    this.Master.AnioFiltroRequerido = true;

                    this.prensentador.PrepararBusqueda();
                }
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, NombreClase + " .Page_Load: " + ex.Message);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método para consultar el reporte
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.prensentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al consultar el Reporte", ETipoMensajeIU.ERROR, NombreClase + ex.Message);
            }
        }

        /// <summary>
        /// Método que inicializa los campos de búsqueda. 
        /// </summary>
        public void PrepararBusqueda()
        {
            this.txtNumVin.Text = "";
        }

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
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
            this.Session_ObjetoBuscador = prensentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscarUnidad('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult2.ClientID + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            prensentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
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
        /// Buscar Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            if (txtNumVin.Text.Length < 1)
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
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, "UpTimeUI.aspx " + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumVin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string serieUnidad = VIN;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                VIN = serieUnidad;
                if (VIN != null)
                {
                    this.EjecutaBuscador("EquipoBepensa", ECatalogoBuscador.Unidad);
                    VIN = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtNumVin_TextChanged" + ex.Message);
            }
        }

        protected void btnResult2_Click(object sender, EventArgs e)
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
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click:" + ex.Message);
            }
        }

        #endregion

    }
}