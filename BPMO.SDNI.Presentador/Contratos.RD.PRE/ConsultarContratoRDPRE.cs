//Satisface el Caso de uso CU003 - Consultar Contratos Renta Diaria
// Satisface al Caso de Uso CU014 - Imprimir Contrato de Renta Diaria
// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
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
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Contratos.RD.BOF;
using BPMO.SDNI.Contratos.RD.BR;
using BPMO.SDNI.Contratos.RD.VIS;
using BPMO.SDNI.Equipos.BOF;

namespace BPMO.SDNI.Contratos.RD.PRE
{
    public class ConsultarContratoRDPRE
    {

        #region Atributos

        private readonly IConsultarContratoRDVIS vista;

        /// <summary>
        /// Controlador de Contratos RD
        /// </summary>
        private readonly ContratoRDBR Controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarContratosRDPRE";

        /// <summary>
        /// Presentador del Control de Herramientas
        /// </summary>
        private readonly ucHerramientasRDPRE HerramientasPRE;

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="vistaActual">Vista Actual</param>
        
        public ConsultarContratoRDPRE(IConsultarContratoRDVIS vistaActual, ucHerramientasRDPRE herramientas)
        {
            try
            {
                if (vistaActual != null) vista = vistaActual;
                else throw new Exception("La vista proporcionada no puede ser nula.");

                HerramientasPRE = herramientas;

                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
                Controlador = new ContratoRDBR();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en la construcción del presentador", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarContratosFSLPRE: " + ex.Message);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Prepara la Vista para una nueva busqueda
        /// </summary>
        public void PrepararBusqueda()
        {
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

            // Barra de Herramientas
            HerramientasPRE.vista.OcultarCerrarContrato();
            HerramientasPRE.vista.OcultarEditarContrato();
            HerramientasPRE.vista.OcultarEliminarContrato();
            HerramientasPRE.vista.OcultarEstatusContrato();
            HerramientasPRE.vista.OcultarMenuImpresion();
            HerramientasPRE.vista.OcultarNoContrato();

            this.CargarSucursalesAutorizadas();

            #region SC0020
            this.EstablecerFiltros();
            #endregion SC0020

            this.EstablecerSeguridad();

        }
        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas() {
            if (this.vista.SucursalesAutorizadas == null || this.vista.SucursalesAutorizadas.Count == 0) {
                this.vista.SucursalesAutorizadas = FacadeBR.ConsultarSucursalesSeguridadSimple(
                    dataContext, new SeguridadBO(Guid.Empty, this.vista.Usuario, new AdscripcionBO { UnidadOperativa = this.vista.UnidadOperativa }));
            }
            this.vista.CargarSucursales(this.vista.SucursalesAutorizadas);
        }
        /// <summary>
        /// Carga y muestra el listados de estatus de contratos en la vista
        /// </summary>
        private void DesplegarListadoEstatus()
        {
            List<EEstatusContrato> listado = new List<EEstatusContrato>(Enum.GetValues(typeof(EEstatusContrato)).Cast<EEstatusContrato>());
            listado = new List<EEstatusContrato>(listado.Where(n => n != EEstatusContrato.Eliminado));
            vista.CargarEstatus(listado);
        }

        /// <summary>
        /// Obtiene un ContratoRD desde la Vista
        /// </summary>
        /// <returns></returns>
        public ContratoRDBOF InterfazUsuarioADatos()
        {
            ContratoRDBOF contrato = new ContratoRDBOF
            {
                Cliente = new CuentaClienteIdealeaseBO
                {
                    Cliente = new ClienteBO(),
                    UnidadOperativa = new UnidadOperativaBO()
                },
                Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() },
                Unidad = new UnidadBOF()
            };
            // Datos Generales
            if (vista.UnidadOperativa != null)
            {
                contrato.Cliente.UnidadOperativa = vista.UnidadOperativa;
                contrato.Sucursal.UnidadOperativa = vista.UnidadOperativa;
            }

            // Datos del Cliente
            if (vista.CuentaClienteID != null) {
                contrato.Cliente.Id = vista.CuentaClienteID;
            }
            if (vista.NombreCuentaCliente != null && !string.IsNullOrEmpty(vista.NombreCuentaCliente.Trim())){
                contrato.Cliente.Nombre = vista.NombreCuentaCliente.Trim();
            }
            if (vista.ClienteID != null){
                contrato.Cliente.Cliente.Id = vista.ClienteID;
            }
            // Datos del Contrato
            if (vista.NumeroContrato != null && !string.IsNullOrEmpty(vista.NumeroContrato.Trim())){
                contrato.NumeroContrato = vista.NumeroContrato.Trim().ToUpper();
            }
            if (vista.FechaInicioContrato != null){
                contrato.FechaContrato = vista.FechaInicioContrato;
            }
            if (vista.FechaFinContrato != null){
                contrato.FechaFin = vista.FechaFinContrato;
            }
            if (vista.Estatus != null){
                contrato.Estatus = vista.Estatus;
            }
            if (vista.UnidadID != null){
                contrato.Unidad.UnidadID = vista.UnidadID;
            }
            if (vista.NumeroSerie != null && !string.IsNullOrEmpty(vista.NumeroSerie.Trim())) {
                contrato.NumeroSerie = vista.NumeroSerie.Trim().ToUpper();
            }
            if (vista.NumeroEcononomico != null && !string.IsNullOrEmpty(vista.NumeroEcononomico.Trim())){
                contrato.NumeroEconomico = vista.NumeroEcononomico.Trim().ToUpper();
            }
            if (vista.SucursalID != null){
                contrato.Sucursal.Id = this.vista.SucursalID;
                contrato.Sucursal.Nombre = this.vista.SucursalNombre;
            }
            else{
                contrato.SucursalesConsulta = this.vista.SucursalesAutorizadas.ConvertAll(x => (SucursalBO)x);//SC_0051
            }

            return contrato;
        }

