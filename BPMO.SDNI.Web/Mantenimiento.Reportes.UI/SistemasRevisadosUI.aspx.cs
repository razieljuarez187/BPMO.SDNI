//Satisface al CU066 - Reporte Sistemas Revisados
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.SDNI.Mantenimiento.Reportes.VIS;
using BPMO.SDNI.Mantenimientos.Reportes.PRE;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.Reportes.UI
{
    /// <summary>
    /// Clase para el control de la UI del reporte
    /// </summary>
    public partial class SistemasRevisadosUI : ReportPage, ISistemasRevisadosVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de sistemas revisados
        /// </summary>
        SistemasRevisadosPRE presentador;

        /// <summary>
        /// Nombre de clase
        /// </summary>
        private const string nombreClase = "SistemasRevisadosUI";
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece un identificador de unidad
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
        /// Obtiene o establece la fecha inicio
        /// </summary>
        public DateTime? FechaInicio
        {
            get
            {
                String fecha = null;
                if (this.TextFechaInicio.Text.Trim().Length > 0)
                    fecha = this.TextFechaInicio.Text;
                DateTime date;
                if (!DateTime.TryParse(fecha, out date))
                {
                    return null;
                }
                else
                {
                    return date;
                }
            }
            set
            {
                if (value != null)
                    this.TextFechaInicio.Text = value.ToString();
                else
                    this.TextFechaInicio.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece la fecha fin
        /// </summary>
        public DateTime? FechaFin
        {
            get
            {
                String fecha = null;
                if (this.TextFechaFin.Text.Trim().Length > 0)
                    fecha = this.TextFechaFin.Text;
                DateTime date;
                if (!DateTime.TryParse(fecha, out date))
                {
                    return null;
                }
                else
                {
                    return date;
                }
            }
            set
            {
                if (value != null)
                    this.TextFechaFin.Text = value.ToString();
                else
                    this.TextFechaFin.Text = String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un identificador de sucursal
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
        /// Obtiene o establece el nombre de la sucursal
        /// </summary>
        public string NombreSucursal
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

        public int? ClienteID
        {
            get
            {
                return this.Master.ClienteID;
            }
            set
            {
                this.Master.ClienteID = value;
            }
        }

        public string NombreCliente
        {
            get
            {
                return this.Master.ClienteNombre;
            }
            set
            {
                this.Master.ClienteNombre = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el numero de serie de la unidad
        /// </summary>
        public string NumeroSerie {
            get
            {
                String numeroSerie = null;
                if (this.TextNumeroSerie.Text.Trim().Length > 0)
                    numeroSerie = this.TextNumeroSerie.Text.Trim().ToUpper();
                return numeroSerie;

            }
            set
            {
                if (value != null)
                    this.TextNumeroSerie.Text = value.ToString();
                else
                    this.TextNumeroSerie.Text = String.Empty;
            }
        }
        #endregion

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
            UnidadIdealease
        }
        #endregion

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
            this.RegistrarScript("Events", "BtnBuscarUnidad('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult2.ClientID + "');");
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

        #region Metodos

        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new SistemasRevisadosPRE(this);
            this.Master.SucursalFiltroVisible = true;
            this.Master.ClienteFiltroVisible = true;
            if (!this.IsPostBack) {
                try
                {
                    this.presentador.ValidarAcceso();
                }
                catch (Exception ex) { }
            }
        }

        /// <summary>
        /// Realiza la consulta por medio del presentador
        /// </summary>
        public override void Consultar()
        {
            try
            {
                this.presentador.Consultar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento para los buscadores adicionales
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Argumentos</param>
        /// </summary>
        protected void btnResult2_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:
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
        /// Evento para el buscador de numero de serie
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
        /// Evento para el cambio de texto en campo de numero de serie
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
        #endregion

    }
}