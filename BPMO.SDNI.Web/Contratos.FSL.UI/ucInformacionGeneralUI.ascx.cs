// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
// Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing  
// Satisface al caso de uso CU001 - Calcular Monto a Facturar FSL
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucInformacionGeneralUI : UserControl, IucInformacionGeneralVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string NombreClase = "ucInformacionGeneralUI";
        #endregion Atributos

        #region Propiedades

        /// <summary>
        /// Presentador de Informacion General
        /// </summary>
        internal ucInformacionGeneralPRE Presentador { get; set; }

        /// <summary>
        /// Unidad Operaiva del Usuario
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id, Nombre = masterMsj.Adscripcion.UnidadOperativa.Nombre};
                return null;
            }
        }

        /// <summary>
        /// Fecha de Contrato
        /// </summary>
        public DateTime? FechaContrato
        {
            get
            {
                DateTime? fechaContrato = null;

                if (!string.IsNullOrEmpty(txtFechaContrato.Text.Trim()))
                    fechaContrato = DateTime.Parse(txtFechaContrato.Text.Trim());

                return fechaContrato;
            }
            set
            {
                txtFechaContrato.Text = value != null ? value.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o Establece el Listado de Sucursales
        /// </summary>
        public List<SucursalBO> ListadoSucursales
        {
            get
            {
                return (List<SucursalBO>)ddlSucursales.DataSource;
            }
            set
            {
                List<SucursalBO> lista = value != null ? new List<SucursalBO>(value) : new List<SucursalBO>();

                // Agregar el SucursalBO de fachada
                lista.Insert(0, new SucursalBO { Id = 0, Nombre = "Seleccione una opción" });
                //Limpiar el DropDownList Actual
                ddlSucursales.Items.Clear();
                // Asignar Lista al DropDownList
                ddlSucursales.DataTextField = "Nombre";
                ddlSucursales.DataValueField = "Id";
                ddlSucursales.DataSource = lista;
                ddlSucursales.DataBind();
            }
        }

        /// <summary>
        /// Identificador del Modulo
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

        /// <summary>
        /// Obtiene la Sucursal Seleccionada
        /// </summary>
        public SucursalBO SucursalSeleccionada
        {
            get
            {
                SucursalBO sucursalSeleccionada = null;

                if (ddlSucursales.SelectedValue != "0")
                    sucursalSeleccionada = new SucursalBO
                    {
                        Id = int.Parse(ddlSucursales.SelectedValue),
                        Nombre = ddlSucursales.SelectedItem.Text,
                        UnidadOperativa = UnidadOperativa
                    };

                return sucursalSeleccionada;
            }
        }

        /// <summary>
        /// Obtiene o Estable el Listado de Monedas
        /// </summary>
        public List<MonedaBO> ListadoMonedas
        {
            get
            {
                return (List<MonedaBO>)ddlMonedas.DataSource;
            }
            set
            {
                List<MonedaBO> lista = value != null ? new List<MonedaBO>(value) : new List<MonedaBO>();

                // Agregar el SucursalBO de fachada
                lista.Insert(0, new MonedaBO { Codigo = "0", Nombre = "Seleccione una opción" });
                //Limpiar el DropDownList Actual
                ddlMonedas.Items.Clear();
                // Asignar Lista al DropDownList
                ddlMonedas.DataTextField = "Nombre";
                ddlMonedas.DataValueField = "Codigo";
                ddlMonedas.DataSource = lista;
                ddlMonedas.DataBind();
            }
        }

        /// <summary>
        /// Obtiene la Moneda Seleccionada
        /// </summary>
        public MonedaBO MonedaSeleccionada
        {
            get
            {
                MonedaBO monedaSeleccionada = null;

                if (ddlMonedas.SelectedValue != "0")
                    monedaSeleccionada = new MonedaBO { Codigo = ddlMonedas.SelectedValue };

                return monedaSeleccionada;
            }
        }

        /// <summary>
        /// Identificador de la Empresa
        /// </summary>
        public int? EmpresaID
        {
            get
            {
                int? id = null;

                if (!string.IsNullOrEmpty(hdnEmpresaID.Value)) id = int.Parse(hdnEmpresaID.Value);

                return id;
            }
            set
            {
                hdnEmpresaID.Value = value == null ? string.Empty : value.ToString();
            }
        }

        /// <summary>
        /// Nombre de la Empresa
        /// </summary>
        public string NombreEmpresa
        {
            get
            {
                return txtEmpresa.Text.Trim().ToUpper();
            }
            set
            {
                txtEmpresa.Text = (value != null)? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        /// Representante de la Empresa
        /// </summary>
        public string Representante
        {
            get
            {
                return txtRepresentante.Text.Trim().ToUpper();
            }
            set
            {
                txtRepresentante.Text = (value != null)? value.Trim(): string.Empty;
            }
        }

        /// <summary>
        /// Direccion de la Empreesa
        /// </summary>
        public string DomicilioEmpresa
        {
            get
            {
                return txtDireccionEmpresa.Text.Trim().ToUpper();
            }
            set {
                txtDireccionEmpresa.Text = value != null ? value.Trim() : string.Empty;
            }
        }

        /// <summary>
        /// Porcentaje de Penalización en el Contrato
        /// </summary>
        public decimal? PorcentajePenalizacion
        {
            get
            {
                if (hdnPorcentajePenalizacion.Value == null || string.IsNullOrEmpty(hdnPorcentajePenalizacion.Value.Trim()))
                    return null;
                return decimal.Parse(hdnPorcentajePenalizacion.Value.Trim());
            }
            set
            {
                hdnPorcentajePenalizacion.Value = ((value != null) ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Configura la Fachada de la Interfaz de acuerdo al modo Consultar
        /// </summary>
        public void ConfigurarModoConsultar()
        {
            ddlMonedas.Enabled = false;
            ddlSucursales.Enabled = false;
            txtDireccionEmpresa.Enabled = false;
            txtEmpresa.Enabled = false;
            txtFechaContrato.Enabled = false;
            txtRepresentante.Enabled = false;   
        }

        /// <summary>
        /// Configura la Fachada de la Interfaz de acuerdo al modo Editar
        /// </summary>
        public void ConfigurarModoEditar()
        {
            ddlMonedas.Enabled = true;
            ddlSucursales.Enabled = true;
            txtDireccionEmpresa.Enabled = false;
            txtEmpresa.Enabled = false;
            txtFechaContrato.Enabled = true;
            txtRepresentante.Enabled = false;
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

        /// <summary>
        /// Establece la Sucursal Seleccionada
        /// </summary>
        /// <param name="Id"></param>
        public void EstablecerSucursalSeleccionada(int? Id)
        {
            if (Id != null)
                ddlSucursales.SelectedValue = Id.Value.ToString(CultureInfo.InvariantCulture);
            else
                ddlSucursales.SelectedIndex = 0;
        }

        /// <summary>
        /// Establece la Moneda Seleccionada
        /// </summary>
        /// <param name="codigo">Codigo de la Moneda Seleccionada</param>
        public void EstablecerMonedaSeleccionada(string codigo)
        {
            if (codigo != null || codigo.Trim().Length > 0)
                ddlMonedas.SelectedValue = codigo;
            else
                ddlMonedas.SelectedIndex = 0;
        }

        /// <summary>
        /// Usuario de Logueado en el Sistema
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

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presentador = new ucInformacionGeneralPRE(this);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        #endregion
    }
}