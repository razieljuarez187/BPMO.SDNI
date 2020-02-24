// Satisface al CU027 - Catálogo Contacto Cliente
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BOF;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BR;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.BOF;

namespace BPMO.SDNI.Mantenimiento.PRE {

    /// <summary>
    /// Presentador aplicable para la gestión de las peticiones de la Vista Consultar Contactos Clientes Idealease.
    /// </summary>
    public class ConsultarContactoClientePRE {

        #region Atributos

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador.
        /// </summary>
        private IConsultarContactoClienteVIS vista;

        /// <summary>
        /// Controlador de Contactos Cliente Idealease.
        /// </summary>
        private ContactoClienteBR contactoClienteBR;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos.
        /// </summary>
        private readonly IDataContext dataContext;

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
        public ConsultarContactoClientePRE(IConsultarContactoClienteVIS vista) {
            this.vista = vista;
            contactoClienteBR = new ContactoClienteBR();
            dataContext = FacadeBR.ObtenerConexion();
        }

        #endregion

        #region Métodos

            #region Buscador

        /// <summary>
        /// Crea el Objeto de filtrado para el buscador.
        /// </summary>
        /// <param name="catalogo">El Tipo de Objeto a filtrar.</param>
        /// <returns>Un Objeto de Tipo Object</returns>
        public object PrepararBOBuscador(string catalogo) {
            object obj = null;

            switch (catalogo) {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaId };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioAutenticado };
                    sucursal.Nombre = this.vista.NombreSucursal;
                    sucursal.Activo = true;
                    obj = sucursal;
                    break;
                case "Cliente":
                    CuentaClienteIdealeaseBOF cliente = new CuentaClienteIdealeaseBOF {
                        UnidadOperativa = new UnidadOperativaBO { Id = vista.UnidadOperativaId },
                        Cliente = new ClienteBO()
                    };
                    if (vista.NombreCliente != null)
                        cliente.Nombre = "%" + vista.NombreCliente + "%";
                    obj = cliente;
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Despliega el Objeto seleccionado del buscador.
        /// </summary>
        /// <param name="catalogo">El Tipo de Objeto a filtrar.</param>
        /// <param name="selecto">El Objeto seleccionado del buscador.</param>
        public void DesplegarResultadoBuscador(string catalogo, object selecto) {
            switch (catalogo) {
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null) {
                        vista.SucursalSeleccionada = new SucursalBO(){
                          Id = sucursal.Id,
                          Nombre = sucursal.Nombre
                        };
                        if (sucursal.Nombre != null)
                            this.vista.NombreSucursal = sucursal.Nombre;
                        else
                            this.vista.NombreSucursal = null;
                    }
                    break;
                case "Cliente":
                     CuentaClienteIdealeaseBOF cliente = (CuentaClienteIdealeaseBOF)selecto ?? new CuentaClienteIdealeaseBOF();
                    if (cliente.Cliente == null)
                        cliente.Cliente = new ClienteBO();
                    vista.NombreCliente = !string.IsNullOrEmpty(cliente.Nombre) ? cliente.Nombre : string.Empty;
                    vista.ClienteSeleccionado = cliente;
                    break;
            }
        }

            #endregion

            #region Seguridad

