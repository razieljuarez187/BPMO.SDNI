//Satisface al CU077 - Registrar Acta de Nacimiento de una Unidad
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using System.Web.UI.WebControls;
using BPMO.SDNI.Equipos.BO;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.VIS;

namespace BPMO.SDNI.Equipos.VIS
{
    public interface IucDatosGeneralesVIS
    {
        int? UsuarioAutenticado { get; }
        string Propietario { get; set; }
        int? PropietarioId { get; set; }
        string Cliente { get; set; }
        int? ClienteId { get; set; }
        int? UnidadOperativaId { get; }
        string SucursalNombre { get; set; }
        int? SucursalId { get; set; }
        EArea? Area { get; set; }
        string Fabricante { get; set; }
        string NombreClienteUnidadOperativa { get; set; }
        /// <summary>
        /// Determina si la unidad se encuentra o no bloqueda en el sistema Lider
        /// </summary>
        bool? UnidadBloqueada { get; set; }

        bool? EntraMantenimiento { get; set; }

        #region [RQM 13285- Integración Construcción y Generacción]

        string OrdenCompraProveedor { get; set; }
        decimal? MontoArrendamiento { get; set; }
        string CodigoMoneda { get; set; }
        DateTime? FechaInicioArrendamiento { get; set; }
        DateTime? FechaFinArrendamiento { get; set; }
        int? ProveedorID { get; set; }
        EAreaConstruccion? ETipoRentaConstruccion { get; set; }
        EAreaGeneracion? ETipoRentaGeneracion { get; set; }
        EAreaEquinova? ETipoRentaEquinova { get; set; }
        ETipoEmpresa EnumTipoEmpresa { set; get; }
        DateTime? FC { get; set; }
        int? UC { get; set; }
        List<CatalogoBaseBO> ListaAcciones { get; set; }
        List<ArchivoBO> ArchivosOC { get; set; }
        List<TipoArchivoBO> TiposArchivo { get; set; }
        IucCatalogoDocumentosVIS VistaDocumentos { get; }
        bool ArrendamientoNuevo { get; set; }

        #endregion


        void PrepararNuevo();
        void CargarAreas(Dictionary<string, object> areas);

        void HabilitarModoEdicion(bool habilitar);
        void HabilitarPropietario(bool habilitar);
        void HabilitarCliente(bool habilitar);
        void HabilitarArea(bool habilitar);
        void HabilitarSucursal(bool habilitar);

        /// <summary>
        /// Se usa para activar o desactivar el campo de Bloqueo de la Unidad
        /// </summary>
        /// <param name="habilitar">Determina si se habilita o no el campo</param>
        void HabilitarUnidadBloqueada(bool habilitar);
        void MostrarFabricante(bool mostrar);

        void LimpiarSesion();

        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null);

        void HabilitarEntraMantenimiento(bool habilitar);
        
        
        void EstablecerEntraMantenimeinto();


        #region [RQM 13285- Integración Construcción y Generacción]
 
        void ActivaCheckMantenimiento(bool habilitar);
        void HabilitaMontoArrendamiento(bool habilitar);
        void HabilitaFechasArrendamiento(bool habilitar);
        void ModificaEtiquetaPropietario(bool modifica);
        void MostrarTipoRentaEmpresas(bool mostrar);
        void HabilitaTipoRentaEmpresas(bool habilitar);
        void MostrarMontoArrendamiento(bool mostrar);
        void MostrarOrdenCompra(bool mostrar);
        void MostrarTipoMoneda(bool mostrar);
        void MostrarAreas(bool mostrar);
        void ReiniciaMontoArrendamiento();
        void HabilitaTipoMonedas(bool habilitar);
        void CargarTipoRentaConstruccion(Dictionary<string, object> tipoRentaC);
        void CargarTipoRentaGeneracion(Dictionary<string, object> tipoRentaG);
        void CargarTipoRentaEquinova(Dictionary<string, object> tipoRentaEqu);
        void EstablecerAcciones(string modo);
        void HabilitarControles(bool mostrar, string modo, ETipoEmpresa empresa);
        void EstablecerOpcionesMoneda(Dictionary<string, string> monedas);
        void HabilitaArea(bool habilitar);
        void CargarTiposArchivos(List<TipoArchivoBO> tipos);
        void HabilitarCargaArchivoOC(bool habilitar);
        void HabilitaControlesOC(bool habilitar);
        void ReiniciarGridOC();
        void EstablecerTipoAdjunto(ETipoAdjunto tipo);
        void EstablecerIdentificadorListaArchivos(string identificador);
        void OcultarControlesRE(bool habilitar);
        void MostrarBotonAgregarFechas(bool habilitar);
        void ModoEdicion(bool Habilitar);
        #endregion
    }
}
