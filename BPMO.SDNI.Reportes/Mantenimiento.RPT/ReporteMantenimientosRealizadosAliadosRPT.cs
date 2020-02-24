using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using System.Drawing.Printing;
using BPMO.SDNI.Mantenimiento.Reportes.DA;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Subreporte para presentacion de servicios realizados a mantenimientos aliados.
    /// </summary>
    public partial class ReporteMantenimientosRealizadosAliadosRPT : XtraReport
    {
        /// <summary>
        /// Anio en curso para la impresion de los mantenimientos
        /// </summary>
        public int? anioCurso;
        /// <summary>
        /// Semestre que se va a presentar
        /// </summary>
        public int? semestre;
        /// <summary>
        /// Constructor de inicio del reporte
        /// </summary>
        public ReporteMantenimientosRealizadosAliadosRPT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Evento de impresion de la banda de detalles
        /// </summary>
        private void Detail_BeforePrint(object sender, PrintEventArgs e)
        {
            var equipoAliadoID = GetCurrentColumnValue<string>("AliadoID");
            ReporteMantenimientosRealizadosDS dataSource = this.DataSource as ReporteMantenimientosRealizadosDS;

            foreach (var row in dataSource.ServicioAliado.Where(x => x.Anio == anioCurso.Value && x.AliadoID == equipoAliadoID))
            {
                if (this.semestre != null)
                {
                    if (this.semestre == 1)
                    {
                        //#region Enero
                        //xrTableCell22.Text = string.Empty;
                        //xrtbTipoSerAliaEne.Text = string.Empty;
                        //xrtbFechaSerAliaEne.Text = string.Empty;
                        //xrtbKmHrsAliaEne.Text = string.Empty;

                        //xrtbTipoSerProxAliaEne.Text = string.Empty;
                        //xrtbFechaProxAliaEne.Text = string.Empty;
                        //xrtbFechaProxAliaEne.Text = string.Empty;

                        //#endregion
                        //#region Febrero

                        //xrTableCell1.Text = string.Empty;

                        //xrTableCell8.Text = string.Empty;
                        //xrTableCell9.Text = string.Empty;
                        //xrTableCell10.Text = string.Empty;

                        //xrTableCell4.Text = string.Empty;
                        //xrTableCell5.Text = string.Empty;
                        //xrTableCell6.Text = string.Empty;

                        //#endregion
                        //#region Marzo

                        //xrTableCell11.Text = string.Empty;

                        //xrTableCell16.Text = string.Empty;
                        //xrTableCell17.Text = string.Empty;
                        //xrTableCell18.Text = string.Empty;

                        //xrTableCell13.Text = string.Empty;
                        //xrTableCell14.Text = string.Empty;
                        //xrTableCell15.Text = string.Empty;

                        //#endregion
                        //#region Abril

                        //xrTableCell9.Text = string.Empty;

                        //xrTableCell27.Text = string.Empty;
                        //xrTableCell28.Text = string.Empty;
                        //xrTableCell29.Text = string.Empty;

                        //xrTableCell24.Text = string.Empty;
                        //xrTableCell25.Text = string.Empty;
                        //xrTableCell26.Text = string.Empty;

                        //#endregion
                        //#region Mayo

                        //xrTableCell12.Text = string.Empty;

                        //xrTableCell35.Text = string.Empty;
                        //xrTableCell36.Text = string.Empty;
                        //xrTableCell37.Text = string.Empty;

                        //xrTableCell32.Text = string.Empty;
                        //xrTableCell33.Text = string.Empty;
                        //xrTableCell34.Text = string.Empty;

                        //#endregion
                        //#region Junio

                        //xrTableCell15.Text = string.Empty;

                        //xrTableCell43.Text = string.Empty;
                        //xrTableCell44.Text = string.Empty;
                        //xrTableCell45.Text = string.Empty;

                        //xrTableCell40.Text = string.Empty;
                        //xrTableCell41.Text = string.Empty;
                        //xrTableCell42.Text = string.Empty;

                        //#endregion
                        
                        switch (row["Mes"].ToString())
                        {
                            #region Enero
                            case "1":
                                xrTableCell22.Text = row.Taller;

                                xrtbTipoSerAliaEne.Text = row.TipoServicio;
                                xrtbFechaSerAliaEne.Text = row.FechaServicio.ToString();
                                xrtbKmHrsAliaEne.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrtbTipoSerProxAliaEne.Text = row.TipoProximoSer.ToString();
                                    xrtbFechaProxAliaEne.Text = row.FechaProximoSer.ToString();
                                    xrtbFechaProxAliaEne.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Febrero
                            case "2":
                                xrTableCell1.Text = row.Taller;

                                xrTableCell8.Text = row.TipoServicio;
                                xrTableCell9.Text = row.FechaServicio.ToString();
                                xrTableCell10.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell4.Text = row.TipoProximoSer.ToString();
                                    xrTableCell5.Text = row.FechaProximoSer.ToString();
                                    xrTableCell6.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Marzo
                            case "3":
                                xrTableCell11.Text = row.Taller;

                                xrTableCell16.Text = row.TipoServicio;
                                xrTableCell17.Text = row.FechaServicio.ToString();
                                xrTableCell18.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell13.Text = row.TipoProximoSer.ToString();
                                    xrTableCell14.Text = row.FechaProximoSer.ToString();
                                    xrTableCell15.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Abril
                            case "4":
                                xrTableCell9.Text = row.Taller;

                                xrTableCell27.Text = row.TipoServicio;
                                xrTableCell28.Text = row.FechaServicio.ToString();
                                xrTableCell29.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell24.Text = row.TipoProximoSer.ToString();
                                    xrTableCell25.Text = row.FechaProximoSer.ToString();
                                    xrTableCell26.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Mayo
                            case "5":
                                xrTableCell12.Text = row.Taller;

                                xrTableCell35.Text = row.TipoServicio;
                                xrTableCell36.Text = row.FechaServicio.ToString();
                                xrTableCell37.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell32.Text = row.TipoProximoSer.ToString();
                                    xrTableCell33.Text = row.FechaProximoSer.ToString();
                                    xrTableCell34.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Junio
                            case "6":
                                xrTableCell15.Text = row.Taller;

                                xrTableCell43.Text = row.TipoServicio;
                                xrTableCell44.Text = row.FechaServicio.ToString();
                                xrTableCell45.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell40.Text = row.TipoProximoSer.ToString();
                                    xrTableCell41.Text = row.FechaProximoSer.ToString();
                                    xrTableCell42.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                        }
                    }
                    else
                    {

                        //#region Julio

                        //xrTableCell22.Text = string.Empty;

                        //xrtbTipoSerAliaEne.Text = string.Empty;
                        //xrtbFechaSerAliaEne.Text = string.Empty;
                        //xrtbKmHrsAliaEne.Text = string.Empty;

                        //xrtbTipoSerProxAliaEne.Text = string.Empty;
                        //xrtbFechaProxAliaEne.Text = string.Empty;
                        //xrtbFechaProxAliaEne.Text = string.Empty;

                        //#endregion
                        //#region Agosto

                        //xrTableCell1.Text = string.Empty;

                        //xrTableCell8.Text = string.Empty;
                        //xrTableCell9.Text = string.Empty;
                        //xrTableCell10.Text = string.Empty;

                        //xrTableCell4.Text = string.Empty;
                        //xrTableCell5.Text = string.Empty;
                        //xrTableCell6.Text = string.Empty;

                        //#endregion
                        //#region Septiembre

                        //xrTableCell11.Text = string.Empty;

                        //xrTableCell16.Text = string.Empty;
                        //xrTableCell17.Text = string.Empty;
                        //xrTableCell18.Text = string.Empty;

                        //xrTableCell13.Text = string.Empty;
                        //xrTableCell14.Text = string.Empty;
                        //xrTableCell15.Text = string.Empty;

                        //#endregion
                        //#region Octubre

                        //xrTableCell9.Text = string.Empty;

                        //xrTableCell27.Text = string.Empty;
                        //xrTableCell28.Text = string.Empty;
                        //xrTableCell29.Text = string.Empty;

                        //xrTableCell24.Text = string.Empty;
                        //xrTableCell25.Text = string.Empty;
                        //xrTableCell26.Text = string.Empty;

                        //#endregion
                        //#region Noviembre

                        //xrTableCell12.Text = string.Empty;

                        //xrTableCell35.Text = string.Empty;
                        //xrTableCell36.Text = string.Empty;
                        //xrTableCell37.Text = string.Empty;

                        //xrTableCell32.Text = string.Empty;
                        //xrTableCell33.Text = string.Empty;
                        //xrTableCell34.Text = string.Empty;

                        //#endregion
                        //#region Diciembre

                        //xrTableCell38.Text = string.Empty;

                        //xrTableCell43.Text = string.Empty;
                        //xrTableCell44.Text = string.Empty;
                        //xrTableCell45.Text = string.Empty;

                        //xrTableCell40.Text = string.Empty;
                        //xrTableCell41.Text = string.Empty;
                        //xrTableCell42.Text = string.Empty;

                        //#endregion
                         
                        switch (row["Mes"].ToString())
                        {
                            #region Julio
                            case "7":
                                xrTableCell22.Text = row.Taller;

                                xrtbTipoSerAliaEne.Text = row.TipoServicio;
                                xrtbFechaSerAliaEne.Text = row.FechaServicio.ToString();
                                xrtbKmHrsAliaEne.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrtbTipoSerProxAliaEne.Text = row.TipoProximoSer.ToString();
                                    xrtbFechaProxAliaEne.Text = row.FechaProximoSer.ToString();
                                    xrtbFechaProxAliaEne.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Agosto
                            case "8":
                                xrTableCell1.Text = row.Taller;

                                xrTableCell8.Text = row.TipoServicio;
                                xrTableCell9.Text = row.FechaServicio.ToString();
                                xrTableCell10.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell4.Text = row.TipoProximoSer.ToString();
                                    xrTableCell5.Text = row.FechaProximoSer.ToString();
                                    xrTableCell6.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Septiembre
                            case "9":
                                xrTableCell11.Text = row.Taller;

                                xrTableCell16.Text = row.TipoServicio;
                                xrTableCell17.Text = row.FechaServicio.ToString();
                                xrTableCell18.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell13.Text = row.TipoProximoSer.ToString();
                                    xrTableCell14.Text = row.FechaProximoSer.ToString();
                                    xrTableCell15.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Octubre
                            case "10":
                                xrTableCell9.Text = row.Taller;

                                xrTableCell27.Text = row.TipoServicio;
                                xrTableCell28.Text = row.FechaServicio.ToString();
                                xrTableCell29.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell24.Text = row.TipoProximoSer.ToString();
                                    xrTableCell25.Text = row.FechaProximoSer.ToString();
                                    xrTableCell26.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Noviembre
                            case "11":
                                xrTableCell12.Text = row.Taller;

                                xrTableCell35.Text = row.TipoServicio;
                                xrTableCell36.Text = row.FechaServicio.ToString();
                                xrTableCell37.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell32.Text = row.TipoProximoSer.ToString();
                                    xrTableCell33.Text = row.FechaProximoSer.ToString();
                                    xrTableCell34.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                            #region Diciembre
                            case "12":
                                xrTableCell38.Text = row.Taller;

                                xrTableCell43.Text = row.TipoServicio;
                                xrTableCell44.Text = row.FechaServicio.ToString();
                                xrTableCell45.Text = row._Km_Hrs.ToString();
                                if (!row.IsFechaProximoSerNull())
                                {
                                    xrTableCell40.Text = row.TipoProximoSer.ToString();
                                    xrTableCell41.Text = row.FechaProximoSer.ToString();
                                    xrTableCell42.Text = row._km_HrsProximoSer.ToString();
                                }
                                break;
                            #endregion
                        }
                    }
                }
            }
        }

        private void Detail_AfterPrint(object sender, EventArgs e)
        {
            #region MyRegion

            #region Enero
            xrTableCell22.Text = string.Empty;
            xrtbTipoSerAliaEne.Text = string.Empty;
            xrtbFechaSerAliaEne.Text = string.Empty;
            xrtbKmHrsAliaEne.Text = string.Empty;

            xrtbTipoSerProxAliaEne.Text = string.Empty;
            xrtbFechaProxAliaEne.Text = string.Empty;
            xrtbFechaProxAliaEne.Text = string.Empty;

            #endregion
            #region Febrero

            xrTableCell1.Text = string.Empty;

            xrTableCell8.Text = string.Empty;
            xrTableCell9.Text = string.Empty;
            xrTableCell10.Text = string.Empty;

            xrTableCell4.Text = string.Empty;
            xrTableCell5.Text = string.Empty;
            xrTableCell6.Text = string.Empty;

            #endregion
            #region Marzo

            xrTableCell11.Text = string.Empty;

            xrTableCell16.Text = string.Empty;
            xrTableCell17.Text = string.Empty;
            xrTableCell18.Text = string.Empty;

            xrTableCell13.Text = string.Empty;
            xrTableCell14.Text = string.Empty;
            xrTableCell15.Text = string.Empty;

            #endregion
            #region Abril

            xrTableCell9.Text = string.Empty;

            xrTableCell27.Text = string.Empty;
            xrTableCell28.Text = string.Empty;
            xrTableCell29.Text = string.Empty;

            xrTableCell24.Text = string.Empty;
            xrTableCell25.Text = string.Empty;
            xrTableCell26.Text = string.Empty;

            #endregion
            #region Mayo

            xrTableCell12.Text = string.Empty;

            xrTableCell35.Text = string.Empty;
            xrTableCell36.Text = string.Empty;
            xrTableCell37.Text = string.Empty;

            xrTableCell32.Text = string.Empty;
            xrTableCell33.Text = string.Empty;
            xrTableCell34.Text = string.Empty;

            #endregion
            #region Junio

            xrTableCell15.Text = string.Empty;

            xrTableCell43.Text = string.Empty;
            xrTableCell44.Text = string.Empty;
            xrTableCell45.Text = string.Empty;

            xrTableCell40.Text = string.Empty;
            xrTableCell41.Text = string.Empty;
            xrTableCell42.Text = string.Empty;

            #endregion

            #region Julio

            xrTableCell22.Text = string.Empty;

            xrtbTipoSerAliaEne.Text = string.Empty;
            xrtbFechaSerAliaEne.Text = string.Empty;
            xrtbKmHrsAliaEne.Text = string.Empty;

            xrtbTipoSerProxAliaEne.Text = string.Empty;
            xrtbFechaProxAliaEne.Text = string.Empty;
            xrtbFechaProxAliaEne.Text = string.Empty;

            #endregion
            #region Agosto

            xrTableCell1.Text = string.Empty;

            xrTableCell8.Text = string.Empty;
            xrTableCell9.Text = string.Empty;
            xrTableCell10.Text = string.Empty;

            xrTableCell4.Text = string.Empty;
            xrTableCell5.Text = string.Empty;
            xrTableCell6.Text = string.Empty;

            #endregion
            #region Septiembre

            xrTableCell11.Text = string.Empty;

            xrTableCell16.Text = string.Empty;
            xrTableCell17.Text = string.Empty;
            xrTableCell18.Text = string.Empty;

            xrTableCell13.Text = string.Empty;
            xrTableCell14.Text = string.Empty;
            xrTableCell15.Text = string.Empty;

            #endregion
            #region Octubre

            xrTableCell9.Text = string.Empty;

            xrTableCell27.Text = string.Empty;
            xrTableCell28.Text = string.Empty;
            xrTableCell29.Text = string.Empty;

            xrTableCell24.Text = string.Empty;
            xrTableCell25.Text = string.Empty;
            xrTableCell26.Text = string.Empty;

            #endregion
            #region Noviembre

            xrTableCell12.Text = string.Empty;

            xrTableCell35.Text = string.Empty;
            xrTableCell36.Text = string.Empty;
            xrTableCell37.Text = string.Empty;

            xrTableCell32.Text = string.Empty;
            xrTableCell33.Text = string.Empty;
            xrTableCell34.Text = string.Empty;

            #endregion
            #region Diciembre

            xrTableCell38.Text = string.Empty;

            xrTableCell43.Text = string.Empty;
            xrTableCell44.Text = string.Empty;
            xrTableCell45.Text = string.Empty;

            xrTableCell40.Text = string.Empty;
            xrTableCell41.Text = string.Empty;
            xrTableCell42.Text = string.Empty;

            #endregion

            #endregion
          
        }

    
                 
        
    }
}
