//Satisface el CU016 – Reporte de Renta Diaria General
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using BPMO.SDNI.Contratos.PSL.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.PSL.RPT
{
    /// <summary>
    /// Clase que imprime la region de encabezado del reporte general por mes
    /// </summary>
    public partial class RentaDiariaGeneralModeloSucursalElementoPSLRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propiedades
        /// <summary>
        /// Tamaño de la celda base que se divide en el número de días del mes a visualizar
        /// </summary>
        public double diasMesEncabezadosSize { get; set; }
        /// <summary>
        /// Determina si se filtra el reporte por sucursal
        /// </summary>
        public Boolean? UsarSucursal { get; set; }
        /// <summary>
        /// Indica el Mes para los calculos
        /// </summary>
        public Int32? Mes { get; set; }
        /// <summary>
        /// Indica el año para los calculos
        /// </summary>
        public Int32? Anio { get; set; }
        #endregion

        #region Metodos
        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public RentaDiariaGeneralModeloSucursalElementoPSLRPT()
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
            XRTableCell[] newHeaderCell = { xrtDiasMes };
            XRTableCell[] newTotalFlotaCell = { xrTDiasMesTotal };

            if(isNewColumn)
            {
                newHeaderCell = this.xrTable3.InsertColumnToLeft(xrtDiasMes);
                newTotalFlotaCell = this.xrTable4.InsertColumnToLeft(xrTDiasMesTotal);
            }

            double size = this.diasMesEncabezadosSize / monthDays;

            foreach(XRTableCell cell in newHeaderCell)
                this.InitializeHeaderCell(cell, size, dayOfMonth, monthDays);

            foreach(XRTableCell cell in newTotalFlotaCell)
                this.InitializeTotalFlotaCell(cell, size, dayOfMonth, monthDays);
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
        private void InitializeTotalFlotaCell(XRTableCell cell, double weight, int dayOfMonth, int monthDays){
            cell.Name = String.Format("xrtTotalFlotaDayCell{0}", dayOfMonth);
            cell.WidthF = (float)weight;
            cell.Text = "";
            if(CalcularDomingo(Anio.Value, Mes.Value, dayOfMonth))
                cell.BackColor = Color.DarkGray;
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
        /// Evento ejecutado antes de que se pinte el detalle para calcular resultados por dia
        /// </summary>
        private void Detail_BeforePrint(object sender, PrintEventArgs e)
        {
            var modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            var mes = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Mes") : Mes.Value;
            var anio = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Anio") : Anio.Value;
            Int32? sucursal = 0;
            if(UsarSucursal.Value)
                sucursal = this.GetCurrentColumnValue<int>("SucursalID");
            var diasEnMes = GetDaysInMonth(mes, anio);

            RentasSucursalDS data = this.DataSource as RentasSucursalDS;
            var row = data.Modelos.First(x => x.ModeloID == modeloId);
            xrtNombreModelo.Text = row.Nombre;

            XRTableRow tableRow = this.xrTable3.Rows.LastRow;
                                 
            for(int i = 1; i < diasEnMes + 1; i++)
            {
                var cell = tableRow.Cells[String.Format("xrtHeaderDayCell{0}", i)] as XRTableCell;
                var dia = i;
                if(dia > 0)
                {
                    if(sucursal != 0)
                    {
                        var dataRow = data.ModelosXDia.FirstOrDefault(x => x.Mes == mes && x.Dia == dia && x.ModeloID == modeloId && x.SucursalID == sucursal);
                        if(dataRow != null)
                            cell.Text = dataRow.UnidadesRentadas.ToString();
                    }
                    else
                    {
                        var dataRow = data.ModelosXDia.Where(x => x.Mes == mes && x.Dia == dia && x.ModeloID == modeloId).ToList();
                        if(dataRow != null && dataRow.Count > 0)
                        {
                            cell.Text = dataRow.Sum(x => x.UnidadesRentadas).ToString();
                        }                            
                    }
                }
                else
                {
                    cell.Text = "-1";
                }
            }
            if(sucursal != 0)
            {
                var subtotalSucursal = data.SubTotalXModelo.FirstOrDefault(x => x.Mes == mes && x.SucursalID == sucursal && x.ModeloID == modeloId);
                this.xrtDiasXUnidad.Text = subtotalSucursal.DiasXUnidad.ToString();
                this.xrtDiasObjetivo.Text = subtotalSucursal.DiasObjetivo.ToString();
                this.xrtDiasRenta.Text = subtotalSucursal.DiasRenta.ToString();
                this.xrtPorcentajeUtilizacion.Text = subtotalSucursal.PorcentajeUtilizacion.ToString();
            }
            else
            {
                var subTotalMes = data.SubTotalXModelo.Where(x => x.Mes == mes && x.ModeloID == modeloId).ToList();
                Decimal sumaDiasRenta = Decimal.Parse(subTotalMes.Sum(x=>x.DiasRenta).ToString());
                Decimal sumaDiasObjetivo = Decimal.Parse(subTotalMes.Sum(x => x.DiasObjetivo).ToString());
                Decimal sumaDiasXUnidad = Decimal.Parse(subTotalMes.Sum(x => x.DiasXUnidad).ToString());

                Decimal porcentaje = sumaDiasXUnidad != 0M ? sumaDiasRenta / sumaDiasXUnidad : 0M;

                this.xrtDiasXUnidad.Text = sumaDiasXUnidad.ToString();
                this.xrtDiasObjetivo.Text = sumaDiasObjetivo.ToString();
                this.xrtDiasRenta.Text = sumaDiasRenta.ToString();
                this.xrtPorcentajeUtilizacion.Text = Math.Round(porcentaje*100).ToString();
            }
        }

        /// <summary>
        /// Evento ejecutado enviar el dataset a los subreportes
        /// </summary>
        private void sbrptTotalesMes_BeforePrint(object sender, PrintEventArgs e)
        {
            XRSubreport reporteSucursal = sender as XRSubreport;
            if(reporteSucursal.ReportSource == null)
                return;
            reporteSucursal.ReportSource.DataSource = this.DataSource;

            var mes = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Mes") : Mes.Value;
            if(UsarSucursal.Value)
            {
                var sucursalId = this.GetCurrentColumnValue<int>("SucursalID");
                RentaDiariaGeneralSucursalModeloPSLRPT report = this.sbrptTotalesMes.ReportSource as RentaDiariaGeneralSucursalModeloPSLRPT;
                report.SetFilter(sucursalId, mes);
                reporteSucursal.ReportSource.FilterString = String.Format("[Mes] = {0} AND [SucursalID] = {1}", mes, sucursalId);
            }
        }

        /// <summary>
        /// Evento ejecutado para calcular los totales por dia en el mes que se esta presentando.
        /// </summary>
        private void GroupFooter1_BeforePrint(object sender, PrintEventArgs e)
        {
            var mes = UsarSucursal.Value == true ? this.GetCurrentColumnValue<int>("Mes") : Mes.Value;
            var anio = Anio.Value;
            Int32? sucursal = 0;
            if(UsarSucursal.Value)
                sucursal = this.GetCurrentColumnValue<int>("SucursalID");
            var diasEnMes = GetDaysInMonth(mes, anio);

            RentasSucursalDS data = this.DataSource as RentasSucursalDS;
            XRTableRow tableRowTotales = this.xrTable4.Rows.LastRow;

            for(int i = 1; i < diasEnMes + 1; i++)
            {
                var cellTotal = tableRowTotales.Cells[String.Format("xrtTotalFlotaDayCell{0}", i)] as XRTableCell;
                var dia = i;
                //if(dia > 0)
                //{
                    var dataRowTotal = sucursal != 0 ? data.ModelosXDia.Where(x => x.Mes == mes && x.Dia == dia && x.SucursalID == sucursal).ToList() : data.ModelosXDia.Where(x => x.Mes == mes && x.Dia == dia).ToList();
                    if(dataRowTotal != null && dataRowTotal.Count > 0)
                    {
                        cellTotal.Text = dataRowTotal.Sum(x => x.UnidadesRentadas).ToString();
                    }
                    //if(sucursal != 0)
                    //{
                    //    var rowTotal = data.ModelosXDia.Where(x => x.Mes == mes && x.Dia == dia && x.SucursalID == sucursal).ToList();
                    //    cellTotal.Text = rowTotal.Sum(x => x.UnidadesRentadas).ToString();
                    //}
                    //else
                    //{
                    //    var rowTotal = data.ModelosXDia.Where(x => x.Mes == mes && x.Dia == dia).ToList();
                    //    cellTotal.Text = rowTotal.Sum(x => x.UnidadesRentadas).ToString();
                    //}
                //}
                //else
                //{
                //    cellTotal.Text = "-1";
                //}
            }

            #region Calculo Totales
            List<RentasSucursalDS.SubTotalXModeloRow> lista = sucursal != 0 ? data.SubTotalXModelo.Where(x => x.Mes == mes && x.SucursalID == sucursal).ToList() : data.SubTotalXModelo.Where(x => x.Mes == mes).ToList();

            var diasRenta = Decimal.Parse(lista.Sum(x => x.DiasRenta).ToString());
            var diasUnidad = Decimal.Parse(lista.Sum(x => x.DiasXUnidad).ToString());
            var diasObjetivo = Decimal.Parse(lista.Sum(x => x.DiasObjetivo).ToString());

            Int32 porcentajeTotal = 0;

            if(diasUnidad != 0)
            {
                var resultado = Math.Round((diasRenta / diasUnidad) * 100);
                porcentajeTotal = (Int32)resultado;
            }

            xrTTotalPorcentajeUtilizacion.Text = porcentajeTotal.ToString();

            this.xrTTotalDiasObjetivo.Text = diasObjetivo.ToString();
            this.xrTTotalDiasXUnidad.Text = diasUnidad.ToString();
            this.xrTTotalDiasRenta.Text = diasRenta.ToString();
            #endregion

            RentaDiariaGeneralSucursalModeloPSLRPT reporte = (this.sbrptTotalesMes.ReportSource = new RentaDiariaGeneralSucursalModeloPSLRPT()) as RentaDiariaGeneralSucursalModeloPSLRPT;
            reporte.diasMesWidth = diasMesEncabezadosSize;
            reporte.Mes = mes;
            reporte.Anio = anio;
            reporte.PorcentajeTotal = porcentajeTotal;
            reporte.UsarSucursal = UsarSucursal;
            reporte.CreateNewColumns();
            if(this.DataMember == "Modelos")
            {
                reporte.DataMember = "Modelos";
                reporte.DataSource = this.DataSource;
            }
        }
        #endregion
    }
}
