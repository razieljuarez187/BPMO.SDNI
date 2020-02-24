// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucDatosRentaVIS
    {
        #region Propiedades
        // Lineas del Contrato
        int? UnidadID { get; set; }
        int? EquipoID { get; set; }
        string NumeroSerie { get; set; }
        List<LineaContratoFSLBO> LineasContrato { get; set; }

        // Informacion Contrato
        int PlazoAnios { get; set;  }
        int? PlazoMeses { get; set; }
        string UbicacionTaller { get; set; }
        ETipoInclusion? IncluyeSeguroSeleccionado { get; }
        /// <summary>
        /// Frecuencia de cobro del Seguro
        /// </summary>
        EFrecuenciaSeguro? FrecuenciaSeguro { get; }
        /// <summary>
        /// Porcentaje Adicional que se le cobra al seguro
        /// </summary>
        int? PorcentajeSeguro { get; set; }
        ETipoInclusion? IncluyeLlantasSeleccionado { get; }
        ETipoInclusion? IncluyeLavadoSeleccionado { get; }
        ETipoInclusion? IncluyePinturaSeleccionado { get; }
        #endregion

        #region Metodos

        void HabilitarAgregarUnidad(bool habilitar);
        void ConfigurarModoConsultar();
        void ConfigurarModoEditar();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void CargarListadoIncluyeSeguro(List<ETipoInclusion> listado);
        void CargarListadoIncluyeLlantas(List<ETipoInclusion> listado);
        void CargarListadoIncluyeLavado(List<ETipoInclusion> listado);
        void CargarListadoIncluyePintura(List<ETipoInclusion> listado);
        void EstablecerIncluyeSeguroSeleccionado(ETipoInclusion? tipo);
        void EstablecerIncluyeLlantasSeleccionado(ETipoInclusion? tipo);
        void EstablecerIncluyeLavadoSeleccionado(ETipoInclusion? tipo);
        void EstablecerIncluyePinturaSeleccionado(ETipoInclusion? tipo);
        /// <summary>
        /// Determina si se puede seleccionar la frecuencia de cobro del seguro
        /// </summary>
        /// <param name="permitir">Bool que determina si se puede seleccionar</param>
        void PermitirFrecuenciaSeguro(bool permitir);
        /// <summary>
        /// Carga la lista de valores de Frecuencias de Cobro del Seguro
        /// </summary>
        /// <param name="listado">Lista con los valores</param>
        void CargarListadoFrecuenciaSeguro(List<EFrecuenciaSeguro> listado);
        /// <summary>
        /// Selecciona el valor configurado de la lista
        /// </summary>
        /// <param name="frecuenciaSeguro">Valor configurado</param>
        void EstablecerFrecuenciaSeguro(EFrecuenciaSeguro? frecuenciaSeguro);
        #endregion
    }
}
