using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Contratos.PSL.BOF;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Contratos.PSL.PRE {
    public class ConsultarContratoROCPRE {

        #region Atributos

        private readonly IConsultarContratoROCVIS vista;

        /// <summary>
        /// Controlador de Contratos PSL
        /// </summary>
        private readonly ContratoPSLBR Controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarContratosROCPRE";

        /// <summary>
        /// Presentador del Control de Herramientas
        /// </summary>
        private readonly ucHerramientasPSLPRE HerramientasPRE;

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="vistaActual">Vista Actual</param>

        public ConsultarContratoROCPRE(IConsultarContratoROCVIS vistaActual) {
            try {
                if (vistaActual != null) vista = vistaActual;
                else throw new Exception("La vista proporcionada no puede ser nula.");

                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
                Controlador = new ContratoPSLBR();
            } catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias en la construcción del presentador", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarContratosFSLPRE: " + ex.Message);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Prepara la Vista para una nueva búsqueda
        /// </summary>
        public void PrepararBusqueda() {
            //Datos de Cliente
            vista.ClienteID = null;
            vista.CuentaClienteID = null;
            vista.NombreCuentaCliente = null;

            // Datos de Contrato
            vista.NumeroContrato = null;
            vista.NumeroEcononomico = null;
            vista.NumeroSerie = null;
            vista.FechaInicioContrato = null;
            vista.FechaFinContrato = null;
            vista.LimpiarContratosEncontrados();
            DesplegarListadoEstatus();

            #region SC0020
            this.EstablecerFiltros();
            #endregion SC0020

            this.EstablecerSeguridad();

            this.CargarSucursalesAutorizadas();//SC_0051
        }
        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas() {
            if (vista.SucursalID != null)
                return;

            if (this.vista.SucursalesAutorizadas != null)
                if (this.vista.SucursalesAutorizadas.Count > 0)
                    return;

            var lstSucursales = Facade.SDNI.BR.FacadeBR.ConsultarSucursalesSeguridad(dataContext,
                                                                                        new SeguridadBO(Guid.Empty,
                                                                                                        this.vista
                                                                                                            .Usuario,
                                                                                                        new AdscripcionBO
                                                                                                        {
                                                                                                            UnidadOperativa
                                                                                                                =
                                                                                                                this
                                                                                                        .vista
                                                                                                        .UnidadOperativa
                                                                                                        }));
            this.vista.SucursalesAutorizadas = lstSucursales.ConvertAll(x => (object)x); //SC00
        }
        /// <summary>
        /// Carga y muestra el listados de estatus de contratos en la vista
        /// </summary>
        private void DesplegarListadoEstatus() {
            List<EEstatusContrato> listado = new List<EEstatusContrato>(Enum.GetValues(typeof(EEstatusContrato)).Cast<EEstatusContrato>());
            listado = new List<EEstatusContrato>(listado.Where(n => n != EEstatusContrato.Eliminado));
            vista.CargarEstatus(listado);
        }

        /// <summary>
        /// Obtiene un ContratoRD desde la Vista
        /// </summary>
        /// <returns></returns>
        public ContratoPSLBOF InterfazUsuarioADatos() {
            ContratoPSLBOF contrato = new ContratoPSLBOF
            {
                Cliente = new CuentaClienteIdealeaseBO
                {
                    Cliente = new ClienteBO(),
                    UnidadOperativa = new UnidadOperativaBO()
                },
                Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() }
            };

            // Datos Generales
            if (vista.UnidadOperativa != null) {
                contrato.Cliente.UnidadOperativa = vista.UnidadOperativa;
                contrato.Sucursal.UnidadOperativa = vista.UnidadOperativa;
            }

            // Datos del Cliente
            if (vista.CuentaClienteID != null) contrato.Cliente.Id = vista.CuentaClienteID;

            if (vista.NombreCuentaCliente != null && !string.IsNullOrEmpty(vista.NombreCuentaCliente.Trim())) contrato.Cliente.Nombre = vista.NombreCuentaCliente.Trim();

            if (vista.ClienteID != null) contrato.Cliente.Cliente.Id = vista.ClienteID;

            // Datos del Contrato
            if (vista.NumeroContrato != null && !string.IsNullOrEmpty(vista.NumeroContrato.Trim())) contrato.NumeroContrato = vista.NumeroContrato.Trim().ToUpper();

            if (vista.FechaInicioContrato != null) contrato.FechaContrato = vista.FechaInicioContrato;

            if (vista.FechaFinContrato != null) contrato.FechaFin = vista.FechaFinContrato;

            if (vista.Estatus != null) contrato.Estatus = vista.Estatus;

            if (vista.NumeroSerie != null && !string.IsNullOrEmpty(vista.NumeroSerie.Trim())) contrato.NumeroSerie = vista.NumeroSerie.Trim().ToUpper();

            if (vista.NumeroEcononomico != null && !string.IsNullOrEmpty(vista.NumeroEcononomico.Trim())) contrato.NumeroEconomico = vista.NumeroEcononomico.Trim().ToUpper();

            if (vista.SucursalID != null) {
                contrato.Sucursal.Id = this.vista.SucursalID;
                contrato.Sucursal.Nombre = this.vista.SucursalNombre;
            } else
                contrato.SucursalesConsulta = this.vista.SucursalesAutorizadas.ConvertAll(x => (SucursalBO)x);//SC_0051

            if (vista.UnidadOperativa != null) {
                List<int> lstTiposContrato = new List<int>();
                lstTiposContrato.Add((int)ETipoContrato.ROC);
                contrato.TiposContrato = lstTiposContrato;
            }


            return contrato;
        }

        #region SC0020

        private void EstablecerFiltros() {
            try {
                Dictionary<string, object> elementosFiltro = this.ObtenerPaqueteNavegacion() as Dictionary<string, object>;
                if (elementosFiltro != null) {
                    ///establecer objeto filtro en la interfaz
                    ContratoPSLBOF contratoFiltro = (ContratoPSLBOF)elementosFiltro["ContratoPSLFiltro"];
                    this.EstablecerPaqueteDatos(contratoFiltro);

                    ///verificar consulta inmediata
                    bool bandera = (bool)elementosFiltro["Bandera"];

                    if (bandera == true) {
                        this.ConsultarContratos();
                        ///establecer paginación del grid
                        int pagGrid = Convert.ToInt32(elementosFiltro["PagActGrid"]);
                        this.vista.EstablecerPagResultados(pagGrid);
                    }

                    this.vista.LimpiarPaqueteNavegacion();
                }
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencias con el diccionario de sesión", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// rellena la UI con los datos del contrato de sesión
        /// </summary>
        /// <returns></returns>
        public void EstablecerPaqueteDatos(ContratoPSLBOF contratoFiltro) {
            if (contratoFiltro != null) {

                //Datos del Contrato
                this.vista.NumeroContrato = contratoFiltro.NumeroContrato;
                this.vista.FechaFinContrato = contratoFiltro.FechaFin;
                this.vista.FechaInicioContrato = contratoFiltro.FechaContrato;
                this.vista.Estatus = contratoFiltro.Estatus;
                this.vista.NumeroSerie = contratoFiltro.NumeroSerie;
                this.vista.NumeroEcononomico = contratoFiltro.NumeroEconomico;

                if (contratoFiltro.Sucursal != null) {
                    this.vista.SucursalID = contratoFiltro.Sucursal.Id;
                    this.vista.SucursalNombre = contratoFiltro.Sucursal.Nombre;
                }

                // Datos del Cliente
                if (contratoFiltro.Cliente != null) {
                    this.vista.CuentaClienteID = contratoFiltro.Cliente.Id;
                    this.vista.NombreCuentaCliente = contratoFiltro.Cliente.Nombre;
                    this.vista.ClienteID = contratoFiltro.Cliente.Cliente.Id;
                }
            }
        }

        private object ObtenerPaqueteNavegacion() {
            return this.vista.ObtenerPaqueteNavegacion();
        }

        private void EstablecerPaqueteNavegacion(int? ContratoID, string ClaveContrato) {
            ContratoPSLBOF contrato = this.vista.ContratosEncontrados.Find(cont => cont.ContratoID == ContratoID);

            #region SC0020
            ContratoPSLBOF contratoFiltro = InterfazUsuarioADatos();
            int numPagGrid = this.vista.ObtenerPagResultados();
            bool bandera = true;

            Dictionary<string, object> elementosFiltro = new Dictionary<string, object>();
            elementosFiltro.Add("ContratoPSLFiltro", contratoFiltro);
            elementosFiltro.Add("PagActGrid", numPagGrid);
            elementosFiltro.Add("Bandera", bandera);

            this.vista.EstablecerPaqueteNavegacion(ClaveContrato, contrato, elementosFiltro);
            #endregion SC0020

        }

        #endregion SC0020

        /// <summary>
        /// Consulta los contratos de acuerdo a los parámetros proporcionados en la vista
        /// </summary>
        public void ConsultarContratos() {
            try {
                ContratoPSLBOF contrato = InterfazUsuarioADatos();

                List<ContratoPSLBOF> contratos = Controlador.Consultar(dataContext, contrato);
                if (contratos.Any())
                    contratos = contratos.OrderBy(x => x.NumeroContrato).ToList();

                List<ContratoPSLBOF> lstNew = new List<ContratoPSLBOF>();
                foreach (ContratoPSLBOF c in contratos) {
                    if (c == null) continue;
                    if (c.Sucursal != null && c.Sucursal.UnidadOperativa != null && c.Sucursal.UnidadOperativa.Id != null && vista.UnidadOperativa != null && vista.UnidadOperativa.Id != null) {
                        if (c.Sucursal.UnidadOperativa.Id != vista.UnidadOperativa.Id) continue;
                    }

                    lstNew.Add(c);
                }

                vista.CargarContratosEncontrados(lstNew);

                if (contratos.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ConsultarContratos: " + ex.Message);
            }
        }

        /// <summary>
        /// Cambia a la vista de detalle del contrato seleccionado
        /// </summary>
        /// <param name="ContratoID"></param>
        public void IrADetalle(int? ContratoID) {
            if (ContratoID != null) {
                this.EstablecerPaqueteNavegacion(ContratoID, "ContratoPSLBO");

                vista.IrADetalle();
            } else {
                vista.MostrarMensaje("No se ha proporcionado un contrato para visualizar el detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Búsqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la búsqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF { Nombre = vista.NombreCuentaCliente, UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO(), Activo = true };
                    obj = cliente;
                    break;
                case "Sucursal":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = this.vista.UnidadOperativa;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = this.vista.Usuario;
                    obj = sucursal;
                    break;
                case "UnidadIdealease":
                    UnidadBOF unidad = new UnidadBOF();

                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    obj = unidad;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Resultado del Buscador
        /// </summary>
        /// <param name="catalogo">Catalogo en el que se realizo la búsqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();

                    vista.CuentaClienteID = cliente.Id;

                    vista.ClienteID = cliente.Cliente.Id;

                    vista.NombreCuentaCliente = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    break;
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null && sucursal.Id != null)
                        this.vista.SucursalID = sucursal.Id;
                    else
                        this.vista.SucursalID = null;

                    if (sucursal != null && sucursal.Nombre != null)
                        this.vista.SucursalNombre = sucursal.Nombre;
                    else
                        this.vista.SucursalNombre = null;
                    break;

                case "UnidadIdealease":
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    if (unidad.NumeroSerie != null) {
                        vista.NumeroSerie = unidad.NumeroSerie;
                    } else {
                        vista.NumeroSerie = string.Empty;
                    }

                    break;
            }
        }
        #endregion

        public void EstablecerSeguridad() {
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadAdscripcionID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID };
                AdscripcionBO adscripcion = new AdscripcionBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID } };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dataContext, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "UI INSERTAR"))
                    this.vista.PermitirRegistrar(false);
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permitidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuentra dentro de la lista de acciones permitidas se devuelve true. En caso ocntario false. bool</returns>        
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion) {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        #region Plantillas
        /// <summary>
        /// Imprime la plantilla del contrato
        /// </summary>
        public void ImprimirPlantillaContratoROC() {
            try {

                ContratoPSLBR contratoBR = new ContratoPSLBR();
                AppSettingsReader n = new AppSettingsReader();
                int moduloID = (int)this.vista.ModuloID;
                //Dictionary<string, Object> datosImprimir = contratoBR.ObtenerPlantillaContrato(dataContext, moduloID, this.vista.UnidadAdscripcionID.Value);
                Dictionary<string, Object> datosImprimir = new Dictionary<string, object>();
                vista.EstablecerPaqueteNavegacionImprimir("CU014", datosImprimir);
                vista.IrAImprimir();


            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ImprimirPlantillaContratoPSL:Inconsistencia al intentar cargar los datos a imprimir." + ex.Message);
            }
        }
        /// <summary>
        /// Genera el reporte para la impresión de la plantilla del Check List
        /// </summary>
        public void ImprimirPlantillaCheckListROC() {
            try {
                ContratoPSLBR contratoBR = new ContratoPSLBR();
                AppSettingsReader n = new AppSettingsReader();
                int moduloID = (int)this.vista.ModuloID;
                Dictionary<string, Object> datosImprimir = contratoBR.ObtenerPlantillaCheckList(dataContext, new ContratoPSLBO { Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID.Value } } }, moduloID);
                vista.EstablecerPaqueteNavegacionImprimir("CU012", datosImprimir);
                vista.IrAImprimir();
            } catch (Exception ex) {
                throw new Exception(nombreClase + ".ImprimirPlantillaCheckListPSL: Inconsistencia al intentar cargar los datos a imprimir." + ex.Message);
            }
        }
        #endregion

        #endregion
    }
}