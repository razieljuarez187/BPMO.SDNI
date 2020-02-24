// Satisface al CU019 - imprimir Anexo A
using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
    public partial class AnexoARevRPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Atributos

        public static string NombreArchivo = "BPMO.SDNI.Reportes.Anexo.A.xml";
        #region SC0007
        private int PaginaInicioFirmas;
        private float UltimoAlto;
        private int lastPage = 0;
        private int numFirmaActual;
        private int numFirmas;
        private string xmlFirmas = "BPMO.SDNI.Reportes.Firmas.xml";
        private string xmlUrl;
        private Boolean esFisico;
        private Dictionary<string, List<Dictionary<string, string>>> firmas;
        private string representanteUnidad;
        private string fechaContrato;
        private string unidadOperativa;
        private string nombreCliente;
        private string numeroContrato;
        private string documento = "REVERSO DEL ANEXO A";
        private bool? soloRepresentantes;
        #endregion
        #endregion

        #region Propiedades
        #region SC0007
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
        #endregion


        #region Metodos

        #region SC0007
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
                List<Dictionary<string, string>> depositarios = firmas["Depositarios"];
                string representantesLeg = this.reemplazoIntermediarios(representantes, true);
                string obligadosSol = this.reemplazoIntermediarios(obligados, false);
                string depos = this.reemplazoIntermediarios(depositarios, false);

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

                    textoSoloRepresentante = textoSoloRepresentante.Replace("[REPRESENTANTES_LEGALES_COMO_DEPOSITARIOS]", depos.ToUpper());

                    if (depositarios.Count > 1)
                        textoSoloRepresentante = textoSoloRepresentante.Replace("[S_DEPOSITARIO]", "S");
                    else
                        textoSoloRepresentante = textoSoloRepresentante.Replace("[S_DEPOSITARIO]", "");

                    leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", textoSoloRepresentante.ToUpper());
                }
                else
                    leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", ".");
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
        #endregion

        protected XmlDocument ObtenerXMLDocumento(string urlXML)
        {
            try
            {
                string path = xmlUrl + urlXML;
                if (File.Exists(path) != true)
                    throw new Exception(@"La plantilla correspondiente al reporte ""ANEXO A"" no se encuentra disponible", new Exception("El archivo " + NombreArchivo + " no se encuentra en la ubicación necesaria."));
                else
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(path);
                    return xDoc;
                }                
            }
            catch (Exception)
            {
                throw new Exception("El formato del archivo xml es incorrecto");
            }
        }

        public void ImprimirReporte(Dictionary<String, Object> dicContratoFSL,  XmlDocument xmlDocument)
        {
            #region Consulta XML
            XmlNodeList xmlContenidoRev = xmlDocument.GetElementsByTagName("ContenidoReverso");
            
            string content = xmlContenidoRev[0].InnerText;
            #region SC0007
            fechaContrato = dicContratoFSL["fechaContrato"].ToString();            
            nombreCliente = dicContratoFSL["nombreCliente"].ToString();
            unidadOperativa = dicContratoFSL["nombreEmpresa"].ToString();
            numeroContrato = dicContratoFSL.ContainsKey("NumeroContrato") ? dicContratoFSL["NumeroContrato"].ToString() : "NO ENCONTRADO";

            content = content.Replace("{NUMERO_CONTRATO}", numeroContrato);
            content = content.Replace("{FECHA_CONTRATO}", fechaContrato);
            content = content.Replace("{NOMBRE_CLIENTE}", nombreCliente);
            content = content.Replace("{UNIDAD_OPERATIVA}", unidadOperativa);           
            content = content.Replace("{OBLIGADOS_Y_DEPOSITARIOS}", dicContratoFSL["obligadosYDepositarios"].ToString());
            #endregion
            content = content.Replace("{DIRECCION_CLIENTE}", dicContratoFSL["direccionesCliente"].ToString());
            content = content.Replace("{KILOMETRAJE_ANUAL_VEHICULO}", dicContratoFSL["kmEstimadoPorVehiculo"].ToString());
            content = content.Replace("{RESPONSABLE_SEGURO_UNIDAD}", dicContratoFSL["responsableSeguro"].ToString());

            if (dicContratoFSL["coberturaSeguro"].ToString().Length > 0)
            {
                var startTag = "<ul>";
                int startIndex = content.IndexOf(startTag) + startTag.Length;
                int endIndex = content.IndexOf("</ul>", startIndex);
                string remplazar = "<ul>" + content.Substring(startIndex, endIndex - startIndex) + "</ul>";

                content = content.Replace(remplazar, dicContratoFSL["coberturaSeguro"].ToString());
            }

            content = content.Replace("{DEPOSITO_GARANTIA}", dicContratoFSL["depositoGarantia"].ToString());
            content = content.Replace("{COMISION_APERTURA}", dicContratoFSL["comisionApertura"].ToString());
            content = content.Replace("{RESPONSABLE_LLANTAS}", dicContratoFSL["incluyeLlantas"].ToString());
            content = content.Replace("{TASA_MORATORIA}", dicContratoFSL["tasaMoratoria"].ToString());
            content = content.Replace("{UBICACION_TALLER}", dicContratoFSL["ubicacionTaller"].ToString());
            content = content.Replace("{RESPONSABLE_ROTULACION_PINTURA}", dicContratoFSL["rotulacionPintura"].ToString());
            content = content.Replace("{RESPONSABLE_LAVADO}", dicContratoFSL["lavado"].ToString());
            content = content.Replace("{CIUDAD_ESTADO_MATRIZ}", dicContratoFSL["lugarFirma"].ToString());
            content = content.Replace("{FECHA_CONTRATO}", dicContratoFSL["fechaFirma"].ToString());

            #region SC0007
            DataTable dtObservaciones = (DataTable)dicContratoFSL["observaciones"];
            DataTable dtDatosAdicionales = (DataTable)dicContratoFSL["datosAdicionales"];

            Boolean conObservaciones = false;

            if (dtObservaciones.Rows.Count > 0)
            {
                content = this.AgregarSeccionObservaciones(content, dtObservaciones, xmlDocument, true);
                conObservaciones = true;
            }

            if (dtDatosAdicionales.Rows.Count > 0)
            {
                content = this.AgregarSeccionDatosAdicionales(content, conObservaciones, dtDatosAdicionales, xmlDocument);
            }
            else
            {                
                content = content.Replace("{DATOS_ADICIONALES}", dicContratoFSL["observaciones"].ToString());

                if (dtObservaciones.Rows.Count == 0)
                    content = content.Replace("{DATOS_ADICIONALES_OBSERVACIONES}", dicContratoFSL["observaciones"].ToString());
            }
            #endregion
            xrrchContentAnexoAReverso.Html = content;
            UltimoAlto = xrrchContentAnexoAReverso.Height;

            #endregion

            if ((bool)dicContratoFSL["esBorrador"] == true)
                this.Watermark.Text = "BORRADOR";

            #region SC0007
            esFisico = (bool)dicContratoFSL["tipoCliente"];
            soloRepresentantes = (bool)dicContratoFSL["soloRepresentantes"];
            representanteUnidad = dicContratoFSL["representanteEmpresa"].ToString();

            firmas =
                dicContratoFSL["firmas"] as Dictionary<string, List<Dictionary<string, string>>>;
            List<Dictionary<string, string>> Distribuidor = null;

            if(firmas["Distribuidor"] != null)
                Distribuidor = firmas["Distribuidor"];

            List<Dictionary<string, string>> Representantes = firmas["Representantes"];
            List<Dictionary<string, string>> Obligados = firmas["ObligadosSolidarios"];
            List<Dictionary<string, string>> Depositarios = firmas["Depositarios"];

            numFirmas = Representantes.Count + Obligados.Count + Depositarios.Count;
            if (Distribuidor != null)
            {
                numFirmas = numFirmas + Distribuidor.Count;
                ProcesaFirmas(Distribuidor);
            }
                
            ProcesaFirmas(Representantes);

            if (soloRepresentantes == false)
            {
                ProcesaFirmas(Obligados);
                ProcesaFirmas(Depositarios);
            }            
            #endregion

            
        }

        #region SC0007

        protected void SetFirma(string htmlFirmas, bool firstElement, bool lastElement)
        {
            CreateDocument();
            //Se obtiene la página actual antes de crear el documento
            int currentPage = PageCount;
            if (firstElement) PaginaInicioFirmas = currentPage;
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
                    if (row["titulo"] != null && row["titulo"].ToString() != String.Empty)
                    {
                        observaciones = observaciones + contentObsConTitulo.Replace("{TITULO}", row["titulo"].ToString());
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
                    if (row["titulo"] != null && row["titulo"].ToString() != String.Empty)
                    {
                        contentObservaciones = contentObservaciones.Replace("{OBSERVACIONES}", row["titulo"].ToString());
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
                int index = 17;

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
        #endregion

        #region Constructores
        public AnexoARevRPT(Dictionary<String, Object> dicContratoFSL, String urlXML)
        {
            InitializeComponent();
            xmlUrl = urlXML;
            this.ImprimirReporte(dicContratoFSL, this.ObtenerXMLDocumento(NombreArchivo));
        }
        #endregion

    }
}
