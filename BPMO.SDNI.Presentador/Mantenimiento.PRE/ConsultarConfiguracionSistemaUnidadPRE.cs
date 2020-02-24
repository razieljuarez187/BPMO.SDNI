// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Consultar Configuraciones Sistemas de Unidad Idealease.
    /// </summary>
    public class ConsultarConfiguracionSistemaUnidadPRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IConsultarConfiguracionSistemaUnidadVIS vista;

        /// <summary>
        /// Controlador de Configuraciones Sistemas de Unidad Idealease.
        /// </summary>
        private ConfiguracionSistemaUnidadBR ctrlConfiguracion;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarConfiguracionSistemaUnidadPRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public ConsultarConfiguracionSistemaUnidadPRE(IConsultarConfiguracionSistemaUnidadVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            ctrlConfiguracion = new ConfiguracionSistemaUnidadBR();
        }

        #endregion

        #region Métodos
        

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario tenga los permisos para realizar la acción de Consulta Configuraciones Sistemas de Unidad Idealease.
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

                //Se valida si el usuario tiene permiso para Consultar las Configuraciones Sistemas de Unidad.
                if (!ExisteAccion(acciones, "UI CONSULTAR") || !ExisteAccion(acciones, "CONSULTAR"))
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
        /// Verifica si existe una acción en la lista de acciones permitidas.
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
        /// Realiza la búsqueda de las Configuraciones Sistemas de Unidad Idealease por el Nombre seleccionado, la Clave seleccionada y el Estado 
        /// de la Configuración Sistema de Unidad Idealease seleccionada, si el Estado del Contacto Cliente Idealease es nulo consulta Activos e 
        /// Inactivos. En caso de no encontrarse resultados despliega un mensaje de advertencia.
        /// </summary>
        public void BuscarConfiguraciones() {
            List<ConfiguracionSistemaUnidadBO> configuraciones = new List<ConfiguracionSistemaUnidadBO>();
            if (vista.Activo != null) {
                configuraciones = ctrlConfiguracion.Consultar(dataContext, getFiltroConfiguracion(vista.Activo.Value));
            }else{
                List<ConfiguracionSistemaUnidadBO> activos = ctrlConfiguracion.Consultar(dataContext, getFiltroConfiguracion(true));
                List<ConfiguracionSistemaUnidadBO> inactivos = ctrlConfiguracion.Consultar(dataContext, getFiltroConfiguracion(false));
                if (activos.Count > 0) {
                    configuraciones.AddRange(activos);
                }
                if(inactivos.Count > 0) {
                    configuraciones.AddRange(inactivos);
                }
            }

            if (configuraciones.Count == 0) {
                vista.MostrarMensaje("No se encontraron resultados", ETipoMensajeIU.ADVERTENCIA);
            }
            vista.Configuraciones = configuraciones;
            vista.DesplegarListaConfiguraciones();
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de Configuración Sistema de Unidad Idealease con Nombre seleccionado, la Clave seleccionada y el
        /// Estado de la Configuración Sistema de Unidad Idealease seleccionada, para realizar la búsqueda de los Contactos Cliente Idealease.
        /// </summary>
        /// <param name="activo">El Estado de la Configuración Sistema de Unidad Idealease.</param>
        /// <returns>Objeto de tipo ConfiguracionSistemaUnidadBO.</returns>
        private ConfiguracionSistemaUnidadBO getFiltroConfiguracion(bool activo) {
            ConfiguracionSistemaUnidadBO filtro = new ConfiguracionSistemaUnidadBO(){
                Nombre = "%"+vista.Nombre+"%",
                Clave = vista.Clave,
                Activo = activo
            };
            return filtro;
        }

        #endregion
    }
}
