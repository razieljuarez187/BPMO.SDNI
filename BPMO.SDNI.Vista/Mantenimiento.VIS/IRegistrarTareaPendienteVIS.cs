//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Equipos.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interface para el control de la vista
    /// </summary>
    public interface IRegistrarTareaPendienteVIS
    {
        #region Métodos
        /// <summary>
        /// Despliega un mensaje en la UI
        /// <param name="mensaje">Mensaje por desplegar</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="msjDetalle">Detalle del mensaje</param>
        /// </summary>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        /// <summary>
        /// Guarda el mensaje exitoso para desplegarlo en la UI
        /// </summary>
        void GuardarMensajeExitoso();

        /// <summary>
        /// Establece datos por desplegar en la UI
        /// <param name="nombre">Identificador de objeto</param>
        /// <param name="valor">Valor por desplegar</param>
        /// </summary>
        void EstablecerPaqueteNavegacion(string p, object bo);

        /// <summary>
        /// Redirige a la UI de detalles
        /// </summary>
        void RedirigirADetalle();

        /// <summary>
        /// Redirige a la UI de sin permisos
        /// </summary>
        void RedirigirSinPermisoAcceso();
        #endregion
    }
}
