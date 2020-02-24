using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.Servicio.Catalogos.BO;
using System.Web.UI.WebControls;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IConsultarConfiguracionParametroMantenimientoVIS
    {
        #region Propiedades
       
        #region Propiedades Idealise
        /// <summary>
        /// Usuario autenticado en el sistema
        /// </summary>
        int? UsuarioAutenticado { get; }
        /// <summary>
        /// Unidada operativa donde esta corriendo el sistema
        /// </summary>
        int? UnidadOperativaId { get; }
        /// <summary>
        /// Modulo desde donde esta corriendo el sistema
        /// </summary>
        int? ModuloID { get; }
        #endregion

        #region Propiedades Interfaz

        string Modelo { get; set; }

        int? ModeloID { get; set; }

        ETipoMantenimiento? TipoMantenimiento { get; set; }

        EUnidaMedida? UnidadMedida { get; set; }

        bool? Estatus { get; set; }

        GridView GridConfiguracionesMantenimiento { get; set; }

        List<ConfiguracionMantenimientoBO> configuraciones { get; set; }

        #endregion

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo para mostrar mensajes en la UI
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="detalle"></param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void RedirigirSinPermisoAcceso();

        //void PermitirRegistrar(bool p);

        #endregion



        
    }
}
