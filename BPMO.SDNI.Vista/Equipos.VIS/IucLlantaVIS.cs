// Satisface al CU089 - Bitácora de Llantas
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Equipos.VIS
{
	public interface IucLlantaVIS
	{
		#region Propiedades

        int? LlantaID { get; set; }
        string Codigo { get; set; }
        string Marca { get; set; }
        string Modelo { get; set; }
		string Medida { get; set; }
		decimal? Profundidad { get; set; }
        bool? Revitalizada { get; set; }
        bool? Stock { get; set; }
        int? EnllantableID { get; set; }        
        string DescripcionEnllantable { get; set; }
        int? TipoEnllantable { get; set; }
        int? Posicion { get; set; }
        bool? EsRefaccion { get; set; }

        List<ArchivoBO> ArchivosAdjuntos { get; set; }

        int? UC { get; set; }
        int? UUA { get; set; }
        string UsuarioCreacion { set; }
        string UsuarioEdicion { set; }
        DateTime? FC { get; set; }
        DateTime? FUA { get; set; }

        bool? Activo { get; set; }

        bool BuscarSoloActivos { get; }
        bool BuscarSoloStock { get; }

        IucCatalogoDocumentosVIS VistaDocumentos { get; }

        int? SucursalEnllantableID { get; set; }
        int? SucursalID { get; set; }
        string SucursalNombre { get; set; }
        int? UnidadOperativaID { get; }
        int? UsuarioAutenticado { get; }

        #endregion Propiedades

        #region Metodos

        void PrepararNuevo();
        void PrepararEdicion();
        void PrepararVisualizacion();

        void HabilitarModoEdicion(bool habilitar);
        void HabilitarProfundidad(bool habilitar);
        void HabilitarPosicion(bool habilitar);
        void HabilitarCodigo(bool habilitar);
        void HabilitarSucursal(bool habilitar);

        void PermitirBusquedaCodigo(bool permitir, bool soloActivos, bool soloStock);

        void MostrarStock(bool mostrar);
        void MostrarEnllantable(bool mostrar);
        void MostrarPosicion(bool mostrar);
        void MostrarActiva(bool mostrar);
        void MostrarDatosRegistro(bool mostrar);
        void MostrarDatosActualizacion(bool mostrar);

        void OcultarDocumentos(bool ocultar);
        
		void EstablecerTiposArchivo(List<TipoArchivoBO> tiposArchivo);

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

		#endregion
	}
}