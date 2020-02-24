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

namespace BPMO.SDNI.Facturacion.MonitoreoPagos.PRE
{
    /// <summary>
    /// Presentador para la vista de edición de notificaciones de empleados
    /// </summary>
    public class EditarConfigurarAlertaPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IEditarConfigurarAlertaVIS vista;

        /// <summary>
        /// Controlador de edición de configuración de alerta
        /// </summary>
        private ConfiguracionAlertaBR controlador;

        /// <summary>
        /// Presentador que esta gestionando la vista que visualiza los campos del registro
        /// </summary>
        private ucConfigurarAlertaPRE presentadorDetalle;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "EditarConfigurarNotificacionPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        /// <param name="vistaConfigurarNotificacion">Vista que mostrará los campos del registro procesado</param>
        public EditarConfigurarAlertaPRE(IEditarConfigurarAlertaVIS vista, IucConfigurarAlertaVIS vistaConfigurarNotificacion)
        {
            try
            {
                this.vista = vista;
                this.controlador = new ConfiguracionAlertaBR();
                this.presentadorDetalle = new ucConfigurarAlertaPRE(vistaConfigurarNotificacion);

                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en la configuración", ETipoMensajeIU.ERROR, nombreClase + ".EditarOperadorPRE: " + ex.GetBaseException().Message);
            }
        }
        #endregion Constructor

        #region Métodos
        /// <summary>
        /// Realiza la carga del registro al editar
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            ConfiguracionAlertaBO confiuguracion = this.vista.ObtenerPaqueteNavegacion("ConfiguracionAlertaBO") as ConfiguracionAlertaBO;
            this.EstablecerDatosNavegacion(confiuguracion);
            this.PrepararEdicion();
            
            this.LimpiarSesion();

