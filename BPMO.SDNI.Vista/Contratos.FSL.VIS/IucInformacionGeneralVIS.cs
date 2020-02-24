// Satisface al caso de uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al caso de uso CU023 - Editar Contrato Full Service Leasing 
// Satisface al caso de uso CU015 - Registrar Contrato Full Service Leasing
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IucInformacionGeneralVIS
    {
        #region Propiedades

        UnidadOperativaBO UnidadOperativa { get; }
        List<SucursalBO> ListadoSucursales { get; set; }
        SucursalBO SucursalSeleccionada { get; }
        List<MonedaBO> ListadoMonedas { get; set; }
        MonedaBO MonedaSeleccionada { get; }
        DateTime? FechaContrato { get; set; }
        int? EmpresaID { get; set; }
        string NombreEmpresa { get; set; }
        string Representante { get; set; }
        string DomicilioEmpresa { get; set; }
        UsuarioBO Usuario { get; }
        int? ModuloID { get; }
        decimal? PorcentajePenalizacion { get; set; } //SC0007
        #endregion

        #region Metodos

        void ConfigurarModoConsultar();
        void ConfigurarModoEditar();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void EstablecerSucursalSeleccionada(int? Id);
        void EstablecerMonedaSeleccionada(string Codigo);

        #endregion
    }
}
