//Satisface al caso de uso CU068 - Catálogo de Clientes
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
    public interface IRegistrarCuentaClienteVIS
    {
        #region Propiedades

        /// <summary>
        /// Lista de acciones para establecer permisos de acceso segun el tipo de empresa.
        /// </summary>
        List<CatalogoBaseBO> ListaAcciones { get; set; }

        ActaConstitutivaBO ActaConstitutivaSeleccionada { get; set; }

        List<ActaConstitutivaBO> ActasConstitutivas { get; set; }

        CuentaClienteIdealeaseBO Cliente { get; set; }

        string CURP { get; set; }

        /// <summary>
        /// Número de Cuenta de objeto CuentaClienteBO
        /// </summary>
        string NumeroCuentaOracle { get; set; }

        DateTime? FC { get; }

        DateTime? FechaRegistro { get; set; }

        bool? Fisica { get; set; }

        DateTime? FUA { get; }

        string GiroEmpresa { get; set; }

        string NombreCliente { get; set; }

        List<ObligadoSolidarioBO> ObligadosSolidarios { get; set; }

        List<RepresentanteLegalBO> RepresentantesLegales { get; set; }
        
        List<TelefonoClienteBO> ListaTelefonos { get; set; }

        string RFC { get; set; }

        ETipoCuenta? TipoCuenta { get; set; }

        #region SC0001

        int? DiasUsoUnidad { get; set; }

        int? HorasUsoUnidad { get; set; }

        string Correo { get; set; }

        /// <summary>
        /// Atributo para establecer el sector perteneciente a las empresas de tipo Construcción o Generación.
        /// </summary>
        Enum SectorCliente { get; set; }

        /// <summary>
        /// Atributo que almacena las observaciones generales del registro de algún cliente.
        /// </summary>
        string Observaciones { get; set; }
        #endregion
        
        int? UC { get; }

        UnidadOperativaBO UnidadOperativa { get; }

        int? UUA { get; }

        #endregion Propiedades

        #region Metodos

        void ActualizarObligadosSolidarios();

        void ActualizarRepresentantesLegales();

        void DeshabilitarCampos();

        void EstablecerDatosNavegacion(string nombre, object objeto);

        void HabilitarCampos();

        void LimpiarSesion();

        void MostrarActaConstitutiva();

        void MostrarDetalleObligado(List<RepresentanteLegalBO> representantes);

        void MostrarHacienda();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void MostrarTipoCuenta();

        void OcultarActaConstitutiva();

        void OcultarHacienda();

        void RedirigirADetalle();

        string ValidarActaConstitutiva(bool? validarrfc = true);

        /// <summary>
        /// Método para establecer mediante la lista de acciones los permisos que se obtienen.
        /// </summary>
        void EstablecerAcciones();

        /// <summary>
        /// Método que reinicia campos e inicializa la lista de teléfonos
        /// </summary>
        void ReiniciarCampos();

        #region SC0005

        void MostrarRegistro();

        void MostrarRepresentanteObligado();

        #endregion SC0005

        void MostrarObservaciones();

        #region SC0008
        void RedirigirSinPermisoAcceso();
        #endregion
        #endregion Metodos
    }
}