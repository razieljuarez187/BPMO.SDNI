using System;
using System.Collections.Generic;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Comun.BO;

namespace BPMO.SDNI.Contratos.PSL.VIS {
    /// <summary>
    /// Vista que implementa la Interfaz que controla los PeriodoTarifarios
    /// </summary>
    public interface IucPeriodoTarifarioPSLVIS {
        #region Propiedades
        /// <summary>
        /// Unidad Operativa que se está configurando
        /// </summary>
        int? UnidadOperativaID { get; }
        /// <summary>
        /// Indica si la configuración incluye sábado y domingo
        /// </summary>
        bool? IncluyeSD { get; set; }
        /// <summary>
        /// Número de días que dura una semana
        /// </summary>
        int? DiasDuracionSemana { get; set; }
        /// <summary>
        /// Número de días que dura un mes
        /// </summary>
        int? DiasDuracionMes { get; set; }
        /// <summary>
        /// Día a partir del cual el período se considera diario. Siempre será 1.
        /// </summary>
        int? InicioPeriodoDia { get; set; }
        /// <summary>
        /// Día a partir del cual el período se considera semanal. Siempre será 1.
        /// </summary>
        int? InicioPeriodoSemana { get; set; }
        /// <summary>
        /// Día a partir del cual el período se considera mensual. Siempre será 1.
        /// </summary>
        int? InicioPeriodoMes { get; set; }
        /// <summary>
        /// Identificador del turno al cual se le están configurando las horas máximas que puede trabajar por período
        /// </summary>
        Enum TarifaTurno { get; set; }
        /// <summary>
        /// Identificador del turno seleccionado para agregar
        /// </summary>
        int? TurnoTarifaID { get; set; }
        /// <summary>
        /// Máximo horas que puede trabajar un equipo por Día.
        /// </summary>
        int? MaximoHorasDia { get; set; }
        /// <summary>
        /// Máximo horas que puede trabajar un equipo por Semana.
        /// </summary>
        int? MaximoHorasSemana { get; set; }
        /// <summary>
        /// Máximo horas que puede trabajar un equipo por Mes.
        /// </summary>
        int? MaximoHorasMes { get; set; }
        /// <summary>
        /// Identificador del usuario que ha iniciado sesion en el sistema
        /// </summary>
        int? UsuarioID { get; }
        /// <summary>
        /// Listado de horas turno tarifa que son configuradas para la unidad operativa
        /// </summary>
        List<DetalleHorasTurnoTarifaBO> listHorasTurno { get; set; }
        #endregion

        #region Métodos
        void LimpiarSesion();
        void LimpiarCamposHorasTurno();
        void ModoEdicion(bool activo);
        void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null);
        void PresentarHorasTurno(List<DetalleHorasTurnoTarifaBO> listHorasTurno);
        void EstablecerOpcionesTarifaTurno(Dictionary<string, string> turno);
        #endregion
    }
}