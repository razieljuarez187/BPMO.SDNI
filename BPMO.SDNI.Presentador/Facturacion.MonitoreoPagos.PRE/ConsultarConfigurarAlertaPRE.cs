//Satisface el caso de uso CU009 – Configuración Notificación de facturación
//Satisface a la solicitud de cambios SC0008

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.MonitoreoPagos.BO;
using BPMO.SDNI.Facturacion.MonitoreoPagos.BR;
using BPMO.SDNI.Facturacion.MonitoreoPagos.VIS;
using BPMO.Facade.SDNI.BOF;
using System.Collections;

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.PRE
{
    /// <summary>
    /// Presentador para la vista de consulta de notificaciones de empleados
    /// </summary>
    public class ConsultarConfigurarAlertaPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;
        
        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IConsultarConfigurarAlertaVIS vista;

        /// <summary>
        /// Controlador de Consulta de configuración de alerta
        /// </summary>
        private ConfiguracionAlertaBR controlador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "ConsultarConfigurarNotificacionPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        public ConsultarConfigurarAlertaPRE(IConsultarConfigurarAlertaVIS vista)
        {
            try
            {
                this.vista = vista;
                this.dctx = FacadeBR.ObtenerConexion();
                this.controlador = new ConfiguracionAlertaBR();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuracion", ETipoMensajeIU.ERROR, nombreClase + ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la Vista para una nueva busqueda
        /// </summary>
        public void PrepararBusqueda()
        {
            this.vista.LimpiarSesion();

            this.PrepararPropiedades();         
            this.EstablecerFiltros();
            this.EstablecerSeguridad();            
        }        

        /// <summary>
        /// Inicializa las propiedades de la vista
        /// </summary>
        private void PrepararPropiedades()
        {
            this.vista.EmpleadoID = null;
            this.vista.EmpleadoNombre = null;
            this.vista.SucursalID = null;
            this.vista.SucursalNombre = null;
            this.vista.Estatus = null;
            this.vista.PermitirSeleccionarEmpleado(false);
        }

        /// <summary>
        /// Estabalece los filtros que se mostraran y aplicarán para la consulta
        /// </summary>
        private void EstablecerFiltros()
        {
            try
            {
                Dictionary<string, object> paquete = this.vista.ObtenerPaqueteNavegacion("FiltrosConfiguracionAlerta") as Dictionary<string, object>;
                if (paquete != null)
                {
                    if (paquete.ContainsKey("ObjetoFiltro"))
                    {
                        if (paquete["ObjetoFiltro"].GetType() == typeof(ConfiguracionAlertaBO))
                            this.DatoAInterfazUsuario(paquete["ObjetoFiltro"]);
                        else
                            throw new Exception("Se esperaba un objeto ConfiguracionAlertaBO, el objeto proporcionado no cumple con esta característica, intente de nuevo por favor.");
                    }
                    if (paquete.ContainsKey("Bandera"))
                    {
                        if ((bool)paquete["Bandera"])
                            this.Consultar();
                    }
                }
                this.vista.LimpiarPaqueteNavegacion("FiltrosConfiguracionAlerta");
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerFiltros: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Obtiene un objeto de seguridad para validación de permisos
        /// </summary>
        /// <returns></returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            //Se crea el objeto de seguridad
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista.UsuarioID };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Establecer las reglas de seguridad que se tendran para los controles y navegación del visor
        /// </summary>
        private void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para consultar
                if (!this.ExisteAccion(lst, "CONSULTAR"))
                    this.vista.RedirigirSinPermisoAcceso();

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".EstablecerSeguridad:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Evalua si una acción se cuentra definida
        /// </summary>
        /// <param name="acciones">Lista de acciones donde realizará la búsqueda</param>
        /// <param name="accion">Acción a evaluar</param>
        /// <returns>Devuelve true si la acción está definida, de lo contrario devolverá false</returns>
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string accion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == accion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Asigna los datos de un objeto a los campos correspondientes de la vista
        /// </summary>
        /// <param name="obj">Objeto que contiene los datos a visualizar</param>
        private void DatoAInterfazUsuario(Object obj)
        {
            ConfiguracionAlertaBO bo = (ConfiguracionAlertaBO)obj;
            if (bo.Empleado == null)
                bo.Empleado = new EmpleadoBO();

            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();

            this.vista.EmpleadoID = bo.Empleado.Id;
            this.vista.EmpleadoNombre = bo.Empleado.NombreCompleto;
            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;
            this.vista.Estatus = bo.Estatus;            
        }

        /// <summary>
        /// Genera un registro a partir de los campos capturados en la vista
        /// </summary>
        /// <returns>Objeto generador a partir de los datos capturador</returns>
        private Object InterfazUsuarioADato()
        {
            ConfiguracionAlertaBO bo = new ConfiguracionAlertaBO
            {
                Sucursal = new SucursalBO { Id = this.vista.SucursalID },
                Empleado = new EmpleadoBO { Id = this.vista.EmpleadoID },
                Estatus = this.vista.Estatus
            };

            return bo;
        }

        /// <summary>
        /// Realiza la consulta de datos a partir de los campos campturados en la vista
        /// </summary>
        public void Consultar()
        {
            string s;
            if ((s = this.ValidarCampos()) != null)
            {
                this.vista.MostrarMensaje(s, ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            try
            {
                ConfiguracionAlertaBO bo = (ConfiguracionAlertaBO)this.InterfazUsuarioADato();
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //SC0008 - Se agrega a los parámetros de consulta el objeto seguridadBO que es requerido
                List<ConfiguracionAlertaBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, seguridadBO);

                if (lst.Count < 1)
                    this.vista.MostrarMensaje("La búsqueda no produjo resultados", ETipoMensajeIU.INFORMACION,
                        "No se encontraron coincidencias con los criterios ingresados. Verifique sus datos");

                this.vista.EstablecerResultado(lst);                
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".Consultar: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        private string ValidarCampos()
        {
            String valor = this.vista.ValidarCampos();
            if (valor != null)
                return valor;

            return null;           
        }

        // <summary>
        /// Cambia a la vista de detalle del registro seleccionado
        /// </summary>
        /// <param name="configuracionAlertaBOID">Identificador del registro seleccionado</param>
        public void IrADetalle(dynamic parameters)
        {
            try
            {
                if (parameters == null)
                    throw new Exception("No se encontró el registro seleccionado");

                #region SC0008 - Se agregan la rutina para recuperación de una configuración que sea de facturista
                ConfiguracionAlertaBO configuracion = new ConfiguracionAlertaBO();                
                if(((IDictionary<String, object>)parameters).ContainsKey("PerfilID"))
                {
                    ConfiguracionAlertaPerfilBO configuracionPerfil = new ConfiguracionAlertaPerfilBO();
                    configuracionPerfil.Perfil = new BPMO.Seguridad.BO.PerfilBO();
                    configuracionPerfil.Perfil.Id = parameters.PerfilID;
                    configuracion = configuracionPerfil;
                }

                configuracion.ConfiguracionAlertaID = parameters.ConfiguracionAlertaID;
                configuracion.Empleado = new EmpleadoBO { Id = parameters.EmpleadoID, NombreCompleto = parameters.EmpleadoNombreCompleto, Email = parameters.EmpleadoEmail };
                configuracion.Sucursal = new SucursalBO { Id = parameters.SucursalID, Nombre = parameters.SucursalNombre };
                configuracion.NumeroDias = parameters.NumeroDias;
                #endregion

                this.vista.LimpiarSesion();

                Dictionary<string, object> paquete = new Dictionary<string, object>();
                paquete.Add("ObjetoFiltro", this.InterfazUsuarioADato());
                paquete.Add("Bandera", true);

                this.vista.EstablecerPaqueteNavegacion("FiltrosConfiguracionAlerta", paquete);
                this.vista.EstablecerPaqueteNavegacion("ConfiguracionAlertaBO", configuracion);

                this.vista.RedirigirADetalle();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".IrADetalle: " + ex.GetBaseException().Message);
            }
        }
        #endregion

        #region Métodos para el Buscador
        /// <summary>
        /// Prepara un BO para la Busqueda en su respectivo catalogo
        /// </summary>
        /// <param name="catalogo">catalogo donde se realizara la busqueda</param>
        /// <returns>Objeto que define el filtro a aplicar al buscador</returns>
        public object PrepararBOBuscador(string catalogo)
        {
            object obj = null;

            switch (catalogo)
            {
                case "Sucursal":
                    SucursalBOF sucursal = new SucursalBOF();
                    sucursal.UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativaID };
                    sucursal.Usuario = new UsuarioBO { Id = this.vista.UsuarioID };

                    sucursal.Nombre = this.vista.SucursalNombre;                   
                    sucursal.Activo = true;

                    obj = sucursal;
                    break;

                case "Empleado":
                    EmpleadoBO empleado = new EmpleadoBO();
                    empleado.Asignacion = new OrganizacionBO { Sucursal = this.vista.ClaveSucursal };

                    empleado.NombreCompleto = this.vista.EmpleadoNombre;
                    empleado.Activo = true;

                    obj = empleado;
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
                case "Sucursal":
                    SucursalBO sucursal = (SucursalBO)selecto;
                    if (sucursal != null)
                    {
                        if (sucursal.Id != null)
                        {
                            if (this.vista.SucursalID != sucursal.Id)
                            {
                                this.vista.EmpleadoID = null;
                                this.vista.EmpleadoNombre = null;
                            }

                            this.vista.SucursalID = sucursal.Id;
                        }
                        else
                        {
                            this.vista.EmpleadoID = null;
                            this.vista.EmpleadoNombre = null;

                            this.vista.SucursalID = null;
                        }

                        if (sucursal.Nombre != null)
                            this.vista.SucursalNombre = sucursal.Nombre;
                        else
                            this.vista.SucursalNombre = null;

                        if (sucursal.NombreCorto != null)
                        {
                            this.vista.ClaveSucursal = sucursal.NombreCorto;
                            this.vista.PermitirSeleccionarEmpleado(true);
                        }
                        else
                        {
                            this.vista.ClaveSucursal = null;
                            this.vista.PermitirSeleccionarEmpleado(false);
                        }
                    }
                    break;
                    
                case "Empleado":
                    EmpleadoBO empleado = (EmpleadoBO)selecto;
                    if (empleado != null)
                    {
                        if (empleado.Id != null)
                            this.vista.EmpleadoID = empleado.Id;
                        else
                            this.vista.EmpleadoID = null;

                        if (empleado.NombreCompleto != null)
                            this.vista.EmpleadoNombre = empleado.NombreCompleto;
                        else
                            this.vista.EmpleadoNombre = null;
                    }
                    break;
            }
        }
        #endregion
    }
}
