using BPMO.Primitivos.Enumeradores;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    public interface ITarifaPersonalizadaPSLVIS {
        #region Propiedades
        int? UnidadOperativaID { get; set; }
        int? ModeloID { get; set; }
        int? ModuloID { get; set; }
        int? SucursalID { get; set; }
        int? UsuarioID { get; set; }
        int? CuentaClienteID { get; set; }


        //Tarifa personalizada
        string TarifaPersonalizadaEtiqueta { get; set; }
        decimal? TarifaPersonalizadaTarifa { get; set; }
        decimal? TarifaPersonalizadaTarifaConDescuento { get; set; }
        string TarifaPersonalizadaTurno { get; set; }
        string TarifaPersonalizadaCodigoAutorizacion { get; set; }
        decimal? TarifaPersonalizadaPorcentajeDescuento { get; set; }
        string TarifaPersonalizadaTipoTarifa { get; set; }
        decimal? TarifaPersonalizadaTarifaHrAdicional { get; set; }
        decimal? TarifaPersonalizadaDescuentoMax { get; set; }
        bool esTarifaAlza { get; }
        decimal? TarifaBase { get; set; }
        decimal? DescuentoBase { get; set; }
        decimal? TarifaPersonalizadaPorcentajeSeguro { get; set; }
        #endregion

        #region Metodos
        void LimpiarPaqueteNavegacion(string key);

        void EstablecerPaqueteNavegacion(string key, object value);

        object ObtenerPaqueteNavegacion(string key);

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void RegistrarScript(string key, string script);

        void Inicializar();

        void PermitirValidarCodigoAutorizacion(bool permitir);

        void PermitirSolicitarCodigoAutorizacion(bool permitir);

        void PermitirAplicarSinCodigoAutorizacion(bool permitir);

        void EstablecerEtiquetaBoton(string etiqueta);

        #endregion
    }
}