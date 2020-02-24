// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al CU022 - Consultar Contratos Full Service Leasing
// Satisface al CU025 - Catalogo Tarifas Comerciales Renta FSL 
// Satisface al CU023 - Editar Contrato Full Service Leasing
// Mejoras Durante Staffing - Cobro de Rangos de Kms /Hrs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.PRE;
using BPMO.SDNI.Contratos.FSL.VIS;
using BPMO.SDNI.MapaSitio.UI;

namespace BPMO.SDNI.Contratos.FSL.UI
{
    public partial class ucTarifasFSLUI : UserControl, IucTarifasFSLVIS
    {
        #region Atributos

        private ucTarifasFSLPRE presentador;

        #endregion

        #region Propiedades

        /// <summary>
        /// Plazo del Contrato en Años
        /// </summary>
        public int? Plazo
        {
            get { return Session["PlazoControl"] as int?; }
            set { Session["PlazoControl"] = value; }
        }

        /// <summary>
        /// Tipo de Cotizacion
        /// </summary>
        public ETipoCotizacion? TipoCotizacion
        {
            get { return Session["TipoCotizacionControl"] as ETipoCotizacion?; }
            set { Session["TipoCotizacionControl"] = value; }
        }

        /// <summary>
        /// Identificador del la lista de Tarifas
        /// </summary>
        public int? Identificador
        {
            get
            {
                if (hdnIdentificador.Value.Trim() == "")
                    return null;
                return Convert.ToInt32(hdnIdentificador.Value);
            }
            set 
            {
                hdnIdentificador.Value = value.ToString();
            }
        }

        /// <summary>
        /// Lista de Tarifas
        /// </summary>
        public List<TarifaFSLBO> Tarifas
        {
            get
            {
                if (Session["Tarifas" + Identificador] == null)
                    return new List<TarifaFSLBO>();
                return (List<TarifaFSLBO>)Session["Tarifas" + Identificador];
            }
            set 
            {
                Session["Tarifas" + Identificador] = value;
            }
        }

        /// <summary>
        /// Lista de tarifas anterios
        /// </summary>
        public List<TarifaFSLBO> UltimasTarifas
        {
            get
            {
                if (Session["UltimasTarifas" + Identificador] == null)
                    return new List<TarifaFSLBO>();
                return (List<TarifaFSLBO>)Session["UltimasTarifas" + Identificador];
            }
            set { Session["UltimasTarifas" + Identificador] = value; }
        }

        public List<RangoTarifaFSLBO> RangosConfigurados
        {
            get
            {
                if(Session["RangosConfigurados"] == null)
                    return new List<RangoTarifaFSLBO>();
                return (List<RangoTarifaFSLBO>) Session["RangosConfigurados"];
            }
            set
            {
                Session["RangosConfigurados"] = value;
            }
        }

        public Boolean? TarifaUnidad { get; set; }

        public Boolean? CargoKm
        {
            get
            {
                var cargo = this.hdnCargoKm.Value;
                if (String.IsNullOrEmpty(cargo)) return null;
                if (cargo == "0") return false;
                if (cargo == "1") return true;

                return null;
            }
            set
            {
                if (value != null)
                {
                    if (value == true)
                        this.hdnCargoKm.Value = "1";
                    if(value == false)
                        this.hdnCargoKm.Value = "0";
                }
                else
                {
                    this.hdnCargoKm.Value = "";
                }
            }
        }

        private Boolean? ModoConsulta
        {
            get
            {
                return hdnModoConsulta.Value == "1";
            }
            set
            {
                if (value != null)
                {
                    hdnModoConsulta.Value = value.Value ? "1" : "0";
                }
            }
        }

        public Boolean? EsModoConsulta
        {
            get { return this.ModoConsulta; }
        }

        public Boolean? SinTarifas
        {
            get
            {
                if (!String.IsNullOrEmpty(hdnSinTarifas.Value))
                {
                    return hdnSinTarifas.Value == "1";
                }
                return null;
            }
            set
            {
                if (value != null)
                    hdnSinTarifas.Value = value.Value ? "1" : "0";
                else
                    hdnSinTarifas.Value = "0";
            }
        }

        public ITarifasEquipoAliadoVIS VistaMensaje { get; set; }

        #endregion

        #region Metodos

