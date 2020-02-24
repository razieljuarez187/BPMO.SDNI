//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.SDNI.Mantenimiento.BR;
using BPMO.Facade.SDNI.BR;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// Presentador para la UI de registrar tareas pendientes
    /// </summary>
    public class RegistrarTareaPendientePRE
    {
        #region Atributos
        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private const string nombreClase = "RegistrarTareaPendientePRE";

        /// <summary>
        /// Contexto de la aplicacion
        /// </summary>
        private readonly IDataContext dctx;

        /// <summary>
        /// Vista de registrar tarea pendiente
        /// </summary>
        private IRegistrarTareaPendienteVIS vista;

        /// <summary>
        /// Vista general de tarea pendiente
        /// </summary>
        private IucTareaPendienteVIS vista1;

        /// <summary>
        /// Controlador de tareas pendientes
        /// </summary>
        private TareaPendienteBR tareaBR;
        #endregion  

        #region Constructor
        public RegistrarTareaPendientePRE(IRegistrarTareaPendienteVIS vista,IucTareaPendienteVIS vista1)
        {
            try
            {
                this.vista = vista;
                this.vista1 = vista1;
                tareaBR = new TareaPendienteBR();
                dctx = FacadeBR.ObtenerConexion();
            }
            catch (Exception ex)
            {
                vista.MostrarMensaje("Inconsistencias en los parámetros de configuración", ETipoMensajeIU.ERROR, nombreClase + ".RegistrarTareaPendientePRE: " + ex.Message);
            }
        }
        #endregion  

        #region Métodos

        /// <summary>
        /// Crea un objeto de seguridad con los datos actuales de la sesión de usuario en curso
        /// </summary>
        /// <returns>Objeto de tipo seguridad</returns>
        private SeguridadBO CrearObjetoSeguridad()
        {
            UsuarioBO usuario = new UsuarioBO() { Id = this.vista1.Usuario.Id };
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = new UnidadOperativaBO() { Id = this.vista1.UnidadOperativaID } };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);

            return seguridadBO;
        }

        /// <summary>
        /// Valida el acceso a la sección en curso
        /// </summary>
        public void ValidarAcceso()
        {
            try
            {
                //Valida que usuario y la unidad operativa no sean nulos
                if (this.vista1.Usuario.Id == null) throw new Exception("El identificador del usuario no debe ser nulo");
                if (this.vista1.UnidadOperativaID == null) throw new Exception("El identificador de la Unidad Operativa no debe ser nulo");
                //Se crea el objeto de seguridad                
                SeguridadBO seguridadBO = this.CrearObjetoSeguridad();

                //Se valida si el usuario tiene permiso a la acción principal
                if (!FacadeBR.ExisteAccion(this.dctx, "UI INSERTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "INSERTAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Realiza el registro de la tarea pendiente a traves del controlador
        /// </summary>
        public void Registrar()
        {
            string s;
            if (!String.IsNullOrEmpty((s = this.ValidarDatos())))
            {
                this.vista.MostrarMensaje("Los siguientes datos son incorrectos:" + s.Substring(1), ETipoMensajeIU.ADVERTENCIA, null);
                return;
            }

            TareaPendienteBO tarea = InterfazUsuarioADato();
             //Se crea el objeto de seguridad
            UsuarioBO usuario = this.vista1.Usuario;
            AdscripcionBO adscripcion = new AdscripcionBO() { UnidadOperativa = this.vista1.UnidadOperativa };
            SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, usuario, adscripcion);
            tareaBR.Insertar(dctx, tarea, seguridadBO);
            this.vista.GuardarMensajeExitoso();
            this.vista.EstablecerPaqueteNavegacion("TareaPendienteBO", tarea);
            this.vista.RedirigirADetalle();
        }

        /// <summary>
        /// Cancela el registro de la tarea pendiente y redirige a la consulta
        /// </summary>
        public void CancelarRegistro()
        {
            this.vista1.LimpiarSesion();
            this.vista1.RedirigirAConsulta();
        }

        /// <summary>
        /// Pasa los datos de la interfaz de usuario a un objeto para consultar
        /// </summary>
        public TareaPendienteBO InterfazUsuarioADato() {
            TareaPendienteBO tarea;
            tarea = new TareaPendienteBO();
            tarea.Modelo = new Servicio.Catalogos.BO.ModeloBO();
            tarea.Modelo.Id = vista1.ModeloID;
            tarea.Modelo.Nombre = vista1.Modelo;
            tarea.Unidad = new Equipos.BO.UnidadBO();
            tarea.Unidad.UnidadID = vista1.UnidadID;
            tarea.Unidad.NumeroSerie = vista1.NumeroSerie;
            tarea.Unidad.NumeroEconomico = vista1.NumeroEconomico;
            tarea.Descripcion = vista1.Descripcion;
            tarea.Activo = vista1.Activo;
            return tarea;
        }

        /// <summary>
        /// Realiza la validación de los datos ingresados en la UI
        /// </summary>
        public string ValidarDatos() {
            string s = string.Empty;
            try {
                if (this.vista1.NumeroSerie == null)
                    s += ", Número de Serie es requerido";
                if (this.vista1.Descripcion == null)
                    s += ", Descripción es requerido";
                if (this.vista1.Activo == null)
                    s += ", Estatus es requerido";
            }
            catch (Exception ex) {
                vista.MostrarMensaje("Inconsistencias al validar los datos", ETipoMensajeIU.ERROR, nombreClase + ".ValidarDatos: " + ex.Message);
            }

            return s;
        }
        #endregion
    }
}
