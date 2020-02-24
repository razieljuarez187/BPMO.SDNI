//Satisface al CU061 - Acceso al Sistema
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Security.BO;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Seguridad.Acceso.VIS
{
    public interface ILoginVIS
    {
        #region Propiedades
        string Usuario { get; set; }
        string Password { get; set; }
        bool sesionVerificada { get; set; }

        UsuarioBO usuarioLogueado { get; set; }
        #endregion

        #region Funciones
        void Redirect();
        void MensajeError(string Mensaje);
        void LimpiarSesionVerificada();
        #endregion
    }
}
