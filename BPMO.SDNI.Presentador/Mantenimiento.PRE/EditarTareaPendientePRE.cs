//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.VIS;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Facade.SDNI.BR;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.PRE
{
    /// <summary>
    /// Presentador para la UI de editar tareas pendientes
    /// </summary>
    public class EditarTareaPendientePRE
    {
        #region Atributos
        /// <summary>
        /// Vista de editar tarea pendiente
        /// </summary>
        private IEditarTareaPendienteVIS vista;

        /// <summary>
        /// Vista general de tarea pendiente
        /// </summary>
        private IucTareaPendienteVIS vista1;

        /// <summary>
        /// Presentador general de tarea pendiente
        /// </summary>
        private ucTareaPendientePRE presentador1;

        /// <summary>
        /// Contexto de la aplicacion
        /// </summary>
        private IDataContext dctx = FacadeBR.ObtenerConexion();

        /// <summary>
        /// Nombre de la clase
        /// </summary>
        private const string nombreClase = "EditarTareaPendientePRE";
        #endregion

        #region Constructores
        public EditarTareaPendientePRE(IEditarTareaPendienteVIS view, IucTareaPendienteVIS view1)
        {
            this.vista = view;
            this.vista1 = view1;
            this.presentador1 = new ucTareaPendientePRE(view1);
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
                if (!FacadeBR.ExisteAccion(this.dctx, "UI ACTUALIZAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "CONSULTAR", seguridadBO) ||
                    !FacadeBR.ExisteAccion(this.dctx, "ACTUALIZAR", seguridadBO))
                    this.vista.RedirigirSinPermisoAcceso();
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ValidarAcceso: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Cancela la edición de una tarea pendiente
        /// </summary>
        public void CancelarEdicion()
        {
            this.presentador1.CancelarEdicion();
        }

        /// <summary>
        /// Prepara la edición de una tarea pendiente
        /// </summary>
        public void PrepararEditar()
        {
            this.presentador1.CargarObjetoInicio();
        }

        /// <summary>
        /// Realiza la edición de la tarea pendiente
        /// </summary>
        public void Editar()
        {
            this.presentador1.Editar();
        }

        /// <summary>
        /// Prepara la vista inicial de la UI
        /// </summary>
        private void PrepararVista()
        {
            this.vista.PrepararVista();
        }
        #endregion
    }
}
