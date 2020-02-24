//Satisface al caso de uso PLEN.BEP.15.MODMTTO.CU030.Recalendarizar.Mantenimiento
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interfaz utilizada para la UI de Registro de Citas de Mantenimiento
    /// </summary>
    public interface IReprogramarCitaMantenimientoVIS
    {
        #region Propiedades
        #endregion
        #region Metodos
        /// <summary>
        /// Redirige a la pantalla que presenta que el usuario no tiene permisos
        /// </summary>
        void RedirigirSinPermisoAcceso();
        #endregion
    }
}
