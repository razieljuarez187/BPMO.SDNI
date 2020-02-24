using System.Collections.Generic;
using BPMO.SDNI.Mantenimiento.BO;
using BPMO.SDNI.Mantenimiento.BO.BOF;

namespace BPMO.SDNI.Mantenimiento.RPT {
    /// <summary>
    /// Reporte Mantenimiento Realizado Contra Programado
    /// </summary>
    public partial class MantenimientoUnidadesRPT : DevExpress.XtraReports.UI.XtraReport {
        public MantenimientoUnidadesRPT(List<MantenimientoBOF> listMantenimientos) {
            InitializeComponent();
            EnlazarControles(listMantenimientos);
        }
        /// <summary>
        /// Enlaza los campos de detalle con el origen de datos
        /// </summary>
        /// <param name="listMantenimientos"></param>
        private void EnlazarControles(List<MantenimientoBOF> listMantenimientos) {
            string nameRpt = string.Empty;
            this.DataSource = listMantenimientos;
            ExportOptions.Xls.SheetName = "Mantenimiento Unidades";
            ExportOptions.Xlsx.SheetName = "Mantenimiento Unidades";
            //Nombre del archivo
            this.DisplayName = nameRpt.Replace(' ', '_');
            
            #region Detail: Reporte            
            this.tdNoSerie.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.Vin");
            this.tdNoEconomico.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.NumeroEconomico");
            this.tdModelo.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.Unidad.Modelo.Nombre");
            this.tdCliente.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.Unidad.Cliente.Nombre");
            this.tdControlista.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.Controlista.NombreCorto");
            this.tdOperador.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.Operador");
            this.tdReporteOperador.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.ObservacionesOperador");
            this.tdProgramacionFecha.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.MantenimientoProgramado.Fecha");
            this.tdProgramacionKms.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.MantenimientoProgramado.Km");
            this.tdProgramacionHrs.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.MantenimientoProgramado.Horas");
            this.tdProgramacionTipoServicio.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.MantenimientoProgramado.TipoMantenimiento");
            this.tdEntradaFecha.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.IngresoUnidad.FechaIngreso");
            this.tdEntradaKms.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.KilometrajeEntrada");
            this.tdEntradaHrs.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.HorasEntrada");
            this.tdEntradaTipoServicio.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.TipoMantenimiento");
            this.tdDiasDiferencia.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.DiasDiferencia");
            this.tdOrdenServicio.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.OrdenServicio.Id");
            this.tdEstatus.DataBindings.Add("Text", DataSource, "MantenimientoUnidad.OrdenServicio.Estatus.Nombre");
            #endregion /Detail: Reporte
        }
        /// <summary>
        /// Establecer si fue impreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tdImpreso_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            try {
                MantenimientoBOF bo = this.GetCurrentRow() as MantenimientoBOF;
                tdImpreso.Text = bo.MantenimientoUnidad != null && bo.MantenimientoUnidad.FechaSalida != null ? "SÍ" : "NO";
            } catch {
                tdImpreso.Text = "--";
            }
        }
    }
}
