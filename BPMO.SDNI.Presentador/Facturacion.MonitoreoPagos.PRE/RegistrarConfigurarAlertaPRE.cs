//Satisface el caso de uso CU009 – Configuración Notificación de facturación
//Satisface a la solicitud de cambios SC0008

using System;
using System.Collections.Generic;
using System.Data;
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
    /// Presentador para la vista de registro de notificaciones de empleados
    /// </summary>
    public class RegistrarConfigurarAlertaPRE
    {
        #region Atributos
        /// <summary>
        /// El DataContext que proveerá acceso a la base de datos
        /// </summary>
        private IDataContext dctx;

        /// <summary>
        /// La vista que esta mostrando los datos procesados en el presentador
        /// </summary>
        private IRegistrarConfigurarAlertaVIS vista;

        /// <summary>
        /// Presentador que esta gestionando la vista que visualiza los campos del registro
        /// </summary>
        private ucConfigurarAlertaPRE presentadorDetalle;

        /// <summary>
        /// Controlador de Consulta de configuración de alerta
        /// </summary>
        private ConfiguracionAlertaBR controlador;

        /// <summary>
        /// Nombre de la clase para agregar a los mensajes de Error
        /// </summary>
        private const string nombreClase = "RegistrarConfigurarNotificacionPRE";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor por default del presentador
        /// </summary>
        /// <param name="vista">Vista que será gestionada por el presentador</param>
        /// <param name="vistaConfigurarNotificacion">Vista que mostrará los campos del registro procesado</param>
        public RegistrarConfigurarAlertaPRE(IRegistrarConfigurarAlertaVIS vista, IucConfigurarAlertaVIS vistaConfigurarNotificacion)
        {
            try
            {
                this.vista = vista;
                this.presentadorDetalle = new ucConfigurarAlertaPRE(vistaConfigurarNotificacion);
                this.controlador = new ConfiguracionAlertaBR();
                this.dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                this.vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarOperadorPRE: " + ex.GetBaseException().Message);
            }
        }
        #endregion Constructor

        #region Métodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>
        public void PrepararNuevo()
        {
            this.LimpiarSesion();

            this.vista.PrepararNuevo();
            this.presentadorDetalle.PrepararNuevo();

            this.EstablecerSeguridad();
        }

        /// <summary>
        /// Establecer las reglas de seguridad que se tendran para los controles y navegación del visor
        /// </summary>
        public void EstablecerSeguridad()
        {
            try
            {
                //se valida que los datos del usuario y la unidad operativa no sean nulos
                if (this.vista.UC == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista.UnidadOperativaID == null) throw new Exception("La Unidad Operativa no debe ser nula ");

                //Se crea el objeto de seguridad
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se obtienen las acciones a las cuales el usuario tiene permiso en este proceso
                List<CatalogoBaseBO> lst = FacadeBR.ConsultarAccion(this.dctx, seguridadBO);

                //Se valida si el usuario tiene permiso para insertar operador
                if (!this.ExisteAccion(lst, "INSERTAR"))
                    this.vista.RedirigirSinPermisoAcceso();
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
        /// Genera un registro a partir de los campos capturados en la vista
        /// </summary>
        /// <returns>Objeto generador a partir de los datos visualizados</returns>
        private object InterfazUsuarioADato()
        {
            ConfiguracionAlertaBO bo = new ConfiguracionAlertaBO
            {
                NumeroDias = this.vista.NumeroDias,
                Estatus = this.vista.Estatus,
                Sucursal = new SucursalBO
                {
                    Id = vista.SucursalID,
                    UnidadOperativa = new UnidadOperativaBO
                    {
                        Id = this.vista.UnidadOperativaID
                    }
                },
                Empleado = new EmpleadoBO 
                { 
                    Id = vista.EmpleadoID 
                },
                Auditoria = new AuditoriaBO 
                {
                    FC = this.vista.FC,
                    FUA = this.vista.FUA,
                    UC = this.vista.UC,
                    UUA = this.vista.UUA
                }
            };

            return bo;
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
        /// Realiza la validación de los datos capturados para detectar errores o inconsistencias
        /// </summary>
        /// <returns>Objeto de tipo String que contiene el error detectado, en caso contrario devolverá nulo</returns>
        public String ValidarCampos()
        {
            string s = "";

            if (this.vista.UnidadOperativaID == null)
                s += "Unidad Operativa, ";
            if (this.vista.SucursalID == null)
                s += "Sucursal, ";
            if (this.vista.EmpleadoID == null)
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
                Sucursal = new SucursalBO { Id = this.vista.SucursalID },
                Empleado = new EmpleadoBO { Id = this.vista.EmpleadoID }               
            };

            SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

            //SC0008 - Se cambia el proceso de Validación de existencia considerando los empleados facturistas que tienen configuración automática            
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
        public void Registrar()
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

                this.controlador.Insertar(this.dctx, bo, seguridadBO);

                //Se consulta lo insertado para recuperar los ID
                DataSet ds = this.controlador.ConsultarSet(this.dctx, bo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Al consultar lo insertado no se encontraron coincidencias.");
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("Al consultar lo insertado se encontró más de una coincidencia.");

                bo.ConfiguracionAlertaID = this.controlador.DataRowToConfiguracionAlertaBO(ds.Tables[0].Rows[0]).ConfiguracionAlertaID;

                this.LimpiarSesion();
                this.vista.EstablecerPaqueteNavegacion("ConfiguracionAlertaBO", bo);
                this.vista.RedirigirADetalles();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".Registrar: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Cancela el nuevo registro a insertar y se rediriege al visor invocador
        /// </summary>
        public void Cancelar()
        {
            this.LimpiarSesion();
            this.vista.RedirigirAConsulta();
        }
        #endregion
    }
}
