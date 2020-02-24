using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Contratos.PSL.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Contratos.PSL.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Contratos.PSL.BOF;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Flota.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.BOF;
using BPMO.SDNI.Equipos.BOF;
using BPMO.Facade.SDNI.BOF;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Contratos.PSL.PRE
{
    public class ConsultarListadoVerificacionPSLPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la página de consulta de chek list
        /// </summary>
        private readonly IConsultarListadoVerificacionPSLVIS vista;
        /// <summary>
        /// Proveé la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "ConsultarListadoVerificacionPRE";
        /// <summary>
        /// Controlador que ejecutará las acciones
        /// </summary>
        private readonly ContratoPSLBR controlador;
        #endregion

        #region Constructor

        public ConsultarListadoVerificacionPSLPRE(IConsultarListadoVerificacionPSLVIS vista)
        {
            if (ReferenceEquals(vista, null))
                throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", nombreClase));
            this.vista = vista;

            this.controlador = new ContratoPSLBR();
            this.dctx = FacadeBR.ObtenerConexion();
        }

        #endregion

        #region Métodos

        private Dictionary<int, string> ObtenerTiposListado()
        {
            try
            {
                string key = "";
                int value = 0;
                Dictionary<int, string> tipos = new Dictionary<int, string>();
                tipos.Add(-1, "TODOS");
                foreach (var tipo in Enum.GetValues(typeof(ETipoListadoVerificacion)))
                {
                    var query =
                        tipo.GetType()
                            .GetField(tipo.ToString())
                            .GetCustomAttributes(true)
                            .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)));
                    value = Convert.ToInt32(tipo);
                    if (query.Any())
                    {
                        key =
                            (tipo.GetType()
                                 .GetField(tipo.ToString())
                                 .GetCustomAttributes(true)
                                 .Where(a => a.GetType().Equals(typeof(System.ComponentModel.DescriptionAttribute)))
                                 .FirstOrDefault() as System.ComponentModel.DescriptionAttribute).Description;
                    }
                    else
                    {
                        key = Enum.GetName(typeof(ETipoListadoVerificacion), value);
                    }
                    tipos.Add(value, key);
                }
                return tipos;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ObtenerTiposListado: Ocurrío una inconsistencia al intentar generar el listado de tipos de Check List." + Environment.NewLine + ex.Message);
            }
        }
        /// <summary>
        /// Recupera las sucursales a las que el usuario autenticado tiene permiso e acceder 
        /// </summary>
        private void DesplegarSucursalesAutorizadas() {
            if (this.vista.SucursalesAutorizadas == null || this.vista.SucursalesAutorizadas.Count == 0) {
                this.vista.SucursalesAutorizadas = FacadeBR.ConsultarSucursalesSeguridadSimple(
                    this.dctx, new SeguridadBO(Guid.Empty, new UsuarioBO() { Id = this.vista.UsuarioID },
                        new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } }));
            }
            this.vista.CargarSucursales(this.vista.SucursalesAutorizadas);
        }
        /// <summary>
        /// Prepara la vita para la consulta de los check list
        /// </summary>
        public void PrepararBusqueda()
        {
            this.vista.EstablecerOpcionesTipoListado(this.ObtenerTiposListado());
            this.DesplegarSucursalesAutorizadas();
            this.LimpiarCampos();

            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Limpia las propiedades de la vista
        /// </summary>
        private void LimpiarCampos() {
            this.vista.LimpiarSesion();
            this.vista.LimpiarListadosEncontrados();
            this.vista.ClienteID = null;
            this.vista.ClienteNombre = string.Empty;
            this.vista.CuentaClienteID = null;
            this.vista.ModeloID = null;
            this.vista.ModeloNombre = string.Empty;
            this.vista.NumeroContrato = string.Empty;
            this.vista.NumeroEconomico = string.Empty;
            this.vista.NumeroSerie = string.Empty;
            this.vista.TipoListado = null;
        }
        
        /// <summary>
        /// Convierte los datos de la vista a un objeto de negocio
        /// </summary>
        /// <returns>Objeto de negocio creado a partir del os datos en la vista</returns>
        private object InterfazUsuarioADatos()
        {
            ListadoVerificacionBOF bof = new ListadoVerificacionBOF();
            bof.CuentaCliente = new CuentaClienteIdealeaseBO { Cliente = new ClienteBO() };
            bof.Sucursal = new SucursalBO { UnidadOperativa = new UnidadOperativaBO { } };
            bof.Unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();

            if (this.vista.ClienteID.HasValue)
                bof.CuentaCliente.Cliente.Id = this.vista.ClienteID.Value;
            if (this.vista.CuentaClienteID.HasValue)
                bof.CuentaCliente.Id = this.vista.CuentaClienteID.Value;
            if (this.vista.ModeloID.HasValue)
                bof.Unidad.Modelo = new ModeloBO { Id = this.vista.ModeloID.Value };
            if (!string.IsNullOrEmpty(this.vista.NumeroContrato) && !string.IsNullOrWhiteSpace(this.vista.NumeroContrato))
                bof.NumeroContrato = this.vista.NumeroContrato;
            if (this.vista.UnidadID.HasValue) {
                bof.Unidad.UnidadID = this.vista.UnidadID;
            } else {
                if (!string.IsNullOrWhiteSpace(this.vista.NumeroEconomico))
                    bof.Unidad.NumeroEconomico = this.vista.NumeroEconomico;
                if (!string.IsNullOrWhiteSpace(this.vista.NumeroSerie))
                    bof.Unidad.NumeroSerie = this.vista.NumeroSerie;
            }
            if (this.vista.SucursalSeleccionada != null)
                bof.Sucursal.Id = this.vista.SucursalSeleccionada.Id;
            else
                bof.Sucursales = this.vista.SucursalesAutorizadas;

            if (this.vista.TipoListado.HasValue)
                bof.TipoListado = (ETipoListadoVerificacion)this.vista.TipoListado.Value;
            if (this.vista.UnidadOperativaID.HasValue)
                bof.Sucursal.UnidadOperativa.Id = this.vista.UnidadOperativaID.Value;

            return bof;
        }

        /// <summary>
        /// Obtiene un elemento de la flota en base a su identificador
        /// </summary>
        /// <param name="unidadID">Identificador de la unidad que se desea consultar</param>
        /// <returns>Elemento de flota que cumple con lso filtros proporcionados</returns>
        public object ObtenerElementoFlota(int unidadID)
        {
            try
            {
                ElementoFlotaBO elemento = new ElementoFlotaBO();
                elemento.Unidad = new BPMO.SDNI.Equipos.BO.UnidadBO();
                elemento.Unidad.UnidadID = unidadID;

                return elemento;
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ObtenerElementoFlota: " + ex.Message);
            }
        }

        public void ConsultarListados()
        {
            try
            {
                List<ListadoVerificacionBOF> lista = this.controlador.ConsultarListadoVerificacion(this.dctx, this.InterfazUsuarioADatos() as ListadoVerificacionBOF);

                if (ReferenceEquals(lista, null))
                    lista = new List<ListadoVerificacionBOF>();

                this.vista.Resultado = lista.ConvertAll(p => (object)p);
                this.vista.CargarElementosFlotaEncontrados(lista.ConvertAll(p => (object)p));

                if (lista.Count <= 0)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION, "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");


            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje(string.Format("{0}.{1}: Inconsistencias al consultar los listados", nombreClase, "ConsultarListados"), ETipoMensajeIU.ERROR, ex.Message);
            }
        }

        /// <summary>
        /// Redirige a la página de Detalle de Flota
        /// </summary>
        /// <param name="unidadID">Unidad que se desea visulizar a tdetalle</param>
        public void RedirigirAFlota(int? unidadID)
        {
            if (unidadID.HasValue)
            {
                this.vista.EstablecerPaqueteNavegacionFlota("DetalleFlotaUI", unidadID);
                this.vista.RedirigirAFlota();
            }
            else
                this.vista.MostrarMensaje("No se ha proporcionado el identificador de la unidad que se quiere consultar a detalle.", ETipoMensajeIU.ADVERTENCIA);
        }

        public void ImprimirListadoVerificacion(ListadoVerificacionBOF listado, int? lineaContratoID) {
            #region Obtener el Tipo de CheckList
            ETipoUnidad? tipoUnidad = null;
            switch (this.vista.UnidadOperativaID) {
                case (int)ETipoEmpresa.Construccion:
                    if (((EAreaConstruccion)listado.Unidad.Area) == EAreaConstruccion.RO || ((EAreaConstruccion)listado.Unidad.Area) == EAreaConstruccion.ROC) {
                        tipoUnidad = controlador.ObtenerTipoUnidadPorClave(this.dctx, (listado.Unidad).TipoEquipoServicio.NombreCorto, listado.Unidad);
                    }
                    if (((EAreaConstruccion)listado.Unidad.Area) == EAreaConstruccion.RE) {
                        tipoUnidad = ETipoUnidad.LV_SUBARRENDADO;
                    }
                    break;
                case (int)ETipoEmpresa.Generacion:
                case (int)ETipoEmpresa.Equinova:
                    tipoUnidad = ETipoUnidad.LV_ENTREGA_RECEPCION;
                    break;
                default:
                    throw new Exception("No es posible recuperar la información de la unidad necesaria para el registro del check list. Intente de nuevo por favor.");

            }
            #endregion

            if (!tipoUnidad.HasValue)
                this.vista.MostrarMensaje("No se ha podido derivar el Tipo de Unidad.", ETipoMensajeIU.ADVERTENCIA);

            Dictionary<string, object> datos = controlador.ObtenerDatosCheckList(
                this.dctx, new ContratoPSLBO() { ContratoID = listado.ContratoID }, this.vista.ModuloID.Value, tipoUnidad.Value, lineaContratoID);

            datos.Add("ContratoPSLBO", datos);
            this.vista.EstablecerPaqueteNavegacion("CheckListEntregaRO", datos);

            string error = string.Empty;
            this.vista.RedirigirAImprimir(error);
        }
        
        /// <summary>
        /// Redirige al check list de entrega de unidad
        /// </summary>
        /// <param name="contratoID">Contrato del cual depende la unidad</param>
        public void RedirigirARegistrarEntrega(int? contratoID, int? lineaContratoID)
        {
            if (contratoID.HasValue && lineaContratoID.HasValue)
            {
                this.vista.EstablecerPaqueteNavegacion("RegistrarEntregaPSLUI", contratoID);
                this.vista.EstablecerPaqueteNavegacion("LineaContratoPSLID", lineaContratoID);
                this.vista.RedirigirARegistrarEntrega();
            }
            else
                this.vista.MostrarMensaje("No se ha proporcionado el identificador del contrato para el CheckList.", ETipoMensajeIU.ADVERTENCIA);
        }

        /// <summary>
        /// Redirtige al check lsit de Recepción de la unidad
        /// </summary>
        /// <param name="contratoID">Contrato del cual depende al unidad</param>
        public void RedirigirARegistrarRecepcion(int? contratoID, int? lineaContratoID)
        {
            if (contratoID.HasValue && lineaContratoID.HasValue)
            {
                this.vista.EstablecerPaqueteNavegacion("RegistrarRecepcionPSLUI", contratoID);
                this.vista.EstablecerPaqueteNavegacion("LineaContratoPSLIDR", lineaContratoID);
                this.vista.RedirigirARegistrarRecepcion();
            }
            else
                this.vista.MostrarMensaje("No se ha proporcionado el identificador del contrato para el CheckList.", ETipoMensajeIU.ADVERTENCIA);
        }

        /// <summary>
        /// Establece el nuevo indice para la página
        /// </summary>
        /// <param name="nuevoIndicePagina"></param>
        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
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
        /// Verifica los permisos para los usuarios autenticados en el sistema
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (!this.vista.UsuarioID.HasValue)
                    throw new Exception("Es necesario proporcionar el usuario que esta identificado en el sistema.");
                if (!this.vista.UnidadOperativaID.HasValue)
                    throw new Exception("Es necesario proporcionar la unidad operativa del usuario autenticado en el sistema.");

                UsuarioBO usr = new UsuarioBO { Id = this.vista.UsuarioID.Value };
                AdscripcionBO adscripcion = new AdscripcionBO
                {
                    UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID.Value }
                };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usr, adscripcion);

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.{1} Excepción interna:{2}{3}", nombreClase, "EstablecerSeguridad", Environment.NewLine, ex.Message));
            }
        }

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns></returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;
            int aux = 0;

            switch (catalogo) {
                case "CuentaClienteIdealease":
                    CuentaClienteIdealeaseBOF cuentaCliente = new CuentaClienteIdealeaseBOF { UnidadOperativa = new UnidadOperativaBO() { Id = vista.UnidadOperativaID } };
                    if (int.TryParse(this.vista.ClienteNombre, out aux))
                        cuentaCliente.Id = aux;
                    else
                        cuentaCliente.Nombre = vista.ClienteNombre;
                    obj = cuentaCliente;
                    break;
                case "Unidad":
                    UnidadBOF unidad = new UnidadBOF();
                    unidad.Sucursal = new SucursalBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
                    if (!string.IsNullOrEmpty(vista.NumeroSerie))
                        unidad.NumeroSerie = vista.NumeroSerie;

                    obj = unidad;
                    break;
                case "Modelo":
                    ModeloBO modelo = new ModeloBO() { Auditoria = new AuditoriaBO(),  Marca = new MarcaBO() };
                    if (int.TryParse(this.vista.ModeloNombre, out aux))
                        modelo.Id = aux;
                    else
                        modelo.Nombre = vista.ModeloNombre;

                    obj = modelo;
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
                    #region Desplegar CuentaCliente
                    CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF() { Cliente = new ClienteBO() };
                    vista.CuentaClienteID = cliente.Id;
                    vista.ClienteID = cliente.Cliente.Id;

                    vista.ClienteNombre = !string.IsNullOrWhiteSpace(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    #endregion
                    break;
                case "Unidad":
                    #region Desplegar Unidad
                    UnidadBOF unidad = (UnidadBOF)selecto ?? new UnidadBOF();
                    vista.UnidadID = unidad.UnidadID;
                    vista.NumeroSerie = !string.IsNullOrWhiteSpace(unidad.NumeroSerie) ? unidad.NumeroSerie : string.Empty;
                    #endregion
                    break;
                case "Modelo":
                    #region Desplegar Modelo
                    ModeloBO modelo = (ModeloBO)selecto;
                    this.vista.ModeloID = modelo.Id;
                    this.vista.ModeloNombre = !string.IsNullOrWhiteSpace(modelo.Nombre) ? modelo.Nombre : string.Empty;
                    #endregion
                    break;
            }
        }
        #endregion

        #endregion
    }
}
