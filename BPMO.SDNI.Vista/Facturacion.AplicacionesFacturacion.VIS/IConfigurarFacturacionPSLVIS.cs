using System;
using BPMO.eFacturacion.Procesos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS {
    /// <summary>
    /// Interface que define el comportamiento que debe de tener una vista para la configuración de una 
    /// prefactura
    /// </summary>
    public interface IConfigurarFacturacionPSLVIS {
        /// <summary>
        /// Obtiene la forma de pago por default
        /// </summary>
        String FormaPagoPorDefault { get; }

        /// <summary>
        /// Obtiene el tipo de tasaCambiariaPorDefault
        /// </summary>
        String TipoTasaCambiarioPorDefault { get; }

        /// <summary>
        /// Obtiene la unidad de medida por default
        /// </summary>
        int? UnidadMedidaIDPorDefault { get; }

        /// <summary>
        /// Obtiene la forma de pago por default
        /// </summary>
        String UnidadMedidaPorDefault { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del usuario actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UsuarioID { get; }

        /// <summary>
        /// Obtiene un valor que representa el nombre del usuario de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String UsuarioNombre { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador de la unidad operativa actual de la sesión en curso
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Codigo de Moneda de la Empresa
        /// </summary>
        String CodigoMonedaEmpresa { get; }

        /// <summary>
        /// Obtiene un valor que representa el identificador del pago que se esta solicitando a facturar
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? PagoContratoID { get; }

        /// <summary>
        /// Obtiene el Tipo del Pago FSL, CM, RD
        /// </summary>
        ETipoContrato? TipoPagoContrato { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String NombreCliente { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el sistema origen
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String SistemaOrigen { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha de inicio de contrato del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la cuenta del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String NumeroCliente { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el RFC del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String RFCCliente { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la direccion del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String DireccionCliente { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el usuario generador del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String UsuarioGenerador { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el pago en curso del cual se estar generando la prefactura
        /// </summary>
        /// <value>Objeto de tipo PagoUnidadContratoBO</value>
        PagoContratoPSLBO PagoActual { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la transaccion actual
        /// </summary>
        /// <value>Objeto de tipo TransaccionBO</value>
        TransaccionBO TransaccionActual { get; set; }

        /// <summary>
        /// Obtiene un valor que representa la vista de costos adicionales asociada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucCostosAdicionalesVIS</value>
        IucCostosAdicionalesFacturaContratoVIS CostosAdicionalesView { get; }

        /// <summary>
        /// Obtiene un valor que representa la vista de herramientas adicionales asociada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucHerramientasPrefacturaVIS</value>
        IucHerramientasPrefacturaVIS HerramientasPrefacturaView { get; }

        /// <summary>
        /// Obtiene un valor que representa la vista de información de cabecera asociada a la prefactura.
        /// </summary>
        /// <value>Objeto de tipo IucInformacionCabeceraVIS</value>
        IucInformacionGeneralPSLVIS InformacionCabeceraView { get; }

        /// <summary>
        /// Obtiene un valor que representa la vista de líneas de factura asociada a la prefactura
        /// </summary>
        /// <value>Objeto de tipo IucLineasFacturaVIS</value>
        IucLineasFacturaContratoPSLVIS LineasFacturaView { get; }

        /// <summary>
        /// Obtiene un valor que representa la página actual visualizada
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? PaginaActual { get; }

        ContratoPSLBO Contrato { get; set; }

        /// <summary>
        /// Información en sesión del modelo que se utiliza en la carga del grid de pagos
        /// </summary>
        object InformacionLineasFacturaModel { get; set; }

        /// <summary>
        /// Identificador del id del pago en el grid
        /// </summary>
        int? PagoUnidadContratoID { get; set; }

        /// <summary>
        /// Indica si la solicitud de pago fue enviada a factura
        /// </summary>
        string EnvioFactura { get; set; }

        /// <summary>
        /// Obtiene el identificador del UsoCFDI
        /// </summary>
        int? UsoCFDIID { get; }
        
        /// <summary>
        /// Obtiene la clave del UsoCFDI
        /// </summary>
        string ClaveUsoCFDI { get; }

        /// <summary>
        /// Obtiene la descripción del UsoCFDI
        /// </summary>
        string DescripcionUsoCFDI { get; }

        /// <summary>
        /// Establece un paquete de navegación en el visor dentro de la sesión en curso
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <param name="value">Valor a asignar dentro del paquete de navegación</param>
        void EstablecerPaqueteNavegacion(string key, object value);

        /// <summary>
        /// Obtiene el valor de un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        /// <returns>Valor de tipo objet dentro del paquete de navegación</returns>
        object ObtenerPaqueteNavegacion(string key);

        /// <summary>
        /// Elimina un paquete de navegación ya guardado previamente
        /// </summary>
        /// <param name="key">Índice o identificador del paquete</param>
        void LimpiarPaqueteNavegacion(string key);

        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();

        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        /// <summary>
        /// Obtiene la carpeta raiz donde se encuentra la aplicación
        /// </summary>
        /// <returns>Dirección donde se encuentra la aplicación</returns>
        String ObtenerCarpetaRaiz();

        /// <summary>
        /// Establece si se permite seleccionar la opción de Regresar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para permitir, false para denegar</param>
        void PermitirRegresar(bool permitir);

        /// <summary>
        /// Establece si se permite seleccionar la opción de Continuar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para permitir, false para denegar</param>
        void PermitirContinuar(bool permitir);

        /// <summary>
        /// Establece si se permite seleccionar la opción de Cancelar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para , false para denegar</param>
        void PermitirCancelar(bool habilitar);

        /// <summary>
        /// Establece si se permite seleccionar la opción de Terminar
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para , false para denegar</param>
        void PermitirTerminar(bool habilitar);

        /// <summary>
        /// Establece si se permite ocultar la opción de Continuar
        /// </summary>
        /// <param name="ocultar">Valor booleano para establecer el comportamiento, true para ocultar, false para visualizar</param>
        void OcultarContinuar(bool ocultar);

        /// <summary>
        /// Establece si se permite ocultar la opción de Anterior
        /// </summary>
        /// <param name="ocultar">Valor booleano para establecer el comportamiento, true para ocultar, false para visualizar</param>
        void OcultarAnterior(bool ocultar);

        /// <summary>
        /// Establece si se permite ocultar la opción de Cancelar
        /// </summary>
        /// <param name="ocultar">Valor booleano para establecer el comportamiento, true para ocultar, false para visualizar</param>
        void OcultarCancelar(bool ocultar);

        /// <summary>
        /// Establece si se permite ocultar la opción de Terminar
        /// </summary>
        /// <param name="ocultar">Valor booleano para establecer el comportamiento, true para ocultar, false para visualizar</param>
        void OcultarTerminar(bool ocultar);

        /// <summary>
        /// Establece si se permirte capturar datos dentro de una factura
        /// </summary>
        /// <param name="habilitar">Valor booleano para establecer el comportamiento, true para , false para denegar</param>
        void PertimirCapturar(bool habilitar);

        /// <summary>
        /// Establece la página a ser visualizada
        /// </summary>
        /// <param name="numeroPagina">Número de pagina a ser visualizada, comenzando por el número 1</param>
        void EstablecerPagina(int pagina);

        /// <summary>
        /// Proceso que prepara la visualización de una nueva factura
        /// </summary>
        void PrepararNuevo();

        /// <summary>
        /// Proceso que se realiza para redirigir a la ventana correspondiente para solicitar una nueva consulta
        /// </summary>
        void RedirigirAConsulta();

        /// <summary>
        /// Proceso que se realiza para redirigir a la ventana correspondiente durante una cancelación
        /// </summary>
        void RedirigirACancelacion();

        /// <summary>
        /// Cambia la vista a captura de información de la factura
        /// </summary>
        void CambiarCapturarFactura();

        /// <summary>
        /// Cambia la la Vista de la interfaz a la información de captura de cargos adicionales
        /// </summary>
        void CambiarCapturarCargo(int PagoUnidadContratoID);

    }
}