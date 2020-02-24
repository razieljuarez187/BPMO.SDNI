//Satisface al CU082 - Registrar Movimiento de Flota
using System;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista para la página de cambio de sucursal de equipo aliado
    /// </summary>
    public interface ICambiarSucursalEquipoAliadoVIS
    {
        #region Propiedades

        /// <summary>
        /// Obtiene el identificador del modulo en el que se encuentra trabajando con el fin de obtener las configuraciones necesarias para editar el contrato
        /// </summary>
        int? ModuloID { get; }
        /// <summary>
        /// Obtiene o establece el último objeto de la página
        /// </summary>
        EquipoAliadoBO UltimoObjeto { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la undiad operativa
        /// </summary>
        int? UnidadOperativaID { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la empresa actual
        /// </summary>
        int? EmpresaActualID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la emresa actual
        /// </summary>
        string NombreEmpresaActual { get; set; }
        /// <summary>
        /// Obtiene o establece el domicilio de la sucursal
        /// </summary>
        string DomicilioSucursalActual { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal actual
        /// </summary>
        int? SucursalActualID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal actual
        /// </summary>
        string SucursalActualNombre { get; set; }
        /// <summary>
        /// Obtiene  o establece el identificador del equipo aliado que se va a editar
        /// </summary>
        int? EquipoAliadoID { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del equipo que se va aeditar
        /// </summary>
        int? EquipoID { get; set; }

        string NumeroSerie { get; set; }

        string Marca { get; set; }

        string Modelo { get; set; }

        int? ModeloID { get; set; }

        string AnioModelo { get; set; }

        decimal? PBV { get; set; }

        decimal? PBC { get; set; }

        string TipoEquipoNombre { get; set; }

        int? TipoEquipoID { get; set; }

        string OracleID { get; set; }

        int? EquipoLiderID { get; set; }
        /// <summary>
        /// Fecha de creación del equipo
        /// </summary>
        DateTime? FC { get; }
        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        DateTime? FUA { get; }
        /// <summary>
        /// obtiene o establece el identificador del usuario que crea el registrop
        /// </summary>
        int? UC { get; }
        /// <summary>
        /// Obtiene o establece el identificador del usuario que actualiza el registro
        /// </summary>
        int? UUA { get; }
        /// <summary>
        /// Usuario del Sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Unidad Operativa de Configurada
        /// </summary>
        UnidadOperativaBO UnidadOperativa { get; }
        /// <summary>
        /// Obtiene o establece las hroas iniciales del equipo
        /// </summary>
        int? HorasIniciales { get; set; }
        /// <summary>
        /// Obtien o establece el identificador del estatus del equipo alaiado
        /// </summary>
        int? EstatusID { get; set; }
        /// <summary>
        /// Obtiene o establece el estatus del equipo aliado
        /// </summary>
        string EstatusNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la sucursal destino
        /// </summary>
        int? SucursalDestinoID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal destino
        /// </summary>
        string SucursalDestinoNombre { get; set; }
        /// <summary>
        /// Obtien o establece el identificador de la emrpesa destino
        /// </summary>
        int? EmpresaDestinoID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la emrpesa destino
        /// </summary>
        string NombreEmpresaDestino { get; set; }
        /// <summary>
        /// Obtiene o establece la direcciónd e la sucursal destino
        /// </summary>
        string DomicilioSucursalDestino { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Prepara la vista para la edición del equipo aliado
        /// </summary>
        void PrepararVista();
        /// <summary>
        /// Limpia la variable de session de la vista
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Redirige a la pagian de consutla de seguimiento
        /// </summary>
        void RedirigirAConsultarSeguimiento();
        /// <summary>
        /// Redirige a la página de permiso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Hbailita o deshabilita los controles de consulta
        /// </summary>
        /// <param name="status">Estatus que se aplica a los controles de consulta</param>
        void PermitirConsultar(bool status);
        /// <summary>
        /// Habilita o deshabilita los controles de registro
        /// </summary>
        /// <param name="status">Estatus que se aplica a lso controles de registro</param>
        void PermitirRegistrar(bool status);
        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje">Descripción del mensaje</param>
        /// <param name="tipo">EMensaje tipo de mensaje a desplegar</param>
        /// <param name="detalle">Detalle del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #endregion
    }
}