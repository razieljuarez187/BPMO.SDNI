﻿//Esta clase satisface los requerimientos especificados en el caso de uso CU082 – REGISTRAR MOVIMIENTO DE FLOTA
//Satisface la solicitud de cambio SC0006

using System;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using System.Collections.Generic;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista para el registro de baja de unidades
    /// </summary>
    public interface IRegistrarBajaUnidadVIS
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

        string UsuarioNombre { get;}

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

        #region Siniestro
        /// <summary>
        /// Obtiene o establece un valor que indica si es una baja por siniestro
        /// </summary>
        bool Siniestro { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de siniestro
        /// </summary>
        DateTime? FechaSiniestro { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de dictamen
        /// </summary>
        DateTime? FechaDictamen { get; set; }

        /// <summary>
        /// Obtiene o establece las observaciones del siniestro
        /// </summary>
        String ObservacionesSiniestro { get; set; }
        #endregion

        /// <summary>
        /// Obtiene o establece las observaciones de la reasignación
        /// </summary>
        string Observaciones { get; set; }
        /// <summary>
        /// Obtiene o establece el objeto que se esta editando
        /// </summary>
        object ObjetoEdicion { get; set; }
        /// <summary>
        /// Obtiene o establece la inforamción de la unidad que se esta editando
        /// </summary>
        object UltimoObjeto { get; set; }

        /// <summary>
        /// Obtiene o establece el centro de costos de la unidad
        /// </summary>
        string CentroCostos { get; set; }

        /// <summary>
        /// Lista de archivos adjuntos al contrato
        /// </summary>
        List<ArchivoBO> Adjuntos { get; set; }
        /// <summary>
        /// Lista de tipo de archivos
        /// </summary>
        List<TipoArchivoBO> TiposArchivo { get; set; }
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
        /// Establece un paquete de navegación para su posterior consulta
        /// </summary>
        /// <param name="key">Clave del paquete</param>
        /// <param name="value">Paquete que se desea guardar</param>
        void EstablecerPaqueteNavegacionReporte(string key, object value);
        /// <summary>
        /// Obtiene un paquete de navegación para usar en la página
        /// </summary>
        /// <param name="key">Clave del paquete que se desea obtener</param>
        /// <returns>Paquete solicitado</returns>
        object ObtenerPaqueteNavegacion(string key);
        /// <summary>
        /// Redirige a la página de detalle de unidad
        /// </summary>
        void RedirigirADetalles(bool lBaja);
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
        /// Establece el estatus para activar o desactivar la sección de servicios
        /// </summary>
        /// <param name="status">Estatus que será aplicado al control</param>
        void ActivarSeccionSiniestro(bool status);

        /// <summary>
        /// Establece el estatus para visualizar u ocultar la sección de servicios
        /// </summary>
        /// <param name="status">Estatus que será aplicado al control</param>
        void MostrarSeccionSiniestro(bool status);

        /// <summary>
        /// Despliega un mensaje en la vista
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo del mensaje que es desplegado</param>
        /// <param name="msjDetalle">Detalle del mensaje desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        /// <summary>
        /// Establece el estatus para visualizar u ocultar la sección de documentos adjuntos
        /// </summary>
        /// <param name="lVisible">Estatus que será aplicado al control</param>
        void MostrarSeccionDocumentos(bool lVisible);

        /// <summary>
        /// Establece el estatus para visualizar u ocultar la sección de centro de costos
        /// </summary>
        /// <param name="lVisible">Estatus que será aplicado al control</param>
        void MostrarSeccionCentroCostos(bool lVisible);
        #endregion
    }
}
