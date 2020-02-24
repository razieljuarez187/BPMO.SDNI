using System;
using System.IO;

//Satisface al CU010 - Catálogo de Documentos
using System.Web;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Comun.PRE;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Comun.UI
{
	/// <summary>
	/// Descripción breve de hdlrCatalogoDocumentos
	/// </summary>
	public class hdlrCatalogoDocumentos : IHttpHandler, IhdlrCatalogoDocumentosVIS
	{
		#region Atributos

		internal hdlrCatalogoDocumentosPRE presentador = null;
		private ArchivoBO archivoBO = null;
		private int? archivoID = null;
		private HttpContext context = null;

		#endregion Atributos

		#region Propiedades

		public ArchivoBO ArchivoTemp
		{
			get
			{
				return this.archivoBO;
			}
		}

		public HttpContext ContextValue
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public int QS_archivoID
		{
			get
			{
				string param = ContextValue.Request.QueryString["archivoID"];
				archivoID = Int32.Parse(param);
				return archivoID.Value;
			}
		}

		#endregion Propiedades

		#region Constructor

		public hdlrCatalogoDocumentos()
		{
			presentador = new hdlrCatalogoDocumentosPRE(this);
		}

		#endregion Constructor

		#region Métodos

		public MemoryStream GetData(int archivoID)
		{
			archivoBO = this.presentador.ObtenerArchivoDescargable(archivoID);
			if (archivoBO != null)
			{
				MemoryStream ReturnStream = new MemoryStream(archivoBO.Archivo);
				return ReturnStream;
			}
			return null;
		}

		public void ProcessRequest(HttpContext context)
		{
			this.ContextValue = context;
			try
			{
				System.IO.MemoryStream mstream = GetData(this.QS_archivoID);

				if (mstream != null)
				{
					byte[] byteArray = mstream.ToArray();
					mstream.Flush();
					mstream.Close();
					context.Response.Clear();
					context.Response.AddHeader("Content-Disposition", "attachment; filename=" + this.ArchivoTemp.Nombre);
					context.Response.AddHeader("Content-Length", this.ArchivoTemp.Archivo.Length.ToString());
					context.Response.ContentType = this.ArchivoTemp.TipoArchivo.MimeType;
					context.Response.BinaryWrite(byteArray);
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		#endregion Métodos
	}
}