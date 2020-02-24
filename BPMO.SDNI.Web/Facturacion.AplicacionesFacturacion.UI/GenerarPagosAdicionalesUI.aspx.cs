using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.Facade.SDNI.BOF;
using System.Collections.Generic;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    public partial class GenerarPagosAdicionalesUI : Page, IGenerarPagosAdicionalesVIS
    {
        #region Constantes
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private static readonly string NombreClase = typeof(GenerarPagosAdicionalesUI).Name;
        #endregion
        #region Atributos
        private GenerarPagosAdicionalesPRE presentador;
        #endregion
        #region Propiedades
        /// <summary>
        /// Folio del Contrato
        /// </summary>
        public string FolioContrato 
        {
            get 
            {
                if(!String.IsNullOrEmpty(this.txtNumeroContrato.Text.Trim()) && !String.IsNullOrWhiteSpace(this.txtNumeroContrato.Text.Trim()))
                    return this.txtNumeroContrato.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if(value != null)
                    this.txtNumeroContrato.Text = value;
                else
                    this.txtNumeroContrato.Text = String.Empty;
            }
        }
        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        public int? ContratoID 
        {
            get
            {
                int val = 0;
                if(!string.IsNullOrEmpty(this.hdnContratoID.Value) && !string.IsNullOrWhiteSpace(this.hdnContratoID.Value))
                    if(Int32.TryParse(this.hdnContratoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if(value != null)
                    this.hdnContratoID.Value = value.Value.ToString();
                else
                    this.hdnContratoID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        public int? SucursalID 
        {
            get
            {
                if(this.ddlSucursal.SelectedValue != "-1")
                    return Int32.Parse(this.ddlSucursal.SelectedItem.Value);
                return null;
            }
            set
            {
                if(value == null)
                {
                    this.ddlSucursal.ClearSelection();
                    this.ddlSucursal.SelectedIndex = 0;
                }
                else
                {
                    this.ddlSucursal.SelectedValue = value.ToString();
                }
            }
        }
        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        public string SucursalNombre
        {
            get;
            set;
        }
        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)this.Page.Master;

                if(masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                Site masterMsj = (Site)this.Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null
                        ? masterMsj.Adscripcion.UnidadOperativa.Id.Value : (int?)null;
            }
        }
        /// <summary>
        /// Tipo del Contrato
        /// </summary>
        public ETipoContrato? TipoContrato 
        {
            get
            {
                if(this.ddlDepartamento.SelectedValue != "-1")
                    return (ETipoContrato?)Int32.Parse(this.ddlDepartamento.SelectedItem.Value);
                return null;
            }
            set
            {
                if(value == null)
                {
                    this.ddlDepartamento.ClearSelection();
                    this.ddlDepartamento.SelectedIndex = 0;
                }
                else
                {
                    this.ddlDepartamento.SelectedValue = ((Int32)value).ToString();
                }
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Inicializa las propiedades de la interfaz
        /// </summary>
        public void Inicializar()
        {

        }
        /// <summary>
        /// Carga las sucursales en la UI
        /// </summary>
        /// <param name="coleccion">Conjunto de Elementos que se presentaran en la UI</param>
        public void CargarSucursales(ICollection coleccion)
        {
            var items = (coleccion as List<SucursalBOF>).Select(x => new { Value = x.Id, Text = x.Nombre }).ToList();

            this.ddlSucursal.ClearSelection();
            this.ddlSucursal.Items.Clear();
            this.ddlSucursal.DataSource = items;
            this.ddlSucursal.DataBind();

            this.ddlSucursal.Items.Insert(0,new ListItem("-- Seleccione --", "-1"));
        }
        /// <summary>
        /// Carga los Departamentos en la UI
        /// </summary>
        /// <param name="coleccion">Conjunto de Elementos que se presentaran en la UI</param>
        public void CargarDepartamentos(ICollection coleccion)
        {
            this.ddlDepartamento.ClearSelection();
            this.ddlDepartamento.Items.Clear();
            this.ddlDepartamento.DataSource = coleccion;
            this.ddlDepartamento.DataBind();

            this.ddlDepartamento.Items.Insert(0,new ListItem("-- Seleccione --", "-1"));
        }
        /// <summary>
        /// Limpia elementos de la session
        /// </summary>
        public void LimpiarSesion()
        {
        }
        /// <summary>
        /// Valida los campos requeridos de la interfaz de usuario
        /// </summary>
        public void ValidarCampos()
        {
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
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #endregion
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.presentador = new GenerarPagosAdicionalesPRE(this);

                if(!this.IsPostBack)
                {
                    this.presentador.ValidarAcceso();
                    this.presentador.PrepararInterfaz();
                }
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, NombreClase + "Page_Load: " + ex.GetBaseException().Message);
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.GenerarPagoAdicional();
            }
            catch(Exception ex)
            {
                this.MostrarMensaje("No se pudo crear el Pago Adicional.", ETipoMensajeIU.ERROR, NombreClase + ".btnAceptar_Click: " + ex.GetBaseException().Message);
            }
        }
        #endregion        
    }
}