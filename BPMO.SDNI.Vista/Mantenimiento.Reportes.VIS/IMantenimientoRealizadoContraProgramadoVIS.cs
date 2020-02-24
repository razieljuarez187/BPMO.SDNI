//Satisface al CU068 - Mantenimiento Realizado Contra Programado

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.SDNI.Reportes.VIS;
using System.Collections;

namespace BPMO.SDNI.Mantenimiento.Reportes.VIS
{
    /// <summary>
    /// Interface para el control de la vista
    /// </summary>
    public interface IMantenimientoRealizadoContraProgramadoVIS : IReportPageVIS
    {
        #region Propiedades
        
        string NumeroVIN { get; set; }
        string LibroActivos { get; set; }
        int? UnidadOperativaId { get; }

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
        /// Obtiene o establece un valor que representa el año de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? Anio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de inicio
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesInicio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes de la fecha de fin
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? MesFin { get; set; }

        /// <summary>
        /// Obtiene el vin de la unidad
        /// </summary>
        String Vin { get; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del reporte a visualizar
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String IdentificadorReporte { get; }

        /// <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindMeses(ICollection items);

        #endregion

        #region Métodos
        void LimpiarSesion();

        void PrepararBusqueda();
        #endregion
    }
}
