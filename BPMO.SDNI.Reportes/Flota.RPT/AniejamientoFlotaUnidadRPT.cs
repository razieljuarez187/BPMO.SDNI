using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Flota.RPT
{
    public partial class AniejamientoFlotaUnidadRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Propiedades
        /// <summary>
        /// Ancho de la columna donde se pintaran los años
        /// </summary>
        public double AniosWidth;
        /// <summary>
        /// Año Inicial del Rango
        /// </summary>
        public int AnioInicio;
        /// <summary>
        /// Año final del Rango
        /// </summary>
        public int Aniofin;
        /// <summary>
        /// Valor usado para definir si el reporte a presentar es completo
        /// </summary>
        public bool ReporteCompleto;
        /// <summary>
        /// Registro de Configuraciones para el reporte
        /// </summary>
        public AniejamientoFlotaDS.ConfiguracionesSistemaRow configuracionAnios;
        #endregion
        #region Metodos
        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public AniejamientoFlotaUnidadRPT()
        {
            InitializeComponent();
            this.AniosWidth = this.xrtCellAnio.WidthF;
        }

        /// <summary>
        /// Crea las nuevas columnas tomando como base el número de días del mes en curso
        /// </summary>
        public void CreateNewColumns()
        {
            var rowAnios = configuracionAnios;

            if(rowAnios == null)
                throw new Exception("No se encuentra la configuracion para el reporte.");

            int numeroAños = (rowAnios.AnioFin - rowAnios.AnioInicio) + 1;

            for(int i = 0; i < numeroAños - 1; i++)
                this.CreateNewColumn(rowAnios.AnioInicio + i, numeroAños, true, rowAnios.AnioInicio + i >= AnioInicio && rowAnios.AnioInicio + i <= Aniofin);

            this.CreateNewColumn(rowAnios.AnioFin, numeroAños, false, rowAnios.AnioFin >= AnioInicio && rowAnios.AnioFin <= Aniofin);
        }

        /// <summary>
        /// Crea una nueva columna de un día especifico del mes
        /// </summary>
        /// <param name="year">Año en curso</param>
        /// <param name="isNewColumn">Indica si se va a contruir una nueva columna o se va a usar la columna que se tiene de base</param>
        private void CreateNewColumn(int year, int numbersOfYears, bool isNewColumn, bool paintBackground)
        {
            XRTableCell[] newHeaderYears = { xrtCellAnio };
            XRTableCell[] newDetailYeas = {xrtCellAnioDetail};

            if(isNewColumn)
            {
                newHeaderYears = this.xrtAnioHeader.InsertColumnToLeft(xrtCellAnio);
                newDetailYeas = this.xtrDetail.InsertColumnToLeft(xrtCellAnioDetail);
            }

            double size = this.AniosWidth / numbersOfYears;

            foreach(XRTableCell cell in newHeaderYears)
                this.InitializeHeaderModels(cell, size, year, paintBackground);
            foreach(XRTableCell cell in newDetailYeas)
                this.InitializeYearDetails(cell, size, year);
        }

        /// <summary>
        /// Prepara una celda que indica el número del día del mes
        /// </summary>
        /// <param name="cell">Celda a inicializar</param>
        /// <param name="weight">Tamaño de la celda</param>
        /// <param name="year">Año</param>
        /// <param name="paintBackground">Determina si se pintara el fondo de la celda</param>
        private void InitializeHeaderModels(XRTableCell cell, double weight, int year, bool paintBackground)
        {
            cell.Name = String.Format("xrtHeaderYear_{0}", year);
            cell.WidthF = (float)weight;
            cell.Text = String.Empty;
            cell.Borders = BorderSide.None;
        }

        /// <summary>
        /// Prepara una Celda para ubicar el nombre del mes en que termina la vida util de una unidad
        /// </summary>
        private void InitializeYearDetails(XRTableCell cell, double weight, int year)
        {
            cell.Name = String.Format("xrtDetailYear_{0}", year);
            cell.WidthF = (float)weight;
            cell.Text = String.Empty;
        }

        /// <summary>
        /// Evento que se ejecuta antes de que se imprima el Subreporte
        /// </summary>
        private void AniejamientoFlotaUnidadRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.Bands["Detail"].Visible = ReporteCompleto;
        }

        /// <summary>
        /// Evento que se ejecuta antes de que se imprima la seccion Detail
        /// </summary>
        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var unidadId = this.GetCurrentColumnValue<int>("UnidadID");
            var nombreMes = this.GetCurrentColumnValue<String>("NombreMesFinalUtilizacion");
            var aniejamientoFlota = (this.DataSource as AniejamientoFlotaDS).AniejamientoFlota.AsEnumerable().FirstOrDefault(x => x.EquipoID == unidadId);
            if (aniejamientoFlota != null)
            {
                this.xrtCellSucursal.Text = aniejamientoFlota.SucursalNombre;
                this.xrtCellSucursal.CanGrow = true;
                this.xrtCellReferencia.Text = aniejamientoFlota.Referencia;
                this.xrtCellReferencia.CanGrow = true;
                this.xrtCellVin.Text = aniejamientoFlota.Serie;
                this.xrtCellVin.CanGrow = true;

                var fechaRentaId = this.GetCurrentColumnValue<int>("FechaRentaId");
                var fechas = (this.DataSource as AniejamientoFlotaDS).FechasRenta.AsEnumerable().FirstOrDefault(x => x.FechaRentaId == fechaRentaId);
                var cells = this.xtrDetail.Rows[0].Cells.Cast<XRTableCell>();
                var xrTableCells = cells as IList<XRTableCell> ?? cells.ToList();
                foreach(var cell in xrTableCells.Where(x => x.Name.Contains("xrtDetailYear_")).ToList())
                {
                    cell.Text = cell.Name == String.Format("xrtDetailYear_{0}", fechas.AnioFin) ? nombreMes : "";
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta antes de que se imprima la seccion Header
        /// </summary>
        private void gHeaderAnio_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var fechaRentaId = this.GetCurrentColumnValue<int>("FechaRentaId");
            var fechas = (this.DataSource as AniejamientoFlotaDS).FechasRenta.AsEnumerable().FirstOrDefault(x => x.FechaRentaId == fechaRentaId);
            if(fechas != null)
            {
                var cells = this.xrtAnioHeader.Rows[0].Cells.Cast<XRTableCell>();
                var color = Color.FromArgb(fechas.R,fechas.G, fechas.B);
                var xrTableCells = cells as IList<XRTableCell> ?? cells.ToList();
                List<String> cellNames = new List<String>();
                for(int i = 0; i <= (fechas.AnioFin - fechas.AnioInicio); i++)
                {
                    int anioInicio = fechas.AnioInicio + i;                    
                    var cell = xrTableCells.FirstOrDefault(x => x.Name == String.Format("xrtHeaderYear_{0}", anioInicio));
                    if(cell != null)
                    {
                        cell.BackColor = color;
                        cellNames.Add(cell.Name);
                    }
                }
                foreach(var celda in xrTableCells.Where(x => !cellNames.Any(y => y == x.Name)))
                {
                    celda.BackColor = Color.Transparent;
                }
            }
        }
        #endregion
    }
}
