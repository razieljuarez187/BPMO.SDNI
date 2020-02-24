//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IEditarCuentaClienteVIS
    {
        #region Propiedades
        CuentaClienteIdealeaseBO Cliente { get; set; }
        CuentaClienteIdealeaseBO ClienteAnterior { get; set; }
        List<RepresentanteLegalBO> Representantes { get; set; }
        List<ObligadoSolidarioBO> Obligados { get; set; }
        List<RepresentanteLegalBO> RepresentantesInactivos { get; set; }
        List<ObligadoSolidarioBO> ObligadosInactivos { get; set; }
        List<TelefonoClienteBO> ListaTelefonos { get; set; }
        /// <summary>
        /// Lista de acciones para establecer permisos de acceso segun el tipo de empresa.
        /// </summary>
        List<CatalogoBaseBO> ListaAcciones { get; set; }
        int? UC { get;}
        int? UUA { get;}
        DateTime? FC { get;}
        DateTime? FUA { get;}
        string Nombre { get; set; }
        bool? Fisica { get; set; }
        string RFC { get; set; }
        int? ClienteID { get; set; }        
        DateTime? FechaRegistro { get; set; }
        string GiroEmpresa { get; set; }
        string CURP { get; set; }
        string Correo { get; set; }
        /// <summary>
        /// Atributo para establecer el sector perteneciente a las empresas de tipo Construcción o Generación.
        /// </summary>
        Enum SectorCliente { get; set; }
        /// <summary>
        /// Atributo que almacena las observaciones generales del registro de algún cliente.
        /// </summary>
        string Observaciones { get; set; }
        ActaConstitutivaBO ActaConstitutivaSeleccionada { get; set; }
        List<ActaConstitutivaBO> ActasConstitutivas { get; set; }
        string ValidarActaConstitutiva();		
        /// <summary>
        /// Número de Cuenta de objeto CuentaClienteBO
        /// </summary>
        string NumeroCuentaOracle { get; set; }
        ETipoCuenta? TipoCuenta { get; set; }
        #region SC0008
        UnidadOperativaBO UnidadOperativa { get; }
        #endregion
        #region SC0001

        int? DiasUsoUnidad { get; set; }

        int? HorasUsoUnidad { get; set; }

        #endregion
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        void MostrarTipoCuenta();
        void HabilitarCampos();
        void DeshabilitarCampos();

        void OcultarActaConstitutiva();
        void OcultarHacienda();

        void MostrarActaConstitutiva();
        void MostrarHacienda();
        void MostrarTelefonos();

        void ActualizarRepresentantesLegales();
        void ActualizarObligadosSolidarios();

        object ObtenerDatos();
        void EstablecerPaquete(object bo);

        void ModoPresentarInformacion();
        void ModoEdicionRepresentante();
        void ModoAgregarRepresentante();
        void ModoEdicionObligado();
        void ModoAgregarObligado();

        void RedirigirADetalle();
        void RedirigirAConsulta();

        void EstablecerAcciones();

		#region SC0005
		void MostrarEdicion();
		void MostrarRepresentanteObligado();
		void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes);
		#endregion       
        #region SC0008
        void RedirigirSinPermisoAcceso();
        void PermitirRegistrar(bool permitir);
        #endregion
        #endregion
    }
}
