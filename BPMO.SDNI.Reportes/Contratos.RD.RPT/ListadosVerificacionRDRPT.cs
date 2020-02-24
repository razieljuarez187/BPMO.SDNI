// Satisface al CU012 - Imprimir Check List de Entrega Recepción de Unidad
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.BO;
using BPMO.SDNI.Contratos.RD.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Tramites.BO;
using DevExpress.XtraReports.UI;

namespace BPMO.SDNI.Contratos.RD.RPT
{
    public partial class ListadosVerificacionRDRPT : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Nombre del archivo de plantilla
        /// </summary>
        private const string xml = "BPMO.SDNI.Reportes.Check.List.xml";
        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRDRPT";
        /// <summary>
        /// Constructor del reporte
        /// </summary>
        /// <param name="datos">Datos del check list</param>
        /// <param name="urlXML">Dirección donde se guarda el xml de la plantilal del reporte</param>
        public ListadosVerificacionRDRPT(Dictionary<string, Object> datos, string urlXML)
        {
            InitializeComponent();

            if (datos != null)
                datos["Path"] = urlXML + xml;

            this.ImprimirReporte(datos, this.ObtenerXMLContenido(urlXML));
            
        }
        /// <summary>
        /// Genera el reporte para el check list
        /// </summary>
        /// <param name="datos">Datos del check list</param>
        /// <param name="xmlDocument">Plantilla del reporte</param>
        private void ImprimirReporte(Dictionary<string, object> datos, XmlDocument xmlDocument)
        {
            try
            {                
                #region Cabecera
                #region Título
                XmlNodeList textoCondiciones = xmlDocument.GetElementsByTagName("titulo");
                if (textoCondiciones.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                this.lblTitulo.Html = textoCondiciones[0].InnerText;
                #endregion

                #region Logo
                var modulo = (ModuloBO)datos["Modulo"];
                int? unidadOperativaID = (int)datos["UnidadOperativaID"];
                if (unidadOperativaID != null && modulo.ObtenerConfiguracion(unidadOperativaID.Value) != null)
                {
                    ConfiguracionModuloBO configuracion = modulo.ObtenerConfiguracion(unidadOperativaID.Value);
                    if (!string.IsNullOrEmpty(configuracion.URLLogoEmpresa) && !string.IsNullOrWhiteSpace(configuracion.URLLogoEmpresa))
                        this.picLogoEmpresa.ImageUrl = configuracion.URLLogoEmpresa;
                }

                #endregion

                #region Folio
                string folio = (string) datos["Folio"];
                this.lblFolioCheckList.Text = !string.IsNullOrEmpty(folio) && !string.IsNullOrWhiteSpace(folio) ? folio : string.Empty;
                this.lblFolioCabecera.Text = !string.IsNullOrEmpty(folio) && !string.IsNullOrWhiteSpace(folio) ? folio : string.Empty;
                #endregion

                #region Unidad
                var unidad = (UnidadBO)datos["Unidad"];
                this.lblModeloUnidad.Text = unidad.Modelo.Nombre;
                this.lblNumeroEconomico.Text = unidad.NumeroEconomico;
                #region Tramites
                var placas = (TramiteBO) datos["Placas"];
                this.lblPlacas.Text = placas.Resultado;
                #endregion                
                #endregion

                #region Estatus Contrato
                EEstatusContrato? estatus = null;
                if (datos.ContainsKey("EstatusContrato") && datos["EstatusContrato"] != null)
                    estatus = (EEstatusContrato)datos["EstatusContrato"];

                switch (estatus)
                {
                    case EEstatusContrato.Cancelado:
                    case EEstatusContrato.Cerrado:
                        this.Watermark.Text = estatus.ToString().ToUpper();
                        break;
                }
                #endregion
                #endregion

                #region Cliente
                var cliente = (CuentaClienteIdealeaseBO) datos["Cliente"];
                this.lblNombreClienteEntrega.Text = cliente.Nombre;                
                #endregion

                #region Datos Entrega
                var entrega = (ListadoVerificacionBO) datos["Entrega"];

                #region Usuario
                this.lblNombreUsuarioEntrega.Text = entrega.UsuarioVerifica.Nombre;
                #endregion

                #region Fecha y hora
                this.lblFechaEntrega.Text = entrega.Fecha.HasValue ? entrega.Fecha.Value.ToShortDateString() : string.Empty;
                this.lblHoraEntrega.Text = entrega.Fecha.HasValue ? entrega.Fecha.Value.ToString("hh:mm tt").Replace(".","").ToUpper() : string.Empty;
                #endregion

                #region Operador
                var operador = (OperadorBO) datos["Operador"];
                this.lblNombreOperadorEntrega.Text = operador.Nombre;                
                #endregion

                #region Kilometros Horas

                this.lblKilometrajeEntrega.Text = entrega.Kilometraje.HasValue
                                                      ? entrega.Kilometraje.Value.ToString("#,##0")
                                                      : string.Empty;
                this.lblHorasEntrega.Text = entrega.Horometro.HasValue
                                                ? entrega.Horometro.Value.ToString("#,##0")
                                                : string.Empty;

                #endregion

                #region Cuestionario

                #region Espejos Completos
                if (entrega.TieneEspejosCompletos.HasValue)
                {
                    if (entrega.TieneEspejosCompletos.Value)
                        chkEntregaEspejosSI.Checked = true;
                    else
                        chkEntregaEspejosNO.Checked = true;
                }
                #endregion

                #region Contrato de Renta
                if (entrega.TieneContratoRenta.HasValue)
                {
                    if (entrega.TieneContratoRenta.Value)
                        this.chkEntregaContratoSI.Checked = true;
                    else
                        this.chkEntregaContratoNO.Checked = false;
                }
                #endregion

                #region Interior Limpio
                if (entrega.TieneInteriorLimpio.HasValue)
                {
                    if (entrega.TieneInteriorLimpio.Value)
                        this.chkEntregaInteriorSI.Checked = true;
                    else
                        this.chkEntregaInteriorNO.Checked = true;
                }
                #endregion

                #region Vestiduras Limpias
                if (entrega.TieneVestidurasLimpias.HasValue)
                {
                    if (entrega.TieneVestidurasLimpias.Value)
                        this.chkEntregaVestidurasSI.Checked = true;
                    else
                        this.chkEntregaVestidurasNO.Checked = true;
                }
                #endregion

                #region Tapetes
                if (entrega.TieneTapetes.HasValue)
                {
                    if (entrega.TieneTapetes.Value)
                        this.chkEntregaTapetesSI.Checked = true;
                    else 
                        this.chkEntregaTapetesNO.Checked = true;
                }
                #endregion

                #region Llave Original
                if (entrega.TieneLlaveOriginal.HasValue)
                {
                    if (entrega.TieneLlaveOriginal.Value)
                        this.chkEntregaLlaveSI.Checked = true;
                    else
                        this.chkEntregaLlaveNO.Checked = true;
                }
                #endregion

                #region Encendedor
                if (entrega.TieneEncendedor.HasValue)
                {
                    if (entrega.TieneEncendedor.Value)
                        this.chkEntregaEncendedorSI.Checked = true;
                    else
                        this.chkEntregaEncendedorNO.Checked = true;
                }
                #endregion

                #region Estereo y Bocinas
                if (entrega.TieneStereoBocinas.HasValue)
                {
                    if (entrega.TieneStereoBocinas.Value)
                        this.chkEntregaEstereoSI.Checked = true;
                    else this.chkEntregaEstereoNO.Checked = true;
                }
                #endregion

                #region Alarma de reversa
                if (entrega.TieneAlarmasReversa.HasValue)
                {
                    if (entrega.TieneAlarmasReversa.Value)
                        this.chkEntregaAlarmaSI.Checked = true;
                    else
                        this.chkEntregaAlarmaNO.Checked = true;
                }
                #endregion

                #region Extinguidor
                if (entrega.TieneExtinguidor.HasValue)
                {
                    if (entrega.TieneExtinguidor.Value)
                        this.chkEntregaExtinguidorSI.Checked = true;
                    else this.chkEntregaExtinguidorNO.Checked = true;
                }
                #endregion

                #region Gato y Llave
                if (entrega.TieneGatoLlaveTuerca.HasValue)
                {
                    if (entrega.TieneGatoLlaveTuerca.Value)
                        this.chkEntregaGatoSI.Checked = true;
                    else this.chkEntregaGatoNO.Checked = true;
                }
                #endregion

                #region 3 Reflejantes
                if (entrega.TieneTresReflejantes.HasValue)
                {
                    if (entrega.TieneTresReflejantes.Value)
                        this.chkEntregaReflejantesSI.Checked = true;
                    else this.chkEntregaReflejantesNO.Checked = true;
                }
                #endregion

                #region Limpieza Int. Caja
                if (entrega.TieneLimpiezaInteriorCaja.HasValue)
                {
                    if (entrega.TieneLimpiezaInteriorCaja.Value)
                        this.chkEntregaCajaSI.Checked = true;
                    else this.chkEntregaCajaNO.Checked = true;
                }
                #endregion

                #region GPS
                if (entrega.TieneGPS.HasValue)
                {
                    if (entrega.TieneGPS.Value)
                        this.chkEntregaGPSSI.Checked = true;
                    else this.chkEntregaGPSNO.Checked = true;
                }
                #endregion

                #region Documentación Completa
                if (entrega.TieneDocumentacionCompleta.HasValue)
                {
                    if (entrega.TieneDocumentacionCompleta.Value)
                        this.chkEntregaDocumentacionSI.Checked = true;
                    else
                        this.chkEntregaDocumentacionNO.Checked = true;
                }

                this.lblEntregaObsDocumentacion.Text = entrega.ObservacionesDocumentacionCompleta;
                #endregion

                #region Baterias Correctas
                if (entrega.BateriasCorrectas.HasValue)
                {
                    if (entrega.BateriasCorrectas.Value)
                        this.chkEntregaBateriasSI.Checked = true;
                    else this.chkEntregaBateriasNO.Checked = true;
                }

                this.lblEntregaObsBaterias.Text = entrega.ObservacionesBaterias;
                #endregion

                #region Golpes En General
                if (entrega.TieneGolpesGeneral.HasValue)
                {
                    if (entrega.TieneGolpesGeneral.Value)
                        this.chkEntregaGolpesSI.Checked = true;
                    else this.chkEntregaGolpesNO.Checked = true;
                }
                #endregion

                #endregion

                #region Observaciones
                this.dtlObservcionesEntrega.DataSource = entrega.VerificacionesSeccion;
                this.xrlPosicion.DataBindings.Add(new XRBinding("Text", entrega.VerificacionesSeccion, "Numero"));
                this.xrlObservacion.DataBindings.Add(new XRBinding("Text", entrega.VerificacionesSeccion, "Observacion"));
                #endregion                

                #region Combustible
                decimal? porcentaje = null;
                if (unidad != null)
                {
                    if(unidad.CaracteristicasUnidad != null)
                        if (unidad.CaracteristicasUnidad.CapacidadTanque.HasValue)
                            porcentaje = entrega.CalcularPorcentajeCombustible(unidad.CaracteristicasUnidad.CapacidadTanque.Value);
                    
                    this.lblEntregaNivelCombustible.Text = porcentaje.HasValue ? porcentaje.Value.ToString("#,##0.00") : string.Empty;                                
                }

                var imagenes = this.CargarImagenes((string)datos["Path"]);
                XmlNodeList txtFracciones = xmlDocument.GetElementsByTagName("fracciones");
                if (txtFracciones.Count < 1) throw new Exception("el formato del archivo XML es incorrecto");
                var fraccion = (int) Convert.ChangeType(txtFracciones[0].InnerText, typeof (int));
                int? indexPic = -1;

                if(unidad != null)
                    if(unidad.CaracteristicasUnidad != null)
                        if(unidad.CaracteristicasUnidad.CapacidadTanque.HasValue)
                            indexPic = entrega.ObtenerFraccionMedidorCombustible(unidad.CaracteristicasUnidad.CapacidadTanque.Value, fraccion);

                var urlFraccion = imagenes.FirstOrDefault(x => string.Compare(x.Key, indexPic.ToString(), System.StringComparison.Ordinal) == 0).Value;
                this.picEntregaNivelCombustible.ImageUrl = urlFraccion;

                this.lblEntregaCombustible.Text = entrega.Combustible.HasValue
                                                      ? entrega.Combustible.Value.ToString("#,##0")
                                                      : string.Empty;
                #endregion

                #endregion

                #region Datos Recepción
                var recepcion = (ListadoVerificacionBO)datos["Recepcion"] ?? new ListadoVerificacionBO();

                this.lblNombreClienteRecibe.Text = recepcion != null
                                                       ? (recepcion.ListadoVerificacionID.HasValue
                                                              ? cliente.Nombre
                                                              : string.Empty)
                                                       : string.Empty;

                #region Usuario
                this.lblNombreUsuarioRecibe.Text = recepcion.UsuarioVerifica != null 
                                                    ? (!string.IsNullOrEmpty(recepcion.UsuarioVerifica.Nombre) && !string.IsNullOrWhiteSpace(recepcion.UsuarioVerifica.Nombre) ? recepcion.UsuarioVerifica.Nombre : string.Empty) 
                                                    : string.Empty;
                #endregion

                #region Fecha y hora
                this.lblFechaRecibe.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : string.Empty;
                this.lblHoraRecibe.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToString("hh:mm tt").Replace(".","").ToUpper() : string.Empty;

                #endregion

                #region Operador
                this.lblNombreOperadorRecibe.Text = recepcion.UsuarioVerifica != null ? operador.Nombre : string.Empty;
                #endregion

                #region Kilometros Horas
                this.lblKilometrajeRecibe.Text = recepcion.Kilometraje.HasValue
                                                      ? recepcion.Kilometraje.Value.ToString("#,##0")
                                                      : string.Empty;
                this.lblHorasRecibe.Text = recepcion.Horometro.HasValue
                                                ? recepcion.Horometro.Value.ToString("#,##0")
                                                : string.Empty;
                #endregion

                #region Cuestionario

                #region Espejos Completos
                if (recepcion.TieneEspejosCompletos.HasValue)
                {
                    if (recepcion.TieneEspejosCompletos.Value)
                        this.chkRecepcionEspejosSI.Checked = true;
                    else
                        this.chkRecepcionEspejosNO.Checked = true;
                }
                #endregion

                #region Contrato de Renta
                if (recepcion.TieneContratoRenta.HasValue)
                {
                    if (recepcion.TieneContratoRenta.Value)
                        this.chkRecepcionContratoSI.Checked = true;
                    else
                        this.chkRecepcionContratoNO.Checked = false;
                }
                #endregion

                #region Interior Limpio
                if (recepcion.TieneInteriorLimpio.HasValue)
                {
                    if (recepcion.TieneInteriorLimpio.Value)
                        this.chkRecepcionInteriorSI.Checked = true;
                    else
                        this.chkRecepcionInteriorNO.Checked = true;
                }
                #endregion

                #region Vestiduras Limpias
                if (recepcion.TieneVestidurasLimpias.HasValue)
                {
                    if (recepcion.TieneVestidurasLimpias.Value)
                        this.chkRecepcionVestidurasSI.Checked = true;
                    else
                        this.chkRecepcionVestidurasNO.Checked = true;
                }
                #endregion

                #region Tapetes
                if (recepcion.TieneTapetes.HasValue)
                {
                    if (recepcion.TieneTapetes.Value)
                        this.chkRecepcionTapetesSI.Checked = true;
                    else
                        this.chkRecepcionTapetesNO.Checked = true;
                }
                #endregion

                #region Llave Original
                if (recepcion.TieneLlaveOriginal.HasValue)
                {
                    if (recepcion.TieneLlaveOriginal.Value)
                        this.chkRecepcionLlaveSI.Checked = true;
                    else
                        this.chkRecepcionLlaveNO.Checked = true;
                }
                #endregion

                #region Encendedor
                if (recepcion.TieneEncendedor.HasValue)
                {
                    if (recepcion.TieneEncendedor.Value)
                        this.chkRecepcionEncendedorSI.Checked = true;
                    else
                        this.chkRecepcionEncendedorNO.Checked = true;
                }
                #endregion

                #region Estereo y Bocinas
                if (recepcion.TieneStereoBocinas.HasValue)
                {
                    if (recepcion.TieneStereoBocinas.Value)
                        this.chkRecepcionEstereoSI.Checked = true;
                    else this.chkRecepcionEstereoNO.Checked = true;
                }
                #endregion

                #region Alarma de reversa
                if (recepcion.TieneAlarmasReversa.HasValue)
                {
                    if (recepcion.TieneAlarmasReversa.Value)
                        this.chkRecepcionAlarmaSI.Checked = true;
                    else
                        this.chkRecepcionAlarmaNO.Checked = true;
                }
                #endregion

                #region Extinguidor
                if (recepcion.TieneExtinguidor.HasValue)
                {
                    if (recepcion.TieneExtinguidor.Value)
                        this.chkRecepcionExtinguidorSI.Checked = true;
                    else this.chkRecepcionExtinguidorNO.Checked = true;
                }
                #endregion

                #region Gato y Llave
                if (recepcion.TieneGatoLlaveTuerca.HasValue)
                {
                    if (recepcion.TieneGatoLlaveTuerca.Value)
                        this.chkRecepcionGatoSI.Checked = true;
                    else this.chkRecepcionGatoNO.Checked = true;
                }
                #endregion

                #region 3 Reflejantes
                if (recepcion.TieneTresReflejantes.HasValue)
                {
                    if (recepcion.TieneTresReflejantes.Value)
                        this.chkRecepcionReflejantesSI.Checked = true;
                    else this.chkRecepcionReflejantesNO.Checked = true;
                }
                #endregion

                #region Limpieza Int. Caja
                if (recepcion.TieneLimpiezaInteriorCaja.HasValue)
                {
                    if (recepcion.TieneLimpiezaInteriorCaja.Value)
                        this.chkRecepcionCajaSI.Checked = true;
                    else this.chkRecepcionCajaNO.Checked = true;
                }
                #endregion

                #region GPS
                if (recepcion.TieneGPS.HasValue)
                {
                    if (recepcion.TieneGPS.Value)
                        this.chkRecepcionGPSSI.Checked = true;
                    else this.chkRecepcionGPSNO.Checked = true;
                }
                #endregion

                #region Documentación Completa
                if (recepcion.TieneDocumentacionCompleta.HasValue)
                {
                    if (recepcion.TieneDocumentacionCompleta.Value)
                        this.chkRecepcionDocumentacionSI.Checked = true;
                    else
                        this.chkRecepcionDocumentacionNO.Checked = true;
                }

                this.lblRecepcionObsDocumentacion.Text = !string.IsNullOrEmpty(recepcion.ObservacionesDocumentacionCompleta) && !string.IsNullOrWhiteSpace(recepcion.ObservacionesDocumentacionCompleta)
                                                        ? recepcion.ObservacionesDocumentacionCompleta
                                                        : string.Empty;
                #endregion

                #region Baterias Correctas
                if (recepcion.BateriasCorrectas.HasValue)
                {
                    if (recepcion.BateriasCorrectas.Value)
                        this.chkRecepcionBateriasSI.Checked = true;
                    else this.chkRecepcionBateriasNO.Checked = true;
                }

                this.lblRecepcionObsBaterias.Text = recepcion.ObservacionesBaterias;
                #endregion

                #region Golpes En General
                if (recepcion.TieneGolpesGeneral.HasValue)
                {
                    if (recepcion.TieneGolpesGeneral.Value)
                        this.chkRecepcionGolpesSI.Checked = true;
                    else this.chkRecepcionGolpesNO.Checked = true;
                }
                #endregion

                #endregion

                #region Observaciones
                this.dtlObsRecepcion.DataSource = recepcion.VerificacionesSeccion;
                this.lblRecepcionPosicion.DataBindings.Add(new XRBinding("Text", recepcion.VerificacionesSeccion, "Numero"));
                this.lblRecepcionObservacion.DataBindings.Add(new XRBinding("Text", recepcion.VerificacionesSeccion, "Observacion"));
                #endregion                

                #region Combustible
                decimal? porcentajeRecep = null;
                if (unidad != null)
                {
                    if (unidad.CaracteristicasUnidad != null)
                        if (unidad.CaracteristicasUnidad.CapacidadTanque.HasValue)
                            porcentajeRecep = recepcion.CalcularPorcentajeCombustible(unidad.CaracteristicasUnidad.CapacidadTanque.Value);

                    this.lblRecepcionNivelCombustible.Text = porcentajeRecep.HasValue ? porcentajeRecep.Value.ToString("#,##0.00") : string.Empty;
                }

                int? indexPicRec = -1;

                if (unidad != null)
                    if (unidad.CaracteristicasUnidad != null)
                        if (unidad.CaracteristicasUnidad.CapacidadTanque.HasValue)
                            indexPicRec = recepcion.ObtenerFraccionMedidorCombustible(unidad.CaracteristicasUnidad.CapacidadTanque.Value, fraccion);

                if (indexPicRec == null) indexPicRec = -1;

                var urlFraccionRec = imagenes.FirstOrDefault(x => string.Compare(x.Key, indexPicRec.ToString(), System.StringComparison.Ordinal) == 0).Value;
                this.picRecepcionNivelCombustible.ImageUrl = urlFraccionRec;

                this.lblRecepcionCombustible.Text = recepcion.Combustible.HasValue
                                                        ? recepcion.Combustible.Value.ToString("#,##0")
                                                        : string.Empty;
                #endregion
                #endregion

                #region Info Llantas
                var lista = new List<LlantaDTO>();

                if (entrega.VerificacionesLlanta != null)
                {
                    if (entrega.VerificacionesLlanta.Count > 0)
                    {
                        foreach (var verificacion in entrega.VerificacionesLlanta)
                        {
                            LlantaDTO item = new LlantaDTO();
                            item.Codigo = verificacion.Llanta != null ? verificacion.Llanta.Codigo : string.Empty;
                            item.Posicion = verificacion.Llanta != null
                                                ? (verificacion.Llanta.Posicion.HasValue
                                                       ? verificacion.Llanta.Posicion
                                                       : null)
                                                : null;
                            item.Correcto = verificacion.Correcto.HasValue
                                                ? (verificacion.Correcto.Value ? verificacion.Correcto : null)
                                                : null;
                            item.Incorrecto = verificacion.Correcto.HasValue
                                                  ? (!verificacion.Correcto.Value ? (bool?)true : null)
                                                  : null;
                            lista.Add(item);
                        }
                    }
                }
                lista.Sort((x, y) => string.Compare(x.Posicion.ToString(), y.Posicion.ToString()));

                if (recepcion != null)
                {
                    if (recepcion.VerificacionesLlanta != null)
                    {
                        if (recepcion.VerificacionesLlanta.Count > 0)
                        {
                            foreach (var verifRecepcion in recepcion.VerificacionesLlanta)
                            {
                                LlantaDTO item = lista.FirstOrDefault( x =>
                                        x.Codigo == verifRecepcion.Llanta.Codigo &&
                                        x.Posicion == verifRecepcion.Llanta.Posicion);
                                
                                if (item != null)
                                {
                                    item.CorrectoRecepcion = verifRecepcion.Correcto.HasValue ? (verifRecepcion.Correcto.Value ? verifRecepcion.Correcto : null) : null;
                                    item.IncorrectoRecepcion = verifRecepcion.Correcto.HasValue ? (!verifRecepcion.Correcto.Value ? (bool?)true : null) : null;
                                }                                
                            }
                        }
                    }
                }

                this.dtlLlantas.DataSource = lista;
                this.xrblPosicion.DataBindings.Add(new XRBinding("Text", null, "Posicion"));
                this.xrblCodigo.DataBindings.Add(new XRBinding("Text", null, "Codigo"));
                this.chkEntregaLlantasSI.DataBindings.Add(new XRBinding("CheckState", null, "Correcto"));
                this.chkEntregaLlantasNO.DataBindings.Add(new XRBinding("CheckState", null, "Incorrecto"));
                this.lblEntregaObsLlanta.Text = entrega.ObservacionesLlantas;

                this.xrlblRecepcionPosicion.DataBindings.Add(new XRBinding("Text", null, "Posicion"));
                this.xrlblRecepcionCodigo.DataBindings.Add(new XRBinding("Text", null, "Codigo"));
                this.chkRecepcionLlantaSI.DataBindings.Add(new XRBinding("CheckState", null, "CorrectoRecepcion"));
                this.chkRecepcionLlantaNO.DataBindings.Add(new XRBinding("CheckState", null, "IncorrectoRecepcion"));

                if (recepcion != null)
                {
                    if (recepcion.VerificacionesLlanta != null)
                    {
                        if (recepcion.VerificacionesLlanta.Count > 0)
                        {
                            this.lblRecepcionObsLlanta.Text = recepcion.ObservacionesLlantas;
                        }
                    }

                    this.lblRecepcionObsLlanta.Text = recepcion.ObservacionesLlantas;
                }
                else this.lblRecepcionObsLlanta.Text = string.Empty;

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(nombreClase + ".ImpirmirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
        /// <summary>
        /// Obtiene las imagenes para la valvula de gasolina
        /// </summary>
        /// <param name="path">url del xml</param>
        /// <returns>Lista con las direcciones de las imagenes</returns>
        private Dictionary<string, string> CargarImagenes(string path)
        {            
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                throw new Exception("El archivo no existe en la ubicación proorcionada.");

            Dictionary<string, string> datos = new Dictionary<string, string>();

            if (File.Exists(path))
            {
                XDocument xmlDoc = XDocument.Load(path);

                var imagenes = xmlDoc.Descendants("imagenes").Descendants("imagen").Select(reps => new
                    {
                        id = reps.Element("id").Value,
                        url = reps.Element("url").Value
                    });

                foreach (var imagen in imagenes)
                {
                    datos.Add(imagen.id, imagen.url);
                }
            }

            return datos;
        }
        /// <summary>
        /// Obtiene la plantilla XML para el reporte
        /// </summary>
        /// <param name="urlXML">url del reporte</param>
        /// <returns>plantilla del XML</returns>
        private XmlDocument ObtenerXMLContenido(string urlXML)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(urlXML + xml);
                return xDoc;
            }
            catch (Exception ex)
            {
                throw new Exception( nombreClase +".ObtenerXMLContenido: Error al cargar el archivo XML." + ex.Message);
            }
        }
        /// <summary>
        /// DTO para desplegar las llantas
        /// </summary>
        private class LlantaDTO
        {
            public int? Posicion { get; set; }
            public string Codigo { get; set; }
            public bool? Correcto { get; set; }
            public bool? Incorrecto { get; set; }
            public bool? CorrectoRecepcion { get; set; }
            public bool? IncorrectoRecepcion { get; set; }
        }
    }    
}
