//Satisface a la solicitud de Cambio SC0001
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.PRE;
using BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS;
using BPMO.SDNI.MapaSitio.UI;
using DevExpress.Utils.Design;
using DevExpress.Utils.OAuth;
using DevExpress.Web.ASPxClasses.Design;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.UI
{
    public partial class ConsultarMasterFacturacionUI : Page, IConsultarMasterFacturacionVIS
    {
        #region Atributos

        /// <summary>
        /// Presentador de la UI
        /// </summary>
        private ConsultarMasterFacturacionPRE presentador;

        private const string nombreClase = "ConsultarMasterFacturacionUI";

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene la Unidad operativa a la cual pertenece el usuario autenticado
        /// </summary> 
        public int? UnidadOperativaID
        {
            get
            {
                var masterMsj = (Site) Page.Master;
                return masterMsj != null && masterMsj.Adscripcion != null &&
                       masterMsj.Adscripcion.UnidadOperativa != null
                    ? masterMsj.Adscripcion.UnidadOperativa.Id.Value
                    : (int?) null;
            }
        }

        /// <summary>
        /// Ambiente de Ejecución del Sistema
        /// </summary>
        public int? Ambiente
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Ambiente"]))
                    return null;

                return int.Parse(ConfigurationManager.AppSettings["Ambiente"]);
            }
        }


        /// <summary>
        /// Identificador del Usuario en Sesion
        /// </summary>
        public int? UsuarioID
        {
            get
            {
                var masterMsj = (Site) this.Page.Master;

                if (masterMsj != null && masterMsj.Usuario != null)
                    return masterMsj.Usuario.Id.Value;
                return null;
            }
        }

        /// <summary>
        /// Identificador del Modulo desde el cual se esta accesando
        /// </summary>
        public int? ModuloID
        {
            get
            {
                return ((Site)Page.Master).ModuloID;
            }
        }

        #endregion

        #region Constructores

        protected void Page_Load(object sender, EventArgs e)
        {
            presentador = new ConsultarMasterFacturacionPRE(this);
            presentador.ValidarAcceso();
            presentador.ConsultarUrl();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Envia al usuario a la página de la Master de Facturación
        /// </summary>
        public void RedirigirMaster(string urlMaster, string uoid, string sucid, string usuarioId, string ambiente)
        {
            const string strHtml = @"<html>
            <body style='display: none'>
            <form action='{0}' method='post' id='form1'>
            <input id='UOId' name='UOId' value='{1}' />
            <input id='SucId' name='SucId' value='{2}' />
            <input id='UsuarioId' name='UsuarioId' value='{3}' />
            <input id='Ambiente' name='Ambiente' value='{4}' />
            </form>
            <script type='text/javascript'>document.getElementById('form1').submit();</script>
            </body>
            </html>";

            hdnHtml.Value = string.Format(strHtml, Server.UrlPathEncode(urlMaster), uoid, sucid, usuarioId, ambiente);
        }

        /// <summary>
        /// Redirige a la pagina "Sin permisos de Acceso"
        /// </summary>
        public void RedirigirSinPermisoAcceso()
        {
            Response.Redirect(ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"), true);
        }

        /// <summary>
        /// Muestra el mensaje del Sistema al Usuario
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="detalle"></param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null)
        {
            var master = (Site)Master;
            if(tipo == ETipoMensajeIU.ERROR)
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo, detalle);
            }
            else
            {
                if(master != null) master.MostrarMensaje(mensaje, tipo);
            }
        }

        #endregion
    }
}