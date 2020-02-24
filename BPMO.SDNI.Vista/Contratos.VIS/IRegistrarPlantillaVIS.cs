// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.VIS
{
    /// <summary>
    /// Vista para el registro de los documentos que serán usados como plantilla en los contratos
    /// </summary>
    public interface IRegistrarPlantillaVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el identificador de la unidad operativa
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Obtiene el identificador del usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Obtiene o establece el modulo al que pertenece la plantilla de acuerdo a la enumeracion  <see cref="BPMO.SDNI.Contratos.BO.EModulo"/>
        ///<para>SI deseas visualizar los valores correspondientes para la propiedad consulta la siguiente enumeración:</para>
        /// <seealso cref="BPMO.SDNI.Contratos.BO.EModulo"/>
        /// </summary>
        int? TipoPlantilla { get; set; }
        /// <summary>
        /// Obtiene el nombre del archivo seleccionado
        /// </summary>
        string NombreArchivo { get; }
        /// <summary>
        /// Obtiene o establece el tipo que corresponde al archivo
        /// </summary>
        object TipoArchivo { get; set; }
        /// <summary>
        /// Onbtiene la extención del archivo seleccionado
        /// </summary>
        string ExtencionArchivo { get; }
        /// <summary>
        /// Obtiene o establece los tipos de archivos configurados
        /// </summary>
        object TiposArchivo { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Obtiene el arreglo de bytes que corresponde al archivo
        /// </summary>
        /// <returns>arreglo de byte</returns>
        byte[] ObtenerArchivosBytes();
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje que es desplegado</param>
        /// <param name="tipo">Tipo del mensaje que es desplegao</param>
        /// <param name="detalle">Detalle del mensaje que es desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Limpia la sesion de la página
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Prepara la interfaz para un nuevo registro
        /// </summary>
        void PrepararNuevo();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Redirige a la página de consulta
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Guarda un paquete en la variable de session
        /// </summary>
        /// <param name="key">Clave para identificar el paquete</param>
        /// <param name="value">Paquete que se desea guardar en sesion</param>
        void EstablecerPaqueteNavegacion(string key, object value);
        /// <summary>
        /// Bloquea la acción de registro en el sistema a demanda
        /// </summary>
        void BloquearRegistro(bool status);
        /// <summary>
        /// Habilita o deshabilita la opción de consulta
        /// </summary>
        /// <param name="p">Estatus que se va a aplicar</param>
        void BloquearConsulta(bool p);
        /// <summary>
        /// Establece las opciones válidas para los tipos de plantilla de documentos
        /// </summary>
        /// <param name="opciones">opciones</param>
        void EstablecerOpcionesTipoPlantilla(Dictionary<int, string> opciones);
        #endregion                 
    }
}