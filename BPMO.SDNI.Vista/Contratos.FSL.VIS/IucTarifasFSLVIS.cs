// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al CU022 - Consultar Contratos Full Service Leasing
// Satisface al CU025 - Catalogo Tarifas Comerciales Renta FSL
// Satisface al CU023 - Editar Contrato Full Service Leasing 
// Mejoras Durante Staffing - Cobro de Rangos de Kms /Hrs

using System;
using System.Collections.Generic;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
	public interface IucTarifasFSLVIS
    {
        #region Propiedades
        int? Plazo { get; set; }

		ETipoCotizacion? TipoCotizacion { get; set; }
        List<TarifaFSLBO> Tarifas { get; set; }
        List<TarifaFSLBO> UltimasTarifas { get; set; }
        int? Identificador { get; set; }
        Boolean? TarifaUnidad { get; set; }
        Boolean? CargoKm { get; set; }
        Boolean? EsModoConsulta { get; }
        Boolean? SinTarifas { get; set; }
        ITarifasEquipoAliadoVIS VistaMensaje { get; set; }
        #endregion

        #region Metodos
        void LimpiarSesion();
        List<TarifaFSLBO> ObtenerTarifas();
        List<TarifaFSLBO> ObtenerUltimasTarifas();
		void InicializarControl(int? plazo, ETipoCotizacion? tipoCotizacion, bool? tarifaUnidad);
        void InicializarControl(List<TarifaFSLBO> tarifas, int? identificador);
	    void EstablecerTitulo(string titulo);
        /// <summary>
        /// Presenta u oculta la seccion con el Tipo de Cargo KM/HR
        /// </summary>
        /// <param name="mostar">Bool que determina si se mostrara o no la seccion</param>
	    void MostarTipoCargo(bool mostar);
        /// <summary>
        /// Diccionario de Tipos de cargo que se presentarán
        /// </summary>
        /// <param name="listaTipoCargo">Contiene los tipo de cargo que se mostraran</param>
	    void EstablecerTipoCargo(Dictionary<String, String> listaTipoCargo);

	    void MostrarModoConsulta(bool mostrar);

	    void EstablecerAnio(Dictionary<String, String> listaAnios);
	    void EstablecerFrecuencias(Dictionary<String, String> listaFrecuencias);
	    void PresentarTarifas(List<TarifaFSLBO> tarifas);
	    void PresentarInformacionRangos(List<RangoTarifaFSLBO> rangoTarifas);
	    void PermitirAnio(bool permitir);
	    void PermitirFrecuencia(bool permitir);
	    void PermitirKmsHrsLibres(bool permitir);
        void PermitirKmHrMinima(bool permitir);
	    void PermitirGuardarAnio(bool permitir);
	    void PermitirAgregarRangos(bool permitir);

	    #endregion
    }
}
