// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Eliminar Contactos Clientes Idealease.
    /// </summary>
    public class EliminarContactoClientePRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IEliminarContactoClienteVIS vista;

        /// <summary>
        /// Controlador de Contactos Cliente Idealease.
        /// </summary>
        private ContactoClienteBR ctrlContactoCliente;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private IDataContext dataContext;

        /// <summary>
        /// Nombre de la clase en curso.
        /// </summary>
        private readonly string nombreClase = typeof(ConsultarContactoClientePRE).Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor predeterminado para el presentador.
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador.</param>
        public EliminarContactoClientePRE(IEliminarContactoClienteVIS vista) {
            this.vista = vista;
            dataContext = FacadeBR.ObtenerConexion();
            ctrlContactoCliente = new ContactoClienteBR();
        }

        #endregion

        #region Métodos

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario logueado tenga los permisos para realizar la acción de Eliminar Contactos Clientes Idealease.
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

                //Se valida si el usuario tiene permiso para Eliminar Contacto cliente
                if (!ExisteAccion(acciones, "UI ACTUALIZAR") || !ExisteAccion(acciones, "ACTUALIZAR") || !ExisteAccion(acciones, "ACTUALIZARCOMPLETO") ||
                    !ExisteAccion(acciones, "CONSULTAR") || !ExisteAccion(acciones, "CONSULTARCOMPLETO"))
                    this.vista.RedirigirSinPermisoAcceso();
                       
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + " .EstablecerSeguridad: " + ex.Message);
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
        /// Realiza la consulta de Contacto Cliente Idealease con la información proporcionada.
        /// </summary>
        /// <returns>Retorna True si se encontraron resultados, en caso contrario retorna False.</returns>
        private bool ExisteContactoCliente() {
            return ctrlContactoCliente.Consultar(dataContext, getFiltroExistencia()).FirstOrDefault() != null;
        }

        /// <summary>
        /// Crea y establece un nuevo Filtro de Contacto Cliente Idealease con la Sucursal seleccionada y el Contacto Cliente Idealease seleccionado.
        /// </summary>
        /// <returns>El Filtro ContactoClienteBO</returns>
        private ContactoClienteBO getFiltroExistencia() { 
            ContactoClienteBO contacto = new ContactoClienteBO() {
                CuentaClienteIdealease = vista.ContactoClienteSeleccionado.CuentaClienteIdealease,
                Sucursal = vista.ContactoClienteSeleccionado.Sucursal,
                Activo = true
            };
            return contacto;
        }

        /// <summary>
        /// Realiza el cambio de Estado del Contacto Cliente Idealease seleccionado. Si el Contacto Cliente Idealease esta Inactivo y se quiere
        /// Reactivar, verifica que no exista un elemento con las mismas características.
        /// </summary>
        public void Eliminar() {
            try {
                if (!vista.ContactoClienteSeleccionado.Activo.Value) {
                    if (ExisteContactoCliente()) {
                        vista.MostrarMensaje("Ya existe un Contacto Cliente con estas características.", ETipoMensajeIU.ADVERTENCIA);
                        return;
                    }
                }
                ctrlContactoCliente.Eliminar(dataContext, vista.ContactoClienteSeleccionado, getSeguridad());
                vista.MostrarMensaje("Eliminado con exito", ETipoMensajeIU.EXITO);
                vista.LimpiarDatosSesion();
                vista.setGuardadoExitoso();
                vista.RedirigirADetalleContactoCliente();
            } catch(Exception e){
                vista.MostrarMensaje(e.Message, ETipoMensajeIU.ERROR);
            } 
        }

        /// <summary>
        /// Realiza la redirección al visor seleccionado.
        /// </summary>
        /// <param name="menuSeleccionado">Menú seleccionado.</param>
        public void Redirigir(string menuSeleccionado) {
             switch (menuSeleccionado) {
                case "EliminarContactoCliente":
                    vista.RedirigirAEliminarContactoCliente();
                    break;
                case "EditarContactoCliente":
                    vista.RedirigirAEditarContactoCliente();
                    break;
            }
        }

        #endregion
    }
}
