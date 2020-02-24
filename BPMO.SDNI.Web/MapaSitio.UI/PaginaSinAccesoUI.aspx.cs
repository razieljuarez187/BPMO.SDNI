using System;

namespace BPMO.SDNI.MapaSitio.UI
{
    public partial class PaginaSinAccesoUI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/MenuPrincipalUI.aspx"));
        }
    }
}