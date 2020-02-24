//Satisface el CU016 – Reporte de Renta Diaria General
using System;
using System.Globalization;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Subreporte que imprime la cantidad de dias del mes con sus numeros de dia
    /// </summary>
    public partial class RentaDiariaGeneralMesSucursalElementoRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propiedades
        /// <summary>
        /// Tamaño de la celda base que se divide en el número de días del mes a visualizar
        /// </summary>
        public double diasMesEncabezadosSize { get; set; }
        /// <summary>
        /// Indica el Mes para los calculos
        /// </summary>
        public Int32? Mes { get; set; }
        /// <summary>
        /// Indica el año para los calculos
        /// </summary>
        public Int32? Anio { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        public RentaDiariaGeneralMesSucursalElementoRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obtiene el numero de días del mes en curso que se esta procesando
        /// </summary>
        /// <returns>Objeto de tipo Int32 con el número total de días del mes</returns>
        private int GetDaysInMonth(Int32? mes, Int32? anio)
        {
            int month = mes.Value;
            int year = anio.Value;

            int monthDays = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDaysInMonth(year, month);
            return monthDays;}

        /// <summary>
        /// Crea las nuevas columnas tomando como base el número de días del mes en curso
        /// </summary>
        public void CreateNewColumns()
        {
            if(Mes == null || Anio == null)
                throw new Exception("El Mes y el Año son parámetros requeridos para el reporte");

            int month = Mes.Value;
            int year = Anio.Value;

            int monthDays = this.GetDaysInMonth(Mes, Anio);

            for(int i = 0; i < monthDays - 1; i++)
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
            XRTableCell[] newHeaderCell = { xrLDiasMes };

            if(isNewColumn)
            {
                newHeaderCell = this.xrtblHeader.InsertColumnToLeft(xrLDiasMes);
            }

            double size = this.diasMesEncabezadosSize / monthDays;

            foreach(XRTableCell cell in newHeaderCell)
                this.InitializeHeaderCell(cell, size, dayOfMonth, monthDays);
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
            cell.BackColor = this.xrLTotalFlota.BackColor;
            cell.ForeColor = xrlDiasXUnidad.ForeColor;
        }
        #endregion
    }
}
