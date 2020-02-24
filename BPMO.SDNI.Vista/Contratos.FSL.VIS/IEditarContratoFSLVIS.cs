//Satisface al Caso de Uso CU023 - Editar Contrato Full Service Leasing
//Satisface al Caso de uso CU026 - Registrar Terminación de Contrato
//Satisface al Reporte de Inconsistencia RI0008
using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.FSL.BO;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    /// <summary>
    /// Interfaz con los metodos que implementara la Vista de Edición de Contrato
    /// </summary>
    public interface IEditarContratoFSLVIS
    {
        #region Propiedades
        /// <summary>
        /// Codigo de Navegación
        /// </summary>
        string Codigo { get; }
        /// <summary>
        /// Codigo de Navegación del Último Objeto en Sesión
        /// </summary>
        string CodigoUltimoObjeto { get; set; }
        /// <summary>
        /// Ultimo contrato consultado
        /// </summary>
        ContratoFSLBO UltimoObjeto { get; set; }
        /// <summary>
        /// Identificador del Contrato
        /// </summary>
        int? ContratoID { get; set; }
        /// <summary>
        /// Estatus del Contrato Actual
        /// </summary>
        EEstatusContrato? Estatus { get; set; }
        /// <summary>
        /// Usuario de Creación
        /// </summary>
        int? UC { get; set; }
        /// <summary>
        /// Fecha de Creación
        /// </summary>
        DateTime? FC { get; set; }
        /// <summary>
        /// Usuario de Ultima Actualización
        /// </summary>
        int? UUA { get; }
        /// <summary>
        /// Fecha de Última Actualización
        /// </summary>
        DateTime? FUA { get; }
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        int? UnidadOperativaContratoID { get; set; }
        /// <summary>
        /// Motivo del Cierre del Contrato
        /// </summary>
        string MotivoCierre { get; set; }
        #region SC_0008
        /// <summary>
        /// Identificador del Usuario Actual
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Identificador de la Unidad Operativa de la Adscripción del Usuario Actual
        /// </summary>
        int? UnidadAdscripcionID { get; }
        #endregion
        #region CU026
        /// <summary>
        /// Fecha de Cierre del Contrato
        /// </summary>
        DateTime? FechaCierre { get; set; }
        /// <summary>
        /// Usuario que Cierra el Contrato
        /// </summary>
        int? UsuarioCierre { get; set; }
        /// <summary>
        /// Observaciones al Cierre del Contrato
        /// </summary>
        string ObservacionesCierre { get; set; }
        /// <summary>
        /// Monto de la Penalización
        /// </summary>
        decimal? CantidadPenalizacion { get; set; }

        #endregion
        /// <summary>
        /// Determina si se cobrara INPC fijo o elegido por el usuario
        /// </summary>
        bool? InpcFijo { get; set; }
        /// <summary>
        /// Fecha en la que inicia el Cobro del INPC
        /// </summary>
        DateTime? FechaInicioInpc { get; set; }
        /// <summary>
        /// Valor del INPC Fijo que se cobrará
        /// </summary>
        decimal? ValorInpc { get; set; }
        /// <summary>
        /// Inpc Configurado para el Contrato
        /// </summary>
        INPCContratoBO InpcContrato { get; set; }
        #endregion

        #region Metodos
        /// <summary>
        /// Limpia los datos en la memoria de la vista
        /// </summary>
        void LimpiarSesion();
        /// <summary>
        /// Despliega la Vista de Edición del Contrato
        /// </summary>
        void CambiaAContrato();
        /// <summary>
        /// Despliega la Vista de Edición de Linea de Contrato
        /// </summary>
        void CambiarALinea();
        /// <summary>
        /// Cambia la Vista de la interfaz a la informacion del INPC
        /// </summary>
        void CambiarAINPC();
        /// <summary>
        /// Regresa a la Consulta de Contratos
        /// </summary>
        void RegresarAConsultar();
        /// <summary>
        /// Despliega un mensaje en la vista
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo del mensaje que es desplegado</param>
        /// <param name="detalle">Detalle del mensaje desplegado</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Carga los tipos de archivos permitidos por el sistema
        /// </summary>
        /// <param name="tipos"></param>
        void CargarTiposArchivos(List<TipoArchivoBO> tipos);
        /// <summary>
        /// Redirige a la vista de Detalle del Contrato
        /// </summary>
        void IrADetalleContrato();
        /// <summary>
        /// Establece el paquete de navegación con el contrato a tranferir
        /// </summary>
        /// <param name="Clave">Código del Paquete de Navegación</param>
        /// <param name="contrato">Contrato a Transferir</param>
        void EstablecerPaqueteNavegacion(string Clave, ContratoBO contrato);
        #region SC_0008
        /// <summary>
        /// Redirige a la vista de "Sin permiso de Acceso"
        /// </summary>
        void RedirigirSinPermisoAcceso();
        /// <summary>
        /// Permite Redirigir a la Vista de Registro de Contrato
        /// </summary>
        /// <param name="p">Indica si se permite o bloquea la opcion Registrar Contratro</param>
        void PermitirRegistrar(bool p);
        #endregion
        #region SC0038
        /// <summary>
        /// Obtiene las Plantilla de Contrato FSL
        /// </summary>
        /// <param name="key">Clave de la Plantilla</param>
        /// <returns></returns>
        List<object> ObtenerPlantillas(string key);
        #endregion

        #region RI0008
        /// <summary>
        /// Permite o bloquea al usuario la opcion de guardar contrato en curso
        /// </summary>
        /// <param name="b"></param>
        void PermitirGuardarEnCurso(bool b);
        #endregion

        /// <summary>
        /// Años Configurados para el Contrato.
        /// </summary>
        /// <param name="listaAnios">Lista de Años configurados</param>
        void PresentarAniosConfigurados(Dictionary<String, String> listaAnios);
        /// <summary>
        /// Activa o desactiva los controles de seleccion de INPC
        /// </summary>
        /// <param name="permitir">Determina el estado de los controles</param>
        void PermitirControlesINPC(bool permitir);
        /// <summary>
        /// Activa o desactiva el control de Valor de INPC
        /// </summary>
        /// <param name="permitir">Determina el estado de los controles</param>
        void PermitirValorINPC(bool permitir);
        #endregion
    }
}
