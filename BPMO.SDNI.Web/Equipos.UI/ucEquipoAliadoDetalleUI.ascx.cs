//Satisface al CU075 - Catálogo de Equipo Aliado
// Satisface a la SC0005
using System;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;
using BPMO.SDNI.MapaSitio.UI;
using BPMO.SDNI.Comun.UI;
using BPMO.SDNI.Comun.BO;
using Newtonsoft.Json;
using BPMO.SDNI.Comun.PRE;

namespace BPMO.SDNI.Equipos.UI
{
    public partial class ucEquipoAliadoDetalleUI : UserControl, IucEquipoAliadoDetalleVIS
    {
        #region Atributos
        private ucEquipoAliadoDetallePRE presentador;
        private const string nombreClase = "ucEquipoAliadoDetalleUI";
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

        public string UnidadOperativaNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEmpresa.Text) && !string.IsNullOrWhiteSpace(this.txtEmpresa.Text))
                    return this.txtEmpresa.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtEmpresa.Text = value;
                else
                    this.txtEmpresa.Text = string.Empty;
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
                    if (Decimal.TryParse(this.txtPBV.Text.Trim().Replace(",", ""), out val)) //RI0012
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
                    if (Decimal.TryParse(this.txtPBC.Text.Trim().Replace(",", ""), out val)) //RI0012
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
                if (!string.IsNullOrEmpty(this.txtTipoEquipo.Text) && !string.IsNullOrWhiteSpace(this.txtTipoEquipo.Text))
                    return this.txtTipoEquipo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtTipoEquipo.Text = value;
                else
                    this.txtTipoEquipo.Text = string.Empty;
            }
        }

        public string TipoEquipoAliado
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtTipoEquipoAliado.Text) && !string.IsNullOrWhiteSpace(this.txtTipoEquipoAliado.Text))
                    return this.txtTipoEquipoAliado.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtTipoEquipoAliado.Text = value;
                else
                    this.txtTipoEquipoAliado.Text = string.Empty;
            }
        }

        public int? TipoEquipoID
        {
            get
            {
                int val = 0;
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
                    return this.txtDimenciones.Text.ToUpper();
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
                    if (Int32.TryParse(this.txtHorasIniciales.Text.Trim(), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtHorasIniciales.Text =  value.ToString(); //RI0012
                else
                    this.txtHorasIniciales.Text = string.Empty;
            }
        }

        public int? KilometrosIniciales
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.txtKilometrosIniciales.Text) && !string.IsNullOrWhiteSpace(this.txtKilometrosIniciales.Text))
                    if (Int32.TryParse(this.txtKilometrosIniciales.Text.Trim(), out val)) //RI0012
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.txtKilometrosIniciales.Text = value.ToString(); //RI0012
                else
                    this.txtKilometrosIniciales.Text = string.Empty;
            }
        }

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

        public int? EstatusID
        {
            get
            {
                int val;
                if (!string.IsNullOrEmpty(this.hdnEstatusID.Value) && !string.IsNullOrWhiteSpace(this.hdnEstatusID.Value))
                    if (Int32.TryParse(this.hdnEstatusID.Value, out val))
                        return val;
                return null;
            }
            set
            {
                if (value != null)
                    this.hdnEstatusID.Value = value.Value.ToString();
                else
                    this.hdnEstatusID.Value = string.Empty;
            }
        }

        public string EstatusNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtEstatusEquipo.Text) && !string.IsNullOrWhiteSpace(this.txtEstatusEquipo.Text))
                    return this.txtEstatusEquipo.Text.Trim().ToUpper();
                return null;
            }
            set
            {
                if (value != null)
                    this.txtEstatusEquipo.Text = value;
                else
                    this.txtEstatusEquipo.Text = string.Empty;
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
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            this.presentador = new ucEquipoAliadoDetallePRE(this);

        }
        #endregion

        #region Métodos
        public void PrepararVista()
        {
            this.txtActivoOracle.Text = string.Empty;
            this.txtAnioModelo.Text = string.Empty;
            this.txtDimenciones.Text = string.Empty;
            this.txtEmpresa.Text = string.Empty;
            this.txtEstatusEquipo.Text = string.Empty;
            this.txtFabricante.Text = string.Empty;
            this.txtHorasIniciales.Text = string.Empty;
            this.txtKilometrosIniciales.Text = string.Empty;
            this.txtMarca.Text = string.Empty;
            this.txtModelo.Text = string.Empty;
            this.txtNumeroSerie.Text = string.Empty;
            this.txtOracleID.Text = string.Empty;
            this.txtPBC.Text = string.Empty;
            this.txtPBV.Text = string.Empty;
            this.txtSucursal.Text = string.Empty;
            this.txtTipoEquipo.Text = string.Empty;
            this.txtTipoEquipoAliado.Text = string.Empty;
            this.hdnEquipoAliadoID.Value = string.Empty;
            this.hdnEquipoID.Value = string.Empty;
            this.hdnLiderID.Value = string.Empty;
            this.hdnModeloID.Value = string.Empty;
            this.hdnOracleID.Value = string.Empty;
            this.hdnSucursalID.Value = string.Empty;
            this.hdnTipoEquipoAliadoID.Value = string.Empty;
            this.hdnUnidadOperativaID.Value = string.Empty;

            this.txtActivoOracle.Enabled = false;
            this.txtAnioModelo.Enabled = false;
            this.txtDimenciones.Enabled = false;
            this.txtEmpresa.Enabled = false;
            this.txtEstatusEquipo.Enabled = false;
            this.txtFabricante.Enabled = false;
            this.txtHorasIniciales.Enabled = false;
            this.txtKilometrosIniciales.Enabled = false;
            this.txtMarca.Enabled = false;
            this.txtModelo.Enabled = false;
            this.txtNumeroSerie.Enabled = false;
            this.txtOracleID.Enabled = false;
            this.txtPBC.Enabled = false;
            this.txtPBV.Enabled = false;
            this.txtSucursal.Enabled = false;
            this.txtTipoEquipo.Enabled = false;
            this.txtTipoEquipoAliado.Enabled = false;			
            this.chkActivo.Enabled = false;			
        }

        public object ObtenerDatosNavegacion()
        {
            return Session["EquipoAliadoBODetalle"];
        }

        public void LimpiarSesion()
        {
            if (Session["EquipoAliadoBODetalle"] != null)
                this.Session.Remove("EquipoAliadoBODetalle");
            if (Session["EquipoAliadoEditar"] != null)
                this.Session.Remove("EquipoAliadoEditar");
        }

        public void RedirigirAEdicion()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/EditarEquipoAliadoUI.aspx"), false);
        }

        public void RedirigirAEliminar()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/EliminarEquipoAliadoUI.aspx"), false);
        }

        public void RedirigirAConsulta()
        {
            this.Response.Redirect(this.ResolveUrl("~/Equipos.UI/ConsultarEquipoAliadoUI.aspx"), false);
        }

        public void EstablecerDatosNavegacion(string nombre, object valor)
        {
            Session[nombre] = valor;
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

        #region REQ 13596
 
        /// <summary>
        /// Prepara los controles (etiquetas y visualización) que serán válidos para la unidad operativa a la cual accedió el usuario.
        /// </summary>
        /// <param name="tipoEmpresa">Indica la unidad operativa, este valor determina el comportamiento de los controles.</param>
        public void EstablecerAcciones(ETipoEmpresa tipoEmpresa)
        {
            //Visibilidad de controles.
            if (tipoEmpresa == ETipoEmpresa.Generacion || tipoEmpresa == ETipoEmpresa.Equinova)
            {
                this.rowHorasIniciales.Visible = false;
                this.rowKilometrajeInicial.Visible = false;
            }

            //Cambiando etiqueta de los controles.
            string PBC = ObtenerEtiquetadelResource("EQ01", tipoEmpresa);
            if (string.IsNullOrEmpty(PBC))
                this.rowPBC.Visible = true;
            else
                this.lblPBC.Text = PBC;
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



    }
}