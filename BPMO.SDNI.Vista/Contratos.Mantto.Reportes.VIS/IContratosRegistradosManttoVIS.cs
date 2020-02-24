//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrados
//Satisface el CU021 – Reporte Contratos de Mantenimiento Registrados
using System;
using BPMO.SDNI.Reportes.VIS;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Contratos.Mantto.Reportes.VIS
{
    /// <summary>
    /// Interface que define la funcionalidad para una vista que generá reportes de mantenimiento de servicios de mantenimiento
    /// </summary>
    public interface IContratosRegistradosManttoVIS : IReportPageVIS
    {
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
        /// Obtiene o establece un valor que representa el identificador de sucursal
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? SucursalID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre de la sucursal
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String SucursalNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador de la cuenta cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? CuentaClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String ClienteNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 1 de inicio de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        DateTime? FechaInicioContrato1 { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 2 de inicio de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        DateTime? FechaInicioContrato2 { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 1 de fin de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        DateTime? FechaFinContrato1 { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la fecha 2 de fin de contrato
        /// </summary>
        /// <value>Objeto de tipo DateTime</value>
        DateTime? FechaFinContrato2 { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        int? Anio { get; set; }

        /// <summary>
        /// Obtiene un valor que establece el tipo de contrato que la vista esta procesando
        /// </summary>
        /// <value>Objeto de tipo ETipoContrato</value>
        ETipoContrato? TipoContrato { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String IdentificadorReporte { get; }
    }
}
