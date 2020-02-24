using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.PRE;
using System.Configuration;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class ucConfiguracionParametrosMantenimientoUI : System.Web.UI.UserControl, IucConfiguracionParametrosAuditoriaVIS
    {
        #region Atributos

        private ucConfiguracionParametrosAuditoriaPRE presentador = null;
        private string nombreClase;

        #endregion

        #region Propiedades

        #region propiedades idealise
        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        public int? UsuarioAutenticado
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }
        /// <summary>
        /// identificador de la unidad operativa desde el cual se esta accesando
        /// </summary>
        public int? UnidadOperativaId
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null)
                    id = masterMsj.Adscripcion.UnidadOperativa.Id;
                return id;
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

        #endregion

        #region Propiedades Buscador

        public enum ECatalogoBuscador
        {
            Modelo,
            Sucursal
        }

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

        #region Propiedades Interfaz

        public string Modelo
        {
            get
            {
                string modelo = string.Empty;
                if (txtModelo.Text != string.Empty)
                    modelo = txtModelo.Text;
                return modelo;
            }
            set
            {
                if (value != string.Empty)
                    txtModelo.Text = value;
                else
                    txtModelo.Text = string.Empty;
            }
        }

        public int? ModeloID
        {
            get
            {
                int? modeloId = null;
                if (hdnModeloID.Value != string.Empty)
                    modeloId = int.Parse(hdnModeloID.Value);
                return modeloId;
            }
            set
            {
                if (value != null)
                    hdnModeloID.Value = value.ToString();
                else
                    hdnModeloID.Value = string.Empty;
            }
        }

        public int? SucursalID
        {
            get
            {
                int? sucursalID = null;
                if (!String.IsNullOrEmpty(this.hdnSucursalID.Value))
                    sucursalID = int.Parse(this.hdnSucursalID.Value.Trim());
                return sucursalID;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        public string SucursalNombre
        {
            get
            {
                String sucursalNombre = null;
                if (this.txtSucursal.Text.Trim().Length > 0)
                    sucursalNombre = this.txtSucursal.Text.Trim().ToUpper();
                return sucursalNombre;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value.ToString();
                else
                    this.txtSucursal.Text = String.Empty;
            }
        }

        public int? TallerID
        {
            get
            {
                int? tallerID = null;
                if (!String.IsNullOrEmpty(this.ddTalleres.SelectedValue))
                    tallerID = int.Parse(this.ddTalleres.SelectedValue);
                return tallerID;
            }
        }


        #endregion

        #endregion

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                nombreClase = this.GetType().Name;
                presentador = new ucConfiguracionParametrosAuditoriaPRE(this);

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la pagina", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load" + ex.Message);
            }
        }

        #endregion

        #region Metodos

        #region Accesos y Seguridad

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }

        #endregion

        public void HabilitarSucursal(bool activo)
        {
            txtSucursal.Enabled = activo;
            btnBuscarSucursal.Enabled = activo;
        }

        public void HabilitarModelo(bool activo)
        {
            txtModelo.Enabled = activo;
            btnBuscarModelo.Enabled = activo;
        }

        public void HabilitarTaller(bool activo)
        {
            ddTalleres.Enabled = activo;
        }

        #region Mensaje Sistema

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            Site masterMsj = (Site)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            }
            else
            {
                if (masterMsj != null)
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }

        #endregion

        public void enlazarControles(List<TallerBO> talleres)
        {
            //ListItem todos = new ListItem()
            //{
            //    Text = "TODOS",
            //    Value = "0"
            //};

            List<ListItem> itemsTalleres = new List<ListItem>();

            foreach (var item in talleres)
            {
                ListItem taller = new ListItem()
                {
                    Text = item.Nombre,
                    Value = item.Id.ToString()
                };

                itemsTalleres.Add(taller);
            }

            ddTalleres.Items.Clear();
            itemsTalleres.ForEach(x => ddTalleres.Items.Add(x));
            ddTalleres.Enabled = true;

            //ddTalleres.Items.Add(todos);
            //ddTalleres.SelectedValue = "0";        
        }


        #region Metodos Buscador

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            try
            {
                ViewState_Catalogo = catalogoBusqueda;
                this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
                this.Session_BOSelecto = null;
                this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "');");
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al consultar configuraciones", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

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
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
            this.Session_BOSelecto = null;
        }

        private void DesplegarFormBusquedaModelo()
        {
            try
            {
                this.EjecutaBuscador("Modelo&hidden=0", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el Modelo", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarModelo_Click" + ex.Message);
            }
        }

        private void DesplegarFormBusquedaSucursal()
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar la Sucursal", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }

        #endregion

        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            DesplegarFormBusquedaSucursal();
        }

        #endregion

        protected void btnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            DesplegarFormBusquedaModelo();
        }

        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Modelo:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Modelo != string.Empty)
                {
                    DesplegarFormBusquedaModelo();
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (SucursalNombre != string.Empty)
                {
                    DesplegarFormBusquedaSucursal();
                }

            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnResult_Click" + ex.Message);
            }

        }

    }
}