//Satisface al caso de uso CU025
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interfaz para la vista de programacion de mantenimientos
    /// </summary>
    public interface IProgramarMantenimientosVIS
    {
        #region Atributos
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        int? UsuarioID { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de sucursal para las que aplica la configuración
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal para las que aplica la configuración
        /// </summary>
        String SucursalNombre { get; set; }

        /// <summary>
        /// Identificador del taller
        /// </summary>
        int? TallerID { get; }

        /// <summary>
        /// Nombre del taller
        /// </summary>
        string NombreTaller { get; set; }

        /// <summary>
        /// Determina si el taller es externo
        /// </summary>
        bool? esExterno { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de creación del registro
        /// </summary>
        DateTime? FC { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de última actualización del registros
        /// </summary>
        DateTime? FUA { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que creo el registro
        /// </summary>
        int? UC { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del usuario que actualizó por última vez el registro
        /// </summary>
        int? UUA { get; }

        /// <summary>
        /// Listado de talleres
        /// </summary>
        List<TallerBO> Talleres { get; set; }

        /// <summary>
        /// Listado de Citas de Mantenimiento
        /// </summary>
        List<CitaMantenimientoBO> CitasMantenimiento { get; set; }

        #region Propiedades Detalle
        /// <summary>
        /// Listado de Mantto de equipos aliados
        /// </summary>
        List<MantenimientoProgramadoEquipoAliadoBO> ListadoManttoEquiposAliados { get; set; }
        /// <summary>
        /// Listado de contactos del cliente
        /// </summary>
        List<ContactoClienteBO> ListadoContactosCliente { get; set; }
        /// <summary>
        /// Nombre del cliente de la unidad
        /// </summary>
        string ClienteNombre { get; set; }
        /// <summary>
        /// Area/Departamento de la Unidad
        /// </summary>
        string Area { get; set; }
        /// <summary>
        /// vin de la unidad
        /// </summary>
        string VINUnidad { get; set; }
        /// <summary>
        /// Numero economico de la unidad
        /// </summary>
        string NumeroEconomico { get; set; }
        /// <summary>
        /// Placa estatal de la unidad
        /// </summary>
        string PlacaEstatal { get; set; }
        /// <summary>
        /// Placa Federal de la unidad
        /// </summary>
        string PlacaFederal { get; set; }
        /// <summary>
        /// Km del ultimo servicio de la unidad
        /// </summary>
        int? KmUltimoServicio { get; set; }
        /// <summary>
        /// Fecha del Ultimo Servicio de la Unidad
        /// </summary>
        DateTime? FechaUltimoServicio { get; set; }
        /// <summary>
        /// Fecha Sugerida de Mantenimiento
        /// </summary>
        DateTime? FechaSugerida { get; set; }
        /// <summary>
        /// Tipo Mantenimiento
        /// </summary>
        string TipoMantenimiento { get; set; }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        string NombreSucursalDetalle { get; set; }
        /// <summary>
        /// Nombre del Taller
        /// </summary>
        string NombreTallerDetalle { get; set; }
        /// <summary>
        /// Estatus del Mantenimiento
        /// </summary>
        string EstatusMantenimiento { get; set; }
        /// <summary>
        /// Dias de retraso de ingreso de la unidad al taller
        /// </summary>
        int? DiasRetraso { get; set; }
        #endregion
        #endregion

        #region Metodos

        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
        void PrepararNuevo();

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Libera o elimina los valores en sesión guardados en el visor
        /// </summary>
        void LimpiarSesion();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        /// <summary>
        /// Establece los talleres a los que tiene acceso de acuerdo a la sucursal
        /// </summary>
        /// <param name="talleres">Talleres Permitidos</param>
        void EstablecerTalleres(List<TallerBO> talleres);

        /// <summary>
        /// Presentas las citas de mantenimiento en el calendario
        /// </summary>
        /// <param name="citasMantenimiento">Lista de Citas de Mantenimiento</param>
        void EstablecerMantenimientos(List<CitaMantenimientoBO> citasMantenimiento, List<DateTime> diasInhabiles);

        /// <summary>
        /// Coloca los equipos aliados en la interfaz
        /// </summary>
        void EstablecerEquiposAliados();

        /// <summary>
        /// Coloca los contactos de clientes en la interfaz
        /// </summary>
        void EstablecerContactosCliente();
        #endregion
    }
}
