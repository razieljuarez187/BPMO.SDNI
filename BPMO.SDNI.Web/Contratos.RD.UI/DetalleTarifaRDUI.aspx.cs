﻿// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.RD.PRE;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.RD.UI
{
    public partial class DetalleTarifaRDUI : Page,IDetalleTarifaRDVIS
    {
        #region Atributos

        private string nombreClase = "DetalleTarifaRDUI";
        private DetalleTarifaRDPRE presentador;
        #endregion

        #region Propiedades
        public int? UnidadOperativaID
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null &&
                    master.Adscripcion.UnidadOperativa.Id != null)
                    id = master.Adscripcion.UnidadOperativa.Id;
                return id;
            }
        }
        public int? UsuarioID
        {
            get
            {
                int? id = null;
                Site master = (Site)Page.Master;
                if (master != null && master.Usuario != null && master.Usuario.Id != null)
                    id = master.Usuario.Id;
                return id;
            }
        }
        public int? TarifaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnTarifaID.Value.Trim()))
                    id = int.Parse(hdnTarifaID.Value.Trim());
                return id;
            }
            set { hdnTarifaID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreSucursal
        {
            get
            {
                return (String.IsNullOrEmpty(txtSucursal.Text.Trim().ToUpper()))
                           ? null
                           : txtSucursal.Text.ToUpper();
            }
            set { txtSucursal.Text = value ?? String.Empty; }
        }
        public int? SucursalID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnSucursalID.Value.Trim()))
                    id = int.Parse(hdnSucursalID.Value.Trim());
                return id;
            }
            set { hdnSucursalID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreModelo
        {
            set { txtModelo.Text = value ?? String.Empty; }
        }
        public int? ModeloID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnModeloID.Value.Trim()))
                    id = int.Parse(hdnModeloID.Value.Trim());
                return id;
            }
            set { hdnModeloID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public string NombreMoneda
        {
            set { txtMoneda.Text = value != null ? value.ToUpper() : String.Empty; }
        }
        public string CodigoMoneda
        {
            get
            {
                return (String.IsNullOrEmpty(hdnCodigoMoneda.Value.Trim().ToUpper()))
                           ? null
                           : hdnCodigoMoneda.Value.ToUpper();
            }
            set { hdnCodigoMoneda.Value = value ?? String.Empty; }
        }
        public string NombreTipoTarifa
        {
            set { txtTipo.Text = value ?? String.Empty; }
        }
        public int? TipoTarifa
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnTipo.Value.Trim()))
                    id = int.Parse(hdnTipo.Value.Trim());
                return id;
            }
            set { hdnTipo.Value = value.ToString() ?? String.Empty; }
        }
        public string Descripcion
        {
            get { return (String.IsNullOrEmpty(txtDescripcion.Text.Trim())) ? null : txtDescripcion.Text.ToUpper(); }
            set { txtDescripcion.Text = value ?? String.Empty; }
        }
        public decimal? PrecioCombustible
        {
            set
            {
                TextBox txtPrecioCombustible = mTarifa.Controls[0].FindControl("txtValue") as TextBox;
                if (txtPrecioCombustible != null)
                {
                    txtPrecioCombustible.Text = value != null ? String.Format("{0:#,##0.00##}", value) : string.Empty; //RI0062
                }
            }
        }
        public string NombreCliente
        {
            set { txtCliente.Text = value ?? String.Empty; }
        }
        public int? CuentaClienteID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(hdnClienteID.Value.Trim()))
                    id = int.Parse(hdnClienteID.Value.Trim());
                return id;
            }
            set { hdnClienteID.Value = value != null ? value.ToString() : String.Empty; }
        }
        public DateTime? Vigencia
        {
            set
            {
                txtVigencia.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string Estatus
        {
            set { txtEstatus.Text = value ?? string.Empty; }
        }

        public DateTime? FechaRegistro
        {
            set { txtFechaRegistro.Text = value.ToString() ?? String.Empty; }
        }

        public DateTime? FechaModificacion
        {
            set { txtFechaModificacion.Text = value.ToString() ?? String.Empty; }
        }

        public string UsuarioRegistro
        {
            set { txtUsuarioRegistro.Text = value ?? String.Empty; }
        }

        public string UsuarioModificacion
        {
            set { txtUsuarioModificacion.Text = value ?? String.Empty; }
        }

        public string Observaciones
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtObservaciones.Text.Trim())) ? null : this.txtObservaciones.Text.ToUpper();
            }
            set
            {
                this.txtObservaciones.Text = value ?? String.Empty;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new DetalleTarifaRDPRE(this, ucTarifaRD);
                if (!IsPostBack)
                {
                    presentador.ValidarAcceso();
                    presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al crear la página", ETipoMensajeIU.ERROR,
                                    nombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion

        #region Métodos
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
        public void PermitirEditar(bool activo)
        {
            this.btnEditar.Enabled = activo;
            this.mTarifa.Items[1].Enabled = activo;
        }
        public void PermitirRegistrar(bool activo)
        {
            hlRegistrar.Enabled = activo;
        }
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void ModoDetalle(bool activo)
        {
            txtCliente.Enabled = !activo;
            txtDescripcion.Enabled = !activo;
            txtEstatus.Enabled = !activo;
            txtFechaModificacion.Enabled = !activo;
            txtFechaRegistro.Enabled = !activo;
            txtModelo.Enabled = !activo;
            txtMoneda.Enabled = !activo;
            txtSucursal.Enabled = !activo;
            txtTipo.Enabled = !activo;
            txtUsuarioModificacion.Enabled = !activo;
            txtUsuarioRegistro.Enabled = !activo;
            txtVigencia.Enabled = !activo;
        }
        public void MostrarDatosCliente(bool activo)
        {
            this.pnlDatosCliente.Visible = activo;
        }
        public void LimpiarSesion()
        {
            if(Session["TarifaBO"] != null)
                Session.Remove("TarifaBO");
        }
        public object ObtenerDatosNavegacion()
        {
            return (object) Session["TarifaBO"];
        }
        public void EstablecerDatosNavegacion(object tarifa)
        {
            Session["TarifaBO"] = tarifa;
        }
        public void RedirigirAEditar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/EditarTarifaRDUI.aspx"));
        }        
        #region SC0024
        /// <summary>
        /// SC0024 Retorna a la página de consulta
        /// </summary>
        public void RegresarAConsultar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Contratos.RD.UI/ConsultarTarifasRDUI.aspx"));
        }
        /// <summary>
        /// SC0024
        /// Deshabilita el botón de regreso a consulta con filtro en caso de que el detalle sea llamado desde el registrar
        /// </summary>
        public void PermitirRegresar(bool permiso)
        {
            if (permiso == true)
                this.btnRegresar.Enabled = true;
            else
                this.btnRegresar.Enabled = false;

        }
        /// <summary>
        /// Obtiene los filtros iniciales de consulta en caso de que la llamada sea realizada desde la consulta
        /// </summary>
        /// <returns>Objeto con los filtros iniciales de consulta</returns>
        public object ObtenerFiltrosConsulta()
        {
            if (Session["FiltrosTarifa"] != null)
                return Session["FiltrosTarifa"] as object;
            return null;
        }
        #endregion
        #endregion

        #region Eventos
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                presentador.IrAEditar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar editar la tarifa", ETipoMensajeIU.ERROR, nombreClase + ".btnEditar_Click:" +ex.Message);
            }
        }
        protected void mTarifa_MenuItemClick(object sender, System.Web.UI.WebControls.MenuEventArgs e)
        {
            try
            {
                switch (e.Item.Value)
                {
                    case "Editar":
                        this.presentador.IrAEditar();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar editar la tarifa", ETipoMensajeIU.ERROR, nombreClase + ".mTarifa_MenuItemClick:" + ex.Message);
            }
        }
        #region SC0024
        /// <summary>
        /// SC0024
        /// Redirige a la página de consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.RetrocederPagina();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al cambiar de página", ETipoMensajeIU.ERROR, nombreClase + ".btnRegresar_Click:" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}