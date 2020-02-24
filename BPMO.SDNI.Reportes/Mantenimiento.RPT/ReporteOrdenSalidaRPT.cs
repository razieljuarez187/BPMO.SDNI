//Satisface el CU051 - Obtener Orden Salida
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.SDNI.Mantenimiento.Reportes.DA;

namespace BPMO.SDNI
{
    /// <summary>
    /// Clase de Reporte de Orden de Salida
    /// </summary>
    public partial class ReporteOrdenSalidaRPT : DevExpress.XtraReports.UI.XtraReport
    {
        public ReporteOrdenSalidaRPT()
        {
            InitializeComponent();
        }

        public ReporteOrdenSalidaRPT(Dictionary<string, object> datos)
        {
            InitializeComponent();
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza la imagen que representa el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void ImageLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as ObtenerOrdenSalidaDS).ConfiguracionesSistema.AsEnumerable().Select(x => x.URLLogoEmpresa).FirstOrDefault();
            this.ImageLogo.ImageUrl = url;
        }

    }
}
