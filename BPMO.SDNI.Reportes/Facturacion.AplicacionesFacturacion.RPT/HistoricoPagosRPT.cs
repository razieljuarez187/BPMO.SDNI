// Satisface al CU006 - Ver Histórico de Pagos 
// Satisface a la solicitud de cambios SC0015
// Satisface a la solución del RI0013
// Satisface a la solicitud de cambio SC0035
using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.SDNI.Facturacion.InfraestructuraPagos.BO;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Facturacion.AplicacionesFacturacion.RPT
{
    public partial class HistoricoPagosRPT : XtraReport
    {
        /// <summary>
        /// Numero de Plazos del Contrato
        /// </summary>
        private string NumeroPlazos;
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        /// <param name="datos">Información del reporte</param>
        public HistoricoPagosRPT(Dictionary<string, object> datos)
        {
            InitializeComponent();
            ImprimirReporte(datos);
        }
        /// <summary>
        /// Se realiza en enlazado de los datos en el reporte
        /// </summary>
        /// <param name="datos">Información del reporte</param>
        public void ImprimirReporte(Dictionary<string, object> datos)
        {
            try
            {
                #region Validación de datos
                if (datos["NombreCliente"] == null) throw new Exception("Se esperaba el nombre del cliente");
                if (datos["NombreSucursal"] == null) throw new Exception("Se esperaba el nombre de la sucursal");
                if (datos["FolioContrato"] == null) throw new Exception("Se esperaba el folio del contrato");
                if (datos["FechaInicioContrato"] == null) throw new Exception("Se esperaba la fecha inicio del contrato");
                if (datos["FechaFinContrato"] == null) throw new Exception("Se esperaba la fecha final del contrato");
                if (datos["NumeroPlazos"] == null) throw new Exception("Se esperaba el número de plazos del contrato");

                if (datos["Pagos"] == null) throw new Exception("Se esperaba los pagos del contrato");
                if (datos["NombreUnidadOperativa"] == null) throw new Exception("Se esperaba el nombre de la unidad operativa");
                if (datos["Logo"] == null) throw new Exception("Se esperaba el logo de la empresa");
                if (datos["PagosPSL"] == null) throw new Exception("Se esperaba los pagos del contrato PSL"); 
                #endregion

                #region Enlazar datos del encabezado
                if (!String.IsNullOrEmpty((string)datos["Logo"]))
                    xrLogo.ImageUrl = datos["Logo"] as string;

                if (!String.IsNullOrEmpty((string)datos["NombreUnidadOperativa"]))
                    xrUO.Text = datos["NombreUnidadOperativa"] as string;
                else
                    xrUO.Text = "";

                if (!String.IsNullOrEmpty((string)datos["NombreCliente"]))
                    xrCliente.Text = datos["NombreCliente"] as string;
                else
                    xrCliente.Text = "";

                if (!String.IsNullOrEmpty((string)datos["NombreSucursal"]))
                    xrSucursal.Text = datos["NombreSucursal"] as string;
                else
                    xrSucursal.Text = "";

                if (!String.IsNullOrEmpty((string)datos["FolioContrato"]))
                    xrNumeroContrato.Text = datos["FolioContrato"] as string;
                else
                    xrNumeroContrato.Text = "";

                xrInicioContrato.Text = datos["FechaInicioContrato"] != null ? ((DateTime)datos["FechaInicioContrato"]).ToShortDateString() : "";

                xrFinContrato.Text = datos["FechaFinContrato"] != null ? ((DateTime)datos["FechaFinContrato"]).ToShortDateString() : "";
                #endregion
                
                #region Enlazar datos del detalle    
                var listaPagos = ((List<PagoUnidadContratoBO>)datos["Pagos"]).Select(p => new
                {
                    //p.NumeroPago,
                    EnviadoFacturacion = (p.EnviadoFacturacion != null && p.EnviadoFacturacion.Value) ? "SI" : "NO",
                    EstatusFacturacion =
                        p.FacturaEnCeros == true
                            ? "Factura en CEROS"
                            : (p.EnviadoFacturacion != null && p.EnviadoFacturacion.Value && p.Facturado != null &&
                                p.Facturado.Value)
                                ? "Facturado"
                                : (p.EnviadoFacturacion != null && p.EnviadoFacturacion.Value && p.Facturado != null &&
                                    p.Facturado.Value == false)
                                    ? "En proceso"
                                    : (p.EnviadoFacturacion != null && !p.EnviadoFacturacion.Value && p.Facturado != null && !p.Facturado.Value
                                        && p.Activo != null && !p.Activo.Value)
                                    ? "Cancelado"
                                        : "",
                    Fecha = p.FechaFacturacion,
                    Folio = p.Factura != null ? p.Factura.Serie + p.Factura.Folio : null,
                    TotalFactura = p.Facturado == true ? p.TotalFactura : null,
                    PagoPlazo = string.Format("{0:00}/{1:00}", p.NumeroPago, p.ReferenciaContrato.Plazo),
                    Referencia = p.Referencia //SC0035
                });

                var listaPagosPSL = ((List<PagoContratoPSLBO>)datos["PagosPSL"]).Select(p => new
                {
                    //p.NumeroPago,
                    EnviadoFacturacion = (p.EnviadoFacturacion != null && p.EnviadoFacturacion.Value) ? "SI" : "NO",
                    EstatusFacturacion =
                        p.FacturaEnCeros == true
                            ? "Factura en CEROS"
                            : (p.EnviadoFacturacion != null && p.EnviadoFacturacion.Value && p.Facturado != null &&
                                p.Facturado.Value)
                                ? "Facturado"
                                : (p.EnviadoFacturacion != null && p.EnviadoFacturacion.Value && p.Facturado != null &&
                                    p.Facturado.Value == false)
                                    ? "En proceso"
                                    : (p.EnviadoFacturacion != null && !p.EnviadoFacturacion.Value && p.Facturado != null && !p.Facturado.Value
                                    && p.Activo != null && !p.Activo.Value)
                                    ? "Cancelado"
                                        : "",
                    Fecha = p.FechaFacturacion,
                    Folio = p.Factura != null ? p.Factura.Serie + p.Factura.Folio : null,
                    TotalFactura = p.Facturado == true ? p.TotalFactura : null,
                    PagoPlazo = string.Format("{0:00}/{1:00}", p.NumeroPago, p.ReferenciaContrato.Plazo),
                    Referencia = p.Referencia 
                });

                bool Pagos = listaPagos.Count() > 0 ? true : false;


                this.DataSource = Pagos ? listaPagos : listaPagosPSL;

                this.xrIdPago.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "PagoPlazo");
                this.xrEnviado.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "EnviadoFacturacion");
                this.xrEstatusFacturacion.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "EstatusFacturacion");
                this.xrFechaFacturacion.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "Fecha", "{0:dd/MM/yyyy}");
                this.xrFolio.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "Folio");
                this.xrMontoFactura.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "TotalFactura", "{0: $#,0.00}");
                this.xrReferencia.DataBindings.Add("Text", Pagos ? listaPagos : listaPagosPSL, "Referencia"); //SC0035
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("HistoricoPagosRPT.ImprimirReporte:Error al intentar generar el reporte." + ex.Message);
            }
        }
    }
}
