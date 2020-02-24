// Satisface al CU016 : Imprimir Constancia de Entrega de Bienes
//Satisface al CU017 : Imprimir Manual de Operaciones
//Satisface al CU018 : Imprimir Contrato Maestro
//Satisface al CU019 : Imprimir Anexo A
//Satisface al CU021 : Imprimir Anexo C Acta de Nacimiento
//Satisface al CU019 - Reporte de Flota Activa de RD Registrados
//Satisface el CU022 – Reporte Contratos de Servicio Dedicado Registrad

using System;
using System.Collections.Generic;
using System.Configuration;
using BPMO.Primitivos.Enumeradores;
using BPMO.SDNI.Buscador.PRE;
using BPMO.SDNI.Buscador.VIS;
using BPMO.SDNI.MapaSitio.UI;
using DevExpress.XtraReports.UI;
using System.Linq;
using DevExpress.XtraReports.Web;

namespace BPMO.SDNI.Buscador.UI
{
	public partial class VisorReporteUI : System.Web.UI.Page, IVisorReporteVIS
	{
		#region Atributos

		private VisorReportePRE presentador;

		#endregion Atributos

		#region Propiedades
		public Dictionary<string, object> DatosReporte {
			get {
				if (!string.IsNullOrWhiteSpace(this.NombreReporteQuery))
                    return Session["rpt." + this.NombreReporteQuery] as Dictionary<string, object>;
                if (Session["DatosReporte"] != null)
					return Session["DatosReporte"] as Dictionary<string, object>;

				return null;
			}
		}
		public string NombreReporte {
			get {
                if (!string.IsNullOrWhiteSpace(this.NombreReporteQuery))
                    return this.NombreReporteQuery;
                else if (Session["NombreReporte"] != null)
					return Session["NombreReporte"] as string;
				return null;
			}
		}
        /// <summary>
        /// Nombre de reporte que se envía como parámetro en la URL
        /// (para no sobrescribir datos de sesión)
        /// </summary>
        private string NombreReporteQuery {
            get {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["rpt"]))
                    return Request.QueryString["rpt"];
                else
                    return null;
            }
        }
		#endregion Propiedades

		#region Metodos

		public string MapRuta(string ruta)
		{
			return MapPath(ruta);
		}

        /// <summary>
        /// Obtene la ruta del directorio donde se encuentran los XML de los reportes
        /// </summary>
        /// <returns>Ruta de </returns>
	    public string ObtenerDirectorioResportes()
        {
            return ConfigurationManager.AppSettings["UbicacionReportesXML"];
        }

	    /// <summary>
		/// Este método despliega un mensaje en pantalla
		/// </summary>
		/// <param name="mensaje">Mensaje a desplegar</param>
		/// <param name="tipo">Tipo de mensaje a desplegar</param>
		/// <param name="msjDetalle">Muestra una cadena de texto que contiene los detalles del mensaje a desplegar</param>
		public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle = null)
		{
			Site masterMsj = (Site)Page.Master;
			if (tipo == ETipoMensajeIU.ERROR)
				masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
			else
				masterMsj.MostrarMensaje(mensaje, tipo);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			presentador = new VisorReportePRE(this);

			rptViewer.Report = (XtraReport)presentador.ObtenerReporte();
		}

        /// <summary>
        /// Establece los formatos a los que el visor de reporte tendrá permitidos mandar a exportar
        /// </summary>
        /// <param name="saveFormats">Lista de formatos a los que podrá mandar a exportar</param>
        public void EstablecerFormatosAExportar(params String[] saveFormats)
        {
            if (saveFormats == null)
                return;

            ReportToolbarComboBox reportItem = this.rptToolBar.Items.OfType<ReportToolbarComboBox>()
                                                .Where(x => x.ItemKind == ReportToolbarItemKind.SaveFormat)
                                                .FirstOrDefault();

            if (reportItem != null)
            {
                reportItem.Elements.Clear();
                foreach (String value in saveFormats)
                {
                    ListElement element = new ListElement();
                    element.Value = value;
                    reportItem.Elements.Add(element);
                }
            }
        }
		#endregion Metodos

		#region Constructores

		#endregion Constructores
	}
}