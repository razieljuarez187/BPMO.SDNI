using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using BPMO.SDNI.Contratos.PSL.Reportes.DA;
using BPMO.SDNI.Flota.BO;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.PSL.RPT {
    /// <summary>
    /// Plantilla de Reporte detallado de Renta por sucursal
    /// </summary>
    public partial class DetalladoPSLSucursalRPT : DevExpress.XtraReports.UI.XtraReport {
        /// <summary>
        /// Tamaño de la celda base que se divide en el número de días del mes a visualizar
        /// </summary>
        private double diasMesEncabezadosSize;

        /// <summary>
        /// Diccionario con todas sumatorias para realizar calculos de promedios y sumatorias
        /// </summary>
        private Dictionary<String, int> _sumatorias;

        /// <summary>
        /// Obtiene o establece un valor que representa un Diccionario con todas sumatorias para realizar calculos de promedios y sumatorias
        /// </summary>
        /// <value>
        /// Objeto de tipo Dictionary<String, int>
        /// </value>
        public Dictionary<String, int> Sumatorias {
            get {
                if (this._sumatorias == null)
                    this._sumatorias = new Dictionary<String, int>();

                return this._sumatorias;
            }
        }

        /// <summary>
        /// Constructor por default que recibe el diccionario con los datos para mostrar el reporte
        /// </summary>
        /// <param name="datos">Diccionario de Datos</param>
        public DetalladoPSLSucursalRPT(Dictionary<string, object> datos) : this() {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            if (!datos.ContainsKey("Mes"))
                throw new ArgumentNullException("datos.Mes");

            if (!datos.ContainsKey("Anio"))
                throw new ArgumentNullException("datos.Anio");

            this.DataSource = datos["DataSource"];
            this.Parameters["Mes"].Value = datos["Mes"];
            this.Parameters["Anio"].Value = datos["Anio"];
        }

        /// <summary>
        /// Contructor por default
        /// </summary>
        public DetalladoPSLSucursalRPT() {
            this.InitializeComponent();
            this.diasMesEncabezadosSize = this.xrtCellDiasMes.WidthF;
        }

        /// <summary>
        /// Realiza un conteo de unidades sobre una fila de registro de estatus de historial de una unidad
        /// </summary>
        private int CountFromHistorial() {
            return this.CountFromHistorial(null, null, null);
        }

        /// <summary>
        /// Realiza un conteo de unidades sobre una fila de registro de estatus de historial de una unidad
        /// </summary>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        private int CountFromHistorial(EEstatusHistorial? estatusToCompare) {
            return this.CountFromHistorial(null, estatusToCompare, null);
        }

        /// <summary>
        /// Realiza un conteo de unidades sobre una fila de registro de estatus de historial de una unidad
        /// </summary>
        /// <param name="source">Fila de datos a evaluar</param>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <returns>Conteo de unidades tomando como base su estatus de historial</returns>
        private int CountFromHistorial(ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source, EEstatusHistorial? estatusToCompare) {
            return this.CountFromHistorial(source, estatusToCompare, null);
        }

        /// <summary>
        /// Realiza un conteo de unidades sobre una fila de registro de estatus de historial de una unidad
        /// </summary>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <param name="dayOfMonth">Día del mes a procesar, si se omite se procesa todo el mes</param>
        /// <returns>Conteo de unidades tomando como base su estatus de historial</returns>
        private int CountFromHistorial(EEstatusHistorial? estatusToCompare, int? dayOfMonth) {
            return this.CountFromHistorial(null, estatusToCompare, dayOfMonth);
        }

        /// <summary>
        /// Realiza un conteo de unidades sobre una fila de registro de estatus de historial de una unidad
        /// </summary>
        /// <param name="source">Fila de datos a evaluar</param>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <param name="dayOfMonth">Día del mes a procesar, si se omite se procesa todo el mes</param>
        /// <returns>Conteo de unidades tomando como base su estatus de historial</returns>
        private int CountFromHistorial(ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source, EEstatusHistorial? estatusToCompare, int? dayOfMonth) {
            int result = 0;

            int limit1 = 0;
            int limit2 = 0;
            if (dayOfMonth.HasValue) {
                limit1 = dayOfMonth.Value;
                limit2 = dayOfMonth.Value;
            } else {
                limit1 = 1;
                limit2 = this.GetDaysInMonth();
            }

            if (source == null) {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow;
                if (source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow)));
            }

            for (int i = limit1; i <= limit2; i++) {
                String index = String.Format("EstatusHistorial_{0}", i);
                if (source.IsNull(index))
                    continue;

                EEstatusHistorial? estatus = (EEstatusHistorial)Enum.ToObject(typeof(EEstatusHistorial), (int)source[index]);
                if (estatusToCompare.HasValue) {
                    if (estatus == estatusToCompare)
                        result++;
                } else {
                    if (estatus != EEstatusHistorial.FueraFlota)
                        result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Determina si la unidad sigue en la flota de la sucursal durante el transcurso del mes
        /// </summary>
        /// <param name="source">Fuente de Datos de la Unidad</param>
        /// <returns>Es TRUE si aun sigue en la flota de la sucursal</returns>
        private bool EnFlota(ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source) {
            bool enFlota = true;

            int limit1 = 1;
            int limit2 = this.GetDaysInMonth();

            if (source == null) {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow;
                if (source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow)));
            }

            for (int i = limit1; i <= limit2; i++) {
                String index = String.Format("EstatusHistorial_{0}", i);
                if (source.IsNull(index) && i == 1) {
                    return enFlota;
                } else {
                    if (source.IsNull(index) && i > 1) {
                        EEstatusHistorial? estatusAnterior = (EEstatusHistorial)Enum.ToObject(typeof(EEstatusHistorial), (int)source[String.Format("EstatusHistorial_{0}", i - 1)]);
                        return estatusAnterior != EEstatusHistorial.FueraFlota;
                    }
                }
            }

            return enFlota;
        }

        /// <summary>
        /// Obtiene el numero de días del mes en curso que se esta procesando
        /// </summary>
        /// <returns>Objeto de tipo Int32 con el número total de días del mes</returns>
        private int GetDaysInMonth() {
            int month = (int)this.Parameters["Mes"].Value;
            int year = (int)this.Parameters["Anio"].Value;

            int monthDays = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDaysInMonth(year, month);
            return monthDays;
        }

        /// <summary>
        /// Crea las nuevas columnas tomando como base el número de días del mes en curso
        /// </summary>
        private void CreateNewColumns() {
            if (this.Parameters["Mes"] == null || this.Parameters["Anio"] == null)
                throw new Exception("El Mes y el Año son parámetros requeridos para el reporte");

            int month = (int)this.Parameters["Mes"].Value;
            int year = (int)this.Parameters["Anio"].Value;

            int monthDays = this.GetDaysInMonth();

            for (int i = 0; i < monthDays - 1; i++)
                this.CreateNewColumn(i + 1, month, year, monthDays, true);

            this.CreateNewColumn(monthDays, month, year, monthDays, false);
        }

        /// <summary>
        /// Crea una nueva columna de un día especifico del mes
        /// </summary>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="month">Mes en curso</param>
        /// <param name="year">Año en curso</param>
        /// <param name="monthDays">Total de días del mes</param>
        /// <param name="isNewColumn">Indica si se va a contruir una nueva columna o se va a usar la columna que se tiene de base</param>
        private void CreateNewColumn(int dayOfMonth, int month, int year, int monthDays, bool isNewColumn) {
            XRTableCell[] newDayOfMonthHeader = new XRTableCell[] { this.xrtCellDaysOfMonth };
            XRTableCell[] newHeaderCell = new XRTableCell[] { this.xrtCellDiasMesEncabezados };
            XRTableCell[] newDataCell = new XRTableCell[] { this.xrtCellDiasMes };
            XRTableCell[] newSumRentadosCell = new XRTableCell[] { this.xrtCellTotalRentados };
            XRTableCell[] newTotalCell = new XRTableCell[] { this.xrtCellTotalFlota };
            XRTableCell[] newSumRentados2Cell = new XRTableCell[] { this.xrtCellTotalRentados2 };
            XRTableCell[] newSumDisponiblesCell = new XRTableCell[] { this.xrtCellTotalDisponibles };
            XRTableCell[] newSumTallerCell = new XRTableCell[] { this.xrtCellTotalTaller };
            XRTableCell[] newSumPorcentajeUtilizacion = new XRTableCell[] { this.xrtCellTotalPorcentajeUtilizacion };
            XRTableCell[] newSumFueraFlotaCell = new XRTableCell[] { this.xrtCellTotalFueraFlota };

            if (isNewColumn) {
                newDayOfMonthHeader = this.xrtblDaysOfMonth.InsertColumnToLeft(this.xrtCellDaysOfMonth);
                newHeaderCell = this.xrtblHeader.InsertColumnToLeft(this.xrtCellDiasMesEncabezados);
                newDataCell = this.xrtblRows.InsertColumnToLeft(this.xrtCellDiasMes);
                newSumRentadosCell = this.xrtblTotalRentados.InsertColumnToLeft(this.xrtCellTotalRentados);

                newTotalCell = this.xrtblTotales.InsertColumnToLeft(this.xrtCellTotalFlota);

                //Debido a que la tabla esta construida por varias columnas se extraen las filas

                newSumRentados2Cell = new XRTableCell[] { newTotalCell[1] };
                newSumDisponiblesCell = new XRTableCell[] { newTotalCell[2] };
                newSumTallerCell = new XRTableCell[] { newTotalCell[3] };
                newSumFueraFlotaCell = new XRTableCell[] { newTotalCell[4] };
                newSumPorcentajeUtilizacion = new XRTableCell[] { newTotalCell[5] };
                newTotalCell = new XRTableCell[] { newTotalCell[0] };
            }

            double size = this.diasMesEncabezadosSize / monthDays;

            foreach (XRTableCell cell in newDayOfMonthHeader)
                this.InitializeDayOfMonthCell(cell, size, dayOfMonth, month, year, monthDays);

            foreach (XRTableCell cell in newHeaderCell)
                this.InitializeHeaderCell(cell, size, dayOfMonth, monthDays);

            foreach (XRTableCell cell in newDataCell)
                this.InitializeDataCell(cell, size, dayOfMonth, month, year, monthDays);

            foreach (XRTableCell cell in newSumRentadosCell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, EEstatusHistorial.Rentado, 1);

            foreach (XRTableCell cell in newTotalCell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, null, 1);

            foreach (XRTableCell cell in newSumRentados2Cell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, EEstatusHistorial.Rentado, 2);

            foreach (XRTableCell cell in newSumDisponiblesCell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, EEstatusHistorial.Disponible, 1);

            foreach (XRTableCell cell in newSumTallerCell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, EEstatusHistorial.EnTaller, 1);

            foreach (XRTableCell cell in newSumFueraFlotaCell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, EEstatusHistorial.FueraFlota, 1);

            foreach (XRTableCell cell in newSumPorcentajeUtilizacion)
                this.InitializePorcentajeUtilizacionCell(cell, size, dayOfMonth, month, year, monthDays);
        }

        /// <summary>
        /// Prepara una celda que indica el día de la semana
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="month">Mes en curso</param>
        /// <param name="year">Año en curso</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeDayOfMonthCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays) {
            cell.Name = String.Format("xrtDayOfMonthCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(new DateTime(year, month, dayOfMonth).DayOfWeek).Substring(0, 2).ToUpper();
        }

        /// <summary>
        /// Prepara una celda que indica el estatus de la unidad de un día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="month">Mes en curso</param>
        /// <param name="year">Año en curso</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeDataCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays) {
            CalculatedField calculatedField = new CalculatedField();
            calculatedField.DataMember = "ConsultarDetalladoPSLSucursal";
            calculatedField.Name = String.Format("EstatusHistorialLetra_{0}", dayOfMonth);
            calculatedField.Expression = String.Format("Iif(IsNull([EstatusHistorial_{0}]), '', Iif([EstatusHistorial_{0}]!= 1, Iif([EstatusHistorial_{0}]!= 2,  Iif([EstatusHistorial_{0}]==3, \'T\', \'FF\'), \'R\'), \'D\'))", dayOfMonth);
            calculatedField.FieldType = DevExpress.XtraReports.UI.FieldType.String;
            this.CalculatedFields.Add(calculatedField);

            cell.Name = String.Format("xrtDataCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;

            if (new DateTime(year, month, dayOfMonth).DayOfWeek == DayOfWeek.Sunday)
                cell.BackColor = Color.LightGray;

            cell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, String.Format("ConsultarDetalladoPSLSucursal.{0}", calculatedField.Name))
            });
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeHeaderCell(XRTableCell cell, double weight, int dayOfMonth, int monthDays) {
            cell.Name = String.Format("xrtHeaderDayCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = dayOfMonth.ToString();
        }

        /// <summary>
        /// Prepara una celda que representa una sumatoria para un estatus de un historial de un día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="month">Mes en curso</param>
        /// <param name="year">Año en curso</param>
        /// <param name="monthDays">Total de días del mes</param>
        /// <param name="estatus">Estatus usado para filtrar la sumatoria</param>
        /// <param name="level">Número para diferenciar una sumatoria de otra, cuando estas se repiten</param>
        private void InitializeTotalStatusCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays, EEstatusHistorial? estatus, int level) {
            String estatusTxt = estatus.HasValue ? Enum.GetName(typeof(EEstatusHistorial), estatus.Value) : "All";
            cell.Name = String.Format("xrtTotal{0}Cell{1}{2}", estatusTxt, level, dayOfMonth);
            cell.WidthF = (float)weight;

            if (new DateTime(year, month, dayOfMonth).DayOfWeek == DayOfWeek.Sunday && (cell.BackColor == Color.Transparent && cell.Parent.BackColor == Color.Transparent))
                cell.BackColor = Color.LightGray;

            cell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, String.Format("ConsultarDetalladoPSLSucursal.EstatusHistorial_{0}", dayOfMonth))
            });

            XRSummary summary = new XRSummary();
            summary.Func = SummaryFunc.Custom;
            summary.Running = SummaryRunning.Group;
            cell.Summary = summary;

            String index = String.Format("{0}_{1}_{2}", estatusTxt, level, dayOfMonth);
            cell.SummaryReset += (object sender, EventArgs e) => {
                this.Sumatorias[index] = 0;
            };

            cell.SummaryRowChanged += (object sender, EventArgs e) => {
                this.Sumatorias[index] += this.CountFromHistorial(estatus, dayOfMonth);
            };

            cell.SummaryGetResult += (object sender, SummaryGetResultEventArgs e) => {
                e.Result = this.Sumatorias[index];
                e.Handled = true;
            };
        }

        /// <summary>
        /// Prepara una celda que representa un porcentaje de utilización de un día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="month">Mes en curso</param>
        /// <param name="year">Año en curso</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializePorcentajeUtilizacionCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays) {
            cell.Name = String.Format("xrtTotalPorcentajeUtilizacionCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;

            if (new DateTime(year, month, dayOfMonth).DayOfWeek == DayOfWeek.Sunday && (cell.BackColor == Color.Transparent && cell.Parent.BackColor == Color.Transparent))
                cell.BackColor = Color.LightGray;

            cell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, String.Format("ConsultarDetalladoPSLSucursal.EstatusHistorial_{0}", dayOfMonth))
            });

            XRSummary summary = new XRSummary();
            summary.FormatString = "{0:0.00}";//<-- Se cambio el formato de porcentaje
            summary.Func = SummaryFunc.Custom;
            summary.Running = SummaryRunning.Group;
            cell.Summary = summary;

            String indexTotal = String.Format("PorcentajeUtilizacion_Total_{0}", dayOfMonth);
            String indexRentado = String.Format("PorcentajeUtilizacion_Rentado_{0}", dayOfMonth);
            String indexTaller = String.Format("PorcentajeUtilizacion_Taller_{0}", dayOfMonth);
            cell.SummaryReset += (object sender, EventArgs e) => {
                this.Sumatorias[indexTotal] = 0;
                this.Sumatorias[indexRentado] = 0;
                this.Sumatorias[indexTaller] = 0;
            };

            cell.SummaryRowChanged += (object sender, EventArgs e) => {
                this.Sumatorias[indexTotal] += this.CountFromHistorial(null, dayOfMonth);
                this.Sumatorias[indexRentado] += this.CountFromHistorial(EEstatusHistorial.Rentado, dayOfMonth);
                this.Sumatorias[indexTaller] += this.CountFromHistorial(EEstatusHistorial.EnTaller, dayOfMonth);
            };

            cell.SummaryGetResult += (object sender, SummaryGetResultEventArgs e) => {
                Double disponibleReal = this.Sumatorias[indexTotal] - this.Sumatorias[indexTaller];
                Double rentado = this.Sumatorias[indexRentado];

                e.Result = disponibleReal > 0 ? Math.Round((rentado / disponibleReal) * 100.00) : 0.0;
                e.Handled = true;
            };
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el nombre del mes en curso
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void NombreMes_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e) {
            int month = (int)this.Parameters["Mes"].Value;
            e.Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToUpper();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días del mes en curso
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void TotalDiasMes_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e) {
            e.Value = this.GetDaysInMonth();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días en renta de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void DiasRenta_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e) {
            ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source = (e.Row as DataRowView).Row as ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow;
            e.Value = this.CountFromHistorial(source, EEstatusHistorial.Rentado);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días en taller de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void DiasTaller_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e) {
            ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source = (e.Row as DataRowView).Row as ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow;
            e.Value = this.CountFromHistorial(source, EEstatusHistorial.EnTaller);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días objetivo de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void DiasObjetivo_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e) {
            ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source = (e.Row as DataRowView).Row as ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow;
            //Dias del mes menos los dias fuera de flota ó los dias de renta mas los dias disponibles
            e.Value = this.EnFlota(source) ? this.GetDaysInMonth() - this.CountFromHistorial(source, EEstatusHistorial.FueraFlota) : this.CountFromHistorial(source, EEstatusHistorial.Rentado) + this.CountFromHistorial(source, EEstatusHistorial.Disponible);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el porcentaje de utilización de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void PorcentajeUtilizacion_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e) {
            ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow source = (e.Row as DataRowView).Row as ConsultarDetalladoPSLSucursalDS.ConsultarDetalladoPSLSucursalRow;
            Double rentados = this.CountFromHistorial(source, EEstatusHistorial.Rentado);
            Double diasObjetivo = this.EnFlota(source) ? this.GetDaysInMonth() - this.CountFromHistorial(source, EEstatusHistorial.FueraFlota) : this.CountFromHistorial(source, EEstatusHistorial.Rentado) + this.CountFromHistorial(source, EEstatusHistorial.Disponible);

            e.Value = diasObjetivo > 0 ? Math.Round((rentados / diasObjetivo) * 100.00) : 0.00;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar la imagen de logo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                    .Select(x => (String)x["URLLogoEmpresa"])
                                                    .FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el reporte principal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void DetalladoPSLSucursalRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            this.CreateNewColumns();

            var rowsConfiguracion = (this.DataSource as ConsultarDetalladoPSLSucursalDS).ConfiguracionModulo.ToList();
            string nombreSucursal = rowsConfiguracion.Any() ? rowsConfiguracion.First().NombreSucursal : String.Empty;
            xrTableCell17.Text = xrTableCell17.Text.Replace("[NombreSucursal]", nombreSucursal);

            if (nombreSucursal.Length > 8) {
                var listaUnidades = (this.DataSource as ConsultarDetalladoPSLSucursalDS).ConsultarDetalladoPSLSucursal.Count > 0 ? (this.DataSource as ConsultarDetalladoPSLSucursalDS).ConsultarDetalladoPSLSucursal.ToList() : null;

                if (listaUnidades != null && listaUnidades.Any()) {
                    var listaSucursales = listaUnidades.Select(x => x.NombreSucursal).Distinct().ToList();
                    string sucursalName = "";
                    int registros = 0;
                    foreach (var sucName in listaSucursales) {
                        if (listaUnidades.Count(x => x.NombreSucursal == sucName) > registros) {
                            sucursalName = sucName;
                            registros = listaUnidades.Count(x => x.NombreSucursal == sucName);
                        }
                    }
                    foreach (var registroUnidad in listaUnidades)
                        registroUnidad.NombreSucursal = sucursalName;
                }
            }

            foreach (var parameter in this.Parameters)
                this.xrSubRptRentados.ReportSource.Parameters[parameter.Name].Value = parameter.Value;

            DetalladoPSLSucursalPromedioMensualRPT reportSource = this.xrSubRptRentados.ReportSource as DetalladoPSLSucursalPromedioMensualRPT;
            reportSource.CreateNewColumns();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se va a visualizar el subreporte de acumulados por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrSubRptRentados_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            XRSubreport subReport = sender as XRSubreport;
            if (subReport.ReportSource == null)
                return;

            subReport.ReportSource.DataSource = this.DataSource;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para el porcentaje de utilización total de un modelo de unidad
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasPorcentajeUtilizacion_SummaryReset(object sender, EventArgs e) {
            this.Sumatorias["SumaDiasRentaParaUtilizacion"] = 0;
            this.Sumatorias["SumaDiasObjectoParaUtilizacion"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para el porcentaje de utilización total de un modelo de unidad
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasPorcentajeUtilizacion_SummaryRowChanged(object sender, EventArgs e) {
            this.Sumatorias["SumaDiasRentaParaUtilizacion"] += this.CountFromHistorial(EEstatusHistorial.Rentado);
            this.Sumatorias["SumaDiasObjectoParaUtilizacion"] += this.EnFlota(null) ? this.GetDaysInMonth() - this.CountFromHistorial(EEstatusHistorial.FueraFlota) : this.CountFromHistorial(EEstatusHistorial.Rentado) + this.CountFromHistorial(EEstatusHistorial.Disponible);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el porcentaje de utilización total de un modelo de unidad
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasPorcentajeUtilizacion_SummaryGetResult(object sender, SummaryGetResultEventArgs e) {
            Double rentados = this.Sumatorias["SumaDiasRentaParaUtilizacion"];
            Double diasObjetivo = this.Sumatorias["SumaDiasObjectoParaUtilizacion"];

            e.Result = diasObjetivo > 0 ? Math.Round((rentados / diasObjetivo) * 100.00) : 0.0;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para el promedio mensual del total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTotal_SummaryReset(object sender, EventArgs e) {
            this.Sumatorias["PromedioTotal"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para el promedio mensual del total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTotal_SummaryRowChanged(object sender, EventArgs e) {
            this.Sumatorias["PromedioTotal"] += this.CountFromHistorial();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio mensual del total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e) {
            Double promedioTotal = this.Sumatorias["PromedioTotal"];
            Double daysInMonth = this.GetDaysInMonth();

            e.Result = promedioTotal / daysInMonth;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para el promedio mensual de las unidades rentadas
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioRentados_SummaryReset(object sender, EventArgs e) {
            this.Sumatorias["PromedioRentados"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para el promedio mensual de las unidades rentadas
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioRentados_SummaryRowChanged(object sender, EventArgs e) {
            this.Sumatorias["PromedioRentados"] += this.CountFromHistorial(EEstatusHistorial.Rentado);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio mensual de las unidades rentadas
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioRentados_SummaryGetResult(object sender, SummaryGetResultEventArgs e) {
            Double rentados = this.Sumatorias["PromedioRentados"];
            Double daysInMonth = this.GetDaysInMonth();

            e.Result = rentados / daysInMonth;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para el porcentaje de utilización total de un modelo de unidad
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioPorcentajeUtilizacion_SummaryReset(object sender, EventArgs e) {
            Double daysInMonth = this.GetDaysInMonth();
            for (int i = 1; i <= daysInMonth; i++) {
                this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Total_{0}", i)] = 0;
                this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Rentados_{0}", i)] = 0;
                this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Taller_{0}", i)] = 0;
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para el porcentaje de utilización total de un modelo de unidad
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioPorcentajeUtilizacion_SummaryRowChanged(object sender, EventArgs e) {
            Double daysInMonth = this.GetDaysInMonth();
            for (int i = 1; i <= daysInMonth; i++) {
                this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Total_{0}", i)] += this.CountFromHistorial(null, i);
                this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Rentados_{0}", i)] += this.CountFromHistorial(EEstatusHistorial.Rentado, i);
                this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Taller_{0}", i)] += this.CountFromHistorial(EEstatusHistorial.EnTaller, i);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el porcentaje de utilización total de un modelo de unidad
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioPorcentajeUtilizacion_SummaryGetResult(object sender, SummaryGetResultEventArgs e) {
            Double porcentajes = 0.0;
            Double daysInMonth = this.GetDaysInMonth();

            for (int i = 1; i <= daysInMonth; i++) {
                Double disponibleReal = this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Total_{0}", i)] - this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Taller_{0}", i)];
                Double rentado = this.Sumatorias[String.Format("PromedioPorcentajeUtilizacion_Rentados_{0}", i)];

                porcentajes += disponibleReal > 0 ? Math.Round((rentado / disponibleReal) * 100.00) : 0.0;
            }

            e.Result = Math.Round((porcentajes / daysInMonth));
            e.Handled = true;
        }
    }
}
