using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.BR;
using BPMO.SDNI.Tramites.BO;
using BPMO.SDNI.Tramites.BR;
using BPMO.Servicio.Catalogos.BO;
using UnidadBO = BPMO.SDNI.Equipos.BO.UnidadBO;
namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ucCargosAdicionalesCierrePSLPRE {
        #region Atributos
        private string nombreClase = "ucCargosAdicionalesCierrePSLPRE";
        /// <summary>
        /// Vista sobre la que actúa el presentador
        /// </summary>
        readonly IucCargosAdicionalesCierrePSLVIS vista;
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        readonly IDataContext dataContext;
        #endregion

        #region Propiedades
        /// <summary>
        /// Vista sobre la que actúa el presentador
        /// </summary>
        internal IucCargosAdicionalesCierrePSLVIS Vista { get { return vista; } }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que recibe la interfaz sobre la que actuará el presentador
        /// </summary>
        /// <param name="vistaActual"></param>
        public ucCargosAdicionalesCierrePSLPRE(IucCargosAdicionalesCierrePSLVIS vistaActual) {
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
        ///  Inicializa la Vista
        /// </summary>
        public void Inicializar() {

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
            Inicializar();
            DatosAInterfazUsuario(unidad);
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
        }

        public void Inicializar(LineaContratoPSLBO linea, ContratoPSLBO contrato) {
            Inicializar();
            vista.UnidadOperativaID = ((UnidadBO)linea.Equipo).Sucursal.UnidadOperativa.Id;
            EstablecerConfiguracionInicial();
            DatosAInterfazUsuario(linea, contrato);
            EstablecerUltimoObjeto(linea);
        }

        /// <summary>
        /// Despliega los datos de la unidad a la interfaz
        /// </summary>
        /// <param name="unidad"></param>
        private void DatosAInterfazUsuario(UnidadBO unidad) {
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

            //Información de los Trámites de la Unidad y Deducible
            TramiteBO tramite = null;
            #region Placa Estatal
            tramite = lstTramites.Find(p => p.Tipo != null && p.Tipo == ETipoTramite.PLACA_ESTATAL && p.Activo != null && p.Activo == true);
            #endregion

            this.vista.EArea = consultada.Area;

            vista.ListadoEquiposAliados = consultada.EquiposAliados;

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

            if (!string.IsNullOrEmpty(mensaje))
                return "Los siguientes campos no pueden estar vacíos: \n" + mensaje.Substring(0, mensaje.Length - 2);

            return mensaje;
        }

        /// <summary>
        /// Obtiene la Línea de Contrato a partir de los datos de la Vista
        /// </summary>
        /// <returns></returns>
        public LineaContratoPSLBO InterfazUsuarioADatos() {
            LineaContratoPSLBO nuevaLinea = this.vista.UltimoObjeto;

            nuevaLinea.CargoAbusoOperacion = this.vista.CargoAbusoOperacion;
            nuevaLinea.CargoDisposicionBasura = this.vista.CargoDisposicionBasura;

            return nuevaLinea;
        }

        /// <summary>
        /// Despliega la línea en la interfaz de usuario
        /// </summary>
        /// <param name="lineaContrato"></param>
        /// <param name="plazoAnio"></param>
        public void DatosAInterfazUsuario(LineaContratoPSLBO lineaContrato, ContratoPSLBO bo) {
            DatosAInterfazUsuario((UnidadBO)lineaContrato.Equipo);

            if (lineaContrato.Equipo != null && lineaContrato.Equipo.Sucursal != null &&
                lineaContrato.Equipo.Sucursal.UnidadOperativa != null)
                vista.UnidadOperativaID = lineaContrato.Equipo.Sucursal.UnidadOperativa.Id;
            //this.vista.TipoTarifaID = (int?)lineaContrato.TipoTarifa;

            if (lineaContrato.Cobrable != null) {
                var tarifaContrato = ((TarifaContratoPSLBO)lineaContrato.Cobrable);
                vista.TarifaPSLID = tarifaContrato.TarifaPSLID;
                vista.DuracionDiasPeriodo = tarifaContrato.DuracionDiasPeriodo;
                vista.MaximoHrsTurno = tarifaContrato.MaximoHrsTurno;

                //Tarifas
                this.vista.TarifaHrsLibres = tarifaContrato.HorasLibres == null ? 0 : tarifaContrato.HorasLibres;
                this.vista.TarifaHrsExcedidas = tarifaContrato.RangoTarifas != null && tarifaContrato.RangoTarifas.Any() ? tarifaContrato.RangoTarifas.First().Cargo ?? 0 : 0;

                AVerificacionLineaPSLBO entrega = lineaContrato.ObtenerListadoVerificacionPorTipo<AVerificacionLineaPSLBO>(ETipoListadoVerificacion.ENTREGA);
                AVerificacionLineaPSLBO recepcion = lineaContrato.ObtenerListadoVerificacionPorTipo<AVerificacionLineaPSLBO>(ETipoListadoVerificacion.RECEPCION);

                this.vista.HrsExcedidas = lineaContrato.CalcularHorasExcedidas();
                this.vista.HrsConsumidas = lineaContrato.CalcularHorasConsumidas();
                this.vista.MontoHrsExcedidas = lineaContrato.CalcularMontoPorHorasExcedidas() == null ? 0 : lineaContrato.CalcularMontoPorHorasExcedidas();
                this.vista.HrsEntrega = entrega.Horometro;
                this.vista.HrsRecepcion = recepcion.Horometro;
                this.vista.DiferenciaCombustible = lineaContrato.CalcularDiferenciaCombustible();

                if (this.vista.ImporteUnidadCombustible != null)
                    this.vista.ImporteTotalCombustible = lineaContrato.CalcularMontoCombustible(this.vista.ImporteUnidadCombustible.Value);
                else
                    this.vista.ImporteUnidadCombustible = null;

                this.vista.CargoAbusoOperacion = lineaContrato.CargoAbusoOperacion;
                this.vista.CargoDisposicionBasura = lineaContrato.CargoDisposicionBasura;

                //Cálculos de Días de Renta
                this.vista.DiasRentaProgramada = bo.CalcularDiasPrometidosRenta();
                this.vista.DiasEnTaller = 0; //TODO: Integración con módulo de mantenimiento
                this.vista.DiasRealesRenta = bo.CalcularDiasTranscurridosRenta();
                if (this.vista.DiasRentaProgramada != null && this.vista.DiasRealesRenta != null)
                    this.vista.DiasAdicionales = this.vista.DiasRealesRenta.Value - this.vista.DiasRentaProgramada.Value;
                else
                    this.vista.DiasAdicionales = null;
                if (this.vista.DiasAdicionales != null && ((TarifaContratoPSLBO)lineaContrato.Cobrable).TarifaDiaria != null)
                    this.vista.MontoTotalDiasAdicionales = this.vista.DiasAdicionales * ((TarifaContratoPSLBO)lineaContrato.Cobrable).TarifaDiaria;
                else
                    this.vista.MontoTotalDiasAdicionales = null;

            } else {
                this.vista.TarifaHrsLibres = 0;
                this.vista.MontoHrsExcedidas = 0;
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
                    this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) :
                    typeof(ETarifaTurnoEquinova);
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

        public void EstablecerConfiguracionInicial() {
            //Se obtienen las configuraciones de la unidad operativa
            if (this.vista.UnidadOperativaID == null)
                throw new Exception("No se identificó en qué unidad operativa trabaja.");
            List<ConfiguracionUnidadOperativaBO> lst = new ModuloBR().ConsultarConfiguracionUnidadOperativa(dataContext, new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }, this.vista.ModuloID);
            if (lst.Count > 0)
                this.vista.ImporteUnidadCombustible = lst[0].PrecioUnidadCombustible;
            else
                this.vista.ImporteUnidadCombustible = null;

        }
        #endregion
    }
}