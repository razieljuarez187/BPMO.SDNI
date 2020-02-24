//Satisface al CU027 - Registrar Contrato de Mantenimiento
using System;
using System.Collections.Generic;

using BPMO.Primitivos.Enumeradores;

using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.Mantto.BO;

namespace BPMO.SDNI.Contratos.Mantto.VIS
{
    public interface IucContratoManttoVIS
    {
        #region Propiedades
        int? UsuarioID { get; }
        int? UnidadOperativaID { get; }
        bool ModoEdicion { get; set; }

        int? ContratoID { get; set; }
        string NumeroContrato { get; set; }
        int? TipoContratoID { get; set; }
        int? EstatusID { get; set; }

        string NombreEmpresa { get; set; }
        string DomicilioEmpresa { get; set; }
        string RepresentanteEmpresa { get; set; }

        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }

        DateTime? FechaContrato { get; set; }
        DateTime? FechaInicioContrato { get; set; }
        DateTime? FechaTerminacionContrato { get; set; }
        int? Plazo { get; set; }

        string CodigoMoneda { get; set; }

        int? ClienteID { get; set; }
        /// <summary>
        /// Numero de Cuenta de Oracle CuentaClienteBO.Numero
        /// </summary>
        String ClienteNumeroCuenta { get; set; }
        bool? ClienteEsFisica { get; set; }
        int? CuentaClienteID { get; set; }
        string CuentaClienteNombre { get; set; }
        int? CuentaClienteTipoID { get; set; }
        string ClienteDireccionCompleta { get; set; }
        string ClienteDireccionCalle { get; set; }
        string ClienteDireccionCodigoPostal { get; set; }
        string ClienteDireccionCiudad { get; set; }
        string ClienteDireccionEstado { get; set; }
        string ClienteDireccionMunicipio { get; set; }
        string ClienteDireccionPais { get; set; }
        string ClienteDireccionColonia { get; set; }

        List<RepresentanteLegalBO> RepresentantesTotales { get; set; }
        List<RepresentanteLegalBO> RepresentantesSeleccionados { get; set; }
        int? RepresentanteLegalSeleccionadoID { get; }
        bool? SoloRepresentantes { get; set; }

        List<ObligadoSolidarioBO> ObligadosSolidariosTotales { get; set; }
        List<ObligadoSolidarioBO> ObligadosSolidariosSeleccionados { get; set; }
        int? ObligadoSolidarioSeleccionadoID { get; }
        List<RepresentanteLegalBO> RepresentantesObligadoTotales { get; set; }
        List<RepresentanteLegalBO> RepresentantesObligadoSeleccionados { get; set; }
        int? RepresentanteObligadoSeleccionadoID { get; }
        bool? ObligadosComoAvales { get; set; }

        List<AvalBO> AvalesTotales { get; set; }
        List<AvalBO> AvalesSeleccionados { get; set; }
        int? AvalSeleccionadoID { get; }
        List<RepresentanteLegalBO> RepresentantesAvalTotales { get; set; }
        List<RepresentanteLegalBO> RepresentantesAvalSeleccionados { get; set; }
        int? RepresentanteAvalSeleccionadoID { get; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string NumeroSerie { get; set; }
        List<LineaContratoManttoBO> LineasContrato { get; set; }
        int? LineaContratoSeleccionadaIndex { get; } //El set debe ser privado

        string UbicacionTaller { get; set; }
        decimal? DepositoGarantia { get; set; }
        decimal? ComisionApertura { get; set; }
        int? IncluyeSeguroID { get; set; }
        int? IncluyeLavadoID { get; set; }
        int? IncluyePinturaRotulacionID { get; set; }
        int? IncluyeLlantasID { get; set; }
        string DireccionAlmacenaje { get; set; }

        string DatoAdicionalTitulo { get; set; }
        string DatoAdicionalDescripcion { get; set; }
        bool? DatoAdicionalEsObservacion { get; set; }
        List<DatoAdicionalAnexoBO> DatosAdicionales { get; set; }

        string Observaciones { get; set; }

        //Datos del cierre del contrato
        DateTime? FechaFinalizacion { get; set; }
        int? UsuarioFinalizacionID { get; set; }
        string ObservacionesFinalizacion { get; set; }
        string MotivoFinalizacion { get; set; }
        //Identificador de Dirección de Cliente
        int? DireccionClienteID { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararVisualizacion();

        void PermitirSeleccionarDireccionCliente(bool permitir);
        void PermitirSeleccionarUnidad(bool permitir);
        void PermitirSeleccionarRepresentantes(bool permitir);
        void PermitirAgregarRepresentantes(bool permitir);
        void PermitirSeleccionarObligadosSolidarios(bool permitir);
        void PermitirAgregarObligadosSolidarios(bool permitir);
        void PermitirSeleccionarAvales(bool permitir);
        void PermitirAgregarAvales(bool permitir);
        void PermitirAsignarTarifasEA(bool permitir);//SC_0051

        void MostrarPersonasCliente(bool mostrar);
        void MostrarObligadosSolidarios(bool mostrar);
        void MostrarAvales(bool mostrar);
        void MostrarRepresentantesObligado(bool mostrar);
        void MostrarRepresentantesAval(bool mostrar);
        void MostrarNumeroEconomico(bool mostrar);
        void MostrarLineaContrato(bool mostrar);
        void MostrarDetalleRepresentantesObligado(List<RepresentanteLegalBO> representantes);
        void MostrarDetalleRepresentantesAval(List<RepresentanteLegalBO> representantes);
        void MostrarDatosAdicionales(bool mostrar);

        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);
        void EstablecerOpcionesIncluyeSeguro(Dictionary<int, string> tiposInclucion);
        void EstablecerOpcionesIncluyeLavado(Dictionary<int, string> tiposInclucion);
        void EstablecerOpcionesIncluyePinturaRotulacion(Dictionary<int, string> tiposInclucion);
        void EstablecerOpcionesIncluyeLlantas(Dictionary<int, string> tiposInclucion);

        void ActualizarRepresentantesLegales();
        void ActualizarObligadosSolidarios();
        void ActualizarAvales();
        void ActualizarLineasContrato();
        void ActualizarDatosAdicionales();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);
        #endregion
    }
}
