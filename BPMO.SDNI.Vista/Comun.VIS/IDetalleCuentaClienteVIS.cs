//Satiface al caso de uso CU068 - Catáloglo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IDetalleCuentaClienteVIS
    {
        #region Propiedades
        CuentaClienteIdealeaseBO Cliente { get; set; }
        CuentaClienteIdealeaseBO ClienteAnterior { get; set; }
        List<RepresentanteLegalBO> RepresentantesInactivos { get; set; }
        List<ObligadoSolidarioBO> ObligadosInactivos { get; set; }
        string Nombre { get; set; }
        bool? Fisica { get; set; }
        string RFC { get; set; }
        int? ClienteID { get; set; }
        string NumeroEscritura { get; set; }
        DateTime? FechaEscritura { get; set; }
        string NombreNotario { get; set; }
        string NumeroNotaria { get; set; }
        string LocalidadNotaria { get; set; }
        string NumeroFolio { get; set; }
        DateTime? FechaRPPC { get; set; }
        string LocalidadRPPC { get; set; }
        DateTime? FechaRegistro { get; set; }
        string GiroEmpresa { get; set; }
        string CURP { get; set; }
        int? UC { get; }
        int? UUA { get; }
        DateTime? FC { get; }
        DateTime? FUA { get; }
        /// <summary>
        /// Número de Cuenta de objeto CuentaClienteBO
        /// </summary>
        string NumeroCuentaOracle { get; set; }
        ETipoCuenta? TipoCuenta { get; set; }
        List<RepresentanteLegalBO> Representantes { get; set; }
        List<ObligadoSolidarioBO> Obligados { get; set; }
        List<TelefonoClienteBO> ListaTelefonos { get; set; }
        /// <summary>
        /// Lista de acciones para establecer permisos de acceso segun el tipo de empresa.
        /// </summary>
        List<CatalogoBaseBO> ListaAcciones { get; set; }
        #region SC_0008
        int? UnidadOperativaId { get; }
        int? UsuarioId { get; }
        #endregion
        #region SC0001
        string Correo { get; set; }
        int? HorasUsoUnidad { get; set; }
        int? DiasUsoUnidad { get; set; }
        #endregion

        /// <summary>
        /// Atributo que almacena las observaciones generales del registro de algún cliente.
        /// </summary>
        string Observaciones { get; set; }

        /// <summary>
        /// Enumerador que contiene el sector al que pertenece el cliente
        /// </summary>
        Enum SectorCliente { get; set; }
        #endregion

        #region Métodos
        void MostrarDatos();
        void OcultarActaConstitutiva();
        void OcultarHacienda();
        void MostrarActaConstitutiva();
        void MostrarHacienda();
        void LimpiarSesion();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        object ObtenerDatos();
        void DeshabilitarCampos();
        void EstablecerDatosNavegacion(string nombre, object bo);
        void RedirigirAEdicion();
	    void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes);
        /// <summary>
        /// Muestra en la Interfaz la lista de Creditos Disponibles del Cliente
        /// </summary>
        /// <param name="creditos">Lista de Creditos del cliente</param>
        void MostrarCreditoCliente(List<CreditoClienteBO> creditos);
        /// <summary>
        /// Muestra en la interfaz el panel de Observaciones del cliente.
        /// </summary>
        void MostrarObservaciones();

        void EstablecerPaquete(object bo);

        /// <summary>
        /// Método para establecer mediante la lista de acciones los permisos que se obtienen.
        /// </summary>
        void EstablecerAcciones();
        #region SC0008
        void PermitirEditar(bool permitir);
        void PermitirRegistrar(bool permitir);
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion
    }
}
