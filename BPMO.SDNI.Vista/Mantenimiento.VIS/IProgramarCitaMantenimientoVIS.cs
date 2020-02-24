//Satisface al caso de uso CU025
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interfaz utilizada para la UI de Registro de Citas de Mantenimiento
    /// </summary>
    public interface IProgramarCitaMantenimientoVIS
    {
        #region Propiedades
        #endregion
        #region Metodos
        /// <summary>
        /// Redirige a la pantalla que presenta que el usuario no tiene permisos
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Redirige a la interfaz de consulta
        /// </summary>
        void RedirigirConsulta();
        #endregion
    }
}
