// Esta clase satisface los requerimientos del CU028 - Editar Contrato de Mantenimiento
// Satisface al caso de uso CU003 - Calcular Monto a Facturar CM y SD
// Satisface a la solución del RI0008
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IEditarContratoManttoVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el módulo en el cual est atrabajando el usuario autenticado en el sistema
        /// </summary>
        int? ModuloID { get; }
        /// <summary>
        /// Obtiene el identificador de la unidad operativa del usuario autenticado en el sistema
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Nombre de la Unidad Operativa
        /// </summary>
        string UnidadOperativaNombre { get; }
        /// <summary>
        /// Obtiene el identificador del usuario autenticado en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// obtiene o establece el identificador de la sucursal para la que se elabora el contrato
        /// </summary>
        int? SucursalID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la sucursal en la que se elabora el contrato
        /// </summary>
        string SucursalNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador del contrato
        /// </summary>
        int? ContratoID { get; set; }
        /// <summary>
        /// Obtiene o establece el número del contrato
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// Obtiene el tipo del contrato que se esta realizando
        /// <para>Si desea consultar los detalles que puede tomar esta propiedad vea <see cref="BPMO.SDNI.Contratos.BO.ETipoContrato"/></para>
        /// </summary>
        int? TipoContrato { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del representante legal de la empresa
        /// </summary>
        string RepresentanteEmpresa { get; set; }
        /// <summary>
        /// Obtiene o establece el cliente del contrato
        /// </summary>
        int? ClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece si la cuenta del cliente es fisica o no
        /// </summary>
        bool? CuentaClienteFisica { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador de la cuenta
        /// </summary>
        int? CuentaClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre de la cuenta del cliente
        /// </summary>
        string CuentaClienteNombre { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de cuenta del cliente
        /// </summary>
        int? CuentaClienteTipoID { get; set; }
        /// <summary>
        /// Obtiene o establece la dirección completa del cliente
        /// </summary>
        string ClienteDireccionCompleta { get; set; }
        /// <summary>
        /// Obtiene o establece la calle del cliente
        /// </summary>
        string ClienteDireccionCalle { get; set; }
        /// <summary>
        /// Obtiene o establece el codigo postla del cliente
        /// </summary>
        string ClienteDireccionCodigoPostal { get; set; }
        /// <summary>
        /// Obtiene o establece la ciudad del cliente
        /// </summary>
        string ClienteDireccionCiudad { get; set; }
        /// <summary>
        /// Obtiene o establece el estado del cliente
        /// </summary>
        string ClienteDireccionEstado { get; set; }
        /// <summary>
        /// Obtiene o establece el municipio del cliente
        /// </summary>
        string ClienteDireccionMunicipio { get; set; }
        /// <summary>
        /// Obtiene o establece el pais del cliente
        /// </summary>
        string ClienteDireccionPais { get; set; }
        /// <summary>
        /// Obtiene o establece la colonia del cliente
        /// </summary>
        string ClienteDireccionColonia { get; set; }
        /// <summary>
        /// Identificador de la Dirección del Cliente.
        /// </summary>
        int? DireccionClienteID { get; set; }
        /// <summary>
        /// Obtiene o establece la moneda en la que se ejecuta el contrato
        /// </summary>
        string DivisaID { get; set; }
        /// <summary>
        /// Obtiene o establece el estatus del contrato
        /// </summary>
        int? EstatusContrato { get; set; }
        /// <summary>
        /// Obtiene o establece el
        /// </summary>
        DateTime? FechaContrato { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha en la que empieza la ejecución del contrato
        /// </summary>
        DateTime? FechaInicioContrato { get; set; }
        /// <summary>
        /// Obtiene o establece el plazo que abarca el contrato
        /// </summary>
        int? Plazo { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha de fin del contrato
        /// </summary>
        DateTime? FechaTerminacionContrato { get; set; }
        /// <summary>
        /// Obtiene o establece si el contrato solo cuenta con representantes legales
        /// </summary>
        bool? SoloRepresentantes { get; set; }
        /// <summary>
        /// obtiene o establece si los obligados del contrato tambien fungiran como avales
        /// </summary>
        bool? ObligadosComoAvales { get; set; }
        /// <summary>
        /// Obtiene o establece la ubicación del taller dnd se atenderán las unidades
        /// </summary>
        string UbicacionTaller { get; set; }
        /// <summary>
        /// Obtiene o establece el deposito en garantia para el contrato
        /// </summary>
        decimal? DepositoGarantia { get; set; }
        /// <summary>
        /// Obtiene o establece el importe de la comision por apertura
        /// </summary>
        decimal? ComisionApertura { get; set; }
        /// <summary>
        /// Obtiene o establece quien es el responsable del seguro
        /// </summary>
        int? IncluyeSeguro { get; set; }
        /// <summary>
        /// Obtiene o establece quien es el responsable del lavado
        /// </summary>
        int? IncluyeLavado { get; set; }
        /// <summary>
        /// Obtiene o establece quien el responsable de la pintura
        /// </summary>
        int? IncluyePinturaRotulación { get; set; }
        /// <summary>
        /// Obtiene o establece quien es el responsable de las llantas
        /// </summary>
        int? IncluyeLlantas { get; set; }
        /// <summary>
        /// Obtiene o establece la dirección del almacenaje de las unidades
        /// </summary>
        string DireccionAlmacenaje { get; set; }
        /// <summary>
        /// Obtiene o establece las observaciones realizadas al contrato
        /// </summary>
        string Observaciones { get; set; }
        /// <summary>
        /// Obtiene o establece los representantes legales del contrato
        /// </summary>
        List<object> RepresentatesLegales { get; set; }
        /// <summary>
        /// Obtiene o establece los obligados solidarios del contrato
        /// </summary>
        List<object> ObligadosSolidarios { get; set; }
        /// <summary>
        /// Obtiene o establece los avales del contrato
        /// </summary>
        List<object> Avales { get; set; }
        /// <summary>
        /// Obtiene o establece las lineas del contrato
        /// </summary>
        List<object> LineasContrato { get; set; } 
        /// <summary>
        /// Obtiene o establece la lista de datos adicionales del contrato
        /// </summary>
        List<object> DatosAdicionales { get; set; }
        /// <summary>
        /// Obtiene o establece el contrato que esta siendo editado
        /// </summary>
        object UltimoObjeto { get; set; }
        /// <summary>
        /// Obtiene el usuario que crea el contrato
        /// </summary>
        int? UC { get;}
        /// <summary>
        /// Obtiene  la fecha en que se registra el contrato
        /// </summary>
        DateTime? FC { get; }
        #endregion

        #region Métodos
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
        /// Habilita o deshabilita los controles del modo borrador
        /// </summary>
        /// <param name="status">Estado del control</param>
        void HabilitarModoBorrador(bool status);
        /// <summary>
        /// Habilita o deshabilita los controles del modo terminar
        /// </summary>
        /// <param name="status">Estado del control</param>
        void HabilitarModoTerminado(bool status);
        /// <summary>
        /// Redirige a la página de detalle de contrato
        /// </summary>
        void RedirigirADetalles();
        /// <summary>
        /// Redirige a la página de consulta de contrato
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Establece el estatus para los controles de registro
        /// </summary>
        /// <param name="status">Estatus que será aplicado al control</param>
        void PermitirRegistrar(bool status);
        /// <summary>
        /// Despliega un mensaje en la vista
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo del mensaje que es desplegado</param>
        /// <param name="msjDetalle">Detalle del mensaje desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        //Obtiene las plantillas correspondientes al tipo de contrato
        List<object> ObtenerPlantillas(string key);

        #region RI0008
        /// <summary>
        /// Permite Guardar el Contrato como Terminado (En Curso)
        /// </summary>
        /// <param name="b">Indica si se permitira Guardar el Contrato En curso</param>
        void PermitirGuardarTerminado(bool b);
        #endregion

        #endregion
    }
}