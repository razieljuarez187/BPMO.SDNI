using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BPMO.Basicos.BO;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.PSL.BO;
namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ContratoRORPT : DevExpress.XtraReports.UI.XtraReport {

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "ContratoRORPT";
        private string xml = "BPMO.SDNI.Reportes.Contrato.Renta.Ordinaria.xml";
        private string Nombre = "<b>NOMBRE: </b>";
        private string Nombre_Razon = "<b>NOMBRE O RAZÓN SOCIAL: </b>";
        private string Representada = "<b>REPRESENTADA POR: </b>";

        public ContratoRORPT(Dictionary<string, Object> datos, string urlXML) {
            InitializeComponent();

            this.ImprimirReporte(datos, ObtenerXMLContenido(urlXML));
        }


        protected XmlDocument ObtenerXMLContenido(string xmlUrl) {
            try {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlUrl + xml);
                return xDoc;
            } catch (Exception ex) {

                throw new Exception("ContratoRentaDiariaRPT.ObtenerXMLContenido:Error al cargar el archivo XML." + ex.Message);
            }
        }

        private void ImprimirReporte(Dictionary<string, object> datos, XmlDocument documento) {
            try {
                var contrato = datos.ContainsKey("ContratoPSLBO") ? (ContratoPSLBO)datos["ContratoPSLBO"] : null;
                var representante = (ConfiguracionUnidadOperativaBO)datos["RepresentanteEmpresa"];
                DireccionSucursalBO dirSucursal = (DireccionSucursalBO)datos["DireccionSucursal"];
                DireccionClienteBO direccion = contrato.Cliente.Direcciones[0];

                #region Ciudad y Estado SUCURSAL
                string strCiudadEdoSuc = string.Empty;
                if (dirSucursal.Ubicacion != null) {
                    if (dirSucursal.Ubicacion.Ciudad != null)
                        strCiudadEdoSuc += ", " + dirSucursal.Ubicacion.Ciudad.Codigo;
                    if (dirSucursal.Ubicacion.Estado != null)
                        strCiudadEdoSuc += ", " + dirSucursal.Ubicacion.Estado.Codigo;
                    if (!string.IsNullOrWhiteSpace(strCiudadEdoSuc))
                        strCiudadEdoSuc = strCiudadEdoSuc.Substring(2);
                }
                #endregion /Ciudad y Estado Sucursal

                #region Fecha
                if (contrato != null && contrato.FechaContrato != null) {
                    this.xrtDia.Text = contrato.FechaInicioArrendamiento.Value.Day.ToString();
                    this.xrtMes.Text = contrato.FechaInicioArrendamiento.Value.Month.ToString();
                    this.xrtAnio.Text = contrato.FechaInicioArrendamiento.Value.Year.ToString();
                }
                #endregion

                #region Empresa
                this.lblEmpresa.Text = "";
                this.lblRepresentante.Text = "";
                this.xrArrendadoraRepPor.Text = "";
                this.xrArrendadoraRepPor2.Text = "";
                this.lblEmpresa.Text = contrato.Sucursal.UnidadOperativa.Empresa.Nombre;
                
                if (representante != null) {
                    this.lblRepresentante.Text = representante.Representante;
                    this.xrArrendadoraRepPor.Text += "REPRESENTADA POR: " + representante.Representante;
                    this.xrArrendadoraRepPor2.Text += "REPRESENTADA POR: " + representante.Representante;
                }

                List<DireccionSucursalBO> listaDireccion = contrato.Sucursal.DireccionesSucursal;

                this.lblDireccionEmpresa.Text = dirSucursal.Calle;
                this.lblCodigoPostal.Text = dirSucursal.CodigoPostal;
                this.lblMunicipioCiudadEstadoEmpresa.Text = "";
                string direccionempresa = "";
                if (dirSucursal.Ubicacion != null && dirSucursal.Ubicacion.Municipio != null)
                    direccionempresa = dirSucursal.Ubicacion.Municipio.Codigo;
                if (dirSucursal.Ubicacion != null && dirSucursal.Ubicacion.Ciudad != null)
                    direccionempresa += ", " + dirSucursal.Ubicacion.Ciudad.Codigo;
                if (dirSucursal.Ubicacion != null && dirSucursal.Ubicacion.Estado != null)
                    direccionempresa += ", " + dirSucursal.Ubicacion.Estado.Codigo;

                this.lblMunicipioCiudadEstadoEmpresa.Text = direccionempresa;
                this.lblDomicilioFiscalEmpresa.Text = "";
                this.lblRegistroFederalContribuyenteEmpresa.Text = contrato.Sucursal.UnidadOperativa.Empresa.RFC;
                #endregion

                #region[Variables para reemplazar en la plantilla]
                string ciudadCliente = string.Empty;
                string ciudadObligadoSolidarioAbal = string.Empty;
                string ciudadDepositario = string.Empty;
                #endregion

                #region Arrendatario
                List<DireccionClienteBO> listaclienteinfo = contrato.Cliente.Direcciones.ConvertAll(s => (DireccionClienteBO)s);
                DireccionClienteBO clienteinfo = new DireccionClienteBO();
                foreach (DireccionClienteBO dircliente in listaclienteinfo) {
                    if (dircliente.Primaria.HasValue && dircliente.Primaria.Value)
                        clienteinfo = dircliente;
                }

                string ciudadestadomuni = string.Empty;
                if (clienteinfo != null && clienteinfo.Ubicacion != null && clienteinfo.Ubicacion.Ciudad != null) {
                    ciudadCliente = clienteinfo.Ubicacion.Ciudad.Codigo;
                    ciudadestadomuni = clienteinfo.Ubicacion.Ciudad.Codigo;
                }
                if (clienteinfo != null && clienteinfo.Ubicacion != null && clienteinfo.Ubicacion.Estado != null)
                    ciudadestadomuni += ", " + clienteinfo.Ubicacion.Estado.Codigo;
                if (clienteinfo != null && clienteinfo.Ubicacion != null && clienteinfo.Ubicacion.Municipio != null)
                    ciudadestadomuni += ", " + clienteinfo.Ubicacion.Municipio.Codigo;

                this.lblNombreRazonSocialArrendatario.Text = contrato.Cliente.Nombre;
                this.lblDireccionArrendatario.Text = contrato.Cliente.Direccion;
                this.lblCodigoPostalArrendatario.Text = clienteinfo.CodigoPostal;
                this.lblMunicipioCiudadEstadoArrendatario.Text = ciudadestadomuni;
                this.lblDomicilioFiscalArrendatario.Text = clienteinfo.Direccion;
                this.lblRFCArrendatario.Text = "";
                if (contrato.Cliente != null && contrato.Cliente.Cliente != null)
                    this.lblRFCArrendatario.Text = contrato.Cliente.Cliente.RFC;
                #endregion

                #region Datos de identificación
                this.lblNombreCompletoRepresentante.Text = "";
                this.xrArrendatarioRepPor.Text = "";
                this.xrArrendatarioRepPor2.Text = "";
                this.lblRegistroFederalContribuyentesRepresentante.Text = "";
                this.lblDomicilioRepresentante.Text = "";

                //obligado solidario y aval
                this.lblNombreSolidarioYAval.Text = "";
                this.lblDireccionSolidarioYAval.Text = "";
                this.lblCodigoPostalSolidarioYAval.Text = "";
                this.lblMunicipioCiudadEstadoSolidarioYAval.Text = "";
                this.lblRegistroFederalContribuyenteolidarioYAval.Text = "";
                string dirmunicipioobligado = string.Empty;
                string municiudadestadosol = string.Empty;
                string repLeg = string.Empty;
                if (contrato.RepresentantesLegales.Count > 0) {
                    RepresentanteLegalBO RepresentantesLegales = contrato.RepresentantesLegales.ConvertAll(s => (RepresentanteLegalBO)s).FirstOrDefault();
                    if (RepresentantesLegales != null) {
                        this.lblNombreCompletoRepresentante.Text = RepresentantesLegales.Nombre;
                        this.xrArrendatarioRepPor.Text = "REPRESENTADA POR: " + RepresentantesLegales.Nombre;
                        this.xrArrendatarioRepPor2.Text = "REPRESENTADA POR: " + RepresentantesLegales.Nombre;
                        this.lblRegistroFederalContribuyentesRepresentante.Text = RepresentantesLegales.RFC;
                        repLeg = RepresentantesLegales.Nombre;
                        //Se busca la dirección del representante
                        if (RepresentantesLegales.DireccionPersona.Ubicacion != null && RepresentantesLegales.DireccionPersona.Ubicacion.Municipio != null)
                            municiudadestadosol = RepresentantesLegales.DireccionPersona.Ubicacion.Municipio.Codigo;
                        if (RepresentantesLegales.DireccionPersona.Ubicacion != null && RepresentantesLegales.DireccionPersona.Ubicacion.Ciudad != null) {
                            municiudadestadosol += ", " + RepresentantesLegales.DireccionPersona.Ubicacion.Ciudad.Codigo;
                            ciudadObligadoSolidarioAbal = RepresentantesLegales.DireccionPersona.Ubicacion.Ciudad.Codigo;
                        }
                        if (RepresentantesLegales.DireccionPersona.Ubicacion != null && RepresentantesLegales.DireccionPersona.Ubicacion.Estado != null)
                            municiudadestadosol += ", " + RepresentantesLegales.DireccionPersona.Ubicacion.Estado.Codigo;

                        //Se validará si se cambiará
                        this.lblDomicilioRepresentante.Text = RepresentantesLegales.Direccion;
                        //Cuando todos los representantes legales son avales se 
                        if (contrato.SoloRepresentantes.GetValueOrDefault() == true) {
                            this.lblNombreSolidarioYAval.Text = RepresentantesLegales.Nombre;
                            //Se validará si se cambiará
                            this.lblDireccionSolidarioYAval.Text = RepresentantesLegales.Direccion;
                            this.lblCodigoPostalSolidarioYAval.Text = RepresentantesLegales.DireccionPersona.CodigoPostal;
                            this.lblMunicipioCiudadEstadoSolidarioYAval.Text = municiudadestadosol;
                            this.lblRegistroFederalContribuyenteolidarioYAval.Text = RepresentantesLegales.RFC;
                        }
                    }
                }

                #endregion

                #region Obligado Solidario Y aval
                this.xrObligadoSolidarioRepPor.Text = "";
                this.xrObligadoSolidarioRepPor2.Text = "";
                dirmunicipioobligado = string.Empty;
                municiudadestadosol = string.Empty;

                AvalBO avalbo = contrato.Avales != null && contrato.Avales.Count > 0 ? contrato.Avales.ConvertAll(s => (AvalBO)s)[0] : null;
                if (avalbo != null) {
                    this.lblNombreSolidarioYAval.Text = avalbo.Nombre;
                    this.lblDireccionSolidarioYAval.Text = avalbo.Direccion;
                    this.lblCodigoPostalSolidarioYAval.Text = avalbo.DireccionPersona.CodigoPostal;
                    if (avalbo.DireccionPersona.Ubicacion != null && avalbo.DireccionPersona.Ubicacion.Municipio != null)
                        municiudadestadosol = avalbo.DireccionPersona.Ubicacion.Municipio.Codigo;
                    if (avalbo.DireccionPersona.Ubicacion != null && avalbo.DireccionPersona.Ubicacion.Ciudad != null) {
                        municiudadestadosol += ", " + avalbo.DireccionPersona.Ubicacion.Ciudad.Codigo;
                        ciudadObligadoSolidarioAbal = avalbo.DireccionPersona.Ubicacion.Ciudad.Codigo;
                    }
                    if (avalbo.DireccionPersona.Ubicacion != null && avalbo.DireccionPersona.Ubicacion.Estado != null)
                        municiudadestadosol += ", " + avalbo.DireccionPersona.Ubicacion.Estado.Codigo;

                    this.lblMunicipioCiudadEstadoSolidarioYAval.Text = municiudadestadosol;
                    this.lblRegistroFederalContribuyenteolidarioYAval.Text = avalbo.RFC;
                }
                #endregion

                #region Depositario
                string depositarioSolidario = "";
                this.lblNombresApellidosMaternoPaternoDepositario.Text = "";
                this.lblMunicipioCiudadEstadoDepositario.Text = "";
                this.lblCodigoPostalDepositario.Text = "";
                this.lblRegistroFederalContribuyentesDepositario.Text = "";
                this.lblDepositarioDireccion.Text = "";
                List<RepresentanteLegalBO> listaPersonaDepositario = contrato.RepresentantesLegales.ConvertAll(a => (RepresentanteLegalBO)a);
                foreach (RepresentanteLegalBO personadepositario in listaPersonaDepositario) {
                    if (personadepositario.EsDepositario.HasValue && personadepositario.EsDepositario.Value) {
                        if (personadepositario.DireccionPersona != null && personadepositario.DireccionPersona.Ubicacion != null && personadepositario.DireccionPersona.Ubicacion.Municipio != null)
                            dirmunicipioobligado = personadepositario.DireccionPersona.Ubicacion.Municipio.Codigo;
                        if (personadepositario.DireccionPersona != null && personadepositario.DireccionPersona.Ubicacion != null && personadepositario.DireccionPersona.Ubicacion.Ciudad != null) {
                            dirmunicipioobligado += ", " + personadepositario.DireccionPersona.Ubicacion.Ciudad.Codigo;
                            ciudadDepositario = personadepositario.DireccionPersona.Ubicacion.Ciudad.Codigo;
                        }
                        if (personadepositario.DireccionPersona != null && personadepositario.DireccionPersona.Ubicacion != null && personadepositario.DireccionPersona.Ubicacion.Estado != null)
                            dirmunicipioobligado += ", " + personadepositario.DireccionPersona.Ubicacion.Estado.Codigo;

                        depositarioSolidario = personadepositario.Nombre;
                        this.lblNombresApellidosMaternoPaternoDepositario.Text = depositarioSolidario;
                        this.lblMunicipioCiudadEstadoDepositario.Text = dirmunicipioobligado;
                        this.lblCodigoPostalDepositario.Text = personadepositario.DireccionPersona != null ? personadepositario.DireccionPersona.CodigoPostal : "";
                        this.lblMunicipioCiudadEstadoSolidarioYAval.Text = dirmunicipioobligado;
                        this.lblRegistroFederalContribuyentesDepositario.Text = personadepositario.RFC;
                        this.lblDepositarioDireccion.Text = personadepositario.Direccion;
                        break;
                    }
                }
                #endregion

                #region Aceptacion y firmas
                XmlNodeList AceptacionyFirmas = documento.GetElementsByTagName("AceptacionYFirmas");
                if (AceptacionyFirmas.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                this.xrRichText3.Html = AceptacionyFirmas[0].InnerText.Replace("{CIUDAD_CLIENTE}", strCiudadEdoSuc).Replace("{CIUDAD_OBLIGADOR_SOLIDARIO_AVAL}", strCiudadEdoSuc).Replace("{CIUDAD_DEPOSITARIO}", strCiudadEdoSuc);
                #endregion

                #region Firmas
                AvalBO aval = contrato.Avales.Count > 0 ? contrato.Avales.ConvertAll(s => (AvalBO)s)[0] : null;

                this.xrlNombreEmpresa.Text = contrato.Sucursal.UnidadOperativa.Empresa.Nombre;
                this.xrRichArrendatario2.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre_Razon + contrato.Cliente.Nombre + "</a></div>";
                
                this.xrRichObligado2.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre_Razon + this.lblNombreSolidarioYAval.Text + "</a></div>";

                this.xrRichDepositario2.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre + depositarioSolidario + " , por su propio y personal derecho.</a></div>";
                this.xrObligadoSolidarioRepPor.Text = "REPRESENTADA POR: ";
                if (!string.IsNullOrEmpty(repLeg))
                    this.xrObligadoSolidarioRepPor.Text += repLeg;
                #endregion

                #region Contrato
                //Información de las Declaraciones
                XmlNodeList contratoCuerpo = documento.GetElementsByTagName("Declaraciones");
                if (contratoCuerpo.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                
                //Información del contrato
                XmlNodeList antesTable = documento.GetElementsByTagName("Informacion");
                if (antesTable.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
              
                //Obtener la tabla de Generación o Construcción
                XmlNodeList TablaConstruccion = documento.GetElementsByTagName(ETipoEmpresa.Generacion == (ETipoEmpresa)contrato.Sucursal.UnidadOperativa.Id ? "Generacion" : 
                    ETipoEmpresa.Construccion == (ETipoEmpresa)contrato.Sucursal.UnidadOperativa.Id ? "Construccion" : "Equinova");
                if (TablaConstruccion.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                
                //InformacionDespuesTablas
                XmlNodeList despuesTablas = documento.GetElementsByTagName("InformacionDespuesTablas");
                if (despuesTablas.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");

                this.xrRichPrimero.Html = "<div style=\"margin: 5px 5px 5px 5px;\">" + contratoCuerpo[0].InnerText + antesTable[0].InnerText + TablaConstruccion[0].InnerText + despuesTablas[0].InnerText + "</div>";
                
                #region Firmas Contrato

                this.xrlNombreEmpresaContrato.Text = contrato.Sucursal.UnidadOperativa.Empresa.Nombre;
                this.xrRichArrendatario.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre_Razon + contrato.Cliente.Nombre + "</a></div>";
                this.xrRichObligado.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre_Razon + this.lblNombreSolidarioYAval.Text + "</a></div>";
                this.xrObligadoSolidarioRepPor2.Text = "REPRESENTADA POR: ";
                if (!string.IsNullOrEmpty(repLeg))
                    this.xrObligadoSolidarioRepPor2.Text += repLeg;
                this.xrRichDepositario.Html = "<div style=\"text-align: center;\"><a style=\"font-family:Times New Roman; font-size:10px;\">" + Nombre + depositarioSolidario + " , por su propio y personal derecho.</a></div>";
                
                #endregion
                #endregion

            } catch (Exception ex) {
                throw new Exception(".ImpirmirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }


    }
}
