//Satisface al caso de uso CU071 - reporte de rendimiento de unidad
using System.Drawing.Printing;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Mantenimiento.RPT
{
    /// <summary>
    /// Subreporte que presenta los datos de del rendimiento por Mes
    /// </summary>
    public partial class ReporteRendimientoUnidadMesesRPT : XtraReport
    {
        #region Constructor
        /// <summary>
        /// Constructor del Reporte
        /// </summary>
        public ReporteRendimientoUnidadMesesRPT()
        {
            InitializeComponent();
        }        
        #endregion

        #region Eventos
        /// <summary>
        /// Se ejecuta antes de la impresion de cada detalle
        /// </summary>
        private void Detail_BeforePrint(object sender, PrintEventArgs e)
        {
            var unidadId = this.GetCurrentColumnValue<int>("UnidadID");
            var mes = this.GetCurrentColumnValue<int>("Mes");
            var Anio = this.GetCurrentColumnValue<int>("Anio");

            ReporteRendimientoUnidadDS dataSource = (ReporteRendimientoUnidadDS)this.DataSource;
            var row = Enumerable.AsEnumerable(dataSource.Meses).Where(x => x.Mes == mes && x.Anio == Anio).FirstOrDefault();
            if (row != null)
            {
                this.xrlMesAnio.Text = row.NombreMes.ToString() + " - " + row.Anio.ToString();
            }
        }

        /// <summary>
        /// Se ejecuta antes de que se pinte el page Header
        /// </summary>
        private void PageHeader_BeforePrint(object sender, PrintEventArgs e)
        {
        }
        
        #endregion
    }
}
