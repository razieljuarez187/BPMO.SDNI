//Construccion durante staffing - Eliminar unidades de un contrato en curso
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.MapaSitio.UI;
using DevExpress.XtraCharts.GLGraphics;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class EditarLineasContratoFSLUI : Page, IEditarLineasContratoFSLVIS
    {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, utilizado para excepciones
        /// </summary>
        private readonly string NombreClase = typeof(EditarLineasContratoFSLUI).Name;
        /// <summary>
        /// Enumerador utilizado para el Buscador
        /// </summary>
        public enum ECatalogoBuscador
        {
            UnidadIdealease = 1,
            EquipoAliado =2,
            ProductoServicio = 3
        }
        #endregion
        #region Propiedades
        #region Generales
        /// <summary>
        /// Presentador de la Interfaz de Usuario
        /// </summary>
        private EditarLineasContratoFSLPRE presentador;
        /// <summary>
        /// Numero de Serie de la Unidad que será agregada al Contrato
        /// </summary>
        public string NumeroSerie
        {
            get { return !String.IsNullOrEmpty(txtNumeroSerie.Text) ? txtNumeroSerie.Text.Trim().ToUpper() : ""; }
            set { txtNumeroSerie.Text = !String.IsNullOrEmpty(value) ? value.ToUpper() : ""; }
        }
        /// <summary>
        /// UnidadId de la Linea que se esta Modificando
        /// </summary>
        public int? LineaUnidadId
        {
            get {if(!String.IsNullOrEmpty(hdnLineaUnidadId.Value)) return int.Parse(hdnLineaUnidadId.Value); return null; } 
            set { hdnLineaUnidadId.Value = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// EquipoID de la Linea que se esta Modificando
        /// </summary>
        public int? LineaEquipoId 
        {
            get { if(!String.IsNullOrEmpty(hdnLineaEquipoId.Value)) return int.Parse(hdnLineaEquipoId.Value); return null; }
            set { hdnLineaEquipoId.Value = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Lista de Lineas de Contrato
        /// </summary>
        public List<LineaContratoFSLBO> LineasContrato
        {
            get
            {
                return Session["LineasContratoFSL"] == null ? new List<LineaContratoFSLBO>() : (List<LineaContratoFSLBO>) Session["LineasContratoFSL"];
            }
            set
            {
                if (value != null)
                {
                    Session["LineasContratoFSL"] = value;
                }
                else
                    Session.Remove("LineasContratoFSL");
            }
        }
        /// <summary>
        /// Obtiene la Lista de sucursales permitidas para el Usuario
        /// </summary>
        public List<SucursalBO> ListaSucursalesPermitidas
        {
            get { return Session["ListaSucursalesPermitidas"] == null ? null : (List<SucursalBO>)Session["ListaSucursalesPermitidas"]; }
            set { if (value != null) Session["ListaSucursalesPermitidas"] = value; else Session.Remove("ListaSucursalesPermitidas"); }
        }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        public string NumeroContrato
        {
            get
            {
                return !String.IsNullOrEmpty(txtNumeroContrato.Text.Trim()) ? txtNumeroContrato.Text.Trim().ToUpper() : "";
            }
            set
            {
                txtNumeroContrato.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : "";
            }
        }
        /// <summary>
        /// Fecha de Inicio del Contrato
        /// </summary>
        public DateTime? FechaInicioContrato
        {
            get
            {
                DateTime fecha;
                if (DateTime.TryParse(!String.IsNullOrEmpty(txtFechaInicioContrato.Text) ? txtFechaInicioContrato.Text.Trim().ToUpper() : "", out fecha))
                {
                    return fecha;
                }
                return null;
            }
            set
            {
                txtFechaInicioContrato.Text = value != null ? value.Value.ToShortDateString() : "";
            }
        }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        public string NombreCliente
        {
            get
            {
                return !String.IsNullOrEmpty(txtClienteNombre.Text) ? txtClienteNombre.Text.Trim().ToUpper() : "";
            }
            set
            {
                txtClienteNombre.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : "";
            }
        }
        /// <summary>
        /// Contrato Original que será Modificado
        /// </summary>
        public ContratoFSLBO ContratoAnterior
        {
            get
            {
                return Session["ContratoFSLAnterior"] == null ? null : (ContratoFSLBO) Session["ContratoFSLAnterior"];
            }
            set
            {
                if (value != null)
                    Session["ContratoFSLAnterior"] = value;
                else
                {
                    if(Session["ContratoFSLAnterior"] != null)
                        Session.Remove("ContratoFSLAnterior");
                }
            }
        }
        /// <summary>
        /// Identificador del usuario logueado
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var master = (Site)Page.Master;
                if(master != null)
                    if(master.Usuario != null && master.Usuario.Usuario != null)
                        if(master.Usuario.Id.HasValue)
                            return master.Usuario.Id.Value;
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
                var master = (Site)Page.Master;
                if(master != null)
                    if(master.Adscripcion != null && master.Adscripcion.UnidadOperativa != null)
                        if(master.Adscripcion.UnidadOperativa.Id.HasValue)
                            return master.Adscripcion.UnidadOperativa.Id.Value;
                return null;
            }
        }
        /// <summary>
        /// Unidad Opertiva cargada de Sesion
        /// </summary>
        public UnidadOperativaBO UnidadOperativa
        {
            get
            {
                var masterMsj = (Site)Page.Master;

                if(masterMsj != null && (masterMsj.Adscripcion != null && masterMsj.Adscripcion.UnidadOperativa != null))
                    return new UnidadOperativaBO { Id = masterMsj.Adscripcion.UnidadOperativa.Id, Nombre = masterMsj.Adscripcion.UnidadOperativa.Nombre };
                return null;
            }
        }
        /// <summary>
        /// Contiene las Observaciones por unidades o Equipos Aliados que salgan de los contratos, 
        /// EquipoID es el Key
        /// </summary>
        public Dictionary<string, string> ObservacionesUnidad {
            get
            {
                if (Session["ObservacionesUnidad"] != null)
                    return (Dictionary<string, string>) Session["ObservacionesUnidad"];
                return null;
            }
            set
            {
                if (value != null)
                    Session["ObservacionesUnidad"] = value;
                else
                {
                    if (Session["ObservacionesUnidad"] != null)
                        Session.Remove("ObservacionesUnidad");
                }
            } 
        }
        /// <summary>
        /// Lista de Unidades que se quitan del Contrato
        /// </summary>
        public List<UnidadBO> UnidadesLiberar
        {
            get
            {
                if (Session["UnidadesLiberar"] != null)
                    return (List<UnidadBO>) Session["UnidadesLiberar"];
                return null;
            }
            set
            {
                if (value != null)
                    Session["UnidadesLiberar"] = value;
                else
                {
                    if(Session["UnidadesLiberar"] != null)
                        Session.Remove("UnidadesLiberar");
                }
            }
        }
        /// <summary>
        /// Lista de Unidades que tuvieron cambio en los equipos Aliados del contrato
        /// </summary>
        public List<UnidadBO> UnidadesCambioEquipos
        {
            get
            {
                if(Session["UnidadesCambio"] != null)
                    return (List<UnidadBO>)Session["UnidadesCambio"];
                return null;
            }
            set
            {
                if(value != null)
                    Session["UnidadesCambio"] = value;
                else
                {
                    if(Session["UnidadesCambio"] != null)
                        Session.Remove("UnidadesCambio");
                }
            }
        }
        #endregion
        #region Propiedades Linea Contrato
        /// <summary>
        /// VIN de la unidad
        /// </summary>
        public string VinUnidad
        {
            get { return !String.IsNullOrEmpty(txtVinUnidad.Text) ? txtVinUnidad.Text.Trim().ToUpper() : null; }
            set { txtVinUnidad.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : ""; }
        }
        /// <summary>
        /// Numero Economico de la Unidad de la Linea
        /// </summary>
        public string NumeroEconomicoUnidad
        {
            get { return !String.IsNullOrEmpty(txtNumeroEconomico.Text) ? txtNumeroEconomico.Text.Trim().ToUpper() : null; }
            set { txtNumeroEconomico.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : ""; }
        }
        /// <summary>
        /// Nombre del Modelo de la Unidad
        /// </summary>
        public string NombreModelo
        {
            get { return !String.IsNullOrEmpty(txtNombreModelo.Text) ? txtNombreModelo.Text.Trim().ToUpper() : null; }
            set { txtNombreModelo.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : ""; }
        }
        /// <summary>
        /// Año de la Unidad
        /// </summary>
        public int? AnioUnidad
        {
            get
            {
                int valor;
                if(String.IsNullOrEmpty(txtAnio.Text)) return null;
                if(Int32.TryParse(txtAnio.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtAnio.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Cantidad Maxima de de PBV
        /// </summary>
        public decimal? PBVMaximoRecomendado
        {
            get
            {
                decimal valor;
                if (String.IsNullOrEmpty(txtPBV.Text)) return null;
                if (Decimal.TryParse(txtPBV.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtPBV.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Cantidad Maxima de PBC
        /// </summary>
        public decimal? PBCMaximoRecomendado
        {
            get
            {
                decimal valor;
                if(String.IsNullOrEmpty(txtPBC.Text)) return null;
                if(Decimal.TryParse(txtPBC.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtPBC.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Kilometra Inicial de la Unidad
        /// </summary>
        public int? KmInicial
        {
            get
            {
                int valor;
                if(String.IsNullOrEmpty(txtKmInicial.Text)) return null;
                if(Int32.TryParse(txtKmInicial.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtKmInicial.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Poliza de Seguro Activa la Unidad
        /// </summary>
        public string PolizaSeguro
        {
            get { return !String.IsNullOrEmpty(txtPolizaSeguro.Text) ? txtPolizaSeguro.Text.Trim().ToUpper() : null; }
            set { txtPolizaSeguro.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : ""; }
        }
        /// <summary>
        /// Kilometraje Estimado Anual
        /// </summary>
        public int? KmEstimadoAnual
        {
            get
            {
                int valor;
                if(String.IsNullOrEmpty(txtKmEstimadoAnual.Text)) return null;
                if(Int32.TryParse(txtKmEstimadoAnual.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtKmEstimadoAnual.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Deposito en Garantia por la Unidad
        /// </summary>
        public decimal? DepositoGarantia
        {
            get
            {
                decimal valor;
                if(String.IsNullOrEmpty(txtDepositoGarantia.Text)) return null;
                if(Decimal.TryParse(txtDepositoGarantia.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtDepositoGarantia.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Comision por Apertura del Contrato
        /// </summary>
        public decimal? ComisionApertura
        {
            get
            {
                decimal valor;
                if(String.IsNullOrEmpty(txtComisionApertura.Text)) return null;
                if(Decimal.TryParse(txtComisionApertura.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtComisionApertura.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Cargo Fijo Mensual de la Unidad
        /// </summary>
        public decimal? CargoFijoMensual
        {
            get
            {
                decimal valor;
                if(String.IsNullOrEmpty(txtCargoFijo.Text)) return null;
                if(Decimal.TryParse(txtCargoFijo.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtCargoFijo.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Tipo de Cotizacion de la Linea
        /// </summary>
        public ETipoCotizacion? TipoCotizacion
        {
            get
            {
                if (ddlTipoCotizacion.SelectedIndex == 0) return null;
                return (ETipoCotizacion)Int32.Parse(ddlTipoCotizacion.SelectedValue);
            }
            set
            {
                if (value == null) ddlTipoCotizacion.SelectedIndex = 0;
                else
                {
                    ddlTipoCotizacion.SelectedValue = ((Int32) value).ToString();
                }
            }
        }
        /// <summary>
        /// Determina si la Unidad sera OC al finalizar el contrato
        /// </summary>
        public bool? ConOpcionCompra
        {
            get
            {
                return cbOpcionCompra.Checked;
            }
            set { cbOpcionCompra.Checked = value != null && value.Value; }
        }
        /// <summary>
        /// Lista de Monedas Disponibles para la Compra
        /// </summary>
        public List<MonedaBO> MonedasDisponibles
        {
            get
            {
                if (Session["MonedasDisponibles"] == null) return null;
                return (List<MonedaBO>) Session["MonedasDisponibles"];
            }
            set
            {
                if (value != null)
                    Session["MonedasDisponibles"] = value;
                else
                {
                    if(Session["MonedasDisponibles"] != null)
                        Session.Remove("MonedasDisponibles");
                }
            }
        }
        /// <summary>
        /// Moneda en la cual se comprará la Unidad
        /// </summary>
        public MonedaBO MonedaCompra
        {
            get
            {
                if (MonedasDisponibles != null)
                {
                    if(ddlMonedaCompra.SelectedIndex == 0) return null;
                    var moneda = MonedasDisponibles.Find(monedaBo => monedaBo.Nombre == ddlMonedaCompra.SelectedItem.Text);
                    return moneda;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (MonedasDisponibles != null)
                    {
                        var nombreMoneda = MonedasDisponibles.FirstOrDefault(x => x.Codigo == value.Codigo);
                        if (nombreMoneda != null)
                            value.Nombre = nombreMoneda.Nombre;
                        var item = ddlMonedaCompra.Items.FindByText(value.Nombre);
                        if (item == null)
                        {
                            ddlMonedaCompra.SelectedIndex = 0;
                            return;
                        }
                        for (int i = 0; i < ddlMonedaCompra.Items.Count; i++)
                        {
                            var itemLista = ddlMonedaCompra.Items[i];
                            if (item.Text == itemLista.Text)
                            {
                                ddlMonedaCompra.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ddlMonedaCompra.SelectedIndex = 0;
                }
            }
        }
        /// <summary>
        /// Cantidad en la cual se comprará la Unidad
        /// </summary>
        public decimal? ImporteCompra
        {
            get
            {
                decimal valor;
                if(String.IsNullOrEmpty(txtImporteCompra.Text)) return null;
                if(Decimal.TryParse(txtImporteCompra.Text.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { txtImporteCompra.Text = value != null ? value.ToString() : ""; }
        }
        /// <summary>
        /// Linea de Contrato que se esta editando
        /// </summary>
        public LineaContratoFSLBO LineaContratoEnEdicion
        {
            get
            {
                if (Session["LineaContratoEnEdicion"] == null) return null;
                return (LineaContratoFSLBO) Session["LineaContratoEnEdicion"];
            }
            set
            {
                if (value != null) { Session["LineaContratoEnEdicion"] = value; }
                else { if (Session["LineaContratoEnEdicion"] != null) Session.Remove("LineaContratoEnEdicion"); }
            }
        }
        /// <summary>
        /// Numero de Serie del Equipo aliado a Agregar
        /// </summary>
        public string NumeroSerieEquipoAliado
        {
            get
            {
                return !String.IsNullOrEmpty(txtVinEquipoAliado.Text) ? txtVinEquipoAliado.Text.Trim().ToUpper() : null;
            } 
            set { txtVinEquipoAliado.Text = !String.IsNullOrEmpty(value) ? value.Trim().ToUpper() : ""; }
        }
        /// <summary>
        /// Identificador de Producto o Servicio (SAT)
        /// </summary>
        public int? ProductoServicioId {
            get { return (string.IsNullOrEmpty(this.hdnProductoServicioId.Value)) ? null : (int?)int.Parse(this.hdnProductoServicioId.Value); }
            set { this.hdnProductoServicioId.Value = (value != null) ? value.ToString() : string.Empty; }
        }
        /// <summary>
        /// Clave de Producto o Servicio (SAT)
        /// </summary>
        public string ClaveProductoServicio {
            get { return (string.IsNullOrEmpty(this.txtClaveProductoServicio.Text)) ? null : this.txtClaveProductoServicio.Text.Trim().ToUpper(); }
            set { this.txtClaveProductoServicio.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }
        /// <summary>
        /// Descripción de Producto o Servicio (SAT)
        /// </summary>
        public string DescripcionProductoServicio {
            get { return (string.IsNullOrEmpty(this.txtDescripcionProductoServicio.Text)) ? null : this.txtDescripcionProductoServicio.Text.Trim().ToUpper(); }
            set { this.txtDescripcionProductoServicio.Text = (value != null) ? value.ToUpper() : string.Empty; }
        }
        #endregion
        #region Propiedades Tarifas
        /// <summary>
        /// Determina si el cobro es por Km o por Horas
        /// </summary>
        public bool? CargoPorKm
        {
            get
            {
                if (ddlTipoCargo.SelectedIndex == 0)
                {
                    hdnCargoKm.Value = "";
                    return null;
                }
                hdnCargoKm.Value = ddlTipoCargo.SelectedIndex == 1 ? "1" : "0";
                return ddlTipoCargo.SelectedIndex == 1;
            }
            set
            {
                if (value != null)
                {
                    ddlTipoCargo.SelectedIndex = value.Value ? 1 : 2;
                    hdnCargoKm.Value = ddlTipoCargo.SelectedIndex == 1 ? "1" : "0";
                }
                else
                {
                    ddlTipoCargo.SelectedIndex = 0;
                    hdnCargoKm.Value = "";
                }
            }
        }
        /// <summary>
        /// Año de la Tarifa a Configurar
        /// </summary>
        public int? AnioTarifa
        {
            get
            {
                if (ddlAniosContrato.SelectedIndex == 0) return null;
                int valor;
                if (Int32.TryParse(ddlAniosContrato.SelectedValue, out valor))
                {
                    return valor+1;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    if(ddlAniosContrato.Items.Count > 0)
                        ddlAniosContrato.SelectedIndex = 0;
                }
                else
                {
                    ddlAniosContrato.SelectedIndex = (value.Value - 1);
                }
            }
        }
        /// <summary>
        /// Frecuencia de Facturacion de la Tarifa
        /// </summary>
        public EFrecuencia? FrecuenciaTarifa
        {
            get
            {
                int valor;
                if (ddlFrecuencia.SelectedIndex == 0) return null;
                if (Int32.TryParse(ddlFrecuencia.SelectedValue, out valor))
                {
                    return (EFrecuencia?) valor;
                }
                return null;
            }
            set
            {
                if (value == null) ddlFrecuencia.SelectedIndex = 0;
                else
                {
                    ddlFrecuencia.SelectedValue = ((Int32) value).ToString();
                }
            }
        }
        /// <summary>
        /// Kilometros u horas libres de la Unidad
        /// </summary>
        public int? KilometrosHorasLibres
        {
            get
            {
                if (!String.IsNullOrEmpty(txtKmHrsLibres.Text))
                {
                    int valor;
                    if (Int32.TryParse(txtKmHrsLibres.Text.Trim(), out valor))
                    {
                        return valor;
                    }
                }
                return null;
            }
            set
            {
                txtKmHrsLibres.Text = value != null ? value.ToString() : "";
            }
        }
        /// <summary>
        /// Cantidad Minima de Kilometros u Horas que se cobraran
        /// </summary>
        public int? KmHrMinima
        {
            get
            {
                if (!String.IsNullOrEmpty(txtKmHrMinimo.Text))
                {
                    int valor;
                    if (int.TryParse(txtKmHrMinimo.Text, out valor))
                    {
                        return valor;
                    }
                }
                return null;
            }
            set
            {
                txtKmHrMinimo.Text = value != null ? value.ToString() : String.Empty;
            }
        }
        /// <summary>
        /// Rango Inicial de la Tarifa
        /// </summary>
        public int? RangoInicialTarifa
        {
            get
            {
                if(!String.IsNullOrEmpty(txtRangoInicial.Text))
                {
                    int valor;
                    if(Int32.TryParse(txtRangoInicial.Text.Trim(), out valor))
                    {
                        return valor;
                    }
                }
                return null;
            }
            set
            {
                txtRangoInicial.Text = value != null ? value.ToString() : "";
            }
        }
        /// <summary>
        /// Determina si es el ultimo Rango de la Tarifa Configurandose
        /// </summary>
        public bool? UltimoRango
        {
            get { return ddlRangoTiempo.SelectedIndex == 1; }
            set
            {
                if (value != null)
                {
                    ddlRangoTiempo.SelectedIndex = value.Value ? 1 : 0;
                }
            }
        }
        /// <summary>
        /// Rango Final de la Tarifa
        /// </summary>
        public int? RangoFinalTarifa
        {
            get
            {
                if(!String.IsNullOrEmpty(txtRangoFinal.Text))
                {
                    int valor;
                    if(Int32.TryParse(txtRangoFinal.Text.Trim(), out valor))
                    {
                        return valor;
                    }
                }
                return null;
            }
            set
            {
                txtRangoFinal.Text = value != null ? value.ToString() : "";
            }
        }
        /// <summary>
        /// Costo por Hora o Kilometro de la Tarifa
        /// </summary>
        public decimal? CostoKmHr
        {
            get
            {
                if(!String.IsNullOrEmpty(txtCargo.Text))
                {
                    decimal valor;
                    if(Decimal.TryParse(txtCargo.Text.Trim(), out valor))
                    {
                        return valor;
                    }
                }
                return null;
            }
            set
            {
                txtCargo.Text = value != null ? value.ToString() : "";
            }
        }
        /// <summary>
        /// Tarifas que se esta configurando
        /// </summary>
        public List<TarifaFSLBO> TarifasEnConfiguracion
        {
            get
            {
                if (Session["TarifasEnConfiguracion"] == null) return null;
                return (List<TarifaFSLBO>) Session["TarifasEnConfiguracion"];
            }
            set
            {
                if (value != null)
                {
                    Session["TarifasEnConfiguracion"] = value;
                }
                else
                {
                    if (Session["TarifasEnConfiguracion"] != null)
                        Session.Remove("TarifasEnConfiguracion");
                }
            }
        }
        /// <summary>
        /// Lista de Rangos que se estan configurando
        /// </summary>
        public List<RangoTarifaFSLBO> RangosEnConfiguracion
        {
            get
            {
                if(Session["RangosEnConfiguracion"] == null) return null;
                return (List<RangoTarifaFSLBO>)Session["RangosEnConfiguracion"];
            }
            set
            {
                if(value != null)
                {
                    Session["RangosEnConfiguracion"] = value;
                }
                else
                {
                    if(Session["RangosEnConfiguracion"] != null)
                        Session.Remove("RangosEnConfiguracion");
                }
            }
        }
        /// <summary>
        /// Tipo de Equipo al que se le configura la Tarifa
        /// </summary>
        public ETipoEquipo? TipoEquipo
        {
            get
            {
                if (String.IsNullOrEmpty(hdnTipoEquipo.Value)) return null;
                int valor;
                if (Int32.TryParse(hdnTipoEquipo.Value.Trim(), out valor))
                {
                    return (ETipoEquipo) valor;
                }
                return null;
            }
            set
            {
                hdnTipoEquipo.Value = value != null ? ((Int32) value.Value).ToString() : "";
            }
        }
        /// <summary>
        /// Identificador de Unidad al que se le esta modificando la Tarifa
        /// </summary>
        public int? UnidadIdTarifa
        {
            get
            {
                if(String.IsNullOrEmpty(hdnUnidadId.Value)) return null;
                int valor;
                if(Int32.TryParse(hdnUnidadId.Value.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { hdnUnidadId.Value = value != null ? value.Value.ToString() : ""; }
        }
        /// <summary>
        /// Identificador de Equipo Aliado al que se le esta modificando la Tarifa
        /// </summary>
        public int? EquipoAliadoIdTarifa
        {
            get
            {
                if(String.IsNullOrEmpty(hdnEquipoAliadoId.Value)) return null;
                int valor;
                if(Int32.TryParse(hdnEquipoAliadoId.Value.Trim(), out valor))
                {
                    return valor;
                }
                return null;
            }
            set { hdnEquipoAliadoId.Value = value != null ? value.Value.ToString() : ""; }
        }
        /// <summary>
        /// Determina si se aplican cargos adicionales a los equipos aliados
        /// </summary>
        public bool? NoAplicaCargosAdicionales
        {
            get { return cbNoAplicarCargos.Checked; }
            set { cbNoAplicarCargos.Checked = value != null && value.Value; }
        }
        #endregion
        #region Propiedades Buscador
        public string ViewState_Guid
        {
            get
            {
                if(ViewState["GuidSession"] == null)
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
                if(Session[nombreSession] != null)
                    objeto = (Session[nombreSession]);

                return objeto;
            }
            set
            {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if(value != null)
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
                if(Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid]);

                return objeto;
            }
            set
            {
                if(value != null)
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
        /// <summary>
        /// PageLoad de la pagina
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new EditarLineasContratoFSLPRE(this);
                if (!Page.IsPostBack)
                {
                    this.presentador.EstablecerSeguridad();
                    presentador.Inicializar();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".Page_Load: " + ex.Message);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Redirige a la página de aviso por fata de permisos
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
        }
        #region Métodos para el Buscador
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del xml</param>
        /// <param name="catalogoBusqueda">Nombre catalogo</param>
        private void EjecutaBuscador(string catalogo, ECatalogoBuscador catalogoBusqueda)
        {
            ViewState_Catalogo = catalogoBusqueda;
            Session_ObjetoBuscador = presentador.PrepararBOBuscador(catalogoBusqueda.ToString());
            Session_BOSelecto = null;
            RegistrarScript("Events", ClientID + "_Buscar('" + ViewState_Guid + "','" + catalogo + "');");
        }
        /// <summary>
        /// Desplegar la información el Objeto Seleccionado
        /// </summary>
        /// <param name="catalogo">Nombre del catalogo</param>
        private void DesplegarBOSelecto(ECatalogoBuscador catalogo)
        {
            presentador.DesplegarResultadoBuscador(catalogo.ToString(), Session_BOSelecto);
            Session_BOSelecto = null;
        }
        #endregion
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
        /// Limpia los campos que se encuentran en la Vista con la información de las Lineas
        /// </summary>
        public void LimpiarInterfazLineas()
        {
            NumeroContrato = null;
            FechaInicioContrato = null;
            NombreCliente = null;
        }
        /// <summary>
        /// Limpia los datos que se encuentran en la Interfaz de Configuracion de Datos de Linea
        /// </summary>
        public void LimpiarLineaUnidad()
        {
            VinUnidad = null;
            NumeroEconomicoUnidad = null;
            NombreModelo = null;
            AnioUnidad = null;
            PBVMaximoRecomendado = null;
            PBCMaximoRecomendado = null;
            KmInicial = null;
            PolizaSeguro = null;
            KmEstimadoAnual = null;
            DepositoGarantia = null;
            ComisionApertura = null;
            CargoFijoMensual = null;
            TipoCotizacion = null;
            ConOpcionCompra = null;
            MonedaCompra = null;
            ImporteCompra = null;
            LineaContratoEnEdicion = null;
        }
        /// <summary>
        /// Limpia los datos que se encuentran en la Interfaz de Tarifas
        /// </summary>
        public void LimpiarInterfazTarifas()
        {
            NoAplicaCargosAdicionales = false;
            CargoPorKm = null;
            AnioTarifa = null;
            FrecuenciaTarifa = null;
            KilometrosHorasLibres = null;
            KmHrMinima = null;
            RangoInicialTarifa = null;
            RangoFinalTarifa = null;
            ddlRangoTiempo.SelectedIndex = 0;
            CostoKmHr = null;
            TarifasEnConfiguracion = null;
            RangosEnConfiguracion = null;
            PresentarRangosTarifa(new List<RangoTarifaFSLBO>());
            PresentarAniosConfigurados(new List<TarifaFSLBO>());
        }
        /// <summary>
        /// Limpia los datos de la Sesion
        /// </summary>
        public void LimpiarSesion()
        {
            if(Session["LineasContratoSession"] != null)
                Session.Remove("LineasContratoFSL");
            if(Session["ContratoFSLAnterior"] != null)
                Session.Remove("ContratoFSLAnterior");
            if(Session["ObservacionesUnidad"] != null)
                Session.Remove("ObservacionesUnidad");
            if(Session["UnidadesLiberar"] != null)
                Session.Remove("UnidadesLiberar");
            if(Session["UnidadesCambio"] != null)
                Session.Remove("UnidadesCambio");
            if(Session["ListaSucursalesPermitidas"] != null) 
                Session.Remove("ListaSucursalesPermitidas");
            if(Session["MonedasDisponibles"] != null)
                Session.Remove("MonedasDisponibles");
            if(Session["LineaContratoEnEdicion"] != null) 
                Session.Remove("LineaContratoEnEdicion");
            if(Session["TarifasEnConfiguracion"] != null)
                Session.Remove("TarifasEnConfiguracion");
        }
        /// <summary>
        /// Obtiene el objeto enviado por la Interfaz Anterior
        /// </summary>
        /// <returns>Objeto obtenido del Paquete de Navegacion</returns>
        public object ObtenerPaqueteNavegacion()
        {
            ContratoFSLBO contratoFsl = null;
            if (Session["ContratoFSLEditar"] != null)
            {
                contratoFsl = (ContratoFSLBO)Session["ContratoFSLEditar"];
                Session.Remove("ContratoFSLEditar");
            }
            return contratoFsl;
        }
        /// <summary>
        /// Envia un objeto hacia otra interfaz
        /// </summary>
        /// <param name="paquete">Objeto que sera enviado</param>
        public void EstablecerPaqueteNavegacion(Object paquete)
        {
            if(paquete != null)
                Session["ContratoFSLBO"] = paquete;
        }
        /// <summary>
        /// Cambia la Interfaz que se esta presentando
        /// </summary>
        /// <param name="interfaz">Nombre usado para el cambio de interfaz</param>
        public void CambiarInterfaz(string interfaz)
        {
            switch (interfaz)
            {
                case "Linea":
                    mvCambioUnidades.SetActiveView(vwLineasContrato);
                    break;
                case "General":
                    mvCambioUnidades.SetActiveView(vwInformacionGeneral);
                    break;
            }
        }
        #region Metodos para Bloqueo de controles
        /// <summary>
        /// Determina si esta bloqueado el boton para cancelar cambios
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirCancelarCambios(bool permitir)
        {
            btnCancelar.Enabled = permitir;
        }
        /// <summary>
        /// Determina si el boton para cancelar cambios
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirGuardarCambios(bool permitir)
        {
            btnGuardar.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede visualizar el detalle de las lineas
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirVerDetalleLineas(bool permitir)
        {
            var lista = grdLineasContrato.Rows;
            foreach (GridViewRow row in lista)
            {
                var botonDetalle = row.FindControl("ibtnDetalles") as ImageButton;
                var botonEliminar = row.FindControl("ibtEliminar") as ImageButton;
                if (botonDetalle != null)
                    botonDetalle.Enabled = permitir;
                if (botonEliminar != null)
                    botonEliminar.Enabled = permitir;
            }
        }
        /// <summary>
        /// Determina si el Numero de Contrato puede ser editable
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirContrato(bool permitir)
        {
            this.txtNumeroContrato.Enabled = permitir;
        }
        /// <summary>
        /// Determina si la Fecha de Inicio del Contrato puede ser editable
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirFechaContrato(bool permitir)
        {
            this.txtFechaInicioContrato.Enabled = permitir;
        }
        /// <summary>
        /// Determina si el Nombre del Cliente puede ser editable
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirNombreCliente(bool permitir)
        {
            this.txtClienteNombre.Enabled = permitir;
        }
        /// <summary>
        /// Determina si el Boton para Agregar Unidad esta Disponible
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirAgregarUnidad(bool permitir)
        {
            this.btnAgregarUnidad.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede buscar el VIN de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirNumeroSerie(bool permitir)
        {
            this.txtNumeroSerie.Enabled = this.ibtnBuscarUnidad.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el VIN de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirVinUnidad(bool permitir)
        {
            txtVinUnidad.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el Numero Economico de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirNumeroEconomico(bool permitir)
        {
            txtNumeroEconomico.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el Nombre del Modelo de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirNombreModelo(bool permitir)
        {
            txtNombreModelo.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el Año de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirAnio(bool permitir)
        {
            txtAnio.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el PBC Maximo Recomendado de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirPBC(bool permitir)
        {
            txtPBC.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el PBV Maximo Recomendado de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirPBV(bool permitir)
        {
            txtPBV.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el Kilometraje Inicial de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirKmInicial(bool permitir)
        {
            txtKmInicial.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar la Poliza de Seguro de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirPolizaSeguro(bool permitir)
        {
            txtPolizaSeguro.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar la opcion de compra de una unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirOpcionCompra(bool permitir)
        {
            cbOpcionCompra.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar la moneda de compra de la unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirMonedaCompra(bool permitir)
        {
            ddlMonedaCompra.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar el importe de compra de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirImporteCompra(bool permitir)
        {
            txtImporteCompra.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede editar Las Tarifas de las Unidades y Equipos Aliados
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirEditarTarifa(bool permitir)
        {
            btnConfigurarTarifasUnidad.Enabled = permitir;
        }
        /// <summary>
        /// Determina si se puede agregar el equipo aliado a la Tabla
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirAgregarEquipoAliado(bool permitir)
        {
            btnAgregarEquipoAliado.Enabled = permitir;
        }
        /// <summary>
        /// Determina si puede usar el check para no agregar tarifa adicional
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirSeleccionarSinTarifaAdiciona(bool permitir)
        {
            trAplicarCargos.Visible = permitir;
        }
        /// <summary>
        /// Activa o bloquea toda la seccion de Tarifas
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirTarifasAdicionales(bool permitir)
        {
            ddlTipoCargo.Enabled = permitir;
            ddlAniosContrato.Enabled = permitir;
            ddlFrecuencia.Enabled = permitir;
            btnGuardarAnio.Enabled = permitir;
            txtKmHrsLibres.Enabled = permitir;
            txtKmHrMinimo.Enabled = permitir;
            txtRangoInicial.Enabled = permitir;
            ddlRangoTiempo.Enabled = permitir;
            txtRangoFinal.Enabled = permitir;
            txtCargo.Enabled = permitir;
            btnAgregar.Enabled = permitir;
            if (!permitir)
            {
                if(ddlAniosContrato.Items.Count > 0)
                    ddlAniosContrato.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Inactiva o Activa la seleccion de KM u Horas
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirTipoCargo(bool permitir)
        {
            ddlTipoCargo.Enabled = permitir;
        }
        /// <summary>
        /// Permite Activar o desactivar el Control de seleccion de Año de la Tarifa
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        public void PermitirSeleccionarAnio(bool permitir)
        {
            if(permitir)
                ddlAniosContrato.SelectedIndex = 0;
            ddlAniosContrato.Enabled = permitir;
        }
        /// <summary>
        /// Permite habilitar o no los controles de Configuracion de la Tarifa
        /// </summary>
        /// <param name="permitir">Bool que determina si se habilitan los controles</param>
        public void PermitirConfiguracionTarifa(bool permitir)
        {
            if (!permitir)
                ddlRangoTiempo.SelectedIndex = 0;

            btnAgregar.Enabled = permitir;
            btnGuardarAnio.Enabled = permitir;
            ddlFrecuencia.Enabled = permitir;
            txtKmHrsLibres.Enabled = permitir;
            txtKmHrMinimo.Enabled = permitir;
            txtRangoInicial.Enabled = permitir;
            ddlRangoTiempo.Enabled = permitir;
            txtRangoFinal.Enabled = permitir;
            txtCargo.Enabled = permitir;
        }
        #endregion
        /// <summary>
        /// Presenta las lineas de contrato
        /// </summary>
        /// <param name="lineasContrato">Lineas de Contrato con los datos de unidades y equipos aliados</param>
        public void PresentarLineasContrato(List<LineaContratoFSLBO> lineasContrato)
        {
            if(lineasContrato == null)
                lineasContrato = new List<LineaContratoFSLBO>();

            grdLineasContrato.DataSource = lineasContrato;
            grdLineasContrato.DataBind();
        }
        /// <summary>
        /// Presentar Tipos de Cotizaciones
        /// </summary>
        /// <param name="listaCotizaciones">Lista con las Cotizaciones</param>
        public void PresentarTipoCotizacion(Dictionary<string, string> listaCotizaciones)
        {
            if(listaCotizaciones == null)
                listaCotizaciones = new Dictionary<string, string>();

            ddlTipoCotizacion.DataSource = listaCotizaciones;
            ddlTipoCotizacion.DataTextField = "value";
            ddlTipoCotizacion.DataValueField = "key";
            ddlTipoCotizacion.DataBind();

            ddlTipoCotizacion.Items.Insert(0, new ListItem("SELECCIONE", "-1"));
        }
        /// <summary>
        /// Presenta las Monedas disponibles para la opcion de compra
        /// </summary>
        /// <param name="monedas">Lista de Monedas Disponibles</param>
        public void PresentarMonedasDisponibles(Dictionary<String, string> monedas)
        {
            if(monedas == null)
                monedas = new Dictionary<string, string>();

            ddlMonedaCompra.DataSource = monedas;
            ddlMonedaCompra.DataValueField = "key";
            ddlMonedaCompra.DataTextField = "value";
            ddlMonedaCompra.DataBind();

            ddlMonedaCompra.Items.Insert(0, new ListItem("SELECCIONE", "-1"));
        }
        /// <summary>
        /// Presenta los cargos por cada equipo aliado
        /// </summary>
        /// <param name="cargosEquiposAliados">Lista de Cargos de los equipos aliados</param>
        public void PresentarCargosEquiposAliados(List<CargoAdicionalEquipoAliadoBO> cargosEquiposAliados)
        {
            if(cargosEquiposAliados == null)
                cargosEquiposAliados = new List<CargoAdicionalEquipoAliadoBO>();

            grvEquiposAliados.DataSource = cargosEquiposAliados;
            grvEquiposAliados.DataBind();
        }

        public void PresentarRangosTarifa(List<RangoTarifaFSLBO> listaRangos)
        {
            if(listaRangos == null)
                listaRangos = new List<RangoTarifaFSLBO>();

            grvRangosConfigurados.DataSource = listaRangos;
            grvRangosConfigurados.DataBind();
        }

        public void PresentarListaAnios(Dictionary<string, string> aniosAConfigurar)
        {
            if(aniosAConfigurar == null)
                aniosAConfigurar = new Dictionary<string, string>();

            ddlAniosContrato.DataSource = aniosAConfigurar;
            ddlAniosContrato.DataTextField = "value";
            ddlAniosContrato.DataValueField = "key";
            ddlAniosContrato.DataBind();

            ddlAniosContrato.Items.Insert(0, new ListItem("SELECCIONE", "-1"));
            ddlAniosContrato.SelectedIndex = 0;
        }

        public void PresentarAniosConfigurados(List<TarifaFSLBO> listaTarifas)
        {
            if(listaTarifas == null)
                listaTarifas = new List<TarifaFSLBO>();

            grvAniosConfigurados.DataSource = listaTarifas;
            grvAniosConfigurados.DataBind();
        }

        public void PresentarFrecuencias(Dictionary<string, string> listaFrecuencias)
        {
            ddlFrecuencia.Items.Clear();
            
            if(listaFrecuencias == null)
                listaFrecuencias = new Dictionary<string, string>();

            ddlFrecuencia.DataSource = listaFrecuencias;
            ddlFrecuencia.DataTextField = "value";
            ddlFrecuencia.DataValueField = "key";
            ddlFrecuencia.DataBind();

            ddlFrecuencia.Items.Insert(0, new ListItem("SELECCIONE", "-1"));
        }
        #endregion
        #region Eventos
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.Actualizar();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Actualizar las Unidades del Contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardar_OnClick: " + ex.Message);
            }
        }
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.LimpiarSesion();
                this.Response.Redirect(this.ResolveUrl("~/Contratos.FSL.UI/DetalleContratoFSLUI.aspx"));
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cancelar las Modificaciones de las Unidades del Contrato.", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelar_OnClick: " + ex.Message);
            }
        }
        protected void btnAgregarUnidad_OnClick(object sender, EventArgs e)
        {
            try
            {
                if(String.IsNullOrEmpty(NumeroSerie) || LineaUnidadId == null || LineaEquipoId == null)
                    throw new Exception("No hay una Unidad que agregar al Contrato");

                presentador.AgregarUnidadBuscadorAContrato();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Intentar Agregar una Unidad al Contrato", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregarUnidad_OnClick: " + ex.Message);
            }
        }
        protected void btnAgregarEquipoAliado_OnClick(object sender, EventArgs e)
        {
            try
            {
                if(String.IsNullOrEmpty(NumeroSerieEquipoAliado) || LineaEquipoId == null)
                    throw new Exception("No hay un Equipo Aliado que agregar al Contrato");

                presentador.AgregarEquipoAliadoBuscadorAContrato();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Intentar Agregar un Equipo Aliado a la Unidad", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregarEquipoAliado_OnClick: " + ex.Message);
            }
        }
        protected void grdLineasContrato_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType == DataControlRowType.DataRow)
                {
                    var linea = ((LineaContratoFSLBO)e.Row.DataItem);
                    var label = e.Row.FindControl("lblVIN") as Label;

                    var button = e.Row.FindControl("ibtnDetalles") as ImageButton;

                    // Numero de Serie
                    if(label != null)
                    {
                        if(linea.Equipo != null && linea.Equipo.NumeroSerie != null) label.Text = linea.Equipo.NumeroSerie;
                        else label.Text = string.Empty;
                    }

                    // Modelo
                    label = e.Row.FindControl("lblModelo") as Label;
                    if(label != null)
                    {
                        if(linea.Equipo != null && linea.Equipo.Modelo != null) label.Text = linea.Equipo.Modelo.Nombre;
                        else label.Text = string.Empty;
                    }

                    //Km Estimado Anual
                    label = e.Row.FindControl("lblKmEstimadoAnual") as Label;
                    if(label != null)
                    {
                        label.Text = linea.KmEstimadoAnual != null ? string.Format("{0:#,##0}", linea.KmEstimadoAnual) : string.Empty;
                    }

                    //Deposito en Garantia
                    label = e.Row.FindControl("lblDepositoGarantia") as Label;
                    if(label != null)
                    {
                        label.Text = linea.DepositoGarantia != null ? string.Format("{0:c4}", linea.DepositoGarantia.Value) : string.Empty;
                    }

                    //Comision por Apertura
                    label = e.Row.FindControl("lblComisionApertura") as Label;
                    if(label != null)
                    {
                        label.Text = linea.ComisionApertura != null ? string.Format("{0:c4}", linea.ComisionApertura.Value) : string.Empty;
                    }

                    //Cargo Fijo Mensual
                    label = e.Row.FindControl("lblCargoFijoMes") as Label;
                    if(label != null)
                    {
                        label.Text = linea.CargoFijoMensual != null ? string.Format("{0:c4}", linea.CargoFijoMensual.Value) : string.Empty;
                    }

                    //Cargo por KM
                    ImageButton img = e.Row.FindControl("ibtnCargoKM") as ImageButton;
                    if(img != null)
                    {
                        CargosAdicionalesFSLBO cargo = linea.Cobrable as CargosAdicionalesFSLBO;
                        if(cargo != null)
                        {
                            string cargoKm = string.Empty;
                            foreach(TarifaFSLBO tarifa in cargo.Tarifas)
                            {
                                cargoKm +=
                                    "<tr>" +
                                        "<td>" + (!string.IsNullOrEmpty(tarifa.Año.ToString()) ? tarifa.Año.ToString() : "N/A") + "</td>" +
                                        "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoKm) + "</td>" + 
                                        "<td> Despues de: " + tarifa.KmLibres.ToString() + "</td>" +
                                        "<td>" + tarifa.Frecuencia + "</td>" +
                                    "</tr>";
                            }
                            cargoKm = "<table class=\"Grid\" width=\"100%\"><tr>" +
                                      "<th>Año</th>" +
                                      "<th colspan=\"3\">Cargo por KM</th>" +
                                      "</tr>" + cargoKm + "</table>";

                            img.OnClientClick = "$('#" + img.ClientID + "').click(function() { $('#" + ClientID + "_divCargos').html(''); $('#" + ClientID + "_divCargos').html('" + cargoKm + "'); $('#" + ClientID + "_divCargos').dialog({ modal:true, autoOpen:true, width: 800, title: 'Cargo por KM' }); }); return false;";
                        }
                    }

                    //Cargo por HR
                    img = e.Row.FindControl("ibtnCargoHR") as ImageButton;
                    if(img != null)
                    {
                        CargosAdicionalesFSLBO cargo = linea.Cobrable as CargosAdicionalesFSLBO;
                        if(cargo != null)
                        {
                            string cargoHr = string.Empty;
                            foreach(TarifaFSLBO tarifa in cargo.Tarifas)
                            {
                                cargoHr +=
                                    "<tr>" +
                                            "<td>" + (!string.IsNullOrEmpty(tarifa.Año.ToString()) ? tarifa.Año.ToString() : "N/A") + "</td>" +
                                            "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoHr) + "</td>" +
                                            "<td> Despues de: " + tarifa.HrLibres.ToString() + "</td>" +
                                            "<td>" + tarifa.Frecuencia + "</td>" +
                                    "</tr>";
                            }
                            cargoHr = "<table class=\"Grid\" width=\"100%\"><tr>" +
                                      "<th>Año</td>" +
                                      "<th colspan=\"3\">Cargo por Hora</th>" +
                                      "</tr>" + cargoHr + "</table>";

                            img.OnClientClick = "$('#" + img.ClientID + "').click(function() { $('#" + ClientID + "_divCargos').html(''); $('#" + ClientID + "_divCargos').html('" + cargoHr + "'); $('#" + ClientID + "_divCargos').dialog({ modal:true, autoOpen:true, width: 800, title: 'Cargo por Hora' }); }); return false;";
                        }
                    }

                    //Cargo por EA
                    img = e.Row.FindControl("ibtnCargoEA") as ImageButton;
                    if(img != null)
                    {
                        string cargos = string.Empty;
                        var cargo = linea.Cobrable as CargosAdicionalesFSLBO;
                        if(cargo != null)
                        {
                            foreach(CargoAdicionalEquipoAliadoBO caea in cargo.CargoAdicionalEquiposAliados)
                            {
                                cargos += (!string.IsNullOrEmpty(cargos))
                                              ? "<tr><td colspan=\"7\"><br/></td></tr>"//modifque el colspan de 5 a 7
                                              : string.Empty;
                                string cargoHr = string.Empty;
                                if(caea.Tarifas != null)
                                {
                                    foreach(TarifaFSLBO tarifa in caea.Tarifas)
                                    {
                                        cargoHr +=
                                            "<tr>" +
                                            "<td>" +
                                            (!string.IsNullOrEmpty(tarifa.Año.ToString()) ? tarifa.Año.ToString() : "N/A") +
                                            "</td>" +
                                            "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoKm) + "</td>" +
                                            "<td> Despues de: " + tarifa.KmLibres.ToString() + "</td>" +
                                            "<td>" + tarifa.Frecuencia + "</td>" +
                                            "<td>" + string.Format("{0:c4}", tarifa.Rangos.FirstOrDefault().CargoHr) + "</td>" +
                                            "<td> Despues de: " + tarifa.HrLibres.ToString() + "</td>" +
                                            "<td>" + tarifa.Frecuencia + "</td>" +
                                            "</tr>";
                                    }
                                    cargoHr = "<tr>" +
                                              "<th>Año</td>" +
                                              "<th colspan=\"3\">Cargo por Km</th>" +
                                              "<th colspan=\"3\">Cargo por Hora</th>" +
                                          "</tr>" + cargoHr;
                                }
                                else
                                {
                                    cargoHr = "<tr>" +
                                              " <td colspan= \"7\">El equipo Aliado no cuenta con Tarifas</td>" +
                                              "</tr>";
                                }

                                cargos += "<tr>" +
                                                "<th colspan=\"7\"># VIN: " + caea.EquipoAliado.NumeroSerie + "</th>" +//modifque el colspan de 5 a 7
                                           "</tr>" + cargoHr;
                            }
                            cargos = "<table class=\"Grid\" width=\"100%\">" + cargos + "</table>";

                            img.OnClientClick = "$('#" + img.ClientID + "').click(function() { $('#" + ClientID + "_divCargos').html(''); $('#" + ClientID + "_divCargos').html('" + cargos + "'); $('#" + ClientID + "_divCargos').dialog({ modal:true, autoOpen:true, width: 800, title: 'Cargo por Equipos Aliados' }); }); return false;";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Se han encontrado Inconsistencias al presentar el detalle del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".grdLineasContrato_RowDataBound: " + ex.Message);
            }
        }
        protected void grdLineasContrato_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if(eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if(e.CommandArgument == null) return;
                int index = 0;
                if(!Int32.TryParse(e.CommandArgument.ToString(), out index)) return;

                LineaContratoFSLBO linea = LineasContrato[index];
                switch(eCommandNameUpper)
                {
                    case "CMDELIMINAR":
                        if(linea!= null)
                            presentador.EliminarLineaContrato(linea);
                        break;
                    case "CMDDETALLES":
                        {
                            if (linea != null)
                            {
                                presentador.PresentarLineaUnidad(linea);
                            }
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar/eliminar la unidad del contrato.", ETipoMensajeIU.ERROR, NombreClase + ".grdLineasContrato_RowCommand: " + ex.Message);
            }
        }
        protected void grdLineasContrato_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdLineasContrato.DataSource = LineasContrato;
                grdLineasContrato.PageIndex = e.NewPageIndex;
                grdLineasContrato.DataBind();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, NombreClase + ".grdLineasContrato_PageIndexChanging: " + ex.Message);
            }
        }
        protected void btnGuardarLinea_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarCambiosLinea();
                mvCambioUnidades.SetActiveView(vwInformacionGeneral);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Guardar los Cambios a la Linea", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardarLinea_OnClick: " + ex.Message);
            }
        }
        protected void btnCancelarLinea_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarLineaUnidad();
                mvCambioUnidades.SetActiveView(vwInformacionGeneral);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cancelar los Cambios a la Linea", ETipoMensajeIU.ERROR, NombreClase + ".btnCancelarLinea_OnClick: " + ex.Message);
            }
        }
        protected void ddlTipoCotizacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                presentador.CambiarTipoCotizacion(TipoCotizacion);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Intentar Cambiar el Tipo de Cotizacion", ETipoMensajeIU.ERROR, NombreClase + ".ddlTipoCotizacion_OnSelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnConfigurarTarifasUnidad_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.PresentarTarifa(ETipoEquipo.Unidad, LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO);
                mvCambioUnidades.SetActiveView(vwConfiguracionTarifa);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Intentar Agregar una Unidad al Contrato", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregarUnidad_OnClick: " + ex.Message);
            }
        }
        protected void cbOpcionCompra_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                presentador.CambiarOpcionCompra(cbOpcionCompra.Checked);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias Cambiar la opción de compra", ETipoMensajeIU.ERROR, NombreClase + ".cbOpcionCompra_OnCheckedChanged: " + ex.Message);
            }
        }
        protected void grvEquiposAliados_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var cargoEquipoAliado = ((CargoAdicionalEquipoAliadoBO)e.Row.DataItem);
                    var labelVin = e.Row.FindControl("lblVIN") as Label;
                    var labelModelo = e.Row.FindControl("lblModelo") as Label;
                    var checkTarifa = e.Row.FindControl("cbTarifaCapturada") as CheckBox;
                    var btnTarifa = e.Row.FindControl("btnConfigurarTarifasEquipoAliado") as Button;

                    if (cargoEquipoAliado.EquipoAliado != null)
                    {
                        if (!String.IsNullOrEmpty(cargoEquipoAliado.EquipoAliado.NumeroSerie))
                        {
                            if (labelVin != null)
                                labelVin.Text = cargoEquipoAliado.EquipoAliado.NumeroSerie.ToUpper();
                        }
                        if (cargoEquipoAliado.EquipoAliado.Modelo != null && !String.IsNullOrEmpty(cargoEquipoAliado.EquipoAliado.Modelo.Nombre))
                        {
                            if (labelModelo != null)
                                labelModelo.Text = cargoEquipoAliado.EquipoAliado.Modelo.Nombre.ToUpper();
                        }
                    }
                    if (checkTarifa != null)
                    {
                        checkTarifa.Checked = (cargoEquipoAliado.CobrableID != null && cargoEquipoAliado.AplicaCargosAdicionales != null) || (cargoEquipoAliado.CobrableID == null && cargoEquipoAliado.AplicaCargosAdicionales != null);
                        checkTarifa.Enabled = false;
                    }

                    if (btnTarifa != null)
                    {
                        btnTarifa.Enabled = TipoCotizacion != null;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(NombreClase + ".grvEquiposAliados_OnRowDataBound: Inconsistencias al Intentar presentar los equipos aliados", ex.InnerException);
            }
        }
        protected void grvEquiposAliados_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if(eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if(e.CommandArgument == null) return;
                int index = 0;
                if(!Int32.TryParse(e.CommandArgument.ToString(), out index)) return;

                var cargoAdicional = LineaContratoEnEdicion.Cobrable as CargosAdicionalesFSLBO;
                if(cargoAdicional == null) throw new Exception("No se encontro la Linea de Contrato que se esta Editando");
                var cargoEquipoAliado = cargoAdicional.CargoAdicionalEquiposAliados.Find(cargo => cargo.EquipoAliado.EquipoAliadoID == index);

                switch(eCommandNameUpper)
                {
                    case "CMDELIMINAR":
                        presentador.EliminarEquipoAliado(cargoEquipoAliado);
                        break;
                    case "CMDEDITARTARIFA":
                        if (cargoEquipoAliado != null)
                        {
                            presentador.PresentarTarifa(ETipoEquipo.EquipoAliado, cargoEquipoAliado);
                            mvCambioUnidades.SetActiveView(vwConfiguracionTarifa);
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Ejecutar la Acción", ETipoMensajeIU.ERROR, NombreClase + ".grvEquiposAliados_OnRowCommand: " + ex.Message);
            }
        }
        protected void cbNoAplicarCargos_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                presentador.SinTarifasAdicionales(cbNoAplicarCargos.Checked);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias al Quitar las Opciones de Tarifas Adicionales", ETipoMensajeIU.ERROR, NombreClase + ".cbNoAplicarCargos_OnCheckedChanged: " + ex.Message);
            }
        }
        protected void ddlTipoCargo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(CargoPorKm == null)
                    ddlRangoTiempo.SelectedIndex = 0;
                presentador.CambiarTipoCargo(CargoPorKm);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cambiar el tipo de Cargo", ETipoMensajeIU.ERROR, NombreClase + ".ddlTipoCargo_OnSelectedIndexChanged: " + ex.Message);
            }
        }
        protected void ddlAniosContrato_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                presentador.PresentarDatosAnioTarifa(AnioTarifa);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cambiar el Año a Configurar", ETipoMensajeIU.ERROR, NombreClase + ".ddlAniosContrato_OnSelectedIndexChanged: " + ex.Message);
            }
        }
        protected void ddlRangoTiempo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRangoTiempo.SelectedIndex == 0)
                {
                    txtRangoFinal.Text = "";
                    txtRangoFinal.Enabled = true;
                }
                if (ddlRangoTiempo.SelectedIndex == 1)
                {
                    txtRangoFinal.Text = "";
                    txtRangoFinal.Enabled = false;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cambiar el Rango de la Tarifa", ETipoMensajeIU.ERROR, NombreClase + ".ddlRangoTiempo_OnSelectedIndexChanged: " + ex.Message);
            }
        }
        protected void btnGuardarAnio_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarConfiguracionAnioTarifa();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al intentar guardar la configuracion del Año.", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardarAnio_OnClick: " + ex.Message);
            }
        }
        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.AgregarRangoTarifa();
                ddlRangoTiempo.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al intentar agregar el rango", ETipoMensajeIU.ERROR, NombreClase + ".btnAgregar_OnClick: " + ex.Message);
            }
        }
        protected void grvRangosConfigurados_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                try
                {
                    string eCommandNameUpper = e.CommandName.ToUpper();
                    if(eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                    if(e.CommandArgument == null) return;
                    int index = 0;
                    if(!Int32.TryParse(e.CommandArgument.ToString(), out index)) return;
                    
                    switch(eCommandNameUpper)
                    {
                        case "CMDELIMINAR":
                            presentador.EliminarRangoTarifa(index);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    MostrarMensaje("Inconsistencias al Ejecutar la Acción", ETipoMensajeIU.ERROR, NombreClase + ".grvRangosConfigurados_OnRowCommand: " + ex.Message);
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al intentar eliminar el rango", ETipoMensajeIU.ERROR, NombreClase + ".grvRangosConfigurados_OnRowCommand: " + ex.Message);
            }
        }
        protected void btnCacelarTarifa_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarInterfazTarifas();
                mvCambioUnidades.SetActiveView(vwLineasContrato);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Cancelar los Cambios a la Tarifa", ETipoMensajeIU.ERROR, NombreClase + ".btnCacelarTarifa_OnClick: " + ex.Message);
            }
        }
        protected void btnGuardarTarifa_OnClick(object sender, EventArgs e)
        {
            try
            {
                presentador.GuardarConfiguracionTarifas();
                LimpiarInterfazTarifas();
                mvCambioUnidades.SetActiveView(vwLineasContrato);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Guardar los cambios de la Tarifa", ETipoMensajeIU.ERROR, NombreClase + ".btnGuardarTarifa_OnClick: " + ex.Message);
            }
        }
        #region Eventos Buscador
        protected void txtNumeroSerie_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = NumeroSerie;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.UnidadIdealease);

                NumeroSerie = numeroSerie;
                if(!String.IsNullOrEmpty(NumeroSerie))
                    EjecutaBuscador("UnidadIdealease", ECatalogoBuscador.UnidadIdealease);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtNumeroSerie_TextChanged: " + ex.Message);
            }
        }
        protected void ibtnBuscarUnidad_OnClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                EjecutaBuscador("UnidadIdealease&hidden=0", ECatalogoBuscador.UnidadIdealease);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarUnidad_Click: " + ex.Message);
            }
        }
        protected void txtVinEquipoAliado_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                string numeroSerie = NumeroSerieEquipoAliado;
                Session_BOSelecto = null;

                DesplegarBOSelecto(ECatalogoBuscador.EquipoAliado);

                NumeroSerieEquipoAliado = numeroSerie;
                if(!String.IsNullOrEmpty(NumeroSerieEquipoAliado))
                    EjecutaBuscador("EquipoAliado", ECatalogoBuscador.EquipoAliado);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtVinEquipoAliado_OnTextChanged: " + ex.Message);
            }
        }
        protected void ibtBuscarEquipoAliado_OnClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                EjecutaBuscador("EquipoAliado&hidden=0", ECatalogoBuscador.EquipoAliado);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencia al buscar una Unidad", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtBuscarEquipoAliado_OnClick: " + ex.Message);
            }
        }
        protected void txtClaveProductoServicio_TextChanged(object sender, EventArgs e) {
            try {
                string clvProducto = this.ClaveProductoServicio;
                Session_BOSelecto = null;
                DesplegarBOSelecto(ECatalogoBuscador.ProductoServicio);
                this.ClaveProductoServicio = clvProducto;
                if (this.ClaveProductoServicio != null) 
                    EjecutaBuscador("ProductoServicio", ECatalogoBuscador.ProductoServicio);

                this.ProductoServicioId = null;
                this.ClaveProductoServicio = null;
                this.DescripcionProductoServicio = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar un Producto", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".txtClaveProductoServicio: " + ex.Message);
            }
        }
        protected void ibtnBuscarProductoServicio_Click(object sender, ImageClickEventArgs e) {
            try {
                EjecutaBuscador("ProductoServicio&hidden=0", ECatalogoBuscador.ProductoServicio);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencia al buscar el producto", ETipoMensajeIU.ADVERTENCIA, NombreClase + ".ibtnBuscarProductoServicio_Click: " + ex.Message);
            }
        }
        protected void btnResult_OnClick(object sender, EventArgs e)
        {
            try
            {
                switch (ViewState_Catalogo)
                {
                    case ECatalogoBuscador.UnidadIdealease:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;

                    case ECatalogoBuscador.EquipoAliado:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                    case ECatalogoBuscador.ProductoServicio:
                        DesplegarBOSelecto(ViewState_Catalogo);
                        break;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, NombreClase + ".btnResult_Click: " + ex.Message);
            }
        }
        #endregion
        #endregion
    }
}