        /// <summary>
        /// Verifica que el Usuario tenga los permisos para realizar la acción de Consulta Contactos Clientes Idealease.
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
                throw new Exception(nombreClase + ".EstablecerInformacionInicial: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de la Configuración Unidad Operativa, para realizar la búsqueda de las 
        /// Configuraciones de Unidades Operativas.
        /// </summary>
        /// <returns>Objeto de Tipo ConfiguracionUnidadOperativaBO</returns>
        private ConfiguracionUnidadOperativaBO getConfiguracionUnidadOperativa() { 
            return new ConfiguracionUnidadOperativaBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
        }

        /// <summary>
        /// Permite configurar el comportamiento de los controles de la vista de acuerdo a los permisos configurados del Usuario.
        /// </summary>
        private void EstablecerSeguridad() { 
            try {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioAutenticado == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaId == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //consulta lista de acciones permitidas
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dataContext, getSeguridad());

                //Se valida si el usuario tiene permiso para Consultar Contactos Clientes Idealease
                if (!ExisteAccion(acciones, "UI CONSULTAR") || !ExisteAccion(acciones, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
                
            }
            catch (Exception ex) {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.Message);
            }
        }

        /// <summary>
        /// Crea y obtiene una nueva instancia de Seguridad.
        /// </summary>
        /// <returns>Objeto de tipo SeguridadBO.</returns>
        private SeguridadBO getSeguridad() {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioAutenticado };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaId } };
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
        /// Realiza la búsqueda de los Contactos Cliente Idealease por la Sucursal seleccionada, el Cliente Idealease seleccionado y el Estado del Contacto Cliente 
        /// Idelaease seleccionado, si el Estado del Contacto Cliente Idealease es nulo consulta Activos e Inactivos. En caso de no encontrarse resultados despliega 
        /// un mensaje de advertencia.
        /// </summary>
        public void MostrarDatosInterfaz() {
            List<ContactoClienteBO> contactosCliente = new List<ContactoClienteBO>();
            if (vista.Activo != null) {
                contactosCliente = contactoClienteBR.Consultar(dataContext, getFiltroContactoCliente((bool)vista.Activo));
            } else {
                bool activo = true;
                List<ContactoClienteBO> contactosClientesActivos = contactoClienteBR.Consultar(dataContext, getFiltroContactoCliente(activo));
                List<ContactoClienteBO> contactosClientesInactivos = contactoClienteBR.Consultar(dataContext, getFiltroContactoCliente(!activo));
                if (contactosClientesActivos.Count > 0)
                    contactosCliente.AddRange(contactosClientesActivos);
                if (contactosClientesInactivos.Count > 0)
                    contactosCliente.AddRange(contactosClientesInactivos);
            }
            
            if (contactosCliente.Count == 0) {
                vista.MostrarMensaje("No se encontraron Resultados", ETipoMensajeIU.ADVERTENCIA);
            }
            vista.Contactos = contactosCliente;
            vista.DesplegarListaContactosCliente();
        }

        /// <summary>
        /// Crea y obtiene un nuevo Filtro de Contacto Cliente Idealease con la Sucursal seleccionada, el Cliente Idealease seleccionado y 
        /// el Estado seleccionado, para realizar la búsqueda de los Contactos Cliente Idealease.
        /// </summary>
        /// <param name="activo">El Estado del Contacto Cliente Idealease.</param>
        /// <returns>Objeto de tipo ContactoClienteBO.</returns>
        private ContactoClienteBO getFiltroContactoCliente(bool activo) {
            ContactoClienteBO filtro = new ContactoClienteBO();
            if (vista.ClienteSeleccionado != null && vista.ClienteSeleccionado.Id != null) {
                filtro.CuentaClienteIdealease = vista.ClienteSeleccionado;
            }else{
                filtro.CuentaClienteIdealease = new CuentaClienteIdealeaseBO(){
                    UnidadOperativa = new UnidadOperativaBO(){
                        Id = vista.UnidadOperativaId
                    }
                };
            }
            if (vista.SucursalSeleccionada != null && vista.SucursalSeleccionada.Id != null) {
                filtro.Sucursal = vista.SucursalSeleccionada;
            } else {
                filtro.Sucursal = new SucursalBO();
            }

            filtro.Activo = activo;
            return filtro;
        }

        #endregion

        /// <summary>
        /// Obtiene los detalles completos del Contacto Cliente Idealease seleccionado.
        /// </summary>
        public void ConsultarContactoClienteCompleto() {
            vista.ContactoClienteSeleccionado = contactoClienteBR.ConsultarCompleto(dataContext, vista.ContactoClienteSeleccionado).FirstOrDefault();
        }
    }
}
