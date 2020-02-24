using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Contratos.RD.VIS
{
    /// <summary>
    /// Vista para cambiar la Frecuencia de Contratos de RD
    /// </summary>
    public interface ICambiarFrecuenciaContratosRDVIS
    {
        #region Propiedades
        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        int? ContratoId { get; set; }
        /// <summary>
        /// Numero del Contrato
        /// </summary>
        string NumeroContrato { get; set; }
        /// <summary>
        /// CuentaClienteID del Cliente
        /// </summary>
        int? ClienteId { get; set; }
        /// <summary>
        /// Nombre del Cliente
        /// </summary>
        string ClienteNombre { get; set; }
        /// <summary>
        /// Id de la Sucursal
        /// </summary>
        int? SucursalId { get; set; }
        /// <summary>
        /// Nombre de la Sucursal
        /// </summary>
        string SucursalNombre { get; set; }
        /// <summary>
        /// Fecha del Contrato
        /// </summary>
        DateTime? FechaContrato { set; }
        /// <summary>
        /// Fecha de Promesa de Devolucion de la Unidad
        /// </summary>
        DateTime? FechaPromesaDevolucion { set; }
        /// <summary>
        /// Dias Restantes para la Devolucion
        /// </summary>
        int? DiasRestantes { get; set; }
        /// <summary>
        /// Lista de Pagos Enviados a Facturar
        /// </summary>
        List<PagoUnidadContratoRDBO> ListaPagosFacturados { get; set; }
        /// <summary>
        /// Frecuencia Actual del Contrato
        /// </summary>
        EFrecuencia? FrecuenciaActual { get; set; }
        /// <summary>
        /// Fecha que será asignada al Contrato
        /// </summary>
        EFrecuencia? FrecuenciaNueva { get; set; }
        /// <summary>
        /// Lista de Pagos Pendientes por Facturar
        /// </summary>
        List<PagoUnidadContratoRDBO> ListaPagosFaltantes { get; set; }
        /// <summary>
        /// Objeto que sera usado para modificar
        /// </summary>
        ContratoRDBO ContratoOriginal { get; set; }
        /// <summary>
        /// Objeto que conserva los Datos Originales del Contrato
        /// </summary>
        ContratoRDBO ContratoAntiguo { get; set; }
        /// <summary>
        /// Cantidad de Pagos que se necesitan generar
        /// </summary>
        int? CantidadPagosFaltantes { get; set; }
        /// <summary>
        /// Id del Usuario Logueado
        /// </summary>
        int? UsuarioId { get; }
        /// <summary>
        /// Unidad Operativa del Usuario Logueado
        /// </summary>
        UnidadOperativaBO UnidadOperativa { get; }
        #endregion
        #region Metodos
        /// <summary>
        /// Limpia la sesion de la página
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Coloca como inactivos los datos del contrato
        /// </summary>
        void InactivarCamposIniciales();
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje que es desplegado</param>
        /// <param name="tipo">Tipo del mensaje que es desplegao</param>
        /// <param name="detalle">Detalle del mensaje que es desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Obtiene el Paquete con el Contrato a Cambiar de Frecuencia
        /// </summary>
        /// <param name="key">Nombre con el que se identifica el objeto</param>
        /// <returns>Objeto Contrato que cambiara de Frecuencia</returns>
        object ObtenerPaqueteNavegacion(string key);
        /// <summary>
        /// Determina si se habilita la opcion de Guardar
        /// </summary>
        /// <param name="permitir">True/False para Guardar</param>
        void PermitirGuardar(bool permitir);
        /// <summary>
        /// Determina si se habilita la opcion de Cancelar
        /// </summary>
        /// <param name="permitir">True/False para Cancelar</param>
        void PermitirCancelar(bool permitir);
        /// <summary>
        /// Activa o desactiva la seleccion de frecuencia de Facturacion
        /// </summary>
        /// <param name="permitir">Determina se se puede o no selecciona la Frecuencia de Facturacion</param>
        void PermitirFrecuencia(bool permitir);
        /// <summary>
        /// Redirige la seccion de Consulta de Contratos
        /// </summary>
        void RedirigirAConsulta();
        /// <summary>
        /// Redirige a la página de acceso denegado
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Presenta solo las Frecuencias de Facturacion Disponibles para Escoger
        /// </summary>
        /// <param name="frecuenciaFacturacion">Diccionario con las opciones de Frecuencia Disponibles</param>
        void EstablecerFrecuenciasFacturacion(Dictionary<string, string> frecuenciaFacturacion);
        /// <summary>
        /// Coloca la informacion inicial del contrato obtenido del paquete de Navegacion
        /// </summary>
        /// <param name="contratoBo">Contrato con las informacion inicial</param>
        void EstablecerInformacionInicial(ContratoRDBO contratoBo);
        /// <summary>
        /// Presenta en la Interfaz los pagos Enviados a Facturar
        /// </summary>
        /// <param name="pagosFacturados">Lista de Pagos Enviados a Facturar</param>
        void EstablecerPagosFacturados(List<PagoUnidadContratoRDBO> pagosFacturados);
        /// <summary>
        /// Presenta en la Interfaz los pagos pendientes por enviar a Facturar
        /// </summary>
        /// <param name="pagosPendientes">Lista de Pagos que falta por enviar a Facturar</param>
        void EstablecerPagosPendientes(List<PagoUnidadContratoRDBO> pagosPendientes);
        #endregion
    }
}
