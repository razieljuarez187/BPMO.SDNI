//Satisface al CU081 - Consultar Seguimiento Flota
//Satisface al CU074 - Consultar Expediente de Unidades
//Satisface la solicitud de cambio SC0006

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Flota.VIS;
using BPMO.SDNI.Flota.PRE;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Flota.UI
{
    public partial class DetalleExpedienteUnidadUI : System.Web.UI.Page, IDetalleExpedienteUnidadVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de Detalle del Expediente
        /// </summary>
        private DetalleExpedienteUnidadPRE presentador;

        /// <summary>
        /// Nombre  de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleExpedienteUnidadUI";

        public enum ECatalogoBuscador
        {
            HistorialUnidad
        }

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

        public int? UnidadID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnUnidadID.Value))
                    id = int.Parse(this.hdnUnidadID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnUnidadID.Value = value.ToString();
                else
                    this.hdnUnidadID.Value = string.Empty;
            }
        }
        public int? EquipoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEquipoID.Value))
                    id = int.Parse(this.hdnEquipoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEquipoID.Value = value.ToString();
                else
                    this.hdnEquipoID.Value = string.Empty;
            }
        }
        public int? AreaID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnAreaID.Value))
                    id = int.Parse(this.hdnAreaID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnAreaID.Value = value.ToString();
                else
                    this.hdnAreaID.Value = string.Empty;
            }
        }
        public string AreaNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtAreaNombre.Text)) ? null : this.txtAreaNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtAreaNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? EstatusID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstatusUnidadID.Value))
                    id = int.Parse(this.hdnEstatusUnidadID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstatusUnidadID.Value = value.ToString();
                else
                    this.hdnEstatusUnidadID.Value = string.Empty;
            }
        }
        public string EstatusNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstatusNombre.Text)) ? null : this.txtEstatusNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstatusNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        /// <summary>
        /// Obtiene o establece el número económico de la unidad que será rentada
        /// </summary>
        public string NumeroSerie
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoNumSerie.Text)) ? null : this.txtEstaticoNumSerie.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoNumSerie.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string NumeroEconomico
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoNumEconomico.Text)) ? null : this.txtEstaticoNumEconomico.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoNumEconomico.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string ClaveActivoOracle
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoClaveOracle.Text)) ? null : this.txtEstaticoClaveOracle.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoClaveOracle.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? IDLider
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEstaticoIDLeader.Text) && !string.IsNullOrWhiteSpace(this.txtEstaticoIDLeader.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtEstaticoIDLeader.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtEstaticoIDLeader.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public string SucursalNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtSucursalNombre.Text)) ? null : this.txtSucursalNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtSucursalNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string TipoUnidadNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoTipoUnidad.Text)) ? null : this.txtEstaticoTipoUnidad.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoTipoUnidad.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string ModeloNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtEstaticoModelo.Text)) ? null : this.txtEstaticoModelo.Text.Trim().ToUpper();
            }
            set
            {
                this.txtEstaticoModelo.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? Anio
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEstaticoAnio.Text) && !string.IsNullOrWhiteSpace(this.txtEstaticoAnio.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtEstaticoAnio.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtEstaticoAnio.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public object Llantas
        {
            set
            {
                this.grdLlantas.DataSource = value;
                this.grdLlantas.DataBind();
            }
        }
        public object EquiposAliados
        {
            set
            {
                this.grdEquiposAliados.DataSource = value;
                this.grdEquiposAliados.DataBind();
            }
        }

        public DateTime? FechaCompra
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtEstaticoFechaCompra.Text))
                    temp = DateTime.Parse(this.txtEstaticoFechaCompra.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoFechaCompra.Text = value.Value.ToShortDateString();
                else
                    this.txtEstaticoFechaCompra.Text = string.Empty;
            }
        }
        public decimal? MontoFactura
        {
            get
            {
                decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtEstaticoMontoFactura.Text))
                    temp = decimal.Parse(this.txtEstaticoMontoFactura.Text.Trim().Replace(",", "")); 
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtEstaticoMontoFactura.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtEstaticoMontoFactura.Text = string.Empty;
            }
        }
        public string FolioFactura
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtFolioFacturaCompra.Text)) ? null : this.txtFolioFacturaCompra.Text.Trim().ToUpper();
            }
            set
            {
                this.txtFolioFacturaCompra.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public decimal? ValorLibros
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtValorLibros.Text))
                    temp = Decimal.Parse(this.txtValorLibros.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtValorLibros.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtValorLibros.Text = string.Empty;
            }
        }
        public decimal? ResidualPorcentaje
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtResidualPorcentaje.Text))
                    temp = Decimal.Parse(this.txtResidualPorcentaje.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtResidualPorcentaje.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtResidualPorcentaje.Text = string.Empty;
            }
        }
        public decimal? ResidualMonto
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtResidualMonto.Text))
                    temp = Decimal.Parse(this.txtResidualMonto.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtResidualMonto.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtResidualMonto.Text = string.Empty;
            }
        }
        public decimal? DepreciacionMensualPorcentaje
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtDepreciacionMensualPorcentaje.Text))
                    temp = Decimal.Parse(this.txtDepreciacionMensualPorcentaje.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtDepreciacionMensualPorcentaje.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtDepreciacionMensualPorcentaje.Text = string.Empty;
            }
        }
        public decimal? DepreciacionMensualMonto
        {
            get
            {
                Decimal? temp = null;
                if (!String.IsNullOrEmpty(this.txtDepreciacionMensualMonto.Text))
                    temp = Decimal.Parse(this.txtDepreciacionMensualMonto.Text.Trim().Replace(",", ""));
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtDepreciacionMensualMonto.Text = string.Format("{0:#,##0.00##}", value);
                else
                    this.txtDepreciacionMensualMonto.Text = string.Empty;
            }
        }
        public int? MesesVidaUtilTotal
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtMesesVidaUtilTotal.Text) && !string.IsNullOrWhiteSpace(this.txtMesesVidaUtilTotal.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtMesesVidaUtilTotal.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtMesesVidaUtilTotal.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public int? MesesVidaUtilRestante
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtMesesVidaUtilRestante.Text) && !string.IsNullOrWhiteSpace(this.txtMesesVidaUtilRestante.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtMesesVidaUtilRestante.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtMesesVidaUtilRestante.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }
        public DateTime? FechaSustitucion
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaSustitucion.Text))
                    temp = DateTime.Parse(this.txtFechaSustitucion.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaSustitucion.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaSustitucion.Text = string.Empty;
            }
        }
        public int? TiempoUsoActivos
        {
            get
            {
                if (!string.IsNullOrEmpty(this.hdnTiempoUsoActivos.Value) && !string.IsNullOrWhiteSpace(this.hdnTiempoUsoActivos.Value))
                {
                    int val;
                    return Int32.TryParse(this.hdnTiempoUsoActivos.Value, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.hdnTiempoUsoActivos.Value = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public string NumeroPlaca
        {
            get
            {
                var txt = this.mFlota.Controls[6].FindControl("txtValue") as TextBox;
                return (String.IsNullOrEmpty(txt.Text)) ? null : txt.Text.Trim().ToUpper();
            }
            set
            {
                var txt = this.mFlota.Controls[6].FindControl("txtValue") as TextBox;
                txt.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string TipoPlaca
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtTipoPlaca.Text)) ? null : this.txtTipoPlaca.Text.Trim().ToUpper();
            }
            set
            {
                this.txtTipoPlaca.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? SeguroID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnSeguroID.Value))
                    id = int.Parse(this.hdnSeguroID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnSeguroID.Value = value.ToString();
                else
                    this.hdnSeguroID.Value = string.Empty;
            }
        }
        public string Aseguradora
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtAseguradora.Text)) ? null : this.txtAseguradora.Text.Trim().ToUpper();
            }
            set
            {
                this.txtAseguradora.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public string NumeroPoliza
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroPoliza.Text)) ? null : this.txtNumeroPoliza.Text.Trim().ToUpper();
            }
            set
            {
                this.txtNumeroPoliza.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public DateTime? FechaVigenciaSeguroInicial
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaVigenciaSeguroInicial.Text))
                    temp = DateTime.Parse(this.txtFechaVigenciaSeguroInicial.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaVigenciaSeguroInicial.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaVigenciaSeguroInicial.Text = string.Empty;
            }
        }
        public DateTime? FechaVigenciaSeguroFinal
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaVigenciaSeguroFinal.Text))
                    temp = DateTime.Parse(this.txtFechaVigenciaSeguroFinal.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaVigenciaSeguroFinal.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaVigenciaSeguroFinal.Text = string.Empty;
            }
        }

        /// <summary>
        /// Identificador del contrato
        /// </summary>
        public int? ContratoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnContratoID.Value))
                    id = int.Parse(this.hdnContratoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnContratoID.Value = value.ToString();
                else
                    this.hdnContratoID.Value = string.Empty;
            }
        }
        public int? TipoContratoID
        {
            get
            {
                int? id = null;
                if (!String.IsNullOrEmpty(this.hdnTipoContratoID.Value))
                    id = int.Parse(this.hdnTipoContratoID.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTipoContratoID.Value = value.ToString();
                else
                    this.hdnTipoContratoID.Value = string.Empty;
            }
        }
        public string NumeroContrato
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtNumeroContrato.Text)) ? null : this.txtNumeroContrato.Text.Trim().ToUpper();
            }
            set
            {
                this.txtNumeroContrato.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public DateTime? FechaInicioContrato
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaInicioContrato.Text))
                    temp = DateTime.Parse(this.txtFechaInicioContrato.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaInicioContrato.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaInicioContrato.Text = string.Empty;
            }
        }
        public DateTime? FechaVencimientoContrato
        {
            get
            {
                DateTime? temp = null;
                if (!String.IsNullOrEmpty(this.txtFechaVencimientoContrato.Text))
                    temp = DateTime.Parse(this.txtFechaVencimientoContrato.Text.Trim());
                return temp;
            }
            set
            {
                if (value != null)
                    this.txtFechaVencimientoContrato.Text = value.Value.ToShortDateString();
                else
                    this.txtFechaVencimientoContrato.Text = string.Empty;
            }
        }
        public string CuentaClienteNombre
        {
            get
            {
                return (String.IsNullOrEmpty(this.txtCuentaClienteNombre.Text)) ? null : this.txtCuentaClienteNombre.Text.Trim().ToUpper();
            }
            set
            {
                this.txtCuentaClienteNombre.Text = !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value)
                                                 ? value.Trim().ToUpper()
                                                 : string.Empty;
            }
        }
        public int? MesesFaltantesContrato
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtMesesFaltanteContrato.Text) && !string.IsNullOrWhiteSpace(this.txtMesesFaltanteContrato.Text))
                {
                    int val;
                    return Int32.TryParse(this.txtMesesFaltanteContrato.Text, out val) ? (int?)val : null;
                }
                return null;
            }
            set { this.txtMesesFaltanteContrato.Text = value.HasValue ? value.Value.ToString() : string.Empty; }
        }

        public bool? EstaDisponible
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstaDisponible.Value))
                    id = bool.Parse(this.hdnEstaDisponible.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstaDisponible.Value = value.ToString();
                else
                    this.hdnEstaDisponible.Value = string.Empty;

                Image img = this.mFlota.Controls[3].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        public bool? EstaEnContrato
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnEstaEnContrato.Value))
                    id = bool.Parse(this.hdnEstaEnContrato.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnEstaEnContrato.Value = value.ToString();
                else
                    this.hdnEstaEnContrato.Value = string.Empty;

                Image img = this.mFlota.Controls[4].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                else
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
            }
        }
        public bool? TieneEquipoAliado
        {
            get
            {
                bool? id = null;
                if (!String.IsNullOrEmpty(this.hdnTieneEquipoAliado.Value))
                    id = bool.Parse(this.hdnTieneEquipoAliado.Value.Trim());
                return id;
            }
            set
            {
                if (value != null)
                    this.hdnTieneEquipoAliado.Value = value.ToString();
                else
                    this.hdnTieneEquipoAliado.Value = string.Empty;

                Image img = this.mFlota.Controls[5].FindControl("imgEstatus") as Image;
                if (value != null && value == true)
                {
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-SI-ICO.png");
                    this.btnEquipoAliado.Enabled = true;
                }
                else
                {
                    img.ImageUrl = this.ResolveUrl("~/Contenido/Imagenes/ESTATUS-NO-ICO.png");
                    this.btnEquipoAliado.Enabled = false;
                }
            }
        }

        #region Propiedades Visor
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
            try
            {
                presentador = new DetalleExpedienteUnidadPRE(this);
                var lBaja = ObtenerPaqueteNavegacion("Baja");
                if (lBaja != null && (bool)lBaja)
                {
                    RegistrarScript("Events", "mostrarReporte('../Buscador.UI/VisorReporteUI.aspx')");
                    this.LimpiarPaqueteNavegacion("Baja");
                }
                
                if (!Page.IsPostBack)
                {
                    //Se valida el acceso a la vista
                    this.presentador.ValidarAcceso();

                    this.presentador.RealizarPrimeraCarga();
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, this.nombreClase + ".Page_Load:" + ex.Message);
            }
        }
        #endregion

        #region Métodos
        public void PrepararVisualizacion()
        {
            this.txtAreaNombre.Enabled = false;
            this.txtAseguradora.Enabled = false;
            this.txtCuentaClienteNombre.Enabled = false;
            this.txtDepreciacionMensualPorcentaje.Enabled = false;
            this.txtDepreciacionMensualMonto.Enabled = false;
            this.txtEstaticoAnio.Enabled = false;
            this.txtEstaticoClaveOracle.Enabled = false;
            this.txtEstaticoFechaCompra.Enabled = false;
            this.txtEstaticoIDLeader.Enabled = false;
            this.txtEstaticoModelo.Enabled = false;
            this.txtEstaticoMontoFactura.Enabled = false;
            this.txtEstaticoNumEconomico.Enabled = false;
            this.txtEstaticoNumSerie.Enabled = false;
            this.txtEstaticoTipoUnidad.Enabled = false;
            this.txtEstatusNombre.Enabled = false;
            this.txtFechaInicioContrato.Enabled = false;
            this.txtFechaSustitucion.Enabled = false;
            this.txtFechaVencimientoContrato.Enabled = false;
            this.txtFechaVigenciaSeguroFinal.Enabled = false;
            this.txtFechaVigenciaSeguroInicial.Enabled = false;
            this.txtFolioFacturaCompra.Enabled = false;
            this.txtNumeroContrato.Enabled = false;
            this.txtNumeroPoliza.Enabled = false;
            this.txtResidualPorcentaje.Enabled = false;
            this.txtResidualMonto.Enabled = false;
            this.txtSucursalNombre.Enabled = false;
            this.txtTipoPlaca.Enabled = false;
            this.txtValorLibros.Enabled = false;
            this.txtMesesFaltanteContrato.Enabled = false;
            this.txtMesesVidaUtilRestante.Enabled = false;
            this.txtMesesVidaUtilTotal.Enabled = false;
        }

        public void EstablecerPaqueteNavegacion(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".EstablecerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            Session[key] = value;
        }
        public object ObtenerPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".ObtenerPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            return Session[key];
        }
        public void LimpiarPaqueteNavegacion(string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                throw new Exception(nombreClase + ".LimpiarPaqueteNavegacion: Tiene que especificar un nombre para el paquete de navegación");

            if (Session[key] != null)
                Session.Remove(key);
        }

        /// <summary>
        /// Envía al usuario a la página de advertencia por falta de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/ConsultarSeguimientoFlotaUI.aspx"));
        }
        public void RedirigirAAltaFlota()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/RegistrarAltaUnidadUI.aspx"));
        }
        public void RedirigirABajaFlota()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/RegistrarBajaUnidadUI.aspx"));
        }
        public void RedirigirAReactivacionFlota()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/RegistrarReactivacionUnidadUI.aspx"));
        }
        public void RedirigirACambioSucursalUnidad()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/CambiarSucursalUnidadUI.aspx"));
        }
        public void RedirigirACambioDepartamentoUnidad()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/CambiarDepartamentoUnidadUI.aspx"));
        }
        public void RedirigirACambioAsignacionEquiposAliados()
        {
            this.Response.Redirect(this.ResolveUrl("~/Flota.UI/CambiarAsignacionEquipoAliadoUI.aspx"));
        }
        public void RedirigirADetalleActaNacimiento()
        {
            Redirect(this.ResolveUrl("~/Equipos.UI/DetalleActaNacimientoUI.aspx"), "_blank", null);
        }
        public void RedirigirADetalleHistorial()
        {
            
        }
        public void RedirigirADetalleTramites()
        {
            Redirect(this.ResolveUrl("~/Tramites.UI/DetalleTramitesUI.aspx"), "_blank", null);
        }
        public void RedirigirADetalleSeguro()
        {
            Redirect(this.ResolveUrl("~/Tramites.UI/DetalleSeguroUI.aspx"), "_blank", null);
        }
        public void RedirigirADetalleContratoFSL()
        {
            Redirect(this.ResolveUrl("~/Contratos.FSL.UI/DetalleContratoFSLUI.aspx"), "_blank", null);
        }
        public void RedirigirADetalleContratoRD()
        {
            Redirect(this.ResolveUrl("~/Contratos.RD.UI/DetalleContratoRDUI.aspx"), "_blank", null);
        }
        public void RedirigirADetalleContratoCM()
        {
            Redirect(this.ResolveUrl("~/Contratos.Mantto.CM.UI/DetalleContratoCMUI.aspx"), "_blank", null);
        }
        public void RedirigirADetalleContratoSD()
        {
            Redirect(this.ResolveUrl("~/Contratos.Mantto.SD.UI/DetalleContratoSDUI.aspx"), "_blank", null);
        }

        public void PermitirRegresar(bool permitir)
        {
            this.btnRegresar.Enabled = permitir;
        }
        public void PermitirRealizarAltaFlota(bool permitir)
        {
            MenuItem itemMovimientos = this.mFlota.FindItem("Movimientos");
            if (itemMovimientos == null)
                return;

            MenuItem itemHijo = this.mFlota.FindItem("Movimientos/Alta");

            if (permitir && itemHijo == null)
                itemMovimientos.ChildItems.AddAt(0, new MenuItem() { Text = "Alta de Unidad", Value = "Alta" });
            if (!permitir && itemHijo != null)
                itemMovimientos.ChildItems.Remove(itemHijo);

            if (itemMovimientos.ChildItems.Count <= 0)
                itemMovimientos.Enabled = false;
            else
                itemMovimientos.Enabled = true;
        }

        public void PermitirRealizarBajaFlota(bool permitir)
        {
            //SC0006 - El presenter considera el estatus de Siniestro para determinar si se pueden dar de baja las unidades
            MenuItem itemMovimientos = this.mFlota.FindItem("Movimientos");
            if (itemMovimientos == null)
                return;

            MenuItem itemHijo = this.mFlota.FindItem("Movimientos/Baja");

            if (permitir && itemHijo == null)
            {
                //Se calcula la posición en la que se va a poner
                int index = 0;
                if (this.mFlota.FindItem("Movimientos/Alta") != null)
                    index = 1;

                //Se agrega
                itemMovimientos.ChildItems.AddAt(index, new MenuItem() { Text = "Baja de Unidad", Value = "Baja" });
            }
            if (!permitir && itemHijo != null)
                itemMovimientos.ChildItems.Remove(itemHijo);

            if (itemMovimientos.ChildItems.Count <= 0)
                itemMovimientos.Enabled = false;
            else
                itemMovimientos.Enabled = true;
        }
        public void PermitirRealizarReactivacionFlota(bool permitir)
        {
            //SC0006 - El presenter considera el estatus de Siniestro para determinar si se pueden reactivar las unidades
            MenuItem itemMovimientos = this.mFlota.FindItem("Movimientos");
            if (itemMovimientos == null)
                return;

            MenuItem itemHijo = this.mFlota.FindItem("Movimientos/Reactivar");

            if (permitir && itemHijo == null)
            {
                //Se calcula la posición en la que se va a poner
                int index = 0;
                if (this.mFlota.FindItem("Movimientos/Alta") != null)
                    index++;
                if (this.mFlota.FindItem("Movimientos/Baja") != null)
                    index++;

                //Se agrega
                itemMovimientos.ChildItems.AddAt(index, new MenuItem() { Text = "Reactivar Unidad", Value = "Reactivar" });
            }
            if (!permitir && itemHijo != null)
                itemMovimientos.ChildItems.Remove(itemHijo);

            if (itemMovimientos.ChildItems.Count <= 0)
                itemMovimientos.Enabled = false;
            else
                itemMovimientos.Enabled = true;
        }

        public void PermitirRealizarCambioSucursalUnidad(bool permitir)
        {
            MenuItem itemMovimientos = this.mFlota.FindItem("Movimientos");
            if (itemMovimientos == null)
                return;

            MenuItem itemHijo = this.mFlota.FindItem("Movimientos/CambiarSucursal");

            if (permitir && itemHijo == null)
            {
                //Se calcula la posición en la que se va a poner
                int index = 0;
                if (this.mFlota.FindItem("Movimientos/Alta") != null)
                    index++;
                if (this.mFlota.FindItem("Movimientos/Baja") != null)
                    index++;
                if (this.mFlota.FindItem("Movimientos/Reactivar") != null)
                    index++;

                //Se agrega
                itemMovimientos.ChildItems.AddAt(index, new MenuItem() { Text = "Cambiar Unidad de Sucursal", Value = "CambiarSucursal" });
            }
            if (!permitir && itemHijo != null)
                itemMovimientos.ChildItems.Remove(itemHijo);

            if (itemMovimientos.ChildItems.Count <= 0)
                itemMovimientos.Enabled = false;
            else
                itemMovimientos.Enabled = true;
        }
        public void PermitirRealizarCambioDepartamentoUnidad(bool permitir)
        {
            MenuItem itemMovimientos = this.mFlota.FindItem("Movimientos");
            if (itemMovimientos == null)
                return;

            MenuItem itemHijo = this.mFlota.FindItem("Movimientos/CambiarDepartamento");

            if (permitir && itemHijo == null)
            {
                //Se calcula la posición en la que se va a poner
                int index = 0;
                if (this.mFlota.FindItem("Movimientos/Alta") != null)
                    index++;
                if (this.mFlota.FindItem("Movimientos/Baja") != null)
                    index++;
                if (this.mFlota.FindItem("Movimientos/Reactivar") != null)
                    index++;
                if (this.mFlota.FindItem("Movimientos/CambiarSucursal") != null)
                    index++;

                //Se agrega
                itemMovimientos.ChildItems.AddAt(index, new MenuItem() { Text = "Cambiar Unidad de Departamento", Value = "CambiarDepartamento" });
            }
            if (!permitir && itemHijo != null)
                itemMovimientos.ChildItems.Remove(itemHijo);

            if (itemMovimientos.ChildItems.Count <= 0)
                itemMovimientos.Enabled = false;
            else
                itemMovimientos.Enabled = true;
        }
        public void PermitirRealizarCambioAsignacionEquiposAliados(bool permitir)
        {
            MenuItem itemMovimientos = this.mFlota.FindItem("Movimientos");
            if (itemMovimientos == null)
                return;

            MenuItem itemHijo = this.mFlota.FindItem("Movimientos/CambiarEquiposAliados");

            if (permitir && itemHijo == null)
                itemMovimientos.ChildItems.Add(new MenuItem() { Text = "Cambiar Asignación de Equipos Aliados", Value = "CambiarEquiposAliados" });
            if (!permitir && itemHijo != null)
                itemMovimientos.ChildItems.Remove(itemHijo);

            if (itemMovimientos.ChildItems.Count <= 0)
                itemMovimientos.Enabled = false;
            else
                itemMovimientos.Enabled = true;
        }
        public void PermitirRealizarCambioSucursalEquipoAliado(bool permitir)
        {
            this.hlkMovimiento.Enabled = permitir;
        }
        public void PermitirConsultarActaNacimiento(bool permitir)
        {
            this.btnActa.Enabled = permitir;
            this.btnLlantas.Enabled = permitir;
            this.btnEquipoAliado.Enabled = permitir && this.TieneEquipoAliado != null && this.TieneEquipoAliado == true;
        }
        public void PermitirConsultarHistorial(bool permitir)
        {
            this.btnHistorial.Enabled = permitir;
        }
        public void PermitirConsultarTramites(bool permitir)
        {
            this.btnTramites.Enabled = permitir;
        }
        public void PermitirConsultarSeguro(bool permitir)
        {
            this.btnSeguro.Enabled = permitir;
        }
        public void PermitirConsultarContrato(bool permitir)
        {
            this.btnContrato.Enabled = permitir;
        }

        public void LimpiarSesion()
        {
        }

        private static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "La página que desea abrir no se encuentra dentro del contexto. No se puede abrir una nueva ventana.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
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

        #region Métodos para el Visor
        /// <summary>
        /// Ejecuta el visor
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="tipoBusqueda">Nombre catalogo</param>
        private void EjecutaVisor(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            this.Session_ObjetoBuscador = presentador.PrepararBOVisor(catalogoBusqueda.ToString());
            this.Session_BOSelecto = null;
            this.RegistrarScript("Events", "BtnVisualizar('" + ViewState_Guid + "','" + catalogo + "');");
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

        /// <summary>
        /// Remover del Querystring un valor del key
        /// </summary>
        /// <param name="url">Direccion url del sitio que se redirige con los parámetros</param>
        /// <param name="key">Llave del parámetro que se desea eliminar de la url</param>
        /// <returns></returns>
        public static string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }
        #endregion

        #region Eventos
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.Regresar();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al intentar regresar.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnRegresar_Click: " + ex.Message);
            }
        }
        protected void btnActa_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarActaNacimiento();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al redirigir al acta de nacimiento.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnActa_Click: " + ex.Message);
            }
        }
        protected void btnContrato_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarContrato();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al redirigir al contrato.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnContrato_Click: " + ex.Message);
            }
        }
        protected void btnSeguro_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarSeguro();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al redirigir al seguro.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnSeguro_Click: " + ex.Message);
            }
        }
        protected void btnTramites_Click(object sender, EventArgs e)
        {
            try
            {
                this.presentador.ConsultarTramites();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al redirigir a los trámites de la unidad.", ETipoMensajeIU.ERROR, this.nombreClase + ".btnTramites_Click: " + ex.Message);
            }
        }

        protected void mFlota_MenuItemClick(object sender, System.Web.UI.WebControls.MenuEventArgs e)
        {
            try
            {
                switch (e.Item.Value)
                {
                    case "Alta":
                        this.presentador.RealizarAltaFlota();
                        break;
                    case "Baja":
                        this.presentador.RealizarBajaFlota();
                        break;
                    case "Reactivar":
                        this.presentador.RealizarReactivacionFlota();
                        break;
                    case "CambiarSucursal":
                        this.presentador.RealizarCambioSucursalUnidad();
                        break;
                    case "CambiarDepartamento":
                        this.presentador.RealizarCambioDepartamentoUnidad();
                        break;
                    case "CambiarEquiposAliados":
                        this.presentador.RealizarCambioAsignacionEquiposAliados();
                        break;
                    default:
                        throw new Exception("No se encontró la acción a realizar");
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencias al ejecutar la acción del menú", ETipoMensajeIU.ERROR, nombreClase + ".mReservacion_MenuItemClick:" + ex.Message);
            }
        }

        #region Eventos para el Visor
        protected void btnHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.UnidadID != null)
                    this.EjecutaVisor("HistorialUnidad&hidden=0", ECatalogoBuscador.HistorialUnidad);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Inconsistencia al visualizar el historial de la unidad", ETipoMensajeIU.ADVERTENCIA, this.nombreClase + ".btnHistorial_Click:" + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}