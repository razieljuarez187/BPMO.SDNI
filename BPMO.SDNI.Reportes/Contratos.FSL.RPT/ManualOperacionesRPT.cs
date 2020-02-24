//Satisface al Caso de uso CU017 : Imprimir Manual de Operaciones
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Text;

namespace BPMO.SDNI.Contratos.FSL.RPT
{
	public partial class ManualOperacionesRPT : DevExpress.XtraReports.UI.XtraReport
	{
		
		#region Atributos
		private string xml = "BPMO.SDNI.Reportes.Manual.Operaciones.xml";
		#endregion
		#region Constructores
		public ManualOperacionesRPT(Dictionary<string, object> datos, string urlXML)
		{
			InitializeComponent();

			#region Consulta XML Partes Estaticas
			if (((bool)datos["Estatus"]))
				this.Watermark.Text = "BORRADOR";

			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(urlXML + xml);

			XmlNodeList xmlReporte = xDoc.GetElementsByTagName("Reporte");

			XmlNodeList xmlIndice = xDoc.GetElementsByTagName("Indice");
			XmlNodeList xmlTitulo = xDoc.GetElementsByTagName("Titulo");
			XmlNodeList xmlSubTitulo = xDoc.GetElementsByTagName("SubTitulo");
			xrRchTitulo.Html = xmlTitulo[0].InnerText;

			xrRchSubtitulo.Html = xmlSubTitulo[0].InnerText;
			string sindex = xmlIndice[0].InnerText.Replace("{NombreCliente}", datos["NombreCliente"] as string);
			xrRchIndice.Html = sindex;
			string nombreCliente=datos["NombreCliente"] as string;
			xrRchCliente.Html = nombreCliente + " - IDEALEASE";
			XmlNodeList contenidoManual = ((XmlElement)xmlReporte[0]).GetElementsByTagName("ContenidoManual");
			xrPicLogo.ImageUrl = datos["Logo"] as string;
			xrPicLogo.Visible = true;
			xrPicture.ImageUrl = datos["Logo"] as string;
			xrPicture.Visible = true;
			DataTable dtDatos = new DataTable("ContenidoManual");
			dtDatos.Columns.Add("Contenido");

			foreach (XmlElement xmlElement in contenidoManual)
			{
				DataRow row = dtDatos.NewRow();
				StringBuilder html = new StringBuilder(xmlElement.InnerText);
				html.Replace("{NombreCliente}", datos["NombreCliente"] as string);
				html.Replace("{TelefonoEmergencia}", datos["TelefonoEmergencia"] as string);
				row["Contenido"] = html.ToString();
				dtDatos.Rows.Add(row);
			}

			xrRchDetalleManual.DataBindings.Add("Html", dtDatos, "Contenido");

			#endregion Consulta XML Partes Estaticas

			DataSource = dtDatos;
		}
		#endregion
		
	}
}