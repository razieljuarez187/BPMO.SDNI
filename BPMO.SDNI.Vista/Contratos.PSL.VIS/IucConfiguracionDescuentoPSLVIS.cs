using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.PSL.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface IucConfiguracionDescuentoPSLVIS {
        #region Propiedades
        #region Descuento
        int? UnidadOperativaId { get; }
        int? UsuarioAutenticado { get; }
        int? ClienteId { get; set; }
        int? SucursalId { get; set; }
        string Cliente { get; set; }
        DateTime? FechaInicio { get; set; }
        DateTime? FechaFin { get; set; }
        decimal? DescuentoMaximo { get; set; }
        bool? Estado { get; set; }
        string ContactoComercial { get; set; }
        string SucursalNombre { get; set; }
        UsuarioBO Usuario { get; }
        UnidadOperativaBO UnidadOperativa { get; }
        #endregion
        ConfiguracionDescuentoBO UltimoObjeto { get; set; }
        List<ConfiguracionDescuentoBO> ListaDescuentos { get; set; }
        List<ConfiguracionDescuentoBO> ListaDetalle { get; set; }
        int IndicePaginaResultado { get; set; }
        List<SucursalBO> ListaSucursales { get; set; }
        List<SucursalBO> SucursalesAutorizadas { get; set; }
        #endregion

        #region Métodos
        void PrepararNuevo();

        void HabilitarModoEdicion(bool habilitar);

        void HabilitaCheckSucursales(bool habilitar);

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void MostrarBotonAgregar(bool visible);

        void HabilitaBotonActualizar(bool habilitar);

        void MostrarBotonCancelar(bool visible);

        void HabilitarCliente(bool habilitar);

        void HabilitarContactoComercial(bool habilitar);

        void DesHabilitarSucursal(bool habilitar);

        bool ExisteDatosEnGrid();

        void Inicializarfechas();
        #endregion

        #region Métodos Editar
        void CrearTabla(List<ConfiguracionDescuentoBO> lstDescuentos);
        void HabilitarCamposEditar(bool habilitar);
        void HabilitarBotonesParaEditar(bool habilitar);
        void DeshabilitarBotonesNoUtilizadosParaEditar();
        void EstablecerDatosNavegacion(object configuracionDescuento);
        #endregion

        #region Metodos Registrar

        void MostrarMensajeRegistro(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void llenaGridPorSucursal(List<SucursalBO> lstSucursales);

        #endregion

        void RedirigirSinPermisoAcceso();
        void IngresarAccion(String Accion);
        void LimpiarSesiones();

        bool EsRegistro();

        bool EsActualizarEnRegistro();
        bool TodasSucursales();
        bool EsCancelar();
    }
}