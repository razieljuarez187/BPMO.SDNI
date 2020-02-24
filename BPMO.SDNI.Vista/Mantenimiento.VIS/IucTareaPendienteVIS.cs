//Satisface el CU063 - Administrar Tareas Pendientes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Equipos.BO;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interface para el control de la vista
    /// </summary>
    public interface IucTareaPendienteVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece el identificador de la tarea pendiente
        /// </summary>
        int? TareaPendienteID { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del modelo
        /// </summary>
        int? ModeloID { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        int? UnidadID { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la unidad operativa
        /// </summary>
        int? UnidadOperativaID { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de serie de la unidad
        /// </summary>
        string NumeroSerie { get; set; }

        /// <summary>
        /// Obtiene o establece el numero economico de la unidad
        /// </summary>
        string NumeroEconomico { get; set; }

        /// <summary>
        /// Obtiene o establece el modelo de la unidad
        /// </summary>
        string Modelo { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de la tarea pendiente
        /// </summary>
        string Descripcion { get; set; }

        /// <summary>
        /// Obtiene o estable el estatus de la Tarea
        /// </summary>
        bool? Activo { get; set; }

        /// <summary>
        /// Obtiene la fecha de creación
        /// </summary>
        DateTime? FC { get; }

        /// <summary>
        /// Obtiene la fecha de ultima actualizacion
        /// </summary>
        DateTime? FUA { get; }

        /// <summary>
        /// Obtiene el identificador del usuario creador
        /// </summary>
        int? UC { get; }

        /// <summary>
        /// Obtiene el identificador del usuario ultima actualizacion
        /// </summary>
        int? UUA { get; }

        /// <summary>
        /// Obtiene el usuario
        /// </summary>
        UsuarioBO Usuario { get; }

        /// <summary>
        /// Obtiene la unidad operativa
        /// </summary>
        UnidadOperativaBO UnidadOperativa { get;}
        #endregion

        #region Métodos
        /// <summary>
        /// Limpia los datos en sesión
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Despliega un mensaje
        /// <param name="mensaje">Mensaje por desplegar</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="msjDetalle">Detalle del mensaje</param>
        /// </summary>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        /// <summary>
        /// Prepara la vista inicial de la UI
        /// </summary>
        void PrepararVista();

        /// <summary>
        /// Deshabilita campos de edicion para detalle
        /// </summary>
        void DeshabilitarCamposEdicion();

        /// <summary>
        /// Habilita o inhabilita la edición de los campos de estatus
        /// </summary>
        /// <param name="permitir">Indica si permitirá manipulación o no</param>
        void PermitirEdicionEstatus(bool permitir);

        /// <summary>
        /// Redirige a la UI de consulta
        /// </summary>
        void RedirigirAConsulta();

        /// <summary>
        /// Redirige a la UI de detalles
        /// </summary>
        void RedirigirADetalles();

        /// <summary>
        /// Obtiene los datos para mostrar en la UI
        /// <returns>Objeto por desplegar en la UI</returns>
        /// </summary>
        object ObtenerDatosNavegacion();

        /// <summary>
        /// Establece datos por desplegar en la UI
        /// <param name="nombre">Identificador de objeto</param>
        /// <param name="valor">Valor por desplegar</param>
        /// </summary>
        void EstablecerPaqueteNavegacion(string p, object bo);
        #endregion
    }
}
