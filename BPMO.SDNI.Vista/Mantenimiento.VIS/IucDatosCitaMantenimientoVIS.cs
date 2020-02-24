//Satisface al caso de uso CU026 - Calendarizar Mantenimiento
using System;
using System.Collections.Generic;
using BPMO.SDNI.Mantenimiento.BO;
using System.Web.UI.WebControls;
using BPMO.Servicio.Catalogos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    /// <summary>
    /// Interfaz utilizada para los datos de la UI de una Cita de Mantenimiento
    /// </summary>
    public interface IucDatosCitaMantenimientoVIS
    {
        #region Atributos
        #endregion
        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la unidad operativa
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del id del susuario
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
        int? TallerID { get; set; }
        /// <summary>
        /// Nombre del taller
        /// </summary>
        string NombreTaller { get; set; }
        /// <summary>
        /// Determina si el taller es externo
        /// </summary>
        bool? esExterno { get; set; }
        /// <summary>
        /// Listado de talleres
        /// </summary>
        List<TallerBO> Talleres { get; set; }
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
        /// Identificador de la Unidad
        /// </summary>
        int? UnidadID { get; set; }
        /// <summary>
        /// Identificador del Equipo de la Unidad
        /// </summary>
        int? EquipoID { get; set; }
        /// <summary>
        /// Identificador del cliente de la unidad
        /// </summary>
        int? ClienteID { get; set; }
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
        /// Tiempo Tomado para el total del mantenimiento
        /// </summary>
        string TipoMantenimiento { get; set; }
        /// <summary>
        /// Tiempo de la unidad en mantenimiento
        /// </summary>
        decimal? TiempoMantenimiento { get; set; }
        /// <summary>
        /// Fecha en la que se planea la cita
        /// </summary>
        DateTime? FechaCita { get; set; }
        /// <summary>
        /// Hora en la que se planea la cita
        /// </summary>
        TimeSpan? HoraCita { get; set; }
        /// <summary>
        /// Identificador del Mantenimiento Programado
        /// </summary>
        int? MantenimientoProgramadoID { get; set; }
        /// <summary>
        /// Identificador de la cita de mantenimiento
        /// </summary>
        int? CitaMantenimientoID { get; set; }
        /// <summary>
        /// Estatus de la cita de mantenimiento
        /// </summary>
        EEstatusCita? EstatusCita { get; set; }
        /// <summary>
        /// Identificador del ContactoCliente
        /// </summary>
        int? ContactoClienteID { get; set; }
        /// <summary>
        /// Establece si se habilita o no el nombre de la sucursal Sucural
        /// </summary>
        bool SucursalFiltroVisible { get; set; }
        /// <summary>
        /// Establece si se habilita o no el nombre del taller
        /// </summary>
        bool TallerFiltroVisible { get; set; }
        /// <summary>
        /// Establece si se habilita o no el botón ibtnBuscarSucursal
        /// </summary>
        bool BtnBuscarSucursalVisible { get; set; }
        /// <summary>
        /// Establece el texto del título
        /// </summary>
        String EtiquetaTitulo { get; set; }

        #endregion
        #region Metodos
        /// <summary>
        /// Realiza el proceso de inicializar el visor para capturar un nuevo registro
        /// </summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
        void PrepararNuevo();
        /// <summary>
        /// Establece los talleres a los que tiene acceso de acuerdo a la sucursal
        /// </summary>
        /// <param name="talleres">Talleres Permitidos</param>
        void EstablecerTalleres(List<TallerBO> talleres);
        /// <summary>
        /// Envia a sesion un objeto para redirigir a otra interfaz
        /// </summary>
        /// <param name="key">Llave utilizada para enviar el objeto</param>
        /// <param name="objeto">Objeto que sera enviado a otra interfaz</param>
        void EstablecerPaqueteNavegacion(string key, object objeto);
        /// <summary>
        /// Obtiene un objeto enviado desde otra interfaz
        /// </summary>
        /// <param name="key">Llave para obtener el objeto</param>
        /// <returns>Objeto que fue enviado por otra interfaz</returns>
        object ObtenerPaqueteNavegacion(string key);
        /// <summary>
        /// Coloca los equipos aliados en la interfaz
        /// </summary>
        void EstablecerEquiposAliados();
        /// <summary>
        /// Coloca los contactos de clientes en la interfaz
        /// </summary>
        void EstablecerContactosCliente();
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        #endregion
    }
}