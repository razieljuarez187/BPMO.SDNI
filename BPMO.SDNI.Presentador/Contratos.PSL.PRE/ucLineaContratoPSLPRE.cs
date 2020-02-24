using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucLineaContratoPSLPRE {
        #region Atributos
        private string nombreClase = "ucLineaContratoPSLPRE";
        /// <summary>
        /// Vista sobre la que actúa el presentador
        /// </summary>
        readonly IucLineaContratoPSLVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        readonly IDataContext dataContext;
        #endregion

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actúa el presentador
        /// </summary>
        internal IucLineaContratoPSLVIS Vista { get { return vista; } }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la interfaz sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual"></param>
        public ucLineaContratoPSLPRE(IucLineaContratoPSLVIS vistaActual) {
            try {
                vista = vistaActual;
                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Prepara para una nueva línea de contrato
        /// </summary>
        private void PrepararNuevaLinea() {
            vista.Modelo = null;
            vista.NumeroEconocimico = null;
            vista.VIN = null;
            vista.LimpiarSesion();
        }

        /// <summary>
        ///  Inicializa la Vista
        /// </summary>
        public void Inicializar() {
            PrepararNuevaLinea();
            vista.Inicializar();
            vista.EstablecerOpcionesTarifaTurno(this.ObtenerTurnos());
            vista.EstablecerOpcionesPeriodoTarifa(this.ObtenerPeriodos());
            vista.EstablecerOpcionesTipoTarifa(this.ObtenerTiposTarifa());           
        }

        /// <summary>
        ///  Inicializa la Vista
        /// </summary>
        /// <param name="unidad">Unidad a Cargar en la Vista</param>
        /// <param name="díasRenta">Duración en días del contrato</param>
        /// <param name="codigoMoneda">Código de la moneda</param>
        public void Inicializar(UnidadBO unidad, int? díasRenta, string codigoMoneda) {
            vista.UnidadOperativaID = unidad.Sucursal.UnidadOperativa.Id;
            vista.CodigoMoneda = codigoMoneda;
            vista.Activo = true;
            Inicializar();
            DatosAInterfazUsuario(unidad);
            EstablecerPeriodo(díasRenta);
        }
        /// <summary>
        ///  Inicializa la Vista con la línea
        /// </summary>
        /// <param name="linea">Línea a Cargar en la Vista</param>
        /// <param name="díasRenta">Duración en días del contrato</param>
        /// <param name="codigoMoneda">Código de la moneda</param>
        public void Inicializar(LineaContratoPSLBO linea, int? díasRenta, string codigoMoneda, string modoRegistro = "") {
            vista.ModoRegistro = modoRegistro;
            vista.UnidadOperativaID = ((UnidadBO)linea.Equipo).Sucursal.UnidadOperativa.Id;
            vista.CodigoMoneda = codigoMoneda;
            Inicializar();

            EstablecerUltimoObjeto(linea);
            DatosAInterfazUsuario(linea);
            EstablecerPeriodo(díasRenta);
        }

        /// <summary>
        /// Despliega los datos de la unidad a la interfaz
        /// </summary>
        /// <param name="unidad"></param>
        private void DatosAInterfazUsuario(UnidadBO unidad){
            UnidadBO consultada = ObtenerUnidad(unidad);
            #region Se obtiene la información completa de la unidad y sus trámites
            List<TramiteBO> lstTramites = new List<TramiteBO>();

            if (consultada != null && (consultada.UnidadID != null || consultada.EquipoID != null)) {
                lstTramites = new TramiteBR().ConsultarCompleto(dataContext, new TramiteProxyBO() { Activo = true, Tramitable = consultada }, false);
            }
            #endregion
            if (consultada == null) consultada = new Equipos.BO.UnidadBO();
            if (consultada.Modelo == null) consultada.Modelo = new ModeloBO();
            if (consultada.Modelo.Marca == null) consultada.Modelo.Marca = new MarcaBO();
            if (consultada.Sucursal == null) consultada.Sucursal = new SucursalBO();
            if (consultada.CaracteristicasUnidad == null) consultada.CaracteristicasUnidad = new CaracteristicasUnidadBO();
            if (consultada == null) throw new Exception("No se ha proporcionado una unidad a agregar");

            vista.UnidadID = consultada.UnidadID;
            vista.EquipoID = consultada.EquipoID;
            vista.VIN = !string.IsNullOrEmpty(consultada.NumeroSerie) ? consultada.NumeroSerie : string.Empty;
            vista.NumeroEconocimico = !string.IsNullOrEmpty(consultada.NumeroEconomico) ? consultada.NumeroEconomico : string.Empty;
            vista.SucursalID = consultada.Sucursal.Id;
            vista.Sucursal = !string.IsNullOrEmpty(consultada.Sucursal.Nombre) ? consultada.Sucursal.Nombre : string.Empty;
            vista.ModeloID = consultada.Modelo.Id;
            vista.Modelo = consultada.Modelo.Nombre;
            vista.Marca = consultada.Modelo.Marca.Nombre;
            this.vista.UnidadCapacidadTanque = consultada.CombustibleConsumidoTotal;
            vista.Anio = consultada.Anio;

            //Información de los Trámites de la Unidad y Deducible
            TramiteBO tramite = null;
            #region Placa Estatal
            tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL && p.Activo != null && p.Activo == true);
            if (tramite != null && tramite.Resultado != null && tramite.Resultado.Trim().CompareTo("") != 0)
                this.vista.UnidadPlacaEstatal = tramite.Resultado;
            else
                this.vista.UnidadPlacaEstatal = null;
            #endregion

            this.vista.EArea = consultada.Area;

            //Información de los Equipos Aliados de la Unidad
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("NumeroSerie"));
            dt.Columns.Add(new DataColumn("Anio"));
            dt.Columns.Add(new DataColumn("Dimensiones"));
            dt.Columns.Add(new DataColumn("PBV"));
            dt.Columns.Add(new DataColumn("PBC"));
            dt.Columns.Add(new DataColumn("Modelo"));
            if (consultada.EquiposAliados != null) {
                foreach (EquipoAliadoBO ea in consultada.EquiposAliados) {
                    DataRow dr = dt.NewRow();
                    dr["NumeroSerie"] = ea.NumeroSerie;
                    dr["Anio"] = ea.Anio;
                    dr["Dimensiones"] = ea.Dimension;
                    dr["PBV"] = ea.PBV;
                    dr["PBC"] = ea.PBC;

                    if (ea.Modelo != null)
                        dr["Modelo"] = ea.Modelo.Nombre;

                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            this.vista.EstablecerEquiposAliadoUnidad(dt);
        } 

        /// <summary>
        /// Obtiene la Unidad con sus datos completos
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        private UnidadBO ObtenerUnidad(UnidadBO unidad) {
            if (unidad != null && (unidad.UnidadID != null)) {
                var unidadBR = new UnidadBR();

                return unidadBR
                    .ConsultarCompleto(dataContext, new UnidadBO() { UnidadID = unidad.UnidadID }, true)
                    .FirstOrDefault();
            }
            throw new Exception("Se requiere proporcionar una Unidad para desplegar su información.");
        }

        /// <summary>
        /// Validación de los datos de la Nueva Línea de Contrato
        /// </summary>
        /// <returns></returns>
        public string ValidarDatos() {
            string mensaje = string.Empty;

            if (vista.PeriodoTarifa == null) mensaje += " Periodo, ";
            if (vista.TipoTarifaID == null) mensaje += " Tipo Tarifa, ";
            if (vista.TarifaTurno == null) mensaje += " Turno, ";

            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);

            return mensaje;
        }

        /// <summary>
        /// Obtiene la Línea de Contrato a partir de los datos de la Vista
        /// </summary>
        /// <returns></returns>
        public LineaContratoPSLBO InterfazUsuarioADatos() {
            this.DatosSesionTarifaPersonalizada();
            LineaContratoPSLBO nuevaLinea = new LineaContratoPSLBO
            {
                Equipo = new UnidadBO
                {
                    UnidadID = vista.UnidadID,
                    EquipoID = vista.EquipoID,
                    NumeroSerie = !String.IsNullOrEmpty(vista.VIN) ? vista.VIN : null,
                    NumeroEconomico = !String.IsNullOrEmpty(vista.NumeroEconocimico) ? vista.NumeroEconocimico : null,
                    Modelo = new ModeloBO { Id = vista.ModeloID, Nombre = vista.Modelo },
                    Sucursal = new SucursalBO
                    {
                        Nombre = vista.Sucursal,
                        UnidadOperativa = new UnidadOperativaBO
                        {
                            Id = vista.UnidadOperativaID,
                        }
                    },
                    CombustibleConsumidoTotal = vista.UnidadCapacidadTanque,
                    Area = vista.EArea,
                    Anio = this.vista.Anio
                },
                TipoTarifa = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaID.ToString())
                ,
                Cobrable = new TarifaContratoPSLBO
                {
                    PeriodoTarifa = (EPeriodosTarifa)vista.PeriodoTarifa,
                    Tarifa = vista.Tarifa,
                    TarifaHrAdicional = vista.TarifaHrAdicional,
                    TarifaTurno = vista.TarifaTurno,
                    TipoTarifaID = vista.TipoTarifaID.ToString(),
                    Maniobra = vista.Maniobra,
                    TarifaPSLID = vista.TarifaPSLID,
                    DuracionDiasPeriodo = vista.DuracionDiasPeriodo,
                    MaximoHrsTurno = vista.MaximoHrsTurno,
                    PorcentajeDescuento = vista.PorcentajeDescuentoTarifa,
                    PorcentajeDescuentoMaximo = vista.PorcentajeMaximoDescuentoTarifa,
                    EtiquetaDescuento = this.vista.TarifaEtiqueta,
                    TarifaConDescuento = this.vista.TarifaConDescuento,
                    PorcentajeSeguro = this.vista.PorcentajeSeguro                    
                },
                Activo = vista.Activo,
                Devuelta = vista.Devuelta,
                LineaOrigenIntercambioID = vista.LineaOrigenIntercambioID
            };

            return nuevaLinea;
        }

        /// <summary>
        /// Despliega la línea en la interfaz de usuario
        /// </summary>
        /// <param name="lineaContrato"></param>
        /// <param name="plazoAnio"></param>
        public void DatosAInterfazUsuario(LineaContratoPSLBO lineaContrato) {
            DatosAInterfazUsuario((UnidadBO)lineaContrato.Equipo);

            if (lineaContrato.Equipo != null && lineaContrato.Equipo.Sucursal != null && lineaContrato.Equipo.Sucursal.UnidadOperativa != null)
                vista.UnidadOperativaID = lineaContrato.Equipo.Sucursal.UnidadOperativa.Id;

            this.vista.TipoTarifaID = (int?)lineaContrato.TipoTarifa;
            this.vista.Activo = lineaContrato.Activo;
            this.vista.Devuelta = lineaContrato.Devuelta;
            this.vista.LineaOrigenIntercambioID = lineaContrato.LineaOrigenIntercambioID;
            if (lineaContrato.Cobrable != null) {
                var tarifaContrato = ((TarifaContratoPSLBO)lineaContrato.Cobrable);
                vista.PeriodoTarifa = (EPeriodosTarifa?)tarifaContrato.PeriodoTarifa;
                vista.Tarifa = tarifaContrato.Tarifa;
                vista.TarifaHrAdicional = tarifaContrato.TarifaHrAdicional;
                vista.TarifaTurno = tarifaContrato.TarifaTurno != null ? tarifaContrato.TarifaTurno : null;
                vista.Maniobra = tarifaContrato.Maniobra;
                vista.TarifaPSLID = tarifaContrato.TarifaPSLID;
                vista.DuracionDiasPeriodo = tarifaContrato.DuracionDiasPeriodo;
                vista.MaximoHrsTurno = tarifaContrato.MaximoHrsTurno;
                vista.PorcentajeDescuentoTarifa = tarifaContrato.PorcentajeDescuento;
                vista.PorcentajeMaximoDescuentoTarifa = tarifaContrato.PorcentajeDescuentoMaximo;
                vista.TarifaEtiqueta = tarifaContrato.EtiquetaDescuento;
                vista.TarifaConDescuento = tarifaContrato.TarifaConDescuento;
                vista.PorcentajeSeguro = tarifaContrato.PorcentajeSeguro;
                CalcularSeguro(tarifaContrato.Tarifa);
            }
        }

        /// <summary>
        /// Establece el Objeto de Línea de Contrato antes de editar
        /// </summary>
        /// <param name="lineaContrato"></param>
        public void EstablecerUltimoObjeto(LineaContratoPSLBO lineaContrato) {
            vista.UltimoObjeto = lineaContrato;
        }

        private Dictionary<int, string> ObtenerTiposTarifa() {
            Dictionary<int, string> TiposTarifas = new Dictionary<int, string>();
            TiposTarifas.Add(-1, "Seleccione una opción");
            foreach (var tipo in Enum.GetValues(typeof(ETipoTarifa))) {
                if (((int)tipo) != 3) {
                    TiposTarifas.Add(((int)tipo), Enum.GetName(typeof(ETipoTarifa), tipo));
                }
            }
            return TiposTarifas;
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de los turnos que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<string, string> ObtenerTurnos() {
            try {
                Dictionary<string, string> listaTurnos = new Dictionary<string, string>();
                listaTurnos.Add("-1", "SELECCIONE UNA OPCIÓN");
                Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) :
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) : typeof(ETarifaTurnoEquinova);
                Array values = Enum.GetValues(type);
                foreach (int value in values) {
                    var memInfo = type.GetMember(type.GetEnumName(value));
                    var display = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        listaTurnos.Add(value.ToString(), display.Description);
                    }
                }

                return listaTurnos;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ListaTurnos:Error al consultar los turnos");
            }
        }
        /// <summary>
        /// Método para obtener un diccionario con los valores de los periodos que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo string,string</returns>
        private Dictionary<string, string> ObtenerPeriodos() {
            try {
                Dictionary<string, string> listaPeriodos = new Dictionary<string, string>();
                listaPeriodos.Add("-1", "SELECCIONE UNA OPCIÓN");
                Type type = typeof(EPeriodosTarifa);
                Array values = Enum.GetValues(type);
                foreach (int value in values) {
                    var memInfo = type.GetMember(type.GetEnumName(value));
                    var display = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (display != null) {
                        listaPeriodos.Add(value.ToString(), display.Description);
                    }
                }

                return listaPeriodos;
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ListaPeriodos:Error al consultar los períodos");
            }
        }
        /// <summary>
        /// Asigna el periodo correspondiente con los días de contrato. 
        /// </summary>
        /// <param name="diasRenta">Duración en días del contrato</param>
        private void EstablecerPeriodo(int? diasRenta) {
            try {
                //Si no existe tarifa agregada se coloca el periodo de tarifa sugerido de acuerdo al numero de días de contrato
                if (this.vista.Tarifa == null) {
                    DiaPeriodoTarifaBR DiaPerioBR = new DiaPeriodoTarifaBR();
                    List<Object> listValues = DiaPerioBR.ObtenerPeriodoTarifa(this.dataContext, this.vista.UnidadOperativaID, diasRenta.Value);

                    this.vista.PeriodoTarifa = (EPeriodosTarifa)listValues[0];
                    this.vista.DuracionDiasPeriodo = Convert.ToInt32(listValues[1]);
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".ObtenerPeriodo:Error al obtener el período");
            }
        }
        /// <summary>
        /// Calcula el MaximoHrsTurno dependiendo de los campos Periodo, Tarifa Turno y Unidad Operativa
        /// </summary>
        public void CalcularMaximoHrsTurno() {
            try {
                if (this.vista.PeriodoTarifa != null && this.vista.TarifaTurno != null) {
                    DiaPeriodoTarifaBR DiaPerioBR = new DiaPeriodoTarifaBR();
                    this.vista.MaximoHrsTurno = DiaPerioBR.ObtenerMaximoHorasTurnoTarifa(this.dataContext, this.vista.UnidadOperativaID, (EPeriodosTarifa)this.vista.PeriodoTarifa, this.vista.TarifaTurno);
                    CalcularTarifas();
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".CalcularMaximoHrsTurno: Error al obtener el máximo hrs. turno");
            }
        }
        /// <summary>
        /// Calcula la duración días periodo dependiendo de los campos Periodo Unidad Operativa
        /// </summary>
        public void CalcularPeriodoTarifa() {
            try {
                if (this.vista.PeriodoTarifa != null) {
                    DiaPeriodoTarifaBR DiaPerioBR = new DiaPeriodoTarifaBR();
                    this.vista.DuracionDiasPeriodo = DiaPerioBR.ObtenerPeriodoTarifa(this.dataContext, this.vista.UnidadOperativaID, (EPeriodosTarifa)this.vista.PeriodoTarifa);
                    CalcularTarifas();
                }
            } catch {
                throw new Exception(this.nombreClase + ".CalcularPeriodoTarifa: Error al obtener la duración días periodo");
            }
        }
        /// <summary>
        /// Calcula la tarifa de la unidad, dependiendo de los campos Periodo, Tarifa Turno, Tipo Tarifa, Unidad Operativa y Modelo de la unidad.
        /// </summary>
        public void CalcularTarifas() {
            try {

                this.vista.Tarifa = null;
                this.vista.TarifaHrAdicional = null;

                if (this.vista.PeriodoTarifa != null && this.vista.TarifaTurno != null && this.vista.TipoTarifaID != null && this.vista.UnidadOperativaID != null && this.vista.ModeloID != null) {
                    ETipoTarifa tipo = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaID.ToString());
                    TarifaPSLBR tarifaBR = new TarifaPSLBR();
                    TarifaPSLBO tarifaBO = new TarifaPSLBO();
                    tarifaBO.PeriodoTarifa = (EPeriodosTarifa)this.vista.PeriodoTarifa;
                    tarifaBO.TipoTarifaID = tipo.ToString();
                    tarifaBO.TarifaTurno = this.vista.TarifaTurno;
                    tarifaBO.Modelo = new ModeloBO { Id = this.vista.ModeloID };
                    tarifaBO.Sucursal = new SucursalBO { Id = this.vista.SucursalID, UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                    tarifaBO.Activo = true;
                    List<TarifaPSLBO> lstTarifas = tarifaBR.Consultar(this.dataContext, tarifaBO);
                    if (lstTarifas.Any()) {
                        Decimal tipoCambioAplicar = 1M;
                        
                        // Seleccionar tarifa de la misma moneda con la que se trabaja
                        TarifaPSLBO tarifaAplicar = lstTarifas.FirstOrDefault(t => t.Divisa.MonedaDestino.Codigo == this.vista.CodigoMoneda);
                        
                        // Si no se encuentra tarifa con misma moneda se selecciona la primera que se encuentre
                        if (tarifaAplicar == null) {
                            tarifaAplicar = lstTarifas.FirstOrDefault();
                            
                            // Calcular el tipo de cambio
                            DivisaBO divisaConversion = new DivisaBO(){ 
                                MonedaOrigen = tarifaAplicar.Divisa.MonedaDestino,
                                MonedaDestino = new MonedaBO(){ Codigo = this.vista.CodigoMoneda },
                                FechaTipoCambio = DateTime.Now.Date };
                            divisaConversion = FacadeBR.ConsultarTipoCambio(this.dataContext, divisaConversion).FirstOrDefault();

                            if (divisaConversion == null)
                                throw new Exception("No se encuentra tipo de cambio de " + tarifaAplicar.Divisa.MonedaDestino.Codigo
                                    + " a " + this.vista.CodigoMoneda + ".");

                            tipoCambioAplicar = divisaConversion.TipoCambio.Value;
                        }
                        
                        this.vista.Tarifa = tarifaAplicar.Tarifa * tipoCambioAplicar;
                        this.vista.TarifaHrAdicional = tarifaAplicar.TarifaHrAdicional * tipoCambioAplicar;
                        this.CalcularSeguro(this.vista.Tarifa);
                    } else {
                        this.vista.MostrarMensaje("No se encontró una tarifa configurada que coincida con el filtro proporcionado.", ETipoMensajeIU.ADVERTENCIA);
                    }
                }
            } catch (Exception ex) {
                throw new Exception(this.nombreClase + ".CalcularTarifas: Error al obtener las tarifas. " + ex.Message);
            }
        }
        /// <summary>
        /// Calcula el porcentaje del seguro de la unidad en base a su tarifa
        /// </summary>
        /// <param name="tarifa"> tarifa base del calculo</param>
        public void CalcularSeguro(decimal? tarifa) {
            if (this.vista.PorcentajeSeguro == null || this.vista.PorcentajeSeguro == 0 )
                this.vista.PorcentajeSeguro = this.ObtenerPorcentajeSeguro(this.vista.UnidadOperativaID, this.vista.ModuloID);
            this.vista.Seguro = tarifa * (this.vista.PorcentajeSeguro / 100);
        }
        /// <summary>
        /// Obtiene el porcentaje de seguro de acuerrdo a la unidad operativa
        /// </summary>
        public decimal? ObtenerPorcentajeSeguro(int? unidadOperativaID, int? ModuloID){
            try {
                decimal? porcentajeSeguro = 0;
                ModuloBR configBR = new ModuloBR();
                ConfiguracionUnidadOperativaBO configBO = new ConfiguracionUnidadOperativaBO()
                {
                    UnidadOperativa = new UnidadOperativaBO() { Id = unidadOperativaID }
                };
                List<ConfiguracionUnidadOperativaBO> listConfigUO = new List<ConfiguracionUnidadOperativaBO>();
                listConfigUO = configBR.ConsultarConfiguracionUnidadOperativa(dataContext, configBO, this.vista.ModuloID);
                if (listConfigUO != null && listConfigUO.Count > 0)
                    porcentajeSeguro = listConfigUO[0].PorcentajeSeguro != null ? listConfigUO[0].PorcentajeSeguro : 0;
                
                return porcentajeSeguro;
            } catch {
                
                throw new Exception(this.nombreClase + ".CalcularSeguro: Error al obtener el Seguro");
            }
        }

        #region Métodos para la Personalización de Tarifas        
        /// <summary>
        /// Valida si se ha configurado un descuento a la tarifa de unidad, en caso de existir se notifica al usuario mediante un mensaje en UI
        /// </summary>
        public void VerificarDescuentoATarifa() {
            this.DatosSesionTarifaPersonalizada();
            if (this.vista.PorcentajeDescuentoTarifa.GetValueOrDefault() > 0)
                this.vista.MostrarMensaje("La tarifa de la unidad tiene configurado un descuento de " + this.vista.PorcentajeDescuentoTarifa.ToString() + "%", ETipoMensajeIU.INFORMACION);
        }

        /// <summary>
        /// Verifica que ciertos campos que son requeridos se hayan capturado para poder presentar el modal de personalizar tarifa 
        /// </summary>
        /// <returns>Retorna mensaje que contiene el error en caso de existir</returns>
        public string ValidarCamposPrevioPersonalizarTarifa() {
            string s = string.Empty;
            if (this.vista.Tarifa == null)
                s += "Tarifa, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            return null;
        }

        /// <summary>
        /// Limpia los campos del dialogo y 
        /// Copia los datos de tarifa de la interfaz de usuario
        /// </summary>
        public void PrepararDatosTarifaPersonalizada() {
            TarifaPersonalizadaPSLModel tarifa = new TarifaPersonalizadaPSLModel();

            tarifa.UnidadOperativaID = vista.UnidadOperativaID;
            tarifa.SucursalID = vista.SucursalID;
            tarifa.ModeloID = vista.ModeloID;
            tarifa.ModuloID = vista.ModuloID;
            tarifa.UsuarioID = vista.UsuarioID;
            tarifa.CuentaClienteID = vista.CuentaClienteID;
            tarifa.TarifaPersonalizadaDescuentoMax = vista.PorcentajeMaximoDescuentoTarifa;
            tarifa.TarifaPersonalizadaTarifa = vista.Tarifa;
            tarifa.TarifaPersonalizadaTipoTarifa = vista.TarifaPersonalizadaTipoTarifa;
            tarifa.TarifaPersonalizadaTurno = vista.TarifaPersonalizadaTurno;
            tarifa.TarifaPersonalizadaTarifaHrAdicional = vista.TarifaHrAdicional;
            tarifa.TarifaBaseTipoTarifaID = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifaID.ToString());
             tarifa.TarifaBasePeriodoTarifa = (EPeriodosTarifa)this.vista.PeriodoTarifa;
             tarifa.TarifaBaseDivisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.CodigoMoneda } };
             tarifa.TarifaBaseActivo = true;
             tarifa.TarifaPersonalizadaPorcentajeDescuento = this.vista.PorcentajeDescuentoTarifa;
             tarifa.TarifaPersonalizadaTarifaConDescuento = this.vista.TarifaConDescuento;
             tarifa.TarifaPersonalizadaPorcentajeSeguro = this.vista.PorcentajeSeguro;
             
            this.vista.LimpiarPaqueteNavegacion("TarifaPersonalizaPSLEnviada");
            this.vista.EstablecerPaqueteNavegacion("TarifaPersonalizaPSLEnviada", tarifa);

        }

        /// <summary>
        /// Obtiene la información relacionada con las tarifas personalizadas
        /// </summary>
        private void DatosSesionTarifaPersonalizada() {
            try {
                object objectoDevuelto = this.vista.ObtenerPaqueteNavegacion("TarifaPersonalizaPSLDevuelta");
                TarifaPersonalizadaPSLModel tarifaModel = null;
                if (objectoDevuelto != null) {
                    if (objectoDevuelto is TarifaPersonalizadaPSLModel) {
                        tarifaModel = (TarifaPersonalizadaPSLModel)objectoDevuelto;
                        vista.Tarifa = tarifaModel.TarifaPersonalizadaTarifa;
                        vista.PorcentajeDescuentoTarifa = tarifaModel.TarifaPersonalizadaPorcentajeDescuento;
                        vista.PorcentajeMaximoDescuentoTarifa = tarifaModel.TarifaPersonalizadaDescuentoMax;
                        vista.TarifaEtiqueta = tarifaModel.TarifaPersonalizadaEtiqueta;
                    }
                }
                if (vista.PorcentajeDescuentoTarifa.GetValueOrDefault() <= 0) {
                    vista.PorcentajeDescuentoTarifa = null;
                    vista.TarifaConDescuento = null;
                } else {
                    decimal tarifa = this.vista.Tarifa.GetValueOrDefault();
                    decimal descuento = this.vista.PorcentajeDescuentoTarifa.GetValueOrDefault();
                    decimal tarifaConDescuento = Math.Round(tarifa - (tarifa * descuento / 100), 2);//Redondea a dos posiciones
                    vista.TarifaConDescuento = tarifaConDescuento;
                }
                this.CalcularSeguro(vista.Tarifa);
            } catch {
                throw new Exception(this.nombreClase + ".ObtenerDatosTarifaPersonalizada: Error al obtener los datos de las tarifas personalizadas");
            }
        }
        /// <summary>
        /// Actualiza la tarifa en base al diálogo de personalización de tarifa
        /// </summary>
        public void ActualizarTarifaPersonalizada() {
            try {
                object objetoDevuelto = this.vista.ObtenerPaqueteNavegacion("TarifaPersonalizaPSLDevuelta");
                if (objetoDevuelto != null) {
                    TarifaPersonalizadaPSLModel tarifaNueva = (TarifaPersonalizadaPSLModel)objetoDevuelto;
                    this.vista.Tarifa = tarifaNueva.TarifaPersonalizadaTarifa;
                    this.vista.PorcentajeDescuentoTarifa = tarifaNueva.TarifaPersonalizadaPorcentajeDescuento;
                    this.vista.TarifaConDescuento = tarifaNueva.TarifaPersonalizadaTarifaConDescuento;
                }
            } catch {
                new Exception(this.nombreClase + ".ActualizarTarifaPersonalizada: Error al actualizar la tarifa personalizada");
            }
        }
        #endregion
        #endregion
    }
}