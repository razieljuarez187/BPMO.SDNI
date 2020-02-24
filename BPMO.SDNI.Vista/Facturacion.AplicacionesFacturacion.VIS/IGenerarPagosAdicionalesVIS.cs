using System.Collections;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.VIS
{
    /// <summary>
    /// Vista que determina que propiedades y metodos necesita una UI para poder generar el pago adicional de un contrato.
    /// </summary>
    public interface IGenerarPagosAdicionalesVIS
    {
        /// <summary>
        /// Folio del Contrato
        /// </summary>
        string FolioContrato { get; set; }
        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        int? ContratoID { get; set; }
        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        int? SucursalID { get; set; }
        /// <summary>
        /// Nombre de la sucursal
        /// </summary>
        //string SucursalNombre { get; set; }
        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Tipo del Contrato
        /// </summary>
        ETipoContrato? TipoContrato { get; set; }
        /// <summary>
        /// Inicializa las propiedades de la interfaz
        /// </summary>
        void Inicializar();
        /// <summary>
        /// Carga las sucursales en la UI
        /// </summary>
        /// <param name="coleccion">Conjunto de Elementos que se presentaran en la UI</param>
        void CargarSucursales(ICollection coleccion);
        /// <summary>
        /// Carga los Departamentos en la UI
        /// </summary>
        /// <param name="coleccion">Conjunto de Elementos que se presentaran en la UI</param>
        void CargarDepartamentos(ICollection coleccion);
        /// <summary>
        /// Limpia elementos de la session
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Valida los campos requeridos de la interfaz de usuario
        /// </summary>
        void ValidarCampos();
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Realiza la redirección al visor correspondiente cuando se intenta acceder a la vista actual sin tener permisos asignados
        /// </summary>
        void RedirigirSinPermisoAcceso();
    }
}
