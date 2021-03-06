﻿using System;
using BPMO.SDNI.Reportes.VIS;

namespace BPMO.SDNI.Flota.Reportes.VIS {
    public interface IFlotaActivaVIS : IReportPageVIS {
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
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        int? Anio { get; set; }
    }
}
