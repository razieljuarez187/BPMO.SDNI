using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BPMO.SDNI.Mantenimiento.Reportes.DA;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    public partial class ReporteMantenimientosRealizadosRPT : DevExpress.XtraReports.UI.XtraReport
    {
        private int? anioCurso;

        public ReporteMantenimientosRealizadosRPT()
        {
            InitializeComponent();
        }

        public ReporteMantenimientosRealizadosRPT(Dictionary<string, object> datos)
            : this()
        {
            if (!datos.ContainsKey("DataSource"))
            {
                throw new ArgumentException("datos.DataSource");
            }

            this.DataSource = datos["DataSource"];
            anioCurso = 0;
        }

        private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                    .Select(x => (String)x["URLLogoEmpresa"]).FirstOrDefault();
            this.pbLogo.ImageUrl = url;
        }

        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                   .Select(x => (String)x["URLLogoEmpresa"]).FirstOrDefault();
            this.pbLogo.ImageUrl = url;
        }

        private void xrLabelPeriodo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string InicioPeriodo = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                   .Select(x => x["FechaInicio"]).FirstOrDefault().ToString();

            string FinPeriodo = (this.DataSource as DataSet).Tables["ConfiguracionModulo"].AsEnumerable()
                                                   .Select(x => x["FechaFin"]).FirstOrDefault().ToString();

            if (InicioPeriodo != string.Empty && FinPeriodo != string.Empty)
                xrLabelPeriodo.Text = "De " + InicioPeriodo + " a " + FinPeriodo;
        }

        private void xrtbEnum_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
             ReporteMantenimientosRealizadosDS Fitro = this.DataSource as ReporteMantenimientosRealizadosDS;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var anio = this.anioCurso;
            string unidadID = GetCurrentColumnValue<string>("UnidadID");
            ReporteMantenimientosRealizadosDS dataset = this.DataSource as ReporteMantenimientosRealizadosDS;
            #region Seleccion de datos
            if (!dataset.EquipoAliado.Any(x => x.UnidadID.Equals(unidadID)))
            {
                this.xrsEquiposSemestre1.Visible = false;
                this.xrsEquiposSemestre2.Visible = false;
                
                this.xrtbEnum.HeightF = 208.5f;
                this.xrTableCell281.HeightF = 208.5f;
                this.xrTableCell281.HeightF = 208.5f;
                this.xrTable110.HeightF = 208.5f;
            }
            else
            {
                this.xrsEquiposSemestre1.Visible = true;
                this.xrsEquiposSemestre2.Visible = true;

                if (dataset.EquipoAliado.Count(x => x.UnidadID.Equals(unidadID)) > 1)
                {
                    int cantidadRowsAdicionales = (dataset.EquipoAliado.Count(x => x.UnidadID.Equals(unidadID)) - 1) * 2;
                    int tamanioIncremento = 39 * cantidadRowsAdicionales;

                    this.xrTable110.HeightF = (208.5f + tamanioIncremento);
                    this.xrtbEnum.HeightF = (208.5f + tamanioIncremento);
                    this.xrTableCell281.HeightF = (208.5f + tamanioIncremento);
                    this.xrTableCell281.HeightF = (208.5f + tamanioIncremento);
                }
            }
            #endregion

            string UnidadAnterior = string.Empty;

            #region Seleccion de Mantenimiento por mes
            foreach (var row in dataset.ServicioUnidad.Where(x => x.Anio == anioCurso.Value && x.UnidadID == unidadID)) {
                switch (row["Mes"].ToString())
                {
                    #region Enero
                    case "1":
                        xrTableCell19.Text = row.Taller;

                        xrtbTipoServicioEnero.Text = row.TipoServicio;
                        xrtbFechaEnero.Text = row.FechaServicio.ToString();
                        xrtbkmHrsEnero.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrtbTipoProxSerEne.Text = row.TipoProximoSer.ToString();
                            xrtbFechaProxSerEne.Text = row.FechaProximoSer.ToString();
                            xrtbKmHrsProxSerEne.Text = row._km_HrsProximoSer.ToString();
                        }
                        else 
                        {
                            xrtbTipoProxSerEne.Text = string.Empty;
                            xrtbFechaProxSerEne.Text = string.Empty;
                            xrtbKmHrsProxSerEne.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region Febrero
                    case "2":
                        xrTableCell103.Text = row.Taller;

                        xrTableCell105.Text = row.TipoServicio;
                        xrTableCell106.Text = row.FechaServicio.ToString();
                        xrTableCell107.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell108.Text = row.TipoProximoSer.ToString();
                            xrTableCell109.Text = row.FechaProximoSer.ToString();
                            xrTableCell110.Text = row._km_HrsProximoSer.ToString();
                        }
                        else 
                        {
                            xrTableCell108.Text = string.Empty;
                            xrTableCell109.Text = string.Empty;
                            xrTableCell110.Text = string.Empty;
 
                        }
                        break;
                    #endregion
                    #region Marzo
                    case "3":
                        xrTableCell119.Text = row.Taller;

                        xrTableCell121.Text = row.TipoServicio;
                        xrTableCell122.Text = row.FechaServicio.ToString();
                        xrTableCell123.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell124.Text = row.TipoProximoSer.ToString();
                            xrTableCell125.Text = row.FechaProximoSer.ToString();
                            xrTableCell126.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell124.Text = string.Empty;
                            xrTableCell125.Text = string.Empty;
                            xrTableCell126.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region Abril
                    case "4":
                        xrTableCell135.Text = row.Taller;

                        xrTableCell137.Text = row.TipoServicio;
                        xrTableCell138.Text = row.FechaServicio.ToString();
                        xrTableCell139.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell140.Text = row.TipoProximoSer.ToString();
                            xrTableCell141.Text = row.FechaProximoSer.ToString();
                            xrTableCell142.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell140.Text = string.Empty;
                            xrTableCell141.Text = string.Empty;
                            xrTableCell142.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region Mayo
                    case "5":
                        xrTableCell151.Text = row.Taller;

                        xrTableCell153.Text = row.TipoServicio;
                        xrTableCell154.Text = row.FechaServicio.ToString();
                        xrTableCell155.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell156.Text = string.Empty;
                            xrTableCell157.Text = string.Empty;
                            xrTableCell158.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region Junio
                    case "6":
                        xrTableCell167.Text = row.Taller;

                        xrTableCell169.Text = row.TipoServicio;
                        xrTableCell170.Text = row.FechaServicio.ToString();
                        xrTableCell171.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell172.Text = row.TipoProximoSer.ToString();
                            xrTableCell173.Text = row.FechaProximoSer.ToString();
                            xrTableCell174.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell172.Text = string.Empty;
                            xrTableCell173.Text = string.Empty;
                            xrTableCell174.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region Julio
                    case "7":
                        xrTableCell63.Text = row.Taller;

                        xrTableCell65.Text = row.TipoServicio;
                        xrTableCell66.Text = row.FechaServicio.ToString();
                        xrTableCell67.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell68.Text = row.TipoProximoSer.ToString();
                            xrTableCell69.Text = row.FechaProximoSer.ToString();
                            xrTableCell70.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell68.Text = string.Empty;
                            xrTableCell69.Text = string.Empty;
                            xrTableCell70.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region Agosto
                    case "8":
                        xrTableCell88.Text = row.Taller;

                        xrTableCell90.Text = row.TipoServicio;
                        xrTableCell91.Text = row.FechaServicio.ToString();
                        xrTableCell92.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell93.Text = row.TipoProximoSer.ToString();
                            xrTableCell94.Text = row.FechaProximoSer.ToString();
                            xrTableCell95.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {

                            xrTableCell93.Text = string.Empty;
                            xrTableCell94.Text = string.Empty;
                            xrTableCell95.Text = string.Empty;
 
                        }
                        break;
                    #endregion
                    #region Septiembre
                    case "9":
                        xrTableCell185.Text = row.Taller;

                        xrTableCell187.Text = row.TipoServicio;
                        xrTableCell188.Text = row.FechaServicio.ToString();
                        xrTableCell189.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell190.Text = row.TipoProximoSer.ToString();
                            xrTableCell191.Text = row.FechaProximoSer.ToString();
                            xrTableCell192.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell190.Text = string.Empty;
                            xrTableCell191.Text = string.Empty;
                            xrTableCell192.Text = string.Empty;
 
                        }
                        break;
                    #endregion
                    #region Octubre
                    case "10":
                        xrTableCell202.Text = row.Taller;

                        xrTableCell204.Text = row.TipoServicio;
                        xrTableCell205.Text = row.FechaServicio.ToString();
                        xrTableCell206.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell207.Text = row.TipoProximoSer.ToString();
                            xrTableCell208.Text = row.FechaProximoSer.ToString();
                            xrTableCell209.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell207.Text = string.Empty;
                            xrTableCell208.Text = string.Empty;
                            xrTableCell209.Text = string.Empty;
 
                        }
                        break;
                    #endregion
                    #region Noviembre
                    case "11":
                        xrTableCell219.Text = row.Taller;

                        xrTableCell221.Text = row.TipoServicio;
                        xrTableCell222.Text = row.FechaServicio.ToString();
                        xrTableCell223.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell224.Text = row.TipoProximoSer.ToString();
                            xrTableCell225.Text = row.FechaProximoSer.ToString();
                            xrTableCell226.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell224.Text = string.Empty;
                            xrTableCell225.Text = string.Empty;
                            xrTableCell226.Text = string.Empty;
 
                        }
                        break;
                    #endregion
                    #region Diciembre
                    case "12":
                        xrTableCell236.Text = row.Taller;

                        xrTableCell238.Text = row.TipoServicio;
                        xrTableCell239.Text = row.FechaServicio.ToString();
                        xrTableCell240.Text = row._Km_Hrs.ToString();
                        if (!row.IsFechaProximoSerNull())
                        {
                            xrTableCell241.Text = row.TipoProximoSer.ToString();
                            xrTableCell242.Text = row.FechaProximoSer.ToString();
                            xrTableCell243.Text = row._km_HrsProximoSer.ToString();
                        }
                        else
                        {
                            xrTableCell241.Text = string.Empty;
                            xrTableCell242.Text = string.Empty;
                            xrTableCell243.Text = string.Empty;
 
                        }
                        break;
                    #endregion
                }
            }
            #endregion
        }

        private void xrsEquiposSemestre1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string unidadID = GetCurrentColumnValue<string>("UnidadID");
            XRSubreport reporteSemestre1 = sender as XRSubreport;
            if (reporteSemestre1.ReportSource == null)
                return;

            var reporte = reporteSemestre1.ReportSource as ReporteMantenimientosRealizadosAliadosRPT;
            reporte.anioCurso = this.anioCurso;
            reporte.semestre = 1;

            reporteSemestre1.ReportSource.DataSource = this.DataSource;
            reporteSemestre1.ReportSource.FilterString = String.Format("[UnidadID] = {0}", unidadID);
        }

        private void xrsEquiposSemestre2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string unidadID = GetCurrentColumnValue<string>("UnidadID");
            XRSubreport reporteSemestre2 = sender as XRSubreport;
            if (reporteSemestre2.ReportSource == null)
                return;

            var reporte = reporteSemestre2.ReportSource as ReporteMantenimientosRealizadosAliadosRPT;
            reporte.anioCurso = this.anioCurso;
            reporte.semestre = 2;

            reporteSemestre2.ReportSource.DataSource = this.DataSource;
            reporteSemestre2.ReportSource.FilterString = String.Format("[UnidadID] = {0}", unidadID);
        }

        private void xrTableCell8_TextChanged(object sender, EventArgs e)
        {
            var cell = sender as XRTableCell;
            if(!String.IsNullOrEmpty(cell.Text))
                this.anioCurso = Convert.ToInt32(cell.Text);
        }

        private void Detail_AfterPrint(object sender, EventArgs e) {
            
        }
    }
}
