// Satisface al caso de uso CU005 - Catálogo de Tarifas de Renta Comercial Diaria.
// Consutrccion de Mejoras - Cobro de Rangos de Km y Horas.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    /// <summary>
    /// Presentador para la Interfaz de Consulta de Tarifas de RD
    /// </summary>
    public class ConsultarTarifaPSLPRE {
        #region Atributos
        /// <summary>
        /// Nombre de la Clase, usado para Excepciones
        /// </summary>
        private string nombreClase = "ConsultarTarifasPSLPRE";
        /// <summary>
        /// Vista de Consulta de Tarifas
        /// </summary>
        private IConsultarTarifaPSLVIS vista;
        /// <summary>
        /// El DataContext que provee acceso a la Base de Datos
        /// </summary>
        private IDataContext dctx;
        /// <summary>
        /// Controlador de Tarifa de Renta Diaria
        /// </summary>
        private TarifaPSLBR tarifaBr;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del Presentador
        /// </summary>
        /// <param name="vistaActual">Vista a la cual estara asociado el Presentador</param>
        public ConsultarTarifaPSLPRE(IConsultarTarifaPSLVIS vistaActual) {
            try {
                this.vista = vistaActual;
                dctx = FacadeBR.ObtenerConexion();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarTarifasPSLPRE:Error al configurar el presentador");
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Limpia la Interfaz de Consulta al Iniciar la misma
        /// </summary>
        public void PrepararNuevo() {
            this.vista.LimpiarSesion();

            this.vista.NombreSucursal = null;
            this.vista.SucursalID = null;
            this.vista.NombreModelo = null;
            this.vista.ModeloID = null;
            this.vista.CodigoMoneda = null;
            this.vista.TipoTarifa = null;
            this.vista.Estatus = null;
            this.vista.EstablecerOpcionesMoneda(this.ObtenerMonedas());
            this.vista.EstablecerOpcionesTipoTarifa(this.ObtenerTiposTarifa());
            this.vista.EstablecerOpcionesTarifaTurno(this.ObtenerTurnos());
            this.vista.EstablecerOpcionesPeriodoTarifa(this.ObtenerPeriodos());
            this.EstablecerFiltros();//SC0024
            this.EstablecerSeguridad();
        }

        /// <summary>
        /// Valida los permisos que se tienen asignado al usuario
        /// </summary>
        public void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTARCOMPLETO"))
                    this.vista.PermitirRegistrar(false);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Valida que de la lista de acciones enviada se encuentra una acción en particular
        /// </summary>
        /// <param name="acciones">Lista de acciones que se recuperan según el usuario loggeado</param>
        /// <param name="accion">Nombre de la acción a validar</param>
        /// <returns></returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de las monedas que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo <string, string> </returns>
        private Dictionary<string, string> ObtenerMonedas() {
            try {
                Dictionary<string, string> listaMonedas = new Dictionary<string, string>();
                listaMonedas.Add("-1", "Todos");
                List<MonedaBO> monedas = FacadeBR.ConsultarMoneda(dctx, new MonedaBO { Activo = true });
                if (monedas != null) {
                    foreach (var monedaBo in monedas) {
                        listaMonedas.Add(monedaBo.Codigo, monedaBo.Nombre);
                    }
                }
                return listaMonedas;
            } catch (Exception ex) {

                throw new Exception(this.nombreClase + ".ListaMonedas:Error al consultar las monedas");
            }
        }

        /// <summary>
        /// Método para obtener un diccionario con los valores de las tarifas que se envía como parámetro para el llenado del combo correspondiente
        /// </summary>
        /// <returns>Diccionario de tipo <string, string> </returns>
        private Dictionary<int, string> ObtenerTiposTarifa() {
            Dictionary<int, string> TiposTarifas = new Dictionary<int, string>();
            TiposTarifas.Add(-1, "Todos");
            foreach (var tipo in Enum.GetValues(typeof(ETipoTarifa))) {
                if (((int)tipo) != 3)
                    TiposTarifas.Add(((int)tipo), Enum.GetName(typeof(ETipoTarifa), tipo));
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
                listaTurnos.Add("-1", "Todos");
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
                listaPeriodos.Add("-1", "Todos");
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
        /// Método que realiza la consulta de las tarifas para cargar a la interfaz
        /// </summary>
        public void ConsultarTarifas() {
            try {
                TarifaPSLBO tarifa = InterfazUsuarioADato();
                tarifaBr = new TarifaPSLBR();
                List<TarifaPSLBO> lstTarifas = tarifaBr.ConsultarCompleto(dctx, tarifa);
                this.vista.ListaTarifas = lstTarifas != null ? lstTarifas.ConvertAll(t => (TarifaPSLBO)t) : new List<TarifaPSLBO>();
                if (lstTarifas.Count != 0)
                    this.vista.ActualizarListaTarifas();
                else {
                    this.vista.ListaTarifas = null;
                    this.vista.ActualizarListaTarifas();
                    MostrarMensaje("No se han encontrado tarifas con los filtros proporcionados",
                                   ETipoMensajeIU.INFORMACION);
                }
            } catch (Exception ex) {

                this.MostrarMensaje("Inconsistencias al momento de buscar las tarifas", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarTarifas:" + ex.Message);
            }

        }

        /// <summary>
        /// Método que redirige a la pantalla de detalle
        /// </summary>
        /// <param name="tarifaID">Identificador de la tarifa que desea ver el detalle</param>
        public void IrADetalle(int? tarifaID) {
            try {
                TarifaPSLBO tarifa = new TarifaPSLBO { TarifaPSLID = tarifaID };
                this.vista.LimpiarSesion();
                #region SC0024
                Dictionary<string, object> elementosFiltro = new Dictionary<string, object>();
                elementosFiltro.Add("ObjetoFiltro", this.InterfazUsuarioADatoNavegacion());
                elementosFiltro.Add("PagActGrid", this.vista.IndicePaginaResultado);
                elementosFiltro.Add("Bandera", true);
                #endregion
                this.vista.EstablecerDatosNavegacion(tarifa, elementosFiltro);
                this.vista.RedirigirADetalle();
            } catch (Exception ex) {
                this.MostrarMensaje("Error al momento de intentar redirigir al detalle de la tarifa seleccionada", ETipoMensajeIU.ERROR, nombreClase + "IrADetalle" + ex.Message);
            }
        }

        /// <summary>
        /// Método para realizar la validación del nombre del modelo
        /// </summary>
        /// <returns>Retorna una cadena con el nombre del Modelo que no tiene un dato definido</returns>
        public string ValidarCamposConsultaModelo() {
            string s = null;
            if (String.IsNullOrEmpty(this.vista.NombreModelo))
                s += ", Modelo";
            return s;
        }

        /// <summary>
        /// Método para obtener la información de la interfaz y asignarla a un objeto de tipo TarifaPSLBO
        /// </summary>
        /// <returns>Regresa un objeto de tipo TarifaPSLBO</returns>
        private TarifaPSLBO InterfazUsuarioADato() {
            try {
                TarifaPSLBO bo = new TarifaPSLBO();
                bo.RangoTarifas = new List<RangoTarifaPSLBO>();
                bo.Sucursal = new SucursalBO();
                bo.Sucursal.UnidadOperativa = new UnidadOperativaBO();

                bo.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;
                bo.Sucursal.Id = this.vista.SucursalID;

                if (this.vista.ModeloID != null)
                    bo.Modelo = new ModeloBO { Id = this.vista.ModeloID };
                if (!String.IsNullOrEmpty(this.vista.CodigoMoneda))
                    bo.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.CodigoMoneda } };
                if (this.vista.TipoTarifa != null)
                    bo.TipoTarifaID = Enum.GetName(typeof(ETipoTarifa), this.vista.TipoTarifa);
                if (this.vista.SucursalID == null) {
                    bo.SucursalesConsulta = FacadeBR.ConsultarSucursalesSeguridad(dctx, new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } }));
                }
                if (this.vista.TarifaTurno != null && Convert.ToInt32(this.vista.TarifaTurno) > 0) {
                    Type type = this.vista.UnidadOperativaID == (int)ETipoEmpresa.Construccion ? typeof(ETarifaTurnoConstruccion) :
                        this.vista.UnidadOperativaID == (int)ETipoEmpresa.Generacion ? typeof(ETarifaTurnoGeneracion) :
                        typeof(ETarifaTurnoEquinova);
                    bo.TarifaTurno = (Enum)Enum.ToObject(type, this.vista.TarifaTurno);
                }
                if (this.vista.PeriodoTarifa > 0) {
                    bo.PeriodoTarifa = (EPeriodosTarifa)this.vista.PeriodoTarifa;
                }
                bo.Activo = this.vista.Estatus;

                return bo;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".InterfazUsuarioADato:Inconsistencias al obtener los datos proporcionados como filtro de la consulta" + ex.Message);
            }
        }

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar.</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="msjDetalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        private void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            this.vista.MostrarMensaje(mensaje, tipo, detalle);
        }

        #region SC0024
        /// <summary>
        /// SC0024
        /// Establece los filtros iniciales en caso de regresar a la página de la consulta
        /// </summary>
        private void EstablecerFiltros() {
            Dictionary<string, object> temp = this.vista.ObtenerPaqueteNavegacion() as Dictionary<string, object>;
            if (!ReferenceEquals(temp, null)) {
                if (temp.ContainsKey("ObjetoFiltro")) {
                    if (temp["ObjetoFiltro"].GetType() == typeof(TarifaPSLBO))
                        this.DatosAIntefazUsuario(temp["ObjetoFiltro"]);
                    else
                        throw new Exception("Se esperaba un objeto TarifaPSLBO, el objeto proporcionado no cumple con esta caracteristica, intente de nuevo por favor.");
                }

                if (temp.ContainsKey("Bandera")) {
                    if ((bool)temp["Bandera"])
                        this.ConsultarTarifas();
                    if (temp.ContainsKey("PagActGrid"))
                        this.CambiarPaginaResultado((int)temp["PagActGrid"]);
                }
            }
            this.vista.LimpiarPaqueteNavegacion();
        }
        /// <summary>
        /// SC0024
        /// Obtiene la información capturada en la vista y la convierte en un objeto
        /// </summary>
        /// <returns>Objeto generado con los datos de la interfaz de usuario</returns>
        private object InterfazUsuarioADatoNavegacion() {
            try {
                TarifaPSLBO bof = new TarifaPSLBO();
                bof.RangoTarifas = new List<RangoTarifaPSLBO>();
                bof.Sucursal = new SucursalBO();
                bof.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                bof.Modelo = new ModeloBO();

                bof.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID;

                if (this.vista.SucursalID.HasValue)
                    bof.Sucursal.Id = this.vista.SucursalID.Value;

                if (!string.IsNullOrEmpty(this.vista.NombreSucursal) && !string.IsNullOrWhiteSpace(this.vista.NombreSucursal))
                    bof.Sucursal.Nombre = this.vista.NombreSucursal.Trim().ToUpper();

                if (this.vista.ModeloID.HasValue)
                    bof.Modelo.Id = this.vista.ModeloID.Value;

                if (!string.IsNullOrEmpty(this.vista.NombreModelo) && !string.IsNullOrWhiteSpace(this.vista.NombreModelo))
                    bof.Modelo.Nombre = this.vista.NombreModelo.Trim().ToUpper();

                if (!String.IsNullOrEmpty(this.vista.CodigoMoneda))
                    bof.Divisa = new DivisaBO { MonedaDestino = new MonedaBO { Codigo = this.vista.CodigoMoneda } };
                if (this.vista.TipoTarifa != null)
                    bof.Tipo = (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), this.vista.TipoTarifa.ToString());

                if (this.vista.SucursalID.HasValue)
                    bof.SucursalesConsulta = FacadeBR.ConsultarSucursalesSeguridad(dctx, new SeguridadBO(Guid.Empty, new UsuarioBO { Id = this.vista.UsuarioID }, new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID } }));

                if (this.vista.Estatus.HasValue)
                    bof.Activo = this.vista.Estatus;
                bof.TarifaTurno = this.vista.TarifaTurno;
                bof.PeriodoTarifa = this.vista.PeriodoTarifa;

                return bof;
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".InterfazUsuarioADato:Inconsistencias al obtener los datos proporcionados como filtro de la consulta" + ex.Message);
            }
        }
        /// <summary>
        /// SC0024
        /// Despliega en la vista un objeto recuperado
        /// </summary>
        /// <param name="obj">Objeto que se desea desplegar</param>
        private void DatosAIntefazUsuario(object obj) {
            TarifaPSLBO tarifa = (TarifaPSLBO)obj;
            if (!ReferenceEquals(tarifa, null)) {
                #region Sucursal
                if (!ReferenceEquals(tarifa.Sucursal, null)) {
                    this.vista.SucursalID = tarifa.Sucursal.Id.HasValue
                                                ? tarifa.Sucursal.Id
                                                : null;
                    this.vista.NombreSucursal = !string.IsNullOrEmpty(tarifa.Sucursal.Nombre) && !string.IsNullOrWhiteSpace(tarifa.Sucursal.Nombre)
                                                    ? tarifa.Sucursal.Nombre
                                                    : string.Empty;
                }
                #endregion

                #region Modelo
                if (!ReferenceEquals(tarifa.Modelo, null)) {
                    this.vista.ModeloID = tarifa.Modelo.Id.HasValue ? tarifa.Modelo.Id : null;
                    this.vista.NombreModelo = !string.IsNullOrEmpty(tarifa.Modelo.Nombre) && !string.IsNullOrWhiteSpace(tarifa.Modelo.Nombre)
                                                  ? tarifa.Modelo.Nombre
                                                  : string.Empty;
                }
                #endregion

                #region Moneda
                if (!ReferenceEquals(tarifa.Divisa, null)) {
                    if (!ReferenceEquals(tarifa.Divisa.MonedaDestino, null)) {
                        this.vista.CodigoMoneda = !string.IsNullOrEmpty(tarifa.Divisa.MonedaDestino.Codigo) &&
                                                  !string.IsNullOrWhiteSpace(tarifa.Divisa.MonedaDestino.Codigo)
                                                      ? tarifa.Divisa.MonedaDestino.Codigo
                                                      : "-1";
                    }
                }
                #endregion

                #region TipoTarifa
                this.vista.TipoTarifa = tarifa.Tipo.HasValue ? (int?)tarifa.Tipo : null;
                #endregion

                this.vista.TarifaTurno = tarifa.TarifaTurno;
                this.vista.PeriodoTarifa = tarifa.PeriodoTarifa;
                this.vista.Estatus = tarifa.Activo.HasValue ? tarifa.Activo : null;
            } else {
                this.LimpiarCampos();
            }
        }
        /// <summary>
        /// SC0024
        /// Establece el nuevo indice para el grid de la página
        /// </summary>
        /// <param name="nuevoIndicePagina"></param>
        public void CambiarPaginaResultado(int nuevoIndicePagina) {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarListaTarifas();
        }
        /// <summary>
        /// SC0024
        /// Limpia las propiedades de la vista
        /// </summary>
        private void LimpiarCampos() {
            this.vista.NombreSucursal = null;
            this.vista.SucursalID = null;
            this.vista.NombreModelo = null;
            this.vista.ModeloID = null;
            this.vista.EstablecerOpcionesMoneda(this.ObtenerMonedas());
            this.vista.EstablecerOpcionesTipoTarifa(this.ObtenerTiposTarifa());
            this.vista.EstablecerOpcionesTarifaTurno(this.ObtenerTurnos());
            this.vista.EstablecerOpcionesPeriodoTarifa(this.ObtenerPeriodos());
            this.vista.CodigoMoneda = null;
            this.vista.TipoTarifa = null;
            this.vista.TarifaTurno = null;
            this.vista.Estatus = null;
        }
        #endregion

        #region Método del Buscador
        /// <summary>
        /// Mostrar en la vista los nombres según el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del catálogo que se está buscando</param>
        /// <param name="selecto"></param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Modelo":
                    #region Desplegar Modelo
                    ModeloBO modelo = (ModeloBO)selecto;

                    this.vista.ModeloID = modelo != null && modelo.Id != null ? modelo.Id : null;

                    this.vista.NombreModelo = modelo != null && modelo.Nombre != null ? modelo.Nombre : null;

                    #endregion
                    break;
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.NombreSucursal = sucursal.Nombre;
                    else
                        this.vista.NombreSucursal = null;
                    break;
            }
        }

        /// <summary>
        /// Asigna los valores de la vista a un Objeto BO
        /// </summary>
        /// <param name="catalogo">Nombre del catálogo que será creado como BO</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo.ToUpper()) {
                case "MODELO":
                    ModeloBO modelo = new ModeloBO();
                    modelo.Auditoria = new AuditoriaBO();
                    modelo.Marca = new MarcaBO();
                    modelo.Nombre = this.vista.NombreModelo;
                    modelo.Activo = true;
                    obj = modelo;
                    break;
                case "SUCURSAL":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursal.Nombre = this.vista.NombreSucursal;
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };
                    obj = sucursal;
                    break;
            }

            return obj;
        }
        #endregion
        #endregion
    }
}