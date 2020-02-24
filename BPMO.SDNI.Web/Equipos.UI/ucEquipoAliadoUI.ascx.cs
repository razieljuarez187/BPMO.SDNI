//Satisface al CU075 - Catálogo de Equipo Aliado
// Satisface a la SC0005
// Satisface a la solicitud de cambio SC0035
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using Newtonsoft.Json;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucEquipoAliadoUI : System.Web.UI.UserControl, IucEquipoAliadoVIS
    {
        #region Atributos
        ucEquipoAliadoPRE presentador;
        private const string nombreClase = "ucEquipoAliadoUI";
        public enum ECatalogoBuscador
        {
            Unidad,
            Sucursal
        }
        #endregion

        #region Propiedades
        public EquipoAliadoBO UltimoObjeto
        {
            get
            {
                if ((EquipoAliadoBO)Session["LastEquipoAliado"] == null)
                    return new EquipoAliadoBO();
                else
                    return (EquipoAliadoBO)Session["LastEquipoAliado"];
            }
            set
            {
                Session["LastEquipoAliado"] = value;
            }
        }

        public int? UnidadOperativaID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnUnidadOperativaID.Value) && !string.IsNullOrWhiteSpace(this.hdnUnidadOperativaID.Value))
                {
                    if (Int32.TryParse(this.hdnUnidadOperativaID.Value, out val))
                        return val;
                    else
                    {
                        Site masterMsj = (Site)Page.Master;

                        if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                            return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    }
                }
                else
                {
                    Site masterMsj = (Site)Page.Master;

                    if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                        return masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                }
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadOperativaID.Value = value.Value.ToString();
                else
                    this.hdnUnidadOperativaID.Value = string.Empty;
            }
        }
        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }
        /// <summary>
        /// Configuración de la unidad operativa que indica a qué libro corresponden los activos
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

        public int? EquipoAliadoID
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnEquipoAliadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoAliadoID.Value))
                    if (Int32.TryParse(this.hdnEquipoAliadoID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoAliadoID.Value = value.Value.ToString();
                else
                    this.hdnEquipoAliadoID.Value = string.Empty;
            }
        }

        public int? EquipoID
        {
            get 
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnEquipoID.Value) && !string.IsNullOrWhiteSpace(this.hdnEquipoID.Value))
                    if (Int32.TryParse(this.hdnEquipoID.Value, out val))
                        return val;
                return null;
            }
            set 
            {
                if (value != null)
                    this.hdnEquipoID.Value = value.Value.ToString();
                else
                    this.hdnEquipoID.Value = string.Empty;
            }
        }

        public string NumeroSerie
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtNumeroSerie.Text) && !string.IsNullOrWhiteSpace(this.txtNumeroSerie.Text))
                    return this.txtNumeroSerie.Text.Trim().ToUpper();

                return null;
            }
            set
            {
                if (value != null)
                    this.txtNumeroSerie.Text = value;
                else
                    this.txtNumeroSerie.Text = string.Empty;
            }
        }

        public string Fabricante
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtFabricante.Text) && !string.IsNullOrWhiteSpace(this.txtFabricante.Text))
                    return this.txtFabricante.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtFabricante.Text = value;
                else
                    this.txtFabricante.Text = string.Empty;
            }
        }

        public string Marca
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtMarca.Text) && !string.IsNullOrWhiteSpace(this.txtMarca.Text))
                    return this.txtMarca.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtMarca.Text = value;
                else
                    this.txtMarca.Text = string.Empty;
            }
        }

        public string Modelo
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

        public int? ModeloID
        {
            get 
            {
                int val;
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

        public string AnioModelo
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtAnioModelo.Text) && !string.IsNullOrWhiteSpace(this.txtAnioModelo.Text))
                    return this.txtAnioModelo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtAnioModelo.Text = value;
                else
                    this.txtAnioModelo.Text = string.Empty;
            }
        }

        public decimal? PBV
        {
            get
            {
                decimal val;
                if (!string.IsNullOrEmpty(this.txtPBV.Text) && !string.IsNullOrWhiteSpace(this.txtPBV.Text))
                    if(Decimal.TryParse(this.txtPBV.Text.Trim().Replace(",",""), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
				if (value != null)
					this.txtPBV.Text = string.Format("{0:#,##0.0000}", value); //RI0012
				else
					this.txtPBV.Text = string.Empty;
            }
        }

        public decimal? PBC
        {
            get
            {
                decimal val;
                if (!string.IsNullOrEmpty(this.txtPBC.Text) && !string.IsNullOrWhiteSpace(this.txtPBC.Text))
                    if (Decimal.TryParse(this.txtPBC.Text, out val)) //RI0012
                        return val;
                
                return null;
            }
            set
            {
                if (value != null)
					this.txtPBC.Text = string.Format("{0:#,##0.0000}", value); //RI0012
                else
                    this.txtPBC.Text = string.Empty;
            }
        }

        public string TipoEquipoNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTipoEquipoNombre.Text) && !string.IsNullOrWhiteSpace(this.txtTipoEquipoNombre.Text))
                    return this.txtTipoEquipoNombre.Text.Trim().ToUpper();
                
                return null;
            }
            set
            {
                if (value != null)
                    this.txtTipoEquipoNombre.Text = value;
                else
                    this.txtTipoEquipoNombre.Text = string.Empty;
            }
        }

        public int? TipoEquipoAliado
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.ddlTipoEquipoAliado.SelectedValue) && !string.IsNullOrWhiteSpace(this.ddlTipoEquipoAliado.SelectedValue))
                    if (Int32.TryParse(this.ddlTipoEquipoAliado.SelectedValue, out val))
                        if (val >= 0)
                            return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.ddlTipoEquipoAliado.SelectedValue = value.Value.ToString();
                else
                    this.ddlTipoEquipoAliado.SelectedValue = "-1";
            }
        }

        public int? TipoEquipoID
        {
            get 
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnTipoEquipoAliadoID.Value) && !string.IsNullOrWhiteSpace(this.hdnTipoEquipoAliadoID.Value))
                    if (Int32.TryParse(this.hdnTipoEquipoAliadoID.Value, out val))
                        return val;
                return null;
            }
            set 
            {
                if (value != null)
                    this.hdnTipoEquipoAliadoID.Value = value.Value.ToString();
                else
                    this.hdnTipoEquipoAliadoID.Value = string.Empty;
            }
        }

        public string Dimension
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtDimenciones.Text) && !string.IsNullOrWhiteSpace(this.txtDimenciones.Text))
                    return this.txtDimenciones.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtDimenciones.Text = value;
                else
                    this.txtDimenciones.Text = string.Empty;
            }
        }

        public string OracleID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnOracleID.Value) && !string.IsNullOrWhiteSpace(this.hdnOracleID.Value))
                    return this.hdnOracleID.Value.Trim().ToUpper();
                return null;                
            }
            set
            {
                if (value != null)
                {
                    this.hdnOracleID.Value = value;
                    this.txtOracleID.Text = value;
                }
                else
                {
                    this.hdnOracleID.Value = string.Empty;
                    this.txtOracleID.Text = string.Empty;
                }
            }
        }
        #region SC0035
        /// <summary>
        /// Indica si es o no un Activo desde Oracle
        /// </summary>
        public bool? ActivoOracle
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtActivoOracle.Text) && !string.IsNullOrWhiteSpace(this.txtActivoOracle.Text))
                {
                    if (this.txtActivoOracle.Text.CompareTo("SI") == 0)
                        return true;
                    else if (this.txtActivoOracle.Text.CompareTo("NO") == 0)
                        return false;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (value.Value)
                        this.txtActivoOracle.Text = "SI";
                    else
                        this.txtActivoOracle.Text = "NO";
                }
                else
                    this.txtActivoOracle.Text = string.Empty;
            }
        }
        #endregion

        public int? EquipoLiderID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnLiderID.Value))
                    id = int.Parse(this.hdnLiderID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnLiderID.Value = value.ToString();
                else
                    this.hdnLiderID.Value = string.Empty;
            }
        }

        public DateTime? FC
        {
            get { return DateTime.Now; }
        }

        public DateTime? FUA
        {
            get { return this.FC; }
        }

        public int? UC
        {
            get
            {
                int? id = null;
                Site masterMsj = (Site)Page.Master;

                if (masterMsj.Usuario != null)
                    id = masterMsj.Usuario.Id;
                return id;
            }
        }

        public int? UUA
        {
            get { return this.UC; }
        }

        /// <summary>
        /// Usuario del Sistema
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
        /// <summary>
        /// Unidad Operativa de Configurada
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                Site masterMsj = (Site)Page.Master;

                if (masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                {
                    this.UnidadOperativaID = masterMsj.Adscripcion.UnidadOperativa.Id.Value;
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id };                    
                }
                return null;
            }
        }

        public int? HorasIniciales
        {
            get 
            {
                int val;
                if (!string.IsNullOrEmpty(this.txtHorasIniciales.Text) && !string.IsNullOrWhiteSpace(this.txtHorasIniciales.Text))
                    if (Int32.TryParse(this.txtHorasIniciales.Text.Trim().Replace(",",""), out val)) //RI0012
                        return val;
                return null;
            }
            set 
            {
                if (value != null)
					this.txtHorasIniciales.Text = string.Format("{0:#,##0}", value); //RI0012
                else
                    this.txtHorasIniciales.Text = string.Empty;
            }
        }

        public int? KilometrosIniciales
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.txtKilometrajeInicial.Text) && !string.IsNullOrWhiteSpace(this.txtKilometrajeInicial.Text))
                    if (Int32.TryParse(this.txtKilometrajeInicial.Text.Trim().Replace(",", ""), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtKilometrajeInicial.Text = string.Format("{0:#,##0}", value); //RI0012
                else
                    this.txtKilometrajeInicial.Text = string.Empty;
            }
        }

        public int? EstatusEquipo
        {
            get 
            {
                int val = 0;
                if (!string.IsNullOrEmpty(this.hdnEstatusEquipoAliado.Value) && !string.IsNullOrWhiteSpace(this.hdnEstatusEquipoAliado.Value))
                    if (Int32.TryParse(this.hdnEstatusEquipoAliado.Value, out val))
                        return val;
                return null;
            }
            set 
            {
                if (value != null)
                    this.hdnEstatusEquipoAliado.Value = value.ToString();
                else
                    this.hdnEstatusEquipoAliado.Value = string.Empty;
            }
        }		
        public bool? Activo
        {
            get
            {
                return this.chkActivo.Checked;
            }
            set
            {
                if (value.HasValue)
                    this.chkActivo.Checked = value.Value;
                else
                    this.chkActivo.Checked = false;
            }
        }
        /// <summary>
        /// Indica la Unidad Operativa "válida" que cuenta con el Permiso "UI EQUIPO ALIADO", su valor default es Idealease.
        /// </summary>
        public ETipoEmpresa EmpresaConPermiso
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnEmpresaConPermiso.Value) && !string.IsNullOrWhiteSpace(this.hdnEmpresaConPermiso.Value))
                    if (Int32.TryParse(this.hdnEmpresaConPermiso.Value, out val)) //RI0012
                        return (ETipoEmpresa)val;
                return ETipoEmpresa.Idealease;
            }
            set
            {
                this.hdnEmpresaConPermiso.Value = ((int)value).ToString();
            }
        }

        /// <summary>
        /// Indica el identificador de la marca de la unidad
        /// </summary>
        public int? MarcaID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnMarcaID.Value) && !string.IsNullOrWhiteSpace(this.hdnMarcaID.Value))
                    if (Int32.TryParse(this.hdnMarcaID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnMarcaID.Value = value.Value.ToString();
                else
                    this.hdnMarcaID.Value = string.Empty;
            }
        }

        #region Propiedades buscador
        //Manejo del buscador en la página principal
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
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucEquipoAliadoPRE(this);
        }
        #endregion

        #region Metodos
        public void LimpiarSesion()
        {
            if (Session["EquipoAliadoBODetalle"] != null)
                this.Session.Remove("EquipoAliadoBODetalle");
            if (Session["EquipoAliadoEditar"] != null)
                this.Session.Remove("EquipoAliadoEditar");
            if (Session["LastEquipoAliado"] != null)
                this.Session.Remove("LastEquipoAliado");
            if (Session["listEquiposAliados"] != null)
                Session.Remove("listEquiposAliados");
        }

        public void PrepararNuevo()
        {
            #region SC0035
            this.txtActivoOracle.Text = string.Empty;
            this.txtOracleID.Text = string.Empty;
            #endregion
            this.txtAnioModelo.Text = string.Empty;
            this.txtDimenciones.Text = string.Empty;
            this.txtFabricante.Text = string.Empty;
            this.txtMarca.Text = string.Empty;
            this.txtModelo.Text = string.Empty;
            this.txtNumeroSerie.Text = string.Empty;
            this.txtPBC.Text = string.Empty;
            this.txtPBV.Text = string.Empty;
            this.txtSucursal.Text = string.Empty;
            this.hdnEquipoAliadoID.Value = string.Empty;
            this.hdnEquipoID.Value = string.Empty;
            this.hdnModeloID.Value = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.hdnUnidadOperativaID.Value = string.Empty;
            this.hdnLiderID.Value = string.Empty;
            this.hdnOracleID.Value = string.Empty;
            this.hdnTipoEquipoAliadoID.Value = string.Empty;
            this.hdnEstatusEquipoAliado.Value = string.Empty;
            
            #region SC001
            this.txtHorasIniciales.Enabled = false;
            this.txtKilometrajeInicial.Enabled = false; 
            #endregion
            
            this.txtTipoEquipoNombre.Text = string.Empty;
            this.txtAnioModelo.Enabled = false;
            this.txtDimenciones.Enabled = true;
            this.txtFabricante.Enabled = false;
            this.txtMarca.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtNumeroSerie.Enabled = true;
            this.txtPBC.Enabled = true;
            this.txtPBV.Enabled = true;
            this.txtSucursal.Enabled = true;
            this.txtTipoEquipoNombre.Enabled = false;			
            this.trActivo.Visible = false;
            this.chkActivo.Checked = true;

            #region SC0001
            this.txtHorasIniciales.Text = string.Empty;
            this.txtKilometrajeInicial.Text = string.Empty; 
            #endregion

            #region SC0035
            this.txtActivoOracle.Enabled = false;
            this.txtOracleID.Enabled = false;
            #endregion

            this.ibtnBuscaEquipo.Enabled = true;
            this.ibtnBuscaEquipo.Visible = true;

            this.ibtnBuscaEquipo.Visible = true;
            this.ibtnBuscaEquipo.Enabled = true;
            this.ibtnBuscarSucursal.Visible = true;
            this.ibtnBuscarSucursal.Visible = true;
        }

        public void PrepararModificacion()
        {
            this.txtAnioModelo.Enabled = false;
            this.txtDimenciones.Enabled = true;
            this.txtFabricante.Enabled = false;
            this.txtMarca.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.ibtnBuscaEquipo.Enabled = false;
            this.ibtnBuscaEquipo.Visible = false;
            this.txtPBC.Enabled = true;
            this.txtPBV.Enabled = true;
            this.txtSucursal.Enabled = false;
            
            this.txtHorasIniciales.Enabled = true;
            this.txtKilometrajeInicial.Enabled = true;
            this.txtTipoEquipoNombre.Enabled = false;			
            this.chkActivo.Enabled = true;			
            #region SC0035
            this.txtActivoOracle.Enabled = false;
            this.txtOracleID.Enabled = false;
            #endregion

            #region SC0001
            if (this.txtHorasIniciales.Text != null)
                this.txtHorasIniciales.Enabled = true;
            if (this.txtKilometrajeInicial.Text != null)
                this.txtKilometrajeInicial.Enabled = true;
            #endregion

            this.ibtnBuscaEquipo.Visible = false;
            this.ibtnBuscaEquipo.Enabled = false;
            this.ibtnBuscarSucursal.Visible = false;
            this.ibtnBuscarSucursal.Visible = false;
        }

        public void PrepararVista()
        {
            this.txtAnioModelo.Enabled = false;
            this.txtDimenciones.Enabled = false;
            this.txtFabricante.Enabled = false;
            this.txtHorasIniciales.Enabled = false;
            this.txtMarca.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.txtPBC.Enabled = false;
            this.txtPBV.Enabled = false;
            this.txtSucursal.Enabled = false;
            this.txtTipoEquipoNombre.Enabled = false;
            #region SC0035
            this.txtActivoOracle.Enabled = false;
            this.txtOracleID.Enabled = false;
            #endregion
        }

        public void AbilitarKMHRS(bool KmHrs)
        {
            if (KmHrs == true)
                this.txtKilometrajeInicial.Enabled = true;
            else if (KmHrs == false) ;
            this.txtHorasIniciales.Enabled = true;

        }

        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
        {
            if (tipo.Equals(ETipoMensajeIU.CONFIRMACION))
            {
                this.hdnTipoMensaje.Value = ((int)tipo).ToString();
                this.hdnMensaje.Value = mensaje;
            }
            else
            {
                Site masterMsj = (Site)Page.Master;
                if (tipo == ETipoMensajeIU.ERROR)
                    masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
                else
                    masterMsj.MostrarMensaje(mensaje, tipo);
            }
        }        
        
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarEquipoAliadoUI.aspx"));
        }

        public object ObtenerDatosNavegacion()
        {
            return Session["EquipoAliadoEditar"];
        }

        public void RedirigirADetalles()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/DetalleEquipoAliadoUI.aspx"));
        }

        public void EstablecerPaqueteNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
        }

        public void CargarTiposEquipoAliado()
        {
            Type type = typeof(ETipoEquipoAliado);
            Array values = System.Enum.GetValues(type);
            ListItem item = new ListItem { Enabled = true, Selected = true, Text = "SELECCIONE UNA OPCIÓN", Value = "-1" };
            this.ddlTipoEquipoAliado.Items.Add(item);

            foreach (int value in values)
            {
                var memInfo = type.GetMember(type.GetEnumName(value));
                var display = memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                if (display != null)
                {
                    ListItem itemenum = new ListItem(display.Description, value.ToString());
                    this.ddlTipoEquipoAliado.Items.Add(itemenum);
                }
            }

            //Se eliminan los items del ddlTipoEquipoAliado dependiendo de la unidad operativa
            switch (this.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Refrigerado).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Seco).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Construccion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Equinova).ToString()));
                    break;
                case (int)ETipoEmpresa.Equinova:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Refrigerado).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Seco).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Generacion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Construccion).ToString()));
                    break;
                case (int)ETipoEmpresa.Construccion:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Refrigerado).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Seco).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Generacion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Equinova).ToString()));
                    break;
                default:
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Generacion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Construccion).ToString()));
                    this.ddlTipoEquipoAliado.Items.Remove(this.ddlTipoEquipoAliado.Items.FindByValue(((int)ETipoEquipoAliado.Equinova).ToString()));
                    break;
            }

            this.ddlTipoEquipoAliado.DataBind();
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
            this.Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnBuscar('" + ViewState_Guid + "','" + catalogo + "', '" + this.btnResult.ClientID + "');");
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

        #region REQ 13596

        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa Generación.
        /// </summary>
        public void EstablecerAcciones()
        {
            //Visibilidad de controles.
            if (this.EmpresaConPermiso == ETipoEmpresa.Generacion || this.EmpresaConPermiso == ETipoEmpresa.Equinova)
            {
                this.spanFabricante.Visible = false;
                this.spanMarca.Visible = false;
                this.spanModelo.Visible = false;
                this.spanPBV.Visible = false;
                this.spanPBC.Visible = false;
                this.rowHorasIniciales.Visible = false;
                this.rowKilometrajeInicial.Visible = false;
            }

            //Cambiando etiqueta de los controles.
            string PBC = ObtenerEtiquetadelResource("EQ01", this.EmpresaConPermiso);
            if (string.IsNullOrEmpty(PBC))
                this.rowPBC.Visible = true;
            else
                this.lblPBC.Text = PBC;

            //Edición de controles
            switch(this.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Construccion:
                case (int)ETipoEmpresa.Equinova:
                   this.ddlTipoEquipoAliado.Enabled = false;
                    break;
                default:
                    break;
            }
 
        }

        /// <summary>
        /// Asigna el valor de tipo de equipo aliado dependiendo de la unidad operativa con la cual se ingreso al sistema.
        /// </summary>
        public void AsignarTipoEquipoAliado()
        {
            switch(this.UnidadOperativaID)
            {
                case (int)ETipoEmpresa.Generacion:
                    this.ddlTipoEquipoAliado.SelectedValue=(((int)ETipoEquipoAliado.Generacion).ToString()); 
                    break;
                case (int)ETipoEmpresa.Construccion:
                    this.ddlTipoEquipoAliado.SelectedValue = (((int)ETipoEquipoAliado.Construccion).ToString());               
                    break;
                case (int)ETipoEmpresa.Equinova:
                    this.ddlTipoEquipoAliado.SelectedValue = (((int)ETipoEquipoAliado.Equinova).ToString());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Método que obtiene el nombre de la etiqueta del archivo resource correspondiente a la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="etiquetaBuscar">Nombre de la etiqueta que será buscada en el archivo resource</param>
        /// <param name="tipoEmpresa">Nombre de la unidad operativa a la cual accedió el usuario.</param>
        /// <returns>Retorna el nombre de la etiqueta correspondiente al valor recibido en el parámetro etiquetaBuscar del archivo resource.</returns>
        private string ObtenerEtiquetadelResource(string etiquetaBuscar, ETipoEmpresa tipoEmpresa)
        {
            string Etiqueta = string.Empty;
            //Instanciamos la clase o webmethod que obtiene las etiquetas
            ObtenerEtiquetaEmpresas obtenerEtiqueta = new ObtenerEtiquetaEmpresas();
            string EtiquetaObtenida = string.Empty;
            EtiquetaObtenida request = null;

            EtiquetaObtenida = obtenerEtiqueta.ObtenerEtiquetadelResource(etiquetaBuscar, (int)tipoEmpresa);
            request = JsonConvert.DeserializeObject<EtiquetaObtenida>(EtiquetaObtenida);
            if (string.IsNullOrEmpty(request.cMensaje))
            {
                EtiquetaObtenida = request.cEtiqueta;
                if (Etiqueta != "NO APLICA")
                {
                    Etiqueta = EtiquetaObtenida;
                }
            }
            return Etiqueta;
        }

        #endregion
        #endregion

        #region Eventos
        protected void btnResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.Unidad:
                        if (this.Session_BOSelecto != null && presentador.VerificarExistenciaEquipo(this.Session_BOSelecto))
                        {
                            this.Session_BOSelecto = null;
                            this.MostrarMensaje("La unidad que seleccionó ya ha sido registrada en el sistema o no se encuentra en Lider", ETipoMensajeIU.ADVERTENCIA, null);
                        }
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.Sucursal:
                        this.DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, nombreClase + ".btnResult_Click:" + ex.Message);
            }
        }

        protected void txtNumeroSerie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = this.NumeroSerie;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Unidad);

                this.NumeroSerie = numeroSerie;
                if (this.NumeroSerie != null)
                {
                    this.EjecutaBuscador("EquipoBepensa", ECatalogoBuscador.Unidad);
                    this.NumeroSerie = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el equipo aliado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        protected void ibtnBuscaEquipo_Click(object sender, ImageClickEventArgs e)
        {
            string s;
            if ((s = this.presentador.ValidarCamposConsultaUnidad()) != null)
            {
                this.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                this.EjecutaBuscador("EquipoBepensa&hidden=0", ECatalogoBuscador.Unidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar el equipo aliado", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".ibtnBuscaEquipo_Click:" + ex.Message);
            }
        }

        protected void txtSucursal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreSucursal = SucursalNombre;
                this.Session_BOSelecto = null;

                this.DesplegarBOSelecto(ECatalogoBuscador.Sucursal);

                SucursalNombre = nombreSucursal;
                if (SucursalNombre != null)
                {
                    this.EjecutaBuscador("SucursalSeguridad", ECatalogoBuscador.Sucursal);
                    SucursalNombre = null;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".txtSucursal_TextChanged" + ex.Message);
            }
        }
        protected void btnBuscarSucursal_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.EjecutaBuscador("SucursalSeguridad&hidden=0", ECatalogoBuscador.Sucursal);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al buscar una Sucursal", ETipoMensajeIU.ADVERTENCIA, nombreClase + ".btnBuscarSucursal_Click" + ex.Message);
            }
        }
        #endregion
       
    }
}