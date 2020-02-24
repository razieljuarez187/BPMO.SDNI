//Esta clase satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista para la página de reasignación de departamento para la unidad
    /// </summary>
    public interface ICambiarDepartamentoUnidadVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el módulo en el cual esa trabajando el usuario autenticado en el sistema
        /// </summary>
        int? ModuloID { get; }
        /// <summary>
        /// Obtiene el identificador de la unidad operativa del usuario autenticado en el sistema
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Obtiene el identificador del usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }

        #region DetalleUnidad
        /// <summary>
        /// Obtiene o establece el identificador de la unidad
        /// </summary>
        int? UnidadID { get; set; }
        /// <summary>
        /// Obtiene o establece el numéro de serie de la unidad
        /// </summary>
        string NumeroSerie { get; set; }
        /// <summary>
        /// Obtiene o establece la clave del activo de oracle para la unidad
        /// </summary>
        string ClaveActivoOracle { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la unidad en líder
        /// </summary>
        int? LiderID { get; set; }
        /// <summary>
        /// Obtiene o establece el numéro económico de la unidad
        /// </summary>
        string NumeroEconomico { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del tipo de la unidad
        /// </summary>
        string TipoUnidadNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del modelo de la unidad
        /// </summary>
        string ModeloNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el año de la unidad
        /// </summary>
        int? Anio { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha de compra de la unidad
        /// </summary>
        DateTime? FechaCompra { get; set; }
        /// <summary>
        /// Obtiene o establece el monto de la factura de la unidad
        /// </summary>
        decimal? MontoFactura { get; set; }
        /// <summary>
        /// Obtiene o establece el folio de la factura de compra de la unidad
        /// </summary>
        string FolioFactura { get; set; }
        #endregion

        #region SucursalActual
        /// <summary>
        /// Identificador de la sucursal actual
        /// </summary>
        int? SucursalActualID { get; set; }
        /// <summary>
        /// Nombre de la sucursal actual
        /// </summary>
        string SucursalActualNombre { get; set; }
        /// <summary>
        /// Domicilio de la sucursal actual
        /// </summary>
        string DomicilioSucursalActual { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la empresa actual
        /// </summary>
        int? EmpresaActualID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la empresa actual
        /// </summary>
        string EmpresaActualNombre { get; set; }
        #endregion

        #region BarraHerramientas
        /// <summary>
        /// Obtiene o establece si la unidad esta disponible
        /// </summary>
        bool? EstaDisponible { get; set; }
        /// <summary>
        /// Obtiene o establece si la unidad se encuentra en un contrato
        /// </summary>
        bool? EstaEnContrato { get; set; }
        /// <summary>
        /// Obtiene o establece si la unidad tiene equipos aliados
        /// </summary>
        bool? TieneEquipoAliado { get; set; }
        /// <summary>
        /// Obtiene o establece el número de placa de la unidad
        /// </summary>
        string NumeroPlaca { get; set; }
        #endregion

        /// <summary>
        /// Identificador del departamento actual
        /// </summary>
        int? DepartamentoActualID { get; set; }
        /// <summary>
        /// Identificador del departamento destino
        /// </summary>
        int? DepartamentoDestinoID { get; set; }
        /// <summary>
        /// Obtiene o establece el propietario de la unidad
        /// </summary>
        string Propietario { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del propietario de la unidad
        /// </summary>
        int? PropietarioID { get; set; }
        /// <summary>
        /// Obtiene o establece el Nombre del cliente de la unidad
        /// </summary>
        string Cliente { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del cliente de la unidad
        /// </summary>
        int? ClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones de la reasignación
        /// </summary>
        string Observaciones { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la unidad operativa
        /// </summary>
        string NombreClienteUnidadOperativa { get; set; }
        /// <summary>
        /// Obtiene o establece el objeto que se esta editando
        /// </summary>
        object ObjetoEdicion { get; set; }
        /// <summary>
        /// Obtiene o establece la inforamción de la unidad que se esta editando
        /// </summary>
        object UltimoObjeto { get; set; }

        ETipoEmpresa EnumTipoEmpresa { set; get; }

        #endregion

        #region métodos
        /// <summary>
        /// Limpia la variable de session los elementos que se usan en la vista
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Limpia el paquete de la session
        /// </summary>
        /// <param name="p">Clave del paquete que se desea eliminar</param>
        void LimpiarPaqueteNavegacion(string key);
        /// <summary>
        /// Establece un paquete de navegación para su posterior consulta
        /// </summary>
        /// <param name="key">Clave del paquete</param>
        /// <param name="value">Paquete que se desea guardar</param>
        void EstablecerPaqueteNavegacion(string key, object value);
        /// <summary>
        /// Obtiene un paquete de navegación para usar en la página
        /// </summary>
        /// <param name="key">Clave del paquete que se desea obtener</param>
        /// <returns>Paquete solicitado</returns>
        object ObtenerPaqueteNavegacion(string key);
        /// <summary>
        /// Redirige a la página de detalle de unidad
        /// </summary>
        void RedirigirADetalles();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Habilita o deshabilita lso controles de consulta
        /// </summary>
        /// <param name="status">estatus que desea aplicar a los controles</param>
        void PermitirConsultar(bool status);
        /// <summary>
        /// Establece el estatus para los controles de registro
        /// </summary>
        /// <param name="status">Estatus que será aplicado al control</param>
        void PermitirRegistrar(bool status);
        /// <summary>
        /// Carga una lista de opciones de departamentos para las unidades
        /// </summary>
        /// <param name="items">Departamentos disponibles para las unidades</param>
        void CargarDepartamentos(Dictionary<int, string> items);
        /// <summary>
        /// Prepara la vista para el cambio de departamento
        /// </summary>
        void PrepararEdicion();
        /// <summary>
        /// Habilita o deshabilita la slección de un propietario para la unidad
        /// </summary>
        /// <param name="habilitar">Estatus que se aplica al control</param>
        void HabilitarPropietario(bool habilitar);
        /// <summary>
        /// Habilita o deshabilita al aselección de un cliente para la unidad
        /// </summary>
        /// <param name="habilitar">Estatus que se aplica al control</param>
        void HabilitarCliente(bool habilitar);
        /// <summary>
        /// Despliega un mensaje en la vista
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo del mensaje que es desplegado</param>
        /// <param name="msjDetalle">Detalle del mensaje desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}