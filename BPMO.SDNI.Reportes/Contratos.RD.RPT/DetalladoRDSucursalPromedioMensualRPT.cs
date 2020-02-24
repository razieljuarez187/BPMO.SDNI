//Satisface el CU018 – Reporte Detallado de Renta Diaria por Sucursal

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Linq;
using BPMO.SDNI.Contratos.RD.Reportes.DA;
using BPMO.SDNI.Flota.BO;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.RD.RPT
{
    /// <summary>
    /// Plantilla de subreporte de amulados por módelo de unidad para el Reporte detallado de Renta diaria por sucursal
    /// </summary>
    public partial class DetalladoRDSucursalPromedioMensualRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Tamaño de la celda base que se divide en el número de días del mes a visualizar
        /// </summary>
        private double diasMesEncabezadosSize;

        private Dictionary<String, int> _totalesDiasObjetivo;

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
        public Dictionary<String, int> Sumatorias
        {
            get
            {
                if (this._sumatorias == null)
                    this._sumatorias = new Dictionary<String, int>();

                return this._sumatorias;
            }
        }

        /// <summary>
        /// Contructor por default
        /// </summary>
        public DetalladoRDSucursalPromedioMensualRPT()
        {
            this.InitializeComponent();
            this.diasMesEncabezadosSize = this.xrtCellDiasMes.WidthF;
        }

        /// <summary>
        /// Obtiene el numero de días del mes en curso que se esta procesando
        /// </summary>
        /// <returns>Objeto de tipo Int32 con el número total de días del mes</returns>
        private int GetDaysInMonth()
        {
            int month = (int)this.Parameters["Mes"].Value;
            int year = (int)this.Parameters["Anio"].Value;

            int monthDays = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDaysInMonth(year, month);
            return monthDays;
        }

        /// <summary>
        /// Realiza un conteo de unidades por modelo tomando como base el estatus de historial de una unidad
        /// </summary>
        /// <returns>Conteo de unidades por modelo tomando como base su estatus de historial</returns>
        private int CountFromModelo()
        {
            return this.CountFromModelo(null, null, null);
        }

        /// <summary>
        /// Realiza un conteo de unidades por modelo tomando como base el estatus de historial de una unidad
        /// </summary>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <returns>Conteo de unidades por modelo tomando como base su estatus de historial</returns>
        private int CountFromModelo(EEstatusHistorial? estatusToCompare)
        {
            return this.CountFromModelo(null, estatusToCompare, null);
        }

        /// <summary>
        /// Realiza un conteo de unidades por modelo tomando como base el estatus de historial de una unidad
        /// </summary>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <param name="dayOfMonth">Día del mes a procesar, si se omite se procesa todo el mes</param>
        /// <returns>Conteo de unidades por modelo tomando como base su estatus de historial</returns>
        private int CountFromModelo(EEstatusHistorial? estatusToCompare, int? dayOfMonth)
        {
            return this.CountFromModelo(null, estatusToCompare, dayOfMonth);
        }

        /// <summary>
        /// Realiza un conteo de unidades por modelo tomando como base el estatus de historial de una unidad
        /// </summary>
        /// <param name="source">Fila de datos a evaluar</param>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <param name="dayOfMonth">Día del mes a procesar, si se omite se procesa todo el mes</param>
        /// <returns>Conteo de unidades por modelo tomando como base su estatus de historial</returns>
        private int CountFromModelo(ConsultarDetalladoRDSucursalDS.ModeloRow source, EEstatusHistorial? estatusToCompare, int? dayOfMonth)
        {
            int result = 0;
            int limit1 = 0;
            int limit2 = 0;

            if (dayOfMonth.HasValue)
            {
                limit1 = dayOfMonth.Value;
                limit2 = dayOfMonth.Value;
            }
            else
            {
                limit1 = 1;
                limit2 = this.GetDaysInMonth();
            }

            if (source == null)
            {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;
                if (source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoRDSucursalDS.ModeloRow)));
            }

            ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow[] detallesRow = source.GetConsultarDetalladoRDSucursalRows();
            foreach (ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow detalleRow in detallesRow)
            {
                for (int i = limit1; i <= limit2; i++)
                {
                    String rowIndex = String.Format("EstatusHistorial_{0}", i);
                    if (detalleRow.IsNull(rowIndex))
                        continue;

                    EEstatusHistorial? estatusHistorial = (EEstatusHistorial)Enum.ToObject(typeof(EEstatusHistorial), detalleRow[rowIndex]);
                    if (estatusToCompare.HasValue)
                    {
                        if (estatusHistorial == estatusToCompare)
                            result++;
                    }
                    else
                    {
                        if (estatusHistorial != EEstatusHistorial.FueraFlota)
                            result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtiene el total de dias objetivos por modelo
        /// </summary>
        /// <param name="source">Fuente de datos del Modelo</param>
        /// <returns>Cantidad de Dias objetivos por modelo</returns>
        private int CountDiasObjetivo(ConsultarDetalladoRDSucursalDS.ModeloRow source)
        {
            int result = 0;
            int limit2 = this.GetDaysInMonth();

            if (source == null)
            {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;
                if (source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoRDSucursalDS.ModeloRow)));
            }

            ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow[] detallesRow = source.GetConsultarDetalladoRDSucursalRows();
            foreach (ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow detalleRow in detallesRow)
            {
                if (EnFlota(detalleRow))
                    result += limit2 - this.CountFromHistorial(detalleRow, EEstatusHistorial.FueraFlota, null);
                else
                    result += (this.CountFromHistorial(detalleRow, EEstatusHistorial.Rentado, null) + this.CountFromHistorial(detalleRow, EEstatusHistorial.Disponible, null));
            }

            return result;
        }

        /// <summary>
        /// Realiza un conteo de unidades sobre una fila de registro de estatus de historial de una unidad
        /// </summary>
        /// <param name="source">Fila de datos a evaluar</param>
        /// <param name="estatusToCompare">Estatus de Historial a comparar, si se omiten se cuentan todas las unidades</param>
        /// <param name="dayOfMonth">Día del mes a procesar, si se omite se procesa todo el mes</param>
        /// <returns>Conteo de unidades tomando como base su estatus de historial</returns>
        private int CountFromHistorial(ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow source, EEstatusHistorial? estatusToCompare, int? dayOfMonth)
        {
            int result = 0;

            int limit1 = 0;
            int limit2 = 0;
            if (dayOfMonth.HasValue)
            {
                limit1 = dayOfMonth.Value;
                limit2 = dayOfMonth.Value;
            }
            else
            {
                limit1 = 1;
                limit2 = this.GetDaysInMonth();
            }

            if (source == null)
            {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow;
                if (source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow)));
            }

            for (int i = limit1; i <= limit2; i++)
            {
                String index = String.Format("EstatusHistorial_{0}", i);
                if (source.IsNull(index))
                    continue;

                EEstatusHistorial? estatus = (EEstatusHistorial)Enum.ToObject(typeof(EEstatusHistorial), (int)source[index]);
                if (estatusToCompare.HasValue)
                {
                    if (estatus == estatusToCompare)
                        result++;
                }
                else
                {
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
        private bool EnFlota(ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow source)
        {
            bool enFlota = true;

            int limit1 = 1;
            int limit2 = this.GetDaysInMonth();

            if (source == null)
            {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow;
                if (source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow)));
            }

            for (int i = limit1; i <= limit2; i++)
            {
                String index = String.Format("EstatusHistorial_{0}", i);
                if (source.IsNull(index) && i == 1)
                {
                    return enFlota;
                }
                else
                {
                    if (source.IsNull(index) && i > 1)
                    {
                        EEstatusHistorial? estatusAnterior = (EEstatusHistorial)Enum.ToObject(typeof(EEstatusHistorial), (int)source[String.Format("EstatusHistorial_{0}", i - 1)]);
                        return estatusAnterior != EEstatusHistorial.FueraFlota;
                    }
                }
            }

            return enFlota;
        }
        
        /// <summary>
        /// Crea las nuevas columnas tomando como base el número de días del mes en curso
        /// </summary>
        public void CreateNewColumns()
        {
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
        private void CreateNewColumn(int dayOfMonth, int month, int year, int monthDays, bool isNewColumn)
        {
            XRTableCell[] newHeaderCell = new XRTableCell[] { this.xrtCellDiasMesEncabezados };
            XRTableCell[] newDataCell = new XRTableCell[] { this.xrtCellDiasMes };
            XRTableCell[] newSumRentadosCell = new XRTableCell[] { this.xrtCellTotalRentados };
            XRTableCell[] newTotalCell = new XRTableCell[] { this.xrtCellTotalFlota };
            XRTableCell[] newSumRentados2Cell = new XRTableCell[] { this.xrtCellTotalRentados2 };
            XRTableCell[] newSumDisponiblesCell = new XRTableCell[] { this.xrtCellTotalDisponibles };
            XRTableCell[] newSumTallerCell = new XRTableCell[] { this.xrtCellTotalTaller };
            XRTableCell[] newSumFueraFlotaCell = new XRTableCell[] {this.xrtCellTotalFueraFlota};
            XRTableCell[] newSumPorcentajeUtilizacion = new XRTableCell[] { this.xrtCellTotalPorcentajeUtilizacion };

            if (isNewColumn)
            {
                newHeaderCell = this.xrtblHeader.InsertColumnToLeft(this.xrtCellDiasMesEncabezados);
                newDataCell = this.xrtblRows.InsertColumnToLeft(this.xrtCellDiasMes);
                newSumRentadosCell = this.xrtblTotalRentados.InsertColumnToLeft(this.xrtCellTotalRentados);

                newTotalCell = this.xrtblTotales.InsertColumnToLeft(this.xrtCellTotalFlota);

                //Debido a que la tabla esta construida por varias columnas se extraen las filas
                newSumRentados2Cell = new XRTableCell[] { newTotalCell[1] };
                newSumDisponiblesCell = new XRTableCell[] { newTotalCell[2] };
                newSumTallerCell = new XRTableCell[] { newTotalCell[3] };
                newSumFueraFlotaCell = new XRTableCell[] {newTotalCell[4]};
                newSumPorcentajeUtilizacion = new XRTableCell[] { newTotalCell[5] };
                newTotalCell = new XRTableCell[] { newTotalCell[0] };
            }

            double size = this.diasMesEncabezadosSize / monthDays;

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

            foreach(XRTableCell cell in newSumFueraFlotaCell)
                this.InitializeTotalStatusCell(cell, size, dayOfMonth, month, year, monthDays, EEstatusHistorial.FueraFlota, 1);

            foreach (XRTableCell cell in newSumPorcentajeUtilizacion)
                this.InitializePorcentajeUtilizacionCell(cell, size, dayOfMonth, month, year, monthDays);
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeHeaderCell(XRTableCell cell, double weight, int dayOfMonth, int monthDays)
        {
            cell.Name = String.Format("xrtHeaderDayCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = dayOfMonth.ToString();
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
        private void InitializeDataCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays)
        {
            CalculatedField calculatedField = new CalculatedField();
            calculatedField.DataMember = "Modelo";
            calculatedField.Name = String.Format("TotalRentados_{0}", dayOfMonth);
            calculatedField.FieldType = DevExpress.XtraReports.UI.FieldType.Int32;
            calculatedField.Scripts.OnGetValue = String.Format("TotalRentados_GetValue{0}", dayOfMonth);
            this.CalculatedFields.Add(calculatedField);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(String.Format(@"private void TotalRentados_GetValue{0}(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e)", dayOfMonth));
            sb.AppendLine("{");
            sb.AppendLine("\tthis.CurrentReport.TotalRentados_GetValue(sender, e);");
            sb.AppendLine("}");

            this.ScriptsSource += sb.ToString();

            cell.Name = String.Format("xrtDataCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Tag = dayOfMonth;

            if (new DateTime(year, month, dayOfMonth).DayOfWeek == DayOfWeek.Sunday && (cell.BackColor == Color.Transparent && cell.Parent.BackColor == Color.Transparent))
                cell.BackColor = Color.LightGray;

            cell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, String.Format("Modelo.{0}", calculatedField.Name))
            });
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
        private void InitializeTotalStatusCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays, EEstatusHistorial? estatus, int level)
        {
            String estatusTxt = estatus.HasValue ? Enum.GetName(typeof(EEstatusHistorial), estatus.Value) : "All";
            cell.Name = String.Format("xrtTotal{0}Cell{1}{2}", estatusTxt, level, dayOfMonth);
            cell.WidthF = (float)weight;

            if (new DateTime(year, month, dayOfMonth).DayOfWeek == DayOfWeek.Sunday && (cell.BackColor == Color.Transparent && cell.Parent.BackColor == Color.Transparent))
                cell.BackColor = Color.LightGray;

            cell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelo.ModeloID")
            });

            XRSummary summary = new XRSummary();
            summary.Func = SummaryFunc.Custom;
            summary.Running = SummaryRunning.Report;
            cell.Summary = summary;

            String index = String.Format("{0}_{1}_{2}", estatusTxt, level, dayOfMonth);
            cell.SummaryReset += (object sender, EventArgs e) =>
            {
                this.Sumatorias[index] = 0;
            };

            cell.SummaryRowChanged += (object sender, EventArgs e) =>
            {
                ConsultarDetalladoRDSucursalDS.ModeloRow modeloRow = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;
                this.Sumatorias[index] += this.CountFromModelo(modeloRow, estatus, dayOfMonth);
            };

            cell.SummaryGetResult += (object sender, SummaryGetResultEventArgs e) =>
            {
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
        private void InitializePorcentajeUtilizacionCell(XRTableCell cell, double weight, int dayOfMonth, int month, int year, int monthDays)
        {
            cell.Name = String.Format("xrtTotalPorcentajeUtilizacionCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;

            if (new DateTime(year, month, dayOfMonth).DayOfWeek == DayOfWeek.Sunday && (cell.BackColor == Color.Transparent && cell.Parent.BackColor == Color.Transparent))
                cell.BackColor = Color.LightGray;

            cell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, "Modelo.ModeloID")
            });

            XRSummary summary = new XRSummary();
            summary.FormatString = "{0:0.00}";
            summary.Func = SummaryFunc.Custom;
            summary.Running = SummaryRunning.Report;
            cell.Summary = summary;

            String indexTotal = String.Format("PorcentajeUtilizacion_Total_{0}", dayOfMonth);
            String indexRentado = String.Format("PorcentajeUtilizacion_Rentado_{0}", dayOfMonth);
            String indexTaller = String.Format("PorcentajeUtilizacion_Taller_{0}", dayOfMonth);
            cell.SummaryReset += (object sender, EventArgs e) =>
            {
                this.Sumatorias[indexTotal] = 0;
                this.Sumatorias[indexRentado] = 0;
                this.Sumatorias[indexTaller] = 0;
            };

            cell.SummaryRowChanged += (object sender, EventArgs e) =>
            {
                this.Sumatorias[indexTotal] += this.CountFromModelo(null, dayOfMonth);
                this.Sumatorias[indexRentado] += this.CountFromModelo(EEstatusHistorial.Rentado, dayOfMonth);
                this.Sumatorias[indexTaller] += this.CountFromModelo(EEstatusHistorial.EnTaller, dayOfMonth);
            };

            cell.SummaryGetResult += (object sender, SummaryGetResultEventArgs e) =>
            {
                Double disponibleReal = this.Sumatorias[indexTotal] - this.Sumatorias[indexTaller];
                Double rentado = this.Sumatorias[indexRentado];

                e.Result = disponibleReal > 0 ? Math.Round((rentado / disponibleReal) * 100.00) : 0.0;
                e.Handled = true;
            };
        }

        /// <summary>
        /// Obtiene el acumlado de número de veces que una unidad fue rentada
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void TotalRentados_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e)
        {
            CalculatedField calculatedField = sender as CalculatedField;
            String sDayOfMonth = calculatedField.Name.Split('_')[1];
            int dayOfMonth = Convert.ToInt32(sDayOfMonth);
            ConsultarDetalladoRDSucursalDS.ModeloRow modeloRow = (e.Row as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;

            e.Value = this.CountFromModelo(modeloRow, EEstatusHistorial.Rentado, dayOfMonth);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días objetivo de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void DiasObjetivo_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e)
        {
            ConsultarDetalladoRDSucursalDS.ModeloRow source = (e.Row as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;
            if(source == null)
            {
                source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;
                if(source == null)
                    throw new Exception(String.Format("La fila de datos actual no corresponde a una fila de tipo {0}", typeof(ConsultarDetalladoRDSucursalDS.ModeloRow)));
            }

            ConsultarDetalladoRDSucursalDS.ConsultarDetalladoRDSucursalRow[] detallesRow = source.GetConsultarDetalladoRDSucursalRows();
            List<String> numeroChasis = new List<String>();
            foreach(var row in detallesRow)
            {
                if(!numeroChasis.Any(x => x.ToUpper() == row.Serie.ToUpper()))
                    numeroChasis.Add(row.Serie);
            }

            if(this._totalesDiasObjetivo == null) this._totalesDiasObjetivo = new Dictionary<String, int>();
            Int32 totalDiasObjetivo = CountDiasObjetivo(source);

            if (!this._totalesDiasObjetivo.Any(x=>x.Key == source.Nombre))
                this._totalesDiasObjetivo.Add(source.Nombre, totalDiasObjetivo);

            e.Value = totalDiasObjetivo;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días en renta de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void DiasRenta_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e)
        {
            e.Value = this.CountFromModelo(EEstatusHistorial.Rentado);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el número de días en taller de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void DiasTaller_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e)
        {
            e.Value = this.CountFromModelo(EEstatusHistorial.EnTaller);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se obtiene el porcentaje de utilización de un equipo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        public void PorcentajeUtilizacion_GetValue(object sender, DevExpress.XtraReports.UI.GetValueEventArgs e)
        {
            ConsultarDetalladoRDSucursalDS.ModeloRow source = (e.Row as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;

            double diasRentado = this.CountFromModelo(EEstatusHistorial.Rentado);
            double diasObjetivo = source != null ? this._totalesDiasObjetivo != null ? this._totalesDiasObjetivo[source.Nombre] : 0 : 0;

            e.Value = diasObjetivo > 0 ? Math.Round((diasRentado / diasObjetivo) * 100.00) : 0.0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para los días objetivo de cada modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasObjeto_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["TotalDiasObjetivo"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para los días objetivo de cada modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasObjeto_SummaryRowChanged(object sender, EventArgs e)
        {
            ConsultarDetalladoRDSucursalDS.ModeloRow source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;
            this.Sumatorias["TotalDiasObjetivo"] += this._totalesDiasObjetivo[source.Nombre];
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve la suma de los días objetivo de cada modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasObjeto_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = this.Sumatorias["TotalDiasObjetivo"];
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para los días en renta de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasRenta_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["TotalDiasRenta"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma de los días en renta de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasRenta_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["TotalDiasRenta"] += this.CountFromModelo(EEstatusHistorial.Rentado);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve la suma de los días en renta de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasRenta_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = this.Sumatorias["TotalDiasRenta"];
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para los días en taller de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasTaller_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["TotalDiasTaller"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma de los días en renta de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasTaller_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["TotalDiasTaller"] += this.CountFromModelo(EEstatusHistorial.EnTaller);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve la suma de los días en renta de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasTaller_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = this.Sumatorias["TotalDiasTaller"];
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria del porcentaje de utilización por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasPorcentajeUtilizacion_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["TotalPorcentajeUtilizacion_DiasRenta"] = 0;
            this.Sumatorias["TotalPorcentajeUtilizacion_DiasObjetivo"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma del porcentaje de utilización por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasPorcentajeUtilizacion_SummaryRowChanged(object sender, EventArgs e)
        {
            ConsultarDetalladoRDSucursalDS.ModeloRow source = (this.GetCurrentRow() as DataRowView).Row as ConsultarDetalladoRDSucursalDS.ModeloRow;

            this.Sumatorias["TotalPorcentajeUtilizacion_DiasRenta"] += this.CountFromModelo(EEstatusHistorial.Rentado);
            this.Sumatorias["TotalPorcentajeUtilizacion_DiasObjetivo"] += this._totalesDiasObjetivo[source.Nombre];
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve la suma del porcentaje de utilización por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellSumDiasPorcentajeUtilizacion_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double renta = this.Sumatorias["TotalPorcentajeUtilizacion_DiasRenta"];
            Double diasObjetivo = this.Sumatorias["TotalPorcentajeUtilizacion_DiasObjetivo"];

            e.Result = diasObjetivo > 0 ? Math.Round((renta / diasObjetivo) * 100.0): 0.0;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa el promedio total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTotal_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_TotalFlota"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace el promedio total de las unidades por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTotal_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_TotalFlota"] += this.CountFromModelo();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTotal_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double suma = this.Sumatorias["Promedio_TotalFlota"];
            Double totalDias = this.GetDaysInMonth();

            e.Result = suma / totalDias;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa el promedio de unidades rentadas de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioRentados_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_Rentados"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace el promedio de unidades rentadas por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioRentados_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_Rentados"] += this.CountFromModelo(EEstatusHistorial.Rentado);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio de unidades rentadas de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioRentados_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double suma = this.Sumatorias["Promedio_Rentados"];
            Double totalDias = this.GetDaysInMonth();

            e.Result = suma / totalDias;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa el promedio de unidades disponibles de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioDisponibles_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_Disponibles"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace el promedio de unidades disponibles por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioDisponibles_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_Disponibles"] += this.CountFromModelo(EEstatusHistorial.Disponible);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio de unidades disponibles de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioDisponibles_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double suma = this.Sumatorias["Promedio_Disponibles"];
            Double totalDias = this.GetDaysInMonth();

            e.Result = suma / totalDias;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa el promedio de unidades en taller de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTaller_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_Taller"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace el promedio de unidades en taller por modelo
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTaller_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_Taller"] += this.CountFromModelo(EEstatusHistorial.EnTaller);
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio de unidades en taller de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioTaller_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double suma = this.Sumatorias["Promedio_Taller"];
            Double totalDias = this.GetDaysInMonth();

            e.Result = suma / totalDias;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para el porcentaje de utilización total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioPorcentajeUtilizacion_SummaryReset(object sender, EventArgs e)
        {
            Double totalDias = this.GetDaysInMonth();
            for (int i = 1; i <= totalDias; i++)
            {
                this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Total_{0}", i)] = 0;
                this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Rentado_{0}", i)] = 0;
                this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Taller_{0}", i)] = 0;
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para el porcentaje de utilización total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioPorcentajeUtilizacion_SummaryRowChanged(object sender, EventArgs e)
        {
            Double totalDias = this.GetDaysInMonth();
            for (int i = 1; i <= totalDias; i++)
            {
                this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Total_{0}", i)] += this.CountFromModelo(null, i);
                this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Rentado_{0}", i)] += this.CountFromModelo(EEstatusHistorial.Rentado, i);
                this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Taller_{0}", i)] += this.CountFromModelo(EEstatusHistorial.EnTaller, i);
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el porcentaje de utilización total de la flota
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos asociados al evento</param>
        private void xrtCellPromedioPorcentajeUtilizacion_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double totalDias = this.GetDaysInMonth();
            Double porcentajes = 0.0;

            for (int i = 1; i <= totalDias; i++)
            {
                Double disponibleReal = this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Total_{0}", i)] - this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Taller_{0}", i)];
                Double rentado = this.Sumatorias[String.Format("Promedio_PorcentajeUtilizacion_Rentado_{0}", i)];

                porcentajes += disponibleReal > 0 ? Math.Round((rentado / disponibleReal) * 100.00) : 0.0;
            }

            e.Result = Math.Round(porcentajes / totalDias);
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se devuelve el promedio de unidades fuera de flota
        /// </summary>
        private void xrtCellPromedioFueraFlota_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Double suma = this.Sumatorias["Promedio_FueraFlota"];
            Double totalDias = this.GetDaysInMonth();

            e.Result = suma / totalDias;
            e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se inicializa la sumatoria para el promedio de unidades Fuera de Flota
        /// </summary>
        private void xrtCellPromedioFueraFlota_SummaryReset(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_FueraFlota"] = 0;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se hace la suma para el promediode unidades fuera de flota
        /// </summary>
        private void xrtCellPromedioFueraFlota_SummaryRowChanged(object sender, EventArgs e)
        {
            this.Sumatorias["Promedio_FueraFlota"] += this.CountFromModelo(EEstatusHistorial.FueraFlota);
        }
    }
}
