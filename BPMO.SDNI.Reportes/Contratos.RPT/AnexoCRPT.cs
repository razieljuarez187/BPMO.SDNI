//Esta clase satisface los requerimientos del CU021 - Imprimir Anexo C Acta de nacimiento
using System;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace BPMO.SDNI.Contratos.RPT
{
    public partial class AnexoCRPT : DevExpress.XtraReports.UI.XtraReport
    {
        const string report = "BPMO.SDNI.Reportes.Anexo.C.xml";
        public AnexoCRPT(Dictionary<string, object> datos, string url)
        {
            try
            {
                InitializeComponent();
                string path = url + report;
                if (File.Exists(path))
                {
                    Dictionary<string, object> data = datos;
                    string titulo = string.Empty; string subtitulo = string.Empty; string transG = string.Empty; string llantasg = string.Empty;
                    XmlDocument xDoc = new XmlDocument();
				    xDoc.Load(path);                   
                    XmlNodeList xmlnode = xDoc.GetElementsByTagName("titulo");
                    if(xmlnode != null)
                        if(xmlnode.Count > 0)
			                titulo = xmlnode[0].InnerText;
                    xmlnode = xDoc.GetElementsByTagName("subtitulo");
                    if(xmlnode != null)
                        if(xmlnode.Count > 0)
                            subtitulo = xmlnode[0].InnerText;
                    xmlnode = xDoc.GetElementsByTagName("transmision");
                    if(xmlnode != null)
                        if(xmlnode.Count > 0)
                            transG = xmlnode[0].InnerText;
                    xmlnode = xDoc.GetElementsByTagName("llantas");
                    if(xmlnode != null)
                        if(xmlnode.Count > 0)
                            llantasg = xmlnode[0].InnerText;

                    data["Titulo"] = titulo;
                    data["Subtitulo"] = subtitulo;
                    data["TransmisionG"] = transG;
                    data["LlantasG"] = llantasg;

                    this.CargarDatos(data);
                }
                else
                    throw new Exception(@"La plantilla correspondiente al reporte ""ANEXO C"" no se encuentra disponible", new Exception("EL archivo BPMO.SDNI.Reportes.Anexo.C.xml no se encuentra en la ubicación necesaria."));
            }
            catch { throw; }
        }

        public void CargarDatos(Dictionary<string, object> datos)
        {
            DataSet data = (DataSet)datos["UnidadesContrato"];
            
            this.DataSource = data;
            #region cabecera
            this.lblReportTitle.Text = (string)Convert.ChangeType(datos["Titulo"], typeof(string));
            this.lblReportSubTitle.Text = (string)Convert.ChangeType(datos["Subtitulo"], typeof(string));
            #endregion
            #region Asociacion de datos al source
            this.lblAnio.DataBindings.Add(new XRBinding("Text", null, "Anio"));
            this.lblCliente.DataBindings.Add(new XRBinding("Text", null, "Cliente"));
            this.lblDate.DataBindings.Add(new XRBinding("Text", null, "Fecha"));
            this.lblKilometros.DataBindings.Add(new XRBinding("Text", null, "Kilometros"));
            this.lblMDireccion.DataBindings.Add(new XRBinding("Text", null, "MDireccion"));
            this.lblMod1.DataBindings.Add(new XRBinding("Text", null, "Mod1"));
            this.lblMod10.DataBindings.Add(new XRBinding("Text", null, "Mod10"));
            this.lblMod2.DataBindings.Add(new XRBinding("Text", null, "Mod2"));
            this.lblMod3.DataBindings.Add(new XRBinding("Text", null, "Mod3"));
            this.lblMod4.DataBindings.Add(new XRBinding("Text", null, "Mod4"));
            this.lblMod5.DataBindings.Add(new XRBinding("Text", null, "Mod5"));
            this.lblMod6.DataBindings.Add(new XRBinding("Text", null, "Mod6"));
            this.lblMod7.DataBindings.Add(new XRBinding("Text", null, "Mod7"));
            this.lblMod8.DataBindings.Add(new XRBinding("Text", null, "Mod8"));
            this.lblMod9.DataBindings.Add(new XRBinding("Text", null, "Mod9"));
            this.lblModelo.DataBindings.Add(new XRBinding("Text", null, "Modelo"));
            this.lblModeloAllison.DataBindings.Add(new XRBinding("Text", null, "ModeloAllison"));
            this.lblModeloEjeTD.DataBindings.Add(new XRBinding("Text", null, "ModeloEjeTD"));
            this.lblModeloEjeTT.DataBindings.Add(new XRBinding("Text", null, "ModeloEjeTT"));
            this.lblModRef.DataBindings.Add(new XRBinding("Text", null, "ModRefaccion"));
            this.lblNumEconomico.DataBindings.Add(new XRBinding("Text", null, "NumEconomico"));
            this.lblNumeroSerie.DataBindings.Add(new XRBinding("Text", null, "VIN"));
            this.lblPlacas.DataBindings.Add(new XRBinding("Text", null, "Placas"));
            this.lblPostEnfriador.DataBindings.Add(new XRBinding("Text", null, "PostEnfriador"));
            this.lblProf1.DataBindings.Add(new XRBinding("Text", null, "Prof1"));
            this.lblProf10.DataBindings.Add(new XRBinding("Text", null, "Prof10"));
            this.lblProf2.DataBindings.Add(new XRBinding("Text", null, "Prof2"));
            this.lblProf3.DataBindings.Add(new XRBinding("Text", null, "Prof3"));
            this.lblProf4.DataBindings.Add(new XRBinding("Text", null, "Prof4"));
            this.lblProf5.DataBindings.Add(new XRBinding("Text", null, "Prof5"));
            this.lblProf6.DataBindings.Add(new XRBinding("Text", null, "Prof6"));
            this.lblProf7.DataBindings.Add(new XRBinding("Text", null, "Prof7"));
            this.lblProf8.DataBindings.Add(new XRBinding("Text", null, "Prof8"));
            this.lblProf9.DataBindings.Add(new XRBinding("Text", null, "Prof9"));
            this.lblProfRef.DataBindings.Add(new XRBinding("Text", null, "ProfRefaccion"));
            this.lblRadiador.DataBindings.Add(new XRBinding("Text", null, "Radiador"));
            this.lblSASElectrico.DataBindings.Add(new XRBinding("Text", null, "SASElectrico"));
            this.lblSBSElectrico.DataBindings.Add(new XRBinding("Text", null, "SBSElectrico"));
            this.lblSeieAllison.DataBindings.Add(new XRBinding("Text", null, "SerieAllison"));            
            this.lblSerie1 .DataBindings.Add(new XRBinding("Text", null, "Serie1"));
            this.lblSerie10.DataBindings.Add(new XRBinding("Text", null, "Serie10"));
            this.lblSerie2.DataBindings.Add(new XRBinding("Text", null, "Serie2"));
            this.lblSerie3.DataBindings.Add(new XRBinding("Text", null, "Serie3"));
            this.lblSerie4.DataBindings.Add(new XRBinding("Text", null, "Serie4"));
            this.lblSerie5.DataBindings.Add(new XRBinding("Text", null, "Serie5"));
            this.lblSerie6.DataBindings.Add(new XRBinding("Text", null, "Serie6"));
            this.lblSerie7.DataBindings.Add(new XRBinding("Text", null, "Serie7"));
            this.lblSerie8.DataBindings.Add(new XRBinding("Text", null, "Serie8"));
            this.lblSerie9.DataBindings.Add(new XRBinding("Text", null, "Serie9"));
            this.lblSerieCompresor.DataBindings.Add(new XRBinding("Text", null, "SerieCompresor"));
            this.lblSerieECM.DataBindings.Add(new XRBinding("Text", null, "SerieECM"));
            this.lblSerieEDireccion.DataBindings.Add(new XRBinding("Text", null, "SerieEDireccion"));
            this.lblSerieEjeTD.DataBindings.Add(new XRBinding("Text", null, "SerieEjeTD"));
            this.lblSerieEjeTT.DataBindings.Add(new XRBinding("Text", null, "SerieEjeTT"));
            this.lblSerieRef.DataBindings.Add(new XRBinding("Text", null, "SerieRefaccion"));
            #region SC0030
            this.lblSerieMotor.DataBindings.Add(new XRBinding("Text", null, "SerieMotor"));
            #endregion
            this.lblSerieTCargador.DataBindings.Add(new XRBinding("Text", null, "SerieTCargador"));
            this.lblSMSElectrico.DataBindings.Add(new XRBinding("Text", null, "SMSElectrico")); 
            #endregion
            #region Datos estaticos de transmision y llantas
            this.lblNombreTransmisionG.Text = (string)Convert.ChangeType(datos["TransmisionG"], typeof(string));
            this.lblModeloLlantasG.Text = (string)Convert.ChangeType(datos["LlantasG"], typeof(string));
            #endregion
        }
            
        /// <summary>
        /// Subreporte
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">PrintEventArgs</param>
        private void xrSubreport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            try {

                var report = new AnexoCRPTDetalle();
                
                var unidadID = GetCurrentColumnValue<string>("VIN");          
                
                //aqui se parsea
                DataSet data = (DataSet)this.DataSource;

                
                report.DataSource = data.Tables["NumerosSerie"];

                 report.FilterString= String.Format("[VINUnidad] = '{0}'", unidadID);

                 this.xrSubreport.ReportSource = report;
                                        
                               
            }
            catch {
                this.xrSubreport.ReportSource = null;
            }

            

        }
           
    }
}
