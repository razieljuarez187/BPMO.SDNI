//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Interfaz para el UserControl de Datos Generales de elemento de flota
    /// </summary>
    public interface IucDatosGeneralesElementoVIS
    {
        #region propiedades
        string UnidadID { get; set; }
        string EquipoID { get; set; }
        string LiderID { get; set; }
        string OracleID { get; set; }
        string NumeroEconomico { get; set; }
        string Numeroserie { get; set; }
        string PlacasFederales { get; set; }
        string PlacasEstatales { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
        string CapacidadCarga { get; set; }
        string CapacidadTanque { get; set; }
        string RendimientoTanque { get; set; }
        string Anio { get; set; }
        string Sucursal { get; set; }
        string ClaveProductoServicio { get; set; }
        string DescripcionProductoServicio { get; set; }
        #endregion

        #region métodos
        void MostrarProductoServicio(bool mostrar);
        void EstablecerAcciones();
        #endregion
    }
}