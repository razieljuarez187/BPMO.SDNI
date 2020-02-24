//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
//Satisface al CU079 - Consultar Acta de Nacimiento de Unidad
//Satisface la solicitud de cambio SC0006

using System;

using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Equipos.BO;
using System.Collections.Generic;
using BPMO.Basicos.BO;

namespace BPMO.SDNI.Equipos.VIS
{
	public interface IucResumenActaNacimientoVIS
    {
        int? EquipoID { get; set; }
        int? UnidadID { get; set; }
        string NumeroSerie { set; }
        string SucursalNombre { set; }
        EEstatusUnidad? EstatusUnidad { get; set; }
        string Area { set; }
        DateTime? FC { set; }
        DateTime? FUA { set; }
        int? UC { get; set; }
        int? UUA { get; set; }
        string UsuarioCreacion { set; }
        string UsuarioModificacion { set; }
        
        void PrepararNuevo();
        void PrepararVisualizacion();

        void MostrarDatosRegistro(bool mostrar);
        void MostrarDatosActualizacion(bool mostrar);
        void MostrarDatosSiniestro(List<SiniestroUnidadBO> historial);

        void PermitirConfigurarMantenimientos(bool permitir);
        void PermitirRegistrarMantenimientos(bool permitir);

		void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        #region RQM 14150
        void EstablecerAcciones();
        string ConfigurarEtiquetaPrincipal(string cEtiquetaBuscar);
        AdscripcionBO Adscripcion { get; set; }
        #endregion
    }
}