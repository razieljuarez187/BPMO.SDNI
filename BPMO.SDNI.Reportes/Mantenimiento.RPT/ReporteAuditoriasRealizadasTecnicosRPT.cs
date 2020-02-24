//Satisface al caso de uso CU071 - Reporte de Auditorias Realizadas
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Reporte de auditorias realizadas
    /// </summary>
    public partial class ReporteAuditoriasRealizadasTecnicosRPT : XtraReport
    {
        #region Constructor
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        public ReporteAuditoriasRealizadasTecnicosRPT()
        {
            InitializeComponent();
        }
        #endregion
        #region Eventos
        /// <summary>
        /// Evento para el detalle
        /// </summary>
        private void dtlTecnicos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");
            int tecnicoID = this.GetCurrentColumnValue<int>("TecnicoID");
            int ordenServicioID = this.GetCurrentColumnValue<int>("OrdenServicioID");

            var auditoriasRealizadas = (this.DataSource as ReporteAuditoriasRealizadasDS);
            var row = Enumerable.AsEnumerable(auditoriasRealizadas.ReporteAuditoriasRealizadasDA).FirstOrDefault(x => x.SucursalID == sucursalID && x.TecnicoID == tecnicoID && x.OrdenServicioID == ordenServicioID);

            double cantidadTotal = double.Parse(row.CantidadAuditoria.ToString());
            double cantidadResultado = double.Parse(row.ResultadoAuditoria.ToString());

            if (cantidadTotal == 0)
            {
                this.xrtCalificacion.Text = "0 %";
            }
            else
            {
                var calificacion = Math.Round(((cantidadResultado * 100) / cantidadTotal), 2);
                this.xrtCalificacion.Text = Math.Round(calificacion, 0).ToString() + " %";
            }

            this.xrtcOrdenServicio.Text = row.OrdenServicioID.ToString();
            this.xrtcFechaAuditoria.Text = row.FechaAuditoria.ToString();
            this.xrtcObservaciones.Text = row.Observaciones;
        }
        /// <summary>
        /// Evento de la banda group footer
        /// </summary>
        private void gfTecnicos_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int tecnicoID = this.GetCurrentColumnValue<int>("TecnicoID");
            int sucursalID = this.GetCurrentColumnValue<int>("SucursalID");

            var auditoriasRealizadas = (this.DataSource as ReporteAuditoriasRealizadasDS);
            var rows = Enumerable.AsEnumerable(auditoriasRealizadas.ReporteAuditoriasRealizadasDA).Where(x => x.SucursalID == sucursalID && x.TecnicoID == tecnicoID).ToList();
            if (rows.Count > 0)
            {
                this.xrlAuditoriasRealizadas.Text = rows.Count.ToString();
                List<double> calificaciones = new List<double>();
                foreach (var row in rows)
                {
                    double cantidadTotal = double.Parse(row.CantidadAuditoria.ToString());
                    double cantidadResultado = double.Parse(row.ResultadoAuditoria.ToString());

                    var calificacion = Math.Round(((cantidadResultado * 100) / cantidadTotal), 2);
                    calificaciones.Add(calificacion);
                }
                var promedio = Math.Round(calificaciones.Sum() / rows.Count, 0);
                this.xrlPromedioAuditorias.Text = promedio.ToString() + " %";
            }
            else
            {
                this.xrlAuditoriasRealizadas.Text = "0";
                this.xrlPromedioAuditorias.Text = "0 %";
            }
        }
        #endregion
    }
}