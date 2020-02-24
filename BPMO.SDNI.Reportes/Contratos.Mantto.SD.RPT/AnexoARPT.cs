// Satisface al CU099 - Imprimir Anexo A de Mantenimiento y Servicio Dedicado
using System;
using System.IO;
using System.Xml;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace BPMO.SDNI.Contratos.Mantto.SD.RPT
{
    public partial class AnexoARPT : DevExpress.XtraReports.UI.XtraReport
    {
        #region Atributos
        public static string NombreArchivo = "BPMO.SDNI.Reportes.Anexo.A.SD.xml";
        
        private int PaginaInicioFirmas;
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
        private string documento = "ANVERSO DEL ANEXO A";
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
                return CantidadControles == 0 ? 0 : DtlAnexoAAnversoFirmasContent.Controls[CantidadControles - 1].LocationF.Y;
            }
        }
        /// <summary>
        /// Devuelve la cantidad de controles que estén en el layout
        /// </summary>
        private int CantidadControles
        {
            get
            {
                return DtlAnexoAAnversoFirmasContent.Controls.Count;
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
        public AnexoARPT(Dictionary<String, Object> dicContratoFSL, String urlXML)
        {
            InitializeComponent();
            xmlUrl = urlXML;
            this.ImprimirReporte(dicContratoFSL, this.ObtenerXmlContenido(NombreArchivo));
        }
        #endregion

        #region Metodos
        public void ImprimirReporte(Dictionary<String, Object> dicContrato, XmlDocument xmlDocument)
        {
            #region Consulta XML

            XmlNodeList xmlAnexo = xmlDocument.GetElementsByTagName("AnexoA");
            XmlNodeList xmlAnverso = xmlDocument.GetElementsByTagName("Anverso");
            XmlNodeList xmlCabecera = xmlDocument.GetElementsByTagName("Cabecera");

            xrrchEncabezadoAnverso.Html = xmlCabecera[0].InnerText;

            #endregion

            #region Asignación de valores generales
            //Fecha del Contrato
            fechaContrato = dicContrato["FechaContrato"].ToString();
            //Nombre del Cliente
            nombreCliente = dicContrato["NombreCliente"].ToString();
            //Nombre de la Empresa
            unidadOperativa = dicContrato["NombreEmpresa"].ToString();

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
            //Código de la Moneda del Contrato
            string codigoMonedaContrato = dicContrato["CodigoMonedaContrato"].ToString();
            #endregion

            //Marca de agua indicando que lo que se imprime es un borrador
            if (!(esBorrador != null && esBorrador == false))
                this.Watermark.Text = "BORRADOR";

            this.xrlblFechaContratoM.Text = fechaContrato;
            this.xrlblResponsable.Text = unidadOperativa;
            this.xrlblNombreCliente.Text = nombreCliente;

            this.DtlAnexoAAnverso.DataSource = dicContrato["Unidades"];
            this.xrtbltxtFechaIni.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "FechaInicioContrato", "{0:dd-MMM-yyyy}");
            this.xrtblTxtTerminoContrato.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "FechaTerminoContrato", "{0:dd-MMM-yyyy}");
            this.xrtblTxtAnio.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "Anio");
            this.xrtblTxtMarcaUnidad.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "NombreMarca");
            this.xrtblTxtModeloTipo.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "NombreModelo");
            this.xrtblTxtNumSerie.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "NumeroSerie");
            this.xrtblTxtPbvMax.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "PBV", "{0:###,##0.00##}");
            this.xrtblTxtPesoAuto.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "PBC", "{0:###,##0.00##}");
            this.xrtblTxtCargoMes.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "CargoFijoMensual", "{0:###,##0.00##} " + codigoMonedaContrato + " +IVA");
            this.xrtblTxtKmIncluido.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "KmIncluido", "{0:###,##0}");
            this.xrtblTxtPeriodoKmIncluido.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "PeriodoKmIncluido");
            this.xrtblTxtCargoKg.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "CargoKilometro"); //, "{0:###,##0.00##} " + codigoMonedaContrato + " +IVA");
            this.xrtblTxtHrsIncluidas.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "HrsIncluidas", "{0:###,##0}");
            this.xrtblTxtPeriodoHrsIncluidas.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "PeriodoHrsIncluidas");
            this.xrtblTxtCargoHra.DataBindings.Add("Text", this.DtlAnexoAAnverso.DataSource, "CargoHoraRefrigerada"); //, "{0:###,##0.00##} " + codigoMonedaContrato + " +IVA");
            
            #region Sección de Firmas
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
        protected XmlDocument ObtenerXmlContenido(string urlXML)
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

        public string ObtenerLeyendaFirmas()
        {
            string leyendaFirma;
            XmlDocument xDoc = ObtenerXmlContenido(xmlFirmas);
            if (esFisico)
            {
                XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaFisico");
                if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

                leyendaFirma = textoLeyenda[0].InnerText;

                //Reemplazo de Partes de la Leyenda
                List<Dictionary<string, string>> Obligados = firmas["ObligadosSolidarios"];
                string obligadosSol = this.ReemplazoIntermediarios(Obligados, false);

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
                string representantesLeg = this.ReemplazoIntermediarios(representantes, true);
                string obligadosSol = this.ReemplazoIntermediarios(obligados, false);

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
        protected string ReemplazoIntermediarios(List<Dictionary<string, string>> intermediario, bool esRep)
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
            XmlDocument xDoc = ObtenerXmlContenido(xmlFirmas);

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
            if (firstElement) PaginaInicioFirmas = currentPage;
            XRRichText richText = new XRRichText();
            richText.Html = htmlFirmas;
            richText.KeepTogether = true;
            richText.LocationF = new PointF(4, UltimaPosicion + UltimoAlto + 3);
            richText.WidthF = PageSize.Width - MargenesHorizontales - 10;
            richText.Html = htmlFirmas;
            DtlAnexoAAnversoFirmasContent.Controls.Add(richText);
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
        #endregion
    }
}
