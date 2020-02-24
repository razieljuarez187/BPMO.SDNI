//Satisface el CU016 – Reporte de Renta Diaria General
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Drawing;
using BPMO.SDNI.Contratos.PSL.Reportes.DA;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;

namespace BPMO.SDNI.Contratos.PSL.RPT
{
    /// <summary>
    /// Subreporte que presenta la ultima seccion del reporte general
    /// </summary>
    public partial class RentaDiariaGeneralSucursalModeloPSLRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propiedades
        /// <summary>
        /// Tamaño de la celda base que se divide en el número de días del mes a visualizar
        /// </summary>
        public double diasMesWidth;
        /// <summary>
        /// Indica si el reporte esta filtrado por sucursal
        /// </summary>
        public Boolean? UsarSucursal { get; set; }
        /// <summary>
        /// Mes usado para los calculos
        /// </summary>
        public Int32? Mes { get; set; }
        /// <summary>
        /// Año usado para los calculos
        /// </summary>
        public Int32? Anio { get; set; }
        /// <summary>
        /// Porcentaje total por mes
        /// </summary>
        public Int32? PorcentajeTotal { get; set; }
        #endregion

        #region Metodos
        /// <summary>
        /// Constructor
        /// </summary>
        public RentaDiariaGeneralSucursalModeloPSLRPT()
        {
            InitializeComponent();
            diasMesWidth = xrtDiasMes.WidthF;
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
            return monthDays;
        }

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
            XRTableCell[] newHeaderModels = { xrtDiasMes };
            XRTableCell[] newHeaderCell = { xrTotalesFlota };
            XRTableCell[] newCellTotalesRentados = { xrTotalesRentados };
            XRTableCell[] newCellTotalesDisponibles = { xrTotalesDisponibles };
            XRTableCell[] newCellTotalesUtilizacion = { xrTotalesUtilizacion };

            if(isNewColumn)
            {
                newHeaderModels = this.xrTable2.InsertColumnToLeft(xrtDiasMes);
                newHeaderCell = this.xrTTotales.InsertColumnToLeft(xrTotalesFlota);
                newCellTotalesRentados = this.xrtTotalRentados.InsertColumnToLeft(xrTotalesRentados);
                newCellTotalesDisponibles = this.xrtTotalDisponibles.InsertColumnToLeft(xrTotalesDisponibles);
                newCellTotalesUtilizacion = this.xrtPorcentajeTotal.InsertColumnToLeft(xrTotalesUtilizacion);
            }

            double size = this.diasMesWidth / monthDays;

            foreach(XRTableCell cell in newHeaderModels)
                this.InitializeHeaderModels(cell, size, dayOfMonth, monthDays);

            foreach(XRTableCell cell in newHeaderCell)
                this.InitializeHeaderCell(cell, size, dayOfMonth, monthDays);

            foreach(XRTableCell cell in newCellTotalesRentados)
                this.InitializeTotalRentados(cell, size, dayOfMonth, monthDays);

            foreach(XRTableCell cell in newCellTotalesDisponibles)
                this.InitializeTotalDisponibles(cell, size, dayOfMonth, monthDays);

