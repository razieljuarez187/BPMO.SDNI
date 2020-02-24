// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al CU022 - Consultar Contrato Full Service Leasing
// Satisface al CU025 - Catalogo Tarifas Comerciales Renta FSL 
// Mejoras Durante Staffing - Cobro de Rangos de Kms /Hrs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.FSL.VIS;

namespace BPMO.SDNI.Contratos.FSL.PRE
{
	public class ucTarifasFSLPRE
    {
        #region Atributos
        private readonly IucTarifasFSLVIS vista;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor del presentador
        /// </summary>
        /// <param name="ucTarifasFslvis"></param>
        public ucTarifasFSLPRE(IucTarifasFSLVIS ucTarifasFslvis)
        {
            vista = ucTarifasFslvis;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Inicializa el Control
        /// </summary>
        /// <param name="plazo">Plazo del Contrato en Años</param>
        /// <param name="tipoCotizacion">Tipo de Cotizacion para las Tarifas</param>
        ///  <param name="tarifaUnidad">Indica si la Tarfia es para Unidades o Equipos Aliados</param>
		public void Inicializar(int? plazo, ETipoCotizacion? tipoCotizacion, bool? tarifaUnidad)
		{
			vista.Plazo = plazo;
			vista.TipoCotizacion = tipoCotizacion;
            vista.TarifaUnidad = tarifaUnidad;
		}
	    /// <summary>
	    /// Inicializa el Control
	    /// </summary>
	    /// <param name="tarifas">Lista de tarifas del contrato</param>
	    /// <param name="identificador">Identificador del Cargo Adicional</param>
	    public void Inicializar(List<TarifaFSLBO> tarifas, int? identificador) 
        {
            vista.Identificador = identificador;
            vista.UltimasTarifas = tarifas;  
        }

	    public void Inicializar(int? plazo, ETipoCotizacion? tipoCotizacion, ETipoEquipo? tipoEquipo, List<TarifaFSLBO> tarifas, int? identificador, bool? permitirModificar)
	    {
	        vista.MostarTipoCargo(tipoEquipo == ETipoEquipo.EquipoAliado);
	        vista.Identificador = identificador;
	        vista.TipoCotizacion = tipoCotizacion;
	        if (tarifas == null)
	            tarifas = new List<TarifaFSLBO>();
	        if (!vista.SinTarifas.Value)
	        {
                if(tipoCotizacion != null)
                {
                    if(tipoCotizacion == ETipoCotizacion.Average)
                    {
                        if(!tarifas.Any())
                            tarifas.Add(new TarifaFSLBO() { Rangos = new List<RangoTarifaFSLBO>() });
                    }
                    if(tipoCotizacion == ETipoCotizacion.Step)
                    {
                        if(!tarifas.Any())
                        {
                            for(var i = 0; i < plazo; i++)
                            {
                                var tarifa = new TarifaFSLBO { Año = i + 1, Rangos = new List<RangoTarifaFSLBO>() };
                                tarifas.Add(tarifa);
                            }
                        }
                    }
                }
                else
                    tarifas = null;
	        }
	        

	        vista.Tarifas = tarifas;
	        vista.PresentarTarifas(tarifas);
	        vista.PresentarInformacionRangos(null);

	        var frecuencias = this.ObtenerFrecuencias();
	        vista.EstablecerFrecuencias(frecuencias);

	        if (tipoCotizacion == null)
	            vista.EstablecerAnio(null);
	        else
	        {
	            if (tipoCotizacion == ETipoCotizacion.Average)
	                vista.EstablecerAnio(new Dictionary<String, String> {{"1", "1"}});
	            else
	            {
	                var anios = new Dictionary<String, String>();
	                for (int i = 0; i < plazo; i++)
	                {
	                    anios.Add((i + 1).ToString(), (i + 1).ToString());
	                }
	                vista.EstablecerAnio(anios);
	            }
	        }

	        this.PermitirModificar(permitirModificar.Value);
            vista.MostrarModoConsulta(vista.EsModoConsulta != null && vista.EsModoConsulta == true);
	    }

	    public void PermitirModificar(bool permitir)
	    {
            vista.PermitirAgregarRangos(permitir);
            vista.PermitirAnio(vista.EsModoConsulta.Value ? vista.SinTarifas.Value ? false : true : permitir);
            vista.PermitirFrecuencia(permitir);
            vista.PermitirGuardarAnio(permitir);
            vista.PermitirKmsHrsLibres(permitir);
            vista.PermitirKmHrMinima(permitir);
	    }

	    private Dictionary<String, String> ObtenerFrecuencias()
	    {
            var frecuencias = new Dictionary<string, string>();
            foreach(var frecuencia in Enum.GetValues(typeof(EFrecuencia)))
            {
                frecuencias.Add(Enum.GetName(typeof(EFrecuencia), frecuencia),((int)frecuencia).ToString(CultureInfo.InvariantCulture));
            }

	        return frecuencias;
	    } 
        /// <summary>
        /// Limpia la las variables de sesion
        /// </summary>
        public void LimpiarSesion()
        {
            vista.LimpiarSesion();
        }
        /// <summary>
        /// Agrega las tarifas para desplegar
        /// </summary>
        /// <param name="tarifas"></param>
        public void AgregarTarifas(List<TarifaFSLBO> tarifas)
        {
            vista.Tarifas= tarifas;
        }
        #endregion
    }
}