            #region SC0008 - Aplicación de condición de búsqueda cuando no sea facturista
            if (!(confiuguracion is ConfiguracionAlertaPerfilBO))
                this.Consultar();
            #endregion 
            this.EstablecerSeguridad();
        }

        /// <summary>
        /// Realiza el proceso de inicializar el visor para editar un registro existente
        /// </summary>
        public void PrepararEdicion()
        {
            this.vista.PrepararEdicion();            
        }
        
        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista.UUA == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "ACTUALIZAR", seguridad))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Establecer las reglas de seguridad que se tendran para los controles y navegación del visor
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //Valida que el usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null)
                    throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                // Creación del objeto seguridad               
                SeguridadBO seguridad = this.CrearObjetoSeguridad();

                //consulta de acciones a la cual el usuario tiene permisos
                List<CatalogoBaseBO> acciones = FacadeBR.ConsultarAccion(dctx, seguridad);

                // se valida si el usuario tiene permisos para registrar
                if (!this.ExisteAccion(acciones, "INSERTAR"))
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
        private bool ExisteAccion(List<CatalogoBaseBO> acciones, string nombreAccion)
        {
            if (acciones != null && acciones.Count > 0 && acciones.Exists(p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
        }

        /// <summary>
        /// Realiza la consulta de datos a partir de los campos campturados en la vista
        /// </summary>
        private void Consultar()
        {
            try
            {
                ConfiguracionAlertaBO bo = new ConfiguracionAlertaBO() { ConfiguracionAlertaID = this.vista.ConfiguracionAlertaID };
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();
                List<ConfiguracionAlertaBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, seguridadBO);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
                this.vista.UltimoObjeto = lst[0];
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ConfiguracionAlertaBO());
                throw new Exception(nombreClase + ".ConsultarCompleto:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Establece un paquete de navegación
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        private void EstablecerDatosNavegacion(object paqueteNavegacion)
        {
            try
            {
                if (paqueteNavegacion == null)
                    throw new Exception("Se esperaba un objeto en la navegación. No se puede identificar qué registro se desea consultar.");
                if (!(paqueteNavegacion is ConfiguracionAlertaBO))
                    throw new Exception("Se esperaba un Autorizador.");

                ConfiguracionAlertaBO bo = (ConfiguracionAlertaBO)paqueteNavegacion;

                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ConfiguracionAlertaBO());
                throw new Exception(nombreClase + ".EstablecerDatosNavegacion: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Asigna los datos de un objeto a los campos correspondientes de la vista
        /// </summary>
        /// <param name="obj">Objeto que contiene los datos a visualizar</param>
        private void DatoAInterfazUsuario(ConfiguracionAlertaBO bo)
        {
            if (bo.Sucursal == null)
                bo.Sucursal = new SucursalBO();
            if (bo.Empleado == null)
                bo.Empleado = new EmpleadoBO();
            if (bo.Auditoria == null)
                bo.Auditoria = new AuditoriaBO();

            this.vista.ConfiguracionAlertaID = bo.ConfiguracionAlertaID;
            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;
            this.vista.EmpleadoID = bo.Empleado.Id;
            this.vista.EmpleadoNombre = bo.Empleado.NombreCompleto;
            this.vista.CorreoElectronico = bo.Empleado.Email;
            this.vista.NumeroDias = bo.NumeroDias;
            this.vista.Estatus = bo.Estatus;
            this.vista.UC = bo.Auditoria.UC;
            this.vista.FC = bo.Auditoria.FC;

            if (bo is ConfiguracionAlertaPerfilBO)
                this.vista.PerfilID = (bo as ConfiguracionAlertaPerfilBO).Perfil.Id;
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.presentadorDetalle.LimpiarSesion();
        }

        /// <summary>
        /// Genera un registro a partir de los campos capturados en la vista
        /// </summary>
        /// <returns>Objeto generador a partir de los datos visualizados</returns>
        private object InterfazUsuarioADato()
        {
            #region SC0008 - Adición de caso cuando se está editando una configuración de tipo facturista
            ConfiguracionAlertaBO bo = new ConfiguracionAlertaBO();
            if (this.vista.PerfilID.HasValue)
            {
                ConfiguracionAlertaPerfilBO configuracionPerfil = new ConfiguracionAlertaPerfilBO();
                configuracionPerfil.Perfil = new BPMO.Seguridad.BO.PerfilBO { Id = this.vista.PerfilID };
                bo = configuracionPerfil;
            }
            #endregion

            bo.ConfiguracionAlertaID = this.vista.ConfiguracionAlertaID;
            bo.NumeroDias = this.vista.NumeroDias;
            bo.Estatus = this.vista.Estatus;
            bo.Sucursal = new SucursalBO
                {
                    Id = this.vista.SucursalID,
                    Nombre = this.vista.SucursalNombre,
                    UnidadOperativa = new UnidadOperativaBO
                    {
                        Id = this.vista.UnidadOperativaID
                    }
                };
            bo.Empleado = new EmpleadoBO
                {
                    Id = this.vista.EmpleadoID,
                    NombreCompleto = this.vista.EmpleadoNombre,
                    Email = this.vista.CorreoElectronico
                };
            bo.Auditoria = new AuditoriaBO
                {
                    FC = this.vista.FC,
                    FUA = this.vista.FUA,
                    UC = this.vista.UC,
                    UUA = this.vista.UUA
                };

            return bo;
        }

        /// <summary>
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        public String ValidarCampos()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalNombre == null)
                s += "Sucursal, ";
            if (this.vista.EmpleadoNombre == null)
                s += "Empleado, ";
            if (this.vista.CorreoElectronico == null)
                s += "Correo Electrónico, ";
            if (this.vista.Estatus == null)
                s += "Estatus, ";
            if (this.vista.NumeroDias == null)
                s += "Número Días, ";

            if (s.Trim().CompareTo("") != 0)
                return "Los siguientes campos no pueden estar vacíos: \n" + s.Substring(0, s.Length - 2);

            ConfiguracionAlertaBO bo = new ConfiguracionAlertaBO
            {
                ConfiguracionAlertaID = this.vista.ConfiguracionAlertaID,
                Sucursal = new SucursalBO { Id = this.vista.SucursalID },
                Empleado = new EmpleadoBO { Id = this.vista.EmpleadoID },               
            };

            SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

            bool any = this.controlador.ConsultarExistencia(this.dctx, bo, seguridadBO);
            if (any)
                return "Ya existe una configuración de alerta para el usuario seleccionado en la sucursal seleccionada";

            return null;
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
        /// Guarda los cambios capturados y se rediriege al visor invocador
        /// </summary>
        public void Editar()
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

                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                this.controlador.Actualizar(this.dctx, bo, this.vista.UltimoObjeto as ConfiguracionAlertaBO, seguridadBO);

                #region SC0008 - Recuperación de configuración de facturista que fue modificado
                if (bo is ConfiguracionAlertaPerfilBO)
                {
                    //Se consulta lo insertado para recuperar los ID
                    List<ConfiguracionAlertaBO> ls = this.controlador.Consultar(this.dctx, bo);
                    if (ls.Count <= 0)
                        throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                    if (ls.Count > 1)
                        throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                    bo = ls[0] as ConfiguracionAlertaBO;
                }
                #endregion

                this.vista.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ConfiguracionAlertaBO", bo);
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".Editar:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Cancela la edición actual y se rediriege al visor invocador
        /// </summary>
        public void Cancelar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirADetalles();
        }
        #endregion
    }
}
