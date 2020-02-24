// Satisface al CU015 - Registrar Contrato Full Service Leasing
// Satisface al Reporte de Inconsistencia RI0008
// Construccion durante staffing - Configuracion de INPC
using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.FSL.BO;

namespace BPMO.SDNI.Contratos.FSL.VIS
{
    /// <summary>
    /// Interfaz con los metodos que debe implementar la vista
    /// </summary>
    public interface IRegistrarContratoFSLVIS
    {
        #region Propiedades
        /// <summary>
        /// Fecha de Creación
        /// </summary>
        DateTime? FC { get; }
        /// <summary>
        /// Fecha de Ultima Actualización
        /// </summary>
        DateTime? FUA { get; }
        /// <summary>
        /// Usuario de Creación
        /// </summary>
        int? UC { get; }
        /// <summary>
        /// Usuario de Ultima Actualización
        /// </summary>
        int? UUA { get;  }
        /// <summary>
        /// Unidad Operativa del Usuario Actual
        /// </summary>
        UnidadOperativaBO UnidadOperativa { get; }
        /// <summary>
        /// Clave
        /// </summary>
        string Clave { get; set; }
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

        #region SC_0008
        /// <summary>
        /// Identificador del Usuario Actual
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Identificador de la Unidad Operativa de la Adscripción
        /// </summary>
        int? UnidadAdscripcionID { get; }
        #endregion
        #endregion

        #region Metodos
        /// <summary>
        /// Muestra mensajes del sistema al usuario
        /// </summary>
        /// <param name="mensaje">Texto del Mensaje</param>
        /// <param name="tipo">Tipo de Mensaje</param>
        /// <param name="detalle">Detalle del Mensaje</param>
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        /// <summary>
        /// Despliega la Linea del Contrato
        /// </summary>
        void CambiarALinea();
        /// <summary>
        /// Despliega los detalles del contrato
        /// </summary>
        void CambiaAContrato();
        /// <summary>
        /// Cambia la Vista de la interfaz a la informacion del INPC
        /// </summary>
        void CambiarAINPC();
        /// <summary>
        /// Establece el paquete de navegación con el contrato a tranferir
        /// </summary>
        /// <param name="codigo">Código del Paquete de Navegación</param>
        /// <param name="contrato">Contrato a Transferir</param>
        void EstablecerPaqueteNavegacion(string codigo, ContratoFSLBO contrato);
        /// <summary>
        /// Redirige a la vista de Detalle del Contrato
        /// </summary>
        void IrADetalle();
        /// <summary>
        /// Redirige a la Consulta de Contratos
        /// </summary>
        void IrAConsultar();
        /// <summary>
        /// Carga los tipos de archivos permitidos por el sistema
        /// </summary>
        /// <param name="tipos"></param>
        void CargarTiposArchivos(List<TipoArchivoBO> tipos);
        #region SC_0008
        /// <summary>
        /// Redirige a la vista de "Sin permiso de Acceso"
        /// </summary>
        void RedirigirSinPermisoAcceso();
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
        #region RI0008
        /// <summary>
        /// Permite o bloquea al usuario la opcion de guardar contrato en curso
        /// </summary>
        /// <param name="b"></param>
        void PermitirGuardarEnCurso(bool b); 
        #endregion
        #endregion
    }
}
