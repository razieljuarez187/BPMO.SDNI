//Satisface al caso de uso CU094 - Imprimir Pagaré Contrato RD

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.RD.RPT
{
	public partial class PagareRPT : DevExpress.XtraReports.UI.XtraReport
	{
		#region Atributos
		private float UltimoAlto;
		private const string xmlFirmas = "BPMO.SDNI.Reportes.Contrato.RD.Firmas.xml";
		private const string xmlPagare = "BPMO.SDNI.Reportes.Contrato.RD.Pagare.xml";
		private readonly Dictionary<string, object> datosPagare;
		private readonly Dictionary<string, object> firmantes;
		private readonly string urlXml;
		#endregion

		#region Propiedades
		private float MargenesHorizontales
		{
			get { return this.Margins.Left + this.Margins.Right + 1; }
		}

		private int CantidadControles
		{
			get
			{
				return this.DetalleFirmas.Controls.Count;
			}
		}

		private float UltimaPosicion
		{
			get
			{
				return CantidadControles == 0 ? 0 : this.DetalleFirmas.Controls[CantidadControles - 1].LocationF.Y;
			}
		}
		#endregion

		#region Constructores
		public PagareRPT()
		{
			InitializeComponent();
		}

		public PagareRPT(Dictionary<string, object> datosReporte, string directorioReportes)
		{
			try
			{
				InitializeComponent();
				if (datosReporte == null) throw new Exception("No se proporcionaron datos para el pagaré.");
				if (datosReporte.ContainsKey("DatosPagare"))
					this.datosPagare = datosReporte["DatosPagare"] as Dictionary<string, object>;
				else throw new Exception("No se proporcionaron datos para el pagaré.");
				if (datosReporte.ContainsKey("Firmas"))
					firmantes = datosReporte["Firmas"] as Dictionary<string, object>;
				else
					throw new Exception("No se proporcionaron datos para la sección de firmas.");

				if (string.IsNullOrEmpty(directorioReportes) || string.IsNullOrWhiteSpace(directorioReportes))
					throw new Exception("Es necesario especificar la url de la plantilla para el pagaré.");
				urlXml = directorioReportes;
				ImprimirPagare();
			}
			catch (Exception ex)
			{
				throw new Exception("PagareRPT():" + ex.Message);
			}
		}
		#endregion

		#region Metodos
		public void ImprimirPagare()
		{
			StringBuilder textoPagare;
			XmlDocument xDoc = ObtenerXMLDocumento(xmlPagare);
			decimal cantidad = 0;

			if (datosPagare.ContainsKey("Cantidad"))
				cantidad = (decimal) datosPagare["Cantidad"];
			cantidad = Math.Round(cantidad, 2);
			string sMoneda = "";
			if (datosPagare.ContainsKey("CodigoMoneda"))
				sMoneda = datosPagare["CodigoMoneda"] as string;
			string sCantidad = string.Format("$ {0:#,##0.00}", cantidad);
			string sCantidadLetras = enletras(cantidad, sMoneda );
			#region titulo


			XmlNodeList textoTitulo = xDoc.GetElementsByTagName("Titulo");
			if (textoTitulo.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");
			textoPagare = new StringBuilder(textoTitulo[0].InnerText);
			lblTitulo.Html = textoPagare.ToString();

			#endregion
			#region BuenoPor
			XmlNodeList textoBuenoPor = xDoc.GetElementsByTagName("BuenoPor");
			if (textoBuenoPor.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");
			textoPagare = new StringBuilder(textoBuenoPor[0].InnerText);
			textoPagare.Replace("[CANTIDAD]", sCantidad.ToUpper());
			textoPagare.Replace("[CODIGO_MONEDA]", sMoneda.ToUpper());
			lblBuenoPor.Html = textoPagare.ToString();
			#endregion
			#region Contenido
			XmlNodeList textoContenido = xDoc.GetElementsByTagName("Contenido");
			if (textoContenido.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");
			textoPagare = new StringBuilder(textoContenido[0].InnerText);
			textoPagare.Replace("[CANTIDAD]", sCantidad.ToUpper());
			textoPagare.Replace("[CODIGO_MONEDA]", sMoneda.ToUpper());
			textoPagare.Replace("[CANTIDAD_LETRAS]", sCantidadLetras.ToUpper());
			if (datosPagare.ContainsKey("UnidadOperativa"))
				textoPagare.Replace("[UNIDAD_OPERATIVA]", datosPagare["UnidadOperativa"].ToString().ToUpper());
			if (datosPagare.ContainsKey("FechaFin"))
			{
				DateTime fechaFin = (DateTime)datosPagare["FechaFin"];

				textoPagare.Replace("[FECHA_FIN]",string.Format("{0: d DE MMMM DE yyyy}",fechaFin).ToUpper());
			}
			if (datosPagare.ContainsKey("FechaInicio"))
			{
				DateTime fechaInicio = (DateTime) datosPagare["FechaInicio"];
				textoPagare.Replace("[FECHA_INICIO]",string.Format("{0: d DE MMMM DE yyyy}",fechaInicio).ToUpper());
				
			}
			lblPagare.Html = textoPagare.ToString();
			#endregion
			#region Firmas
			if (firmantes.ContainsKey("Suscriptores"))
				ProcesaFirmas(firmantes["Suscriptores"] as List<Dictionary<string, string>>);
			if (firmantes.ContainsKey("Avales"))
				ProcesaFirmas(firmantes["Avales"] as List<Dictionary<string, string>>);
			#endregion
		}

		public void ProcesaFirmas(List<Dictionary<string, string>> firmas)
		{
			if (firmas.Count == 0) return;
			int cantFirmas = firmas.Count, firmaActual = 0;

			while (firmaActual < cantFirmas)
			{
				firmaActual += 2;

				if (firmaActual <= cantFirmas)
					EstablecerFirmas(ObtenerFirma(firmas[firmaActual - 2], firmas[firmaActual - 1]));
				else EstablecerFirmas(ObtenerFirma(firmas[firmaActual - 2]));

			}

		}

		protected XmlDocument ObtenerXMLDocumento(string xmlFile)
		{
			try
			{
				string path = urlXml + xmlFile;
				if (File.Exists(path) != true)
					throw new Exception("La plantilla correspondiente no se encuentra disponible", new Exception("El archivo " + xmlFile + " no se encuentra en la ubicación necesaria."));
				else
				{
					XmlDocument xDoc = new XmlDocument();
					xDoc.Load(path);
					return xDoc;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("El formato del archivo xml es incorrecto");
			}
		}

		public string ObtenerFirma(Dictionary<string, string> firma1, Dictionary<string, string> firma2 = null)
		{
			string firma;
			XmlDocument xDoc = ObtenerXMLDocumento(xmlFirmas);

			if (firma2 != null)
			{
				XmlNodeList textoFirma = xDoc.GetElementsByTagName("Firmas");
				if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				firma = textoFirma[0].InnerText;

				//Firma Izquierda
				firma = firma.Replace("[TITULO_IZQUIERDA]", firma1["Titulo"].ToUpper());
				firma = firma.Replace("[NOMBRE_IZQUIERDA]", firma1["Nombre"].ToUpper());
				firma = firma.Replace("[NOMBRE_REP_IZQUIERDA]", firma1["NombreRepresentante"].ToUpper());
				firma = firma.Replace("[DIRECCION_IZQUIERDA]", firma1["Direccion"].ToUpper());

				//Firma Derecha
				firma = firma.Replace("[TITULO_DERECHA]", firma2["Titulo"].ToUpper());
				firma = firma.Replace("[NOMBRE_DERECHA]", firma2["Nombre"].ToUpper());
				firma = firma.Replace("[NOMBRE_REP_DERECHA]", firma2["NombreRepresentante"].ToUpper());
				firma = firma.Replace("[DIRECCION_DERECHA]", firma2["Direccion"].ToUpper());
			}
			else
			{
				XmlNodeList textoFirma = xDoc.GetElementsByTagName("FirmasCentro");
				if (textoFirma.Count < 1) throw new Exception("El formato del archivo xml es incorrecto");

				firma = textoFirma[0].InnerText;

				firma = firma.Replace("[TITULO_CENTRO]", firma1["Titulo"].ToUpper());
				firma = firma.Replace("[NOMBRE_CENTRO]", firma1["Nombre"].ToUpper());
				firma = firma.Replace("[NOMBRE_REP_CENTRO]", firma1["NombreRepresentante"].ToUpper());
				firma = firma.Replace("[DIRECCION_CENTRO]", firma1["Direccion"].ToUpper());
			}
			return firma.Replace("[Leyenda]", string.Empty);
		}

		public void EstablecerFirmas(string htmlFirmas)
		{
			CreateDocument();
			//Se obtiene la página actual antes de crear el documento
			XRRichText richText = new XRRichText();
			richText.Html = htmlFirmas;
			richText.KeepTogether = true;
			richText.LocationF = new PointF(4, UltimaPosicion + UltimoAlto + 3);
			richText.WidthF = PageSize.Width - MargenesHorizontales - 10;
			richText.Html = htmlFirmas;
			this.DetalleFirmas.Controls.Add(richText);
			UltimoAlto = richText.HeightF;

			//Una vez agregado el control se vuelve a generar para saber donde quedó
			CreateDocument();


		}

		#region Letras a Numeros
		public string enletras(decimal num, string moneda)
		{
			string res, dec = "";
			Int64 entero;
			int decimales;


			entero = Convert.ToInt64(Math.Truncate(num));
			decimales = Convert.ToInt32(Math.Round((num - entero) * 100, 2));
			if (decimales > 0)
			{
				dec = " " + decimales.ToString() + "/100";
			}
			else
			{
				dec = " 00/100";
			}
			res = ATexto(entero) + " " + moneda + " " + dec;
			return res;
		}

		private static string ATexto(decimal value)
		{

			string Num2Text = "";

			value = Math.Truncate(value);
			if (value == 0) Num2Text = "CERO";
			else if (value == 1) Num2Text = "UN";
			else if (value == 2) Num2Text = "DOS";
			else if (value == 3) Num2Text = "TRES";
			else if (value == 4) Num2Text = "CUATRO";
			else if (value == 5) Num2Text = "CINCO";
			else if (value == 6) Num2Text = "SEIS";
			else if (value == 7) Num2Text = "SIETE";
			else if (value == 8) Num2Text = "OCHO";
			else if (value == 9) Num2Text = "NUEVE";
			else if (value == 10) Num2Text = "DIEZ";
			else if (value == 11) Num2Text = "ONCE";
			else if (value == 12) Num2Text = "DOCE";
			else if (value == 13) Num2Text = "TRECE";
			else if (value == 14) Num2Text = "CATORCE";
			else if (value == 15) Num2Text = "QUINCE";
			else if (value < 20) Num2Text = "DIECI" + ATexto(value - 10);
			else if (value == 20) Num2Text = "VEINTE";
			else if (value < 30)
			{
				string segundaCifra;
				if ((value % 20).Equals(1)) segundaCifra = "UN";
				else segundaCifra = ATexto(value % 20);
				Num2Text = "VEINTI" + segundaCifra;
			}
			else if (value == 30) Num2Text = "TREINTA";
			else if (value == 40) Num2Text = "CUARENTA";
			else if (value == 50) Num2Text = "CINCUENTA";
			else if (value == 60) Num2Text = "SESENTA";
			else if (value == 70) Num2Text = "SETENTA";
			else if (value == 80) Num2Text = "OCHENTA";
			else if (value == 90) Num2Text = "NOVENTA";
			else if (value < 100)
			{
				string segundaCifra;

				if ((value % 10).Equals(1))
					segundaCifra = "UN";
				else segundaCifra = ATexto(value % 10);
				Num2Text = ATexto(Math.Truncate(value / 10) * 10) + " Y " + segundaCifra;
			}

			else if (value == 100) Num2Text = "CIEN";
			else if (value < 200) Num2Text = "CIENTO " + ATexto(value - 100);
			else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = ATexto(Math.Truncate(value / 100)) + "CIENTOS";
			else if (value == 500) Num2Text = "QUINIENTOS";
			else if (value == 700) Num2Text = "SETECIENTOS";
			else if (value == 900) Num2Text = "NOVECIENTOS";
			else if (value < 1000) Num2Text = ATexto(Math.Truncate(value / 100) * 100) + " " + ATexto(value % 100);
			else if (value == 1000) Num2Text = "UN MIL";
			else if (value < 2000) Num2Text = "UN MIL " + ATexto(value % 1000);
            else if (value < 1000000) Num2Text = ATexto(Math.Truncate(value / 1000)) + " MIL " + (((value % 1000) > 0) ? ATexto(value % 1000) : string.Empty);
				else if (value == 1000000) Num2Text = "UN MILLON";
				else if (value < 2000000) Num2Text = "UN MILLON " + ATexto(value % 1000000);
				else if (value < 1000000000000)
				{
					Num2Text = ATexto(Math.Truncate(value / 1000000)) + " MILLONES ";
					if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + ATexto(value - Math.Truncate(value / 1000000) * 1000000);
				}
				else if (value == 1000000000000) Num2Text = "UN BILLON";
				else if (value < 2000000000000) Num2Text = "UN BILLON " + ATexto(value - Math.Truncate(value / 1000000000000) * 1000000000000);
				else
				{
					Num2Text = ATexto(Math.Truncate(value / 1000000000000)) + " BILLONES";

					if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + ATexto(value - Math.Truncate(value / 1000000000000) * 1000000000000);
				}
			return Num2Text;

		}

		#endregion
		#endregion
	}
}