            foreach(XRTableCell cell in newCellTotalesUtilizacion)
                this.InitializePorcentajeTotal(cell, size, dayOfMonth, monthDays);
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeHeaderModels(XRTableCell cell, double weight, int dayOfMonth, int monthDays)
        {
            cell.Name = String.Format("xrtHeaderModelDay{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = "";
            if(CalcularDomingo(Anio.Value, Mes.Value, dayOfMonth))
                cell.BackColor = Color.DarkGray;
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
            cell.Text = "";
            if(CalcularDomingo(Anio.Value, Mes.Value, dayOfMonth))
                cell.BackColor = Color.DarkGray;
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeTotalRentados(XRTableCell cell, double weight, int dayOfMonth, int monthDays)
        {
            cell.Name = String.Format("xrtTotalRentadosDay{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = "";
            if(CalcularDomingo(Anio.Value, Mes.Value, dayOfMonth))
                cell.BackColor = Color.DarkGray;
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeTotalDisponibles(XRTableCell cell, double weight, int dayOfMonth, int monthDays)
        {
            cell.Name = String.Format("xrtTotalDisponiblesDay{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = "";
            if(CalcularDomingo(Anio.Value, Mes.Value, dayOfMonth))
                cell.BackColor = Color.DarkGray;
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="dayOfMonth">Día del mes</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializePorcentajeTotal(XRTableCell cell, double weight, int dayOfMonth, int monthDays)
        {
            cell.Name = String.Format("xrtPorcentajeTotalDay{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = "";
            if(CalcularDomingo(Anio.Value, Mes.Value, dayOfMonth))
                cell.BackColor = Color.DarkGray;
        }

        /// <summary>
        /// Modifica la grafica que se va a presentar
        /// </summary>
        /// <param name="mes">Mes usado para pintar el eje X</param>
        /// <param name="sucursalId">Identificador de la Sucursal a usar</param>
        /// <param name="datos">DataSet con la informacion</param>
        /// <param name="diasMes">Numero de dias del mes</param>
        /// <param name="totalFlota">Lista con la suma de totales de la flota</param>
        /// <param name="totalRentados">Lista con las suma de los totales rentados de la flota</param>
        private void ModificarGrafica(int mes, int sucursalId, RentasSucursalDS datos, int diasMes, List<Int32> totalFlota, List<Int32> totalRentados)
        {
            XRChart grafica = this.xrGraficaUtilizacion as XRChart;
            var nombreMes = datos.Meses.First(x => x.Mes == mes).Nombre;
            if(sucursalId != 0)
            {
                var nombreSucursal = datos.Sucursales.First(x => x.SucursalID == sucursalId).Nombre;
                grafica.Titles[0].Text = "SUCURSAL " + nombreSucursal + " " + nombreMes;
            }
            else
            {
                grafica.Titles[0].Text = "TOTAL " + nombreMes;
            }

            XYDiagram diagramaGrafica = grafica.Diagram as XYDiagram;
            diagramaGrafica.AxisX.CustomLabels.Clear();
            diagramaGrafica.AxisX.Range.MaxValue = diasMes;

            for(int i = 1; i < diasMes + 1; i++)
            {
                CustomAxisLabel customAxisLabel = new CustomAxisLabel();
                customAxisLabel.Name = i.ToString();
                customAxisLabel.AxisValue = i.ToString();
                diagramaGrafica.AxisX.CustomLabels.Add(customAxisLabel);
            }

            if(sucursalId == 0)
            {
                RentasSucursalDS newDataSet = new RentasSucursalDS();
                for(int i = 0; i < totalFlota.Count; i++)
                {
                    Int32 totalFlotaDia = totalFlota[i];
                    Int32 totalRentadosDia = totalRentados[i];
                    Double? porcentaje = null;
                    if(totalFlotaDia != 0)
                        porcentaje = (Double.Parse(totalRentadosDia.ToString()) / Double.Parse(totalFlotaDia.ToString()));
                    else
                        porcentaje = 0;
                    var row = newDataSet.SubTotalXDia.NewSubTotalXDiaRow();

                    row.Dia = i + 1;
                    row.Mes = mes;
                    row.PorcentajeUtilizacionFlota = Int32.Parse(Math.Round((Double)(porcentaje * 100)).ToString());
                    row.Anio = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Anio") : Anio.Value;

                    newDataSet.SubTotalXDia.AddSubTotalXDiaRow(row);
                }
                grafica.DataSource = newDataSet;
            }
            else
                grafica.DataSource = datos;
        }

        /// <summary>
        /// Establece el filtro a aplicar a la gráfcia
        /// </summary>
        /// <param name="sucursalID">Id de la sucursal a mostrar</param>
        /// <param name="mes">Mes a filtrar</param>
        public void SetFilter(int sucursalID, int mes)
        {
            this.FilterString = String.Format("[SucursalID] = {0} And [Mes] = {1}", sucursalID, mes);
            Series serie = this.xrGraficaUtilizacion.Series[0];

            serie.DataFilters.Clear();
            serie.DataFilters.ConjunctionMode = ConjunctionTypes.And;

            DataFilter filterSucursal = new DataFilter("SucursalID", typeof(Int32).FullName, DataFilterCondition.Equal, sucursalID);
            serie.DataFilters.Add(filterSucursal);

            DataFilter filterMes = new DataFilter("Mes", typeof(Int32).FullName, DataFilterCondition.Equal, mes);
            serie.DataFilters.Add(filterMes);
        }

        /// <summary>
        /// Determina si la fecha proporcionada corresponde al Domingo
        /// </summary>
        /// <param name="anio">Año</param>
        /// <param name="mes">Mes</param>
        /// <param name="dia">Dia</param>
        /// <returns>Devuelve true si la fecha es domingo</returns>
        private Boolean CalcularDomingo(int anio, int mes, int dia)
        {
            DateTime diaMes = new DateTime(anio, mes, dia);
            if(diaMes.DayOfWeek == DayOfWeek.Sunday)
                return true;

            return false;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento Ejecutado para calcular el nombre del modelo a presentar
        /// </summary>
        private void xrtNombreModelo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            RentasSucursalDS dataSource = this.Report.DataSource as RentasSucursalDS;

            var row = dataSource.Modelos.First(x => x.ModeloID == modeloId);
            xrtNombreModelo.Text = row.Nombre;
        }
        /// <summary>
        /// Evento Ejecutado para calcular el total de flota el ultimo dia de renta
        /// </summary>
        private void xrTPromedio_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }
        /// <summary>
        /// Evento Ejecutado para calcular los resultados por dia en el mes solicitado para cada modelo
        /// </summary>
        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var mes = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Mes") : Mes.Value;
            Int32? sucursal = 0;
            if(UsarSucursal.Value)
                sucursal = this.GetCurrentColumnValue<int>("SucursalID");
            var modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            var anio = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Anio") : Anio;
            var diasMes = this.GetDaysInMonth(mes, anio);

            RentasSucursalDS data = this.DataSource as RentasSucursalDS;
            XRTableRow tableRow = this.xrTable2.Rows.LastRow;

            Int32 flotaUltimoDia = 0;

            for(int i = 1; i < diasMes + 1; i++)
            {
                var cell = tableRow.Cells[String.Format("xrtHeaderModelDay{0}", i)] as XRTableCell;
                var dia = i;
                if(dia > 0)
                {
                    if(sucursal != 0)
                    {
                        var dataRow = data.ModelosXDia.FirstOrDefault(x => x.Mes == mes && x.Dia == dia && x.ModeloID == modeloId && x.SucursalID == sucursal);
                        if(dataRow != null)
                        {
                            cell.Text = (dataRow.UnidadesRentadas + dataRow.UnidadesDisponibles).ToString();
                            flotaUltimoDia = Int32.Parse(cell.Text);
                        }
                    }
                    else
                    {
                        var dataRow = data.ModelosXDia.Where(x => x.Mes == mes && x.Dia == dia && x.ModeloID == modeloId).ToList();
                        if(dataRow != null && dataRow.Count > 0)
                        {
                            cell.Text = (dataRow.Sum(x => x.UnidadesRentadas) + dataRow.Sum(x => x.UnidadesDisponibles)).ToString();
                            flotaUltimoDia = Int32.Parse(cell.Text);
                        }
                    }                    
                }
                else
                {
                    cell.Text = "-1";
                }
            }
            if(!UsarSucursal.Value)
            {
                this.xrtWhiteSpace.Text = flotaUltimoDia.ToString();
                this.xrtWhiteSpace.BackColor = Color.White;
            }
            else
            {
                this.xrtWhiteSpace.BackColor = Color.DarkGray;
            }
        }
        /// <summary>
        /// Evento Ejecutado para calcular los resultados por dias en el mes solicitado
        /// </summary>
        private void grpFTotales_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var mes = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Mes") : Mes.Value;
            Int32? sucursal = 0;
            if(UsarSucursal.Value)
                sucursal = this.GetCurrentColumnValue<int>("SucursalID");
            var anio = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Anio") : Anio.Value;
            var diasMes = this.GetDaysInMonth(mes, anio);

            RentasSucursalDS data = this.DataSource as RentasSucursalDS;
            
            XRTableRow tableRowTotalFlota = this.xrTTotales.Rows.LastRow;
            XRTableRow tableRowTotalRentados = this.xrtTotalRentados.Rows.LastRow;
            XRTableRow tableRowTotalDisponibles = this.xrtTotalDisponibles.Rows.LastRow;
            XRTableRow tableRowTotalPorcentaje = this.xrtPorcentajeTotal.Rows.LastRow;

            var totalFlotaRentados = 0;
            List<Int32> totalFlotaList = new List<Int32>();
            List<Int32> totalRentadosList = new List<Int32>();

            for(int i = 1; i < diasMes + 1; i++)
            {
                var cellTotalFlota = tableRowTotalFlota.Cells[String.Format("xrtHeaderDayCell{0}", i)] as XRTableCell;
                var cellTotalRentado = tableRowTotalRentados.Cells[String.Format("xrtTotalRentadosDay{0}", i)] as XRTableCell;
                var cellTotalDisponibles = tableRowTotalDisponibles.Cells[String.Format("xrtTotalDisponiblesDay{0}", i)] as XRTableCell;
                var cellTotalPorcentaje = tableRowTotalPorcentaje.Cells[String.Format("xrtPorcentajeTotalDay{0}", i)] as XRTableCell;

                var dia = i;
                
                if(dia > 0)
                {
                    if(sucursal != 0)
                    {
                        var dataRow = data.SubTotalXDia.FirstOrDefault(x => x.Mes == mes && x.Dia == dia && x.SucursalID == sucursal);
                        if(dataRow != null)
                        {
                            cellTotalFlota.Text = dataRow.TotalFlota.ToString();
                            cellTotalRentado.Text = dataRow.TotalRentados.ToString();
                            cellTotalDisponibles.Text = dataRow.TotalDisponible.ToString();
                            cellTotalPorcentaje.Text = dataRow.PorcentajeUtilizacionFlota.ToString();

                            totalFlotaRentados += dataRow.TotalRentados;
                        }
                    }
                    else
                    {
                        var dataRow = data.SubTotalXDia.Where(x => x.Mes == mes && x.Dia == dia).ToList();
                        if(dataRow != null && dataRow.Count > 0)
                        {
                            var sumaTotalFlota = Decimal.Parse(dataRow.Sum(x => x.TotalFlota).ToString());
                            var sumaTotalRentados = Decimal.Parse(dataRow.Sum(x => x.TotalRentados).ToString());
                            var sumaTotalDisponibles = Decimal.Parse(dataRow.Sum(x => x.TotalDisponible).ToString());
                            var sumaTotalPorcentaje = sumaTotalFlota != 0 ? sumaTotalRentados/sumaTotalFlota : 0M;

                            cellTotalFlota.Text = (sumaTotalFlota).ToString();
                            cellTotalRentado.Text = (sumaTotalRentados).ToString();
                            cellTotalDisponibles.Text = (sumaTotalDisponibles).ToString();
                            cellTotalPorcentaje.Text = Math.Round(sumaTotalPorcentaje*100).ToString();

                            totalFlotaRentados += (Int32)sumaTotalRentados;

                            totalFlotaList.Add((Int32)sumaTotalFlota);
                            totalRentadosList.Add((Int32)sumaTotalRentados);
                        }
                    }
                }
                else 
                {
                    cellTotalFlota.Text = "-1";
                    cellTotalRentado.Text = "-1";
                    cellTotalDisponibles.Text = "-1";
                    cellTotalPorcentaje.Text = "-1";
                }
            }

            totalFlotaRentados = totalFlotaRentados != 0 ? Int32.Parse(Math.Round(((Decimal)totalFlotaRentados / (Decimal)diasMes)).ToString()) : 0;
            this.xrPromedioTotalRentados.Text = totalFlotaRentados.ToString();

            if(sucursal != 0)
            {
                List<RentasSucursalDS.SubTotalXDiaRow> listaDias = data.SubTotalXDia.Where(x => x.Mes == mes && x.SucursalID == sucursal.Value).ToList();
                var totalFlota = listaDias.Last().TotalFlota;
                this.xrTPromedio.Text = totalFlota.ToString();
                this.xrPromedioDisponibles.Text = (totalFlota - totalFlotaRentados).ToString();
                this.xrPromedioUtilizacion.Text = PorcentajeTotal.ToString();
                this.ModificarGrafica(mes, sucursal.Value, data, diasMes, new List<Int32>(), new List<Int32>());
            }
            else
            {
                var totalFlota = totalFlotaList.Last();
                this.xrTPromedio.Text = totalFlota.ToString();
                this.xrPromedioDisponibles.Text = (totalFlota - totalFlotaRentados).ToString();
                this.xrPromedioUtilizacion.Text = PorcentajeTotal.ToString();
                this.ModificarGrafica(mes, 0, data, diasMes, totalFlotaList, totalRentadosList);
            }
        }
        #endregion
    }
}
