using System;
using System.Collections.Generic;
using BPMO.SDNI.Comun.BO;
using BPMO.SDNI.Contratos.PSL.BO;
using BPMO.SDNI.Equipos.BO;
using BPMO.SDNI.Contratos.BO;


namespace BPMO.SDNI.Contratos.PSL.RPT {
    public partial class ListadoVerificacionMartilloHidraulicoRPT : DevExpress.XtraReports.UI.XtraReport {
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;

        /// <summary>
        /// Nombre de la clase que será usada en los mensajes de error
        /// </summary>
        private const string nombreClase = "CheckListRORPT";

        public ListadoVerificacionMartilloHidraulicoRPT(Dictionary<string, Object> datos) {
            InitializeComponent();

            this.ImprimirReporte(datos);
        }

        /// <summary>
        /// Genera el reporte para el check list
        /// </summary>
        /// <param name="datos">Datos del check list</param>
        private void ImprimirReporte(Dictionary<string, object> datos) {
            try {
                #region Parte Generica
                #region Cabecera

                #region Logo
                var modulo = (ModuloBO)datos["Modulo"];
                int? unidadOperativaID = (int)datos["UnidadOperativaID"];
                if (unidadOperativaID != null && modulo.ObtenerConfiguracion(unidadOperativaID.Value) != null) {
                    ConfiguracionModuloBO configuracion = modulo.ObtenerConfiguracion(unidadOperativaID.Value);
                    if (!string.IsNullOrEmpty(configuracion.URLLogoEmpresa) && !string.IsNullOrWhiteSpace(configuracion.URLLogoEmpresa))
                        this.picLogoEmpresa.ImageUrl = configuracion.URLLogoEmpresa;
                }
                #endregion

                #region Folio
                string folio = (string)datos["Folio"];
                lblNumeroContrato.Text = folio;
                #endregion

                #region Contrato
                var contrato = (ContratoPSLBO)datos["Contrato"];
                if (contrato != null) {
                    if (contrato.Sucursal != null)
                        if (contrato.Sucursal.DireccionesSucursal != null && contrato.Sucursal.DireccionesSucursal.Count > 0)
                            xrlDireccionEmpresa.Text = string.Format("{0} {1} {2} {3} CP. {4}", contrato.Sucursal.DireccionesSucursal[0].Calle, contrato.Sucursal.DireccionesSucursal[0].Colonia, contrato.Sucursal.DireccionesSucursal[0].Ubicacion.Municipio.Codigo, contrato.Sucursal.DireccionesSucursal[0].Ubicacion.Estado.Codigo, contrato.Sucursal.DireccionesSucursal[0].CodigoPostal);
                    lblUbicacion.Text = !string.IsNullOrEmpty(contrato.DestinoAreaOperacion) ? contrato.DestinoAreaOperacion : string.Empty;
                }
                #endregion

                #region Fecha
                lblFechaCreacion.Text = DateTime.Now.ToShortDateString();
                #endregion

                #region Unidad

                var unidad = (UnidadBO)datos["Unidad"];
                this.lblNumeroSerie.Text = unidad.NumeroSerie.ToString();
                this.lblModelo.Text = unidad.Modelo.Nombre;
                this.lblAnio.Text = unidad.Anio.ToString();
                this.lblItemEcode.Text = unidad.NumeroEconomico.ToString();
                if (unidad.Modelo != null)
                    if (unidad.Modelo.Marca != null)
                        this.lblMarca.Text = unidad.Modelo.Marca.Nombre.ToString();

                var descripcion = string.Empty;
                if (unidad.TipoEquipoServicio != null)
                    descripcion = unidad.TipoEquipoServicio.Nombre + " ";
                if (unidad.Modelo != null)
                    descripcion += unidad.Modelo.Nombre;
                this.lblTipoUnidadModelo.Text = descripcion;


                #endregion

                #endregion

                #region Estatus contrato

                EEstatusContrato? estatus = null;
                if (datos.ContainsKey("EstatusContrato") && datos["EstatusContrato"] != null)
                    estatus = (EEstatusContrato)datos["EstatusContrato"];
                

                #endregion

                #region Cliente

                var cliente = (CuentaClienteIdealeaseBO)datos["Cliente"];
                this.lblClienteFacturacion.Text = cliente.Id.ToString();
                this.lblDireccion.Text = cliente.Direccion;
                if (cliente.Cliente != null) {
                    this.lblCorreoClienteFacturacion.Text = cliente.Correo;
                    this.lblEmailEmpresaEntrega.Text = cliente.Correo;
                    this.lblNombreCliente.Text = cliente.Nombre.ToUpper();

                    if (cliente.Telefonos != null && cliente.Telefonos.Count > 0) {
                        lblTelfonoClienteFacturacion.Text = !string.IsNullOrEmpty(cliente.Telefonos[0].Telefono) ? cliente.Telefonos[0].Telefono : string.Empty;
                        lblTelefonoEmpresaEntrega.Text = !string.IsNullOrEmpty(cliente.Telefonos[0].Telefono) ? cliente.Telefonos[0].Telefono : string.Empty;
                    }
                }

                #endregion

                #region Datos entrega

                var entrega = (ListadoVerificacionMartilloHidraulicoBO)datos["Entrega"] ?? new ListadoVerificacionMartilloHidraulicoBO();

                #endregion

                #region Linea
                var linea = (LineaContratoPSLBO)datos["Linea"];
                if (linea != null)
                    lblFolio.Text = !string.IsNullOrEmpty(linea.FolioCheckList) ? linea.FolioCheckList : string.Empty;
                #endregion

                #region Kilómetros y horas
                this.lblKilometrajeEntrega.Text = entrega.Horometro.HasValue
                                                      ? entrega.Horometro.Value.ToString("#,##0")
                                                      : string.Empty;

                #endregion

                #region Capacidad
                this.lblTanqueCombustible.Text = unidad == null || unidad.CombustibleConsumidoTotal == null ? "N/A" : unidad.CombustibleConsumidoTotal.ToString() + " L"; this.lblTanqueHidraulico.Text = "N/A";
                this.lblCarterMotor.Text = "N/A";
                this.lblDepositoRefrigerante.Text = "N/A";
                #endregion

                #region Cuestionario Entrega

                #region Observaciones
                this.lblObservaciones.Text = !string.IsNullOrEmpty(entrega.Observaciones) ? entrega.Observaciones : string.Empty;
                #endregion


                #endregion

                #region Combustible

                this.lblCombustibleSalida.Text = entrega.Combustible.HasValue ? Convert.ToInt32(entrega.Combustible.Value).ToString("#,##0") + " L" : "N/A";

                #endregion

                #region Datos Recepción
                var recepcion = (ListadoVerificacionMartilloHidraulicoBO)datos["Recepcion"] ?? new ListadoVerificacionMartilloHidraulicoBO();

                #region Fecha recepción
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : string.Empty;
                #endregion

                #region Horómetro
                this.lblHorometroRetorno.Text = recepcion.Horometro.HasValue
                                                ? recepcion.Horometro.Value.ToString("#,##0")
                                                : string.Empty;
                #endregion



                #region Observaciones recepción
                this.xrObservacionesRecepcion.Text = !string.IsNullOrEmpty(recepcion.Observaciones) ? recepcion.Observaciones : string.Empty;
                #endregion

                #region Combustible
                this.lblCombustibleRetorno.Text = recepcion.Combustible.HasValue ? Convert.ToInt32(recepcion.Combustible.Value).ToString("#,##0") + " L" : string.Empty;

                #endregion
                #endregion

                #region Fecha y Hora salida
                this.lblFechaEntrega.Text = contrato.FechaInicioArrendamiento.HasValue ? contrato.FechaInicioArrendamiento.Value.ToShortDateString() : string.Empty;
                this.lblFechaRecepcion.Text = recepcion.Fecha.HasValue ? recepcion.Fecha.Value.ToShortDateString() : (contrato.FechaPromesaActual.HasValue ? contrato.FechaPromesaActual.Value.ToShortDateString() : string.Empty);
                #endregion
                #endregion

                #region Cuestionarios
                #region CheckBox Entrega
                if (entrega.TieneBujes.HasValue) {
                    if (entrega.TieneBujes.Value)
                        chbxBujes.Checked = true;
                    else
                        chbxBujes.Checked = false;
                }



                if (entrega.TieneCondicionBujes.HasValue) {
                    if (entrega.TieneCondicionBujes.Value)
                        chbxCondicionBujes.Checked = true;
                    else
                        chbxCondicionBujes.Checked = false;
                }

                if (entrega.TieneCondicionCalcas.HasValue) {
                    if (entrega.TieneCondicionCalcas.Value)
                        chbxCondicionCalcas.Checked = true;
                    else
                        chbxCondicionCalcas.Checked = false;
                }

                if (entrega.TieneCondicionMangueras.HasValue) {
                    if (entrega.TieneCondicionMangueras.Value)
                        chbxCondicionMangueras.Checked = true;
                    else
                        chbxCondicionMangueras.Checked = false;
                }

                if (entrega.TieneCondicionPasadores.HasValue) {
                    if (entrega.TieneCondicionPasadores.Value)
                        chbxCondicionPasadores.Checked = true;
                    else
                        chbxCondicionPasadores.Checked = false;
                }

                if (entrega.TieneCondicionPica.HasValue) {
                    if (entrega.TieneCondicionPica.Value)
                        chbxCondicionPica.Checked = true;
                    else
                        chbxCondicionPica.Checked = false;
                }

                if (entrega.TieneCondicionPintura.HasValue) {
                    if (entrega.TieneCondicionPintura.Value)
                        chbxCondicionPintura.Checked = true;
                    else
                        chbxCondicionPintura.Checked = false;
                }

                if (entrega.TieneEstructura.HasValue) {
                    if (entrega.TieneEstructura.Value)
                        chbxEstructura.Checked = true;
                    else
                        chbxEstructura.Checked = false;
                }

                if (entrega.TieneGraseraManual.HasValue) {
                    if (entrega.TieneGraseraManual.Value)
                        chbxGraseraManual.Checked = true;
                    else
                        chbxGraseraManual.Checked = false;
                }

                if (entrega.TienePasadores.HasValue) {
                    if (entrega.TienePasadores.Value)
                        chbxPasadores.Checked = true;
                    else
                        chbxPasadores.Checked = false;
                }

                if (entrega.TienePica.HasValue) {
                    if (entrega.TienePica.Value)
                        chbxPica.Checked = true;
                    else
                        chbxPica.Checked = false;
                }


                if (entrega.TieneSimbolosSeguridad.HasValue) {
                    if (entrega.TieneSimbolosSeguridad.Value)
                        chbxSimbolosSeguridadMaquina.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquina.Checked = false;
                }

                if (entrega.TieneTaponesMangueras.HasValue) {
                    if (entrega.TieneTaponesMangueras.Value)
                        chbxTaponesMangueraAlimentacion.Checked = true;
                    else
                        chbxTaponesMangueraAlimentacion.Checked = false;
                }

                if (entrega.TieneTorqueTornillos.HasValue) {
                    if (entrega.TieneTorqueTornillos.Value)
                        chbxTorqueTornillosBaseMartillo.Checked = true;
                    else
                        chbxTorqueTornillosBaseMartillo.Checked = false;
                }
                #endregion
                #region CheckBox Recepción
                if (recepcion.TieneBujes.HasValue) {
                    if (recepcion.TieneBujes.Value)
                        chbxBujesRecepcion.Checked = true;
                    else
                        chbxBujesRecepcion.Checked = false;
                }



                if (recepcion.TieneCondicionBujes.HasValue) {
                    if (recepcion.TieneCondicionBujes.Value)
                        chbxCondicionBujesRecepcion.Checked = true;
                    else
                        chbxCondicionBujesRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionCalcas.HasValue) {
                    if (recepcion.TieneCondicionCalcas.Value)
                        chbxCondicionCalcasRecepcion.Checked = true;
                    else
                        chbxCondicionCalcasRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionMangueras.HasValue) {
                    if (recepcion.TieneCondicionMangueras.Value)
                        chbxCondicionManguerasRecepcion.Checked = true;
                    else
                        chbxCondicionManguerasRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionPasadores.HasValue) {
                    if (recepcion.TieneCondicionPasadores.Value)
                        chbxCondicionPasadoresRecepcion.Checked = true;
                    else
                        chbxCondicionPasadoresRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionPica.HasValue) {
                    if (recepcion.TieneCondicionPica.Value)
                        chbxCondicionPicaRecepcion.Checked = true;
                    else
                        chbxCondicionPicaRecepcion.Checked = false;
                }

                if (recepcion.TieneCondicionPintura.HasValue) {
                    if (recepcion.TieneCondicionPintura.Value)
                        chbxCondicionPinturaRecepcion.Checked = true;
                    else
                        chbxCondicionPinturaRecepcion.Checked = false;
                }

                if (recepcion.TieneEstructura.HasValue) {
                    if (recepcion.TieneEstructura.Value)
                        chbxEstructuraRecepcion.Checked = true;
                    else
                        chbxEstructuraRecepcion.Checked = false;
                }

                if (recepcion.TieneGraseraManual.HasValue) {
                    if (recepcion.TieneGraseraManual.Value)
                        chbxGraseraManualRecepcion.Checked = true;
                    else
                        chbxGraseraManualRecepcion.Checked = false;
                }

                if (recepcion.TienePasadores.HasValue) {
                    if (recepcion.TienePasadores.Value)
                        chbxPasadoresRecepcion.Checked = true;
                    else
                        chbxPasadoresRecepcion.Checked = false;
                }

                if (recepcion.TienePica.HasValue) {
                    if (recepcion.TienePica.Value)
                        chbxPicaRecepcion.Checked = true;
                    else
                        chbxPicaRecepcion.Checked = false;
                }


                if (recepcion.TieneSimbolosSeguridad.HasValue) {
                    if (recepcion.TieneSimbolosSeguridad.Value)
                        chbxSimbolosSeguridadMaquinaRecepcion.Checked = true;
                    else
                        chbxSimbolosSeguridadMaquinaRecepcion.Checked = false;
                }

                if (recepcion.TieneTaponesMangueras.HasValue) {
                    if (recepcion.TieneTaponesMangueras.Value)
                        chbxTaponesManguerasAlimentacionRecepcion.Checked = true;
                    else
                        chbxTaponesManguerasAlimentacionRecepcion.Checked = false;
                }

                if (recepcion.TieneTorqueTornillos.HasValue) {
                    if (recepcion.TieneTorqueTornillos.Value)
                        chbxTorqueTornillosBaseMartilloRecepcion.Checked = true;
                    else
                        chbxTorqueTornillosBaseMartilloRecepcion.Checked = false;
                }
                #endregion
                #endregion
            } catch (Exception ex) {
                throw new Exception(".ImprimirReporte: Error al imprimir el reporte. " + ex.Message);
            }
        }
    }
}