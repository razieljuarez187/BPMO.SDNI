// Satisface al CU014 - Imprimir Contrato de Renta Diaria
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;

namespace BPMO.SDNI.Contratos.RD.RPT
{
    public partial class ContratoRDRevRPT : DevExpress.XtraReports.UI.XtraReport
    {
        private string xml = "BPMO.SDNI.Reportes.Contrato.Renta.Diaria.xml";
        public ContratoRDRevRPT(Dictionary<string, Object> datos, string urlXML)
        {
            try
            {
                InitializeComponent();
                ImprimirReporte(datos, ObtenerXMLContenido(urlXML));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al imprimir el reporte. ContratoRentaDiariaRPT: " + ex.Message);
            }
        }
        protected XmlDocument ObtenerXMLContenido(string xmlUrl)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlUrl + xml);
                return xDoc;
            }
            catch (Exception ex)
            {

                throw new Exception("ContratoRentaDiariaRPT.ObtenerXMLContenido:Error al cargar el archivo XML." + ex.Message);
            }
        }
        protected void ImprimirReporte(Dictionary<string, Object> datos, XmlDocument documento)
        {
            try
            {
                string leyendaTitulo = string.Empty;
                string leyendaDatosEncabezado = string.Empty;
                string leyendaClausulas = string.Empty;
                XmlNodeList textoEncabezado = documento.GetElementsByTagName("datosEncabezado");
                if (textoEncabezado.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaDatosEncabezado = textoEncabezado[0].InnerText;
                XmlNodeList textoTitulo = documento.GetElementsByTagName("Titulo");
                if (textoTitulo.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaTitulo = textoTitulo[1].InnerText;
                XmlNodeList textoClausulas = documento.GetElementsByTagName("clausulas");
                if (textoClausulas.Count < 1) throw new Exception("El formato del archivo XML es incorrecto");
                leyendaClausulas = textoClausulas[0].InnerText;

                #region Inicializacion de Variables
                //Contrato
                ContratoRDBO contrato = (ContratoRDBO)datos["Contrato"];
                if (contrato == null)
                    contrato = new ContratoRDBO();
                if (contrato.Sucursal == null)
                    contrato.Sucursal = new SucursalBO();
                if (contrato.Sucursal.UnidadOperativa == null)
                    contrato.Sucursal.UnidadOperativa = new UnidadOperativaBO();
                if(contrato.Cliente == null)
                    contrato.Cliente = new CuentaClienteIdealeaseBO();
                LineaContratoRDBO linea = contrato.ObtenerLineaContrato();
                if (linea == null)
                    linea = new LineaContratoRDBO();
                if (linea.Cobrable == null)
                    linea.Cobrable = new TarifaContratoRDBO();
                contrato.LineasContrato = new List<ILineaContrato>();
                contrato.AgregarLineaContrato(linea);

                //Configuración del modulo

                ModuloBO modulo = (ModuloBO)datos["Modulo"];
                ConfiguracionUnidadOperativaBO unidadOperativaConfiguracion;
                if (modulo == null)
                    modulo = new ModuloBO();
                if (modulo.Configuracion == null)
                    modulo.Configuracion = new ConfiguracionModuloBO();
                if (contrato.Sucursal.UnidadOperativa.Id == null)
                    unidadOperativaConfiguracion = new ConfiguracionUnidadOperativaBO();
                else
                    unidadOperativaConfiguracion = modulo.ObtenerConfiguracionUO(new UnidadOperativaBO { Id = contrato.Sucursal.UnidadOperativa.Id });
                if (unidadOperativaConfiguracion == null)
                    unidadOperativaConfiguracion = new ConfiguracionUnidadOperativaBO();
                if (unidadOperativaConfiguracion.ConfiguracionModulo == null)
                    unidadOperativaConfiguracion.ConfiguracionModulo = new ConfiguracionModuloBO();

                //Sucursal Matriz
                SucursalBO matriz = (SucursalBO)datos["SucursalMatriz"];
                if (matriz == null)
                    matriz = new SucursalBO();
                if (matriz.UnidadOperativa == null)
                    matriz.UnidadOperativa = new UnidadOperativaBO();
                
                #endregion
                #region Asignacion de Datos

                xrlblTitulo.Html = leyendaTitulo;
                if (!String.IsNullOrEmpty(contrato.NumeroContrato))
                    leyendaDatosEncabezado = leyendaDatosEncabezado.Replace("{NUMEROCONTRATO}", "<u>" + contrato.NumeroContrato + "</u>");
                else
                    leyendaDatosEncabezado = leyendaDatosEncabezado.Replace("{NUMEROCONTRATO}", "_____________");
                if (!String.IsNullOrEmpty(contrato.Sucursal.UnidadOperativa.Nombre))
                    leyendaDatosEncabezado = leyendaDatosEncabezado.Replace("{ARRENDADOR}",
                                                                            "<u>" +
                                                                            contrato.Sucursal.UnidadOperativa.Nombre +
                                                                            "</u>");
                else
                    leyendaDatosEncabezado = leyendaDatosEncabezado.Replace("{ARRENDADOR}",
                                                                            "________________________________________________________");
                if (!String.IsNullOrEmpty(contrato.Cliente.Nombre))
                    leyendaDatosEncabezado = leyendaDatosEncabezado.Replace("{ARRENDATARIO}",
                                                                            "<u>" + contrato.Cliente.Nombre + "</u>");
                else
                    leyendaDatosEncabezado = leyendaDatosEncabezado.Replace("{ARRENDATARIO}", "________________________________________________________");

                xrlblDatosEncabezado.Html = leyendaDatosEncabezado;
                LineaContratoRDBO lineaTemp = contrato.ObtenerLineaContrato();
                

                if (((TarifaContratoRDBO) lineaTemp.Cobrable).TarifaDiaria == null)
                {
                    leyendaClausulas = leyendaClausulas.Replace("{CARGOHORASADICIONALES}", "____");
                    leyendaClausulas = leyendaClausulas.Replace("{DAÑOSODOMETRO}", "____");
                    leyendaClausulas = leyendaClausulas.Replace("{ENTREGAIMPUNTUAL}", "____");
                }
                else
                {
                    //SC0021 formato de decimales
                    leyendaClausulas = leyendaClausulas.Replace("{CARGOHORASADICIONALES}", String.Format("{0:#,##0.00##}", ((TarifaContratoRDBO)lineaTemp.Cobrable).TarifaDiaria));
                    leyendaClausulas = leyendaClausulas.Replace("{DAÑOSODOMETRO}", String.Format("{0:#,##0.00##}", ((TarifaContratoRDBO)lineaTemp.Cobrable).TarifaDiaria));
                    leyendaClausulas = leyendaClausulas.Replace("{ENTREGAIMPUNTUAL}", String.Format("{0:#,##0.00##}",((TarifaContratoRDBO)lineaTemp.Cobrable).TarifaDiaria));
                }
                leyendaClausulas = leyendaClausulas.Replace("{PORCENTAJEPOSTFACTURA}", unidadOperativaConfiguracion.PorcentajePagoPostFactura == null ? "____" : String.Format("{0:#,##0.00}", unidadOperativaConfiguracion.PorcentajePagoPostFactura));

                leyendaClausulas = leyendaClausulas.Replace("{DIASPOSTFACTURA}", unidadOperativaConfiguracion.DiasPagoPostFactura == null ? "____" : unidadOperativaConfiguracion.DiasPagoPostFactura.ToString());

                leyendaClausulas = leyendaClausulas.Replace("{UBICACIONTRIBUNALES}",
                                                                          CultureInfo.InvariantCulture.TextInfo.ToTitleCase(unidadOperativaConfiguracion.UbicacionTribunales.ToLower()) ??
                                                                          "_____________________________");
                leyendaClausulas = leyendaClausulas.Replace("{UNIDADOPERATIVA}",
                                                            matriz.UnidadOperativa.Nombre ??
                                                            "____________________________");
                xrlblClausulas.Html = leyendaClausulas;

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("ContratoRentaDiariaRevRPT.ImprimirReporte:Error al intentar generar el reporte." +
                                    ex.Message);
            }
        }

    }
}