        public void LimpiarSesion()
        {
            if (Session["Tarifas" + Identificador] != null)
                Session.Remove("Tarifas" + Identificador);
            if (Session["UltimasTarifas" + Identificador] != null)
                Session.Remove("UltimasTarifas" + Identificador);
            if (Session["PlazoControl"] != null)
                Session.Remove("PlazoControl");
            if (Session["TipoCotizacionControl"] != null)
                Session.Remove("TipoCotizacionControl");
        }
        
        /// <summary>
        /// Obtiene la lista de tarifas obtenidas a partir de los datos capturados
        /// </summary>
        /// <returns>Lista que contiene las tarifas capturadas por el usuario</returns>
        public List<TarifaFSLBO> ObtenerTarifas()
        {
            return InterfazUsuarioADatos();
        }

        /// <summary>
        /// Obtiene la lista anterior de tarifas
        /// </summary>
        /// <returns>Lista que contiene las tarifas anteriores</returns>
        public List<TarifaFSLBO> ObtenerUltimasTarifas()
        {
            if (UltimasTarifas.Count == 0) return null; 
            return UltimasTarifas;
        }
        /// <summary>
        /// Inicializa el control y despliega lo necesario para poder realizar la captura de las tarifas
        /// </summary>
        /// <param name="plazo">Cantidad de meses que durará</param>
        /// <param name="tipoCotizacion">El tipo de Cotización para el cual se capturarán las tarifas</param>
        /// <param name="tarifaUnidad">Indica si la Tarfia es para Unidades o Equipos Aliados</param>
        public void InicializarControl(int? plazo, ETipoCotizacion? tipoCotizacion, bool? tarifaUnidad)
        {
            LimpiarSesion();
            presentador = new ucTarifasFSLPRE(this);
            presentador.Inicializar(plazo, tipoCotizacion, tarifaUnidad);
        }

        public void InicializarControl(List<TarifaFSLBO> tarifas, int? identificador)
        {
            presentador = new ucTarifasFSLPRE(this);
            presentador.Inicializar(tarifas,identificador);
        }

        public void Inicializar(int? plazo, ETipoCotizacion? tipoCotizacion, ETipoEquipo? tipoEquipo, List<TarifaFSLBO> tarifas, int? identificador, bool? permitirModificar)
        {
            presentador = new ucTarifasFSLPRE(this);
            this.presentador.Inicializar(plazo, tipoCotizacion, tipoEquipo,tarifas, identificador, permitirModificar);
        }

        /// <summary>
        /// Inactiva o Activa el Modo de Consulta, Inactivo por Default
        /// </summary>
        /// <param name="establecer">Bool que determina si se establecer el modo de Consulta</param>
        public void EstablecerModoConsulta(bool establecer, bool? sinTarifas = null)
        {
            this.ModoConsulta = establecer;
            this.PermitirAnio(sinTarifas != null ? sinTarifas.Value : true);
        }

        public void EstablecerModoEditar(ETipoEquipo tipoEquipo)
        {
            this.ModoConsulta = false;
            PermitirAgregarRangos(true);
            PermitirAnio(true);
            PermitirFrecuencia(true);
            PermitirGuardarAnio(true);
            PermitirKmsHrsLibres(true);
            PermitirKmHrMinima(true);
            if(tipoEquipo == ETipoEquipo.EquipoAliado)
                PermitirTipoCargo(true);
        }

        public void EstablecerSinTarifas()
        {
            if(presentador == null)
                presentador = new ucTarifasFSLPRE(this);
            PermitirTipoCargo(false);
            ddlTipoCargo.SelectedIndex = 0;
            PermitirModificar(false);
            Tarifas = new List<TarifaFSLBO>();
            RangosConfigurados = new List<RangoTarifaFSLBO>();
            PresentarTarifas(Tarifas);
            PresentarInformacionRangos(RangosConfigurados);
        }
        /// <summary>
        /// Convierte los datos capturados por el usuario en una lista de tarifas
        /// </summary>
        /// <returns>Lista que contiene las tarifas capturadas por el usuario</returns>
        private List<TarifaFSLBO> InterfazUsuarioADatos()
        {
            var tarifasTemp = new List<TarifaFSLBO>();
            tarifasTemp = Tarifas;

            return tarifasTemp;
        }

