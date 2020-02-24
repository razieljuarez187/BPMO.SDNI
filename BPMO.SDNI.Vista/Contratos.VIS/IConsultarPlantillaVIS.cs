// Satisface al CU097 - Cargar y descargar formatos y plantillas
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.VIS
{
    /// <summary>
    /// Vista para la consulta de los documentos que serán usados como plantilla en los contratos
    /// </summary>
    public interface IConsultarPlantillaVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el identificador del usuario que ha iniciado sesion
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Obtiene el identificador de la unidad operativa correspondiente al usuario
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Obtiene o establece el modulo al que pertenece la plantilla de acuerdo a la enumeracion  <see cref="BPMO.SDNI.Contratos.BO.EModulo"/>
        /// <para>SI deseas visualizar los valores correspondientes para la propiedad consulta la siguiente enumeración:</para>
        /// <seealso cref="BPMO.SDNI.Contratos.BO.EModulo"/>
        /// </summary>
        int? TipoPlantilla { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del archivo
        /// </summary>
        string Nombre { get; set; }
        /// <summary>
        /// Obtiene o establece el estatus del archivo
        /// </summary>
        bool? Estatus { get; set; }
        /// <summary>
        /// Obtiene o establece el indice de la página de resultado
        /// </summary>
        int IndicePaginaResultado { get; set; }
        #endregion

        #region Métodos
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
        /// Limpia los resultados encontrado
        /// </summary>
        void LimpiarDocumentosEncontrados();
        /// <summary>
        /// Establece las opciones válidas para los tipos de plantilla de documentos
        /// </summary>
        /// <param name="opciones">opciones</param>
        void EstablecerOpcionesTipoPlantilla(Dictionary<int, string> opciones);
        /// <summary>
        /// Redirige a la página de Consulta de plantilla
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la página de registro de plantilla
        /// </summary>
        void RedirigirARegistro();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Actualiza el resultado de la consulta
        /// </summary>
        void ActualizarResultado();
        /// <summary>
        /// Habilita o deshabilita la opcion de registrar
        /// </summary>
        /// <param name="p">Estatus que se va a aplicar</param>
        void PermitirRegistrar(bool p);
        /// <summary>
        /// Habilita o deshabilita la opción de eliminar
        /// </summary>
        /// <param name="p">Estatus que se va a aplicar</param>
        void PermitirEliminar(bool p);
        #endregion                   
    }
}