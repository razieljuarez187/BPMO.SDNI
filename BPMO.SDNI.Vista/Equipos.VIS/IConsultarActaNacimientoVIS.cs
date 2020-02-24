//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IConsultarActaNacimientoVIS
    {
        #region Propiedades
        int? UsuarioAutenticado { get; }
        int? UnidadOperativaId
        {
            get;
        }
        int? ModuloID { get; }
        string LibroActivos { get; set; }

        String NumeroVIN
        {
            get;
            set;
        }
        String SucursalNombre
        {
            get;
            set;
        }
        int? SucursalID
        {
            get;
            set;
        }
        String NumeroEconomico
        {
            get;
            set;
        }
        String ClienteNombre
        {
            get;
            set;
        }
        int? ClienteID
        {
            get;
            set;
        }
        DateTime? FechaCompra
        {
            get;
            set;
        }
        int? Area
        {
            get;
            set;
        }
        int? EstatusActa
        {
            get;
            set;
        }

        List<UnidadBO> Resultado { get; set; }
        int IndicePaginaResultado { get; set; }

        #region REQ 13285 Lista de acciones permitirdas para el usuario.

        List<CatalogoBaseBO> ListaAcciones { get; set; }

        #endregion
        #endregion

        #region Métodos
        void PrepararBusqueda();

        void ActualizarResultado();

        void EstablecerPaqueteNavegacion(string nombre, object valor);
        void RedirigirADetalles();
        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        #region SC0008

        void PermitirRegistrar(bool habilitar);
        void RedirigirSinPermisoAcceso();

        #endregion

        #region REQ 13285 Método que determina el funcionamiento de los controles de acuerdo a la unidad operativa.

        void EstablecerAcciones(ETipoEmpresa tipoEmpresa);

        #endregion
        #endregion
    }
}
