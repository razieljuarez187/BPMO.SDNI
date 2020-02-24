//Satisface al CU019 - Reporte de Flota Activa de RD Registrados

using BPMO.Primitivos.Enumeradores;
using System.Collections.Generic;

namespace BPMO.SDNI.Reportes.VIS
{
    /// <summary>
    /// Interface que aplica una página de generación de reportes
    /// </summary>
    public interface IReportPageVIS
    {
        /// <summary>
        /// Identificador del Modulo
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        int? UsuarioID { get; }      

        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Método abstract que se ejecuta cuando 
        /// </summary>
        void Consultar();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        /// <summary>
        /// Despliega el visor de los formatos a imprimir
        /// </summary>
        void IrAImprimir();

        /// <summary>
        /// Establece el Paquete de Navegacion para imprimir el reporte
        /// </summary>
        /// <param name="clave">Clave del Paquete de Navegacion</param>
        /// <param name="datosReporte">Datos del Reporte</param>
        void EstablecerPaqueteNavegacionImprimir(string clave, Dictionary<string, object> datosReporte);

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();
    }
}
