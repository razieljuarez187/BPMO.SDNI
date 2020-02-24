// Satisface al Caso de uso CU015 - Registrar Contrato Full Service Leasing
// Satisface al Caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al Caso de uso CU023 - Editar Contrato Full Service Leasing
using System.Collections.Generic;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface ITarifasEquipoAliadoVIS
    {
        #region Propiedades
        
        int? Plazo { get; }
        ETipoCotizacion? TipoCotizacion { get;  }
        int? EquipoAliadoID { get; }
        int? EquipoID { get; }
        int? UnidadID { get; }
        int? UnidadOperativaID { get; }
        int? SucursalID { get; }
        CargoAdicionalEquipoAliadoBO CargoAdicional { get; }
        string NombreModelo { set; }
        string NumeroSerie { set; get; }
        string NombreSessiondeColeccion { get; }
        List<CargoAdicionalEquipoAliadoBO> ListadoCargosAdicionalesEquipoAliado { get; set; }
        bool ModoConsultar{get;}
        int? CargoAdicionalID { get; }
        bool SinTarifas { get; set; }
        #endregion

        #region Metodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void Inicializar();
        void Inicializar(List<TarifaFSLBO> tarifasEquiposAliados);
        void ConfigurarModoConsultar();
        void ConsultarModoEditar();
        void ConfigurarTipoCargoConsultado(bool? cargoPorKm);
        void DesplegarTarifas(List<TarifaFSLBO> list);
        void EstablecerSinTarifas();

        #endregion
    }
}
