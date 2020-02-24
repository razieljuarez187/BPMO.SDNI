using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    public partial class AniejamientoFlotaRPT : XtraReport
    {
        #region Atributos
        /// <summary>
        /// Ancho de la columna donde se pintaran los años
        /// </summary>
        private double AniosWidth;
        /// <summary>
        /// Valor usado para definir si el reporte a presentar es completo
        /// </summary>
        private bool? ReporteDetallado;
        #endregion

        #region Metodos
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        public AniejamientoFlotaRPT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Crea las nuevas columnas tomando como base el número de días del mes en curso
        /// </summary>
        public void CreateNewColumns()
        {
            var rowAnios = (this.DataSource as AniejamientoFlotaDS).ConfiguracionesSistema.AsEnumerable().FirstOrDefault();

            if(rowAnios == null)
                throw new Exception("No se encuentra la configuracion para el reporte.");

            int numeroAños = (rowAnios.AnioFin - rowAnios.AnioInicio) + 1;

            for(int i = 0; i < numeroAños-1; i++)
                this.CreateNewColumn(rowAnios.AnioInicio + i, numeroAños, true);

            this.CreateNewColumn(rowAnios.AnioFin, numeroAños, false);
        }

        /// <summary>
        /// Crea una nueva columna de un día especifico del mes
        /// </summary>
        /// <param name="year">Año en curso</param>
        /// <param name="isNewColumn">Indica si se va a contruir una nueva columna o se va a usar la columna que se tiene de base</param>
        private void CreateNewColumn(int year, int numbersOfYears, bool isNewColumn)
        {
            XRTableCell[] newHeaderYears = { xrtCellAnio };

            if(isNewColumn)
            {
                newHeaderYears = this.xrtAnioHeader.InsertColumnToLeft(xrtCellAnio);
            }

            double size = this.AniosWidth / numbersOfYears;

            foreach(XRTableCell cell in newHeaderYears)
                this.InitializeHeaderModels(cell, size, year);
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="year">Año</param>
        /// <param name="monthDays">Total de días del mes</param>
        private void InitializeHeaderModels(XRTableCell cell, double weight, int year)
        {
            cell.Name = String.Format("xrtHeaderYear_{0}", year);
            cell.WidthF = (float)weight;
            cell.Text = year.ToString();
        }

        /// <summary>
        /// Constructor por default que recibe el diccionario con los datos para mostrar el reporte
        /// </summary>
        /// <param name="datos">Diccionario de Datos</param>
        public AniejamientoFlotaRPT(Dictionary<string, object> datos): this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];
            this.AniosWidth = this.xrtCellAnio.WidthF;
            this.ReporteDetallado = (bool?)datos["ReporteDetallado"];
            this.xrtEtiquetaReporte.Text = datos.ContainsKey("EtiquetaReporte") ? datos["EtiquetaReporte"].ToString() : String.Empty;
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza la imagen que representa el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as AniejamientoFlotaDS).ConfiguracionesSistema.AsEnumerable().Select(x => x.UrlLogoEmpresa).FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }

        /// <summary>
        /// Evento ejecutado antes de que se imprima el reporte
        /// </summary>
        private void AniejamientoFlotaRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.CreateNewColumns();
        }

        /// <summary>
        /// Evento ejecutado antes de que se imprima cada subReporte
        /// </summary>
        private void xrsrpUnidad_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport reporteModelo = sender as XRSubreport;
            if(reporteModelo.ReportSource == null)
                return;
            reporteModelo.ReportSource.DataSource = this.DataSource;

            var modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            reporteModelo.ReportSource.FilterString = String.Format("[ModeloID] = {0}", modeloId);
        }

        /// <summary>
        /// Evento ejecutado antes de que se imprima la seccion Detail
        /// </summary>
        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            var aniejamiento = (this.DataSource as AniejamientoFlotaDS).FlotaXFechaCompra.AsEnumerable().Where(x => x.ModeloID == modeloId).ToList();

            AniejamientoFlotaUnidadRPT reporte = (this.xrsrpUnidad.ReportSource = new AniejamientoFlotaUnidadRPT()) as AniejamientoFlotaUnidadRPT;
            reporte.AniosWidth = this.AniosWidth;
            reporte.configuracionAnios = (this.DataSource as AniejamientoFlotaDS).ConfiguracionesSistema.AsEnumerable().FirstOrDefault();
            reporte.AnioInicio = aniejamiento.Min(x => x.FechaFactura.Year);
            reporte.Aniofin = aniejamiento.Max(x => x.FechaFinalUtilizacion.Year);
            reporte.ReporteCompleto = this.ReporteDetallado != null ? this.ReporteDetallado.Value : false; 
            reporte.CreateNewColumns();
        }

        /// <summary>
        /// Evento ejecutado antes de se imprima la seccion Header
        /// </summary>
        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var modeloId = this.GetCurrentColumnValue<int>("ModeloID");
            var aniejamiento = (this.DataSource as AniejamientoFlotaDS).FlotaXFechaCompra.AsEnumerable().Where(x => x.ModeloID == modeloId).ToList();
            this.xrtCellCountUnidades.Text = aniejamiento.Count().ToString();
        }

        /// <summary>
        /// Obtiene el total de las unidades en el reporte
        /// </summary>
        private void xrtCellTotalUnidades_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var aniejamiento = (this.DataSource as AniejamientoFlotaDS).FlotaXFechaCompra.AsEnumerable().ToList();
            this.xrtCellTotalUnidades.Text = aniejamiento.Count().ToString();
        }
        #endregion        
    }
}
