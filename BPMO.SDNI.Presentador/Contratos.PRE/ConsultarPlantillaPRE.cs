// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.BR;
using BPMO.SDNI.Contratos.VIS;

namespace BPMO.SDNI.Contratos.PRE
{
    /// <summary>
    /// Presentador para la consulta de los documentos que serán usados como plantilla en los contratos
    /// </summary>
    public class ConsultarPlantillaPRE
    {
        #region Atributos
        /// <summary>
        /// Vista para la página de consulta de documentos
        /// </summary>
        private readonly IConsultarPlantillaVIS vista;
        /// <summary>
        /// Vista para el user control de resultados
        /// </summary>
        private readonly IucListadoPlantillasVIS vistaDocs;
        /// <summary>
        /// Presentador del user control de los documentos registrados
        /// </summary>
        private readonly ucListadoPlantillasPRE presentadorDoctos;
        /// <summary>
        /// Proveé la conexión a la BD
        /// </summary>
        private readonly IDataContext dctx;
        /// <summary>
        /// Nombre de la clase que se usará en los mensajes de error
        /// </summary>
        private const string nombreClase = "ConsultarPlantillaPRE";
        /// <summary>
        /// Controlador que ejecutará las acciones
        /// </summary>
        private readonly PlantillaBR controlador;
        #endregion

