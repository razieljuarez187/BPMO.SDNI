//Satisface al CU062 - Menú Principal
using System.Collections.Generic;

using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BO;

namespace BPMO.SDNI.MapaSitio.VIS
{
    public interface IMasterPageVIS
    {
        #region Propiedades
        List<DatosConexionBO> ListadoDatosConexion { get; set; }
        UsuarioBO Usuario { get; set; }
        AdscripcionBO Adscripcion { get; set; }
        string Ambiente { get; set; }
        List<ProcesoBO> ListadoProcesos { get; set; }

        string URLLogoEmpresa { get; }
        string DireccionCSS { get; }
        string NombreSistema { get; }
        #endregion

        #region Métodos
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void CargarProcesos();
        void MenuPredeterminado();
        #region SC0008
        void InicializarConfiguracionPrueba();
        #endregion SC0008
        #endregion
    }
}
