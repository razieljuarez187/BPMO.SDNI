using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Servicio.Procesos.BO;

namespace BPMO.SDNI.Mantenimiento.UI
{
    public partial class PruebaAuditoria : System.Web.UI.Page
    {
        /// <summary>
        /// Variable de session recibida desde modulo de recepcion de unidades CU009
        /// </summary>
        public OrdenServicioBO MantenimientoRecibido
        {
            get { return Session["MantenimientoAuditoria"] != null ? Session["MantenimientoAuditoria"] as OrdenServicioBO : null; }
            set { Session["MantenimientoAuditoria"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Redireccionar_Click(object sender, EventArgs e)
        {
            var OrdenServicio = new OrdenServicioBO() { Id = int.Parse(txtFolio.Text) };

            MantenimientoRecibido = OrdenServicio;
            Response.Redirect("~/Mantenimiento.UI/RealizarAuditoriaMantenimientoUI.aspx");
        }
    }
}