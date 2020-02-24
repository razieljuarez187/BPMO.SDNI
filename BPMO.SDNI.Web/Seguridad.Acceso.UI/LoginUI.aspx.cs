//Satisface al CU061 - Acceso al Sistema
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using BPMO.Security.BO;
using BPMO.Basicos;
using BPMO.Basicos.BO;

using BPMO.SDNI.Seguridad.Acceso.VIS;
using BPMO.SDNI.Seguridad.Acceso.PRE;

namespace BPMO.SDNI.Seguridad.Acceso.UI
{
    public partial class LoginUI : System.Web.UI.Page, ILoginVIS
    {
        LoginPRE loginPre;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();
            this.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();
            
            loginPre = new LoginPRE(this);

            if (!IsPostBack)
            {
                this.Session.Clear();
            }

            this.Page.Validate();
        }

        #region Propiedades y Funciones de la vista
        public string Usuario
        {
            get
            {
                if (this.txtUsuario.Text.Trim() != string.Empty)
                {
                    return txtUsuario.Text.Trim();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    txtUsuario.Text = value.Trim();
                }
                else
                {
                    txtUsuario.Text = "";
                }
            }
        }

        public string Password
        {
            get
            {
                if (txtContrasenia.Text.Trim() != string.Empty)
                {
                    return txtContrasenia.Text.Trim();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    txtContrasenia.Text = value.Trim();
                }
                else
                {
                    txtContrasenia.Text = "";
                }
            }
        }

        public UsuarioBO usuarioLogueado
        {
            get
            {
                if (this.Session["Usuario"] != null)
                {
                    return (UsuarioBO)this.Session["Usuario"];
                }
                else
                {
                    UsuarioBO usBO = null;
                    return usBO;
                }
            }
            set
            {
                this.Session["Usuario"] = value;
            }

        }

        public void Redirect()
        {
            Response.Redirect(this.ResolveUrl("~/Seguridad.Acceso.UI/ConfiguracionAccesoUI.aspx"));
        }

        public bool sesionVerificada
        {
            get
            {
                if (this.Session["SESION_VERIFICADA"] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                this.Session["SESION_VERIFICADA"] = value;
            }
        }

        public void LimpiarSesionVerificada()
        {
            this.Session.Remove("SESION_VERIFICADA");
        }

        public void MensajeError(string Mensaje)
        {            
            this.lblMensajeError.Text = Mensaje;
        }

        #endregion

        #region Botón Entrar al Sistema
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            loginPre.EntrarSistema();
        }
        #endregion
    }
}