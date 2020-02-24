//Satisface al caso de uso CU071 - Reporte de Auditorias Realizadas
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// reporte de auditoria realizado
    /// </summary>
    public partial class ReporteAuditoriasRealizadasRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        public ReporteAuditoriasRealizadasRPT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor del reporte con informacion del reporte.
        /// </summary>
        public ReporteAuditoriasRealizadasRPT(Dictionary<string, object> datos) : this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new Exception("La fuente de datos no puede ser nula.");

            this.DataSource = datos["DataSource"];
        }
        /// <summary>
        /// Evento que se ejecuta antes de visualiza la imagen que representa el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as ReporteAuditoriasRealizadasDS).ConfiguracionesSistema.AsEnumerable()
                                                    .Select(x => x.UrlLogoEmpresa).FirstOrDefault();
            this.pbLogo.ImageUrl = url;
        }
        /// <summary>
        /// Group header de sucursal
        /// </summary>
        private void ghSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }
        /// <summary>
        /// Evento de presentacion de los subreportes
        /// </summary>
        private void xrsTecnicos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            var auditoriasRealizadas = (this.DataSource as ReporteAuditoriasRealizadasDS);
            var tecnicos = Enumerable.AsEnumerable(auditoriasRealizadas.ReporteAuditoriasRealizadasDA).Where(x => x.SucursalID == sucursalID).ToList();
            var tecnicosId = tecnicos.Select(x => x.TecnicoID).Distinct().ToList();

            XRSubreport subReporte = sender as XRSubreport;
            if (subReporte.ReportSource == null)
                return;
            
            string filtroTecnicos = "";
            foreach (int tecnicoID in tecnicosId)
            {
                if(String.IsNullOrEmpty(filtroTecnicos))
                    filtroTecnicos = filtroTecnicos + " [TecnicoID] = " + tecnicoID.ToString() + " ";
                else
                    filtroTecnicos = filtroTecnicos + " OR [TecnicoID] = " + tecnicoID.ToString() + " ";
            }
            filtroTecnicos = "( " + filtroTecnicos + " ) ";

            subReporte.ReportSource.DataSource = this.DataSource;
            subReporte.ReportSource.FilterString = String.Format("[SucursalID] = {0} AND {1} ", sucursalID, filtroTecnicos);

        }
        /// <summary>
        /// Evento Before Print
        /// </summary>
        private void gfSucursal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            var auditoriasRealizadas = (this.DataSource as ReporteAuditoriasRealizadasDS);
            var auditorias = Enumerable.AsEnumerable(auditoriasRealizadas.ReporteAuditoriasRealizadasDA).Where(x => x.SucursalID == sucursalID).ToList();

            double totalAuditoriasRealizadas = 0;
            double numeroTecnicosAuditados = 0;
            double promedioAuditoriasTecnico = 0;
            List<double> calificaciones = new List<double>();
            double promedioCalificacionGeneral = 0;

            foreach (var row in auditorias)
            {
                double cantidadTotal = double.Parse(row.CantidadAuditoria.ToString());
                double cantidadResultado = double.Parse(row.ResultadoAuditoria.ToString());

                var calificacion = Math.Round(((cantidadResultado * 100) / cantidadTotal), 2);
                calificaciones.Add(calificacion);
            }

            totalAuditoriasRealizadas = auditorias.Select(x=>x.AuditoriaMantenimientoID).Distinct().Count();
            numeroTecnicosAuditados = auditorias.Select(x => x.TecnicoID).Distinct().Count();
            promedioAuditoriasTecnico = numeroTecnicosAuditados > 0 ? Math.Round(((totalAuditoriasRealizadas/numeroTecnicosAuditados)*100)/totalAuditoriasRealizadas,2) : 0;
            promedioCalificacionGeneral = Math.Round(calificaciones.Sum() / auditorias.Count, 0);

            this.xrtTotalAuditoriasSucursal.Text = totalAuditoriasRealizadas.ToString();
            this.xrtTecnicosAuditados.Text = numeroTecnicosAuditados.ToString();
            this.xrtPromedioTecnicos.Text = promedioAuditoriasTecnico.ToString() + " %";
            this.xrtPromedioCalificacion.Text = promedioCalificacionGeneral.ToString();            
        }
        /// <summary>
        /// Evento de impresion del report footer
        /// </summary>
        private void rfGeneral_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var auditoriasRealizadas = (this.DataSource as ReporteAuditoriasRealizadasDS);
            var auditorias = Enumerable.AsEnumerable(auditoriasRealizadas.ReporteAuditoriasRealizadasDA).ToList();

            double totalAuditoriasRealizadas = 0;
            double numeroTecnicosAuditados = 0;
            double promedioAuditoriasTecnico = 0;
            List<double> calificaciones = new List<double>();
            double promedioCalificacionGeneral = 0;

            var auditoriasRealizadasId = auditorias.Select(x => x.AuditoriaMantenimientoID).Distinct().ToList();
            for (int i = 0; i < auditoriasRealizadasId.Count; i++)
            {
                var row = auditorias.FirstOrDefault(x=>x.AuditoriaMantenimientoID == auditoriasRealizadasId[i]);
            }

            foreach (var row in auditorias)
            {
                double cantidadTotal = double.Parse(row.CantidadAuditoria.ToString());
                double cantidadResultado = double.Parse(row.ResultadoAuditoria.ToString());

                var calificacion = Math.Round(((cantidadResultado * 100) / cantidadTotal), 2);
                calificaciones.Add(calificacion);
            }

            totalAuditoriasRealizadas = auditorias.Select(x => x.AuditoriaMantenimientoID).Distinct().Count();
            numeroTecnicosAuditados = auditorias.Select(x => x.TecnicoID).Distinct().Count();
            promedioAuditoriasTecnico = numeroTecnicosAuditados > 0 ? Math.Round(((totalAuditoriasRealizadas / numeroTecnicosAuditados) * 100) / totalAuditoriasRealizadas, 2) : 0;
            promedioCalificacionGeneral = Math.Round(calificaciones.Sum() / auditorias.Count, 0);

            this.xrtTotalGeneralAuditorias.Text = totalAuditoriasRealizadas.ToString();
            this.xrtTotalGeneralTecnicos.Text = numeroTecnicosAuditados.ToString();
            this.xrtTotalGeneralPromedioAuditorias.Text = promedioAuditoriasTecnico.ToString() + " %";
            this.xrtTotalGeneralPromedioCalificaciones.Text = promedioCalificacionGeneral.ToString();
        }
    }
}
