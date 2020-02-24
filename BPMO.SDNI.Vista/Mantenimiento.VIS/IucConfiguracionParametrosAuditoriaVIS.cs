using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Primitivos.Enumeradores;
using BPMO.Servicio.Catalogos.BO;
using System.Windows.Forms;

namespace BPMO.SDNI.Mantenimiento.VIS
{
    public interface IucConfiguracionParametrosAuditoriaVIS
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

        int? SucursalID { get; set; }

        string SucursalNombre { get; set; }

        int? TallerID { get;}

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

        void enlazarControles(List<TallerBO> talleres);

        void HabilitarSucursal(bool activo);

        void HabilitarModelo(bool activo);

        void HabilitarTaller(bool activo);

        #endregion

        void RedirigirSinPermisoAcceso();
    }
}
