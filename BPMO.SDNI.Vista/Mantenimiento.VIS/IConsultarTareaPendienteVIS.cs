//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interface para el control de la vista
    /// </summary>
    public interface IConsultarTareaPendienteVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece una lista de tareas pendientes
        /// </summary>
        List<TareaPendienteBO> Tareas { get; set; }

        /// <summary>
        /// Obtiene o establece un indice de pagina
        /// </summary>
        int IndicePaginaResultado { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Actualiza el resultado en la UI
        /// </summary>
        void ActualizarResultado();

        /// <summary>
        /// Establece datos por desplegar en la UI
        /// <param name="nombre">Identificador de objeto</param>
        /// <param name="valor">Valor por desplegar</param>
        /// </summary>
        void EstablecerPaqueteNavegacion(string nombre, object valor);

        /// <summary>
        /// Redirige a la UI de detalles
        /// </summary>
        void RedirigirADetalles();

        /// <summary>
        /// Limpia los datos en sesion
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Redirige a la UI de sin permisos
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Despliega un mensaje en la UI
        /// <param name="mensaje">Mensaje por desplegar</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="msjDetalle">Detalle del mensaje</param>
        /// </summary>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
