// Satisface al CU073 - Catálogo Configuración Sistemas Unidad
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Facade.SDNI.BR;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Eliminar Configuraciones Sistemas de Unidad Idealease.
    /// </summary>
    public class EliminarConfiguracionSistemaUnidadPRE {

        #region Atributos

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// Controlador de Configuración Sistemas de Unidad Idealease.
        /// </summary>
        private ConfiguracionSistemaUnidadBR ctrlConfiguracion;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        
        private IEliminarConfiguracionSistemaUnidadVIS vista;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(EliminarConfiguracionSistemaUnidadPRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public EliminarConfiguracionSistemaUnidadPRE(IEliminarConfiguracionSistemaUnidadVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            ctrlConfiguracion = new ConfiguracionSistemaUnidadBR();
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario logueado tenga los permisos para realizar la acción de Eliminar Configuración Sistema de Unidad.
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

                //Se valida si el usuario tiene permiso para Eliminar Configuración Sistema de Unidad
                if (!ExisteAccion(acciones, "UI ACTUALIZAR") || !ExisteAccion(acciones, "ACTUALIZAR") || !ExisteAccion(acciones, "CONSULTAR"))
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
        /// Realiza el Eliminado lógico de la Configuración Sistema de Unidad Idealease seleccionada. Verifica que no exista 
        /// la Configuración Sistema de Unidad Idealease con la misma Clave. Realiza la redirección a Detalle Configuración Sistema de
        /// Unidad si se guardo con éxito.
        /// </summary>
        public void ActivarOInactivar() {
            if(vista.TipoUI.Equals("Reactivar")){
                if(Validar()){
                    return;
                }
            }
            try{ 
                ctrlConfiguracion.Activo(dataContext, vista.ConfiguracionSeleccionada, getSeguridad());    
                vista.MostrarMensaje("Guardado con exito", ETipoMensajeIU.EXITO);
                vista.RedirigirADetalleConfiguracionSistemaUnidad();
            }catch(Exception e){
                vista.MostrarMensaje("Error", ETipoMensajeIU.ERROR, e.Message);
            }
        }

        /// <summary>
        /// Verifica que no existan una Configuración Sistema de Unidad Idealease con la misma Clave al Reactivar.
        /// </summary>
        /// <returns>Retorna True si encontro resultados, en caso contrario retorna False.</returns>
        private bool Validar() {
            ConfiguracionSistemaUnidadBO configuracionEncontrada = ctrlConfiguracion.Consultar(dataContext, getFiltro()).FirstOrDefault();
            if (configuracionEncontrada != null && configuracionEncontrada.ConfiguracionSistemaUnidadId != null)
                if(!configuracionEncontrada.ConfiguracionSistemaUnidadId.Equals(vista.ConfiguracionSeleccionada.ConfiguracionSistemaUnidadId)) {
                    vista.MostrarMensaje("La información del sistema ingresada ya existe en la unidad. Favor de verificar", ETipoMensajeIU.ADVERTENCIA);
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Crea y establece un nuevo Filtro de Configuración Sistema de Unidad Idealease con la Clave seleccionada y el Estado Activo.
        /// </summary>
        /// <returns>El Filtro ConfiguracionSistemaUnidadBO</returns>
        private ConfiguracionSistemaUnidadBO getFiltro() { 
            ConfiguracionSistemaUnidadBO filtro = new ConfiguracionSistemaUnidadBO(){
                Clave = vista.ConfiguracionSeleccionada.Clave.ToUpper(),
                Activo = true
            };
            return filtro;
        }

        #endregion
    }
}
