// Satisface el CU023 – Reporte Porcentaje Utilización de Renta Diaria
// Satisface el CU025 – Reporte Porcentaje Utilización de Renta Diaria por Tipo de Unidad
// Satisface el CU026 – Reporte Porcentaje Utilización de RD Refrigerados
// Satisface el CU027 – Reporte Porcentaje Utilización de RD Refrigerados por Tipo

using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Contratos.RD.Reportes.VIS
{
    /// <summary>
    /// Interface que define la funcionalidad para una vista que generá un Reporte de Porcentaje Utilización de Renta Diaria
    /// </summary>
    public interface IPorcentajeUtilizacionRDVIS : IReportPageVIS
    {
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
        /// Obtiene o establece un valor que representa el año de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? AnioFechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesFechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? AnioFechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesFechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String IdentificadorReporte { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de equipo aliado que debe de tener las unidades a mostrar
        /// </summary>
        /// <value>Objeto de tipo ETipoEquipoAliado</value>
        ETipoEquipoAliado? TipoEquipoAliado { get; }
    }
}
