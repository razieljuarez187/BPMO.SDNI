// Satisface al CU064 - Enviar Correo Servicio Realizado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Mantenimiento.BO.BOF;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.VIS {

    /// <summary>
    /// Interface que define las acciones que debe de contener una vista que ejecuta la funcionalidad de Envio de Correo
    /// de Servicio Realizado.
    /// </summary>
    public interface IEnviarCorreoServicioRealizadoVIS {

        #region Atributos

            #region Seguridad

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Usuario Actual de la sesión en curso.
        /// </summary>
        int? UsuarioAutenticado { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador de la Unidad Operativa Actual de la sesión en curso.
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del Módulo desde el cual se está accesando.
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Configuración de la Unidad Operativa que indica a que libro corresponden los activos.
        /// </summary>
        string LibroActivos { get; set; }

            #endregion

        /// <summary>
        /// Obtiene o establece un valor que representa el Mantenimiento Seleccionado.
        /// </summary>
        MantenimientoBOF Mantenimiento { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que reprenta el Diccionario de datos de la información a enviar.
        /// </summary>
        Dictionary<string, string> Datos { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la Lista de Contactos Clientes disponibles para recibir el Correo.
        /// </summary>
        List<DetalleContactoClienteBO> ContactosCliente { get; set; }
        
        /// <summary>
        /// Obtiene o establece un valor que representa el Contacto Cliente disponible para recibir Correos.
        /// </summary>
        DetalleContactoClienteBO ContactoClienteSeleccionado { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Taller del servicio.
        /// </summary>
        TallerBO Taller { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Título del Mensaje a enviar.
        /// </summary>
        string TituloMensaje { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Cuerpo del Mensaje a enviar.
        /// </summary>
        string CuerpoMensaje { get; set; }

        /// <summary>
        /// Obtiene la ruta del sistema.
        /// </summary>
        string RootPath { get; }

        #endregion

        #region Propiedades
        /// <summary>
        /// Indica si se imprimirán las Tareas Pendientes en el Correo
        /// </summary>
        bool ImprimirPendientes { get; }
        /// <summary>
        /// Lista de Tareas Pendientes de
        /// </summary>
        List<TareaPendienteBO> ListaTareasPendientes { get; set; }
        #endregion /Propiedades

        #region Métodos

        #region Seguridad

        /// <summary>
        /// Realiza la redirección a la página de advertencia por falta de permisos.
        /// </summary>
        void RedirigirSinPermisoAcceso();
            
            #endregion

        /// <summary>
        /// Realiza la redirección al visor de Registro de Mantenimientos Idealease.
        /// </summary>
        void RedireccionarARegistrarUnidad();

        /// <summary>
        /// Despliega la información del Mantenimiento Unidad Idealease realizado así como su Mantenimiento 
        /// Programado.
        /// </summary>
        void CargarDatosUnidad();

        /// <summary>
        /// Despliega la información del Contacto Cliente configurado para recibir correos.
        /// </summary>
        void CargarDatosContactoCliente();

        /// <summary>
        /// Despliega un mensaje en pantalla.
        /// </summary>
        /// <param name="mensaje">Mensaje a desplega.r</param>
        /// <param name="tipo">Tipo de mensaje a desplegar.</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar.</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #endregion

    }
}
