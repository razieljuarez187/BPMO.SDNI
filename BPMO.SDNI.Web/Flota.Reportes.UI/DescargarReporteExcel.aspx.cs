using System;

namespace BPMO.SDNI.Flota.Reportes.UI {
    public partial class DescargarReporteExcel : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["DatosReporte"] != null) {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ReporteCentroControlRentas.xlsx");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                byte[] rtpExcel = (byte[])Session["DatosReporte"];

                Response.BinaryWrite(rtpExcel);
                Response.Flush();
                Response.End();
            }
        }
    }
}