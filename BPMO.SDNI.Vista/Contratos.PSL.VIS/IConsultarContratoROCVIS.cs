﻿using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BOF;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IConsultarContratoROCVIS {

        #region Propiedades

        UnidadOperativaBO UnidadOperativa { get; }
        string NumeroContrato { get; set; }
        int? CuentaClienteID { get; set; }
        int? ClienteID { get; set; }
        string NombreCuentaCliente { get; set; }
        DateTime? FechaInicioContrato { get; set; }
        DateTime? FechaFinContrato { get; set; }
        EEstatusContrato? Estatus { get; set; }
        string NumeroSerie { get; set; }
        string NumeroEcononomico { get; set; }
        List<ContratoPSLBOF> ContratosEncontrados { get; }
        UsuarioBO Usuario { get; }
        String SucursalNombre { get; set; }
        int? SucursalID { get; set; }
        int? UsuarioID { get; }
        int? UnidadAdscripcionID { get; }
        List<object> SucursalesAutorizadas { get; set; }
        int? ModuloID { get; }
        #endregion

        #region Métodos

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarContratosEncontrados();
        void CargarEstatus(List<EEstatusContrato> listado);
        void CargarContratosEncontrados(List<ContratoPSLBOF> contratos);
        void IrADetalle();
        void EstablecerPaqueteNavegacionImprimir(string codigoNavegacion, Dictionary<string, object> DatosReporte);
        void IrAImprimir();
        void BloquearConsulta();
        void PermitirRegistrar(bool status);
        void RedirigirSinPermisoAcceso();
        object ObtenerPaqueteNavegacion();
        void EstablecerPaqueteNavegacion(string ClaveContrato, ContratoPSLBOF contrato, Dictionary<string, object> elementosFiltro);
        void LimpiarPaqueteNavegacion();
        void EstablecerPagResultados(int indice);
        int ObtenerPagResultados();
        #endregion
    }
}