        public void EstablecerTitulo(string titulo)
        {
            //var lblTitulo = lvwTarifas.FindControl("lblTitulo") as Label;
            //if (lblTitulo != null) lblTitulo.Text = titulo.Trim().ToUpper();
        }
        /// <summary>
        /// Registra Script en el cliente
        /// </summary>
        /// <param name="key">Key del script</param>
        /// <param name="script">script a registrar</param>
        public void RegistrarScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
        }
        /// <summary>
        /// Presenta u oculta la seccion con el Tipo de Cargo KM/HR
        /// </summary>
        /// <param name="mostar">Bool que determina si se mostrara o no la seccion</param>
        public void MostarTipoCargo(bool mostar)
        {
            this.divTipoCargo.Visible = mostar;
        }
        /// <summary>
        /// Diccionario de Tipos de cargo que se presentarán
        /// </summary>
        /// <param name="listaTipoCargo">Contiene los tipo de cargo que se mostraran</param>
        public void EstablecerTipoCargo(Dictionary<String, String> listaTipoCargo)
        {
            ddlTipoCargo.DataSource = listaTipoCargo;
            ddlTipoCargo.DataValueField = "value";
            ddlTipoCargo.DataTextField = "key";
            ddlTipoCargo.DataBind();
        }

        public void EstablecerAnio(Dictionary<string, string> listaAnios)
        {
            if(listaAnios == null)
                listaAnios = new Dictionary<string, string>();

            ddlAniosContrato.DataSource = listaAnios;
            ddlAniosContrato.DataValueField = "key";
            ddlAniosContrato.DataTextField = "value";
            ddlAniosContrato.DataBind();

            ddlAniosContrato.Items.Insert(0, new ListItem("SELECCIONE","-1"));
        }

        public void EstablecerFrecuencias(Dictionary<string, string> listaFrecuencias)
        {
            if(listaFrecuencias == null)
                listaFrecuencias = new Dictionary<string, string>();

            ddlFrecuencia.DataSource = listaFrecuencias;
            ddlFrecuencia.DataValueField = "value";
            ddlFrecuencia.DataTextField = "key";
            ddlFrecuencia.DataBind();

            ddlFrecuencia.Items.Insert(0, new ListItem("SELECCIONE", "-1"));
        }

        public void EstablecerCargoPorKmEquipoAliado(bool? cargoPorKm)
        {
            this.PermitirTipoCargo(EsModoConsulta.Value ? false : true);
            this.ddlTipoCargo.SelectedValue = cargoPorKm != null ? cargoPorKm.Value ? "0" : "1" : "-1";
            this.CargoKm = cargoPorKm;
        }

        public void IniciarCapturaAnio(int? anioConfigurar)
        {
            ddlFrecuencia.SelectedIndex = 0;
            txtKmHrsLibres.Text = "";
            txtKmHrMinimo.Text = "";
            txtRangoInicial.Text = "";
            txtRangoFinal.Text = "";
            txtCargo.Text = "";
            ddlRangoTiempo.SelectedIndex = 0;

            if (anioConfigurar == null)
            {
                PermitirGuardarAnio(false);
                PermitirAgregarRangos(false);
                PermitirFrecuencia(false);
                PermitirKmsHrsLibres(false);
                PermitirKmHrMinima(false);
                PresentarInformacionRangos(new List<RangoTarifaFSLBO>());
                RangosConfigurados = null;
            }
            else
            {
                if (!ModoConsulta.Value)
                {
                    PermitirGuardarAnio(true);
                    PermitirAgregarRangos(true);
                    PermitirFrecuencia(true);
                    PermitirKmsHrsLibres(true);
                    PermitirKmHrMinima(true);
                }
                
                var tarifa = Tarifas[anioConfigurar.Value - 1];
                var listaRangos = tarifa.Rangos.Select(rango => new RangoTarifaFSLBO(rango)).ToList();

                ddlFrecuencia.SelectedValue = tarifa.Frecuencia != null ? ((Int32) tarifa.Frecuencia).ToString() : "-1";
                txtKmHrsLibres.Text = CargoKm.Value ? (tarifa.KmLibres != null ? tarifa.KmLibres.ToString() : "") : (tarifa.HrLibres != null ? tarifa.HrLibres.ToString() : "");
                txtKmHrMinimo.Text = tarifa.CantidadMinima != null ? tarifa.CantidadMinima.ToString() : "";

                RangosConfigurados = listaRangos;

                PresentarInformacionRangos(RangosConfigurados);
            }
        }

