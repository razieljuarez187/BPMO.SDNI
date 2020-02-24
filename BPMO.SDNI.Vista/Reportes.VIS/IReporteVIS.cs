//Satisface el CU018 – Reporte Detallado de Renta Diaria por Sucursal
//Satisface al CU019 - Reporte de Flota Activa de RD Registrados

using System;
using System.Collections;
using BPMO.SDNI.Contratos.BO;

namespace BPMO.SDNI.Reportes.VIS
{
    /// <summary>
    /// Interface para un Masterpage de tipo reportes
    /// </summary>
    public interface IReporteVIS
    {
        /// <summary>
        /// Identificador de la Unidad Operativa
        /// </summary>
        int? UnidadOperativaID { get; }

        /// <summary>
        /// Identificador del Modulo de configuraciones
        /// </summary>
        int? ModuloID { get; }

        /// <summary>
        /// Identificador del Usuario
        /// </summary>
        int? UsuarioID { get; }

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
        /// Obtiene o establece un valor que determina si el filtro de modelo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool ModeloFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el filtro de modelo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool ModeloFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene el numero de serie de la unidad
        /// </summary>
        string NumeroSerie { get; set; }

        /// <summary>
        /// Obtiene o establece el ID de la unidad
        /// </summary>
        int? UnidadID { get; set; }

        /// <summary>
        /// Obtiene o estable si el filtro de unidad es visible
        /// </summary>
        bool UnidadFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece si se visualiza el requerido para elfiltro de unidad
        /// </summary>
        bool UnidadFiltroRequerido { get; set; }

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
        /// Obtiene o establece un valor que determina si el filtro de sucursal es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool SucursalFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de sucursal es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool SucursalFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente de Oracle
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? CuentaClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del cliente
        /// </summary>
        /// <value>Objeto de tipo Nullable de Int32</value>
        int? ClienteID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del cliente
        /// </summary>
        /// <value>Objeto de tipo String</value>
        String ClienteNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de cliente es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool ClienteFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de cliente es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool ClienteFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo Tecnico
        /// </summary>
        String TecnicoEtiqueta { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el identificador del Tecnico
        /// </summary>
        int? TecnicoID { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el nombre del tecnico para las que aplica la configuración
        /// </summary>
        string TecnicoNombre { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de tecnico es visible
        /// </summary>
        bool TecnicoFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el filtro de tecnico es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool TecnicoFiltroRequerido { get; set; }
       
        /// <summary>
        /// Obtiene o establece un valor que representa el departamento o área seleccionada
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        ETipoContrato? Departamento { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool DepartamentoFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool DepartamentoFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa la etiqueta del campo departamento
        /// </summary>
        /// <value>Objeto de tipo String</value>
        string AreaUnidadEtiqueta { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el departamento o área seleccionada
        /// </summary>
        /// <value>Valor de tipo EArea</value>
        EArea? AreaUnidad { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool AreaUnidadFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de departamento es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool AreaUnidadFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el año
        /// </summary>
        /// <value>Valor de tipo ETipoContrato</value>
        int? Anio { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de año es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool AnioFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de año es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool AnioFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el tipo de periodo que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? TipoReporte { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el periodo que se aplicará al reporte
        /// </summary>
        int? PeriodoReporte { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool PeriodoReporteFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool PeriodoReporteFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que representa el mes que se aplicará al reporte
        /// </summary>
        /// <value>Objeto de tipo Int32</value>
        int? Mes { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool MesFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool MesFiltroRequerido { get; set; }

        /// <summary>
        /// Obtiene o eatablce u valor que representa el día de corte
        /// </summary>
        /// <value>Objeto de tipo Día</value>
        int? DiaCorte { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool DiaCorteFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de periodo es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool DiaCorteFiltroRequerido { get; set; }

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
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio de contrato es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaInicioContratoFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio de contrato es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaInicioContratoFiltroRequerido { get; set; }

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
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin de contrato es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaFinContratoFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin de contrato es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaFinContratoFiltroRequerido { get; set; }

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
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaInicioFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de inicio es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaInicioFiltroRequerido { get; set; }

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
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin es visible
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaFinFiltroVisible { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que determina si el filtro de la fecha de fin es requerido
        /// </summary>
        /// <value>Objeto de tipo Boolean</value>
        bool FechaFinFiltroRequerido { get; set; }

        /// <summary>
        /// Liga las áreas departamentos al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindAreasDepartamentos(ICollection items);

        /// <summary>
        /// Liga las áreas departamentos al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindAreasUnidad(ICollection items);

        /// <summary>
        /// liga los tipos reportes al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindTiposReportes(ICollection items);
      
        /// <summary>
        /// Liga los periodos de reportes al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        /// <param name="isEmpty">Indica si elementos representan una colección vacia</param>
        void BindPeriodosReportes(ICollection items, bool isEmpty);

        /// <summary>
        /// Liga los años del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindAnios(ICollection items);
      
        /// <summary>
        /// Liga los meses del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindMeses(ICollection items);
      
        /// <summary>
        /// Liga los años de fecha de inicio del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindAniosFechaInicio(ICollection items);
      
        /// <summary>
        /// Liga los meses de fecha de inicio del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindMesesFechaInicio(ICollection items);

        /// <summary>
        /// Liga los años de fecha de fin del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindAniosFechaFin(ICollection items);
       
        /// <summary>
        /// Liga los meses de fecha de fin del reporte al control asignado
        /// </summary>
        /// <param name="items">Elemento con que se va a ligar el control</param>
        void BindMesesFechaFin(ICollection items);
       
        /// <summary>
        /// Este método despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipo">Tipo de mensaje a desplegar</param>
        /// <param name="detalle">Muestra una cadena de texto que contiene los datalles del mensaje a desplegar</param>
        void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipo, string detalle = null);
    }
}
