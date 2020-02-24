//Satisface al CU019 - Reporte de Flota Activa de RD Registrados

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Reportes.PRE;
using BPMO.SDNI.Reportes.VIS;
using ReportPage = BPMO.SDNI.Reportes.UI.Page;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Reportes.UI
{
    /// <summary>
    /// Masterpage para la generación de Reportes
    /// </summary>
    public partial class Reportes : System.Web.UI.MasterPage, IReporteVIS
    {
        #region Constantes
        /// <summary>
        /// Clave del Guid asignado la instancia de la página
        /// </summary>
        private const String PAGEGUIDINDEX = "__REPORTGUID";

        /// <summary>
        /// Formato por default aplicado para los controles de fechas
        /// </summary>
        private const String DATETIME_FORMAT = "dd/MM/yyyy";

        /// <summary>
        /// Nombre de la clase de master page en curso
        /// </summary>
        private static readonly String nombreClase = typeof(Reportes).Name;
        #endregion

        #region Atributos
        /// <summary>
        /// Id único global de la instancia del control
        /// </summary>        
        private Guid _GUID;

        /// <summary>
        /// Presentador de para editar registro
        /// </summary>
        private ReportesPRE presentador;
        #endregion

        #region Tipos de Datos
        /// <summary>
        /// Enumerador de Catalogos para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            Sucursal = 0,
            Modelo,
            CuentaClienteIdealease,
            UnidadIdealease,
            Tecnico
        }        

        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                var masterMsj = (Site)this.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return masterMsj.Adscripcion.UnidadOperativa.Id;
                return null;
            }
        }
        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site)this.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id;
                return null;
            }
        }
        /// <summary>
        /// Identificador del Modulo de configuraciones
        /// </summary>
        public int? ModuloID {
            get {
                var masterMsj = (Site)this.Master;

                if (masterMsj != null && masterMsj.ModuloID != null)
                    return masterMsj.ModuloID;
                return null;
            }
        }

        ///<summary>
        ///Obtiene un valor que representa un Id único global de la instancia del control
        ///</summary>
        ///<value>
        ///Objeto GUID con clave única de la instancia
        ///</value>
        internal Guid GUID
        {
            get
            {
                if (this._GUID == Guid.Empty)
                    this.RegisterGuid();

                return this._GUID;
            }
        }

        /// <summary>
        /// Obtiene una referencia hacia la página de reporte
        /// </summary>
        public new ReportPage Page
        {
            get
            {
                return base.Page as ReportPage;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa un identificador único para la UI
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ViewState_Guid
        {
            get
            {
                return this.GUID.ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo modelo
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ModeloEtiqueta
        {
            get
            {
                return this.lblModelo.Text;
            }
            set
            {
                this.lblModelo.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del ´modelo
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ModeloID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnModeloID.Value) && !string.IsNullOrWhiteSpace(this.hdnModeloID.Value))
                    if (Int32.TryParse(this.hdnModeloID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnModeloID.Value = value.Value.ToString();
                else
                    this.hdnModeloID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del modelo para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ModeloNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtModelo.Text) && !string.IsNullOrWhiteSpace(this.txtModelo.Text))
                    return this.txtModelo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtModelo.Text = value;
                else
                    this.txtModelo.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de modelo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool ModeloFiltroVisible 
        {
            get { return this.trModelo.Visible; }
            set { this.trModelo.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si el filtro de modelo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool ModeloFiltroRequerido 
        {
            get { return this.rfvtxtModelo.Visible; }
            set 
            {
                this.lblModeloRequired.Visible = false;
                this.rfvtxtModelo.Visible = false; 
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo Numero Serie
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string UnidadEtiqueta
        {
            get
            {
                return this.lblVIN.Text;
            }
            set
            {
                this.lblVIN.Text = value;
            }
        }

        /// <summary>
        /// Numero de serie
        /// </summary>
        public string NumeroSerie
        {
            get { return string.IsNullOrEmpty(txtNumeroSerie.Text.Trim()) ? null : txtNumeroSerie.Text.Trim(); }
            set { txtNumeroSerie.Text = value ?? string.Empty; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la Unidad
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
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
        /// Obtiene o establece un valor que determina si el filtro de unidad es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool UnidadFiltroVisible
        {
            get { return this.trVIN.Visible; }
            set { this.trVIN.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de unidad es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool UnidadFiltroRequerido
        {
            get { return this.rfvtxtNumeroSerie.Visible; }
            set
            {
                this.lblVINRequired.Visible = value;
                this.rfvtxtNumeroSerie.Visible = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalEtiqueta
        {
            get
            {
                return this.lblSucursal.Text;
            }
            set
            {
                this.lblSucursal.Text = value;
            }
        }        
        
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? SucursalID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnSucursalID.Value) && !string.IsNullOrWhiteSpace(this.hdnSucursalID.Value))
                    if (Int32.TryParse(this.hdnSucursalID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnSucursalID.Value = value.Value.ToString();
                else
                    this.hdnSucursalID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string SucursalNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtSucursal.Text) && !string.IsNullOrWhiteSpace(this.txtSucursal.Text))
                    return this.txtSucursal.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtSucursal.Text = value;
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de sucursal es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool SucursalFiltroVisible 
        {
            get { return this.trSucursal.Visible; }
            set { this.trSucursal.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de sucursal es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool SucursalFiltroRequerido 
        {
            get { return this.rfvtxtSucursal.Visible; }
            set 
            {
                this.lblSucursalRequired.Visible = value;
                this.rfvtxtSucursal.Visible = value; 
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string ClienteEtiqueta
        {
            get
            {
                return this.lblCliente.Text;
            }
            set
            {
                this.lblCliente.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? ClienteID 
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnClienteID.Value))
                    if (Int32.TryParse(this.hdnClienteID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnClienteID.Value = value.Value.ToString();
                else
                    this.hdnClienteID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente de Oracle
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        public int? CuentaClienteID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnCuentaClienteID.Value) && !string.IsNullOrWhiteSpace(this.hdnCuentaClienteID.Value))
                    if (Int32.TryParse(this.hdnCuentaClienteID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnCuentaClienteID.Value = value.Value.ToString();
                else
                    this.hdnCuentaClienteID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public String ClienteNombre 
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtCliente.Text) && !string.IsNullOrWhiteSpace(this.txtCliente.Text))
                    return this.txtCliente.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtCliente.Text = value;
                else
                    this.txtCliente.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de cliente es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool ClienteFiltroVisible
        {
            get { return this.trCliente.Visible; }
            set { this.trCliente.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de cliente es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool ClienteFiltroRequerido
        {
            get { return this.rfvtxtCliente.Visible; }
            set
            {
                this.lblClienteRequired.Visible = value;
                this.rfvtxtCliente.Visible = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo Tecnico
        /// </summary>
        public String TecnicoEtiqueta
        {
            get
            {
                return this.lblTecnico.Text;
            }
            set
            {
                this.lblTecnico.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del Tecnico
        /// </summary>
        public int? TecnicoID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnTecnicoID.Value) && !string.IsNullOrWhiteSpace(this.hdnTecnicoID.Value))
                    if (Int32.TryParse(this.hdnTecnicoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnTecnicoID.Value = value.Value.ToString();
                else
                    this.hdnTecnicoID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del tecnico para las que aplica la configuración
        /// </summary>
        public string TecnicoNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTecnico.Text) && !string.IsNullOrWhiteSpace(this.txtTecnico.Text))
                    return this.txtTecnico.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtTecnico.Text = value;
                else
                    this.txtTecnico.Text = string.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de tecnico es visible
        /// </summary>
        public bool TecnicoFiltroVisible
        {
            get { return this.trTecnico.Visible; }
            set { this.trTecnico.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si el filtro de tecnico es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool TecnicoFiltroRequerido
        {
            get { return this.rfvtxtTecnico.Visible; }
            set
            {
                this.lblTecnicoRequired.Visible = false;
                this.rfvtxtTecnico.Visible = false;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo departamento
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string DepartamentoEtiqueta
        {
            get
            {
                return this.lblDepartamento.Text;
            }
            set
            {
                this.lblDepartamento.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el departamento o área seleccionada
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        public ETipoContrato? Departamento 
        {
            get
            {
                if (this.ddlDepartamento.SelectedIndex == -1 || this.ddlDepartamento.SelectedValue == "-1")
                    return null;

                return (ETipoContrato)Enum.ToObject(typeof(ETipoContrato), Convert.ToInt32(this.ddlDepartamento.SelectedValue));
            }
            set
            {
                if (value == null)
                    this.ddlDepartamento.ClearSelection();
                else
                    this.ddlDepartamento.SelectedValue = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool DepartamentoFiltroVisible 
        {
            get { return this.trAreaDeparmaneto.Visible; }
            set 
            {
                if (this.trAreaDeparmaneto.Visible != value)
                {
                    this.trAreaDeparmaneto.Visible = value;
                    if (this.trAreaDeparmaneto.Visible)
                        this.FillAreasDepartamentos();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool DepartamentoFiltroRequerido 
        {
            get { return this.rfvddlDepartamento.Visible; }
            set 
            {
                if (this.rfvddlDepartamento.Visible != value)
                {
                    this.lblDepartamentoRequired.Visible = value;
                    this.rfvddlDepartamento.Visible = value; 
                    this.FillAreasDepartamentos();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo departamento
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string AreaUnidadEtiqueta
        {
            get
            {
                return this.lblAreaUnidad.Text;
            }
            set
            {
                this.lblAreaUnidad.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el departamento o área seleccionada
        /// </summary>
        /// <value>Valor de tipo EArea</value>
        public EArea? AreaUnidad
        {
            get
            {
                if (this.ddlAreaUnidad.SelectedIndex == -1 || this.ddlAreaUnidad.SelectedValue == "-1")
                    return null;

                return (EArea)Enum.ToObject(typeof(EArea), Convert.ToInt32(this.ddlAreaUnidad.SelectedValue));
            }
            set
            {
                if (value == null)
                    this.ddlAreaUnidad.ClearSelection();
                else
                    this.ddlAreaUnidad.SelectedValue = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool AreaUnidadFiltroVisible
        {
            get { return this.trAreaUnidad.Visible; }
            set
            {
                if (this.trAreaUnidad.Visible != value)
                {
                    this.trAreaUnidad.Visible = value;
                    if (this.trAreaUnidad.Visible)
                        this.FillAreasUnidad();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool AreaUnidadFiltroRequerido
        {
            get { return this.rfvddlAreaUnidad.Visible; }
            set
            {
                if (this.rfvddlAreaUnidad.Visible != value)
                {
                    this.lblAreaUnidadRequired.Visible = value;
                    this.rfvddlAreaUnidad.Visible = value;
                    this.FillAreasUnidad();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo Año
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string AnioEtiqueta
        {
            get
            {
                return this.lblAnio.Text;
            }
            set
            {
                this.lblAnio.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        public int? Anio 
        {
            get
            {
                if (this.ddlAnio.SelectedIndex == -1 || this.ddlAnio.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlAnio.Text);
            }
            set
            {
                this.ddlAnioFechaInicio.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlAnio.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de año es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool AnioFiltroVisible
        {
            get { return this.trAnio.Visible; }
            set 
            {
                if (this.trAnio.Visible != value)
                {
                    this.trAnio.Visible = value;
                    if (this.trAnio.Visible)
                        this.FillAnios();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de año es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool AnioFiltroRequerido
        {
            get 
            { 
                return this.rfvtxtAnio.Visible; 
            }
            set
            {
                if (this.rfvtxtAnio.Visible != value)
                {
                    this.lblAnioRequired.Visible = value;
                    this.rfvtxtAnio.Visible = value;
                    this.FillAnios();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo tipo reporte
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string TipoReporteEtiqueta
        {
            get
            {
                return this.lblTipoReporte.Text;
            }
            set
            {
                this.lblTipoReporte.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo periodo
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string PeriodoEtiqueta
        {
            get
            {
                return this.lblPeriodoReporte.Text;
            }
            set
            {
                this.lblPeriodoReporte.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de periodo que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? TipoReporte 
        {
            get
            {
                if (this.rblsTipoReporte.SelectedIndex == -1 || this.rblsTipoReporte.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.rblsTipoReporte.SelectedValue);
            }
            set
            {
                if (value != null)
                {
                    if (this.rblsTipoReporte.SelectedIndex == -1 || this.rblsTipoReporte.SelectedValue != value.ToString())
                    {
                        ListItem item = this.rblsTipoReporte.Items.FindByValue(value.ToString());
                        if (item != null)
                        {
                            item.Selected = true;
                            this.FillPeriodosReportes();
                        }
                    }
                }
                else
                {
                    if (this.rblsTipoReporte.SelectedIndex != -1)
                    {
                        this.rblsTipoReporte.ClearSelection();
                        this.FillPeriodosReportes();
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el periodo que se aplicará al reporte
        /// </summary>
        public int? PeriodoReporte 
        {
            get
            {
                if (this.ddlPeriodoReporte.SelectedIndex == -1 || this.ddlPeriodoReporte.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlPeriodoReporte.SelectedValue);
            }
            set
            {
                this.ddlPeriodoReporte.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlPeriodoReporte.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool PeriodoReporteFiltroVisible 
        {
            get 
            { 
                return this.trTipoReporte.Visible && this.trPeriodoReporte.Visible; 
            }
            set
            {
                if (this.trTipoReporte.Visible != value)
                {
                    this.trTipoReporte.Visible = value;
                    this.trPeriodoReporte.Visible = value;
                    if (this.trTipoReporte.Visible)
                    {
                        this.FillTiposReportes();
                        this.FillPeriodosReportes();
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool PeriodoReporteFiltroRequerido 
        {
            get
            {
                return this.rfvrblsTipoReporte.Visible && this.rfvrblsTipoReporte.Visible;
            }
            set
            {
                if (this.rfvrblsTipoReporte.Visible != value)
                {
                    this.rfvrblsTipoReporte.Visible = value;
                    this.lblTipoReporteRequired.Visible = value;
                    this.imgBtnTipoReporteClear.Visible = !value;

                    this.rfvrblsTipoReporte.Visible = value;
                    this.lblPeriodoReporteRequired.Visible = value;

                    this.FillTiposReportes();
                    this.FillPeriodosReportes();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo mes
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string MesEtiqueta
        {
            get
            {
                return this.lblMes.Text;
            }
            set
            {
                this.lblMes.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? Mes
        {
            get
            {
                if (this.ddlMes.SelectedIndex == -1 || this.ddlMes.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlMes.SelectedValue);
            }
            set
            {
                this.ddlMes.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlMes.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool MesFiltroVisible
        {
            get { return this.trMes.Visible; }
            set 
            {
                if (this.trMes.Visible != value)
                {
                    this.trMes.Visible = value;
                    if (this.trMes.Visible)
                        this.FillMeses();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool MesFiltroRequerido
        {
            get { return this.rfvtxtMes.Visible; }
            set
            {
                if (this.rfvtxtMes.Visible != value)
                {
                    this.rfvtxtMes.Visible = value;
                    this.lblMesRequired.Visible = value;
                    this.FillMeses();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo día corte
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string DiaCorteEtiqueta
        {
            get
            {
                return this.lblDiaCorte.Text;
            }
            set
            {
                this.lblDiaCorte.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o eatablce u valor que representa el día de corte
        /// </summary>
        /// <value>Objeto de tipo Día</value>
        public int? DiaCorte 
        {
            get
            {
                int value = 0;
                if (!String.IsNullOrEmpty(this.txtDiaCorte.Text) && !String.IsNullOrWhiteSpace(this.txtDiaCorte.Text) && int.TryParse(this.txtDiaCorte.Text.Trim(), out value))
                    return value;

                return null;
            }
            set
            {
                this.txtDiaCorte.Text = value != null ? value.ToString() : String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool DiaCorteFiltroVisible
        {
            get { return this.trDiaCorte.Visible; }
            set { this.trDiaCorte.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool DiaCorteFiltroRequerido
        {
            get { return this.rfvtxtDiaCorte.Visible; }
            set
            {
                this.lblDiaCorteRequired.Visible = value;
                this.rfvtxtDiaCorte.Visible = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo fecha inicio contrato
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string FechaInicioContratoEtiqueta
        {
            get
            {
                return this.lblFechaInicioContrato.Text;
            }
            set
            {
                this.lblFechaInicioContrato.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 1 de inicio de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaInicioContrato1 
        {
            get 
            { 
                DateTime result = DateTime.MinValue;
                if (!String.IsNullOrEmpty(this.txtFechaInicioContrato1.Text) &&
                    !String.IsNullOrWhiteSpace(this.txtFechaInicioContrato1.Text) &&
                    DateTime.TryParseExact(this.txtFechaInicioContrato1.Text, Reportes.DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return null;
            }
            set
            {
                this.txtFechaInicioContrato1.Text = value != null ? value.GetValueOrDefault().ToString(Reportes.DATETIME_FORMAT) : String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 2 de inicio de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaInicioContrato2
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (!String.IsNullOrEmpty(this.txtFechaInicioContrato2.Text) &&
                    !String.IsNullOrWhiteSpace(this.txtFechaInicioContrato2.Text) &&
                    DateTime.TryParseExact(this.txtFechaInicioContrato2.Text, Reportes.DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return null;
            }
            set
            {
                this.txtFechaInicioContrato2.Text = value != null ? value.GetValueOrDefault().ToString(Reportes.DATETIME_FORMAT) : String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio de contrato es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaInicioContratoFiltroVisible 
        {
            get { return this.trFechaInicioContrato.Visible; }
            set
            {
                this.trFechaInicioContrato.Visible = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio de contrato es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaInicioContratoFiltroRequerido 
        {
            get
            {
                return this.rfvtxtFechaInicioContrato1.Visible && this.rfvtxtFechaInicioContrato2.Visible;
            }
            set
            {
                this.lblFechaInicioContratoRequired.Visible = value;
                this.rfvtxtFechaInicioContrato1.Visible = value; 
                this.rfvtxtFechaInicioContrato2.Visible = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo fecha fin contrato
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string FechaFinContratoEtiqueta
        {
            get
            {
                return this.lblFechaFinContrato.Text;
            }
            set
            {
                this.lblFechaFinContrato.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 1 de fin de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaFinContrato1
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (!String.IsNullOrEmpty(this.txtFechaFinContrato1.Text) &&
                    !String.IsNullOrWhiteSpace(this.txtFechaFinContrato1.Text) &&
                    DateTime.TryParseExact(this.txtFechaFinContrato1.Text, Reportes.DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return null;
            }
            set
            {
                this.txtFechaFinContrato1.Text = value != null ? value.GetValueOrDefault().ToString(Reportes.DATETIME_FORMAT) : String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 2 de fin de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        public DateTime? FechaFinContrato2
        {
            get
            {
                DateTime result = DateTime.MinValue;
                if (!String.IsNullOrEmpty(this.txtFechaFinContrato2.Text) &&
                    !String.IsNullOrWhiteSpace(this.txtFechaFinContrato2.Text) &&
                    DateTime.TryParseExact(this.txtFechaFinContrato2.Text, Reportes.DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                    return result;

                return null;
            }
            set
            {
                this.txtFechaFinContrato2.Text = value != null ? value.GetValueOrDefault().ToString(Reportes.DATETIME_FORMAT) : String.Empty;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin de contrato es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaFinContratoFiltroVisible 
        {
            get { return this.trFechaFinContrato.Visible; }
            set { this.trFechaFinContrato.Visible = value; }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin de contrato es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaFinContratoFiltroRequerido
        {
            get 
            {
                return this.rfvtxtFechaFinContrato1.Visible && this.rfvtxtFechaFinContrato2.Visible;
            }
            set
            {
                this.lblFechaFinContratoRequired.Visible = value;
                this.rfvtxtFechaFinContrato1.Visible = value;
                this.rfvtxtFechaFinContrato2.Visible = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo fecha inicio
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string FechaInicioEtiqueta
        {
            get
            {
                return this.lblFechaInicio.Text;
            }
            set
            {
                this.lblFechaInicio.Text = value;
            }
        }        

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? AnioFechaInicio 
        {
            get 
            {
                if (this.ddlAnioFechaInicio.SelectedIndex == -1 || this.ddlAnioFechaInicio.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlAnioFechaInicio.Text); 
            }
            set
            {
                this.ddlAnioFechaInicio.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlAnioFechaInicio.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesFechaInicio 
        {
            get
            {
                if (this.ddlMesFechaInicio.SelectedIndex == -1 || this.ddlMesFechaInicio.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlMesFechaInicio.Text);
            }
            set
            {
                this.ddlMesFechaInicio.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlMesFechaInicio.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaInicioFiltroVisible 
        {
            get { return this.trFechaInicio.Visible; }
            set 
            { 
                if (this.trFechaInicio.Visible != value)
                {
                    this.trFechaInicio.Visible = value;
                    if (this.trFechaInicio.Visible)
                    {
                        this.FillAniosFechaInicio();
                        this.FillMesesFechaInicio();
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaInicioFiltroRequerido 
        {
            get
            {
                return this.rfvddlAnioFechaInicio.Visible && this.rfvddlMesFechaInicio.Visible;
            }
            set
            {
                if (this.rfvddlAnioFechaInicio.Visible != value)
                {
                    this.lblFechaInicioRequired.Visible = value;
                    this.rfvddlAnioFechaInicio.Visible = value;
                    this.rfvddlMesFechaInicio.Visible = value;

                    this.FillAniosFechaInicio();
                    this.FillMesesFechaInicio();
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo fecha fin
        /// </summary>
        /// <value>Objeto de tipo String</value>
        public string FechaFinEtiqueta
        {
            get
            {
                return this.lblFechaFin.Text;
            }
            set
            {
                this.lblFechaFin.Text = value;
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? AnioFechaFin
        {
            get
            {
                if (this.ddlAnioFechaFin.SelectedIndex == -1 || this.ddlAnioFechaFin.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlAnioFechaFin.Text);
            }
            set
            {
                this.ddlAnioFechaInicio.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlAnioFechaFin.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        public int? MesFechaFin
        {
            get
            {
                if (this.ddlMesFechaFin.SelectedIndex == -1 || this.ddlMesFechaFin.SelectedValue == "-1")
                    return null;

                return Convert.ToInt32(this.ddlMesFechaFin.Text);
            }
            set
            {
                this.ddlAnioFechaInicio.ClearSelection();
                if (value != null)
                {
                    ListItem item = this.ddlMesFechaFin.Items.FindByValue(value.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaFinFiltroVisible 
        {
            get { return this.trFechaFin.Visible; }
            set 
            {
                if (this.trFechaFin.Visible != value)
                {
                    this.trFechaFin.Visible = value;
                    if (this.trFechaFin.Visible)
                    {
                        this.FillAniosFechaFin();
                        this.FillMesesFechaFin();
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        public bool FechaFinFiltroRequerido
        {
            get
            {
                return this.rfvddlAnioFechaFin.Visible && this.rfvddlMesFechaFin.Visible;
            }
            set
            {
                if (this.rfvddlAnioFechaFin.Visible != value)
                {
                    this.lblFechaFinRequired.Visible = value;
                    this.rfvddlAnioFechaFin.Visible = value;
                    this.rfvddlMesFechaFin.Visible = value;

                    this.FillAniosFechaFin();
                    this.FillMesesFechaFin();
                }
            }
        }

        /// <summary>
        /// Enumerador que contiene los buscadores existentes en la UI
        /// </summary>
        public ECatalogoBuscador ViewState_Catalogo
        {
            get
            {
                return (ECatalogoBuscador)this.ViewState["BUSQUEDA"];
            }
            set
            {
                this.ViewState["BUSQUEDA"] = value;
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el objeto que fue seleccionado del buscador
        /// </summary>
        /// <value>
        /// Objeto que fue seleccionado de tipo Object
        /// </value>
        protected object Session_BOSelecto
        {
            get
            {
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", this.ViewState_Guid);
                if (this.Session[nombreSession] != null)
                    objeto = (this.Session[nombreSession]);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", this.ViewState_Guid);
                if (value != null)
                    this.Session[nombreSession] = value;
                else
                    this.Session.Remove(nombreSession);
            }
        }

        /// <summary>
        /// Obtiene un valor que representa el objeto que tiene la información de filtrado del buscador
        /// </summary>
        /// <value>
        /// Objeto de filtrado de tipo Object
        /// </value>
        protected object Session_ObjetoBuscador
        {
            get
            {
                object objeto = null;
                if (this.Session[this.ViewState_Guid] != null)
                    objeto = (this.Session[this.ViewState_Guid]);

                return objeto;
            }
            set
            {
                if (value != null)
                    this.Session[this.ViewState_Guid] = value;
                else
                    this.Session.Remove(this.ViewState_Guid);
            }
        }

        /// <summary>
        /// URL del Logotipo de la Unidad Operativa
        /// </summary>
        public string URLLogoEmpresa {
            get {
                string s = this.ResolveUrl("~/Contenido/Imagenes/LogoBepensaMotriz.png");

                if (Session["ConfiguracionModuloSDNI"] != null && Session["ConfiguracionModuloSDNI"] is ConfiguracionModuloBO) {
                    string config = ((ConfiguracionModuloBO)Session["ConfiguracionModuloSDNI"]).URLLogoEmpresa;
                    if (!string.IsNullOrEmpty(config) && !string.IsNullOrWhiteSpace(config))
                        return config;
                }

                return s;
            }
        }
        #endregion

        #region Constructors
        public Reportes()
        {
            this.presentador = new ReportesPRE(this);
        }
        #endregion
        
        #region Métodos
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="sBusqueda">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            this.presentador.DesplegarResultadoBuscador(catalogo.ToString(), this.Session_BOSelecto);
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

        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            this.ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = this.presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript(Guid.NewGuid().ToString(), "BtnBuscar('" + this.ViewState_Guid + "','" + catalogo + "');");
        }

        /// <summary>
        /// Registra la clave única global del control en la página
        /// </summary>
        private void RegisterGuid()
        {
            string hiddenFieldValue = this.Request.Form[Reportes.PAGEGUIDINDEX];
            if (hiddenFieldValue == null)
            {
                this._GUID = Guid.NewGuid();
                hiddenFieldValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._GUID.ToString()));
            }
            else
            {
                string guidValue = Encoding.UTF8.GetString(Convert.FromBase64String(hiddenFieldValue));
                this._GUID = new Guid(guidValue);
            }

            ScriptManager.RegisterHiddenField(this, Reportes.PAGEGUIDINDEX, hiddenFieldValue);
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
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
        /// Carga las áreas departamentos al control asignado
        /// </summary>
        protected virtual void FillAreasDepartamentos()
        {
            this.presentador.BindAreasDepartamentos();            
        }

        /// <summary>
        /// Carga las áreas departamentos al control asignado
        /// </summary>
        protected virtual void FillAreasUnidad()
        {
            this.presentador.BindAreasUnidad();
        }

        /// <summary>
        /// Carga los tipos reportes al control asignado
        /// </summary>
        protected virtual void FillTiposReportes()
        {
            this.presentador.BindTiposReportes();           
        }

        /// <summary>
        /// Carga los periodos de reportes al control asignado
        /// </summary>
        protected virtual void FillPeriodosReportes()
        {
            this.presentador.BindPeriodosReportes();
        }

        /// <summary>
        /// Carga los años del reporte al control asignado
        /// </summary>
        protected virtual void FillAnios()
        {
            this.presentador.BindAnios();
        }

        /// <summary>
        /// Carga los meses del reporte al control asignado
        /// </summary>
        protected virtual void FillMeses()
        {
            this.presentador.BindMeses();            
        }

        /// <summary>
        /// Carga los años de fecha de inicio del reporte al control asignado
        /// </summary>
        protected virtual void FillAniosFechaInicio()
        {
            this.presentador.BindAniosFechaInicio();            
        }

        /// <summary>
        /// Carga los meses de fecha de inicio del reporte al control asignado
        /// </summary>
        protected virtual void FillMesesFechaInicio()
        {
            this.presentador.BindMesesFechaInicio();           
        }

        /// <summary>
        /// Carga los años de fecha de fin del reporte al control asignado
        /// </summary>
        protected virtual void FillAniosFechaFin()
        {
            this.presentador.BindAniosFechaFin();            
        }

        /// <summary>
        /// Carga los meses de fecha de fin del reporte al control asignado
        /// </summary>
        protected virtual void FillMesesFechaFin()
        {
            this.presentador.BindMesesFechaFin();            
        }

        /// <summary>
        /// Liga las áreas departamentos al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindAreasDepartamentos(ICollection items)
        {            
            this.ddlDepartamento.Items.Clear();            
            this.ddlDepartamento.DataSource = items;
            this.ddlDepartamento.DataBind();

            this.ddlDepartamento.ClearSelection();
        }

        /// <summary>
        /// Liga las áreas departamentos al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindAreasUnidad(ICollection items)
        {
            this.ddlAreaUnidad.Items.Clear();
            this.ddlAreaUnidad.DataSource = items;
            this.ddlAreaUnidad.DataBind();

            this.ddlAreaUnidad.ClearSelection();
        }

        /// <summary>
        /// liga los tipos reportes al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindTiposReportes(ICollection items)
        {            
            this.rblsTipoReporte.Items.Clear();            
            this.rblsTipoReporte.DataSource = items;
            this.rblsTipoReporte.DataBind();

            this.rblsTipoReporte.ClearSelection();
            if (this.PeriodoReporteFiltroRequerido)
                this.rblsTipoReporte.SelectedValue = "1";
        }

        /// <summary>
        /// Liga los periodos de reportes al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        /// <param name="isEmpty">Indica si elementos representan una colección vacia</param>
        public void BindPeriodosReportes(ICollection items, bool isEmpty)
        {
            this.ddlPeriodoReporte.Enabled = isEmpty;
            this.ddlPeriodoReporte.Items.Clear();             
            this.ddlPeriodoReporte.DataSource = items;
            this.ddlPeriodoReporte.DataBind();

            this.ddlPeriodoReporte.ClearSelection();
        }

        /// <summary>
        /// Liga los años del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindAnios(ICollection items)
        {            
            this.ddlAnio.Items.Clear();            
            this.ddlAnio.DataSource = items;
            this.ddlAnio.DataBind();

            this.ddlAnio.ClearSelection();
            if (this.AnioFiltroRequerido)
                this.ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindMeses(ICollection items)
        {           
            this.ddlMes.Items.Clear();            
            this.ddlMes.DataSource = items;
            this.ddlMes.DataBind();

            this.ddlMes.ClearSelection();
            if (this.MesFiltroRequerido)
                this.ddlMes.SelectedValue = DateTime.Now.Month.ToString();
        }

        /// <summary>
        /// Liga los años de fecha de inicio del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindAniosFechaInicio(ICollection items)
        {
            this.ddlAnioFechaInicio.Items.Clear();                      
            this.ddlAnioFechaInicio.DataSource = items;
            this.ddlAnioFechaInicio.DataBind();

            this.ddlAnioFechaInicio.ClearSelection();  
            if (this.FechaInicioFiltroRequerido)
                this.ddlAnioFechaInicio.SelectedValue = DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// Liga los meses de fecha de inicio del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindMesesFechaInicio(ICollection items)
        {
            this.ddlMesFechaInicio.Items.Clear();            
            this.ddlMesFechaInicio.DataSource = items;
            this.ddlMesFechaInicio.DataBind();

            this.ddlMesFechaInicio.ClearSelection();
            if (this.FechaInicioFiltroRequerido)
                this.ddlMesFechaInicio.SelectedValue = DateTime.Now.Month.ToString();
        }

        /// <summary>
        /// Liga los años de fecha de fin del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindAniosFechaFin(ICollection items)
        {
            this.ddlAnioFechaFin.Items.Clear();                     
            this.ddlAnioFechaFin.DataSource = items;
            this.ddlAnioFechaFin.DataBind();

            this.ddlAnioFechaFin.ClearSelection();
            if (this.FechaFinFiltroRequerido)
                this.ddlAnioFechaFin.SelectedValue = DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// Liga los meses de fecha de fin del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        public void BindMesesFechaFin(ICollection items)
        {
            this.ddlMesFechaFin.Items.Clear();                       
            this.ddlMesFechaFin.DataSource = items;
            this.ddlMesFechaFin.DataBind();

            this.ddlMesFechaFin.ClearSelection(); 
            if (this.FechaFinFiltroRequerido)
                this.ddlMesFechaFin.SelectedValue = DateTime.Now.Month.ToString();
        }

        /// <summary>
        /// Valida los rangos de fechas de dos campos de texto
        /// </summary>
        /// <param name="input1">Campo de texto 1</param>
        /// <param name="input2">Campo de texto 2</param>
        /// <returns>Devuelve true si las fechas de los campos son válidos</returns>
        private bool ValidarRangoFechas(TextBox input1, TextBox input2)
        {
            DateTime? val1 = null;
            DateTime? val2 = null;
            DateTime val = DateTime.MinValue;

            if (!String.IsNullOrEmpty(input1.Text) && !String.IsNullOrWhiteSpace(input1.Text))
            {
                if (!DateTime.TryParseExact(input1.Text, Reportes.DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out val))                
                    return false;                

                val1 = val;
            }

            if (!String.IsNullOrEmpty(input2.Text) && !String.IsNullOrWhiteSpace(input2.Text))
            {
                if (!DateTime.TryParseExact(input2.Text, Reportes.DATETIME_FORMAT, CultureInfo.CurrentCulture, DateTimeStyles.None, out val))
                    return false;

                val2 = val;
            }

            if (val1.HasValue && val2.HasValue)
                return val1.Value <= val2.Value;

            return true;
        }
        #endregion

        #region Sobreedición de métodos
        /// <summary>
        /// Evento que se ejecuta cuando se inicia la página maestra
        /// </summary>
        /// <param name="e">Argumentos asociados al evento</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!(base.Page is ReportPage))
                throw new Exception(String.Format("La página en curso debe de estar heredando de la clase {0}", typeof(ReportPage)));

            this.presentador.Inicializar();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se carga la página maestra
        /// </summary>
        /// <param name="e">Argumentos asociados al evento</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RegisterGuid();

            if (!this.IsPostBack)
            {
                //this.FillAreasDepartamentos();
                //this.FillTiposReportes();
                //this.FillPeriodosReportes();

                //this.FillAnios();
                //this.FillMeses();

                //this.FillAniosFechaInicio();
                //this.FillMesesFechaInicio();

                //this.FillAniosFechaFin();
                //this.FillMesesFechaFin();
            }
        }
        #endregion

        #region Controladores de Eventos
        /// <summary>
        /// Evento que se ejecuta cuando se selecciona un tipo de reporte
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void rblsTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillPeriodosReportes();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se limipa el tipo de reporte seleccionado
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void imgBtnTipoReporteClear_Click(object sender, ImageClickEventArgs e)
        {
            this.TipoReporte = null;            
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void txtModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreModelo = this.ModeloNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Modelo);

                this.ModeloNombre = nombreModelo;
                if (this.ModeloNombre != null)
                {
                    this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
                    this.ModeloNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtModelo_TextChanged: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = this.SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                this.SucursalNombre = nombreSucursal;
                if (this.SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    this.SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged: " + ex.GetBaseException().Message);
            }
        }

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                NumeroSerie = numeroSerie;
                if (NumeroSerie != null)
                {
                    EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
                    NumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }

        protected void btnBuscarVin_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtNumeroSerie.Text.Length < 1)
            {
                this.MostrarMensaje("Es necesario un número de serie.", ETipoMensajeIU.ADVERTENCIA);
                return;
            }

            try
            {
                this.EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarVin_Click" + ex.Message);
            }
        }

        /// <summary>
        /// TextChanged activa el llamado al Buscador para la busqueda de Cliente
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreCliente = this.ClienteNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.CuentaClienteIdealease);

                this.ClienteNombre = nombreCliente;
                if (this.ClienteNombre != null)
                {
                    this.EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
                    this.ClienteNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtCliente_TextChanged: " + ex.GetBaseException().Message);
            }
        }        

        /// <summary>
        /// Evento que se ejecuta cuando se regresa el resultado de un buscador
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                this.DesplegarBOSelecto(this.ViewState_Catalogo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, Reportes.nombreClase + ".btnResult_Click: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Buscar sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Buscar Modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void btnBuscarModelo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("Modelo", ECatalogoBuscador.Modelo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Modelo", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarModelo_Click: " + ex.GetBaseException().Message);
            }
        }


        /// <summary>
        /// Buscar Cliente
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void btnBuscarClientes_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("CuentaClienteIdealease&&hidden=0", ECatalogoBuscador.CuentaClienteIdealease);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Cliente", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarClientes_Click: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento de consultar
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            this.Page.Consultar();
        }

        /// <summary>
        /// Evento que se ejecuta al validar el rango de las fechas de inicio de contrato
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void ctmvtxtFechaInicioContrato1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.ValidarRangoFechas(this.txtFechaInicioContrato1, this.txtFechaInicioContrato2);            
        }

        /// <summary>
        /// Evento que se ejecuta al validar el rango de las fechas de fin de contrato
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        protected void ctmvtxtFechaFinContrato1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.ValidarRangoFechas(this.txtFechaFinContrato1, this.txtFechaFinContrato2);            
        }

        /// <summary>
        /// Evento de cambio de texto en buscador de tecnicos
        /// </summary>
        protected void txtTecnico_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreTecnico = this.TecnicoNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Tecnico);

                this.TecnicoNombre = nombreTecnico;
                if (!String.IsNullOrEmpty(this.TecnicoNombre))
                {
                    this.EjecutaBuscador("Tecnico&&hidden=0", ECatalogoBuscador.Tecnico);
                    this.TecnicoNombre = String.Empty;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Técnico", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtTecnico_TextChanged: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evento de buscador para el tecnico
        /// </summary>
        protected void btnBuscarTecnico_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.TecnicoNombre) || !String.IsNullOrWhiteSpace(this.TecnicoNombre))
                {
                    this.MostrarMensaje("ES NECESARIO UN NOMBRE DE TÉCNICO", ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                this.EjecutaBuscador("Tecnico&&hidden=0", ECatalogoBuscador.Tecnico);
                this.TecnicoNombre = String.Empty;
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar un Técnico", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarTecnico_Click: " + ex.GetBaseException().Message);
            }
        }
        #endregion                  

        
    }
}