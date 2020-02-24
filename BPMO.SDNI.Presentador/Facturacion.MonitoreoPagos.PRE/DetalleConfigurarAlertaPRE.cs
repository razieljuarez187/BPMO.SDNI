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
    /// Presentador para la vista de detalles de notificaciones de empleados
    /// </summary>
    public class DetalleConfigurarAlertaPRE
    {
        #region Atributos
        /// <summary>
        /// Controlador de Consulta de configuración de alerta
        /// </summary>
        private ConfiguracionAlertaBR controlador;

        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx = null;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IDetalleConfigurarAlertaVIS vista;
        
        /// <summary>
        /// La vista que esta mostrando los campos del registro procesado en el presentador
        /// </summary>
        private IucConfigurarAlertaVIS vistaDetalle;

        /// <summary>
        /// Presentador que esta gestionando la vista que visualiza los campos del registro
        /// </summary>
        private ucConfigurarAlertaPRE presentadorDetalle;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private string nombreClase = "DetalleConfigurarAlertaPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        /// <param name="vista">Vista que mostrará los campos del registro procesado</param>
        public DetalleConfigurarAlertaPRE(IDetalleConfigurarAlertaVIS view, IucConfigurarAlertaVIS viewConfigurarAlerta)
        {
            try
            {
                this.vista = view;
                this.vistaDetalle = viewConfigurarAlerta;

                this.presentadorDetalle = new ucConfigurarAlertaPRE(viewConfigurarAlerta);

                this.controlador = new ConfiguracionAlertaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".DetalleConfigurarAlertaPRE: " + ex.GetBaseException().Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza la carga del registro al visualizar
        /// </summary>
        public void RealizarPrimeraCarga()
        {
            try
            {
                this.PrepararVisualizacion();
                
                ConfiguracionAlertaBO configuracionAlerta = this.vista.ObtenerPaqueteNavegacion("ConfiguracionAlertaBO") as ConfiguracionAlertaBO;
                this.EstablecerDatosNavegacion(configuracionAlerta);
                this.vista.PermitirRegresar(configuracionAlerta != null);

                this.LimpiarSesion();
                #region SC0008 - Búsqueda únicamente paras las connfiguraciones que no sean de facturistas
                if (!(configuracionAlerta is ConfiguracionAlertaPerfilBO))                                    
                    this.Consultar();                
                #endregion
                this.EstablecerSeguridad();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".RealizarPrimeraCarga: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Establece un paquete de navegación en el visor
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
                    throw new Exception("Se esperaba un ConfiguracionAlerta.");

                ConfiguracionAlertaBO bo = (ConfiguracionAlertaBO)paqueteNavegacion;
                this.DatoAInterfazUsuario(bo);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ConfiguracionAlertaBO());
                throw new Exception(this.nombreClase + ".EstablecerDatosNavegacion: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Realiza el proceso de inicializar el visor para mostrar los datos de un registro
        /// </summary>
        private void PrepararVisualizacion()
        {
            this.vista.PrepararVisualizacion();            
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

            this.vista.ConfiguracionAlertaID = bo.ConfiguracionAlertaID;
            this.vista.EmpleadoID = bo.Empleado.Id;
            this.vista.EmpleadoNombre = bo.Empleado.NombreCompleto;
            this.vista.SucursalID = bo.Sucursal.Id;
            this.vista.SucursalNombre = bo.Sucursal.Nombre;
            this.vista.CorreoElectronico = bo.Empleado.Email;
            this.vista.Estatus = bo.Estatus;
            this.vista.NumeroDias = bo.NumeroDias;

            #region SC0008 - Guardado del perfil asociado a la configuracion
            if (bo is ConfiguracionAlertaPerfilBO)
                this.vista.PerfilID = (bo as ConfiguracionAlertaPerfilBO).Perfil.Id;
            #endregion
        }

        /// <summary>
        /// Genera un registro a partir de los campos capturados en la vista
        /// </summary>
        /// <returns>Objeto generador a partir de los datos visualizados</returns>
        private Object InterfazUsuarioADato()
        {
            ConfiguracionAlertaBO bo = new ConfiguracionAlertaBO
            {
                ConfiguracionAlertaID = this.vista.ConfiguracionAlertaID,
                Sucursal = new SucursalBO(),
                Empleado = new EmpleadoBO()                
            };

            return bo;
        }

        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UsuarioID == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo ");

                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(dctx, "CONSULTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso:" + ex.GetBaseException().Message);
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
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(dctx, seguridad);

                //Se valida si el usuario tiene permiso para registrar
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.PermitirRegistrar(false);

                //Se valida si el usuario tiene permiso para editar
                if (!this.ExisteAccion(lst, "ACTUALIZAR"))
                    this.vista.PermitirEditar(false);
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".EstablecerSeguridad:" + ex.GetBaseException().Message);
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
            if (acciones != null && acciones.Count > 0 &&
                acciones.Exists(
                    p => p != null && p.Nombre != null && p.Nombre.Trim().ToUpper() == nombreAccion.Trim().ToUpper()))
                return true;

            return false;
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
        /// Realiza la consulta de datos a partir de los campos campturados en la vista
        /// </summary>
        private void Consultar()
        {
            try
            {
                ConfiguracionAlertaBO bo = (ConfiguracionAlertaBO)this.InterfazUsuarioADato();
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                List<ConfiguracionAlertaBO> lst = this.controlador.ConsultarCompleto(this.dctx, bo, seguridadBO);

                if (lst.Count < 1)
                    throw new Exception("No se encontró ningún registro que corresponda a la información proporcionada.");
                if (lst.Count > 1)
                    throw new Exception("La consulta devolvió más de un registro.");

                this.DatoAInterfazUsuario(lst[0]);
            }
            catch (Exception ex)
            {
                this.DatoAInterfazUsuario(new ConfiguracionAlertaBO());
                throw new Exception(this.nombreClase + ".ConsultarCompleto:" + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        public void LimpiarSesion()
        {
            this.vista.LimpiarSesion();
            this.vistaDetalle.LimpiarSesion();
        }

        /// <summary>
        /// Redirecciona al visor anterior
        /// </summary>
        public void Regresar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }

        /// <summary>
        /// Redirecciona al visor de edición del registro en curso
        /// </summary>
        public void IrAEditar()
        {
            try
            {
                #region SC0008 - Envio de datos completo para las configuraciones de perfil facturista
                ConfiguracionAlertaBO configuracion = new ConfiguracionAlertaBO();
                if (this.vista.PerfilID.HasValue)
                {
                    ConfiguracionAlertaPerfilBO configuracionPerfil = new ConfiguracionAlertaPerfilBO();
                    configuracionPerfil.Perfil = new BPMO.Seguridad.BO.PerfilBO { Id = this.vista.PerfilID.Value };
                    configuracion = configuracionPerfil;
                }

                configuracion.ConfiguracionAlertaID = this.vista.ConfiguracionAlertaID;
                configuracion.Empleado = new EmpleadoBO { Id = this.vista.EmpleadoID, NombreCompleto = this.vista.EmpleadoNombre, Email = this.vista.CorreoElectronico };
                configuracion.Sucursal = new SucursalBO { Id = this.vista.SucursalID, Nombre = this.vista.SucursalNombre };                
                configuracion.NumeroDias = this.vista.NumeroDias;
                #endregion

                this.vista.EstablecerPaqueteNavegacion("ConfiguracionAlertaBO", configuracion);
                this.vista.RedirigirAEditar();
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".IrAEditar: " + ex.GetBaseException().Message);
            }
        }
        #endregion
    }
}
