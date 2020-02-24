//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interface para el control de la vista
    /// </summary>
    public interface IEditarTareaPendienteVIS
    {
        #region Métodos
        /// <summary>
        /// Prepara la UI inicial
        /// </summary>
        void PrepararVista();

        /// <summary>
        /// Redirige a la UI de sin permisos
        /// </summary>
        void RedirigirSinPermisoAcceso();
        #endregion
    }
}
