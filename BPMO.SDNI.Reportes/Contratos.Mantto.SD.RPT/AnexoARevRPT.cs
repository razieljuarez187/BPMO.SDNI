// Satisface al CU099 - Imprimir Anexo A de Mantenimiento y Servicio Dedicado
using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace BPMO.SDNI.Contratos.Mantto.SD.RPT
{
    public partial class AnexoARevRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Atributos
        public static string nombreArchivo = "BPMO.SDNI.Reportes.Anexo.A.SD.xml";
        private string nombreClase = "AnexoARevRPT";
        
        private int paginaInicioFirmas;
        private float UltimoAlto;
        private int lastPage = 0;
        private int numFirmaActual;
        private int numFirmas;
        private string xmlFirmas = "BPMO.SDNI.Reportes.Contrato.Mantto.SD.Firmas.xml";
        private string xmlUrl;
        private Boolean esFisico;
        private Dictionary<string, List<Dictionary<string, string>>> firmas;
        private string representanteUnidad;
        private string fechaContrato;
        private string unidadOperativa;
        private string nombreCliente;
        private string documento = "REVERSO DEL ANEXO A";
        private bool? soloRepresentantes;
        #endregion

        #region Propiedades
        private int PageCount
        {
            get { return PrintingSystem.Document.PageCount; }
        }
        private float UltimaPosicion
        {
            get
            {
                return CantidadControles == 0 ? 0 : DtlAnexoAReversoFirmas1Content.Controls[CantidadControles - 1].LocationF.Y;
            }
        }
        /// <summary>
        /// Devuelve la cantidad de controles que estén en el layout
        /// </summary>
        private int CantidadControles
        { 
            get 
            {
                return DtlAnexoAReversoFirmas1Content.Controls.Count; 
            } 
        }
        /// <summary>
        /// Devuelve los márgenes horizontales
        /// </summary>
        private float MargenesHorizontales
        {
            get { return this.Margins.Left + this.Margins.Right + 1; }
        }
        #endregion

        #region Constructores
        public AnexoARevRPT(Dictionary<String, Object> dicContratoFSL, String urlXML)
        {
            InitializeComponent();
            xmlUrl = urlXML;
            this.ImprimirReporte(dicContratoFSL, this.ObtenerXMLDocumento(nombreArchivo));
        }
        #endregion

        #region Metodos
        public void ImprimirReporte(Dictionary<String, Object> dicContrato, XmlDocument xmlDocument)
        {
            try
            {
                //Consulta XML
                XmlNodeList xmlContenidoRev = xmlDocument.GetElementsByTagName("ContenidoReverso");
                string content = xmlContenidoRev[0].InnerText;

                #region Se obtienen las declaraciones de datos estáticos del XML
                XmlNodeList xmlDeclaraciones = xmlDocument.GetElementsByTagName("Declaraciones");

                string diaReportaCliente = string.Empty;
                if (xmlDeclaraciones != null)
                {
                    foreach (XmlNode nodo in xmlDeclaraciones[0].ChildNodes)
                    {
                        if (xmlDeclaraciones[0].ChildNodes[0].Attributes["ID"] != null && xmlDeclaraciones[0].ChildNodes[0].Attributes["ID"].Value == "DIA_REPORTA_CLIENTE")
                            diaReportaCliente = xmlDeclaraciones[0].ChildNodes[0].Attributes["Valor"].Value;
                    }
                }
                #endregion

                #region Se asignan los valores del paquete de datos y se les asigna el formato correspondiente
                //Fecha del Contrato
                fechaContrato = dicContrato["FechaContrato"].ToString();
                //Nombre del Cliente
                nombreCliente = dicContrato["NombreCliente"].ToString();
                //Nombre de la Empresa
                unidadOperativa = dicContrato["NombreEmpresa"].ToString();

                //Leyenda con el listado de los Nombres de los Obligados Solidarios
                string nombresObligadosSoldarios = dicContrato["NombresObligadosSolidario"].ToString();
                //Domicilios del Cliente
                string direccionCliente = null;
                if (dicContrato["DomiciliosCliente"] != null)
                {
                    foreach (DataRow dr in ((DataTable)dicContrato["DomiciliosCliente"]).Rows)
                    {
                        if (dr["Direccion"] != null)
                            direccionCliente += dr["Direccion"] + ", ";
                        if (dr["CodigoPostal"] != null)
                            direccionCliente += "CP: " + dr["CodigoPostal"];
                        direccionCliente += "<br />";
                    }
                }

                //Kilometraje Estimado Anual por cada Unidad/Vehículo
                string kilometrajeAnualVehiculo = "";
                if (dicContrato["Unidades"] != null)
                {
                    DataTable dtUnidades = (DataTable)dicContrato["Unidades"];
                    foreach (DataRow dr in dtUnidades.Rows)
                        kilometrajeAnualVehiculo += (dr["Modelo"] != null ? dr["Modelo"].ToString() : "") + " " + string.Format("{0:###,##0}", (int)dr["KmEstimadoAnual"]) + " Kms <br />";
                }

                //Incluye Seguro
                string responsableSeguroUnidad = dicContrato["IncluyeSeguro"].ToString();
                string coberturaSeguro = dicContrato["CoberturaSeguro"].ToString();
                //Depósito en Garantía
                string depositoGarantia = "";
                if (dicContrato["DepositoGarantia"] != null)
                    depositoGarantia = string.Format("{0:###,##0.00##}", (decimal)dicContrato["DepositoGarantia"]);
                //Comisión por Apertura
                string comisionApertura = "";
                if (dicContrato["ComisionApertura"] != null)
                    comisionApertura = string.Format("{0:###,##0.00##}", (decimal)dicContrato["ComisionApertura"]);
                //Incluye Llantas
                string responsableLlantas = dicContrato["IncluyeLlantas"].ToString();
                //Tasa Moratoria
                string tasaMoratoria = "";
                if (dicContrato["TasaMoratoria"] != null)
                    tasaMoratoria = string.Format("{0:###,##0.00##}", (decimal)dicContrato["TasaMoratoria"]);
                //Ubicación del Taller
                string ubicacionTaller = dicContrato["UbicacionTaller"].ToString();
                //Incluye Rotulación y/o Pintura
                string responsableRotulacionPintura = dicContrato["IncluyePinturaRotulacion"].ToString();
                //Incluye Lavado
                string responsableLavado = dicContrato["IncluyeLavado"].ToString();
                //Lugar en dónde se firma el contrato
                string ciudadEstadoMatriz = "";
                if (dicContrato["NombreCiudadMatriz"] != null)
                    ciudadEstadoMatriz += dicContrato["NombreCiudadMatriz"].ToString() + " ";
                if (dicContrato["NombreEstadoMatriz"] != null)
                    ciudadEstadoMatriz += dicContrato["NombreEstadoMatriz"].ToString();

                //¿El contrato está en Borrador?
                bool? esBorrador = true;
                if (dicContrato["EsBorrador"] != null)
                    esBorrador = (bool)dicContrato["EsBorrador"];

                //¿El Cliente es Físico o Moral?
                if (dicContrato["ClienteEsFisico"] != null)
                    esFisico = (bool)dicContrato["ClienteEsFisico"];
                else
                    esFisico = false;
                //¿Sólo aplican Representantes Legales al Contrato?
                if (dicContrato["SoloRepresentantes"] != null)
                    soloRepresentantes = (bool)dicContrato["SoloRepresentantes"];
                else
                    soloRepresentantes = false;
                //Nombre del Representante de la Empresa
                representanteUnidad = dicContrato["RepresentanteEmpresa"].ToString();

                string telefonoEmergencia1 = dicContrato["TelefonoEmergencia1"].ToString();
                string telefonoEmergencia2 = dicContrato["TelefonoEmergencia2"].ToString();
                string responsableTecnicos = dicContrato["ResponsableTecnicos"].ToString();
                #endregion

                //Marca de agua indicando que lo que se imprime es un borrador
                if (!(esBorrador != null && esBorrador == false))
                    this.Watermark.Text = "BORRADOR";

                content = content.Replace("{FECHA_CONTRATO}", fechaContrato);
                content = content.Replace("{NOMBRE_CLIENTE}", nombreCliente);
                content = content.Replace("{UNIDAD_OPERATIVA}", unidadOperativa);
                content = content.Replace("{OBLIGADOS_Y_DEPOSITARIOS}", nombresObligadosSoldarios);
                content = content.Replace("{DIRECCION_CLIENTE}", direccionCliente);
                content = content.Replace("{KILOMETRAJE_ANUAL_VEHICULO}", kilometrajeAnualVehiculo);
                content = content.Replace("{RESPONSABLE_SEGURO_UNIDAD}", responsableSeguroUnidad);
                content = content.Replace("{TELEFONO_EMERGENCIA_UNIDAD_OPERATIVA}", telefonoEmergencia1);
                content = content.Replace("{TELEFONO_EMERGENCIA_UNIDAD_OPERATIVA_2}", telefonoEmergencia2);
                content = content.Replace("{RESPONSABLE_TECNICOS}", responsableTecnicos);
                content = content.Replace("{DIA_REPORTA_CLIENTE}", diaReportaCliente);
                if (coberturaSeguro.Length > 0)
                {
                    var startTag = "<ul>";
                    int startIndex = content.IndexOf(startTag) + startTag.Length;
                    int endIndex = content.IndexOf("</ul>", startIndex);
                    string remplazar = "<ul>" + content.Substring(startIndex, endIndex - startIndex) + "</ul>";

                    content = content.Replace(remplazar, coberturaSeguro);
                }
                content = content.Replace("{DEPOSITO_GARANTIA}", depositoGarantia);
                content = content.Replace("{COMISION_APERTURA}", comisionApertura);
                content = content.Replace("{RESPONSABLE_LLANTAS}", responsableLlantas);
                content = content.Replace("{TASA_MORATORIA}", tasaMoratoria);
                content = content.Replace("{UBICACION_TALLER}", ubicacionTaller);
                content = content.Replace("{RESPONSABLE_ROTULACION_PINTURA}", responsableRotulacionPintura);
                content = content.Replace("{RESPONSABLE_LAVADO}", responsableLavado);
                content = content.Replace("{CIUDAD_ESTADO_MATRIZ}", ciudadEstadoMatriz);
                content = content.Replace("{FECHA_CONTRATO}", fechaContrato);

                #region Manejo de los Datos Adicionales del Anexo
                DataTable dtObservaciones = (DataTable)dicContrato["ObservacionesAdicionales"];
                DataTable dtDatosAdicionales = (DataTable)dicContrato["DatosAdicionales"];
                Boolean conObservaciones = false;

                if (dtObservaciones.Rows.Count > 0)
                {
                    content = this.AgregarSeccionObservaciones(content, dtObservaciones, xmlDocument, true);
                    conObservaciones = true;
                }

                if (dtDatosAdicionales.Rows.Count > 0)
                    content = this.AgregarSeccionDatosAdicionales(content, conObservaciones, dtDatosAdicionales, xmlDocument);
                else
                {
                    content = content.Replace("{DATOS_ADICIONALES}", dicContrato["ObservacionesAdicionales"].ToString());

                    if (dtObservaciones.Rows.Count == 0)
                        content = content.Replace("{DATOS_ADICIONALES_OBSERVACIONES}", dicContrato["ObservacionesAdicionales"].ToString());
                }
                #endregion

                xrrchContentAnexoAReverso.Html = content;
                UltimoAlto = xrrchContentAnexoAReverso.Height;

                #region Sección de firmas
                firmas = dicContrato["Firmas"] as Dictionary<string, List<Dictionary<string, string>>>;
                List<Dictionary<string, string>> Distribuidor = null;

                if (firmas["Distribuidor"] != null)
                    Distribuidor = firmas["Distribuidor"];

                List<Dictionary<string, string>> Representantes = firmas["Representantes"];
                List<Dictionary<string, string>> Obligados = firmas["ObligadosSolidarios"];

                numFirmas = Representantes.Count + Obligados.Count;
                if (Distribuidor != null)
                {
                    numFirmas = numFirmas + Distribuidor.Count;
                    ProcesaFirmas(Distribuidor);
                }

                ProcesaFirmas(Representantes);

                if (soloRepresentantes == false)
                    ProcesaFirmas(Obligados);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ImprimirReporte: " + ex.Message);
            }
        }
        protected XmlDocument ObtenerXMLDocumento(string urlXML)
        {
            try
            {
                string path = xmlUrl + urlXML;
                if (File.Exists(path) != true)
                    throw new Exception(@"La plantilla correspondiente al reporte ""ANEXO A"" no se encuentra disponible", new Exception("El archivo " + nombreArchivo + " no se encuentra en la ubicación necesaria."));
                else
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(path);
                    return xDoc;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.nombreClase + ".ObtenerXMLDocumento: El formato del archivo xml es incorrecto. " + ex.Message);
            }
        }

        public string ObtenerLeyendaFirmas()
        {
            string leyendaFirma;
            XmlDocument xDoc = ObtenerXMLDocumento(xmlFirmas);
            if (esFisico)
            {
                XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaFisico");
                if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

                leyendaFirma = textoLeyenda[0].InnerText;

                //Reemplazo de Partes de la Leyenda
                List<Dictionary<string, string>> Obligados = firmas["ObligadosSolidarios"];
                string obligadosSol = this.reemplazoIntermediarios(Obligados, false);

                leyendaFirma = leyendaFirma.Replace("[DOCUMENTO]", documento.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[FECHA_CONTRATO]", fechaContrato.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[UNIDAD_OPERATIVA]", unidadOperativa.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[REPRESENTANTE_UNIDAD]", representanteUnidad.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[NOMBRE_CLIENTE]", nombreCliente.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", obligadosSol.ToUpper());
                if (Obligados.Count > 1)
                {
                    leyendaFirma = leyendaFirma.Replace("[S]", "S");
                    leyendaFirma = leyendaFirma.Replace("[N]", "N");
                }
                else
                {
                    leyendaFirma = leyendaFirma.Replace("[S]", "");
                    leyendaFirma = leyendaFirma.Replace("[N]", "");
                }
                    
            }
            else
            {
                XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaMoral");
                if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

                leyendaFirma = textoLeyenda[0].InnerText;

                //Reemplazo de Partes de la Leyenda
                List<Dictionary<string, string>> representantes = firmas["Representantes"];
                List<Dictionary<string, string>> obligados = firmas["ObligadosSolidarios"];
                string representantesLeg = this.reemplazoIntermediarios(representantes, true);
                string obligadosSol = this.reemplazoIntermediarios(obligados, false);

                leyendaFirma = leyendaFirma.Replace("[DOCUMENTO]", documento.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[FECHA_CONTRATO]", fechaContrato.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[UNIDAD_OPERATIVA]", unidadOperativa.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[REPRESENTANTE_UNIDAD]", representanteUnidad.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[NOMBRE_CLIENTE]", nombreCliente.ToUpper());
                leyendaFirma = leyendaFirma.Replace("[REPRESENTANTE_LEGALES_CLIENTE]", representantesLeg.ToUpper());

                if (soloRepresentantes == false)
                {
                    string textoSoloRepresentante;
                    XmlNodeList textoRepresentantes = xDoc.GetElementsByTagName("SoloRepresentantes");
                    if (textoRepresentantes.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

                    textoSoloRepresentante = textoRepresentantes[0].InnerText;

                    textoSoloRepresentante = textoSoloRepresentante.Replace("[OBLIGADOS_SOLIDARIOS]", obligadosSol).ToUpper();

                    if (obligados.Count > 1)
                        textoSoloRepresentante = textoSoloRepresentante.Replace("[S]", "S");
                    else
                        textoSoloRepresentante = textoSoloRepresentante.Replace("[S]", "");

                    leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", textoSoloRepresentante.ToUpper());
                }
                else
                    leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", ".");

                leyendaFirma = leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", obligadosSol.ToUpper());
            }
            return leyendaFirma;
        }
        protected string reemplazoIntermediarios(List<Dictionary<string, string>> intermediario, bool esRep)
        {
            string txtIntermediario = string.Empty;
            int intermediariosCount = intermediario.Count;

            foreach (Dictionary<string, string> dic in intermediario)
            {
                if (dic["Titulo"].ToString().Contains("DISTRIBUIDOR"))
                {
                    intermediariosCount = intermediariosCount - 1;
                    break;
                }
            }

            foreach (Dictionary<string, string> dic in intermediario)
            {

                if (!dic["Titulo"].ToString().Contains("DISTRIBUIDOR") && esRep == true && esFisico == false)
                {
                    int existe = txtIntermediario.IndexOf(dic["NombreRepresentante"].Replace("Rep. Legal: ", string.Empty), System.StringComparison.Ordinal);

                    if (intermediariosCount > 0 && intermediariosCount > 1)
                    {
                        if (dic == intermediario[0])
                        {
                            txtIntermediario = dic["NombreRepresentante"].Replace("Rep. Legal: ", string.Empty);
                        }
                        else if (dic == intermediario[intermediariosCount - 1] && existe == -1)
                        {
                            txtIntermediario = txtIntermediario + " Y " + dic["NombreRepresentante"].Replace("Rep. Legal: ", string.Empty);
                        }
                        else if (existe == -1)
                        {
                            txtIntermediario = txtIntermediario + ", " + dic["NombreRepresentante"].Replace("Rep. Legal: ", string.Empty);
                        }
                    }
                    else
                    {
                        txtIntermediario = dic["NombreRepresentante"].Replace("Rep. Legal: ", string.Empty);
                    }
                }
                else if (!dic["Titulo"].ToString().Contains("DISTRIBUIDOR"))
                {
                    int existe = txtIntermediario.IndexOf(dic["Nombre"], System.StringComparison.Ordinal);

                    if (intermediariosCount > 0 && intermediariosCount > 1)
                    {
                        if (dic == intermediario[0])
                        {
                            txtIntermediario = dic["Nombre"];
                        }
                        else if (dic == intermediario[intermediariosCount - 1] && existe == -1)
                        {
                            txtIntermediario = txtIntermediario + " Y " + dic["Nombre"];
                        }
                        else if (existe == -1)
                        {
                            txtIntermediario = txtIntermediario + ", " + dic["Nombre"];
                        }
                    }
                    else
                    {
                        txtIntermediario = dic["Nombre"];
                    }
                }
            }
            return txtIntermediario;
        }
        protected string ObtenerFirma(Dictionary<string, string> firma1, Dictionary<string, string> firma2 = null)
        {
            string firma;
            XmlDocument xDoc = ObtenerXMLDocumento(xmlFirmas);

            if (firma2 != null)
            {
                XmlNodeList textoFirma = xDoc.GetElementsByTagName("Firmas");
                if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

                firma = textoFirma[0].InnerText;

                //Firma Izquierda
                firma = firma.Replace("[TITULO_IZQUIERDA]", firma1["Titulo"]);
                firma = firma.Replace("[NOMBRE_IZQUIERDA]", firma1["Nombre"]);
                firma = firma.Replace("[NOMBRE_REP_IZQUIERDA]", firma1["NombreRepresentante"]);
                firma = firma.Replace("[DIRECCION_IZQUIERDA]", string.Empty);

                //Firma Derecha
                firma = firma.Replace("[TITULO_DERECHA]", firma2["Titulo"]);
                firma = firma.Replace("[NOMBRE_DERECHA]", firma2["Nombre"]);
                firma = firma.Replace("[NOMBRE_REP_DERECHA]", firma2["NombreRepresentante"]);
                firma = firma.Replace("[DIRECCION_DERECHA]", string.Empty);
            }
            else
            {
                XmlNodeList textoFirma = xDoc.GetElementsByTagName("FirmasCentro");
                if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

                firma = textoFirma[0].InnerText;

                firma = firma.Replace("[TITULO_CENTRO]", firma1["Titulo"]);
                firma = firma.Replace("[NOMBRE_CENTRO]", firma1["Nombre"]);
                firma = firma.Replace("[NOMBRE_REP_CENTRO]", firma1["NombreRepresentante"]);
                firma = firma.Replace("[DIRECCION_CENTRO]", string.Empty);
            }
            return firma;
        }
        protected void ProcesaFirmas(List<Dictionary<string, string>> firmas)
        {
            int cantFirmas = firmas.Count;
            if (cantFirmas == 0) return;

            if (cantFirmas == 1)
            {
                numFirmaActual += 1;
                SetFirma(ObtenerFirma(firmas[0]), numFirmaActual == 1, numFirmas == numFirmaActual);
                return;
            }
            bool esPar = cantFirmas % 2 == 0;
            int firmaActual = 2;
            while (firmaActual <= cantFirmas + 1)
            {
                if (esPar)
                {
                    numFirmaActual += 2;
                    SetFirma(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]), numFirmaActual == 1, numFirmas == numFirmaActual);
                }
                else
                {
                    if (firmaActual > cantFirmas)
                    {
                        numFirmaActual += 1;
                        SetFirma(ObtenerFirma(firmas[cantFirmas - 1]), numFirmaActual == 1, numFirmas == numFirmaActual);
                    }
                    else
                    {
                        numFirmaActual += 2;
                        SetFirma(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]), numFirmaActual == 1, numFirmas == numFirmaActual);
                    }
                }
                firmaActual += 2;
            }
        }
        protected void SetFirma(string htmlFirmas, bool firstElement, bool lastElement)
        {
            CreateDocument();
            //Se obtiene la página actual antes de crear el documento
            int currentPage = PageCount;
            if (firstElement) paginaInicioFirmas = currentPage;
            XRRichText richText = new XRRichText();
            richText.Html = htmlFirmas;
            richText.KeepTogether = true;
            richText.LocationF = new PointF(4, UltimaPosicion + UltimoAlto + 3);
            richText.WidthF = PageSize.Width - MargenesHorizontales - 10;
            richText.Html = htmlFirmas;
            DtlAnexoAReversoFirmas1Content.Controls.Add(richText);
            UltimoAlto = richText.HeightF;
            lastPage = currentPage;
            //Una vez agregado el control se vuelve a generar para saber donde quedó
            CreateDocument();
            currentPage = PageCount;
            if (currentPage > lastPage)
                richText.Html = htmlFirmas.Replace("[Leyenda]", ObtenerLeyendaFirmas());
            else richText.Html = htmlFirmas.Replace("[Leyenda]", string.Empty);

            if (lastElement)
                CreateDocument();
        }

        protected string AgregarSeccionObservaciones(string content, DataTable dtObservaciones, XmlDocument xmlDocument, bool conObs)
        {
            XmlNodeList xmlContenidoObservaciones = xmlDocument.GetElementsByTagName("PlantillaObservaciones");
            XmlNodeList xmlContenidoObservacionesConTitulo = xmlDocument.GetElementsByTagName("ObservacionConTitulo");
            XmlNodeList xmlContenidoObservacionesSinTitulo = xmlDocument.GetElementsByTagName("ObservacionSinTitulo");

            string contentObservaciones = xmlContenidoObservaciones[0].InnerText;
            string contentObsConTitulo = xmlContenidoObservacionesConTitulo[0].InnerText;
            string contentObsSinTitulo = xmlContenidoObservacionesSinTitulo[0].InnerText;

            string observaciones = String.Empty;

            if (conObs == true)
            { 
                contentObservaciones = contentObservaciones.Replace("{OBSERVACIONES}", "OBSERVACIONES");
                foreach (DataRow row in dtObservaciones.Rows)
                {
                    if (row["Titulo"] != null && row["Titulo"].ToString() != String.Empty)
                    {
                        observaciones = observaciones + contentObsConTitulo.Replace("{TITULO}", row["Titulo"].ToString());
                        observaciones = observaciones.Replace("{DESCRIPCION}", row["Observacion"].ToString());
                    }
                    else
                    {
                        observaciones = observaciones + contentObsSinTitulo.Replace("{DESCRIPCION}", row["Observacion"].ToString());
                    }
                }
            }
            else
            {                
                foreach (DataRow row in dtObservaciones.Rows)
                {
                    if (row["Titulo"] != null && row["Titulo"].ToString() != String.Empty)
                    {
                        contentObservaciones = contentObservaciones.Replace("{OBSERVACIONES}", row["Titulo"].ToString());
                        observaciones = observaciones + contentObsConTitulo.Replace("{TITULO}", string.Empty);
                        observaciones = observaciones.Replace("{DESCRIPCION}", row["Observacion"].ToString());
                    }
                    else
                    {
                        contentObservaciones = contentObservaciones.Replace("{OBSERVACIONES}", row["Observacion"].ToString());
                        observaciones = observaciones + contentObsSinTitulo.Replace("{DESCRIPCION}", string.Empty);
                    }
                }
            }            

            contentObservaciones = contentObservaciones.Replace("[OBSERVACIONES_O_DATOS]", observaciones);
            content = content.Replace("{DATOS_ADICIONALES_OBSERVACIONES}", contentObservaciones);

            return content;
        }
        protected string AgregarSeccionDatosAdicionales(string content, bool conObservaciones, DataTable dtDatosAdicionales, XmlDocument xmlDocument)
        {
            string datosAdicionales = String.Empty;
            string contentGral = string.Empty;

                int countPar = 1;
                int index = 19; //A partir de qué número de sección empieza a contabilizarse

                XmlNodeList xmlContenidoDA = xmlDocument.GetElementsByTagName("PlantillaDatosAdicionales");
                XmlNodeList xmlContenidoDACNTitulo = xmlDocument.GetElementsByTagName("DAConTitulo");
                XmlNodeList xmlContenidoDASNTitulo = xmlDocument.GetElementsByTagName("DASinTitulo");

                string contentDA = xmlContenidoDA[0].InnerText;
                string contentDACNTitulo = xmlContenidoDACNTitulo[0].InnerText;
                string contentDASNTitulo = xmlContenidoDASNTitulo[0].InnerText;
                

                foreach (DataRow row in dtDatosAdicionales.Rows)
                {
                    if (conObservaciones == false && row == dtDatosAdicionales.Rows[0])
                    {
                        DataTable dtDASNObservaciones = new DataTable();
                        dtDASNObservaciones.Columns.Add("Titulo", typeof(string));
                        dtDASNObservaciones.Columns.Add("Observacion", typeof(string));

                        DataRow newRow = dtDASNObservaciones.NewRow();
                        newRow["Titulo"] = dtDatosAdicionales.Rows[0]["Titulo"];
                        newRow["Observacion"] = dtDatosAdicionales.Rows[0]["Observacion"];
                        dtDASNObservaciones.Rows.Add(newRow);

                        content = this.AgregarSeccionObservaciones(content, dtDASNObservaciones, xmlDocument, false);
                    }
                    else
                    {
                        if (countPar % 2 != 0)
                        {
                            datosAdicionales = contentDA;

                            if (row["Titulo"] != null && row["Titulo"] != string.Empty)
                            {
                                datosAdicionales = datosAdicionales.Replace("[IMPAR]", (contentDACNTitulo.Replace("{INDEX_TITULO}", index + ") " + row["Titulo"])));
                                datosAdicionales = datosAdicionales.Replace("{DESCRIPCION}", row["Observacion"].ToString());
                            }
                            else
                            {
                                datosAdicionales = datosAdicionales.Replace("[IMPAR]", (contentDASNTitulo.Replace("{INDEX_DESCRIPCION}", index + ") " + row["Observacion"])));
                            }

                            if (row == dtDatosAdicionales.Rows[dtDatosAdicionales.Rows.Count - 1])
                            {
                                datosAdicionales = datosAdicionales.Replace("[PAR]", string.Empty);
                                contentGral = contentGral + datosAdicionales;
                            }
                        }
                        else
                        {
                            if (row["Titulo"] != null && row["Titulo"] != String.Empty)
                            {
                                datosAdicionales = datosAdicionales.Replace("[PAR]", (contentDACNTitulo.Replace("{INDEX_TITULO}", index + ") " + row["Titulo"])));
                                datosAdicionales = datosAdicionales.Replace("{DESCRIPCION}", row["Observacion"].ToString());

                            }
                            else
                            {
                                datosAdicionales = datosAdicionales.Replace("[PAR]", (contentDASNTitulo.Replace("{INDEX_DESCRIPCION}", index + ") " + row["Observacion"])));
                            }

                            contentGral = contentGral + datosAdicionales;
                        }
                        countPar++;
                        index++;
                    }                    
                }

            content = content.Replace("{DATOS_ADICIONALES}", contentGral);    
            return content;
        }
        #endregion
    }
}
