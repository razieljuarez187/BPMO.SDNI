//Satisface al Caso de Uso CU016: Imprimir Constancia de Entrega de Bienes
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Xml;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
	public partial class ConstanciaEntregaBienesRPT : DevExpress.XtraReports.UI.XtraReport
	{
		#region atributos

		private string xml = "BPMO.SDNI.Reportes.Constancia.Entrega.Bienes.xml";

		#region SC0007

		private int lastPage;
		private int numFirmaActual;
		private int numFirmas;
		private float UltimoAlto;
		private string xmlFirmas = "BPMO.SDNI.Reportes.Firmas.xml";
		private string xmlUrl;
		private bool esFisico;
		private List<Dictionary<string, string>> repLegales;
		private List<Dictionary<string, string>> obSolidarios;
		private List<Dictionary<string, string>> depositarios;
		private string fechaContrato;
		private string unidadOperativa;
		private string representanteUnidad;
		private string nombreCliente;
	    private string direccionCliente;
	    private bool? soloRepresentantes;
        /// <summary>
        /// Devuelve la cantidad de controles que estén en el layout
        /// </summary>
        private int CantidadControles
        { get { return Detail4.Controls.Count; } }

        /// <summary>
        /// Devuelve los márgenes horizontales
        /// </summary>
        private float MargenesHorizontales
        {
            get { return this.Margins.Left + this.Margins.Right + 1; }
        }

        private int PageCount
        {
            get { return PrintingSystem.Document.PageCount; }
        }

        private float UltimaPosicion
        {
            get
            {
                return CantidadControles == 0 ? 0 : Detail4.Controls[CantidadControles - 1].LocationF.Y;
            }
        }
		#endregion SC0007

		#endregion atributos

		#region Metodos

		#region SC0007

	    protected string ObtenerLeyendaFirmas()
	    {
	        string leyendaFirma;
	        string concatenacion = string.Empty;
	        XmlDocument xDoc = ObtenerXMLContenido(xmlFirmas);
	        if (esFisico)
	        {
	            XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaFisico");
	            if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

	            leyendaFirma = textoLeyenda[0].InnerText;

	        }
	        else
	        {
	            XmlNodeList textoLeyenda = xDoc.GetElementsByTagName("LeyendaMoral");
	            if (textoLeyenda.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

	            leyendaFirma = textoLeyenda[0].InnerText;
	            if (soloRepresentantes != null)
	            {
	                if (soloRepresentantes == false)
	                {
	                    XmlNodeList textoAdicional = xDoc.GetElementsByTagName("SoloRepresentantes");
	                    if (textoAdicional.Count < 1)
	                        throw new Exception("El formato del archivo xml es incorrecto para los textos opcionales");
	                    leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", textoAdicional[0].InnerText);
	                }
	                else
	                {
	                    leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", ".");
	                }
	            }
	            else
	            {
	                XmlNodeList textoAdicional = xDoc.GetElementsByTagName("SoloRepresentantes");
	                if (textoAdicional.Count < 1)
	                    throw new Exception("El formato del archivo xml es incorrecto para los textos opcionales");
	                leyendaFirma = leyendaFirma.Replace("[SOLOREPRESENTANTES]", textoAdicional[0].InnerText);
	            }

	            string representantes = "";
	            string depositaris = "";

	            if (repLegales != null)
	            {
	                foreach (Dictionary<string, string> reps in repLegales)
	                {
	                    if (reps == repLegales[0])
	                    {
	                        concatenacion = string.Empty;
	                    }
	                    if (reps == repLegales[(repLegales.Count - 1)] && repLegales.Count>1)
	                    {
	                        concatenacion = " Y ";
	                    }
	                    if (reps != repLegales[0] && reps != repLegales[(repLegales.Count - 1)])
	                    {
	                        concatenacion = ",";
	                    }
	                    representantes += concatenacion + reps["NombreRepresentante"].ToString().Replace("Rep. Legal:", "");
	                }
	                leyendaFirma = leyendaFirma.Replace("[REPRESENTANTE_LEGALES_CLIENTE]", representantes);
	            }

	            if (depositarios != null)
	            {
	                foreach (Dictionary<string, string> deps in depositarios)
	                {
	                    if (deps == depositarios[0])
	                    {
	                        concatenacion = string.Empty;
	                    }
	                    if (deps == depositarios[(depositarios.Count - 1)] && depositarios.Count>1)
	                    {
	                        concatenacion = " Y ";
	                    }
	                    if (deps != depositarios[0] && deps != depositarios[(depositarios.Count - 1)])
	                    {
	                        concatenacion = ",";
	                    }
	                    depositaris += concatenacion + deps["Nombre"].ToString();
	                }
	                leyendaFirma = leyendaFirma.Replace("[REPRESENTANTES_LEGALES_COMO_DEPOSITARIOS]", depositaris);
	                leyendaFirma = leyendaFirma.Replace("[S_DEPOSITARIO]", depositarios.Count > 1 ? "S" : "");
	            }
	        }
	        leyendaFirma = leyendaFirma.Replace("[FECHA_CONTRATO]", fechaContrato);
	        leyendaFirma = leyendaFirma.Replace("[UNIDAD_OPERATIVA]", unidadOperativa);
	        leyendaFirma = leyendaFirma.Replace("[REPRESENTANTE_UNIDAD]", representanteUnidad);
	        leyendaFirma = leyendaFirma.Replace("[NOMBRE_CLIENTE]", nombreCliente);
	        string obligados = "";
	        if (obSolidarios != null)
	        {
	            foreach (Dictionary<string, string> obSolidario in obSolidarios)
	            {
	                if (obSolidario == obSolidarios[0])
	                {
	                    concatenacion = string.Empty;
	                }
	                if (obSolidario == obSolidarios[(obSolidarios.Count - 1)] && obSolidarios.Count>1)
	                {
	                    concatenacion = " Y ";
	                }
	                if (obSolidario != obSolidarios[0] && obSolidario != obSolidarios[(obSolidarios.Count - 1)])
	                {
	                    concatenacion = ", ";
	                }

	                if ((String.IsNullOrEmpty(obSolidario["NombreRepresentante"].ToString())) &&
	                    (String.IsNullOrWhiteSpace(obSolidario["NombreRepresentante"].ToString())))
	                {
	                    obligados += concatenacion + obSolidario["Nombre"].ToString();
	                }
	                else
	                {
	                    string obligadoMoral = obSolidario["Nombre"].ToString();
	                    int buscar = obligados.IndexOf(obligadoMoral, System.StringComparison.Ordinal);
	                    if (buscar == -1)
	                    {
	                        obligados += concatenacion + obSolidario["Nombre"].ToString();
	                    }
	                    else
	                    {
	                        if (concatenacion.Trim().ToUpper() == "Y")
	                        {
	                            string ultimoObligadoMoral = concatenacion + obSolidario["Nombre"].ToString();
                                int ultimaComa= obligados.LastIndexOf(",");
                                if(ultimaComa>=0)
	                            obligados = obligados.Substring(0, ultimaComa - 1)+ultimoObligadoMoral;
	                        }
	                            
	                    }

	                }

	            }
	            leyendaFirma = leyendaFirma.Replace("[S]", obSolidarios.Count > 1 ? "S" : "");
	            leyendaFirma = leyendaFirma.Replace("[N]", obSolidarios.Count > 1 ? "N" : "");
                leyendaFirma = leyendaFirma.Replace("[OBLIGADOS_SOLIDARIOS]", obligados);
	        }

			leyendaFirma = leyendaFirma.Replace("[DOCUMENTO]", "DE LA CONSTANCIA DE ENTREGA DE BIENES");
			return leyendaFirma;
		}
		protected void PonerFirma(string htmlFirmas, bool firstElement, bool lastElement)
		{
			CreateDocument();

			//Se obtiene la página actual antes de crear el documento
			int currentPage = PageCount;
			XRRichText richText = new XRRichText();
			richText.Html = htmlFirmas;
			richText.KeepTogether = true;
			richText.LocationF = new PointF(4, UltimaPosicion + UltimoAlto + 3);
			richText.WidthF = PageSize.Width - MargenesHorizontales - 10;
			richText.Html = htmlFirmas;
			Detail4.Controls.Add(richText);
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

		protected string ObtenerFirma(Dictionary<string, string> firma1, Dictionary<string, string> firma2 = null)
		{
			string firma;
			XmlDocument xDoc = ObtenerXMLContenido(xmlFirmas);

			if (firma2 != null)
			{
				XmlNodeList textoFirma = xDoc.GetElementsByTagName("Firmas");
				if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				firma = textoFirma[0].InnerText;

				//Firma Izquierda
				firma = firma.Replace("[TITULO_IZQUIERDA]", firma1["Titulo"]);
				firma = firma.Replace("[NOMBRE_IZQUIERDA]", firma1["Nombre"]);
				firma = firma.Replace("[NOMBRE_REP_IZQUIERDA]", firma1["NombreRepresentante"]);
				firma = firma.Replace("[DIRECCION_IZQUIERDA]", firma1["Direccion"]);

				//Firma Derecha
				firma = firma.Replace("[TITULO_DERECHA]", firma2["Titulo"]);
				firma = firma.Replace("[NOMBRE_DERECHA]", firma2["Nombre"]);
				firma = firma.Replace("[NOMBRE_REP_DERECHA]", firma2["NombreRepresentante"]);
				firma = firma.Replace("[DIRECCION_DERECHA]", firma2["Direccion"]);
			}
			else
			{
				XmlNodeList textoFirma = xDoc.GetElementsByTagName("FirmasCentro");
				if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				firma = textoFirma[0].InnerText;

				firma = firma.Replace("[TITULO_CENTRO]", firma1["Titulo"]);
				firma = firma.Replace("[NOMBRE_CENTRO]", firma1["Nombre"]);
				firma = firma.Replace("[NOMBRE_REP_CENTRO]", firma1["NombreRepresentante"]);
				firma = firma.Replace("[DIRECCION_CENTRO]", firma1["Direccion"]);
			}
			return firma;
		}

        protected Dictionary<string, string> ObtenerPlatillaFirma(bool? representante = null, bool? obligado = null, bool? depositario = null)
        {
            Dictionary<string, string> plantillaFirma;
            
            if (representante.HasValue)
            {
                if (representante.Value)
                {
                    plantillaFirma= new Dictionary<string, string>();
                    plantillaFirma.Add("Titulo","CLIENTE:");
                    plantillaFirma.Add("Nombre",String.IsNullOrEmpty(nombreCliente)&& String.IsNullOrWhiteSpace(nombreCliente)?"NOMBRE_CLIENTE":nombreCliente);
                    plantillaFirma.Add("NombreRepresentante", esFisico ? string.Empty:"NOMBRE_REPRESENTANTE");
                    plantillaFirma.Add("Direccion",String.IsNullOrEmpty(direccionCliente)&& String.IsNullOrWhiteSpace(direccionCliente)?"DIRECCION_CLIENTE":direccionCliente);
                    return plantillaFirma;
                }
            }
            if (obligado.HasValue)
            {
                if (obligado.Value)
                {
                    plantillaFirma = new Dictionary<string, string>();
                    plantillaFirma.Add("Titulo", "OBLIGADO SOLIDARIO:");
                    plantillaFirma.Add("Nombre", "NOMBRE_OBLIGADO_SOLIDARIO");
                    plantillaFirma.Add("NombreRepresentante", "NOMBRE_REPRESENTANTE");
                    plantillaFirma.Add("Direccion", "DIRECCION_REPRESENTANTE");

                    return plantillaFirma;
                }
            }
            if (depositario.HasValue)
            {
                if (depositario.Value)
                {
                    plantillaFirma = new Dictionary<string, string>();
                    plantillaFirma.Add("Titulo", "DEPOSITARIO:");
                    plantillaFirma.Add("Nombre", esFisico ? "NOMBRE_CLIENTE" : "NOMBRE_REPRESENTANTE_DEPOSITARIO");
                    plantillaFirma.Add("NombreRepresentante", string.Empty);
                    
                    plantillaFirma.Add("Direccion", esFisico ?(!String.IsNullOrEmpty(direccionCliente)&&String.IsNullOrWhiteSpace(direccionCliente)?direccionCliente:"DIRECCION_CLIENTE"):"DIRECCION_REPRESENTANTE_DEPOSITARIO");

                    return plantillaFirma;
                }
            }
            return null;
	    }
		protected void ProcesaFirmas(List<Dictionary<string, string>> firmas)
		{
            int cantFirmas = firmas.Count;
			if (cantFirmas == 0) return;

			if (cantFirmas == 1)
			{
				numFirmaActual += 1;
				PonerFirma(ObtenerFirma(firmas[0]), numFirmaActual == 1, numFirmas == numFirmaActual);
				return;
			}
			bool esPar = cantFirmas % 2 == 0;
			int firmaActual = 2;
			while (firmaActual <= cantFirmas + 1)
			{
				if (esPar)
				{
					numFirmaActual += 2;
                    PonerFirma(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]), numFirmaActual == 1, numFirmas == numFirmaActual);
				}
				else
				{
					if (firmaActual > cantFirmas)
					{
						numFirmaActual += 1;
                        PonerFirma(ObtenerFirma(firmas[cantFirmas - 1]), numFirmaActual == 1, numFirmas == numFirmaActual);
					}
					else
					{
						numFirmaActual += 2;
                        PonerFirma(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]), numFirmaActual == 1, numFirmas == numFirmaActual);
					}
				}
				firmaActual += 2;
			}
		}

		#endregion SC0007

		protected void ImprimirReporte(Dictionary<string, object> data, XmlDocument xmlDocument)
		{
		    try
		    {
		        DataTable equipos = data["Unidades"] as DataTable;
		        Dictionary<string, string> datos = data["Datos"] as Dictionary<string, string>;

		        XmlNodeList textoReporte = xmlDocument.GetElementsByTagName("Contenido");
		        if (textoReporte.Count < 2) throw new Exception("El formato del archivo xml es incorrecto");
		        string textoInicial = textoReporte[0].InnerText;
		        string textoFinal = textoReporte[1].InnerText;

		        foreach (var dato in datos)
		        {
		            string anterior = "{" + dato.Key + "}";
		            textoInicial = textoInicial.Replace(anterior, dato.Value);
		            textoFinal = textoFinal.Replace(anterior, dato.Value);
		        }
		        if (datos.ContainsKey("Borrador"))
		            if (string.Compare("true", datos["Borrador"], true) == 0)
		                this.Watermark.Text = "BORRADOR";
		        xrRchTextoInicial.Html = textoInicial;
		        xrRchTextoFinal.Html = textoFinal;
		        dtlRPTUnidades.DataSource = equipos;
		        xrTCTipo.DataBindings.Add("Text", equipos, "Tipo");
		        xrTCMarca.DataBindings.Add("Text", equipos, "Marca");
		        xrTCModelo.DataBindings.Add("Text", equipos, "Modelo");
		        xrTCSerie.DataBindings.Add("Text", equipos, "Serie");

		        #region SC0007

		        if (data["TipoCliente"] == null) throw new Exception("El tipo de cliente no es válido");
		        esFisico = (bool) data["TipoCliente"];
		        if (data["SoloRepresentantes"] == null)
		            throw new Exception("La opción de solo representantes no debe estar nulo");
		        soloRepresentantes = (bool) data["SoloRepresentantes"];
                fechaContrato = datos["FechaContrato"] == null ? "FECHA_CONTRATO" : datos["FechaContrato"].ToUpper();

                unidadOperativa = datos["NombreEmpresa"] ?? "NOMBRE_EMPRESA";
		        representanteUnidad = datos["RepresentanteUnidad"] ?? "REPRESENTANTE_UNIDAD_OPERATIVA";
		        nombreCliente = datos["NombreCliente"] ?? "NOMBRE_CLIENTE";
		        direccionCliente = datos["DireccionCliente"] ?? "DIRECCION_CLIENTE";


		        Dictionary<string, List<Dictionary<string, string>>> firmas =
		            data["Firmas"] as Dictionary<string, List<Dictionary<string, string>>>;
		        List<Dictionary<string, string>> Representantes = firmas["Representantes"];
		        List<Dictionary<string, string>> Obligados = firmas["ObligadosSolidarios"];
		        List<Dictionary<string, string>> Depositarios = firmas["Depositarios"];
		        if (!soloRepresentantes.Value)
		        {
		            if (Representantes.Count == 0)
		                Representantes.Add(ObtenerPlatillaFirma(true));
		            if (Obligados.Count == 0)
		                Obligados.Add(ObtenerPlatillaFirma(false, true));
		            if (Depositarios.Count == 0)
		                Depositarios.Add(ObtenerPlatillaFirma(false, false, true));
		        }
		        else
		        {
		            if (Representantes.Count == 0)
		                Representantes.Add(ObtenerPlatillaFirma(true));
		        }

		        numFirmas = Representantes.Count + Obligados.Count + Depositarios.Count;
		        repLegales = Representantes;
		        obSolidarios = Obligados;
		        depositarios = Depositarios;

		        ProcesaFirmas(Representantes);
		        ProcesaFirmas(Obligados);
		        ProcesaFirmas(Depositarios);

		        #endregion SC0007
		    }
		    catch (Exception ex)
		    {
                throw new Exception("ConstanciaEntregaBienesRPT.ImprimirReporte: " + ex.Message);
		    }
		}

		protected XmlDocument ObtenerXMLContenido(string XMLDoc)
		{
			try
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.Load(xmlUrl + XMLDoc);

				return xDoc;
			}
			catch (Exception)
			{
				throw new Exception("El formato del archivo xml es incorrecto");
			}
		}

		#endregion Metodos

		#region Constructores

		public ConstanciaEntregaBienesRPT(Dictionary<string, object> datos, string urlXML)
		{
		    try
		    {
		        InitializeComponent();
		        xmlUrl = urlXML;
		        ImprimirReporte(datos, ObtenerXMLContenido(xml));
		    }
		    catch (Exception ex)
		    {
                throw new Exception("Error al imprimir el reporte. ConstanciaEntregaBienesRPT: " + ex.Message);
		    }
		}

		#endregion Constructores
	}
}