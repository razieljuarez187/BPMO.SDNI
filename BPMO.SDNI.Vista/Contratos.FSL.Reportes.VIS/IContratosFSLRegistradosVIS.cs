// Satisface al caso de uso CU020 - Reporte de Contratos FSL Registrados
using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Contratos.FSL.Reportes.VIS
{
    /// <summary>
    /// Interface que define la funcionalidad para una vista que generá reportes de contratos full service leasing registrados
    /// </summary>
    public interface IContratosFSLRegistradosVIS : IReportPageVIS
    {
        #region Propiedades
        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del ´modelo
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ModeloID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del modelo
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String ModeloNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Identificador de la Sucursal
        /// </summary>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre de la Sucursal
        /// </summary>
        string SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        int? Anio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Identificador de la Cuenta del Cliente
        /// </summary>
        int? CuentaClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el Nombre de la Cuenta del Cliente
        /// </summary>
        string CuentaClienteNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor Fecha de Inicio del Contrato (Rango Fechas - Inicio)
        /// </summary>
        DateTime? FechaInicioContratoInicial { get; set; }

        /// <summary>
        /// Obtiene o establece un valor Fecha de Inicio del Contrato (Rango Fechas - Fin)
        /// </summary>
        DateTime? FechaInicioContratoFinal { get; set; }

        /// <summary>
        /// Fecha de Fin del Contrato (Rango Fechas - Inicio)
        /// </summary>
        DateTime? FechaFinContratoInicial { get; set; }

        /// <summary>
        /// Obtiene o establece un valorFecha de Fin del Contrato (Rango Fechas - Fin)
        /// </summary>
        DateTime? FechaFinContratoFinal { get; set; }

        /// <summary>
        /// Codigo de Navegación del Reporte
        /// </summary>
        string CodigoReporte { get; } 
        #endregion
    }
}
