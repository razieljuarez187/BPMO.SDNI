//Construccion durante staffing - Eliminar unidades de un contrato en curso

using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IEditarLineasContratoFSLVIS
    {
        #region Propiedades
        /// <summary>
        /// Contrato Original que será Modificado
        /// </summary>
        ContratoFSLBO ContratoAnterior { get; set; }
        /// <summary>
        /// Identificador del usuario logueado
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Contiene las Observaciones por unidades o Equipos Aliados que salgan de los contratos, 
        /// EquipoID es el Key
        /// </summary>
        Dictionary<string, string> ObservacionesUnidad { get; set; }
        /// <summary>
        /// Sucursales a las que tiene permiso el usuario
        /// </summary>
        List<SucursalBO> ListaSucursalesPermitidas { get; set; }
        /// <summary>
        /// Lista de Unidades que se quitan del Contrato
        /// </summary>
        List<UnidadBO> UnidadesLiberar { get; set; }
        /// <summary>
        /// Lista de Unidades que tuvieron cambio en los equipos Aliados del contrato
        /// </summary>
        List<UnidadBO> UnidadesCambioEquipos { get; set; }
        #region Propiedades Vista General
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// Fecha en la que inicia el Contrato
        /// </summary>
        DateTime? FechaInicioContrato { get; set; }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        string NombreCliente { get; set; }
        /// <summary>
        /// Numero de Serie de la Unidad que se agregara al Contrato
        /// </summary>
        string NumeroSerie { get; set; }
        /// <summary>
        /// UnidadId de la Linea que se esta Modificando
        /// </summary>
        int? LineaUnidadId { get; set; }
        /// <summary>
        /// EquipoID de la Linea que se esta Modificando
        /// </summary>
        int? LineaEquipoId { get; set; }
        /// <summary>
        /// Lista de lineas de contrato
        /// </summary>
        List<LineaContratoFSLBO> LineasContrato { get; set; }
        #endregion
        #region Propiedades Vista Linea
        /// <summary>
        /// VIN de la unidad
        /// </summary>
        string VinUnidad { get; set; }
        /// <summary>
        /// Numero Economico de la Unidad de la Linea
        /// </summary>
        string NumeroEconomicoUnidad { get; set; }
        /// <summary>
        /// Nombre del Modelo de la Unidad
        /// </summary>
        string NombreModelo { get; set; }
        /// <summary>
        /// Año de la Unidad
        /// </summary>
        int? AnioUnidad { get; set; }
        /// <summary>
        /// Cantidad Maxima de de PBV
        /// </summary>
        decimal? PBVMaximoRecomendado { get; set; }
        /// <summary>
        /// Cantidad Maxima de PBC
        /// </summary>
        decimal? PBCMaximoRecomendado { get; set; }
        /// <summary>
        /// Kilometra Inicial de la Unidad
        /// </summary>
        int? KmInicial { get; set; }
        /// <summary>
        /// Poliza de Seguro Activa la Unidad
        /// </summary>
        string PolizaSeguro { get; set; }
        /// <summary>
        /// Kilometraje Estimado Anual
        /// </summary>
        int? KmEstimadoAnual { get; set; }
        /// <summary>
        /// Deposito en Garantia por la Unidad
        /// </summary>
        decimal? DepositoGarantia { get; set; }
        /// <summary>
        /// Comision por Apertura del Contrato
        /// </summary>
        decimal? ComisionApertura { get; set; }
        /// <summary>
        /// Cargo Fijo Mensual de la Unidad
        /// </summary>
        decimal? CargoFijoMensual { get; set; }
        /// <summary>
        /// Tipo de Cotizacion de la Linea
        /// </summary>
        ETipoCotizacion? TipoCotizacion { get; set; }
        /// <summary>
        /// Determina si la Unidad sera OC al finalizar el contrato
        /// </summary>
        bool? ConOpcionCompra { get; set; }
        /// <summary>
        /// Lista de Monedas Disponibles para la Compra
        /// </summary>
        List<MonedaBO> MonedasDisponibles { get; set; }
        /// <summary>
        /// Moneda en la cual se comprará la Unidad
        /// </summary>
        MonedaBO MonedaCompra { get; set; }
        /// <summary>
        /// Cantidad en la cual se comprará la Unidad
        /// </summary>
        decimal? ImporteCompra { get; set; }
        /// <summary>
        /// Linea de Contrato que se esta editando
        /// </summary>
        LineaContratoFSLBO LineaContratoEnEdicion { get; set; }
        /// <summary>
        /// Numero de Serie del Equipo aliado a Agregar
        /// </summary>
        string NumeroSerieEquipoAliado { get; set; }
        /// <summary>
        /// Identificador del producto Servicio
        /// </summary>
        int? ProductoServicioId { get; set; }
        /// <summary>
        /// Clave de Producto o Servicio
        /// </summary>
        string ClaveProductoServicio { get; set; }
        /// <summary>
        /// Descripción de Producto o Servicio
        /// </summary>
        string DescripcionProductoServicio { get; set; }
        #endregion
        #region Propiedades Tarifas
        /// <summary>
        /// Determina si el cobro es por Km o por Horas
        /// </summary>
        bool? CargoPorKm { get; set; }
        /// <summary>
        /// Año de la Tarifa a Configurar
        /// </summary>
        int? AnioTarifa { get; set; }
        /// <summary>
        /// Frecuencia de Facturacion de la Tarifa
        /// </summary>
        EFrecuencia? FrecuenciaTarifa { get; set; }
        /// <summary>
        /// Kilometros u horas libres de la Unidad
        /// </summary>
        int? KilometrosHorasLibres { get; set; }
        /// <summary>
        /// Cantidad Minima de Kilometros u Horas que se cobraran
        /// </summary>
        int? KmHrMinima { get; set; }
        /// <summary>
        /// Rango Inicial de la Tarifa
        /// </summary>
        int? RangoInicialTarifa { get; set; }
        /// <summary>
        /// Determina si es el ultimo Rango de la Tarifa Configurandose
        /// </summary>
        bool? UltimoRango { get; set; }
        /// <summary>
        /// Rango Final de la Tarifa
        /// </summary>
        int? RangoFinalTarifa { get; set; }
        /// <summary>
        /// Costo por Hora o Kilometro de la Tarifa
        /// </summary>
        decimal? CostoKmHr { get; set; }
        /// <summary>
        /// Tarifas que se esta configurando
        /// </summary>
        List<TarifaFSLBO>  TarifasEnConfiguracion { get; set; }
        /// <summary>
        /// Lista de Rangos que se estan configurando
        /// </summary>
        List<RangoTarifaFSLBO> RangosEnConfiguracion { get; set; }
        /// <summary>
        /// Tipo de Equipo al que se le configura la Tarifa
        /// </summary>
        ETipoEquipo? TipoEquipo { get; set; }
        /// <summary>
        /// Identificador de Unidad al que se le esta modificando la Tarifa
        /// </summary>
        int? UnidadIdTarifa { get; set; }
        /// <summary>
        /// Identificador de Equipo Aliado al que se le esta modificando la Tarifa
        /// </summary>
        int? EquipoAliadoIdTarifa { get; set; }

        bool? NoAplicaCargosAdicionales { get; set; }
        #endregion
        #endregion
        #region Metodos
        /// <summary>
        /// Evita el ingreso a la Interfaz sino cuenta con el permiso necesario
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Limpia los campos que se encuentran en la Vista con la información de las Lineas
        /// </summary>
        void LimpiarInterfazLineas();
        /// <summary>
        /// Limpia los datos que se encuentran en la Interfaz de Configuracion de Datos de Linea
        /// </summary>
        void LimpiarLineaUnidad();
        /// <summary>
        /// Limpia los datos que se encuentran en la Interfaz de Tarifas
        /// </summary>
        void LimpiarInterfazTarifas();
        /// <summary>
        /// Elimina los datos guardados por la interfaz en Session
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
        /// Obtiene el objeto enviado por la Interfaz Anterior
        /// </summary>
        /// <returns>Objeto obtenido del Paquete de Navegacion</returns>
        object ObtenerPaqueteNavegacion();
        /// <summary>
        /// Envia un objeto hacia otra interfaz
        /// </summary>
        /// <param name="paquete">Objeto que sera enviado</param>
        void EstablecerPaqueteNavegacion(Object paquete);
        /// <summary>
        /// Cambia la Interfaz que se esta presentando
        /// </summary>
        /// <param name="interfaz">Nombre usado para el cambio de interfaz</param>
        void CambiarInterfaz(string interfaz);
        #region Metodos para bloquear Campos
        /// <summary>
        /// Determina si esta bloqueado el boton para cancelar cambios
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirCancelarCambios(bool permitir);
        /// <summary>
        /// Determina si el boton para cancelar cambios
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirGuardarCambios(bool permitir);
        /// <summary>
        /// Determina si se puede visualizar el detalle de las lineas
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirVerDetalleLineas(bool permitir);
        /// <summary>
        /// Determina si el Numero de Contrato puede ser editable
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirContrato(bool permitir);
        /// <summary>
        /// Determina si la Fecha de Inicio del Contrato puede ser editable
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirFechaContrato(bool permitir);
        /// <summary>
        /// Determina si el Nombre del Cliente puede ser editable
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirNombreCliente(bool permitir);
        /// <summary>
        /// Determina si el Boton para Agregar Unidad esta Disponible
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirAgregarUnidad(bool permitir);
        /// <summary>
        /// Determina si se puede buscar el VIN de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirNumeroSerie(bool permitir);
        /// <summary>
        /// Determina si se puede editar el VIN de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirVinUnidad(bool permitir);
        /// <summary>
        /// Determina si se puede editar el Numero Economico de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirNumeroEconomico(bool permitir);
        /// <summary>
        /// Determina si se puede editar el Nombre del Modelo de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirNombreModelo(bool permitir);
        /// <summary>
        /// Determina si se puede editar el Año de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirAnio(bool permitir);
        /// <summary>
        /// Determina si se puede editar el PBC Maximo Recomendado de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirPBC(bool permitir);
        /// <summary>
        /// Determina si se puede editar el PBV Maximo Recomendado de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirPBV(bool permitir);
        /// <summary>
        /// Determina si se puede editar el Kilometraje Inicial de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirKmInicial(bool permitir);
        /// <summary>
        /// Determina si se puede editar la Poliza de Seguro de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirPolizaSeguro(bool permitir);
        /// <summary>
        /// Determina si se puede editar la opcion de compra de una unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirOpcionCompra(bool permitir);
        /// <summary>
        /// Determina si se puede editar la moneda de compra de la unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirMonedaCompra(bool permitir);
        /// <summary>
        /// Determina si se puede editar el importe de compra de una Unidad
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirImporteCompra(bool permitir);
        /// <summary>
        /// Determina si se puede editar Las Tarifas de las Unidades y Equipos Aliados
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirEditarTarifa(bool permitir);
        /// <summary>
        /// Determina si se puede agregar el equipo aliado a la Tabla
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirAgregarEquipoAliado(bool permitir);
        /// <summary>
        /// Determina si puede usar el check para no agregar tarifa adicional
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirSeleccionarSinTarifaAdiciona(bool permitir);
        /// <summary>
        /// Inactiva toda la Seccion de Tarifas
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirTarifasAdicionales(bool permitir);
        /// <summary>
        /// Inactiva o Activa la seleccion de KM u Horas
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirTipoCargo(bool permitir);
        /// <summary>
        /// Permite Activar o desactivar el Control de seleccion de Año de la Tarifa
        /// </summary>
        /// <param name="permitir">Bool que determina la edicion</param>
        void PermitirSeleccionarAnio(bool permitir);
        /// <summary>
        /// Permite habilitar o no los controles de Configuracion de la Tarifa
        /// </summary>
        /// <param name="permitir">Bool que determina si se habilitan los controles</param>
        void PermitirConfiguracionTarifa(bool permitir);
        #endregion
        /// <summary>
        /// Presenta las lineas de contrato
        /// </summary>
        /// <param name="lineasContrato">Lineas de Contrato con los datos de unidades y equipos aliados</param>
        void PresentarLineasContrato(List<LineaContratoFSLBO> lineasContrato);
        /// <summary>
        /// Presentar Tipos de Cotizaciones
        /// </summary>
        /// <param name="listaCotizaciones">Lista con las Cotizaciones</param>
        void PresentarTipoCotizacion(Dictionary<string, string> listaCotizaciones);
        /// <summary>
        /// Presenta las Monedas disponibles para la opcion de compra
        /// </summary>
        /// <param name="monedas">Lista de Monedas Disponibles</param>
        void PresentarMonedasDisponibles(Dictionary<String, String> monedas);
        /// <summary>
        /// Presenta los cargos por cada equipo aliado
        /// </summary>
        /// <param name="cargosEquiposAliados">Lista de Cargos de los equipos aliados</param>
        void PresentarCargosEquiposAliados(List<CargoAdicionalEquipoAliadoBO> cargosEquiposAliados);

        void PresentarRangosTarifa(List<RangoTarifaFSLBO> listaRangos);

        void PresentarListaAnios(Dictionary<string, string> aniosAConfigurar);

        void PresentarAniosConfigurados(List<TarifaFSLBO> listaTarifas);

        void PresentarFrecuencias(Dictionary<string, string> listaFrecuencias);

        #endregion
    }
}