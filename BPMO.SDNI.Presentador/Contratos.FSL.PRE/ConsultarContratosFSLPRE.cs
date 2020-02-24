// Satisface al Caso de uso CU022 - Consultar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.FSL.BOF;
using BPMO.SDNI.Contratos.FSL.BR;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
    public class ConsultarContratosFSLPRE
    {
        #region Atributos
        private readonly IConsultarContratosFSLVIS vista;

        /// <summary>
        /// Controlador de Contratos FSL
        /// </summary>
        private readonly ContratoFSLBR Controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private readonly IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarContratosFSLPRE";

        /// <summary>
        /// Presentador del Control de Herramientas
        /// </summary>
        private readonly ucHerramientasFSLPRE HerramientasPRE;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="vistaActual">Vista Actual</param>
        public ConsultarContratosFSLPRE(IConsultarContratosFSLVIS vistaActual, ucHerramientasFSLPRE herramientas)
        {
            try
            {
                if (vistaActual != null) vista = vistaActual;
                else throw new Exception("La vista proprocionada no puede ser null.");

                HerramientasPRE = herramientas;

                dataContext = Facade.SDNI.BR.FacadeBR.ObtenerConexion();
                Controlador = new ContratoFSLBR();
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
            this.vista.LimpiarSession();
            //Datos de Cliente
            vista.ClienteID = null;
            vista.CuentaClienteID = null;
            vista.NombreCuentaCliente = null;

            // Datos de Contrato
            vista.NumeroContrato = null;
            vista.FechaInicioContrato = null;
            vista.FechaTerminoContrato = null;

            DesplegarListadoEstatus();

            // Barra de Herramientas
            HerramientasPRE.vista.OcultarCerrarContrato();
            HerramientasPRE.vista.OcultarEditarContrato();
            HerramientasPRE.vista.OcultarEliminarContrato();
            HerramientasPRE.vista.OcultarEstatusContrato();
            HerramientasPRE.vista.OcultarMenuImpresion();
            HerramientasPRE.vista.OcultarNoContrato();

            this.EstablecerSeguridad();//SC_0008

            this.CargarSucursalesAutorizadas();//SC_0051
        }
        //SC_0051
        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void CargarSucursalesAutorizadas()
        {
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
        //End_SC_0051
        /// <summary>
        /// Carga y muestra el listados de estatus de contratos en la vista
        /// </summary>
        private void DesplegarListadoEstatus()
        {
            List<EEstatusContrato> listado = new List<EEstatusContrato>(Enum.GetValues(typeof(EEstatusContrato)).Cast<EEstatusContrato>());
            listado = new List<EEstatusContrato>(listado.Where(n => n == EEstatusContrato.Borrador || n == EEstatusContrato.Cerrado || n == EEstatusContrato.EnCurso));
            vista.CargarEstatus(listado);
        }

        /// <summary>
        /// Obtiene un ContratoFSL desde la Vista
        /// </summary>
        /// <returns></returns>
        public ContratoFSLBOF InterfazUsuarioADatos()
        {
            ContratoFSLBOF contrato = new ContratoFSLBOF
                {
                    Cliente = new CuentaClienteIdealeaseBO
                            {
                                Cliente = new ClienteBO(),
                                UnidadOperativa = new UnidadOperativaBO()
                            },
                    Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO() }
                };

            // Datos Generales
            if (vista.UnidadOperativa != null)
            {
                contrato.Cliente.UnidadOperativa = vista.UnidadOperativa;
                contrato.Sucursal.UnidadOperativa = vista.UnidadOperativa;
            }

            // Datos del Cliente
            if (vista.CuentaClienteID != null) contrato.Cliente.Id = vista.CuentaClienteID;

            if (vista.NombreCuentaCliente != null && !string.IsNullOrEmpty(vista.NombreCuentaCliente.Trim())) contrato.Cliente.Nombre = vista.NombreCuentaCliente.Trim();

            if (vista.ClienteID != null) contrato.Cliente.Cliente.Id = vista.ClienteID;

            // Datos del Contrato
            if (vista.NumeroContrato != null && !string.IsNullOrEmpty(vista.NumeroContrato.Trim())) contrato.NumeroContrato = vista.NumeroContrato.Trim().ToUpper();

            if (vista.PlazoMeses != null) contrato.Plazo = vista.PlazoMeses;

            if (vista.FechaInicioContrato != null) contrato.FechaInicioContrato = vista.FechaInicioContrato;

            if (vista.FechaTerminoContrato != null) contrato.FechaTerminoContrato = vista.FechaTerminoContrato;

            if (vista.Estatus != null) contrato.Estatus = vista.Estatus;

            if (vista.SucursalID != null)
            {
                contrato.Sucursal.Id = this.vista.SucursalID;
                contrato.Sucursal.Nombre = this.vista.SucursalNombre;
            }
            else
                contrato.SucursalesConsulta = this.vista.SucursalesAutorizadas.ConvertAll(x => (SucursalBO)x);//SC_0051

            return contrato;
        }

        /// <summary>
        /// Consulta los contratos de acuerdo a los parametros proporcionados en la vista
        /// </summary>
        public void ConsultarContratos()
        {
            try
            {
                ContratoFSLBOF contrato = InterfazUsuarioADatos();

                List<ContratoFSLBOF> contratos = Controlador.ConsultarParcial(dataContext, contrato);
                contratos = contratos != null
                                ? new List<ContratoFSLBOF>(contratos.Where(
                                    cont =>
                                    cont.Estatus == EEstatusContrato.Borrador ||
                                    cont.Estatus == EEstatusContrato.Cerrado ||
                                    cont.Estatus == EEstatusContrato.EnCurso))
                                : new List<ContratoFSLBOF>();

                List<ContratoFSLBOF> lstNew = new List<ContratoFSLBOF>();
                foreach (ContratoFSLBOF c in contratos)
                {
                    if (c == null) continue;
                    if (c.Sucursal != null && c.Sucursal.UnidadOperativa != null && c.Sucursal.UnidadOperativa.Id != null && vista.UnidadOperativa != null && vista.UnidadOperativa.Id != null)
                    {
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
                vista.MostrarMensaje("Inconsistencias al consultar los contratos F.S.L.", ETipoMensajeIU.ERROR, nombreClase + ".ConsultarContratos: " + ex.Message);
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
                vista.EstablecerPaqueteNavegacion("ContratoFSLBO", ContratoID);
                vista.LimpiarSession();
                vista.IrADetalle();
            }
            else
            {
                vista.MostrarMensaje("No se ha proporcionado un contrato para visualizar el detalle.", ETipoMensajeIU.ADVERTENCIA);
            }
        }

        public void ImprimirFormatoContrato(bool personaFisica)
        {
            try
            {
                //True si quieres que sea fisica false si squieres que sea moral
                Dictionary<string, object> datos = Controlador.ObtenerPlantillaContratoMaestro(personaFisica);
                vista.EstablecerPaqueteNavegacionImprimir("CU018A", datos);
                vista.IrAImprimir();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias al Intentar  desplegar el Contrato Maestro", ETipoMensajeIU.ERROR, nombreClase + ".ImprimirFormatoContrato: " + ex.Message);
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

            switch (catalogo)
            {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF { Nombre = vista.NombreCuentaCliente, UnidadOperativa = vista.UnidadOperativa, Cliente = new ClienteBO(), Activo = true};
                    obj = cliente;
                    break;
                case "Sucursal":
                    Facade.SDNI.BOF.SucursalBOF sucursal = new Facade.SDNI.BOF.SucursalBOF();
                    sucursal.UnidadOperativa = this.vista.UnidadOperativa;
                    sucursal.Nombre = this.vista.SucursalNombre;
                    sucursal.Usuario = this.vista.Usuario;
                    obj = sucursal;
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
            }
        }
        #endregion

        #region SC_0008

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
                //Se valida si el usuario tiene permiso para imprimir
                if (!this.ExisteAccion(lst, "UI IMPRIMIR"))
                    this.PermitirImprimir();
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

        /// <summary>
        /// Deshabilita las opciones de Imprimir de la barra de herramientas
        /// </summary>
        private void PermitirImprimir()
        {
            this.HerramientasPRE.DeshabilitarMenuImprimir();
        }

        #endregion

        #region SC0038
        /// <summary>
        /// despliega en la vista las platillas correspondientes al modulo
        /// </summary>
        public void CargarPlantillas()
        {
            var controlador = new PlantillaBR();

            var precargados = this.vista.ObtenerPlantillas("ucContratosFSL");
            var resultado = new List<object>();

            if (precargados != null)
                if (precargados.Count > 0)
                    resultado = precargados;

            if (resultado.Count <= 0)
            {
                var lista = controlador.Consultar(this.dataContext, new PlantillaBO { Activo = true, TipoPlantilla = EModulo.FSL });

                if (ReferenceEquals(lista, null))
                    lista = new List<PlantillaBO>();

                resultado = lista.ConvertAll(p => (object)p);
            }
            this.HerramientasPRE.CargarArchivos(resultado);
        }
        #endregion
        #endregion
    }
}