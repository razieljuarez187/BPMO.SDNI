//Esta clase satisface los requerimientos especificados en el caso de uso CU006 Consultar Flota de Renta Diaria
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Flota.BO;

namespace BPMO.SDNI.Flota.VIS
{
    /// <summary>
    /// Vista de la página de consulta de flota
    /// </summary>
    public interface IConsultarFlotaVIS
    {
        #region Propiedades
        UsuarioBO Usuario { get; }
        UnidadOperativaBO UnidadOperativa { get; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        string NumeroSerie { get; set; }
        string NumeroEconomico { get; set; }
        int? MarcaID { get; set; }
        string MarcaNombre { get; set; }
        int? ModeloID { get; set; }        
        string ModeloNombre { get; set; }
        #region SC0019
        int? ModeloEAID { get; set; }
        string ModeloEANombre { get; set; }
        #endregion
        int? Anio { get; set; }
        string Placas { get; set; }
        int IndicePaginaResultado { get; set; }
        List<ElementoFlotaBO> Resultado { get; set; }
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void LimpiarSesion();
        void LimpiarFlotaEncontrada();
        void CargarElementosFlotaEncontrados(List<ElementoFlotaBO> elementos);
        void EstablecerPaqueteNavegacion(string Clave, int? UnidadID);
        void IrADetalle();
        void HabilitarModelo(bool estado);
        void BloquearConsulta(bool estado);
        void RedirigirSinPermisoAcceso();       
        void ActualizarResultado();
        object ObtenerPaqueteNavegacion();//SC0019
        void LimpiarPaqueteNavegacion();//SC0019
        #endregion                
    }
}