//Satisface al CU072 - Obtener Auditoría
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using BPMO.SDNI.Mantenimiento.PRE;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.VIS;
using System.Web.SessionState;

namespace BPMO.SDNI.Mantenimiento.UI {

    /// <summary>
    /// Handler para la descarga de la Evidencia de Auditoría Mantenimiento Idealease.
    /// </summary>
    public class hdlrDescargarEvidenciaAuditoria : IHttpHandler, IRequiresSessionState {
        
        #region Propiedades

        /// <summary>
        /// Obtiene o establece un valor que representa el contexto.
        /// </summary>
		private HttpContext context = null;

        /// <summary>
        /// Obtiene o establece un valor que representa el contexto.
        /// </summary>
        public HttpContext ContextValue {
			get { return this.context; }
			set { this.context = value; }
		}

        /// <summary>
        /// Obtiene un valor que representa el archivo a descargar.
        /// </summary>
        private EvidenciaAuditoriaMantenimientoBO auditoria;

        /// <summary>
        /// Obtiene un valor que representa el archivo a descargar.
        /// </summary>
        public EvidenciaAuditoriaMantenimientoBO ArchivoTemp {
            get { return auditoria; }
        }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre del archivo a descargar.
        /// </summary>
        private String NombreArchivo {
            get;
            set;
        }

        /// <summary>
        /// Obtiene un valor que representa el Identificador del archivo.
        /// </summary>
        private int? archivoID;

        /// <summary>
        /// Obtiene un valor que representa el Identificador del archivo.
        /// </summary>
        public int QS_auditoriaID {
			get {
                string param = ContextValue.Request.QueryString["archivoID"];
				archivoID = Int32.Parse(param);
				return archivoID.Value;
			}
		}

		/// <summary>
		/// Obtiene un valor que indica si la instancia se crea por petición.
		/// </summary>
        public bool IsReusable {
            get { return false; }
        }

		#endregion

		#region Constructor

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public hdlrDescargarEvidenciaAuditoria() { }

		#endregion

		#region Métodos

        /// <summary>
        /// Obtiene un valor que representa el espacio de memoria del archivo a descargar.
        /// </summary>
        /// <returns>El espacio de memoria del archivo a descargar.</returns>
		public MemoryStream GetData() {
			if (auditoria != null) {
			    if (auditoria.DocumentoEvidencia != null) {
                    MemoryStream ReturnStream = new MemoryStream(auditoria.DocumentoEvidencia);
                    return ReturnStream;
			    }			    
			}
			return null;
		}

        /// <summary>
        /// Obtiene el archivo y el tipo de archivo a descargar.
        /// </summary>
        /// <param name="context"></param>
		public void ProcessRequest(HttpContext context) {
			this.ContextValue = context;
            auditoria = context.Session["evidencia"] as EvidenciaAuditoriaMantenimientoBO;
            string mimeType = getMimeType();
            if(mimeType != null){
			    try {
                    System.IO.MemoryStream mstream = GetData();
				    if (mstream != null) {
					    byte[] byteArray = mstream.ToArray();
					    mstream.Flush();
					    mstream.Close();
					    context.Response.Clear();
					    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + ArchivoTemp.Nombre);
					    context.Response.AddHeader("Content-Length", this.ArchivoTemp.DocumentoEvidencia.Length.ToString());
                        context.Response.ContentType = mimeType;
					    context.Response.BinaryWrite(byteArray);
				    }
			    } catch (Exception Ex) {
				    throw Ex;
			    }
            } 
		}
        
        /// <summary>
        /// Obtiene el tipo de contenido del archivo a descargar.
        /// </summary>
        /// <returns>El mimetype del archivo a descargar.</returns>
        private String getMimeType() {
            String formatosValidos = ConfigurationManager.AppSettings["FormatosValidos"];
            string[] datos = ArchivoTemp.Nombre.Split('.');
            NombreArchivo = datos[0];
            string extension = "." + datos[datos.Length - 1];
            if(formatosValidos.Contains(extension)){
                switch (datos[datos.Length - 1]) { 
                    case "pdf":
                        return "application/pdf";
                    default:
                        return "image/" + extension;
                }
            }
            return null;
        }

		#endregion
    }
}