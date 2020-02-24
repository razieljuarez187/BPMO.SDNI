//Satisface al CU010 - Catálogo de Documentos
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Comun.VIS
{
	public interface IucCatalogoDocumentosVIS
	{
		#region Propiedades

        string Identificador { get; set; }
        bool? ModoEdicion { get; set; }
        ETipoAdjunto? TipoAdjunto { get; set; }
        List<TipoArchivoBO> TiposArchivo { get; set; }
        bool? TieneObservaciones { get; set; }

        List<ArchivoBO> NuevosArchivos { get; set; }
        List<ArchivoBO> OriginalesArchivos { get; set; }        

		#endregion Propiedades

		#region Métodos

		void AgregarArchivo(ArchivoBO archivoBO);

		void DespliegaArchivos();

		void EliminaArchivo(ArchivoBO archivoBO);

		void EstablecerListasArchivos(List<ArchivoBO> listaArchivos, List<TipoArchivoBO> listaTiposArchivos);

		void EstablecerModoEdicion(bool? edicion);

		void EstablecerRequiereObservaciones(bool? tieneObservaciones);

		List<ArchivoBO> ObtenerArchivos();

		byte[] ObtenerArchivosBytes();

		string ObtenerExtension();

		string ObtenerNombreArchivo();

		string ObtenerObservaciones();

		void InicializarControl(List<ArchivoBO> listaArchivos, List<TipoArchivoBO> listaTiposArchivos);

		void LimpiaCampos();

        bool ValidaArchivo();

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);

        void ReiniciarGrid();

        void OcultarCarga();


		#endregion Métodos
	}
}