        public void PresentarInformacionRangos(List<RangoTarifaFSLBO> rangoTarifas)
        {
            grvRangosConfigurados.DataSource = rangoTarifas;
            grvRangosConfigurados.DataBind();
        }
        /// <summary>
        /// Agrega un rango a la lista de Rangos de una Tarifa
        /// </summary>
        private void AgregarRangoATarifa()
        {
            var rango = ObtenerRangoTarifaInterfaz();
            var año = Int32.Parse(ddlAniosContrato.SelectedItem.Value);

            var tarifaGuardada = Tarifas[año - 1];
            var tarifa = ObtenerTarifaInterfaz(tarifaGuardada);

            string error = "";
            error = ValidarRango(tarifa, RangosConfigurados, rango);
            if (!String.IsNullOrEmpty(error))
            {
                MostrarMensaje(error, ETipoMensajeIU.ADVERTENCIA);
                return;
            }
            RangosConfigurados.Add(rango);

            ddlRangoTiempo.SelectedIndex = 0;
            ddlRangoTiempo.Enabled = true;
            txtRangoInicial.Text = "";
            txtRangoFinal.Text = "";
            txtCargo.Text = "";

            this.PresentarInformacionRangos(RangosConfigurados);
        }

        private TarifaFSLBO ObtenerTarifaInterfaz(TarifaFSLBO tarifaFslBo)
        {
            TarifaFSLBO tarifa = tarifaFslBo != null ? new TarifaFSLBO(tarifaFslBo) : new TarifaFSLBO();

            tarifa.Frecuencia = this.ddlFrecuencia.SelectedIndex != 0 ? (EFrecuencia?)Int32.Parse(ddlFrecuencia.SelectedValue) : null;
            tarifa.CobraKm = CargoKm;
            if (CargoKm.Value)
                tarifa.KmLibres = !String.IsNullOrEmpty(txtKmHrsLibres.Text) ? (Int32?) Int32.Parse(txtKmHrsLibres.Text) : null;
            else
                tarifa.HrLibres = !String.IsNullOrEmpty(txtKmHrsLibres.Text) ? (Int32?)Int32.Parse(txtKmHrsLibres.Text) : null;
            tarifa.CantidadMinima = !String.IsNullOrEmpty(txtKmHrMinimo.Text) ? (Int32?) Int32.Parse(txtKmHrMinimo.Text) : null;

            return tarifa;
        }

        private RangoTarifaFSLBO ObtenerRangoTarifaInterfaz()
        {
            RangoTarifaFSLBO rango = new RangoTarifaFSLBO();

            if (CargoKm.Value)
            {
                rango.KmRangoInicial = !String.IsNullOrEmpty(txtRangoInicial.Text) ? (Int32?)Int32.Parse(txtRangoInicial.Text) : null;
                rango.KmRangoFinal = !String.IsNullOrEmpty(txtRangoFinal.Text) ? (Int32?)Int32.Parse(txtRangoFinal.Text) : null;
                rango.CargoKm = !String.IsNullOrEmpty(txtCargo.Text) ? (Decimal?)Decimal.Parse(txtCargo.Text) : null;
            }
            else
            {
                rango.HrRangoInicial = !String.IsNullOrEmpty(txtRangoInicial.Text) ? (Int32?)Int32.Parse(txtRangoInicial.Text) : null;
                rango.HrRangoFinal = !String.IsNullOrEmpty(txtRangoFinal.Text) ? (Int32?)Int32.Parse(txtRangoFinal.Text) : null;
                rango.CargoHr = !String.IsNullOrEmpty(txtCargo.Text) ? (Decimal?)Decimal.Parse(txtCargo.Text) : null;
            }

            return rango;
        }

