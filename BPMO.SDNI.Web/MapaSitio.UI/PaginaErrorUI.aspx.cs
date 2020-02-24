using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BPMO.SDNI.MapaSitio.UI
{
    public partial class PaginaErrorUI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //if (Request.QueryString["aspxerrorpath"] != null)
                //    this.ibnRegresar.PostBackUrl = this.ResolveUrl(Request.QueryString["aspxerrorpath"].ToString());

                //this.SetErrorCode();

                //this.SetMessage();
            }

        }

        //private void SetErrorCode()
        //{
        //    if (Request.QueryString["ErrorCode"] != null)
        //    {
        //        string sErrorCode = Request.QueryString["ErrorCode"].ToString();

        //        switch (sErrorCode)
        //        {
        //            case "400":
        //                this.lblTitulo.Text = "400: Petición Incorrecta";
        //                this.lblSubTitulo.Text = "El servidor no entendió lo que necesita";
        //                break;
        //            case "403":
        //                this.lblTitulo.Text = "403: Prohibido";
        //                this.lblSubTitulo.Text = "El servidor entendió lo que necesita pero se niega a hacerlo";
        //                break;
        //            case "404":
        //                this.lblTitulo.Text = "404: Página No Encontrada";
        //                this.lblSubTitulo.Text = "¿Está seguro(a) de que es la dirección correcta?";
        //                break;
        //            case "408":
        //                this.lblTitulo.Text = "403: La Petición Excedió el Tiempo Permitido";
        //                this.lblSubTitulo.Text = "El servidor tardó más del tiempo permitido en realizar la operación";
        //                break;
        //            default:
        //                this.lblTitulo.Text = "Ha Ocurrido un Error Inesperado";
        //                this.lblSubTitulo.Text = "Y estamos trabajando en ello";
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        this.lblTitulo.Text = "Ha Ocurrido un Error Inesperado";
        //        this.lblSubTitulo.Text = "Y estamos trabajando en ello";
        //    }
        //}

        //private void SetMessage()
        //{
        //    if (Mensaje.UltimaExcepcion != null)
        //    {
        //        this.txtMensaje.Text = "Error Caught in Application_Error event\n" +
        //            "Error in: " + Request.Url.ToString() +
        //            "\n\nError Message:" + Mensaje.UltimaExcepcion.Message.ToString() +
        //            "\n\nStack Trace:" + Mensaje.UltimaExcepcion.StackTrace.ToString();
        //    }
        //    else
        //    {
        //        this.txtMensaje.Text = "There is no exception.";
        //    }
        //}
    }
}