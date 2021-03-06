﻿//Satisface el CU016 – Reporte de Renta Diaria General
using System;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    /// <summary>
    /// Clase que imprime los encabezado por mes
    /// </summary>
    public partial class RentaDiariaGeneralMesSucursalRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Atributos
        /// <summary>
        /// Tamaño de la celda base que se divide en el número de días del mes a visualizar
        /// </summary>
        private double diasMesEncabezadosSize;
        #endregion

        #region Propiedades
        /// <summary>
        /// Determina si el subreporte es agrupado por sucursal
        /// </summary>
        public Boolean? UsarSucursal { get; set; }

        #endregion

        #region Metodos
        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public RentaDiariaGeneralMesSucursalRPT()
        {
            InitializeComponent();
            diasMesEncabezadosSize = xrtMesAnio.WidthF;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento ejecutado antes de que se imprima la seccion por Grupo
        /// </summary>
        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var mes = this.GetCurrentColumnValue<int>("Mes");
            var anio = this.GetCurrentColumnValue<int>("Anio");

            RentaDiariaGeneralMesSucursalElementoRPT reporte = (this.sbrptElementoHder.ReportSource = new RentaDiariaGeneralMesSucursalElementoRPT()) as RentaDiariaGeneralMesSucursalElementoRPT;
            reporte.diasMesEncabezadosSize = diasMesEncabezadosSize;
            reporte.Mes = mes;
            reporte.Anio = anio;
            reporte.CreateNewColumns();

            RentaDiariaGeneralModeloSucursalElementoRPT reportes = (this.sbrptElementoDetail.ReportSource = new RentaDiariaGeneralModeloSucursalElementoRPT()) as RentaDiariaGeneralModeloSucursalElementoRPT;
            reportes.diasMesEncabezadosSize = diasMesEncabezadosSize;
            reportes.Mes = mes;
            reportes.Anio = anio;
            reportes.UsarSucursal = UsarSucursal;
            reportes.CreateNewColumns();
            if(this.DataMember == "Meses")
            {
                reportes.DataMember = "Modelos";
                reportes.DataSource = this.DataSource;
            }
        }

        /// <summary>
        /// Evento ejecutado para obtener el nombre del Mes
        /// </summary>
        private void xrtMesNombre_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var mes = this.GetCurrentColumnValue<int>("Mes");

            ReporteRDSucursalDS dataSource = this.DataSource as ReporteRDSucursalDS;
            var meses = dataSource.Meses.First(x => x.Mes == mes);
            xrtMesNombre.Text = meses.Nombre;
        }

        /// <summary>
        /// Evento ejecutado para obtener la concatenacion de año con mes
        /// </summary>
        private void xrtMesAnio_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var mes = this.GetCurrentColumnValue<int>("Mes");
            ReporteRDSucursalDS dataSource = this.Report.DataSource as ReporteRDSucursalDS;
            var row = dataSource.Meses.First(x => x.Mes == mes);
            xrtMesAnio.Text = row.Nombre + " - " + row.Anio;
        }

        /// <summary>
        /// Evento ejecutado para llamar a los subreportes con los filtros
        /// </summary>
        private void sbrptElementoDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteSucursal = sender as XRSubreport;
            if(reporteSucursal.ReportSource == null)
                return;
            reporteSucursal.ReportSource.DataSource = this.DataSource;

            var mes = this.GetCurrentColumnValue<int>("Mes");
            if(UsarSucursal.Value)
            {
                var sucursalId = this.GetCurrentColumnValue<int>("SucursalID");

                reporteSucursal.ReportSource.FilterString = String.Format("[Mes] = {0} AND [SucursalID] = {1}", mes, sucursalId);
            }
        }
        #endregion
    }
}
