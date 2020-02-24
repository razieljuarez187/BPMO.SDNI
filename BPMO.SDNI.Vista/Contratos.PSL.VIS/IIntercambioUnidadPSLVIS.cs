using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Equipos.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IIntercambioUnidadPSLVIS {

        #region Propiedades
        UnidadOperativaBO UnidadOperativa { get; }
        int? UsuarioID { get; }
        UsuarioBO Usuario { get; }
        int? UnidadOperativaID { get; }
        int? ModuloID { get; }

        object UltimoObjeto { get; set; }

        int? ContratoID { get; set; }
        int? EstatusID { get; set; }
        int? UUA { get; set; }
        DateTime? FUA { get; set; }
        DateTime? FC { get; }
        int? UC { get; }

        int? UnidadID { get; set; }
        int? EquipoID { get; set; }

        string ECodeCliente { get; set; }
        string ModeloCliente { get; set; }
        string HorometroUnidadCliente { get; set; }
        string PorcentajeUnidadCliente { get; set; }

        string HorometroUnidadIntercambio { get; set; }
        string PorcentajeCombustibleIntercambio { get; set; }


        List<UnidadBO> lstUnidades { get; set; }

        DateTime? FechaIntercambio { get; set; }
        List<SucursalBO> SucursalesAutorizadas { get; set; }

        string NumeroSerie { get; set; }

        int? SucursalID { get; set; }
        string ModeloNombre { get; set; }
        string ECode { get; set; }
        int? ModeloID { get; set; }
        ETipoContrato? TipoContrato { get; set; }

        int? IntercambioUnidadID { get; set; }
        int? IntercambioEquipoID { get; set; }

        #endregion

        #region Métodos

        void EstablecerPaqueteNavegacion(string key, object value);
        object ObtenerPaqueteNavegacion(string key);
        void LimpiarPaqueteNavegacion(string key);

        void RedirigirACancelar();
        void RedirigirADetalles();
        void RedirigirAConsulta();
        void RedirigirSinPermisoAcceso();

        void PermitirIntercambiar(bool permitir);

        void LimpiarSesion();
        void LimpiarCampos();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void CargarSerie(Dictionary<string, object> listadoEquipos);
        void HabilitarBotonTerminar(bool habilitar);
        #endregion
    }
}