        private String ValidarRango(TarifaFSLBO tarifa, List<RangoTarifaFSLBO> rangosTarifa, RangoTarifaFSLBO nuevoRango)
        {
            if(rangosTarifa == null)
                rangosTarifa = new List<RangoTarifaFSLBO>();
            if (String.IsNullOrEmpty(txtRangoInicial.Text))
                return "Es requerido capturar el rango de Inicio";
            if (String.IsNullOrEmpty(txtRangoFinal.Text) && this.ddlRangoTiempo.SelectedIndex == 2)
                return "Es requerido capturar el rango Final";
            if (String.IsNullOrEmpty(txtCargo.Text))
                return "Es requerido ingresar el valor del Cargo. Sino se cobrará cargo, llenar el campo con 0";

            if(!rangosTarifa.Any())
            {
                if (tarifa.CobraKm.Value)
                {
                    if (tarifa.KmLibres > nuevoRango.KmRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if(tarifa.KmLibres == nuevoRango.KmRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if ((nuevoRango.KmRangoInicial - tarifa.KmLibres) > 1)
                        return "El Rango inicial debe ser los Kilometros Libres MAS Uno";
                    if (nuevoRango.KmRangoFinal == null && this.ddlRangoTiempo.SelectedIndex == 0)
                        return "Debe Existir un Rango Final";
                    if (nuevoRango.KmRangoFinal != null && nuevoRango.KmRangoInicial >= nuevoRango.KmRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
                else
                {
                    if(tarifa.HrLibres > nuevoRango.HrRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if(tarifa.HrLibres == nuevoRango.HrRangoInicial)
                        return "El Rango Inicial debe ser Mayor a la los Kilometros Libres";
                    if((nuevoRango.HrRangoInicial - tarifa.HrLibres) > 1)
                        return "El Rango inicial debe ser las Horas Libres MAS Una";
                    if(nuevoRango.HrRangoFinal == null && this.ddlRangoTiempo.SelectedIndex == 0)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.HrRangoFinal != null && nuevoRango.HrRangoInicial >= nuevoRango.HrRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
            }
            else
            {
                var rangos = rangosTarifa.OrderBy(x => x.KmRangoInicial).ToList();
                var ultimoRango = rangos.Last();
                if (tarifa.CobraKm.Value)
                {
                    if (ultimoRango.KmRangoFinal == null)
                        return "El Rango Anterior al que se quiere agregar no tiene un valor de 'Rango Final'";
                    if (ultimoRango.KmRangoFinal >= nuevoRango.KmRangoInicial)
                        return "El Rango Inicial debe ser Mayor al Ultimo Rango Final";
                    if((nuevoRango.KmRangoInicial - ultimoRango.KmRangoFinal) > 1)
                        return "El Rango inicial debe ser el Ultimo Rango Final mas uno";
                    if(nuevoRango.KmRangoFinal == null && this.ddlRangoTiempo.SelectedIndex == 0)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.KmRangoFinal != null && nuevoRango.KmRangoInicial >= nuevoRango.KmRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
                else
                {
                    if(ultimoRango.HrRangoFinal == null)
                        return "El Rango Anterior al que se quiere agregar no tiene un valor de 'Rango Final'";
                    if(ultimoRango.HrRangoFinal >= nuevoRango.HrRangoInicial)
                        return "El Rango Inicial debe ser Mayor al Ultimo Rango Final";
                    if((nuevoRango.HrRangoInicial - ultimoRango.HrRangoFinal) > 1)
                        return "El Rango inicial debe ser el Ultimo Rango Final mas uno";
                    if(nuevoRango.HrRangoFinal == null && this.ddlRangoTiempo.SelectedIndex == 0)
                        return "Debe Existir un Rango Final";
                    if(nuevoRango.HrRangoFinal != null && nuevoRango.HrRangoInicial >= nuevoRango.HrRangoFinal)
                        return "El Rango final debe ser Mayor al Rango Inicial";
                }
            }
            return "";
        }

        private String ValidarAgregarRango()
        {
            string sError = "";

            if (ddlFrecuencia.SelectedIndex == 0)
                return "Se debe Seleccionar la Frecuencia de " + (CargoKm.Value ? "Kilometros" : "Horas") + " libres.";
            if (String.IsNullOrEmpty(txtKmHrsLibres.Text))
                return "Se debe agregar la cantidad de " + (CargoKm.Value ? "Kilometros" : "Horas") + " libres.";
            if (String.IsNullOrEmpty(txtKmHrMinimo.Text))
                return "Se debe agregar la cantidad de " + (CargoKm.Value ? "Kilometros" : "Horas") + " Minimas.";

            return sError;
        }

        private String ValidarConfigurarTarifa(int añoTarifa)
        {
            string sError = "";

            var tarifa = ObtenerTarifaInterfaz(Tarifas[añoTarifa - 1]);
            tarifa.Rangos = RangosConfigurados;

            if (tarifa.Frecuencia == null)
                return "Se debe configurar la Frecuencia para la Tarifa del año " + añoTarifa;
            if (!tarifa.Rangos.Any())
                return "Se debe configurar al menos UN rango para la Tarifa";

            var rango = tarifa.Rangos.OrderBy(x => CargoKm.Value ? x.KmRangoInicial : x.HrRangoInicial).ToList().Last();
            if (CargoKm.Value)
            {
                if (rango.KmRangoFinal != null)
                    return "Se debe configurar un valor que sea de un Rango de Km Incial EN ADELANTE";
            }
            else
            {
                if(rango.HrRangoFinal != null)
                    return "Se debe configurar un valor que sea de un Rango de Hora Incial EN ADELANTE";
            }

            return sError;
        }

        public void PresentarTarifas(List<TarifaFSLBO> tarifas)
        {
            if(tarifas == null)
                tarifas = new List<TarifaFSLBO>();

            this.grvAniosConfigurados.DataSource = tarifas;
            this.grvAniosConfigurados.DataBind();
        }

        public void PermitirTipoCargo(bool permitir)
        {
            this.ddlTipoCargo.Enabled = permitir;
        }

        public void PermitirModificar(bool permitir)
        {
            this.presentador.PermitirModificar(permitir);
        }

        public void PermitirAnio(bool permitir)
        {
            this.ddlAniosContrato.Enabled = permitir;
        }

        public void PermitirFrecuencia(bool permitir)
        {
            ddlFrecuencia.Enabled = permitir;
        }

        public void PermitirKmsHrsLibres(bool permitir)
        {
            txtKmHrsLibres.Enabled = permitir;
        }

        public void PermitirKmHrMinima(bool permitir)
        {
            txtKmHrMinimo.Enabled = permitir;
        }

        public void PermitirGuardarAnio(bool permitir)
        {
            btnGuardarAnio.Enabled = permitir;
        }

        public void PermitirAgregarRangos(bool permitir)
        {
            btnAgregar.Enabled = permitir;
            txtRangoInicial.Enabled = permitir;
            txtRangoFinal.Enabled = permitir;
            ddlRangoTiempo.SelectedIndex = 0;
            ddlRangoTiempo.Enabled = permitir;
            txtCargo.Enabled = permitir;
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
                if (master != null)
                    master.MostrarMensaje(mensaje, tipo, detalle);
                else
                {
                    if(VistaMensaje != null)
                        VistaMensaje.MostrarMensaje(mensaje, tipo, detalle);
                }
            }
            else
            {
                if(master != null) 
                    master.MostrarMensaje(mensaje, tipo);
                else
                {
                    if(VistaMensaje != null)
                        VistaMensaje.MostrarMensaje(mensaje, tipo, detalle);
                }
            }
        }

        public void MostrarModoConsulta(bool mostrar)
        {
            if (!mostrar)
            {
                Configuracion.Visible = true;
                AniosConfigurados.Visible = true;
                ConsultaConfiguracion.Visible = false;
            }
            else
            {
                Configuracion.Visible = false;
                AniosConfigurados.Visible = false;
                ConsultaConfiguracion.Visible = true;
                if (Tarifas != null && Tarifas.Any())
                    rptConfiguracion.DataSource = Tarifas;
                else
                    rptConfiguracion.DataSource = new List<TarifaFSLBO>();

                rptConfiguracion.DataBind();
            }
        }
        
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                presentador = new ucTarifasFSLPRE(this);
            }
            catch (Exception ex)
            {
                throw new Exception("ucTarifasFSLUI.Page_Load: Inconsistencias al presentar la información.");
            }
        }

        protected void ddlTipoCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlTipoCargo.SelectedIndex == 0)
                {
                    Inicializar(Plazo, TipoCotizacion, ETipoEquipo.EquipoAliado, new List<TarifaFSLBO>(), Identificador, false);
                    CargoKm = null;
                    PermitirAnio(false);
                }
                else
                {
                    Inicializar(Plazo, TipoCotizacion, ETipoEquipo.EquipoAliado, new List<TarifaFSLBO>(), Identificador, false);
                    CargoKm = ddlTipoCargo.SelectedValue == "0";
                    PermitirAnio(true);
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar la información del Tipo de Cargo", ETipoMensajeIU.ERROR, "ucTarifasFSLUI.ddlTipoCargo_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void ddlAniosContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(ddlAniosContrato.SelectedIndex != 0)
                    IniciarCapturaAnio(Int32.Parse(ddlAniosContrato.SelectedValue));
                else
                {
                    IniciarCapturaAnio(null);
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al cambiar la información del Año", ETipoMensajeIU.ERROR, "ucTarifasFSLUI.ddlAniosContrato_SelectedIndexChanged: " + ex.Message);
            }
        }

        protected void btnGuardarAnio_Click(object sender, EventArgs e)
        {
            try
            {
                var año = Int32.Parse(ddlAniosContrato.SelectedItem.Value);

                var sError = ValidarConfigurarTarifa(año);
                if (!String.IsNullOrEmpty(sError))
                {
                    MostrarMensaje(sError, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                var tarifa = ObtenerTarifaInterfaz(Tarifas[año - 1]);
                tarifa.Rangos = new List<RangoTarifaFSLBO>();
                foreach(var rango in RangosConfigurados)
                    tarifa.Rangos.Add(new RangoTarifaFSLBO(rango));
                Tarifas[año - 1] = tarifa;

                this.PresentarTarifas(Tarifas);
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Guardar los cambios en la configuración del Año", ETipoMensajeIU.ERROR, "ucTarifasFSLUI.btnGuardarAnio_Click: " + ex.Message);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var sError = this.ValidarAgregarRango();
                if (!String.IsNullOrEmpty(sError))
                {
                    MostrarMensaje(sError, ETipoMensajeIU.ADVERTENCIA);
                    return;
                }

                AgregarRangoATarifa();
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al agregar Rangos", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        protected void grvRangosConfigurados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string eCommandNameUpper = e.CommandName.ToUpper();
                if(eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                if(e.CommandArgument == null) return;
                if(this.EsModoConsulta.Value) return;

                int index;

                if(!Int32.TryParse(e.CommandArgument.ToString(), out index))
                    return;

                var rangoTarifa = RangosConfigurados[index];

                switch(eCommandNameUpper)
                {
                    case "CMDELIMINAR":
                        if(RangosConfigurados.Count > 1 && (index + 1) != RangosConfigurados.Count)
                        {
                            MostrarMensaje("Debe Eliminarse el Último Rango Primero", ETipoMensajeIU.ADVERTENCIA);
                            break;
                        }

                        RangosConfigurados.Remove(rangoTarifa);
                        PresentarInformacionRangos(RangosConfigurados);
                        break;
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al eliminar el rango de la Tarifa", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        protected void ddlRangoTiempo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtRangoFinal.Text ="";
                txtRangoFinal.Enabled = ddlRangoTiempo.SelectedValue == "0";
            }
            catch (Exception ex)
            {
                MostrarMensaje("Inconsistencias cambiar la seleccion de Rango de Tiempo", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        protected void rptConfiguracion_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                
                TarifaFSLBO tarifa = e.Item.DataItem != null ? e.Item.DataItem as TarifaFSLBO : null;

                if(tarifa == null) return;

                TextBox txtAnio = e.Item.FindControl("txtAnioConsulta") as TextBox;
                TextBox txtFrecuencia = e.Item.FindControl("txtFrecuenciaConsulta") as TextBox;
                TextBox txtKmLibres = e.Item.FindControl("txtKmLibresConsulta") as TextBox;
                TextBox txtKmMinimos = e.Item.FindControl("txtKmMinimosConsulta") as TextBox;
                GridView grvRangos = e.Item.FindControl("grvConsultaConfiguracion") as GridView;
                if (txtAnio != null)
                {
                    txtAnio.Text = tarifa.Año != null ? tarifa.Año.ToString() : "N/A";
                }
                if(txtFrecuencia != null)
                {
                    txtFrecuencia.Text = tarifa.Frecuencia != null ? tarifa.Frecuencia.ToString() : "N/A";
                }
                if(txtKmLibres != null)
                {
                    txtKmLibres.Text = tarifa.CobraKm != null ? tarifa.CobraKm.Value ? tarifa.KmLibres.ToString() : tarifa.HrLibres.ToString() : "N/A";
                }
                if(txtKmMinimos != null)
                {
                    txtKmMinimos.Text = tarifa.CantidadMinima != null ? tarifa.CantidadMinima.ToString() : "N/A";
                }
                if (grvRangos != null)
                {
                    grvRangos.DataSource = tarifa.Rangos ?? new List<RangoTarifaFSLBO>();
                    grvRangos.DataBind();
                }
            }
            catch(Exception ex)
            {
                MostrarMensaje("Inconsistencias al Presentar el Modo de Consulta", ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        #endregion
    }
}