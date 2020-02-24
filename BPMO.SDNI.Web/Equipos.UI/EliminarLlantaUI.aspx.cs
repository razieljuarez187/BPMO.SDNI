//Satisface al CU089 - Bitácora de llantas
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Facade.SDNI.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Equipos.PRE;
using BPMO.SDNI.Equipos.VIS;

namespace BPMO.SDNI.Equipos.UI
{
	public partial class EliminarLlantaUI : System.Web.UI.Page, IEliminarLlantaVIS
	{
		#region Atributos

		private IDataContext dctx = FacadeBR.ObtenerConexion();
		private EliminarLlantaPRE presentador;

		#endregion Atributos

		#region Constructores

		#endregion Constructores

		#region Metodos

		public void EstablecerDatosNavegacion(string nombre, object valor)
		{
			Session[nombre] = valor;
		}

		public void InicializarControles(List<TipoArchivoBO> tiposArchivo)
		{
			DocumentosEliminar.LimpiarSesion();
			DocumentosEliminar.LimpiaCampos();
			DocumentosEliminar.TiposArchivo = tiposArchivo;
			Session.Remove("ArchivosEliminar");
		}

		/// <summary>
		/// Desplega el mensaje de error/advertencia/información en la UI
		/// </summary>
		/// <param name="">Mensaje a desplegar</param>
		/// <param name="tipoMensaje">1: Error, 2: Advertencia, 3: Información</param>
		public void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null)
		{
			string sError = string.Empty;
			if (tipoMensaje == ETipoMensajeIU.ERROR)
			{
				if (this.hdnMensaje == null)
					sError += " , hdnDetalle";
				this.hdnDetalle.Value = msjDetalle;
			}
			if (hdnMensaje == null)
				sError += " , hdnMensaje";
			if (hdnTipoMensaje == null)
				sError += " , hdnTipoMensaje";
			if (sError.Length > 0)
				throw new Exception("No se pudo desplegar correctamente el error. No se encontró el control: " + sError.Substring(2) + " en la MasterPage.");

			this.hdnMensaje.Value = mensaje;
			this.hdnTipoMensaje.Value = ((int)tipoMensaje).ToString();
		}

		public void RedirigirDetalleLlanta()
		{
			
			this.RegistrarScript("RedirigirLlanta", "Cerrar(1);");
		}

		/// <summary>
		/// Registra Script en el cliente
		/// </summary>
		/// <param name="key">Key del script</param>
		/// <param name="script">script a registrar</param>
		private void RegistrarScript(string key, string script)
		{
			ScriptManager.RegisterStartupScript(Page, typeof(Page), key, script, true);
		}

		//SC_0008
		public void RedirigirSinPermisoAcceso()
		{
			this.Response.Redirect(this.ResolveUrl("~/MapaSitio.UI/PaginaSinAccesoUI.aspx"));
		}
		#endregion Metodos

		#region Propiedades

		public int CantidadArchivos
		{
			get
			{
				return DocumentosEliminar.NuevosArchivos != null ? DocumentosEliminar.NuevosArchivos.Count : 0;
			}
		}

		public List<ArchivoBO> DocumentosAdjuntos
		{
			get
			{
				return DocumentosEliminar.NuevosArchivos ?? new List<ArchivoBO>();
			}
		}

		public string[] Extensiones
		{
			get
			{
				string[] exts = { "jpg", "png", "doc", "docx", "xls", "xlsx", "pdf", "txt" };
				return exts;
			}
		}

		//Propiedades de auditoria
		public DateTime? FC
		{
			get { return DateTime.Now; }
		}

		public DateTime? FUA
		{
			get { return FC; }
		}

		public int? LlantaID
		{
			get
			{
				int? id = null;
				if (!string.IsNullOrEmpty(Session["LlantaIDEliminar"].ToString()))
					id = int.Parse(Session["LlantaIDEliminar"].ToString());
				return id;
			}
		}

		public List<TipoArchivoBO> TiposArchivos
		{
			get
			{
				return DocumentosEliminar.TiposArchivo ?? new List<TipoArchivoBO>();
			}
			set
			{
				DocumentosEliminar.TiposArchivo = value ?? new List<TipoArchivoBO>();
			}
		}

		public int? UC
		{
			get
			{
				int? uc = null;
				if (!string.IsNullOrEmpty(Session["UCEliminar"].ToString()))
					uc = int.Parse(Session["UCEliminar"].ToString());
				return uc;
			}
		}

		public LlantaBO UltimoObjetoLlanta
		{
			get
			{
				if ((LlantaBO)Session["UltimoObjetoLlanta"] == null)
					return new LlantaBO();
				else
					return (LlantaBO)Session["UltimoObjetoLlanta"];
			}
			set
			{
				Session["UltimoObjetoLlanta"] = value;
			}
		}

		public int? UUA
		{
			get { return UC; }
		}

		#region SC008

		public int? UnidadOperativaID
		{
			get
			{
				AdscripcionBO adscripcion= this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
				if (adscripcion == null) return null;
				return adscripcion.UnidadOperativa.Id;
			}
		}

		public int? UsuarioID
		{
			get
			{
				int? value = null;

				if (!string.IsNullOrEmpty(Session["UsuarioIDEliminar"].ToString()))
					value = int.Parse(Session["UsuarioIDEliminar"].ToString());

				return value;
			}
		}

		#endregion SC008

		public UsuarioBO Usuario
		{
			get
			{
				return this.Session["Usuario"] != null ? (UsuarioBO)this.Session["Usuario"] : null;
			}
		}

		public List<DatosConexionBO> ListadoDatosConexion
		{
			get
			{
				return this.Session["DatosConexion"] != null ? (List<DatosConexionBO>)this.Session["DatosConexion"] : null;
			}
		}

		private string Logueo
		{
			get { return ConfigurationManager.AppSettings["Logueo"]; }
		}

		#endregion Propiedades

		#region Eventos

		protected void btnConfirmar_Click(object sender, EventArgs e)
		{
			presentador.Baja();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			
			
			try
			{
				presentador = new EliminarLlantaPRE(this);

			if (this.Usuario != null && this.ListadoDatosConexion != null && this.ListadoDatosConexion.Count > 0)
			{
				if (!IsPostBack)
				{
					if (presentador.ValidarDatosSesion())
					{
						//Se valida el acceso a la vista
						this.presentador.ValidarAcceso(); //SC_0008
						presentador.InicializarControles();
					}else
					{
						RedirigirSinPermisoAcceso();
					}

				}

			}
			else
			{
				this.Response.Redirect(this.Logueo);
			}
			}
			catch (Exception ex)
			{
				this.MostrarMensaje("Inconsistencia al crear la página", ETipoMensajeIU.ERROR, "EliminarLlantaUI Page_Load" + ex.Message);
			}

			
		}

		#endregion Eventos
	}
}