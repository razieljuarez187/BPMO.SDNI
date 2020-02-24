//Satisface al CU061 - Acceso al Sistema
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Security.BO;
using BPMO.Security.BR;
using System.Configuration;
using System.Xml.Linq;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;

using BPMO.SDNI.Seguridad.Acceso.VIS;

namespace BPMO.SDNI.Seguridad.Acceso.PRE
{
    public class LoginPRE
    {
        #region Atributos
        private ILoginVIS vista;
        private IDataContext dataContext = null;
        #endregion

        #region Constructores
        public LoginPRE(ILoginVIS vistaActual)
        {
            this.vista = vistaActual;

        }
        #endregion

        #region Método para validación del usuario
        public void EntrarSistema()
        {
            UsuarioLoginBO usuarioBO = new UsuarioLoginBO();
            dataContext = FacadeBR.ObtenerConexion();
            usuarioBO.Usuario = vista.Usuario;
            usuarioBO.Password = vista.Password;
            
            int? numeroEmpleado = null;

            try
            {
                numeroEmpleado = FacadeBR.VerificarAcceso(dataContext, usuarioBO);
            }
            catch (Exception ex)
            {
                if (numeroEmpleado.HasValue)
                {
                    this.vista.MensajeError("* Usuario o Contraseña no valido");
                    return;
                }
                else
                {
                    this.vista.MensajeError(ex.GetBaseException().Message);
                    return;
                }
            }

            vista.usuarioLogueado = usuarioBO;
            vista.Redirect();
        }

        public void PageLoad_NoIspostBack()
        {
            if (vista.usuarioLogueado == null)
            {
                if (vista.sesionVerificada == false)
                {
                    //Enviar a Pagina de Inicio.
                    vista.sesionVerificada = true;
                }
            }

        }
        #endregion
    }
}