        #region SC0020

        private void EstablecerFiltros()
        {
            try
            {
                Dictionary<string, object> elementosFiltro = this.ObtenerPaqueteNavegacion() as Dictionary<string, object>;
                if (elementosFiltro != null)
                {
                    ///establecer objeto filtro en la interfaz
                    ContratoRDBOF contratoFiltro = (ContratoRDBOF)elementosFiltro["ContratoRDFiltro"];
                    this.EstablecerPaqueteDatos(contratoFiltro);

                    ///verificar consulta inmediata
                    bool bandera = (bool)elementosFiltro["Bandera"];

                    if (bandera == true)
                    {
                        this.ConsultarContratos();
                        ///establecer paginación del grid
                        int pagGrid = Convert.ToInt32(elementosFiltro["PagActGrid"]);
                        this.vista.EstablecerPagResultados(pagGrid);
                    }

                    this.vista.LimpiarPaqueteNavegacion();
                }
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias con el diccionario de sesión", ETipoMensajeIU.ERROR, nombreClase + ".Page_Load:" + ex.Message);
            }
        }

        /// <summary>
        /// rellena la UI con los datos del contrato de sesión
        /// </summary>
        /// <returns></returns>
        public void EstablecerPaqueteDatos(ContratoRDBOF contratoFiltro)
        {
            if (contratoFiltro != null)
            {
                //Datos del Contrato
                this.vista.NumeroContrato = contratoFiltro.NumeroContrato;
                this.vista.FechaFinContrato = contratoFiltro.FechaFin;
                this.vista.FechaInicioContrato = contratoFiltro.FechaContrato;
                this.vista.Estatus = contratoFiltro.Estatus;
                this.vista.NumeroSerie = contratoFiltro.NumeroSerie;
                this.vista.NumeroEcononomico = contratoFiltro.NumeroEconomico;
                // Datos del Cliente
                if (contratoFiltro.Cliente != null)
                {
                    this.vista.CuentaClienteID = contratoFiltro.Cliente.Id;
                    this.vista.NombreCuentaCliente = contratoFiltro.Cliente.Nombre;
                    this.vista.ClienteID = contratoFiltro.Cliente.Cliente.Id;
                }
            }
        }

        private object ObtenerPaqueteNavegacion()
        {
            return this.vista.ObtenerPaqueteNavegacion();
        }

        private void EstablecerPaqueteNavegacion(int? ContratoID, string ClaveContrato)
        {
            ContratoRDBOF contrato = this.vista.ContratosEncontrados.Find(cont => cont.ContratoID == ContratoID);
            
            #region SC0020
            ContratoRDBOF contratoFiltro = InterfazUsuarioADatos();
            int numPagGrid = this.vista.ObtenerPagResultados();
            bool bandera = true;

            Dictionary<string, object> elementosFiltro = new Dictionary<string, object>();
            elementosFiltro.Add("ContratoRDFiltro", contratoFiltro);
            elementosFiltro.Add("PagActGrid", numPagGrid);
            elementosFiltro.Add("Bandera", bandera);

            this.vista.EstablecerPaqueteNavegacion(ClaveContrato, contrato, elementosFiltro);
            #endregion SC0020

        }

        #endregion SC0020

