// Satisface al CU092 - Catálogo de Operadores
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Comun.UI
{
    public partial class RegistrarOperadorUI : System.Web.UI.Page, IRegistrarOperadorVIS
    {
        #region Atributos
        private RegistrarOperadorPRE presentador;
        private string nombreClase = "RegistrarOperadorUI";
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

        public int? OperadorID
        {
            get { return this.ucDatosOperadorUI.OperadorID; }
            set { this.ucDatosOperadorUI.OperadorID = value; }
        }
        public int? CuentaClienteID
        {
            get { return this.ucDatosOperadorUI.CuentaClienteID; }
            set { this.ucDatosOperadorUI.CuentaClienteID = value; }
        }
        public string CuentaClienteNombre
        {
            get { return this.ucDatosOperadorUI.CuentaClienteNombre; }
            set { this.ucDatosOperadorUI.CuentaClienteNombre = value; }
        }
        public string Nombre
        {
            get { return this.ucDatosOperadorUI.Nombre; }
            set { this.ucDatosOperadorUI.Nombre = value; }
        }
        public int? AñosExperiencia
        {
            get { return this.ucDatosOperadorUI.AñosExperiencia; }
            set { this.ucDatosOperadorUI.AñosExperiencia = value; }
        }
        public DateTime? FechaNacimiento
        {
            get { return this.ucDatosOperadorUI.FechaNacimiento; }
            set { this.ucDatosOperadorUI.FechaNacimiento = value; }
        }
        public string DireccionCalle
        {
            get { return this.ucDatosOperadorUI.DireccionCalle; }
            set { this.ucDatosOperadorUI.DireccionCalle = value; }
        }
        public string DireccionCiudad
        {
            get { return this.ucDatosOperadorUI.DireccionCiudad; }
            set { this.ucDatosOperadorUI.DireccionCiudad = value; }
        }
        public string DireccionCP
        {
            get { return this.ucDatosOperadorUI.DireccionCP; }
            set { this.ucDatosOperadorUI.DireccionCP = value; }
        }
        public string DireccionEstado
        {
            get { return this.ucDatosOperadorUI.DireccionEstado; }
            set { this.ucDatosOperadorUI.DireccionEstado = value; }
        }
        public int? LicenciaTipoID
        {
            get { return this.ucDatosOperadorUI.LicenciaTipoID; }
            set { this.ucDatosOperadorUI.LicenciaTipoID = value; }
        }
        public string LicenciaNumero
        {
            get { return this.ucDatosOperadorUI.LicenciaNumero; }
            set { this.ucDatosOperadorUI.LicenciaNumero = value; }
        }
        public DateTime? LicenciaFechaExpiracion
        {
            get { return this.ucDatosOperadorUI.LicenciaFechaExpiracion; }
            set { this.ucDatosOperadorUI.LicenciaFechaExpiracion = value; }
        }
        public string LicenciaEstado
        {
            get { return this.ucDatosOperadorUI.LicenciaEstado; }
            set { this.ucDatosOperadorUI.LicenciaEstado = value; }
        }
        public bool? Estatus 
        {
            get { return this.ucDatosOperadorUI.Estatus; }
            set { this.ucDatosOperadorUI.Estatus = value; }
        }

        public DateTime? FC
        {
            get { return DateTime.Now; }
        }
        public DateTime? FUA
        {
            get { return FC; }
        }
        public int? UC
        {
            get { return this.UsuarioID; }
        }
        public int? UUA
        {
            get { return UC; }
        }

        public List<OperadorBO> Operadores
        {
            get
            {
                if (Session["ListaOperadores"] == null)
                    return new List<OperadorBO>();
                else
                    return (List<OperadorBO>)Session["ListaOperadores"];
            }
            set
            {
                Session["ListaOperadores"] = value;
                this.grdOperadores.DataSource = value;
                this.grdOperadores.DataBind();
            }
        }
        #endregion

        #region Constructor
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new RegistrarOperadorPRE(this, this.ucDatosOperadorUI);
                if (!Page.IsPostBack)
                    presentador.PrepararNuevo();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararNuevo()
        {
            this.grdOperadores.DataSource = null;
            this.grdOperadores.DataBind();
        }

        /// <summary>
        /// Establece el Paquete de Navegacion para el Detalle del Contrato Seleccionado
        /// </summary>
        /// <param name="Clave">Clave del Paquete</param>
        /// <param name="value">Valor del paquete</param>
        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede tener una llave nula o vacía.");
            if (value == null)
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: El paquete de navegación no puede ser nulo.");

            Session[key] = value;
        }

        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/ConsultarOperadorUI.aspx"));
        }
        public void RedirigirADetalle()
        {
            this.Response.Redirect(this.ResolveUrl("~/Comun.UI/DetalleOperadorUI.aspx"));
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        
        public void LimpiarSesion()
        {
            if (Session["ListaOperadores"] != null)
                Session.Remove("ListaOperadores");
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
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Registrar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al guardar el operador.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnGuardar_Click:" + ex.Message);
            }

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Cancelar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al cancelar el registro del operador.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnCancelar_Click: " + ex.Message);
            }
        }
        protected void btnAgregarOperador_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.AgregarOperador();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al agregar el operador a la lista", ETipoMensajeIU.ERROR, this.nombreClase + ".btnAgregarOperador_Click: " + ex.Message);
            }
        }

        protected void grdOperadores_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                grdOperadores.DataSource = this.Operadores;
                grdOperadores.PageIndex = e.NewPageIndex;
                grdOperadores.DataBind();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, nombreClase + ".grdOperadores_PageIndexChanging: " + ex.Message);
            }
        }
        protected void grdOperadores_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "PAGE" || e.CommandName.ToUpper() == "SORT") return;
                int index = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName.Trim())
                {
                    case "CMDELIMINAR":
                        this.presentador.QuitarOperador(index);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al realizar la acción sobre el operador", ETipoMensajeIU.ERROR, this.nombreClase + ".grdOperadores_RowCommand: " + ex.Message);
            }
        }
        protected void grdOperadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    OperadorBO bo = (OperadorBO)e.Row.DataItem;

                    Label lblFecha = e.Row.FindControl("lblFechaExpiracion") as Label;
                    if (lblFecha != null)
                    {
                        string fecha = string.Empty;
                        if (bo.Licencia != null && bo.Licencia.FechaExpiracion != null)
                            fecha = String.Format("{0:dd/MM/yyyy}", bo.Licencia.FechaExpiracion);
                        lblFecha.Text = fecha;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información", ETipoMensajeIU.ERROR, this.nombreClase + ".grdOperadores_RowDataBound: " + ex.Message);
            }
        }
        #endregion
    }
}