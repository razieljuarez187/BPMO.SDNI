//Satisface el CU027 – Reporte Porcentaje Utilización de RD Refrigerados por Tipo

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BPMO.SDNI.Flota.Reportes.DA;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.RD.RPT
{
    /// <summary>
    /// Plantilla de Reporte Porcentaje Utilización de Renta Diaria de Refirgerados por Tipo
    /// </summary>
    public partial class PorcentajeUtilizacionRDRefrigeradosTipoRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Contructor por default
        /// </summary>
        public PorcentajeUtilizacionRDRefrigeradosTipoRPT()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Constructor por default que recibe el diccionario con los datos para mostrar el reporte
        /// </summary>
        /// <param name="datos">Diccionario de Datos</param>
        public PorcentajeUtilizacionRDRefrigeradosTipoRPT(Dictionary<string, object> datos)
            : this()
        {
            if (!datos.ContainsKey("DataSource"))
                throw new ArgumentNullException("datos.DataSource");

            this.DataSource = datos["DataSource"];

            if (((BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS)(datos["DataSource"])).Sucursales.Count == 1)
            {
                this.xrTableCell5.Text = "TIME UTILIZATION REFRIGERADOS POR TIPO: " + ((BPMO.SDNI.Flota.Reportes.DA.ReporteRDSucursalDS)(datos["DataSource"])).Sucursales[0].Nombre;
            }
            else { this.xrTableCell5.Text = "TIME UTILIZATION REFRIGERADOS POR TIPO"; }
        }

        /// <summary>
        /// Regresa el nombre del modelo asociado una fila del origen de datos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void ModeloNombre_GetValue(object sender, GetValueEventArgs e)
        {
            ReporteRDSucursalDS.Modelo_SucursalRow row = ((e.Row as DataRowView).Row as ReporteRDSucursalDS.Modelo_SucursalRow);
            e.Value = row.ModelosRow.Nombre;
        }

        /// <summary>
        /// Regresa el nombre de la sucursal asociado una fila del origen de datos
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void SucursalNombre_GetValue(object sender, GetValueEventArgs e)
        {
            ReporteRDSucursalDS.Modelo_SucursalRow row = ((e.Row as DataRowView).Row as ReporteRDSucursalDS.Modelo_SucursalRow);
            e.Value = row.SucursalesRow.Nombre;
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza el reporte
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void PorcentajeUtilizacionRDTipoUnidadRPT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            PorcentajeUtilizacionRDElementoGraficaModeloRPT subReport = this.xsbrptChartDetails.ReportSource as PorcentajeUtilizacionRDElementoGraficaModeloRPT;
            subReport.BindToDataSource(this);
            subReport.CreateAxisX();     

            PorcentajeUtilizacionRDGlobalGraficaRPT subGlobalReport = this.xsbrptChartGlobal.ReportSource as PorcentajeUtilizacionRDGlobalGraficaRPT;
            subGlobalReport.BindToDataSource(this);
            subGlobalReport.CreateAxisX();            
        }

        /// <summary>
        /// Evento que se ejecutará antes de visualizar la gráfica por sucursal
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void xsbrptChartDetails_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport report = (sender as XRSubreport);
            PorcentajeUtilizacionRDElementoGraficaModeloRPT subReport = report.ReportSource as PorcentajeUtilizacionRDElementoGraficaModeloRPT;
            int modeloID = this.GetCurrentColumnValue<int>("ModeloID");
            subReport.SetFilter(modeloID);
        }

        /// <summary>
        /// Evento que se ejecuta antes de visualiza la imagen que representa el logo de la empresa
        /// </summary>
        /// <param name="sender">Objeto que genero el evento</param>
        /// <param name="e">Argumentos relacionados con el evento</param>
        private void pbLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String url = (this.DataSource as ReporteRDSucursalDS).ConfiguracionesSistema.AsEnumerable()
                                                    .Select(x => x.UrlLogoEmpresa)
                                                    .FirstOrDefault();

            this.pbLogo.ImageUrl = url;
        }       
    }
}