        /// <summary>
        /// Consulta los contratos de acuerdo a los parametros proporcionados en la vista
        /// </summary>
        public void ConsultarContratos()
        {
            try
            {
                ContratoRDBOF contrato = InterfazUsuarioADatos();

                List<ContratoRDBOF> contratos = Controlador.Consultar(dataContext, contrato);
                if(contratos.Any())
                    contratos = contratos.OrderBy(x => x.NumeroContrato).ToList();

                List<ContratoRDBOF> lstNew = new List<ContratoRDBOF>();
                foreach (ContratoRDBOF c in contratos) {
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
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ConsultarContratos: " + ex.Message);
            }
        }

        /// <summary>
        /// Cambia a la vista de detalle del contrato seleccionado
        /// </summary>
        /// <param name="ContratoID"></param>
        public void IrADetalle(int? ContratoID)
        {
            if (ContratoID != null)
            {
                this.EstablecerPaqueteNavegacion(ContratoID, "ContratoRDBO");

                vista.IrADetalle();
            }
            else
            {
                vista.MostrarMensaje("No se ha proporcionado un contrato para visualizar el detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo) {
                case "CuentaClienteIdealease":
                    ClienteBO clienteBo = new ClienteBO();
                    clienteBo.Id = vista.ClienteID ?? vista.ClienteID;                    
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF { Nombre = vista.NombreCuentaCliente, 
                                            UnidadOperativa = vista.UnidadOperativa, 
                                                Cliente = new ClienteBO(){Id = clienteBo.Id}, Activo = true};
                    obj = cliente;
                    break;
               
                case "UnidadIdealease":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.UnidadID = vista.UnidadID ?? vista.UnidadID;
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
        /// <param name="catalogo">Catalogo en el que se realizo la busqueda</param>
        /// <param name="selecto">Objeto Resultante</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto)
        {
            switch (catalogo)
            {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();

                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();

                    vista.CuentaClienteID = cliente.Id;
                    vista.ClienteID = cliente.Cliente.Id;
                    vista.NombreCuentaCliente = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    break;
                
                case "UnidadIdealease":
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    vista.UnidadID = unidad.UnidadID ?? unidad.UnidadID;
                    if (unidad.NumeroSerie != null){                        
                        vista.NumeroSerie = unidad.NumeroSerie;
                    }
                    else {
                        vista.NumeroSerie = string.Empty;
                    }
                    break;
            }
        }
        #endregion

        public void EstablecerSeguridad()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Valida una acción en especifico dentro de la lista de acciones permtidas para la pagina
        /// </summary>
        /// <param name="acciones">Listado de acciones permitidas para la página</param>
        /// <param name="accion">Acción que se desea validar</param>
        /// <returns>si la acción a evaluar se encuantra dentro de la lista de acciones permitidas se devuelve true. En caso ocntario false. bool</returns>        
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }
        
        #region CU012
        /// <summary>
        /// Imprime la plantilla del contrato
        /// </summary>
        /// CU014
        public void ImprimirPlantillaContratoRD()
        {
            try
            {

                ContratoRDBR contratoBR = new ContratoRDBR();
                AppSettingsReader n = new AppSettingsReader();
                int moduloID = Convert.ToInt32(n.GetValue("ModuloID", System.Type.GetType("System.Int32")));
                Dictionary<string, Object> datosImprimir = contratoBR.ObtenerPlantillaContrato(dataContext, moduloID, this.vista.UnidadAdscripcionID.Value);
                vista.EstablecerPaqueteNavegacionImprimir("CU014", datosImprimir);
                vista.IrAImprimir();


            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ImprimirPlantillaContratoRD:Inconsistencia al intentar cargar los datos a imprimir." + ex.Message);
            }
        }
        /// <summary>
        /// Genera el reporte para la impresión de la plantilla del Check List
        /// </summary>
        public void ImprimirPlantillaCheckListRD()
        {
            try
            {
                ContratoRDBR contratoBR = new ContratoRDBR();
                AppSettingsReader n = new AppSettingsReader();
                int moduloID = Convert.ToInt32(n.GetValue("ModuloID", typeof(int)));
                Dictionary<string, Object> datosImprimir = contratoBR.ObtenerPlantillaCheckList(dataContext, new ContratoRDBO { Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadAdscripcionID.Value } } }, moduloID);
                vista.EstablecerPaqueteNavegacionImprimir("CU012", datosImprimir);
                vista.IrAImprimir();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ImprimirPlantillaCheckListRD: Inconsistencia al intentar cargar los datos a imprimir." + ex.Message);
            }
        }
        #endregion

        #region SC0038
        /// <summary>
        /// Despliega en la vista las plantillas correspondientes al módulo
        /// </summary>
        public void CargarPlantillas()
        {
            var controlador = new PlantillaBR();

            var precargados = this.vista.ObtenerPlantillas("ucContratosRentaDiaria");
            var resultado = new List<object>();

            if (precargados != null)
                if (precargados.Count > 0)
                    resultado = precargados;

            if (resultado.Count <= 0)
            {
                var lista = controlador.Consultar(this.dataContext, new PlantillaBO { Activo = true, TipoPlantilla = EModulo.RD });

                if (ReferenceEquals(lista, null))
                    lista = new List<PlantillaBO>();

                resultado = lista.ConvertAll(p => (object)p);
            }

            this.vista.CargarArchivos(resultado);
        }
        #endregion
        #endregion        
    }
}