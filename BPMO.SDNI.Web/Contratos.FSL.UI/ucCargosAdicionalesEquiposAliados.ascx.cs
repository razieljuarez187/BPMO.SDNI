// Satisface al Caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
// Satisface al Caso de uso CU023 - Editar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using System.Text;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucCargosAdicionalesEquiposAliados : System.Web.UI.UserControl, IucCargosAdicionalesEquiposAliadosVIS
    {
        #region Atributos
        ucCargosAdicionalesEquiposAliadosPRE presentador;
        private const string NombreClase = "ucCargosAdicionalesEquiposAliados";
        #endregion

        #region Propiedades
        /// <summary>
        /// Indica si el detalle de Tarifas de equipos aliados se despliega en modo consultar
        /// </summary>
        public bool ModoConsultar{
            get { return hdnModoConsultar.Value == "1"; }
        }
        /// <summary>
        /// Titulo que se aplica los botonees
        /// </summary>
        public string TituloBotones {
            get { return hdnTituloBotones.Value; }
        }
        /// <summary>
        /// Plazo del contrato en Años
        /// </summary>
        public int? Plazo
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnPlazo.Value)) value = int.Parse(hdnPlazo.Value);

                return value;
            }
            set
            {
                hdnPlazo.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        public int? UnidadOperativaID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidadOperativaID.Value)) value = int.Parse(hdnUnidadOperativaID.Value);

                return value;
            }
            set
            {
                hdnUnidadOperativaID.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Listado de Equipos Aliados de la Unidad
        /// </summary>
        public List<EquipoAliadoBO> EquipoAliados
        {
            get
            {
                List<EquipoAliadoBO> listValue = null;
                if (Session["EquipoAliadosCargosAdicionales"] != null)
                    listValue = (List<EquipoAliadoBO>)Session["EquipoAliadosCargosAdicionales"];
                else
                    listValue = new List<EquipoAliadoBO>();

                return listValue;
            }
            set
            {
                List<EquipoAliadoBO> listValue = null;
                listValue = value ?? new List<EquipoAliadoBO>();

                Session["EquipoAliadosCargosAdicionales"] = listValue;

                lvwEquiposAliados.DataSource = listValue;
                lvwEquiposAliados.DataBind();
            }
        }
        /// <summary>
        /// Tipo de Cotizacion para los cargos adicionales de los equipos aliados
        /// </summary>
        public ETipoCotizacion? TipoCotizacion
        {
            get
            {
                ETipoCotizacion? value = null;

                if (!string.IsNullOrEmpty(hdnTipoCotizacion.Value)) value = (ETipoCotizacion)int.Parse(hdnTipoCotizacion.Value);

                return value;
            }
            set {
                hdnTipoCotizacion.Value = value != null ? Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture) : string.Empty;
            }
        }
        /// <summary>
        /// Indetificador de la Unidad
        /// </summary>
        public int? UnidadID
        {
            get
            {
                int? value = null;

                if (!string.IsNullOrEmpty(hdnUnidadID.Value)) value = int.Parse(hdnUnidadID.Value);

                return value;
            }
            set {
                hdnUnidadID.Value = value != null ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// Nombre la Coleccion en Session que almacena los cargos adicionales
        /// </summary>
        public string NombreColeccionSesion
        {
            get
            {
                return "CargoAdicional_" + UnidadID.ToString();
            }
        }
        #endregion

        #region Metodos
        public List<CargoAdicionalEquipoAliadoBO> ObtenerCargosAdicionales()
        {
            List<CargoAdicionalEquipoAliadoBO> listado;
            if (Session[NombreColeccionSesion] != null)
                listado = (List<CargoAdicionalEquipoAliadoBO>)Session[NombreColeccionSesion];
            else
                listado = new List<CargoAdicionalEquipoAliadoBO>();

            return listado;
        }

        public void EstablecerCargosAdicionales(List<CargoAdicionalEquipoAliadoBO> cargosAdicionales)
        {
            Session[NombreColeccionSesion] = cargosAdicionales ?? new List<CargoAdicionalEquipoAliadoBO>();
        }

        public void InicializarControl(int? plazo, ETipoCotizacion? tipoCotizacion, List<EquipoAliadoBO> equiposAliados, int? unidadID, int? unidadOperativaID)
        {
            TipoCotizacion = tipoCotizacion;
            Plazo = plazo;
            UnidadID = unidadID;
            UnidadOperativaID = unidadOperativaID;
            lvwEquiposAliados.DataSource = equiposAliados;
            lvwEquiposAliados.DataBind();
        }

        public void InicializarControl(int? plazo, ETipoCotizacion? tipoCotizacion,List<CargoAdicionalEquipoAliadoBO> cargos,int? unidadID, int? unidadOperativaID)
        {
            TipoCotizacion = tipoCotizacion;
            Plazo = plazo;
            UnidadID = unidadID;
            UnidadOperativaID = unidadOperativaID;
            
            lvwEquiposAliados.DataSource = cargos;
            lvwEquiposAliados.DataBind();
        }

        public void ConfigurarModoConsultar()
        {
            hdnModoConsultar.Value = "1";
            hdnTituloBotones.Value = "Ver Tarifas";
        }

        public void ConfigurarModoEditar()
        {
            hdnModoConsultar.Value = string.Empty;
            hdnTituloBotones.Value = "Tarifas";
        }

        public void LimpiarSesion()
        {
            Session.Remove(NombreColeccionSesion);
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucCargosAdicionalesEquiposAliadosPRE(this);
                if (!Page.IsPostBack)
                {
                    presentador.Inicializar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".Page_Load: " + ex.Message);
            }
        }

        protected void lvwEquiposAliados_ItemDataBound(object sender, ListViewItemEventArgs e) {
            try {
                if (e.Item != null) {
                    var lblVIN = e.Item.FindControl("lblVINDato") as Label;
                    var lblModelo = e.Item.FindControl("lblModeloDato") as Label;
                    var boton = e.Item.FindControl("btnCapturarTarifas") as Button;
                    if (boton != null)
                        boton.Text = TituloBotones;
                    var check = e.Item.FindControl("cbCapturado") as CheckBox;

                    var script = new StringBuilder();

                    if (e.Item.DataItem is EquipoAliadoBO) {
                        var equipo = e.Item.DataItem as EquipoAliadoBO;
                        if (lblVIN != null)
                            lblVIN.Text = equipo.NumeroSerie;
                        if (equipo.Modelo != null) {
                            if (lblModelo != null)
                                lblModelo.Text = equipo.Modelo.Nombre;
                        }

                        string openDialog =
                            string.Format(
                                " showDialogModal('TarifasEquipoAliadoUI.aspx?UnidadID='+ {0} +'&EquipoAliadoID='+ {1} +'&EquipoID='+ {2} +'&TipoCotizacion='+ {3} +'&Plazo=' + {4} + '&SucursalID=' + {5}  + '&UnidadOperativaID=' + {6} + '&Variable={7}' + '&checkTarifa={8}', 'Tarifa Equipo Aliado','955px', '450px', 'ValidarCapturaTarifa()'); initDialog(); ",
                                UnidadID.ToString(), equipo.EquipoAliadoID.ToString(), equipo.EquipoID.ToString(),
                                Convert.ToInt32(TipoCotizacion).ToString(CultureInfo.InvariantCulture), Plazo.ToString(),
                                equipo.Sucursal.Id.ToString(), UnidadOperativaID.ToString(), NombreColeccionSesion, check.ClientID);

                        script.Append(openDialog);

                        //script.Append(" if(valorRetornado == '1'){ ");
                        //if (check != null) script.Append("     $('#" + check.ClientID + "').attr('checked', true) }");
                        script.Append(" return false; ");

                    } else {
                        var cargo = e.Item.DataItem as CargoAdicionalEquipoAliadoBO;
                        if (cargo != null) {
                            lblVIN.Text = cargo.EquipoAliado.NumeroSerie;

                            if (cargo.EquipoAliado.Modelo != null) lblModelo.Text = cargo.EquipoAliado.Modelo.Nombre;

                            string openDialog =
                                string.Format(
                                    " showDialogModal('TarifasEquipoAliadoUI.aspx?UnidadID='+ {0} +'&EquipoAliadoID='+ {1} +'&EquipoID='+ {2} +'&TipoCotizacion='+ {3} +'&Plazo=' + {4} + '&SucursalID=' + {5}  + '&UnidadOperativaID=' + {6} + '&Variable={7}&CargoAdicionalID={8}&Consultar={9}', 'Tarifa Equipo Aliado','955px', '450px', undefined); initDialog(); ",
                                    UnidadID.ToString(), cargo.EquipoAliado.EquipoAliadoID.ToString(),
                                    cargo.EquipoAliado.EquipoID.ToString(),
                                    Convert.ToInt32(TipoCotizacion).ToString(CultureInfo.InvariantCulture),
                                    Plazo.ToString(),
                                    cargo.EquipoAliado.Sucursal.Id.ToString(), UnidadOperativaID.ToString(),
                                    NombreColeccionSesion, cargo.CobrableID.ToString(), ModoConsultar.ToString());

                            script.Append(openDialog);

                            script.Append(" return false;");
                            check.Checked = true;
                        }
                    }

                    check.Attributes.Add("disabled", "true");
                    boton.OnClientClick = script.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(NombreClase + ".lvwEquiposAliados_ItemDataBound:" + ex.Message);
            }
        }

        #endregion
    }
}