using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace BPMO.SDNI.Buscador.UI {
    public partial class ExportaFormatoUI : System.Web.UI.Page {
        #region Propiedades
        private string FormatoQuery {
            get {
                if (Request.QueryString["formato"] != null)
                    return Request.QueryString["formato"];
                else
                    return null;
            }
        }
        protected Byte[] DocumentoBase64 {
            get {
                return (this.Session["DocumentoBase64"] == null) ? null : (Byte[])this.Session["DocumentoBase64"];
            }
            set {
                if (value != null)
                    Session["DocumentoBase64"] = value;
                else
                    Session.Remove("DocumentoBase64");
            }
        }
        #endregion

        #region Métodos
        protected void Page_Load(object sender, EventArgs e) {
            Byte[] report = this.DocumentoBase64;

            if (report != null && !string.IsNullOrWhiteSpace(this.FormatoQuery)) {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("content-lenght", report.Length.ToString());
                switch (this.FormatoQuery.ToUpper()) { 
                    case "XLS":
                        Page.Response.ContentType = "application/vnd.xls";
                        Response.AddHeader("content-disposition", @"inline;filename=""Reporte.xls""");
                        break;
                    case "XLSX":
                        Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", @"inline;filename=""Reporte.xlsx""");
                        //Page.Response.OutputStream.Write(report, 0, report.Length);
                        break;
                    default:
                        // PDF
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", @"inline;filename=""Reporte.pdf""");
                        break;
                }
                Response.BinaryWrite(report);
                Response.Flush();
                Response.End();
            } else {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<div>");
                sb.AppendLine("<fieldset style=" + "'text-align:center; background-color:#FFFF99'>");
                sb.AppendLine("<h3>No se cuenta con la información para mostrar ó los datos han caducado!!!.</h3>");
                sb.AppendLine("</fieldset>");
                sb.AppendLine("</div>");
                Response.Write(sb.ToString());
            }
            this.DocumentoBase64 = null;
        }
        #endregion
    }
}