        #region Constructores
        /// <summary>
        /// Cosntructor del presentador para la página de consulta de docuemntos plantilla para los contratos
        /// </summary>
        /// <param name="vista">Vista de la página padre</param>
        /// <param name="vistaDocs">Vista del userControl de resultados</param>
        public ConsultarPlantillaPRE(IConsultarPlantillaVIS vista, IucListadoPlantillasVIS vistaDocs)
        {
            if (ReferenceEquals(vista, null))
                throw new Exception(String.Format("{0}: La vista proporcionada no puede ser nula", nombreClase));
            if (ReferenceEquals(vistaDocs, null))
                throw new Exception(String.Format("{0}: La vista proporcionada para la lista de los resultados de los documentos no puede ser nula", nombreClase));

            this.vista = vista;
            this.vistaDocs = vistaDocs;
            this.presentadorDoctos = new ucListadoPlantillasPRE(this.vistaDocs);
            this.controlador = new PlantillaBR();
            this.dctx = FacadeBR.ObtenerConexion();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Genera la lista de opciones para el tipo de plantilla en base a la enumeracion correspondiente
        ///<para>SI deseas visualizar los valores correspondientes para la enumeración consulta:</para>
        /// <seealso cref="BPMO.SDNI.Contratos.BO.EModulo"/>
        /// </summary>
        /// <returns>Diccionario con las posibles opciones de tipo</returns>
        private Dictionary<int, string> ObtenerTiposPlantilla()
        {
            try
            {
                string key = "";
                int value = 0;
                Dictionary<int, string> tipos = new Dictionary<int, string>();
                tipos.Add(-1, "TODOS");
                foreach (var tipo in Enum.GetValues(typeof(EModulo)))
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
                        key = Enum.GetName(typeof(EModulo), value);
                    }
                    tipos.Add(value, key);
                }
                return tipos;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.ObtenerTiposPlantilla:{1}Ocurrío una inconsistencia al intentar generar el listado de modulos de contrato{2}{3}", nombreClase, Environment.NewLine, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Limpia las propiedades de la vista
        /// </summary>
        private void LimpiarCampos()
        {
            this.vista.LimpiarSesion();
            this.vista.LimpiarDocumentosEncontrados();
            this.vista.Estatus = null;
            this.vista.Nombre = string.Empty;
            this.vista.TipoPlantilla = null;
        }
        /// <summary>
        /// Prepara la vista para la consulta de los documentos
        /// </summary>
        public void PrepararBusqueda()
        {
            this.vista.EstablecerOpcionesTipoPlantilla(this.ObtenerTiposPlantilla());
            this.LimpiarCampos();            
            this.EstablecerSeguridad();
        }
        /// <summary>
        /// Convierte los datos de la vista a un objeto de negocio
        /// </summary>
        /// <returns>Objeto de negocio creado a partir de los datos en la vista</returns>
        private object InterfazUsuarioADatos()
        {
            PlantillaBO bo = new PlantillaBO();

            bo.TipoArchivo = new TipoArchivoBO();
            bo.Auditoria = new AuditoriaBO();

            if (this.vista.Estatus.HasValue)
                bo.Activo = this.vista.Estatus.Value;

            if (!string.IsNullOrEmpty(this.vista.Nombre) && !string.IsNullOrWhiteSpace(this.vista.Nombre))
                bo.Nombre = this.vista.Nombre;

            if (this.vista.TipoPlantilla.HasValue)
                bo.TipoPlantilla = (EModulo)this.vista.TipoPlantilla.Value;

            return bo;
        }
        /// <summary>
        /// COnsulta los documentos que se hayan registrado en el sistema
        /// </summary>
        public void Consultar()
        {
            try
            {
                List<PlantillaBO> lista = this.controlador.Consultar(this.dctx, this.InterfazUsuarioADatos() as PlantillaBO);

                if (ReferenceEquals(lista, null))
                    lista = new List<PlantillaBO>();

                var resultado = lista.ConvertAll(p => (object)p);
                this.vistaDocs.Documentos = resultado;
                this.presentadorDoctos.CargarElementosEncontrados(resultado);
                if (lista.Count <= 0)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION, "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Ocurrio un problema al intentar consultar los documentos", ETipoMensajeIU.ERROR, string.Format("{0}.Consultar:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Elimina un documento del sistema
        /// </summary>
        private void Eliminar(PlantillaBO archivo)
        {
            try
            {
                //Se crea el objeto seguridad
                UsuarioBO usuario = new UsuarioBO() {Id = this.vista.UsuarioID};
                AdscripcionBO adscripcion = new AdscripcionBO()
                    {
                        UnidadOperativa = new UnidadOperativaBO() {Id = this.vista.UnidadOperativaID}
                    };
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

                if (archivo != null)
                {
                    var nuevo = new PlantillaBO {Id = archivo.Id.Value, Activo = false, Nombre = archivo.Nombre, NombreCorto = archivo.NombreCorto, TipoPlantilla = archivo.TipoPlantilla};
                    nuevo.Auditoria = new AuditoriaBO {FUA = DateTime.Now, UUA = this.vista.UsuarioID};
                    nuevo.TipoArchivo = archivo.TipoArchivo;
                    this.controlador.Actualizar(this.dctx, nuevo, archivo, seguridadBO);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.Eliminar:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }                       
        }
        /// <summary>
        /// Elimina un archivo del sistema
        /// </summary>
        public void EliminarArchivo()
        {
            try
            {
                var archivoID = this.vistaDocs.ArchivoEliminarID;   

                if (archivoID.HasValue)
                {
                    var archivos = this.vistaDocs.Documentos.ConvertAll(x => (PlantillaBO) x);
                    var archivo = archivos.FirstOrDefault(x => x.Id == archivoID.Value);
                    if (archivo != null)
                    {
                        this.Eliminar(archivo);

                        this.Consultar();
                        //Notificamos al usuario que el borrado fue exitoso
                        this.vista.MostrarMensaje("El documento fue eliminado exitosamente.", ETipoMensajeIU.EXITO, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.EliminarArchivo:{1}{2}", nombreClase, Environment.NewLine, ex.Message));
            }
        }
        /// <summary>
        /// Cambia la página de la vista de resultados
        /// </summary>
        /// <param name="nuevoIndicePagina">Página que se desea visualizar</param>
        public void CambiarPaginaResultado(int nuevoIndicePagina)
        {
            this.vista.IndicePaginaResultado = nuevoIndicePagina;
            this.vista.ActualizarResultado();
            this.vistaDocs.IndicePaginaResultado = nuevoIndicePagina;
            this.vistaDocs.ActualizarResultado();
        }
        /// <summary>
        /// Carga los archivos encontrados en la vista
        /// </summary>
        /// <param name="list">Lista con los archivos encontrados</param>
        public void CargarElementosEncontrados(List<object> list)
        {
            this.vistaDocs.Documentos = list;

            this.presentadorDoctos.CargarElementosEncontrados(list);
        }

        #region Seguridad
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
                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);
                //Se valida si el usuario tiene permiso para eliminar
                if (!this.ExisteAccion(lst, "ACTUALIZAR"))
                    this.vista.PermitirEliminar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}.{1} Excepción interna:{2}{3}", nombreClase, "EstablecerSeguridad", Environment.NewLine, ex.Message));
            }
        }
        #endregion        
        #endregion
    }
}