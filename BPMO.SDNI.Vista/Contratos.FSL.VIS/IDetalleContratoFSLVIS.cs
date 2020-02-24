// Satisface al Caso de Uso CU022 - Consultar Contratos Full Service Leasing
// Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
// Satisface al CU026 - Registrar Terminacion de Contrato Full Service Leasing
// Construccion durante staffing - Configuracion de INPC
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    public interface IDetalleContratoFSLVIS
    {
        #region Propiedades

        string Codigo { get; }
        int? ContratoID { get; set; }
        ContratoFSLBO Contrato { get; set; }
        EEstatusContrato? EstatusContrato { get; set; }
        int? UUA { get; }
        DateTime? FUA { get; }
        int? UnidadOperativaContratoID { get; set; }
        //SC_0008
        int? UsuarioID { get; }
        int? UnidadAdscripcionID { get; }
        /// <summary>
        /// Fecha en que se empieza a Aplicar el INPC del Contrato
        /// </summary>
        DateTime? FechaInicioINPC { get; set; }
        /// <summary>
        /// Determina si el INPC del Contrato es Fijo o Calculado por el Sistema
        /// </summary>
        bool? InpcFijo { get; set; }
        /// <summary>
        /// Valor del INPC que se aplicara al Contrato
        /// </summary>
        decimal? ValorInpc { get; set; }
        #endregion Propiedades

        #region Metodos

        void InicializarControles();        
        void CambiarAContrato();
        void CambiarALinea();
        void RegresarAConsultar();
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void DatosAInterfazUsuario(ContratoFSLBO contrato);
        void EstablecerPaqueteNavegacionEditar(string clave, ContratoFSLBO contrato);
        void IrAEditar();
        /// <summary>
        /// Redirige a la Interfaz para Editar las Lineas de Contrato
        /// </summary>
        void IrAModificarLineas();
        #region SC0002
        void IrAAgregarDocs();
        #endregion
        void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, Dictionary<string, object> DatosReporte);
        void IrAImprimir();
        #region SC_0008
        void PermitirRegistrar(bool status);
        void RedirigirSinPermisoAcceso();
        #endregion

		#region CU026

	    void IrACerrarContrato();

	    #endregion

        #region SC0038
        List<object> ObtenerPlantillas(string key);
        #endregion
	    #endregion Metodo
    }
}
