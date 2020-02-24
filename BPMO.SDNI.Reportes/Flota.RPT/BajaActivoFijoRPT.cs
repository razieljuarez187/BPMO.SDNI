using System;
using System.Collections.Generic;


namespace BPMO.SDNI.Flota.RPT {
    public partial class BajaActivoFijoRPT : DevExpress.XtraReports.UI.XtraReport {
        public BajaActivoFijoRPT(Dictionary<string, object> datos) {
            InitializeComponent();
            ImprimirReporte(datos);

        }

        /// <summary>
        /// Se realiza en enlazado de los datos en el reporte
        /// </summary>
        /// <param name="datos">Información del reporte</param>
        public void ImprimirReporte(Dictionary<string, object> datos) {
            try {
                #region Header

                #region Folio
                string folio = (string)datos["Folio"];
               
                #endregion

                #region Fecha
                lblFecha.Text = DateTime.Now.ToShortDateString();
                #endregion
                #endregion

                #region Enlazar datos del detalle

                lblEmpresa.Text = (string)datos["Empresa"];
                lblDepartamento.Text = (string)datos["Departamento"];
                lblPersonaSolicitante.Text = (string)datos["PersonaSolicitante"];
                lblSucursal.Text = (string)datos["Sucursal"];
                lblCostos.Text = (string)datos["CentroCostos"];


                bool siniestro = (bool)datos["Siniestro"];

                if (siniestro) {
                    lblSiniestro.Text = "X";
                } else {
                    lblSiniestro.Text = string.Empty;
                    lblMotivoBaja.Text = "INVENTARIO DESFLOTE";
                    lblDescripcionMotivo.Text = "Venta Por Desflote";
                }

                lblTipo.Text = (string)datos["Tipo"];
                lblMarca.Text = (string)datos["Marca"];
                lblNumSerie.Text = (string)datos["#Serie"];

                lblResponsable.Text = (string)datos["Responsable"];

                lblEtiqueta.Text = (string)datos["Etiqueta"];

                lblOtrosDescripcion.Text = (string)datos["Otros"];



                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImpirmirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
    }
}