// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Registrar Configuración Sistema de Unidad Idealease.
    /// </summary>
    public class RegistrarConfiguracionSistemaUnidadPRE {

        #region Atributos

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IRegistrarConfiguracionSistemaUnidadVIS vista;

        /// <summary>
        /// Controlador de Contactos Cliente Idealease.
        /// </summary>
        private ConfiguracionSistemaUnidadBR ctrlConfiguracion;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(RegistrarConfiguracionSistemaUnidadPRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public RegistrarConfiguracionSistemaUnidadPRE(IRegistrarConfiguracionSistemaUnidadVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            ctrlConfiguracion = new ConfiguracionSistemaUnidadBR();
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario logueado tenga los permisos para realizar la acción de Registrar Configuración Sistema de Unidad.
        /// </summary>
        public void PrepararSeguridad() {
            EstablecerInformacionInicial();
            EstablecerSeguridad();
        }

        /// <summary>
        /// Establece la información Inicial en la Vista.
        /// </summary>
        private void EstablecerInformacionInicial() { 
            try {
                #region Configuraciones de Unidad Operativa
                //Obtener las configuraciones de la unidad operativa
                List<ConfiguracionUnidadOperativaBO> lstConfigUO = new ModuloBR().ConsultarConfiguracionUnidadOperativa(
                                                                                        this.dataContext, getConfiguracionUnidadOperativa(),  this.vista.ModuloID
                                                                                    );
                if (lstConfigUO.Count <= 0)
                    throw new Exception("No se encontraron las configuraciones del sistema para la unidad operativa en la que trabaja.");

                //Establecer las configuraciones de la unidad operativa
                this.vista.LibroActivos = lstConfigUO[0].Libro;
                #endregion
            } catch (Exception ex) {
                throw new Exception(nombreClase + " .EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Configuración Unidad Operativa, para realizar la búsqueda de las 
        /// Configuraciones de Unidades Operativas.
        /// </summary>
        /// <returns>Objeto de Tipo ConfiguracionUnidadOperativaBO</returns>
        private ConfiguracionUnidadOperativaBO getConfiguracionUnidadOperativa() { 
            return new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
        }

        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados del Usuario.
        /// </summary>
        private void EstablecerSeguridad() { 
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //consulta lista de acciones permitidas
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dataContext, getSeguridad());

                //Se valida si el usuario tiene permiso para Registrar Configuración Sistema de Unidad
                if (!ExisteAccion(acciones, "UI INSERTAR") || !ExisteAccion(acciones, "CONSULTAR") || !ExisteAccion(acciones, "INSERTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + " .EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene una nueva instancia de Seguridad.
        /// </summary>
        /// <returns>Objeto de tipo SeguridadBO.</returns>
        private SeguridadBO getSeguridad() {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            return new SeguridadBO(Guid.Empty, usuario, adscripcion);
        }

        /// <summary>
        /// Verifica si existe la acción en la lista de acciones permitidas.
        /// </summary>
        /// <param name="acciones">Lista de Acciones permitidas.</param>
        /// <param name="nombreAccion">Acción a verificar.</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion){
            if (acciones != null && 
                acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

            #endregion

        /// <summary>
        /// Registra las Configuraciones de Sistemas de Unidad. Verifica que no exista una Configuración con la misma clave.
        /// En caso de encontrarse una duplicada se manteniene en la Lista, en caso contrario se elimina de la Lista.
        /// </summary>
        public void Agregar() {
            List<ConfiguracionSistemaUnidadBO> configuracionesDuplicadas = new List<ConfiguracionSistemaUnidadBO>();
            foreach (ConfiguracionSistemaUnidadBO configuracion in vista.Configuraciones) {
                if (!ExisteConfiguracion(configuracion)) {
                    ctrlConfiguracion.Insertar(dataContext, configuracion, getSeguridad());
                } else {
                    configuracionesDuplicadas.Add(configuracion);
                }
            }
            if (configuracionesDuplicadas.Count > 0) {
                vista.Configuraciones = configuracionesDuplicadas;
                vista.CargarConfiguraciones();
                vista.MostrarMensaje("Algunas configuraciones estan duplicadas", ETipoMensajeIU.ADVERTENCIA);
            } else {
                vista.BloquearCampos();
                vista.MostrarMensaje("Guardado con exito!", ETipoMensajeIU.EXITO);
            }
        }

        /// <summary>
        /// Verifica que no exista una Configuración Sistema de Unidad Idealease con la misma Clave y el Estado Activo.
        /// </summary>
        /// <param name="configuracion">El filtro ConfiguracionSistemaUnidadBO para realizar la búsqueda.</param>
        /// <returns>Retorna True si encontró resultados, en caso contrario retorna False.</returns>
        private bool ExisteConfiguracion(ConfiguracionSistemaUnidadBO configuracion) {
            ConfiguracionSistemaUnidadBO configuracionEncontrada = ctrlConfiguracion.Consultar(dataContext, getFiltro(configuracion.Clave)).FirstOrDefault();
            return configuracionEncontrada != null && configuracionEncontrada.ConfiguracionSistemaUnidadId != null;
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Configuración Sistema de Unidad Idealease por medio de la Clave seleccionada
        /// y el Estado Activo, para para realizar la búsqueda de las Configuraciones Sistema de Unidad Idealease.
        /// </summary>
        /// <param name="param">La Clave de la Configuración Sistema de Unidad Idealease.</param>
        /// <returns>Un objeto de Tipo ConfiguracionSistemaUnidadBO</returns>
        private ConfiguracionSistemaUnidadBO getFiltro(string param) {
            ConfiguracionSistemaUnidadBO filtro = new ConfiguracionSistemaUnidadBO(){
                Clave = param.ToUpper(),
                Activo = true
            };
            return filtro;
        }

        #endregion
    }
}
